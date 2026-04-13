# Wave 4 Priority Lanes: MariloAllocationScheduler — Second-Cycle R1..R12

**Stage:** 02-prioritize (second cycle)
**Date:** 2026-04-11
**Input:** `output/stage-01/allocation-scheduler-wave4-intake.md` (12 R-lanes, review PASS)
**Worker:** `w-allocation-scheduler-gap-analysis`
**Session:** marilo-grid-pipeline-2026-04-11-1200

---

## Lane Classification Summary

| Category | Lanes | Count |
|---|---|---|
| **In-workspace** (this worker dispatches) | R1, R2, R3, R4, R7, R8, R10, R11, R12 | 9 |
| **Cross-component** (orchestrator dispatches) | R5, R6 | 2 |
| **Out-of-scope / locked out** | R9 | 1 |
| **Total accounted** | R1..R12 | **12** |

---

## Dependency Graph

```
                  ┌──────────────────────────────────────────────────┐
                  │  CROSS-COMPONENT (orchestrator-owned)            │
                  │                                                  │
                  │  R6 (SCSS dedup)                                 │
                  │    │                                             │
                  │    ▼                                             │
                  │  R5 (#fff sweep, 19 instances)                   │
                  │    │                                             │
                  └────┼──────────────────────────────────────────────┘
                       │ soft-dep
                       ▼
  ┌─────────────────────────────────────────────────────────────────┐
  │  IN-WORKSPACE                                                   │
  │                                                                 │
  │  Priority Lane 1 (independent, no deps):                        │
  │    R2  R3  R4  R8                                               │
  │                                                                 │
  │  Priority Lane 2 (soft-dep on R5):                              │
  │    R1 ◁ ─ ─ (soft) ─ ─ R5                                      │
  │                                                                 │
  │  Priority Lane 3 (after P1 source lanes settle):                │
  │    R7  R10  R11  R12                                            │
  │                                                                 │
  └─────────────────────────────────────────────────────────────────┘

  ┌─────────────────────────────────────────────────────────────────┐
  │  LOCKED OUT                                                     │
  │    R9 (Material stubs — Tick-8 Pattern 5)                       │
  └─────────────────────────────────────────────────────────────────┘
```

### Dependency edges (exhaustive list)

| From | To | Type | Rationale |
|---|---|---|---|
| R6 | R5 | **hard** | SCSS dedup MUST land before `#fff` sweep; otherwise sweep edits duplicate files and dedup loses one copy's changes |
| R5 | R1 | **soft** | `#fff` sweep may flip the exact token R1 needs (`color: var(--marilo-color-text)`). If R5 lands first, R1's fix MAY be absorbed into the surface-token flip. If R1 runs first, no harm — R5 sweep skips selectors already tokenized. |
| Phase C | Phase B | **phase-gate** | P1 source-change lanes (R1, R2) should run after Phase B sweeps settle to avoid merge conflicts in shared SCSS files |
| Phase D | Phase C | **phase-gate** | P2 polish lanes (R7, R10, R11, R12) run after P1 source lanes to avoid churn |

No other inter-lane dependencies exist. All remaining lanes have disjoint file ownership.

---

## Priority Lane Definitions

### Priority Lane 1: Independent Work (no external deps)

These lanes can begin immediately and run in parallel with each other. They have no dependency on cross-component lanes R5/R6.

#### PL1-A: R8 — Spec Re-Audit Batch

| Field | Value |
|---|---|
| `lane_id` | R8 |
| `priority` | **P1** (aggregate: 3x P1, 6x P2, 3x P3 records) |
| `dispatch_phase` | **B** (immediate) |
| `scope` | batch (12 spec-update-only records: SPEC-AS-W1-010..021) |
| `sync_areas` | spec, docs, gap-plan |
| `files_touched` | `docs/component-specs/allocation-scheduler/**/*.md` |
| `depends_on` | none |
| `parallel_with` | R3, R4, R2 |
| `effort` | medium (12 spec records, text-only edits) |
| `notes` | Highest-value immediate lane: P1 type-name mismatch (SPEC-AS-W1-010 `AllocationScenarioStatus` vs `ScenarioStatus`) affects consumer-compilable code snippets. No source changes required. |

#### PL1-B: R3 — AccessibilityDemo.razor

