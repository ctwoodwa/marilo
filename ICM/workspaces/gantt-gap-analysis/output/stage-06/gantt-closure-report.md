# MariloGantt Gap Analysis — Closure Report

**Date:** 2026-04-09
**Pipeline:** Stages 01 through 06 (systematic scope)
**Source:** Marilo-gantt-rewrite worktree
**Build:** 0 errors, 0 warnings (Marilo.Components)
**Tests:** 648 passed, 0 failed, 0 skipped

---

## Gaps Resolved This Run

### Phase A — Spec Corrections (Tier 1: 12 items)

| ID | Correction | Status |
|---|---|---|
| T1-01 | TreeListWidth → TaskListWidth (name, type, default) | Resolved |
| T1-02 | OnEdit → OnTaskEdit (all spec files) | Resolved |
| T1-03 | ToolTipTemplate → TooltipTemplate | Resolved |
| T1-04 | GanttColumn.Visible bool? → bool | Resolved |
| T1-05 | RangeStart/RangeEnd DateTime → DateTime? | Resolved |
| T1-06 | TooltipTemplate RenderFragment(object) → RenderFragment(TItem) | Resolved |
| T1-07 | TaskTemplate scope clarification | Resolved |
| T1-08 | Dependency databind: Current Impl + Target Arch sections | Resolved |
| T1-09 | Sorting tri-state rewrite + filter auto-expand | Resolved |
| T1-10 | Timeline ARIA role="treeitem" → role="img" | Resolved |
| T1-11 | @bind-View convention note | Resolved |
| T1-12 | Per-view SlotWidth defaults | Resolved |

### Phase A — Documentation Additions (Tier 2: 11 items)

| ID | Addition | Status |
|---|---|---|
| T2-01 | OnTaskClick + ViewChanged events | Resolved |
| T2-02 | 7 field mapping parameters table | Resolved |
| T2-03 | OnExpand/OnCollapse/OnCreate expanded docs | Resolved |
| T2-04 | RowHeight + GanttToolBarTemplate | Resolved |
| T2-05 | Filterable/Sortable in column param table | Resolved |
| T2-06 | DayWidth legacy + GanttViews fallback | Resolved |
| T2-07 | DependsOnField (already in T1-08) | Skipped/covered |
| T2-08 | Accessibility attrs (aria-sort, tabindex, labels) | Resolved |
| T2-09 | Keyboard navigation table | Resolved |
| T2-10 | RowHeight + ViewChanged in timeline | Resolved |
| T2-11 | Rebind() in state spec | Resolved |
| T2-12 | Auto-detection + Rebind recomputation | Resolved |

### Phase B — State API Implementation

| Item | Status | Files |
|---|---|---|
| GanttState(TItem) class | Implemented | GanttState.cs |
| GanttSortDescriptor class | Implemented | GanttState.cs |
| GanttStateEventArgs(TItem) class | Implemented | GanttState.cs |
| OnStateInit parameter | Implemented | MariloGantt.razor.cs |
| OnStateChanged parameter | Implemented | MariloGantt.razor.cs |
| GetState() method | Implemented | MariloGantt.razor.cs |
| SetStateAsync() method | Implemented | MariloGantt.razor.cs |
| FireStateChanged wiring (4 methods) | Implemented | MariloGantt.razor.cs |
| state.md Phase 1/2 split | Documented | state.md |

### Phase C — Feature Implementations

| Task | Item | Status | Files |
|---|---|---|---|
| C1 | GanttCommandColumn | Implemented | GanttCommandColumn.razor, MariloGantt.razor/.cs |
| C2 | Sortable param | Implemented | MariloGantt.razor.cs |
| C2 | GanttFilterMode enum + FilterMode param | Implemented | GanttState.cs, MariloGantt.razor/.cs |
| C2 | FilterRowDebounceDelay + CTS debounce | Implemented | MariloGantt.razor.cs |
| C3 | Percent-complete bar rendering | Implemented | MariloGantt.razor |
| C4 | GanttTreeListEditMode enum | Implemented | GanttState.cs |
| C4 | GanttNewRowPosition enum | Implemented | GanttState.cs |
| C4 | GanttEditorType enum | Implemented | GanttState.cs |
| C4 | GanttEditEventArgs (cancellable) | Implemented | GanttEventArgs.cs |
| C4 | OnTaskEdit → EventCallback(GanttEditEventArgs) | Implemented | MariloGantt.razor.cs |
| C4 | EditorType on GanttColumn | Implemented | GanttColumn.razor |
| C5 | MainHeaderTemplate/SubHeaderTemplate on GanttViewBase | Implemented | GanttViewBase.cs |
| C5 | MainHeaderDateFormat/SubHeaderDateFormat | Implemented | GanttViewBase.cs |
| C5 | Template/format rendering in razor | Implemented | MariloGantt.razor |
| C6 | Navigable removed from spec | Resolved | 4 spec files |
| C6 | Hover delete button on bars | Implemented | MariloGantt.razor |

### Phase D — Accessibility

| Task | Item | Status |
|---|---|---|
| D1 | Skip navigation links | Implemented |
| D2 | prefers-reduced-motion | Deferred (no Gantt SCSS) |

---

## Verification

