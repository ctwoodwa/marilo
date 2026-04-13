# MariloGantt — Visual Parity Summary (Wave 3)

**Worker:** `w-gantt-delivery`
**Stage:** `03-visual-parity`
**Method:** Static analysis of source (SCSS + razor). No runtime screenshots captured in this wave.
**Build verification:** `dotnet build Marilo.slnx` → succeeded, 0 warnings, 0 errors (2026-04-11T17:40Z).
**Total direct gap records:** 16 (VP-gantt-01 … VP-gantt-16)
**DEFERRED records:** 2 (VP-gantt-17 EUX-04, VP-gantt-18 EUX-05)

---

## Top-Line Verdict

MariloGantt is **not at visual-parity condition** with Telerik Gantt in any theme or mode. The dominant finding is **structural**: the BEM class scheme was declared in razor but the provider SCSS files never added base rules for several core elements — task bars, tree-list rows, timeline header cells, today marker, dependency lines, focus states, selection state. As a result, these elements render with browser defaults rather than themed visuals.

This is **not** a fine-grained token or shade problem — it is a "base rules were never written" problem. That shapes the remediation plan: the fix is largely "write the missing SCSS rules" rather than "tweak existing tokens."

## Parity Scores Per Theme/Mode (static-analysis estimate)

Scores are estimated from SCSS coverage and known inline-style patterns, not from screenshots. A proper runtime pass with Playwright is still required for a confirmed number — these are the floor.

| Theme | Mode | Est. Parity Score | Notes |
|---|---|---|---|
| Fluent | Light | **1.2 / 3** | Has coverage for milestone, summary, progress, filter-menu; missing base bar, today line, dependency tokens, header cells, row rules, focus. |
| Fluent | Dark | **0.5 / 3** | Fluent has **zero** dark-mode patches — only token defaults carry through. Filter-menu surface does not flip. Delete icon stays red-dark on dark bg. |
| Bootstrap | Light | **1.1 / 3** | Parity with Fluent Light on covered states; identical missing-base-rule gaps. |
| Bootstrap | Dark | **1.0 / 3** | Has a minimal dark patch for filter-menu + bar-delete; still missing most structural base rules. Slightly ahead of Fluent Dark in chrome elements but behind on everything else. |
| Material | Light | **0.0 / 3** | Provider file is a 5-line TODO stub. All captures blocked. Out-of-scope per VP-gantt-16. |
| Material | Dark | **0.0 / 3** | Same as Material Light. Out-of-scope. |

**Averaged across non-blocked provider/mode cells: ~0.95 / 3.** Materially below the 2.5 delivery-gate target.

## Severity Breakdown

| Severity | Count | IDs |
|---|---|---|
| Critical | 3 | VP-gantt-01 (base bar), VP-gantt-02 (Fluent Dark), VP-gantt-03 (dep line stroke), VP-gantt-16 (Material stub blocker) |
| Major | 7 | VP-gantt-04 (today line), VP-gantt-05 (milestone), VP-gantt-06 (summary), VP-gantt-07 (hover), VP-gantt-08 (selected), VP-gantt-09 (rows), VP-gantt-10 (header), VP-gantt-14 (focus) |
| Minor | 4 | VP-gantt-11 (progress formula), VP-gantt-12 (indent math), VP-gantt-13 (fluent elevation), VP-gantt-15 (duplicate file) |
| Deferred | 2 | VP-gantt-17 (EUX-04), VP-gantt-18 (EUX-05) |

