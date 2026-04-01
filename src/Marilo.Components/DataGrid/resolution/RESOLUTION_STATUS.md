---
component: MariloDataGrid, MariloGridColumn, MariloGridToolbar
phase: 2
status: complete
complexity: multi-pass
priority: high
owner: "claude"
last-updated: 2026-04-01
depends-on: [MariloThemeProvider, MariloPagination, MariloForm]
external-resources:
  - name: "Blazor Virtualize"
    url: "https://learn.microsoft.com/aspnet/core/blazor/components/virtualization"
    license: "MIT (framework)"
    approved: true
---

# Resolution Status: MariloDataGrid, MariloGridColumn, MariloGridToolbar

## Current Phase
Phase 2: Complete. Remaining work tracked in `../GAP_ANALYSIS.md` (Phases A–D).

## Gap Summary
MariloDataGrid had 44 gaps. MariloGridColumn had 8 gaps. MariloGridToolbar had 2 gaps.

## Pass 1 Resolutions (2026-03-31)

### MariloDataGrid — Resolved Gaps

| # | Gap | Severity | Resolution |
|---|-----|----------|------------|
| 1 | `GridState` internal-only | **High** | Made `GridState` public with full doc comments. Added `GetState()` public method. |
| 2 | No `OnStateInit` / `OnStateChanged` events | **Medium** | Added `OnStateInit` (fires once on init) and `OnStateChanged` (fires on every state mutation with `GridStateChangedEventArgs`). |
| 3 | No `PageChanged` event | **High** | Added `PageChanged` EventCallback<int> with two-way bindable `Page` parameter. |
| 4 | No `PageSizeChanged` event | **High** | Added `PageSizeChanged` EventCallback<int> and `PageSizes` parameter for page-size dropdown UI. |
| 5 | `OnRead` declared but never invoked | **High** | Implemented full `OnRead` data flow. When `OnRead` is bound, grid delegates sort/filter/page to consumer via `GridReadEventArgs<TItem>`. |
| 6 | No `Height` parameter | **High** | Added `Height` parameter. Wraps table in scrollable `<div>` with `max-height` and `overflow:auto`. |
| 7 | No `Width` parameter | **Medium** | Added `Width` parameter applied to root element. |
| 8 | No public `OnRowClick` event | **High** | Added `OnRowClick` EventCallback with `GridRowClickEventArgs<TItem>` (Item, Field, EventArgs). |
| 9 | No `OnRowDoubleClick` event | **Medium** | Added `OnRowDoubleClick` EventCallback with same args type. |
| 10 | No `OnRowContextMenu` event | **Medium** | Added `OnRowContextMenu` EventCallback with same args type. |
| 11 | No `OnRowRender` callback | **Medium** | Added `OnRowRender` Action<GridRowRenderEventArgs<TItem>> for per-row CSS customization. |
| 12 | No checkbox selection column | **Medium** | Added `ShowCheckboxColumn` parameter with select-all header checkbox. |
| 13 | No `Rebind` method | **Medium** | Added public `Rebind()` method to force data refresh. |
| 14 | No `Navigable` parameter | **High** | Added `Navigable` parameter (placeholder for keyboard nav implementation). |
| 15 | No `Page` bindable parameter | **High** | Added bindable `Page` parameter (1-based). |
| 16 | `EndsWith` filter operator not supported | **Low** | Added `EndsWith` case to client-side filter logic. |

### MariloGridColumn — Resolved Gaps

| # | Gap | Severity | Resolution |
|---|-----|----------|------------|
| 1 | No `Visible` parameter | **Medium** | Added `Visible` parameter (default true). Grid filters by `_visibleColumns`. |
| 2 | No `EditorTemplate` | **High** | Added `EditorTemplate` RenderFragment<TItem> parameter. |
| 3 | No `FooterTemplate` | **Low** | Added `FooterTemplate` RenderFragment parameter. |
| 4 | No `OnCellRender` event | **Medium** | Added `OnCellRender` Action<GridCellRenderEventArgs<TItem>> for per-cell CSS customization. |

