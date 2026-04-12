# Stage 01 -- Gap Inventory: MariloTreeList

**Date:** 2026-04-12
**Worker:** w-treelist-gap-analysis
**Mode:** Import (from delivery audit + existing gap-treelist-inventory.md)
**Source snapshot:**
- Component: `src/Marilo.Components/DataGrid/MariloTreeList.razor` (199 lines)
- Model: `src/Marilo.Core/Models/TreeListColumn.cs` (3 properties: Title, Field, Width)
- Demo: `samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor` (placeholder only)
- Spec root: `docs/component-specs/treelist/` (53 markdown files across 9+ sub-areas)

---

## Summary

| Severity | Count |
|----------|-------|
| Critical | 6 |
| High | 17 |
| Medium | 14 |
| Low | 6 |
| **Total** | **43** |

| Type | Count |
|------|-------|
| spec-ahead | 37 |
| mismatch | 3 |
| unstyled | 2 |
| source-ahead | 0 |
| demo-gap | 1 |

---

## Gap Inventory

### GAP-TREELIST-001: Missing `<TreeListColumns>` / `<TreeListColumn>` child-tag architecture
- **Severity:** Critical
- **Type:** spec-ahead
- **Description:** Spec documents declarative `<TreeListColumns>` wrapper with `<TreeListColumn>` children. Source uses `[Parameter] public List<TreeListColumn> Columns` (a POCO list, not Blazor components). Every spec example fails to compile against current source.
- **Affected files:** `src/Marilo.Components/DataGrid/MariloTreeList.razor` (line 33), `src/Marilo.Core/Models/TreeListColumn.cs`
- **Blocking:** Yes -- blocks all per-column features (templates, Expandable, Sortable, Filterable, etc.)

### GAP-TREELIST-002: Missing `Expandable` column parameter
- **Severity:** Critical
- **Type:** spec-ahead
- **Description:** Spec: `<TreeListColumn Expandable="true" />` marks which column renders expand arrows. Source: hardcoded to column index 0 (`if (ci == 0)` at line 147).
- **Affected files:** `MariloTreeList.razor` (line 147)
- **Blocking:** Yes -- consumers cannot choose which column shows the tree hierarchy

### GAP-TREELIST-003: Not using generic interface pattern for external types
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents interface-driven binding. Source uses reflection-based string field lookup only (IdField/ParentIdField).
- **Affected files:** `MariloTreeList.razor` (lines 46-117)
- **Blocking:** No

### GAP-TREELIST-004: Missing Load-on-Demand
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents `OnExpand` lazy loading for async child resolution. Source builds the entire tree synchronously from `Data`.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No -- but required for large trees

### GAP-TREELIST-005: Missing `<TreeListToolBar>` child-tag
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents toolbar wrapper with Add/Search/Custom tool children. No toolbar concept exists.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No -- but needed for editing workflows

### GAP-TREELIST-006: IdField/ParentIdField/ItemsField/HasChildrenField string-only
- **Severity:** Medium
- **Type:** mismatch
- **Description:** Source accepts only string field names. Spec implies Expression<Func<TItem,TKey>> overloads for strong typing.
- **Affected files:** `MariloTreeList.razor` (lines 29-32)
- **Blocking:** No

### GAP-TREELIST-007: Missing Paging entirely
- **Severity:** Critical
- **Type:** spec-ahead
- **Description:** Spec documents Pageable, PageSize, Page parameters with pager UI. Source renders all rows at once.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** Yes -- browsers lock up on large datasets

### GAP-TREELIST-008: Missing Sorting
- **Severity:** Critical
- **Type:** spec-ahead
- **Description:** Spec documents Sortable bool, per-column Sortable, SortMode (Single/Multiple), clickable header indicators. Source has none.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** Yes -- cannot order data

### GAP-TREELIST-009: Missing FilterMode (FilterMenu / FilterRow / CheckboxList)
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents TreeListFilterMode enum with multiple filter UI modes. Source has none.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-010: Missing filter SearchBox
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents `<TreeListToolBarSearchBox>` for cross-column text filter. No implementation.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No -- depends on toolbar (GAP-005)

### GAP-TREELIST-011: Missing editing pipeline (Inline / InCell / Popup)
- **Severity:** Critical
- **Type:** spec-ahead
- **Description:** Spec documents TreeListEditMode enum, CRUD event set, EditorTemplate per column, validation integration. Component is read-only.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** Yes -- no write-path

### GAP-TREELIST-012: Missing CRUD events
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents OnCreate, OnUpdate, OnDelete, OnEdit, OnAdd, OnCancel with typed event args. Source has only OnRowClick.
- **Affected files:** `MariloTreeList.razor` (line 34)
- **Blocking:** No -- follows from GAP-011

