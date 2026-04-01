using Marilo.Core.Base;
using Marilo.Core.Data;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Marilo.Components.DataGrid;

public partial class MariloDataGrid<TItem> : MariloComponentBase
{
    // ── Internal state ─────────────────────────────────────────────────
    internal readonly List<MariloGridColumn<TItem>> _columns = [];
    internal readonly GridState _state = new();
    internal List<TItem> _displayedItems = [];
    internal HashSet<TItem> _selectedItems = [];
    internal HashSet<TItem> _expandedDetailItems = [];
    private bool _stateInitialized;

    // Grouping state
    internal List<GridGroupRow<TItem>> _groupedRows = [];
    internal HashSet<string> _collapsedGroups = [];

    // Editing state (managed in MariloDataGrid.Editing.cs)
    internal TItem? _editingItem;
    internal TItem? _originalItem;
    internal bool _isCreating;
    internal string? _inCellEditingField;

    // AutoGenerate state
    internal bool _autoColumnsGenerated;

    // Search state
    internal string _searchText = "";

    // FilterMenu state
    internal string? _filterMenuField;
    internal FilterOperator _filterMenuOperator = FilterOperator.Contains;
    internal string _filterMenuValue = "";

    // ── Parameters: Data Binding ────────────────────────────────────────

    /// <summary>Client-side data source. Mutually exclusive with <see cref="OnRead"/>.</summary>
    [Parameter] public IEnumerable<TItem>? Data { get; set; }

    /// <summary>Server-side data read callback. When bound, the grid delegates sorting/filtering/paging to the consumer.</summary>
    [Parameter] public EventCallback<GridReadEventArgs<TItem>> OnRead { get; set; }

    // ── Parameters: Paging ──────────────────────────────────────────────

    /// <summary>Whether paging is enabled.</summary>
    [Parameter] public bool Pageable { get; set; }

    /// <summary>Number of items per page. Defaults to 10.</summary>
    [Parameter] public int PageSize { get; set; } = 10;

    /// <summary>The current page number (1-based). Supports two-way binding.</summary>
    [Parameter] public int Page { get; set; }

    /// <summary>Fires when the current page changes.</summary>
    [Parameter] public EventCallback<int> PageChanged { get; set; }

    /// <summary>Available page sizes for the page-size dropdown. Set to null to hide the dropdown.</summary>
    [Parameter] public int[]? PageSizes { get; set; }

    /// <summary>Fires when the page size changes.</summary>
    [Parameter] public EventCallback<int> PageSizeChanged { get; set; }

    // ── Parameters: Sorting, Filtering & Grouping ────────────────────────

    /// <summary>Whether sorting is enabled. Defaults to true.</summary>
    [Parameter] public bool Sortable { get; set; } = true;

    /// <summary>The filter mode for the grid.</summary>
    [Parameter] public GridFilterMode FilterMode { get; set; } = GridFilterMode.None;

    /// <summary>Whether grouping is enabled. When true, columns can be grouped via the API.</summary>
    [Parameter] public bool Groupable { get; set; }

    /// <summary>Template for group header rows. Receives a <see cref="GridGroupHeaderContext{TItem}"/> as context.</summary>
    [Parameter] public RenderFragment<GridGroupHeaderContext<TItem>>? GroupHeaderTemplate { get; set; }

    /// <summary>Template for group footer rows. Receives a <see cref="GridGroupHeaderContext{TItem}"/> as context.</summary>
    [Parameter] public RenderFragment<GridGroupHeaderContext<TItem>>? GroupFooterTemplate { get; set; }

    // ── Parameters: Selection ───────────────────────────────────────────

    /// <summary>The selection mode for the grid.</summary>
    [Parameter] public GridSelectionMode SelectionMode { get; set; } = GridSelectionMode.None;

    /// <summary>Whether to show a checkbox column for selection.</summary>
    [Parameter] public bool ShowCheckboxColumn { get; set; }

