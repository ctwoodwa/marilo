# DataGrid Spec Gap List

**Audit date:** 2026-04-11
**Component:** `MariloDataGrid<TItem>`
**Stage:** 01-spec-review (refresh pass)
**Spec directory:** `/docs/component-specs/grid/` (78 markdown files, 25 feature areas)
**Source:** `/src/Marilo.Components/DataGrid/` (`MariloDataGrid.*.cs` + `MariloGridColumn.razor`)
**Previous pass:** 2026-04-03 (`datagrid-spec-gaps.md`, left in place for history)

Format follows `../../shared/spec-coverage-format.md`.
This pass is a **full-surface refresh** — it re-inventories the source against the spec,
reclassifies gaps per the three-list rule (undocumented / spec-ahead / mismatch), and
records deltas against the 2026-04-03 findings.

---

## Delta Since 2026-04-03

| Previously listed gap | Status now | Notes |
|---|---|---|
| #4 `SetStateAsync()` missing | **CLOSED** in source | `public async Task SetStateAsync(GridState state)` exists (razor.cs L518). Spec side still `GridState` (non-generic). |
| #10 No cell selection | **CLOSED** in source | `SelectedCells` + `SelectedCellsChanged` + `SelectionUnit` exist (razor.cs L107-116). |
| #11 No frozen/locked columns | **CLOSED** in source | `Locked` + `FrozenPosition` on `MariloGridColumn` (L59-62). Sticky offsets computed in `GetFrozenCellStyle`. |
| #12 Format string mismatch | **CLOSED** in source | `DisplayFormat` parameter added on column (L65) alongside legacy `Format`. Both supported. |
| #16 No `ConfirmDelete` | **CLOSED** in source | `ConfirmDelete` + `ConfirmDeleteText` parameters (L237-240). |
| #18 No row drag-and-drop | **CLOSED** in source | `RowDraggable` + `OnRowDrop` (L252-255). Test coverage in `MariloDataGridRowDragTests.cs`. |
| Test count "4 bUnit tests" | **OUT OF DATE** | Actual count: **66 facts across 7 files** (Phase1 18 + Phase2 15 + Phase3 10 + Frozen 9 + RowDrag 7 + base 5 + FixedWidthProvider 2). |
| Parameter count "49" | **OUT OF DATE** | Actual public [Parameter] count: **66** on `MariloDataGrid` (62 in `razor.cs` + 4 in `Interop.cs`) plus **17** on `MariloGridColumn`. |

All six blocking/important gaps above are now *source-closed* but remain **spec-mismatched** — the spec still documents the old Telerik shape. They are re-listed below as `mismatch` records, not as `spec-ahead`.

---

## Source Inventory (refreshed)

Public API surface on `MariloDataGrid<TItem>`:

**Data binding (2):** `Data`, `OnRead`.
**Paging (6):** `Pageable`, `PageSize`, `Page`, `PageChanged`, `PageSizes`, `PageSizeChanged`, `PagerButtonCount`.
**Sorting / filtering / grouping (6):** `Sortable`, `SortMode`, `FilterMode`, `Groupable`, `GroupHeaderTemplate`, `GroupFooterTemplate`.
**Selection (6):** `SelectionMode`, `SelectionUnit`, `SelectedCells`, `SelectedCellsChanged`, `ShowCheckboxColumn`, `SelectedItems`, `SelectedItemsChanged`.
**Layout & display (11):** `Striped`, `Height`, `Width`, `Navigable`, `AutoGenerateColumns`, `EnableVirtualization`, `VirtualizeOverscanCount`, `IsLoading`, `ShowSearchBox`, `SearchBoxPlaceholder`, `ColumnWidthProvider`.
**Templates (5):** `ChildContent`, `ToolbarTemplate`, `DetailTemplate`, `NoDataTemplate`, `RowTemplate`.
**State events (2):** `OnStateInit`, `OnStateChanged`.
**Row events (6):** `OnRowClick`, `OnRowDoubleClick`, `OnRowContextMenu`, `OnRowRender`, `OnRowExpand`, `OnRowCollapse`.
**Editing (10):** `EditMode`, `OnAdd`, `OnCreate`, `OnUpdate`, `OnDelete`, `OnEdit`, `OnCancel`, `OnModelInit`, `OnCommand`, `ConfirmDelete`, `ConfirmDeleteText`.
**Export (3):** `OnBeforeExport`, `OnAfterExport`, `ExportAllPages`.
**Row drag (2):** `RowDraggable`, `OnRowDrop`.
**Interop (4):** `Resizable`, `Reorderable`, `OnColumnReorder`, `OnColumnResize`.
**Public methods / props (5):** `GetState()`, `SetStateAsync()`, `Rebind()`, `IsEditing`, `IsCreating`.

