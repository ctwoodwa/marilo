# MariloGantt — Delivery Report (Stage 04 Sync Check)

**Worker:** `w-gantt-delivery`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Stage:** `04-sync-check`
**Date:** 2026-04-11
**Supersedes:** 2026-04-10 AMBER report (stale — pre-dates Wave 1/2/3 audits)
**Checklist source:** `stages/04-sync-check/shared/delivery-checklist.md`
**Build verification:** `dotnet build Marilo.slnx` → succeeded, 0 warnings, 0 errors (2026-04-11T18:00Z).

---

## Gate Verdict: **BLOCKED**

Three CRITICAL visual-parity gaps (VP-gantt-01, VP-gantt-02, VP-gantt-03) plus a blocker-class Material provider stub (VP-gantt-16) prevent the delivery gate from passing. Nine of thirteen primary visual states fail at the source-rule level. Spec↔source API divergence (NM-01, NM-02, SA-01..SA-05) is being resolved through the `gantt-gap-analysis` workspace (Tracked-Out-of-Session) rather than this delivery pipeline. This report is the formal close of the gantt delivery pipeline; remediation runs inside `gantt-gap-analysis`.

| Metric | Count |
|---|---|
| Checklist items evaluated | 25 / 25 |
| Checklist items PASS | 3 |
| Checklist items AMBER | 8 |
| Checklist items BLOCKED | 14 |
| Distinct blockers | 7 (4 critical visual + 3 spec/API divergences) |
| Tracked-Out-of-Session items | 2 (EUX-04 / VP-gantt-17, EUX-05 / VP-gantt-18) |

The prior 2026-04-10 AMBER gate is superseded. It predated the Wave 3 visual-parity static analysis that surfaced VP-gantt-01 (zero-height bars) as a structural critical.

---

## Section 1 — API Spec

Citations: `stages/01-spec-review/output/gantt-spec-gap-list.md`, `stages/01-spec-review/output/gantt-spec-gaps.md` (2026-04-10 prior audit).

| # | Checklist Item | Status | Evidence |
|---|---|---|---|
| 1.1 | All implemented parameters documented in spec | **BLOCKED** | SA-06: `overview.md:157-161` lists only `RowHeight` and `GanttToolBarTemplate`; source exposes 30+ parameters on `MariloGantt<TItem>` plus many more on `GanttColumn`/`GanttCommandColumn`/views. Parameter table is materially under-populated. |
| 1.2 | All documented parameters implemented in source | **BLOCKED** | SA-04: `state.md:186-189` uses `ColumnResizable="true"` and `@bind-TaskListWidth` — neither exists in source. SA-05: `TaskListWidthChanged` event documented but not declared. SA-03: `state.md:108` references paging (`PropertyName == "Page"`) which does not exist — stale copy/paste from DataGrid. |
| 1.3 | Parameter types match between spec and source | **BLOCKED** | NM-01: spec `state.md:232` uses `SortDescriptors` (plural, `List<SortDescriptor>`); source `GanttState.cs:70` has `SortDescriptor` (singular, `GanttSortDescriptor?`). NM-02: spec uses `FilterDescriptors : List<IFilterDescriptor>`; source has `FilterValues : Dictionary<string,string>?`. SA-01: the spec example in `state.md:232-239` does not compile against the current API. SA-02: spec references `Marilo.DataSource.*` descriptor types not consumed anywhere in `MariloGantt`. |
| 1.4 | Parameter defaults match between spec and source | **AMBER** | No conflicting-default findings recorded by Wave 1; the evidence base is limited because the spec parameter table is itself incomplete (SA-06). The 2026-04-10 report flagged a P1 SlotWidth default mismatch — carried forward as a follow-up under SA-06's parameter-table rewrite. Cannot be declared PASS. |
| 1.5 | All events documented and implemented | **BLOCKED** | SA-05: `TaskListWidthChanged` documented without a type signature and not declared in source. SRC-04 / NM-03: source fires `FireStateChanged("VisibleColumns")` but `state.md:63` omits `"VisibleColumns"` from the enumerated `PropertyName` values. |
| 1.6 | Spec version reflects current implementation phase | **BLOCKED** | Spec is marked `unversioned` in `_config/delivery-context.md`. Cannot be reconciled while NM-01/NM-02 (public-API decisions for `GanttState`) are still open. |

