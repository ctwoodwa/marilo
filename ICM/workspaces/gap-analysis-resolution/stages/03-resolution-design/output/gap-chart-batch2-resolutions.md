# Resolution Records: MariloChart Batch 2 — Events & Polish

> Date: 2026-04-04
> Source: `stages/02-prioritize/output/gap-chart-priorities.md`
> Component: `MariloChart` — `src/Marilo.Components/Charts/`

---

## RES-CHART-B2-01: Chart type coverage — Bubble rendering

**Resolves:** GAP-CHART-009
**Status:** Ready for implementation

### Analysis

`ChartSeriesType` enum includes 9 types: Line, Bar, Column, Area, Pie, Donut, Scatter, ScatterLine, Bubble. The rendering switch in `MariloChart.razor` handles all except **Bubble** — it falls through silently. `MariloChartSeries` already has `SizeField` and `BubbleSize` on `ChartDataPoint`, so the data extraction pipeline is ready.

### Target Pattern

Add `case ChartSeriesType.Bubble:` to the cartesian rendering switch. Reuse `RenderScatterSeries` concept but render circles with radius derived from `BubbleSize`.

```csharp
case ChartSeriesType.Bubble:
    RenderBubbleSeries(builder, points, color, plotWidth, plotHeight,
        marginLeft, marginTop, minY, maxY, categoryCount, seriesGlobalIndex);
    break;
```

`RenderBubbleSeries` follows the same pattern as `RenderScatterSeries` but uses `point.BubbleSize` to scale the circle radius.

### Success Criteria
- [ ] `ChartSeriesType.Bubble` has a rendering path (no silent fallthrough)
- [ ] Bubble size is derived from `BubbleSize` data point property
- [ ] Circles are rendered with scaled radius
- [ ] Tooltip and click events work on bubbles

---

## RES-CHART-B2-02: Data binding parameter name alignment

**Resolves:** GAP-CHART-011
**Status:** Already resolved (close immediately)

### Analysis

Code review of `MariloChartSeries.razor` shows parameters match spec:
- `Field` — Y value property name ✓
- `CategoryField` — X category property name ✓
- `Data` — data source ✓
- `Name` — series display name ✓
- `Type` — chart series type ✓
- `XField`, `YField`, `SizeField` — scatter/bubble support ✓

All parameter names match the spec conventions. No change needed.

### Decision: Close as already resolved

---

## RES-CHART-B2-03: Transitions bool → bool?

**Resolves:** GAP-CHART-012
**Status:** Ready for implementation

### Target Pattern

```csharp
// In MariloChart.razor @code block
[Parameter] public bool? Transitions { get; set; }
```

Change default from `true` to `null`. Rendering logic checks `Transitions ?? true` (default enabled). This allows consumers to distinguish "not set" from "explicitly true" for theme-level animation defaults.

### Success Criteria
- [ ] `Transitions` typed as `bool?`
- [ ] Default behavior unchanged (animations enabled when null)
- [ ] Can explicitly set `false` to disable animations

---

## RES-CHART-B2-04: OnRender / OnAxisRender events

**Resolves:** GAP-CHART-013
**Status:** Ready for implementation

### Target Pattern

Add render lifecycle events that fire during chart rendering. Consumers can use these for custom annotations, axis label formatting, etc.

```csharp
// In MariloChart.razor @code block

/// <summary>Fires after the chart SVG is rendered. Provides the SVG element reference.</summary>
[Parameter] public EventCallback<ChartRenderEventArgs> OnRender { get; set; }

/// <summary>Fires after the chart is rendered, providing access to chart dimensions and render context.</summary>
public class ChartRenderEventArgs
{
    public double Width { get; init; }
    public double Height { get; init; }
    public int SeriesCount { get; init; }
    public int TotalDataPoints { get; init; }
}
```

### Decision

Keep it simple for v1: fire `OnRender` after the render completes with basic chart info. `OnAxisRender` is deferred — it would require intercepting the axis rendering pipeline which is currently inline. The simpler `OnRender` covers the primary use case (post-render customization).

### Success Criteria
- [ ] `OnRender` EventCallback fires after chart renders
- [ ] `ChartRenderEventArgs` provides width, height, series count, data point count
- [ ] Event does not fire when there's no data

---

## RES-CHART-B2-05: Tooltip template support

**Resolves:** GAP-CHART-014
**Status:** Ready for implementation

### Analysis

`ChartTooltip` already has `Visible`, `Background`, `Color`, `Format` parameters. Missing: `Template` (custom content), `Shared` (shared tooltip for multiple series at same category).

### Target Pattern

```csharp
// In ChartTooltip.razor
/// <summary>Custom content template for the tooltip. Receives ChartTooltipContext.</summary>
[Parameter] public RenderFragment<ChartTooltipContext>? Template { get; set; }

/// <summary>Whether to show a shared tooltip for all series at the hovered category.</summary>
[Parameter] public bool Shared { get; set; }

// New model
public class ChartTooltipContext
{
    public string SeriesName { get; init; } = "";
    public string Category { get; init; } = "";
    public double Value { get; init; }
    public string FormattedValue { get; init; } = "";
    public string Color { get; init; } = "";
    public object? DataItem { get; init; }
}
```

### Decision

Add `Template` and `Shared` parameters. Template takes precedence over Format when set. Shared tooltip shows all series values at the hovered category index. For v1, shared tooltip uses a simple multi-line format; custom shared templates are deferred.

### Success Criteria
- [ ] `Template` parameter accepts `RenderFragment<ChartTooltipContext>`
- [ ] Template receives series name, category, value, formatted value, color, data item
- [ ] `Shared` parameter shows values from all series at same category
- [ ] When `Format` is set and no `Template`, format string applies to value
- [ ] Default tooltip behavior unchanged when neither Template nor Shared is set
