using System.Globalization;
using Marilo.Core.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Marilo.Components.DataDisplay;

public partial class MariloGantt<TItem> : MariloComponentBase, IGanttViewHost, IAsyncDisposable
    where TItem : class
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    internal GanttFieldAccessor<TItem>? _accessor;
    private string? _accessorKey;
    private IEnumerable<TItem>? _lastData;
    private int _lastDataCount = -1;
    private readonly List<GanttNode<TItem>> _roots = new();
    private List<GanttNode<TItem>> _flatVisible = new();
    private readonly HashSet<object> _expandedIds = new();

    // ── Dependency rendering ──────────────────────────────────────────
    private record DependencyLine(double X1, double Y1, double X2, double Y2);
    private List<DependencyLine> _dependencyLines = new();
    private readonly string _instanceId = Guid.NewGuid().ToString("N")[..8];

    // ── Keyboard navigation ─────────────────────────────────────────
    private int _focusedIndex;

    // ── Filter state ──────────────────────────────────────────────────
    private readonly Dictionary<string, string> _filterValues = new();
    /// <summary>Ids that were auto-expanded by the filter so we can restore state when cleared.</summary>
    private readonly HashSet<object> _filterExpandedIds = new();

    // ── Sort state ────────────────────────────────────────────────────
    private string? _sortField;
    private bool _sortAscending = true;
    /// <summary>Tri-state cycle counter: 0 = first click (asc), 1 = second (desc), 2 = third (clear).</summary>
    private int _sortCycleStep;

    private IJSObjectReference? _jsModule;
    private IJSObjectReference? _jsInstance;
    private DotNetObjectReference<MariloGantt<TItem>>? _dotNetRef;
    private ElementReference _containerRef;

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

    /// <summary>Custom template for the inner content of timeline bars. Replaces the default progress fill. Receives the task item as context.</summary>
    [Parameter] public RenderFragment<TItem>? TaskTemplate { get; set; }

    /// <summary>Custom tooltip template shown on bar hover. Receives the task item as context. When null, a default tooltip with title + dates is shown.</summary>
    [Parameter] public RenderFragment<TItem>? TooltipTemplate { get; set; }

    [Parameter] public EventCallback<TItem> OnTaskClick { get; set; }
    [Parameter] public EventCallback<TItem> OnTaskEdit { get; set; }

    [Parameter] public EventCallback<GanttCreateEventArgs> OnCreate { get; set; }
    [Parameter] public EventCallback<GanttUpdateEventArgs> OnUpdate { get; set; }
    [Parameter] public EventCallback<GanttDeleteEventArgs> OnDelete { get; set; }
    [Parameter] public EventCallback<GanttExpandEventArgs> OnExpand { get; set; }
    [Parameter] public EventCallback<GanttCollapseEventArgs> OnCollapse { get; set; }

    /// <summary>Custom toolbar content rendered inside the toolbar area above the Gantt chart.</summary>
    [Parameter] public RenderFragment? GanttToolBarTemplate { get; set; }

    /// <summary>Child content slot where GanttColumn instances are declared.</summary>
    [Parameter] public RenderFragment? GanttColumns { get; set; }

    /// <summary>Child content slot where GanttView instances (GanttDayView, GanttWeekView, etc.) are declared.</summary>
    [Parameter] public RenderFragment? GanttViews { get; set; }

    /// <summary>The currently active view. Supports two-way binding via @bind-View.</summary>
    [Parameter] public GanttView View { get; set; } = GanttView.Week;

    /// <summary>Callback fired when the active view changes.</summary>
    [Parameter] public EventCallback<GanttView> ViewChanged { get; set; }

    // ── Column management ──────────────────────────────────────────────

    private readonly List<GanttColumn<TItem>> _columns = new();
    private List<GanttColumn<TItem>>? _visibleColumnsCache;

    internal void RegisterColumn(GanttColumn<TItem> column)
    {
        if (!_columns.Contains(column))
        {
            _columns.Add(column);
            _visibleColumnsCache = null;
            _ = InvokeAsync(StateHasChanged);
        }
    }

    internal void UnregisterColumn(GanttColumn<TItem> column)
    {
        _ = InvokeAsync(() =>
        {
            if (_columns.Remove(column))
            {
                _visibleColumnsCache = null;
                StateHasChanged();
            }
        });
    }

    internal List<GanttColumn<TItem>> VisibleColumns
        => _visibleColumnsCache ??= _columns.Where(c => c.Visible).ToList();

    // ── View management (IGanttViewHost) ───────────────────────────────

    private readonly List<GanttViewBase> _views = new();

    void IGanttViewHost.RegisterView(GanttViewBase view)
    {
        if (!_views.Contains(view))
        {
            _views.Add(view);
            ComputeTimeline();
            _ = InvokeAsync(StateHasChanged);
        }
    }

    void IGanttViewHost.UnregisterView(GanttViewBase view)
    {
        _ = InvokeAsync(() =>
        {
            if (_views.Remove(view))
                StateHasChanged();
        });
    }

    /// <summary>The view component matching the current View enum, or the first registered, or null.</summary>
    internal GanttViewBase? ActiveView => _views.FirstOrDefault(v => v.ViewType == View) ?? _views.FirstOrDefault();

    /// <summary>Whether timeline rendering should use the view-driven engine (true) or legacy DayWidth fallback (false).</summary>
    private bool UseViewEngine => _views.Count > 0;

    /// <summary>Whether the toolbar area should render (view selector buttons or custom template present).</summary>
    private bool ShowToolbar => _views.Count > 1 || GanttToolBarTemplate is not null;

    private async Task SwitchView(GanttView view)
    {
        View = view;
        await ViewChanged.InvokeAsync(view);
        ComputeTimeline();
        await InvokeAsync(StateHasChanged);
    }

    // ── Timeline engine ────────────────────────────────────────────────

    internal record TimelineSlot(DateTime Start, DateTime End, string Label);
    internal record TimelineHeader(DateTime Start, DateTime End, string Label, int SpanSlots);

    private DateTime _rangeStart;
    private DateTime _rangeEnd;
    private List<TimelineSlot> _slots = new();
    private List<TimelineHeader> _mainHeaders = new();
    private double _totalTimelineWidth;
    private bool _timelineComputed;

    private void ComputeTimeline()
    {
        var accessor = _accessor!;
        var visible = _flatVisible;

        if (UseViewEngine)
        {
            var view = ActiveView!;
            _rangeStart = view.RangeStart ?? ComputeDataMin(accessor, visible);
            _rangeEnd = view.RangeEnd ?? ComputeDataMax(accessor, visible);

            // Align range to slot boundaries
            _rangeStart = AlignToSlotStart(_rangeStart, View);
            _rangeEnd = AlignToSlotEnd(_rangeEnd, View);

            _slots = GenerateSlots(_rangeStart, _rangeEnd, View);
            _mainHeaders = GenerateMainHeaders(_slots, View);
            _totalTimelineWidth = _slots.Count * view.SlotWidth;
        }
        else
        {
            // Legacy DayWidth fallback
            var minDate = visible.Count > 0 ? visible.Min(n => accessor.GetStart(n.Item)) : DateTime.Today;
            var maxDate = visible.Count > 0 ? visible.Max(n => accessor.GetEnd(n.Item)) : DateTime.Today.AddDays(30);
            _rangeStart = minDate;
            _rangeEnd = maxDate;
            var totalDays = (maxDate - minDate).TotalDays;
            if (totalDays <= 0) totalDays = 1;
            _totalTimelineWidth = totalDays * DayWidth;
            _slots = new List<TimelineSlot>();
            _mainHeaders = new List<TimelineHeader>();
        }

        _timelineComputed = true;
        ComputeDependencyLines();
    }

    private static DateTime ComputeDataMin(GanttFieldAccessor<TItem> accessor, List<GanttNode<TItem>> visible)
        => visible.Count > 0 ? visible.Min(n => accessor.GetStart(n.Item)) : DateTime.Today;

    private static DateTime ComputeDataMax(GanttFieldAccessor<TItem> accessor, List<GanttNode<TItem>> visible)
        => visible.Count > 0 ? visible.Max(n => accessor.GetEnd(n.Item)) : DateTime.Today.AddDays(30);

    private static DateTime AlignToSlotStart(DateTime date, GanttView view) => view switch
    {
        GanttView.Day => date.Date,
        GanttView.Week => StartOfWeek(date),
        GanttView.Month => StartOfWeek(new DateTime(date.Year, date.Month, 1)),
        GanttView.Year => new DateTime(date.Year, date.Month, 1),
        _ => date
    };

    private static DateTime AlignToSlotEnd(DateTime date, GanttView view) => view switch
    {
        GanttView.Day => date.Date.AddDays(1),
        GanttView.Week => StartOfWeek(date).AddDays(7),
        GanttView.Month => StartOfWeek(new DateTime(date.Year, date.Month, 1).AddMonths(1)).AddDays(7),
        GanttView.Year => new DateTime(date.Year, date.Month, 1).AddMonths(1),
        _ => date
    };

    private static DateTime StartOfWeek(DateTime date)
    {
        var diff = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        return date.Date.AddDays(-diff);
    }

    internal List<TimelineSlot> GenerateSlots(DateTime rangeStart, DateTime rangeEnd, GanttView view)
    {
        var slots = new List<TimelineSlot>();
        switch (view)
        {
            case GanttView.Day:
                // Each slot = 1 hour
                var hourCursor = rangeStart;
                while (hourCursor < rangeEnd)
                {
                    var next = hourCursor.AddHours(1);
                    slots.Add(new TimelineSlot(hourCursor, next, hourCursor.ToString("HH:00")));
                    hourCursor = next;
                }
                break;

            case GanttView.Week:
                // Each slot = 1 day
                var dayCursor = rangeStart.Date;
                while (dayCursor < rangeEnd.Date)
                {
                    var next = dayCursor.AddDays(1);
                    slots.Add(new TimelineSlot(dayCursor, next, dayCursor.ToString("ddd")));
                    dayCursor = next;
                }
                break;

            case GanttView.Month:
                // Each slot = 1 week (starting Monday)
                var weekCursor = StartOfWeek(rangeStart);
                while (weekCursor < rangeEnd)
                {
                    var next = weekCursor.AddDays(7);
                    var weekNum = ISOWeek.GetWeekOfYear(weekCursor);
                    slots.Add(new TimelineSlot(weekCursor, next, $"W{weekNum}"));
                    weekCursor = next;
                }
                break;

            case GanttView.Year:
                // Each slot = 1 month
                var monthCursor = new DateTime(rangeStart.Year, rangeStart.Month, 1);
                while (monthCursor < rangeEnd)
                {
                    var next = monthCursor.AddMonths(1);
                    slots.Add(new TimelineSlot(monthCursor, next, monthCursor.ToString("MMM")));
                    monthCursor = next;
                }
                break;
        }
        return slots;
    }

    internal List<TimelineHeader> GenerateMainHeaders(List<TimelineSlot> slots, GanttView view)
    {
        var headers = new List<TimelineHeader>();
        if (slots.Count == 0) return headers;

        switch (view)
        {
            case GanttView.Day:
                // Main headers = days, grouping hourly slots
                GroupSlots(slots, s => s.Start.Date, s => s.Start.ToString("ddd, MMM d"), headers);
                break;

            case GanttView.Week:
                // Main headers = weeks (Mon DD - Sun DD)
                GroupSlots(slots, s => StartOfWeek(s.Start),
                    s =>
                    {
                        var ws = StartOfWeek(s.Start);
                        var we = ws.AddDays(6);
                        return $"{ws:MMM d} - {we:MMM d}";
                    }, headers);
                break;

            case GanttView.Month:
                // Main headers = months, grouping weekly slots
                GroupSlots(slots, s => new DateTime(s.Start.Year, s.Start.Month, 1),
                    s => s.Start.ToString("MMMM yyyy"), headers);
                break;

            case GanttView.Year:
                // Main headers = years, grouping monthly slots
                GroupSlots(slots, s => new DateTime(s.Start.Year, 1, 1),
                    s => s.Start.Year.ToString(), headers);
                break;
        }
        return headers;
    }

    private static void GroupSlots(
        List<TimelineSlot> slots,
        Func<TimelineSlot, DateTime> groupKey,
        Func<TimelineSlot, string> labelFn,
        List<TimelineHeader> headers)
    {
        var i = 0;
        while (i < slots.Count)
        {
            var key = groupKey(slots[i]);
            var label = labelFn(slots[i]);
            var start = slots[i].Start;
            var spanCount = 0;
            while (i < slots.Count && groupKey(slots[i]) == key)
            {
                spanCount++;
                i++;
            }
            var end = i < slots.Count ? slots[i].Start : slots[i - 1].End;
            headers.Add(new TimelineHeader(start, end, label, spanCount));
        }
    }

    /// <summary>
    /// Gets the pixel offset from the range start for a given date, based on the active view.
    /// For WeekView (slot = 1 day), a date 7 days after RangeStart returns 7 * SlotWidth.
    /// </summary>
    internal double GetPixelOffset(DateTime date)
    {
        if (!UseViewEngine)
        {
            // Legacy: pixel offset based on DayWidth
            return (date - _rangeStart).TotalDays * DayWidth;
        }

        var view = ActiveView!;
        var slotWidth = view.SlotWidth;

        var raw = View switch
        {
            GanttView.Day => (date - _rangeStart).TotalHours * slotWidth,
            GanttView.Week => (date - _rangeStart).TotalDays * slotWidth,
            GanttView.Month => (date - _rangeStart).TotalDays / 7.0 * slotWidth,
            GanttView.Year => GetMonthFractionalOffset(date, _rangeStart) * slotWidth,
            _ => 0
        };

        return Math.Clamp(raw, 0, _totalTimelineWidth);
    }

    /// <summary>
    /// Computes fractional month offset between two dates for YearView pixel mapping.
    /// </summary>
    private static double GetMonthFractionalOffset(DateTime date, DateTime rangeStart)
    {
        int wholeMonths = (date.Year - rangeStart.Year) * 12 + (date.Month - rangeStart.Month);
        double dayFraction = (date.Day - 1.0) / DateTime.DaysInMonth(date.Year, date.Month);
        return wholeMonths + dayFraction;
    }

    /// <summary>
    /// Gets the bar width in pixels for a task spanning from start to end.
    /// Clamped to minimum 4px.
    /// </summary>
    internal double GetBarWidth(DateTime start, DateTime end)
        => Math.Max(GetPixelOffset(end) - GetPixelOffset(start), 4);

    // ── Lifecycle ──────────────────────────────────────────────────────

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (UseViewEngine && !_timelineComputed)
            {
                ComputeTimeline();
                StateHasChanged();
            }

            _dotNetRef = DotNetObjectReference.Create(this);
            _jsModule = await JS.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Marilo.Components/js/marilo-gantt.js");
            _jsInstance = await _jsModule.InvokeAsync<IJSObjectReference>(
                "initGantt", _dotNetRef, _containerRef,
                new { rowHeight = RowHeight, slotWidth = ActiveView?.SlotWidth ?? DayWidth });
        }
    }

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
        _visibleColumnsCache = null;
        _timelineComputed = false;
        ComputeTimeline();
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
        ComputeTimeline();
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

        var insertionIndex = 0;
        foreach (var item in items)
        {
            var node = new GanttNode<TItem>
            {
                Item = item,
                Id = accessor.GetId(item),
                ParentId = accessor.GetParentId(item),
                OriginalIndex = insertionIndex++,
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

    /// <summary>
    /// Toggles sort on the given field. Cycle: ascending -> descending -> unsorted.
    /// Called from clickable column headers.
    /// </summary>
    internal void SortBy(string field)
    {
        if (string.IsNullOrEmpty(field)) return;

        if (_sortField == field)
        {
            _sortCycleStep++;
            if (_sortCycleStep == 1)
            {
                // Second click: descending
                _sortAscending = false;
            }
            else
            {
                // Third click: clear sort
                _sortField = null;
                _sortAscending = true;
                _sortCycleStep = 0;
            }
        }
        else
        {
            // New field: ascending
            _sortField = field;
            _sortAscending = true;
            _sortCycleStep = 0;
        }

        RebuildFlatVisible();
        _ = InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Sorts sibling groups recursively by the current sort field, preserving hierarchy.
    /// Children stay under their parent; only the order among siblings changes.
    /// </summary>
    private void ApplyHierarchicalSort()
    {
        if (_accessor is null) return;

        if (_sortField is null)
        {
            RestoreOriginalOrder(_roots);
            return;
        }

        SortSiblings(_roots, _sortField, _sortAscending, _accessor);
    }

    private static void RestoreOriginalOrder(List<GanttNode<TItem>> siblings)
    {
        siblings.Sort((a, b) => a.OriginalIndex.CompareTo(b.OriginalIndex));
        foreach (var node in siblings)
            if (node.Children.Count > 0)
                RestoreOriginalOrder(node.Children);
    }

    private static void SortSiblings(List<GanttNode<TItem>> siblings, string field, bool ascending, GanttFieldAccessor<TItem> accessor)
    {
        if (siblings.Count <= 1) return;

        var sorted = ascending
            ? siblings.OrderBy(n => accessor.GetFieldValue(n.Item, field), NullSafeObjectComparer.Instance).ToList()
            : siblings.OrderByDescending(n => accessor.GetFieldValue(n.Item, field), NullSafeObjectComparer.Instance).ToList();
        siblings.Clear();
        siblings.AddRange(sorted);

        foreach (var node in siblings)
        {
            if (node.Children.Count > 0)
                SortSiblings(node.Children, field, ascending, accessor);
        }
    }

    private sealed class NullSafeObjectComparer : IComparer<object?>
    {
        public static readonly NullSafeObjectComparer Instance = new();
        public int Compare(object? x, object? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;
            if (x is IComparable cx)
            {
                try { return cx.CompareTo(y); }
                catch { return string.Compare(x.ToString(), y.ToString(), StringComparison.Ordinal); }
            }
            return string.Compare(x.ToString(), y.ToString(), StringComparison.Ordinal);
        }
    }

    // ── Filter logic ──────────────────────────────────────────────────

    /// <summary>Called from @oninput on filter row inputs.</summary>
    internal void OnFilterInput(string field, string value)
    {
        if (string.IsNullOrEmpty(value))
            _filterValues.Remove(field);
        else
            _filterValues[field] = value;

        RebuildFlatVisible();
        _ = InvokeAsync(StateHasChanged);
    }

    /// <summary>Whether any filter is currently active.</summary>
    private bool HasActiveFilters => _filterValues.Count > 0;

    /// <summary>Returns true if a node's own field values match ALL active filters (AND logic).</summary>
    private bool NodeMatchesAllFilters(GanttNode<TItem> node)
    {
        var accessor = _accessor!;
        foreach (var kvp in _filterValues)
        {
            var fieldValue = accessor.GetFieldValue(node.Item, kvp.Key)?.ToString();
            if (fieldValue is null || !fieldValue.Contains(kvp.Value, StringComparison.OrdinalIgnoreCase))
                return false;
        }
        return true;
    }

    /// <summary>Returns true if the node or any of its descendants match all filters.</summary>
    private bool NodeOrDescendantMatches(GanttNode<TItem> node)
    {
        if (NodeMatchesAllFilters(node)) return true;
        foreach (var child in node.Children)
        {
            if (NodeOrDescendantMatches(child)) return true;
        }
        return false;
    }

    /// <summary>
    /// Marks nodes as filter-visible using a bottom-up check.
    /// A node is visible if it matches all filters OR any descendant does.
    /// Auto-expands parents of matches so results are visible.
    /// Returns the set of visible node references.
    /// </summary>
    private HashSet<GanttNode<TItem>>? ComputeFilterVisibility()
    {
        if (!HasActiveFilters)
        {
            // Restore any filter-expanded ids back to collapsed
            foreach (var id in _filterExpandedIds)
                _expandedIds.Remove(id);
            _filterExpandedIds.Clear();
            return null; // null means "show all"
        }

        // First, undo previous auto-expansions so MarkVisible sees a clean slate
        foreach (var id in _filterExpandedIds)
            _expandedIds.Remove(id);
        _filterExpandedIds.Clear();

        // Now compute visibility fresh
        var visible = new HashSet<GanttNode<TItem>>();
        foreach (var root in _roots)
            MarkVisible(root, visible);
        return visible;
    }

    /// <summary>Recursively marks a node visible if it or any descendant matches. Returns true if visible.</summary>
    private bool MarkVisible(GanttNode<TItem> node, HashSet<GanttNode<TItem>> visible)
    {
        bool selfMatches = NodeMatchesAllFilters(node);
        bool anyChildVisible = false;

        foreach (var child in node.Children)
        {
            if (MarkVisible(child, visible))
                anyChildVisible = true;
        }

        if (selfMatches || anyChildVisible)
        {
            visible.Add(node);

            // Auto-expand parents with matching descendants so matches are visible
            if (anyChildVisible && node.Id is not null && !_expandedIds.Contains(node.Id))
            {
                _expandedIds.Add(node.Id);
                _filterExpandedIds.Add(node.Id);
            }

            return true;
        }

        return false;
    }

    private void RebuildFlatVisible()
    {
        // Compute filter visibility (non-destructive), then sort, then flatten
        var filterVisible = ComputeFilterVisibility();
        ApplyHierarchicalSort();

        var list = new List<GanttNode<TItem>>();
        var visited = new HashSet<GanttNode<TItem>>();
        var stack = new Stack<GanttNode<TItem>>();
        for (int i = _roots.Count - 1; i >= 0; i--)
        {
            if (filterVisible is null || filterVisible.Contains(_roots[i]))
                stack.Push(_roots[i]);
        }
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (!visited.Add(n)) continue;
            list.Add(n);
            if (IsExpanded(n))
            {
                for (int i = n.Children.Count - 1; i >= 0; i--)
                {
                    if (filterVisible is null || filterVisible.Contains(n.Children[i]))
                        stack.Push(n.Children[i]);
                }
            }
        }
        _flatVisible = list;

        // Clamp focused index after visibility changes
        if (_focusedIndex >= _flatVisible.Count)
            _focusedIndex = Math.Max(0, _flatVisible.Count - 1);
    }

    private void ComputeDependencyLines()
    {
        _dependencyLines.Clear();
        var accessor = _accessor;
        if (accessor is null) return;

        for (int i = 0; i < _flatVisible.Count; i++)
        {
            var node = _flatVisible[i];
            var deps = accessor.GetDependsOn(node.Item);
            if (deps is null) continue;

            foreach (var depId in deps)
            {
                // Find the dependency source node in _flatVisible
                var srcIdx = _flatVisible.FindIndex(n => Equals(n.Id, depId));
                if (srcIdx < 0) continue;
                var srcNode = _flatVisible[srcIdx];

                // Line from end of source bar to start of dependent bar
                var srcEnd = accessor.GetEnd(srcNode.Item);
                var nodeStart = accessor.GetStart(node.Item);
                var x1 = GetPixelOffset(srcEnd);
                var y1 = srcIdx * RowHeight + RowHeight / 2.0;
                var x2 = GetPixelOffset(nodeStart);
                var y2 = i * RowHeight + RowHeight / 2.0;

                _dependencyLines.Add(new DependencyLine(x1, y1, x2, y2));
            }
        }
    }

    private async Task ToggleExpanded(GanttNode<TItem> node)
    {
        if (node.Id is null) return;

        bool wasExpanded = _expandedIds.Contains(node.Id);
        if (!_expandedIds.Add(node.Id))
        {
            _expandedIds.Remove(node.Id);
        }
        RebuildFlatVisible();

        bool shouldRender = true;
        if (wasExpanded)
        {
            var args = new GanttCollapseEventArgs { Item = node.Item };
            await OnCollapse.InvokeAsync(args);
            shouldRender = args.ShouldRender;
        }
        else
        {
            var args = new GanttExpandEventArgs { Item = node.Item };
            await OnExpand.InvokeAsync(args);
            shouldRender = args.ShouldRender;
        }

        if (shouldRender)
            await InvokeAsync(StateHasChanged);
    }

    // ── Keyboard navigation handler ─────────────────────────────────
    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (_flatVisible.Count == 0) return;

        switch (e.Key)
        {
            case "ArrowDown":
                _focusedIndex = Math.Min(_focusedIndex + 1, _flatVisible.Count - 1);
                break;
            case "ArrowUp":
                _focusedIndex = Math.Max(_focusedIndex - 1, 0);
                break;
            case "ArrowRight":
                var rightNode = _flatVisible[_focusedIndex];
                if (rightNode.Children.Count > 0 && !IsExpanded(rightNode))
                    await ToggleExpanded(rightNode);
                else if (rightNode.Children.Count > 0 && IsExpanded(rightNode))
                    _focusedIndex = Math.Min(_focusedIndex + 1, _flatVisible.Count - 1);
                break;
            case "ArrowLeft":
                var leftNode = _flatVisible[_focusedIndex];
                if (leftNode.Children.Count > 0 && IsExpanded(leftNode))
                    await ToggleExpanded(leftNode);
                else if (leftNode.Parent is not null)
                    _focusedIndex = _flatVisible.IndexOf(leftNode.Parent);
                break;
            case "Home":
                _focusedIndex = 0;
                break;
            case "End":
                _focusedIndex = _flatVisible.Count - 1;
                break;
            case "Enter":
            case " ":
                if (_focusedIndex >= 0 && _focusedIndex < _flatVisible.Count)
                    await OnTaskClick.InvokeAsync(_flatVisible[_focusedIndex].Item);
                break;
            default:
                return; // Don't re-render for unhandled keys
        }
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>Returns the number of siblings (including this node) at the same level under the same parent.</summary>
    private int GetSiblingCount(GanttNode<TItem> node)
        => node.Parent is not null ? node.Parent.Children.Count : _roots.Count;

    /// <summary>Returns the 1-based position of this node among its siblings.</summary>
    private int GetPositionInSiblings(GanttNode<TItem> node)
    {
        var siblings = node.Parent is not null ? node.Parent.Children : _roots;
        return siblings.IndexOf(node) + 1;
    }

    // ── JS interop callbacks (D1) ─────────────────────────────────────

    /// <summary>Converts a pixel delta to a TimeSpan based on the active view's scale.</summary>
    private TimeSpan PixelsToTimeSpan(double deltaPixels)
    {
        if (UseViewEngine)
        {
            var view = ActiveView!;
            var slotWidth = view.SlotWidth;
            if (slotWidth <= 0) return TimeSpan.Zero;

            return View switch
            {
                GanttView.Day => TimeSpan.FromHours(deltaPixels / slotWidth),
                GanttView.Week => TimeSpan.FromDays(deltaPixels / slotWidth),
                GanttView.Month => TimeSpan.FromDays(deltaPixels / slotWidth * 7),
                GanttView.Year => TimeSpan.FromDays(deltaPixels / slotWidth * 30.44),
                _ => TimeSpan.Zero
            };
        }

        // Legacy DayWidth fallback
        return DayWidth > 0 ? TimeSpan.FromDays(deltaPixels / DayWidth) : TimeSpan.Zero;
    }

    [JSInvokable]
    public async Task OnBarMoved(int barIndex, double deltaPixels)
    {
        if (barIndex < 0 || barIndex >= _flatVisible.Count) return;
        var node = _flatVisible[barIndex];
        var accessor = _accessor!;
        var delta = PixelsToTimeSpan(deltaPixels);

        var oldStart = accessor.GetStart(node.Item);
        var oldEnd = accessor.GetEnd(node.Item);
        accessor.SetStart(node.Item, oldStart + delta);
        accessor.SetEnd(node.Item, oldEnd + delta);

        ComputeTimeline();
        await OnUpdate.InvokeAsync(new GanttUpdateEventArgs { Item = node.Item });
        await InvokeAsync(StateHasChanged);
    }

    [JSInvokable]
    public async Task OnBarResized(int barIndex, double leftDelta, double rightDelta)
    {
        if (barIndex < 0 || barIndex >= _flatVisible.Count) return;
        var node = _flatVisible[barIndex];
        var accessor = _accessor!;

        if (leftDelta != 0)
        {
            var oldStart = accessor.GetStart(node.Item);
            accessor.SetStart(node.Item, oldStart + PixelsToTimeSpan(leftDelta));
        }

        if (rightDelta != 0)
        {
            var oldEnd = accessor.GetEnd(node.Item);
            accessor.SetEnd(node.Item, oldEnd + PixelsToTimeSpan(rightDelta));
        }

        ComputeTimeline();
        await OnUpdate.InvokeAsync(new GanttUpdateEventArgs { Item = node.Item });
        await InvokeAsync(StateHasChanged);
    }

    [JSInvokable]
    public async Task OnBarProgressChanged(int barIndex, double newPercent)
    {
        if (barIndex < 0 || barIndex >= _flatVisible.Count) return;
        var node = _flatVisible[barIndex];
        var clamped = Math.Clamp(newPercent, 0, 100);
        _accessor!.SetPercentComplete(node.Item, Math.Round(clamped, 1));

        await OnUpdate.InvokeAsync(new GanttUpdateEventArgs { Item = node.Item });
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        if (_jsInstance is not null)
        {
            try
            {
                if (_jsModule is not null)
                    await _jsModule.InvokeVoidAsync("dispose", _jsInstance);
            }
            catch (Exception ex) when (ex is JSDisconnectedException or TaskCanceledException or ObjectDisposedException) { }
        }
        if (_jsModule is not null)
        {
            try { await _jsModule.DisposeAsync(); }
            catch (Exception ex) when (ex is JSDisconnectedException or TaskCanceledException or ObjectDisposedException) { }
        }
        _dotNetRef?.Dispose();
    }
}