Spec-level source-ahead items (SRC-01 milestone, SRC-02 summary-task auto-aggregation, SRC-03 `ComputedStart/End/PercentComplete`, SRC-04 `"VisibleColumns"` PropertyName, SRC-05 `GanttState` actual surface, SRC-06 `Data` rebind semantics) are tracked against Section 1 items 1.1, 1.5, and 1.6 — they are the same defects reported from the source side.

---

## Section 2 — Example UX

Citations: `stages/02-example-ux/output/gantt-example-ux-gap-list.md`, `stages/02-example-ux/output/gantt-demo-gap-list.md` (2026-04-10 prior audit).

| # | Checklist Item | Status | Evidence |
|---|---|---|---|
| 2.1 | Every spec parameter has at least one demo scenario | **AMBER** | Overview / Views / Hierarchical / Templates / Editing / Features collectively cover the **documented** surface. But the documented surface is itself under-populated (SA-06), so PASS cannot be claimed. `refresh-data.md` has zero demo coverage (EUX-06). |
| 2.2 | Every spec event has at least one demo scenario | **BLOCKED** | EUX-03: no demo subscribes to `OnStateChanged`. EUX-05: `TaskListWidthChanged` is un-demoable because source does not declare it (Wave 1 SA-05). EUX-04: `GetState()`/`SetStateAsync()` round-trip demo is blocked by the `GanttState` public-API decision — **Tracked-Out-of-Session**, rewrite queued to `gantt-gap-analysis`. |
| 2.3 | Disabled state demonstrated | **AMBER** | Not explicitly demoed on any Gantt page. No recorded Wave-2 gap ID; flagged as follow-up for a future demo pass. |
| 2.4 | Readonly state demonstrated (if supported) | **AMBER** | Inverse of Editing.razor — readonly mode is implicit when `Editable` is not set, but not explicitly labelled in any demo. Follow-up. |
| 2.5 | Empty/no-data state demonstrated | **AMBER** | All six Gantt demo pages seed a static `List<TItem>` with populated data. No empty-state demo. |
| 2.6 | Error state demonstrated (if supported) | **AMBER** | No error-state demo. Gantt has no documented error surface, so the "if supported" qualifier applies — but absent an explicit spec decision, this is AMBER not PASS. |
| 2.7 | All code snippets use current parameter names and types | **BLOCKED** | `state.md:232-239` example uses API surfaces (`SortDescriptors`, `FilterDescriptors`, `IFilterDescriptor`) that do not compile against source. Same root cause as 1.3 / NM-01 / NM-02. |
| 2.8 | No Telerik component references in demo pages | **PASS** | No Telerik references in any Gantt demo file recorded by Wave 2 inventory. |

New demo gaps from Wave 2 to carry forward (all currently OPEN unless flagged):

- **EUX-01** (P1): no milestone (zero-duration) task demo — diamond rendering never exercised.
- **EUX-02** (P1): no summary-task auto-aggregation demo — every demo pre-fills parent rows so the bottom-up `ComputedStart/End/PercentComplete` behaviour is invisible.
- **EUX-03** (P2): no `OnStateChanged` demo — add a `State.razor` page logging `PropertyName`.
- **EUX-04** (P2, **Tracked-Out-of-Session**): save/restore round-trip demo — blocked on `GanttState` rewrite, queued to `gantt-gap-analysis`.
- **EUX-05** (P2, **Tracked-Out-of-Session**): `TaskListWidthChanged` / `@bind-TaskListWidth` demo — blocked on source addition, queued to `gantt-gap-analysis`.
- **EUX-06** (P2): no `refresh-data.md` demo — add a `RefreshData.razor` page with in-place mutation + reference-swap.
- **EUX-07** (P2): column-chooser / `VisibleColumns` toggle not demoed.
- **EUX-08** (P2): no drag-to-move / drag-to-resize demo (carried forward from 2026-04-10 G3).

---

## Section 3 — Visual Parity

Citations: `stages/03-visual-parity/output/gantt-visual-parity-gaps.md`, `stages/03-visual-parity/output/gantt-parity-summary.md`, `stages/03-visual-parity/output/gantt-visual-parity-plan.md`.

