# Visual Parity Plan -- MariloWizard

## Component

MariloWizard

## Reference Strategy

**Telerik Wizard parity.** Telerik's Blazor Wizard provides the visual reference baseline for Wizard states including step indicator completed/active/pending, connector lines, step content area, prev/next/finish navigation buttons, disabled steps, and validation error treatment. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Step indicator active | Primary orientation point for users — must be immediately clear |
| P1 | Step content area | Largest visual surface — padding and border anchor the layout |
| P1 | Navigation buttons prev/next | Core interaction chrome seen on every step |
| P1 | Button alignment | Spatial relationship of prev/next/finish must match reference |
| P2 | Step indicator completed | Progression feedback — checkmark or fill must read clearly |
| P2 | Connector line | Weight and color between steps carries the timeline metaphor |
| P2 | Step indicator pending | Muted future steps — must be distinct without being invisible |
| P2 | Finish button | Terminal step variant — position and styling |
| P3 | Disabled step | Visual suppression of unreachable steps |
| P3 | Validation error | Error badge on step indicator — high stakes if missing |
| P4 | Non-linear mode | All steps clickable — changes active/pending distinction |
| P4 | All states in Bootstrap Dark | Dark-mode token coverage for Bootstrap bridge |
| P4 | All states in Material Light/Dark | Material provider coverage (blocked — see Blockers) |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates Bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

- Step indicator active color tokens — active circle/highlight must be the brand accent, not a gray
- Connector line fill for completed steps — half-fill (completed portion colored) is a common Telerik pattern; verify Marilo supports it
- Button alignment in footer — Telerik right-aligns prev/next by default; confirm Marilo matches
- Validation error indicator — may not be implemented; flag as missing-state if absent
- Dark-mode step indicator backgrounds — active and completed fills must invert correctly
- Content area padding — inner whitespace must be comfortable, not cramped

## Known Unknowns

- Validation error indicator implementation status not yet confirmed
- Non-linear mode step click behavior not yet audited visually
- Step indicator animation (if any) on transition not captured
- Finish button position and label customization not yet reviewed

## Blockers

- Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10) — Material Light and Material Dark captures are blocked until the runtime project exists
- Stage 02 (Example UX) output needed for demo scenario list — confirm availability before executing captures

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
