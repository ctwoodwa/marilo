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