    /// <summary>The currently selected items. Supports two-way binding.</summary>
    [Parameter] public IEnumerable<TItem>? SelectedItems { get; set; }

    /// <summary>Fires when the selected items change.</summary>
    [Parameter] public EventCallback<IEnumerable<TItem>> SelectedItemsChanged { get; set; }

    // ── Parameters: Layout & Display ────────────────────────────────────

    /// <summary>Whether to apply striped row styling.</summary>
    [Parameter] public bool Striped { get; set; }

    /// <summary>The height of the grid (e.g. "400px", "50vh"). Enables vertical scrolling.</summary>
    [Parameter] public string? Height { get; set; }

    /// <summary>The width of the grid (e.g. "100%", "800px").</summary>
    [Parameter] public string? Width { get; set; }

    /// <summary>Whether keyboard navigation is enabled within the grid.</summary>
    [Parameter] public bool Navigable { get; set; }

    /// <summary>When true, columns are auto-generated from TItem's public properties if no explicit columns are defined.</summary>
    [Parameter] public bool AutoGenerateColumns { get; set; }

    /// <summary>Whether to use Blazor Virtualize for row rendering.</summary>
    [Parameter] public bool EnableVirtualization { get; set; }

    /// <summary>The overscan count for virtualization (rows rendered outside visible area). Defaults to 5.</summary>
    [Parameter] public int VirtualizeOverscanCount { get; set; } = 5;

    /// <summary>Whether data is currently loading. Shows a loading overlay.</summary>
    [Parameter] public bool IsLoading { get; set; }

    /// <summary>Whether to show a built-in search box above the grid. Searches across all visible columns.</summary>
    [Parameter] public bool ShowSearchBox { get; set; }

    /// <summary>Placeholder text for the search box.</summary>
    [Parameter] public string SearchBoxPlaceholder { get; set; } = "Search...";

    // ── Parameters: Templates ───────────────────────────────────────────

    /// <summary>Column definitions (MariloGridColumn components).</summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>Optional toolbar content rendered above the grid.</summary>
    [Parameter] public RenderFragment? ToolbarTemplate { get; set; }

    /// <summary>Detail row template for master-detail expansion. Receives the row item as context.</summary>
    [Parameter] public RenderFragment<TItem>? DetailTemplate { get; set; }

    /// <summary>Custom template displayed when the grid has no data.</summary>
    [Parameter] public RenderFragment? NoDataTemplate { get; set; }

    /// <summary>Custom row template. When set, replaces the default row rendering. Receives the row item as context.</summary>
    [Parameter] public RenderFragment<TItem>? RowTemplate { get; set; }

    // ── Parameters: State Events ────────────────────────────────────────

    /// <summary>Fires once when the grid initializes. Set properties on the provided GridState to restore saved state.</summary>
    [Parameter] public EventCallback<GridState> OnStateInit { get; set; }

    /// <summary>Fires after any state change (page, sort, filter, selection). Use this to persist grid state.</summary>
    [Parameter] public EventCallback<GridStateChangedEventArgs> OnStateChanged { get; set; }

    // ── Parameters: Row Events ──────────────────────────────────────────

    /// <summary>Fires when a row is clicked.</summary>
    [Parameter] public EventCallback<GridRowClickEventArgs<TItem>> OnRowClick { get; set; }

    /// <summary>Fires when a row is double-clicked.</summary>
    [Parameter] public EventCallback<GridRowClickEventArgs<TItem>> OnRowDoubleClick { get; set; }

    /// <summary>Fires when a row is right-clicked.</summary>
    [Parameter] public EventCallback<GridRowClickEventArgs<TItem>> OnRowContextMenu { get; set; }

    /// <summary>Callback for per-row render customization (CSS classes, styles).</summary>
    [Parameter] public Action<GridRowRenderEventArgs<TItem>>? OnRowRender { get; set; }

    /// <summary>Fires when a detail row is expanded.</summary>
    [Parameter] public EventCallback<TItem> OnRowExpand { get; set; }

