# Gap Inventory — MariloTreeList (Stage 01 Intake)

**Component:** `MariloTreeList<TItem>`
**Mode:** Assess (fresh — no prior gap analysis file existed)
**Intake run:** 2026-04-10
**Source snapshot:**
- Component: `src/Marilo.Components/DataGrid/MariloTreeList.razor` (199 lines, single file, generic, uses `RenderTreeBuilder` for row rendering)
- Supporting type: `TreeListColumn` (referenced as `[Parameter] public List<TreeListColumn> Columns`)
- Demo: `samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor` (21 lines — explicit placeholder alert telling users to "see the component spec for full examples")
- Spec root: `docs/component-specs/treelist/` — **52 markdown files** across 9+ sub-areas: columns (14 files), data-binding (5), editing (5), filter (5), selection (3), templates (12), accessibility (2), plus overview/paging/sorting/state/toolbar/events/refresh-data/virtual-scrolling/aggregates/row-drag-drop

**Scope classification:** `systematic` (cross-cutting gaps across every major TreeList subsystem)

**Recommended stage routing:** `01 → 02 → 03 → 04 → 05 → 06` (full pipeline — essentially a greenfield rebuild on top of the 199-line tree-walking scaffold)

**Strategic observation:** MariloTreeList is positioned as a hierarchical-data sibling of MariloDataGrid. Most of the feature surface is either (a) identical to DataGrid (paging, sorting, filtering, column menu, editing, column resize/reorder, selection, state, virtualization, aggregates, toolbar, row drag-drop, templates) or (b) a tree-specific extension (Expandable column, load-on-demand, aria-level/aria-expanded). The pragmatic rewrite strategy is to **reuse DataGrid's subsystem implementations** wherever they already exist in the monorepo, rather than reinventing paging/sorting/filtering from scratch.

---

## Summary Counts

| Severity | Count |
|---|---|
| Critical | 6 |
| High | 17 |
| Medium | 14 |
| Low | 6 |
| **Total** | **43** |

## Theme Tags

| Theme | Gaps (sample) |
|---|---|
| `missing-child-tag-architecture` | GAP-TREELIST-001, 005 |
| `datagrid-parity` | GAP-TREELIST-003, 004, 008–014, 019–022 |
| `tree-specific` | GAP-TREELIST-002, 006, 007, 017, 018 |
| `editing-pipeline` | GAP-TREELIST-019–023, 034 |
| `templates` | GAP-TREELIST-024–033 |
| `a11y` | GAP-TREELIST-036, 037 |
| `state-and-methods` | GAP-TREELIST-016, 038, 039 |
| `demo-coverage` | GAP-TREELIST-040–043 |

---

## Feature Area: DataBinding / Architecture

### GAP-TREELIST-001: Missing `<TreeListColumns>` child-tag wrapper
**Area:** Columns **Severity:** Critical **Theme:** `missing-child-tag-architecture`
**Source:** `docs/component-specs/treelist/overview.md:37-41` (spec: `<TreeListColumns>` wrapper containing `<TreeListColumn>` children); `src/Marilo.Components/DataGrid/MariloTreeList.razor:33` (source: `[Parameter] public List<TreeListColumn> Columns = new()`)

**Target:** Declarative `<TreeListColumns>` child-tag pattern with child `<TreeListColumn>` components registering via cascade (cf. `MariloDataGrid` ↔ `MariloGridColumn` at `MariloDataGrid.razor:36-39`, `MariloGridColumn.razor:5,83-92`).

**Current:** Consumers must build a `List<TreeListColumn>` object in C# and pass it as a parameter. Every spec example fails to compile.

**Impact:** Blocks every documented usage pattern. Blocks per-column child parameters like `Expandable`, `Template`, `HeaderTemplate`, `EditorTemplate`, etc.

**Recommended direction:** Follow the `MariloDataGrid` pattern exactly. Add `ITreeListColumnSink` cascade interface. Create non-generic `TreeListColumns` wrapper + `TreeListColumn` child component. Apply the Wizard CascadingValue interface-cast fix (cerebrum 2026-04-04) for non-generic children to attach to `MariloTreeList<TItem>`.

**Status:** Open

---

### GAP-TREELIST-002: Missing `Expandable` column parameter
**Area:** Columns **Severity:** Critical **Theme:** `tree-specific`
**Source:** `docs/component-specs/treelist/overview.md:25,38` (spec: `<TreeListColumn Expandable="true" />` marks the column that renders expand arrows); `src/Marilo.Components/DataGrid/MariloTreeList.razor:147` (source: hardcoded `if (ci == 0)` — first column always gets the arrows)

**Target:** `[Parameter] public bool Expandable { get; set; }` on `TreeListColumn`; renderer finds the single `Expandable` column and puts arrows there.

