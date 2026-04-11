using Marilo.Core.Enums;
using Marilo.Core.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Marilo.Components.DataGrid;

/// <summary>
/// Active cell state, edit mode transitions, and keyboard handlers for MariloDataSheet.
/// </summary>
public partial class MariloDataSheet<TItem>
{
    // ── Active Cell State ──────────────────────────────────────────────

    internal TItem? _activeCellRow;
    internal string? _activeCellField;
    internal bool _isEditMode;
    internal object? _editValueBeforeEdit; // for undo/cancel

    // Track last committed value per cell for Ctrl+Z
    private readonly Dictionary<string, object?> _undoBuffer = [];

    // ── Cell Activation ────────────────────────────────────────────────

    internal void ActivateCell(TItem row, string field)
    {
        _activeCellRow = row;
        _activeCellField = field;
        _isEditMode = false;
        StateHasChanged();
    }

    /// <summary>Enters edit mode on the specified cell.</summary>
    public void EnterEditMode(TItem row, string field)
    {
        var column = _columns.FirstOrDefault(c => c.Field == field);
        if (column is null || !column.Editable || column.ColumnType == DataSheetColumnType.Computed)
            return;

        _activeCellRow = row;
        _activeCellField = field;
        _isEditMode = true;
        _editValueBeforeEdit = GridReflectionHelper.GetValue(row, field);
        StateHasChanged();
    }

    internal void ClearActiveCell()
    {
        _activeCellRow = default;
        _activeCellField = null;
        _isEditMode = false;
        _editValueBeforeEdit = null;
    }

    internal bool IsCellActive(TItem row, string field)
    {
        return _activeCellRow != null
               && EqualityComparer<TItem>.Default.Equals(_activeCellRow, row)
               && _activeCellField == field;
    }

    /// <summary>Whether a specific cell is currently in edit mode.</summary>
    public bool IsCellEditing(TItem row, string field)
    {
        return IsCellActive(row, field) && _isEditMode;
    }

    // ── Cell Click Handler ─────────────────────────────────────────────

    internal void OnCellClick(TItem row, string field)
    {
        var column = _columns.FirstOrDefault(c => c.Field == field);
        if (column is null) return;

        if (column.ColumnType == DataSheetColumnType.Computed || !column.Editable)
        {
            ActivateCell(row, field);
            return;
        }

        // Checkbox columns toggle immediately on click
        if (column.ColumnType == DataSheetColumnType.Checkbox)
        {
            var currentVal = GridReflectionHelper.GetValue(row, field);
            var newVal = currentVal is true ? (object)false : (object)true;
            _ = CommitCellEdit(row, field, newVal);
            return;
        }

        if (IsCellActive(row, field) && !_isEditMode)
        {
            EnterEditMode(row, field);
        }
        else
        {
            ActivateCell(row, field);
        }
    }

    // ── Cell Value Commit ──────────────────────────────────────────────

    internal async Task OnCellValueCommit(TItem row, string field, object? newValue)
    {
        // Store undo
        var undoKey = $"{GetRowKey(row)}:{field}";
        _undoBuffer[undoKey] = _editValueBeforeEdit;

        await CommitCellEdit(row, field, newValue);
        _isEditMode = false;
    }

    // ── Keyboard Handler (called from JS) ──────────────────────────────