### GAP-TREELIST-013: Missing Selection (row + cell)
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents SelectionMode, SelectionType, SelectedItems two-way binding, SelectedItemsChanged. No selection exists.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-014: Missing Checkbox column
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents `<TreeListCheckboxColumn>` for tri-state parent/child selection. No implementation.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No -- depends on selection (GAP-013)

### GAP-TREELIST-015: Missing Command column
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents `<TreeListCommandColumn>` with standard CRUD buttons. No implementation.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No -- depends on editing (GAP-011)

### GAP-TREELIST-016: Missing State API (GetState / SetState / OnStateChanged)
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents public GetState()/SetStateAsync() methods and OnStateChanged event. State is all in private fields.
- **Affected files:** `MariloTreeList.razor` (lines 37-38)
- **Blocking:** No -- but needed for enterprise scenarios

### GAP-TREELIST-017: Missing row virtualization
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents Virtualize-based row rendering. Source renders all visible rows -- O(n) DOM nodes.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No -- but needed for large-data performance

### GAP-TREELIST-018: Missing column virtualization
- **Severity:** Low
- **Type:** spec-ahead
- **Description:** Spec documents horizontal column virtualization for wide trees. Rare use case.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-019: Missing Column resizing
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents Resizable parameter with drag handles. Source has static Width string only.
- **Affected files:** `MariloTreeList.razor`, `TreeListColumn.cs`
- **Blocking:** No

### GAP-TREELIST-020: Missing Column reordering
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents Reorderable parameter with drag-and-drop reorder.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-021: Missing Frozen / Locked columns
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents Locked + FrozenPosition (Left/Right) on columns.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-022: Missing Column Menu
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents per-column popup menu with sort/filter/lock/hide controls.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-023: Missing Multi-column headers
- **Severity:** Low
- **Type:** spec-ahead
- **Description:** Spec documents nested `<TreeListColumnGroup>` for grouped headers.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-024: Missing auto-generated columns
- **Severity:** Low
- **Type:** spec-ahead
- **Description:** Spec documents reflection-based auto-column generation when no columns defined.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-025: Missing Column Visible / column chooser
- **Severity:** Low
- **Type:** spec-ahead
- **Description:** Spec documents Visible parameter per column and column-chooser popup.
- **Affected files:** `MariloTreeList.razor`, `TreeListColumn.cs`
- **Blocking:** No

### GAP-TREELIST-026: Missing Column DisplayFormat
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents DisplayFormat parameter for date/number formatting. Source does raw ToString().
- **Affected files:** `MariloTreeList.razor` (line 191), `TreeListColumn.cs`
- **Blocking:** No

### GAP-TREELIST-027: Missing Column events
- **Severity:** Low
- **Type:** spec-ahead
- **Description:** Spec documents per-column events: OnCellClick, OnHeaderClick, OnResize.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-028: Missing cell Template on columns
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents RenderFragment<TItem> Template per column for custom cell rendering. Source uses hardcoded GetCellValue only.
- **Affected files:** `MariloTreeList.razor` (lines 169, 175)
- **Blocking:** No -- but critical for custom UI

### GAP-TREELIST-029: Missing HeaderTemplate
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents RenderFragment HeaderTemplate per column. Source uses hardcoded col.Title.
- **Affected files:** `MariloTreeList.razor` (line 15)
- **Blocking:** No

### GAP-TREELIST-030: Missing RowTemplate
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents RenderFragment<TItem> RowTemplate for full row override.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-031: Missing NoDataTemplate
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents RenderFragment NoDataTemplate. Source renders empty tbody silently.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-032: Missing Editor / Popup / Filter / Pager templates
- **Severity:** Low
- **Type:** spec-ahead
- **Description:** Spec documents EditorTemplate, PopupFormTemplate, PopupButtonsTemplate, FilterTemplate, PagerTemplate. All blocked on parent features.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No -- downstream of editing/filter/paging

### GAP-TREELIST-033: Missing OnRead remote-data pattern
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents EventCallback<TreeListReadEventArgs> OnRead for server-side paging/sorting/filtering. Source only supports local Data.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-034: Missing expand/collapse events
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents OnExpand + OnCollapse events. ToggleExpand (line 194) fires no events.
- **Affected files:** `MariloTreeList.razor` (lines 194-198)
- **Blocking:** No

