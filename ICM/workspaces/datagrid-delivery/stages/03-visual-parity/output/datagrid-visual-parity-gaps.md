# DataGrid Visual-Parity Gap Records

**Wave:** 3 — 03-visual-parity
**Worker:** w-datagrid-delivery
**Audit date:** 2026-04-11T17:40Z
**Approach:** Static-analysis audit — SCSS tokens, provider component styles, and MariloDataGrid.razor markup cross-referenced against the capture matrix. No browser capture this pass. Gaps that genuinely require pixel inspection are recorded as `DEFERRED-TO-CAPTURE` in the deferred section below.
**Base SHA:** 93bd1a54778818a48f20d01188b7240c916bf4e7

## Files Read

- `src/Marilo.Providers.FluentUI/Styles/components/_data-grid.scss` (191 lines)
- `src/Marilo.Providers.Bootstrap/Styles/components/_data-grid.scss` (154 lines)
- `src/Marilo.Providers.Material/Styles/components/_data-grid.scss` (5 lines — TODO placeholder only)
- `src/Marilo.Providers.FluentUI/Styles/foundation/_colors.scss` (163 lines)
- `src/Marilo.Providers.FluentUI/Styles/foundation/_spacing.scss` (15 lines)
- `src/Marilo.Providers.FluentUI/Styles/foundation/_typography.scss` (47 lines)
- `src/Marilo.Providers.FluentUI/Styles/foundation/_elevation.scss` (31 lines)
- `src/Marilo.Providers.Bootstrap/Styles/_tokens.scss` (83 lines)
- `src/Marilo.Providers.Bootstrap/Styles/_tokens-dark.scss` (55 lines)
- `src/Marilo.Components/DataGrid/MariloDataGrid.razor` (440 lines, 41 `mar-datagrid-*` class references)

---

## Static-Analysis Gap Records

Records below are the audit findings that can be scored from source alone. Each record follows `visual-parity-gap-format.md`. Scoring rationale is token-level evidence, not pixel comparison.

---

### VP-datagrid-001

**ID:** VP-datagrid-001
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Row hover
**Reference Source:** Telerik Grid
**Parity Score:** 1
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `.mar-datagrid-row:hover { background: var(--marilo-color-surface); }` — hover uses the same token as the header and filter row background (`#f3f2f1`). The hover tint is therefore identical to the header surface, so a row under the pointer blends into the header/filter chrome and is indistinguishable from stripe background in default mode. | Telerik Grid uses a dedicated "subtle hover" state layer (a different value from the header fill) so that hover is visually distinct from both the default row and header bands. |
| Likely cause | Token-level: `--marilo-color-surface-hover` exists (`#edebe9`) and is used by `.mar-datagrid-header-cell--sortable:hover`, but the row-level hover rule references `--marilo-color-surface` instead. | |

**Category:** state treatment
**Recommended change:** Change `.mar-datagrid-row:hover` in `_data-grid.scss` (FluentUI) to use `var(--marilo-color-surface-hover)`, or introduce a dedicated `--marilo-color-state-hover` token that layers cleanly over surface and striped-even rows.
**Acceptance criteria:** Hovering a row produces a visible background change vs. the resting row AND vs. the header band in both striped and non-striped demos.
**Remediation handoff target:** SCSS source fix (FluentUI provider `_data-grid.scss`).

---

### VP-datagrid-002

**ID:** VP-datagrid-002
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Dark
**State/Scenario:** Row hover + striped rows
**Reference Source:** Telerik Grid
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | In dark mode `--marilo-color-surface` = `#252423`, `--marilo-color-surface-hover` = `#323130`, and `--marilo-color-background` = `#1b1a19`. The striped-even rule `.mar-datagrid-row--striped:nth-child(even) { background: var(--marilo-color-surface); }` paints the even rows with `#252423` — but the hover rule also paints `#f3f2f1` → `#252423`, which is **exactly the striped-even value**. Result: hovering a striped-even row produces zero visual change. Additionally, the header rule and the hover rule share the same surface token, meaning hover on a striped-even row produces no delta at all. | Hover should produce a distinct state layer, independent of stripe parity. Telerik's dark-mode grid uses a separate `state-hover` overlay (alpha mix over surface) that is perceptible on both odd and even rows. |
| Likely cause | Token reuse between default background, stripe fill, and hover fill. Dark mode has no dedicated state-layer token. | |

