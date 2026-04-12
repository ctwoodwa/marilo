# Stage 01 — Spec Review: MariloTreeList
**Audited:** 2026-04-11  
**Source file:** `src/Marilo.Components/DataGrid/MariloTreeList.razor` (single file, 199 lines)  
**Model file:** `src/Marilo.Core/Models/TreeListColumn.cs`  
**Spec directory:** `docs/component-specs/treelist/`

---

## Summary

| Category | Count |
|---|---|
| Undocumented (in source, not in spec) | 1 |
| Spec-ahead (in spec, not in source) | 47 |
| Mismatch (name/type/shape differs) | 5 |

The component is an **early-stage scaffold**. The source implements a minimal viable tree render loop with 6 parameters and 1 event. The spec defines a rich enterprise-grade API surface spanning paging, sorting, filtering, editing, selection, state, drag-drop, virtual scrolling, toolbar, templates, and more — none of which are implemented.

---

## Section 1 — Undocumented (in source, not in spec)

Parameters/members present in `MariloTreeList.razor` or `TreeListColumn.cs` that are not described in any spec file.

---

**ID:** SPEC-treelist-001  
**Type:** undocumented  
**Parameter/Event:** `Columns` (on `MariloTreeList`)  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing (spec uses `<TreeListColumns>` child content tag) | `Columns` — `[Parameter] public List<TreeListColumn> Columns` |
| Type | missing | `List<TreeListColumn>` |
| Default | missing | `new()` |
| Description | missing | A flat list of column definitions passed as a parameter rather than child content |

**Recommended action:** Spec describes columns as declared via `<TreeListColumns>/<TreeListColumn>` child-content syntax (see `columns/bound.md` line 40). The source accepts a raw `List<TreeListColumn>` parameter. This is a structural mismatch — spec pattern not yet implemented; the raw list approach should either be updated to match the spec's child-content model or the spec should document both approaches.  
**Delegated to:** gap-analysis-resolution intake (architecture decision — parameter vs. child-content pattern)

---

## Section 2 — Spec-Ahead (in spec, not in source)

Parameters, events, child components, and sub-features defined in the spec with no corresponding implementation in source.

---

### MariloTreeList — Root Component Parameters

**ID:** SPEC-treelist-002  
**Type:** spec-ahead  
**Parameter/Event:** `Pageable`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Pageable` | missing |
| Type | `bool` | missing |
| Default | `false` | N/A |
| Description | Enables paging of visible (expanded) rows | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-003  
**Type:** spec-ahead  
**Parameter/Event:** `PageSize`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `PageSize` | missing |
| Type | `int` | missing |
| Default | `10` | N/A |
| Description | Number of visible rows per page | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-004  
**Type:** spec-ahead  
**Parameter/Event:** `Page`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Page` | missing |
| Type | `int` | missing |
| Default | `1` | N/A |
| Description | Current page index (1-based), supports `@bind-Page` | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-005  
**Type:** spec-ahead  
**Parameter/Event:** `Sortable`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Sortable` | missing |
| Type | `bool` | missing |
| Default | `false` | N/A |
| Description | Enables column header click sorting; hierarchical order preserved | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-006  
**Type:** spec-ahead  
**Parameter/Event:** `SortMode`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `SortMode` | missing |
| Type | `Marilo.Blazor.SortMode` enum | missing |
| Default | `Single` | N/A |
| Description | Allows single or multiple column sorting | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-007  
**Type:** spec-ahead  
**Parameter/Event:** `FilterMode`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `FilterMode` | missing |
| Type | `TreeListFilterMode` enum (`FilterRow`, `FilterMenu`, `None`) | missing |
| Default | `None` | N/A |
| Description | Activates filter row or filter menu UI on columns | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-008  
**Type:** spec-ahead  
**Parameter/Event:** `SelectionMode`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `SelectionMode` | missing |
| Type | `TreeListSelectionMode` enum (`None`, `Single`, `Multiple`) | missing |
| Default | `None` | N/A |
| Description | Enables row or cell selection | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-009  
**Type:** spec-ahead  
**Parameter/Event:** `SelectedItems`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `SelectedItems` | missing |
| Type | `IEnumerable<T>` | missing |
| Default | empty | N/A |
| Description | Two-way bindable collection of selected rows | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-010  
**Type:** spec-ahead  
**Parameter/Event:** `SelectedCells`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `SelectedCells` | missing |
| Type | `IEnumerable<TreeListSelectedCellDescriptor>` | missing |
| Default | empty | N/A |
| Description | Two-way bindable collection of selected cells | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-011  
**Type:** spec-ahead  
**Parameter/Event:** `Height`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Height` | missing |
| Type | `string` | missing |
| Default | `null` | N/A |
| Description | CSS height of the component (any valid CSS unit) — from `overview.md` line 152 | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-012  
**Type:** spec-ahead  
**Parameter/Event:** `Width`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Width` | missing |
| Type | `string` | missing |
| Default | `null` (expands to fill container) | N/A |
| Description | CSS width of the component — from `overview.md` line 153 | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-013  
**Type:** spec-ahead  
**Parameter/Event:** `Navigable`  
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Navigable` | missing |
| Type | `bool` | missing |
| Default | `false` | N/A |
| Description | Enables keyboard navigation — from `overview.md` line 151 | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-014  
**Type:** spec-ahead  
**Parameter/Event:** `EditMode`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `EditMode` | missing |
| Type | `TreeListEditMode` enum (`Inline`, `Popup`, `Incell`) | missing |
| Default | unspecified | N/A |
| Description | Controls the editing UX mode — from `toolbar.md` line 55 | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-015  
**Type:** spec-ahead  
**Parameter/Event:** `ConfirmDelete`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `ConfirmDelete` | missing |
| Type | `bool` | missing |
| Default | `false` | N/A |
| Description | Shows a confirmation dialog before deleting — from `toolbar.md` line 54 | missing |