`MariloGridColumn<TItem>` parameters (17): `Field`, `Title`, `Width`, `Sortable`, `Filterable`, `Visible`, `Template`, `HeaderTemplate`, `EditorTemplate`, `FooterTemplate`, `TextAlign`, `Format`, `DisplayFormat`, `OnCellRender`, `Editable`, `Groupable`, `Locked`, `FrozenPosition`.

---

## Three-List Classification

### (a) Undocumented (implemented but not in spec)

| # | Member | Priority | Record |
|---|--------|---|---|
| U-01 | `ShowSearchBox`, `SearchBoxPlaceholder` | P2 | Built-in search toolbar — spec treats search only as a toolbar tool (not a root parameter). Action: **document in spec**. Delegated: **spec update only**. |
| U-02 | `EnableVirtualization`, `VirtualizeOverscanCount` | P2 | Virtual scrolling exposed as bool+int rather than `ScrollMode`/`RowHeight`. Keep implementation, document the current shape. See M-01 for the mismatch side. Action: **update spec**. Delegated: **spec update only**. |
| U-03 | `Striped` | P3 | Appearance toggle not in spec overview. Action: **update spec appearance section**. Delegated: **spec update only**. |
| U-04 | `AutoGenerateColumns` | P2 | Spec references auto-generation but does not document the `AutoGenerateColumns` boolean switch. Action: **update spec**. Delegated: **spec update only**. |
| U-05 | `Resizable`, `Reorderable` (grid-level) | P2 | Spec scopes resize/reorder to column level; source has grid-level toggles. Action: **document on grid + column pages**. Delegated: **spec update only**. |
| U-06 | `OnRowContextMenu` | P3 | Right-click event, not in spec events page. Action: **update events spec**. Delegated: **spec update only**. |
| U-07 | `OnRowExpand` / `OnRowCollapse` | P3 | Detail expand/collapse events missing from spec events page. Action: **update events spec**. Delegated: **spec update only**. |
| U-08 | `PagerButtonCount` | P2 | Spec documents a rich `GridPagerSettings` object; source exposes a flat `PagerButtonCount` int on the grid. Action: **update paging spec**. Delegated: **spec update only**. |
| U-09 | `ColumnWidthProvider` / `IColumnWidthProvider` | P3 | Advanced extension point absent from spec. Action: **add to sizing spec**. Delegated: **spec update only**. |
| U-10 | `GridGroupHeaderContext<TItem>` context type on `GroupHeaderTemplate` / `GroupFooterTemplate` | P2 | Spec templates page describes generic context; source context type not named. Action: **document**. Delegated: **spec update only**. |

### (b) Spec-ahead (documented but not implemented)

