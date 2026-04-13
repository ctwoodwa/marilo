# DataGrid Gap Analysis — Stage 01 Intake

**Intake date:** 2026-04-11
**Component:** `MariloDataGrid<TItem>`
**Source gap list:** `ICM/workspaces/datagrid-delivery/stages/01-spec-review/output/datagrid-spec-gap-list.md`
**Scope:** Spec-ahead gaps only (S-01..S-17, SA-01..SA-14). Spec-update-only items (U-*, M-*, NM-*, SRC-*) are handled by the delivery workspace, not here.

---

## Phase A — P1 Blocking (Coordinator Escalation Required)

These gaps cannot be resolved in the gap-analysis workspace without a coordinator naming/API decision. They are registered here for tracking but blocked pending escalation resolution.

---

## GAP-DATAGRID-S04

**Source gap ID:** S-04
**Priority:** P1
**Category:** spec-ahead
**Summary:** `<GridColumns>` wrapper element required by spec; source takes columns as direct `ChildContent`
**Spec reference:** `docs/component-specs/grid/` — column examples use `<GridColumns>…</GridColumns>` wrapper throughout; e.g. `columns/bound.md`
**Source state:** `MariloDataGrid` accepts column children via `ChildContent` (`RenderFragment`); no `<GridColumns>` wrapper component exists
**Target state:** Introduce `<GridColumns>` wrapper component, or update all spec examples to use direct `ChildContent` (coordinator decision required — affects shared markup contract and every spec example)
**Blocking:** yes — every spec code example fails to compile/render without this alignment
**Dependencies:** M-01 (component name), M-02 (column element name) — all three must be resolved together

---

## GAP-DATAGRID-S05

**Source gap ID:** S-05
**Priority:** P1
**Category:** spec-ahead
**Summary:** `GridCommandColumn` + `GridCommandButton` built-in command-column scheme — partial implementation only
**Spec reference:** `docs/component-specs/grid/columns/command.md`
**Source state:** `MariloGridCommandButton.razor` and `GridCommandTypes.cs` exist; no dedicated `GridCommandColumn` element
**Target state:** Implement `GridCommandColumn` Razor component that hosts `GridCommandButton` instances, with built-in `Edit`, `Save`, `Cancel`, `Delete` commands
**Blocking:** yes — spec command column pattern is not functional without `GridCommandColumn`
**Dependencies:** S-04 (column wrapper), M-01 (component name)

---

## Phase B — P2 Core Gaps (This Resolution Phase)

---

## GAP-DATAGRID-S02

**Source gap ID:** S-02
**Priority:** P2
**Category:** spec-ahead
**Summary:** `Class` CSS parameter missing from `MariloDataGrid`
**Spec reference:** `docs/component-specs/grid/overview.md` — Grid Parameters table lists `Class` as `string`
**Source state:** No `Class` parameter on `MariloDataGrid.razor.cs`
**Target state:** Add `[Parameter] public string? Class { get; set; }` on `MariloDataGrid`; render it on the root `<div class="k-grid ...">` element
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-S06

**Source gap ID:** S-06
**Priority:** P2
**Category:** spec-ahead
**Summary:** 13 built-in toolbar tool components (`GridToolBarAddTool`, `GridToolBarSaveEditTool`, `GridToolBarCancelEditTool`, `GridToolBarDeleteTool`, `GridToolBarEditTool`, `GridToolBarCsvExportTool`, `GridToolBarExcelExportTool`, `GridToolBarSearchBoxTool`, `GridToolBarFilterTool`, `GridToolBarSortTool`, `GridToolBarGroupTool`, `GridToolBarSelectAllTool`, `GridToolBarSpacerTool`, `GridToolBarCustomTool`) are documented but not implemented as discrete Razor components
**Spec reference:** `docs/component-specs/grid/toolbar.md` — Built-In Tools table
**Source state:** `MariloGridToolbar.razor` shell exists; `GridToolbarTemplate` render fragment is supported; individual tool components are not present
**Target state:** Implement each toolbar tool as a Razor component usable inside `<GridToolBar>` tag; implement `<GridToolBar>` wrapper and `<GridToolBarTemplate>` slot distinction
**Blocking:** no — `ToolbarTemplate` is functional; toolbar tools are an enhancement
**Dependencies:** S-04 (column/child structure conventions)

---

## GAP-DATAGRID-S07

