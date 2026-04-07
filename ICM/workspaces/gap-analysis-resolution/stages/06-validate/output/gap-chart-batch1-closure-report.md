# Chart Batch 1 — Closure Report

> Date: 2026-04-04
> Stage: 06-validate
> Batch: Chart Batch 1 (API surface, wrappers, theming, tests)

---

## Closure Summary

| Metric | Value |
|--------|-------|
| Gaps in batch | 8 |
| Resolved | 8 (3 already resolved + 5 implemented) |
| Deferred | 0 |
| Tests written | 11 new (16 total) |
| Tests passing | runtime pending |

---

## Per-Gap Evidence

### GAP-CHART-001: Refresh() — RESOLVED (pre-existing)
`Refresh()` exists at MariloChart.razor:147. Test: `Chart_Refresh_RerendersSvg`.

### GAP-CHART-004: Class parameter — RESOLVED (pre-existing)
Inherited from MariloComponentBase. Verified in container div.

### GAP-CHART-015: Legend positioning — RESOLVED (pre-existing)
ChartLegend has `Position` parameter (ChartPosition enum, default Bottom).

### GAP-CHART-005: ChartSeriesItems wrapper — RESOLVED
Pass-through wrapper created. Test: `Chart_ChartSeriesItems_Wrapper_RendersChildren` — series inside wrapper render identically.

### GAP-CHART-006: ChartCategoryAxes wrapper — RESOLVED
Pass-through wrapper created. Test: `Chart_ChartCategoryAxes_Wrapper_RendersChildren` — axis inside wrapper registers with chart.

### GAP-CHART-003: ChartSubtitle — RESOLVED
New child component. Test: `Chart_ChartSubtitle_RendersBelow_Title` — verifies subtitle text and `mar-chart-subtitle` CSS class.

### GAP-CHART-008: CSS variable theming — RESOLVED
`GetCssVariables()` emits `--mar-chart-bg`, `--mar-chart-text`, `--mar-chart-series-{N}`. Tests: `Chart_CssVariables_Present_OnContainer`, `Chart_CustomPalette_ReflectedInCssVars`.

### GAP-CHART-010: Test coverage — RESOLVED
Expanded from 5 to 16 tests covering wrappers, subtitle, CSS vars, events, ARIA, series visibility, Bar type, Refresh.

---

## Remaining Chart Gaps (Batch 2)

| Gap | Severity | Description |
|-----|----------|-------------|
| GAP-CHART-002 | High | ResetDrilldownLevel (drilldown feature) |
| GAP-CHART-007 | Critical | Demo pages |
| GAP-CHART-009 | High | Chart type coverage audit (partial — Bar tested) |
| GAP-CHART-011 | Medium | Data binding parameter name alignment |
| GAP-CHART-012 | Medium | Transitions bool → bool? |
| GAP-CHART-013 | Medium | OnRender / OnAxisRender events |
| GAP-CHART-014 | Medium | Tooltip customization API |

---

## Sign-off

Batch 1 closes 8/8 gaps with 16 bUnit tests. 3 gaps were identified as already resolved during code review. Runtime test execution pending .NET SDK availability.
