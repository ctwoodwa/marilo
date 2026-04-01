# MariloDataGrid — Phased Gap Analysis & Implementation Tracker

Generated: 2026-03-30 | Last Updated: 2026-04-01
Spec Source: `/docs/component-specs/grid/` (78 files)

---

## Current Implementation Status

**Estimated API Coverage: ~55–60%**

### Completed (Pass 1 — 2026-03-31)
- Public GridState with OnStateInit/OnStateChanged
- OnRead server-side data binding
- Height/Width parameters
- Row events (OnRowClick, OnRowDoubleClick, OnRowContextMenu, OnRowRender)
- Bindable Page parameter, PageChanged, PageSizeChanged, PageSizes dropdown
- Checkbox selection column, multi-select
- Column: Visible, EditorTemplate, FooterTemplate, OnCellRender
- Rebind() public method
- Navigable parameter (placeholder — no keyboard logic yet)

### Completed (Pass 2 — 2026-03-31)
- Three edit modes: Inline, InCell (double-click), Popup
- All CRUD events: OnAdd, OnCreate, OnUpdate, OnDelete, OnEdit, OnCancel, OnModelInit, OnCommand
- MariloGridCommandButton component (Add/Edit/Save/Cancel/Delete + custom commands)
- GridCommandTypes: GridCommandDefinition, GridCommandPlacement enum
- DetailTemplate with OnRowExpand/OnRowCollapse
- FilterMenu mode with operator dropdown (11 operators) and Apply/Clear
- Extended filter operators: all 11 FilterOperator values with IComparable type-aware comparison
- Multi-sort with Ctrl+Click
- IsLoading overlay
- EnableVirtualization / VirtualizeOverscanCount
- Footer row (`<tfoot>`) with FooterTemplate rendering
- Partial file architecture (Rendering.cs, Data.cs, Editing.cs)

### Architecture
```
MariloDataGrid.razor           — Markup (~240 lines)
MariloDataGrid.razor.cs        — Parameters, state, lifecycle (~300 lines)
MariloDataGrid.Rendering.cs    — RenderTreeBuilder: rows, cells, filter menu (~360 lines)
MariloDataGrid.Data.cs         — Filter/sort/page pipeline, event handlers (~430 lines)
MariloDataGrid.Editing.cs      — CRUD operations, detail expansion (~130 lines)
MariloGridColumn.razor         — Column definition component
MariloGridCommandButton.razor  — Command button component
MariloGridToolbar.razor        — Toolbar container
GridState.cs                   — State persistence model
GridCommandTypes.cs            — Command/edit event arg types
GridEventArgs.cs               — Row/cell/read event arg types
```

---

## Phase A — Pure C# Features (No JS Interop)

*Estimated effort: Medium. All tasks are self-contained Blazor/C# work.*

### A1: Grouping
Spec: `grouping/overview.md`, `grouping/aggregates.md`, `grouping/load-on-demand.md`

- [ ] A1.1 — Add `Groupable` parameter to `MariloGridColumn` (default: true)
- [ ] A1.2 — Implement client-side grouping in `ProcessDataClientSide()` using `GroupDescriptors`
- [ ] A1.3 — Render group header rows with collapse/expand toggle
- [ ] A1.4 — Track collapsed groups in `_collapsedGroups` HashSet
- [ ] A1.5 — Add `GroupHeaderTemplate` RenderFragment<GroupHeaderContext<TItem>> to grid
- [ ] A1.6 — Add `GroupFooterTemplate` RenderFragment<GroupFooterContext<TItem>> to grid
- [ ] A1.7 — Implement aggregate functions (Count, Sum, Average, Min, Max) for group footers
- [ ] A1.8 — Add `CollapsedGroups` to `GridState` for state persistence
- [ ] A1.9 — Support OnRead grouping (pass GroupDescriptors in request, expect pre-grouped data)

### A2: AutoGenerateColumns
Spec: `columns/auto-generated.md`

- [ ] A2.1 — Add `AutoGenerateColumns` bool parameter (default: false)
- [ ] A2.2 — On init, reflect `typeof(TItem)` public properties to auto-create column definitions
- [ ] A2.3 — Respect `[Display]` and `[Editable]` attributes for Title/Editable
- [ ] A2.4 — Allow explicit `MariloGridColumn` children to override auto-generated columns
- [ ] A2.5 — Skip navigation properties and complex types

### A3: SearchBox Filter
Spec: `filtering/searchbox.md`

- [ ] A3.1 — Add `ShowSearchBox` bool parameter
- [ ] A3.2 — Render search input in toolbar area
- [ ] A3.3 — Implement global text search across all string-type visible columns
- [ ] A3.4 — Add `SearchFilter` string to `GridState` for persistence
- [ ] A3.5 — Debounce input (configurable `SearchDelay` parameter, default 300ms)

