# DataGrid Remediation Plan -- Stage 04

**Worker:** `w-datagrid-gap-analysis`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Stage:** 04-remediation-plan
**Input:** `stages/03-resolution-design/output/datagrid-resolution-designs.md` (104 designed, 9 skipped)
**Date:** 2026-04-12

---

## Scope

104 resolution designs from S03 are converted into atomic implementation tasks. Each task is a single-turn worker assignment. Tasks are grouped into dispatch waves respecting the critical path.

**Skipped (blocked):** Lane F (8 rows: M-03, M-05, M-12, S-07, S-09, S-10, S-15, SA-11) + VP-006 (density public API). Total: 9 skipped.

**Deferred (Lane H):** 3 rows (VP-016, S-13, S-01). No implementation tasks -- tracked for future waves.

**Active scope:** 104 - 3 (Lane H deferred) = **101 rows across 37 atomic tasks**.

---

## Dispatch Wave Summary

| Wave | Name | Tasks | Depends On | Gate |
|------|------|------:|------------|------|
| W1 | Naming Cascade | 3 | None | All spec files use `MariloDataGrid` naming; `dotnet build` passes |
| W2 | Provider SCSS + Spec Docs + Demo E.1 | 12 | W1 | SCSS compiles; dark-mode tokens correct; spec docs updated; E.1 demos render |
| W3 | Keyboard Engine | 5 | W1, W2 (VP-015 focus rings) | Keyboard nav functional; `dotnet build` + `dotnet test` pass |
| W4 | Source-Ahead Implementation | 12 | W1; W3 for edit-keyboard integration | New components/params compile; tests pass |
| W5 | Source-Blocked Demos + Polish | 5 | W3 (keyboard), W4 (source features) | All demos render; gap-plan updated |

**Critical path:** W1 -> W2 (B.4 focus rings) -> W3 -> W5
**Parallel:** W2 lanes B/D/E.1 run simultaneously. W4 can start after W1 (source work independent of SCSS).

---

## Wave 1 -- Naming Cascade (Lane A)

All 22 Lane A rows are resolved by 3 atomic tasks. This wave unblocks every subsequent wave.

### DG-A-01: Bulk spec rename -- tag names

| Field | Value |
|-------|-------|
| **Description** | Find-and-replace `<MariloGrid` to `<MariloDataGrid`, `<GridColumn` to `<MariloGridColumn`, `<GridColumns>` wrapper removal, `<GridCommandColumn>` to `<MariloGridCommandColumn>` across all spec markdown files. |
| **Covers** | M-01, M-02, S-04, S-05, NM-01, FU-1 through FU-12 (naming portion of each FU lane) |
| **files_owned** | `docs/component-specs/grid/**/*.md` (all ~15 spec markdown files -- tag-level replacements only) |
| **Acceptance criteria** | (1) Zero occurrences of `<MariloGrid `, `<MariloGrid>`, `</MariloGrid>`, `<GridColumn `, `<GridColumns>`, `<GridCommandColumn` in any spec file. (2) All replaced with `<MariloDataGrid`, `<MariloGridColumn`, `<MariloGridCommandColumn`. (3) `<GridColumns>` wrapper removed (columns go directly inside `<MariloDataGrid>`). |
| **Build verification** | No (spec-only) |
| **Wave** | W1 |
| **Effort** | S |

### DG-A-02: Bulk spec rename -- C# code blocks and namespaces

