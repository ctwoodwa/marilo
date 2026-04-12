using Marilo.Core.Base;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Marilo.Components.DataGrid;

public partial class MariloTreeList<TItem> : MariloComponentBase, IColumnHost, ITreeListEditController
{
    [Parameter] public IEnumerable<TItem> Data { get; set; } = Enumerable.Empty<TItem>();
    [Parameter] public string? IdField { get; set; }
    [Parameter] public string? ParentIdField { get; set; }
    [Parameter] public string? ItemsField { get; set; }
    [Parameter] public string? HasChildrenField { get; set; }
    [Parameter] public bool Sortable { get; set; }
    [Parameter] public EventCallback<TreeListSortEventArgs> OnSortChanged { get; set; }
    [Parameter] public EventCallback<TItem> OnExpand { get; set; }
    [Parameter] public EventCallback<TItem> OnCollapse { get; set; }
    [Parameter] public TreeListSelectionMode SelectionMode { get; set; } = TreeListSelectionMode.None;
    [Parameter] public IReadOnlyList<TItem>? SelectedItems { get; set; }
    [Parameter] public EventCallback<IReadOnlyList<TItem>> SelectedItemsChanged { get; set; }
    [Parameter] public EventCallback<TreeListSelectionEventArgs<TItem>> OnSelectionChanged { get; set; }
    [Parameter] public TreeListFilterMode FilterMode { get; set; } = TreeListFilterMode.None;
#pragma warning disable CS0618
    [Parameter][Obsolete("Use <MariloTreeListColumn> child components instead.")]
    public List<TreeListColumn>? Columns { get; set; }
#pragma warning restore CS0618
    [Parameter] public EventCallback<TItem> OnRowClick { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public TreeListEditMode EditMode { get; set; } = TreeListEditMode.None;
    [Parameter] public EventCallback<TreeListCommandEventArgs<TItem>> OnCreate { get; set; }
    [Parameter] public EventCallback<TreeListCommandEventArgs<TItem>> OnUpdate { get; set; }
    [Parameter] public EventCallback<TreeListCommandEventArgs<TItem>> OnDelete { get; set; }

    /// <summary>When true, columns can be resized by dragging the right edge of header cells.</summary>
    [Parameter] public bool Resizable { get; set; }

    /// <summary>When true, columns can be reordered by dragging header cells.</summary>
    [Parameter] public bool Reorderable { get; set; }

    /// <summary>Fires after a column is reordered via drag-and-drop.</summary>
    [Parameter] public EventCallback<TreeListColumnReorderEventArgs> OnColumnReordered { get; set; }

    private List<TreeListNode> _rootItems = new();
    private HashSet<string> _expandedIds = new();
    private string? _sortField;
    private SortDirection? _sortDirection;
    private HashSet<TItem> _selectedItemsSet = new();
    private readonly Dictionary<string, string> _filterValues = new();
    internal TItem? _editingItem;
    internal Dictionary<string, object?> _editingValues = new();
    internal bool _isNewItem;
    private readonly List<MariloColumnBase> _registeredColumns = new();
    private record TreeListNode(string Id, TItem Item, List<TreeListNode> Children, bool HasChildren);

    void IColumnHost.RegisterColumn(MariloColumnBase column)
    {
        if (!_registeredColumns.Contains(column)) { _registeredColumns.Add(column); InvokeAsync(StateHasChanged); }
    }

    void IColumnHost.UnregisterColumn(MariloColumnBase column)
    {
        if (_registeredColumns.Remove(column)) InvokeAsync(StateHasChanged);
    }

    internal List<IColumnDescriptor> EffectiveColumns
    {
        get
        {
            if (_registeredColumns.Count > 0) return _registeredColumns.Cast<IColumnDescriptor>().ToList();
#pragma warning disable CS0618
            if (Columns is { Count: > 0 }) return Columns.Select(c => (IColumnDescriptor)new LegacyColumnAdapter(c)).ToList();
#pragma warning restore CS0618
            return new();
        }
    }

    protected override void OnParametersSet()
    {
        if (SelectedItems is not null) _selectedItemsSet = new HashSet<TItem>(SelectedItems);
        _rootItems = BuildTree();
    }

    private List<TreeListNode> BuildTree()
    {
        var items = Data.ToList();
        if (!items.Any()) return new();
        items = ApplyFilter(items);
        items = ApplySort(items);
        if (!string.IsNullOrEmpty(ItemsField)) return BuildHierarchical(items.Cast<object>(), 0);
        if (!string.IsNullOrEmpty(IdField) && !string.IsNullOrEmpty(ParentIdField)) return BuildFlat(items);
        int idx = 0;
        return items.Select(i => new TreeListNode($"auto-{idx++}", i, new(), false)).ToList();
    }

    private List<TItem> ApplySort(List<TItem> items)
    {
        if (string.IsNullOrEmpty(_sortField) || _sortDirection is null) return items;
        var prop = typeof(TItem).GetProperty(_sortField);
        if (prop is null) return items;
        return _sortDirection == SortDirection.Ascending
            ? items.OrderBy(i => prop.GetValue(i)).ToList()
            : items.OrderByDescending(i => prop.GetValue(i)).ToList();
    }

    private bool IsColumnSortable(IColumnDescriptor col)
    {
        if (col is MariloColumnBase mcb && mcb.Sortable.HasValue) return mcb.Sortable.Value;
        return Sortable;
    }

    private async Task HandleHeaderClick(IColumnDescriptor col)
    {
        if (!IsColumnSortable(col)) return;
        var field = col.Field;
        if (_sortField == field)
        {
            _sortDirection = _sortDirection switch
            {
                SortDirection.Ascending => SortDirection.Descending,
                SortDirection.Descending => null,
                _ => SortDirection.Ascending
            };
            if (_sortDirection is null) _sortField = null;
        }
        else { _sortField = field; _sortDirection = SortDirection.Ascending; }
        _rootItems = BuildTree();
        if (OnSortChanged.HasDelegate)
            await OnSortChanged.InvokeAsync(new TreeListSortEventArgs { Field = _sortField, Direction = _sortDirection });
    }

    private List<TreeListNode> BuildHierarchical(IEnumerable<object> items, int depth)
    {
        var result = new List<TreeListNode>();
        int idx = 0;
        foreach (var item in items)
        {
            var id = GetProp(item, IdField) ?? $"h-{depth}-{idx++}";
            var children = new List<TreeListNode>();
            var childItems = item.GetType().GetProperty(ItemsField!)?.GetValue(item);
            if (childItems is System.Collections.IEnumerable en)
            {
                var list = en.Cast<object>().ToList();
                if (list.Any()) children = BuildHierarchical(list, depth + 1);
            }
            var hasKids = children.Any();
            if (!string.IsNullOrEmpty(HasChildrenField))
            {
                var v = item.GetType().GetProperty(HasChildrenField)?.GetValue(item);
                if (v is bool b) hasKids = b;
            }
            result.Add(new TreeListNode(id, (TItem)item, children, hasKids));
        }
        return result;
    }

    private List<TreeListNode> BuildFlat(List<TItem> items)
    {
        var lookup = new Dictionary<string, TreeListNode>();
        var roots = new List<TreeListNode>();
        foreach (var item in items)
        {
            var id = GetProp(item!, IdField) ?? "";
            var hasKids = false;
            if (!string.IsNullOrEmpty(HasChildrenField))
            {
                var v = item!.GetType().GetProperty(HasChildrenField)?.GetValue(item);
                if (v is bool b) hasKids = b;
            }
            lookup[id] = new TreeListNode(id, item, new(), hasKids);
        }
        foreach (var item in items)
        {
            var id = GetProp(item!, IdField) ?? "";
            var parentId = GetProp(item!, ParentIdField);
            if (string.IsNullOrEmpty(parentId) || !lookup.ContainsKey(parentId)) roots.Add(lookup[id]);
            else lookup[parentId].Children.Add(lookup[id]);
        }
        return roots;
    }

    private string? GetProp(object item, string? propName)
    {
        if (string.IsNullOrEmpty(propName)) return null;
        return item.GetType().GetProperty(propName)?.GetValue(item)?.ToString();
    }

    private List<TItem> ApplyFilter(List<TItem> items)
    {
        if (FilterMode == TreeListFilterMode.None || !_filterValues.Any(kv => !string.IsNullOrEmpty(kv.Value))) return items;
        var activeFilters = _filterValues.Where(kv => !string.IsNullOrEmpty(kv.Value)).ToList();
        if (!activeFilters.Any()) return items;
        if (!string.IsNullOrEmpty(IdField) && !string.IsNullOrEmpty(ParentIdField)) return ApplyFilterWithHierarchy(items, activeFilters);
        return items.Where(item => MatchesAllFilters(item, activeFilters)).ToList();
    }

    private List<TItem> ApplyFilterWithHierarchy(List<TItem> items, List<KeyValuePair<string, string>> activeFilters)
    {
        var matchingIds = new HashSet<string>();
        var parentIds = new HashSet<string>();
        foreach (var item in items)
        {
            if (MatchesAllFilters(item, activeFilters))
            {
                var id = GetProp(item!, IdField);
                if (id != null) matchingIds.Add(id);
                var parentId = GetProp(item!, ParentIdField);
                while (!string.IsNullOrEmpty(parentId))
                {
                    parentIds.Add(parentId);
                    var parent = items.FirstOrDefault(i => GetProp(i!, IdField) == parentId);
                    if (parent == null) break;
                    parentId = GetProp(parent!, ParentIdField);
                }
            }
        }
        return items.Where(item => { var id = GetProp(item!, IdField); return (id != null && matchingIds.Contains(id)) || (id != null && parentIds.Contains(id)); }).ToList();
    }

    private bool MatchesAllFilters(TItem item, List<KeyValuePair<string, string>> activeFilters)
    {
        foreach (var filter in activeFilters)
        {
            var prop = typeof(TItem).GetProperty(filter.Key);
            if (prop is null) continue;
            var value = prop.GetValue(item)?.ToString() ?? string.Empty;
            if (!value.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)) return false;
        }
        return true;
    }