    /// <summary>Fires when a detail row is collapsed.</summary>
    [Parameter] public EventCallback<TItem> OnRowCollapse { get; set; }

    // ── Parameters: Editing ─────────────────────────────────────────────

    /// <summary>The editing mode. Defaults to None (no editing).</summary>
    [Parameter] public GridEditMode EditMode { get; set; } = GridEditMode.None;

    /// <summary>Fires when the user initiates adding a new item.</summary>
    [Parameter] public EventCallback<GridEditEventArgs<TItem>> OnAdd { get; set; }

    /// <summary>Fires when a new item is confirmed for creation.</summary>
    [Parameter] public EventCallback<GridEditEventArgs<TItem>> OnCreate { get; set; }

    /// <summary>Fires when an existing item's edit is confirmed.</summary>
    [Parameter] public EventCallback<GridEditEventArgs<TItem>> OnUpdate { get; set; }

    /// <summary>Fires when an item is deleted.</summary>
    [Parameter] public EventCallback<GridEditEventArgs<TItem>> OnDelete { get; set; }

    /// <summary>Fires when a row enters edit mode.</summary>
    [Parameter] public EventCallback<GridEditEventArgs<TItem>> OnEdit { get; set; }

    /// <summary>Fires when editing is cancelled.</summary>
    [Parameter] public EventCallback<GridEditEventArgs<TItem>> OnCancel { get; set; }

    /// <summary>Fires when a new model is needed for creation. Set the Item property on the args.</summary>
    [Parameter] public EventCallback<GridModelInitEventArgs<TItem>> OnModelInit { get; set; }

    /// <summary>Fires when a custom command is executed on a row.</summary>
    [Parameter] public EventCallback<GridCommandEventArgs<TItem>> OnCommand { get; set; }

    // ── Column Registry ─────────────────────────────────────────────────

    internal void RegisterColumn(MariloGridColumn<TItem> column)
    {
        if (!_columns.Contains(column))
        {
            _columns.Add(column);
            StateHasChanged();
        }
    }

    internal void UnregisterColumn(MariloGridColumn<TItem> column)
    {
        _columns.Remove(column);
        StateHasChanged();
    }

    // ── Computed Properties ─────────────────────────────────────────────

    internal List<MariloGridColumn<TItem>> _visibleColumns => _columns.Where(c => c.Visible).ToList();

    internal int TotalColumnCount =>
        _visibleColumns.Count
        + (ShowCheckboxColumn ? 1 : 0)
        + (DetailTemplate != null ? 1 : 0)
        + (EditMode != GridEditMode.None && EditMode != GridEditMode.InCell ? 1 : 0);

    internal int TotalPages => _state.TotalCount > 0 && _state.PageSize > 0
        ? (int)Math.Ceiling((double)_state.TotalCount / _state.PageSize)
        : 1;

    private string? RootStyle
    {
        get
        {
            var parts = new List<string>();
            if (Width != null) parts.Add($"width:{Width};");
            return parts.Count > 0 ? string.Join("", parts) : null;
        }
    }

    private string? ContentStyle
    {
        get
        {
            if (Height == null) return null;
            return $"max-height:{Height};overflow:auto;";
        }
    }

    // ── Lifecycle ───────────────────────────────────────────────────────

    protected override async Task OnParametersSetAsync()
    {
        _state.PageSize = PageSize;

        if (Page > 0)
            _state.CurrentPage = Page;

        if (SelectedItems != null)
            _selectedItems = new HashSet<TItem>(SelectedItems);

        if (!_stateInitialized)
        {
            _stateInitialized = true;
            if (OnStateInit.HasDelegate)
            {
                await OnStateInit.InvokeAsync(_state);
            }
        }

        // Auto-generate columns from TItem's public properties
        if (AutoGenerateColumns && !_autoColumnsGenerated && _columns.Count == 0)
        {
            _autoColumnsGenerated = true;
            GenerateColumnsFromModel();
        }

        await ProcessDataAsync();
    }

