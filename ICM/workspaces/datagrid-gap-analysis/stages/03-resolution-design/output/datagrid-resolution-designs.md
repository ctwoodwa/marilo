# DataGrid Resolution Designs -- Stage 03

**Worker:** `w-datagrid-gap-analysis`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Stage:** 03-resolution-design
**Input:** `stages/02-prioritize/output/datagrid-priority-lanes.md` (8 lanes, 113 rows)
**Date:** 2026-04-12

---

## Skipped Items

Per orchestrator dispatch (Tick 11):

- **Lane F** (8 rows: M-03, M-05, M-12, S-07, S-09, S-10, S-15, SA-11) -- SKIP entirely. Blocked on user shape decisions (M-03 virtual-scrolling, M-05 GridState generic, M-12 Pager shape).
- **VP-006** (density public API) in Lane B -- SKIP. Blocked on user decision for `Density` parameter.

---

## Lane A -- Naming-Cascade (22 rows)

### Resolution Summary

Bulk find-and-replace across all spec markdown files under `docs/component-specs/grid/`. The tick-8 decision is definitive: spec-side rename only, no source changes. All `<MariloGrid>` tags become `<MariloDataGrid>`, all `<GridColumn>` become `<MariloGridColumn>`, etc.

### Detailed Changes

| # | Target File(s) | Change Nature | Details |
|--:|----------------|---------------|---------|
| 1 | `docs/component-specs/grid/**/*.md` (all ~15+ spec files) | Bulk text replacement in markup blocks | `<MariloGrid` -> `<MariloDataGrid`, `</MariloGrid>` -> `</MariloDataGrid>`, `<GridColumn` -> `<MariloGridColumn`, `</GridColumn>` -> `</MariloGridColumn>`, `<GridColumns>` -> `<MariloGridColumns>` (or remove wrapper if source takes `ChildContent` directly -- see note below) |
| 2 | `docs/component-specs/grid/**/*.md` (C# code blocks) | Bulk text replacement in `@code` sections | `MariloGrid<` -> `MariloDataGrid<` (covers `MariloGrid<Employee>`, `MariloGrid<T>`, etc.) |
| 3 | `docs/component-specs/grid/**/*.md` (@using directives) | Namespace correction | `@using Marilo.Blazor.Components.Grid` -> `@using Marilo.Components.DataGrid`, `Marilo.Blazor.*` -> correct `Marilo.Core.Enums` / `Marilo.Components.DataGrid` |
| 4 | `docs/component-specs/grid/keyboard-navigation.md:231` (NM-05) | Targeted namespace fix | Stale `Marilo.Blazor.*` slug references -> `Marilo.Components.DataGrid` / `Marilo.Core.Enums` |
| 5 | `docs/component-specs/grid/editing/overview.md:147` (NM-04) | Type name fix | `GridCommandEventArgs` -> `GridEditEventArgs<TItem>` for OnCreate/OnUpdate/OnDelete/OnEdit/OnCancel callbacks |
| 6 | `docs/component-specs/grid/editing/overview.md:164` (NM-06) | Phantom type removal | Remove reference to `GridEditorType` in `Marilo.Blazor` namespace (enum does not exist; SA-09 tracks future creation) |
| 7 | `docs/component-specs/grid/refresh-data.md:25,48` (SRC-05, NM-03) | Snippet correction | `MariloGrid<Employee> GridRef` -> `MariloDataGrid<Employee> GridRef`; tag in markup `<MariloGrid` -> `<MariloDataGrid` |
| 8 | `docs/component-specs/grid/**/*.md` | `<GridCommandColumn>` -> `<MariloGridCommandColumn>` | Source has `MariloGridCommandButton.razor`; spec references `GridCommandColumn`. Per tick-8: if spec expects a column-shaped API, the spec tag becomes `<MariloGridCommandColumn>` but the actual component may need to be created (S-05 source gap tracked in Lane G/C). For now, spec uses the correct Marilo-prefixed name. |

### Note on `<MariloGridColumns>` Wrapper

Source uses `ChildContent` directly on `MariloDataGrid<TItem>` (no wrapper element). The spec references `<GridColumns>` as a wrapper. Two options:

- **Option A (recommended):** Remove `<GridColumns>` / `<MariloGridColumns>` wrapper from spec snippets -- columns go directly inside `<MariloDataGrid>`. Matches current source.
- **Option B:** Create a `MariloGridColumns` pass-through `RenderFragment` component in source. Adds no value since `ChildContent` already works.

**Recommendation:** Option A. Delete the wrapper from all spec snippets.

### Breaking Change Assessment

**No breaking changes.** Spec-only edits. Source is untouched.

### Effort Estimate

**S** (Small) -- Mechanical find-and-replace with manual verification of ~15 markdown files. Approximately 0.5 worker day.

### Inter-Lane Dependencies

None. Lane A is the root -- it unblocks all other lanes.

---

## Lane B -- Provider Visual Gap Batch (18 rows, excl VP-006)

### Resolution Summary

SCSS-only changes across FluentUI and Bootstrap providers to fix 18 visual-parity gaps. Organized into 4 active sub-lanes (B.1-B.4; B.5 partially skipped due to VP-006).

### Sub-Lane B.1 -- State Treatment Tokens (VP-001, VP-002, VP-003, VP-004, VP-017)

**Root cause:** Hover, selected, and striped states reuse the same token values, producing zero visual delta in various theme/mode combinations.

| # | ID | File | Change | Details |
|--:|-----|------|--------|---------|
| 1 | VP-001 | `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss` L30 | Modify hover rule | `.mar-datagrid-row:hover` currently uses `var(--marilo-color-surface)` which is identical to the header background. Change to `var(--marilo-color-surface-hover)` or define a new `--marilo-datagrid-row-hover-bg` token. |
| 2 | VP-002 | `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss` | Add dark-mode state-layer token | Add dark-mode override: `.mar-datagrid-row--striped:nth-child(even):hover` needs a distinct token. Introduce `--marilo-datagrid-stripe-hover-bg` with `color-mix(in srgb, var(--marilo-color-surface) 85%, var(--marilo-color-primary) 15%)` for dark mode. |
| 3 | VP-003 | `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss` L31-32 | Fix selected+hover | `.mar-datagrid-row--selected:hover` currently duplicates `--marilo-color-primary-light`. Add a `:hover` delta: `color-mix(in srgb, var(--marilo-color-primary-light) 90%, var(--marilo-color-primary) 10%)`. Add left accent border: `border-left: 3px solid var(--marilo-color-primary)`. |
| 4 | VP-004 | `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss` L31 | Fix dark selected row visibility | `--marilo-color-primary-light` at `#0a2e4a` has insufficient contrast against `#1b1a19`. Define dark-mode override for `--marilo-datagrid-selected-bg` with higher luminance delta, e.g. `color-mix(in srgb, var(--marilo-color-primary) 25%, var(--marilo-color-surface) 75%)`. |
| 5 | VP-017 | `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss` L82-85 | Fix group header token | `.mar-datagrid-group-header` uses `var(--marilo-color-surface)` same as column header. Change to `var(--marilo-color-surface-variant)` or add left indent + accent. |

**Approach:** Create a `_data-grid-tokens.scss` partial (or extend existing) with DataGrid-specific design tokens. Both FluentUI and Bootstrap providers inherit the token layer; overrides happen at the provider level.

### Sub-Lane B.2 -- Unstyled-Selector Cluster (VP-007, VP-008, VP-009, VP-010, VP-011, VP-012, VP-018)

**Root cause:** CSS classes are emitted by the Razor templates but have zero corresponding SCSS rules. User-agent defaults render.

| # | ID | File | Change | Details |
|--:|-----|------|--------|---------|
| 1 | VP-007 | FluentUI `_data-grid.scss` | Add new rules | `.mar-datagrid-sort-indicator` / `.mar-datagrid-sort-order`: add `display: inline-flex; align-items: center; margin-left: 0.25rem; font-size: 0.75rem; color: var(--marilo-color-text-muted);` |
| 2 | VP-008 | FluentUI `_data-grid.scss` | Add new rules (~30 LOC) | `.mar-datagrid-pager-btn`, `.mar-datagrid-pager-btn--active`, `.mar-datagrid-pager-btn:hover`, `.mar-datagrid-pager-btn:disabled`: full button styling with Fluent design tokens (border, radius, padding, hover/active/disabled states). |
| 3 | VP-009 | Bootstrap `_data-grid.scss` | Add new rules (~25 LOC) | Same pager button selectors with Bootstrap design tokens. Reuse `.btn-outline-primary` token pattern from Bootstrap. |
| 4 | VP-010 | FluentUI `_data-grid.scss` | Add new rules (~15 LOC) | `.mar-datagrid-empty`: `display: flex; align-items: center; justify-content: center; min-height: 120px; color: var(--marilo-color-text-muted); font-style: italic; padding: var(--marilo-space-xl);` |
| 5 | VP-011 | FluentUI `_data-grid.scss` | Add new rules (~20 LOC) | `.mar-datagrid-loading-overlay`: `position: absolute; inset: 0; display: flex; align-items: center; justify-content: center; background: color-mix(in srgb, var(--marilo-color-surface) 80%, transparent); z-index: 10;`. `.mar-datagrid-loading-spinner`: keyframe animation. |
| 6 | VP-012 | FluentUI `_data-grid.scss` | Add new rules (~40 LOC) | `.mar-datagrid-popup-overlay`: scrim layer. `.mar-datagrid-popup-form`: card-like container with `border-radius`, `box-shadow: var(--elevation-shadow-dialog)`, padding, max-width. `.mar-datagrid-popup-header`, `.mar-datagrid-popup-body`, `.mar-datagrid-popup-footer`: layout rules. Reuse `patterns/_overlay.scss` mixin if available. |
| 7 | VP-018 | FluentUI `_data-grid.scss` | Add new rules (~10 LOC) | `.mar-datagrid-checkbox-cell`: `display: flex; align-items: center; justify-content: center; width: 40px;`. Style the inner `<input type="checkbox">` with Fluent checkbox tokens. |

**Bootstrap equivalents:** VP-009 is the only Bootstrap-specific item here. VP-010, VP-011, VP-012, VP-018 also need Bootstrap equivalents. Add corresponding rules to `src/Marilo.Providers.Bootstrap/Styles/components/_data-grid.scss` using Bootstrap token conventions.

### Sub-Lane B.3 -- Hardcoded `#fff` Literals (VP-013, VP-014, VP-019)

**Root cause:** Literal `#fff` / `#ffffff` / `rgba(0,0,0,0.12)` values break dark mode.

| # | ID | File | Change | Details |
|--:|-----|------|--------|---------|
| 1 | VP-013 | FluentUI `_data-grid.scss` L128,160,176,189 | Token replacement | All `#fff` / `#ffffff` in `color-mix()` bases and `background:` -> `var(--marilo-color-surface)`. In filter menu operator/value inputs (L176): `background: #fff` -> `background: var(--marilo-color-input-bg, var(--marilo-color-surface))`. |
| 2 | VP-014 | FluentUI `_data-grid.scss` L160 | Token replacement | `box-shadow: 0 8px 24px rgba(0,0,0,0.12)` -> `box-shadow: var(--elevation-shadow-flyout, 0 8px 24px rgba(0,0,0,0.12))`. |
| 3 | VP-019 | Bootstrap `_data-grid.scss` L72,99 | Token replacement | `background: #fff` -> `background: var(--marilo-color-surface)`. Three selectors in Bootstrap filter menu. |

### Sub-Lane B.4 -- Focus Treatment (VP-015)

**Root cause:** Zero `:focus` / `:focus-visible` rules on any DataGrid interactive element. `--focus-stroke-outer` token exists but is unused.

| File | Change | Details |
|------|--------|---------|
| FluentUI `_data-grid.scss` | Add focus rules (~25 LOC) | Global focus-visible rule for all interactive DataGrid elements: `.mar-datagrid-row[tabindex]:focus-visible`, `.mar-datagrid-cell[tabindex]:focus-visible`, `.mar-datagrid-pager-btn:focus-visible`, `.mar-datagrid-cmd-btn:focus-visible`, `.mar-datagrid-filter-menu-btn:focus-visible`, `.mar-datagrid-header-cell--sortable:focus-visible`. Apply: `outline: 2px solid var(--focus-stroke-outer); outline-offset: -2px; border-radius: var(--marilo-radius-sm);`. |
| Bootstrap `_data-grid.scss` | Add focus rules (~15 LOC) | Same selectors, using Bootstrap focus ring convention: `box-shadow: 0 0 0 0.25rem rgba(var(--bs-primary-rgb), 0.25);`. |

### Sub-Lane B.5 -- Typography/Density/Elevation (VP-005, VP-020) [VP-006 SKIPPED]

| # | ID | File | Change | Details |
|--:|-----|------|--------|---------|
| 1 | VP-005 | FluentUI `_data-grid.scss` L14-20 | Add header typography tokens | Add `font-size: var(--marilo-font-size-sm); letter-spacing: 0.02em; text-transform: uppercase;` to `.mar-datagrid-header` or `.mar-datagrid-header-cell`. |
| 2 | VP-020 | Bootstrap `_data-grid.scss` L33-36 | Refactor compile-time Sass to runtime | Replace `#{$table-striped-bg}` with `var(--marilo-datagrid-stripe-bg, var(--bs-table-striped-bg))` so dark-theme toggle can override at runtime. |

### Breaking Change Assessment

**No breaking changes.** All changes are additive SCSS rules or token replacements. No public API changes.

### Effort Estimate

**M** (Medium) -- ~200-250 LOC of new SCSS across 2 provider files plus a potential tokens partial. Approximately 1.5-2 worker days.

### Inter-Lane Dependencies

- **Depends on Lane A:** Any spec snippets touched during VP work inherit the naming cascade (minor -- VP work is SCSS-only, but if spec docs reference styling, they need correct names).
- **Feeds Lane C:** VP-015 focus rings must land before keyboard engine is visually testable.

---

## Lane C -- Keyboard Engine (7 rows)

### Resolution Summary

Implement the keyboard navigation engine for `MariloDataGrid`. This is the largest source-change lane. Requires a new partial class file, enum definitions, default key bindings, and focus management.

### Detailed Changes

| # | ID | File(s) | Change Nature | Details |
|--:|-----|---------|---------------|---------|
| 1 | SA-06 | `src/Marilo.Components/DataGrid/MariloDataGrid.Keyboard.cs` (NEW) | New partial class file (~150 LOC) | `onkeydown` handler dispatching to action methods. Default key bindings: Arrow keys (cell navigation), Home/End (row start/end), Ctrl+Home/End (grid start/end), PageUp/PageDown (page navigation). Sets `tabindex="0"` on the grid root via `MariloDataGrid.razor`. Updates `_focusedRowIndex` / `_focusedColIndex` internal state. Calls `JS.InvokeVoidAsync("mariloDataGridFocus", elementRef)` for DOM focus management. |
| 2 | SA-07 | `src/Marilo.Components/DataGrid/MariloDataGrid.Keyboard.cs` | Data-cell keyboard actions | Enter/F2 = begin edit on focused cell (delegates to `BeginEdit` / `BeginCellEdit`). Space = toggle selection on focused row. Delete/Backspace = call `DeleteItem` on focused row (with confirmation if enabled). |
| 3 | SA-08 | `src/Marilo.Components/DataGrid/MariloDataGrid.Editing.cs` | Extend existing file (~30 LOC) | Edit-row keyboard: Tab/Shift+Tab cycle through editable cells in the edit row. Enter = save edit. Esc = cancel edit. Add focus management to `BeginEdit`/`BeginCellEdit` to focus the first editor input. |
| 4 | S-03 / SA-05 | `src/Marilo.Components/DataGrid/GridKeyboardEnums.cs` (NEW) | New enum file (~40 LOC) | `GridKeyboardScope` enum (`Grid`, `Row`, `Cell`). `GridKeyboardCommand` enum (`NavigateUp`, `NavigateDown`, `NavigateLeft`, `NavigateRight`, `Home`, `End`, `PageUp`, `PageDown`, `Edit`, `Select`, `Delete`, `Save`, `Cancel`, `Tab`, `ShiftTab`). |
| 5 | S-03 | `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs` | Add parameter (~5 LOC) | `[Parameter] public Dictionary<string, GridKeyboardCommand>? CustomKeyboardShortcuts { get; set; }` -- allows consumers to override or extend default bindings. |
| 6 | A-01 | `docs/component-specs/grid/keyboard-navigation.md` | Spec update | Remove or gate the keyboard cheat sheet that advertises unimplemented behavior. Replace with accurate documentation of the implemented key bindings. If implementation is partial at demo time, add a "Pending" banner. |
| 7 | A-09 | Demo files (e.g. `DataGrid/KeyboardNavigation.razor`) | Demo update/creation | Update D4 "Navigable Grid" demo to demonstrate actual keyboard behavior. Add sections for: cell navigation, edit-row keyboard, custom shortcuts. Remove cheat sheet that lists unimplemented features. |
| 8 | -- | `src/Marilo.Components/DataGrid/MariloDataGrid.razor` | Modify template (~10 LOC) | Add `@onkeydown="HandleKeyDown"` to grid root `<div>`. Add `tabindex="@(Navigable ? 0 : -1)"`. Track `_focusedRowIndex` / `_focusedColIndex` for `aria-activedescendant`. |
| 9 | -- | `src/Marilo.Components/DataGrid/MariloDataGrid.Interop.cs` | Add JS interop method (~10 LOC) | `FocusCellAsync(int rowIndex, int colIndex)` -- calls JS to set DOM focus on the target cell element. |

### Breaking Change Assessment

**No breaking changes.** All additions are:
- New files (`.Keyboard.cs`, `GridKeyboardEnums.cs`)
- New additive parameters (`CustomKeyboardShortcuts`)
- Template attribute additions (`@onkeydown`, `tabindex`)
- Existing `Navigable` parameter already exists but had no implementation -- this fulfills its contract.

### Effort Estimate

**L** (Large) -- New keyboard engine (~200 LOC source), JS interop, enum definitions, spec rewrite, demo rework. Approximately 2-3 worker days.

### Inter-Lane Dependencies

- **Depends on Lane A:** Spec/demo files need naming cascade first.
- **Depends on Lane B VP-015:** Focus rings must be styled for keyboard to be visually meaningful.
- **Feeds Lane E:** A-09 keyboard demo scenarios unblock once keyboard engine lands.

---

## Lane D -- Spec-Update Batch (28 rows)

### Resolution Summary

Pure documentation work. Update ~12 spec markdown files to document existing source parameters/behaviors. No source changes. Every edit inherits Lane A naming cascade.

### Detailed Changes

Organized by spec file touched:

#### `docs/component-specs/grid/overview.md`

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 1 | U-01 | Add parameter docs | Add `ShowSearchBox` (bool) and `SearchBoxPlaceholder` (string) to the parameters table. Source: `MariloDataGrid.razor.cs` L153-157. |
| 2 | U-02 | Add parameter docs | Add `EnableVirtualization` (bool) and `VirtualizeOverscanCount` (int, default 5) to parameters table. Source: L145-148. Note: paired with M-03 shape decision in Lane F -- document the source shape now; if shape changes later, update. |
| 3 | U-03 | Add parameter docs | Add `Striped` (bool) to appearance section. Source: L130. |
| 4 | U-04 | Add parameter docs | Add `AutoGenerateColumns` (bool) with description. Source: L142. |
| 5 | U-05 | Add parameter docs | Add grid-level `Resizable` and `Reorderable` bools (if they exist in source -- verify). Column-level already documented. |
| 6 | U-09 | Add parameter docs | Add `ColumnWidthProvider` / `IColumnWidthProvider` to sizing section. Source: L160. |
| 7 | SRC-01 | Add parameter docs | Add `SelectionUnit` to selection overview. Source: L110. |

#### `docs/component-specs/grid/selection/overview.md`

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 8 | NM-02 | Fix type name | Change `IEnumerable<GridSelectedCellDescriptor>` -> `IEnumerable<GridCellReference<TItem>>` to match source L113. |
| 9 | SRC-02 | Fix type reference | Update `SelectedCells` type to `IEnumerable<GridCellReference<TItem>>` (after NM-02 resolved). |

#### `docs/component-specs/grid/sorting.md`

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 10 | M-04 | Fix enum name | Update references from `SortMode` to `GridSortMode` to match source L90. |

#### `docs/component-specs/grid/editing/overview.md`

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 11 | M-06 | Fix event-args type | Change `GridCommandEventArgs` -> `GridEditEventArgs<TItem>` for OnCreate/OnUpdate/OnDelete/OnEdit/OnCancel. Source: `GridCommandTypes.cs` L52-59. |
| 12 | M-07 | Fix OnModelInit signature | Update spec to show `EventCallback<GridModelInitEventArgs<TItem>>` pattern. Source: `MariloDataGrid.razor.cs` L231. |
| 13 | SRC-03 | Document imperative API | Add new section documenting 7 public methods: `BeginEdit`, `BeginCellEdit`, `BeginAdd`, `SaveEdit`, `CancelEdit`, `DeleteItem`, `ExecuteCommand`. Source: `MariloDataGrid.Editing.cs` L13-23. |
| 14 | SA-14 | Document BeginAdd silent failure | Add note about `BeginAdd()` behavior when `OnModelInit` is not wired (assigns null/default). Recommend defensive pattern. |

#### `docs/component-specs/grid/columns/*.md`

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 15 | M-08 | Document DisplayFormat | Show both `DisplayFormat` (composite format, e.g. `"{0:C2}"`) and `Format` (simple format, e.g. `"C2"`) with precedence rule. Source: `MariloGridColumn.razor` L64-65, L82-87. |
| 16 | M-09 | Verify frozen params | Verify `Locked` and `FrozenPosition` parameter names in `columns/frozen.md` match source L58-62. Update if needed. |

#### `docs/component-specs/grid/selection/cells.md`

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 17 | M-10 | Verify cell selection params | Verify `SelectedCells`, `SelectedCellsChanged`, `GridCellReference<TItem>` names match source L112-116. |

#### `docs/component-specs/grid/row-drag-drop.md`

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 18 | M-11 | Verify OnRowDrop | Verify `OnRowDrop`, `GridRowDropEventArgs<TItem>`, `RowDraggable` match source L252-255. |

#### `docs/component-specs/grid/events.md`

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 19 | U-06 | Add event docs | Add `OnRowContextMenu` to events page. Source: L196. |
| 20 | U-07 | Add event docs | Add `OnRowExpand` / `OnRowCollapse` with note that args are `EventCallback<TItem>` (not typed event-args). Source: L202-205. |
| 21 | M-13 | Fix event-args type | Update expand/collapse args from `GridRowExpandEventArgs<TItem>` to plain `EventCallback<TItem>` to match source. |
| 22 | SRC-04 | Document OnCommand | Add `OnCommand` with `GridCommandEventArgs<TItem>` type. Source: L234, `GridCommandTypes.cs` L73-83. |

#### `docs/component-specs/grid/templates/*.md`

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 23 | U-10 | Document context type | Add `GridGroupHeaderContext<TItem>` to GroupHeaderTemplate/GroupFooterTemplate documentation. Source: `GridEventArgs.cs` L127-164. |

#### `docs/component-specs/grid/refresh-data.md`

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 24 | SRC-07 | Clarify RowIndex | Add note that `GridCellReference<TItem>.RowIndex` is page-relative, not absolute. Source: `GridCellReference.cs`. |
| 25 | SRC-08 | Document detail-row persistence | Add note about detail-row expansion behavior across `Rebind()` / `ProcessDataAsync()`. |

#### Misc

| # | IDs | Change | Details |
|--:|-----|--------|---------|
| 26 | SRC-06 | Document native confirm | Document that `DeleteItem` uses browser-native `window.confirm` (not `MariloDialog`). Source: `MariloDataGrid.Editing.cs` L91-95. |
| 27 | S-02 | Add Class parameter note | Document `Class` parameter in spec. Note: `MariloComponentBase` already provides `AdditionalAttributes` which covers `class`. Verify if explicit `Class` parameter is needed or if the base class handles it. If spec-only: just document the existing `AdditionalAttributes` pattern. |
| 28 | U-08 | Document PagerButtonCount | Document `PagerButtonCount` (int, default 5). Source: L82. Note: paired with M-12 pager shape in Lane F -- document flat parameter now. |

### Breaking Change Assessment

**No breaking changes.** Spec-only documentation updates.

### Effort Estimate

**S** (Small) -- ~28 targeted text edits across ~12 markdown files. Approximately 0.5-1 worker day.

### Inter-Lane Dependencies

- **Depends on Lane A:** Every spec file inherits the naming cascade. Lane A must land first.
- **No downstream dependencies:** Lane D is a terminal documentation lane.

---

## Lane E -- Demo Batch (12 rows)

### Resolution Summary

Create/extend demo `.razor` files to cover Wave 2 action items. Split into E.1 (demo-only, can proceed after Lane A) and E.2 (source-blocked, waits for Lane C/G).

### Sub-Group E.1 -- Demo-Only (7 rows)

| # | ID | File | Change | Details |
|--:|-----|------|--------|---------|
| 1 | A-02 | `samples/DataGrid/RefreshData.razor` (NEW) | New demo page (~80 LOC) | Demonstrate 4 refresh patterns: `Rebind()`, `ObservableCollection`, new collection assignment, `OnRead` + `SetStateAsync`. Cross-reference `refresh-data.md` spec. P0-blocker for refresh-data coverage. |
| 2 | A-03 | `samples/DataGrid/Selection.razor` (extend D1) | Add section (~30 LOC) | Add cell-selection section: `SelectionUnit="Cell"`, `SelectedCells`, `SelectedCellsChanged`, inspect `GridCellReference` values. |
| 3 | A-04 | `samples/DataGrid/Selection.razor` or `samples/DataGrid/Editing.razor` | Add section (~40 LOC) | Combined "Selection + Inline Edit" and "Selection + Popup Edit" sections showing coexistence. |
| 4 | A-05 | `samples/DataGrid/Editing.razor` (extend D2) | Add section (~20 LOC) | `ConfirmDelete` section: `<MariloDataGrid ConfirmDelete="true" ConfirmDeleteText="Delete this record?">`. |
| 5 | A-06 | `samples/DataGrid/Editing.razor` (extend D2) | Add sections (~30 LOC) | `OnAdd` callback section. `OnCommand` custom command section with `MariloGridCommandButton CommandId="Archive"`. |
| 6 | A-07 | `samples/DataGrid/ProgrammaticControl.razor` (NEW) or D2 extension | New section (~50 LOC) | Imperative API demo: buttons calling `GridRef.BeginEdit(item)`, `GridRef.SaveEdit()`, `GridRef.CancelEdit()`, `GridRef.DeleteItem(item)`, `GridRef.BeginAdd()`. |
| 7 | A-08 | `samples/DataGrid/Selection.razor` (extend D1) | Add section (~15 LOC) | `@bind-SelectedItems` shorthand demo. |

### Sub-Group E.2 -- Source-Blocked (4 rows + SA-12)

These cannot proceed until their source dependencies land.

| # | ID | Blocked By | Demo Content | Details |
|--:|-----|------------|--------------|---------|
| 8 | A-10 | SA-01/SA-04 (Lane G) | Shift-click / Ctrl-click / drag-to-select | Selection modifier demos. ~30 LOC once source lands. |
| 9 | A-11 | SA-03 (Lane G) | `GridCheckboxColumn` with SelectAll | Checkbox column demo. ~25 LOC once source lands. |
| 10 | A-12 | SA-09 (Lane G) | `EditorType` enum demos | Editor type selector demo. ~25 LOC once source lands. |
| 11 | A-13 | SA-10 (Lane G) | `NewRowPosition` demo | New row position demo. ~15 LOC once source lands. |
| 12 | SA-12 | -- | Editing guard documentation | Add note/demo showing cell-selection + InCell edit combo behavior. ~10 LOC. Can proceed without source since it is documentation of existing limitation. |

### Breaking Change Assessment

**No breaking changes.** Demo-only additions.

### Effort Estimate

**S-M** (Small to Medium) -- 7 demo-only items (~265 LOC new demo code), 5 source-blocked items (deferred). Approximately 1 worker day for E.1; E.2 deferred until dependencies land.

### Inter-Lane Dependencies

- **Depends on Lane A:** All demo code must use correct `<MariloDataGrid>` names.
- **E.2 depends on Lane C:** A-09 keyboard demos.
- **E.2 depends on Lane G:** A-10, A-11, A-12, A-13 selection/editing feature demos.

---

## Lane G -- Source-Ahead Implementation (14 rows)

### Resolution Summary

Implement spec-documented parameters and features that are absent from source. This is the largest implementation lane. Organized into 4 sub-clusters.

### Sub-Cluster G.1 -- Selection Extensions (SA-01, SA-02, SA-03, SA-04, SA-13)

| # | ID | File(s) | Change | Details |
|--:|-----|---------|--------|---------|
| 1 | SA-01 | `MariloDataGrid.razor.cs` | Add parameter (~3 LOC) | `[Parameter] public bool DragToSelect { get; set; }` -- enables rubber-band selection on cells. Requires JS interop for mouse-drag tracking. |
| 2 | SA-01 | `MariloDataGrid.Interop.cs` | Add JS interop (~20 LOC) | Register `mousedown`/`mousemove`/`mouseup` handlers for drag-to-select when `DragToSelect=true`. Calculate selected cell range from coordinates. |
| 3 | SA-02 | `MariloDataGrid.razor.cs` or a new `GridSelectionSettings.cs` | Add type/parameter (~15 LOC) | If using sub-component: create `GridSelectionSettings` with `SelectionType` property. If using flat parameter: add `[Parameter] public GridSelectionType SelectionType { get; set; }` enum (`Row`, `Cell`, `Both`). Decision: flat parameter preferred for consistency with existing `SelectionUnit`. |
| 4 | SA-03 | `src/Marilo.Components/DataGrid/MariloGridCheckboxColumn.razor` (NEW) | New component (~60 LOC) | Dedicated checkbox column component with `SelectAll` (bool) and `CheckBoxOnlySelection` (bool) parameters. Renders header checkbox for select-all, per-row checkboxes. Currently `ShowCheckboxColumn` is a flat bool on the grid -- this provides a richer API. Non-breaking: `ShowCheckboxColumn` continues to work; `MariloGridCheckboxColumn` is an additive alternative. |
| 5 | SA-04 | `MariloDataGrid.Data.cs` or `.Keyboard.cs` | Modify click handler (~30 LOC) | Add modifier key detection in `OnRowClick` handler: `if (e.ShiftKey)` = range selection from last selected to current. `if (e.CtrlKey)` = toggle selection on current row. Requires tracking `_lastSelectedIndex`. |
| 6 | SA-13 | `MariloDataGrid.Data.cs` | Modify row-drop handler (~5 LOC) | In `HandleRowDrop()`, clear `_selectedItems` after successful drop to prevent stale selection state. |

### Sub-Cluster G.2 -- Editing Extensions (SA-09, SA-10, S-16)

| # | ID | File(s) | Change | Details |
|--:|-----|---------|--------|---------|
| 7 | SA-09 | `src/Marilo.Components/DataGrid/GridEditorType.cs` (NEW) | New enum (~15 LOC) | `GridEditorType` enum: `TextBox`, `NumericTextBox`, `CheckBox`, `Switch`, `DatePicker`, `TimePicker`, `DropDownList`, `ComboBox`, `Custom`. Add `[Parameter] public GridEditorType EditorType { get; set; }` to `MariloGridColumn.razor`. |
| 8 | SA-09 | `MariloDataGrid.Rendering.cs` | Modify editor rendering (~40 LOC) | When `EditorTemplate` is null, auto-render appropriate input based on `EditorType`. Switch statement mapping enum to Marilo input components. |
| 9 | SA-10 | `MariloDataGrid.razor.cs` | Add parameter (~3 LOC) | `[Parameter] public GridNewRowPosition NewRowPosition { get; set; } = GridNewRowPosition.Top;`. New enum with `Top`, `Bottom` values. |
| 10 | SA-10 | `MariloDataGrid.Editing.cs` | Modify BeginAdd (~10 LOC) | Position the new row based on `NewRowPosition` parameter. If `Bottom`, append to `_displayedItems` end; if `Top`, prepend. |
| 11 | S-16 | `MariloDataGrid.razor.cs` | Add template parameters (~10 LOC) | `[Parameter] public RenderFragment<TItem>? PopupFormTemplate { get; set; }`, `[Parameter] public RenderFragment? PopupButtonsTemplate { get; set; }`, `[Parameter] public RenderFragment? PagerTemplate { get; set; }`. Render these in the appropriate template slots when provided. |

### Sub-Cluster G.3 -- Column Features (S-06, S-08, S-11, S-12, S-17)

These are larger features that are P2/P3. Brief resolution sketch per item.

| # | ID | Effort | Change Summary |
|--:|-----|--------|----------------|
| 12 | S-06 | L | Toolbar expansion: 13 tool components (`GridToolBarAdd`, `GridToolBarSave`, `GridToolBarExport`, `GridToolBarSearchBox`, `GridToolBarColumnChooser`, etc.). Each is a small `RenderFragment` component ~20-30 LOC. Total ~300 LOC new files. Currently only `MariloGridToolbar.razor` shell exists. |
| 13 | S-08 | L | Excel + PDF export: Requires third-party library integration (e.g., ClosedXML for Excel, QuestPDF or similar for PDF). Currently CSV-only via `ExportToCsvAsync()`. New methods: `ExportToExcelAsync()`, `ExportToPdfAsync()`. ~200 LOC + NuGet dependency. |
| 14 | S-11 | M | Multi-column headers: New `MariloGridColumnGroup` component that wraps multiple `MariloGridColumn` children. Renders as nested `<th colspan>` in header. ~80 LOC. |
| 15 | S-12 | M | Column menu / column chooser: Context menu on header cells with sort/filter/hide options. Column chooser panel listing all columns with visibility toggles. ~120 LOC + JS interop for popover positioning. |
| 16 | S-17 | S | Checkbox-list filter: Public parameters for the existing `_checkBoxFilter*` internal state. Add `FilterType` parameter to `MariloGridColumn` with `FilterType.CheckboxList` option. ~40 LOC. |

### Sub-Cluster G.4 -- Misc (S-14)

| # | ID | File(s) | Change | Details |
|--:|-----|---------|--------|---------|
| 17 | S-14 | `MariloDataGrid.razor.cs` | Add parameter (~5 LOC) | `[Parameter] public IEnumerable<TItem>? HighlightedItems { get; set; }`. In rendering: apply `mar-datagrid-row--highlighted` CSS class when row item is in the set. Add SCSS rule. |

### Breaking Change Assessment

**No breaking changes.** All additions are:
- New files and components
- New additive parameters on existing components
- New enum types
- Existing behavior is preserved

### Effort Estimate

**L** (Large overall) -- Sub-clusters vary:
- G.1 Selection: M (~130 LOC, 1 worker day)
- G.2 Editing: S-M (~80 LOC, 0.5 worker day)
- G.3 Column features: L (~740 LOC total, 3-4 worker days, can be phased)
- G.4 Misc: XS (~5 LOC)

Total: ~4-5 worker days. Recommend splitting G.3 into separate sub-waves at Stage 04.

### Inter-Lane Dependencies

- **Depends on Lane A:** Spec/demo updates need naming cascade.
- **Depends on Lane F (partially):** S-06 toolbar and S-08 export are independent of shape decisions. S-11, S-12, S-17 are independent. SA-01/SA-02 selection extensions are independent.
- **Feeds Lane E:** A-10, A-11, A-12, A-13 demos unblock when G.1/G.2 land.

---

## Lane H -- Deferred Tracks (3 rows)

### Resolution Summary

No implementation work this wave. Track for future waves.

| # | ID | Deferral Rationale | Future Action |
|--:|-----|--------------------|---------------|
| 1 | VP-016 | Material provider is a 5-line TODO stub. Separate implementation track per tick-8 Cerebrum Pattern 5. | Create Material provider implementation track when Material provider work begins. All DataGrid styles will need to be written from scratch. |
| 2 | S-13 | AI features (9 spec pages under `smart-ai-features/`). Phase D deferred. | Revisit when AI feature roadmap is prioritized. 9 spec pages already exist; source implementation is Phase D. |
| 3 | S-01 | `AdaptiveMode` -- known planned feature. P3-cosmetic. | Add `[Parameter] public GridAdaptiveMode AdaptiveMode { get; set; }` when responsive/adaptive design pass is planned. Enum: `None`, `Auto`, `StackedOnMobile`. |

### Breaking Change Assessment

N/A -- no changes.

### Effort Estimate

**N/A** this wave.

### Inter-Lane Dependencies

None.

---

## Cross-Lane Dependency Graph

```
Lane A (Naming) ──────────────────────────────────────────┐
    │                                                      │
    ├──> Lane B (SCSS Visual) ──> Lane C (Keyboard)        │
    │         │                       │                    │
    │         │ VP-015 focus rings    │ keyboard engine    │
    │         └───────────────────────┘                    │
    │                                                      │
    ├──> Lane D (Spec Batch) [terminal]                    │
    │                                                      │
    ├──> Lane E.1 (Demo-only) [after A]                    │
    │                                                      │
    ├──> Lane G (Source-ahead) ──> Lane E.2 (blocked demos)│
    │                                                      │
    └──> Lane H (Deferred) [no work]                       │
                                                           │
Lane F (Shape Decisions) ── SKIPPED this pass ─────────────┘
    VP-006 ── SKIPPED this pass
```

**Critical path:** Lane A -> Lane B (VP-015) -> Lane C -> Lane E.2

**Parallel opportunities after Lane A:**
- Lane B + Lane D can run in parallel
- Lane G can start in parallel with Lane B/C (no SCSS dependency for source work)
- Lane E.1 can start after Lane A (demo-only, no source dependency)

---

## Coverage Verification

### Rows per lane (113 total)

| Lane | Rows | Status |
|------|-----:|--------|
| A | 22 | Designed |
| B | 18 (excl VP-006) | Designed (VP-006 skipped) |
| C | 7 | Designed |
| D | 28 | Designed |
| E | 12 | Designed |
| F | 8 | SKIPPED (blocked) |
| G | 14 | Designed |
| H | 3 | Designed (deferred) |
| **Total** | **112 designed + 1 skipped (VP-006)** | |

**113 rows accounted for:** 104 designed + 8 Lane F skipped + 1 VP-006 skipped = 113.

Wait -- let me recount. Lane B has 19 rows per priority-lanes (VP-001 through VP-020 minus VP-016). VP-006 is skipped. So Lane B designed = 18 rows.

**Corrected:** 22 + 18 + 7 + 28 + 12 + 0 + 14 + 3 = 104 designed. 8 (Lane F) + 1 (VP-006) = 9 skipped. 104 + 9 = 113. **Verified.**

---

## Checkpoint

This is the end of Stage 03 (resolution-design). Every non-skipped lane has a concrete resolution with specific file paths, change nature, breaking change assessment, effort estimate, and inter-lane dependencies. **STOP here.** Stage 04 (remediation-plan) will sequence these into implementation phases.
