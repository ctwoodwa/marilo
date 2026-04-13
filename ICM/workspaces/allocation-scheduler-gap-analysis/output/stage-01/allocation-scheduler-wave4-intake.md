# Wave 4 Intake: MariloAllocationScheduler — Second-Cycle R1..R12

**Intake date:** 2026-04-11
**Cycle:** Second (Phase 1 previously closed 2026-04-05 per `output/stage-06/closure-report.md`)
**Scope:** batch (12 remediation lanes surfaced in Wave 4 delivery-pipeline re-run)
**Source:** `ICM/workspaces/allocation-scheduler-delivery/stages/04-sync-check/output/allocation-scheduler-delivery-report.md` §Wave 4 (gate **AMBER**, 0 workspace blockers)
**Decisions inherited:** `.claude/orchestration/_orchestrator/decisions/tick-8-2026-04-11-1830.md` (cross-component patterns 2/3/4/5)
**Worker:** `w-allocation-scheduler-gap-analysis`
**Session:** marilo-grid-pipeline-2026-04-11-1200

---

## Context & Why This Second Cycle Exists

MariloAllocationScheduler was the **healthiest of the four advanced components in Wave 4** (delivery report §Wave 4 — Gate Verdict). It did not BLOCK, and the first-cycle Phase 1 closure (2026-04-05, §`output/stage-06/closure-report.md`) resolved all 14 P3 demo gaps and test-coverage gaps. The 2026-04-11 Wave 4 sync-check re-run surfaced **12 fresh remediation lanes (R1..R12)** that were not present at first-cycle close:

- New visual-parity findings from Wave 3 (VP-allocation-scheduler-001..020).
- New Wave 1 spec-re-audit findings (SPEC-AS-W1-010..021, 12 records).
- Two demo-coverage Missing topics from Wave 2 (F1 accessibility, F2 theming).
- A cross-component `#fff` fallback literal anti-pattern with 19 instances in this component's Fluent SCSS.

This intake layers those 12 lanes onto the existing Phase 1 workspace as a second-cycle batch. **It does NOT overwrite `gap-inventory.md` or `closure-report.md`.** The first-cycle artifacts remain authoritative for their scope.

**Ingestion note.** No conflict detected between first-cycle closure and Wave 4 findings. First-cycle closed `GAP-P3-001..012 + GAP-TEST-001..002` (demo and test coverage). R1..R12 are a disjoint set: visual parity, spec re-audit, new demo topics, and cross-component hygiene. No record from this cycle re-opens a closed first-cycle gap.

---

## R1..R12 Lane Breakdown

Priority, scope, sync areas, and cross-component routing for every lane. Sources cite delivery report §Wave 4 — Open Remediation Items (rows R1..R12 in the delivery report are preserved here 1:1 so downstream Stage 02 prioritization has a clean map).

### R1 — Dark-mode invisible cell-edit text (runtime correctness)

| Field | Value |
|---|---|
| `lane_id` | R1 |
| `title` | Dark-mode cell-edit input has no explicit `color:` declaration → invisible text in Fluent Dark |
| `origin_wave4_record` | VP-allocation-scheduler-006 (Wave 3 visual parity, 03-visual-parity) |
| `priority` | **P1** (runtime correctness bug, not cosmetic) |
| `scope` | single |
| `sync_areas` | source, tests, gap-plan |
| `rationale` | User enters edit mode in Fluent Dark and can't see what they're typing. Non-blocking at workspace level (AMBER) but user-visible defect. Delivery report §Why not CLEAR item 1. |
| `cross_component` | **yes-route-to-dark-mode-hygiene-lane** (cerebrum Pattern 2: `_dark-mode.scss` partial should be mandatory per component; this is the symptom) — remediation still owned in-workspace because it requires an explicit `color: var(--marilo-color-text)` rule on the cell-edit input selector, which is allocation-scheduler-specific CSS. Route the **systemic convention** to cross-component lane; keep the **fix** here. |

### R2 — Hidden-scrollbar a11y blocker

