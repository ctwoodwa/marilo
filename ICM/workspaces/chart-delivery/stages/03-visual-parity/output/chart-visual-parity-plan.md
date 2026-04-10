# Visual Parity Plan -- MariloChart

## Component

MariloChart

## Reference Strategy

**Telerik Chart parity.** Telerik's Blazor Chart provides the visual reference baseline for Chart states including line, bar, area, and pie series, axis labels, gridlines, tooltips, legends, and interactive hover/selection states. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Line chart | Most common series type — structural baseline |
| P1 | Bar chart | Second most common series type |
| P1 | Series hover | Most common interaction state |
| P1 | Tooltip visible | High-visibility interaction element |
| P2 | Axis labels | Typography anchor for chart quality |
| P2 | Gridlines | Background visual quality |
| P2 | Legend idle | Compositional element present on most charts |
| P2 | Area chart | Secondary series type |
| P3 | Pie chart | Distinct series type requiring separate review |
| P3 | Series selected | Selection visual quality |
| P3 | Data point markers | Detail quality on line/area series |
| P3 | Empty state | Empty plot area styling |
| P4 | Loading state | Skeleton/spinner quality |
| P4 | Legend layout overflow | Responsive behavior at narrow widths |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component structure and cerebrum learnings:
- Series color palette — theme-mapped palette tokens may not be wired to provider colors
- Gridline weight — thin lines may disappear or over-contrast in dark mode
- Axis typography — font size and weight may not inherit provider tokens correctly
- Tooltip chrome — border, shadow, and background in dark mode
- Legend layout — swatch-to-label spacing and row density
- Dark mode surface — plot area background must use surface token not hardcoded color

## Known Unknowns

- Series color palette design not audited against provider token systems
- Tooltip positioning and overflow behavior at chart edges not confirmed
- Pie chart label placement and collision avoidance not confirmed
- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures blocked

## Blockers

- **Material runtime provider not yet implemented** (SCSS-only scaffold as of 2026-04-10) — Material Light and Material Dark captures cannot proceed
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
