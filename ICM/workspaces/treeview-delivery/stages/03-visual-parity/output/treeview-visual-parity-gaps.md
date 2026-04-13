# TreeView Visual Parity Gaps

**Audit Date:** 2026-04-11 (Tick 13 -- Stage 03 visual-parity)
**Worker:** `w-treeview-delivery`
**SCSS Source (FluentUI):** `src/Marilo.Providers.FluentUI/Styles/components/_tree-view.scss` (31 lines)
**SCSS Source (Bootstrap):** `src/Marilo.Providers.Bootstrap/Styles/components/_tree-view.scss` (22 lines)
**CSS Provider Contract:** `IMariloCssProvider.TreeViewClass()`, `IMariloCssProvider.TreeItemClass(bool, bool)`
**Component Markup:** `MariloTreeView.razor`, `MariloTreeItem.razor`
**Spec References:** `docs/component-specs/treeview/` (overview, fluent-ui-gap-analysis, accessibility, icons, selection, checkboxes, navigation)

---

## Audit Methodology

1. Read all SCSS partials for FluentUI and Bootstrap treeview providers.
2. Read the component `.razor` templates to identify all CSS classes emitted in the DOM.
3. Cross-referenced emitted classes against SCSS rules to find unstyled selectors.
4. Compared SCSS token usage against spec visual descriptions (overview.md `Size` param, fluent-ui-gap-analysis.md appearance variants, wai-aria-support.md keyboard/focus requirements, checkboxes overview screenshots).
5. Checked for dark-mode overrides, density/size variants, focus-visible states, disabled/readonly visual treatment, and drag-drop visual indicators.
6. Scored each gap per `shared/parity-score-rubric.md`.

---

## Summary

| Severity | Count |
|----------|-------|
| Critical | 3 |
| Major | 6 |
| Minor | 5 |
| Polish | 3 |
| **Total** | **17** |

---

## Structural Finding: Dual Class Systems

The SCSS file contains **two parallel class hierarchies** that do not interconnect:

1. **Phase 1 classes** (lines 5-22): `.mar-treeview`, `.mar-tree-item`, `.mar-tree-item--selected`, `.mar-tree-item--expanded`
2. **Phase 2 classes** (lines 25-30): `.mar-treeitem`, `.mar-treeitem__row`, `.mar-treeitem--selected`, `.mar-treeitem__toggle`, `.mar-treeitem--expanded`

The component markup (`MariloTreeItem.razor`) uses **hardcoded class names** (`mar-tree-item__header`, `mar-tree-item__toggle`, `mar-tree-item__icon`, `mar-tree-item__title`, `mar-tree-item__children`, `mar-tree-item__checkbox`) that **do not match either SCSS hierarchy**. The Phase 2 SCSS uses `mar-treeitem__*` (no dash between "tree" and "item") while the Razor uses `mar-tree-item__*` (with dash).

The `CssProvider.TreeItemClass()` returns Phase 1 classes (`mar-tree-item`, `mar-tree-item--expanded`, `mar-tree-item--selected`) which are applied to the `<li>` element, but the inner DOM elements use hardcoded class names from neither Phase 1 nor Phase 2.

This naming collision is the root cause of many gaps below.

---

## Gap Records

### VP-treeview-001: No focus-visible ring on tree container or items

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** Keyboard-focused tree item
**Reference Source:** FluentUI design system (all interactive elements use `--marilo-focus-ring`)
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | No `:focus-visible` rule exists anywhere in `_tree-view.scss`. The `<ul>` root has `tabindex="0"` and receives keyboard focus but has no visual indicator. Individual items have no focus ring. | FluentUI standard: `box-shadow: var(--marilo-focus-ring)` on `:focus-visible`, matching `.mar-button:focus-visible`, `.mar-link:focus-visible`, `.mar-toolbar-btn:focus-visible` patterns used elsewhere in the design system. The focused item should show a 2px primary-color ring per `_generated-base.scss` line 90. |
| Likely cause | Missing SCSS rules. Neither the component nor Phase 2 SCSS implements `:focus-visible`. | |

