# Visual Parity Plan -- MariloDataSheet

## Component

MariloDataSheet

## Reference Strategy

**Internal Marilo delivery-quality baseline.** MariloDataSheet has no Telerik Blazor equivalent — it is a true spreadsheet component unique to Marilo. Visual parity review scores against Marilo's own delivery-quality bar: consistent tokens, correct state treatment, appropriate density, and spreadsheet-standard UX conventions (Excel/Google Sheets visual grammar as the informal reference for cell grid behavior).

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Cell grid default | Baseline visual impression and grid density |
| P1 | Selected cell | Primary selection visual — border and highlight |
| P1 | Cell editing | Most common interaction state |
| P2 | Column/row headers | Header background and typography anchor |
| P2 | Frozen rows/columns | Separator quality and sticky positioning |
| P2 | Cell range selection | Range highlight and border treatment |
| P3 | Formula bar | Input chrome and integration with grid |
| P3 | Sheet tabs | Tab active/inactive contrast |
| P4 | Empty sheet | Empty grid lines only |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component characteristics:

- Cell border weight: grid line thickness and contrast in dark mode
- Header background: sufficient contrast between header and cell area
- Selection highlight: selected cell border and range fill visibility
- Frozen separator: separator line weight and color at freeze boundary
- Dark-mode token gaps: known risk for subtle-background and border tokens missing from dark blocks
- Bootstrap bridge dark-mode: `[data-marilo-theme="dark"]` vs `[data-bs-theme="dark"]` mechanism

## Known Unknowns

- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures may be blocked
- Formula bar integration with grid not yet audited
- Sheet tab scrolling behavior under many tabs not confirmed
- Scrollbar styling across providers not confirmed

## Blockers

- Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10)
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes

---

## 2026-04-11 wave 3 static-analysis pass (w-datasheet-delivery)

A static-analysis pass (no browser capture) was run on 2026-04-11 by
`w-datasheet-delivery` to populate `datasheet-visual-parity-gaps.md` and
`datasheet-parity-summary.md`. Findings that inform the remainder of the
plan:

- **The assumption in step 2 above (set up Playwright capture scripts) is
  blocked for this component.** Every `mar-datasheet*` BEM class is emitted
  but no provider SCSS defines any of them. Captures would score browser-
  default `<table>` in all 6 theme/mode combinations across all 9 target
  states — the scoring dispersion between themes would be zero. Wave 3
  therefore produced the gap list from source + provider + SCSS audit
  instead, which is faster and yields the same critical finding.
- **Every primary state scored 0 in every theme/mode** (except validation
  error and focused cell, which scored 1). The root cause is missing
  `_data-sheet.scss` / `_bridge-data-sheet.scss` in all three providers.
  This is tracked as the umbrella record `VP-datasheet-01` with per-theme
  child records `VP-datasheet-02` through `VP-datasheet-12`.
- **Three scenarios are DEFERRED by inbox instruction** and MUST NOT be
  re-escalated:
  - EU-06 theming side-by-side → `DEFERRED-PENDING-ARCHITECTURE`
    (`datasheet-theming-architecture` user-decision OPEN).
  - EU-07 rectangular range selection → `DEFERRED-PENDING-SOURCE`
    (`DataSheetSelection<TItem>` source model does not exist).
  - 10k-row virtualization capture → `DEFERRED-PENDING-SCOPE`
    (`datasheet-10k-rows` user-decision OPEN).
  These are recorded as `VP-datasheet-D01` / `D02` / `D03`.
- **Review order adjustment for the next capture pass** (when it can run):
  do not start with Fluent Light alone. The structural gap is identical
  across providers, so the useful capture pass is "a single theme/mode
  after `_data-sheet.scss` lands, to verify the fix pattern scales".
  That sequencing is plan-level guidance for the 04-sync-check stage and
  the remediation work, not this wave.
- **Parallelizable remediation lanes** are enumerated in the parity
  summary's "Primary remediation lanes" section. Five lanes, one per
  provider / concern, all blocked on the `datasheet-theming-architecture`
  decision landing first.

## Known blockers (updated 2026-04-11)

- `datasheet-theming-architecture` user-decision OPEN — foundation of every
  SCSS remediation lane. Do not dispatch remediation workers until
  resolved.
- Wave 1 `SA-01` (grid root `tabindex=0`) — source-side prerequisite for
  VP-datasheet-12 focus-visible styling. Cannot author the focus SCSS
  until the source DOM has a focus target.
- Wave 1 `V03` (range selection source model) — source-side prerequisite
  for VP-datasheet-D02. Deferred until after source lands.
- `datasheet-10k-rows` user-decision OPEN — demo-dataset cap prerequisite
  for VP-datasheet-D03.
- Material runtime provider implementation status — secondary gate on
  VP-datasheet-11 (Material lane).