    private void GenerateColumnsFromModel()
    {
        var props = typeof(TItem).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (var prop in props)
        {
            // Skip indexers and non-readable properties
            if (prop.GetIndexParameters().Length > 0 || !prop.CanRead) continue;

            // Skip complex types (collections, etc.) — only include simple/primitive types
            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            if (!IsSimpleType(type)) continue;

            var col = new MariloGridColumn<TItem>();
            // Set properties via reflection since Parameters are normally set by Blazor
            typeof(MariloGridColumn<TItem>).GetProperty(nameof(MariloGridColumn<TItem>.Field))!.SetValue(col, prop.Name);
            typeof(MariloGridColumn<TItem>).GetProperty(nameof(MariloGridColumn<TItem>.Title))!.SetValue(col, SplitCamelCase(prop.Name));
            _columns.Add(col);
        }
    }

    private static bool IsSimpleType(Type type) =>
        type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal)
        || type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(TimeSpan)
        || type == typeof(Guid) || type == typeof(DateOnly) || type == typeof(TimeOnly);

    private static string SplitCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var result = new System.Text.StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            if (i > 0 && char.IsUpper(input[i]) && !char.IsUpper(input[i - 1]))
                result.Append(' ');
            result.Append(input[i]);
        }
        return result.ToString();
    }

    // ── Public API ──────────────────────────────────────────────────────

    /// <summary>Gets a snapshot of the current grid state.</summary>
    public GridState GetState() => new()
    {
        CurrentPage = _state.CurrentPage,
        PageSize = _state.PageSize,
        SortDescriptors = _state.SortDescriptors.Select(s => new SortDescriptor { Field = s.Field, Direction = s.Direction }).ToList(),
        FilterDescriptors = _state.FilterDescriptors.Select(f => new FilterDescriptor { Field = f.Field, Operator = f.Operator, Value = f.Value }).ToList(),
        GroupDescriptors = _state.GroupDescriptors.Select(g => new GroupDescriptor { Field = g.Field, Direction = g.Direction }).ToList(),
        TotalCount = _state.TotalCount,
        SearchFilter = _searchText,
        EditItem = _editingItem,
        OriginalEditItem = _originalItem,
        InsertedItem = _isCreating ? _editingItem : default,
        CollapsedGroups = new HashSet<string>(_collapsedGroups),
        ColumnStates = _visibleColumns.Select((c, i) => new GridColumnState
        {
            Field = c.Field,
            Width = c.Width,
            Order = i,
            Visible = c.Visible
        }).ToList()
    };

    /// <summary>Whether a row is currently being edited.</summary>
    public bool IsEditing => _editingItem != null;

    /// <summary>Whether a new item is being created.</summary>
    public bool IsCreating => _isCreating;

    /// <summary>Checks if the given item is currently being edited.</summary>
    internal bool IsItemEditing(TItem item) => _editingItem != null && EqualityComparer<TItem>.Default.Equals(_editingItem, item);

    /// <summary>Forces the grid to re-read its data source.</summary>
    public async Task Rebind()
    {
        await ProcessDataAsync();
        StateHasChanged();
    }

    // ── State Notification ──────────────────────────────────────────────

    internal async Task NotifyPageChanged()
    {
        if (PageChanged.HasDelegate)
            await PageChanged.InvokeAsync(_state.CurrentPage);
    }

    internal async Task NotifyStateChanged(string propertyName)
    {
        if (OnStateChanged.HasDelegate)
        {
            await OnStateChanged.InvokeAsync(new GridStateChangedEventArgs
            {
                PropertyName = propertyName,
                State = GetState()
            });
        }
    }

    internal static string? GetAriaSortValue(SortDescriptor? sort)
    {
        if (sort is null) return null;
        return sort.Direction == SortDirection.Ascending ? "ascending" : "descending";
    }
}