**Category:** state treatment
**Recommended change:** Add `.mar-treeview:focus-visible { outline: none; box-shadow: var(--marilo-focus-ring); border-radius: var(--marilo-radius-md); }` and `.mar-tree-item--focused { outline: none; box-shadow: var(--marilo-focus-ring); border-radius: var(--marilo-radius-md); }` (the `--focused` modifier is already emitted by `MariloTreeItem.razor.cs` line 41).
**Acceptance criteria:** When navigating by keyboard, the focused tree item shows a visible 2px primary-color outline ring.
**Remediation handoff target:** SCSS source fix

---

### VP-treeview-002: Hardcoded Razor classes have no matching SCSS rules

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** All states -- default rendering
**Reference Source:** Internal Marilo baseline
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `MariloTreeItem.razor` emits `mar-tree-item__header`, `mar-tree-item__toggle`, `mar-tree-item__icon`, `mar-tree-item__title`, `mar-tree-item__children`, `mar-tree-item__checkbox`, `mar-tree-item__checkbox--indeterminate`. **Zero SCSS rules exist for any of these selectors.** The SCSS has `mar-treeitem__row` and `mar-treeitem__toggle` (Phase 2) which differ by the dash. | Every class emitted in the DOM should have a corresponding SCSS rule in the provider stylesheet. |
| Likely cause | Class naming mismatch between Razor markup (uses `mar-tree-item__*` BEM pattern) and SCSS (uses `mar-treeitem__*` without the dash, or Phase 1 flat classes). The Razor markup was likely written against a planned BEM convention that the SCSS was never updated to match. | |

**Category:** layout
**Recommended change:** Either (a) rename Razor classes to match SCSS `mar-treeitem__*` convention, or (b) rename SCSS selectors to `mar-tree-item__*` to match Razor. Option (b) is recommended since the Razor classes follow proper BEM naming and the `CssProvider` already returns `mar-tree-item` (with dash). Also add missing rules for `__header`, `__icon`, `__title`, `__children`, `__checkbox`, `__checkbox--indeterminate`.
**Acceptance criteria:** Every CSS class in the DOM has a matching SCSS rule. No unstyled structural selectors.
**Remediation handoff target:** SCSS source fix + possible Razor class rename (architecture decision -- escalate to orchestrator)

---

### VP-treeview-003: No disabled visual treatment

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** `Disabled=true` tree
**Reference Source:** FluentUI design system (`--marilo-color-text-disabled`)
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | When `Disabled=true`, the component sets `aria-disabled="true"` on the root `<ul>` and disables the toggle buttons and checkboxes via HTML `disabled` attribute. However, **no SCSS rule adjusts the visual appearance** -- text color, opacity, and cursor remain identical to the enabled state. The tree looks fully interactive even when disabled. | Disabled state should apply `color: var(--marilo-color-text-disabled)`, `cursor: default`, and `pointer-events: none` on `.mar-treeview[aria-disabled="true"]` or a `--disabled` modifier class. Other Marilo components (buttons, inputs) follow this pattern. |
| Likely cause | Missing SCSS rules for disabled state. | |

**Category:** state treatment
**Recommended change:** Add `.mar-treeview[aria-disabled="true"] { color: var(--marilo-color-text-disabled); cursor: default; pointer-events: none; }` and `.mar-treeview[aria-disabled="true"] .mar-tree-item { cursor: default; }`.
**Acceptance criteria:** Disabled tree renders with muted text color and non-interactive cursor. Toggle chevrons and checkboxes appear visually disabled.
**Remediation handoff target:** SCSS source fix

---

### VP-treeview-004: No Size/density variant SCSS

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** `Size="sm"` / `Size="lg"`
**Reference Source:** Spec `overview.md` line 136 (Size parameter: sm/md/lg)
**Parity Score:** 1
**Severity:** Major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | The spec documents `Size` parameter accepting `"sm"`, `"md"`, `"lg"` values that affect "the amount of space between items." Source implements `Size` as an inline `font-size` style (`MariloTreeView.razor.cs` line 148). No CSS class modifiers exist (`--sm`, `--md`, `--lg`). The implementation only changes font size, not padding/gap/indent. | Size should affect item padding, child indent depth, toggle button dimensions, and overall density. FluentUI button/icon-button pattern: `--small`, `--medium`, `--large` modifier classes with distinct padding/sizing values. |
| Likely cause | Incomplete implementation -- Size treated as simple font-size override rather than full density variant. | |

