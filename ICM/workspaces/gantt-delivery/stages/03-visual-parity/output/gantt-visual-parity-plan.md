# Visual Parity Plan -- MariloGantt

## Component

MariloGantt

## Reference Strategy

**Telerik Gantt parity.** Telerik's Blazor Gantt provides the visual reference baseline for Gantt states including task bars, milestone diamonds, summary bars, tree column, timeline headers, dependency lines, progress indicators, current date line, and interactive hover/selection states. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Default task bar | Baseline visual impression of task rendering |
| P1 | Tree column idle | Primary structural anchor for Gantt layout |
| P1 | Timeline header | Typography and density anchor |
| P1 | Task hover | Most common interaction state |
| P2 | Task selected | Primary selection visual |
| P2 | Summary bar | Parent task treatment quality |
| P2 | Current date line | High-visibility marker |
| P2 | Progress indicator | Fill quality within task bars |
| P3 | Milestone diamond | Precision sizing and shape quality |
| P3 | Dependency lines | Connector weight and arrowhead quality |
| P3 | Expanded row | Tree expand state treatment |
| P3 | Collapsed row | Tree collapse state treatment |
| P4 | Editing row | Inline edit chrome quality |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component structure and cerebrum learnings:
- Bar height — task bar height relative to row may be too tall or too short vs. Telerik reference
- Milestone sizing — diamond dimensions and border weight are precision-sensitive
- Dependency line weight — thin SVG lines may disappear in dark mode
- Tree column indentation — pixels-per-level consistency with provider typography scale
- Timeline density — header cell width and tier separator line weight
- Progress fill color — contrast against task bar background in dark mode
- Current date line — opacity and color may wash out in dark mode

## Known Unknowns

- Dependency line arrowhead rendering method (SVG vs. CSS) not confirmed
- Summary bar collapse/expand animation visual not audited
- Timeline header depth (2-tier vs. 3-tier) support not confirmed
- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures blocked

## Blockers

- **Material runtime provider not yet implemented** (SCSS-only scaffold as of 2026-04-10) — Material Light and Material Dark captures cannot proceed
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