**Source gap ID:** S-07
**Priority:** P2
**Category:** spec-ahead
**Summary:** `GridPagerSettings` compound settings object documented but source uses flat parameters
**Spec reference:** `docs/component-specs/grid/paging.md` — Pager Settings section
**Source state:** Flat `PagerButtonCount` int on `MariloDataGrid`; no `GridPagerSettings` component; no `PageSizes` compound object
**Target state:** Introduce `GridPagerSettings` child component inside `<GridSettings>`, supporting at minimum `ButtonCount`, `PageSizes`, `InputType`, `Responsive`, `Position` parameters
**Blocking:** no — flat `PagerButtonCount` and `PageSizes` parameters are functional
**Dependencies:** none

---

## GAP-DATAGRID-S08

**Source gap ID:** S-08
**Priority:** P2
**Category:** spec-ahead
**Summary:** Excel and PDF export — spec documents full export pipeline; source supports CSV string return only
**Spec reference:** `docs/component-specs/grid/export/excel.md`, `docs/component-specs/grid/export/pdf.md`
**Source state:** `OnBeforeExport`, `OnAfterExport`, `ExportAllPages` parameters exist; source generates CSV text only; no Excel or PDF library integration
**Target state:** Integrate spreadsheet/PDF generation; expose `GridExcelExport`, `GridPdfExport` child configurations; wire `ExcelExport` and `PdfExport` toolbar tool commands
**Blocking:** no
**Dependencies:** S-06 (toolbar tools)

---

## GAP-DATAGRID-S09

**Source gap ID:** S-09
**Priority:** P2
**Category:** spec-ahead
**Summary:** Composite filter descriptors / filter menu AND/OR logic — not implemented
**Spec reference:** `docs/component-specs/grid/filter/filter-menu.md`
**Source state:** Single filter per field stored in `GridState`; no AND/OR operator logic; no composite `FilterDescriptor` support
**Target state:** Support composite filter descriptors with AND/OR operators in filter menu UI and in `GridState`
**Blocking:** no — filter-row mode (single filter) is functional
**Dependencies:** none

---

## GAP-DATAGRID-S10

**Source gap ID:** S-10
**Priority:** P2
**Category:** spec-ahead
**Summary:** Drag-to-group header panel UI — spec shows a drag-panel above grid; source has only programmatic grouping
**Spec reference:** `docs/component-specs/grid/grouping/overview.md`
**Source state:** Programmatic `GroupBy`/`Ungroup` via `GridState`; `Groupable="true"` exposes no drag panel in render output
**Target state:** Render a drag-panel header region when `Groupable="true"`; support column header drag-to-group interaction
**Blocking:** no — programmatic grouping is functional
**Dependencies:** none

---

## GAP-DATAGRID-S15

**Source gap ID:** S-15
**Priority:** P2
**Category:** spec-ahead
**Summary:** DataAnnotations validation integration for edit mode — not wired
**Spec reference:** `docs/component-specs/grid/editing/validation.md`
**Source state:** No `EditContext` creation or `DataAnnotationsValidator` wiring in `MariloDataGrid.Editing.cs`; validation attributes on model are not evaluated
**Target state:** Wrap the in-edit-mode form in an `EditForm` with `EditContext`; wire `DataAnnotationsValidator`; show validation messages inline; block `OnUpdate`/`OnCreate` on invalid state
**Blocking:** no — editing is functional without validation
**Dependencies:** none

---

## Phase C — P3 Next-Phase Gaps

---

## GAP-DATAGRID-S01

**Source gap ID:** S-01
**Priority:** P3
**Category:** spec-ahead
**Summary:** `AdaptiveMode` parameter — responsive/mobile adaptive rendering
**Spec reference:** `docs/component-specs/grid/overview.md` — Grid Parameters table lists `AdaptiveMode` as `AdaptiveMode` enum
**Source state:** Not present on `MariloDataGrid.razor.cs`
**Target state:** Add `AdaptiveMode` parameter; implement adaptive popups (FilterMenu, ContextMenu as ActionSheet) when `AdaptiveMode.Auto` and window width < 768px
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-S03

**Source gap ID:** S-03
**Priority:** P3
**Category:** spec-ahead
**Summary:** `CustomKeyboardShortcuts` parameter (`Dictionary<GridKeyboardScope, Dictionary<string, GridKeyboardCommand?>>`) — not implemented
**Spec reference:** `docs/component-specs/grid/keyboard-navigation.md` — Using Custom Keys section (also logged as SA-05 in Wave 1)
**Source state:** `GridKeyboardScope` and `GridKeyboardCommand` enums absent from `src/Marilo.Core`; no `CustomKeyboardShortcuts` parameter on grid
**Target state:** Add enums to `Marilo.Core.Enums`; add `CustomKeyboardShortcuts` parameter; wire key-event dispatch to override table
**Blocking:** no
**Dependencies:** SA-06, SA-07, SA-08 (default key binding implementation — must exist before custom overrides make sense)

