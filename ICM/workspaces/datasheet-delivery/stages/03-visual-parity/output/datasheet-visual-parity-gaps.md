# DataSheet Visual Parity Gaps

Running ledger of visual-parity findings for `MariloDataSheet` against the
internal Marilo delivery-quality baseline. Each dated section is the output of
one static-analysis pass. Browser captures deferred pending Playwright harness
decisions — this wave scores from source, SCSS, provider code, and demo pages.

---

## 2026-04-11 orchestrator wave 3 (w-datasheet-delivery, static-analysis)

**Worker:** `w-datasheet-delivery`
**Session:** marilo-grid-pipeline-2026-04-11-1200
**Stage:** 03-visual-parity
**Method:** Static analysis — no browser capture. Sources read: `MariloDataSheet.razor`, `MariloDataSheet.Rendering.cs`, `FluentUICssProvider.cs`, `MaterialCssProvider.cs`, `BootstrapCssProvider.cs`, all three provider `Styles/components/*.scss` trees, the four DataSheet demo pages, `docs/component-specs/datasheet/theming-and-css-provider.md`.

### Scope and method

Target state inventory is the 9 scenarios from
`stages/03-visual-parity/shared/capture-matrix.md`: cell grid default, header
row, row hover, cell selection, row selection, focused cell, editable cell,
validation error, frozen column. Each state is scored on the 0–3 rubric from
`shared/parity-score-rubric.md` against Marilo's internal delivery-quality
baseline (Excel / Google Sheets visual grammar as informal spreadsheet
reference). Scope is **Fluent × (Light/Dark)**, **Bootstrap × (Light/Dark)**,
**Material × (Light/Dark)** — 6 theme/mode combinations, 54 intended capture
points, of which several are DEFERRED for reasons listed below.

Deferrals that MUST NOT be treated as failures:

- **EU-06 theming side-by-side scenarios** → `DEFERRED-PENDING-ARCHITECTURE`
  (`datasheet-theming-architecture` user-decision OPEN; see Wave 2 gap list and
  Wave 1 SRC-02). No theming demo can be scored until the orchestrator decides
  whether to extend `IMariloCssProvider` or narrow the spec.
- **EU-07 rectangular range-selection scenarios** → `DEFERRED-PENDING-SOURCE`
  (source model `DataSheetSelection<TItem>` does not exist — Wave 1 `V03`
  carried).
- **10k-row virtualization scenarios (EU-01 upper threshold)** →
  `DEFERRED-PENDING-SCOPE` (`datasheet-10k-rows` user-decision OPEN). The
  virtualization scenario capture capped at ≤500 rows until the demo-dataset
  cap is decided.

### Foundational finding (context for every gap below)

Static analysis of all three provider SCSS trees
(`src/Marilo.Providers.FluentUI/Styles/components/*`,
`src/Marilo.Providers.Material/Styles/components/*`,
`src/Marilo.Providers.Bootstrap/Styles/components/*` + all `_bridge-*.scss`)
shows **zero SCSS rules targeting any `mar-datasheet*` BEM class in any
provider**. Evidence:

- `ripgrep mar-datasheet src/Marilo.Providers.FluentUI` → 20 hits, all in
  `FluentUICssProvider.cs` (class-name emission), 0 SCSS hits.
- `ripgrep mar-datasheet src/Marilo.Providers.Material/Styles` → 0 hits.
- `ripgrep mar-datasheet src/Marilo.Providers.Bootstrap/Styles` → 0 hits.
- `find src -iname "_data-sheet*.scss" -o -iname "_datasheet*.scss"` → 0 files.

At the same time, `MariloDataSheet.razor` (lines 21, 30, 36, 42, 48, 64, 79,
82, 85, 89, 141, 173) and `MariloDataSheet.Rendering.cs` (9 hits) bake hard-
coded `mar-datasheet__*` classes directly into the Razor/RenderTreeBuilder
output. The provider methods `DataSheetClass`, `DataSheetCellClass`,
`DataSheetHeaderCellClass`, `DataSheetRowClass`, `DataSheetToolbarClass`,
`DataSheetBulkBarClass`, `DataSheetSaveFooterClass` in
`FluentUICssProvider.cs:544-588` emit class names but the provider ships no
corresponding styles.

