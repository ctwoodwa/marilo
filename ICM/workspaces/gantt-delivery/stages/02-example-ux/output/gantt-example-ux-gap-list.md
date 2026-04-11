# Gantt Example-UX Gap List

Complement to `gantt-demo-gap-list.md` (2026-04-10). This file captures demo coverage findings from ICM stage 02-example-ux as executed by Wave 2 of the orchestrator-driven grid pipeline. Cross-references Wave 1 spec-review gaps recorded in `ICM/workspaces/gantt-delivery/stages/01-spec-review/output/gantt-spec-gap-list.md`.

## 2026-04-11 orchestrator wave 2 (subagent dispatch)

**Worker:** `w-gantt-delivery`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Stage:** `02-example-ux`
**Demos audited:** `samples/Marilo.Demo/Pages/Components/Gantt/{Overview,Views,Templates,Hierarchical,Editing,Features}.razor`
**Topic specs audited:** `docs/component-specs/gantt/{overview.md, events.md, refresh-data.md, state.md}`
**Scope note:** The 2026-04-10 `gantt-demo-gap-list.md` did not include `Features.razor` in its coverage table. Features.razor covers sortable columns, text filtering, and dependency-line rendering — so Wave 1 gaps G1 and G2 from that file are already resolved at the demo layer. This section supersedes them and records the true outstanding gaps against the four topic specs plus cerebrum-flagged source-ahead behaviours.

### Demo inventory

| # | Page | Scenarios | Primary API surface |
|---|------|-----------|---------------------|
| 1 | `Overview.razor` | Basic Usage | `Data`, `IdField`, `ParentIdField`, `Height`, `Width`, `OnUpdate`, `OnDelete`, `@bind-View`, `GanttColumns`, `GanttWeekView`, `GanttMonthView`, `DisplayFormat` |
| 2 | `Views.razor` | All Four Views | `GanttDayView`, `GanttWeekView`, `GanttMonthView`, `GanttYearView`, `SlotWidth`, `@bind-View`, programmatic view switching |
| 3 | `Templates.razor` | Task Bar, Column, Toolbar Templates | `TaskTemplate`, column `<Template>`, `GanttToolBarTemplate` |
| 4 | `Hierarchical.razor` | Three-Level Hierarchy | `ParentIdField` at 3 depths, expand/collapse chevrons, `@bind-View` |
| 5 | `Editing.razor` | Inline Editing | `OnUpdate`, `OnDelete`, `OnCreate`, `Editable="true"`, double-click edit, add-via-handler |
| 6 | `Features.razor` | Sorting & Filtering, Dependencies | `Sortable`, `Filterable`, filter text-input binding, `DependsOnField`, dependency lines |

**Total scenarios:** 6 pages / ~9 demo sections.

### Coverage classification by topic spec

| Topic spec | Status | Evidence | Notes |
|------------|--------|----------|-------|
| `overview.md` — basic Gantt setup, parameters, views | **Covered** | Overview.razor + Views.razor + Hierarchical.razor collectively exercise `Data`, `IdField`, `ParentIdField`, `Height`, `Width`, `@bind-View`, `GanttColumns`, all four view types | Overview spec itself is under-populated (see Wave 1 SA-06/SA-08) but the demo coverage of the documented surface is complete. |
| `events.md` — `OnUpdate`, `OnDelete`, `OnCreate`, `TaskListWidthChanged`, `OnStateChanged` | **Partial** | Editing.razor fully covers `OnUpdate` / `OnDelete` / `OnCreate`. **No demo fires `OnStateChanged`**. `TaskListWidthChanged` is undemo-able (Wave 1 SA-05: event is not implemented in source). | See EUX-02 and EUX-05 below. |
| `refresh-data.md` — `ObservableCollection<T>` reactivity, `Rebind()`, reference swap semantics | **Missing** | No demo mutates `Data` in-place and calls `Rebind()`. No demo demonstrates reference-swap reload. All 6 pages use static `List<T>` initialised once. | See EUX-06 below. |
| `state.md` — `GanttState<TItem>`, `GetState()`, `SetStateAsync()`, `OnStateChanged.PropertyName` | **Missing / Blocked-by-orchestrator-decision** | No demo calls `GetState()` or `SetStateAsync()`. No demo subscribes to `OnStateChanged`. The `GanttState<TItem>` public API itself is under orchestrator review (Wave 1 NM-01/NM-02/SA-01/SA-02). | See EUX-03 + EUX-04 below, and blocked notice. |

### Cerebrum / source-ahead demo gaps (Wave 1 SRC-0x cross-reference)

| ID | Wave 1 ref | Behaviour | Demo coverage | Status |
|----|------------|-----------|---------------|--------|
| EUX-01 | SRC-01 | Milestone diamond rendering (`isMilestone = taskStart == taskEnd`, `&#x25C6;` glyph, `mar-gantt__milestone` CSS class, `aria-label="Milestone: …"`) | **No demo** includes a zero-duration task. Every seed dataset across all six pages uses `Start < End`. The diamond glyph never renders in any demo. | **Missing** |
| EUX-02 | SRC-02, SRC-03 | Summary-task auto-aggregation — any task with `Children.Count > 0` renders via `ComputedStart/ComputedEnd/ComputedPercentComplete` and gets `mar-gantt__bar--summary` class | Overview / Hierarchical / Editing / Features / Views / Templates all pre-set parent `Start`/`End`/`PercentComplete` values. **No demo proves the parent bar is computed bottom-up** — in every case the parent fields happen to match the aggregated child range, hiding the behaviour. A correct demo would leave parent fields at default / zero / deliberately-wrong values and show that the rendered bar still reflects the children. | **Missing** |
| EUX-03 | SRC-04 | `FireStateChanged("VisibleColumns")` raised when column chooser toggles visibility | No demo wires `OnStateChanged`. No demo toggles column visibility. No demo enables the column chooser. | **Missing** |

