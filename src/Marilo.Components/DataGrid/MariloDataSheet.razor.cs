using Marilo.Core.Base;
using Marilo.Core.Enums;
using Marilo.Core.Models.DataSheet;
using Microsoft.AspNetCore.Components;

namespace Marilo.Components.DataGrid;

/// <summary>
/// A spreadsheet-like, inline-editable data grid with bulk-save, cell-level
/// dirty tracking, and Excel-style keyboard/paste UX.
/// </summary>
public partial class MariloDataSheet<TItem> : MariloComponentBase
{
    // ── Internal state ─────────────────────────────────────────────────
    internal readonly List<MariloDataSheetColumn<TItem>> _columns = [];
    internal List<TItem> _displayRows = [];
    internal readonly HashSet<TItem> _selectedRows = [];
    internal string _ariaAnnouncement = "";

    // ── Parameters: Data Binding ────────────────────────────────────────

    /// <summary>Client-side data source.</summary>
    [Parameter] public IEnumerable<TItem>? Data { get; set; }

    /// <summary>Property name used as row key for dirty tracking. Defaults to "Id".</summary>
    [Parameter] public string KeyField { get; set; } = "Id";

    // ── Parameters: Events ──────────────────────────────────────────────

    /// <summary>Fires with dirty rows when Save All is confirmed.</summary>
    [Parameter] public EventCallback<DataSheetSaveArgs<TItem>> OnSaveAll { get; set; }

    /// <summary>Fires after each cell commit.</summary>
    [Parameter] public EventCallback<DataSheetRowChangedArgs<TItem>> OnRowChanged { get; set; }

    /// <summary>Fires before Save All; handler may add errors to args to block save.</summary>
    [Parameter] public EventCallback<DataSheetValidateArgs<TItem>> OnValidate { get; set; }

    // ── Parameters: State ───────────────────────────────────────────────

    /// <summary>Shows saving spinner; disables Save All button.</summary>
    [Parameter] public bool IsSaving { get; set; }

    /// <summary>Shows "+ Add Row" button in toolbar.</summary>
    [Parameter] public bool AllowAddRow { get; set; }

    /// <summary>Shows delete button per row and in bulk action bar.</summary>
    [Parameter] public bool AllowDeleteRow { get; set; }

    /// <summary>Enables Ctrl+V TSV paste into cell range.</summary>
    [Parameter] public bool AllowBulkPaste { get; set; } = true;

    /// <summary>Text shown when Data is empty.</summary>
    [Parameter] public string EmptyStateMessage { get; set; } = "No data.";

    /// <summary>Container height. Enables vertical scroll + sticky header.</summary>
    [Parameter] public string? Height { get; set; }

    /// <summary>Shows loading skeleton rows.</summary>
    [Parameter] public bool IsLoading { get; set; }

    /// <summary>Use Virtualize for row rendering. Defaults to true.</summary>
    [Parameter] public bool EnableVirtualization { get; set; } = true;

    /// <summary>Accessible label on role="grid" element.</summary>
    [Parameter] public string AriaLabel { get; set; } = "Editable data grid";

    // ── Parameters: Templates ───────────────────────────────────────────

    /// <summary>MariloDataSheetColumn definitions.</summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>Additional toolbar content (renders inside toolbar area).</summary>
    [Parameter] public RenderFragment? ToolbarTemplate { get; set; }

    // ── Column Registry ─────────────────────────────────────────────────

    internal void RegisterColumn(MariloDataSheetColumn<TItem> column)
    {
        if (!_columns.Contains(column))
        {
            _columns.Add(column);
            StateHasChanged();
        }
    }

    internal void UnregisterColumn(MariloDataSheetColumn<TItem> column)
    {
        _columns.Remove(column);
        StateHasChanged();
    }

    // ── Computed ────────────────────────────────────────────────────────

    private string? _rootStyle => Height != null ? null : null;

    private string? _contentStyle => Height != null ? $"max-height:{Height};overflow:auto;" : null;

    internal int _totalColumnCount =>
        _columns.Count
        + (AllowDeleteRow ? 2 : 0); // checkbox + actions columns

    // ── Lifecycle ───────────────────────────────────────────────────────

    protected override void OnParametersSet()
    {
        if (Data != null)
        {
            _displayRows = Data.ToList();
        }
        else
        {
            _displayRows = [];
        }
    }

    // ── Public API ──────────────────────────────────────────────────────

    /// <summary>Returns the current dirty row snapshot.</summary>
    public IReadOnlyList<TItem> GetDirtyRows()
    {
        return _dirtyRows
            .Where(kv => kv.Value.DirtyFields.Count > 0 && !kv.Value.IsDeleted)
            .Select(kv => kv.Value.Current)
            .ToList();
    }

    /// <summary>Replaces the dataset without full re-render.</summary>
    public Task SetDataAsync(IEnumerable<TItem> data)
    {
        _displayRows = data.ToList();
        _dirtyRows.Clear();
        ClearActiveCell();
        StateHasChanged();
        return Task.CompletedTask;
    }

    // ── Selection ───────────────────────────────────────────────────────

    internal void OnSelectAllChanged(ChangeEventArgs e)
    {
        var selectAll = e.Value is true or "true";
        if (selectAll)
        {
            foreach (var row in _displayRows)
                _selectedRows.Add(row);
        }
        else
        {
            _selectedRows.Clear();
        }
    }

    internal void ToggleRowSelection(TItem row)
    {
        if (!_selectedRows.Remove(row))
            _selectedRows.Add(row);
    }

    internal async Task BulkDeleteAsync()
    {
        foreach (var row in _selectedRows.ToList())
        {
            MarkRowDeleted(row);
        }
        _selectedRows.Clear();
        StateHasChanged();
        await Task.CompletedTask;
    }

    internal async Task BulkResetAsync()
    {
        foreach (var row in _selectedRows.ToList())
        {
            var key = GetRowKey(row);
            if (key != null) _dirtyRows.Remove(key);
        }
        _selectedRows.Clear();
        StateHasChanged();
        await Task.CompletedTask;
    }

    internal async Task AddRowAsync()
    {
        if (!AllowAddRow) return;
        var newItem = Activator.CreateInstance<TItem>();
        _displayRows.Insert(0, newItem);
        StateHasChanged();
        await Task.CompletedTask;
    }
}
