# MariloGantt Remediation Plan — Stage 04

**Date:** 2026-04-09
**Input:** output/stage-03/gantt-resolution-designs.md
**Execution model:** Subagent-driven development (SDD)
**Branch:** Marilo-gantt-rewrite worktree

## Phase A — Spec Corrections (Tier 1 + Tier 2)

Spec-only changes. No code. Can be executed as a single batch.

### Task A1: Bulk spec corrections (Tier 1)

**Scope:** 12 find-replace + structural edits across spec files
**Files to modify:**
- `docs/component-specs/gantt/overview.md` — T1-03 (ToolTipTemplate casing)
- `docs/component-specs/gantt/events.md` — T1-02 (OnEdit→OnTaskEdit)
- `docs/component-specs/gantt/state.md` — T1-01 (TreeListWidth→TaskListWidth)
- `docs/component-specs/gantt/refresh-data.md` — T1-01 (TreeListWidth→TaskListWidth)
- `docs/component-specs/gantt/gantt-tree/overview.md` — T1-01
- `docs/component-specs/gantt/gantt-tree/columns/visible.md` — T1-04 (bool?→bool)
- `docs/component-specs/gantt/gantt-tree/columns/bound.md` — T1-04
- `docs/component-specs/gantt/gantt-tree/sorting.md` — T1-09 (tri-state rewrite)
- `docs/component-specs/gantt/gantt-tree/filter/filter-row.md` — T1-09 (auto-expand + fill empty sections)
- `docs/component-specs/gantt/timeline/views.md` — T1-05 (DateTime?), T1-11 (binding), T1-12 (defaults)
- `docs/component-specs/gantt/timeline/templates/tooltip.md` — T1-06 (RenderFragment<TItem>)
- `docs/component-specs/gantt/timeline/templates/task.md` — T1-07 (scope clarification)
- `docs/component-specs/gantt/dependencies/overview.md` — T1-08 (current impl section)
- `docs/component-specs/gantt/dependencies/databind.md` — T1-08 (current impl + target arch)
- `docs/component-specs/gantt/accessibility/wai-aria-support.md` — T1-10 (role=img)

**Complexity:** Mechanical — haiku model
**Estimated subtasks:** 1 (batch all corrections in one pass)

### Task A2: Document undocumented source features (Tier 2)

**Scope:** 12 documentation additions across spec files
**Files to modify:**
- `docs/component-specs/gantt/events.md` — T2-01, T2-03 (add OnTaskClick, ViewChanged, OnExpand, OnCollapse, expand OnCreate)
- `docs/component-specs/gantt/gantt-tree/data-binding/overview.md` — T2-02 (field mapping table)
- `docs/component-specs/gantt/gantt-tree/columns/bound.md` — T2-05 (Filterable, Sortable)
- `docs/component-specs/gantt/overview.md` or `gantt-tree/overview.md` — T2-04 (RowHeight, GanttToolBarTemplate)
- `docs/component-specs/gantt/timeline/views.md` — T2-06 (DayWidth legacy)
- `docs/component-specs/gantt/timeline/overview.md` — T2-10 (RowHeight, ViewChanged)
- `docs/component-specs/gantt/dependencies/databind.md` — T2-07 (DependsOnField)
- `docs/component-specs/gantt/accessibility/wai-aria-support.md` — T2-08 (a11y attrs)
- `docs/component-specs/gantt/accessibility/overview.md` — T2-09 (keyboard table)
- `docs/component-specs/gantt/state.md` — T2-11 (Rebind)
- `docs/component-specs/gantt/refresh-data.md` — T2-12 (auto-detection, Rebind timeline)

**Complexity:** Mechanical — haiku model
**Estimated subtasks:** 1 (batch all additions in one pass)

---

## Phase B — Minimal State API (Tier 3, RES-T3-STATE)

Code changes in the Marilo-gantt-rewrite worktree.

### Task B1: Create GanttState&lt;TItem&gt; and GanttStateEventArgs

**Files to create:**
- `src/Marilo.Components/DataDisplay/GanttState.cs`

**Files to modify:**
- `src/Marilo.Components/DataDisplay/MariloGantt.razor.cs` — add parameters, wire events

**Scope:**
1. Define `GanttState<TItem>` class with `SortDescriptors`, `FilterDescriptors`, `ExpandedItems`, `View`
2. Define `GanttStateEventArgs<TItem>` with `State` and `PropertyName`
3. Add `OnStateInit` and `OnStateChanged` parameters to MariloGantt
4. Add `GetState()` and `SetStateAsync()` public methods
5. Fire `OnStateInit` from `OnInitializedAsync`
6. Fire `OnStateChanged` from `SortBy()`, `OnFilterInput()`, `ToggleExpanded()`, `SwitchView()`