### New demo gaps — Wave 2

| ID | Gap | Maps to spec | Priority | Notes |
|----|-----|--------------|----------|-------|
| **EUX-01** | No milestone (zero-duration) task demo — diamond rendering is invisible to users reading the sample app | `overview.md`, `timeline/templates/task.md` (also Wave 1 SRC-01 spec rewrite) | P1 | Smallest possible fix: add one `Start == End` task to Overview.razor, or a dedicated `Milestones.razor` page. |
| **EUX-02** | No summary-task auto-aggregation demo — the headline value of parent bars being derived is completely hidden because every demo pre-fills parent fields to match child ranges | `overview.md`, `gantt-tree/data-binding/overview.md` (also Wave 1 SRC-02/SRC-03) | P1 | Add a `SummaryTasks.razor` page (or extend Hierarchical.razor) where parent rows have `Start`/`End`/`PercentComplete` deliberately *not* matching children and the UI shows the parent bar rendering the aggregate. |
| **EUX-03** | No `OnStateChanged` demo — `state.md` documents the event but no demo subscribes to it | `state.md`, `events.md` | P2 | Add a `State.razor` page that logs `OnStateChanged.PropertyName` to a status panel. Must demonstrate at least one of `SortDescriptor` / `FilterValues` / `ExpandedItems` / `View` / `VisibleColumns`. Note the `VisibleColumns` PropertyName is itself a Wave 1 spec gap (SRC-04 / NM-03). |
| **EUX-04** | No `GetState()` / `SetStateAsync()` round-trip demo (save / restore) | `state.md` | **Blocked-by-orchestrator-decision** | `state.md` example code uses API surfaces that do not compile (Wave 1 NM-01, NM-02, SA-01, SA-02). A worker cannot build a working demo of save/restore until the orchestrator decides whether to rewrite the spec or expand the source. **No fix proposed in this wave.** |
| **EUX-05** | No `TaskListWidthChanged` / `@bind-TaskListWidth` demo | `events.md`, `overview.md` | **Blocked-by-source** | Wave 1 SA-04 / SA-05 — the event and two-way-bindable parameter are not implemented in `MariloGantt.razor.cs`. Cannot be demoed until source adds the EventCallback. Not a worker decision. |
| **EUX-06** | No `refresh-data.md` demo — no demo mutates `Data` in place and calls `Rebind()`, no demo swaps the `Data` reference, no demo uses `ObservableCollection<GanttTask>` | `refresh-data.md` | P2 | Add a `RefreshData.razor` page with two buttons: "Mutate in place (call Rebind)" and "Swap reference". Show the tree re-render in both modes. |
| **EUX-07** | Column-chooser / column visibility toggle not demoed anywhere | `state.md` (`VisibleColumns` PropertyName), `overview.md` column reference | P2 | Needed to make EUX-03 observable. Can be combined into the State demo. |
| **EUX-08** | No drag-to-move / drag-to-resize demo on the timeline bars | `events.md` (`OnUpdate`) | P2 | Carried forward from 2026-04-10 G3 — still outstanding, not a regression. Editing.razor only covers double-click inline edit, not drag gestures. |

### Status per topic spec (final)

- `overview.md` — **Covered** (base surface), with **Missing** sub-items EUX-01, EUX-02 for undocumented source behaviours.
- `events.md` — **Partial** (update/delete/create covered; OnStateChanged missing = EUX-03; TaskListWidthChanged blocked-by-source = EUX-05).
- `refresh-data.md` — **Missing** (EUX-06).
- `state.md` — **Missing / Blocked-by-orchestrator-decision** (EUX-03 can be built; EUX-04 cannot until API decision).

### Orphan demos

None. Every existing demo maps to at least one topic spec or documented parameter set. No orphan pages to remove.

### Notes to orchestrator

- EUX-01, EUX-02, EUX-03, EUX-06, EUX-07, EUX-08 are all worker-resolvable in a Wave 3 stage (02-example-ux remediation) — they need new demo files under `samples/Marilo.Demo/Pages/Components/Gantt/` and route registration. Source is correct per design; no public-API changes required.
- EUX-04 is **Blocked-by-orchestrator-decision** — it depends on how the user resolves the `GanttState<TItem>` public-API questions raised in Wave 1 (NM-01/NM-02/SA-01/SA-02). Do not schedule a demo for it until that decision is made.
- EUX-05 is **Blocked-by-source** — `TaskListWidthChanged` / `@bind-TaskListWidth` require a source addition that is out of scope for `w-gantt-delivery` (demo worker only owns demo files). Escalate to source lane.
- The 2026-04-10 gap file's G1 (sorting/filtering) and G2 (dependencies) are closed by `Features.razor`, which was not counted in the original coverage table. G3 (drag interactions) is carried forward as EUX-08.
- Cerebrum entry suggestion for orchestrator to apply at review time: "Gantt demo audits must count `Features.razor` — the 2026-04-10 coverage table omitted it and mis-reported 2 gaps."
