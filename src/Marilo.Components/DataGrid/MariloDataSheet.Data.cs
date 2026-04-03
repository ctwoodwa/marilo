using Marilo.Core.Enums;
using Marilo.Core.Helpers;
using Marilo.Core.Models.DataSheet;

namespace Marilo.Components.DataGrid;

/// <summary>
/// Dirty-state tracking, commit, reset, and validation logic for MariloDataSheet.
/// </summary>
public partial class MariloDataSheet<TItem>
{
    // ── Dirty State ────────────────────────────────────────────────────

    internal readonly Dictionary<object, DirtyRowEntry<TItem>> _dirtyRows = [];

    internal class DirtyRowEntry<T>
    {
        public T Original { get; set; } = default!;
        public T Current { get; set; } = default!;
        public HashSet<string> DirtyFields { get; } = [];
        public Dictionary<string, string> ValidationErrors { get; } = [];
        public bool IsDeleted { get; set; }
        public CellState OverallState => ValidationErrors.Count > 0 ? CellState.Invalid
                                        : DirtyFields.Count > 0 ? CellState.Dirty
                                        : CellState.Pristine;
    }

    // ── Key Resolution ─────────────────────────────────────────────────

    internal object? GetRowKey(TItem item)
    {
        return GridReflectionHelper.GetValue(item, KeyField);
    }

    // ── Cell Commit ────────────────────────────────────────────────────

    /// <summary>Commits a cell value change and updates dirty state.</summary>
    public async Task CommitCellEdit(TItem row, string field, object? newValue)
    {
        var key = GetRowKey(row);
        if (key is null) return;

        // Get or create dirty entry
        if (!_dirtyRows.TryGetValue(key, out var entry))
        {
            entry = new DirtyRowEntry<TItem>
            {
                Original = GridReflectionHelper.DeepClone(row),
                Current = row
            };
            _dirtyRows[key] = entry;
        }

        var oldValue = GridReflectionHelper.GetValue(row, field);

        // Set the new value
        GridReflectionHelper.SetValue(row, field, newValue);
        entry.DirtyFields.Add(field);

        // Run column validation
        var column = _columns.FirstOrDefault(c => c.Field == field);
        if (column != null)
        {
            var error = RunColumnValidation(column, row);
            if (error != null)
                entry.ValidationErrors[field] = error;
            else
                entry.ValidationErrors.Remove(field);
        }

        // Fire OnRowChanged
        if (OnRowChanged.HasDelegate)
        {
            await OnRowChanged.InvokeAsync(new DataSheetRowChangedArgs<TItem>
            {
                Row = row,
                Field = field,
                OldValue = oldValue,
                NewValue = newValue
            });
        }

        StateHasChanged();
    }

    // ── Validation ─────────────────────────────────────────────────────

    internal string? RunColumnValidation(MariloDataSheetColumn<TItem> column, TItem row)
    {
        // Required check
        if (column.Required)
        {
            var value = GridReflectionHelper.GetValue(row, column.Field);
            if (value is null || (value is string s && string.IsNullOrWhiteSpace(s)))
                return $"{column.DisplayTitle} is required.";
        }

        // Custom validate func
        if (column.Validate != null)
        {
            return column.Validate(row);
        }

        return null;
    }

    /// <summary>Runs full grid validation. Returns true if all cells are valid.</summary>
    public async Task<bool> ValidateAllAsync()
    {
        var hasErrors = false;

        foreach (var entry in _dirtyRows.Values)
        {
            if (entry.IsDeleted) continue;

            foreach (var column in _columns)
            {
                if (!column.Editable || column.ColumnType == DataSheetColumnType.Computed) continue;

                var error = RunColumnValidation(column, entry.Current);
                if (error != null)
                {
                    entry.ValidationErrors[column.Field] = error;
                    hasErrors = true;
                }
                else
                {
                    entry.ValidationErrors.Remove(column.Field);
                }
            }
        }

        // Also validate required fields on all dirty rows
        StateHasChanged();
        await Task.CompletedTask;
        return !hasErrors;
    }

