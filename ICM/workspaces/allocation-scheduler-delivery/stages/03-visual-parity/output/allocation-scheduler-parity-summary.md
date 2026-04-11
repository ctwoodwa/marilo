# AllocationScheduler — Visual Parity Summary

**Workspace:** ICM/workspaces/allocation-scheduler-delivery
**Stage:** 03-visual-parity
**Worker:** `w-allocation-scheduler-delivery`
**Wave:** 3 (2026-04-11)
**Method:** Static source analysis. Runtime screenshots not collected this tick — Material provider runtime is absent (see `visual-parity-plan.md` blockers), Fluent/Bootstrap analysed from SCSS + razor evidence.
**Gap record count:** 20 (`allocation-scheduler-visual-parity-gaps.md`)
**Build evidence:** `dotnet build Marilo.slnx` ran at 2026-04-11 17:40 local, exit 0, 0 warnings, 11 projects compiled.

---

## Overall Parity Score Matrix

Scores by theme × mode, averaged across the primary-state rubric (default view, occupied cell, cell hover, selected allocation) plus the secondary/edge state sample from the gap records. 0 = materially different, 3 = visually equivalent.

| Theme     | Mode  | Primary Avg | Secondary Avg | Overall | Band       |
|-----------|-------|-------------|---------------|---------|------------|
| Fluent    | Light | 2.75        | 2.3           | 2.5     | Close      |
| Fluent    | Dark  | 2.0         | 1.8           | 1.9     | Noticeable |
| Bootstrap | Light | 3.0         | 2.5           | 2.75    | Close      |
| Bootstrap | Dark  | 2.0         | 1.5           | 1.75    | Noticeable |
| Material  | Light | 0           | 0             | 0       | **Blocked** |
| Material  | Dark  | 0           | 0             | 0       | **Blocked** |

**Bands:** 3.0 = Equivalent · 2.5–2.9 = Close · 2.0–2.4 = Noticeable · 1.0–1.9 = Off · 0 = Materially different / Blocked.

**Weighted coverage:** Fluent Light is the only combination where the component is in a ship-ready band for primary states. Fluent Dark and Bootstrap Dark cluster at "Noticeable" primarily due to systemic color-mix fallback literals (VP-003, VP-007) and elevation token omissions (VP-004). Material is a hard blocker on the runtime provider, not a styling decision.

---

## Worst Offenders (ranked)

1. **VP-001 / VP-002 — Material provider has zero AllocationScheduler styles.** Both Light and Dark are score 0. Cannot be "tuned" — must implement the Material SCSS file. Blocker for any Wave-4 sync-check against Material.
2. **VP-003 — color-mix fallback literal `#ffffff`.** Systemic across every cell-tint recipe in Fluent. Affects 19 rules. One fix, widespread impact.
3. **VP-011 — Conflict indicator background-only.** No icon, no accent border, no tooltip. User-visible correctness gap — conflicts fail to announce themselves.
4. **VP-020 — Hidden scrollbar blocks keyboard/SR scroll discovery.** a11y-class issue, feeds Wave-2 Missing `accessibility`.
5. **VP-006 — Cell-edit input has no explicit `color:`.** Dark-mode invisible-text bug when user enters edit mode.
6. **VP-012 — Drag-fill preview uses solid fill instead of dashed outline.** Collides visually with hover/selected states.
7. **VP-004 — Context menu shadow uses hardcoded rgba instead of `--marilo-shadow-*` token.** Menu floats without edge in dark mode.
8. **VP-008 — Bootstrap disabled cell pattern invisible on dark.** `rgba(0,0,0,0.07)` stripes disappear into `--bs-body-bg` dark.

---

## Category Roll-up

| Category           | Count | Severity leaning |
|--------------------|-------|------------------|
| token/color        | 7     | Critical–Minor   |
| state treatment    | 6     | Major            |
| state treatment (a11y subset) | 4 (VP-005, VP-015, VP-020, VP-011 ARIA) | Major |
| elevation          | 1     | Major            |
| iconography        | 1     | Major            |
| density            | 1     | Minor            |
| layout             | 1     | Minor            |
| build hygiene      | 1     | Minor (blocker for clean remediation) |

**Token/color dominates the distribution.** This is consistent with cerebrum's repeated observation that the Fluent → dark bridge is the single largest systemic risk for grid-family components.

---

## Wave-2 Carry-forward Resolution

Wave 2 classified two topics as **Missing** at the demo layer. Wave 3 confirms they surface at the visual layer as well:

