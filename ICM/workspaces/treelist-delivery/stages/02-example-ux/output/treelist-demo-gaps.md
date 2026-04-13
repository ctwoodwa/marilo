# Stage 02 -- Example UX Audit: MariloTreeList

**Date:** 2026-04-12
**Auditor:** w-treelist-delivery
**Demo page:** `samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor`

---

## Summary

The demo page is a **placeholder stub**. It shows an `<MariloAlert>` info box saying "TreeList requires typed data and column definitions" and renders a static code string. It does not instantiate the actual `<MariloTreeList>` component or demonstrate any features.

**Demo scenarios present:** 0 (functional)
**Demo scenarios expected (based on spec):** 15+ minimum

---

## A. Current Demo Assessment

| Aspect | Status | Notes |
|--------|--------|-------|
| Component rendered | NO | Alert box only, no actual TreeList |
| Live data binding | NO | No data source |
| Interactive controls | NO | No parameter toggles |
| Code-behind | MINIMAL | Single const string with markup snippet |
| Routes | OK | `/components/treelist` and `/components/treelist/overview` |

## B. Missing Demo Scenarios

### Priority 1 -- Core Features (must-have for any demo page)

| # | Scenario | Notes |
|---|----------|-------|
| 1 | Basic flat data binding | Show TreeList with IdField/ParentIdField |
| 2 | Hierarchical data binding | Show TreeList with ItemsField |
| 3 | Expand/collapse interaction | Currently the only implemented feature -- not demonstrated |
| 4 | Column definitions | Show multiple columns with different widths and titles |

### Priority 2 -- Data Operations (blocked on implementation)

| # | Scenario | Blocked By |
|---|----------|------------|
| 5 | Paging | `Pageable` not implemented |
| 6 | Sorting | `Sortable` not implemented |
| 7 | Filtering (filter row) | `FilterMode` not implemented |
| 8 | Filtering (filter menu) | `FilterMode` not implemented |

### Priority 3 -- Editing (blocked on implementation)

| # | Scenario | Blocked By |
|---|----------|------------|
| 9 | Inline editing | `EditMode` not implemented |
| 10 | Popup editing | `EditMode` not implemented |
| 11 | InCell editing | `EditMode` not implemented |

### Priority 4 -- Selection and Interaction (blocked on implementation)

| # | Scenario | Blocked By |
|---|----------|------------|
| 12 | Row selection (single/multiple) | `SelectionMode` not implemented |
| 13 | Cell selection | `SelectedCells` not implemented |
| 14 | Row drag and drop | `RowDraggable` not implemented |
| 15 | Toolbar with search | `<TreeListToolBar>` not implemented |

### Priority 5 -- Advanced (blocked on implementation)

| # | Scenario | Blocked By |
|---|----------|------------|
| 16 | Virtual scrolling | `ScrollMode` not implemented |
| 17 | Column reorder/resize | Column features not implemented |
| 18 | Frozen columns | Column `Locked` not implemented |
| 19 | Load on demand | Load-on-demand not implemented |
| 20 | State persistence | State management not implemented |

## C. Missing Interactive Controls

The demo page should (once features are implemented) provide toggles for:
- `Pageable` (bool toggle)
- `Sortable` (bool toggle)
- `FilterMode` (dropdown: None, FilterRow, FilterMenu)
- `SelectionMode` (dropdown: None, Single, Multiple)
- `PageSize` (numeric input)

## D. Actionable Items (not blocked)

Even with the current scaffold implementation, the demo could be improved:

| # | Action | Priority |
|---|--------|----------|
| 1 | Render an actual `<MariloTreeList>` with flat sample data | HIGH |
| 2 | Show expand/collapse working with hierarchical sample data | HIGH |
| 3 | Add code-behind with realistic Employee-like model | MEDIUM |
| 4 | Display columns with different widths | LOW |

---

## Conclusion

The demo page is non-functional. It neither renders the TreeList component nor demonstrates any feature. Even the currently implemented features (flat binding, hierarchical binding, expand/collapse) are not shown. Minimum viable demo requires items D.1 and D.2 above.
