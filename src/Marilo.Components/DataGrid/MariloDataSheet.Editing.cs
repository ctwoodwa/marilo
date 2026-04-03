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

        var lines = tsvData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        for (var r = 0; r < lines.Length && startRowIdx + r < _displayRows.Count; r++)
        {
            var cells = lines[r].Split('\t');
            for (var c = 0; c < cells.Length && startColIdx + c < _columns.Count; c++)
            {
                var column = _columns[startColIdx + c];
                if (!column.Editable || column.ColumnType == DataSheetColumnType.Computed) continue;

                var row = _displayRows[startRowIdx + r];
                object? parsedValue = ParseCellValue(column, cells[c].Trim());
                await CommitCellEdit(row, column.Field, parsedValue);
            }
        }
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

    private static object? ParseCellValue(MariloDataSheetColumn<TItem> column, string text)
    {
        return column.ColumnType switch
        {
            DataSheetColumnType.Number when decimal.TryParse(text, out var d) => d,
            DataSheetColumnType.Date when DateTime.TryParse(text, out var dt) => dt,
            DataSheetColumnType.Checkbox => text.Equals("true", StringComparison.OrdinalIgnoreCase)
                                          || text == "1",
            _ => text
        };
    }
}