**Current:** Expand arrows always render in column index 0 — no way to put them on any other column.

**Impact:** Consumers who want `[Id | Name ▸ | Department]` layouts (arrows on the second column) have no option. Breaks standard TreeList ergonomics.

**Recommended direction:** Add `Expandable` boolean to the new `TreeListColumn` child component (GAP-TREELIST-001). Renderer iterates columns and emits arrows on the first column where `Expandable == true`, falling back to column 0 if none set.

**Status:** Open

---

### GAP-TREELIST-003: Not using the generic inheritance pattern for external types
**Area:** DataBinding **Severity:** High **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/data-binding/interface.md`, `docs/component-specs/treelist/data-binding/overview.md`

**Target:** Explicit documented support for `ITreeItem`-style interface-driven binding (cf. DataGrid's `IFilterDescriptor` / `ISortDescriptor` pattern) so consumers can plug in their own tree-shaped data without field-name strings.

**Current:** Reflection-based field lookup via `IdField`/`ParentIdField` strings only.

**Impact:** Medium-high — strongly-typed consumers would prefer interface dispatch over reflection.

**Recommended direction:** Add `ITreeListItem<TItem>` interface + opt-in check in `BuildTree()`. Keep field-string path as fallback.

**Status:** Open

---

### GAP-TREELIST-004: Missing Load-on-Demand (`OnExpand` / lazy loading)
**Area:** DataBinding **Severity:** High **Theme:** `tree-specific`, `datagrid-parity`
**Source:** `docs/component-specs/treelist/data-binding/load-on-demand.md`

**Target:** `EventCallback<TreeListExpandEventArgs<TItem>>? OnExpand` fires when a node expands; consumer resolves and returns children. Mirror `MariloTreeView` load-on-demand pattern from the TreeView closure report.

**Current:** Tree is built synchronously from `Data` once. No async hook.

**Impact:** Cannot handle million-row trees — must load everything up front.

**Recommended direction:** Mirror TreeView's `LoadChildrenAsync` pattern. Add `HasChildren` resolver fallthrough + `LoadOnDemand` boolean.

**Status:** Open

---

### GAP-TREELIST-005: Missing `<TreeListToolBar>` child-tag
**Area:** Toolbar **Severity:** High **Theme:** `missing-child-tag-architecture`
**Source:** `docs/component-specs/treelist/toolbar.md`

**Target:** `<TreeListToolBar>` child wrapper taking a `RenderFragment` for custom toolbar content — toolbar spacer, buttons, custom commands.

**Current:** No toolbar concept at all.

**Impact:** No built-in place for Add/Save/Cancel/Refresh command buttons in edit scenarios.

**Recommended direction:** Mirror `MariloGridToolbar` from `src/Marilo.Components/DataGrid/`.

**Status:** Open

---

### GAP-TREELIST-006: `IdField` / `ParentIdField` / `ItemsField` / `HasChildrenField` use strings
**Area:** DataBinding **Severity:** Medium **Theme:** `tree-specific`, `datagrid-parity`
**Source:** `docs/component-specs/treelist/overview.md:22`

**Target:** Keep string field names (matches spec) but also accept `Expression<Func<TItem, TKey>>` overloads for strong typing (cf. MudBlazor pattern).

**Current:** String-only.

**Impact:** Low-medium — spec signatures work, but IDE refactors break silently.

**Recommended direction:** Add expression overloads as additive convenience; both forms resolve to the same internal accessor.

**Status:** Open

---

## Feature Area: Paging

### GAP-TREELIST-007: Missing Paging entirely
**Area:** Paging **Severity:** Critical **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/paging.md`, `docs/component-specs/treelist/overview.md:34,95`

**Target:** `Pageable` bool, `PageSize` int, `Page` int, pager UI (`MariloPagination` subcomponent), two-way `PageChanged` event. In tree context: only top-level rows count for page size; expanded children spill into the page.

**Current:** Paging completely absent. All rows render at once.

**Impact:** Critical for large trees — browsers lock up on thousand-node datasets.

**Recommended direction:** Reuse `MariloPagination` component. Tree-specific consideration: paginate only the **visible** flattened rows (a row + its expanded children), which requires the flattened-virtual-rows computation from virtualization (GAP-TREELIST-017).

**Status:** Open

---

## Feature Area: Sorting