    private bool IsColumnFilterable(IColumnDescriptor col)
    {
        if (col is MariloColumnBase mcb && mcb.Filterable.HasValue) return mcb.Filterable.Value;
        return true;
    }

    private void HandleFilterInput(string field, string value)
    {
        if (string.IsNullOrEmpty(value)) _filterValues.Remove(field);
        else _filterValues[field] = value;
        _rootItems = BuildTree();
    }

    private bool IsItemSelected(TItem item) => _selectedItemsSet.Contains(item);

    private async Task HandleRowClick(TItem item)
    {
        if (OnRowClick.HasDelegate) await OnRowClick.InvokeAsync(item);
        if (SelectionMode == TreeListSelectionMode.None) return;
        if (SelectionMode == TreeListSelectionMode.Single)
        {
            if (_selectedItemsSet.Contains(item)) _selectedItemsSet.Clear();
            else { _selectedItemsSet.Clear(); _selectedItemsSet.Add(item); }
        }
        else { if (!_selectedItemsSet.Remove(item)) _selectedItemsSet.Add(item); }
        var selectedList = _selectedItemsSet.ToList().AsReadOnly();
        if (SelectedItemsChanged.HasDelegate) await SelectedItemsChanged.InvokeAsync(selectedList);
        if (OnSelectionChanged.HasDelegate) await OnSelectionChanged.InvokeAsync(new TreeListSelectionEventArgs<TItem> { SelectedItems = selectedList });
    }