    // ── Save All ───────────────────────────────────────────────────────

    /// <summary>Triggers the Save All flow: validate, then fire OnSaveAll.</summary>
    public async Task SaveAllAsync()
    {
        if (_dirtyRows.Count == 0) return;

        // Step 1: Validate all
        var isValid = await ValidateAllAsync();

        // Step 2: Fire OnValidate for consumer-side validation
        var dirtyRowsList = _dirtyRows.Values
            .Where(e => e.DirtyFields.Count > 0 && !e.IsDeleted)
            .Select(e => e.Current)
            .ToList();

        if (OnValidate.HasDelegate)
        {
            var validateArgs = new DataSheetValidateArgs<TItem>
            {
                DirtyRows = dirtyRowsList
            };
            await OnValidate.InvokeAsync(validateArgs);

            // Apply consumer errors to entries
            foreach (var error in validateArgs.Errors)
            {
                var errorKey = GetRowKey(error.Row);
                if (errorKey != null && _dirtyRows.TryGetValue(errorKey, out var entry))
                {
                    entry.ValidationErrors[error.Field] = error.Message;
                    isValid = false;
                }
            }
        }

        // Step 3: Block if invalid
        if (!isValid)
        {
            _ariaAnnouncement = "Save blocked: fix validation errors first.";
            StateHasChanged();
            return;
        }

        // Step 4: Fire OnSaveAll
        if (OnSaveAll.HasDelegate)
        {
            var deletedRows = _dirtyRows.Values
                .Where(e => e.IsDeleted)
                .Select(e => e.Current)
                .ToList();

            await OnSaveAll.InvokeAsync(new DataSheetSaveArgs<TItem>
            {
                DirtyRows = dirtyRowsList,
                DeletedRows = deletedRows
            });
        }

        _ariaAnnouncement = "Changes saved successfully.";
        StateHasChanged();
    }

    // ── Reset ──────────────────────────────────────────────────────────

    /// <summary>Discards all dirty state and restores original values.</summary>
    public Task ResetAsync()
    {
        foreach (var entry in _dirtyRows.Values)
        {
            if (!entry.IsDeleted)
            {
                // Restore original values to the current row object
                foreach (var field in entry.DirtyFields)
                {
                    var originalValue = GridReflectionHelper.GetValue(entry.Original, field);
                    GridReflectionHelper.SetValue(entry.Current, field, originalValue);
                }
            }
        }

        _dirtyRows.Clear();
        ClearActiveCell();
        _ariaAnnouncement = "All changes have been reset.";
        StateHasChanged();
        return Task.CompletedTask;
    }

    // ── Delete ─────────────────────────────────────────────────────────

    internal void MarkRowDeleted(TItem row)
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

        entry.IsDeleted = true;
    }

    // ── State Queries ──────────────────────────────────────────────────

    internal CellState GetCellState(TItem row, string field)
    {
        var key = GetRowKey(row);
        if (key is null || !_dirtyRows.TryGetValue(key, out var entry))
            return CellState.Pristine;

        if (entry.ValidationErrors.ContainsKey(field))
            return CellState.Invalid;

        if (entry.DirtyFields.Contains(field))
            return CellState.Dirty;

        return CellState.Pristine;
    }

    internal bool IsRowDirty(TItem row)
    {
        var key = GetRowKey(row);
        if (key is null || !_dirtyRows.TryGetValue(key, out var entry))
            return false;
        return entry.DirtyFields.Count > 0;
    }

    internal bool IsRowDeleted(TItem row)
    {
        var key = GetRowKey(row);
        if (key is null || !_dirtyRows.TryGetValue(key, out var entry))
            return false;
        return entry.IsDeleted;
    }

    internal string? GetCellError(TItem row, string field)
    {
        var key = GetRowKey(row);
        if (key is null || !_dirtyRows.TryGetValue(key, out var entry))
            return null;
        return entry.ValidationErrors.GetValueOrDefault(field);
    }
}