---

## GAP-DATAGRID-S11

**Source gap ID:** S-11
**Priority:** P3
**Category:** spec-ahead
**Summary:** Multi-column headers (`MariloGridColumnGroup`) — not present
**Spec reference:** `docs/component-specs/grid/columns/multi-column-headers.md`
**Source state:** No `MariloGridColumnGroup` component or stacked header rendering in source
**Target state:** Implement `MariloGridColumnGroup` Razor component with `Title` parameter; render `colspan` header rows
**Blocking:** no
**Dependencies:** S-04

---

## GAP-DATAGRID-S12

**Source gap ID:** S-12
**Priority:** P3
**Category:** spec-ahead
**Summary:** Column menu / column chooser — not present
**Spec reference:** `docs/component-specs/grid/columns/menu.md`
**Source state:** No column menu Razor component or UI surface
**Target state:** Implement column menu popup with filter, sort, column chooser options; integrate with column header context
**Blocking:** no
**Dependencies:** S-09 (filter), S-06 (toolbar for chooser)

---

## GAP-DATAGRID-S13

**Source gap ID:** S-13
**Priority:** P3
**Category:** spec-ahead
**Summary:** AI features (9 spec pages under `smart-ai-features/`) — future scope
**Spec reference:** `docs/component-specs/grid/smart-ai-features/` (9 pages)
**Source state:** No AI integration surface on `MariloDataGrid`
**Target state:** Integrate AI assistant tools, semantic search, smart box, row highlight, and AI service setup per spec pages
**Blocking:** no
**Dependencies:** External AI service contracts; deferred to Phase D

---

## GAP-DATAGRID-S14

**Source gap ID:** S-14
**Priority:** P3
**Category:** spec-ahead
**Summary:** `HighlightedItems` + highlighting API — not present
**Spec reference:** `docs/component-specs/grid/highlighting.md`
**Source state:** No `HighlightedItems` parameter or highlighting API on `MariloDataGrid`
**Target state:** Add `HighlightedItems` (`IEnumerable<TItem>`) parameter; render highlight CSS class on matching rows/cells
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-S16

**Source gap ID:** S-16
**Priority:** P3
**Category:** spec-ahead
**Summary:** `PopupFormTemplate`, `PopupButtonsTemplate`, `PagerTemplate` — template slots not implemented
**Spec reference:** `docs/component-specs/grid/templates/popup-form-template.md`, `templates/popup-buttons-template.md`, `templates/pager.md`
**Source state:** No `PopupFormTemplate`, `PopupButtonsTemplate`, or `PagerTemplate` render-fragment parameters on `MariloDataGrid`
**Target state:** Add three render-fragment parameters; wire into popup edit form and pager rendering paths
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-S17

**Source gap ID:** S-17
**Priority:** P3
**Category:** spec-ahead
**Summary:** Checkbox-list filter control — internal state fields exist but no public surface
**Spec reference:** `docs/component-specs/grid/filter/checkboxlist.md`
**Source state:** `_checkBoxFilterField` and similar internal fields referenced in source; no public parameter or UI component renders the checkbox-list filter
**Target state:** Expose checkbox-list filter mode via `FilterMode` enum value or column-level parameter; implement checkbox-list filter popup UI
**Blocking:** no
**Dependencies:** S-09 (composite filter descriptors)

---

## Phase D — Wave 1 Spec-Ahead Gaps (SA-01..SA-14)

These are additional spec-ahead items discovered during the Wave 1 orchestrator review (focused on `selection/`, `keyboard-navigation.md`, `refresh-data.md`, `editing/overview.md`).

---

## GAP-DATAGRID-SA01

**Source gap ID:** SA-01
**Priority:** P3
**Category:** spec-ahead
**Summary:** `DragToSelect` parameter on `GridSelectionSettings` for rectangle drag-to-select cells
**Spec reference:** `docs/component-specs/grid/selection/cells.md:20`
**Source state:** Cell selection is click-only; no `DragToSelect` parameter; no `GridSelectionSettings` type
**Target state:** Introduce `GridSelectionSettings` child component with `DragToSelect` bool; implement rectangle drag-select pointer event handler
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-SA02

**Source gap ID:** SA-02
**Priority:** P3
**Category:** spec-ahead
**Summary:** `<GridSelectionSettings SelectionType="GridSelectionType.Row">` as alternate selection enablement via settings object
**Spec reference:** `docs/component-specs/grid/selection/rows.md:27`
**Source state:** No `GridSelectionSettings` component/type in source; selection controlled by flat `SelectionMode` parameter
**Target state:** Implement `GridSelectionSettings` child component; map `SelectionType` to existing `SelectionMode` enum values
**Blocking:** no
**Dependencies:** SA-01