| Field | Value |
|---|---|
| `lane_id` | R3 |
| `priority` | **P1** (sustains AMBER; Missing demo topic) |
| `dispatch_phase` | **B** (immediate) |
| `scope` | single |
| `sync_areas` | demo, docs, gap-plan |
| `files_touched` | `samples/Marilo.Demo/Pages/Components/AllocationScheduler/AccessibilityDemo.razor` (new) |
| `depends_on` | none |
| `parallel_with` | R4, R8, R2 |
| `effort` | low-medium (new razor file, keyboard + ARIA demo scenarios) |
| `notes` | New demo page. No source changes. Wave 2 F1 topic gap. |

#### PL1-C: R4 — ThemingDemo.razor

| Field | Value |
|---|---|
| `lane_id` | R4 |
| `priority` | **P1** (sustains AMBER; Missing demo topic) |
| `dispatch_phase` | **B** (immediate) |
| `scope` | single |
| `sync_areas` | demo, docs, gap-plan |
| `files_touched` | `samples/Marilo.Demo/Pages/Components/AllocationScheduler/ThemingDemo.razor` (new) |
| `depends_on` | none |
| `parallel_with` | R3, R8, R2 |
| `effort` | low-medium (new razor file, provider swap + dark/light toggle demo) |
| `notes` | New demo page. No source changes. Wave 2 F2 topic gap. |

#### PL1-D: R2 — Hidden-Scrollbar A11y Fix

| Field | Value |
|---|---|
| `lane_id` | R2 |
| `priority` | **P1** (a11y correctness) |
| `dispatch_phase` | **B** (immediate) |
| `scope` | single |
| `sync_areas` | source, tests, gap-plan |
| `files_touched` | `src/Marilo.Providers.FluentUI/Styles/components/allocation-scheduler/*.scss`, tests |
| `depends_on` | none |
| `parallel_with` | R3, R4, R8 |
| `effort` | low (replace `scrollbar-width: none` with styled-visible scrollbar) |
| `notes` | Disjoint from R5 sweep (different SCSS selectors). Can safely run before R5. Stage 02 note: verify at dispatch time whether gantt/scheduler/datagrid share same hide-scrollbar pattern — if so, flag for potential cross-component promotion. |

### Priority Lane 2: Soft-Dep on R5 (Cross-Component)

#### PL2-A: R1 — Dark-Mode Invisible Cell-Edit Text

| Field | Value |
|---|---|
| `lane_id` | R1 |
| `priority` | **P1** (runtime correctness bug) |
| `dispatch_phase` | **C** (after R5 lands, or after confirming disjoint selectors) |
| `scope` | single |
| `sync_areas` | source, tests, gap-plan |
| `files_touched` | `src/Marilo.Providers.FluentUI/Styles/components/allocation-scheduler/*.scss`, tests |
| `depends_on` | **R5 (soft)** — preferred to run after `#fff` sweep. MAY run in parallel if `files_owned` excludes selectors R5 touches. |
| `parallel_with` | R2 (if disjoint selectors confirmed) |
| `effort` | low (add `color: var(--marilo-color-text)` to cell-edit input selector) |
| `notes` | The `#fff` sweep (R5) may implicitly fix or conflict with this lane's token. Preferred sequencing: R5 first, then evaluate if R1 is still needed. If R5 is delayed (cross-component lane backlog), R1 can proceed independently — the fix is additive and won't break the subsequent sweep. |
| `dispatch_decision` | **If R5 has landed:** check if cell-edit input already has `color` token. If yes, R1 is absorbed (mark closed). If no, proceed with R1. **If R5 is pending:** proceed with R1 using explicit `color: var(--marilo-color-text)` — the sweep will skip already-tokenized selectors. |

### Priority Lane 3: P2 Polish (After P1 Source Lanes)

These lanes are all P2 and can run in parallel with each other, subject to file-ownership verification at dispatch time.

#### PL3-A: R7 — Conflict Indicator Icon + ARIA Label

| Field | Value |
|---|---|
| `lane_id` | R7 |
| `priority` | **P2** |
| `dispatch_phase` | **D** |
| `scope` | single |
| `sync_areas` | source, spec, demo, tests, gap-plan |
| `files_touched` | component source (razor/cs), spec, demo, SCSS, tests |
| `depends_on` | Phase C complete (avoid churn in shared component files) |
| `parallel_with` | R10, R11, R12 (disjoint files) |
| `effort` | medium (source + spec + demo + tests — widest sync footprint of the P2 lanes) |

#### PL3-B: R10 — Drag-Fill Dashed Outline