| Field | Value |
|---|---|
| `lane_id` | R2 |
| `title` | `scrollbar-width: none` + `::-webkit-scrollbar { display: none }` blocks keyboard and screen-reader scroll discovery |
| `origin_wave4_record` | VP-allocation-scheduler-020 (Wave 3 visual parity) |
| `priority` | **P1** (a11y correctness, Wave-2 carry-forward) |
| `scope` | single |
| `sync_areas` | source, tests, gap-plan |
| `rationale` | Replace hidden scrollbar with a styled-visible scrollbar so keyboard + SR users can discover that the region scrolls. Delivery report §Why not CLEAR item 2. |
| `cross_component` | no (allocation-scheduler-specific SCSS selector, but verify in Stage 02 whether gantt/scheduler/datagrid share the same hide-scrollbar pattern — if so, promote to systemic lane at that time) |

### R3 — Missing `AccessibilityDemo.razor` demo page

| Field | Value |
|---|---|
| `lane_id` | R3 |
| `title` | Wave 2 `accessibility` topic is Missing — no demo page exercises keyboard walkthrough or ARIA live-region logging |
| `origin_wave4_record` | Wave 2 F1 (02-example-ux topic matrix) |
| `priority` | **P1** (sustains AMBER; Missing topic) |
| `scope` | single |
| `sync_areas` | demo, docs, gap-plan |
| `rationale` | New razor file `samples/Marilo.Demo/Pages/Components/AllocationScheduler/AccessibilityDemo.razor`. No source changes. Delivery report §Why not CLEAR item 3. |
| `cross_component` | no (demo-page shape is component-specific) |

### R4 — Missing `ThemingDemo.razor` demo page

