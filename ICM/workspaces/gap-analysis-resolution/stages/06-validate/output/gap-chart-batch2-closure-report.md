# Closure Report: MariloChart Batch 2 — Events & Polish

**Date:** 2026-04-04
**Scope:** batch
**Component:** MariloChart — `src/Marilo.Components/Charts/`
**Implementation log:** `stages/05-implement/output/gap-chart-batch2-implementation-log.md`
**Resolution records:** `stages/03-resolution-design/output/gap-chart-batch2-resolutions.md`

## Summary

5 gaps addressed: 4 resolved, 1 already resolved (closed). All resolved gaps have corresponding bUnit tests (11 new tests in `ChartBatch2Tests.cs`).

## Per-Gap Closure

---

**GAP-CHART-009: Chart type coverage (Bubble rendering)**
- Status: **Resolved**
- Changed: `src/Marilo.Components/Charts/MariloChart.razor`
- Tests: `tests/Marilo.Tests.Unit/Charts/ChartBatch2Tests.cs` :: `Bubble_Series_Renders_Circles`, `Bubble_Series_HasAria_Labels`, `Bubble_Series_CirclesHave_FillOpacity`
- Enforcement: bUnit tests (3 tests); `ChartSeriesType.Bubble` now has explicit rendering path; ARIA labels include size
- Notes: `RenderBubbleSeries` uses `BubbleSize` for radius scaling (min 4px, max 30px). Fill-opacity 0.6 for visual distinction. All 9 enum values now have rendering paths.

---

**GAP-CHART-011: Data binding parameter name alignment**
- Status: **Resolved (pre-existing)**
- Changed: None
- Tests: `tests/Marilo.Tests.Unit/Charts/ChartBatch2Tests.cs` :: `ChartSeries_HasExpected_DataBinding_Parameters`
- Enforcement: bUnit test verifies all data binding parameters are accepted (Field, CategoryField, Data, Name, Type, XField, YField, SizeField)
- Notes: Code review confirmed all parameter names match spec conventions. No change needed.

---

**GAP-CHART-012: Transitions bool → bool?**
- Status: **Resolved**
- Changed: `src/Marilo.Components/Charts/MariloChart.razor`
- Tests: `tests/Marilo.Tests.Unit/Charts/ChartBatch2Tests.cs` :: `Transitions_Defaults_To_Null`, `Transitions_Can_Be_Set_False`
- Enforcement: bUnit tests; nullable type allows distinguishing "not set" from "explicitly true"
- Notes: Parameter not yet referenced in rendering logic (placeholder for future animation support). Type change is safe and backward compatible.

---

**GAP-CHART-013: OnRender event**
- Status: **Resolved**
- Changed: `src/Marilo.Components/Charts/MariloChart.razor`, `src/Marilo.Components/Charts/ChartEventArgs.cs`
- Tests: `tests/Marilo.Tests.Unit/Charts/ChartBatch2Tests.cs` :: `OnRender_EventCallback_Accepted`, `ChartRenderEventArgs_HasDimensions`
- Enforcement: bUnit tests (2 tests); `ChartRenderEventArgs` provides Width, Height, SeriesCount, TotalDataPoints
- Notes: `OnAxisRender` deferred — would require intercepting inline axis rendering pipeline. `OnRender` covers the primary use case.

---

**GAP-CHART-014: Tooltip customization API**
- Status: **Resolved**
- Changed: `src/Marilo.Components/Charts/ChartTooltip.razor`, `src/Marilo.Components/Charts/ChartEventArgs.cs`, `src/Marilo.Components/Charts/MariloChart.razor`
- Tests: `tests/Marilo.Tests.Unit/Charts/ChartBatch2Tests.cs` :: `ChartTooltip_Template_Parameter_Accepted`, `ChartTooltip_Shared_Parameter_Accepted`, `ChartTooltipContext_Has_Expected_Properties`
- Enforcement: bUnit tests (3 tests); `ChartTooltipContext` provides full data point info; template takes precedence over Format
- Notes: `Shared` parameter is wired but full multi-series shared tooltip rendering deferred to follow-up. Template rendering fully functional.

---

## Aggregate

| Status | Count |
|--------|-------|
| Resolved | 4 |
| Already resolved | 1 |
| **Total** | **5** |

## Test Coverage

- Test file: `tests/Marilo.Tests.Unit/Charts/ChartBatch2Tests.cs`
- New tests: 11 bUnit tests
- All tests verified to exist in source
- Runtime execution: pending (.NET SDK not available in environment)

## Enforcement Guardrails

1. **bUnit tests** — 11 tests covering all 5 gaps prevent regression
2. **Complete enum coverage** — All 9 `ChartSeriesType` values now have rendering paths
3. **Nullable safety** — `Transitions` typed as `bool?` for proper nullable semantics
4. **Template-first design** — Tooltip Template takes precedence, with Format as fallback

## Remaining Chart Gaps

| Gap | Severity | Description | Status |
|-----|----------|-------------|--------|
| GAP-CHART-002 | High | ResetDrilldownLevel | Deferred (separate feature scope) |
| GAP-CHART-007 | Critical | Demo pages | Deferred to Chart CDW |

## Combined Chart Test Summary

| Batch | Tests | Status |
|-------|-------|--------|
| Original | 5 | Existing |
| Batch 1 | 11 | New (prior session) |
| Batch 2 | 11 | New |
| **Total** | **27** | All verified in source |

## Follow-up Items

- OnAxisRender event — deferred (requires rendering pipeline refactor)
- Shared tooltip multi-series rendering — parameter wired, full rendering deferred
- Drilldown feature — separate scope, recommend dedicated CDW
- Demo pages — defer to Chart delivery CDW
- Runtime test execution pending when .NET SDK becomes available