**Consequence:** DataSheet in every provider/mode currently renders with
browser-default `<table>` presentation — no grid lines, no header background,
no selection highlight, no cell-state tinting, no density control, no frozen-
column separator, no focus ring, no dark-mode overrides. Every primary state
scores **0 (materially different)** against any internal delivery-quality
baseline for a spreadsheet component in every theme/mode.

This is the dominant parity issue and it is structural, not a token nudge.
Individual gap records below therefore track **the state** (so that the
remediation workstream has discrete scope) but most of them collapse into one
root cause: missing DataSheet SCSS. The gap records also capture the
secondary issues (hard-coded BEM vs. provider delegation, SR-only inline
styles, etc.) that must be handled separately.

Gap `VP-datasheet-01` is the umbrella structural record that all Fluent /
Bootstrap / Material theme/mode gaps ultimately depend on. Per-theme records
are kept so each provider owns its own SCSS lane at remediation time.

### Gap records

---

**ID:** `VP-datasheet-01`
**Component:** MariloDataSheet
**Theme:** Fluent / Bootstrap / Material (all)
**Mode:** Light / Dark (all)
**State/Scenario:** Cell grid default + header row (structural baseline)
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Razor emits `<table>` inside `class="mar-datasheet"` root with `mar-datasheet__header-cell`, `mar-datasheet__cell`, `mar-datasheet__row` classes, but no provider SCSS defines any `mar-datasheet*` selector. Browser-default table renders with no grid lines, no header fill, default serif/system font mix, no cell padding, no min-row-height. | Contiguous grid with visible 1px cell borders that match the provider's neutral-divider token, tinted header row (neutral-surface-raised), uniform row height (32–36px in Fluent/Bootstrap; 36–40px in Material per density guidance), monospace-aligned numeric columns, and theme-appropriate font family from `--marilo-font-*` tokens. |
| Likely cause | Missing-state coverage: `_data-sheet.scss` has never been authored in any provider. Hard-coded BEM classes in `MariloDataSheet.razor` imply a provider-delegation model that the SCSS side never filled in. Wave 1 SRC-02 already tracked the Razor side of this as "hard-coded BEM vs. all-styling-delegated-to-provider". | |

**Category:** cell border weight + header background + typography + density (umbrella)
**Recommended change:** Author `_data-sheet.scss` in each provider's `Styles/components/` folder and register it in the provider's `_index.scss`. At minimum, implement: grid root container, header cell fill + border-bottom, data cell borders (1px neutral-divider), row height (theme-density-aware), cell padding (8px horizontal / 4px vertical baseline), font family from provider tokens, alternating-row option disabled by default but tokenized so the gap record below can opt in.
**Acceptance criteria:** Static review confirms a `_data-sheet.scss` file exists in each provider, the provider `_index.scss` `@forward`s it, and a fresh `dotnet build` of `Marilo.Demo` compiles the provider stylesheet without missing-partial errors. Capture-time verification re-scores `cell grid default` and `header row` in all 6 theme/mode cells to 2+ before this gap can close.
**Remediation handoff target:** gap-analysis-resolution intake → new gap `VP-datasheet-scss-foundation` (structural). Parallelizable per provider after contract agreement with `datasheet-theming-architecture` decision.

---

**ID:** `VP-datasheet-02`
**Component:** MariloDataSheet
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Cell grid default
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | No Fluent-specific `_data-sheet.scss` exists in `src/Marilo.Providers.FluentUI/Styles/components/`. The Razor emits `<table>` with no `border-collapse`, no width set on the root container, so the grid auto-shrinks to content width and shows no cell boundaries. | Fluent light grid uses `border-collapse: collapse`, `width: 100%` on the root, 1px `neutralStrokeLayer1` cell borders, neutral-layer-1 background, 13px `segoeui` text, and Fluent's standard 32px row height. Header row uses `neutralStrokeLayer2` with a 2px bottom border for separation. |
| Likely cause | Missing-state coverage — downstream of `VP-datasheet-01`. Scoring Fluent Light explicitly because it is the primary-design target per the parity plan's first-pass order. | |

