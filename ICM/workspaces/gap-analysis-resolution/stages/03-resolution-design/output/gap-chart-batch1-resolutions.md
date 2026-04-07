# Chart Batch 1 — Resolution Records

> Batch scope: API surface wrappers, ChartSubtitle, test expansion, quick closures
> Date: 2026-04-04
> Stage: 03-resolution-design

---

## RES-CHART-001: Close GAP-CHART-001 (Refresh) — Already Resolved

**Resolves:** GAP-CHART-001
**Status:** Resolved (no code change needed)

`Refresh()` exists at MariloChart.razor line 147 as `public void Refresh() => StateHasChanged()`. Gap was misidentified during intake.

---

## RES-CHART-004: Close GAP-CHART-004 (Class) — Already Resolved

**Resolves:** GAP-CHART-004
**Status:** Resolved (no code change needed)

`Class` parameter is inherited from `MariloComponentBase`. Applied to the chart container via `@Class`. Gap was misidentified during intake.

---

## RES-CHART-015: Close GAP-CHART-015 (Legend positioning) — Already Resolved

**Resolves:** GAP-CHART-015
**Status:** Resolved (no code change needed)

`ChartLegend` already has `Position` parameter (type `ChartPosition`, default `Bottom`). Supports Top, Bottom, Left, Right. Gap was misidentified during intake.

---

## RES-CHART-005: ChartSeriesItems pass-through wrapper

**Resolves:** GAP-CHART-005
**Status:** Proposed

### Target Pattern

```razor
@* ChartSeriesItems.razor *@
@ChildContent
@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

Same pattern as `MariloSplitterPanes`, `MariloWizardSteps`. Pass-through wrapper enabling spec-compatible `<ChartSeriesItems>` tag. No behavioral change.

### Success Criteria
- [ ] `<ChartSeriesItems>` wrapper renders children identically to direct nesting
- [ ] Direct nesting without wrapper continues to work
- [ ] bUnit test verifies wrapper

---

## RES-CHART-006: ChartCategoryAxes pass-through wrapper

**Resolves:** GAP-CHART-006
**Status:** Proposed

### Target Pattern

```razor
@* ChartCategoryAxes.razor *@
@ChildContent
@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

Same pass-through pattern. Enables `<ChartCategoryAxes><ChartCategoryAxis .../></ChartCategoryAxes>` tag structure.

### Success Criteria
- [ ] `<ChartCategoryAxes>` wrapper works
- [ ] Direct nesting without wrapper continues to work

---

## RES-CHART-003: ChartSubtitle child component

**Resolves:** GAP-CHART-003
**Status:** Proposed

### Target Pattern

```razor
@* ChartSubtitle.razor *@
<CascadingValue Value="this" IsFixed="true">
    @ChildContent
</CascadingValue>

@code {
    [CascadingParameter] private ChartTitle? ParentTitle { get; set; }
    [Parameter] public string? Text { get; set; }
    [Parameter] public ChartPosition Position { get; set; } = ChartPosition.Bottom;
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        ParentTitle?.SetSubtitle(this);
    }
}
```

ChartSubtitle registers with ChartTitle (not directly with MariloChart). ChartTitle needs a `SetSubtitle(ChartSubtitle)` method and to CascadingValue itself to children. The chart's title rendering section needs to include subtitle text below the main title.

### Decision

Create `ChartSubtitle.razor`. Modify `ChartTitle.razor` to cascade itself and expose `SetSubtitle()`. Modify `MariloChart.razor` title rendering to include subtitle when present.

### Success Criteria
- [ ] ChartSubtitle renders below title in SVG
- [ ] Position parameter controls subtitle placement
- [ ] bUnit test verifies subtitle text appears

---

## RES-CHART-008: CSS variable theming bridge

**Resolves:** GAP-CHART-008
**Status:** Proposed

### Target Pattern

Add a `RenderCssVariableOverrides()` method that emits a `<style>` element scoped to the chart's class/ID, mapping documented CSS variables to the chart's internal color array.

```csharp
private string GetSeriesColorFromCssVar(int index)
{
    // In SSR/prerender: fall back to Palette[index] or DefaultColors[index]
    // CSS variables are applied via generated style element
    return Palette?.Length > index ? Palette[index] : DefaultColors[index % DefaultColors.Length];
}
```

Also add CSS variable declarations to the chart container's style attribute so consumers can override via CSS:

```html
<svg class="mar-chart @Class" style="--mar-chart-bg: ...; --mar-chart-text: ...; @Style">
```

### Decision

Add `--mar-chart-bg`, `--mar-chart-text`, and `--mar-chart-series-{N}` CSS custom properties to the SVG container element. These serve as the bridge: CSS consumers can override them, and the chart's rendering reads from Palette/DefaultColors as before (CSS variables handle the cascade automatically for colors applied via `fill`/`stroke` in SVG).

### Success Criteria
- [ ] Chart container emits CSS variables for series colors
- [ ] External CSS can override `--mar-chart-series-0` etc.
- [ ] Default rendering unchanged when no CSS overrides applied
- [ ] bUnit test verifies CSS variable presence

---

## RES-CHART-010: Test coverage expansion

**Resolves:** GAP-CHART-010
**Status:** Proposed

### Target

Expand from 5 tests to 15+ tests covering:
- Series type rendering (Column, Line, Bar, Area, Pie, Donut, Scatter)
- Events (OnSeriesClick, OnLegendItemClick)
- Child component registration (ChartTitle, ChartLegend, ChartTooltip)
- ChartSeriesItems wrapper
- ChartSubtitle
- CSS variable output
- Accessibility (role, aria-label)
- Refresh() method
- Transitions parameter

### Decision

Add tests to existing `ChartTests.cs`. Cover each resolution from this batch plus key existing features.

---

## Summary

| Resolution | Gaps | Effort | Notes |
|------------|------|--------|-------|
| RES-CHART-001 | GAP-CHART-001 | — | Already resolved (close) |
| RES-CHART-004 | GAP-CHART-004 | — | Already resolved (close) |
| RES-CHART-015 | GAP-CHART-015 | — | Already resolved (close) |
| RES-CHART-005 | GAP-CHART-005 | S | Pass-through wrapper |
| RES-CHART-006 | GAP-CHART-006 | S | Pass-through wrapper |
| RES-CHART-003 | GAP-CHART-003 | M | New child component + ChartTitle modification |
| RES-CHART-008 | GAP-CHART-008 | M | CSS variable bridge |
| RES-CHART-010 | GAP-CHART-010 | L | 10+ new tests |