**Category:** state treatment
**Recommended change:** Introduce a dark-mode state-layer token (e.g. `--marilo-color-row-hover` = `rgba(255,255,255,0.06)`) applied as an overlay on top of the row background. Apply to `.mar-datagrid-row:hover` via `background-color` or `box-shadow inset`.
**Acceptance criteria:** In dark mode, hovering a striped-even row visibly shifts the row tint and cannot be confused with the resting stripe fill.
**Remediation handoff target:** SCSS source fix (FluentUI provider + foundation `_colors.scss` dark block).

---

### VP-datagrid-003

**ID:** VP-datagrid-003
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Selected row
**Reference Source:** Telerik Grid
**Parity Score:** 2
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `.mar-datagrid-row--selected { background: var(--marilo-color-primary-light); }` (`#deecf9`) with a suppressed hover: `&--selected:hover { background: var(--marilo-color-primary-light); }` — selected rows do **not** change visual state on hover, so the combined "selected + hover" state is identical to "selected + rest". There is also no left-edge accent indicator. | Telerik's selected-row treatment uses a primary-tinted background AND either a slightly darker primary on hover OR a 2–3px left accent bar. The "selected + hover" combo is distinguishable. |
| Likely cause | Deliberate suppression of hover on selected, but no alternative accent. Missing combined state-layer. | |

**Category:** state treatment
**Recommended change:** Either (a) let `&--selected:hover` use `color-mix(in srgb, var(--marilo-color-primary-light) 85%, var(--marilo-color-primary) 15%)`, or (b) add a `border-left: 2px solid var(--marilo-color-primary);` inset on `.mar-datagrid-row--selected`.
**Acceptance criteria:** A selected row under the pointer is visibly different from a selected row at rest, AND a selected row is distinguishable from an unselected row at a glance.
**Remediation handoff target:** SCSS source fix.

---

### VP-datagrid-004

**ID:** VP-datagrid-004
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Dark
**State/Scenario:** Selected row
**Reference Source:** Telerik Grid
**Parity Score:** 1
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Dark-mode `--marilo-color-primary-light` = `#0a2e4a` (a near-black navy). At row scale on a `#1b1a19` background, this tint has insufficient luminance delta (both are effectively "very dark"). Selection visibility is poor. | Dark-mode selected-row treatment should push closer to a desaturated primary with adequate lightness delta (e.g. `rgba(96,205,255,0.18)` over surface), so the selected row is unmistakable against the background. |
| Likely cause | Token-level: `--marilo-color-primary-light` was picked as a "darker primary" for dark mode instead of as an alpha-blended state layer. | |

**Category:** token/color
**Recommended change:** Redefine dark-mode `.mar-datagrid-row--selected` to use `background: color-mix(in srgb, var(--marilo-color-primary) 18%, var(--marilo-color-surface));` OR introduce a `--marilo-color-row-selected` token with a lightness-delta-adequate value in both modes.
**Acceptance criteria:** In dark mode, a selected row has ≥ 1.3:1 luminance contrast against the unselected row (enough to be noticed without squinting).
**Remediation handoff target:** SCSS source fix + foundation token addition.

---

### VP-datagrid-005

**ID:** VP-datagrid-005
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Header row
**Reference Source:** Telerik Grid
**Parity Score:** 2
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Header uses `background: var(--marilo-color-surface);` and `font-weight: var(--marilo-font-weight-semibold);` (600). There is no dedicated header typography token (size or letter-spacing). Header cells inherit `--marilo-font-size-base` (0.875rem). Default grid row cells use the same font-size, so header typography only differs by weight. | Telerik Grid (Fluent flavor) uses a slightly reduced header font-size (0.8125rem) OR a text-transform/letter-spacing treatment to anchor the header visually. |
| Likely cause | Typography: no dedicated header font-size/weight token in foundation; header-cell rule leans on component defaults. | |

**Category:** typography
**Recommended change:** Add `--marilo-datagrid-header-font-size: 0.8125rem` or apply `letter-spacing: 0.01em` to `.mar-datagrid-header-cell` to differentiate from body rows.
**Acceptance criteria:** Header typography is distinguishable from body cells at 1280px without relying solely on weight.
**Remediation handoff target:** SCSS source fix (FluentUI component file).