**Category:** cell border weight + header background + token/color
**Recommended change:** In `_data-sheet.scss` (Fluent), implement the selector set in VP-01 using Fluent foundation tokens. Specifically reuse tokens already defined in `foundation/_tokens.scss` for `--marilo-color-neutral-stroke-layer-*` (same palette the data-grid uses).
**Acceptance criteria:** Fluent Light `cell grid default` scores ≥2 against the internal baseline after the provider stylesheet ships.
**Remediation handoff target:** gap-analysis-resolution intake — lane `vp-datasheet-fluent-light`.

---

**ID:** `VP-datasheet-03`
**Component:** MariloDataSheet
**Theme:** Fluent
**Mode:** Dark
**State/Scenario:** Cell grid default
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Same as VP-datasheet-02 but in dark mode. No dark-mode block for any `mar-datasheet*` selector anywhere in the Fluent SCSS tree (`ripgrep mar-datasheet src/Marilo.Providers.FluentUI` returns 0 SCSS hits). | Fluent Dark grid uses `neutralLayer2Dark` cell background, `neutralStrokeDark` 1px borders that remain visible at 4.5:1 against background, 13px text with dark-mode `--marilo-color-neutral-foreground-rest` — all tokens already defined for Fluent data-grid in `_data-grid.scss` dark blocks. |
| Likely cause | Missing-state coverage — dark-mode token block is not even scaffolded. Carries SRC-02 risk: even when SCSS is authored, the `[data-marilo-theme="dark"]` selector has to pair with the bootstrap-bridge dark mechanism without hard-forking. | |

**Category:** token/color + cell border weight
**Recommended change:** Add a `[data-marilo-theme="dark"]` block inside `_data-sheet.scss` that overrides cell background, border, and header fill. Reuse the dark-mode token names already active for Fluent data-grid.
**Acceptance criteria:** Fluent Dark `cell grid default` scores ≥2 and borders pass 3:1 contrast against cell background.
**Remediation handoff target:** gap-analysis-resolution intake — lane `vp-datasheet-fluent-dark`.

---

**ID:** `VP-datasheet-04`
**Component:** MariloDataSheet
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Cell selection (single active cell)
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `FluentUICssProvider.DataSheetCellClass` appends `mar-datasheet__cell--active` when `IsCellActive(row, field)` is true, and `IsCellActive` is set by `OnCellClick` in `MariloDataSheet.Editing.cs`. No SCSS rule targets `mar-datasheet__cell--active` — active cell is visually indistinguishable from inactive. | Active cell has a 2px Fluent `--marilo-color-brand-stroke-rest` outline inset by 1px so it does not cover the cell border, no fill change (Excel-style box), and keyboard-focus shadow `0 0 0 2px --marilo-color-brand-stroke-focus` when the grid holds DOM focus. |
| Likely cause | Missing-state coverage — downstream of VP-01. Separate record kept because the remediation is a distinct selector set (`.mar-datasheet__cell--active`) and the state scenario is independently capturable. | |

**Category:** selection highlight + focus treatment
**Recommended change:** Implement `.mar-datasheet__cell--active` in the Fluent `_data-sheet.scss` with a 2px inset outline using the Fluent brand-stroke token. Use `:focus-visible` on the root grid as the gate for the stronger focus ring.
**Acceptance criteria:** Fluent Light `cell selection` scores ≥2 and active-cell outline is visible against row hover and the `cell editing` state.
**Remediation handoff target:** gap-analysis-resolution intake — lane `vp-datasheet-fluent-selection`.

---