### A4: Additional Templates
Spec: `templates/no-data.md`, `templates/row.md`

- [ ] A4.1 — Add `NoDataTemplate` RenderFragment parameter
- [ ] A4.2 — Render `NoDataTemplate` when `_displayedItems` is empty (instead of empty table body)
- [ ] A4.3 — Add `RowTemplate` RenderFragment<TItem> parameter for full custom row rendering
- [ ] A4.4 — When `RowTemplate` is set, bypass default cell rendering and use the template

### A5: Column Enhancements
Spec: `columns/bound.md`, `columns/frozen.md`, `columns/visible.md`

- [ ] A5.1 — Add `Editable` bool parameter to `MariloGridColumn` (default: true)
- [ ] A5.2 — Respect `Editable` in Rendering.cs — skip EditorTemplate for non-editable columns
- [ ] A5.3 — Add `HeaderClass` string parameter to `MariloGridColumn`
- [ ] A5.4 — Add `Id` string parameter to `MariloGridColumn`
- [ ] A5.5 — Add `ShowColumnMenu` bool parameter to `MariloGridColumn` (default: false)
- [ ] A5.6 — Add `VisibleInColumnChooser` bool parameter (default: true)

### A6: GridState Enrichment
Spec: `state.md`

- [ ] A6.1 — Add `EditItem` property to GridState
- [ ] A6.2 — Add `OriginalEditItem` property to GridState
- [ ] A6.3 — Add `InsertedItem` property to GridState
- [ ] A6.4 — Add `ExpandedItems` HashSet to GridState
- [ ] A6.5 — Add `ColumnStates` list (order, width, visible) to GridState
- [ ] A6.6 — Add `SearchFilter` string to GridState
- [ ] A6.7 — Add `Skip` int to GridState (for virtual scroll position)
- [ ] A6.8 — Add `TableWidth` string to GridState
- [ ] A6.9 — Sync editing state (_editingItem, _originalItem) to GridState on changes
- [ ] A6.10 — Sync _expandedDetailItems to GridState.ExpandedItems on changes

### A7: Highlighting & Size
Spec: `highlighting.md`, `sizing.md`

- [ ] A7.1 — Add `Size` parameter (enum: Small, Medium, Large) with CSS class mapping
- [ ] A7.2 — Add CSS variables for each size tier (font-size, padding, row-height)
- [ ] A7.3 — Add `HighlightedItems` IEnumerable<TItem> parameter
- [ ] A7.4 — Apply `mar-datagrid-row--highlighted` CSS class to highlighted rows in rendering

### A8: CSV Export
Spec: `export/csv.md`, `export/events.md`

- [ ] A8.1 — Add `ExportToCsv()` public method
- [ ] A8.2 — Generate CSV from visible columns and current data
- [ ] A8.3 — Trigger browser download via JS interop (`URL.createObjectURL` + click)
- [ ] A8.4 — Add `OnBeforeExport` / `OnAfterExport` EventCallback parameters
- [ ] A8.5 — Support export of all data (not just current page) via `ExportAllPages` option

---

## Phase B — JS Interop Features

*Estimated effort: High. All tasks require a shared `marilo-datagrid.js` module and `IJSRuntime` injection.*

### B0: JS Interop Infrastructure
- [ ] B0.1 — Create `wwwroot/js/marilo-datagrid.js` module
- [ ] B0.2 — Register JS module in `MariloDataGrid` via `IJSRuntime.InvokeAsync<IJSObjectReference>("import", ...)`
- [ ] B0.3 — Add `IAsyncDisposable` implementation to dispose JS module reference
- [ ] B0.4 — Add `[Inject] IJSRuntime JS { get; set; }` to MariloDataGrid.razor.cs

### B1: Keyboard Navigation
Spec: `keyboard-navigation.md`

- [ ] B1.1 — Add `@onkeydown` handler to grid root element
- [ ] B1.2 — Track focused cell position (`_focusedRow`, `_focusedCol`)
- [ ] B1.3 — Arrow keys: move focus between cells
- [ ] B1.4 — Enter: begin edit on focused cell (InCell mode) or focused row (Inline mode)
- [ ] B1.5 — Escape: cancel edit
- [ ] B1.6 — Tab/Shift+Tab: move between editable cells
- [ ] B1.7 — Add `aria-activedescendant` for screen reader support
- [ ] B1.8 — Add `tabindex` management for focusable cells
- [ ] B1.9 — Add `CustomKeyboardShortcuts` Dictionary parameter for user-defined shortcuts

