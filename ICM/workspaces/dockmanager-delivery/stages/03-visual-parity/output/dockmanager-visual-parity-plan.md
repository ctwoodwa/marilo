# Visual Parity Plan -- MariloDockManager

## Component

MariloDockManager

## Reference Strategy

**Internal Marilo delivery-quality baseline.** MariloDockManager has no Telerik Blazor equivalent. Visual parity review scores against Marilo's own delivery-quality bar: consistent tokens, correct state treatment, appropriate density, and dock-manager-standard UX conventions (VS Code / JetBrains docking chrome as the informal reference for panel header, tab strip, and drag/drop indicator behavior).

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Docked panel | Baseline layout and panel chrome |
| P1 | Panel header | Header height, typography, and control alignment |
| P1 | Tab strip | Active/inactive tab visual quality |
| P2 | Floating panel | Elevation shadow and border quality |
| P2 | Split layout | Splitter handle styling between panels |
| P2 | Close/minimize buttons | Icon sizing and hover state |
| P3 | Drop indicator | Drop zone highlight during drag |
| P3 | Drag preview | Ghost panel opacity and border |
| P4 | Empty dock zone | Dock zone placeholder styling |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component characteristics:

- Panel header height: consistent across themes and modes
- Tab strip active/inactive contrast: sufficient in dark mode
- Splitter handle: hover state visibility in dark mode
- Floating panel elevation: shadow depth using correct surface tokens
- Drop indicator: highlight color and opacity across themes
- Dark-mode token gaps: known risk for subtle-background and border tokens missing from dark blocks
- Bootstrap bridge dark-mode: `[data-marilo-theme="dark"]` vs `[data-bs-theme="dark"]` mechanism

## Known Unknowns

- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures may be blocked
- Drag preview visual behavior not yet audited
- Drop indicator animation or transition not confirmed
- Empty dock zone placeholder design not finalized

## Blockers

- Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10)
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