**ID:** `VP-datasheet-05`
**Component:** MariloDataSheet
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Editable cell (cell in edit mode)
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `MariloDataSheet.Rendering.cs:233/247/264/279` emit `<input class="mar-datasheet__editor-input">` or `<select class="mar-datasheet__editor-select">` when a cell is editing. No provider SCSS styles these classes — inputs render with browser-default chrome (grey border, different font than the cell), breaking the illusion of in-place editing. | Inline input fills the cell (`width: 100%`, `height: 100%`), inherits the cell font, has a 1px Fluent brand stroke on all sides, and no inner padding so the caret sits where the cell text was. Material variant uses an underline-style focus indicator instead of an outline. |
| Likely cause | Missing-state coverage for `mar-datasheet__editor-input` / `mar-datasheet__editor-select`. This is a separate selector set from cell borders so it is tracked as its own gap. | |

**Category:** editing input chrome + typography
**Recommended change:** Add `.mar-datasheet__cell.is-editing > .mar-datasheet__editor-input` and sibling `__editor-select` selectors in the Fluent SCSS. Match the cell font inheritance (`font: inherit`) and remove default input padding/border; add a 1px brand-stroke border + 2px focus outline.
**Acceptance criteria:** Fluent Light `editable cell` shows the input aligned to the cell without layout shift, font unchanged from display mode, brand-stroke outline visible, and Fluent Dark shows the same with dark-mode brand stroke.
**Remediation handoff target:** gap-analysis-resolution intake — lane `vp-datasheet-fluent-editor`.

---

**ID:** `VP-datasheet-06`
**Component:** MariloDataSheet
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Validation error (cell with cellState == Invalid)
**Reference Source:** internal Marilo baseline
**Parity Score:** 1
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Provider emits `mar-datasheet__cell--invalid` when `CellState.Invalid` and a `title` + aria-describedby visually-hidden error message (`Rendering.cs:98-159`). The visually-hidden span uses a **hard-coded inline `style=` attribute** for the sr-only treatment (`Rendering.cs:156-158`) and `mar-datasheet__sr-only` class has no provider SCSS. No SCSS tints the invalid cell background or shows an inline error indicator — the user only discovers the error by hovering for the tooltip. | Invalid cell shows a 2px red (`--marilo-color-palette-red-border-*`) left border or a subtle red tint on the cell background, a small error-icon glyph in the corner, and optionally an inline error message below the cell in a popover. The sr-only class is applied via provider SCSS rather than inline styles so dark mode and text-zoom can override as needed. |
| Likely cause | Missing-state coverage (`mar-datasheet__cell--invalid` + `mar-datasheet__sr-only`) AND component-level (inline `style=` attribute in RenderTreeBuilder output bypasses the provider). Ties to Wave 1 SRC-02 (hard-coded styling in source). | |

**Category:** cell border weight + token/color + selection highlight (error)
**Recommended change:** Implement `.mar-datasheet__cell--invalid` in Fluent `_data-sheet.scss` using the red palette tokens already defined in `foundation/_palette.scss`. Migrate the sr-only inline `style=` in `MariloDataSheet.Rendering.cs:156-158` to a provider class — but that is SRC-02 territory and requires the `datasheet-theming-architecture` decision, so record that step as DEFERRED on this record rather than doing it here.
**Acceptance criteria:** Fluent Light `validation error` scores ≥2 (visible without hover) AND the inline `style=` attribute on the sr-only span is removed in a follow-up source pass after SRC-02 resolves.
**Remediation handoff target:** gap-analysis-resolution intake — lane `vp-datasheet-fluent-validation`. Follow-up to Wave 1 SRC-02 for the inline-style migration.

---

**ID:** `VP-datasheet-07`
**Component:** MariloDataSheet
**Theme:** Fluent / Bootstrap / Material (all)
**Mode:** Light / Dark (all)
**State/Scenario:** Frozen column
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** Major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | `MariloDataSheet.razor` and `Rendering.cs` emit no freeze-column classes (`ripgrep mar-datasheet__frozen src/Marilo.Components/DataGrid/MariloDataSheet*` returns 0). The `<th style="width:40px">` select-column and `<th style="width:60px">` actions-column are inline-styled but neither has `position: sticky`. Spec `columns-and-schema.md` and Wave 1 spec-review both list frozen-column as in-scope. | Frozen column should use `position: sticky; left: 0` (or `right` for end-frozen), paired with a 2px neutral-divider right-border and a subtle right-side shadow `0 0 8px -4px rgba(0,0,0,0.16)` to indicate the scroll boundary. This needs both source (add `Frozen` property and render-time sticky class) and SCSS (freeze separator). |
| Likely cause | Missing-state coverage for both source and SCSS. This is a dual gap but only the SCSS half belongs in stage 03; the source half is a gap-analysis item. | |