### B2: Column Resizing
Spec: `columns/resize.md`

- [ ] B2.1 — Add `Resizable` bool parameter to `MariloGridColumn` (default: false)
- [ ] B2.2 — Add `MinResizableWidth` / `MaxResizableWidth` string parameters
- [ ] B2.3 — Render drag handle element in header cells for resizable columns
- [ ] B2.4 — JS: `initColumnResize(elementRef, dotNetRef)` — mousedown/mousemove/mouseup handlers
- [ ] B2.5 — .NET callback: `OnColumnResized(string field, double newWidth)` updates column width
- [ ] B2.6 — Persist widths in `GridState.ColumnStates`
- [ ] B2.7 — Add `OnColumnResized` EventCallback parameter

### B3: Column Reordering
Spec: `columns/reorder.md`

- [ ] B3.1 — Add `Reorderable` bool parameter to `MariloGridColumn` (default: false)
- [ ] B3.2 — JS: `initColumnReorder(elementRef, dotNetRef)` — HTML5 drag-and-drop on headers
- [ ] B3.3 — .NET callback: `OnColumnReordered(string field, int newIndex)` reorders `_columns` list
- [ ] B3.4 — Persist column order in `GridState.ColumnStates`
- [ ] B3.5 — Add `OnColumnReordered` EventCallback parameter

### B4: Row Drag-and-Drop
Spec: `row-drag-drop.md`

- [ ] B4.1 — Add `RowDraggable` bool parameter
- [ ] B4.2 — JS: `initRowDrag(elementRef, dotNetRef)` — HTML5 drag-and-drop on rows
- [ ] B4.3 — Render drag handle element in rows when enabled
- [ ] B4.4 — .NET callback: `OnRowDropped(int sourceIndex, int destIndex)`
- [ ] B4.5 — Add `OnRowDrop` EventCallback<GridRowDropEventArgs<TItem>>
- [ ] B4.6 — Create `GridRowDropEventArgs<TItem>` (Item, DestinationItem, DestinationIndex, DropPosition)

### B5: Frozen/Locked Columns
Spec: `columns/frozen.md`

- [ ] B5.1 — Add `Locked` bool parameter to `MariloGridColumn` (default: false)
- [ ] B5.2 — Add `Lockable` bool parameter to `MariloGridColumn` (default: true)
- [ ] B5.3 — Apply `position: sticky` and calculated `left`/`right` offsets via JS measurement
- [ ] B5.4 — Add CSS class `mar-datagrid-col--locked` with z-index layering
- [ ] B5.5 — JS: calculate and set left offset for each locked column on resize/reorder

---

## Phase C — Advanced Features (May Require External Dependencies)

*Estimated effort: High. Some features need NuGet packages or complex UI.*

### C1: Excel Export
Spec: `export/excel.md`
Dependency: ClosedXML (MIT) or similar

- [ ] C1.1 — Add ClosedXML NuGet reference (MIT license)
- [ ] C1.2 — Add `ExportToExcel()` public method
- [ ] C1.3 — Generate .xlsx from visible columns and data
- [ ] C1.4 — Trigger browser download via JS interop
- [ ] C1.5 — Respect Format strings for cell formatting in Excel

### C2: PDF Export
Spec: `export/pdf.md`
Dependency: QuestPDF (MIT) or similar

- [ ] C2.1 — Add QuestPDF NuGet reference (MIT license)
- [ ] C2.2 — Add `ExportToPdf()` public method
- [ ] C2.3 — Generate table layout matching grid columns
- [ ] C2.4 — Trigger browser download via JS interop

### C3: Column Menu
Spec: `columns/menu.md`

- [ ] C3.1 — Create `MariloGridColumnMenu` component (dropdown popup)
- [ ] C3.2 — Render menu trigger button in header when `ShowColumnMenu` is true
- [ ] C3.3 — Menu items: Sort Ascending, Sort Descending, Clear Sort, Filter, Lock/Unlock
- [ ] C3.4 — Add click-outside-to-close behavior (JS interop)

### C4: Column Chooser
Spec: `templates/column-chooser.md`

- [ ] C4.1 — Create `MariloGridColumnChooser` component (dialog/popup)
- [ ] C4.2 — List all columns with `VisibleInColumnChooser == true`
- [ ] C4.3 — Checkbox per column to toggle `Visible`
- [ ] C4.4 — Add toolbar button or API to open column chooser
- [ ] C4.5 — Add `ColumnChooserTemplate` RenderFragment parameter

### C5: CheckBoxList Filter Mode
Spec: `filtering/checkboxlist.md`