| Check | Result |
|---|---|
| dotnet build (Marilo.Components) | 0 errors, 0 warnings |
| dotnet test (Marilo.Tests.Unit) | 648 passed, 0 failed |
| TreeListWidth in spec | 0 hits |
| ToolTipTemplate in spec | 0 hits |
| TooltipTemplateContext in spec | 0 hits |
| OnEdit= in spec | 0 hits |
| Navigable= in spec | 0 hits |
| role="treeitem" in spec | 0 hits |
| Visible bool? in spec | 0 hits (fixed) |

---

## Phase E — Deferred Gap Resolution (second pass)

### E1: Milestone rendering (SPEC-gantt-512)
- Zero-duration tasks (Start==End) render as diamond marker instead of bar
- CSS class: `mar-gantt__milestone` + `mar-gantt__milestone-diamond`
- **Status:** Resolved

### E2: Summary task auto-calculation (SPEC-gantt-513)
- Parent nodes auto-compute Start (min children), End (max children), PercentComplete (weighted avg)
- `ComputeSummaryValues()` runs bottom-up after `BuildTree()`
- CSS class: `mar-gantt__bar--summary`
- **Status:** Resolved

### E3: GanttState Phase 2 (SPEC-gantt-205–209)
- Added EditItem, OriginalEditItem (TODO: clone), InsertedItem, EditField, ParentItem to GanttState
- EditItem + EditField wired to GetState/SetStateAsync; FireStateChanged on edit start/commit/cancel
- **Status:** Partially resolved (OriginalEditItem, InsertedItem, ParentItem not yet wired)

### E4: Hierarchical data binding (SPEC-gantt-406)
- `ItemsField` parameter enables hierarchical mode (children nested in items)
- `HasChildrenField` parameter shows expand arrow for lazy-loaded children
- `BuildTreeHierarchical()` + `CreateNodeRecursive()` added
- **Status:** Resolved

### E5: Incell edit mode (SPEC-gantt-408)
- `GanttTreeListEditMode.Incell` added to enum
- Click cell to edit, Tab to next, Enter to commit, Escape to cancel
- `BeginCellEdit`, `TabToNextCell`, `HandleCellKeyDown` methods
- **Status:** Resolved

### E6: Filter menu (SPEC-gantt-414)
- `GanttFilterMode.FilterMenu` added to enum
- Funnel icon button in column headers, popup with input/Filter/Clear
- `ToggleFilterMenu`, `ApplyFilterMenu`, `ClearFilterMenu` methods
- **Status:** Resolved

### E7: Gantt SCSS + accessibility (D2 + SPEC-gantt-721)
- Created `_gantt.scss` (FluentUI) and `_bridge-gantt.scss` (Bootstrap)
- Styles for milestone, summary bar, progress bar, hover delete, filter menu, command buttons, incell cursor
- `@media (prefers-reduced-motion: reduce)` — disables animations
- `@media (forced-colors: active)` — system colors for bars, milestones, filter indicators
- Dark mode patch in Bootstrap provider
- **Status:** Resolved

### E8: Spec updates
- All 7 feature areas documented in respective spec files
- **Status:** Resolved

---

## Breaking Changes

| Change | Impact |
|---|---|
| OnTaskEdit: EventCallback(TItem) → EventCallback(GanttEditEventArgs) | Consumers binding OnTaskEdit must update handler signature |
| BeginEdit: void → async Task | Internal only; no consumer impact |

---

## Remaining Deferred Items

| Item | Gap IDs | Reason |
|---|---|---|
| GanttState: OriginalEditItem (clone), InsertedItem, ParentItem wiring | 206-209 | Needs item cloning strategy |
| GanttState: ColumnStates, TaskListWidth | 213-214 | Depends on column reorder/resize |
| GanttDependencies component model | 600-618 | Architecture planned, implementation deferred |
| Column reorder + resize | 402-403 | Needs JS interop |
| Column menu + chooser | 404, 426-427 | P3 |
| Popup edit mode | 409 | Needs dialog/modal infrastructure |
| Filter checkbox list | 415-417 | P3 |
| Timeline drag-move + resize | 501-502 | Needs JS interop |
| RangeSnapTo / zooming | 500 | Needs JS interop |
| Screen reader drag announcements | 720 | Deferred with drag |

---

## Files Changed Summary

### Spec files (c:/Projects/Marilo/docs/component-specs/gantt/)
- overview.md, events.md, state.md, refresh-data.md
- gantt-tree/overview.md, data-binding/overview.md, columns/bound.md, columns/visible.md, columns/command.md
- gantt-tree/sorting.md, filter/filter-row.md
- gantt-tree/editing/overview.md
- timeline/views.md, overview.md, templates/tooltip.md, templates/task.md
- dependencies/overview.md, databind.md, editing.md
- accessibility/overview.md, wai-aria-support.md

### Source files (c:/Projects/Marilo-gantt-rewrite/src/Marilo.Components/DataDisplay/)
- GanttState.cs (NEW)
- GanttCommandColumn.razor (NEW)
- GanttEventArgs.cs (modified)
- GanttViewBase.cs (modified)
- GanttColumn.razor (modified)
- MariloGantt.razor (modified)
- MariloGantt.razor.cs (modified)