---

### VP-datagrid-006

**ID:** VP-datagrid-006
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light/Dark
**State/Scenario:** Row height / density
**Reference Source:** Telerik Grid
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `.mar-datagrid-cell { padding: var(--marilo-space-sm) var(--marilo-space-md); }` = 8px × 12px. Header cells use the same. There is NO `line-height` set on the cell, so cells inherit `--marilo-line-height-base` (1.5). Result: every row is `0.875rem × 1.5 + 16px vertical padding ≈ 37px` minimum — with no density variants (comfortable / compact / dense). | Telerik Grid exposes density variants. Marilo needs at least a "compact" density where cell padding drops to `--marilo-space-xs` (4px) vertically. |
| Likely cause | Missing-state coverage: no density mixin applied to DataGrid; `_density.scss` pattern exists in the foundation but is not referenced. | |

**Category:** density
**Recommended change:** Introduce `.mar-datagrid[data-density="compact"] .mar-datagrid-cell { padding: var(--marilo-space-xs) var(--marilo-space-md); }` and a comfortable variant. Wire to `Density` parameter on `MariloDataGrid` (if present) or expose via CSS only.
**Acceptance criteria:** Three density modes render with distinct row heights (comfortable ≈ 40px, default ≈ 36px, compact ≈ 28px).
**Remediation handoff target:** SCSS source fix + possible component parameter (escalate if parameter required).

---

### VP-datagrid-007

**ID:** VP-datagrid-007
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Sorted column (sort indicator)
**Reference Source:** Telerik Grid
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | MariloDataGrid.razor L125-129 renders `<span class="mar-datagrid-sort-indicator">` containing the arrow glyph and an optional `<sub class="mar-datagrid-sort-order">`. **Neither `.mar-datagrid-sort-indicator` nor `.mar-datagrid-sort-order` has any style rule in the FluentUI `_data-grid.scss`, the Bootstrap `_data-grid.scss`, or the foundation files.** The arrow will render with default user-agent sub/span typography — no sizing, no alignment, no color accent. | Telerik shows a filled/outlined chevron icon inline with the header label, color-matched to the header text, with an integer badge for multi-sort. |
| Likely cause | Component-level missing rule: DOM elements emitted with no SCSS selector to style them. | |

**Category:** iconography
**Recommended change:** Add rules for `.mar-datagrid-sort-indicator` (display inline-flex, margin-left, color: currentColor, font-size: 0.75rem) and `.mar-datagrid-sort-order` (vertical-align, font-size: 0.625rem, color: --marilo-color-primary) to FluentUI + Bootstrap provider files.
**Acceptance criteria:** Sorted-asc/desc columns show a visible chevron aligned with the header label; multi-sort order badge is readable at 1280px.
**Remediation handoff target:** SCSS source fix (both providers).

---

### VP-datagrid-008

**ID:** VP-datagrid-008
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Pager (idle)
**Reference Source:** Telerik Grid
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | MariloDataGrid.razor L367-428 renders `.mar-datagrid-pager-btn`, `.mar-datagrid-pager-btn--active`, `.mar-datagrid-pager-btn--prev`, `.mar-datagrid-pager-btn--next`, `.mar-datagrid-pager-ellipsis`, `.mar-datagrid-pager-info`, `.mar-datagrid-pagesize-select`. **Grep of FluentUI `_data-grid.scss` finds zero selectors matching `mar-datagrid-pager-btn`.** Only the parent container `.mar-datagrid-pager` is styled. All pager buttons will render with user-agent `<button>` styling — mismatched font, default border, default background — and the active state (`--active` modifier) has no visual differentiation. | Telerik pager: tinted button backgrounds, rounded corners, active page number filled with primary color, hover/focus rings. |
| Likely cause | Component-level missing rules: pager sub-elements have no SCSS at all. | |

