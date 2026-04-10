# Visual Parity Plan -- MariloDiagram

## Component

MariloDiagram

## Reference Strategy

**Telerik Diagram parity.** Telerik's Blazor Diagram provides the visual reference baseline for Diagram states including node default/selected/hover, connector lines and endpoints, canvas background and grid, zoom controls, minimap, and group containers. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Node default | Primary canvas element — border, fill, shadow, label must all read correctly |
| P1 | Node selected | Selection handles are core interaction feedback |
| P1 | Node hover | Hover ring or border change must be clearly perceptible |
| P1 | Connector line | Connection chrome is seen on every diagram with edges |
| P2 | Canvas background | Grid or flat fill anchors the entire canvas experience |
| P2 | Zoom controls | Always-visible chrome — sizing and styling must match reference |
| P2 | Group container | Structural grouping chrome is a common diagram pattern |
| P2 | Connector endpoint | Arrowhead quality affects perceived polish significantly |
| P3 | Text label only | Label-only nodes are common in flowcharts |
| P3 | Group container selected | Selection treatment inside group context |
| P3 | Node selected with group | Nested selection interaction |
| P4 | Minimap | Auxiliary chrome — important for large diagrams |
| P4 | Empty canvas | First-render state — canvas background and chrome only |
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

- Node border weight — Telerik uses a clearly visible but not heavy stroke; verify Marilo matches
- Selection handles — must be present, correctly sized, and positioned at node corners/edges
- Canvas grid — dot or line grid must be visible against canvas background in both light and dark
- Connector stroke — stroke weight and color must be theme-appropriate, not hardcoded
- Dark-mode node fill — node background must switch correctly; white nodes on dark canvas score 0
- Zoom control button chrome — border radius, icon size, and hover treatment must match reference
- Minimap viewport rect — the "current view" indicator inside the minimap must be clearly visible

## Known Unknowns

- Connector routing algorithm (straight vs. orthogonal vs. curved) visual output not yet audited
- Group container label placement and font treatment not yet confirmed
- Minimap implementation status not yet confirmed — may be missing-state if not implemented
- Node resize handle behavior during drag not yet captured

## Blockers

- Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10) — Material Light and Material Dark captures are blocked until the runtime project exists
- Stage 02 (Example UX) output needed for demo scenario list — confirm availability before executing captures

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