**Category:** density
**Recommended change:** Add `.mar-treeview--sm`, `.mar-treeview--md`, `.mar-treeview--lg` classes with varying `--marilo-space-*` values for item padding, child indent, and toggle size. Update `CssProvider.TreeViewClass()` to accept a size parameter, or emit a modifier class from the component. Remove the inline `font-size` style in favor of CSS-driven sizing.
**Acceptance criteria:** Size sm/md/lg produce visually distinct density levels with appropriate padding and indent scaling.
**Remediation handoff target:** SCSS source fix + CssProvider contract change (escalate -- provider contract modification)

---

### VP-treeview-005: No Appearance variant SCSS

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** Default appearance
**Reference Source:** `fluent-ui-gap-analysis.md` section 1 (Appearance parameter: Subtle, Transparent, SubtleAlpha)
**Parity Score:** 1
**Severity:** Major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `fluent-ui-gap-analysis.md` proposes `TreeViewAppearance` enum (Subtle, Transparent, SubtleAlpha) with CSS classes `marilo-treeview--subtle`, etc. No such parameter exists in source. No appearance-variant SCSS exists. | Per gap analysis P0 priority: "Renders as a CSS class on the root element with corresponding SCSS styles." |
| Likely cause | Feature not yet implemented. Gap analysis is a design proposal, not current state. | |

**Category:** state treatment
**Recommended change:** This is a new feature tracked in `fluent-ui-gap-analysis.md` as P0. Record as visual-parity gap for tracking. Implementation requires new parameter + SCSS + CssProvider change.
**Acceptance criteria:** `Appearance="Transparent"` removes hover/selected backgrounds. `Appearance="SubtleAlpha"` uses alpha-blended backgrounds.
**Remediation handoff target:** gap-analysis-resolution intake (new feature)

---

