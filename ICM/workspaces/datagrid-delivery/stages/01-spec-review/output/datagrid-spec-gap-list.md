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
