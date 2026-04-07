# Gap Inventory: MariloAllocationScheduler — Phase 1

**Intake date:** 2026-04-05
**Source:** allocation-scheduler-delivery Stage 02 + Stage 03 outputs
**Scope:** batch (related gaps in demo coverage and test coverage)

---

## A. Deferred P3 Demo Gaps

These parameters and events have no demo scenario. Each needs a scenario added to an existing or new demo page.

### Parameters

| ID | Parameter | Type | Default | Current Demo Coverage |
|----|-----------|------|---------|----------------------|
| GAP-P3-001 | `ShowCriticalPath` | `bool` | `false` | None |
| GAP-P3-002 | `Width` | `string` | — | None (only `Height` demonstrated) |
| GAP-P3-003 | `Class` | `string` | — | None |
| GAP-P3-004 | `EnableLoaderContainer` | `bool` | `true` | None |
| GAP-P3-005 | `VisibleEnd` | `DateTime?` | derived | None |

### Events

| ID | Event | Type | Current Demo Coverage |
|----|-------|------|----------------------|
| GAP-P3-006 | `OnAllocationOverridden` | `EventCallback<AllocationOverriddenArgs>` | None |
| GAP-P3-007 | `OnScenarioStatusChanged` | `EventCallback<ScenarioStatusChangedArgs>` | None |
| GAP-P3-008 | `OnScenarioPromoted` | `EventCallback<ScenarioPromotedArgs>` | None |
| GAP-P3-009 | `CanExecuteAction` | `EventCallback<CanExecuteActionArgs>` | None |
| GAP-P3-010 | `VisibleStartChanged` | `EventCallback<DateTime>` | None (two-way binding callback) |
| GAP-P3-011 | `ActiveSetIdChanged` | `EventCallback<Guid>` | Partially — used in ScenarioPlanning.razor but not primary focus |
| GAP-P3-012 | `BaselineDateFormat` | `string?` | None |

## B. Test Coverage Gaps

| ID | Gap | Current State | Target |
|----|-----|---------------|--------|
| GAP-TEST-001 | Not all 32 parameters have dedicated bUnit test assertions | 18 tests cover key scenarios (rendering, interactions, accessibility, CSS, toolbar) | At least one test assertion per parameter |
| GAP-TEST-002 | Scenario planning parameters untested | No tests for AllocationSets, ScenarioOverrides, ActiveSetId, CompareSetId, ShowBaselineDiff | Basic rendering + scenario switch test |

---

## Summary

| Category | Count |
|----------|-------|
| P3 Demo gaps (parameters) | 5 |
| P3 Demo gaps (events) | 7 |
| Test coverage gaps | 2 |
| **Total** | **14** |