---

## GAP-DATAGRID-SA03

**Source gap ID:** SA-03
**Priority:** P3
**Category:** spec-ahead
**Summary:** `<GridCheckboxColumn>` dedicated element with `SelectAll` and `CheckBoxOnlySelection` parameters
**Spec reference:** `docs/component-specs/grid/selection/rows.md:29`, `docs/component-specs/grid/columns/checkbox.md`
**Source state:** Flat `ShowCheckboxColumn` bool on grid; no `GridCheckboxColumn` Razor component, no `SelectAll` or `CheckBoxOnlySelection` parameters
**Target state:** Implement `GridCheckboxColumn` Razor component with `SelectAll` and `CheckBoxOnlySelection` parameters; deprecate flat `ShowCheckboxColumn`
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-SA04

**Source gap ID:** SA-04
**Priority:** P3
**Category:** spec-ahead
**Summary:** Shift-click range selection and Ctrl-click toggle for row selection — not implemented
**Spec reference:** `docs/component-specs/grid/selection/rows.md:19`
**Source state:** Only single-item toggle present in row click logic; no Shift/Ctrl modifier handling
**Target state:** Handle `shiftKey` and `ctrlKey` modifiers on row click to extend or toggle selection range
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-SA05

**Source gap ID:** SA-05
**Priority:** P3
**Category:** spec-ahead
**Summary:** `CustomKeyboardShortcuts` parameter — duplicate of S-03, re-confirmed in Wave 1
**Spec reference:** `docs/component-specs/grid/keyboard-navigation.md:175-210`
**Source state:** `GridKeyboardScope` and `GridKeyboardCommand` enums absent; no `CustomKeyboardShortcuts` parameter
**Target state:** See GAP-DATAGRID-S03
**Blocking:** no
**Dependencies:** GAP-DATAGRID-S03

---

## GAP-DATAGRID-SA06

**Source gap ID:** SA-06
**Priority:** P2
**Category:** spec-ahead
**Summary:** Default key bindings for cell navigation (arrows, Home/End, Ctrl+Home/End, PageUp/PageDown) — not implemented
**Spec reference:** `docs/component-specs/grid/keyboard-navigation.md:51-62`
**Source state:** `Navigable` bool exists; no `onkeydown` handler or navigation key dispatch in Razor markup or partials
**Target state:** Add keyboard navigation JS interop or Blazor keyboard event handler; implement default key binding table from spec
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-SA07

**Source gap ID:** SA-07
**Priority:** P2
**Category:** spec-ahead
**Summary:** Data-cell keyboard actions (Enter/F2=edit, Esc=cancel, Space=select, Delete/Backspace=delete row)
**Spec reference:** `docs/component-specs/grid/keyboard-navigation.md:67-76`
**Source state:** None of these key actions are wired to source commands
**Target state:** Wire keyboard commands to grid's edit/select/delete pipeline when a data cell is focused
**Blocking:** no
**Dependencies:** SA-06

---

## GAP-DATAGRID-SA08

**Source gap ID:** SA-08
**Priority:** P2
**Category:** spec-ahead
**Summary:** Edit-row keyboard navigation (Tab/Shift+Tab across editors, Enter=save, Esc=cancel)
**Spec reference:** `docs/component-specs/grid/keyboard-navigation.md:122-137`
**Source state:** No editor-focus management in `MariloDataGrid.Editing.cs`
**Target state:** Implement Tab/Shift+Tab focus traversal across column editors in edit mode; wire Enter=save, Esc=cancel
**Blocking:** no
**Dependencies:** SA-06, SA-07

---

## GAP-DATAGRID-SA09

**Source gap ID:** SA-09
**Priority:** P3
**Category:** spec-ahead
**Summary:** Column `EditorType` parameter + `GridEditorType` enum — not on `MariloGridColumn`
**Spec reference:** `docs/component-specs/grid/editing/overview.md:158-172`
**Source state:** `MariloGridColumn` only exposes `EditorTemplate` render fragment; no `EditorType` parameter; no `GridEditorType` enum in `Marilo.Core`
**Target state:** Add `GridEditorType` enum to `Marilo.Core.Enums`; add `EditorType` parameter to `MariloGridColumn`; render appropriate built-in editor based on enum value
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-SA10

