# Gantt Wave 2 — Spec Cleanup Report (Lane L1)

**Date:** 2026-04-12
**Scope:** W4-INT-01, 02, 03, 04, 05, 06, 07, 26

---

## Changes Made

### W4-INT-02 — Overview parameter table under-populated (P1)

**File:** `docs/component-specs/gantt/overview.md`

Rewrote the Gantt Parameters section from a 2-row table to a comprehensive, categorized parameter reference. Now organized into 7 sub-tables:

- **Data Binding** (10 params): Data, IdField, ParentIdField, TitleField, StartField, EndField, PercentCompleteField, DependsOnField, ItemsField, HasChildrenField
- **Layout and Dimensions** (5 params): Width, Height, TaskListWidth, DayWidth, RowHeight
- **Features** (7 params): Sortable, FilterMode, FilterPopupMode, FilterRowDebounceDelay, TreeListEditMode, NewRowPosition, ShowColumnChooser
- **Templates** (3 params): TaskTemplate, TooltipTemplate, GanttToolBarTemplate
- **Child Content Slots** (3 params): GanttColumns, GanttViews, GanttDependenciesSlot
- **Timeline View** (2 params): View, ViewChanged
- **State** (2 params): OnStateInit, OnStateChanged
- **Events** (7 params): OnTaskClick, OnTaskEdit, OnCreate, OnUpdate, OnDelete, OnExpand, OnCollapse

All parameter names, types, and defaults verified against `src/Marilo.Components/DataDisplay/MariloGantt.razor.cs`.

**Status: CLOSED**

### W4-INT-03 — Overview methods table missing GetState/SetStateAsync (P2)

**File:** `docs/component-specs/gantt/overview.md`

Added `GetState()` and `SetStateAsync(GanttState<TItem>?)` to the methods table with return types and descriptions. Also improved the `Rebind()` entry with return type column. Updated API reference slug from `Marilo.Blazor.Components` to `Marilo.Components.DataDisplay`.

**Status: CLOSED**

### W4-INT-04 — Stale namespace `Marilo.Blazor.Components` (P2)

**File:** `docs/component-specs/gantt/overview.md`

Replaced all instances of `slug:Marilo.Blazor.Components.MariloGantt-1` with `slug:Marilo.Components.DataDisplay.MariloGantt-1`. There is no `Marilo.Blazor` namespace in the codebase.

**Status: CLOSED**

### W4-INT-01 — `VisibleColumns` absent from state.md enumeration (P2)

**File:** `docs/component-specs/gantt/state.md`

Added `VisibleColumns` to the Phase 1 "Currently Available" list. Verified against `GanttState.cs:97` (`public IEnumerable<string>? VisibleColumns`) and `MariloGantt.razor.cs:1701-1703` (populates VisibleColumns in GetState).

**Status: CLOSED**

### W4-INT-05 — Stale DataGrid paging bullet in state.md (P2)

**File:** `docs/component-specs/gantt/state.md`

Removed the stale paragraph about "Filtering always resets the current page to 1" and `PropertyName == "Page"` / `"FilterDescriptors"`. Replaced with correct text about `PropertyName == "FilterValues"`. The Gantt does not have paging or FilterDescriptor types.

**Status: CLOSED**

### W4-INT-26 — state.md examples use non-existent APIs (P2)

**File:** `docs/component-specs/gantt/state.md`

Fixed two examples:

1. **"Set Default (Initial) State"** — Removed `@using Marilo.DataSource`, `ColumnResizable="true"` (doesn't exist), `FilterDescriptorCollection`, `CompositeFilterDescriptor`, `SortDescriptors` (list of Marilo.DataSource.SortDescriptor). Replaced with correct `GanttSortDescriptor`, `FilterValues` dictionary, and `GanttView.Week`. Added proper `TItem` type parameter.

2. **"Get and Override User Action"** — Removed `@using Marilo.DataSource`, `FilterDescriptors`, `CompositeFilterDescriptor`, `FilterDescriptor`, `FilterOperator`. Replaced with correct `FilterValues` dictionary API. Updated `PropertyName == "FilterDescriptors"` to `PropertyName == "FilterValues"`.

3. **"Save and Load from LocalStorage"** — Replaced `@bind-TaskListWidth="@TaskListWidth"` with `TaskListWidth="@TaskListWidth"` (no two-way binding exists).

**Status: CLOSED**

### W4-INT-06 — Milestone/summary spec coverage gap (P2)

**File:** `docs/component-specs/gantt/overview.md`

Added two new sections:
- **Milestones** — Explains zero-duration detection (`Start == End`), diamond rendering, CSS class `mar-gantt__milestone`.
- **Summary Tasks** — Explains automatic bottom-up aggregation (`ComputeSummaryValues`), CSS class `mar-gantt__bar--summary`, no manual parent-date maintenance required.

Both verified against source: milestone detection at `MariloGantt.razor` line pattern `GetStart(item) == GetEnd(item)`, summary computation at `MariloGantt.razor.cs:755-776`.

**Status: CLOSED**

### W4-INT-07 — refresh-data.md missing reference-and-count detection (P2)

**File:** `docs/component-specs/gantt/refresh-data.md`

Added "Automatic Change Detection" subsection explaining the two-check mechanism:
1. Reference equality check (`ReferenceEquals`) on Data
2. Field parameter key change detection (accessor key string)

Both verified against `MariloGantt.razor.cs:554-574` (`OnParametersSetAsync`).

**Status: CLOSED**

### Additional Fix — events.md missing OnStateInit/OnStateChanged

**File:** `docs/component-specs/gantt/events.md`

Added `OnStateInit and OnStateChanged` section to the events article with event types, `PropertyName` values, and cross-reference to the state article. Also added the entry to the article's table of contents.

---

## Verification

All spec changes cross-referenced against:
- `src/Marilo.Components/DataDisplay/MariloGantt.razor.cs` (parameters, methods, state API)
- `src/Marilo.Components/DataDisplay/GanttState.cs` (state class, enums, event args)
- `src/Marilo.Components/DataDisplay/MariloGantt.razor` (template usage, CSS classes)
- `src/Marilo.Components/DataDisplay/MariloGanttDependencies.razor` (dependency slot)

Build: `dotnet build Marilo.slnx` — 0 errors.

---

## Summary

| Gap ID | Description | Status |
|---|---|---|
| W4-INT-01 | VisibleColumns in state.md | CLOSED |
| W4-INT-02 | Overview parameter table | CLOSED |
| W4-INT-03 | Methods table (GetState/SetStateAsync) | CLOSED |
| W4-INT-04 | Stale namespace | CLOSED |
| W4-INT-05 | Stale paging reference | CLOSED |
| W4-INT-06 | Milestone/summary coverage | CLOSED |
| W4-INT-07 | Refresh-data detection explanation | CLOSED |
| W4-INT-26 | State examples using wrong APIs | CLOSED |

**8 of 8 L1 spec cleanup gaps closed.**
