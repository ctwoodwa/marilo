# DataGrid Spec Gap List

**Audit Date:** 2026-04-03
**Component:** MariloDataGrid<TItem>
**Spec Directory:** /workspaces/Marilo/docs/component-specs/grid/ (78 markdown files)
**Source:** /workspaces/Marilo/src/Marilo.Components/DataGrid/MariloDataGrid*.cs (5 partial files, 14 total)
**Existing gap tracker:** /workspaces/Marilo/src/Marilo.Components/DataGrid/GAP_ANALYSIS.md (134 remaining tasks)

---

## Summary

| Metric | Value |
|--------|-------|
| Implemented parameters | 49 |
| Implemented events | 18 |
| Spec feature areas | 24 (78 spec files) |
| Known gaps (prior 3 passes) | 44 resolved |
| Remaining tracked tasks | 134 (from GAP_ANALYSIS.md: Phase A: 49, B: 35, C: 29, D: 21) |
| Estimated spec coverage | ~55-60% |
| Tests | 4 bUnit tests (very low) |
| Demo pages | 4 (good coverage of implemented features) |

| Type | Count |
|------|-------|
| Blocking (naming/API shape mismatch) | 2 |
| Important (production readiness) | 16 |
| Nice-to-have (polish) | 9+ |
| **Total remaining gaps** | **~27+** (on top of 134 tracked tasks) |

---

## BLOCKING Gaps

### 1. Component Naming Mismatch
**Severity:** Blocking
Every spec example uses `<MariloGrid>` / `<GridColumn>` / `<GridColumns>`. The implementation uses `<MariloDataGrid>` / `<MariloGridColumn>` with no `<GridColumns>` wrapper. Consumers following spec examples will get compile errors.

**Decision needed:** Rename component to match spec, or update all spec examples.

### 2. Virtual Scrolling API Shape
**Severity:** Blocking
Spec requires `ScrollMode` enum (Scrollable/Virtual) + `RowHeight` decimal. Implementation uses `EnableVirtualization` bool + `VirtualizeOverscanCount` int. The spec approach is needed for proper virtual scrolling with skeletons and correct sizing.

---

## IMPORTANT Gaps (Production Readiness)

| # | Gap | Feature Area | Notes |
|---|-----|-------------|-------|
| 3 | Event args type mismatch | Editing | Spec: `GridCommandEventArgs` (untyped). Code: `GridEditEventArgs<TItem>` (typed). Compile errors for spec followers. |
| 4 | `SetStateAsync()` missing | State | Cannot programmatically set grid state. |
| 5 | No DataAnnotations validation | Editing | Production grids require validation integration. |
| 6 | No `SortMode` parameter | Sorting | Spec documents Single/Multiple; code always multi-sort. |
| 7 | Minimal pager | Paging | Spec: `GridPagerSettings` with page buttons, input, position. Code: prev/next only. |
| 8 | No drag-to-group UI | Grouping | Spec: drag panel. Code: API-only (`GroupBy()`/`Ungroup()`). |
| 9 | No composite filter descriptors | Filtering | Spec: AND/OR composition. Code: one filter per field. |
| 10 | No cell selection | Selection | Spec: `SelectedCells`/`SelectedCellsChanged`. Code: row-only. |
| 11 | No frozen/locked columns | Columns | Table-stakes feature. `Locked` parameter missing. |
| 12 | Format string mismatch | Columns | Spec: `DisplayFormat` with `{0:C2}`. Code: `Format` with `C2`. |
| 13 | `OnModelInit` signature mismatch | Editing | Spec: return-based. Code: event args pattern. |
| 14 | `GridState` non-generic | State | Spec: `GridState<TItem>`. Code: `GridState` (loses type info). |
| 15 | No Excel/PDF export | Export | Only CSV (string return, no JS download). |
| 16 | No `ConfirmDelete` | Editing | Production grids need delete confirmation. |
| 17 | No built-in toolbar tools | Toolbar | Spec: 13 tool components. Code: bare `ToolbarTemplate` container. |
| 18 | No row drag-and-drop | Rows | `OnRowDrop` not implemented. |

---

## NICE-TO-HAVE Gaps

| # | Gap | Feature Area |
|---|-----|-------------|
| 19 | `Size` parameter (Small/Medium/Large) | Sizing |
| 20 | `HighlightedItems` | Highlighting |
| 21 | `AdaptiveMode` | Responsive |
| 22 | Popup edit templates (`PopupFormTemplate`, `PopupButtonsTemplate`) | Templates |
| 23 | `PagerTemplate` | Templates |
| 24 | Column menu/chooser | Columns |
| 25 | Multi-column headers (`MariloGridColumnGroup`) | Columns |
| 26 | AI features (9 spec files) | Future |
| 27 | `CustomKeyboardShortcuts` | Accessibility |

---

## Implemented But Not Documented

| Feature | Notes |
|---------|-------|
| `ShowSearchBox` / `SearchBoxPlaceholder` | Spec references as toolbar tool only |
| `EnableVirtualization` / `VirtualizeOverscanCount` | Spec uses different API shape |
| `Striped` parameter | Not in spec overview |
| `ExportToCsv()` (returns string) | Spec expects JS-triggered download |
| `GroupBy()`/`Ungroup()`/`UngroupAll()` programmatic API | Not in spec (spec uses drag UI) |
| `BeginCellEdit(item, field)` | Not documented as public method |

---

## Mismatches Summary

| Area | Spec | Implementation |
|------|------|----------------|
| Component tag | `MariloGrid` | `MariloDataGrid` |
| Column tag | `GridColumn` | `MariloGridColumn` |
| Column wrapper | `<GridColumns>` | None (direct children) |
| Virtual scroll | `ScrollMode` enum + `RowHeight` | `EnableVirtualization` bool |
| Sort control | `SortMode` param | Always multi-sort |
| Format | `DisplayFormat` `{0:C2}` | `Format` `C2` |
| Edit events | `GridCommandEventArgs` | `GridEditEventArgs<TItem>` |
| Expand/Collapse | Typed event args | Direct `EventCallback<TItem>` |
| State | `GridState<TItem>` | `GridState` |
| Toolbar | `<GridToolBarTemplate>` child | `ToolbarTemplate` parameter |
| Pager | `GridPagerSettings` rich | Simple prev/next |

---

## Gap Workspace Integration

- Existing: `/workspaces/Marilo/src/Marilo.Components/DataGrid/GAP_ANALYSIS.md` tracks 134 tasks in 4 phases
- Target: `/workspaces/Marilo/workspaces/datagrid-gap-analysis/` (stub/pending state)
- The naming mismatch (gap #1) and API shape mismatches should be raised as high-priority gaps

---

## Next Recommended Actions

1. **Resolve naming:** Decide `MariloGrid` vs `MariloDataGrid` — affects all consumers and all spec examples
2. **Merge gap trackers:** Consolidate the 134-task GAP_ANALYSIS.md with these spec gap findings
3. **Per-area detailed audits:** Process 24 feature areas individually per delivery-context.md guidance
4. **Test coverage:** 4 tests is critically low for a 49-parameter component — needs major expansion
5. **Proceed to Stage 02** for areas that pass review (data binding, basic sorting/selection)
