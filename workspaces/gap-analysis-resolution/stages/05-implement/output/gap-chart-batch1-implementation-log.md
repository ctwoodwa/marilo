# Chart Batch 1 — Implementation Log

> Date: 2026-04-04
> Stage: 05-implement
> Batch: Chart Batch 1 (8 gaps: 3 already resolved, 5 implemented)

---

## Summary

| Metric | Value |
|--------|-------|
| Gaps addressed | 8 (3 already resolved, 5 implemented) |
| Files created | 3 (ChartSeriesItems.razor, ChartCategoryAxes.razor, ChartSubtitle.razor) |
| Files modified | 2 (ChartTitle.razor, MariloChart.razor) |
| Tests written | 11 new bUnit tests (5 existing → 16 total) |
| Tests passing | runtime pending (.NET SDK not available) |

---

## Already Resolved (No Code Change)

### GAP-CHART-001: Refresh() — exists at MariloChart.razor:147
### GAP-CHART-004: Class — inherited from MariloComponentBase
### GAP-CHART-015: Legend Position — ChartLegend has Position parameter (ChartPosition enum)

---

## RES-CHART-005: ChartSeriesItems pass-through wrapper

**Files created:** `src/Marilo.Components/Charts/ChartSeriesItems.razor`
Simple pass-through rendering `@ChildContent`. Enables `<ChartSeriesItems>` spec-compatible wrapper.

---

## RES-CHART-006: ChartCategoryAxes pass-through wrapper

**Files created:** `src/Marilo.Components/Charts/ChartCategoryAxes.razor`
Same pass-through pattern. Enables `<ChartCategoryAxes>` wrapper around axis children.

---

## RES-CHART-003: ChartSubtitle child component

**Files created:** `src/Marilo.Components/Charts/ChartSubtitle.razor`
**Files modified:** `src/Marilo.Components/Charts/ChartTitle.razor`

- ChartSubtitle registers with ChartTitle via CascadingParameter
- ChartTitle modified: added CascadingValue self-cascade, ChildContent parameter, Subtitle property, SetSubtitle() method
- MariloChart.razor: subtitle rendered as `<div class="mar-chart-subtitle">` below the title

---

## RES-CHART-008: CSS variable theming bridge

**Files modified:** `src/Marilo.Components/Charts/MariloChart.razor`

- Added `GetCssVariables()` method generating `--mar-chart-bg`, `--mar-chart-text`, and `--mar-chart-series-{N}` CSS custom properties
- Variables emitted on the chart container `style` attribute
- Consumers can override via external CSS; defaults unchanged

---

## RES-CHART-010: Test expansion

**Files modified:** `tests/Marilo.Tests.Unit/P1Content/ChartTests.cs`

| Test Method | Gap | Status |
|-------------|-----|--------|
| Chart_ChartSeriesItems_Wrapper_RendersChildren | GAP-CHART-005 | pending |
| Chart_ChartCategoryAxes_Wrapper_RendersChildren | GAP-CHART-006 | pending |
| Chart_ChartSubtitle_RendersBelow_Title | GAP-CHART-003 | pending |
| Chart_CssVariables_Present_OnContainer | GAP-CHART-008 | pending |
| Chart_CustomPalette_ReflectedInCssVars | GAP-CHART-008 | pending |
| Chart_OnSeriesClick_EventFires | GAP-CHART-010 | pending |
| Chart_AriaLabel_FromTitleChild | GAP-CHART-010 | pending |
| Chart_LegendItemClick_TogglesVisibility | GAP-CHART-010 | pending |
| Chart_Refresh_RerendersSvg | GAP-CHART-001 | pending |
| Chart_Bar_Renders_HorizontalRects | GAP-CHART-009 | pending |
| Chart_HiddenSeries_NotRendered | GAP-CHART-010 | pending |
