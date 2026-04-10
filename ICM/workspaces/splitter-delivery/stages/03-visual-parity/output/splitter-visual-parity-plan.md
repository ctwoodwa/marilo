# Visual Parity Plan -- MariloSplitter

## Component

MariloSplitter

## Reference Strategy

**Telerik Splitter parity.** Telerik's Blazor Splitter provides the visual reference baseline for Splitter states including horizontal/vertical layout, resize handle idle/hover/active, collapsed panes, nested splitters, and min/max size constraints. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Horizontal split default | Baseline visual impression, handle at rest |
| P1 | Resize handle hover | Most common interaction — handle must read clearly |
| P1 | Resize handle active | Drag-in-progress state — must feel responsive |
| P1 | Collapsed pane | High-visibility state change |
| P2 | Vertical split default | Orientation variant — same handle logic, different axis |
| P2 | Collapse button styling | Button must be discoverable and well-styled |
| P2 | Nested splitters | Verifies handle and pane styles stack correctly |
| P3 | Three-pane layout | Multi-handle density check |
| P3 | Min/max constraint hit | Constraint boundary visual |
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

- Handle hover/active color tokens — must be visually distinct without being harsh
- Handle width precision — Telerik uses a narrow but clearly grabbable handle; verify Marilo matches
- Collapse button border, icon size, and color in dark mode
- Pane border weight — should be a subtle divider, not a heavy line
- `col-resize` / `row-resize` cursor — must apply on handle hover, not on pane content
- Dark-mode background bleed — pane backgrounds must not leak into handle zone

## Known Unknowns

- Collapse button animation (if any) not yet audited
- Nested splitter handle z-index behavior under overlap not confirmed
- Three-pane layout with mixed min/max constraints not yet captured

## Blockers

- Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10) — Material Light and Material Dark captures are blocked until the runtime project exists
- Stage 02 (Example UX) output needed for demo scenario list — confirm availability before executing captures

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