| Field | Value |
|---|---|
| `lane_id` | R4 |
| `title` | Wave 2 `theming` topic is Missing — no demo page exercises provider swap + dark/light toggle |
| `origin_wave4_record` | Wave 2 F2 (02-example-ux topic matrix) |
| `priority` | **P1** (sustains AMBER; Missing topic) |
| `scope` | single |
| `sync_areas` | demo, docs, gap-plan |
| `rationale` | New razor file `samples/Marilo.Demo/Pages/Components/AllocationScheduler/ThemingDemo.razor`. No source changes. Delivery report §Why not CLEAR item 3. |
| `cross_component` | no (demo-page shape is component-specific; however, verify Stage 02 whether datagrid/gantt/scheduler Wave-2 matrices also list `theming` as Missing — if yes, a shared `ThemingDemo` helper could be factored later, but that's a different kind of dedup, not a cross-component remediation lane) |

### R5 — Fluent-dark `#fff` fallback literal sweep (19 instances)

| Field | Value |
|---|---|
| `lane_id` | R5 |
| `title` | `color-mix(..., var(--marilo-color-surface, #ffffff))` fallback literal across 19 rules in `src/Marilo.Providers.FluentUI/Styles/components/allocation-scheduler/*.scss` → white-bleed-through risk in Fluent Dark |
| `origin_wave4_record` | VP-allocation-scheduler-003 + VP-allocation-scheduler-007 (Wave 3 visual parity) |
| `priority` | **P1** (delivery report §Why not CLEAR item 4) |
| `scope` | systematic |
| `sync_areas` | source, tests, gap-plan |
| `rationale` | One-class-of-bug with widespread impact. Tick-8 Pattern 4 mandates global find-replace `#fff` → `var(--mar-color-surface, #fff)`. 19 instances in allocation-scheduler alone. |
| `cross_component` | **yes-route-to-fff-literal-replace-lane** (Tick-8 cerebrum Pattern 4). This is the canonical candidate for the single orchestrator-dispatched lane described in Pattern 4. AllocationScheduler contributes 19 instances; datagrid/gantt/scheduler likely contribute more. The LANE remediates all components; this workspace only tracks the allocation-scheduler subset for accounting. |

### R6 — Duplicate root-level SCSS files (prerequisite to R5)

| Field | Value |
|---|---|
| `lane_id` | R6 |
| `title` | Mechanical delete of byte-identical root-level `_<component>.scss` copies that duplicate `Styles/components/allocation-scheduler/` |
| `origin_wave4_record` | VP-allocation-scheduler-019 (Wave 3 visual parity) |
| `priority` | **P1-prereq** (must land before R5 or the `#fff` sweep diverges from its duplicate) |
| `scope` | single |
| `sync_areas` | source, gap-plan |
| `rationale` | Delivery report §Wave 4 Open Remediation R6 notes this as a prerequisite-class item. Tick-8 Pattern 3 mandates SCSS lives ONLY in `Styles/components/`, and duplicates are verified byte-identical for gantt and allocation-scheduler. |
| `cross_component` | **yes-route-to-scss-dedup-lane** (Tick-8 Pattern 3). Single orchestrator-dispatched lane with `files_owned` limited to the duplicate files to delete. This workspace only accounts for the allocation-scheduler subset. **Action required in Stage 02:** verify the byte-identical assertion against current disk state before dispatching R5, and route via the systemic dedup lane rather than an allocation-scheduler-local fix. |

### R7 — Conflict indicator iconography + ARIA label

| Field | Value |
|---|---|
| `lane_id` | R7 |
| `title` | Conflict indicator uses raw glyph; needs MariloIcon accent + ARIA label |
| `origin_wave4_record` | VP-allocation-scheduler-011 (Wave 3 visual parity) |
| `priority` | **P2** |
| `scope` | single |
| `sync_areas` | source, spec, demo, tests, gap-plan |
| `rationale` | Delivery report §Wave 4 Open Remediation R7: source + razor edit. Uses MariloIcon accent. Needs ARIA label for SR users. Spec needs a line about the icon semantic; demo should show a conflict scenario. |
| `cross_component` | no (allocation-scheduler-specific UI element) |

### R8 — Spec-writer batch: SPEC-AS-W1-010..021 (12 spec-update-only records)

| Field | Value |
|---|---|
| `lane_id` | R8 |
| `title` | Wave-1 spec re-audit batch: 12 spec-update-only records (3× P1, 6× P2, 3× P3) |
| `origin_wave4_record` | SPEC-AS-W1-010..021 (01-spec-review, 2026-04-11 audit) |
| `priority` | **P1 (3) / P2 (6) / P3 (3)** — aggregate P1 for the lane because at least one P1 item (SPEC-AS-W1-010 type-name mismatch) affects consumer-compilable code snippets |
| `scope` | batch |
| `sync_areas` | spec, docs, gap-plan |
| `rationale` | Delivery report §API Spec AMBER-PASS summary. Source is ahead; spec needs catch-up pass. All records are spec-update-only (no source rework). SPEC-AS-W1-010 = `AllocationScenarioStatus` vs `ScenarioStatus` type name; W1-012 = `OnTimeColumnResized` spec-ahead addition; W1-013 = `DefaultRangeLength` marked obsolete in source but not spec; W1-015 = ShowJumpToDate; W1-016 = grouped headers; W1-017 = current-period highlight; W1-018 = dynamic column fill. |
| `cross_component` | no (spec files are component-specific; this is a normal batched spec-writer lane) |

### R9 — Material provider stubs (explicitly out-of-scope)

| Field | Value |
|---|---|
| `lane_id` | R9 |
| `title` | `src/Marilo.Providers.Material/Styles/components/_allocation-scheduler.scss` is a 5-line TODO placeholder |
| `origin_wave4_record` | VP-allocation-scheduler-001 + VP-allocation-scheduler-002 (Wave 3 visual parity — Material 0.0/0.0) |
| `priority` | **Out-of-scope** — not a Wave 4 regression; not a gap-analysis workspace concern |
| `scope` | (n/a — do not dispatch) |
| `sync_areas` | (n/a) |
| `rationale` | Delivery report §Why not BLOCKED item 1: Material is a pre-existing matrix blocker, not a delivery-pipeline regression. Tick-8 Pattern 5: Material 5-line-stub is accepted technical debt; agents MUST NOT expand stubs in the current wave. |
| `cross_component` | **yes-route-to-material-tech-debt-tracker** (Tick-8 Pattern 5). File registration goes to `docs/provider-material/OPEN-STUBS.md` when that tracker lane is dispatched. **Not this workspace's work.** Record the routing so Stage 02 prioritization does not accidentally schedule R9 work. |

### R10 — Drag-fill preview solid fill → dashed outline

| Field | Value |
|---|---|
| `lane_id` | R10 |
| `title` | Drag-fill preview currently uses solid fill; should be dashed outline pattern |
| `origin_wave4_record` | VP-allocation-scheduler-012 (Wave 3 visual parity) |
| `priority` | **P2** (state-treatment quality gap, non-blocking) |
| `scope` | single |
| `sync_areas` | source, tests, gap-plan |
| `rationale` | Delivery report §Wave 4 Open Remediation R10: SCSS change to match Fluent pattern guidance for preview state. Low risk, low effort. |
| `cross_component` | no (allocation-scheduler-specific drag-fill affordance) |

### R11 — Context-menu shadow hardcoded rgba → elevation token

| Field | Value |
|---|---|
| `lane_id` | R11 |
| `title` | Context menu shadow uses hardcoded `rgba(0,0,0,0.15)`; should route to `--marilo-shadow-*` elevation token |
| `origin_wave4_record` | VP-allocation-scheduler-004 (Wave 3 visual parity) |
| `priority` | **P2** (elevation token hygiene) |
| `scope` | single |
| `sync_areas` | source, tests, gap-plan |
| `rationale` | Delivery report §Wave 4 Open Remediation R11: token routing so provider swap actually changes shadow elevation. Same shape as R5 (literal → token) but for shadow rather than color; not bundled with R5 because shadow tokens are a separate family. |
| `cross_component` | no at lane level — but **note** for Stage 02: if a systematic "hardcoded rgba → elevation token" pattern emerges across datagrid/gantt/scheduler, promote to a shadow-token sweep similar to R5. Not this workspace's call to dispatch. |

### R12 — Bootstrap disabled-cell stripes invisible on dark

| Field | Value |
|---|---|
| `lane_id` | R12 |
| `title` | Bootstrap disabled-cell stripe color is invisible on dark background |
| `origin_wave4_record` | VP-allocation-scheduler-008 (Wave 3 visual parity) |
| `priority` | **P2** (state-treatment, Bootstrap dark only) |
| `scope` | single |
| `sync_areas` | source, tests, gap-plan |
| `rationale` | Delivery report §Wave 4 Open Remediation R12: SCSS change in the Bootstrap bridge file: token-aware stripe color. Fluent dark is not affected (different bridge file). |
| `cross_component` | **maybe-route-to-dark-mode-hygiene-lane** (Tick-8 Pattern 2 indirect). Root cause is the same family as R1 (dark-mode hygiene missing token-aware treatment), but the specific fix is in a different provider bridge file, so keep the fix local and cross-reference Pattern 2 at convention-enforcement time. |

---

## Cross-Component Routing Summary

Lanes in this intake that must be handled by a **cross-component orchestrator-dispatched lane**, not by this workspace in isolation:

| Pattern | AllocScheduler lanes | Cross-component lane | Action |
|---|---|---|---|
| `_dark-mode.scss` partial mandatory convention (Tick-8 Pattern 2) | R1 (primary symptom), R12 (indirect) | Cross-component **dark-mode hygiene lane** | Route the **convention enforcement** (CI presence check) to the systemic lane. Keep the **per-component fix** (R1, R12) in this workspace because the selectors and tokens differ per component. |
| `#fff` fallback literal → `var(--mar-color-surface, #fff)` (Tick-8 Pattern 4) | R5 (19 instances in allocation-scheduler) | Cross-component **`#fff` replace lane** | Route **entire R5 remediation** to the systemic lane. This workspace accounts for 19 instances; lane remediates all four advanced components in a single pass. |
| Duplicate SCSS: root-level `_<component>.scss` = byte-identical copy of `Styles/components/<component>/` (Tick-8 Pattern 3) | R6 | Cross-component **SCSS dedup lane** | Route **entire R6 remediation** to the systemic lane. Must land before R5 (prerequisite — avoids divergence during `#fff` sweep). |
| Material 5-line stub systemic debt (Tick-8 Pattern 5) | R9 | **Material tech-debt tracker** (not this workspace, not this wave) | Register VP-001/VP-002 in `docs/provider-material/OPEN-STUBS.md` when the Material tracker lane is dispatched. **Do NOT dispatch R9 from this workspace.** |

**None of the four patterns above authorize this worker to edit cross-component lane files.** The routing is recorded here so Stage 02 prioritization knows which lanes are deferred to cross-component dispatch versus handled in-workspace.

---

## Lane Sequencing

Suggested dispatch order for Stage 04 remediation planning (when Stage 02/03 reach it). Parallel vs. dependent relationships are called out explicitly.

### Phase A — Prerequisites and hygiene (must land first)

**R6** — SCSS dedup (cross-component routing).

- **Blocks:** R5. Running R5 before R6 would double-edit the duplicate root-level SCSS and create divergence.
- **Dispatch:** cross-component SCSS dedup lane (orchestrator-owned).
- **Cannot parallelize with R5.**

### Phase B — Systematic sweeps (can run in parallel after Phase A)

**R5** — `#fff` literal replace (cross-component routing).

- **Depends on:** R6 complete.
- **Parallel with:** R8 (spec writer), R3 (AccessibilityDemo), R4 (ThemingDemo) — all disjoint file ownership.
- **Dispatch:** cross-component `#fff` replace lane (orchestrator-owned).

**R8** — spec re-audit batch (12 records, spec-update-only).

- **Depends on:** none.
- **Parallel with:** R5, R3, R4, R1, R2, R7, R10, R11, R12 (no file overlap — spec files vs. SCSS files vs. razor files are disjoint).
- **Dispatch:** this workspace, Stage 04 spec-writer lane.

**R3** — AccessibilityDemo.razor.

- **Depends on:** none.
- **Parallel with:** R4, R8, R5, most others.
- **Dispatch:** this workspace, Stage 04 demo-writer lane.

**R4** — ThemingDemo.razor.

- **Depends on:** none.
- **Parallel with:** R3, R8, R5, most others.
- **Dispatch:** this workspace, Stage 04 demo-writer lane.

### Phase C — P1 source-change lanes (after Phase B settles)

**R1** — Dark-mode invisible cell-edit text.

- **Depends on:** R5 complete (otherwise the fix in R1 could collide with the `#fff` sweep). Soft dependency — can run in parallel if and only if R1's `files_owned` excludes the selectors R5 touches.
- **Sync:** source + tests.
- **Dispatch:** this workspace, Stage 04 source-change lane.

**R2** — Hidden-scrollbar a11y fix.

- **Depends on:** none.
- **Parallel with:** R1 (disjoint SCSS selectors).
- **Sync:** source + tests.
- **Dispatch:** this workspace, Stage 04 source-change lane.

### Phase D — P2 polish (after P1 complete)

Can run in parallel with each other if file ownership is disjoint:

- **R7** — Conflict indicator icon + ARIA. Source + spec + demo + tests.
- **R10** — Drag-fill dashed outline. SCSS + tests.
- **R11** — Context-menu elevation token. SCSS + tests.
- **R12** — Bootstrap disabled-stripes dark. SCSS (Bootstrap bridge) + tests.

R11 and R10 both touch Fluent SCSS so verify `files_owned` does not collide before parallelizing.
R12 is in the Bootstrap bridge — disjoint from R5/R6/R1/R10/R11 (which are Fluent).

### Phase E — Deferred / out-of-scope

**R9** — Material VP-001/VP-002. **Do not dispatch from this workspace.** Route to Material tech-debt tracker when that lane exists.

### Parallelism summary

| Phase | Lanes | Runs-in-parallel-with | Depends-on |
|---|---|---|---|
| A | R6 | — | — |
| B | R5 | R8, R3, R4 | R6 |
| B | R8 | R5, R3, R4 | — |
| B | R3 | R5, R8, R4 | — |
| B | R4 | R5, R8, R3 | — |
| C | R1 | R2 (if disjoint) | R5 (soft) |
| C | R2 | R1 | — |
| D | R7, R10, R11, R12 | each other if disjoint files_owned | Phase C complete |
| E | R9 | — | Material tracker lane (not this workspace) |

**Total lanes this workspace owns:** R1, R2, R3, R4, R7, R8, R10, R11, R12 = **9 lanes**.
**Lanes routed cross-component:** R5, R6 = **2 lanes**.
**Lanes out-of-scope:** R9 = **1 lane**.
**Total accounted:** 12 / 12.

---

## Non-Conflict Statement vs. First-Cycle Closure

Verified disjointness with the first-cycle Phase 1 artifacts (read-only):

- **`output/stage-01/gap-inventory.md` §A/B** — first cycle tracked `GAP-P3-001..012` (demo scenarios for 5 parameters + 7 events) and `GAP-TEST-001..002` (test coverage). **None of R1..R12 overlap** these IDs.
- **`output/stage-06/closure-report.md`** — all 14 first-cycle records CLOSED 2026-04-05 against AdvancedFeatures.razor scenarios + 13 new bUnit tests. R1..R12 do not re-open any closed record.
- **Sync-area hygiene** — first-cycle touched `demo`, `tests`. R1..R12 span `source`, `spec`, `demo`, `tests`, `docs`, `gap-plan`. No sync-area regression.

If a Stage 02 prioritization pass finds a conflict I missed, escalate with type `architecture-question`.

---

## What This Intake Does NOT Do

- **Does NOT prioritize across lanes globally** — that's Stage 02's job. Priorities listed here are inherited from the delivery report.
- **Does NOT design any remediation** — that's Stage 03's job.
- **Does NOT dispatch any R-lane** — intake only.
- **Does NOT touch `gap-inventory.md` or `closure-report.md`** — first-cycle artifacts are read-only to this worker.
- **Does NOT touch source, spec, demo, docs, or test files** — read-only audit.
- **Does NOT open Material stub work** — R9 is out of scope per Tick-8 Pattern 5.

---

## Verification

- **Files read (read-only):**
  - `ICM/workspaces/allocation-scheduler-delivery/stages/04-sync-check/output/allocation-scheduler-delivery-report.md` (full) — R1..R12 table extracted from §Wave 4 — Open Remediation Items.
  - `.claude/orchestration/_orchestrator/decisions/tick-8-2026-04-11-1830.md` (full) — Cross-Component Patterns 2/3/4/5 extracted.
  - `ICM/workspaces/allocation-scheduler-gap-analysis/CLAUDE.md` — confirmed Phase 1 workspace state.
  - `ICM/workspaces/allocation-scheduler-gap-analysis/output/stage-01/gap-inventory.md` (head, 50 lines) — confirmed first-cycle IDs are disjoint from R1..R12.
  - `ICM/workspaces/allocation-scheduler-gap-analysis/output/stage-06/closure-report.md` (head, 30 lines) — confirmed first-cycle closure status.
  - `.claude/orchestration/_memory/workers/w-allocation-scheduler-gap-analysis.json` — confirmed worker state, files_owned, files_read_only.
  - `.claude/orchestration/_orchestrator/inbox/w-allocation-scheduler-gap-analysis.md` — confirmed scope, hard stops, required skills.
- **Lane count:** 12 / 12 (R1..R12 covered).
- **Cross-component routing:** 4 / 4 patterns (dark-mode hygiene, `#fff` replace, SCSS dedup, Material tracker) all mapped.
- **Lane sequencing:** 5 phases (A–E) with explicit dependency + parallel callouts.
- **Existing file mutation:** NONE. `gap-inventory.md` and `closure-report.md` untouched.
- **Build/test:** N/A for read-only intake (`verification-before-completion` skill: this is an audit turn, not a code turn; no `dotnet build` / `dotnet test` applicable).
- **Skill discipline:**
  - `test-driven-development` — not applicable (no source/test edit).
  - `verification-before-completion` — satisfied via the read-citations above and the lane-count check.
  - `systematic-debugging` — not triggered (no failure encountered).
  - `requesting-code-review` — the result file is shaped per the skill template.
  - `receiving-code-review` — will be honored if Stage 01 review returns FAIL.

**End of Stage 01 second-cycle intake. STOP.**