**Complexity:** Integration — sonnet model
**Dependencies:** None
**Tests:** New test file `tests/Marilo.Tests.Unit/DataDisplay/MariloGanttStateTests.cs`

### Task B2: Update state.md spec

**Files to modify:**
- `docs/component-specs/gantt/state.md`

**Scope:** Add "Phase 1 (Current)" section documenting the minimal API. Mark Phase 2 properties as "Planned". Update code examples to use only Phase 1 properties.

**Complexity:** Mechanical — haiku model
**Dependencies:** Task B1

---

## Phase C — Feature Implementations (Tier 3 P2)

Each task is independent and can be executed sequentially via SDD.

### Task C1: GanttCommandColumn

**Files to create:**
- `src/Marilo.Components/DataDisplay/GanttCommandColumn.razor`

**Files to modify:**
- `src/Marilo.Components/DataDisplay/MariloGantt.razor` (render command cells)
- `src/Marilo.Components/DataDisplay/MariloGantt.razor.cs` (register command column)

**Complexity:** Integration — sonnet model
**Tests:** Add command column tests to `MariloGanttTests.cs`

### Task C2: Sortable parameter + FilterMode + FilterRowDebounceDelay

**Files to modify:**
- `src/Marilo.Components/DataDisplay/MariloGantt.razor.cs` — add parameters, guard methods

**Complexity:** Mechanical — haiku model
**Tests:** Add to existing test file

### Task C3: Percent-complete bar rendering

**Files to modify:**
- `src/Marilo.Components/DataDisplay/MariloGantt.razor` — add progress fill div inside bar

**Complexity:** Mechanical — haiku model
**Tests:** Add visual assertion test

### Task C4: TreeListEditMode enum + Cancellable OnEdit + NewRowPosition + EditorType

**Files to create:**
- `src/Marilo.Components/DataDisplay/GanttEnums.cs` (TreeListEditMode, NewRowPosition, EditorType enums)

**Files to modify:**
- `src/Marilo.Components/DataDisplay/MariloGantt.razor.cs` — add parameters, wire OnEdit cancellation
- `src/Marilo.Components/DataDisplay/GanttColumn.razor` — add EditorType parameter
- `src/Marilo.Components/DataDisplay/GanttEventArgs.cs` — add GanttEditEventArgs

**Complexity:** Integration — sonnet model
**Tests:** Add edit mode tests

### Task C5: Date header templates + format parameters

**Files to modify:**
- `src/Marilo.Components/DataDisplay/GanttViewBase.cs` — add template + format parameters
- `src/Marilo.Components/DataDisplay/GanttDayView.cs` (+ Week, Month, Year) — add format defaults
- `src/Marilo.Components/DataDisplay/MariloGantt.razor` — use template/format in header rendering
- `src/Marilo.Components/DataDisplay/MariloGantt.razor.cs` — header generation using formats

**Complexity:** Integration — sonnet model
**Tests:** Add view template tests

### Task C6: Remove Navigable from spec + hover delete button

**Spec change:** Remove `Navigable` from spec examples.
**Code change:** Add delete icon button on bar hover, wire to OnDelete.

**Files to modify:**
- `src/Marilo.Components/DataDisplay/MariloGantt.razor` — add hover button markup
- Spec files with `Navigable` references

**Complexity:** Mechanical — haiku model

---

## Phase D — Accessibility (Tier 4)

### Task D1: Skip navigation links

**Files to modify:**
- `src/Marilo.Components/DataDisplay/MariloGantt.razor` — add sr-only skip links

**Complexity:** Mechanical — haiku model

### Task D2: prefers-reduced-motion

**Files to modify:**
- SCSS file for MariloGantt (find via Glob)

**Complexity:** Mechanical — haiku model

---

## Execution Sequence

```
Phase A: [A1] ──> [A2]                    (spec only, no code)
Phase B: [B1] ──> [B2]                    (state API + spec update)
Phase C: [C1] [C2] [C3] [C4] [C5] [C6]   (sequential via SDD)
Phase D: [D1] [D2]                         (a11y, can run after B)
```

**Total tasks:** 12
**Estimated subagent dispatches:** 12 implementers + 12 spec reviews + 12 quality reviews = 36

**Gate:** After Phase C, run full `dotnet build` + `dotnet test` on Marilo-gantt-rewrite worktree. All tests must pass before Stage 06 validation.
