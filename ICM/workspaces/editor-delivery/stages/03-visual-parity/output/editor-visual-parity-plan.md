# Visual Parity Plan -- MariloEditor

## Component

MariloEditor

## Reference Strategy

**Telerik Editor parity.** Telerik's Blazor Editor provides the visual reference baseline for Editor states including toolbar default, toolbar button hover/active/focus, content area, formatting states, link and image dialogs, source view, readonly mode, and placeholder. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Toolbar default | First visual impression — toolbar quality |
| P1 | Content area idle | Primary editing surface |
| P1 | Toolbar button hover | Most common toolbar interaction |
| P1 | Toolbar button active | Active/toggled formatting button state |
| P2 | Toolbar button focus | Keyboard accessibility quality |
| P2 | Formatting active | Content with formatting applied |
| P2 | Toolbar separator | Toolbar density detail |
| P2 | Placeholder | Empty editor state |
| P3 | Link dialog | Dialog chrome quality |
| P3 | Image dialog | Dialog chrome quality |
| P4 | Source view | Mode variant quality |
| P4 | Readonly mode | Disabled mode visual distinction |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component structure and cerebrum learnings:
- Toolbar button sizing — height and icon size may not match provider token scale
- Toolbar separator — line weight and margin may be too heavy or too light
- Content area padding — internal padding may not inherit provider spacing tokens
- Dialog chrome — border, shadow, and background in dark mode
- Formatting indicator states — active/toggled button background may use wrong token
- Focus ring visibility — keyboard focus ring must be clearly visible for accessibility
- Dark mode toolbar surface — toolbar background tint in dark mode

## Known Unknowns

- Toolbar overflow behavior (dropdown vs. wrap) at narrow widths not confirmed
- Image dialog upload vs. URL-only mode not confirmed
- Source view syntax highlighting (if any) not audited
- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures blocked

## Blockers

- **Material runtime provider not yet implemented** (SCSS-only scaffold as of 2026-04-10) — Material Light and Material Dark captures cannot proceed
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
