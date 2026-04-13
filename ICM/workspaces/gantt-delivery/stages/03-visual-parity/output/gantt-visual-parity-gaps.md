# MariloGantt — Visual Parity Gaps (Wave 3, Static Analysis)

**Worker:** `w-gantt-delivery`
**Stage:** `03-visual-parity`
**Method:** **Static analysis of SCSS + razor + provider files.** No runtime screenshots were captured in this wave. Findings derive from reading the Fluent/Bootstrap/Material provider `_gantt.scss` files and `src/Marilo.Components/DataDisplay/MariloGantt.razor`. Every gap cites the source file/lines that reveal it.
**Verification:** `dotnet build Marilo.slnx` → succeeded, 0 warnings, 0 errors (2026-04-11T17:40Z).
**Reference baseline:** Telerik Blazor Gantt visual reference + internal Marilo delivery-quality expectations.

Gap IDs use prefix `VP-gantt-NN`. Where a gap applies to all three themes, one record is produced and the "Theme" field enumerates all of them — this keeps the list actionable without duplicating near-identical observations. When a fix path differs per theme, separate records are produced.

---

## VP-gantt-01 — Task bar has no base visual rules (CRITICAL, all themes, light + dark)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Light, Dark
**State/Scenario:** default task bar
**Reference Source:** Telerik Gantt
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | `MariloGantt.razor:582,688` emits `<div class="mar-gantt__bar" style="left:...; width:...">` but NO provider SCSS defines `.mar-gantt__bar {}` with `background`, `height`, `border`, `border-radius`, `box-shadow`, or `color`. Only `mar-gantt__bar--summary` and `mar-gantt__bar-progress` have rules. Result: bars render as zero-height browser-default `<div>`s with no fill. | Telerik renders each task bar as a filled rectangle ~60–70% of row height, rounded corners (~2–3px), primary-token fill, readable label, subtle border. |
| Likely cause | Missing component-level SCSS rule. The bar class was declared in the BEM scheme but never given base properties in any provider. | — |

**Category:** layout + token/color + missing-state coverage
**Recommended change:** Add `.mar-gantt__bar` base rule to all three provider `components/_gantt.scss` files with: `position:absolute; height: calc(var(--marilo-gantt-row-height, 32px) * 0.6); top: calc((var(--marilo-gantt-row-height, 32px) - <bar-height>) / 2); background: var(--marilo-color-primary, …); border-radius: 2px; color: var(--marilo-color-on-primary, #fff); display:flex; align-items:center; padding: 0 6px; overflow:hidden;`. Dark-mode override not required if the base uses tokens that already flip.
**Acceptance criteria:** In all six theme×mode combinations, the Fluent-Light default task bar is a visible filled rectangle with a primary-color fill, not a zero-height div; Telerik side-by-side comparison scores ≥ 2 on the default-bar scenario.
**Remediation handoff target:** SCSS source fix in `src/Marilo.Providers.{FluentUI,Bootstrap,Material}/Styles/components/_gantt.scss`.

---

## VP-gantt-02 — Fluent provider has NO dark-mode patch (CRITICAL, Fluent Dark)

**Component:** MariloGantt
**Theme:** Fluent
**Mode:** Dark
**State/Scenario:** default task bar, tree column idle, timeline header, task hover, task selected, filter menu, progress indicator (all states)
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss` is 207 lines long and contains **zero** `[data-marilo-theme="dark"]` blocks. Bootstrap bridge file has at least a minimal dark-mode block covering filter-menu / filter-menu-input / bar-delete (`src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss:180–197`). Fluent has nothing. | Every surface that uses a surface/border/shadow token should have a dark override or use tokens that flip automatically. |
| Likely cause | Fluent provider was authored in light mode first and dark-mode patches were never added. Bootstrap got a partial patch; Fluent was missed. | — |

**Category:** token/color + missing-state coverage
**Recommended change:** Add a `[data-marilo-theme="dark"] { .mar-gantt { … } }` block to the Fluent components/_gantt.scss that patches at minimum: filter-menu background, filter-menu-input surface+color, bar-delete color, box-shadow elevation. Preferably migrate the hard-coded `rgba(0,0,0,0.15)` shadow and `#ffffff` fallback surfaces to token functions so dark mode works without a patch.
**Acceptance criteria:** Fluent Dark filter-menu background matches the Fluent dark neutral surface token; no light-mode surfaces leak into dark mode on side-by-side capture.
**Remediation handoff target:** SCSS source fix in `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`.