    private RenderFragment RenderRows(List<TreeListNode> nodes, int depth) => builder =>
    {
        int seq = 0;
        var columns = OrderedColumns;
        foreach (var node in nodes)
        {
            var isExpanded = _expandedIds.Contains(node.Id);
            var hasKids = node.Children.Any() || node.HasChildren;
            var nodeId = node.Id; var nodeItem = node.Item;
            var isSelected = IsItemSelected(node.Item);
            var isEditingRow = IsEditing(node.Item);
            var rowCss = isEditingRow ? "mar-treelist__row mar-treelist__row--editing" : isSelected ? "mar-treelist__row mar-treelist__row--selected" : "mar-treelist__row";
            builder.OpenElement(seq++, "tr");
            builder.AddAttribute(seq++, "class", rowCss);
            builder.AddAttribute(seq++, "role", "row");
            builder.AddAttribute(seq++, "aria-level", depth + 1);
            if (isSelected) builder.AddAttribute(seq++, "aria-selected", "true");
            builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () => HandleRowClick(node.Item)));
            if (EditMode == TreeListEditMode.Inline) builder.AddAttribute(seq++, "ondblclick", EventCallback.Factory.Create(this, () => HandleRowDoubleClick(node.Item)));
            for (var ci = 0; ci < columns.Count; ci++)
            {
                var col = columns[ci]; var cellTemplate = (col as MariloTreeListColumn)?.Template;
                builder.OpenElement(seq++, "td"); builder.AddAttribute(seq++, "class", "mar-treelist__td");
                var tdStyle = GetColumnWidthStyle(col);
                if (!string.IsNullOrEmpty(tdStyle)) builder.AddAttribute(seq++, "style", tdStyle);
                if (isEditingRow)
                {
                    var fieldName = col.Field; var currentVal = GetEditValue(fieldName)?.ToString() ?? "";
                    if (ci == 0)
                    {
                        builder.OpenElement(seq++, "span"); builder.AddAttribute(seq++, "style", $"padding-left: {depth * 20}px; display: inline-flex; align-items: center; gap: 4px;");
                        builder.OpenElement(seq++, "span"); builder.AddAttribute(seq++, "style", "width: 20px;"); builder.CloseElement();
                        builder.OpenElement(seq++, "input"); builder.AddAttribute(seq++, "type", "text"); builder.AddAttribute(seq++, "class", "mar-treelist__edit-input");
                        builder.AddAttribute(seq++, "value", currentVal);
                        builder.AddAttribute(seq++, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this, e => SetEditValue(fieldName, e.Value?.ToString() ?? "")));
                        builder.CloseElement(); builder.CloseElement();
                    }
                    else
                    {
                        builder.OpenElement(seq++, "input"); builder.AddAttribute(seq++, "type", "text"); builder.AddAttribute(seq++, "class", "mar-treelist__edit-input");
                        builder.AddAttribute(seq++, "value", currentVal);
                        builder.AddAttribute(seq++, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this, e => SetEditValue(fieldName, e.Value?.ToString() ?? "")));
                        builder.CloseElement();
                    }
                }
                else if (ci == 0)
                {
                    builder.OpenElement(seq++, "span"); builder.AddAttribute(seq++, "style", $"padding-left: {depth * 20}px; display: inline-flex; align-items: center; gap: 4px;");
                    if (hasKids) { builder.OpenElement(seq++, "button"); builder.AddAttribute(seq++, "type", "button"); builder.AddAttribute(seq++, "class", "mar-tree-item__toggle"); builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () => ToggleExpand(nodeId, nodeItem))); builder.AddEventStopPropagationAttribute(seq++, "onclick", true); builder.AddContent(seq++, isExpanded ? "\u25BC" : "\u25B6"); builder.CloseElement(); }
                    else { builder.OpenElement(seq++, "span"); builder.AddAttribute(seq++, "style", "width: 20px;"); builder.CloseElement(); }
                    if (cellTemplate is not null) builder.AddContent(seq++, cellTemplate((object)node.Item!));
                    else builder.AddContent(seq++, col.GetDisplayValue(node.Item));
                    builder.CloseElement();
                }
                else
                {
                    if (cellTemplate is not null) builder.AddContent(seq++, cellTemplate((object)node.Item!));
                    else builder.AddContent(seq++, col.GetDisplayValue(node.Item));
                }
                builder.CloseElement();
            }
            if (EditMode == TreeListEditMode.Inline)
            {
                builder.OpenElement(seq++, "td"); builder.AddAttribute(seq++, "class", "mar-treelist__td mar-treelist__td--commands");
                if (isEditingRow)
                {
                    builder.OpenElement(seq++, "button"); builder.AddAttribute(seq++, "type", "button"); builder.AddAttribute(seq++, "class", "mar-treelist__cmd-btn mar-treelist__cmd-btn--save"); builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, SaveEditInternalAsync)); builder.AddEventStopPropagationAttribute(seq++, "onclick", true); builder.AddContent(seq++, "Save"); builder.CloseElement();
                    builder.OpenElement(seq++, "button"); builder.AddAttribute(seq++, "type", "button"); builder.AddAttribute(seq++, "class", "mar-treelist__cmd-btn mar-treelist__cmd-btn--cancel"); builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, CancelEditInternal)); builder.AddEventStopPropagationAttribute(seq++, "onclick", true); builder.AddContent(seq++, "Cancel"); builder.CloseElement();
                }
                else
                {
                    builder.OpenElement(seq++, "button"); builder.AddAttribute(seq++, "type", "button"); builder.AddAttribute(seq++, "class", "mar-treelist__cmd-btn mar-treelist__cmd-btn--delete"); builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () => DeleteItem(node.Item))); builder.AddEventStopPropagationAttribute(seq++, "onclick", true); builder.AddContent(seq++, "Delete"); builder.CloseElement();
                }
                builder.CloseElement();
            }
            builder.CloseElement();
            if (hasKids && isExpanded) builder.AddContent(seq++, RenderRows(node.Children, depth + 1));
        }
    };

    private RenderFragment RenderEditRow(TItem item, int depth) => builder =>
    {
        int seq = 0; var columns = OrderedColumns;
        builder.OpenElement(seq++, "tr"); builder.AddAttribute(seq++, "class", "mar-treelist__row mar-treelist__row--editing mar-treelist__row--new"); builder.AddAttribute(seq++, "role", "row"); builder.AddAttribute(seq++, "aria-level", 1);
        for (var ci = 0; ci < columns.Count; ci++)
        {
            var col = columns[ci]; var fieldName = col.Field; var currentVal = GetEditValue(fieldName)?.ToString() ?? "";
            builder.OpenElement(seq++, "td"); builder.AddAttribute(seq++, "class", "mar-treelist__td");
            builder.OpenElement(seq++, "input"); builder.AddAttribute(seq++, "type", "text"); builder.AddAttribute(seq++, "class", "mar-treelist__edit-input"); builder.AddAttribute(seq++, "value", currentVal);
            builder.AddAttribute(seq++, "oninput", EventCallback.Factory.Create<ChangeEventArgs>(this, e => SetEditValue(fieldName, e.Value?.ToString() ?? "")));
            builder.CloseElement(); builder.CloseElement();
        }
        if (EditMode == TreeListEditMode.Inline)
        {
            builder.OpenElement(seq++, "td"); builder.AddAttribute(seq++, "class", "mar-treelist__td mar-treelist__td--commands");
            builder.OpenElement(seq++, "button"); builder.AddAttribute(seq++, "type", "button"); builder.AddAttribute(seq++, "class", "mar-treelist__cmd-btn mar-treelist__cmd-btn--save"); builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, SaveEditInternalAsync)); builder.AddContent(seq++, "Save"); builder.CloseElement();
            builder.OpenElement(seq++, "button"); builder.AddAttribute(seq++, "type", "button"); builder.AddAttribute(seq++, "class", "mar-treelist__cmd-btn mar-treelist__cmd-btn--cancel"); builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, CancelEditInternal)); builder.AddContent(seq++, "Cancel"); builder.CloseElement();
            builder.CloseElement();
        }
        builder.CloseElement();
    };

    private async Task ToggleExpand(string id, TItem item)
    {
        if (_expandedIds.Remove(id)) { if (OnCollapse.HasDelegate) await OnCollapse.InvokeAsync(item); }
        else { _expandedIds.Add(id); if (OnExpand.HasDelegate) await OnExpand.InvokeAsync(item); }
    }

    internal bool IsEditing(TItem item) => EditMode == TreeListEditMode.Inline && _editingItem is not null && EqualityComparer<TItem>.Default.Equals(_editingItem, item);

    private void BeginEdit(TItem item)
    {
        if (EditMode != TreeListEditMode.Inline) return;
        _editingItem = item; _isNewItem = false; _editingValues = new Dictionary<string, object?>();
        foreach (var col in EffectiveColumns) { var prop = typeof(TItem).GetProperty(col.Field); if (prop is not null) _editingValues[col.Field] = prop.GetValue(item); }
    }

    private async Task HandleRowDoubleClick(TItem item)
    {
        if (EditMode == TreeListEditMode.Inline) { BeginEdit(item); await InvokeAsync(StateHasChanged); }
    }

    internal void SetEditValue(string field, object? value) => _editingValues[field] = value;
    internal object? GetEditValue(string field) => _editingValues.TryGetValue(field, out var val) ? val : null;

    async Task ITreeListEditController.BeginAddAsync()
    {
        if (EditMode != TreeListEditMode.Inline) return;
        var newItem = Activator.CreateInstance<TItem>(); _editingItem = newItem; _isNewItem = true; _editingValues = new Dictionary<string, object?>();
        foreach (var col in EffectiveColumns) { var prop = typeof(TItem).GetProperty(col.Field); if (prop is not null) _editingValues[col.Field] = prop.GetValue(newItem); }
        await InvokeAsync(StateHasChanged);
    }

    async Task ITreeListEditController.SaveEditAsync() => await SaveEditInternalAsync();
    async Task ITreeListEditController.CancelEditAsync() { CancelEditInternal(); await InvokeAsync(StateHasChanged); }

    internal async Task SaveEditInternalAsync()
    {
        if (_editingItem is null) return;
        foreach (var kvp in _editingValues)
        {
            var prop = typeof(TItem).GetProperty(kvp.Key);
            if (prop is not null && prop.CanWrite)
            {
                try { var t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType; prop.SetValue(_editingItem, kvp.Value is null ? null : Convert.ChangeType(kvp.Value, t)); }
                catch { }
            }
        }
        if (_isNewItem) { if (OnCreate.HasDelegate) await OnCreate.InvokeAsync(new TreeListCommandEventArgs<TItem> { Item = _editingItem, IsNew = true }); }
        else { if (OnUpdate.HasDelegate) await OnUpdate.InvokeAsync(new TreeListCommandEventArgs<TItem> { Item = _editingItem }); }
        _editingItem = default; _editingValues.Clear(); _isNewItem = false;
    }

    internal void CancelEditInternal() { _editingItem = default; _editingValues.Clear(); _isNewItem = false; }

    private async Task DeleteItem(TItem item)
    {
        if (OnDelete.HasDelegate) await OnDelete.InvokeAsync(new TreeListCommandEventArgs<TItem> { Item = item });
    }

    // ── Column resize ────────────────────────────────────────────
    private readonly Dictionary<string, double> _runtimeWidths = new();
    private bool _isResizing;
    private string? _resizingField;
    private double _resizeStartX;
    private double _resizeStartWidth;

    internal string GetColumnWidthStyle(IColumnDescriptor col)
    {
        if (_runtimeWidths.TryGetValue(col.Field, out var w))
            return $"width:{w}px;min-width:{w}px;";
        return col.Width != null ? $"width:{col.Width};" : "";
    }

    private void OnResizeMouseDown(MouseEventArgs e, IColumnDescriptor col)
    {
        if (!Resizable) return;
        _isResizing = true;
        _resizingField = col.Field;
        _resizeStartX = e.ClientX;
        _resizeStartWidth = _runtimeWidths.TryGetValue(col.Field, out var w) ? w : 150;
    }

    private void OnResizeMouseMove(MouseEventArgs e)
    {
        if (!_isResizing || _resizingField is null) return;
        var delta = e.ClientX - _resizeStartX;
        var newWidth = Math.Max(40, _resizeStartWidth + delta);
        _runtimeWidths[_resizingField] = newWidth;
    }

    private void OnResizeMouseUp(MouseEventArgs e)
    {
        _isResizing = false;
        _resizingField = null;
    }

    // ── Column reorder ───────────────────────────────────────────
    private readonly List<int> _columnOrder = new();
    private int _dragSourceIndex = -1;
    private int _dragOverIndex = -1;

    internal List<IColumnDescriptor> OrderedColumns
    {
        get
        {
            var cols = EffectiveColumns;
            if (_columnOrder.Count != cols.Count)
            {
                _columnOrder.Clear();
                for (var i = 0; i < cols.Count; i++) _columnOrder.Add(i);
            }
            return _columnOrder.Select(i => i < cols.Count ? cols[i] : cols[0]).ToList();
        }
    }

    private void OnDragStart(DragEventArgs e, int index)
    {
        if (!Reorderable) return;
        _dragSourceIndex = index;
    }

    private void OnDragOver(DragEventArgs e, int index)
    {
        _dragOverIndex = index;
    }

    private async Task OnDrop(DragEventArgs e, int targetIndex)
    {
        if (!Reorderable || _dragSourceIndex < 0 || _dragSourceIndex == targetIndex) { _dragSourceIndex = -1; _dragOverIndex = -1; return; }
        var cols = EffectiveColumns;
        if (_columnOrder.Count != cols.Count)
        {
            _columnOrder.Clear();
            for (var i = 0; i < cols.Count; i++) _columnOrder.Add(i);
        }
        var movingOriginalIndex = _columnOrder[_dragSourceIndex];
        _columnOrder.RemoveAt(_dragSourceIndex);
        _columnOrder.Insert(targetIndex, movingOriginalIndex);
        var movedCol = cols[movingOriginalIndex];
        var oldIdx = _dragSourceIndex;
        _dragSourceIndex = -1;
        _dragOverIndex = -1;
        if (OnColumnReordered.HasDelegate)
            await OnColumnReordered.InvokeAsync(new TreeListColumnReorderEventArgs { Field = movedCol.Field, OldIndex = oldIdx, NewIndex = targetIndex });
    }

    private void OnDragEnd(DragEventArgs e)
    {
        _dragSourceIndex = -1;
        _dragOverIndex = -1;
    }
}