| # | Checklist Item | Status | Evidence |
|---|---|---|---|
| 3.1 | All three themes captured: Fluent, Bootstrap, Material | **BLOCKED** | **VP-gantt-16** BLOCKER — Material `_gantt.scss` is a 5-line TODO stub. Material Light and Material Dark score 0.0 / 3 in the Wave 3 estimate. Capture pass cannot run for Material until the provider exists. |
| 3.2 | Light and dark modes captured for each theme | **BLOCKED** | **VP-gantt-02 CRITICAL** — Fluent provider has zero `[data-marilo-theme="dark"]` Gantt blocks. Fluent Dark scores 0.5 / 3. Bootstrap Dark has a partial patch (filter-menu + bar-delete only). Dark-mode parity is not achievable from the current SCSS. |
| 3.3 | All applicable states reviewed: default, hover, focus, selected, disabled | **BLOCKED** | Wave 3 static analysis: **9 of 13** primary states fail at the source-rule level. VP-gantt-01 (default bar: no base rule), VP-gantt-07 (hover reveals delete icon only, no bar change), VP-gantt-08 (selected state has no style anywhere), VP-gantt-14 (focus-visible missing). Nine primary states cannot be reviewed because the rules do not exist. |
| 3.4 | Parity score of 3 (visually equivalent) achieved for primary states in all themes | **BLOCKED** | Estimated scores: Fluent Light 1.2, Fluent Dark 0.5, Bootstrap Light 1.1, Bootstrap Dark 1.0, Material Light 0.0, Material Dark 0.0. Averaged: ~0.95 / 3. Materially below the 2.5 delivery-gate target. |
| 3.5 | Gaps below score 3 documented with severity classification | **PASS** | 16 direct gap records (VP-gantt-01 … VP-gantt-16) with severity tiers: 3 Critical, 7 Major, 4 Minor, plus 2 Deferred (VP-gantt-17 / VP-gantt-18). See `gantt-visual-parity-gaps.md`. |
| 3.6 | Visual parity gaps assigned to gap-analysis-resolution or remediation phase | **AMBER** | Wave 3 recommended remediation order is documented in `gantt-parity-summary.md` and `gantt-visual-parity-plan.md` but the gaps are not yet formally intaked into `gantt-gap-analysis` as tickets. The plan exists; the intake link-up is a follow-up task for the orchestrator — see Section 6. |

Critical visual-parity findings (re-cited, not re-audited):

- **VP-gantt-01 CRITICAL** — `.mar-gantt__bar` has no base rule in any provider. Task bars render zero-height using browser defaults.
- **VP-gantt-02 CRITICAL** — Fluent provider has zero `[data-marilo-theme="dark"]` Gantt blocks. Dark mode inherits token defaults only.
- **VP-gantt-03 CRITICAL** — Dependency SVG hardcodes `stroke="#999"` inline in razor. No theme awareness, no arrowhead marker.
- **VP-gantt-04 MAJOR** — Today / current-date line feature entirely missing from source.
- **VP-gantt-08 MAJOR** — Task selected state has no style rule in any provider.
- **VP-gantt-15 MINOR (hygiene)** — Two byte-identical `_gantt.scss` files in Fluent provider tree. Cross-component pattern — orchestrator flagged for sweep.
- **VP-gantt-16 BLOCKER** — Material `_gantt.scss` is a 5-line TODO stub.
- **VP-gantt-17 DEFERRED-PENDING-SOURCE** — `GetState()`/`SetStateAsync` visual audit blocked by EUX-04 (`GanttState` rewrite queued to `gantt-gap-analysis`). **Tracked-Out-of-Session.**
- **VP-gantt-18 DEFERRED-PENDING-SOURCE** — `TaskListWidthChanged` splitter visual audit blocked by EUX-05 (source addition required). **Tracked-Out-of-Session.**

---

## Section 4 — Source and Tests

Citations: `workspaces/Marilo/workspaces/gantt-gap-analysis/CLAUDE.md`, `ICM/workspaces/gantt-gap-analysis/_config/coverage-summary.md`, `stages/01-spec-review/output/gantt-spec-gap-list.md`.