| Field | Value |
|-------|-------|
| **Description** | Replace `MariloGrid<` with `MariloDataGrid<` in all `@code` sections. Replace `@using Marilo.Blazor.Components.Grid` with `@using Marilo.Components.DataGrid`. Fix stale `Marilo.Blazor.*` namespace references. |
| **Covers** | NM-03, NM-05, SRC-05 (C# block + namespace fixes) |
| **files_owned** | `docs/component-specs/grid/**/*.md` (C# code block and @using replacements only) |
| **Acceptance criteria** | (1) Zero occurrences of `MariloGrid<` (without `Data` prefix) in @code sections. (2) Zero occurrences of `Marilo.Blazor.Components.Grid` or `Marilo.Blazor.*` in @using directives. (3) All replaced with `Marilo.Components.DataGrid` or `Marilo.Core.Enums` as appropriate. |
| **Build verification** | No (spec-only) |
| **Wave** | W1 |
| **Effort** | XS |

### DG-A-03: Targeted spec fixes -- type names and phantom types

| Field | Value |
|-------|-------|
| **Description** | Fix `GridCommandEventArgs` to `GridEditEventArgs<TItem>` in editing/overview.md:147. Remove phantom `GridEditorType` reference in editing/overview.md:164. Fix `MariloGrid<Employee> GridRef` in refresh-data.md. |
| **Covers** | NM-04, NM-06, SRC-05 (targeted line fixes) |
| **files_owned** | `docs/component-specs/grid/editing/overview.md`, `docs/component-specs/grid/refresh-data.md` |
| **Acceptance criteria** | (1) `editing/overview.md:147` uses `GridEditEventArgs<TItem>`. (2) `editing/overview.md:164` has no reference to `GridEditorType` in `Marilo.Blazor` namespace. (3) `refresh-data.md` uses `MariloDataGrid<Employee> GridRef`. |
| **Build verification** | No (spec-only) |
| **Wave** | W1 |
| **Effort** | XS |

**W1 Gate:** All spec files pass manual naming audit. No stale `<MariloGrid`/`<GridColumn`/`Marilo.Blazor` references remain.

---

## Wave 2 -- Provider SCSS + Spec Docs + Demos E.1 (Lanes B, D, E.1)

Three parallel tracks after W1 lands. 12 tasks total.

### Track B: Provider Visual Gap (Lane B, 18 rows excl VP-006)

#### DG-B-01: State treatment tokens (FluentUI)

| Field | Value |
|-------|-------|
| **Description** | Fix hover/selected/striped token collisions in FluentUI `_data-grid.scss`. Add `--marilo-datagrid-row-hover-bg`, `--marilo-datagrid-stripe-hover-bg`, `--marilo-datagrid-selected-bg` tokens. Fix dark-mode luminance for selected rows. Fix group header surface token. |
| **Covers** | VP-001, VP-002, VP-003, VP-004, VP-017 |
| **files_owned** | `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss` |
| **Acceptance criteria** | (1) Hover rows visually distinct from default in both light and dark mode. (2) Selected+hover has visible delta from selected. (3) Dark selected rows have sufficient contrast (not invisible against dark surface). (4) Group header uses distinct token from column header. |
| **Build verification** | Yes (SCSS compile) |
| **Wave** | W2 |
| **Effort** | S |

#### DG-B-02: Unstyled selectors -- FluentUI

| Field | Value |
|-------|-------|
| **Description** | Add SCSS rules for sort indicator, pager buttons, empty state, loading overlay, popup edit dialog, and checkbox cell in FluentUI provider. Approximately 135 LOC of new SCSS. |
| **Covers** | VP-007, VP-008, VP-010, VP-011, VP-012, VP-018 |
| **files_owned** | `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss` |
| **Acceptance criteria** | (1) `.mar-datagrid-sort-indicator` styled with icon alignment and muted color. (2) `.mar-datagrid-pager-btn` has full button styling with hover/active/disabled states. (3) `.mar-datagrid-empty` displays centered italic muted message. (4) `.mar-datagrid-loading-overlay` shows centered spinner with semi-transparent scrim. (5) `.mar-datagrid-popup-*` rules render card-like dialog with shadow. (6) `.mar-datagrid-checkbox-cell` centered with Fluent checkbox tokens. |
| **Build verification** | Yes (SCSS compile) |
| **Wave** | W2 |
| **Effort** | M |

#### DG-B-03: Unstyled selectors -- Bootstrap pager

| Field | Value |
|-------|-------|
| **Description** | Add Bootstrap-specific pager button styling using Bootstrap design tokens and `btn-outline-primary` pattern. Also add Bootstrap equivalents for empty state, loading overlay, popup dialog, and checkbox cell. |
| **Covers** | VP-009 (plus Bootstrap equivalents of VP-010, VP-011, VP-012, VP-018) |
| **files_owned** | `src/Marilo.Providers.Bootstrap/Styles/components/_data-grid.scss` |
| **Acceptance criteria** | (1) Bootstrap pager buttons use `.btn-outline-primary` token pattern. (2) Bootstrap empty/loading/popup/checkbox selectors styled with BS tokens. |
| **Build verification** | Yes (SCSS compile) |
| **Wave** | W2 |
| **Effort** | S |

#### DG-B-04: Hardcoded `#fff` token replacement

| Field | Value |
|-------|-------|
| **Description** | Replace all literal `#fff`/`#ffffff`/`rgba(0,0,0,0.12)` values with design tokens in both FluentUI and Bootstrap provider SCSS files. |
| **Covers** | VP-013, VP-014, VP-019 |
| **files_owned** | `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_data-grid.scss` |
| **Acceptance criteria** | (1) Zero literal `#fff`/`#ffffff` in either file. (2) `box-shadow` uses `--elevation-shadow-flyout` token. (3) Dark mode renders correctly after token swap. |
| **Build verification** | Yes (SCSS compile) |
| **Wave** | W2 |
| **Effort** | XS |

#### DG-B-05: Focus treatment (FluentUI + Bootstrap)

| Field | Value |
|-------|-------|
| **Description** | Add `:focus-visible` rules for all interactive DataGrid elements in both providers. FluentUI: `outline: 2px solid var(--focus-stroke-outer)`. Bootstrap: `box-shadow: 0 0 0 0.25rem rgba(var(--bs-primary-rgb), 0.25)`. |
| **Covers** | VP-015 |
| **files_owned** | `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_data-grid.scss` |
| **Acceptance criteria** | (1) Tab-navigating through DataGrid elements shows visible focus ring in FluentUI. (2) Same in Bootstrap. (3) Focus rings appear on rows, cells, pager buttons, command buttons, filter buttons, sortable headers. |
| **Build verification** | Yes (SCSS compile) |
| **Wave** | W2 |
| **Effort** | S |

#### DG-B-06: Typography and Bootstrap runtime tokens

| Field | Value |
|-------|-------|
| **Description** | Add header typography tokens (`font-size`, `letter-spacing`, `text-transform`) to FluentUI. Refactor Bootstrap compile-time Sass striped variable to runtime CSS custom property. |
| **Covers** | VP-005, VP-020 |
| **files_owned** | `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_data-grid.scss` |
| **Acceptance criteria** | (1) FluentUI header cells use `font-size: var(--marilo-font-size-sm)` with uppercase + letter-spacing. (2) Bootstrap striped rows use `var(--marilo-datagrid-stripe-bg)` runtime token instead of compile-time `$table-striped-bg`. |
| **Build verification** | Yes (SCSS compile) |
| **Wave** | W2 |
| **Effort** | XS |

### Track D: Spec-Update Batch (Lane D, 28 rows)

#### DG-D-01: Spec params -- overview.md

| Field | Value |
|-------|-------|
| **Description** | Add missing parameter documentation to `overview.md`: `ShowSearchBox`, `SearchBoxPlaceholder`, `EnableVirtualization`, `VirtualizeOverscanCount`, `Striped`, `AutoGenerateColumns`, grid-level `Resizable`/`Reorderable`, `ColumnWidthProvider`, `SelectionUnit`. |
| **Covers** | U-01, U-02, U-03, U-04, U-05, U-09, SRC-01 |
| **files_owned** | `docs/component-specs/grid/overview.md` |
| **Acceptance criteria** | (1) Each listed parameter appears in the parameters table with type, default, and description. (2) `EnableVirtualization` note references M-03 shape decision (document source shape now). |
| **Build verification** | No (spec-only) |
| **Wave** | W2 |
| **Effort** | S |

#### DG-D-02: Spec fixes -- selection, sorting, columns, events, templates

| Field | Value |
|-------|-------|
| **Description** | Fix type names in selection spec (`GridCellReference<TItem>`). Fix `SortMode` to `GridSortMode` in sorting spec. Document `DisplayFormat`/`Format` precedence in columns. Verify `Locked`/`FrozenPosition`, cell selection, `OnRowDrop` param names. Add events: `OnRowContextMenu`, `OnRowExpand`/`OnRowCollapse` (as `EventCallback<TItem>`), `OnCommand`. Document `GridGroupHeaderContext<TItem>` in templates. |
| **Covers** | NM-02, SRC-02, M-04, M-06, M-07, M-08, M-09, M-10, M-11, M-13, U-06, U-07, U-10, SRC-04 |
| **files_owned** | `docs/component-specs/grid/selection/overview.md`, `docs/component-specs/grid/selection/cells.md`, `docs/component-specs/grid/sorting.md`, `docs/component-specs/grid/editing/overview.md`, `docs/component-specs/grid/columns/frozen.md`, `docs/component-specs/grid/row-drag-drop.md`, `docs/component-specs/grid/events.md`, `docs/component-specs/grid/templates/overview.md` |
| **Acceptance criteria** | (1) `SelectedCells` type is `IEnumerable<GridCellReference<TItem>>` not `GridSelectedCellDescriptor`. (2) `SortMode` -> `GridSortMode`. (3) Edit event args are `GridEditEventArgs<TItem>`. (4) `OnModelInit` shows correct signature. (5) `DisplayFormat` and `Format` both documented with precedence. (6) Param names verified against source for frozen/cell-selection/row-drag-drop. (7) Events page has `OnRowContextMenu`, `OnRowExpand`/`OnRowCollapse`, `OnCommand`. (8) Templates page has `GridGroupHeaderContext<TItem>`. |
| **Build verification** | No (spec-only) |
| **Wave** | W2 |
| **Effort** | M |

#### DG-D-03: Spec additions -- imperative API, refresh-data, misc

| Field | Value |
|-------|-------|
| **Description** | Document 7 public editing methods (`BeginEdit`, `BeginCellEdit`, `BeginAdd`, `SaveEdit`, `CancelEdit`, `DeleteItem`, `ExecuteCommand`). Document `BeginAdd()` silent-failure behavior. Document native `window.confirm` for `DeleteItem`. Document `RowIndex` page-relative semantics. Document detail-row expansion persistence. Document `Class` parameter / `AdditionalAttributes` pattern. Document `PagerButtonCount`. |
| **Covers** | SRC-03, SA-14, SRC-06, SRC-07, SRC-08, S-02, U-08 |
| **files_owned** | `docs/component-specs/grid/editing/overview.md`, `docs/component-specs/grid/refresh-data.md`, `docs/component-specs/grid/overview.md` |
| **Acceptance criteria** | (1) Editing spec has complete imperative API section with all 7 methods. (2) `BeginAdd()` has defensive pattern note. (3) `DeleteItem` documents `window.confirm` behavior. (4) `RowIndex` documented as page-relative. (5) Detail-row persistence behavior documented. (6) `Class`/`AdditionalAttributes` documented. (7) `PagerButtonCount` documented with M-12 cross-reference. |
| **Build verification** | No (spec-only) |
| **Wave** | W2 |
| **Effort** | S |

### Track E.1: Demo-Only Batch (Lane E, 8 rows)

#### DG-E-01: RefreshData demo

| Field | Value |
|-------|-------|
| **Description** | Create new `RefreshData.razor` demo page showing 4 refresh patterns: `Rebind()`, `ObservableCollection`, new collection assignment, `OnRead` + `SetStateAsync`. |
| **Covers** | A-02 |
| **files_owned** | `samples/DataGrid/RefreshData.razor` |
| **Acceptance criteria** | (1) Demo page renders without errors. (2) Four distinct refresh patterns demonstrated. (3) Cross-references `refresh-data.md` spec. |
| **Build verification** | Yes (`dotnet build`) |
| **Wave** | W2 |
| **Effort** | S |

#### DG-E-02: Selection demos -- cell selection + bind shorthand

| Field | Value |
|-------|-------|
| **Description** | Extend D1 Selection demo with cell-selection section (`SelectionUnit="Cell"`, `SelectedCells`, `SelectedCellsChanged`, `GridCellReference` inspection) and `@bind-SelectedItems` shorthand demo. |
| **Covers** | A-03, A-08 |
| **files_owned** | `samples/DataGrid/Selection.razor` |
| **Acceptance criteria** | (1) Cell-selection section shows `SelectionUnit="Cell"` usage. (2) `@bind-SelectedItems` shorthand section compiles and renders. |
| **Build verification** | Yes (`dotnet build`) |
| **Wave** | W2 |
| **Effort** | XS |

#### DG-E-03: Editing demos -- ConfirmDelete + OnAdd/OnCommand + imperative API

| Field | Value |
|-------|-------|
| **Description** | Extend D2 Editing demo with (1) `ConfirmDelete` section, (2) `OnAdd` + `OnCommand` custom command section, (3) imperative API buttons calling `GridRef.BeginEdit`, `SaveEdit`, `CancelEdit`, `DeleteItem`, `BeginAdd`. Add selection+edit combined section and editing guard documentation for cell-selection + InCell combo. |
| **Covers** | A-04, A-05, A-06, A-07, SA-12 |
| **files_owned** | `samples/DataGrid/Editing.razor` |
| **Acceptance criteria** | (1) ConfirmDelete demo renders with `ConfirmDelete="true"`. (2) OnAdd/OnCommand section shows custom `CommandId="Archive"`. (3) Imperative API section has buttons for all 5 methods. (4) Selection+edit combined section works. (5) Cell-selection + InCell guard note present. |
| **Build verification** | Yes (`dotnet build`) |
| **Wave** | W2 |
| **Effort** | S |

**W2 Gate:** SCSS compiles without errors. Spec files have no stale type names or missing parameters. E.1 demo pages render. `dotnet build` passes.

---

## Wave 3 -- Keyboard Engine (Lane C)

Depends on W1 (naming) and W2 DG-B-05 (focus rings). 7 Lane C rows across 5 tasks.

### DG-C-01: Keyboard enums and parameter

| Field | Value |
|-------|-------|
| **Description** | Create `GridKeyboardEnums.cs` with `GridKeyboardScope` (`Grid`, `Row`, `Cell`) and `GridKeyboardCommand` enums. Add `CustomKeyboardShortcuts` dictionary parameter to `MariloDataGrid.razor.cs`. |
| **Covers** | S-03, SA-05 |
| **files_owned** | `src/Marilo.Components/DataGrid/GridKeyboardEnums.cs` (NEW), `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs` |
| **Acceptance criteria** | (1) `GridKeyboardScope` enum has 3 values. (2) `GridKeyboardCommand` enum has 14+ values covering all navigation/edit/selection actions. (3) `CustomKeyboardShortcuts` parameter compiles as `Dictionary<string, GridKeyboardCommand>?`. (4) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`) |
| **Wave** | W3 |
| **Effort** | XS |

### DG-C-02: Keyboard handler and navigation

| Field | Value |
|-------|-------|
| **Description** | Create `MariloDataGrid.Keyboard.cs` partial class with `HandleKeyDown` dispatching to navigation methods. Default bindings: Arrow keys (cell nav), Home/End (row start/end), Ctrl+Home/End (grid start/end), PageUp/PageDown. Add `@onkeydown="HandleKeyDown"` and `tabindex` to `MariloDataGrid.razor`. Track `_focusedRowIndex`/`_focusedColIndex`. |
| **Covers** | SA-06 (default key bindings) |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloDataGrid.Keyboard.cs` (NEW), `src/Marilo.Components/DataGrid/MariloDataGrid.razor` |
| **Acceptance criteria** | (1) Arrow keys navigate between cells. (2) Home/End move to row start/end. (3) Ctrl+Home/End move to grid start/end. (4) PageUp/PageDown navigate pages. (5) `tabindex="0"` on grid root when `Navigable=true`. (6) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W3 |
| **Effort** | M |

### DG-C-03: Data-cell keyboard actions + edit-row keyboard

| Field | Value |
|-------|-------|
| **Description** | Extend `MariloDataGrid.Keyboard.cs` with data-cell actions: Enter/F2 = begin edit, Space = toggle selection, Delete = delete row. Extend `MariloDataGrid.Editing.cs` with edit-row keyboard: Tab/Shift+Tab cycle editors, Enter = save, Esc = cancel. Add focus management to `BeginEdit`/`BeginCellEdit`. Add `FocusCellAsync` JS interop method. |
| **Covers** | SA-07, SA-08 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloDataGrid.Keyboard.cs`, `src/Marilo.Components/DataGrid/MariloDataGrid.Editing.cs`, `src/Marilo.Components/DataGrid/MariloDataGrid.Interop.cs` |
| **Acceptance criteria** | (1) Enter/F2 on focused cell initiates edit. (2) Space toggles row selection. (3) Delete triggers `DeleteItem` with confirmation. (4) Tab/Shift+Tab cycle through editable cells in edit row. (5) Enter saves, Esc cancels edit. (6) `FocusCellAsync` JS interop callable. (7) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W3 |
| **Effort** | M |

### DG-C-04: Keyboard navigation spec rewrite

| Field | Value |
|-------|-------|
| **Description** | Rewrite `keyboard-navigation.md` to document the implemented key bindings accurately. Remove or gate the unimplemented keyboard cheat sheet (honesty fix). Add accurate key binding table, custom shortcuts documentation. |
| **Covers** | A-01 |
| **files_owned** | `docs/component-specs/grid/keyboard-navigation.md` |
| **Acceptance criteria** | (1) Cheat sheet replaced with accurate key binding documentation. (2) Every documented binding matches implementation. (3) Custom shortcuts parameter documented. (4) "Pending" banner removed (or added for not-yet-implemented items). |
| **Build verification** | No (spec-only) |
| **Wave** | W3 |
| **Effort** | S |

### DG-C-05: Keyboard navigation demo

| Field | Value |
|-------|-------|
| **Description** | Update D4 "Navigable Grid" demo to demonstrate actual keyboard behavior. Add sections: cell navigation, edit-row keyboard, custom shortcuts. Remove content that advertises unimplemented features. |
| **Covers** | A-09 |
| **files_owned** | `samples/DataGrid/KeyboardNavigation.razor` |
| **Acceptance criteria** | (1) Demo renders and keyboard navigation works interactively. (2) Cell navigation section demonstrates arrow keys. (3) Edit-row section demonstrates Enter/Tab/Esc flow. (4) Custom shortcuts section shows `CustomKeyboardShortcuts` usage. (5) No claims about unimplemented features. |
| **Build verification** | Yes (`dotnet build`) |
| **Wave** | W3 |
| **Effort** | S |

**W3 Gate:** Keyboard navigation functional in demo. `dotnet build` passes. `dotnet test` passes (keyboard-related tests). Focus rings visible during keyboard navigation (W2 DG-B-05 dependency).

---

## Wave 4 -- Source-Ahead Implementation (Lane G)

14 Lane G rows across 12 tasks. Can start after W1 for source work that does not depend on keyboard engine. Organized by sub-cluster.

### Sub-Cluster G.1: Selection Extensions

#### DG-G-01: Shift-click / Ctrl-click range selection

| Field | Value |
|-------|-------|
| **Description** | Add modifier key detection in `OnRowClick` handler: Shift = range selection from `_lastSelectedIndex`, Ctrl = toggle current row. Track `_lastSelectedIndex`. |
| **Covers** | SA-04 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloDataGrid.Data.cs` |
| **Acceptance criteria** | (1) Ctrl+click toggles individual row selection. (2) Shift+click selects range from last selected to current. (3) Existing single-click selection behavior preserved. (4) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | S |

#### DG-G-02: DragToSelect parameter + JS interop

| Field | Value |
|-------|-------|
| **Description** | Add `DragToSelect` bool parameter. Register mouse event handlers via JS interop for rubber-band cell selection when enabled. |
| **Covers** | SA-01 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs`, `src/Marilo.Components/DataGrid/MariloDataGrid.Interop.cs` |
| **Acceptance criteria** | (1) `DragToSelect` parameter compiles. (2) Mouse drag selects cell range when enabled. (3) Disabled by default. (4) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | M |

#### DG-G-03: GridCheckboxColumn component

| Field | Value |
|-------|-------|
| **Description** | Create `MariloGridCheckboxColumn.razor` with `SelectAll` and `CheckBoxOnlySelection` parameters. Renders header checkbox for select-all, per-row checkboxes. Non-breaking: existing `ShowCheckboxColumn` continues to work. |
| **Covers** | SA-03 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloGridCheckboxColumn.razor` (NEW), `src/Marilo.Components/DataGrid/MariloGridCheckboxColumn.razor.cs` (NEW) |
| **Acceptance criteria** | (1) Component renders header checkbox and per-row checkboxes. (2) `SelectAll` toggles all visible rows. (3) `CheckBoxOnlySelection` restricts selection to checkbox clicks. (4) Existing `ShowCheckboxColumn` bool still works. (5) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | M |

#### DG-G-04: SelectionType parameter

| Field | Value |
|-------|-------|
| **Description** | Add `GridSelectionType` enum (`Row`, `Cell`, `Both`) and flat `SelectionType` parameter to `MariloDataGrid`. Consistent with existing `SelectionUnit` pattern. |
| **Covers** | SA-02 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs` |
| **Acceptance criteria** | (1) `GridSelectionType` enum exists with 3 values. (2) `SelectionType` parameter compiles. (3) Selection behavior changes based on parameter value. (4) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | S |

#### DG-G-05: Clear selection on row-drop

| Field | Value |
|-------|-------|
| **Description** | In `HandleRowDrop()`, clear `_selectedItems` after successful drop to prevent stale selection state. |
| **Covers** | SA-13 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloDataGrid.Data.cs` |
| **Acceptance criteria** | (1) After row drag-drop, `SelectedItems` collection is cleared. (2) No regression in drag-drop behavior. (3) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | XS |

### Sub-Cluster G.2: Editing Extensions

#### DG-G-06: EditorType enum and auto-render

| Field | Value |
|-------|-------|
| **Description** | Create `GridEditorType.cs` enum (9 values). Add `EditorType` parameter to `MariloGridColumn`. Modify editor rendering to auto-select input component based on `EditorType` when `EditorTemplate` is null. |
| **Covers** | SA-09 |
| **files_owned** | `src/Marilo.Components/DataGrid/GridEditorType.cs` (NEW), `src/Marilo.Components/DataGrid/MariloGridColumn.razor`, `src/Marilo.Components/DataGrid/MariloDataGrid.Rendering.cs` |
| **Acceptance criteria** | (1) `GridEditorType` enum has 9 values: TextBox, NumericTextBox, CheckBox, Switch, DatePicker, TimePicker, DropDownList, ComboBox, Custom. (2) Setting `EditorType` on a column auto-renders appropriate input in edit mode. (3) `EditorTemplate` still takes precedence when provided. (4) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | M |

#### DG-G-07: NewRowPosition parameter

| Field | Value |
|-------|-------|
| **Description** | Add `GridNewRowPosition` enum (`Top`, `Bottom`) and `NewRowPosition` parameter. Modify `BeginAdd()` to position new row based on parameter value. |
| **Covers** | SA-10 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs`, `src/Marilo.Components/DataGrid/MariloDataGrid.Editing.cs` |
| **Acceptance criteria** | (1) `NewRowPosition` parameter defaults to `Top`. (2) `BeginAdd()` prepends when `Top`, appends when `Bottom`. (3) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | S |

#### DG-G-08: Template parameters (PopupForm, Buttons, Pager)

| Field | Value |
|-------|-------|
| **Description** | Add `PopupFormTemplate`, `PopupButtonsTemplate`, and `PagerTemplate` RenderFragment parameters. Render in appropriate template slots when provided. |
| **Covers** | S-16 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs`, `src/Marilo.Components/DataGrid/MariloDataGrid.razor` |
| **Acceptance criteria** | (1) All 3 template parameters compile and are `[Parameter]` decorated. (2) Custom templates render in correct slots when provided. (3) Default rendering unchanged when templates are null. (4) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | S |

### Sub-Cluster G.3: Column Features

#### DG-G-09: Toolbar expansion

| Field | Value |
|-------|-------|
| **Description** | Create 13 toolbar tool components (`GridToolBarAdd`, `GridToolBarSave`, `GridToolBarExport`, `GridToolBarSearchBox`, `GridToolBarColumnChooser`, etc.). Each is a small RenderFragment component ~20-30 LOC. Shell `MariloGridToolbar.razor` already exists. |
| **Covers** | S-06 |
| **files_owned** | `src/Marilo.Components/DataGrid/Toolbar/` (NEW directory + ~13 new .razor files) |
| **Acceptance criteria** | (1) All 13 toolbar components compile. (2) Each renders appropriate UI inside the toolbar shell. (3) `MariloGridToolbar` renders child tool components. (4) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | L |

#### DG-G-10: Multi-column headers (GridColumnGroup)

| Field | Value |
|-------|-------|
| **Description** | Create `MariloGridColumnGroup` component that wraps multiple `MariloGridColumn` children. Renders nested `<th colspan>` in header. |
| **Covers** | S-11 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloGridColumnGroup.razor` (NEW), `src/Marilo.Components/DataGrid/MariloGridColumnGroup.razor.cs` (NEW) |
| **Acceptance criteria** | (1) `MariloGridColumnGroup` wraps child columns. (2) Header renders with correct `colspan`. (3) Existing single-level headers unaffected. (4) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | M |

#### DG-G-11: Column menu + column chooser + checkbox-list filter

| Field | Value |
|-------|-------|
| **Description** | Add context menu on header cells with sort/filter/hide options. Column chooser panel with visibility toggles. Add `FilterType` parameter to `MariloGridColumn` with `CheckboxList` option. Public parameters for `_checkBoxFilter*` internal state. |
| **Covers** | S-12, S-17 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloGridColumn.razor`, `src/Marilo.Components/DataGrid/MariloGridColumn.razor.cs`, `src/Marilo.Components/DataGrid/MariloDataGrid.razor` |
| **Acceptance criteria** | (1) Right-click on header shows context menu with sort/filter/hide. (2) Column chooser panel lists all columns with toggles. (3) `FilterType.CheckboxList` renders checkbox filter UI. (4) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | L |

### Sub-Cluster G.4: Misc

#### DG-G-12: HighlightedItems parameter

| Field | Value |
|-------|-------|
| **Description** | Add `HighlightedItems` parameter (`IEnumerable<TItem>?`). Apply `mar-datagrid-row--highlighted` CSS class to matching rows. Add SCSS rule for highlighted state. |
| **Covers** | S-14 |
| **files_owned** | `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs`, `src/Marilo.Components/DataGrid/MariloDataGrid.razor` |
| **Acceptance criteria** | (1) `HighlightedItems` parameter compiles. (2) Rows matching items in the collection get `mar-datagrid-row--highlighted` class. (3) `dotnet build` passes. |
| **Build verification** | Yes (`dotnet build`, `dotnet test`) |
| **Wave** | W4 |
| **Effort** | XS |

**W4 Gate:** All new components/parameters compile. `dotnet build` passes. `dotnet test` passes for all DataGrid tests. No regressions in existing functionality.

---

## Wave 5 -- Source-Blocked Demos + Gap Plan Update (Lane E.2 + closure)

Depends on W3 (keyboard) and W4 (source features). 5 tasks.

### DG-E-04: Keyboard demo scenarios (post-engine)

| Field | Value |
|-------|-------|
| **Description** | Add remaining keyboard navigation demo scenarios that were blocked on keyboard engine: advanced cell navigation patterns, keyboard + selection integration. |
| **Covers** | A-09 (extended scenarios beyond DG-C-05 basics) |
| **files_owned** | `samples/DataGrid/KeyboardNavigation.razor` |
| **Acceptance criteria** | (1) Demo covers all implemented keyboard commands. (2) Interactive scenarios work end-to-end. |
| **Build verification** | Yes (`dotnet build`) |
| **Wave** | W5 |
| **Effort** | XS |

### DG-E-05: Selection modifier demos

| Field | Value |
|-------|-------|
| **Description** | Add Shift-click, Ctrl-click, and drag-to-select demo sections to the Selection demo page. Demonstrate `GridCheckboxColumn` with `SelectAll`. |
| **Covers** | A-10, A-11 |
| **files_owned** | `samples/DataGrid/Selection.razor` |
| **Acceptance criteria** | (1) Shift-click range selection demonstrated. (2) Ctrl-click toggle demonstrated. (3) Drag-to-select section present (when `DragToSelect=true`). (4) `GridCheckboxColumn` with `SelectAll` demonstrated. |
| **Build verification** | Yes (`dotnet build`) |
| **Wave** | W5 |
| **Effort** | S |

### DG-E-06: EditorType + NewRowPosition demos

| Field | Value |
|-------|-------|
| **Description** | Add `EditorType` enum demo showing different editor types per column. Add `NewRowPosition` demo showing top vs bottom placement. |
| **Covers** | A-12, A-13 |
| **files_owned** | `samples/DataGrid/Editing.razor` |
| **Acceptance criteria** | (1) EditorType demo shows columns with different auto-rendered editors. (2) NewRowPosition demo shows `Top` vs `Bottom` placement for new rows. |
| **Build verification** | Yes (`dotnet build`) |
| **Wave** | W5 |
| **Effort** | XS |

### DG-E-07: Excel/PDF export placeholder

| Field | Value |
|-------|-------|
| **Description** | If S-08 (Excel + PDF export) is implemented in W4, add export demo. Otherwise, add placeholder with note referencing S-08 status. |
| **Covers** | S-08 (demo portion, conditional on implementation) |
| **files_owned** | `samples/DataGrid/Export.razor` (NEW, conditional) |
| **Acceptance criteria** | (1) If S-08 landed: demo shows Excel and PDF export buttons. (2) If S-08 deferred: placeholder page documents planned export capabilities. |
| **Build verification** | Yes (`dotnet build`, conditional) |
| **Wave** | W5 |
| **Effort** | XS |

### DG-E-08: Gap plan update and closure tracking

| Field | Value |
|-------|-------|
| **Description** | Update `_config/gap-context.md` and `GAP_ANALYSIS_RESOLUTION_PLAN.md` with closure status for all implemented items. Mark Lane F + VP-006 as still-blocked. Mark Lane H as deferred. |
| **Covers** | Gap-plan sync area for all lanes |
| **files_owned** | `ICM/workspaces/datagrid-gap-analysis/_config/gap-context.md` |
| **Acceptance criteria** | (1) Every implemented task has closure status recorded. (2) Lane F items marked blocked with escalation reference. (3) Lane H items marked deferred. (4) Row counts verified against S01 inventory. |
| **Build verification** | No |
| **Wave** | W5 |
| **Effort** | S |

**W5 Gate:** All demo pages render. Gap plan updated. `dotnet build` passes. No claims about unimplemented features in any demo.

---

## Task Summary Table

| ID | Wave | Lane | Description | Effort | Build? | Covers (row count) |
|----|------|------|-------------|--------|--------|---------------------|
| DG-A-01 | W1 | A | Bulk spec rename -- tags | S | No | 18 |
| DG-A-02 | W1 | A | Bulk spec rename -- C# + namespaces | XS | No | 3 |
| DG-A-03 | W1 | A | Targeted spec fixes -- types | XS | No | 3 |
| DG-B-01 | W2 | B | State treatment tokens (FluentUI) | S | Yes | 5 |
| DG-B-02 | W2 | B | Unstyled selectors (FluentUI) | M | Yes | 6 |
| DG-B-03 | W2 | B | Unstyled selectors (Bootstrap) | S | Yes | 5 |
| DG-B-04 | W2 | B | Hardcoded #fff replacement | XS | Yes | 3 |
| DG-B-05 | W2 | B | Focus treatment (both providers) | S | Yes | 1 |
| DG-B-06 | W2 | B | Typography + Bootstrap runtime | XS | Yes | 2 |
| DG-D-01 | W2 | D | Spec params -- overview | S | No | 7 |
| DG-D-02 | W2 | D | Spec fixes -- multi-file | M | No | 14 |
| DG-D-03 | W2 | D | Spec additions -- imperative/misc | S | No | 7 |
| DG-E-01 | W2 | E.1 | RefreshData demo | S | Yes | 1 |
| DG-E-02 | W2 | E.1 | Selection demos | XS | Yes | 2 |
| DG-E-03 | W2 | E.1 | Editing demos | S | Yes | 5 |
| DG-C-01 | W3 | C | Keyboard enums + parameter | XS | Yes | 2 |
| DG-C-02 | W3 | C | Keyboard handler + navigation | M | Yes | 1 |
| DG-C-03 | W3 | C | Data-cell + edit-row keyboard | M | Yes | 2 |
| DG-C-04 | W3 | C | Keyboard nav spec rewrite | S | No | 1 |
| DG-C-05 | W3 | C | Keyboard nav demo | S | Yes | 1 |
| DG-G-01 | W4 | G | Shift-click / Ctrl-click | S | Yes | 1 |
| DG-G-02 | W4 | G | DragToSelect + JS interop | M | Yes | 1 |
| DG-G-03 | W4 | G | GridCheckboxColumn | M | Yes | 1 |
| DG-G-04 | W4 | G | SelectionType parameter | S | Yes | 1 |
| DG-G-05 | W4 | G | Clear selection on row-drop | XS | Yes | 1 |
| DG-G-06 | W4 | G | EditorType enum + auto-render | M | Yes | 1 |
| DG-G-07 | W4 | G | NewRowPosition parameter | S | Yes | 1 |
| DG-G-08 | W4 | G | Template parameters | S | Yes | 1 |
| DG-G-09 | W4 | G | Toolbar expansion (13 components) | L | Yes | 1 |
| DG-G-10 | W4 | G | Multi-column headers | M | Yes | 1 |
| DG-G-11 | W4 | G | Column menu + chooser + filter | L | Yes | 2 |
| DG-G-12 | W4 | G | HighlightedItems | XS | Yes | 1 |
| DG-E-04 | W5 | E.2 | Keyboard demo scenarios (extended) | XS | Yes | 1 |
| DG-E-05 | W5 | E.2 | Selection modifier demos | S | Yes | 2 |
| DG-E-06 | W5 | E.2 | EditorType + NewRowPosition demos | XS | Yes | 2 |
| DG-E-07 | W5 | E.2 | Export demo (conditional) | XS | Yes | 1 |
| DG-E-08 | W5 | -- | Gap plan update + closure | S | No | all |

**Total: 37 tasks covering 101 active rows** (104 designed - 3 Lane H deferred).

---

## Effort Distribution

| Effort | Count | % |
|--------|------:|---|
| XS | 12 | 32% |
| S | 14 | 38% |
| M | 8 | 22% |
| L | 3 | 8% |

---

## Blocked Items (Not Scheduled)

| Blocker | Items | Type | Unblocked By |
|---------|-------|------|--------------|
| Lane F shape decisions | M-03, M-05, M-12, S-07, S-09, S-10, S-15, SA-11 | User decision | Orchestrator escalation resolved |
| VP-006 density | VP-006 | Public API change | Orchestrator escalation resolved |
| Lane H deferred | VP-016, S-13, S-01 | Future wave | Separate implementation track |

---

## Row Coverage Verification

| Source | Rows | Status |
|--------|-----:|--------|
| Lane A (W1) | 22 | 3 tasks: DG-A-01, DG-A-02, DG-A-03 |
| Lane B (W2, excl VP-006) | 18 | 6 tasks: DG-B-01 through DG-B-06 |
| Lane C (W3) | 7 | 5 tasks: DG-C-01 through DG-C-05 |
| Lane D (W2) | 28 | 3 tasks: DG-D-01 through DG-D-03 |
| Lane E (W2+W5) | 12 | 7 tasks: DG-E-01 through DG-E-03 (W2), DG-E-04 through DG-E-07 (W5) |
| Lane G (W4) | 14 | 12 tasks: DG-G-01 through DG-G-12 |
| Lane H (deferred) | 3 | No tasks (future wave) |
| Lane F (blocked) | 8 | No tasks (blocked on decisions) |
| VP-006 (blocked) | 1 | No task (blocked on decision) |
| **Scheduled** | **101** | **37 tasks** |
| **Blocked/Deferred** | **12** | **Not scheduled** |
| **Total** | **113** | **All accounted for** |

---

## Checkpoint

This is the end of Stage 04 (remediation-plan). All 113 inventory rows are accounted for: 101 scheduled across 37 atomic tasks in 5 waves, 9 blocked on decisions, 3 deferred. **STOP here.** Worker sets status to `review-pending` and writes result + handoff.