- [ ] C5.1 — Add `GridFilterMode.CheckBoxList` enum value
- [ ] C5.2 — Render distinct values as checkbox list in filter popup
- [ ] C5.3 — Apply multi-value filter (IN operator semantics)
- [ ] C5.4 — Support async loading of distinct values for large datasets

### C6: Multi-Column Headers
Spec: `columns/multi-column-headers.md`, `columns/stacked.md`

- [ ] C6.1 — Create `MariloGridColumnGroup` component
- [ ] C6.2 — Render `<thead>` with multiple `<tr>` rows for nested headers
- [ ] C6.3 — Calculate `colspan` for parent header cells
- [ ] C6.4 — Support arbitrary nesting depth

### C7: Cell Selection
Spec: `selection/cells.md`

- [ ] C7.1 — Add `GridSelectionMode.Cell` enum value
- [ ] C7.2 — Track `_selectedCells` as HashSet<(TItem, string)>
- [ ] C7.3 — Add `SelectedCells` / `SelectedCellsChanged` parameters
- [ ] C7.4 — Render `aria-selected` on selected cells
- [ ] C7.5 — Support Ctrl+Click for multi-cell selection

### C8: Editing Validation
Spec: `editing/validation.md`

- [ ] C8.1 — Wrap inline/popup editors in `EditForm` with `DataAnnotationsValidator`
- [ ] C8.2 — Show `ValidationMessage` per field
- [ ] C8.3 — Block `SaveEdit()` when validation fails
- [ ] C8.4 — Add `ValidationMessageTemplate` RenderFragment parameter

---

## Phase D — Future / Out of Scope

*These are documented in the spec but have low priority or require significant infrastructure.*

### D1: AI Features
Spec: `ai/` (9 files)
*Requires AI service integration, out of scope for core grid.*

- [ ] D1.1 — Define `IMariloAIService` interface
- [ ] D1.2 — AI Column (auto-fill suggestions)
- [ ] D1.3 — AI Highlight (anomaly detection)
- [ ] D1.4 — AI Search (natural language query)
- [ ] D1.5 — AI Smart Box (contextual actions)

### D2: Popup Editing Templates
Spec: `templates/popup-form.md`, `templates/popup-buttons.md`

- [ ] D2.1 — Add `PopupFormTemplate` RenderFragment<TItem> parameter
- [ ] D2.2 — Add `PopupButtonsTemplate` RenderFragment parameter
- [ ] D2.3 — Add popup width/height configuration

### D3: Pager Template
Spec: `templates/pager.md`

- [ ] D3.1 — Add `PagerTemplate` RenderFragment parameter
- [ ] D3.2 — Provide pager context (CurrentPage, TotalPages, PageSize) to template

### D4: Toolbar Built-in Tools
Spec: `toolbar/toolbar.md` (13 tools)

- [ ] D4.1 — Create individual `GridToolBar*` components (Add, Search, Export, ColumnChooser, etc.)
- [ ] D4.2 — Wire each tool to corresponding grid action
- [ ] D4.3 — Support custom toolbar item positioning

### D5: Advanced Column Types
Spec: `columns/virtual.md`, `columns/checkbox.md`

- [ ] D5.1 — Add `FilterEditorType` / `FilterEditorFormat` parameters
- [ ] D5.2 — Add `FieldType` parameter for ExpandoObject / DataTable support
- [ ] D5.3 — Add `FilterCellTemplate` / `FilterMenuTemplate` parameters
- [ ] D5.4 — Virtual column (non-data-bound computed column)
- [ ] D5.5 — GridCheckboxColumn dedicated component

### D6: AdaptiveMode
Spec: `overview.md`

- [ ] D6.1 — Add `AdaptiveMode` enum parameter
- [ ] D6.2 — Switch to card layout on narrow viewports
- [ ] D6.3 — JS: viewport size detection and breakpoint management

---

## Progress Summary

| Phase | Total Tasks | Completed | Status |
|-------|------------|-----------|--------|
| A — Pure C# | 42 | 0 | Not Started |
| B — JS Interop | 28 | 0 | Not Started |
| C — Advanced | 29 | 0 | Not Started |
| D — Future | 21 | 0 | Not Started |
| **Total** | **120** | **0** | — |

## Session Log

| Date | Session | Phase/Tasks | Notes |
|------|---------|-------------|-------|
| 2026-03-31 | Pass 1 | Pre-phase | 16 grid gaps + 4 column gaps + 1 toolbar gap resolved |
| 2026-03-31 | Pass 2 | Pre-phase | Editing modes, DetailTemplate, FilterMenu, loading, footer, multi-sort |
| | | | |

*Update this table at the start/end of each session to track progress across iterations.*