**Recommended action:** Implement parameter  
**Delegated to:** gap-analysis-resolution intake

---

### MariloTreeList — Events

**ID:** SPEC-treelist-016  
**Type:** spec-ahead  
**Parameter/Event:** `OnExpand`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnExpand` | missing |
| Type | `EventCallback<TreeListExpandEventArgs<T>>` | missing |
| Default | N/A | N/A |
| Description | Fires when user expands a collapsed row — from `events.md` line 43 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-017  
**Type:** spec-ahead  
**Parameter/Event:** `OnCollapse`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnCollapse` | missing |
| Type | `EventCallback<TreeListCollapseEventArgs<T>>` | missing |
| Default | N/A | N/A |
| Description | Fires when user collapses an expanded row — from `events.md` line 48 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-018  
**Type:** spec-ahead  
**Parameter/Event:** `OnRowDoubleClick`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnRowDoubleClick` | missing |
| Type | `EventCallback<TreeListRowClickEventArgs<T>>` | missing |
| Default | N/A | N/A |
| Description | Fires on double-click of a row — from `events.md` line 36 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-019  
**Type:** spec-ahead  
**Parameter/Event:** `OnRowContextMenu`  
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnRowContextMenu` | missing |
| Type | `EventCallback<TreeListRowClickEventArgs<T>>` | missing |
| Default | N/A | N/A |
| Description | Fires on right-click of a row — from `events.md` line 37 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-020  
**Type:** spec-ahead  
**Parameter/Event:** `OnRowRender`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnRowRender` | missing |
| Type | `Action<TreeListRowRenderEventArgs<T>>` | missing |
| Default | N/A | N/A |
| Description | Fires before each row is rendered, allows adding CSS classes — from `events.md` line 38 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-021  
**Type:** spec-ahead  
**Parameter/Event:** `OnRowDrop`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnRowDrop` | missing |
| Type | `EventCallback<TreeListRowDropEventArgs<T>>` | missing |
| Default | N/A | N/A |
| Description | Fires when a dragged row is dropped — from `events.md` line 39 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-022  
**Type:** spec-ahead  
**Parameter/Event:** `SelectedItemsChanged`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `SelectedItemsChanged` | missing |
| Type | `EventCallback<IEnumerable<T>>` | missing |
| Default | N/A | N/A |
| Description | Fires when row selection changes — from `events.md` line 33 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-023  
**Type:** spec-ahead  
**Parameter/Event:** `SelectedCellsChanged`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `SelectedCellsChanged` | missing |
| Type | `EventCallback<IEnumerable<TreeListSelectedCellDescriptor>>` | missing |
| Default | N/A | N/A |
| Description | Fires when cell selection changes — from `events.md` line 34 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-024  
**Type:** spec-ahead  
**Parameter/Event:** `OnModelInit`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnModelInit` | missing |
| Type | `Func<T>` | missing |
| Default | N/A | N/A |
| Description | Returns a new model instance for add/insert operations — from `events.md` line 35 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-025  
**Type:** spec-ahead  
**Parameter/Event:** `PageChanged`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `PageChanged` | missing |
| Type | `EventCallback<int>` | missing |
| Default | N/A | N/A |
| Description | Fires when the user changes the current page — from `paging.md` line 207 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-026  
**Type:** spec-ahead  
**Parameter/Event:** `PageSizeChanged`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `PageSizeChanged` | missing |
| Type | `EventCallback<int>` | missing |
| Default | N/A | N/A |
| Description | Fires when the user changes the page size — from `paging.md` line 208 | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-027  
**Type:** spec-ahead  
**Parameter/Event:** `OnStateInit`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnStateInit` | missing |
| Type | `EventCallback<TreeListStateEventArgs<T>>` | missing |
| Default | N/A | N/A |
| Description | Fires on first render to allow setting initial state — from `state.md` | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-028  
**Type:** spec-ahead  
**Parameter/Event:** `OnStateChanged`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnStateChanged` | missing |
| Type | `EventCallback<TreeListStateEventArgs<T>>` | missing |
| Default | N/A | N/A |
| Description | Fires when user changes sorting, paging, filtering state — from `state.md` | missing |

**Recommended action:** Implement event  
**Delegated to:** gap-analysis-resolution intake

---

### CRUD Events (from `events.md`)

**ID:** SPEC-treelist-029  
**Type:** spec-ahead  
**Parameter/Event:** `OnAdd`, `OnCreate`, `OnUpdate`, `OnDelete`, `OnEdit`, `OnCancel`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnAdd / OnCreate / OnUpdate / OnDelete / OnEdit / OnCancel` | missing |
| Type | `EventCallback<TreeListCommandEventArgs<T>>` (each) | missing |
| Default | N/A | N/A |
| Description | CUD lifecycle events for inline/popup/incell editing — from `events.md` lines 21-25 | missing |