    [JSInvokable]
    public async Task HandleKeyDown(string key, bool ctrl, bool shift)
    {
        // Ctrl+S: Save All
        if (ctrl && key == "s")
        {
            await SaveAllAsync();
            return;
        }

        // Ctrl+Z: Undo last cell change
        if (ctrl && key == "z" && _activeCellRow != null && _activeCellField != null)
        {
            var undoKey = $"{GetRowKey(_activeCellRow)}:{_activeCellField}";
            if (_undoBuffer.TryGetValue(undoKey, out var prevValue))
            {
                await CommitCellEdit(_activeCellRow, _activeCellField, prevValue);
                _undoBuffer.Remove(undoKey);
            }
            return;
        }

        // Ctrl+C: Copy (handled in JS for clipboard access)
        // Ctrl+V: Paste (handled in JS, calls PasteFromClipboard)

        // Ctrl+D: Fill down
        if (ctrl && key == "d" && _activeCellRow != null && _activeCellField != null)
        {
            var value = GridReflectionHelper.GetValue(_activeCellRow, _activeCellField);
            var startIdx = _displayRows.IndexOf(_activeCellRow);
            if (startIdx >= 0)
            {
                foreach (var selectedRow in _selectedRows)
                {
                    var idx = _displayRows.IndexOf(selectedRow);
                    if (idx > startIdx)
                    {
                        await CommitCellEdit(selectedRow, _activeCellField, value);
                    }
                }
            }
            return;
        }

        // Escape: Cancel edit
        if (key == "Escape" && _isEditMode && _activeCellRow != null && _activeCellField != null)
        {
            GridReflectionHelper.SetValue(_activeCellRow, _activeCellField, _editValueBeforeEdit);
            _isEditMode = false;
            StateHasChanged();
            return;
        }

        // F2: Enter edit mode
        if (key == "F2" && !_isEditMode && _activeCellRow != null && _activeCellField != null)
        {
            EnterEditMode(_activeCellRow, _activeCellField);
            return;
        }

        // Delete: Clear selected cells
        if (key == "Delete" && _activeCellRow != null && _activeCellField != null)
        {
            var column = _columns.FirstOrDefault(c => c.Field == _activeCellField);
            if (column != null && column.Editable && column.ColumnType != DataSheetColumnType.Computed)
            {
                await CommitCellEdit(_activeCellRow, _activeCellField, GetDefaultValue(column));
            }
            return;
        }

        // Navigation: only when not in edit mode
        if (!_isEditMode && _activeCellRow != null && _activeCellField != null)
        {
            var rowIdx = _displayRows.IndexOf(_activeCellRow);
            var colIdx = _columns.FindIndex(c => c.Field == _activeCellField);
            var editableColumns = _columns.Where(c => c.Editable || c.ColumnType == DataSheetColumnType.Computed).ToList();

            switch (key)
            {
                case "ArrowDown" when rowIdx < _displayRows.Count - 1:
                    ActivateCell(_displayRows[rowIdx + 1], _activeCellField);
                    break;
                case "ArrowUp" when rowIdx > 0:
                    ActivateCell(_displayRows[rowIdx - 1], _activeCellField);
                    break;
                case "ArrowRight" when colIdx < _columns.Count - 1:
                    ActivateCell(_activeCellRow, _columns[colIdx + 1].Field);
                    break;
                case "ArrowLeft" when colIdx > 0:
                    ActivateCell(_activeCellRow, _columns[colIdx - 1].Field);
                    break;
            }
        }

        // Tab / Enter: commit and navigate
        if (_isEditMode && _activeCellRow != null && _activeCellField != null)
        {
            var rowIdx = _displayRows.IndexOf(_activeCellRow);
            var colIdx = _columns.FindIndex(c => c.Field == _activeCellField);

            if (key == "Tab")
            {
                if (shift && colIdx > 0)
                {
                    _isEditMode = false;
                    ActivateCell(_activeCellRow, _columns[colIdx - 1].Field);
                }
                else if (!shift && colIdx < _columns.Count - 1)
                {
                    _isEditMode = false;
                    ActivateCell(_activeCellRow, _columns[colIdx + 1].Field);
                }
                else if (!shift && rowIdx < _displayRows.Count - 1)
                {
                    _isEditMode = false;
                    ActivateCell(_displayRows[rowIdx + 1], _columns[0].Field);
                }
            }
            else if (key == "Enter" && rowIdx < _displayRows.Count - 1)
            {
                _isEditMode = false;
                ActivateCell(_displayRows[rowIdx + 1], _activeCellField);
            }
        }
    }

    // ── Paste Handler (called from JS) ─────────────────────────────────

    [JSInvokable]
    public async Task PasteFromClipboard(string tsvData)
    {
        if (!AllowBulkPaste || _activeCellRow is null || _activeCellField is null) return;

        var startRowIdx = _displayRows.IndexOf(_activeCellRow);
        var startColIdx = _columns.FindIndex(c => c.Field == _activeCellField);
        if (startRowIdx < 0 || startColIdx < 0) return;

        // V04.1 — Normalize line endings. Windows clipboards produce "\r\n";
        // splitting on '\n' alone leaves '\r' appended to the last cell of
        // each row and breaks decimal/DateTime parsing.
        var normalized = tsvData.Replace("\r\n", "\n").Replace("\r", "\n");
        var lines = normalized.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // V04.3 — Skip rows that are marked for deletion. The TSV cursor
        // advances independently from the display-row cursor so that a
        // pasted row lands on the next non-deleted row rather than being
        // silently dropped onto a deleted one.
        var rowCursor = startRowIdx;
        for (var r = 0; r < lines.Length; r++)
        {
            while (rowCursor < _displayRows.Count && IsRowDeleted(_displayRows[rowCursor]))
            {
                rowCursor++;
            }
            if (rowCursor >= _displayRows.Count) break;

            var row = _displayRows[rowCursor];
            var cells = lines[r].Split('\t');
            for (var c = 0; c < cells.Length && startColIdx + c < _columns.Count; c++)
            {
                var column = _columns[startColIdx + c];
                if (!column.Editable || column.ColumnType == DataSheetColumnType.Computed) continue;

                var (success, parsedValue, errorMessage) = TryParseCellValue(column, cells[c].Trim());
                if (success)
                {
                    await CommitCellEdit(row, column.Field, parsedValue);
                }
                else
                {
                    // V04.2 — Do NOT write the raw pasted string to the model.
                    // Mark the cell invalid with a type-specific message and
                    // leave the row's property at its pre-paste value.
                    MarkPasteCellInvalid(row, column.Field, errorMessage!);
                }
            }

            rowCursor++;
        }

        StateHasChanged();
    }