**Category:** frozen separator + layout
**Recommended change:** Add `.mar-datasheet__cell--frozen` and `.mar-datasheet__header-cell--frozen` selector pair across all three providers with `position: sticky`, border-right, and shadow. The source-side freeze property is a gap-analysis follow-up (Wave 1 already tracks `columns-and-schema` partial coverage).
**Acceptance criteria:** After the source side lands the frozen property, `frozen column` scenario scores ≥2 in all 6 theme/mode cells.
**Remediation handoff target:** gap-analysis-resolution intake — dual ticket: source (freeze model) + SCSS (separator).

---

**ID:** `VP-datasheet-08`
**Component:** MariloDataSheet
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Row hover
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** Major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Razor output has no `:hover` affordance anywhere — no SCSS, no class, no JS. Hover over a data row is visually identical to resting state. Row-hover is not a capture-matrix primary state but is listed in the wave-3 target set. | Row-hover tints the row background with a Fluent `neutralLayerAlt` 50% token (or equivalent subtle tint), borders unchanged, and cursor remains default. Hover must not conflict with selected-row styling — selected rows win in the cascade. |
| Likely cause | Missing-state coverage. Added as a focused SCSS line rather than a source change. | |

**Category:** selection highlight + density
**Recommended change:** Add `.mar-datasheet__row:hover:not(.mar-datasheet__row--selected)` selector with a low-alpha hover token across all three providers. Trivial if `_data-sheet.scss` exists.
**Acceptance criteria:** Fluent Light `row hover` scores ≥2 without conflicting with the `row selection` state.
**Remediation handoff target:** gap-analysis-resolution intake — part of the `_data-sheet.scss` umbrella lane.

---

**ID:** `VP-datasheet-09`
**Component:** MariloDataSheet
**Theme:** Fluent
**Mode:** Light
**State/Scenario:** Row selection
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Provider emits `mar-datasheet__row--selected` (`FluentUICssProvider.cs:568-574`) via the bulk-checkbox column path. No SCSS styles this modifier — selected rows and unselected rows look identical. The bulk-bar `mar-datasheet__bulk-bar--visible` modifier also has no SCSS. Scenario C of BulkOperations.razor depends on this being visually distinguishable. | Selected row tints the whole row with the Fluent brand-subtle-selected token (`--marilo-color-brand-background-selected`), leaves the per-cell active-outline (VP-04) independent of row selection, and the bulk-bar above the grid transitions in (`transform: translateY` or opacity) using the `--visible` modifier. |
| Likely cause | Missing-state coverage. Row selection is a primary demo pathway — BulkOperations scenario C is the worked example — so absence is user-visible in the demo walkthrough. | |

**Category:** selection highlight + token/color
**Recommended change:** Implement `.mar-datasheet__row--selected` (brand-subtle tint) and `.mar-datasheet__bulk-bar` + `--visible` modifier across all three providers.
**Acceptance criteria:** Fluent Light `row selection` scores ≥2 and bulk-bar visibly animates/displays when `_selectedRows.Count > 0`.
**Remediation handoff target:** gap-analysis-resolution intake — lane `vp-datasheet-fluent-selection`.

---