**Recommended action:** Implement all 6 events as part of editing feature  
**Delegated to:** gap-analysis-resolution intake

---

### TreeListColumn — Additional Parameters

**ID:** SPEC-treelist-030  
**Type:** spec-ahead  
**Parameter/Event:** `Expandable` (on `TreeListColumn`)  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Expandable` | missing |
| Type | `bool` | missing |
| Default | `false` | N/A |
| Description | Column shows expand/collapse arrow and indentation — from `columns/bound.md` line 123 | missing |

**Recommended action:** Implement on column model/component  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-031  
**Type:** spec-ahead  
**Parameter/Event:** `DisplayFormat` (on `TreeListColumn`)  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `DisplayFormat` | missing |
| Type | `string` | missing |
| Default | `null` | N/A |
| Description | C# format string for cell value rendering — from `columns/bound.md` line 129 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-032  
**Type:** spec-ahead  
**Parameter/Event:** `TextAlign` (on `TreeListColumn`)  
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `TextAlign` | missing |
| Type | `ColumnTextAlign` enum (`Left`, `Right`, `Center`) | missing |
| Default | unset (browser default) | N/A |
| Description | Horizontal alignment of data cells — from `columns/bound.md` line 130 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-033  
**Type:** spec-ahead  
**Parameter/Event:** `HeaderClass` (on `TreeListColumn`)  
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `HeaderClass` | missing |
| Type | `string` | missing |
| Default | `null` | N/A |
| Description | Custom CSS class for the header cell — from `columns/bound.md` line 132 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-034  
**Type:** spec-ahead  
**Parameter/Event:** `MinResizableWidth` / `MaxResizableWidth` (on `TreeListColumn`)  
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MinResizableWidth` / `MaxResizableWidth` | missing |
| Type | `decimal` | missing |
| Default | `30` / `0` | N/A |
| Description | Min/max pixel constraints during column resize — from `columns/bound.md` lines 133-134 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-035  
**Type:** spec-ahead  
**Parameter/Event:** `Locked` (on `TreeListColumn`)  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Locked` | missing |
| Type | `bool` | missing |
| Default | `false` | N/A |
| Description | Frozen/pinned column — from `columns/bound.md` line 135 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-036  
**Type:** spec-ahead  
**Parameter/Event:** `Reorderable` (on `TreeListColumn`)  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Reorderable` | missing |
| Type | `bool` | missing |
| Default | `true` | N/A |
| Description | Whether the user can drag-reorder this column — from `columns/bound.md` line 136 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-037  
**Type:** spec-ahead  
**Parameter/Event:** `Resizable` (on `TreeListColumn`)  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Resizable` | missing |
| Type | `bool` | missing |
| Default | `true` | N/A |
| Description | Whether the user can resize this column — from `columns/bound.md` line 137 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-038  
**Type:** spec-ahead  
**Parameter/Event:** `Visible` (on `TreeListColumn`)  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Visible` | missing |
| Type | `bool?` | missing |
| Default | `null` (treated as `true`) | N/A |
| Description | Hides the column when set to `false` — from `columns/bound.md` line 138 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-039  
**Type:** spec-ahead  
**Parameter/Event:** `ShowColumnMenu` (on `TreeListColumn`)  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `ShowColumnMenu` | missing |
| Type | `bool` | missing |
| Default | `true` | N/A |
| Description | Enables/disables column menu for this column — from `columns/bound.md` line 140 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-040  
**Type:** spec-ahead  
**Parameter/Event:** `VisibleInColumnChooser` (on `TreeListColumn`)  
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `VisibleInColumnChooser` | missing |
| Type | `bool` | missing |
| Default | `true` | N/A |
| Description | Controls appearance in column chooser — from `columns/bound.md` line 141 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-041  
**Type:** spec-ahead  
**Parameter/Event:** `Editable` (on `TreeListColumn`)  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Editable` | missing |
| Type | `bool` | missing |
| Default | `true` | N/A |
| Description | Allows/prevents editing of this column — from `columns/bound.md` line 150 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-042  
**Type:** spec-ahead  
**Parameter/Event:** `Filterable` (on `TreeListColumn`)  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Filterable` | missing |
| Type | `bool` | missing |
| Default | `true` | N/A |
| Description | Enables/disables filtering for this column — from `columns/bound.md` line 153 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-043  
**Type:** spec-ahead  
**Parameter/Event:** `Sortable` (on `TreeListColumn`)  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Sortable` (column-level) | missing |
| Type | `bool` | missing |
| Default | `true` | N/A |
| Description | Enables/disables sorting for this column — from `columns/bound.md` line 155 | missing |

**Recommended action:** Implement on column model  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-044  
**Type:** spec-ahead  
**Parameter/Event:** `Template`, `HeaderTemplate`, `EditorTemplate` (on `TreeListColumn`)  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Template` / `HeaderTemplate` / `EditorTemplate` | missing |
| Type | `RenderFragment<T>` | missing |
| Default | N/A | N/A |
| Description | Custom cell, header, and editor content — from `columns/bound.md` lines 158-162 | missing |

