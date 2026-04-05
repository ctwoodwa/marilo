# Component Requirements: MariloAllocationScheduler

## Component Identity

- **Name:** MariloAllocationScheduler
- **Category:** DataDisplay (no dedicated Scheduling subfolder exists under Marilo.Components)
- **Complexity:** Complex (multiple partials, child components, core service integration, JS interop)
- **JS Interop:** Yes (drag-fill, cell selection, keyboard traversal)
- **Reference patterns:** MariloDataGrid (grid rendering, column model), MariloDataSheet (cell selection, range interaction, spreadsheet-style editing)
- **Distinction:** This is NOT MariloScheduler. MariloScheduler handles calendar/time-based scheduling. MariloAllocationScheduler handles resource allocation -- assigning task-linked hours or budget across a navigable timeline with spreadsheet-style editing.

## Description

A resource-centric planning component that combines configurable resource metadata columns on the left with editable time-bucket allocation cells on the right. Users assign, redistribute, and analyze task-linked hours or currency values across a navigable timeline with spreadsheet-style editing. Business logic for validation, authorization, undo/redo, and change tracking is provided by Marilo.Core.BusinessLogic building blocks.


## Primary Use Cases

1. **Weekly project staffing** -- Add resources to a project, assign hours by week, navigate across configurable date ranges. Each cell value is an `AllocationRecord` linking a resource, task, time bucket, and value.
2. **Budget planning** -- Switch ValueMode to Currency to plan cost allocations per resource and task. Compare planned cost against target totals.
3. **Bulk editing** -- Select a range of cells via click-drag or keyboard, apply a single value across the selection. Drag-fill to broadcast or increment values across cells.
4. **Reallocation** -- Move planned effort from one resource to another, or from one task to another, using context menu commands.
5. **Schedule shifting** -- Shift a task's allocations forward or backward by N periods using context menu commands.
6. **Variance review** -- Set desired totals via AllocationTarget records, enable ShowDeltas, highlight over-allocated and under-allocated states.
7. **Read-only rollup navigation** -- When ViewGrain is coarser than AuthoritativeLevel, cells display aggregate sums and are not directly editable. Distribution commands allow explicit breakdown.
8. **Scenario planning** -- Create named allocation sets (baselines and scenarios), compare differences via diff overlay, promote a scenario to become the new baseline.


## Domain Model

| Entity | Purpose |
|---|---|
| `Resource` (TResource) | The schedulable entity -- person, role, team, machine. Drives the left-side resource columns. |
| `Task` | The work item receiving the allocation. A resource can have multiple tasks. |
| `AllocationRecord` | A single value record: one resource, one task, one time bucket. The authoritative, persisted unit of data. |
| `AllocationTarget` | A desired total for delta analysis. Stored separately; never mutates allocation records. |
| `AllocationSet` | A named collection of allocations -- either Baseline (locked) or Scenario (divergent branch). |
| `ScenarioOverride` | A delta record for a scenario -- modifies, adds, or tombstones a baseline allocation. |


## User Interactions

- **Single-cell edit:** Click a cell at AuthoritativeLevel zoom and type a value.
- **Drag-fill:** Click-and-drag across cells to broadcast or increment values.
- **Range selection:** Click and drag to select a rectangle; apply a bulk value.
- **Keyboard traversal:** Tab moves right, Shift+Tab moves left, Enter moves down, Arrow keys navigate cells.
- **Context menu:** Right-click for transformation commands (set, clear, shift, move, spread, distribute).
- **Timeline navigation:** Forward/back buttons, jump-to-today, programmatic NavigateTo.
- **Zoom change:** Switch ViewGrain between Day/Week/Month/Quarter/Year.
- **Scenario switching:** Click scenario chips in the Scenario Strip.
- **Undo/redo:** Ctrl+Z / Ctrl+Y at cell level and range level.


## Visual States

| State | Description |
|---|---|
| Default | Resource rows populated, time-bucket cells empty or filled with values |
| Allocated cell | Cell contains an AllocationRecord value (hours or currency) |
| Over-allocated | Resource total exceeds capacity or target; highlighted with warning color |
| Under-allocated | Resource total below target; subtle indicator |
| On-target | Resource actual matches target within tolerance |
| Selected cell | Single cell has focus/selection highlight |
| Selected range | Multiple cells selected via drag or Shift+click |
| Disabled slot | Time bucket not available (holiday, non-working time) -- shaded |
| Drag-fill in progress | Cells being drag-filled show preview values |
| Read-only rollup | Cells at coarser-than-authoritative zoom -- displayed as aggregates, not editable |
| Conflict/validation error | Cell value violates a business rule |
| Loading | Async data operations in progress |
| Empty state | No allocations bound |
| Scenario diff overlay | Baseline ghost bars behind active scenario values |
| Locked baseline | Entire baseline set is read-only; lock icon in scenario strip |


