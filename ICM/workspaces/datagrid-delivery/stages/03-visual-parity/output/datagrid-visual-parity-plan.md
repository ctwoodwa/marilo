# Visual Parity Plan -- MariloDataGrid

## Component

MariloDataGrid

## Reference Strategy

**Telerik Grid parity.** Telerik's Blazor Grid provides the visual reference baseline for DataGrid states including default rendering, sorting indicators, filtering UI, grouping, editing, selection, and paging. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Default grid (data at rest) | Baseline visual impression |
| P1 | Header row | Typography and structure anchor |
| P1 | Row hover | Most common interaction state |
| P1 | Selected row | Primary selection visual |
| P2 | Sorted ascending | Sort indicator quality |
| P2 | Sorted descending | Sort indicator quality |
| P2 | Filter row | Inline filter density |
| P2 | Filter menu/popover | Popup chrome quality |
| P2 | Pager idle | Density and alignment |
| P3 | Checkbox selection | Alignment precision |
| P3 | Grouped state | Group header treatment |
| P3 | Inline edit row | Edit mode density |
| P3 | Empty state | Empty message styling |
| P3 | Loading state | Skeleton/spinner quality |
| P4 | Popup edit dialog | Overlay chrome |
| P4 | Pager hover/active/focus | Interactive pager states |
| P4 | Toolbar/command area | Toolbar integration |
| P4 | Virtualization/dense scroll | Performance-adjacent visual |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on cerebrum learnings:
- Dark-mode token gaps (known issue: `--marilo-color-subtle-background` etc. missing from dark blocks)
- Bootstrap bridge dark-mode mechanism (`[data-marilo-theme="dark"]` vs `[data-bs-theme="dark"]`)
- `color-mix()` base color must use `var(--marilo-color-surface)` not hardcoded `#ffffff`
- Row hover/stripe using black tints invisible on dark surfaces
- Table semantics rule: provider styles must not set `display:flex` on table rows

## Known Unknowns

- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures may be blocked
- Filter menu popup positioning and chrome quality not yet audited
- Virtualization visual behavior under large datasets not confirmed
- Toolbar/command area design not finalized

## Blockers

- Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10)
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes
