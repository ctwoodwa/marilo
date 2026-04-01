---
component: MariloBreadcrumb, MariloBreadcrumbItem, MariloContextMenu, MariloEnvironmentBadge, MariloMenu, MariloMenuItem, MariloPagination, MariloTimeRangeSelector, MariloToolbar, MariloToolbarButton, MariloToolbarGroup, MariloToolbarSeparator, MariloToolbarToggleButton, MariloTreeItem, MariloTreeView
phase: 2
status: not-started
complexity: mixed
priority: high
owner: ""
last-updated: 2026-03-31
depends-on: [MariloThemeProvider]
external-resources:
  - name: ""
    url: ""
    license: ""
    approved: false
---

# Resolution Status: Navigation

## Current Phase
Phase 2: TreeView, Menu, ContextMenu, Pagination are Phase 2; Breadcrumb, Toolbar are Phase 3; remaining components are Phase 4

## Gap Summary
TreeView has 6 gaps (no expanded/selected binding), Menu has 7 gaps (hierarchy not wired), ContextMenu has 8 gaps (no selector/data binding), Breadcrumb 7 gaps, Toolbar 5 gaps, Pagination 6 gaps. Other components have minor gaps.

## Resolution Progress

### Completed
- [x] **MariloPagination** — IMPLEMENTED (6/6 gaps resolved): Added `Total`+`PageSize` model (auto-computes pages), renamed `CurrentPage`→`Page`, `MaxVisiblePages`→`ButtonCount`, added `PageSizes` dropdown, `PageSizeChanged` event, `ShowInfo` page info text. Updated all sample pages.

### In Progress
- [ ] **MariloTreeView** — 6 original gaps IMPLEMENTED; **16 new gaps identified** from source review (see below)

### Not Started
- [ ] MariloMenu — 7 gaps
- [ ] MariloContextMenu — 8 gaps
- [ ] MariloBreadcrumb — 7 gaps
- [ ] MariloToolbar — 5 gaps
- [ ] Minor components (BreadcrumbItem, EnvironmentBadge, MenuItem, ToolbarButton, etc.)

---

## MariloTreeView — Source Review Findings

### Sources Evaluated

| Source | Type | Key Value |
|--------|------|-----------|
| **Radzen Tree** | Blazor OSS | Tri-state checkbox propagation, SingleExpand, CheckedValues binding, TreeLevel data binding |
| **MudBlazor TreeView** | Blazor OSS | SelectionMode enum, ServerData lazy loading, FilterFunc, ExpandOnClick, AutoExpand, Dense mode |
| **BlazorVirtualTreeView** | Blazor OSS | Virtualization strategy for hierarchical data, programmatic navigation (SelectNodeAsync), dynamic node manipulation |
| **Fancytree** | JS OSS | Keyboard navigation, persistence, filtering/search, table/tree hybrid, extension system, conditional per-node options |
| **excubo-ag Blazor.TreeViews** | Blazor OSS | Minimal JS footprint, CheckboxTemplate delegate, RefreshSelection() explicit sync |
| **Fluent UI Blazor** | Blazor OSS | LazyLoadItems callback, SelectedItem binding, model-driven API |
| **jsTree** | JS OSS | Plugin architecture for feature modularity, checkbox/DnD/search/context-menu plugins, mass loading |
| **Syncfusion TreeView** | Commercial | Enterprise feature benchmark: lazy loading, checkbox mode, node editing, DnD, multi-select, keyboard nav |
| **Telerik TreeView** | Commercial | Direct parity target: checkboxes, DnD, bindings, lazy loading |

### Original 6 Gaps (IMPLEMENTED in code)

| # | Gap | Status | Implementation |
|---|-----|--------|----------------|
| 1 | ExpandedItems two-way binding | ✅ Done | `@bind-ExpandedItems` via `IEnumerable<string>` parameter + `ExpandedItemsChanged` |
| 2 | SelectedItems binding | ✅ Done | `@bind-SelectedItems` via `IEnumerable<string>` parameter + `SelectedItemsChanged` |
| 3 | Size parameter | ✅ Done | `Size` string parameter applies `font-size` style |
| 4 | Drag-and-drop | ✅ Done | `EnableDragDrop` + `OnItemDrop` event with HTML5 drag events |
| 5 | Rebind() method | ✅ Done | Public `Rebind()` calls `StateHasChanged()` |
| 6 | Item selection API | ✅ Done | `SelectItem()` clears + adds to `_selectedIds`, fires `SelectedItemsChanged` |