| Field | Value |
|---|---|
| `lane_id` | R10 |
| `priority` | **P2** |
| `dispatch_phase` | **D** |
| `scope` | single |
| `sync_areas` | source, tests, gap-plan |
| `files_touched` | Fluent SCSS (drag-fill selectors), tests |
| `depends_on` | Phase C complete |
| `parallel_with` | R7, R11 (verify disjoint), R12 |
| `effort` | low (SCSS border-style change) |
| `notes` | R10 and R11 both touch Fluent SCSS. Verify at dispatch that their selectors are disjoint before parallelizing. |

#### PL3-C: R11 — Context-Menu Elevation Token

| Field | Value |
|---|---|
| `lane_id` | R11 |
| `priority` | **P2** |
| `dispatch_phase` | **D** |
| `scope` | single |
| `sync_areas` | source, tests, gap-plan |
| `files_touched` | Fluent SCSS (context-menu shadow selector), tests |
| `depends_on` | Phase C complete |
| `parallel_with` | R7, R10 (verify disjoint), R12 |
| `effort` | low (rgba literal to `--marilo-shadow-*` token) |
| `notes` | Same shape as R5 (literal to token) but for shadow family, not color family. If a systematic `rgba to elevation-token` sweep emerges across components, this could be promoted — but that is not this workspace's call. |

#### PL3-D: R12 — Bootstrap Disabled-Cell Stripes

| Field | Value |
|---|---|
| `lane_id` | R12 |
| `priority` | **P2** |
| `dispatch_phase` | **D** |
| `scope` | single |
| `sync_areas` | source, tests, gap-plan |
| `files_touched` | Bootstrap bridge SCSS file, tests |
| `depends_on` | Phase C complete |
| `parallel_with` | R7, R10, R11 (disjoint — Bootstrap bridge vs. Fluent SCSS) |
| `effort` | low (token-aware stripe color in Bootstrap dark) |
| `notes` | Bootstrap bridge file is disjoint from all Fluent SCSS files. Safe to parallelize with R10/R11. |

### Cross-Component Lanes (Orchestrator-Owned)

#### XC-1: R6 — SCSS Dedup

| Field | Value |
|---|---|
| `lane_id` | R6 |
| `priority` | **P1-prereq** |
| `dispatch_phase` | **A** (must land first) |
| `routing` | Cross-component SCSS dedup lane (Tick-8 Pattern 3) |
| `owner` | Orchestrator — NOT this workspace |
| `scope` | Mechanical delete of byte-identical root-level `_<component>.scss` duplicates |
| `blocks` | R5 (hard dependency) |
| `this_workspace_accounting` | Allocation-scheduler subset of the dedup. Gantt has the same pattern. |
| `action_for_this_workspace` | None. Record routing. Await orchestrator confirmation that R6 has landed before allowing R5 dispatch. |

#### XC-2: R5 — `#fff` Literal Replace

| Field | Value |
|---|---|
| `lane_id` | R5 |
| `priority` | **P1** |
| `dispatch_phase` | **B** (after R6) |
| `routing` | Cross-component `#fff` replace lane (Tick-8 Pattern 4) |
| `owner` | Orchestrator — NOT this workspace |
| `scope` | Global find-replace `#fff` to `var(--mar-color-surface, #fff)` across all four advanced components |
| `depends_on` | R6 complete |
| `softly_blocks` | R1 (soft dependency) |
| `this_workspace_accounting` | 19 instances in allocation-scheduler Fluent SCSS |
| `action_for_this_workspace` | None. Record routing. After R5 lands, evaluate whether R1 is absorbed. |

### Locked-Out Lane

#### OOS-1: R9 — Material Provider Stubs

| Field | Value |
|---|---|
| `lane_id` | R9 |
| `priority` | **Out-of-scope** |
| `dispatch_phase` | **E** (deferred indefinitely) |
| `routing` | Material tech-debt tracker (Tick-8 Pattern 5) |
| `owner` | Future Material tracker lane — NOT this workspace, NOT this wave |
| `action_for_this_workspace` | Do NOT dispatch. Record VP-allocation-scheduler-001/002 for future tracker registration. |

---

## Dispatch Phases (Concrete Sequencing)

### Phase A — Prerequisites (cross-component, orchestrator-owned)

| Lane | Type | Action |
|---|---|---|
| R6 | cross-component | Orchestrator dispatches SCSS dedup lane |

**Gate:** R6 complete (byte-identical duplicates deleted, `Styles/components/` is sole source). Orchestrator confirms.

### Phase B — Immediate Work (in-workspace, parallel fan-out)

