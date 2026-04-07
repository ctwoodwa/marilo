# AllocationScheduler Demo Gap List

**Audit Date:** 2026-04-05
**Demo Page:** samples/Marilo.Demo/Pages/Components/AllocationScheduler/AllocationSchedulerDemo.razor
**Stage 01 Input:** stages/01-spec-review/output/allocation-scheduler-spec-gap-list.md (all 9 gaps resolved)

---

## Current Demo Inventory

The demo page has 6 scenarios:

| # | Scenario Title | Parameters Demonstrated | Interactive |
|---|---------------|------------------------|-------------|
| 1 | Basic Resource Grid | Resources, Allocations, AuthoritativeLevel, ViewGrain, ValueMode, VisibleStart, DefaultRangeLength, DefaultRangeUnit, AllowDragFill=false, AllowBulkEdit=false, Height, AllocationResourceColumns, Field, Title, Width | No (read-only) |
| 2 | Interactive Allocation | AllowDragFill=true, AllowKeyboardEdit=true, OnCellEdited | Yes (edit + output) |
| 3 | Conflict Detection | Overlapping allocations on same resource | No |
| 4 | Grouped Resources | Multiple resource columns (Name, Department, Role) | No |
| 5 | Custom Templates | Column-level Template RenderFragment | No |
| 6 | Disabled Slots | AuthoritativeLevel=Day, ViewGrain=Day, daily granularity | No |

---

## Demo Gap List

### (A) Parameters with no demo scenario

| # | Parameter | Priority | Notes |
|---|-----------|----------|-------|
| A1 | `ValueMode=Currency` | P1 | Only Hours shown; Currency mode is a primary use case (budget planning) |
| A2 | `ShowTargets` + `Targets` | P1 | Core analysis feature, no demo |
| A3 | `ShowDeltas` + `DeltaDisplayMode` | P1 | Variance display is a primary feature area |
| A4 | `SelectionMode` (None/Cell/Range) | P1 | No demo toggling selection modes |
| A5 | `EnableContextMenu` + `ContextMenuItems` | P1 | Context menu is a major feature area, no demo |
| A6 | `DefaultDistributionMode` | P2 | Distribution policy, demonstrated via context menu |
| A7 | `AllowZoomEdit` | P2 | Advanced opt-in editing at coarser levels |
| A8 | `AllocationSets` + `ScenarioOverrides` + `ActiveSetId` + `CompareSetId` | P1 | Scenario planning is a top-level feature, no demo at all |
| A9 | `ShowBaselineDiff` | P2 | Baseline comparison |
| A10 | `ShowComparisonPanel` | P2 | Side-by-side scenario view |
| A11 | `ShowCriticalPath` | P3 | Critical path highlighting |
| A12 | `Width` | P3 | Only Height is shown |
| A13 | `Class` | P3 | Custom CSS class |
| A14 | `EnableLoaderContainer` | P3 | Loading animation toggle |
| A15 | `VisibleEnd` | P3 | Explicit end-date binding |
| A16 | `ResourceRowTemplate` | P2 | Scenario 5 uses column Template, not ResourceRowTemplate |
| A17 | `EmptyTemplate` | P2 | Empty state rendering |
| A18 | `ToolbarTemplate` | P2 | Custom toolbar content |
| A19 | `CellTemplate` | P2 | Custom cell rendering (scenario 5 has column template, not CellTemplate) |

### (B) Scenarios with stale code snippets

None identified — all existing snippets use current API names and types.

### (C) Events with no demo scenario