(Note: some critical findings are counted under both "critical" and the state's major tier per rubric severity rules; the above lists each gap once under its **primary** tier.)

## State/Scenario Coverage vs. Capture Matrix

The capture matrix lists 13 states. Static-analysis reveals the following coverage status at the **source-rule** level (not capture level — the capture pass is still pending a runtime session):

| # | State | Source rule coverage | Primary gap ID |
|---|---|---|---|
| 1 | Default task bar | None (browser default) | VP-gantt-01 |
| 2 | Summary bar | Opacity patch only | VP-gantt-06 |
| 3 | Milestone diamond | Unicode glyph, no shape | VP-gantt-05 |
| 4 | Progress indicator | Present, but formula differs per provider | VP-gantt-11 |
| 5 | Tree column idle | None (row chrome missing) | VP-gantt-09 |
| 6 | Expanded row | None (no row wrapper rule) | VP-gantt-09 |
| 7 | Collapsed row | None (no row wrapper rule) | VP-gantt-09 |
| 8 | Timeline header | None (classes exist, no rules) | VP-gantt-10 |
| 9 | Current date line | **Feature missing entirely** | VP-gantt-04 |
| 10 | Task hover | Reveals delete icon only, no bar change | VP-gantt-07 |
| 11 | Task selected | **Feature missing entirely (no style rule)** | VP-gantt-08 |
| 12 | Editing row | Cell cursor patch only | (rolls up to VP-gantt-09) |
| 13 | Dependency lines | SVG present, hard-coded stroke, no arrowhead | VP-gantt-03 |

**Nine of thirteen primary states have critical or major source-level gaps.** Two of thirteen states (#9 today line, #11 selected) are not implemented in source at all.

## Recommended Remediation Order

1. **Foundation pass** (VP-gantt-01, VP-gantt-02, VP-gantt-15). Fix the missing base `.mar-gantt__bar` rule, add a Fluent dark-mode patch, delete the duplicate Fluent file. This unlocks scoring for every other state.
2. **Critical visual features** (VP-gantt-03, VP-gantt-04). Fix the dependency stroke and add the today line. These are both "missing feature" not "wrong token" — require source + SCSS.
3. **Primary state treatment** (VP-gantt-07, VP-gantt-08, VP-gantt-14). Hover, selected, focus-visible. All primary states per rubric. Depends on the foundation pass.
4. **Tree-list + timeline-header chrome** (VP-gantt-09, VP-gantt-10). Row height, borders, header typography.
5. **Polish** (VP-gantt-05 milestone shape, VP-gantt-06 summary shape, VP-gantt-11 progress formula, VP-gantt-12 indent math, VP-gantt-13 elevation token).
6. **Material provider** (VP-gantt-16): gated on Material runtime project — not this stage's responsibility.
7. **Deferred source-blocked items** (VP-gantt-17, VP-gantt-18): re-audit after source lands in `gantt-gap-analysis`.

## Blockers Identified

- **Material runtime provider** — blocks 2/6 theme×mode cells entirely (VP-gantt-16).
- **Source-level state blockers** — EUX-04 `GanttState` rewrite queued to `gantt-gap-analysis` (VP-gantt-17); EUX-05 `TaskListWidthChanged` source-missing (VP-gantt-18). Both prevent splitter / state-restore visual audits.
- **Runtime capture pass still required** — this wave produced a static-analysis audit as directed by the inbox. Scores above are floors; a Playwright-based pass should confirm them or tighten specific records. Not in scope for this worker's turn.

## Required Sync Areas

Per the worker state JSON, declared `required_sync_areas: ["spec"]`. The static-analysis findings in this audit primarily target SCSS source (not the spec) but surface that **several spec files describe behavior with no corresponding visual-layer support**:

- `docs/component-specs/gantt/timeline/overview.md` references a today-line / current-date marker behavior — source has no implementation (VP-gantt-04). Spec/source sync gap.
- `docs/component-specs/gantt/dependencies/overview.md` references dependency rendering — source hard-codes stroke in razor rather than theming (VP-gantt-03). Spec/source sync gap.
- `docs/component-specs/gantt/events.md` references `TaskListWidthChanged` which is source-missing — splitter visuals have no rule (VP-gantt-18). Already tracked in Wave 1 SA-04/05.

These are cross-sync signals for the Stage 04 sync-check pass, not asks on this worker.

## Cerebrum Learning Candidates

Two learnings worth promoting at orchestrator review time (this worker does not mutate cerebrum during its turn per orchestration rules):

1. **BEM class declared ≠ BEM class styled.** Razor emits `.mar-gantt__bar`, `.mar-gantt__tasklist-row`, `.mar-gantt__timeline-header`, etc. but neither the Fluent nor Bootstrap SCSS defines rules for the base class — only modifier classes. A static-analysis check that diffs `class="mar-gantt__…"` instances in razor against the provider SCSS symbol table would catch this kind of "declared but unstyled" gap automatically and should be added to the delivery pipeline.
2. **Provider-level dark-mode patches are inconsistently applied.** Fluent has zero dark patches for Gantt; Bootstrap has a partial patch. Recommend adding a per-component "has dark patch: yes/no" grid check to the sync-check stage.