**Source gap ID:** SA-10
**Priority:** P3
**Category:** spec-ahead
**Summary:** `NewRowPosition` parameter (`GridNewRowPosition.Top`/`Bottom`) — not present
**Spec reference:** `docs/component-specs/grid/editing/overview.md:208-215`
**Source state:** `BeginAdd()` in `Editing.cs:39` has no position concept; new rows always insert at index 0
**Target state:** Add `NewRowPosition` parameter (`GridNewRowPosition` enum); pass position hint to `BeginAdd()` for inline/incell modes
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-SA11

**Source gap ID:** SA-11
**Priority:** P2
**Category:** spec-ahead
**Summary:** Automatic `OnRead` re-fire after `OnCancel`/`OnCreate`/`OnDelete`/`OnUpdate` in server-binding mode
**Spec reference:** `docs/component-specs/grid/editing/overview.md:188-197`
**Source state:** `ProcessDataAsync()` is called after Save/Delete; needs confirmation that this routes through `OnRead` when consumer is in server-binding mode
**Target state:** Verify `ProcessDataAsync` fires `OnRead` event when grid is in `OnRead` mode; if not, wire the explicit re-fire; add test coverage
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-SA12

**Source gap ID:** SA-12
**Priority:** P3
**Category:** spec-ahead
**Summary:** No guard enforcing "Cell selection not supported with InCell edit mode"
**Spec reference:** `docs/component-specs/grid/selection/rows.md:175-184`
**Source state:** `SelectionUnit=Cell` + `EditMode=InCell` is allowed without error or warning
**Target state:** Add a defensive check (dev-time warning or enforced no-op) when `SelectionUnit=Cell` and `EditMode=InCell` are combined
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-SA13

**Source gap ID:** SA-13
**Priority:** P3
**Category:** spec-ahead
**Summary:** Grid does not clear `SelectedItems` when user drags and drops selected rows
**Spec reference:** `docs/component-specs/grid/selection/rows.md:210`
**Source state:** Row drag handler in `MariloDataGrid` does not clear `_selectedItems` after drop
**Target state:** In `OnRowDrop` dispatch, clear `SelectedItems` and fire `SelectedItemsChanged` after the drop completes
**Blocking:** no
**Dependencies:** none

---

## GAP-DATAGRID-SA14

**Source gap ID:** SA-14
**Priority:** P2
**Category:** spec-ahead
**Summary:** No dev-time diagnostic when model lacks parameterless constructor and `OnModelInit` is not wired
**Spec reference:** `docs/component-specs/grid/editing/overview.md:37`
**Source state:** `BeginAdd()` in `Editing.cs:39` silently assigns null/default item if `OnModelInit` is not wired and model has no parameterless constructor
**Target state:** Add a defensive check in `BeginAdd()` that throws a descriptive `InvalidOperationException` (or emits a `Console.Error` warning) when `Activator.CreateInstance<TItem>()` fails and `OnModelInit` is not wired
**Blocking:** no
**Dependencies:** none

---

## Summary Statistics

| Priority | Count | Gap IDs |
|----------|-------|---------|
| P1 | 2 | S-04, S-05 |
| P2 | 11 | S-02, S-06, S-07, S-08, S-09, S-10, S-15, SA-06, SA-07, SA-08, SA-11, SA-14 |
| P3 | 17 | S-01, S-03, S-11, S-12, S-13, S-14, S-16, S-17, SA-01, SA-02, SA-03, SA-04, SA-05, SA-09, SA-10, SA-12, SA-13 |
| **Total** | **30** | S-01..S-17 (17), SA-01..SA-14 (14, minus SA-05 which duplicates S-03 = 13 net) |

> Note: SA-05 is a re-confirmation of S-03; both are registered for traceability but resolve as one item.

---

## Coordinator-Blocked Gaps

The following gaps require a coordinator decision before resolution can begin. They are tracked here but WILL NOT enter the implementation pipeline until unblocked:

| Gap ID | Blocker |
|--------|---------|
| S-04 | `<GridColumns>` wrapper vs direct ChildContent — markup contract decision |
| S-05 | `GridCommandColumn` element shape — depends on S-04 |
| M-01 | `<MariloGrid>` vs `<MariloDataGrid>` naming (tracked in delivery workspace) |
| M-02 | `<GridColumn>` vs `<MariloGridColumn>` naming (tracked in delivery workspace) |
| M-03 | Virtual-scrolling parameter shape — `ScrollMode`/`RowHeight` vs `EnableVirtualization`/`VirtualizeOverscanCount` |
| M-05 | `GridState<TItem>` genericization — shared Core model |
| M-12 | Pager settings shape — `GridPagerSettings` object vs flat parameters |
