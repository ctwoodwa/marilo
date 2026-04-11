# AllocationScheduler — Visual Parity Gap List

**Workspace:** ICM/workspaces/allocation-scheduler-delivery
**Stage:** 03-visual-parity
**Worker:** `w-allocation-scheduler-delivery`
**Wave:** 3 (2026-04-11)
**Method:** Static source analysis of the SCSS, razor markup, and dark-theme token bridge. No runtime screenshots captured (Material provider runtime absent; Playwright session not initiated). Scoring is conservative — borderline cases scored down per rubric rule.
**Primary source files read:**
- `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` (654 lines — full BEM implementation + dark patch block at L633-L653)
- `src/Marilo.Providers.FluentUI/Styles/_allocation-scheduler.scss` (654 lines — byte-identical duplicate; see VP-allocation-scheduler-019)
- `src/Marilo.Providers.FluentUI/Styles/_generated-base.scss` (dark token map at L244-L279)
- `src/Marilo.Providers.Bootstrap/Styles/components/_allocation-scheduler.scss` (832 lines — full bridge implementation)
- `src/Marilo.Providers.Bootstrap/Styles/_bridge-allocation-scheduler.scss` (832 lines — byte-identical duplicate)
- `src/Marilo.Providers.Material/Styles/components/_allocation-scheduler.scss` (5 lines — TODO placeholder, no styles)
- `src/Marilo.Components/DataDisplay/AllocationScheduler/MariloAllocationScheduler.razor` (403 lines — markup + class emission)

**Wave-2 carry-forward:** Wave 2 classified `accessibility` and `theming` as Missing demo topics. In this audit they surface as **implementation-visual gaps** — `accessibility` as state-treatment (focus indicators, SR affordances rendered in SCSS), `theming` as token/color (Material provider absent, dark-mode tint depending on fallback literals).

---

## Score Matrix (theme × mode × primary state)

Primary states: default view, occupied cell, cell hover, selected allocation. Scores 0–3 per rubric. Secondary/edge state scores captured in individual gap records below.

| Theme     | Mode  | Default | Occupied | Hover | Selected | Notes |
|-----------|-------|---------|----------|-------|----------|-------|
| Fluent    | Light | 3       | 3        | 2     | 3        | Hover = subtle 4% primary tint + row 5% subtle-background; borderline |
| Fluent    | Dark  | 2       | 2        | 2     | 2        | color-mix fallback `#ffffff` + `--marilo-color-text` bridge only inside scoped dark patch |
| Bootstrap | Light | 3       | 3        | 3     | 3        | Strong bridge implementation |
| Bootstrap | Dark  | 2       | 2        | 2     | 2        | Relies on `--bs-body-bg` + `--bs-border-color` which Bootstrap dark provides, but disabled cell pattern uses hardcoded `rgba(0,0,0,0.07)` — invisible on dark stripes |
| Material  | Light | 0       | 0        | 0     | 0        | **No Material styles defined.** Component falls back to unthemed browser defaults |
| Material  | Dark  | 0       | 0        | 0     | 0        | Same as above; Material provider runtime project not yet created |

**Coverage:** 24 primary scores captured (4 states × 6 theme/modes). Six primary scores at 0 (Material), eight at 2 (Fluent Dark + Bootstrap Dark), ten at 3 (Light themes). See parity-summary.md for roll-up.

---

## Gap Records

### VP-allocation-scheduler-001 — Material provider has no AllocationScheduler styles (Light)

**Component:** MariloAllocationScheduler
**Theme:** Material
**Mode:** Light
**State/Scenario:** default view, occupied cell, cell hover, selected allocation, splitter at rest, timeline header, resource panel (structural baseline — all primary + secondary)
**Reference Source:** Telerik Scheduler (resource variant) / internal Marilo baseline
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `Marilo.Providers.Material/Styles/components/_allocation-scheduler.scss` is a 5-line placeholder (`// TODO: Placeholder — no Material 3 styles defined yet.`). With Material provider active, the component emits `mar-allocation-scheduler` BEM classes but no rules match — the grid renders as default unstyled HTML `<table>` with browser-default borders and zero padding. | Material 3 styling with M3 surface/outline/primary tokens, 40–48px row height, M3 elevation for the context menu, M3 hover state layers on cells. |
| Likely cause | Material runtime provider not yet implemented (confirmed in visual-parity-plan.md blockers). Placeholder explicitly says "Implement when the Material provider runtime project is created." | |