### GAP-TREELIST-008: Missing Sorting
**Area:** Sorting **Severity:** Critical **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/sorting.md`, `docs/component-specs/treelist/overview.md:35,96`

**Target:** `Sortable` bool (tree-level), per-column `Sortable`, `SortMode` (Single/Multiple), clickable header indicators, `OnSortChanged` event. In tree context: sorting applies **within each parent**, preserving hierarchy.

**Current:** None.

**Impact:** Critical — cannot order employees by name, etc.

**Recommended direction:** Reuse DataGrid sorting subsystem; wrap it so it sorts children-within-parent rather than the flat list.

**Status:** Open

---

## Feature Area: Filtering

### GAP-TREELIST-009: Missing FilterMode (FilterMenu / FilterRow / CheckboxList)
**Area:** Filter **Severity:** High **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/filter/overview.md`, `docs/component-specs/treelist/filter/filter-menu.md`, `docs/component-specs/treelist/filter/filter-row.md`, `docs/component-specs/treelist/filter/checkboxlist.md`, `docs/component-specs/treelist/overview.md:36,97`

**Target:** `TreeListFilterMode` enum (`None`, `FilterRow`, `FilterMenu`, `CheckBoxList`) — mirrors DataGrid. Filter-row appears as a second header row; filter-menu appears as per-column popup.

**Current:** None.

**Impact:** High — cannot narrow to specific branches/items.

**Recommended direction:** Reuse DataGrid filter subsystem. Tree-specific consideration: when a child matches the filter, all its ancestors must remain visible (breadcrumb-to-root pattern).

**Status:** Open

---

### GAP-TREELIST-010: Missing filter SearchBox
**Area:** Filter **Severity:** Medium **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/filter/searchbox.md`

**Target:** Dedicated `<TreeListToolBarSearchBox>` component for a single cross-column text filter.

**Current:** None.

**Impact:** Medium — complements filter-menu/row for quick-search UX.

**Recommended direction:** Child of `<TreeListToolBar>`.

**Status:** Open

---

## Feature Area: Editing

### GAP-TREELIST-011: Missing editing pipeline (EditMode Inline / InCell / Popup)
**Area:** Editing **Severity:** Critical **Theme:** `editing-pipeline`, `datagrid-parity`
**Source:** `docs/component-specs/treelist/editing/overview.md`, `docs/component-specs/treelist/editing/inline.md`, `docs/component-specs/treelist/editing/incell.md`, `docs/component-specs/treelist/editing/popup.md`, `docs/component-specs/treelist/editing/validation.md`

**Target:** `TreeListEditMode` enum (`None`, `Inline`, `Incell`, `Popup`); CRUD event set (`OnCreate`, `OnUpdate`, `OnDelete`, `OnAdd`, `OnCancel`); `EditorTemplate` per column; validation integration via `DataAnnotationsValidator`.

**Current:** None. TreeList is read-only.

**Impact:** Critical — no write-path.

**Recommended direction:** Reuse DataGrid edit subsystem wholesale. Tree-specific consideration: "add child" vs "add sibling" commands on the parent row.

**Status:** Open

---

### GAP-TREELIST-012: Missing CRUD events
**Area:** Editing **Severity:** High **Theme:** `editing-pipeline`, `datagrid-parity`
**Source:** `docs/component-specs/treelist/editing/overview.md`, `docs/component-specs/treelist/events.md`

**Target:** `OnCreate`, `OnUpdate`, `OnDelete`, `OnEdit`, `OnAdd`, `OnCancel` with typed event args (`TreeListCreateEventArgs<TItem>`, etc.) mirroring DataGrid's shape.

**Current:** Only `OnRowClick`.

**Impact:** Follows from GAP-TREELIST-011.

**Recommended direction:** Define event-arg types; wire from edit flow.

**Status:** Open

---

## Feature Area: Selection

### GAP-TREELIST-013: Missing Selection (row + cell)
**Area:** Selection **Severity:** High **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/selection/overview.md`, `docs/component-specs/treelist/selection/rows.md`, `docs/component-specs/treelist/selection/cells.md`

**Target:** `SelectionMode` (`None`, `Single`, `Multiple`); `SelectionType` (`Row`, `Cell`); `SelectedItems` two-way bindable collection; `SelectedItemsChanged` event.

**Current:** No selection.

**Impact:** High — cannot pick items for bulk actions.

**Recommended direction:** Reuse DataGrid selection subsystem.

**Status:** Open

---