| # | Event | Priority | Notes |
|---|-------|----------|-------|
| C1 | `OnRangeEdited` | P1 | Bulk range editing, no demo |
| C2 | `OnContextMenuAction` | P1 | Context menu interaction, no demo |
| C3 | `OnDistributeRequested` | P2 | Distribution interception |
| C4 | `OnShiftValues` | P2 | Shift-forward/backward |
| C5 | `OnMoveValues` | P2 | Move between resources/tasks |
| C6 | `OnTargetChanged` | P2 | Target set/update |
| C7 | `OnVisibleRangeChanged` | P2 | Navigation event |
| C8 | `OnSelectionChanged` | P2 | Selection change event |
| C9 | `OnScenarioChanged` | P2 | Scenario switch |
| C10 | `OnScenarioCreated` | P2 | New scenario creation |
| C11 | `OnAllocationOverridden` | P3 | Scenario override event |
| C12 | `OnScenarioStatusChanged` | P3 | Scenario status transition |
| C13 | `OnScenarioPromoted` | P3 | Scenario promotion |
| C14 | `CanExecuteAction` | P3 | Context menu enable/disable |
| C15 | `ViewGrainChanged` (two-way) | P2 | Two-way ViewGrain binding |
| C16 | `VisibleStartChanged` (two-way) | P3 | Two-way VisibleStart binding |
| C17 | `ActiveSetIdChanged` (two-way) | P3 | Two-way ActiveSetId binding |

### (D) Edge cases not demonstrated

| # | Edge Case | Priority | Notes |
|---|-----------|----------|-------|
| D1 | Empty state (no allocations) | P2 | EmptyTemplate exists but no scenario shows it |
| D2 | Read-only rollup (ViewGrain coarser than AuthoritativeLevel) | P1 | Key editing grain concept, spec scenario #7 |
| D3 | Zoom level navigation (switch ViewGrain at runtime) | P2 | Switching Day/Week/Month at runtime |
| D4 | AllowBulkEdit with range selection | P2 | Scenario 1 disables it but never shows it enabled with visible selection |

---

## Summary

| Gap Type | Count | P1 | P2 | P3 |
|----------|-------|----|----|-----|
| (A) No demo scenario | 19 | 6 | 8 | 5 |
| (B) Stale snippets | 0 | -- | -- | -- |
| (C) Events no demo | 17 | 2 | 8 | 7 |
| (D) Edge cases | 4 | 1 | 3 | 0 |
| **Total** | **40** | **9** | **19** | **12** |

---

## Resolution

**Scope decision:** P1 + P2 (28 gaps). P3 (12 gaps) deferred.
**Page structure:** Split into separate pages by feature area.

### New Demo Pages Created

| Page | Route | Scenarios | Gaps Resolved |
|------|-------|-----------|---------------|
| BudgetAndTargets.razor | /components/allocation-scheduler/budget-and-targets | 3 (Budget Planning, Targets Overlay, Variance Analysis) | A1, A2, A3, C6 |
| SelectionAndEditing.razor | /components/allocation-scheduler/selection-and-editing | 2 (Selection Modes, Bulk Range Editing) | A4, C1, C8, D4 |
| ContextMenuDemo.razor | /components/allocation-scheduler/context-menu | 4 (Built-in Menu, Custom Commands, Distribution Policy, Shift & Move) | A5, A6, C2, C3, C4, C5 |
| ScenarioPlanning.razor | /components/allocation-scheduler/scenario-planning | 3 (Baseline & Scenario Sets, Baseline Diff, Comparison Panel) | A8, A9, A10, C9, C10 |
| TemplatesDemo.razor | /components/allocation-scheduler/templates | 4 (ResourceRowTemplate, CellTemplate, ToolbarTemplate, Empty State) | A16, A17, A18, A19, D1 |
| NavigationAndZoom.razor | /components/allocation-scheduler/navigation-and-zoom | 3 (Read-Only Rollup, Zoom Level Switching, AllowZoomEdit) | A7, D2, D3, C7, C15 |

### Updated Summary

| Gap Type | Total | Resolved | Deferred (P3) |
|----------|-------|----------|---------------|
| (A) No demo scenario | 19 | 14 | 5 |
| (B) Stale snippets | 0 | 0 | 0 |
| (C) Events no demo | 17 | 10 | 7 |
| (D) Edge cases | 4 | 4 | 0 |
| **Total** | **40** | **28** | **12** |

### Deferred P3 Gaps

A11 (ShowCriticalPath), A12 (Width), A13 (Class), A14 (EnableLoaderContainer), A15 (VisibleEnd), C11 (OnAllocationOverridden), C12 (OnScenarioStatusChanged), C13 (OnScenarioPromoted), C14 (CanExecuteAction), C15 — resolved via NavigationAndZoom, C16 (VisibleStartChanged), C17 (ActiveSetIdChanged).