**Recommended action:** Implement render fragments on column component  
**Delegated to:** gap-analysis-resolution intake

---

### Root-Level Child Content / Sub-components

**ID:** SPEC-treelist-045  
**Type:** spec-ahead  
**Parameter/Event:** `<TreeListColumns>` child content tag  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `<TreeListColumns>` | missing (source uses flat `List<TreeListColumn>` parameter) |
| Type | `RenderFragment` wrapping `<TreeListColumn>` instances | missing |
| Default | N/A | N/A |
| Description | Declarative column definitions block — from `overview.md` line 23, `columns/bound.md` line 40 | missing |

**Recommended action:** Implement child content pattern  
**Delegated to:** gap-analysis-resolution intake (architecture decision)

---

**ID:** SPEC-treelist-046  
**Type:** spec-ahead  
**Parameter/Event:** `<TreeListToolBar>` sub-component  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `<TreeListToolBar>` | missing |
| Type | `RenderFragment` | missing |
| Default | N/A | N/A |
| Description | Toolbar with built-in Add/SearchBox/Spacer and custom tools — from `toolbar.md` | missing |

**Recommended action:** Implement toolbar sub-component  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-047  
**Type:** spec-ahead  
**Parameter/Event:** `<TreeListSettings>/<TreeListPagerSettings>` sub-component  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `<TreeListSettings>/<TreeListPagerSettings>` | missing |
| Type | Child component config block | missing |
| Default | N/A | N/A |
| Description | Advanced pager configuration (InputType, PageSizes, ButtonCount, Adaptive, Position) — from `paging.md` lines 224-228 | missing |