**Category:** state treatment (compounded with layout + typography)
**Recommended change:** Add a full pager rule block to `_data-grid.scss`: `.mar-datagrid-pager-btn { padding: var(--marilo-space-xs) var(--marilo-space-sm); border: 1px solid var(--marilo-color-border); background: var(--marilo-color-background); border-radius: var(--marilo-radius-sm); font: inherit; cursor: pointer; min-width: 2rem; } .mar-datagrid-pager-btn--active { background: var(--marilo-color-primary); color: var(--marilo-color-on-primary); border-color: var(--marilo-color-primary); } .mar-datagrid-pager-btn:hover:not(:disabled) { background: var(--marilo-color-surface-hover); } .mar-datagrid-pager-btn:disabled { opacity: 0.5; cursor: not-allowed; }` plus `.mar-datagrid-pager-ellipsis` and `.mar-datagrid-pagesize-select` sizing.
**Acceptance criteria:** Pager buttons have consistent Marilo font/border/padding; active page is filled with primary color; disabled prev/next are visually disabled.
**Remediation handoff target:** SCSS source fix (FluentUI + Bootstrap providers).

---

### VP-datagrid-009

**ID:** VP-datagrid-009
**Component:** MariloDataGrid
**Theme:** Bootstrap
**Mode:** Light
**State/Scenario:** Pager (idle)
**Reference Source:** Telerik Grid
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Same as VP-datagrid-008, applied to Bootstrap provider. `_data-grid.scss` (Bootstrap) defines `.mar-bs-datagrid-pager` padding/border but **no selector matching `mar-datagrid-pager-btn*`**. The DataGrid razor emits the shared `mar-datagrid-pager-btn` classes regardless of provider, so Bootstrap inherits the same unstyled buttons. | Bootstrap-themed pager should use `.page-item/.page-link` idiom or at minimum map the Marilo pager classes onto Bootstrap button typography/borders. |
| Likely cause | Component-level missing rule — Bootstrap bridge does not re-style the pager buttons. | |

**Category:** state treatment
**Recommended change:** Add a pager rule block to Bootstrap `_data-grid.scss` that maps the `mar-datagrid-pager-btn*` classes onto Bootstrap pagination idiom (or equivalent border/spacing tokens).
**Acceptance criteria:** Pager buttons match Bootstrap card chrome and expose active/disabled/hover states.
**Remediation handoff target:** SCSS source fix (Bootstrap provider).

---

### VP-datagrid-010

**ID:** VP-datagrid-010
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Empty state
**Reference Source:** Telerik Grid
**Parity Score:** 0
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | L236 emits `<td class="mar-datagrid-empty">` as the empty-row `gridcell`. **No `.mar-datagrid-empty` selector in FluentUI `_data-grid.scss`, Bootstrap `_data-grid.scss`, or foundation files.** The empty message will render as a single left-aligned text node with default cell padding and no center alignment, icon, or padding increase. | Telerik Grid empty state: centered text, larger padding (2–3× row height), optional icon. |
| Likely cause | Component-level missing rule. | |

**Category:** layout
**Recommended change:** Add `.mar-datagrid-empty { text-align: center; padding: var(--marilo-space-xl) var(--marilo-space-md); color: var(--marilo-color-text-secondary); font-size: var(--marilo-font-size-base); }` to both provider files.
**Acceptance criteria:** Empty state visually reads as "no data" — centered, spaced, differentiated from a real data row.
**Remediation handoff target:** SCSS source fix (both providers).

---

### VP-datagrid-011

**ID:** VP-datagrid-011
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Loading state
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | L44-46 emit `.mar-datagrid-loading-overlay`, `.mar-datagrid-loading-spinner`, `.mar-datagrid-loading-text`. **Zero matching selectors in either provider SCSS.** The overlay will render in document flow with no absolute positioning, no backdrop tint, and no spinner — just a bare text node. | A loading overlay should be absolutely-positioned over the data area with a semi-transparent backdrop and a centered spinner. |
| Likely cause | Missing-state coverage: the component emits the overlay, but the provider never styled it. | |

**Category:** layout (compounded with elevation)
**Recommended change:** Add `.mar-datagrid { position: relative; } .mar-datagrid-loading-overlay { position: absolute; inset: 0; display: flex; align-items: center; justify-content: center; background: color-mix(in srgb, var(--marilo-color-surface) 70%, transparent); z-index: 5; } .mar-datagrid-loading-spinner { ... } .mar-datagrid-loading-text { margin-left: var(--marilo-space-sm); }` to FluentUI and Bootstrap providers.
**Acceptance criteria:** Toggling loading state shows a translucent overlay with a visible spinner that does not shift the grid layout.
**Remediation handoff target:** SCSS source fix (both providers).