    // V04.2 — Records a paste-time coercion failure on the dirty-row entry
    // without mutating the underlying TItem property. Mirrors the pattern
    // used by CommitCellEdit's ValidationErrors dictionary so the cell
    // surfaces CellState.Invalid through the existing GetCellState path.
    private void MarkPasteCellInvalid(TItem row, string field, string errorMessage)
    {
        var key = GetRowKey(row);
        if (key is null) return;

        if (!_dirtyRows.TryGetValue(key, out var entry))
        {
            entry = new DirtyRowEntry<TItem>
            {
                Original = GridReflectionHelper.DeepClone(row),
                Current = row
            };
            _dirtyRows[key] = entry;
        }

        entry.ValidationErrors[field] = errorMessage;
    }

    // ── Helpers ─────────────────────────────────────────────────────────

    private static object? GetDefaultValue(MariloDataSheetColumn<TItem> column)
    {
        return column.ColumnType switch
        {
            DataSheetColumnType.Text => "",
            DataSheetColumnType.Number => 0m,
            DataSheetColumnType.Checkbox => false,
            DataSheetColumnType.Date => null,
            DataSheetColumnType.Select => "",
            _ => null
        };
    }

    // V04.2 — Returns (success, parsedValue, errorMessage). On success the
    // parsed value is type-correct for the column's CLR property; on
    // failure the error message matches the spec (bulk-paste-and-clipboard.md
    // "Type Coercion on Paste" table) and the paste loop will mark the cell
    // invalid without writing the raw string to the model.
    private static (bool Success, object? Value, string? Error) TryParseCellValue(
        MariloDataSheetColumn<TItem> column, string text)
    {
        switch (column.ColumnType)
        {
            case DataSheetColumnType.Number:
                {
                    var targetType = typeof(TItem).GetProperty(column.Field)?.PropertyType
                                     ?? typeof(decimal);
                    var (success, value) = ParseNumericValue(text, targetType);
                    return success
                        ? (true, value, null)
                        : (false, null, "Invalid number");
                }
            case DataSheetColumnType.Date:
                if (DateTime.TryParse(text, out var dt))
                    return (true, dt, null);
                return (false, null, "Invalid date");
            case DataSheetColumnType.Checkbox:
                return (true,
                    text.Equals("true", StringComparison.OrdinalIgnoreCase) || text == "1",
                    null);
            case DataSheetColumnType.Select:
                if (column.Options != null && column.Options.Any(o => o.Value == text))
                    return (true, text, null);
                return (false, null, "Value not in options");
            default:
                return (true, text, null);
        }
    }

    /// <summary>
    /// Parses a string into the requested numeric target type, handling all
    /// primitive numeric types (int, long, short, byte, decimal, double, float)
    /// and their nullable counterparts. Returns (true, parsedValue) on success,
    /// (false, defaultOfTarget) on failure. Empty/null input returns
    /// (true, null) for nullable targets and (false, default) otherwise.
    /// </summary>
    internal static (bool Success, object? Value) ParseNumericValue(string? input, Type targetType)
    {
        var underlying = Nullable.GetUnderlyingType(targetType);
        var isNullable = underlying != null;
        var effectiveType = underlying ?? targetType;

        if (string.IsNullOrEmpty(input))
        {
            return isNullable
                ? (true, null)
                : (false, GetDefaultForType(effectiveType));
        }

        if (!decimal.TryParse(input, System.Globalization.NumberStyles.Any,
                              System.Globalization.CultureInfo.CurrentCulture, out var parsed))
        {
            return (false, GetDefaultForType(effectiveType));
        }

        try
        {
            var converted = Convert.ChangeType(parsed, effectiveType);
            return (true, converted);
        }
        catch
        {
            return (false, GetDefaultForType(effectiveType));
        }
    }

    private static object? GetDefaultForType(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
