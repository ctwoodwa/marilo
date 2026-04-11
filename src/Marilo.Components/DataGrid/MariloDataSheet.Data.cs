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

        // V05.3 — Rows created by AddRowAsync are flagged so ResetAsync can
        // remove them from _displayRows entirely instead of trying to restore
        // nonexistent "original" values.
        public bool IsNewlyAdded { get; set; }

        // V02.2 / V05.1 — Transient state override for Save All lifecycle.
        // When set, GetCellState returns this value instead of the computed
        // Invalid/Dirty/Pristine state. SaveAllAsync assigns CellState.Saving
        // before OnSaveAll fires and CellState.Saved after it succeeds, then
        // clears the override to complete the transition to Pristine.
        public CellState? TransientState { get; set; }

        public CellState OverallState => TransientState
                                        ?? (ValidationErrors.Count > 0 ? CellState.Invalid
                                        : DirtyFields.Count > 0 ? CellState.Dirty
                                        : CellState.Pristine);
    }

    // V02.2 / V05.1 — Duration of the CellState.Saved visual indicator
    // before cells transition to Pristine. Kept configurable internally so
    // tests can shrink it to zero for deterministic assertions.
    internal int _savedStateDurationMs = 1000;

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

        // Get or create dirty entry. The TryGetValue result doubles as
        // "is this the first touch on this row?" — on a first touch we
        // snapshot the current row as Original before any mutation.
        var hadExistingEntry = _dirtyRows.TryGetValue(key, out var entry);
        if (!hadExistingEntry)
        {
            entry = new DirtyRowEntry<TItem>
            {
                Original = GridReflectionHelper.DeepClone(row),
                Current = row
            };
            _dirtyRows[key] = entry;
        }

        var oldValue = GridReflectionHelper.GetValue(row, field);
        var originalValue = GridReflectionHelper.GetValue(entry!.Original, field);

        // Set the new value
        GridReflectionHelper.SetValue(row, field, newValue);

        // NOTE: object.Equals uses reference equality for user POCOs that do
        // not override Equals. For value types and strings this is correct;
        // for reference-typed cell values the caller must override Equals
        // or the revert detection will not fire.
        // V05.3 — Newly-added rows keep their dirty fields regardless of
        // value comparison: the "original" snapshot is the default TItem,
        // so a user typing the default value should NOT drop the field.
        // First-touch edits always add the field too; the very first commit
        // on a previously-clean row means the cell IS being touched.
        var revertedToOriginal = object.Equals(newValue, originalValue);
        if (hadExistingEntry && !entry.IsNewlyAdded && revertedToOriginal)
        {
            entry.DirtyFields.Remove(field);
        }
        else
        {
            entry.DirtyFields.Add(field);
        }

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

        // If the row has no remaining dirty fields and is not deleted or
        // newly added, remove its entry entirely so state queries report
        // Pristine. Newly added rows are preserved so SaveAllAsync still
        // emits them in DataSheetSaveArgs.DirtyRows.
        if (entry.DirtyFields.Count == 0 && !entry.IsDeleted && !entry.IsNewlyAdded)
        {
            _dirtyRows.Remove(key);
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

            // Checkbox: a required checkbox must be checked (true).
            if (column.ColumnType == DataSheetColumnType.Checkbox)
            {
                if (value is null || value is false)
                    return $"{column.DisplayTitle} is required.";
            }
            else if (value is null || (value is string s && string.IsNullOrWhiteSpace(s)))
            {
                return $"{column.DisplayTitle} is required.";
            }
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

        // Step 4: Mark non-deleted dirty entries as Saving so the cell state
        // transitions from Dirty -> Saving before OnSaveAll fires. (V02.2 / V05.1)
        foreach (var entry in _dirtyRows.Values)
        {
            if (!entry.IsDeleted)
            {
                entry.TransientState = CellState.Saving;
            }
        }
        StateHasChanged();

        // Step 5: Fire OnSaveAll
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

        // Step 6: On success, remove deleted rows from _displayRows (V05.2),
        // update original snapshots for saved dirty rows so subsequent edits
        // that revert to the just-saved value are correctly dirty-tracked,
        // and mark dirty entries as Saved for a brief visual indicator.
        var deletedKeys = new HashSet<object>();
        foreach (var kv in _dirtyRows)
        {
            if (kv.Value.IsDeleted)
            {
                deletedKeys.Add(kv.Key);
            }
        }

        if (deletedKeys.Count > 0)
        {
            _displayRows.RemoveAll(row =>
            {
                var k = GetRowKey(row);
                return k != null && deletedKeys.Contains(k);
            });
            foreach (var k in deletedKeys)
            {
                _dirtyRows.Remove(k);
            }
        }

        // For remaining (non-deleted) dirty entries, update the original
        // snapshot to reflect the just-saved current values and flip to
        // CellState.Saved for the brief visual indicator period.
        var savedKeys = _dirtyRows.Keys.ToList();
        foreach (var key in savedKeys)
        {
            if (_dirtyRows.TryGetValue(key, out var entry))
            {
                entry.Original = GridReflectionHelper.DeepClone(entry.Current);
                entry.IsNewlyAdded = false;
                entry.TransientState = CellState.Saved;
            }
        }

        _ariaAnnouncement = "Changes saved successfully.";
        StateHasChanged();

        // Step 7: After a brief visual indicator period, clear the
        // TransientState and drop the entries so cells report Pristine.
        if (_savedStateDurationMs > 0)
        {
            await Task.Delay(_savedStateDurationMs);
        }

        foreach (var key in savedKeys)
        {
            if (_dirtyRows.TryGetValue(key, out var entry) && entry.TransientState == CellState.Saved)
            {
                entry.TransientState = null;
                entry.DirtyFields.Clear();
                entry.ValidationErrors.Clear();
                _dirtyRows.Remove(key);
            }
        }
        StateHasChanged();
    }

    // ── Reset ──────────────────────────────────────────────────────────

    /// <summary>Discards all dirty state and restores original values.</summary>
    public Task ResetAsync()
    {
        // V05.3 — Collect rows created via AddRowAsync so they can be
        // removed from _displayRows entirely. A newly-added row has no
        // meaningful Original snapshot and cannot be "restored" to
        // anything — the spec says added rows are removed on reset.
        var newlyAddedRows = _dirtyRows.Values
            .Where(e => e.IsNewlyAdded)
            .Select(e => e.Current)
            .ToList();

        foreach (var entry in _dirtyRows.Values)
        {
            if (entry.IsNewlyAdded || entry.IsDeleted)
            {
                continue;
            }

            // Restore original values to the current row object
            foreach (var field in entry.DirtyFields)
            {
                var originalValue = GridReflectionHelper.GetValue(entry.Original, field);
                GridReflectionHelper.SetValue(entry.Current, field, originalValue);
            }
        }

        if (newlyAddedRows.Count > 0)
        {
            foreach (var row in newlyAddedRows)
            {
                _displayRows.Remove(row);
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

        // V05.4 — Toggle delete state. Clicking delete again on a row that
        // is already marked for deletion restores it to its prior editable
        // state. If the row has no other dirty tracking (no dirty fields,
        // not newly added), drop the entry entirely so it goes back to
        // Pristine instead of lingering as an empty entry.
        entry.IsDeleted = !entry.IsDeleted;

        if (!entry.IsDeleted
            && !entry.IsNewlyAdded
            && entry.DirtyFields.Count == 0
            && entry.ValidationErrors.Count == 0)
        {
            _dirtyRows.Remove(key);
        }
    }

    // ── State Queries ──────────────────────────────────────────────────

    internal CellState GetCellState(TItem row, string field)
    {
        var key = GetRowKey(row);
        if (key is null || !_dirtyRows.TryGetValue(key, out var entry))
            return CellState.Pristine;

        // V02.2 / V05.1 — A transient Save All override applies to every
        // dirty cell on the row while the save is in flight (Saving) or
        // briefly after it succeeds (Saved). It wins over the computed
        // Invalid/Dirty states but only for fields that are actually in
        // DirtyFields; non-dirty fields on the row remain Pristine.
        if (entry.TransientState is { } transient && entry.DirtyFields.Contains(field))
        {
            return transient;
        }

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
