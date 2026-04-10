# Visual Parity Plan -- MariloFileManager

## Component

MariloFileManager

## Reference Strategy

**Telerik FileManager parity.** Telerik's Blazor FileManager provides the visual reference baseline for FileManager states including tree navigation pane, file grid pane, breadcrumb, toolbar, file and folder icons, selected item, context menu, upload area, empty folder, and search input. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Default view | Baseline visual impression of full dual-pane layout |
| P1 | File grid pane | Primary content area — file/folder presentation quality |
| P1 | Item hover | Most common interaction state |
| P1 | Selected item | Primary selection visual |
| P2 | Tree navigation pane | Structural anchor for folder hierarchy |
| P2 | Breadcrumb | Navigation path quality |
| P2 | Toolbar idle | Toolbar chrome quality |
| P2 | Pane splitter | Layout boundary between tree and file grid |
| P3 | File/folder icons | Icon sizing and color fidelity |
| P3 | Context menu | Menu chrome quality |
| P3 | Search input | Input chrome quality |
| P3 | Tree expanded/collapsed | Tree expand/collapse state treatment |
| P4 | Upload area | Upload zone visual quality |
| P4 | Empty folder | Empty state styling |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component structure and cerebrum learnings:
- Pane splitter — splitter bar visibility and drag handle sizing between tree and file grid
- Tree indentation — pixels-per-level in folder tree consistent with provider spacing scale
- File grid density — item sizing and icon-to-label spacing in grid and list views
- Icon sizing — file and folder icon dimensions may not scale with provider token
- Breadcrumb separator — separator character weight and spacing
- Context menu chrome — border, background, shadow, and item hover in dark mode
- Toolbar spacing — button spacing relative to provider token scale
- Dark mode surfaces — tree pane, file grid, and toolbar background tints

## Known Unknowns

- Grid view vs. list view toggle — both views need separate capture points
- Context menu item count and grouping not confirmed
- Upload area implementation (drag-drop zone vs. button vs. panel) not confirmed
- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures blocked

## Blockers

- **Material runtime provider not yet implemented** (SCSS-only scaffold as of 2026-04-10) — Material Light and Material Dark captures cannot proceed
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