| # | Member | Priority | Record |
|---|--------|---|---|
| S-01 | `AdaptiveMode` | P3 | Overview table lists it. Not in source. Known planned gap. **Delegated: gap-analysis intake.** |
| S-02 | `Class` parameter on grid | P2 | Overview table. Not present on `MariloDataGrid`. Known gap. **Delegated: gap-analysis intake.** |
| S-03 | `CustomKeyboardShortcuts` | P3 | Overview table + keyboard-navigation spec. Not in source. **Delegated: gap-analysis intake.** |
| S-04 | `<GridColumns>` wrapper element | P1 | Spec nests `<GridColumn>` inside `<GridColumns>`. Source takes columns as direct `ChildContent`. Coordinator escalation (shared markup contract). |
| S-05 | `GridCommandColumn` + `GridCommandButton` built-in | P1 | Spec documents rich command-column scheme. Source has `MariloGridCommandButton.razor` + `GridCommandTypes.cs` but no dedicated `GridCommandColumn` element. Partial. **Delegated: gap-analysis intake.** |
| S-06 | `<GridToolBarTemplate>` + 13 tool components (Add, Save, Export*, SearchBox, ColumnChooser, CsvExport, ExcelExport, etc.) | P2 | Spec toolbar page and editing pages use tool components. Source exposes `ToolbarTemplate` render fragment + `MariloGridToolbar.razor` shell. **Delegated: gap-analysis intake.** |
| S-07 | `GridPagerSettings` compound settings object | P2 | Spec paging page. Source uses flat parameters. **Delegated: gap-analysis intake.** |
| S-08 | Excel + PDF export | P2 | Spec `export/excel.md`, `export/pdf.md`. Source supports CSV string return only. **Delegated: gap-analysis intake.** |
| S-09 | Composite filter descriptors / filter menu AND/OR | P2 | `filter/filter-menu.md`. Source stores a single filter per field. **Delegated: gap-analysis intake.** |
| S-10 | Drag-to-group header panel UI | P2 | `grouping/overview.md`. Source has programmatic `GroupBy`/`Ungroup` but no drag panel. **Delegated: gap-analysis intake.** |
| S-11 | Multi-column headers (`MariloGridColumnGroup`) | P3 | `columns/multi-column-headers.md`. Not present. **Delegated: gap-analysis intake.** |
| S-12 | Column menu / column chooser | P3 | `columns/menu.md`. Not present. **Delegated: gap-analysis intake.** |
| S-13 | AI features (9 spec pages under `smart-ai-features/`) | P3 | Future scope. **Delegated: gap-analysis intake (Phase D).** |
| S-14 | `HighlightedItems` + highlighting API | P3 | `highlighting.md`. Not present. **Delegated: gap-analysis intake.** |
| S-15 | DataAnnotations validation integration for edit | P2 | `editing/validation.md`. Source edit pipeline does not wire `EditContext` / validation. **Delegated: gap-analysis intake.** |
| S-16 | `PopupFormTemplate`, `PopupButtonsTemplate`, `PagerTemplate` | P3 | Template spec pages. Not present. **Delegated: gap-analysis intake.** |
| S-17 | Checkbox-list filter control | P3 | `filter/checkboxlist.md`. Internal state fields exist (`_checkBoxFilterField` etc.) but no public parameters. **Delegated: gap-analysis intake.** |

### (c) Mismatched (documented AND implemented, but names/shape differ)

| # | Spec side | Source side | Priority | Record |
|---|---|---|---|---|
| M-01 | `<MariloGrid>` component tag | `<MariloDataGrid>` | **P1 — BLOCKING** | Naming decision. Every spec example fails to compile against source. **Coordinator escalation** — rename crosses public API + every consumer. |
| M-02 | `<GridColumn>` column tag | `<MariloGridColumn>` | **P1 — BLOCKING** | Same as M-01. **Coordinator escalation.** |
| M-03 | `ScrollMode` enum + `RowHeight` decimal | `EnableVirtualization` bool + `VirtualizeOverscanCount` int | P2 | Virtual-scrolling shape differs. Two acceptable resolutions: (1) introduce `ScrollMode`, deprecate `EnableVirtualization`; (2) update spec to match current shape. **Coordinator decision required** — affects public API. |
| M-04 | `SortMode` single/multiple **(spec param on `MariloGrid`)** | `SortMode` enum with `Single`/`Multiple` on `MariloDataGrid` | **RESOLVED in source** (since 2026-04-03) | Update spec to reference `GridSortMode` enum name. **Delegated: spec update only.** |
| M-05 | `GridState<TItem>` | `GridState` (non-generic) | P2 | Type-loss on edit item/original item. Two fixes possible: genericize `GridState` or document current non-generic shape. **Coordinator decision required** — shared Core model. |
| M-06 | `GridCommandEventArgs` (untyped) for edit callbacks | `GridEditEventArgs<TItem>` (typed) | P2 | Spec edit examples won't compile. Prefer keeping the typed source shape and rewriting spec. **Delegated: spec update only.** |
| M-07 | `OnModelInit` lambda-returning-new-model | `EventCallback<GridModelInitEventArgs<TItem>>` (set `Item` on args) | P2 | Spec idiom differs from source pattern. **Delegated: spec update only** (current source shape is reasonable). |
| M-08 | `DisplayFormat` `"{0:C2}"` composite format | **Now implemented** alongside legacy `Format` `"C2"` | **RESOLVED in source** (since 2026-04-03) | Update column spec to show both parameters. **Delegated: spec update only.** |
| M-09 | `Locked` + frozen column API | **Now implemented** | **RESOLVED in source** | Spec matches; just re-verify on `columns/frozen.md` page. **Delegated: spec update only (verify).** |
| M-10 | Cell selection `SelectedCells` / `SelectedCellsChanged` | **Now implemented** | **RESOLVED in source** | Spec `selection/cells.md` can be validated. **Delegated: spec update only (verify).** |
| M-11 | Row drag-drop `OnRowDrop` | **Now implemented** | **RESOLVED in source** | Validate spec `row-drag-drop.md` parameter names match. **Delegated: spec update only (verify).** |
| M-12 | Pager structure `GridPagerSettings { ButtonCount, PageSizes, InputType, ... }` | Flat `PagerButtonCount` + `PageSizes` on grid | P2 | Shape mismatch. Keep flat or introduce settings object. **Coordinator decision required** — public API shape. |
| M-13 | `OnRowExpand` / `OnRowCollapse` typed event args (`GridRowExpandEventArgs<TItem>`) | `EventCallback<TItem>` directly | P3 | Minor shape mismatch. **Delegated: spec update only** (current source shape is simpler). |