| # | Checklist Item | Status | Evidence |
|---|---|---|---|
| 4.1 | All spec parameters covered by bUnit tests | **AMBER** | Gap-analysis workspace reports 648 bUnit tests passing at Stage 06 closure. Per-parameter coverage map is not enumerated in the delivery-pipeline outputs; cannot assert complete coverage of the spec parameter set (which is itself incomplete — SA-06). Follow-up for the test-coverage-expansion workspace. |
| 4.2 | No undocumented parameters in component source | **BLOCKED** | SRC-01..SRC-06 (milestone, summary auto-aggregation, `ComputedStart/End/PercentComplete`, `"VisibleColumns"` PropertyName, `GanttState` actual surface, `Data` rebind semantics) all represent source behaviour absent from spec. Same root cause as Section 1. |
| 4.3 | Stage 06 closure reports exist for all active gap phases | **PASS** | `gantt-gap-analysis/CLAUDE.md` reports Stages 01-06 complete with closure report at `output/stage-06/gantt-closure-report.md`. |
| 4.4 | Pre-existing test failures documented in regression triage log | **PASS** | Gap-analysis closure reports 648 tests passing; no regression triage log required for active state. |
| 4.5 | All active gap phases show Tests Passing = YES in coverage summary | **AMBER** | `_config/coverage-summary.md` shows Tests Passing = `N/A` for every listed phase (events, state, refresh-data, gantt-tree, timeline, dependencies, accessibility) because the test column is tracked against the intake+prioritize stage, not the implementation stage. The CLAUDE.md asserts 648 passing but the coverage-summary.md surface is stale. Reporting-hygiene follow-up for the gap-analysis workspace; not a delivery-gate blocker. |

---

## Section 5 — Alignment

| # | Checklist Item | Status | Evidence |
|---|---|---|---|
| 5.1 | Spec version consistent with gap workspace active phase | **BLOCKED** | Spec is `unversioned`; gap-analysis workspace reports COMPLETE for Stages 01-06 but the `gantt-state-shape` user decision (2026-04-11T17:20Z) queues a new source rewrite to `gantt-gap-analysis`. The two workspaces are momentarily out of sync — a new phase in `gantt-gap-analysis` will reopen `state.md` work. |
| 5.2 | Demo page parameter names match current source parameter names | **BLOCKED** | SA-04 (`ColumnResizable`, `@bind-TaskListWidth` in `state.md` example), NM-01/NM-02 (`SortDescriptors`/`FilterDescriptors` plural types) — the `state.md` example code does not compile. Demo pages themselves (Overview/Views/Templates/Hierarchical/Editing/Features) use real parameter names, but the checklist item spans spec code snippets too. |
| 5.3 | No parameter renamed without spec and demo page update | **PASS** | No rename operation recorded in this delivery cycle. |
| 5.4 | `delivery-context.md` reflects current state of all three artifacts | **PASS (after this stage)** | `_config/delivery-context.md` is updated as part of this stage (Last sync check = 2026-04-11, Gate status = BLOCKED, Blocking items = 7). |

---

## Section 6 — Blockers with Remediation Paths

Each blocker lists the owner lane and the phase at which remediation is expected. `Tracked-Out-of-Session` items are explicitly not assigned to this delivery pipeline.