| Lane | Priority | Sync Areas | Parallel Group |
|---|---|---|---|
| R8 | P1 (agg) | spec, docs, gap-plan | B-all |
| R3 | P1 | demo, docs, gap-plan | B-all |
| R4 | P1 | demo, docs, gap-plan | B-all |
| R2 | P1 | source, tests, gap-plan | B-all |
| R5 | P1 | source, tests, gap-plan | B-all (cross-component, orchestrator-owned) |

**Gate:** All Phase B lanes complete OR review-pending. R5 confirmed landed by orchestrator (or explicitly deferred with R1 dispatch-decision recorded).

**Parallelism:** All 4 in-workspace lanes (R2, R3, R4, R8) are fully independent — disjoint file ownership across spec files, demo razor files, and SCSS files. Can run as a single worker doing them serially or as multiple workers if orchestrator chooses to split.

### Phase C — P1 Source Changes (after Phase B gate)

| Lane | Priority | Sync Areas | Depends On |
|---|---|---|---|
| R1 | P1 | source, tests, gap-plan | R5 (soft) — see dispatch_decision above |

**Gate:** R1 complete or absorbed by R5. All P1 source lanes done.

**Note:** R2 runs in Phase B (not C) because it has no dependency on R5. R1 is the only lane gated on the cross-component sweep.

### Phase D — P2 Polish (after Phase C gate)

| Lane | Priority | Sync Areas | Parallel Group |
|---|---|---|---|
| R7 | P2 | source, spec, demo, tests, gap-plan | D-all |
| R10 | P2 | source, tests, gap-plan | D-all (verify R10/R11 disjoint) |
| R11 | P2 | source, tests, gap-plan | D-all (verify R10/R11 disjoint) |
| R12 | P2 | source, tests, gap-plan | D-all (disjoint — Bootstrap bridge) |

**Gate:** All P2 lanes complete. Full second-cycle remediation done (in-workspace scope).

**Pre-dispatch check:** Before parallelizing R10 and R11, verify their SCSS selectors target different rules in different partials. If they share a file, serialize R10 then R11.

### Phase E — Deferred

| Lane | Status | Action |
|---|---|---|
| R9 | Locked out | No dispatch. Material tech-debt tracker. |

---

## Priority Summary Table

| Rank | Lane | Priority | Phase | Category | Dispatch Owner |
|---|---|---|---|---|---|
| 1 | R6 | P1-prereq | A | cross-component | orchestrator |
| 2 | R5 | P1 | B | cross-component | orchestrator |
| 3 | R8 | P1 (agg) | B | in-workspace | this worker |
| 4 | R3 | P1 | B | in-workspace | this worker |
| 5 | R4 | P1 | B | in-workspace | this worker |
| 6 | R2 | P1 | B | in-workspace | this worker |
| 7 | R1 | P1 | C | in-workspace | this worker |
| 8 | R7 | P2 | D | in-workspace | this worker |
| 9 | R10 | P2 | D | in-workspace | this worker |
| 10 | R11 | P2 | D | in-workspace | this worker |
| 11 | R12 | P2 | D | in-workspace | this worker |
| 12 | R9 | OOS | E | locked out | future Material tracker |

---

## Verification

- **Lane count:** 12 / 12 accounted (9 in-workspace + 2 cross-component + 1 OOS)
- **Dependency graph:** 4 edges (R6→R5 hard, R5→R1 soft, Phase C→Phase B phase-gate, Phase D→Phase C phase-gate). No cycles.
- **R6 MUST precede R5:** Honored — Phase A (R6) gates Phase B (R5).
- **R9 locked out:** Confirmed — Phase E, no dispatch.
- **R5/R6 routed to cross-component:** Confirmed — XC-1 and XC-2 sections, orchestrator-owned.
- **R1 soft-dep on R5:** Confirmed — PL2-A dispatch_decision documents both paths (R5 landed vs. R5 pending).
- **5-phase advisory from Stage 01 honored:** Phase A-E structure preserved with concrete lane assignments and gates added.
- **First-cycle artifacts:** NOT touched. No edits to `gap-inventory.md` or `closure-report.md`.
- **Files written:** Only `output/stage-02/allocation-scheduler-wave4-priority-lanes.md` (this file).
- **Build/test:** N/A — this is an audit/planning turn, not a code turn.
- **Skill discipline:**
  - `verification-before-completion` — lane count verified, dependency graph verified, all constraints from review record honored.
  - `requesting-code-review` — result file follows template (see below).
  - `systematic-debugging` — not triggered (no contradictions found).
  - `test-driven-development` — N/A (no source/test edits).

**End of Stage 02 second-cycle prioritize. STOP.**