### MariloGridToolbar — Resolved Gaps

| # | Gap | Severity | Resolution |
|---|-----|----------|------------|
| 1 | No ARIA role | **Low** | Added `role="toolbar"` and `aria-label`. |

### New Files Created (Pass 1)

| File | Purpose |
|------|---------|
| `GridEventArgs.cs` | Event args: `GridRowClickEventArgs<T>`, `GridReadEventArgs<T>`, `GridRowRenderEventArgs<T>`, `GridCellRenderEventArgs<T>`, `GridStateChangedEventArgs` |

## Pass 2 Resolutions (2026-03-31)

### MariloDataGrid — Resolved Gaps

| # | Gap | Severity | Resolution |
|---|-----|----------|------------|
| 1 | No editing support (CRUD) | **High** | Implemented Inline, InCell, and Popup edit modes with full CRUD lifecycle. |
| 2 | No CUD events | **High** | Added OnAdd, OnCreate, OnUpdate, OnDelete, OnEdit, OnCancel, OnModelInit, OnCommand EventCallbacks. |
| 3 | No command column | **High** | Inline/Popup modes render Edit/Delete and Save/Cancel buttons in command cell. |
| 4 | No DetailTemplate / hierarchy | **Medium** | Added DetailTemplate RenderFragment<TItem>, expand/collapse button, OnRowExpand/OnRowCollapse events. |
| 5 | No FilterMenu mode | **Medium** | Added FilterMenu with operator dropdown (11 operators), value input, Apply/Clear actions. |
| 6 | Extended filter operators incomplete | **Low** | All 11 FilterOperator values now work: Contains, Equals, NotEquals, StartsWith, EndsWith, GT, GTE, LT, LTE, IsNull, IsNotNull with IComparable type-aware comparison. |
| 7 | Single-sort only | **Low** | Multi-sort via Ctrl+Click with ThenBy/ThenByDescending chaining and sort-order indicator. |
| 8 | No loading animation | **Low** | Added `IsLoading` parameter with overlay (`aria-busy`, loading spinner). |
| 9 | No virtual scrolling | **Medium** | Added `EnableVirtualization` and `VirtualizeOverscanCount` parameters using Blazor `<Virtualize>`. |
| 10 | No footer rendering | **Low** | Added `<tfoot>` section that renders `FooterTemplate` from each column. |
| 11 | InCell double-click editing | **Medium** | InCell mode: double-click cell to edit, inline ✓/✗ save/cancel buttons per cell. |

### New Files Created (Pass 2)

| File | Purpose |
|------|---------|
| `GridCommandTypes.cs` | `GridCommandDefinition`, `GridCommandPlacement` enum, `GridEditEventArgs<T>`, `GridModelInitEventArgs<T>`, `GridCommandEventArgs<T>` |
| `MariloGridCommandButton.razor` | Reusable command button component with CascadingParameter to parent grid |
| `MariloDataGrid.Editing.cs` | Partial class: BeginEdit, BeginCellEdit, BeginAdd, SaveEdit, CancelEdit, DeleteItem, ExecuteCommand, ToggleDetailRow |
| `MariloDataGrid.Data.cs` | Partial class: ProcessDataAsync, ApplyFilter (11 operators), ApplySort (multi), event handlers |
| `MariloDataGrid.Rendering.cs` | Partial class: RenderDataRow, RenderEditRow, RenderFilterMenu, row/cell render callbacks |

## Remaining Work

All remaining gaps are tracked in `../GAP_ANALYSIS.md` with phased task breakdowns:

| Phase | Description | Task Count |
|-------|-------------|------------|
| A | Pure C# features (grouping, auto-columns, search, templates, CSV export) | 42 |
| B | JS interop features (keyboard nav, column resize/reorder, row drag, frozen cols) | 28 |
| C | Advanced features (Excel/PDF export, column menu/chooser, cell selection, validation) | 29 |
| D | Future/out-of-scope (AI, popup templates, toolbar tools, adaptive mode) | 21 |