### GAP-TREELIST-035: Missing OnItemRender event
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents per-row conditional styling callback. No implementation.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-036: Missing expand-state ARIA attributes
- **Severity:** High
- **Type:** mismatch
- **Description:** Source has role="treegrid", role="row", aria-level. Missing aria-expanded, aria-setsize, aria-posinset required by WAI-ARIA treegrid pattern.
- **Affected files:** `MariloTreeList.razor` (lines 135-136)
- **Blocking:** No -- but fails WCAG conformance

### GAP-TREELIST-037: Missing keyboard navigation
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents Navigable parameter with arrow-key navigation (Up/Down/Left/Right), Home/End, Space/Enter. Zero keyboard support currently.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No -- but fails WCAG 2.1 AA

### GAP-TREELIST-038: Missing public methods (Rebind, Refresh, AutoFitAllColumns)
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents imperative control methods. No public methods exist.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-039: Missing Height / Width / Class / Navigable parameters
- **Severity:** Medium
- **Type:** mismatch
- **Description:** Spec documents these as top-level parameters. Source only has CombineClasses/CombineStyles from base. No Height/Width/Navigable.
- **Affected files:** `MariloTreeList.razor` (lines 4-6)
- **Blocking:** No

### GAP-TREELIST-040: Missing Aggregates
- **Severity:** Medium
- **Type:** spec-ahead
- **Description:** Spec documents per-column aggregates (Sum, Count, Average, Min, Max) rendered as footers.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-041: Missing Row drag-drop / reparenting
- **Severity:** High
- **Type:** spec-ahead
- **Description:** Spec documents RowDraggable + OnRowDrop with tree-specific reparenting and cycle detection.
- **Affected files:** `MariloTreeList.razor`
- **Blocking:** No

### GAP-TREELIST-042: Demo is placeholder
- **Severity:** Medium
- **Type:** demo-gap
- **Description:** Demo page renders a MariloAlert telling users to "see the component spec." Zero demonstrated functionality.
- **Affected files:** `samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor`
- **Blocking:** No -- but users learn nothing

### GAP-TREELIST-043: Missing flat-data / hierarchical-data / load-on-demand demos
- **Severity:** Low
- **Type:** demo-gap
- **Description:** No data-binding scenario demos exist. Only the placeholder Overview.
- **Affected files:** `samples/Marilo.Demo/Pages/Components/TreeList/`
- **Blocking:** No

---

## Provider Style Gaps (from delivery Stage 03)

### GAP-TREELIST-STYLE-01: FluentUI SCSS missing
- **Severity:** High
- **Type:** unstyled
- **Description:** No `_treelist.scss` file exists in FluentUI provider. 6 BEM classes emitted, none styled.
- **Affected files:** FluentUI provider SCSS directory
- **Blocking:** No -- but component renders unstyled

### GAP-TREELIST-STYLE-02: Bootstrap SCSS missing
- **Severity:** High
- **Type:** unstyled
- **Description:** No `_treelist.scss` file exists in Bootstrap provider.
- **Affected files:** Bootstrap provider SCSS directory
- **Blocking:** No -- but component renders unstyled

### GAP-TREELIST-STYLE-03: BEM naming inconsistency
- **Severity:** Low
- **Type:** mismatch
- **Description:** Toggle button uses `mar-tree-item__toggle` (TreeView block name) instead of `mar-treelist__toggle`.
- **Affected files:** `MariloTreeList.razor` (line 156)
- **Blocking:** No

### GAP-TREELIST-STYLE-04: Inline styles should migrate to SCSS
- **Severity:** Low
- **Type:** mismatch
- **Description:** Indentation and layout use inline `style` attributes instead of CSS classes/custom properties.
- **Affected files:** `MariloTreeList.razor` (lines 14, 150, 166)
- **Blocking:** No

---

## Total Counts (including style gaps)

| Category | Count |
|----------|-------|
| Functional gaps (001-043) | 43 |
| Style gaps (STYLE-01 through STYLE-04) | 4 |
| **Grand total** | **47** |

## Architecture Decisions Needed

1. **TreeListColumn backward compat** -- break the `List<TreeListColumn>` parameter or keep as fallback alongside new child-tag wrapper?
2. **DataGrid subsystem reuse strategy** -- extract shared abstractions or copy-paste implementations?
3. **Branch strategy** -- rebuild in place or dedicated `treelist-rewrite` branch?
4. **Flat data vs hierarchical data default** -- which is the recommended quick-start pattern?
5. **Editing UX** -- built-in popup (DataGrid-style) or consumer EditTemplate?
6. **Virtualization + paging composition** -- if both enabled, which wins?
7. **Row drag-drop reparenting semantics** -- child-of vs sibling-before vs sibling-after based on cursor Y-position?