---

### VP-datagrid-012

**ID:** VP-datagrid-012
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Popup edit dialog
**Reference Source:** Telerik Grid
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | L327-353 emit `.mar-datagrid-popup-overlay`, `.mar-datagrid-popup-dialog`, `.mar-datagrid-popup-header`, `.mar-datagrid-popup-body`, `.mar-datagrid-popup-field`, `.mar-datagrid-popup-actions`, `.mar-datagrid-validation-summary`, `.mar-datagrid-cmd-btn`. The only matching selectors in FluentUI `_data-grid.scss` are `.mar-datagrid-cmd-btn` **inside the filter-menu-actions rule only** (scoped). **None of the popup selectors are styled.** The popup will render as an inline `div` with no overlay, no centering, no dialog chrome. | Telerik popup edit: centered modal, backdrop scrim, elevation shadow, header/body/actions layout, primary-styled Save, secondary-styled Cancel. |
| Likely cause | Component-level missing rules — entire popup sub-tree is unstyled. | |

**Category:** elevation (+ layout + state treatment)
**Recommended change:** Add a full `.mar-datagrid-popup-*` rule block with `position: fixed; inset: 0; z-index: var(--marilo-z-modal);` on the overlay; `.mar-datagrid-popup-dialog { background: var(--marilo-color-background); border-radius: var(--marilo-radius-lg); box-shadow: var(--marilo-shadow-xl); max-width: 520px; margin: auto; }`. Reuse the foundation overlay pattern from `patterns/_overlay.scss`.
**Acceptance criteria:** Popup edit mode renders a centered modal with scrim, shadow, and distinct header/body/actions regions.
**Remediation handoff target:** SCSS source fix (FluentUI + Bootstrap providers).

---

### VP-datagrid-013

**ID:** VP-datagrid-013
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Filter menu popover (hardcoded fallbacks)
**Reference Source:** Telerik Grid
**Parity Score:** 2
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_data-grid.scss` L128, L160, L176, L189 hardcode `#ffffff` as the `color-mix()` base color and input/button background — specifically `background: color-mix(in srgb, var(--marilo-color-primary, #0f6cbd) 10%, #ffffff);` and `background: #fff;` for filter-menu operator/value inputs. **In dark mode, `--marilo-color-surface` becomes `#252423` but the filter menu still uses `#fff`**, producing a white popover on a dark grid. This is the cerebrum "color-mix base color must use `var(--marilo-color-surface)`" issue, confirmed. | Filter menu chrome must respect dark-mode tokens. Base color for `color-mix` should be `var(--marilo-color-surface)`; input/button `background: #fff` should be `var(--marilo-color-background)` or `var(--marilo-color-surface)`. |
| Likely cause | Token-level: hardcoded `#fff` instead of token reference. | |

**Category:** token/color
**Recommended change:** Replace all `#fff`/`#ffffff` fallbacks in filter-menu selectors with `var(--marilo-color-background)` or `var(--marilo-color-surface)`. Replace the `color-mix` base `#ffffff` with `var(--marilo-color-surface)`.
**Acceptance criteria:** Filter menu popover chrome flips to dark surface tokens in `[data-marilo-theme="dark"]` without pixel-level inspection needing to be a workaround.
**Remediation handoff target:** SCSS source fix (FluentUI provider).

---

### VP-datagrid-014