## Events

| Event | Args Type | When it fires |
|---|---|---|
| OnCellEdited | CellEditedArgs | Single cell value committed |
| OnRangeEdited | RangeEditedArgs | Bulk range edit committed |
| OnContextMenuAction | ContextMenuActionArgs | Built-in or custom context menu command invoked |
| OnDistributeRequested | DistributeArgs | Distribution command initiated; host can intercept |
| OnShiftValues | ShiftValuesArgs | Shift-forward or shift-backward confirmed |
| OnMoveValues | MoveValuesArgs | Move-to-task or move-to-resource confirmed |
| OnTargetChanged | TargetChangedArgs | Desired total set or updated via context menu |
| OnVisibleRangeChanged | VisibleRangeChangedArgs | User navigates to a different date range |
| OnSelectionChanged | SelectionChangedArgs | User changes selected cell or range |
| OnScenarioChanged | ScenarioChangedArgs | User switches active scenario in Scenario Strip |
| OnScenarioCreated | ScenarioCreatedArgs | New scenario created from the strip |
| OnAllocationOverridden | AllocationOverriddenArgs | Edit in a scenario produces a new or updated override |
| OnScenarioStatusChanged | ScenarioStatusChangedArgs | Scenario status changes (Draft to Shared, etc.) |
| OnScenarioPromoted | ScenarioPromotedArgs | Scenario promoted to become new baseline |
| CanExecuteAction | CanExecuteActionArgs | Called before context menu action shown; enable/disable logic |


## Composition

### Component Hierarchy

- **MariloAllocationScheduler** (root) -- manages state, renders resource grid + timeline surface
  - **AllocationResourceColumn** (child, declared in markup) -- configures left-side resource metadata columns
  - Uses `CascadingValue` to pass parent reference to child columns

### RenderFragment Slots

- `AllocationResourceColumns` -- RenderFragment for declaring `AllocationResourceColumn` children
- `ToolbarTemplate` -- optional custom toolbar content
- `EmptyTemplate` -- custom content when no allocations are bound
- `CellTemplate` -- optional per-cell rendering customization
- `ResourceRowTemplate` -- optional custom resource row header rendering


## Core Integration

The component consumes these building blocks from `src/Marilo.Core/BusinessLogic/`:

| Block | How it is used |
|---|---|
| `BusinessObjectBase<T>` | Consumer-side AllocationEntry business object inherits from this. Component surfaces IsDirty, BrokenRules, CanUndo/Redo. |
| `FieldManager` | Backing store for allocation properties. Dirty tracking for change detection. |
| `BusinessRuleEngine` | Registered validation rules (ValueNonNegative, CapacityCheck, etc.) run on SetProperty. |
| `AuthorizationEngine` | Most-restrictive-wins authorization composition. CanReadProperty/CanWriteProperty drive cell editability and visibility. |
| `UndoStack` | BeginEdit/CancelEdit/ApplyEdit for cell-level and range-level undo/redo. |

Existing enums in `src/Marilo.Core/BusinessLogic/Enums/BusinessLogicEnums.cs`:
- `ScenarioStatus` (Draft, Shared, Approved, Promoted, Rejected)
- `AllocationSetType` (Baseline, Scenario)
- `AccessMode` (None, ReadOnly, ReadWrite)
- `AuthorizationAction` (Read, Write)
- `RuleOutcome` (Valid, Broken)

**Critical constraint:** Do NOT re-implement logic that already exists in core. The component wraps and consumes these contracts.


## Accessibility