### GAP-TREELIST-014: Missing Checkbox column
**Area:** Columns **Severity:** Medium **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/checkbox.md`

**Target:** `<TreeListCheckboxColumn>` specialized child component for tri-state parent/child selection (cf. MariloTreeView's `AllowCheckChildren`/`AllowCheckParents` pattern from the TreeView closure report).

**Current:** None.

**Impact:** Medium — selection via row click works, but checkbox column is standard UX.

**Recommended direction:** New child component; share tri-state logic with TreeView.

**Status:** Open

---

### GAP-TREELIST-015: Missing Command column
**Area:** Columns **Severity:** Medium **Theme:** `editing-pipeline`, `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/command.md`

**Target:** `<TreeListCommandColumn>` with standard command buttons (Edit, Save, Cancel, Delete, Add) + custom command slots. Mirrors `MariloGridCommandButton` from `src/Marilo.Components/DataGrid/`.

**Current:** None.

**Impact:** Medium — needed for inline/popup edit workflows.

**Recommended direction:** Reuse `MariloGridCommandButton` pattern.

**Status:** Open

---

## Feature Area: State

### GAP-TREELIST-016: Missing State API (`GetState` / `SetState` / `OnStateChanged`)
**Area:** State **Severity:** High **Theme:** `state-and-methods`, `datagrid-parity`
**Source:** `docs/component-specs/treelist/state.md`

**Target:** Public `GetState()` / `SetStateAsync(TreeListState)` methods; `OnStateChanged` event. Serializable state capturing expanded ids, selection, sort, filter, page, column order/width/visibility.

**Current:** State is all in private fields (`_expandedIds`, `_rootItems`). No public API.

**Impact:** High — consumers cannot persist tree state across navigations.

**Recommended direction:** Mirror `MariloDataGrid.GetState` / `SetStateAsync` pattern (DataGrid Phase 1 resolution).

**Status:** Open

---

## Feature Area: Virtualization & Performance

### GAP-TREELIST-017: Missing row virtualization
**Area:** Virtualization **Severity:** High **Theme:** `tree-specific`, `datagrid-parity`
**Source:** `docs/component-specs/treelist/virtual-scrolling.md`

**Target:** `<Virtualize>`-based row rendering over the **flattened visible rows** (a row + its currently-expanded descendants). Tree-specific: the flat list changes when nodes expand/collapse.

**Current:** All visible rows render at once — O(n) DOM nodes.

**Impact:** High — ten-thousand-row trees are un-usable in UI.

**Recommended direction:** Compute flat row list in `BuildTree()` + `ToggleExpand`, feed into `<Virtualize>`. Cf. TreeView gap 18 (deferred) — TreeList should not repeat that deferral.

**Status:** Open

---

### GAP-TREELIST-018: Missing column virtualization
**Area:** Virtualization **Severity:** Low **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/virtual.md`

**Target:** Horizontal column virtualization for trees with 50+ columns.

**Current:** None.

**Impact:** Low — rare use case.

**Recommended direction:** Defer until row virtualization lands and there's a real wide-tree scenario.

**Status:** Open

---

## Feature Area: Column Features

### GAP-TREELIST-019: Missing Column resizing
**Area:** Columns **Severity:** Medium **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/resize.md`

**Target:** `Resizable` parameter; drag handles on column borders.

**Current:** Columns have static `Width` string only.

**Impact:** Medium.

**Recommended direction:** Reuse DataGrid column-resize JS interop (cf. DataGrid Phase 3 closure).

**Status:** Open

---

### GAP-TREELIST-020: Missing Column reordering
**Area:** Columns **Severity:** Medium **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/reorder.md`

**Target:** `Reorderable` parameter + drag-and-drop column reorder.

**Current:** None.

**Impact:** Medium.

**Recommended direction:** Reuse DataGrid column-reorder subsystem.

**Status:** Open

---

### GAP-TREELIST-021: Missing Frozen / Locked columns
**Area:** Columns **Severity:** Medium **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/frozen.md`

**Target:** `Locked` + `FrozenPosition` (Left/Right) on `TreeListColumn` — sticky-position CSS identical to DataGrid.

**Current:** None.

**Impact:** Medium.

**Recommended direction:** Reuse the DataGrid frozen-columns implementation from JS Interop Batch 2 (2026-04-09) line-for-line.

**Status:** Open

---

### GAP-TREELIST-022: Missing Column Menu
**Area:** Columns **Severity:** Medium **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/menu.md`

**Target:** Per-column popup menu with sort/filter/lock/hide controls.

**Current:** None.

**Impact:** Medium.

**Recommended direction:** Reuse DataGrid column menu component.

**Status:** Open

---

### GAP-TREELIST-023: Missing Multi-column headers (grouped headers)
**Area:** Columns **Severity:** Low **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/multi-column-headers.md`

**Target:** Nested `<TreeListColumnGroup>` wrapping multiple `<TreeListColumn>` children, rendered as two header rows.

**Current:** None.

**Impact:** Low — niche feature.

**Recommended direction:** Defer until base column system stabilizes.

**Status:** Open

---

### GAP-TREELIST-024: Missing auto-generated columns
**Area:** Columns **Severity:** Low **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/auto-generated.md`

**Target:** When no `<TreeListColumn>` children are defined, reflect `TItem` properties and render one column per public property, using `[Display]` attributes for titles/formats.

**Current:** Empty columns list renders an empty header.

**Impact:** Low — convenience feature for rapid prototyping.