**ID:** `VP-datasheet-10`
**Component:** MariloDataSheet
**Theme:** Bootstrap
**Mode:** Light
**State/Scenario:** Cell grid default + cell state modifiers
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Bootstrap bridge provider exposes the same `mar-datasheet*` class names via `BootstrapCssProvider` but `src/Marilo.Providers.Bootstrap/Styles/` has 0 SCSS rules for any of them (`ripgrep datasheet src/Marilo.Providers.Bootstrap/Styles` → 0 hits). There is no `_bridge-data-sheet.scss`. The bridge files `_bridge-data.scss` and `_bridge-forms.scss` cover data-grid and form inputs respectively but neither touches DataSheet. | Bootstrap bridge re-maps `mar-datasheet*` selectors to Bootstrap 5 table classes (`table`, `table-striped`, `table-hover` equivalents) while keeping the `mar-datasheet` prefix — the same pattern `_bridge-data.scss` uses for DataGrid. Separate `[data-bs-theme="dark"]` dark-mode block required. |
| Likely cause | Missing-state coverage — no bridge file was ever created for DataSheet. | |

**Category:** token/color + cell border weight + header background (umbrella, Bootstrap lane)
**Recommended change:** Author `_bridge-data-sheet.scss` in the Bootstrap provider modeled on `_bridge-data.scss`. Map the `mar-datasheet*` selectors to Bootstrap 5 table tokens + `[data-bs-theme="dark"]` blocks.
**Acceptance criteria:** Bootstrap Light and Dark `cell grid default` score ≥2 after the bridge lands; dark-mode selector uses `[data-bs-theme="dark"]` not `[data-marilo-theme="dark"]` (bridge convention from SRC-02-adjacent work).
**Remediation handoff target:** gap-analysis-resolution intake — lane `vp-datasheet-bootstrap-bridge`.

---

**ID:** `VP-datasheet-11`
**Component:** MariloDataSheet
**Theme:** Material
**Mode:** Light / Dark
**State/Scenario:** Cell grid default + all states
**Reference Source:** internal Marilo baseline
**Parity Score:** 0
**Severity:** Critical

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Material provider `_data-sheet.scss` does not exist; `MaterialCssProvider` still emits `mar-datasheet*` classes via the interface. Stage 02 already noted "Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10)". Material grid currently inherits the same unstyled `<table>` as Fluent/Bootstrap. Additionally, the Material density bar (40px row height, 48px header) cannot be applied without provider SCSS. | Material theme uses 40px row height, 48px header height, 16dp horizontal padding, Material-spec ripple on row click, 1dp cell divider (hairline), `--marilo-color-surface` background, and the Material Dark palette variants. |
| Likely cause | Missing-state coverage and provider scaffold status. | |

**Category:** density + typography + cell border weight (umbrella, Material lane)
**Recommended change:** Author `_data-sheet.scss` in the Material provider using the Material density tokens already declared in `foundation/_density.scss` (or equivalent). This depends on the Material runtime implementation decision — if the Material runtime project is still scaffold-only, mark this gap as SCSS-ready but blocked on the provider runtime.
**Acceptance criteria:** Material Light `cell grid default` scores ≥2 with visible hairline dividers and Material row height.
**Remediation handoff target:** gap-analysis-resolution intake — lane `vp-datasheet-material`. Secondary dependency: Material runtime provider implementation status.

---

**ID:** `VP-datasheet-12`
**Component:** MariloDataSheet
**Theme:** Fluent (primary)
**Mode:** Light / Dark
**State/Scenario:** Focused cell (keyboard focus, distinct from mouse-activated cell)
**Reference Source:** internal Marilo baseline
**Parity Score:** 1
**Severity:** Major

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | Rendering path does not use the `mar-datasheet__cell--active` modifier to represent "has keyboard focus" — it represents "was last clicked". Wave 1 SA-01 already notes the grid root lacks `tabindex=0`, so there is no DOM focus on the grid to begin with. Visually, pressing Tab does nothing observable in the grid. `Keyboard-and-Accessibility.razor` scenario A relies on the user believing they have focus. | Focused cell shows a distinct 2px Fluent brand-stroke-focus outline that is visible independent of `--active` / hover / selection states. The keyboard focus indicator follows `:focus-visible` rules (not shown on mouse click). The grid root owns `tabindex=0` and delegates cell focus via roving-tabindex. |
| Likely cause | Source-side + SCSS-side. Source-side is SA-01 (Wave 1) — already tracked. SCSS-side is a focus-visible selector we cannot author until SA-01 resolves because there is no DOM focus target. Scoring 1 rather than 0 because the `--active` styling loosely stands in (if it had any SCSS). | |