| Aspect | Specification |
|---|---|
| ARIA role (outer) | `role="grid"` on the root element |
| Resource rows | `role="row"` on each resource row |
| Slot cells | `role="gridcell"` on each time-bucket cell |
| Header cells | `role="columnheader"` on time-bucket headers and resource column headers |
| aria-selected | Applied to selected cells and selected range |
| aria-disabled | Applied to non-editable rollup cells and disabled slots |
| aria-readonly | Applied to read-only rollup cells |
| aria-label | Descriptive label per resource row and per cell (e.g., "Alice Chen, Week of Apr 6, 32 hours") |
| aria-live | Polite announcements for value changes and conflict detection |
| Keyboard: Arrow keys | Navigate between cells |
| Keyboard: Tab / Shift+Tab | Move right / left between cells |
| Keyboard: Enter | Move down / begin edit |
| Keyboard: Escape | Cancel current edit / exit selection |
| Keyboard: Delete | Clear selected cell(s) |
| Keyboard: Ctrl+Z / Ctrl+Y | Undo / redo |
| Focus management | Focus ring visible on active cell; focus trapped within grid during keyboard navigation |
| Screen reader | Announce cell value, resource name, and time bucket when cell receives focus |


## Theme Considerations

| Property | FluentUI | Bootstrap |
|---|---|---|
| Slot separators | Fluent border tokens (subtle borders) | Bootstrap table border utilities |
| Conflict/over-allocated highlight | Fluent semantic error color token (`--colorStatusDangerBackground2`) | Bootstrap `danger` color |
| Under-allocated indicator | Fluent semantic warning token | Bootstrap `warning` color |
| On-target indicator | Fluent semantic success token | Bootstrap `success` color |
| Selected cell | Fluent selected background token (`--colorNeutralBackground1Selected`) | Bootstrap `primary` background with opacity |
| Selected range | Fluent selection highlight with border | Bootstrap primary outline |
| Disabled slot shading | Fluent disabled background token | Bootstrap `bg-light` with reduced opacity |
| Drag-fill preview | Fluent accent background with opacity | Bootstrap primary with opacity |
| Scenario strip chips | Fluent chip/badge tokens | Bootstrap badge classes |
| Ghost baseline bars | Fluent dashed border with reduced opacity | Bootstrap dashed border utilities |
| Font family | Fluent type ramp | Bootstrap native font stack |
| Spacing | Fluent spacing tokens | Bootstrap spacing utilities |
| Read-only rollup cells | Fluent subtle background | Bootstrap muted text on light background |


## Enumerations Needed

| Enum | Values | Purpose |
|---|---|---|
| TimeGranularity | Day, Week, Month, Quarter, Year | View grain and authoritative level |
| AllocationValueMode | Hours, Currency | Whether cells display hours or currency |
| DistributionMode | EvenSpread, ProportionalToExisting, FrontLoaded, BackLoaded, WorkingDaysWeighted, Custom | Policy for distributing higher-level values to sub-buckets |
| AllocationSelectionMode | None, Cell, Range | Selection behavior |
| DeltaDisplayMode | Value, Percentage, StatusIcon | How variance is displayed |
| AllocationSetType | Baseline, Scenario | Already exists in BusinessLogicEnums.cs |
| AllocationScenarioStatus | Draft, Shared, Approved, Promoted, Rejected | Maps to existing ScenarioStatus in BusinessLogicEnums.cs |


## File Structure (Anticipated)

```
src/Marilo.Core/Enums/
  AllocationSchedulerEnums.cs        (TimeGranularity, AllocationValueMode, DistributionMode, etc.)

src/Marilo.Core/Models/
  AllocationSchedulerModels.cs       (EventArgs classes)

src/Marilo.Core/Contracts/
  IMariloCssProvider.cs              (add AllocationScheduler methods)

src/Marilo.Components/DataDisplay/AllocationScheduler/
  MariloAllocationScheduler.razor
  MariloAllocationScheduler.razor.cs
  AllocationResourceColumn.razor.cs

src/Marilo.Components/wwwroot/js/
  allocation-scheduler.js            (drag-fill, cell selection, keyboard traversal)

src/Marilo.Providers.FluentUI/
  FluentUICssProvider.cs             (add method implementations)
  Styles/_allocation-scheduler.scss

src/Marilo.Providers.Bootstrap/
  BootstrapCssProvider.cs            (add method implementations)
  Styles/_bridge-allocation-scheduler.scss

samples/Marilo.Demo/Pages/Components/AllocationScheduler/
  AllocationSchedulerDemo.razor

docs/component-specs/allocation-scheduler/
  (update existing docs as needed)

tests/Marilo.Tests.Unit/AllocationScheduler/
  MariloAllocationSchedulerTests.cs
```
