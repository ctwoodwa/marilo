# Visual Parity Plan -- MariloPivotGrid

## Component

MariloPivotGrid

## Reference Strategy

**Telerik PivotGrid parity.** Telerik's Blazor PivotGrid provides the visual reference baseline for PivotGrid states including row headers, column headers, data cells, aggregation typography, expanded/collapsed groups, field chooser, and edge states. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Default view | Baseline visual impression of full layout |
| P1 | Row headers | Primary structural anchor — nesting quality |
| P1 | Column headers | Secondary structural anchor — nesting quality |
| P1 | Data cells | Core data display — alignment and density |
| P2 | Aggregation typography | Visual hierarchy between data and totals |
| P2 | Expanded group | Grouping state treatment |
| P2 | Collapsed group | Grouping state treatment |
| P2 | Group expand/collapse icon | Icon precision within header cell |
| P3 | Field chooser idle | Panel chrome quality |
| P3 | Empty state | Empty configuration styling |
| P4 | Loading state | Skeleton/spinner quality |
| P4 | Field chooser drag | Drag affordance quality |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component structure and cerebrum learnings:
- Header nesting indentation — pixels-per-level consistency across row and column axes
- Data cell alignment — right-alignment of numeric values may not inherit provider token correctly
- Aggregation typography — font weight and color distinction for totals rows
- Group expand/collapse icons — icon sizing and alignment within dense header cells
- Field chooser chrome — panel border and zone background in light and dark
- Dark mode cell surfaces — header and data cell background tints

## Known Unknowns

- Field chooser implementation (panel vs. dialog vs. popover) not confirmed
- Aggregation row count and nesting depth not confirmed
- Column header depth for deeply nested scenarios not audited
- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures blocked

## Blockers

- **Material runtime provider not yet implemented** (SCSS-only scaffold as of 2026-04-10) — Material Light and Material Dark captures cannot proceed
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