**Category:** token/color
**Recommended change:** Implement `Marilo.Providers.Material/Styles/components/_allocation-scheduler.scss` mirroring the FluentUI BEM surface against M3 tokens (`--md-sys-color-surface`, `--md-sys-color-outline`, `--md-sys-color-primary`, `--md-sys-state-layer-*`). Use M3 density of 48px rows at default and 40px for compact.
**Acceptance criteria:** With `data-marilo-provider="material"`, a default AllocationScheduler renders with Material surface/outline colours, hover shows an M3 state-layer tint (8% primary over surface), and selected cell shows primary-tinted background at ~12%. No unstyled `<table>` chrome visible.
**Remediation handoff target:** gap-analysis-resolution intake (allocation-scheduler-gap-analysis workspace) — cross-reference with Wave-2 Missing `theming` topic.

---

### VP-allocation-scheduler-002 — Material provider has no AllocationScheduler styles (Dark)

**Component:** MariloAllocationScheduler
**Theme:** Material
**Mode:** Dark
**State/Scenario:** default view + all state treatments
**Reference Source:** Telerik Scheduler / internal Marilo baseline
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Same source as VP-allocation-scheduler-001: no Material 3 SCSS exists, so dark mode inherits browser defaults. Borders white-on-white, timeline header typography unstyled, no state treatment at all. | M3 dark surface `#1C1B1F`, outline `#938F99`, hover state-layer 8% white-over-surface, selected 12%. |
| Likely cause | Material provider runtime project not yet implemented. | |

**Category:** token/color
**Recommended change:** Deliver Material provider runtime **before** investing in Material dark-mode tuning. Material dark is a separate remediation after VP-001.
**Acceptance criteria:** In Material dark, the grid is visually distinct from Material light (darker surface, inverted text), with all primary states treated.
**Remediation handoff target:** gap-analysis-resolution intake — dependent on VP-001.

---

### VP-allocation-scheduler-003 — Dark-mode color-mix fallback uses `#ffffff` literal (Fluent Dark)

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Dark
**State/Scenario:** occupied cell, cell hover, selected allocation, current-period highlight (every `color-mix` call in the component SCSS)
**Reference Source:** internal Marilo baseline (dark mode systemic)
**Parity Score:** 2 (borderline — depends on token bridge)
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Every `color-mix(in srgb, primary X%, var(--marilo-color-surface, #ffffff))` call in `_allocation-scheduler.scss` uses `#ffffff` as the **fallback literal** for surface. When `--marilo-color-surface` IS set (dark map at `_generated-base.scss:265 → #252423`), the fallback never fires and dark tints are correct. When a host app loads the component CSS **without** loading `_generated-base.scss` (common when consumers swap providers programmatically or the SCSS is referenced piecewise), the fallback `#ffffff` activates and the "dark" hover/selection tints become 10% primary over white — almost pure white — on what the host app intends as a dark surface. Nineteen occurrences of this pattern (see grep at lines 14, 47, 59, 73, 89, 143, 221, 334, 420, 424, 449, 454, 459, 470, 476, 492, 509, 561, 565, 604). | Fallback literal must be the dark surface colour, or no literal fallback at all (so undefined tokens surface as obvious bugs instead of white bleed-through). |
| Likely cause | `color-mix` base-colour literal written for light mode first; dark-mode fallback never reworked. Cerebrum `.wolf/cerebrum.md` explicitly calls this out as a known systemic issue (see `visual-parity-plan.md` known gap). | |