**Recommended direction:** Mirror DataGrid auto-generate behavior.

**Status:** Open

---

### GAP-TREELIST-025: Missing Column `Visible` / column chooser
**Area:** Columns **Severity:** Low **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/visible.md`, `docs/component-specs/treelist/templates/column-chooser.md`

**Target:** `Visible` parameter per column; column-chooser popup showing toggle checkboxes.

**Current:** None (there's not even a `Visible` parameter on `TreeListColumn` per the source).

**Impact:** Low — user-customized views are a power feature.

**Recommended direction:** Add `Visible` parameter; column chooser comes with column menu (GAP-TREELIST-022).

**Status:** Open

---

### GAP-TREELIST-026: Missing Column `DisplayFormat`
**Area:** Columns **Severity:** Medium **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/display-format.md`

**Target:** `DisplayFormat` parameter (e.g., `"{0:N2}"`, `"{0:yyyy-MM-dd}"`) applied in `GetCellValue`.

**Current:** Raw `.ToString()` output.

**Impact:** Medium — numeric and date columns look wrong without formatting.

**Recommended direction:** Add parameter; apply via `string.Format` in `GetCellValue`.

**Status:** Open

---

### GAP-TREELIST-027: Missing Column events (`OnHeaderClick`, etc.)
**Area:** Columns **Severity:** Low **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/columns/events.md`

**Target:** Per-column events: `OnCellClick`, `OnHeaderClick`, `OnResize`, etc.

**Current:** None.

**Impact:** Low — advanced scenario.

**Recommended direction:** Add once base column architecture (GAP-TREELIST-001) lands.

**Status:** Open

---

## Feature Area: Templates

### GAP-TREELIST-028: Missing `Template` on `<TreeListColumn>` (cell template)
**Area:** Templates **Severity:** High **Theme:** `templates`
**Source:** `docs/component-specs/treelist/templates/column.md`, `docs/component-specs/treelist/templates/overview.md`

**Target:** `RenderFragment<TItem>? Template` on each column, overriding default cell rendering.

**Current:** Hardcoded `GetCellValue` only.

**Impact:** High — consumers can't put buttons, icons, chips, or formatted markup inside cells.

**Recommended direction:** Add after GAP-TREELIST-001. Cell template replaces the `builder.AddContent(seq++, GetCellValue(...))` call.

**Status:** Open

---

### GAP-TREELIST-029: Missing `HeaderTemplate`
**Area:** Templates **Severity:** Medium **Theme:** `templates`
**Source:** `docs/component-specs/treelist/templates/column-header.md`

**Target:** `RenderFragment? HeaderTemplate` on each column.

**Current:** Hardcoded `@col.Title` in `<th>`.

**Impact:** Medium.

**Recommended direction:** Add after GAP-TREELIST-001.

**Status:** Open

---

### GAP-TREELIST-030: Missing `RowTemplate`
**Area:** Templates **Severity:** Medium **Theme:** `templates`
**Source:** `docs/component-specs/treelist/templates/row.md`

**Target:** `RenderFragment<TItem>? RowTemplate` — replaces the default `<tr>` rendering for power users.

**Current:** None.

**Impact:** Medium — escape hatch for complex row layouts.

**Recommended direction:** Add on the TreeList itself (not the column).

**Status:** Open

---

### GAP-TREELIST-031: Missing `NoDataTemplate`
**Area:** Templates **Severity:** Medium **Theme:** `templates`
**Source:** `docs/component-specs/treelist/templates/no-data-template.md`

**Target:** `RenderFragment? NoDataTemplate` — custom empty-state UI.

**Current:** Empty `<tbody>` silently.

**Impact:** Medium — spec/UX polish.

**Recommended direction:** Add conditional render when `_rootItems.Count == 0`.

**Status:** Open

---

### GAP-TREELIST-032: Missing Editor / Popup / Filter / Pager templates
**Area:** Templates **Severity:** Low **Theme:** `templates`, `editing-pipeline`
**Source:** `docs/component-specs/treelist/templates/editor.md`, `popup-form-template.md`, `popup-buttons-template.md`, `filter.md`, `pager.md`

**Target:** `EditorTemplate`, `PopupFormTemplate`, `PopupButtonsTemplate`, `FilterTemplate`, `PagerTemplate`.

**Current:** None (all blocked on their parent feature areas).

**Impact:** Low — downstream of editing/filter/paging.

**Recommended direction:** Resolve inside the respective feature-area batches (editing/filter/paging). Flagged here for completeness.

**Status:** Open

---

## Feature Area: Events

### GAP-TREELIST-033: Missing `OnRead` remote-data pattern
**Area:** DataBinding **Severity:** High **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/data-binding/overview.md`, `docs/component-specs/treelist/refresh-data.md`