### New Gaps Identified from Source Review (16 gaps)

#### Priority 1 — Core Behavioral Gaps

| # | Gap | Severity | Source(s) | Description | Recommended Pattern |
|---|-----|----------|-----------|-------------|---------------------|
| 7 | **Tri-state checkbox propagation** | High | Radzen, MudBlazor, Fancytree | Checking a parent should cascade to children; checking all children should update parent to checked; partial children = indeterminate state. Current `CheckBoxMode` only supports `None`/`Single` with no hierarchical propagation. | **Radzen pattern**: Add `AllowCheckChildren` (bool, default true) and `AllowCheckParents` (bool, default true) parameters. Implement `GetAllChildValues()` recursive helper and `IsChecked()` returning `bool?` (null = indeterminate). Use `CheckedValues`/`CheckedValuesChanged` bindable collection instead of internal `_checkedIds`. Render indeterminate state via HTML `indeterminate` property (requires minimal JS interop or CSS `:indeterminate`). |
| 8 | **CheckedItems two-way binding** | High | Radzen, MudBlazor | `_checkedIds` is internal-only; consumers cannot bind to or observe checked state. | Expose `CheckedItems` / `CheckedItemsChanged` as `IEnumerable<string>` parameters mirroring `ExpandedItems` pattern already in place. Sync from parameter in `OnParametersSet`. |
| 9 | **Multi-selection mode** | High | MudBlazor, Fancytree, Telerik | `SelectItem()` always clears `_selectedIds` (single-select only). No multi-select or toggle-select support. | **MudBlazor pattern**: Add `SelectionMode` enum (`Single`, `Multiple`, `Toggle`). In `Multiple` mode, hold Ctrl/Shift to add; in `Toggle`, click toggles individual items. Keep `SelectedItems`/`SelectedItemsChanged` binding. |
| 10 | **Lazy loading callback** | High | MudBlazor, Fluent UI, Fancytree, Syncfusion | `HasChildrenField` marks expandable nodes and a loading indicator exists, but there is no actual callback to load children on demand. | **MudBlazor/Fluent UI pattern**: Add `LoadChildrenAsync` parameter of type `Func<object, Task<IEnumerable<object>>>`. On first expand of a node with `HasChildren=true` and empty children, invoke callback, merge results into tree, clear loading state. Track loaded state per-node to avoid re-fetching. |
| 11 | **Keyboard navigation** | High | Fancytree, Telerik, Syncfusion | No keyboard support. Tree should be navigable via arrow keys, expandable via Right/Left, activatable via Enter/Space. | **Fancytree pattern**: Tree is single tab-stop (`tabindex="0"` on `<ul role="tree">`). Track `_focusedNodeId`. Arrow Up/Down move focus, Left collapses or moves to parent, Right expands or moves to first child, Enter/Space selects/toggles. Add `@onkeydown` handler on root element. |

#### Priority 2 — Enhanced Interaction Gaps