---

## Priority-Ordered Gap List

**P1 (blocking — coordinator escalation only):**
1. M-01 `<MariloGrid>` vs `<MariloDataGrid>` naming.
2. M-02 `<GridColumn>` vs `<MariloGridColumn>` naming.
3. S-04 `<GridColumns>` wrapper requirement.
4. S-05 `GridCommandColumn` element shape.

**P2 (this phase):**
5. M-03 Virtual-scrolling parameter shape.
6. M-05 `GridState<TItem>` genericization.
7. M-12 Pager parameter shape.
8. S-06 Built-in toolbar tool components.
9. S-07 `GridPagerSettings` compound object.
10. S-08 Excel + PDF export.
11. S-09 Composite filter descriptors.
12. S-10 Drag-to-group UI.
13. S-15 Edit-mode validation integration.
14. S-02 `Class` parameter.
15. U-01, U-02, U-04, U-05, U-08, U-10 (spec-update-only undocumented parameters).
16. M-04, M-06, M-07, M-08, M-09, M-10, M-11 (spec-update-only mismatches).

**P3 (next phase):**
17. S-01 `AdaptiveMode`, S-03 `CustomKeyboardShortcuts`, S-11 multi-column headers, S-12 column menu, S-13 AI features, S-14 highlighting, S-16 popup templates, S-17 checkbox-list filter.
18. U-03, U-06, U-07, U-09 (doc-only undocumented parameters).
19. M-13 row expand/collapse event args.

---

## What This Pass Closed In-Folder

Scope rule for this agent: changes restricted to the DataGrid folder only. After inventory:

