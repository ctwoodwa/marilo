# Visual Parity Plan -- TreeView

## Component

TreeView

## Reference Strategy

**Telerik TreeView parity.** Telerik's Blazor TreeView provides the visual reference baseline for hierarchical data display including node expand/collapse, selection, checkboxes (with tri-state), icons, templates, and load-on-demand. Marilo targets visual quality equivalence in indentation rhythm, node density, and state treatment.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Default hierarchy (2-3 levels) | Baseline visual impression and indentation rhythm |
| P1 | Expanded node | Most common parent node state |
| P1 | Hovered node | Most common interaction state |
| P1 | Selected node | Primary selection visual |
| P2 | Checkbox unchecked/checked | Core checkbox quality |
| P2 | Checkbox indeterminate | Tri-state is a common quality differentiator |
| P2 | Icon + text alignment | Layout precision indicator |
| P2 | Nested indentation rhythm | Structural consistency at depth |
| P3 | Focused node | Keyboard accessibility visual |
| P3 | Collapsed node | Disclosure icon direction |
| P3 | Disabled node | Dimming treatment |
| P3 | Load-on-demand indicator | Async loading visual |
| P4 | Templated node | Custom content rendering |
| P4 | Empty tree | Edge case styling |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage, especially node contrast
3. **Bootstrap Light** — validates bridge token mapping for tree-specific classes
4. **Bootstrap Dark** — validates dark-mode patches for indentation and checkbox visuals
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — highest gap density expected

## Known Gap Categories to Watch

Based on TreeView-specific concerns:
- Indentation spacing consistency across nesting depth
- Disclosure icon (expand/collapse arrow) size and alignment relative to text
- Checkbox geometry: border weight, check mark size, indeterminate dash rendering
- Node hover background tint: must flip from black-tint (light) to white-tint (dark)
- Icon-to-text baseline alignment when icons vary in size
- Dark mode contrast for nested node borders and backgrounds

## Known Unknowns

- Material provider runtime not yet implemented — Material captures may be blocked
- Templated node rendering quality depends on demo template complexity
- Load-on-demand spinner/indicator design not yet confirmed
- Empty tree state design (message, icon) not yet finalized

## Blockers

- Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10)

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for TreeView screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