| # | Gap | Severity | Source(s) | Description | Recommended Pattern |
|---|-----|----------|-----------|-------------|---------------------|
| 12 | **ExpandOnClick / ExpandOnDoubleClick** | Medium | MudBlazor | Currently only the toggle button (▶/▼) expands nodes. Clicking the label should optionally expand. | Add `ExpandOnClick` (bool) and `ExpandOnDoubleClick` (bool) parameters. Wire `onclick`/`ondblclick` on the header `<div>` to call `ToggleNodeAsync` when enabled. |
| 13 | **SingleExpand (accordion) mode** | Medium | Radzen | No option to auto-collapse siblings when a node expands. | Add `SingleExpand` (bool) parameter. In `ToggleNodeAsync`, when expanding and `SingleExpand=true`, remove sibling IDs from `_expandedIds` before adding the new one. Requires tracking parent-child relationships in the flattened ID set. |
| 14 | **AutoExpand to show selection** | Medium | MudBlazor | When `SelectedItems` is set programmatically, ancestor nodes should auto-expand to reveal the selected node. | Add `AutoExpand` (bool) parameter. In `OnParametersSet`, when `SelectedItems` changes and `AutoExpand=true`, walk the tree to find ancestors of selected nodes and add them to `_expandedIds`. |
| 15 | **ExpandAllAsync / CollapseAllAsync** | Medium | MudBlazor, BlazorVirtualTreeView | Only `Rebind()` is exposed. No batch expand/collapse. | Add public `ExpandAllAsync()` and `CollapseAllAsync()` methods. `ExpandAll` walks tree recursively adding all IDs to `_expandedIds`; `CollapseAll` clears `_expandedIds`. Both fire `ExpandedItemsChanged`. |
| 16 | **FilterFunc / Search** | Medium | MudBlazor, Fancytree, jsTree | No filtering or search capability. | Add `FilterFunc` parameter as `Func<object, bool>` (or async variant). In `RenderNodes`, skip nodes that don't match predicate but keep ancestors of matching nodes visible. Add `FilterAsync()` public method to trigger re-evaluation. |
| 17 | **Disabled / ReadOnly** | Medium | MudBlazor, Fancytree | No way to disable the entire tree or make it read-only. | Add `Disabled` (bool) and `ReadOnly` (bool) parameters. When `Disabled`, suppress all click/drag/keyboard handlers and apply `aria-disabled`. When `ReadOnly`, show current state but prevent changes. |

#### Priority 3 — Advanced / Enterprise Gaps

| # | Gap | Severity | Source(s) | Description | Recommended Pattern |
|---|-----|----------|-----------|-------------|---------------------|
| 18 | **Virtualization** | Medium | BlazorVirtualTreeView | All nodes render to DOM regardless of visibility. Degrades with 1000+ nodes. | **BlazorVirtualTreeView pattern**: Flatten visible tree into a list (only expanded branches), wrap in Blazor `<Virtualize>` component with `ItemSize` parameter. Maintain a `FlattenVisibleNodes()` method that recomputes on expand/collapse. This is the most architecturally significant enhancement. |
| 19 | **Programmatic navigation (SelectNodeAsync)** | Low | BlazorVirtualTreeView | No way to programmatically navigate to a node, expanding its ancestors and scrolling it into view. | Add `SelectNodeAsync(string id)` method that expands all ancestors, sets selection, and (with virtualization) scrolls to the node. |
| 20 | **ItemContextMenu event** | Low | Radzen, jsTree, Fancytree | No right-click/context menu integration point. | Add `OnItemContextMenu` `EventCallback<(object Item, MouseEventArgs Args)>`. Wire `@oncontextmenu` on item headers. Pairs with `MariloContextMenu` component. |
| 21 | **CheckboxTemplate** | Low | excubo-ag | Checkbox rendering is hardcoded `<input type="checkbox">`. No customization. | Add `CheckboxTemplate` `RenderFragment<CheckboxContext>` where `CheckboxContext` includes `(bool Checked, bool? Indeterminate, Action<bool> OnChange, bool Disabled)`. |
| 22 | **Node editing (inline rename)** | Low | jsTree, Fancytree, Syncfusion | No inline editing of node text. | Add `AllowEditing` (bool) and `OnItemEdit` `EventCallback<(string Id, string NewText)>`. Double-click or F2 replaces label with input field. |

### Implementation Priority Recommendation

**Phase 1 (Core):** Gaps 7-11 — Tri-state checkboxes, CheckedItems binding, multi-selection, lazy loading, keyboard navigation
**Phase 2 (Enhanced):** Gaps 12-17 — ExpandOnClick, SingleExpand, AutoExpand, batch expand/collapse, filtering, disabled/readonly
**Phase 3 (Advanced):** Gaps 18-22 — Virtualization, programmatic navigation, context menu, checkbox template, inline editing

## Blockers
- None
