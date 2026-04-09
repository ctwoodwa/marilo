# MariloGantt Resolution Designs — Stage 03

**Date:** 2026-04-09
**Decisions incorporated:**
- Dependency model: plan richer `GanttDependencies` component (don't simplify spec)
- State API: design minimal subset now (ExpandedItems, Sort, Filter + OnStateChanged)
- Timeline drag/resize: JS interop deferred to P3

---

## Tier 1 — Spec Corrections (12 items, spec-only changes)

### RES-T1-01: Rename TreeListWidth → TaskListWidth

**Files:** `gantt-tree/overview.md`, `refresh-data.md`, `state.md`, all code examples referencing `TreeListWidth`
**Change:** Find-replace `TreeListWidth` → `TaskListWidth`. Change type from `string` to `int` where documented. Update default from `"30"` to `250`.
**Rationale:** Telerik-naming-is-canonical decision does NOT apply here — `TaskListWidth` is the Marilo-specific name chosen in the rewrite. The spec imported Telerik's `TreeListWidth` but the source diverged intentionally.

### RES-T1-02: Rename OnEdit → OnTaskEdit

**Files:** `events.md`
**Change:** Rename `OnEdit` to `OnTaskEdit` in parameter table and prose. Update event args reference.
**Note:** `OnTaskClick` is the companion event — both use the `OnTask*` prefix pattern.

### RES-T1-03: Normalize TooltipTemplate casing

**Files:** `overview.md` (has `ToolTipTemplate` with capital T)
**Change:** Replace `ToolTipTemplate` → `TooltipTemplate` to match source and other spec pages.

### RES-T1-04: Fix GanttColumn.Visible type

**Files:** `gantt-tree/columns/visible.md`, `gantt-tree/columns/bound.md`
**Change:** Type from `bool?` → `bool`, default from `null (treated as true)` → `true`.

### RES-T1-05: Fix RangeStart/RangeEnd nullability

**Files:** `timeline/views.md`
**Change:** Parameter table type from `DateTime` → `DateTime?`. Add note: "When null, auto-calculated from data source."

### RES-T1-06: Update TooltipTemplate to RenderFragment&lt;TItem&gt;

**Files:** `timeline/templates/tooltip.md`
**Change:** Replace `RenderFragment<object>` + `TooltipTemplateContext` cast pattern with `RenderFragment<TItem>` strongly typed. Remove `TooltipTemplateContext` class documentation. Update code examples to use `@context.Title` directly (no cast needed).
**Note:** This is a spec-to-source alignment. The `TooltipTemplateContext` class is NOT planned for implementation.

### RES-T1-07: Clarify TaskTemplate scope

**Files:** `timeline/templates/task.md`
**Change:** Add note: "TaskTemplate controls the inner content of the timeline bar (replacing the default progress fill). It does not replace the bar container itself." Update any examples that imply full bar override.

### RES-T1-08: Update dependency data binding (partial)

**Files:** `dependencies/databind.md`, `dependencies/overview.md`
**Change:** Add a "Current Implementation" section documenting the `DependsOnField` approach as the interim API. Keep the `GanttDependencies` component model as "Target Architecture — Planned" with a migration guide placeholder.
**Rationale:** Per decision, the richer model IS the target. Don't delete it. But document what works today.

### RES-T1-09: Fix sorting/filter behavior descriptions

**Files:** `gantt-tree/sorting.md`, `gantt-tree/filter/filter-row.md`
**Sorting change:** Rewrite behavior to describe tri-state single-column sort (asc → desc → clear). Remove `SortMode.Multiple` reference. Keep as "Planned" note.
**Filter change:** Document auto-expand behavior. Fill in empty "Debouncing" and "Configuring" sections with "Planned" placeholder.

### RES-T1-10: Update timeline ARIA roles

**Files:** `accessibility/wai-aria-support.md`
**Change:** Replace `role="tree"` / `role="treeitem"` for timeline bars with `role="img"` + `aria-label="{title}: {start} – {end}"`. Remove `aria-level` from bar elements (it belongs on treegrid rows).

### RES-T1-11: Clarify View/ViewChanged binding

**Files:** `timeline/views.md`
**Change:** Add note that `@bind-View` works because `View` + `ViewChanged` follow Blazor's standard two-way binding convention. No mismatch exists.

### RES-T1-12: Add SlotWidth defaults to parameter table

**Files:** `timeline/views.md`
**Change:** Add Defaults column: Day=40, Week=40, Month=60, Year=80.

---

## Tier 2 — Document Undocumented Source Features (12 items, spec-only)

### RES-T2-01: Document OnTaskClick and ViewChanged events

**Files:** `events.md`
**Add:** `OnTaskClick: EventCallback<TItem>` (fires on row click) and `ViewChanged: EventCallback<GanttView>` (fires on view switch). Include usage examples.

### RES-T2-02: Document field mapping parameters

**Files:** `gantt-tree/data-binding/overview.md` or `refresh-data.md`
**Add:** Table documenting all 7 field parameters: `IdField`, `ParentIdField`, `TitleField`, `StartField`, `EndField`, `PercentCompleteField`, `DependsOnField` with types (all `string`), defaults, and a code example showing custom property names.

### RES-T2-03: Document OnExpand/OnCollapse/OnCreate events

**Files:** `events.md`
**Add:** Full parameter tables for `OnExpand: EventCallback<GanttExpandEventArgs>`, `OnCollapse: EventCallback<GanttCollapseEventArgs>`, and expand `OnCreate: EventCallback<GanttCreateEventArgs>` with `Item` and `ParentItem` properties.

### RES-T2-04: Document RowHeight and GanttToolBarTemplate

**Files:** `overview.md` or `gantt-tree/overview.md`
**Add:** `RowHeight: int` (default 36) and `GanttToolBarTemplate: RenderFragment?` to parameter tables.

### RES-T2-05: Add Filterable/Sortable to column parameter table

**Files:** `gantt-tree/columns/bound.md`
**Add:** `Filterable: bool` (default true) and `Sortable: bool` (default true) to the bound column parameter table.

### RES-T2-06: Document DayWidth legacy and GanttViews fallback

**Files:** `timeline/views.md`
**Add:** Note documenting `DayWidth: int` (default 30) as legacy fallback when no `GanttViews` children registered. Document fallback behavior and exception conditions.

### RES-T2-07: Document DependsOnField

**Files:** `dependencies/databind.md` (see RES-T1-08 — "Current Implementation" section)
**Add:** `DependsOnField: string` parameter with example showing `List<int> DependsOn` property pattern.

### RES-T2-08: Document accessibility features (aria-sort, tabindex, labels)

**Files:** `accessibility/wai-aria-support.md`, `accessibility/overview.md`
**Add:** `aria-sort` on column headers (ascending/descending/none), treegrid `tabindex="0"` + roving focus, chevron `aria-label="Expand/Collapse {title}"`, timeline bar `aria-label="{title}: {start} – {end}"`.

### RES-T2-09: Document keyboard navigation details

**Files:** `accessibility/overview.md`
**Add:** Full keyboard interaction table: ArrowUp/Down (move focus), ArrowRight (expand or move down), ArrowLeft (collapse or focus parent), Home/End (first/last), Enter/Space (invoke click), edit mode Enter (save), Escape (cancel).

### RES-T2-10: Document RowHeight and ViewChanged in timeline spec

**Files:** `timeline/overview.md`
**Add:** `RowHeight` affects timeline bar vertical positioning. `ViewChanged` fires when user switches views.

### RES-T2-11: Document Rebind() in state spec

**Files:** `state.md`
**Add:** `Rebind()` method documentation — clears cached data, rebuilds tree, recomputes timeline, calls StateHasChanged.

### RES-T2-12: Document auto-detection and Rebind timeline recomputation

**Files:** `refresh-data.md`
**Add:** OnParametersSet detects both Data reference changes AND field parameter changes. Rebind() also triggers timeline recomputation.

---

## Tier 3 — Minimal State API Design (new implementation)

### RES-T3-STATE: Minimal GanttState&lt;TItem&gt;

**Design:** Implement a subset of the spec's state system wired to existing internal state.

**Phase 1 (this phase):**

```csharp
public class GanttState<TItem> where TItem : class
{
    public IReadOnlyList<SortDescriptor>? SortDescriptors { get; set; }
    public IReadOnlyList<IFilterDescriptor>? FilterDescriptors { get; set; }
    public IReadOnlyCollection<object>? ExpandedItems { get; set; }
    public GanttView? View { get; set; }
}
```

**Events:**
- `OnStateInit: EventCallback<GanttStateEventArgs<TItem>>` — fires in OnInitialized, allows loading saved state
- `OnStateChanged: EventCallback<GanttStateEventArgs<TItem>>` — fires from `SortBy()`, `OnFilterInput()`, `ToggleExpanded()`, `SwitchView()` with `PropertyName` indicating which changed

**Methods:**
- `GetState(): GanttState<TItem>` — snapshots current internal state into a state object
- `SetStateAsync(GanttState<TItem>? state): Task` — applies state; null resets to defaults

**Wiring to internals:**
- `SortDescriptors` → read from `_sortField` + `_sortAscending`; write applies sort
- `FilterDescriptors` → read from `_filterValues` dict; write applies filters
- `ExpandedItems` → read from `_expandedIds` HashSet; write replaces expanded set
- `View` → read from `View` parameter; write calls `SwitchView()`

**Phase 2 (next phase):**
- Add `EditItem`, `OriginalEditItem`, `InsertedItem`, `EditField`, `ParentItem`
- Add `TreeListWidth` (when splitter resize is implemented)
- Add `ColumnStates` (when column reorder/resize is implemented)

### RES-T3-DEPS: Dependency Component Architecture

**Design:** Plan the richer model without implementing this phase.

**Target architecture:**
```razor
<MariloGantt Data="@Tasks" ...>
    <GanttDependenciesSettings>
        <GanttDependencies Data="@Dependencies"
                           IdField="Id"
                           PredecessorIdField="PredecessorId"
                           SuccessorIdField="SuccessorId"
                           TypeField="Type" />
    </GanttDependenciesSettings>
</MariloGantt>
```

**Migration path from current `DependsOnField`:**
1. Keep `DependsOnField` as a simple-mode shortcut (like DataGrid's flat vs OnRead)
2. When `GanttDependencies` component is present, it takes precedence
3. Internal `ComputeDependencyLines()` reads from whichever source is active
4. `GanttDependencyType` enum: FinishToStart (default), FinishToFinish, StartToStart, StartToFinish
5. Line rendering updated to respect dependency type for anchor points

**This phase:** Update spec with "Current Implementation" + "Target Architecture" sections. No code changes to dependency system.

---

## Tier 3 P2 — Feature Implementations (11 items)

### RES-T3-CMD: GanttCommandColumn

**Design:** Follow `GanttColumn` registration pattern. `GanttCommandColumn` registers with parent Gantt, renders buttons (Add/Edit/Delete/Save/Cancel) per row based on edit state. Wire to existing `BeginEdit`/`CommitEdit`/`CancelEdit` + `OnCreate`/`OnDelete` events.
**Prerequisite:** None — editing infrastructure exists.

### RES-T3-EDITMODE: TreeListEditMode enum

**Design:** Add `TreeListEditMode` parameter (Inline only this phase). Mark Incell and Popup as Planned in spec. Current dbl-click editing becomes the `Inline` mode.
**Scope:** Parameter + enum definition. No behavior change needed — current behavior IS inline mode.

### RES-T3-SORT: Sortable parameter + SortMode

**Design:** Add `Sortable: bool` (default true) on MariloGantt to globally enable/disable. Keep single-column sort. Mark `SortMode.Multiple` as Planned.
**Wire:** `Sortable` check in `SortBy()` method.

### RES-T3-FILTER: FilterMode parameter

**Design:** Add `FilterMode: GanttFilterMode` (FilterRow only this phase). Mark FilterMenu as Planned. Add `FilterRowDebounceDelay: int` parameter.
**Wire:** `FilterMode` check before rendering filter row. Debounce via `CancellationTokenSource` pattern (same as MultiSelect).

### RES-T3-PCT: Percent-complete bar + drag handle

**Design:** Add progress fill div inside timeline bar, width = `PercentComplete%`. Optional drag handle below bar for mouse-based adjustment. Fire `OnUpdate` with updated percent.
**Prerequisite:** `PercentCompleteField` already mapped by `GanttFieldAccessor`.

### RES-T3-DATEHDR: Date header templates

**Design:** Add `RenderFragment<DateTime>?` template parameters to each concrete view class. When set, template overrides the default label. Also add string format parameters (`DayHeaderDateFormat`, etc.) as lighter alternative.
**Wire:** In `GenerateSlots()` / `GenerateMainHeaders()`, use template or format when available.

### RES-T3-ONEDIT: Cancellable OnEdit event

**Design:** Add `OnEdit: EventCallback<GanttEditEventArgs>` with `IsCancelled` property. Fire before `BeginEdit()`. If cancelled, skip edit mode entry.
**Pattern:** Same as `PopupEventArgs` cancellation pattern.

### RES-T3-NEWROW: NewRowPosition

**Design:** Add `NewRowPosition: GanttTreeListNewRowPosition` (Top/Bottom, default Top). Wire into add-new-item logic.

### RES-T3-EDTYPE: EditorType per column

**Design:** Add `EditorType: GanttTreeListEditorType?` on `GanttColumn`. When set, overrides automatic input type detection in edit mode.
**Scope:** Parameter + enum. Existing `GetInputType()` method checks `EditorType` first, falls back to reflection.

### RES-T3-NAV: Remove Navigable from spec

**Design:** Keyboard nav is always on. Remove `Navigable` parameter from spec examples. Add note: "Keyboard navigation is built-in and always active."

### RES-T3-HOVER: Hover delete button + popup edit from bar

**Deferred.** Popup editing requires the full popup system (GanttPopupEditSettings). Mark as Planned. Hover delete button can be added independently — add delete icon on bar hover, wire to `OnDelete`.

---

## Tier 4 — Missing A11y Features

### RES-T4-SKIPNAV: Skip navigation links

**Design:** Add visually-hidden skip links at top of Gantt: "Skip to task list" + "Skip to timeline". Target anchors on `.mar-gantt__tasklist` and `.mar-gantt__container > div:nth-child(2)`.

### RES-T4-MOTION: prefers-reduced-motion

**Design:** Add `@media (prefers-reduced-motion: reduce)` block to Gantt SCSS disabling transitions/animations.

### RES-T4-DRAG: Screen reader drag announcements

**Deferred** (drag itself is deferred).

### RES-T4-CONTRAST: High-contrast mode

**Deferred to next phase.** Add `@media (prefers-contrast: more)` when theme system supports it.