| Blocker | Severity | Owner | Remediation Phase | Notes |
|---|---|---|---|---|
| **VP-gantt-01** `.mar-gantt__bar` has no base rule → zero-height bars | CRITICAL | `gantt-gap-analysis` visual-parity lane | Foundation pass (Stage 03→05) | Write base bar rule in Fluent + Bootstrap SCSS. Precondition for every other visual state. |
| **VP-gantt-02** Fluent dark-mode patches missing for Gantt | CRITICAL | `gantt-gap-analysis` visual-parity lane | Foundation pass (Stage 03→05) | Add `[data-marilo-theme="dark"]` patches for Fluent Gantt. |
| **VP-gantt-03** Dependency SVG stroke hardcoded `#999`, no arrowhead | CRITICAL | `gantt-gap-analysis` visual-parity lane | Critical features pass (Stage 03→05) | Move stroke to a themed token; add arrowhead marker. Touches razor + SCSS. |
| **VP-gantt-16** Material `_gantt.scss` is a 5-line TODO stub | BLOCKER | `gantt-gap-analysis` visual-parity lane, gated on Material runtime project | Foundation pass (Stage 03→05) | Material provider is the broader blocker — Gantt cannot parity-audit Material themes until it exists. |
| **NM-01 / NM-02 / SA-01 / SA-02** `GanttState<TItem>` descriptor-type divergence | BLOCKER (spec/API) | `gantt-gap-analysis` state lane | **Tracked-Out-of-Session** — source rewrite queued by 2026-04-11T17:20Z user decision `gantt-state-shape` | Source moves to `SortDescriptors`/`FilterDescriptors` lists with `Marilo.DataSource.*` descriptors. Breaking change; part of the gantt-gap-analysis workspace, not this delivery pipeline. |
| **SA-06 + SRC-01..SRC-06** spec parameter table under-populated; multiple source-ahead behaviours undocumented | MAJOR (spec) | `gantt-gap-analysis` spec lane | Intake → remediation (Stage 01→05 in gap-analysis) | Rewrite `overview.md` parameter table; document milestone, summary auto-aggregation, compute routine, `"VisibleColumns"` PropertyName, `Data` rebind semantics. |
| **EUX-01 / EUX-02 / EUX-03 / EUX-06 / EUX-07 / EUX-08** demo coverage gaps | MAJOR (demos) | `gantt-gap-analysis` demo lane (or separate demo follow-up pass) | Stage 03→05 in gap-analysis | Worker-resolvable; source is correct per design. New demo files under `samples/Marilo.Demo/Pages/Components/Gantt/`. |

### Tracked-Out-of-Session (not assigned to this delivery pipeline)

- **VP-gantt-17 / EUX-04** — `GetState()`/`SetStateAsync()` save/restore demo and visual audit. Blocked on `gantt-state-shape` user decision (RESOLVED 2026-04-11T17:20Z). Source rewrite is queued to the `gantt-gap-analysis` workspace.
- **VP-gantt-18 / EUX-05** — `TaskListWidthChanged` / `@bind-TaskListWidth` demo and visual audit. Blocked on source addition. Same escalation path as VP-gantt-17.

Both Tracked-Out-of-Session items must **not** be re-audited by this delivery pipeline. Re-entry happens only after `gantt-gap-analysis` closes the relevant phase and publishes updated source.

---

## Section 7 — Wave 3 Cerebrum Learning Candidates

Two learnings surfaced by Wave 3 are worth recording as cross-component patterns at orchestrator review time:

1. **BEM class declared ≠ BEM class styled.** Razor emits `.mar-gantt__bar`, `.mar-gantt__tasklist-row`, `.mar-gantt__timeline-header` but neither the Fluent nor Bootstrap SCSS defines base-class rules — only modifier classes. A static-analysis check that diffs `class="mar-component__…"` instances in razor against the provider SCSS symbol table would catch "declared but unstyled" gaps automatically. Recommend adding this to the delivery pipeline's Stage 03 rubric.
2. **Provider-level dark-mode patches are inconsistently applied.** Fluent has zero dark patches for Gantt; Bootstrap has a partial patch. A per-component "has dark patch: yes/no" grid added to Stage 04 would make this visible at delivery time, not discovery time.

Promotion target: orchestrator to apply at Wave 4 review, not this worker turn.

---

## Section 8 — Verification Citation

- **Build:** `dotnet build Marilo.slnx` → Build succeeded. 0 Warning(s), 0 Error(s). Time Elapsed 00:00:03.31. (2026-04-11T18:00Z)
- **Stage 03 static analysis already verified by Wave 3 (same `Marilo.slnx` build, 0 warnings 0 errors at 2026-04-11T17:40Z).** No source changes were made in Stage 04 (docs-only stage), so the Wave 3 build state is still current.

---

## Section 9 — Final Gate

**BLOCKED.** 14 checklist items blocked out of 25. 7 distinct blockers (4 critical visual + 3 spec/API divergences). 2 Tracked-Out-of-Session items. Delivery gate does not pass.

This report is the formal close of the MariloGantt delivery pipeline for the `marilo-grid-pipeline-2026-04-11-1200` session. All remediation for the cited blockers flows through the `gantt-gap-analysis` workspace, not through re-entry into this delivery pipeline.
