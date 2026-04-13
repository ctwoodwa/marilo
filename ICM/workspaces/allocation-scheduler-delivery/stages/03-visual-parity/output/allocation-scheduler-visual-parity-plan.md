# Visual Parity Plan -- MariloAllocationScheduler

## Component

MariloAllocationScheduler

## Reference Strategy

**Telerik Scheduler (resource scheduling variant) parity.** Telerik's Blazor Scheduler configured for resource allocation provides the visual reference baseline for AllocationScheduler states including resource panels, timeline headers, bucket cells, drag-fill, splitter, grouped timelines, conflict indicators, and current-period highlights. Marilo targets visual quality equivalence, not feature cloning.

## Prioritized Scenarios (First Pass)

Capture and score these scenarios first — they represent the most-seen states:

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Default view | Baseline visual impression of full layout |
| P1 | Resource panel | Primary structural anchor for allocation layout |
| P1 | Timeline header | Typography and grouping density anchor |
| P1 | Occupied cell | Most common data state |
| P2 | Cell hover | Most common interaction state |
| P2 | Splitter at rest | Layout quality — alignment between panels |
| P2 | Current period highlight | High-visibility state treatment |
| P2 | Disabled cell | Secondary state treatment quality |
| P3 | Cell editing | Edit chrome density |
| P3 | Selected allocation | Selection visual |
| P3 | Conflict indicator | Severity treatment quality |
| P3 | Drag-fill in progress | Interaction preview quality |
| P4 | Empty resource | Edge state styling |
| P4 | Loading state | Skeleton/spinner quality |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage (known color-mix issue)
3. **Bootstrap Light** — validates bridge token mapping
4. **Bootstrap Dark** — validates bootstrap dark-mode patches
5. **Material Light** — newest provider, likely most gaps
6. **Material Dark** — newest + dark = highest gap density expected

## Known Gap Categories to Watch

Based on component structure and cerebrum learnings:
- Splitter alignment between resource panel and timeline pane
- Bucket cell density — cell height and padding compactness vs. Telerik reference
- Resource column width default and text truncation behavior
- Timeline header typography weight and tier separator lines
- Cell editing chrome — input sizing and confirm/cancel button placement
- Drag-fill indicators — ghost preview color and fill handle sizing
- Dark mode tints — `color-mix()` base color must use `var(--marilo-color-surface)` not hardcoded `#ffffff`
- Conflict indicator overlay — color and icon treatment not yet audited

## Known Unknowns

- Conflict indicator visual design not finalized — no Telerik exact equivalent
- Drag-fill ghost preview behavior under dark mode not confirmed
- Timeline header grouping depth (2-tier vs. 3-tier) not confirmed
- Material provider SCSS is scaffolded but runtime project does not exist yet — Material captures blocked

## Blockers

- **Material runtime provider not yet implemented** (SCSS-only scaffold as of 2026-04-10) — Material Light and Material Dark captures cannot proceed
- Stage 02 (Example UX) output needed for demo scenario list — check if available before proceeding

## Next Steps After Plan

1. Confirm Stage 02 output exists or create baseline demo scenarios
2. Set up Playwright capture scripts for automated screenshot collection
3. Execute first-pass Fluent Light review across P1 scenarios
4. Document gaps and iterate through remaining themes/modes

---

## 2026-04-11 Wave-3 execution note (orchestrator tick 6)

Stage 03 executed this tick as a **static-analysis pass** rather than a Playwright capture pass, because:

- Material provider runtime is still absent (confirmed in Blockers above) — captures for 2 of 6 theme/mode combinations are impossible without building the provider first.
- Stage 02 Wave-2 output is available and all 8 demo files catalogued, so the capture targets are known.
- The four primary SCSS source files (Fluent component + root duplicate, Bootstrap component + root duplicate, Material placeholder, razor markup) contained enough evidence for 20 concrete gap records at the rubric's 1–2 score band without screenshots.

Static-analysis scope delivered:

- `allocation-scheduler-visual-parity-gaps.md` — 20 gap records (target was 10–20), covering all 9 structural elements named in the wave-3 scope × 3 themes × 2 modes to the degree each combination produced distinct findings.
- `allocation-scheduler-parity-summary.md` — score matrix, worst-offenders ranking, category roll-up, Wave-2 carry-forward resolution, gate check.
- Wave-2 Missing topics (`accessibility`, `theming`) surfaced as four implementation-visual gaps each — NOT deferred.

Carry-over for Wave 4 (sync-check):

- Verify the gap list reconciles against the Wave-1 spec gap list (a11y gaps here should map to `accessibility.md` spec entries).
- Material runtime implementation remains the hard blocker on the 6-way parity closure — escalate during sync-check if still unresolved.
- Fluent color-mix fallback literal issue (VP-003, VP-007) is systemic and likely shared across gantt/datagrid/scheduler — worth cross-workspace referral.
