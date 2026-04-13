# Stage 03 -- Visual Parity Audit: MariloTreeList

**Date:** 2026-04-12
**Auditor:** w-treelist-delivery

---

## Summary

The MariloTreeList component emits BEM-style CSS classes in its razor template. **Neither the FluentUI provider nor the Bootstrap provider has any SCSS rules targeting these classes.** The component is completely unstyled at the provider level.

---

## A. BEM Classes Emitted by MariloTreeList.razor

| # | CSS Class | Element | Context |
|---|-----------|---------|---------|
| 1 | `mar-treelist` | Root `<div>` | Always rendered |
| 2 | `mar-treelist__table` | `<table>` | Always rendered |
| 3 | `mar-treelist__th` | `<th>` | Per column header |
| 4 | `mar-treelist__row` | `<tr>` | Per data row |
| 5 | `mar-treelist__td` | `<td>` | Per data cell |
| 6 | `mar-tree-item__toggle` | `<button>` | Expand/collapse toggle (note: uses tree-item BEM block, not treelist) |

## B. FluentUI Provider SCSS Coverage

| # | Class | Has SCSS Rule? | File |
|---|-------|----------------|------|
| 1 | `mar-treelist` | NO | -- |
| 2 | `mar-treelist__table` | NO | -- |
| 3 | `mar-treelist__th` | NO | -- |
| 4 | `mar-treelist__row` | NO | -- |
| 5 | `mar-treelist__td` | NO | -- |
| 6 | `mar-tree-item__toggle` | NO | -- |

**No SCSS file exists for TreeList in FluentUI provider.** No `_treelist.scss` or `_tree-list.scss` file found. The `mar-treelist` prefix does not appear in any FluentUI SCSS file. The `mar-tree-item__toggle` class used for the expand button may or may not be styled by `_tree-view.scss` (different component).

## C. Bootstrap Provider SCSS Coverage

| # | Class | Has SCSS Rule? | File |
|---|-------|----------------|------|
| 1 | `mar-treelist` | NO | -- |
| 2 | `mar-treelist__table` | NO | -- |
| 3 | `mar-treelist__th` | NO | -- |
| 4 | `mar-treelist__row` | NO | -- |
| 5 | `mar-treelist__td` | NO | -- |
| 6 | `mar-tree-item__toggle` | NO | -- |

**No SCSS file exists for TreeList in Bootstrap provider either.**

## D. BEM Naming Concern

The expand/collapse toggle button uses class `mar-tree-item__toggle`, which belongs to the TreeView BEM block (`mar-tree-item`), not the TreeList block (`mar-treelist`). This is a naming inconsistency that should be resolved. The toggle should either:
- Use `mar-treelist__toggle` (preferred -- keeps styling scoped to TreeList)
- Or explicitly document the cross-component class dependency

## E. Inline Styles in Source

The component uses inline styles for indentation and layout:
- `padding-left: {depth * 20}px; display: inline-flex; align-items: center; gap: 4px;` on the first-column span
- `width: 20px;` on the spacer span for leaf nodes
- Column width via `style="width:{col.Width};"` on `<th>`

These inline styles should migrate to SCSS when provider stylesheets are created, using CSS custom properties or BEM modifiers for proper theming.

---

## F. Parity Score

| Provider | Classes Covered | Total Classes | Score |
|----------|----------------|---------------|-------|
| FluentUI | 0 | 6 | **0%** |
| Bootstrap | 0 | 6 | **0%** |

**Overall visual parity: 0% -- BLOCKED**

---

## Conclusion

The TreeList has zero provider-level styling. All visual presentation relies on browser defaults and inline styles. A dedicated `_treelist.scss` file is needed in both FluentUI and Bootstrap providers covering at minimum the 6 BEM classes listed above, plus tokens for spacing, borders, hover states, selection highlights, and theming.