**ID:** VP-datagrid-014
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Dark
**State/Scenario:** Filter menu popover shadow
**Reference Source:** Telerik Grid
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_data-grid.scss` L160: `box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);` — hardcoded shadow, not the `--marilo-shadow-lg` or `--elevation-shadow-flyout` token. In dark mode the dark-mode shadow block in `_elevation.scss` overrides `--marilo-shadow-*` to 0.4/0.32 alpha, but this filter menu ignores it entirely. Shadow stays a 12% black overlay — too subtle against dark surface. | Dark-mode popovers should use a visibly deeper shadow or a light-halo border to separate from the grid. |
| Likely cause | Token-level: hardcoded rgba shadow instead of elevation token. | |

**Category:** elevation
**Recommended change:** Replace hardcoded `box-shadow` on `.mar-datagrid-filter-menu` with `box-shadow: var(--elevation-shadow-flyout);` (already defined in `_elevation.scss`).
**Acceptance criteria:** Filter menu popover carries an elevation distinct from the grid surface in both light and dark modes.
**Remediation handoff target:** SCSS source fix (FluentUI provider).

---

### VP-datagrid-015

**ID:** VP-datagrid-015
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Default grid — focus treatment
**Reference Source:** Telerik Grid
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | No `:focus` or `:focus-visible` rule anywhere in FluentUI `_data-grid.scss` for `.mar-datagrid-cell`, `.mar-datagrid-row`, `.mar-datagrid-header-cell`, `.mar-datagrid-pager-btn`, `.mar-datagrid-filter-menu-btn`, or any of the pager/filter inputs. Foundation `_elevation.scss` defines `--focus-stroke-outer` and `--focus-stroke-width: 2` but none of them are applied to DataGrid selectors. | Every interactive element must carry a visible keyboard focus ring, especially given spec claims that `Navigable=true` enables keyboard navigation (Wave 1 SA-06 flags source does not implement). |
| Likely cause | Component-level missing rules AND token underuse (`--focus-stroke-outer` unused). | |

**Category:** state treatment
**Recommended change:** Add `:focus-visible` rules to `.mar-datagrid-cell`, `.mar-datagrid-header-cell--sortable`, `.mar-datagrid-pager-btn`, `.mar-datagrid-filter-menu-btn`, `.mar-datagrid-pagesize-select` using `outline: var(--focus-stroke-width, 2px) solid var(--focus-stroke-outer, var(--marilo-color-primary)); outline-offset: -2px;`.
**Acceptance criteria:** Every DataGrid interactive element shows a visible focus ring on keyboard focus in both light and dark modes.
**Remediation handoff target:** SCSS source fix (both providers).

---

### VP-datagrid-016

**ID:** VP-datagrid-016
**Component:** MariloDataGrid
**Theme:** Material
**Mode:** Light/Dark
**State/Scenario:** All states
**Reference Source:** Telerik Grid
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `src/Marilo.Providers.Material/Styles/components/_data-grid.scss` is a 5-line TODO placeholder with zero rules. Material-themed grids currently inherit nothing — even the base `.mar-datagrid` border/radius is absent. | Material provider should implement the full grid visual treatment. |
| Likely cause | Missing-state coverage: Material provider runtime project not yet implemented (per Wave 3 plan's known unknowns). | |

**Category:** missing implementation
**Recommended change:** Implement Material provider DataGrid SCSS as a separate gap-analysis intake. Until then, every Material-theme state is an implicit score 0.
**Acceptance criteria:** Material provider renders DataGrid with Material 3 tokens for all states in the capture matrix.
**Remediation handoff target:** gap-analysis-resolution intake (not a SCSS patch — requires new provider implementation track).

---

### VP-datagrid-017

**ID:** VP-datagrid-017
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Grouped state
**Reference Source:** Telerik Grid
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `.mar-datagrid-group-header { font-weight: semibold; background: var(--marilo-color-surface); padding: xs md; }`. Same background as the header row and the filter row — the group-header band is indistinguishable from the header when multiple headers stack. No indent or expand/collapse chevron affordance in the rule. | Group-header should have a distinct accent OR a left-indent indicating group nesting, plus chevron iconography. |
| Likely cause | Layout: no indent/chevron styling; shared surface token. | |

**Category:** layout
**Recommended change:** Add a subtle left border accent (`border-left: 3px solid var(--marilo-color-primary);`) and reserved indentation on `.mar-datagrid-group-header`.
**Acceptance criteria:** Group headers are visually distinct from column headers and filter rows.
**Remediation handoff target:** SCSS source fix.

---

### VP-datagrid-018

**ID:** VP-datagrid-018
**Component:** MariloDataGrid
**Theme:** Fluent
**Mode:** Light/Dark
**State/Scenario:** Checkbox column alignment
**Reference Source:** Telerik Grid
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | L89 emits `<th class="mar-datagrid-checkbox-cell" ... style="width:40px;">` and cells similarly constrained inline. **No `.mar-datagrid-checkbox-cell` selector in either provider SCSS.** With cell padding `8px 12px` and a native checkbox, the checkbox will be left-aligned and vertically drift depending on row height. No flex centering applied. | Telerik column: centered checkbox horizontally and vertically, no text padding. |
| Likely cause | Component-level missing rule + inline width instead of token. | |

**Category:** layout (alignment)
**Recommended change:** Add `.mar-datagrid-checkbox-cell { width: 40px; padding: 0; text-align: center; vertical-align: middle; } .mar-datagrid-checkbox-cell input[type="checkbox"] { margin: 0; vertical-align: middle; }` to both provider files.
**Acceptance criteria:** Checkbox column: checkboxes centered both axes, consistent across data/header/footer rows.
**Remediation handoff target:** SCSS source fix (both providers).

---

### VP-datagrid-019

**ID:** VP-datagrid-019
**Component:** MariloDataGrid
**Theme:** Bootstrap
**Mode:** Dark
**State/Scenario:** Filter menu (hardcoded `#fff` backgrounds)
**Reference Source:** Telerik Grid
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Bootstrap `_data-grid.scss` L72, L125, L147: `background: #fff;` hardcoded on `.mar-datagrid-filter-menu-btn`, `.mar-datagrid-filter-menu`, and `.mar-datagrid-cmd-btn` inside the filter-menu actions. In dark mode (`[data-marilo-theme="dark"]` or `[data-bs-theme="dark"]`), these remain white — same cerebrum "base color must use surface token" issue as VP-datagrid-013 but applied to the Bootstrap bridge. | Bootstrap-dark popover should use `var(--marilo-color-surface)` or `var(--bs-body-bg)` instead of literal `#fff`. |
| Likely cause | Token-level: hardcoded `#fff` instead of token references in the Bootstrap bridge. | |