### VP-treeview-006: Hover background token inconsistency between Phase 1 and Phase 2

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Hovered tree item
**Reference Source:** FluentUI design tokens
**Parity Score:** 1
**Severity:** Major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Phase 1 hover: `.mar-tree-item:hover { background: var(--marilo-color-surface-hover); }` uses `--marilo-color-surface-hover` (#edebe9). Phase 2 hover: `.mar-treeitem__row:hover { background: var(--marilo-color-surface); }` uses `--marilo-color-surface` (#f3f2f1). These are different tokens producing different colors. The `CssProvider` returns Phase 1 classes, so Phase 1 hover is what takes effect, but Phase 2 is dead code with a wrong token. | Single consistent hover treatment using `--marilo-color-surface-hover` per FluentUI token convention. |
| Likely cause | Phase 2 SCSS was written using the wrong token (`surface` vs `surface-hover`). Phase 2 is also dead code since no selector in the Razor matches it. | |

**Category:** token/color
**Recommended change:** When unifying the class system (see VP-002), use `--marilo-color-surface-hover` consistently for hover state. Remove or unify Phase 2 dead code.
**Acceptance criteria:** Hover produces a single, consistent background color derived from `--marilo-color-surface-hover`.
**Remediation handoff target:** SCSS source fix (part of VP-002 unification)

---

### VP-treeview-007: No ReadOnly visual differentiation

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** `ReadOnly=true` tree
**Reference Source:** FluentUI read-only input patterns
**Parity Score:** 1
**Severity:** Major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | When `ReadOnly=true`, toggle buttons and checkboxes are HTML-disabled, but no visual cue distinguishes a read-only tree from an editable one. The cursor remains `pointer` on items. No SCSS class or modifier exists for read-only state. | Read-only should reduce interactivity cues: `cursor: default` on items, slightly muted toggle icons, no hover background change. Distinct from disabled (which is fully grayed out). |
| Likely cause | Missing SCSS rules and missing modifier class in `CssProvider`/component. | |

**Category:** state treatment
**Recommended change:** Add a `--readonly` modifier class to the tree root. SCSS: `.mar-treeview--readonly .mar-tree-item { cursor: default; }`, `.mar-treeview--readonly .mar-tree-item:hover { background: transparent; }`.
**Acceptance criteria:** Read-only tree items show no hover effect and use a default cursor.
**Remediation handoff target:** SCSS source fix + component modifier

---

### VP-treeview-008: Checkbox has no styled appearance

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** Checkbox checked, unchecked, indeterminate
**Reference Source:** Spec screenshots (`checkboxes/images/checkboxes-overview-checkeditems-example.png`, `checkchildren-example.gif`, `checkparents-example.png`)
**Parity Score:** 1
**Severity:** Major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `MariloTreeItem.razor` renders a native `<input type="checkbox">` with class `mar-tree-item__checkbox`. No SCSS rule styles this class. The checkbox renders as a browser-default native checkbox with no FluentUI theming. The `--indeterminate` modifier class also has no SCSS rule. | FluentUI-themed checkbox with brand-color checked state, proper indeterminate visual (dash icon), consistent sizing. The spec checkbox screenshots show styled checkboxes with FluentUI appearance. The WAI-ARIA spec notes `role="none/presentation"` on checkbox wrapper to prevent duplicate announcements -- this wrapper does not exist in current markup. |
| Likely cause | Missing SCSS rules for `.mar-tree-item__checkbox` and `.mar-tree-item__checkbox--indeterminate`. | |

**Category:** iconography / state treatment
**Recommended change:** Add SCSS rules for `.mar-tree-item__checkbox` with FluentUI-styled appearance (accent color, border radius, sizing). Add `.mar-tree-item__checkbox--indeterminate` visual (dash/minus icon). Consider using the `CheckboxTemplate` parameter to inject a Marilo-themed checkbox, or style the native input with CSS.
**Acceptance criteria:** Checkboxes match FluentUI design language. Indeterminate state shows a horizontal dash. Checked state uses brand color.
**Remediation handoff target:** SCSS source fix

---

### VP-treeview-009: No dark-mode-specific SCSS overrides for tree component

**Component:** TreeView
**Theme:** Fluent
**Mode:** Dark
**State/Scenario:** All dark-mode states
**Reference Source:** FluentUI dark theme design tokens
**Parity Score:** 2
**Severity:** Major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | The tree SCSS uses CSS custom properties (`--marilo-color-surface-hover`, `--marilo-color-primary-light`, `--marilo-color-primary`) which **do** get dark-mode values via `[data-marilo-theme="dark"]` in `_generated-base.scss`. So basic token-level dark mode works. However, there are no tree-specific dark-mode overrides for: (a) the toggle chevron characters (Unicode `\u25BC`/`\u25B6` rendered as text -- may need color override), (b) checkbox border visibility against dark backgrounds, (c) the selected item combination of `--marilo-color-primary-light` (#0a2e4a in dark) + `--marilo-color-primary` (#60cdff in dark) needs contrast verification. | Dark-mode should be verified end-to-end. While token inheritance handles basics, component-specific adjustments are often needed for contrast and readability. |
| Likely cause | Token inheritance covers the basics but no dark-mode-specific QA or override rules exist for the tree component. | |

**Category:** token/color
**Recommended change:** Add `[data-marilo-theme="dark"] .mar-tree-item__toggle { color: var(--marilo-color-on-surface); }` and verify contrast ratios for selected state in dark mode. Add visual QA step for dark mode in delivery gate.
**Acceptance criteria:** Selected items, hover states, toggle icons, and checkboxes are all legible and meet WCAG 2.2 AA contrast (4.5:1 for text) in dark mode.
**Remediation handoff target:** SCSS source fix + visual QA

---

### VP-treeview-010: Icon alignment has no SCSS

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** Tree item with icon
**Reference Source:** Spec `icons.md`, spec screenshot `images/icons.png`
**Parity Score:** 2
**Severity:** Minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `MariloTreeItem.razor` line 34 wraps icons in `<span class="mar-tree-item__icon">`. No SCSS rule exists for this class. Icon alignment, sizing, and spacing relative to the title text are unstyled. The icon relies on whatever default the `IconProvider.GetIcon()` returns. | Icons should be vertically centered with the title text, sized consistently (16px for small as per `icons.md` example), with a defined gap between icon and text. Phase 2 SCSS has `gap: var(--marilo-space-xs)` on `mar-treeitem__row` but this does not apply to the actual markup. |
| Likely cause | Missing SCSS rule for `mar-tree-item__icon`. | |

**Category:** iconography / spacing
**Recommended change:** Add `.mar-tree-item__icon { display: inline-flex; align-items: center; width: 16px; height: 16px; flex-shrink: 0; }`.
**Acceptance criteria:** Icons are vertically centered with title text and consistently sized.
**Remediation handoff target:** SCSS source fix

---

### VP-treeview-011: Title text has no typography SCSS

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** Default tree item text
**Reference Source:** FluentUI typography scale
**Parity Score:** 2
**Severity:** Minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `<span class="mar-tree-item__title">` has no SCSS rule. Text inherits whatever parent font settings exist. No explicit font-size, font-weight, line-height, or text-overflow handling. | Should use `font-size: var(--marilo-font-size-base)` (0.875rem / 14px), `font-weight: var(--marilo-font-weight-regular)`, `line-height: var(--marilo-line-height-base)`. Should handle overflow with `text-overflow: ellipsis` and `white-space: nowrap` when horizontal space is constrained. |
| Likely cause | Missing SCSS rule for `mar-tree-item__title`. | |

**Category:** typography
**Recommended change:** Add `.mar-tree-item__title { font-size: var(--marilo-font-size-base); line-height: var(--marilo-line-height-base); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }`. For link variant (`a.mar-tree-item__title`), add `color: var(--marilo-color-primary); text-decoration: none; &:hover { text-decoration: underline; }`.
**Acceptance criteria:** Tree item text uses FluentUI base typography. Long text truncates with ellipsis.
**Remediation handoff target:** SCSS source fix

---

### VP-treeview-012: Header row container has no SCSS

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** Default tree item layout
**Reference Source:** FluentUI Tree item layout
**Parity Score:** 2
**Severity:** Minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `<div class="mar-tree-item__header">` wraps toggle + checkbox + icon + title. No SCSS rule exists for this class. The header's flex layout, alignment, and gap are undefined -- children stack based on default flow. | Should use `display: flex; align-items: center; gap: var(--marilo-space-xs);` similar to the Phase 2 `.mar-treeitem__row` pattern. |
| Likely cause | Missing SCSS rule for `mar-tree-item__header`. Phase 2 has `.mar-treeitem__row` with the right layout but the class name does not match. | |

**Category:** layout
**Recommended change:** Add `.mar-tree-item__header { display: flex; align-items: center; gap: var(--marilo-space-xs); padding: var(--marilo-space-xxs) var(--marilo-space-sm); border-radius: var(--marilo-radius-md); }`.
**Acceptance criteria:** Toggle, checkbox, icon, and title are horizontally aligned with consistent spacing.
**Remediation handoff target:** SCSS source fix

---

### VP-treeview-013: Children container has no SCSS

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** Expanded node with children
**Reference Source:** FluentUI tree indent convention
**Parity Score:** 2
**Severity:** Minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `<ul role="group" class="mar-tree-item__children">` has no SCSS rule. Indent is only applied by `.mar-treeview ul { padding-left: var(--marilo-space-lg) }` (Phase 1), which targets all `ul` descendants of the root. The `__children` class itself is unstyled. | Should have explicit indent via `padding-left` or `margin-left`, and `list-style: none`. |
| Likely cause | Phase 1 generic `ul` rule provides indent, but the BEM-named class is not directly styled. | |

**Category:** spacing
**Recommended change:** Add `.mar-tree-item__children { list-style: none; padding-left: var(--marilo-space-lg); margin: 0; }`.
**Acceptance criteria:** Child nodes are indented consistently at each level.
**Remediation handoff target:** SCSS source fix

---

### VP-treeview-014: Toggle button not styled in the rendered class

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** Expand/collapse toggle chevron
**Reference Source:** FluentUI expand chevron pattern
**Parity Score:** 2
**Severity:** Minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | The Razor emits `<button class="mar-tree-item__toggle">`. The SCSS has `.mar-treeitem__toggle` (no dash). The Phase 2 rule with rotation transform on expanded state does not apply because the class names don't match. The toggle button renders as a bare `<button>` with Unicode chevron characters and no styling beyond browser defaults. | Toggle should be a compact, invisible-border button with smooth rotation animation on expand. Phase 2 SCSS has the right styles (`width: 20px; height: 20px; background: none; border: none; cursor: pointer; transition: transform`) but they target the wrong class name. |
| Likely cause | Class name mismatch (same root cause as VP-002). | |

**Category:** layout / state treatment
**Recommended change:** Align class names (see VP-002). Once aligned, the Phase 2 toggle styles will apply correctly. Additionally, add `color: var(--marilo-color-on-surface)` for the chevron text.
**Acceptance criteria:** Toggle chevron is properly sized, borderless, and rotates smoothly on expand/collapse.
**Remediation handoff target:** SCSS source fix (part of VP-002 unification)

---

### VP-treeview-015: No drag-drop visual indicators in SCSS

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** Dragging a tree item, hovering over a drop target
**Reference Source:** FluentUI drag interaction patterns
**Parity Score:** 1
**Severity:** Polish

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | The component supports `EnableDragDrop` and fires `OnItemDrop`, but no SCSS rules exist for drag visual states: no `--dragging` modifier (opacity reduction, shadow), no `--drop-target` modifier (highlight/border on the target item), no drop-position indicator (above/below/inside). | Drag source should have reduced opacity or elevation. Drop target should highlight with a border or background change. Drop position indicator should show a line or zone highlight. |
| Likely cause | Drag-drop is behavioral only -- visual treatment was not implemented in SCSS. | |

**Category:** state treatment
**Recommended change:** Add `.mar-tree-item--dragging { opacity: 0.5; }`, `.mar-tree-item--drop-target { background: var(--marilo-color-primary-light); border: 1px dashed var(--marilo-color-primary); }`, `.mar-tree-item--drop-before::before` / `--drop-after::after` position indicators.
**Acceptance criteria:** Drag-and-drop operations have clear visual feedback for source, target, and position.
**Remediation handoff target:** SCSS source fix + component modifier classes

---

### VP-treeview-016: No editing-mode visual treatment

**Component:** TreeView
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** Inline editing active (`AllowEditing=true`, item in edit mode)
**Reference Source:** FluentUI inline-edit input patterns
**Parity Score:** 1
**Severity:** Polish

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | The spec notes `AllowEditing=true` enables inline text editing. No SCSS rules exist for an editing state -- no `--editing` modifier, no input styling for the edit field. | When a node enters edit mode, the title should transform into a styled text input with FluentUI input appearance (border, focus ring, padding). |
| Likely cause | Edit-mode visual treatment not implemented in SCSS. | |

**Category:** state treatment
**Recommended change:** Add `.mar-tree-item--editing .mar-tree-item__title { /* hide */ }` and `.mar-tree-item__edit-input { border: 1px solid var(--marilo-color-border); border-radius: var(--marilo-radius-sm); padding: var(--marilo-space-xxs) var(--marilo-space-xs); font-size: var(--marilo-font-size-base); &:focus { box-shadow: var(--marilo-focus-ring); } }`.
**Acceptance criteria:** Inline edit mode shows a properly styled text input that matches FluentUI input appearance.
**Remediation handoff target:** SCSS source fix + component markup for edit input

---

### VP-treeview-017: Bootstrap provider is minimal/incomplete

**Component:** TreeView
**Theme:** Bootstrap
**Mode:** Light
**State/Scenario:** All states
**Reference Source:** Bootstrap 5 list-group component
**Parity Score:** 1
**Severity:** Polish

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Bootstrap SCSS (22 lines) only styles: expanded child visibility, child indent, and basic list-group-item cursor. Missing: focus states, disabled/readonly treatment, checkbox theming, icon alignment, drag-drop indicators, size variants, selection highlight (relies solely on Bootstrap's `.active` class). The `BootstrapCssProvider` returns `list-group mar-bs-treeview` for the root and `list-group-item list-group-item-action` + `active`/`mar-bs-tree-item--expanded` for items. | Bootstrap provider should cover the same state matrix as FluentUI: focus, disabled, readonly, checkbox, size, drag-drop, editing. Using Bootstrap utility classes where possible. |
| Likely cause | Bootstrap provider was scaffolded as a minimal bridge and not extended to full state coverage. | |

**Category:** state treatment (multi-state)
**Recommended change:** Extend Bootstrap SCSS to cover disabled, readonly, focus, checkbox, size, drag-drop states using Bootstrap utility classes and `mar-bs-*` overrides. Lower priority than FluentUI gaps since FluentUI is the primary provider.
**Acceptance criteria:** Bootstrap-themed tree has visual parity with FluentUI-themed tree across all primary and secondary states.
**Remediation handoff target:** SCSS source fix (Bootstrap provider)

---

## Gap Inventory by Category

| Category | Gap IDs | Count |
|----------|---------|-------|
| State treatment | VP-001, VP-003, VP-005, VP-006, VP-007, VP-008, VP-014, VP-015, VP-016, VP-017 | 10 |
| Layout | VP-002, VP-012 | 2 |
| Density | VP-004 | 1 |
| Typography | VP-011 | 1 |
| Spacing | VP-013 | 1 |
| Iconography | VP-010 | 1 |
| Token/color | VP-009 | 1 |

## Gap Inventory by Remediation Target

| Target | Gap IDs | Count |
|--------|---------|-------|
| SCSS source fix | VP-001, VP-002, VP-003, VP-006, VP-008, VP-009, VP-010, VP-011, VP-012, VP-013, VP-014, VP-015, VP-016 | 13 |
| CssProvider contract change (escalate) | VP-004 | 1 |
| gap-analysis-resolution intake | VP-005 | 1 |
| Bootstrap SCSS fix | VP-017 | 1 |
| Architecture decision (escalate) | VP-002 (class naming) | 1 |

## Root Cause Summary

The majority of gaps trace to **two root causes:**

1. **Class naming mismatch** (VP-002, VP-006, VP-010, VP-011, VP-012, VP-013, VP-014): The Razor template emits `mar-tree-item__*` classes but the SCSS defines `mar-treeitem__*` (Phase 2) or flat `mar-tree-item` / `mar-tree-item--*` (Phase 1). Fixing the naming alignment would immediately resolve or reduce 7 of 17 gaps.

2. **Missing state coverage** (VP-001, VP-003, VP-005, VP-007, VP-008, VP-015, VP-016): Several states (focus, disabled, readonly, drag, edit) have behavioral support in the component but zero visual treatment in SCSS. This is a pure SCSS authoring gap.

## Recommended Priority Order

1. **VP-002** (class system unification) -- prerequisite for most other fixes
2. **VP-001** (focus ring) -- critical a11y requirement
3. **VP-003** (disabled state) -- critical a11y requirement
4. **VP-008** (checkbox theming) -- high visual impact
5. **VP-012** (header layout) -- structural prerequisite
6. **VP-004** (density) -- spec-documented feature
7. **VP-007** (readonly) -- state treatment
8. **VP-009** (dark mode QA) -- token verification
9. **VP-010, VP-011, VP-013, VP-014** (icon/typography/children/toggle) -- mostly resolved by VP-002
10. **VP-015, VP-016** (drag/edit) -- polish
11. **VP-005** (appearance variants) -- new feature
12. **VP-017** (Bootstrap) -- secondary provider

---

## CHECKPOINT -- Orchestrator Review Required

**Worker:** `w-treeview-delivery`
**Stage:** 03-visual-parity
**Build verification:** `dotnet build Marilo.slnx` -- succeeded (0 warnings, 0 errors)
**Output file:** `stages/03-visual-parity/output/treeview-visual-parity-gaps.md`
**Gap count:** 17 total (3 critical, 6 major, 5 minor, 3 polish)
**Key escalation items:**
- VP-002: Class naming convention decision (Razor `mar-tree-item__*` vs SCSS `mar-treeitem__*`) requires architecture-level decision
- VP-004: `Size` parameter needs CssProvider contract extension (provider contract change)

Awaiting orchestrator review. Worker will set status to `review-pending` after writing result file and handoff.