**Category:** focus treatment
**Recommended change:** After SA-01 lands, add `.mar-datasheet__cell:focus-visible` with a brand-stroke-focus outline. Do not attempt before SA-01.
**Acceptance criteria:** Fluent Light `focused cell` scores ≥2 after source + SCSS both ship. Classify as `DEFERRED-PENDING-SOURCE` until SA-01 lands.
**Remediation handoff target:** gap-analysis-resolution intake — waits on Wave 1 SA-01 source fix.

---

### Deferrals (classified, not gaps to remediate this wave)

**ID:** `VP-datasheet-D01`
**State/Scenario:** EU-06 theming side-by-side demo (Fluent × Bootstrap × Material)
**Classification:** `DEFERRED-PENDING-ARCHITECTURE`
**Blocker:** `datasheet-theming-architecture` user-decision OPEN.
**Why deferred:** Per Wave 2 EU-06 and Wave 1 SRC-02, no theming demo or capture is possible until the orchestrator decides whether `IMariloCssProvider` grows per-subregion methods (datasheet button / skeleton / badge / editor / sr-only) or the spec is narrowed. Until then any capture would be scoring a contract that hasn't been agreed on. Scoped out of this wave by inbox instruction — worker MUST NOT attempt to resolve.

---

**ID:** `VP-datasheet-D02`
**State/Scenario:** Rectangular range-selection (cell range) across all themes/modes
**Classification:** `DEFERRED-PENDING-SOURCE`
**Blocker:** Wave 1 V03 — `DataSheetSelection<TItem>` source model does not exist. Wave 2 EU-07 tracks the demo side.
**Why deferred:** No source model means no DOM attributes for range-selection to target. Capturing the scenario would score a non-existent feature. Revisit after the source model lands.

---

**ID:** `VP-datasheet-D03`
**State/Scenario:** 10k-row virtualization capture (upper-threshold from `virtualization-and-performance.md`)
**Classification:** `DEFERRED-PENDING-SCOPE`
**Blocker:** `datasheet-10k-rows` user-decision OPEN — demo-dataset cap must be agreed before a 10k-row scene can be added to `Marilo.Demo`.
**Why deferred:** Static analysis cannot evaluate virtualization visual quality at thresholds that don't exist in the demo, and the orchestrator has not authorized expanding `Marilo.Demo` startup cost. Smaller virtualization thresholds (≤500 rows) were implicitly covered by the existing BulkOperations scenario E and did not surface additional visual gaps beyond those in VP-datasheet-01.

---

### Coverage summary

| State/Scenario | Scored? | Worst score observed | Notes |
|---|---|---|---|
| 1. Cell grid default | Yes (Fluent L/D, Bootstrap, Material) | 0 | VP-01/02/03/10/11 |
| 2. Header row | Yes (covered by VP-01 umbrella) | 0 | rolled into VP-01 |
| 3. Row hover | Yes (Fluent) | 0 | VP-08 |
| 4. Cell selection | Yes (Fluent) | 0 | VP-04 |
| 5. Row selection | Yes (Fluent) | 0 | VP-09 |
| 6. Focused cell | Yes (Fluent) | 1 | VP-12, partially source-blocked on SA-01 |
| 7. Editable cell | Yes (Fluent) | 0 | VP-05 |
| 8. Validation error | Yes (Fluent) | 1 | VP-06 |
| 9. Frozen column | Yes (umbrella) | 0 | VP-07 (also source-gapped) |

**Total records:** 12 gap records + 3 deferrals = **15** (within the 10–20 target).

**Severity distribution:**
- Critical: 8 (VP-01, 02, 03, 04, 05, 06, 09, 10, 11)
- Major: 3 (VP-07, VP-08, VP-12)
- Minor: 0
- Polish: 0