**Category:** token/color
**Recommended change:** Replace `#fff` literals with `var(--marilo-color-background)` (or `var(--bs-body-bg)` if preferred in the bridge layer). Also confirm `[data-marilo-theme="dark"]` vs `[data-bs-theme="dark"]` scoping per cerebrum learning.
**Acceptance criteria:** Bootstrap-dark filter menu popover matches the surrounding dark surface.
**Remediation handoff target:** SCSS source fix (Bootstrap provider).

---

### VP-datagrid-020

**ID:** VP-datagrid-020
**Component:** MariloDataGrid
**Theme:** Bootstrap
**Mode:** Light
**State/Scenario:** Striped rows (Sass interpolation)
**Reference Source:** internal Marilo baseline
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Bootstrap `_data-grid.scss` L34: `background-color: #{$table-striped-bg};` — interpolates a Bootstrap Sass variable at compile time, which means: (a) the value is frozen at the compile-time Bootstrap theme, and (b) `[data-marilo-theme="dark"]` toggles cannot override it. In dark mode the striped rows retain the light-mode Bootstrap striping. Not strictly wrong, but it locks out runtime dark theme switching. | Striping should use a runtime CSS variable that the dark block can override: e.g. `background-color: var(--marilo-color-surface);`. |
| Likely cause | Token-level: compile-time Sass interpolation instead of runtime custom property. | |

**Category:** token/color
**Recommended change:** Replace `#{$table-striped-bg}` with `var(--marilo-color-surface)` or a new dedicated `--marilo-color-stripe` token that has a dark-mode override in `_tokens-dark.scss`.
**Acceptance criteria:** Striping remains visible in both light and dark Bootstrap modes without a SCSS recompile.
**Remediation handoff target:** SCSS source fix (Bootstrap provider).

---

## DEFERRED-TO-CAPTURE

Combinations below cannot be scored from static SCSS analysis alone — they require actual rendering to judge. Listed with the single reason each needs pixels.

