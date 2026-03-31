---
component: MariloDataGrid, MariloGridColumn, MariloGridToolbar
phase: 2
status: in-progress
complexity: multi-pass
priority: high
owner: "claude"
last-updated: 2026-03-31
depends-on: [MariloThemeProvider, MariloPagination, MariloForm]
external-resources:
  - name: "Blazor Virtualize"
    url: "https://learn.microsoft.com/aspnet/core/blazor/components/virtualization"
    license: "MIT (framework)"
    approved: true
---

# Resolution Status: MariloDataGrid, MariloGridColumn, MariloGridToolbar

## Current Phase
Phase 2: Data grid infrastructure — Pass 1 complete

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
| 2 | No `EditorTemplate` | **High** | Added `EditorTemplate` RenderFragment<TItem> parameter (ready for editing pass). |
| 3 | No `FooterTemplate` | **Low** | Added `FooterTemplate` RenderFragment parameter. |
| 4 | No `OnCellRender` event | **Medium** | Added `OnCellRender` Action<GridCellRenderEventArgs<TItem>> for per-cell CSS customization. |

### MariloGridToolbar — Resolved Gaps

| # | Gap | Severity | Resolution |
|---|-----|----------|------------|
| 1 | No ARIA role | **Low** | Added `role="toolbar"` and `aria-label`. |

### New Files Created

| File | Purpose |
|------|---------|
| `GridEventArgs.cs` | Event args: `GridRowClickEventArgs<T>`, `GridReadEventArgs<T>`, `GridRowRenderEventArgs<T>`, `GridCellRenderEventArgs<T>`, `GridStateChangedEventArgs` |

## Remaining Gaps (Deferred to Pass 2+)

| Gap | Severity | Notes |
|-----|----------|-------|
| Editing (CRUD) — InCell, Inline, Popup modes | **High** | Requires command column, edit templates, CUD events |
| Grouping | **Medium** | GroupDescriptors added to GridState; rendering not yet implemented |
| Virtual scrolling | **Medium** | Will use Blazor `<Virtualize>` |
| Column resizing | **Medium** | Requires JS interop |
| Column reordering | **Medium** | Requires JS interop |
| Frozen/locked columns | **Low** | Sticky positioning |
| Column menu | **Low** | Popup per column header |
| Multi-column headers | **Low** | Nested column groups |
| Hierarchy / DetailTemplate | **Medium** | Row expansion |
| Row drag and drop | **Low** | Requires JS interop |
| Export (Excel) | **Low** | Requires library decision |
| Loading animation | **Low** | Skeleton/spinner overlay |
| Aggregates / FooterTemplate rendering | **Low** | Template added, rendering not wired |
| Multi-sort (Ctrl+click) | **Low** | Currently single-sort only |
| FilterMenu mode | **Medium** | Only FilterRow implemented |
| GridCommandButton for toolbar | **Medium** | Toolbar cascades grid, but no command button component yet |

## Blockers
- Editing support requires decision on command column architecture
- Virtual scrolling requires JS interop infrastructure
- Column resize/reorder requires shared JS interop module (shared with MariloWindow/MariloSplitter)
