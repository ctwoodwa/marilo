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

## 2026-04-11 Wave 3 Update — Static-Analysis Pass Complete

**Worker:** w-datagrid-delivery (Wave 3)
**Output:** See `datagrid-visual-parity-gaps.md` (20 records) and `datagrid-parity-summary.md`.

Instead of waiting for a Playwright capture pipeline, Wave 3 executed a pure static-analysis pass over the DataGrid SCSS (FluentUI + Bootstrap + Material) and the component razor. Major new learning:

- **The headline issue is unstyled selectors, not token drift.** Nine+ razor-emitted classes (`.mar-datagrid-pager-btn*`, `.mar-datagrid-popup-*`, `.mar-datagrid-empty`, `.mar-datagrid-loading-*`, `.mar-datagrid-sort-indicator`, `.mar-datagrid-checkbox-cell`, `.mar-datagrid-detail-row`, `.mar-datagrid-footer-*`, `.mar-datagrid-col--locked`) have zero matching SCSS rules in either provider. A single SCSS pass can lift 7–8 gap records by a full score each.
- **Dark-mode token collisions confirmed** on `--marilo-color-surface` (used as header, stripe, AND hover fill). Introducing a dedicated `--marilo-color-state-hover` fixes VP-datagrid-001/002/003/004 in one stroke.
- **Hardcoded `#fff`/`#ffffff` literals confirmed** in FluentUI filter-menu popover (4 occurrences) and Bootstrap filter-menu (3 occurrences). Matches cerebrum learning about `color-mix` base color requiring `var(--marilo-color-surface)`.
- **Material provider is a 5-line TODO placeholder.** All 6 Material state/mode slots score 0 by default. Requires new provider track — not a SCSS patch.

**Revised next steps:**

1. Orchestrator review of the 20 gap records and parity summary.
2. Route the unstyled-selector cluster to `datagrid-gap-analysis` as a single "DataGrid provider visual gap batch" intake.
3. Route Material provider scaffolding to its own gap-analysis track (not Wave 3 scope).
4. Stand up the Playwright capture pipeline after the first remediation pass lands — the 20 DEFERRED-TO-CAPTURE entries are ready to execute once the unstyled selectors have at least baseline rules.
5. Re-score Wave 3 after remediation; target is Fluent Light ≥ 2.5 average on primary states.