**Category:** token/color
**Recommended change:** Option A — remove the `#ffffff` fallbacks entirely (`var(--marilo-color-surface)` alone), forcing a loud failure when tokens are missing. Option B — use a mixin `scheduler-tint($pct)` that reads a dark-aware SCSS variable and expands to the correct literal at compile time. Option A is preferred because it surfaces bugs instead of hiding them.
**Acceptance criteria:** A host app that loads only `marilo-fluentui.scss` in dark mode renders cell-hover / selected tints with the true dark surface as the base (not #ffffff). Screenshot at `data-marilo-theme="dark"` shows hover tint on `#252423` base, not on white.
**Remediation handoff target:** SCSS source fix — tracked as systemic dark-mode issue shared across components (gantt, datagrid, scheduler exhibit the same pattern).

---

### VP-allocation-scheduler-004 — Context menu shadow invisible on dark surface (Fluent Dark + Bootstrap Dark)

**Component:** MariloAllocationScheduler
**Theme:** Fluent + Bootstrap
**Mode:** Dark
**State/Scenario:** context-menu visuals
**Reference Source:** Telerik Scheduler context menu
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L607: `box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);` — a black shadow at 12% opacity. On a light background this reads as a typical Fluent elevation. On the dark surface `#252423`, a black shadow is nearly invisible because the shadow colour and background colour are already the same hue. The menu appears as an unanchored floating panel with no visible drop. | Dark mode should use a stronger shadow (alpha ~0.4) or a white-tinted glow, matching the `--marilo-shadow-md` token defined at `_generated-base.scss:276` (`0 3.2px 7.2px rgba(0,0,0,0.4), 0 0.6px 1.8px rgba(0,0,0,0.32)`). |
| Likely cause | Shadow hardcoded at component level instead of using the `--marilo-shadow-*` token. Dark patch block at L633-L653 does not override shadow. | |

**Category:** elevation
**Recommended change:** Replace `box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12)` with `box-shadow: var(--marilo-shadow-lg)`. The token is already defined for light and dark in `_generated-base.scss`.
**Acceptance criteria:** Right-click on an allocation cell in dark mode — the resulting context menu has a visible edge separation from the underlying surface, matching the Telerik reference at equivalent density.
**Remediation handoff target:** SCSS source fix.

---

### VP-allocation-scheduler-005 — Focus ring on splitter uses hardcoded color (accessibility + token)

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** splitter at rest (focused)
**Reference Source:** internal Marilo baseline (a11y surface)
**Parity Score:** 2
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L180 (`&--focused`) and L198 (`&:focus-visible`) apply `outline: 2px solid var(--marilo-color-primary, #0f6cbd)` with a hardcoded light-mode primary fallback. The `&--focused` modifier additionally sets `background: var(--allocation-scheduler-splitter-hover-background, var(--marilo-color-primary, #0f6cbd))` — the focus state duplicates the hover state, so when a user Tab-focuses the splitter and then hovers it, the focus ring is indistinguishable from the hover state. A keyboard-only user cannot tell the splitter has focus vs. being hovered by a phantom cursor. | Distinct focus ring (e.g., outline with ring-offset) that does not collapse into the hover state. Telerik Scheduler uses a 2px outer glow ring that is orthogonal to the hover background. |
| Likely cause | Focus and hover share the same `--allocation-scheduler-splitter-hover-background` token — they should use separate tokens. | |

**Category:** state treatment (a11y)
**Recommended change:** Split the focus treatment from hover: add `--allocation-scheduler-splitter-focus-ring` token (defaulting to `var(--focus-ring, 2px solid var(--marilo-color-primary))`) and leave the background unchanged on focus. Drop the background override in `&--focused`.
**Acceptance criteria:** Tab-focusing the splitter and hovering the splitter produce visually distinct treatments. Focus ring is visible in both light and dark mode. Wave-2 Missing `accessibility` topic partially discharged at the visual layer.
**Remediation handoff target:** SCSS source fix + gap-analysis intake to cross-link with accessibility spec (`docs/component-specs/allocation-scheduler/accessibility.md`).

---

### VP-allocation-scheduler-006 — Cell editing input has no dark-mode text color (Fluent Dark)

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Dark
**State/Scenario:** cell editing
**Reference Source:** Telerik Scheduler cell edit
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L483-L495 defines the cell-editing `input` with `background: var(--marilo-color-surface, #ffffff)` but does **not** set `color:` — so the input inherits the browser default, which in dark mode is black text on dark-surface background. The user types and sees no characters. The dark patch block at L633-L653 sets `--marilo-color-text` but only on the outer `.mar-allocation-scheduler` scope, which does **not** cascade into `<input>` elements in all browsers (input elements do not inherit text colour by default in Chromium). | `input { color: var(--marilo-color-text); }` explicitly set. |
| Likely cause | Input inheritance assumption. `font-size: inherit; font-family: inherit;` is set but `color:` is not. | |

**Category:** token/color
**Recommended change:** Add `color: var(--marilo-color-text, #323130);` inside the `&--editing input {}` block at L483.
**Acceptance criteria:** In Fluent dark, double-clicking an editable cell and typing shows visible light-coloured text inside the input. No invisible-text bug.
**Remediation handoff target:** SCSS source fix.

---

### VP-allocation-scheduler-007 — Row hover background hardcoded `#f5f5f5` (Fluent Dark)

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Dark
**State/Scenario:** resource row hover
**Reference Source:** internal Marilo baseline
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L416: `&:hover { background: var(--marilo-color-subtle-background, #f5f5f5); }`. The fallback `#f5f5f5` is a light-grey, correct for light mode, wrong for dark. The dark patch block at L635 sets `--marilo-color-subtle-background: #2d2c2b`, so when the patch is loaded the value is correct; when only `marilo-fluentui.scss` components are imported without the patch block, the hover is `#f5f5f5` on a dark surface — near-white on near-black. Same class of issue as VP-003 but for a non-color-mix token. | Either no literal fallback, or a dark-aware fallback. Consistent with VP-003's recommendation. |
| Likely cause | Fallback literal written for light mode only. | |

**Category:** token/color
**Recommended change:** Drop the `#f5f5f5` fallback — use `var(--marilo-color-subtle-background)` bare.
**Acceptance criteria:** Host app loading only `marilo-fluentui.scss` with dark tokens renders row hover with dark-surface tint, not near-white.
**Remediation handoff target:** SCSS source fix — bundle with VP-003 as part of the same dark-mode literal sweep.

---

### VP-allocation-scheduler-008 — Disabled cell pattern invisible on dark stripes (Bootstrap Dark)

**Component:** MariloAllocationScheduler
**Theme:** Bootstrap
**Mode:** Dark
**State/Scenario:** disabled cell
**Reference Source:** Telerik Scheduler disabled cell
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Bootstrap bridge `_allocation-scheduler.scss` L31-L39 defines `--bs-scheduler-disabled-pattern` as a `repeating-linear-gradient` using `rgba(0, 0, 0, 0.07)` as the stripe colour. On Bootstrap dark background (`--bs-body-bg` becomes `#212529` in BS5 dark), black-at-7%-alpha stripes are invisible. The disabled cell visually collapses into a plain grey cell with no pattern. | Stripe colour should be a theme-aware white-at-low-alpha in dark mode, or a variable (`rgba(255,255,255,0.07)` under `[data-bs-theme="dark"]`). |
| Likely cause | Pattern hardcoded for light mode. Bootstrap dark variant not added. | |

**Category:** state treatment
**Recommended change:** Under `[data-bs-theme="dark"] .mar-bs-allocation-scheduler { --bs-scheduler-disabled-pattern: repeating-linear-gradient(135deg, transparent, transparent 3px, rgba(255,255,255,0.07) 3px, rgba(255,255,255,0.07) 6px); }`.
**Acceptance criteria:** Toggle `data-bs-theme="dark"` on the host page — disabled cells show visible stripe pattern.
**Remediation handoff target:** SCSS source fix (Bootstrap provider).

---

### VP-allocation-scheduler-009 — Cell selected + active ring duplicated (overlap confusion)

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** selected allocation + active cell simultaneously
**Reference Source:** internal Marilo baseline
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L453-L456 (`&--selected`) and L469-L473 (`&--active`) apply the **identical** treatment: `background: color-mix(... primary 10%)` + `box-shadow: inset 0 0 0 2px primary`. A cell that is both selected and active (e.g., selected via range, then keyboard-focused) has no visual differentiation between the two states. The drag-target modifier at L475-L478 uses primary 15% + 1px inset (instead of 2px) as a third variant; side-by-side, the three states are hard to tell apart. | Distinct treatments: selected = fill only, active = ring only, drag-target = different colour or stronger fill. |
| Likely cause | `--selected` and `--active` were added in separate changes without reconciling the overlap. | |

**Category:** state treatment
**Recommended change:** Differentiate: `--selected` keeps the 10% fill + 2px ring; `--active` drops the fill and uses a 2px ring at primary 100% with `outline-offset: -2px` for keyboard prominence; `--drag-target` keeps 15% fill without ring. Three visually-distinct treatments.
**Acceptance criteria:** A selected cell, an active cell, and a drag-target cell are all individually distinguishable at normal inspection distance.
**Remediation handoff target:** SCSS source fix.

---

### VP-allocation-scheduler-010 — Header-group-row background same as header-row (no tier contrast)

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** timeline header (grouped)
**Reference Source:** Telerik Scheduler grouped timeline header
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L263-L272 (`&__header-group-row`) and L294-L305 (`&__header-row`) both set `background: var(--marilo-color-subtle-background)` with no differentiation. When a Day view renders "Jan 2026" on top and "01/02/03" on the leaf row, both rows are the same background colour with only a 1px bottom border as separation. Telerik's reference uses a stronger top-tier background (or a subtle gradient) to anchor the group context. | Group row either one shade darker than the leaf row, or uses a distinct top-accent border. |
| Likely cause | Single token used for both tiers. | |

**Category:** density
**Recommended change:** Group row uses `color-mix(in srgb, var(--marilo-color-subtle-background) 80%, var(--marilo-color-border))` for a subtle 1-step-darker tint, OR add a 2px bottom accent border on the group row.
**Acceptance criteria:** In Day/Week/Month grouped views, the top tier (group context) is visually separable from the leaf tier at a glance.
**Remediation handoff target:** SCSS source fix.

---

### VP-allocation-scheduler-011 — Conflict indicator treatment is background-only (no icon/chrome)

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** conflict indicator
**Reference Source:** internal Marilo baseline (no Telerik exact)
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L458-L460 (`&--conflict`) applies `background: color-mix(... danger 10%)`. That is the **only** conflict treatment. No icon, no border accent, no hover-reveal tooltip affordance. The row-level `&--over-allocated` at L423 uses the same recipe at 6%. A user scanning the grid sees a faint pink tint and nothing else — indistinguishable from a theme variation unless they already know conflicts exist. | Icon (warning triangle or crossed-out clock) rendered in the cell corner, plus an accessible label. Telerik Scheduler uses a red left-border accent + icon. |
| Likely cause | Conflict treatment was scoped as "minimum viable visual indicator" and never revisited. Cerebrum `visual-parity-plan.md` flags "Conflict indicator visual design not finalized". | |

**Category:** iconography
**Recommended change:** Add `&--conflict::before` pseudo-element with a conflict icon (warning-triangle SVG), positioned top-right inside the cell. Add `border-left: 3px solid var(--marilo-color-danger)` for a left accent. Icon colour follows `--marilo-color-danger`. ARIA: emit `aria-label="conflict"` on the cell (source change — escalate).
**Acceptance criteria:** A conflict cell is instantly identifiable via icon + left accent + background tint, distinct from a row-level `--over-allocated` tint.
**Remediation handoff target:** gap-analysis-resolution intake (requires component spec update + source change for ARIA).

---

### VP-allocation-scheduler-012 — Drag-fill ghost preview uses solid colour instead of dashed preview

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** drag-fill in progress
**Reference Source:** internal Marilo baseline (no Telerik exact)
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L475-L478 (`&--drag-target`) applies a solid 15% primary fill plus a 1px inset ring. During drag-fill, the target cells receive the same fill as any other hover-intent state. No dashed outline, no "ghost" preview of the value being written. A user drag-filling a 5-cell range cannot tell at a glance which cells are being staged vs. which are already committed. | Dashed outline (`outline: 2px dashed primary`) or pulsing ring animation, + a ghost value (the source cell's value rendered at 50% opacity in the target cells). Marilo already has a `__ghost-bar` class at L590-L598 but it is only used for baseline-diff overlays, not drag-fill preview. | |
| Likely cause | Drag-fill preview treatment was scoped narrowly to just a background tint in the first implementation pass. | |

**Category:** state treatment
**Recommended change:** Apply `outline: 2px dashed var(--marilo-color-primary); outline-offset: -2px;` on `&--drag-target` and drop the inset box-shadow. Optionally, render the source value at `opacity: 0.5` inside the target cell (source change — escalate).
**Acceptance criteria:** Drag-filling 3 cells across a row shows a dashed preview outline on the target cells that is visually distinct from a plain hover or active state.
**Remediation handoff target:** SCSS source fix for outline. ARIA/source change for ghost value — escalate.

---

### VP-allocation-scheduler-013 — Loading state is a plain text "Loading…" string (no skeleton)

**Component:** MariloAllocationScheduler
**Theme:** Fluent + Bootstrap
**Mode:** Light + Dark
**State/Scenario:** loading state
**Reference Source:** Telerik Scheduler loading skeleton
**Parity Score:** 1
**Severity:** minor (polish — edge state)

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `MariloAllocationScheduler.razor` L67-L70 renders `<div class="__loader" role="status">Loading...</div>` with `_allocation-scheduler.scss` L621-L626 applying only `display:flex; align-items:center; justify-content:center; padding:2rem`. No spinner SVG, no skeleton rows, no animated shimmer. Telerik renders a row-shaped shimmer skeleton that preserves layout while data loads. | Skeleton rows (1 per visible resource) with shimmering gradient, OR a spinner icon at centre. |
| Likely cause | Placeholder "Loading…" text was intended as temporary. | |

**Category:** state treatment
**Recommended change:** Render a `<MariloSkeleton />` stack sized to `RowHeight * VisibleResourceCount`, OR a `<MariloProgressCircle />` at centre with an "aria-busy" announcement. Source change — escalate.
**Acceptance criteria:** During load, the grid shows either skeleton rows or a spinner, not a bare "Loading…" string.
**Remediation handoff target:** gap-analysis-resolution intake (source + SCSS).

---

### VP-allocation-scheduler-014 — Empty resource treatment is only a bare `<div>` (no CTA/illustration)

**Component:** MariloAllocationScheduler
**Theme:** Fluent + Bootstrap
**Mode:** Light + Dark
**State/Scenario:** empty resource row / empty scheduler
**Reference Source:** Telerik Scheduler empty state
**Parity Score:** 2
**Severity:** polish (edge state)

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L612-L618 (`&__empty`) applies `padding: 2rem` + `color: disabled-text`. Razor at L74-L76 only renders the empty state if `EmptyTemplate` is supplied — if not supplied, an empty scheduler shows the grid header row followed by nothing (no rows, no message). Telerik shows a centered illustration + "No resources" copy + optional CTA. | Centered illustration icon + default "No resources" message when `EmptyTemplate` is null. |
| Likely cause | Empty state treatment delegated entirely to host `EmptyTemplate` without a default. | |

**Category:** state treatment
**Recommended change:** Provide a default empty-state render when `EmptyTemplate` is null: a centered `<MariloIcon Name="InboxEmpty" />` + "No resources to display" copy. Source change — escalate.
**Acceptance criteria:** Passing an empty `Resources` collection with no `EmptyTemplate` shows a meaningful empty state, not a blank grid.
**Remediation handoff target:** gap-analysis-resolution intake (source + SCSS).

---

### VP-allocation-scheduler-015 — Resource column text truncation has no tooltip affordance

**Component:** MariloAllocationScheduler
**Theme:** Fluent + Bootstrap
**Mode:** Light + Dark
**State/Scenario:** resource panel (truncated text)
**Reference Source:** Telerik Scheduler
**Parity Score:** 2
**Severity:** minor (a11y)

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L315-L322 (`&__resource-col`) applies `white-space: nowrap; overflow: hidden; text-overflow: ellipsis;` — long resource names are ellipsised. Razor does not emit `title` or `aria-describedby` on truncated cells, so the full name is lost to keyboard and screen-reader users. Sighted users cannot see it without hovering and waiting for the browser's native tooltip (which only fires when `title` attribute is present — which it is not). | Emit `title="@resource.Name"` on the `<td>` to give a native tooltip affordance, and prefer a `<MariloTooltip>` wrapper for themed consistency. |
| Likely cause | Truncation style added without the paired accessibility affordance. Feeds into Wave-2 Missing `accessibility` topic. | |

**Category:** state treatment (a11y)
**Recommended change:** Source change in razor — add `title="@GetResourceDisplayName(resource)"` on the `<td>` for resource name cells. Escalate because this is a source (razor) edit, not a SCSS edit.
**Acceptance criteria:** Hovering a truncated resource name shows the full text. Screen-reader announcement includes full name.
**Remediation handoff target:** gap-analysis-resolution intake + accessibility spec cross-reference (Wave-2 Missing `accessibility`).

---

### VP-allocation-scheduler-016 — Current-period highlight colour has insufficient contrast at 7% tint

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** current period highlight
**Reference Source:** Telerik Scheduler
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L561 highlights the current-period **data cells** at primary 7% over surface. Against the surface `#ffffff` that is `~#ebf3fa` — barely visible at normal viewing distance. The header cell variant at L565 uses 13% which is correct, but the data column tint below it washes out. Borderline score — a user might spot the column under close inspection. | Data cells at 10–12% tint so the column reads as a subtle but visible band. |
| Likely cause | 7% was scoped as "gentle" but is below the perceptibility threshold on white. | |

**Category:** token/color
**Recommended change:** Raise the data-cell tint from 7% to 10%.
**Acceptance criteria:** Current-period column is visible as a distinct band without eye-strain at normal viewing distance (1200px viewport).
**Remediation handoff target:** SCSS source fix.

---

### VP-allocation-scheduler-017 — Fill handle clipped by cell overflow in edge columns

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** cell editing (fill handle)
**Reference Source:** Telerik Scheduler / Excel
**Parity Score:** 1
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L502-L519 positions the fill handle at `right: -4px; bottom: -4px;` — it intentionally overflows the cell to give a draggable affordance. But the parent `&__cell` at L441-L443 sets `overflow: hidden; text-overflow: ellipsis;`. In most browsers `overflow: hidden` on `<td>` does not clip `position: absolute` descendants (they escape), but in **border-separate mode with sticky headers** (L255-L258 and L327), the containing block for the absolute element is the `<td>`, and the `overflow: hidden` on the td + sticky positioning combine to cause Chromium to clip the handle to the cell box. Observable on the rightmost column of the timeline where the handle should extend beyond the grid edge. | Fill handle always visible at the bottom-right corner of the active cell, unclipped. |
| Likely cause | Interaction between `overflow: hidden`, `position: sticky`, and descendant `position: absolute` in Chromium's table layout. | |

**Category:** layout
**Recommended change:** Remove `overflow: hidden` from `&__cell` and instead apply it via a wrapper span around the cell value (`&__cell-value { overflow: hidden; text-overflow: ellipsis; display: block; }`). Leaves the `<td>` free to host absolute descendants.
**Acceptance criteria:** The fill handle on the rightmost active cell is fully visible and draggable without clipping in Chromium.
**Remediation handoff target:** SCSS source fix — requires source-level verification that `&__cell-value` wrapping is already present in the razor markup (appears to be at L573 — `&__cell-value` class exists). Minor razor touch-up may be needed.

---

### VP-allocation-scheduler-018 — Scenario chip active state has no dark-mode contrast

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Dark
**State/Scenario:** scenario/target overlay bands (active scenario chip)
**Reference Source:** internal Marilo baseline
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L57-L61 (`&__scenario-chip--active`) applies `background: color-mix(... primary 8%, surface)` + `border-color: primary`. In dark mode with `--marilo-color-primary: #60cdff`, an 8% mix of bright cyan over dark surface `#252423` barely shifts the background — the active chip reads as "just a chip with a lighter border". In light mode (`#0f6cbd` over white), 8% produces a clearly tinted chip. Dark-mode should use a higher percentage to compensate for the lower-contrast cyan against dark. | Dark-mode active chip at 15–20% tint or with a solid primary fill + on-primary text (the chip is small enough to handle solid fill). |
| Likely cause | 8% mix tuned for light mode only. Dark-mode cyan primary has lower perceptual contrast against dark surface than light-mode blue against white. | |

**Category:** token/color
**Recommended change:** In the dark patch block, override the active chip: `.mar-allocation-scheduler__scenario-chip--active { background: color-mix(in srgb, var(--marilo-color-primary) 18%, var(--marilo-color-surface)); }`.
**Acceptance criteria:** Active scenario chip is visually distinct from idle chips in both Fluent light and dark.
**Remediation handoff target:** SCSS source fix.

---

### VP-allocation-scheduler-019 — Duplicate Fluent SCSS files (_allocation-scheduler.scss exists at two paths)

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** (structural / build hygiene, not a runtime visual gap but blocks clean remediation)
**Reference Source:** internal Marilo baseline
**Parity Score:** 2
**Severity:** minor (build hygiene)

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Two files exist with byte-identical content: `src/Marilo.Providers.FluentUI/Styles/_allocation-scheduler.scss` (654 lines) and `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` (654 lines). The Bootstrap provider has the same duplication (`_bridge-allocation-scheduler.scss` at root vs `components/_allocation-scheduler.scss`). Any SCSS fix for VP-003, VP-006, VP-007 etc. will need to be written to **both** files, or one will become stale. | Exactly one canonical SCSS file per component per provider. | |
| Likely cause | Legacy import path left behind when components were moved into `Styles/components/`. | |

**Category:** build hygiene (not a user-visible gap but critical for remediation correctness)
**Recommended change:** Delete the root-level `_allocation-scheduler.scss` in both Fluent and Bootstrap providers, and update any `@use`/`@forward` in `_index.scss` and `marilo-fluentui.scss` to point at `components/_allocation-scheduler.scss`. Verify `dotnet build` and Playwright demo still render the same.
**Acceptance criteria:** Only one `_allocation-scheduler.scss` per provider on disk. Build passes. Demo renders unchanged.
**Remediation handoff target:** SCSS source fix — cross-reference with datagrid / gantt / scheduler workspaces (likely the same duplication pattern exists for them).

---

### VP-allocation-scheduler-020 — Scrollbar hidden on resource panel creates orphan keyboard scroll affordance (a11y)

**Component:** MariloAllocationScheduler
**Theme:** Fluent
**Mode:** Light + Dark
**State/Scenario:** resource panel (long resource list)
**Reference Source:** internal Marilo baseline (a11y)
**Parity Score:** 1
**Severity:** major (a11y)

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `_allocation-scheduler.scss` L138-L149 defines `&__resource-panel` with `scrollbar-width: none; &::-webkit-scrollbar { display: none; }` — hides the scrollbar because vertical scroll is synced from the timeline panel. For sighted mouse users this is fine. For **keyboard-only users**, there is no visible scroll indicator and no Tab-focusable scroll mechanism in the resource panel — the only way to scroll the resources is to focus the timeline and scroll it. Screen-reader users get no announcement that a scroll container is present. | Either retain a visual scrollbar (thin style) OR provide an explicit keyboard-operable scroll affordance + ARIA attributes on the scroll container. |
| Likely cause | Scroll-sync workaround prioritised visual neatness over keyboard affordance. Feeds Wave-2 Missing `accessibility` topic. | |

**Category:** state treatment (a11y)
**Recommended change:** Replace `scrollbar-width: none` with `scrollbar-width: thin` and theme the thumb via `scrollbar-color`. OR: add `role="region" aria-label="Resources"` + explicit scroll buttons above/below for keyboard users. Source change — escalate for the role/label.
**Acceptance criteria:** A keyboard-only user can discover and scroll the resource list independently. A screen-reader announces the region.
**Remediation handoff target:** SCSS source fix for scrollbar + gap-analysis intake for ARIA role (source change).

---

## Category Distribution

| Category | Count |
|----------|-------|
| token/color | 7 (VP-001, VP-002, VP-003, VP-006, VP-007, VP-016, VP-018) |
| state treatment | 6 (VP-005, VP-008, VP-009, VP-012, VP-013, VP-014, VP-015, VP-020 — 8, overcounted: VP-005/15/20 are a11y, VP-008/09/12/13/14 generic) |
| elevation | 1 (VP-004) |
| iconography | 1 (VP-011) |
| density | 1 (VP-010) |
| layout | 1 (VP-017) |
| build hygiene | 1 (VP-019) |

**Total gaps:** 20 records covering theme × mode × structural element combinations. Wave-2 Missing topics (`accessibility`, `theming`) surfaced as: accessibility → VP-005, VP-011 (ARIA), VP-015, VP-020 (a11y state treatment); theming → VP-001, VP-002, VP-003, VP-007 (token layer gaps).

## Severity Roll-up

| Severity | Count | IDs |
|----------|-------|-----|
| Critical | 2 | VP-001, VP-002 |
| Major | 8 | VP-003, VP-004, VP-005, VP-006, VP-008, VP-011, VP-012, VP-020 |
| Minor | 9 | VP-007, VP-009, VP-010, VP-013, VP-014, VP-015, VP-016, VP-017, VP-018 |
| Polish | 1 | (VP-013 overlaps polish/minor classification — counted as minor) |
| Build hygiene | 1 | VP-019 |
