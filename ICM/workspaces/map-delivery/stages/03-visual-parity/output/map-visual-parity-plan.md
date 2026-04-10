# Visual Parity Plan -- MariloMap

## Component

MariloMap

## Reference Strategy

**Telerik Map parity.** The Telerik Blazor Map provides the visual reference baseline for MariloMap states including map canvas rendering, marker default and interaction states, bubble layers, tooltips, zoom and navigation controls, and the legend. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Map canvas default | Baseline visual impression and tile rendering |
| P1 | Marker default | Primary data visualization element |
| P1 | Marker hover | Most common interaction state |
| P1 | Marker selected | Primary selection visual |
| P2 | Zoom controls | Control chrome quality and positioning |
| P2 | Tooltip | Popup chrome quality in light and dark |
| P2 | Legend | Layout, typography, and icon alignment |
| P3 | Bubble layer | Layer rendering opacity and stroke |
| P3 | Navigation controls | Control styling and icon quality |
| P4 | Empty map | Empty canvas with no data layers |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component characteristics:

- Tooltip chrome: border, shadow, background color in dark mode
- Control button styling: zoom/nav button hover state token mapping
- Marker sizing: icon dimensions vs. Telerik reference
- Bubble opacity: fill opacity and stroke weight across themes
- Dark mode tile contrast: map tile visibility against control overlays

## Known Unknowns

- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures may be blocked
- Tooltip popup positioning and chrome quality not yet audited
- Navigation control design not finalized
- Legend layout behavior under varying data set sizes not confirmed

## Blockers

- Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10)
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
