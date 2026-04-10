# Visual Parity Plan -- MariloResizableContainer

## Component

MariloResizableContainer

## Reference Strategy

**Internal Marilo delivery-quality baseline.** MariloResizableContainer is a simple utility component with no Telerik Blazor equivalent. Visual parity review is lightweight — 5 states, focused on handle visibility, handle cursor, resize border, and constraint indicator across themes. Score against Marilo's delivery-quality bar: correct token usage, legible handle states, and consistent constraint feedback.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Container default | Baseline chrome and handle visibility at rest |
| P1 | Resize handle hover | Primary interaction state — handle discoverability |
| P1 | Resize handle active | Drag state — border and cursor feedback |
| P2 | Min/max constraint visible | Constraint indicator quality |
| P3 | Corner handles | Corner handle styling if present |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component characteristics:

- Handle visibility: handles must be discoverable without being obtrusive at rest
- Handle cursor: correct directional resize cursor per handle position
- Resize border: clear visual feedback during active drag across all themes
- Dark-mode token gaps: known risk for border and subtle-background tokens missing from dark blocks

## Known Unknowns

- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures may be blocked
- Corner handle presence and styling not yet confirmed
- Constraint indicator animation or visual feedback not confirmed

## Blockers

- Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10)
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Execute first-pass Fluent Light review across P1 scenarios
3. Document gaps and iterate through remaining themes/modes