**Recommended action:** Implement settings sub-components  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-048  
**Type:** spec-ahead  
**Parameter/Event:** `<TreeListCommandColumn>` sub-component  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `<TreeListCommandColumn>` with `<TreeListCommandButton>` | missing |
| Type | Specialized column sub-component | missing |
| Default | N/A | N/A |
| Description | Edit/Delete/Add/Save/Cancel command buttons in a column — from `toolbar.md` lines 74-82 | missing |

**Recommended action:** Implement command column  
**Delegated to:** gap-analysis-resolution intake

---

## Section 3 — Mismatches

Parameters present in both source and spec but with name, type, or structural differences.

---

**ID:** SPEC-treelist-M01  
**Type:** mismatch  
**Parameter/Event:** `OnRowClick`  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnRowClick` | `OnRowClick` |
| Type | `EventCallback<TreeListRowClickEventArgs<T>>` (spec, from `events.md` line 36) | `EventCallback<TItem>` (source, `MariloTreeList.razor` line 34) |
| Default | N/A | N/A |
| Description | Fires on row click | Fires on row click, but passes `TItem` directly not wrapped in event args |

**Recommended action:** Update source to use `TreeListRowClickEventArgs<T>` wrapper type to match spec contract  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-M02  
**Type:** mismatch  
**Parameter/Event:** `Columns` parameter vs `<TreeListColumns>` child content  
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `<TreeListColumns>` (RenderFragment child content) | `Columns` (`[Parameter] List<TreeListColumn>`) |
| Type | Blazor child-content / render fragment pattern | C# list parameter |
| Default | N/A | `new()` |
| Description | The spec expects declarative Razor child-content column syntax; the source uses an imperative list parameter |

**Recommended action:** Replace `List<TreeListColumn>` parameter with `<TreeListColumns>` child-content pattern; `TreeListColumn` should become a Blazor component  
**Delegated to:** gap-analysis-resolution intake (architecture decision)

---

**ID:** SPEC-treelist-M03  
**Type:** mismatch  
**Parameter/Event:** `TreeListColumn.Title`  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Title` | `Title` |
| Type | `string` | `string` |
| Default | spec: derived from `[Display]` attribute or field name if not set | source: `string.Empty` (`TreeListColumn.cs` line 9) |
| Description | Spec states the treelist falls back to `[Display(Name=...)]` attribute or the field name; source initialises to empty string with no fallback |

**Recommended action:** Implement attribute-based and field-name fallback in column title resolution  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-M04  
**Type:** mismatch  
**Parameter/Event:** `HasChildrenField` — hierarchy detection  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `HasChildrenField` | `HasChildrenField` |
| Type | `string` (field name) | `string?` (`MariloTreeList.razor` line 31) |
| Default | `null` | `null` |
| Description | Source supports this field for load-on-demand sentinel. Spec (`data-binding/load-on-demand.md`) describes it as used to show expand arrows for lazily-loaded nodes with no actual children yet. Source uses it at lines 76-80 and 94-98 but the expand toggle still renders children from `_expandedIds` — lazy load callback (`OnExpand`) is not implemented. |

**Recommended action:** `HasChildrenField` exists in source but the connected `OnExpand` lazy-load pattern is incomplete; implement `OnExpand` event (see SPEC-treelist-016) to close the loop  
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treelist-M05  
**Type:** mismatch  
**Parameter/Event:** `Class` parameter (from `MariloComponentBase`)  
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Class` | `Class` (inherited from `MariloComponentBase`) |
| Type | `string` | `string` (base class) |
| Default | spec: "additional CSS class rendered to `div.k-treelist`" | source: `CombineClasses("mar-treelist")` — CSS prefix is `mar-` not `k-` |
| Description | Spec uses Telerik `k-` prefix in the description text (`overview.md` line 150); source correctly uses Marilo `mar-` prefix. The spec prose needs updating, but the parameter itself exists. |

**Recommended action:** Update spec description to reference `div.mar-treelist` not `div.k-treelist`  
**Delegated to:** spec update only

---

## Notes

- The `_expandedIds` and `ToggleExpand` private implementation in source is correct behavior but has no corresponding spec-exposed state API (the `state.md` spec describes `TreeListState<TItem>` with expanded-item tracking that should surface this).
- No test files were found for `MariloTreeList` (`_config/delivery-context.md` records test files as `UNKNOWN`). A separate test-file audit is needed.
- The `src/Marilo.Components/DataGrid/` folder is a co-location with `MariloDataGrid` — the delivery context `component-source` path is correct but misleading; a future cleanup to move `MariloTreeList.razor` to its own folder would align with Marilo folder conventions.
