# Stage 01 -- Spec Review: MariloTreeList

**Date:** 2026-04-12
**Auditor:** w-treelist-delivery
**Component source:** `src/Marilo.Components/DataGrid/MariloTreeList.razor`
**Model:** `src/Marilo.Core/Models/TreeListColumn.cs`
**Spec root:** `docs/component-specs/treelist/`

---

## Summary

The spec documents a full-featured enterprise TreeList (sorting, filtering, paging, editing, selection, drag-drop, virtual scrolling, templates, state management, aggregates, toolbar, column features). The actual component implementation is a minimal scaffold that supports only basic hierarchical rendering with expand/collapse.

**Spec feature areas documented:** 17
**Features implemented in source:** 3 (flat data binding, hierarchical data binding, expand/collapse)
**Gap ratio:** ~82% of documented spec features have zero implementation

---

## A. Parameters Documented in Spec but NOT Implemented in Source

| # | Parameter | Spec Location | Severity |
|---|-----------|---------------|----------|
| 1 | `Pageable` | overview.md, paging.md | HIGH |
| 2 | `PageSize` | paging.md | HIGH |
| 3 | `Page` | paging.md, state.md | HIGH |
| 4 | `Sortable` | sorting.md | HIGH |
| 5 | `SortMode` | sorting.md | MEDIUM |
| 6 | `FilterMode` (TreeListFilterMode enum) | filter/overview.md | HIGH |
| 7 | `Filterable` (column-level) | filter/overview.md | MEDIUM |
| 8 | `SelectionMode` | selection/overview.md | HIGH |
| 9 | `SelectedItems` | selection/overview.md, state.md | HIGH |
| 10 | `SelectedCells` | selection/overview.md | MEDIUM |
| 11 | `EditMode` | editing/overview.md | HIGH |
| 12 | `RowDraggable` | row-drag-drop.md | MEDIUM |
| 13 | `ScrollMode` (TreeListScrollMode enum) | virtual-scrolling.md | MEDIUM |
| 14 | `RowHeight` | virtual-scrolling.md | MEDIUM |
| 15 | `Height` | overview.md, virtual-scrolling.md | MEDIUM |
| 16 | `Width` | overview.md | LOW |
| 17 | `Navigable` | overview.md | MEDIUM |
| 18 | `Class` | overview.md | LOW -- partially via CombineClasses |
| 19 | `Expandable` (column-level) | overview.md | MEDIUM -- hardcoded to first column |

## B. Parameters Implemented but NOT Documented in Spec

| # | Parameter | Notes |
|---|-----------|-------|
| 1 | `ItemsField` | Documented in data-binding/overview.md -- OK |
| 2 | `HasChildrenField` | Documented in data-binding/overview.md -- OK |
| 3 | `OnRowClick` | Partially documented in events.md -- OK |

No undocumented parameters found. All source parameters have spec coverage.

## C. Events Documented but NOT Implemented

| # | Event | Spec Location |
|---|-------|---------------|
| 1 | `OnExpand` | events.md |
| 2 | `OnCollapse` | events.md |
| 3 | `OnAdd` | events.md, editing/overview.md |
| 4 | `OnCreate` | events.md |
| 5 | `OnUpdate` | events.md |
| 6 | `OnDelete` | events.md |
| 7 | `OnEdit` | events.md |
| 8 | `OnCancel` | events.md |
| 9 | `SelectedItemsChanged` | events.md, selection/rows.md |
| 10 | `SelectedCellsChanged` | events.md, selection/cells.md |
| 11 | `OnModelInit` | events.md |
| 12 | `OnRowDoubleClick` | events.md |
| 13 | `OnRowContextMenu` | events.md |
| 14 | `OnRowRender` | events.md |
| 15 | `OnRowDrop` | events.md, row-drag-drop.md |
| 16 | `PageChanged` | events.md |
| 17 | `PageSizeChanged` | events.md |
| 18 | `OnStateInit` | state.md |
| 19 | `OnStateChanged` | state.md |

## D. Child Component Tags Documented but NOT Implemented

| # | Tag | Spec Location |
|---|-----|---------------|
| 1 | `<TreeListColumns>` wrapper | overview.md (columns rendered via List<TreeListColumn> parameter instead) |
| 2 | `<TreeListColumn>` component | overview.md (using POCO model, not Blazor component) |
| 3 | `<TreeListToolBar>` | toolbar.md |
| 4 | `<TreeListToolBarAddTool>` | toolbar.md |
| 5 | `<TreeListToolBarSearchBoxTool>` | toolbar.md |
| 6 | `<TreeListToolBarCustomTool>` | toolbar.md |
| 7 | `<TreeListCommandColumn>` | columns/command.md |

## E. Feature Areas with Zero Implementation

| # | Feature Area | Spec Files |
|---|-------------|------------|
| 1 | Paging | paging.md |
| 2 | Sorting | sorting.md |
| 3 | Filtering | filter/*.md (5 files) |
| 4 | Editing (InCell, Inline, Popup) | editing/*.md (5 files) |
| 5 | Selection (row, cell) | selection/*.md (3 files) |
| 6 | Templates (column, row, header, editor, filter, no-data, pager, popup) | templates/*.md (11 files) |
| 7 | Column features (reorder, resize, frozen, visible, auto-generated, multi-column headers, checkbox, command, menu, virtual) | columns/*.md (14 files) |
| 8 | State management | state.md |
| 9 | Toolbar | toolbar.md |
| 10 | Virtual scrolling | virtual-scrolling.md |
| 11 | Row drag and drop | row-drag-drop.md |
| 12 | Aggregates | aggregates.md (published: false, but documented) |
| 13 | Accessibility / WAI-ARIA | accessibility/*.md (2 files) |
| 14 | Refresh data / Rebind | refresh-data.md |

## F. TreeListColumn Model Gaps

The `TreeListColumn` POCO class (`Marilo.Core.Models.TreeListColumn`) has only 3 properties: `Title`, `Field`, `Width`.

Spec documents these additional column properties (not exhaustive):
- `Expandable`, `Editable`, `Sortable`, `Filterable`, `Visible`, `Locked`, `Reorderable`, `Resizable`
- `DisplayFormat`, `TextAlign`, `HeaderClass`, `MinResizableWidth`, `MaxResizableWidth`
- `Template`, `HeaderTemplate`, `FilterCellTemplate`, `EditorTemplate`
- `Id`, `ShowColumnMenu`

---

## Conclusion

The MariloTreeList component is at **early scaffold** stage. It provides basic flat/hierarchical data binding and expand/collapse rendering. The spec documents a comprehensive enterprise-grade TreeList. The implementation gap is very large -- approximately 82% of specified features have no source code.