- **Typos / missing `[Parameter]` XML docs:** none found. All 66 grid parameters and 17 column parameters have XML doc comments.
- **Demo coverage gaps for documented-and-implemented behaviour:** the four demo pages (Overview, Appearance, Events, Accessibility) already exercise the primary surface. No in-folder demo gap is blocking.
- **bUnit test gaps for documented-and-implemented behaviour:** test count is 66 facts across 7 files (not 4 as the stale gap list said). The resolved items (M-08 `DisplayFormat`, M-09 frozen columns, M-10 cell selection, M-11 row drop, #4 `SetStateAsync`, #16 `ConfirmDelete`) already have test files or are covered within Phase1/2/3 tests.

No source edits, no test additions, no demo edits were made this pass. The next stage (02-example-ux) should verify demo coverage per feature area, and `datagrid-gap-analysis` should pick up all items flagged "**Delegated: gap-analysis intake**".

---

## Next Recommended Actions

1. **Coordinator decision:** resolve M-01 / M-02 / S-04 / S-05 (naming + column wrapper + command-column shape). This is a public-API rename that crosses the shared-contract boundary — not this agent's scope.
2. **Spec-update batch:** apply the "spec update only" items (M-04, M-06, M-07, M-08, M-09, M-10, M-11, U-01…U-10). These edit only `docs/component-specs/grid/` — owned by the delivery workspace, not the gap-analysis workspace.
3. **Gap-analysis intake:** push the `S-*` spec-ahead items into `datagrid-gap-analysis/stages/01-intake`.
4. **Move to stage 02 (example-ux)** for the RESOLVED areas (data binding, paging, sorting, selection, frozen columns, row drag, cell edit) — they are safe to demo-audit.

---

## Audit Checklist

| Check | Pass? | Notes |
|---|---|---|
| All source parameters inventoried | YES | 66 on grid, 17 on column — counts match file scan. |
| All spec parameters inventoried | YES | Overview + per-area spec pages scanned for parameter tables. |
| Every gap record classified (a/b/c) | YES | U-\* undocumented, S-\* spec-ahead, M-\* mismatch. |
| Priority order justified | YES | P1 = blocking compile / shared contracts, P2 = this phase, P3 = next phase. |
| No spec content duplicated in this output | YES | Output references spec pages by path; no copy-paste of spec prose. |

---

## 2026-04-11 orchestrator wave 1 (subagent dispatch)

**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Worker:** `w-datagrid-delivery` (ICM stage 01-spec-review, Wave 1)
**Scope:** Focus topics — `selection/`, `keyboard-navigation.md`, `refresh-data.md`, `editing/overview.md`
**Files read (spec):** `docs/component-specs/grid/selection/overview.md`, `selection/rows.md`, `selection/cells.md`, `keyboard-navigation.md`, `refresh-data.md`, `editing/overview.md`
**Files read (source):** `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs`, `MariloDataGrid.Editing.cs`, `MariloGridColumn.razor`

This pass narrows to the four Wave 1 topic areas and only logs gaps **not** already captured in the 2026-04-11 refresh pass above. Sequential numbering restarts within each category (SA / SRC / NM) per the orchestrator scope message.

### Spec-ahead (spec documents behavior source does not implement)

- **SA-01 (Spec-ahead):** `docs/component-specs/grid/selection/cells.md:20` — `DragToSelect` parameter on `GridSelectionSettings` enabling rectangle drag-to-select cells — missing from `MariloDataGrid.razor.cs` (no `DragToSelect` / no `GridSelectionSettings` type; cell selection is click-only).
- **SA-02 (Spec-ahead):** `docs/component-specs/grid/selection/rows.md:27` — `<GridSelectionSettings SelectionType="GridSelectionType.Row">` as alternate way to enable selection — no `GridSelectionSettings` component/type in `src/Marilo.Components/DataGrid/` at all.
- **SA-03 (Spec-ahead):** `docs/component-specs/grid/selection/rows.md:29` and `columns/checkbox.md` — `<GridCheckboxColumn SelectAll="true" CheckBoxOnlySelection="false" />` dedicated column element — source only has a flat `ShowCheckboxColumn` bool on the grid (`MariloDataGrid.razor.cs:119`); no `GridCheckboxColumn` razor component, no `SelectAll` or `CheckBoxOnlySelection` parameters.
- **SA-04 (Spec-ahead):** `docs/component-specs/grid/selection/rows.md:19` — Shift-click range selection and Ctrl-click toggle selection — scan of `MariloDataGrid.Data.cs` / `razor.cs` shows no Shift/Ctrl modifier handling in row click logic; only single-item toggle present. Behavior documented as default but not implemented.
- **SA-05 (Spec-ahead):** `docs/component-specs/grid/keyboard-navigation.md:175-210` — `CustomKeyboardShortcuts` parameter (`Dictionary<GridKeyboardScope, Dictionary<string, GridKeyboardCommand?>>`) — no such parameter, enums (`GridKeyboardScope`, `GridKeyboardCommand`) absent from `src/Marilo.Core` and `MariloDataGrid.razor.cs`. (Previously logged as S-03; re-confirmed for Wave 1 keyboard focus.)
- **SA-06 (Spec-ahead):** `docs/component-specs/grid/keyboard-navigation.md:51-62` — Default key bindings for navigation (arrows, Home/End, Ctrl+Home/End, PageUp/PageDown) — source `MariloDataGrid.razor.cs:139` only exposes `Navigable` bool; no `onkeydown` handler nor any navigation key dispatch in `razor` markup or partials. The whole default key-binding table is undelivered.
- **SA-07 (Spec-ahead):** `docs/component-specs/grid/keyboard-navigation.md:67-76` — Data-cell keyboard actions (`Enter`=edit, `F2`=edit, `Esc`=cancel edit, `Space`=select, `Delete`/`Backspace`=delete row) — none of these are wired to source commands.
- **SA-08 (Spec-ahead):** `docs/component-specs/grid/keyboard-navigation.md:122-137` — Edit-row keyboard (Tab/Shift+Tab across editors, Enter=save, Esc=cancel) — no editor-focus management in `MariloDataGrid.Editing.cs`.
- **SA-09 (Spec-ahead):** `docs/component-specs/grid/editing/overview.md:158-172` — Column `EditorType` parameter + `GridEditorType` enum (`CheckBox`/`Switch`/`DatePicker`/`DateTimePicker`/`TimePicker`/`TextArea`/`TextBox`) — not on `MariloGridColumn` (which only exposes `EditorTemplate`); no `GridEditorType` enum in Core.
- **SA-10 (Spec-ahead):** `docs/component-specs/grid/editing/overview.md:208-215` — `NewRowPosition` parameter on grid (`GridNewRowPosition.Top`/`Bottom`) — missing from `MariloDataGrid.razor.cs`. `BeginAdd()` in `Editing.cs:39` has no position concept.
- **SA-11 (Spec-ahead):** `docs/component-specs/grid/editing/overview.md:188-197` — Automatic `OnRead` call after `OnCancel`/`OnCreate`/`OnDelete`/`OnUpdate` — `MariloDataGrid.Editing.cs:54-101` calls `ProcessDataAsync()` after Save/Delete but does not explicitly re-fire `OnRead` when the consumer is in server-binding mode. Needs confirmation that `ProcessDataAsync` routes through `OnRead`; if not, this is a gap.
- **SA-12 (Spec-ahead):** `docs/component-specs/grid/selection/rows.md:175-184` — "Cell selection not supported with InCell edit mode" and "row selection in InCell works only via checkbox column" — source has no guard enforcing this; `SelectionUnit=Cell` + `EditMode=InCell` is allowed and not explicitly handled.
- **SA-13 (Spec-ahead):** `docs/component-specs/grid/selection/rows.md:210` — "Grid clears `SelectedItems` when the user drags and drops selected rows" — row drag handler in `MariloDataGrid` does not clear `_selectedItems` after drop.
- **SA-14 (Spec-ahead):** `docs/component-specs/grid/editing/overview.md:37` — Model requires parameterless constructor OR `OnModelInit`-provided instance; when neither is true, grid should surface a diagnostic — `BeginAdd()` in `Editing.cs:39` will silently assign a default/null item if `OnModelInit` is not wired. No defensive throw or dev-time warning.

### Source-ahead (source implements behavior spec does not document)

- **SRC-01 (Source-ahead):** `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs:110` — `SelectionUnit` parameter (`GridSelectionUnit` enum, default `Row`) — spec `selection/overview.md` discusses "row vs cell" selection but never names `SelectionUnit` as a parameter on the grid; `selection/cells.md:25` does mention it but scoped to "enable cell selection". Spec should name it at the overview level.
- **SRC-02 (Source-ahead):** `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs:113-116` — `SelectedCells` uses `IEnumerable<GridCellReference<TItem>>` — spec `selection/overview.md:38` lists the type as `IEnumerable<GridSelectedCellDescriptor>` (non-generic, different name). See also NM-02.
- **SRC-03 (Source-ahead):** `src/Marilo.Components/DataGrid/MariloDataGrid.Editing.cs:13-23` — `public BeginEdit(TItem item)`, `BeginCellEdit(TItem, string)`, `BeginAdd()`, `SaveEdit()`, `CancelEdit()`, `DeleteItem(TItem)`, `ExecuteCommand(string, TItem?)` — public imperative edit API exists in source but is not documented in `editing/overview.md`. Spec only discusses command buttons and events, not the imperative `BeginEdit`/`SaveEdit`/`CancelEdit` entry points.
- **SRC-04 (Source-ahead):** `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs:234` — `OnCommand` with `GridCommandEventArgs<TItem>` typed args — spec `editing/overview.md:147` shows `GridCommandEventArgs` as the argument for `OnCreate`/`OnUpdate`/`OnDelete`/`OnEdit`/`OnCancel`, but source uses `GridEditEventArgs<TItem>` for those five and reserves `GridCommandEventArgs<TItem>` for `OnCommand` alone. Spec conflates the two.
- **SRC-05 (Source-ahead):** `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs:511` — `public Task Rebind()` — `refresh-data.md:25` documents `Rebind` usage but the spec's example code targets `MariloGrid<T>` (not `MariloDataGrid<T>`) and does not link to the exact public signature. Spec needs to update the method reference with the correct component name + return type.
- **SRC-06 (Source-ahead):** `src/Marilo.Components/DataGrid/MariloDataGrid.Editing.cs:89-101` — `DeleteItem` calls `JS.InvokeAsync<bool>("confirm", ...)` for the `ConfirmDelete` dialog — spec `editing/overview.md:79` mentions "the component will show a Dialog", but the source delegates to the browser's native `window.confirm`, not a Marilo `MariloDialog` component. Spec should document this or source should swap to the Marilo dialog.
- **SRC-07 (Source-ahead):** `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs:19` — internal cell-selection state keyed by `(RowIndex, string Field)` tuples — `GridCellReference<TItem>.RowIndex` is exposed per `selection/cells.md:188`, but the spec does not make clear that row-index is page-relative (current-page display order) vs source-absolute. Behavior-level clarification needed.
- **SRC-08 (Source-ahead):** `src/Marilo.Components/DataGrid/MariloDataGrid.Editing.cs:120-137` — `ToggleDetailRow` fires `OnRowExpand`/`OnRowCollapse` and then `NotifyStateChanged("DetailExpand")` — `editing/overview.md` does not cover detail-row expansion at all; `refresh-data.md` doesn't mention that detail-expand state survives `Rebind()`. (See also U-07 in prior pass.)

### Naming mismatch

- **NM-01:** All four Wave 1 spec files use `<MariloGrid>` as the component tag (e.g. `selection/rows.md:34`, `keyboard-navigation.md:22`, `refresh-data.md:34`, `editing/overview.md:59`) — source component is `<MariloDataGrid>` (`MariloDataGrid.razor.cs:11`). Already logged as M-01 in 2026-04-11 refresh pass; re-confirmed present in every Wave 1 topic spec. **Blocking P1.**
- **NM-02:** `docs/component-specs/grid/selection/overview.md:38` declares `SelectedCells` as `IEnumerable<GridSelectedCellDescriptor>`, but `docs/component-specs/grid/selection/cells.md:24` uses `IEnumerable<GridCellReference<TItem>>`. **Spec is internally inconsistent** — pick one type name and make both pages agree. Source uses `GridCellReference<TItem>` (`MariloDataGrid.razor.cs:113`).
- **NM-03:** `docs/component-specs/grid/refresh-data.md:48` — `private MariloGrid<Employee> GridRef` field declaration in `@code` block uses the tag-name form; source type is `MariloDataGrid<TItem>`. Same root cause as NM-01 but worth calling out separately since it appears in `@code`-block C# (not just Razor markup), so a naive find/replace on tags won't catch it.
- **NM-04:** `docs/component-specs/grid/editing/overview.md:147` — `GridCommandEventArgs` named as the event-args type for `OnCreate`/`OnUpdate`/`OnDelete`/`OnEdit`/`OnCancel` — source uses `GridEditEventArgs<TItem>` for those (`MariloDataGrid.razor.cs:213-228`) and keeps `GridCommandEventArgs<TItem>` only for `OnCommand` (`razor.cs:234`). Type-name disagreement is consumer-breaking.
- **NM-05:** `docs/component-specs/grid/keyboard-navigation.md:231` — `@using Marilo.Blazor.Components.Grid` and slug references like `Marilo.Blazor.GridKeyboardCommand` — the Marilo namespace in source is `Marilo.Components.DataGrid` / `Marilo.Core.Enums`, not `Marilo.Blazor`. Every `Marilo.Blazor.*` slug reference in specs is stale from the Telerik provenance.
- **NM-06:** `docs/component-specs/grid/editing/overview.md:164` — `GridEditorType` enum namespace implied as `Marilo.Blazor.GridEditorType`. Not defined in source anywhere — see SA-09. Both naming and existence mismatch.

---