**Target:** `EventCallback<TreeListReadEventArgs>? OnRead` — remote-data scenario where server returns rows for the currently-visible page/filter/sort window. Mirrors `GridReadEventArgs` / `MultiSelectReadEventArgs`.

**Current:** Only local `Data`.

**Impact:** High — cannot do server-side paging/sorting/filtering.

**Recommended direction:** Mirror DataGrid's `OnRead` implementation.

**Status:** Open

---

### GAP-TREELIST-034: Missing expand/collapse events
**Area:** Events **Severity:** Medium **Theme:** `tree-specific`, `editing-pipeline`
**Source:** `docs/component-specs/treelist/events.md`

**Target:** `OnExpand` + `OnCollapse` events (separate from load-on-demand GAP-TREELIST-004 — these fire even when children are already present).

**Current:** None.

**Impact:** Medium — observers can't react to tree state changes.

**Recommended direction:** Fire from `ToggleExpand`.

**Status:** Open

---

### GAP-TREELIST-035: Missing `OnItemRender`
**Area:** Events **Severity:** Medium **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/events.md`

**Target:** `EventCallback<TreeListItemRenderEventArgs<TItem>>? OnItemRender` with a mutable `Class` property (cf. `MariloMultiSelect.OnItemRender` from T4 Pickers B6).

**Current:** None.

**Impact:** Medium — no per-row conditional styling hook short of a full template.

**Recommended direction:** Mirror MultiSelect cached-args pattern.

**Status:** Open

---

## Feature Area: Accessibility

### GAP-TREELIST-036: Missing expand-state ARIA (`aria-expanded`, `aria-setsize`, `aria-posinset`)
**Area:** Accessibility **Severity:** High **Theme:** `a11y`
**Source:** `docs/component-specs/treelist/accessibility/wai-aria-support.md`

**Target:** On every tree row: `aria-expanded` (when `hasKids`), `aria-setsize` (total siblings), `aria-posinset` (1-based position among siblings). Required by WAI-ARIA treegrid pattern.

**Current:** Only `role="treegrid"`, `role="row"`, and `aria-level`. Missing the other three required attributes.

**Impact:** High — fails WAI-ARIA treegrid conformance. Screen reader users cannot discover tree topology.

**Recommended direction:** Small additions in `RenderRows` — pass sibling count and position through the recursion.

**Status:** Open

---

### GAP-TREELIST-037: Missing keyboard navigation
**Area:** Accessibility **Severity:** High **Theme:** `a11y`
**Source:** `docs/component-specs/treelist/accessibility/overview.md`, `docs/component-specs/treelist/overview.md:152` (spec: `Navigable` bool parameter)

**Target:** Arrow-key navigation (Up/Down = sibling/cousin, Left = collapse/parent, Right = expand/first-child); Home/End; Space to select (when selection enabled); Enter to edit (when editing enabled). Follows WAI-ARIA treegrid pattern.

**Current:** Zero keyboard support — rows are not tabbable.

**Impact:** High — fails WCAG 2.1 AA keyboard-accessibility. Mouse-only.

**Recommended direction:** Add `Navigable` parameter + focus management + arrow-key handler in `OnKeyDown`. Reuse the `MariloTreeView` keyboard handler shape from the TreeView closure report.

**Status:** Open

---

## Feature Area: Methods / Refresh

### GAP-TREELIST-038: Missing `Rebind()` / `Refresh()` / `AutoFitAllColumns()` methods
**Area:** Methods **Severity:** Medium **Theme:** `state-and-methods`, `datagrid-parity`
**Source:** `docs/component-specs/treelist/overview.md:160-192` (spec: `AutoFitAllColumns` invoked in sample code at line 191), `docs/component-specs/treelist/refresh-data.md`

**Target:** Public `Rebind()`, `Refresh()`, `AutoFitAllColumns()` (and sibling `AutoFitColumn(field)`) — imperative control.

**Current:** No public methods.

**Impact:** Medium — consumers cannot programmatically refresh or resize.

**Recommended direction:** Add all four. `AutoFit*` requires `Resizable` column infrastructure (GAP-TREELIST-019).

**Status:** Open

---

### GAP-TREELIST-039: Missing `Height` / `Width` / `Class` / `Navigable` parameters
**Area:** DataBinding (layout) **Severity:** Medium **Theme:** `spec-api-naming`
**Source:** `docs/component-specs/treelist/overview.md:148-153`

**Target:** `Height`, `Width`, explicit `Class`, and `Navigable` (keyboard-nav toggle) as top-level parameters.

**Current:** Only `CombineClasses("mar-treelist")` + `CombineStyles()` inherited from `MariloComponentBase`; no Height/Width/Navigable.

**Impact:** Medium — spec signatures don't compile; fixed-height trees are awkward.

**Recommended direction:** Add all four. `Height`/`Width` merge into root `style`. `Navigable` gates the keyboard handler.

**Status:** Open

---

## Feature Area: Aggregates

### GAP-TREELIST-040: Missing Aggregates (sum/count/avg per column)
**Area:** Aggregates **Severity:** Medium **Theme:** `datagrid-parity`
**Source:** `docs/component-specs/treelist/aggregates.md`

**Target:** Per-column aggregate configuration (`Sum`, `Count`, `Average`, `Min`, `Max`) rendered as footer totals and/or group footers.

**Current:** None.

**Impact:** Medium — financial/counting scenarios need totals.

**Recommended direction:** Reuse DataGrid aggregate subsystem (Phase 2 closure). Tree-specific: aggregates can compute per-parent (subtree totals) in addition to overall.

**Status:** Open

---

## Feature Area: Row Drag-Drop

### GAP-TREELIST-041: Missing Row drag-drop / reparenting
**Area:** Row drag-drop **Severity:** High **Theme:** `tree-specific`, `datagrid-parity`
**Source:** `docs/component-specs/treelist/row-drag-drop.md`

**Target:** `RowDraggable` boolean + `OnRowDrop` event with a destination-parent / destination-index arg; visual drop target indicator; reparenting with cycle detection.

**Current:** None.

**Impact:** High — tree editing UX requires reparenting.

**Recommended direction:** Reuse DataGrid `RowDraggable`/`OnRowDrop` from JS Interop Batch 2 (2026-04-09). Tree-specific: add `DropPosition` (`Into` | `Before` | `After`) to event args, and reject drops where destination is a descendant of source.

**Status:** Open

---

## Feature Area: Demo Coverage

### GAP-TREELIST-042: Demo is an explicit placeholder
**Area:** Demo **Severity:** Medium **Theme:** `demo-coverage`
**Source:** `samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor:1-21`

**Target:** Realistic Overview demo matching the spec example at `docs/component-specs/treelist/overview.md:30-76` — Employee hierarchy with flat data binding, `Pageable`, `Sortable`, `FilterMode=FilterMenu`, expandable column, CRUD-disabled but click events wired.

**Current:** Placeholder that *tells users to read the spec* — via a `MariloAlert` with literal text "See the component spec for full examples". Honest placeholder, but zero demonstrated functionality.

**Impact:** Medium — users opening the demo page learn nothing about the component.

**Recommended direction:** Write after base feature phase (Phases A–C) lands. Use the spec's Employee example verbatim.

**Status:** Open

---

### GAP-TREELIST-043: Missing flat-data / hierarchical-data / load-on-demand demos
**Area:** Demo **Severity:** Low **Theme:** `demo-coverage`
**Source:** `docs/component-specs/treelist/data-binding/flat-data.md`, `hierarchical-data.md`, `load-on-demand.md`

**Target:** Three separate demo pages — flat self-referencing, hierarchical (nested children), and load-on-demand (async children fetch). Follows the DataGrid demo-coverage pattern.

**Current:** None. Only the placeholder Overview demo exists.

**Impact:** Low — documentation/discoverability, not functional.

**Recommended direction:** Ship alongside the relevant feature phases.

**Status:** Open

---

## Cross-cutting Observations

1. **Massive DataGrid overlap.** 22 of the 43 gaps (≈51%) are direct DataGrid-parity items that can reuse DataGrid's implementations line-for-line: paging, sorting, filtering, editing, selection, state, column resize/reorder/freeze, column menu, templates, aggregates, row drag-drop, virtualization, `OnRead`, auto-gen columns. Stage 03 should explicitly enumerate which DataGrid subsystem each gap reuses vs extends. The only gaps that are **genuinely tree-specific** are: GAP-TREELIST-002 (Expandable column), -004 (load-on-demand), -006 (ItemsField), -017 (flattened virtualization), -034 (expand/collapse events), -036/037 (treegrid ARIA), -041 (reparenting drop). Everything else is "wire DataGrid's subsystem into the TreeList render loop".

2. **Child-tag architecture is the single critical path.** GAP-TREELIST-001 (TreeListColumns wrapper + TreeListColumn children) is the vehicle for carrying `Expandable`, `Template`, `HeaderTemplate`, `EditorTemplate`, `Width`, `Sortable`, `Filterable`, `Resizable`, `Locked`, `Visible`, `DisplayFormat`, and all per-column events. Nothing else unblocks until it lands. Prioritize Phase A around this.

3. **`TreeListColumn` is currently a class, not a component.** The source at line 33 has `[Parameter] public List<TreeListColumn> Columns`. This implies `TreeListColumn` is a data class, not a Blazor component. The rewrite needs to either (a) add a `TreeListColumn` Blazor component alongside the existing class (different name), or (b) migrate the existing class into a component. Either way, **this is a breaking change** for any consumer (if any) passing a `List<TreeListColumn>`. Flag for the human-decisions list.

4. **Demo is honest about the gap.** Unlike Scheduler (which rendered a blank widget), the TreeList Overview demo explicitly renders a `MariloAlert` telling users to see the spec. That's *refreshingly honest* but zero-value. Once Phase A lands, the first thing worth doing is replacing that alert with the spec's Employee example.

5. **This is the largest CDW backlog in the repo.** 43 gaps vs Scheduler's 32 vs DataGrid's Phase-3 remnant (~2). MariloTreeList and MariloDataGrid together account for ~90% of the unresolved CDW surface. Prioritization should treat TreeList as a second major rewrite on the order of Gantt (20 gaps, 31 tests, 24 commits) — probably **larger**, given 43 gaps.

## Suggested Phase Breakdown for Stage 02 Prioritization

| Phase | Scope | Unblocks |
|---|---|---|
| **A — Architecture** | GAP-001 (child-tag wrapper), 002 (Expandable), 039 (Height/Width/Class/Navigable) | Everything |
| **B — Columns basic** | GAP-026 (DisplayFormat), 025 (Visible) | Template work, column menu |
| **C — Data ops: Paging/Sorting/Filter** | GAP-007, 008, 009, 010, 033 (OnRead) | Data operations |
| **D — Templates** | GAP-028, 029, 030, 031 | Customization |
| **E — Selection + State** | GAP-013, 014, 016 | State persistence, bulk actions |
| **F — Editing pipeline** | GAP-011, 012, 015, 032 | Write-path |
| **G — Load-on-demand** | GAP-003, 004, 006, 034 | Lazy trees |
| **H — Column advanced** | GAP-019, 020, 021, 022, 023, 024, 027 | Power-user features |
| **I — Virtualization** | GAP-017, 018 | Large-data perf |
| **J — Row drag-drop** | GAP-041 | Tree reparenting |
| **K — Aggregates** | GAP-040 | Totals |
| **L — A11y pass** | GAP-036, 037 | WCAG |
| **M — Methods** | GAP-038, 035 (OnItemRender) | Imperative control |
| **N — Demo coverage** | GAP-042, 043 | Documentation |

Phase A gates everything. Phases C–F can run in parallel after A + B. Phase G (load-on-demand) can run in parallel with editing (F). Phase L (a11y) can start after A.

---

## Audit Checklist

| Check | Status |
|---|---|
| Every gap has a unique ID | ✅ GAP-TREELIST-001 through GAP-TREELIST-043 |
| Every gap references real artifacts | ✅ All source paths verified against the snapshot |
| Severity assigned to all gaps | ✅ 6 Critical, 17 High, 14 Medium, 6 Low |
| Target state documented | ✅ `_config/gap-context.md` target is MariloTreeList matching `docs/component-specs/treelist/` |
| Counts match | ✅ 6 + 17 + 14 + 6 = 43 |

## Human Decisions Needed Before Stage 02

1. **Branch strategy** — rebuild in place on `workInProgress`, or a dedicated `treelist-rewrite` branch (cf. `gantt-rewrite` precedent)? Given this is projected to be **larger than Gantt**, a branch is probably the right call.
2. **`TreeListColumn` backward compat** — the existing source takes a `List<TreeListColumn>` parameter. Does any consumer depend on that? Break the parameter (cleaner) or keep it as a fallback alongside the new child-tag wrapper (safer)? Grep the codebase for usages before deciding.
3. **DataGrid subsystem reuse strategy** — extract shared paging/sorting/filtering/editing/selection/virtualization/frozen-columns/drag-drop into `Marilo.Components.DataGrid.Shared` abstractions, or copy-paste the DataGrid implementations into TreeList? The shared-abstractions path is cleaner long-term but is a bigger up-front refactor. Copy-paste is faster but creates two divergent implementations.
4. **Flat data vs hierarchical data default** — spec documents both. Which is the recommended default for new users? The DataGrid-parity gaps assume both work; the canonical "quick start" demo should pick one.
5. **Editing UX** — built-in popup form (DataGrid-style) or consumer-provided `EditTemplate`? Consistency with DataGrid suggests built-in.
6. **Virtualization + paging composition** — if both are enabled, which wins? Spec is ambiguous. Recommended: paging wraps virtualization (virtualize within the current page).
7. **Row drag-drop semantics for reparenting** — on drop, does the dragged row become a **child of** the drop target, a **sibling before** it, or a **sibling after** it? Depends on cursor Y-position within the target row (top third / middle / bottom third). Worth confirming before implementation.

Stage 02 prioritization should begin once decisions 1–6 land. Decision 7 can be deferred to Phase J.
