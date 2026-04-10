# Visual Parity Plan -- MariloTreeList

## Component

MariloTreeList

## Reference Strategy

**Telerik TreeList parity.** Telerik's Blazor TreeList provides the visual reference baseline for TreeList states including hierarchical rows, expand/collapse, inline editing, sorting, filtering, paging, checkbox selection, and row interaction states. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Default view | Baseline visual impression |
| P1 | Hierarchical rows | Core differentiator — indentation quality |
| P1 | Row hover | Most common interaction state |
| P1 | Selected row | Primary selection visual |
| P2 | Header row | Typography and structure anchor |
| P2 | Expanded row | Tree expand state treatment |
| P2 | Collapsed row | Tree collapse state treatment |
| P2 | Sorted column | Sort indicator quality |
| P3 | Checkbox selection | Alignment precision |
| P3 | Filter row | Inline filter density |
| P3 | Pager idle | Density and alignment |
| P3 | Inline edit row | Edit mode density |
| P4 | Pager hover/active | Interactive pager states |
| P4 | Empty state | Empty message styling |
| P4 | Loading state | Skeleton/spinner quality |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component structure and cerebrum learnings:
- Indentation per level — pixels-per-level must be consistent and match provider spacing scale
- Expand icon alignment — vertical centering within the tree cell at all row densities
- Row density — row height may differ from DataGrid reference; TreeList has its own density target
- Header typography — font weight and column header case treatment
- Filter/sort indicators — sort arrow sizing and filter icon placement
- Pager compactness — button sizing relative to provider token scale
- Dark mode row tints — hover and selected must use surface token not hardcoded black tints

## Known Unknowns

- Maximum supported hierarchy depth not confirmed — test at 3+ levels minimum
- Filter row input sizing behavior under narrow viewports not audited
- Checkbox selection behavior with mixed parent/child states not confirmed
- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures blocked

## Blockers

- **Material runtime provider not yet implemented** (SCSS-only scaffold as of 2026-04-10) — Material Light and Material Dark captures cannot proceed
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
