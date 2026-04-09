using Marilo.Core.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Marilo.Components.DataDisplay;

public partial class MariloGantt<TItem> : MariloComponentBase, IAsyncDisposable
    where TItem : class
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    private GanttFieldAccessor<TItem>? _accessor;
    private string? _accessorKey;
    private IEnumerable<TItem>? _lastData;
    private int _lastDataCount = -1;
    private readonly List<GanttNode<TItem>> _roots = new();
    private List<GanttNode<TItem>> _flatVisible = new();
    private readonly HashSet<object> _expandedIds = new();

#pragma warning disable CS0649 // Assigned in D1 phase (JS interop)
    private IJSObjectReference? _jsModule;
    private DotNetObjectReference<MariloGantt<TItem>>? _dotNetRef;
#pragma warning restore CS0649

    [Parameter] public IEnumerable<TItem> Data { get; set; } = Enumerable.Empty<TItem>();

    [Parameter] public string IdField { get; set; } = "Id";
    [Parameter] public string ParentIdField { get; set; } = "ParentId";
    [Parameter] public string TitleField { get; set; } = "Title";
    [Parameter] public string StartField { get; set; } = "Start";
    [Parameter] public string EndField { get; set; } = "End";
    [Parameter] public string PercentCompleteField { get; set; } = "PercentComplete";
    [Parameter] public string DependsOnField { get; set; } = "DependsOn";

    [Parameter] public string? Width { get; set; }
    [Parameter] public string? Height { get; set; }

    [Parameter] public int TaskListWidth { get; set; } = 250;
    [Parameter] public int DayWidth { get; set; } = 30;
    [Parameter] public int RowHeight { get; set; } = 36;

    [Parameter] public EventCallback<TItem> OnTaskClick { get; set; }
    [Parameter] public EventCallback<TItem> OnTaskEdit { get; set; }

    /// <summary>Child content slot where GanttColumn instances are declared. Rendered inside a CascadingValue so columns can discover the parent.</summary>
    [Parameter] public RenderFragment? GanttColumns { get; set; }

    // Column management
    private readonly List<GanttColumn<TItem>> _columns = new();
    internal void RegisterColumn(GanttColumn<TItem> column)
    {
        if (!_columns.Contains(column))
        {
            _columns.Add(column);
            StateHasChanged();
        }
    }
    internal void UnregisterColumn(GanttColumn<TItem> column)
    {
        _columns.Remove(column);
        StateHasChanged();
    }
    internal List<GanttColumn<TItem>> VisibleColumns => _columns.Where(c => c.Visible).ToList();

    protected override void OnParametersSet()
    {
        var prevKey = _accessorKey;
        _accessor = GetAccessor();
        var keyChanged = prevKey != _accessorKey;
        var refChanged = !ReferenceEquals(_lastData, Data);
        if (refChanged || keyChanged)
        {
            var newCount = (Data ?? Enumerable.Empty<TItem>()).Count();
            var skipEmpty = !keyChanged && newCount == 0 && _lastDataCount == 0;
            if (!skipEmpty)
            {
                _lastData = Data;
                _lastDataCount = newCount;
                BuildTree();
                RebuildFlatVisible();
            }
        }
        base.OnParametersSet();
    }

    /// <summary>Refreshes the Gantt's internal tree from the current Data collection. Call this after mutating Data in place.</summary>
    public async Task Rebind()
    {
        _lastData = null;
        _lastDataCount = -1;
        BuildTree();
        RebuildFlatVisible();
        _lastData = Data;
        _lastDataCount = (Data ?? Enumerable.Empty<TItem>()).Count();
        await InvokeAsync(StateHasChanged);
    }

    private GanttFieldAccessor<TItem> GetAccessor()
    {
        var key = $"{IdField}|{ParentIdField}|{TitleField}|{StartField}|{EndField}|{PercentCompleteField}|{DependsOnField}";
        if (_accessor is null || _accessorKey != key)
        {
            _accessor = new GanttFieldAccessor<TItem>(
                IdField, ParentIdField, TitleField, StartField, EndField, PercentCompleteField, DependsOnField);
            _accessorKey = key;
        }
        return _accessor;
    }

    private void BuildTree()
    {
        _roots.Clear();
        var accessor = _accessor!;
        var items = (Data ?? Enumerable.Empty<TItem>()).ToList();
        var byId = new Dictionary<object, GanttNode<TItem>>();
        var ordered = new List<GanttNode<TItem>>(items.Count);

        foreach (var item in items)
        {
            var node = new GanttNode<TItem>
            {
                Item = item,
                Id = accessor.GetId(item),
                ParentId = accessor.GetParentId(item),
            };
            ordered.Add(node);
            if (node.Id is not null && !byId.ContainsKey(node.Id))
            {
                byId[node.Id] = node;
            }
        }

        // Prune stale expanded ids from deleted nodes so reappearing ids get default-expanded treatment
        _expandedIds.IntersectWith(byId.Keys);

        foreach (var node in ordered)
        {
            if (node.ParentId is not null
                && byId.TryGetValue(node.ParentId, out var parent)
                && !WouldCreateCycle(node, parent))
            {
                parent.Children.Add(node);
                node.Parent = parent;
            }
            else
            {
                _roots.Add(node);
            }
        }

        // Depth assignment via DFS from roots
        var stack = new Stack<GanttNode<TItem>>();
        foreach (var r in _roots)
        {
            r.Depth = 0;
            stack.Push(r);
        }
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            foreach (var c in n.Children)
            {
                c.Depth = n.Depth + 1;
                stack.Push(c);
            }
        }

        // Seed expanded state for nodes with children, preserving existing entries
        foreach (var node in ordered)
        {
            if (node.Id is not null && node.Children.Count > 0 && !_expandedIds.Contains(node.Id))
            {
                _expandedIds.Add(node.Id);
            }
        }
    }

    private static bool WouldCreateCycle(GanttNode<TItem> candidateChild, GanttNode<TItem> candidateParent)
    {
        var cursor = candidateParent;
        while (cursor is not null)
        {
            if (ReferenceEquals(cursor, candidateChild)) return true;
            cursor = cursor.Parent;
        }
        return false;
    }

    private bool IsExpanded(GanttNode<TItem> node)
        => node.Id is null || _expandedIds.Contains(node.Id);

    private void RebuildFlatVisible()
    {
        var list = new List<GanttNode<TItem>>();
        var visited = new HashSet<GanttNode<TItem>>();
        var stack = new Stack<GanttNode<TItem>>();
        for (int i = _roots.Count - 1; i >= 0; i--) stack.Push(_roots[i]);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (!visited.Add(n)) continue;
            list.Add(n);
            if (IsExpanded(n))
            {
                for (int i = n.Children.Count - 1; i >= 0; i--) stack.Push(n.Children[i]);
            }
        }
        _flatVisible = list;
    }

    private async Task ToggleExpanded(GanttNode<TItem> node)
    {
        if (node.Id is null) return;
        if (!_expandedIds.Add(node.Id))
        {
            _expandedIds.Remove(node.Id);
        }
        RebuildFlatVisible();
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            try { await _jsModule.DisposeAsync(); }
            catch (Exception ex) when (ex is JSDisconnectedException or TaskCanceledException or ObjectDisposedException) { }
        }
        _dotNetRef?.Dispose();
    }
}
