using Marilo.Core.Base;
using Marilo.Core.BusinessLogic.Enums;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Marilo.Components.DataDisplay;

public partial class MariloAllocationScheduler<TResource> : MariloComponentBase
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    // ── Data Binding ────────────────────────────────────────────────────

    [Parameter] public IEnumerable<TResource>? Resources { get; set; }
    [Parameter] public IEnumerable<AllocationRecord>? Allocations { get; set; }
    [Parameter] public IEnumerable<AllocationTarget>? Targets { get; set; }

    // ── Timeline Configuration ──────────────────────────────────────────

    [Parameter] public TimeGranularity AuthoritativeLevel { get; set; }
    [Parameter] public TimeGranularity ViewGrain { get; set; }
    [Parameter] public EventCallback<TimeGranularity> ViewGrainChanged { get; set; }
    [Parameter] public DateTime VisibleStart { get; set; } = DateTime.Today;
    [Parameter] public EventCallback<DateTime> VisibleStartChanged { get; set; }
    [Parameter] public DateTime? VisibleEnd { get; set; }
    [Parameter] public int DefaultRangeLength { get; set; } = 3;
    [Parameter] public TimeGranularity DefaultRangeUnit { get; set; } = TimeGranularity.Month;

    // ── Display ─────────────────────────────────────────────────────────

    [Parameter] public AllocationValueMode ValueMode { get; set; } = AllocationValueMode.Hours;
    [Parameter] public bool ShowTargets { get; set; }
    [Parameter] public bool ShowDeltas { get; set; }
    [Parameter] public DeltaDisplayMode DeltaDisplayMode { get; set; } = DeltaDisplayMode.Value;
    [Parameter] public string? Height { get; set; }
    [Parameter] public string? Width { get; set; }
    [Parameter] public bool EnableLoaderContainer { get; set; } = true;

    // ── Interaction ─────────────────────────────────────────────────────

    [Parameter] public bool AllowDragFill { get; set; } = true;
    [Parameter] public bool AllowKeyboardEdit { get; set; } = true;
    [Parameter] public bool AllowBulkEdit { get; set; } = true;
    [Parameter] public bool EnableContextMenu { get; set; } = true;
    [Parameter] public IEnumerable<AllocationMenuDescriptor>? ContextMenuItems { get; set; }
    [Parameter] public AllocationSelectionMode SelectionMode { get; set; } = AllocationSelectionMode.Range;
    [Parameter] public DistributionMode DefaultDistributionMode { get; set; } = DistributionMode.EvenSpread;
    [Parameter] public bool AllowZoomEdit { get; set; }

    // ── Scenario Planning ───────────────────────────────────────────────

    [Parameter] public IEnumerable<AllocationSet>? AllocationSets { get; set; }
    [Parameter] public IEnumerable<ScenarioOverride>? ScenarioOverrides { get; set; }
    [Parameter] public Guid ActiveSetId { get; set; }
    [Parameter] public EventCallback<Guid> ActiveSetIdChanged { get; set; }
    [Parameter] public Guid? CompareSetId { get; set; }
    [Parameter] public bool ShowBaselineDiff { get; set; }
    [Parameter] public string? BaselineDateFormat { get; set; }
    [Parameter] public bool ShowComparisonPanel { get; set; }
    [Parameter] public bool ShowCriticalPath { get; set; }

    // ── Templates ───────────────────────────────────────────────────────

    [Parameter] public RenderFragment? AllocationResourceColumns { get; set; }
    [Parameter] public RenderFragment? ToolbarTemplate { get; set; }
    [Parameter] public RenderFragment? EmptyTemplate { get; set; }
    [Parameter] public RenderFragment<AllocationCellContext>? CellTemplate { get; set; }
    [Parameter] public RenderFragment<TResource>? ResourceRowTemplate { get; set; }

    // ── Events ──────────────────────────────────────────────────────────

    [Parameter] public EventCallback<CellEditedArgs> OnCellEdited { get; set; }
    [Parameter] public EventCallback<RangeEditedArgs> OnRangeEdited { get; set; }
    [Parameter] public EventCallback<ContextMenuActionArgs> OnContextMenuAction { get; set; }
    [Parameter] public EventCallback<DistributeArgs> OnDistributeRequested { get; set; }
    [Parameter] public EventCallback<ShiftValuesArgs> OnShiftValues { get; set; }
    [Parameter] public EventCallback<MoveValuesArgs> OnMoveValues { get; set; }
    [Parameter] public EventCallback<TargetChangedArgs> OnTargetChanged { get; set; }
    [Parameter] public EventCallback<VisibleRangeChangedArgs> OnVisibleRangeChanged { get; set; }
    [Parameter] public EventCallback<SelectionChangedArgs> OnSelectionChanged { get; set; }
    [Parameter] public EventCallback<ScenarioChangedArgs> OnScenarioChanged { get; set; }
    [Parameter] public EventCallback<ScenarioCreatedArgs> OnScenarioCreated { get; set; }
    [Parameter] public EventCallback<AllocationOverriddenArgs> OnAllocationOverridden { get; set; }
    [Parameter] public EventCallback<ScenarioStatusChangedArgs> OnScenarioStatusChanged { get; set; }
    [Parameter] public EventCallback<ScenarioPromotedArgs> OnScenarioPromoted { get; set; }
    [Parameter] public EventCallback<CanExecuteActionArgs> CanExecuteAction { get; set; }

    // ── Internal State ──────────────────────────────────────────────────

    private readonly List<AllocationResourceColumn<TResource>> _columns = new();
    private IEnumerable<AllocationSet>? _allocationSets;
    private IEnumerable<AllocationRecord>? _effectiveAllocations;
    private List<DateRange> _visibleBuckets = new();
    private HashSet<(object ResourceKey, DateTime BucketStart)> _selectedCells = new();
    private TimeGranularity _currentViewGrain;
    private bool _isLoading;

    // ── Lifecycle ────────────────────────────────────────────────────────

    protected override void OnInitialized()
    {
        _currentViewGrain = ViewGrain == default ? AuthoritativeLevel : ViewGrain;
        _allocationSets = AllocationSets;
    }

    protected override void OnParametersSet()
    {
        _currentViewGrain = ViewGrain == default ? AuthoritativeLevel : ViewGrain;
        _allocationSets = AllocationSets;
        _effectiveAllocations = ComputeEffectiveAllocations();
        _visibleBuckets = ComputeVisibleBuckets();
    }

    // ── Public Methods (via @ref) ───────────────────────────────────────

    public async Task Rebind()
    {
        _effectiveAllocations = ComputeEffectiveAllocations();
        _visibleBuckets = ComputeVisibleBuckets();
        await InvokeAsync(StateHasChanged);
    }

    public async Task Refresh()
    {
        await InvokeAsync(StateHasChanged);
    }

    public async Task NavigateTo(DateTime date)
    {
        VisibleStart = date;
        _visibleBuckets = ComputeVisibleBuckets();
        await VisibleStartChanged.InvokeAsync(VisibleStart);
        await OnVisibleRangeChanged.InvokeAsync(new VisibleRangeChangedArgs
        {
            NewStart = VisibleStart,
            NewEnd = GetVisibleEnd(),
            ViewGrain = _currentViewGrain
        });
        await InvokeAsync(StateHasChanged);
    }

    public Task NavigateForward() => NavigateTo(AdvanceDate(VisibleStart, _currentViewGrain, 1));
    public Task NavigateBack() => NavigateTo(AdvanceDate(VisibleStart, _currentViewGrain, -1));
    public Task NavigateToToday() => NavigateTo(DateTime.Today);

    public IReadOnlyList<AllocationCellRef> GetSelectedCells() =>
        _selectedCells.Select(c => new AllocationCellRef
        {
            ResourceKey = c.ResourceKey,
            BucketStart = c.BucketStart
        }).ToList();

    public async Task ClearSelection()
    {
        _selectedCells.Clear();
        await OnSelectionChanged.InvokeAsync(new SelectionChangedArgs
        {
            SelectedCells = Array.Empty<AllocationCellRef>(),
            SelectionMode = SelectionMode
        });
        await InvokeAsync(StateHasChanged);
    }

    // ── Column Registration ─────────────────────────────────────────────

    internal void AddColumn(AllocationResourceColumn<TResource> column)
    {
        if (!_columns.Contains(column))
            _columns.Add(column);
    }

    internal void RemoveColumn(AllocationResourceColumn<TResource> column)
    {
        _columns.Remove(column);
    }

    // ── Event Handlers ──────────────────────────────────────────────────

    private async Task HandleNavigateBack() => await NavigateBack();
    private async Task HandleNavigateForward() => await NavigateForward();
    private async Task HandleNavigateToToday() => await NavigateToToday();

    private async Task HandleScenarioSwitch(Guid setId)
    {
        if (setId == ActiveSetId) return;
        var previous = ActiveSetId;
        ActiveSetId = setId;
        _effectiveAllocations = ComputeEffectiveAllocations();
        await ActiveSetIdChanged.InvokeAsync(setId);
        await OnScenarioChanged.InvokeAsync(new ScenarioChangedArgs
        {
            PreviousSetId = previous,
            NewSetId = setId
        });
        await InvokeAsync(StateHasChanged);
    }

    private async Task HandleCellClick(object resourceKey, DateRange bucket)
    {
        if (SelectionMode == AllocationSelectionMode.None) return;

        var key = (resourceKey, bucket.Start);
        if (SelectionMode == AllocationSelectionMode.Cell)
        {
            _selectedCells.Clear();
            _selectedCells.Add(key);
        }
        else
        {
            if (_selectedCells.Contains(key))
                _selectedCells.Remove(key);
            else
                _selectedCells.Add(key);
        }

        await OnSelectionChanged.InvokeAsync(new SelectionChangedArgs
        {
            SelectedCells = GetSelectedCells(),
            SelectionMode = SelectionMode
        });
    }

    private Task HandleCellDoubleClick(object resourceKey, DateRange bucket)
    {
        // Double-click to enter edit mode -- handled via JS interop for inline editing
        return Task.CompletedTask;
    }

    // ── Computed Properties ─────────────────────────────────────────────

    private string? SizeStyle
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(Height)) parts.Add($"height:{Height}");
            if (!string.IsNullOrEmpty(Width)) parts.Add($"width:{Width}");
            return parts.Count > 0 ? string.Join(";", parts) : null;
        }
    }

    // ── Helper Methods ──────────────────────────────────────────────────

    private IEnumerable<AllocationRecord> ComputeEffectiveAllocations()
    {
        var baseAllocations = Allocations ?? Enumerable.Empty<AllocationRecord>();

        if (AllocationSets is null || ScenarioOverrides is null)
            return baseAllocations;

        var activeSet = AllocationSets.FirstOrDefault(s => s.SetId == ActiveSetId);
        if (activeSet is null || activeSet.Type == AllocationSetType.Baseline)
            return baseAllocations;

        var overrides = ScenarioOverrides.Where(o => o.SetId == ActiveSetId).ToList();
        var overriddenIds = overrides
            .Where(o => o.OriginalAllocationId.HasValue)
            .Select(o => o.OriginalAllocationId!.Value)
            .ToHashSet();
        var deletedIds = overrides
            .Where(o => o.IsDeleted && o.OriginalAllocationId.HasValue)
            .Select(o => o.OriginalAllocationId!.Value)
            .ToHashSet();

        var result = new List<AllocationRecord>();

        foreach (var alloc in baseAllocations)
        {
            if (deletedIds.Contains(alloc.AllocationId)) continue;
            if (overriddenIds.Contains(alloc.AllocationId))
            {
                var ovr = overrides.First(o => o.OriginalAllocationId == alloc.AllocationId && !o.IsDeleted);
                result.Add(ovr.Override);
            }
            else
            {
                result.Add(alloc);
            }
        }

        // Add new allocations (OriginalAllocationId is null, not deleted)
        result.AddRange(overrides
            .Where(o => o.OriginalAllocationId is null && !o.IsDeleted)
            .Select(o => o.Override));

        return result;
    }

    private List<DateRange> ComputeVisibleBuckets()
    {
        var buckets = new List<DateRange>();
        var end = GetVisibleEnd();
        var current = VisibleStart;

        while (current < end)
        {
            var next = AdvanceDate(current, _currentViewGrain, 1);
            buckets.Add(new DateRange { Start = current, End = next });
            current = next;
        }

        return buckets;
    }

    private DateTime GetVisibleEnd()
    {
        if (VisibleEnd.HasValue) return VisibleEnd.Value;
        return AdvanceDate(VisibleStart, DefaultRangeUnit, DefaultRangeLength);
    }

    private static DateTime AdvanceDate(DateTime date, TimeGranularity grain, int count)
    {
        return grain switch
        {
            TimeGranularity.Day => date.AddDays(count),
            TimeGranularity.Week => date.AddDays(7 * count),
            TimeGranularity.Month => date.AddMonths(count),
            TimeGranularity.Quarter => date.AddMonths(3 * count),
            TimeGranularity.Year => date.AddYears(count),
            _ => date.AddDays(count)
        };
    }

    private object GetResourceKey(TResource resource)
    {
        // Use reflection to find an Id or Key property
        var type = typeof(TResource);
        var idProp = type.GetProperty("Id") ?? type.GetProperty("Key");
        return idProp?.GetValue(resource) ?? resource!;
    }

    private string GetResourceLabel(TResource resource)
    {
        var type = typeof(TResource);
        var nameProp = type.GetProperty("Name") ?? type.GetProperty("Title");
        return nameProp?.GetValue(resource)?.ToString() ?? resource?.ToString() ?? string.Empty;
    }

    private object? GetFieldValue(TResource resource, string field)
    {
        var prop = typeof(TResource).GetProperty(field);
        return prop?.GetValue(resource);
    }

    private AllocationRecord? GetRecord(object resourceKey, DateRange bucket)
    {
        return _effectiveAllocations?.FirstOrDefault(a =>
            Equals(a.ResourceId, resourceKey) &&
            a.BucketStart >= bucket.Start &&
            a.BucketStart < bucket.End);
    }

    private Func<object, DateRange, AllocationRecord?> _baselineRecord =>
        (resourceKey, bucket) =>
        {
            return Allocations?.FirstOrDefault(a =>
                Equals(a.ResourceId, resourceKey) &&
                a.BucketStart >= bucket.Start &&
                a.BucketStart < bucket.End);
        };

    private bool IsCellEditable(DateRange bucket)
    {
        if (_currentViewGrain != AuthoritativeLevel && !AllowZoomEdit)
            return false;
        return true;
    }

    private bool IsCellSelected(object resourceKey, DateRange bucket) =>
        _selectedCells.Contains((resourceKey, bucket.Start));

    private bool IsCellConflict(object resourceKey, DateRange bucket)
    {
        // Check for overlapping allocations on same resource in same bucket
        if (_effectiveAllocations is null) return false;
        var count = _effectiveAllocations.Count(a =>
            Equals(a.ResourceId, resourceKey) &&
            a.BucketStart < bucket.End &&
            a.BucketEnd > bucket.Start);
        return count > 1;
    }

    private bool IsCellDisabled(DateRange bucket) => false;

    private bool IsResourceOverAllocated(object resourceKey)
    {
        if (Targets is null || _effectiveAllocations is null) return false;
        var totalActual = _effectiveAllocations.Where(a => Equals(a.ResourceId, resourceKey)).Sum(a => a.Value);
        var totalTarget = Targets.Where(t => Equals(t.ResourceId, resourceKey)).Sum(t => t.TargetValue);
        return totalTarget > 0 && totalActual > totalTarget;
    }

    private bool IsResourceRowSelected(object resourceKey) =>
        _selectedCells.Any(c => Equals(c.ResourceKey, resourceKey));

    private decimal? GetDelta(object resourceKey, DateRange bucket)
    {
        if (Targets is null || _effectiveAllocations is null) return null;
        var actual = _effectiveAllocations
            .Where(a => Equals(a.ResourceId, resourceKey) && a.BucketStart >= bucket.Start && a.BucketStart < bucket.End)
            .Sum(a => a.Value);
        var target = Targets
            .Where(t => Equals(t.ResourceId, resourceKey) && t.PeriodStart <= bucket.Start && t.PeriodEnd >= bucket.End)
            .Sum(t => t.TargetValue);
        if (target == 0) return null;
        return actual - target;
    }

    private string FormatBucketHeader(DateRange bucket)
    {
        return _currentViewGrain switch
        {
            TimeGranularity.Day => bucket.Start.ToString("MMM d"),
            TimeGranularity.Week => $"W{GetIsoWeekNumber(bucket.Start)} {bucket.Start:MMM d}",
            TimeGranularity.Month => bucket.Start.ToString("MMM yyyy"),
            TimeGranularity.Quarter => $"Q{(bucket.Start.Month - 1) / 3 + 1} {bucket.Start:yyyy}",
            TimeGranularity.Year => bucket.Start.ToString("yyyy"),
            _ => bucket.Start.ToShortDateString()
        };
    }

    private string FormatValue(decimal value)
    {
        return ValueMode switch
        {
            AllocationValueMode.Hours => $"{value:N1}h",
            AllocationValueMode.Currency => $"${value:N0}",
            _ => value.ToString("N1")
        };
    }

    private string FormatDelta(decimal delta)
    {
        var sign = delta > 0 ? "+" : "";
        return DeltaDisplayMode switch
        {
            DeltaDisplayMode.Value when ValueMode == AllocationValueMode.Hours => $"{sign}{delta:N1}h",
            DeltaDisplayMode.Value => $"{sign}${delta:N0}",
            DeltaDisplayMode.Percentage => $"{sign}{delta:P0}",
            _ => delta > 0 ? "Over" : "Under"
        };
    }

    private string GetCellAriaLabel(TResource resource, DateRange bucket, AllocationRecord? record)
    {
        var resourceName = GetResourceLabel(resource);
        var bucketLabel = FormatBucketHeader(bucket);
        if (record is null) return $"{resourceName}, {bucketLabel}, empty";
        return $"{resourceName}, {bucketLabel}, {FormatValue(record.Value)}";
    }

    private string GetScenarioLabel(AllocationSet set)
    {
        if (!string.IsNullOrEmpty(set.DisplayLabel)) return set.DisplayLabel;
        if (set.Type == AllocationSetType.Baseline && set.IsLocked && set.FinalizedDate.HasValue)
        {
            var fmt = BaselineDateFormat ?? "MMM d, yyyy";
            return $"Baseline As of {set.FinalizedDate.Value.ToString(fmt)}";
        }
        return set.Name;
    }

    private static int GetIsoWeekNumber(DateTime date)
    {
        var day = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            date = date.AddDays(3);
        return System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
            date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
}