| # | Theme | Mode | State | Reason pixels required |
|---|-------|------|-------|------------------------|
| DC-01 | Fluent | Light | Default grid — at-rest rendering | Confirm that the semantic `<table>` overrides (L88-91 in `_data-grid.scss`) actually un-do the `display:flex` from the generic row rule. Requires DOM inspection or screenshot. |
| DC-02 | Fluent | Light | Header row — alignment drift | Need pixels to see whether `flex: 1` cells align with `table-row` header cells (mixed layout risk). |
| DC-03 | Fluent | Light/Dark | Row hover — text-ellipsis overflow | `.mar-datagrid-cell { text-overflow: ellipsis; white-space: nowrap; }` with `flex: 1` produces different widths than `<td>` cells. Needs capture to see if truncation is consistent. |
| DC-04 | Fluent | Light | Sorted ascending — chevron glyph rendering | `mar-datagrid-sort-indicator` has no SCSS (VP-datagrid-007) but also need pixels to confirm what the razor emits as the arrow glyph and how the default sub styling looks. |
| DC-05 | Fluent | Dark | Filter row input chrome | `.mar-datagrid-filter-row` only sets a background — the inputs inside inherit from `patterns/_field-base.scss`. Need capture to see if field chrome contrasts with row background. |
| DC-06 | Fluent | Light | Filter menu popover — positioning overflow | `.mar-datagrid-filter-popup { top: calc(100% + 0.35rem); right: 0; }` — requires capture to confirm it does not clip at viewport edge. |
| DC-07 | Fluent | Light/Dark | Pager hover/active/focus | VP-datagrid-008 notes the pager buttons are unstyled. Once styled, hover/active/focus deltas need pixel confirmation against Telerik's Fluent pager. |
| DC-08 | Fluent | Light | Inline edit row — form control alignment | Edit mode emits `<input>` elements inside `<td>` cells; vertical alignment with non-editing cells in the same row needs capture. |
| DC-09 | Fluent | Light | Popup edit dialog — backdrop blur / scrim | VP-datagrid-012 notes the whole popup is unstyled. Once styled, scrim opacity and dialog shadow need pixel confirmation. |
| DC-10 | Fluent | Light/Dark | Virtualization / dense scroll | Virtualized row heights must be pixel-consistent with non-virtualized; requires capture. |
| DC-11 | Bootstrap | Light | Default grid | Bootstrap-provider grid inherits much from Bootstrap table styles; needs capture to see how the Marilo overrides compose with Bootstrap's `.table` baseline. |
| DC-12 | Bootstrap | Dark | Row hover | Bootstrap `--bs-table-hover-bg` interacts with Marilo's hover rule; needs capture for dark-mode delta. |
| DC-13 | Bootstrap | Light | Header row border contrast | Bootstrap's `$table-border-color` vs `--marilo-color-border` — need capture to see which wins. |
| DC-14 | Bootstrap | Light | Pager (once fixed) | Post-fix for VP-datagrid-009, need capture confirmation. |
| DC-15 | Bootstrap | Dark | Filter menu (once fixed) | Post-fix for VP-datagrid-019, need capture confirmation. |
| DC-16 | Material | All | All states | VP-datagrid-016 blocks everything — no runtime Material provider exists. Capture is literally impossible until the provider project is created. |
| DC-17 | Fluent/Bootstrap | Light/Dark | Toolbar / command area | Razor emits `.mar-datagrid-toolbar` and the SCSS has a rule, but the command buttons inside (`.mar-datagrid-cmd-btn` outside the filter-menu scope) are unstyled — need capture. |
| DC-18 | Fluent | Light | Detail template row | `.mar-datagrid-detail-row` is emitted at L275 but has no SCSS; need capture to see default behavior. |
| DC-19 | Fluent | Light | Footer row / footer cell | `.mar-datagrid-footer-row` and `.mar-datagrid-footer-cell` are emitted at L289/306 but have no SCSS. Need capture to see the inherit/fallback rendering. |
| DC-20 | Fluent/Bootstrap | Dark | All primary states | No single-source token problem fully validates without side-by-side capture at the 6 theme/mode combinations. |

---

## Next Steps

1. Route VP-datagrid-001 through -020 to the orchestrator review gate.
2. Any `critical` severity record (001, 002, 004, 008, 009, 010, 012, 015, 016) should fan out to `datagrid-gap-analysis` intake for source remediation, ideally bundled as a single "DataGrid provider visual gap batch".
3. Run Playwright capture against the 20 DEFERRED-TO-CAPTURE combinations in a follow-up wave once a Playwright capture pipeline is stood up (per the visual-parity plan's "Next Steps After Plan").
4. Re-score after first remediation pass; expect the unstyled-selector cluster (007, 008, 009, 010, 011, 012, 015, 018) to collapse to score 2/3 once a single SCSS pass lands the missing rules.