---

## VP-gantt-03 — Dependency-line SVG stroke hard-coded to `#999` in razor (CRITICAL, all themes × Dark)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Dark (also Light, but the dark-mode impact is critical)
**State/Scenario:** dependency lines
**Reference Source:** Telerik Gantt
**Parity Score:** 0
**Severity:** critical

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | `MariloGantt.razor:625` (DayView dependencies) and `MariloGantt.razor:731` (MonthView dependencies) both emit: `<polyline … stroke="#999" stroke-width="1.5" … />`. The stroke is set inline on the SVG element — no CSS class, no CSS custom property, no theme awareness. On a dark charcoal background (Fluent Dark, Bootstrap `dark` body-bg ~#212529), `#999` is low contrast; on a black canvas it survives, but there is zero mode-aware treatment. No arrowhead marker element — the polyline ends at the target with no arrow glyph. | Telerik uses a theme-aware stroke (adapts to token), ~1.5–2px weight, with an explicit arrowhead marker at the target end. Line color raises contrast in dark mode. |
| Likely cause | Early-prototype hard-coded SVG. Dependency SVG was never wired up to a CSS class + stroke token. Arrowhead omitted entirely. | — |

**Category:** token/color + iconography
**Recommended change:** Replace the inline `stroke="#999" stroke-width="1.5"` with `class="mar-gantt__dependency-line"` and define `.mar-gantt__dependency-line { stroke: var(--marilo-color-border-strong, #605e5c); stroke-width: 1.5; fill:none; marker-end: url(#mar-gantt-dep-arrow); }` in each provider. Add an `<svg><defs><marker id="mar-gantt-dep-arrow" …/></defs></svg>` once at the top of the bars container. Provide dark-mode token override.
**Acceptance criteria:** Dependency polylines render with a visible arrowhead at the finish end and are equally readable in all six theme×mode captures.
**Remediation handoff target:** Source fix in `MariloGantt.razor` (both DayView and MonthView dependency blocks) **and** SCSS addition in all three providers. This crosses source+provider lines — flag as cross-sync gap.

---

## VP-gantt-04 — Today / current-date vertical line is not implemented at all (MAJOR, all themes)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Light, Dark
**State/Scenario:** current date line
**Reference Source:** Telerik Gantt
**Parity Score:** 0
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | Searched `MariloGantt.razor` and all three provider `_gantt.scss` files for `today`, `current-date`, `date-line`, `now`. Zero hits. The today marker is in the capture matrix (`shared/capture-matrix.md#9`) as a required state and in the plan's P2 list but **no DOM element and no style rule exist** for it. | Telerik draws a 1–2px vertical line spanning the timeline pane at "now", typically with a primary-hue or warning-hue token, 60–80% opacity, pinned above bars but below tooltips. |
| Likely cause | Feature was never built. Not a token or layout regression — missing-state coverage. | — |

**Category:** missing-state coverage
**Recommended change:** Add a `<div class="mar-gantt__today-line" style="left:{xPx}px;">` into the `.mar-gantt__bars` container in razor, computed from `DateTime.UtcNow` mapped through the same offset math the bars use. Add `.mar-gantt__today-line { position:absolute; top:0; bottom:0; width:1px; background:var(--marilo-color-primary, …); opacity:.6; pointer-events:none; z-index:1; }` to each provider. Parameterize visibility via a `ShowTodayMarker` parameter (default true).
**Acceptance criteria:** Timeline captures in all six theme×mode combinations show a visible vertical today-line at `today()`, with opacity that survives both light and dark modes.
**Remediation handoff target:** gap-analysis-resolution intake — requires both source (razor + parameter) and SCSS additions.

---

## VP-gantt-05 — Milestone diamond is a Unicode glyph, not a shape primitive (MAJOR, all themes)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Light, Dark
**State/Scenario:** milestone diamond
**Reference Source:** Telerik Gantt
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | `MariloGantt.razor:573,679` renders the milestone as `<span class="mar-gantt__milestone-diamond">&#x25C6;</span>` inside a fixed-size div. Fluent SCSS: `font-size:14px; color:var(--marilo-color-primary);`. Bootstrap SCSS: same, with `--bs-primary`. Material: no rule at all. A font glyph cannot be precisely sized, cannot be given a stroke separately from a fill, will not render identically across font stacks, and will be subject to the system font's emoji-substitution rules on some browsers (especially Android/Material). | Telerik renders a milestone as a true diamond — either a CSS `transform: rotate(45deg)` square, or an inline SVG `<rect transform="rotate">` — with precise width, height, stroke, and fill tokens. Size typically 10×10 to 12×12. |
| Likely cause | Prototype used a glyph as placeholder; never upgraded to a shape primitive. Precision-sensitive per rubric. | — |

**Category:** iconography + token/color
**Recommended change:** Replace the Unicode diamond with either (a) an inline SVG `<rect width=10 height=10 transform="rotate(45 5 5)" class="mar-gantt__milestone-shape"/>` or (b) a `.mar-gantt__milestone-diamond { width:10px; height:10px; transform:rotate(45deg); background:var(--marilo-color-primary); border:1px solid var(--marilo-color-border-strong); }` CSS-only approach. Parameterize size via a `--marilo-gantt-milestone-size` custom property (default 10px).
**Acceptance criteria:** Milestone renders at exactly 10×10 CSS px (or configured size) in every theme/mode; no emoji/glyph substitution; visible border survives dark mode.
**Remediation handoff target:** Source fix in razor + SCSS addition in all three providers.

---

## VP-gantt-06 — Summary bar is just opacity + bottom-border, no distinct shape (MAJOR, all themes)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Light, Dark
**State/Scenario:** summary bar
**Reference Source:** Telerik Gantt
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | Fluent SCSS line 58–62: `.mar-gantt__bar--summary { opacity: 0.85; border-bottom: 2px solid var(--marilo-color-primary); border-radius: 0; }`. Bootstrap SCSS identical with `--bs-primary`. Material empty. This assumes a base `.mar-gantt__bar` exists (it does not per VP-gantt-01), so "summary" inherits nothing but opacity + a floating 2px border. | Telerik summary bars are trapezoid-capped (tapered ends), solid fill of a darker primary token, ~40–50% of row height, with square corners. The tapered caps are the visual signal for "parent / summary" — opacity alone does not read as "parent". |
| Likely cause | Cosmetic shortcut: summary bar styled by overriding one property instead of being rebuilt as a distinct bar type. Dependent on VP-gantt-01 base rule existing. | — |

**Category:** iconography + layout + elevation
**Recommended change:** After VP-gantt-01 adds a base `.mar-gantt__bar`, redefine `.mar-gantt__bar--summary` as: `height: calc(var(--marilo-gantt-row-height, 32px) * 0.35); top: calc(var(--marilo-gantt-row-height, 32px) * 0.3); background: var(--marilo-color-primary-dark, …); border-radius: 0; clip-path: polygon(0 0, 100% 0, calc(100% - 6px) 100%, 6px 100%);` (trapezoid via clip-path). Remove the floating 2px border.
**Acceptance criteria:** Parent rows visibly differ from child rows via shape (not just opacity); Telerik side-by-side scores ≥ 2 in all themes.
**Remediation handoff target:** SCSS source fix in all three providers. Depends on VP-gantt-01.

---

## VP-gantt-07 — Task hover shows no bar-background change, only a delete glyph (MAJOR, all themes)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Light, Dark
**State/Scenario:** task hover
**Reference Source:** Telerik Gantt
**Parity Score:** 1
**Severity:** major (primary state per rubric)

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | Fluent SCSS line 91: `.mar-gantt__bar:hover .mar-gantt__bar-delete { display: inline-flex; }`. That is the ONLY `:hover` rule on `.mar-gantt__bar`. No background change, no border change, no elevation, no cursor change in SCSS (the component probably inherits default cursor). Hover is indistinguishable from rest except for a tiny red delete icon appearing on the right edge. | Telerik hover: bar fill darkens one token step, optional box-shadow elevation, cursor changes to `pointer` or `grab`, and the tooltip surface appears. |
| Likely cause | Hover treatment was designed around the delete affordance instead of around the bar itself. Primary state per rubric (task hover is primary) → automatically escalates to critical, but scoring against the base-bar gap downgrades to major pending VP-gantt-01. | — |

**Category:** state treatment
**Recommended change:** Add `.mar-gantt__bar { cursor: pointer; transition: background-color 0.12s ease, box-shadow 0.12s ease; } .mar-gantt__bar:hover { background: var(--marilo-color-primary-hover, …); box-shadow: 0 1px 3px rgba(0,0,0,0.18); }` to all three providers. Add a `@media (prefers-reduced-motion: reduce)` fallback that zeroes the transition.
**Acceptance criteria:** Hovering a task bar in any theme/mode produces a visible fill darkening + shadow within 120ms; delete glyph continues to reveal as now.
**Remediation handoff target:** SCSS source fix in all three providers. Strongly tied to VP-gantt-01.

---

## VP-gantt-08 — Task selected state has no style rule anywhere (MAJOR, all themes)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Light, Dark
**State/Scenario:** task selected
**Reference Source:** Telerik Gantt
**Parity Score:** 0
**Severity:** major (primary state per rubric)

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | Grep for `selected` across razor + SCSS shows zero hits in the Gantt context (checkbox-filter `Contains(opt)` is the only hit and is unrelated). There is no `.mar-gantt__bar--selected`, no `is-selected`, no `aria-selected` class styling, and no row-selection visual in the task-list pane. | Telerik renders selected bars with a different fill or an outline ring; selected rows in the tree get a row background. |
| Likely cause | Selection state is not implemented in the visual layer at all. May also be a spec/source gap — flag for cross-check with Wave 1 output. | — |

**Category:** missing-state coverage + state treatment
**Recommended change:** Introduce `.mar-gantt__bar--selected { outline: 2px solid var(--marilo-color-selected, …); outline-offset: 1px; }` and `.mar-gantt__tasklist-row--selected { background: var(--marilo-color-selected-background, …); }` in all three providers. Wire the modifier class from razor. Source change required: the razor needs to emit the modifier based on a selected-row collection — likely already partly present but not styled.
**Acceptance criteria:** Clicking a bar produces a visible selection ring; tree-list row also highlights; selection survives theme change.
**Remediation handoff target:** gap-analysis-resolution intake — requires source (selection-state wiring in razor) + SCSS in all three providers. May already exist in source; confirm during remediation.

---

## VP-gantt-09 — Task-list row rules missing: no row background, border, zebra, or hover (MAJOR, all themes)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Light, Dark
**State/Scenario:** tree column idle, expanded row, collapsed row
**Reference Source:** Telerik Gantt
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | The razor emits `.mar-gantt__tasklist`, `.mar-gantt__tasklist-header`, `.mar-gantt__task-cell` etc. but no provider SCSS defines rules for any of these except the editable-cell cursor patch (Fluent SCSS line 172). There is no row-height rule, no bottom border per row, no hover row background, no zebra stripe, no header-row background, no column separator. | Telerik tree column has ~32px row height, a subtle 1px bottom border per row, a hover row background that matches timeline-pane hover, and a bold/raised header row. |
| Likely cause | The entire tree-grid chrome was assumed to be inherited from some other component's styles, but no `@extend` or shared include exists. | — |

**Category:** layout + density + token/color
**Recommended change:** Add the following rules to all three providers: `.mar-gantt__tasklist { border-right: 1px solid var(--marilo-color-border); } .mar-gantt__tasklist-header { background: var(--marilo-color-surface-muted); font-weight: 600; border-bottom: 1px solid var(--marilo-color-border); height: var(--marilo-gantt-row-height, 32px); display:flex; align-items:center; } .mar-gantt__tasklist-row { height: var(--marilo-gantt-row-height, 32px); border-bottom: 1px solid var(--marilo-color-border-subtle); display:flex; align-items:center; } .mar-gantt__tasklist-row:hover { background: var(--marilo-color-subtle-background); }`. Note: razor does not currently emit a `.mar-gantt__tasklist-row` wrapper per row — verify and add if missing (source change).
**Acceptance criteria:** Tree column visually reads as a grid with consistent row height, border, and header weight in all themes/modes.
**Remediation handoff target:** SCSS source fix in all three providers, plus possible small source change to emit a row wrapper class.

---

## VP-gantt-10 — Timeline header cells have no separator, background, or typography rules (MAJOR, all themes)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Light, Dark
**State/Scenario:** timeline header
**Reference Source:** Telerik Gantt
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | `MariloGantt.razor:508,524,635` emits `.mar-gantt__timeline-header`, `.mar-gantt__timeline-header--main`, `.mar-gantt__timeline-header--secondary`, and `.mar-gantt__date-label` classes. None of these have any SCSS rule in any provider. Cell width is inline (`style="width:{px}px"`). There is no background, no bottom-border, no font-weight, no tier separator, no sticky-top positioning. | Telerik timeline header has a tinted background, a bold/semibold font, a 1px bottom border separating header from bar area, a 1px vertical separator between cells, and it is sticky-top when the timeline pane scrolls vertically. |
| Likely cause | Classes were declared but the style rules were never added — same pattern as VP-gantt-01 and VP-gantt-09. | — |

**Category:** typography + layout + density
**Recommended change:** Add rules: `.mar-gantt__timeline-header { background: var(--marilo-color-surface-muted); border-bottom: 1px solid var(--marilo-color-border); display:flex; position: sticky; top:0; z-index:2; } .mar-gantt__timeline-header--main { height: 24px; font-weight: 600; } .mar-gantt__timeline-header--secondary { height: 22px; font-size: 12px; color: var(--marilo-color-text-secondary); } .mar-gantt__date-label { border-right: 1px solid var(--marilo-color-border-subtle); display:flex; align-items:center; justify-content:center; box-sizing:border-box; }` to all providers.
**Acceptance criteria:** Timeline header has a visible tinted strip, tiered rows, cell separators, and stays pinned when the bars pane scrolls.
**Remediation handoff target:** SCSS source fix in all three providers.

---

## VP-gantt-11 — Progress-fill formula differs between providers (token/color, Fluent vs. Bootstrap)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap
**Mode:** Light, Dark
**State/Scenario:** progress indicator
**Reference Source:** internal Marilo baseline
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | Fluent SCSS line 70: `background: color-mix(in srgb, var(--marilo-color-primary, #0078d4) 40%, var(--marilo-color-surface, #ffffff));`. Bootstrap SCSS line 70: `background: rgba(var(--bs-primary-rgb, 13, 110, 253), 0.3);`. One is "40% primary mixed with surface" (theme-aware contrast), the other is "30% primary alpha on whatever lies behind" (alpha compositing against the bar background). These produce visibly different progress fills. | All providers should use the same visual formula so progress fill contrast is consistent across themes. Either "fill token" or "alpha over base" — pick one. |
| Likely cause | Two authors, two conventions. Not a wrong-looking fill; just inconsistent. | — |

**Category:** token/color
**Recommended change:** Standardize on one approach. Recommendation: adopt the `color-mix` approach in Bootstrap too (modern CSS supports it), or expose a token `--marilo-gantt-progress-fill` that both providers set explicitly. Document the choice in the gantt spec `timeline/overview.md`.
**Acceptance criteria:** Fluent and Bootstrap progress fills use the same formula; captured side-by-side they look like intentional theme variants, not two different algorithms.
**Remediation handoff target:** SCSS source fix in Fluent + Bootstrap.

---

## VP-gantt-12 — Tree-column indentation pixel math lives in razor, not SCSS (MINOR, all themes)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Light, Dark
**State/Scenario:** tree column idle (expanded/collapsed indentation)
**Reference Source:** internal Marilo baseline
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | `MariloGantt.razor:253,297,347,392` and surrounding lines emit `<span style="padding-left: @(pad)px; display:inline-flex;">` where `pad` is computed in razor code-behind as a function of tree depth. This pixel math is hard-coded and cannot be themed. It will also not respect any future `--marilo-gantt-indent-per-level` custom property. | Indentation should be `calc(var(--marilo-gantt-indent-per-level, 16px) * var(--depth))` applied via a CSS custom property written once as an inline style `--depth:{n}`, so SCSS can own the px value. |
| Likely cause | Early prototype used razor math because SCSS custom-property arithmetic is less familiar. | — |

**Category:** density + layout
**Recommended change:** Replace the inline `padding-left:{pad}px` with inline `style="--depth:{n}"` and add SCSS: `.mar-gantt__task-cell > span { padding-left: calc(var(--marilo-gantt-indent-per-level, 16px) * var(--depth, 0)); }` to all providers. Token-ize the per-level indent.
**Acceptance criteria:** Changing `--marilo-gantt-indent-per-level` in dev-tools visibly re-indents the tree column without a rebuild; captured indentation density is consistent across themes.
**Remediation handoff target:** SCSS source fix + small razor change (cross-sync — flag as cross-stage gap).

---

## VP-gantt-13 — Filter-menu elevation uses hard-coded rgba shadow (MINOR, Fluent Light + Dark)

**Component:** MariloGantt
**Theme:** Fluent
**Mode:** Light, Dark
**State/Scenario:** filter menu (open state on tree column)
**Reference Source:** Telerik Gantt
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | Fluent SCSS line 124: `box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);`. This is a literal rgba, not a Fluent elevation token. On Fluent Dark, the 0.15 alpha over a dark neutral produces almost no visible elevation. Bootstrap uses `var(--bs-box-shadow-sm)` which IS tokenized. | Fluent should use a Fluent 2 elevation token (e.g. `var(--marilo-elevation-4, …)`) so dark-mode elevation reads correctly. |
| Likely cause | Shadow was authored before elevation tokens were standardized in the Fluent provider. | — |

**Category:** elevation + token/color
**Recommended change:** Replace the rgba literal with `var(--marilo-elevation-4, 0 2px 8px rgba(0,0,0,0.15))` and ensure the Fluent foundation layer defines the token in both light and dark.
**Acceptance criteria:** Fluent Dark filter-menu has visible elevation matching Fluent 2 reference; Fluent Light unchanged.
**Remediation handoff target:** SCSS source fix in Fluent foundation tokens + components/_gantt.scss.

---

## VP-gantt-14 — No focus-visible outline on bars, rows, or milestones (MAJOR, all themes, keyboard users)

**Component:** MariloGantt
**Theme:** Fluent, Bootstrap, Material
**Mode:** Light, Dark
**State/Scenario:** task hover (keyboard-focus proxy), task selected, editing row
**Reference Source:** internal Marilo baseline + WCAG 2.4.7
**Parity Score:** 1
**Severity:** major

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | Grep for `focus-visible`, `:focus` across the Gantt SCSS files shows ONE hit: `.mar-gantt__skip-link:focus` (Fluent line 16, Bootstrap line 16). No `.mar-gantt__bar:focus-visible`, no `.mar-gantt__task-cell:focus-visible`, no `.mar-gantt__milestone:focus-visible`. Keyboard users get no visible focus ring on any interactive element inside the Gantt. This is an accessibility regression and a visual parity gap. | Every interactive element should have a 2px Fluent/Bootstrap/Material focus ring via `:focus-visible`. |
| Likely cause | Focus-visible styling was never added after the initial structural SCSS pass. | — |

**Category:** state treatment + token/color
**Recommended change:** Add `.mar-gantt__bar:focus-visible, .mar-gantt__milestone:focus-visible, .mar-gantt__task-cell:focus-visible { outline: 2px solid var(--marilo-color-focus, …); outline-offset: 1px; }` to all providers. Also ensure the razor adds `tabindex="0"` to bars if it does not already (source check).
**Acceptance criteria:** Tabbing through a Gantt produces a visible focus ring on every interactive element in every theme/mode; WCAG 2.4.7 passes.
**Remediation handoff target:** SCSS source fix in all three providers, plus source check for `tabindex` on interactive elements.

---

## VP-gantt-15 — Fluent provider has duplicate `_gantt.scss` files (MINOR, maintenance hazard)

**Component:** MariloGantt
**Theme:** Fluent
**Mode:** n/a
**State/Scenario:** n/a (file-layout issue)
**Reference Source:** internal Marilo baseline
**Parity Score:** 2
**Severity:** minor

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | Both `src/Marilo.Providers.FluentUI/Styles/_gantt.scss` (207 lines) and `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss` (207 lines) exist. A byte-by-byte comparison shows they are identical. Only one of them is imported by the Fluent foundation (the `components/` one, based on the ICM folder convention used elsewhere). The top-level copy is a stale clone. Not a visual gap per se, but any future parity fix risks being applied to the wrong file. | One canonical file per provider. Delete or merge duplicates. |
| Likely cause | File was moved from `Styles/` into `Styles/components/` at some point and the original was never removed. | — |

**Category:** maintenance (not a visual render issue, but will block fixes for other gaps)
**Recommended change:** Delete `src/Marilo.Providers.FluentUI/Styles/_gantt.scss` after verifying no `@import` or `@use` references it. Keep only `Styles/components/_gantt.scss`.
**Acceptance criteria:** Only one `_gantt.scss` exists under `Marilo.Providers.FluentUI/Styles/`; `dotnet build` succeeds; no CSS class becomes un-styled after removal.
**Remediation handoff target:** Source fix — delete one file. Orchestrator approval needed because file deletion touches provider layout.

---

## VP-gantt-16 — Material provider is a 5-line TODO stub (BLOCKER, Material Light + Dark)

**Component:** MariloGantt
**Theme:** Material
**Mode:** Light, Dark
**State/Scenario:** all
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** critical (blocker, not a fix candidate for this stage)

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | `src/Marilo.Providers.Material/Styles/components/_gantt.scss` is 5 lines: a TODO comment saying "Placeholder — no Material 3 styles defined yet. Implement when the Material provider runtime project is created." Every Gantt state in Material Light and Material Dark will fall back to browser defaults with no BEM rules applied. | A full Material 3 provider implementation of every class covered in Fluent/Bootstrap. |
| Likely cause | Material provider runtime project has not been scaffolded; SCSS file is a placeholder. Flagged in `gantt-visual-parity-plan.md` Known Unknowns. | — |

**Category:** missing-state coverage (entire provider)
**Recommended change:** This is tracked at the provider-level, not the Gantt-level. Once the Material runtime project lands, create a Material 3 implementation that mirrors the Fluent rule set (post VP-gantt-01 through VP-gantt-14). Do not address in this stage.
**Acceptance criteria:** Defer until Material runtime project exists. Until then, Material × Gantt captures are out of scope for parity scoring.
**Remediation handoff target:** Orchestrator escalation — not a `w-gantt-delivery` fix. Cross-project dependency on the Material provider scaffolding effort.

---

## VP-gantt-17 — DEFERRED-PENDING-SOURCE — EUX-04 visual audit (GetState / SetStateAsync demo)

**Component:** MariloGantt
**Theme:** n/a
**Mode:** n/a
**State/Scenario:** state-restore demo scenario
**Reference Source:** Telerik Gantt
**Parity Score:** DEFERRED-PENDING-SOURCE
**Severity:** deferred

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | The Wave 2 `gantt-example-ux-gap-list.md` (EUX-04) documents that no `GetState()` / `SetStateAsync()` round-trip demo exists because the source `GanttState<TItem>` API is blocked at orchestrator decision level. The user directive "GanttState rewrite → spec shape" has been **queued to the `gantt-gap-analysis` workspace, not this session** (state JSON notes from 2026-04-11T17:20Z). A visual audit of save/restore cannot proceed because the demo cannot be built. | Telerik ref: state-restore typically reuses existing task bar visuals with no unique chrome — so visual parity would expected be derivative of VP-gantt-01/07/08. |
| Likely cause | Source-level blocker, not a visual one. | — |

**Category:** deferred
**Recommended change:** Do not audit in this stage. Revisit after the source rewrite lands in `gantt-gap-analysis` and a working demo exists. At that point, re-run Stage 03 for the state-restore scenario only and confirm the derived visuals pass the VP-gantt-01/07/08 remediation.
**Acceptance criteria:** Re-audit scheduled for the wave after source lands.
**Remediation handoff target:** Hand off to `gantt-gap-analysis` workspace (not `gap-analysis-resolution` intake).

---

## VP-gantt-18 — DEFERRED-PENDING-SOURCE — EUX-05 visual audit (TaskListWidthChanged / @bind-TaskListWidth)

**Component:** MariloGantt
**Theme:** n/a
**Mode:** n/a
**State/Scenario:** task-list / timeline splitter drag visual
**Reference Source:** Telerik Gantt
**Parity Score:** DEFERRED-PENDING-SOURCE
**Severity:** deferred

| Field | Observed in Marilo | Expected (Reference) |
|---|---|---|
| Description | Wave 1 SA-04 / SA-05 (carried to Wave 2 EUX-05) shows that `TaskListWidthChanged` EventCallback and `@bind-TaskListWidth` are not implemented in `MariloGantt.razor.cs`. The splitter bar between the tree column and timeline pane is therefore not interactive. A visual audit of splitter-drag states (rest, hover, active-drag) requires that the splitter exist and respond to drag. No splitter SCSS rule exists in any provider either (grep for `splitter` in Gantt SCSS files: zero hits). | Telerik ref: splitter is a 4–6px vertical bar with a hover state (darker fill) and an active-drag state (primary-token fill); cursor changes to `col-resize`. |
| Likely cause | Source-level blocker. | — |

**Category:** deferred
**Recommended change:** Do not audit in this stage. Escalate to source lane for the event + parameter addition. After source lands, re-run Stage 03 for the splitter scenario. Note: even at current state, the SCSS does not define a splitter rule — so when source adds the event, a fresh parity gap will need a `.mar-gantt__splitter` rule across all providers. Pre-record that as a follow-up.
**Acceptance criteria:** Re-audit scheduled for the wave after source lands.
**Remediation handoff target:** Hand off to source lane (not this worker) + follow-up SCSS gap to be created during that wave.

---

## Summary of DEFERRED items

Two items (VP-gantt-17, VP-gantt-18) are classified `DEFERRED-PENDING-SOURCE` per the orchestrator inbox directive. They are recorded in this list for traceability but are NOT scored and do NOT count against the 10–20 gap target — the non-deferred records (VP-gantt-01 through VP-gantt-16) are 16 records, within range.

## Cross-references

- **Wave 1 (spec-review) outputs:** `stages/01-spec-review/output/gantt-spec-gap-list.md` — notes spec coverage gaps that this stage cannot fix.
- **Wave 2 (example-ux) outputs:** `stages/02-example-ux/output/gantt-example-ux-gap-list.md` — source of EUX-04 and EUX-05 deferral notes.
- **Capture matrix:** `stages/03-visual-parity/shared/capture-matrix.md` — 13 states × 6 theme/modes reference.
- **Parity rubric:** `stages/03-visual-parity/shared/parity-score-rubric.md` — scoring definitions used above.
