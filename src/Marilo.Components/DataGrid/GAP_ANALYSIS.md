# MariloDataGrid Component vs API Gap Analysis

Generated: 2026-03-30

## Summary

The current implementation covers basic grid functionality (data binding, sorting, filtering, paging, selection, templates). However, numerous documented features and events are missing. The grid is at an early/MVP stage relative to the full documented API.

---

## 1. Missing Events (from `events.md`)

| Documented Event | Severity | Notes |
|---|---|---|
| `OnRowClick` (with `GridRowClickEventArgs`) | **[High]** | Implementation has internal `OnRowClick` for selection only; no public `EventCallback` exposing `Field`, `EventArgs` |
| `OnRowDoubleClick` | **[Medium]** | Not implemented |
| `OnRowContextMenu` | **[Medium]** | Not implemented |
| `OnRowRender` | **[Medium]** | No row-render callback for custom CSS classes per row |
| `OnRowExpand` / `OnRowCollapse` | **[Low]** | Requires hierarchy/DetailTemplate support first |
| `OnRowDrop` | **[Low]** | Requires drag-and-drop support first |
| `PageChanged` | **[High]** | No public event when the user changes pages |
| `PageSizeChanged` | **[High]** | No page-size change UI or event |
| `OnStateInit` / `OnStateChanged` | **[Medium]** | `GridState` is internal-only; no public state events |
| CUD events (`OnAdd`, `OnCreate`, `OnUpdate`, `OnDelete`, `OnEdit`, `OnCancel`) | **[High]** | No editing support at all |
| `OnModelInit` | **[Medium]** | No editing support |
| `OnBeforeExport` / `OnAfterExport` | **[Low]** | No export support |
| `SelectedCellsChanged` | **[Low]** | No cell selection support |
| Column `OnCellRender` | **[Medium]** | Not implemented |

## 2. Missing Grid Parameters (from `overview.md`)

| Documented Parameter | Severity | Notes |
|---|---|---|
| `Height` | **[High]** | No height parameter; grid cannot be scrollable |
| `Width` | **[Medium]** | No width parameter |
| `Navigable` | **[High]** | No keyboard navigation support |
| `CustomKeyboardShortcuts` | **[Low]** | Depends on `Navigable` |
| `AdaptiveMode` | **[Low]** | Not implemented |
| `Page` (bindable current page) | **[High]** | `_state.CurrentPage` is internal; not a `[Parameter]` |
| `EditMode` | **[High]** | No editing modes (Incell, Inline, Popup) |
| `AutoGenerateColumns` | **[Medium]** | Not implemented |
| `GroupBy` / Grouping parameters | **[Medium]** | Not implemented |
| `FilterMenu` mode | **[Medium]** | Only `FilterRow` mode exists; no `FilterMenu` |

## 3. Missing Major Features (from `overview.md`)

| Feature | Severity | Notes |
|---|---|---|
| **Editing (CRUD)** | **[High]** | No inline/incell/popup editing; no command columns |
| **Grouping** | **[Medium]** | Not implemented |
| **Virtual scrolling** | **[Medium]** | Not implemented; no row or column virtualization |
| **Column resizing** | **[Medium]** | Not implemented |
| **Column reordering** | **[Medium]** | Not implemented |
| **Frozen/locked columns** | **[Low]** | Not implemented |
| **Column menu** | **[Low]** | Not implemented |
| **Multi-column headers** | **[Low]** | Not implemented |
| **Hierarchy / DetailTemplate** | **[Medium]** | Not implemented |
| **Row drag and drop** | **[Low]** | Not implemented |
| **Export (Excel, etc.)** | **[Low]** | Not implemented |
| **Loading animation** | **[Low]** | Not implemented |
| **Highlighting** | **[Low]** | Not implemented |
| **Aggregates** | **[Low]** | Not implemented |
| **Selection: checkbox column** | **[Medium]** | Selection works via row click only; no checkbox UI |
| **Cell selection** | **[Low]** | Only row selection exists |
| **Rebind method** | **[Medium]** | No public method to force data refresh |
| **Column visibility toggle** | **[Low]** | No `Visible` parameter on columns |

## 4. MariloGridColumn Gaps

| Gap | Severity | Notes |
|---|---|---|
| No `Visible` parameter | **[Medium]** | Cannot hide columns programmatically |
| No `Locked`/`Frozen` parameter | **[Low]** | No frozen column support |
| No `Resizable` parameter | **[Low]** | No per-column resize control |
| No `Reorderable` parameter | **[Low]** | No per-column reorder control |
| No `EditorTemplate` | **[High]** | Required for editing |
| No `FooterTemplate` | **[Low]** | No footer/aggregate support |
| No `OnCellRender` event | **[Medium]** | Cannot customize individual cells |
| No `DisplayFormat` parameter | **[Low]** | `Format` exists but docs reference `DisplayFormat` naming |

## 5. MariloGridToolbar Gaps

| Gap | Severity | Notes |
|---|---|---|
| No built-in command buttons | **[Medium]** | Toolbar is a plain container; no `GridCommandButton` support |
| No integration with grid actions | **[Medium]** | Cannot trigger Add/Export from toolbar |

## 6. GridState Gaps

| Gap | Severity | Notes |
|---|---|---|
| Internal-only class | **[High]** | Not exposed as public API; users cannot get/set grid state |
| No grouping descriptors | **[Medium]** | `GroupDescriptors` missing from state |
| No selected items in state | **[Medium]** | Selection not tracked in state object |
| No expanded rows in state | **[Low]** | Hierarchy not supported |

## 7. Implementation Quality Notes

- `OnRead` parameter is declared but never invoked -- server-side data binding is non-functional.
- Sorting clears all existing sorts on new column click (single-sort only); docs show multi-sort as supported.
- Filter row uses plain text `<input>` with `Contains` operator only; no type-aware filters (date picker, numeric, etc.).
- Pager has no page-size selector dropdown.
- No `@ref` capture pattern documented or `@key` on rows for efficient diffing.

---

## Priority Recommendations

1. **Expose `GridState` publicly** and add `OnStateInit`/`OnStateChanged` events -- unblocks state persistence.
2. **Add `PageChanged` event and `Page` parameter** -- basic paging contract is incomplete without two-way binding.
3. **Implement `OnRead` data flow** -- declared but dead code; server-side scenarios are broken.
4. **Add `Height` parameter** -- without it, large datasets render without scroll.
5. **Add editing support** (start with Inline mode) -- largest feature gap by doc surface area.