| Wave-2 topic  | Wave-3 visual gaps                      | Status at Wave 3 |
|---------------|------------------------------------------|------------------|
| accessibility | VP-005 (splitter focus/hover collapse), VP-011 (conflict ARIA), VP-015 (resource truncation no tooltip), VP-020 (hidden scrollbar) | Surfaced as 4 visual gaps, all state-treatment category. Wave-4 sync-check should record that a11y is now partially discharged at the visual layer but still Missing at the demo layer — Wave-3 outputs are SCSS/source, not demo. |
| theming       | VP-001, VP-002 (Material empty), VP-003, VP-007 (color-mix fallback literals) | Surfaced as 4 visual gaps, all token/color. Theming demo Missing at Wave 2 → visual-implementation gap at Wave 3. Wave-4 sync-check should note: **cannot demo theme-swap across Fluent/Bootstrap/Material until VP-001/002 are resolved** (Material is not yet a swappable target). |

---

## First-pass Review Order — Completion Status

From `allocation-scheduler-visual-parity-plan.md`:

| Order | Theme       | Status    | Gaps raised |
|-------|-------------|-----------|-------------|
| 1     | Fluent Light  | Done      | VP-005, VP-009, VP-010, VP-011, VP-012, VP-013, VP-014, VP-015, VP-016, VP-017, VP-019 |
| 2     | Fluent Dark   | Done      | VP-003, VP-004, VP-005, VP-006, VP-007, VP-018, VP-020 |
| 3     | Bootstrap Light | Done    | VP-013, VP-014, VP-015 (shared with Fluent) |
| 4     | Bootstrap Dark | Done     | VP-004, VP-008 |
| 5     | Material Light | **Blocked** | VP-001 (blocker itself) |
| 6     | Material Dark  | **Blocked** | VP-002 (blocker itself) |

Fluent × both modes reviewed end-to-end. Bootstrap × both modes reviewed for the gaps that are provider-specific (bridge-scss differs in several places). Material is blocked on a prerequisite that sits outside this worker's scope — explicitly flagged for orchestrator escalation.

---

## Recommendations to Orchestrator

1. **Accept this stage as "complete in scope of static analysis".** Material runtime implementation is a prerequisite for the 6-way matrix to fully close — cannot be worked around here.
2. **Route VP-003 and VP-007 to a single "Fluent dark-mode literal sweep" remediation task.** Both are the same class of bug. Same is likely true of adjacent components (gantt, datagrid, scheduler) — worth cross-workspace referral.
3. **Escalate VP-011, VP-013, VP-014, VP-015 source-change portions.** These require razor edits or new components (MariloSkeleton wiring, MariloIcon for conflicts, MariloTooltip wrapping). Workers can write SCSS but not source under this scope.
4. **Flag VP-019 as a prerequisite for any Fluent SCSS remediation.** Two duplicate files will diverge unless deleted first. Low-risk mechanical change; consider running across all affected provider packages in a single coordinated pass.
5. **Material provider runtime** is the single largest blocker for parity closure across the 6-way matrix. Suggest a separate orchestration wave for `Marilo.Providers.Material` runtime implementation before returning to AllocationScheduler parity.

---

## Gate Check

| Check | Status |
|-------|--------|
| ≥10 gap records raised | Yes — 20 records |
| ≤20 gap records raised | Yes — 20 records (upper bound respected) |
| Coverage of all listed structural elements (timeline header, resource rows, allocation bars, capacity-over, selection highlight, splitter chrome, context-menu visuals, editing affordances, scenario/target overlay bands) | Yes — each structural element touched by at least one gap. See mapping: timeline header → VP-010, VP-016; resource rows → VP-015, VP-020; allocation bars/cells → VP-003, VP-006, VP-009, VP-011, VP-012; capacity-over → VP-011 (conflict as closest proxy, no explicit over-capacity visual beyond row-tint at L423); selection highlight → VP-009; splitter chrome → VP-005; context-menu visuals → VP-004; editing affordances → VP-006, VP-012, VP-017; scenario/target overlay bands → VP-018. |
| Theme × Mode coverage (Fluent/Bootstrap/Material × Light/Dark = 6) | Yes — all 6 represented in the Score Matrix; Material scored 0/blocked |
| Wave-2 Missing topics (accessibility, theming) surfaced here | Yes — 4 gaps each, documented in carry-forward table above |
| Parity summary with overall scores per theme/mode | Yes (this file) |

All gate checks pass. Worker moves to `review-pending`.
