# MariloGantt — Wave 4 Second-Cycle Intake

**Date:** 2026-04-11
**Cycle:** SECOND — layers Wave 4 findings onto the completed first-cycle workspace
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Worker:** `w-gantt-gap-analysis` (second-cycle dispatch, tick 9)
**Supersedes:** nothing. This file ADDS to `gantt-spec-gap-list.md` (first-cycle intake) and is read alongside `output/stage-06/gantt-closure-report.md`.
**Method:** Read-only cross-reference of the Wave 4 delivery report against the first-cycle closure report. No first-cycle artifact has been mutated.

---

## Status of the first cycle

First cycle (`stages 01 → 06`) is CLOSED. ~60 gaps resolved, ~47 deferred, 648 tests passing, 0 build warnings/errors. See `output/stage-06/gantt-closure-report.md` for the authoritative closure record. **No item in that file is re-assessed or re-closed here.**

This intake only lists:

1. **Genuinely new findings** surfaced by Wave 4 (Wave 1 spec-review static analysis, Wave 2 example-ux audit, Wave 3 visual-parity static analysis, Wave 4 sync-check) that were NOT covered by the first cycle.
2. **Items that extend** a first-cycle resolution (partial fix / coverage gap remains).
3. **Items routed to cross-component lanes** (flagged, NOT to be resolved in this workspace).
4. **Items already-queued** in this workspace via a prior decision record (flagged so Wave 4 doesn't double-count them).
5. **Tracked-Out-of-Session items** (explicitly not in this intake's resolution path).

---

## Record format

Each record:

| Field | Meaning |
|---|---|
| `id` | Wave-4 origin ID (SA-NN / SRC-NN / NM-NN / EUX-NN / VP-gantt-NN) |
| `origin_stage` | Wave 1 / Wave 2 / Wave 3 / Wave 4 |
| `priority` | P1 / P2 / P3 (inherited where available, otherwise inferred from severity) |
| `scope` | systematic / batch / single |
| `sync_areas` | spec / source / demo / docs / tests / gap-plan (the Marilo six) |
| `rationale` | Why it is NEW / partial / re-open / cross-component / already-queued |
| `cross_ref` | `new` / `partial` / `re-open` / `cross-component:<lane>` / `already-queued:<decision>` / `out-of-session` |

---

## Section A — NEW gaps (this workspace owns them)

### Spec coverage additions (Wave 1 source-ahead — refine first-cycle E1/E2/E3 closures)

#### W4-INT-01 — `"VisibleColumns"` PropertyName value absent from `state.md` enumeration
- `id`: SRC-04 + NM-03 (same site, two tags)
- `origin_stage`: Wave 1 (spec-review)
- `priority`: P2
- `scope`: single
- `sync_areas`: spec, gap-plan
- `rationale`: `MariloGantt.razor.cs:211` raises `FireStateChanged("VisibleColumns")`; `docs/component-specs/gantt/state.md:63` enumerates only `"SortDescriptor"`, `"FilterValues"`, `"ExpandedItems"`, `"View"`. First-cycle T2-04 and T2-12 updated state.md, but the enumeration line was not touched. Strictly a spec gap — source is correct per design.
- `cross_ref`: new

#### W4-INT-02 — Overview parameter table under-populated
- `id`: SA-06
- `origin_stage`: Wave 1
- `priority`: P1
- `scope`: batch
- `sync_areas`: spec, gap-plan
- `rationale`: `docs/component-specs/gantt/overview.md:157-161` lists only `RowHeight` and `GanttToolBarTemplate`. Source exposes 30+ parameters on `MariloGantt<TItem>` plus many more on `GanttColumn`/`GanttCommandColumn`/views. First-cycle resolved individual parameters but did not rewrite the overview parameter table. Wave 4 BLOCKED checklist items 1.1 and 1.4 depend on this.
- `cross_ref`: new

#### W4-INT-03 — Overview "Gantt Reference and Methods" table missing `GetState()` / `SetStateAsync(...)`
- `id`: SA-08
- `origin_stage`: Wave 1
- `priority`: P2
- `scope`: single
- `sync_areas`: spec
- `rationale`: `docs/component-specs/gantt/overview.md:226-228` references `Rebind` only. Source `MariloGantt.razor.cs:1680,1708` also exposes `GetState()` and `SetStateAsync(GanttState<TItem>?)` as public methods. First-cycle Phase B added these methods and wrote Phase 1/2 docs in state.md but did not back-fill the overview table.
- `cross_ref`: new

#### W4-INT-04 — Stale namespace reference `Marilo.Blazor.Components.MariloGantt-1`
- `id`: SA-07 + NM-04
- `origin_stage`: Wave 1
- `priority`: P2
- `scope`: single
- `sync_areas`: spec
- `rationale`: `docs/component-specs/gantt/overview.md:221,253` cross-refs `Marilo.Blazor.Components.MariloGantt-1` and `Marilo.Blazor.Components.GanttState-1`. Actual namespace is `Marilo.Components.DataDisplay` — slug links resolve nowhere. Pure spec fix.
- `cross_ref`: new

#### W4-INT-05 — Stale DataGrid copy-paste in `state.md` (paging bullet)
- `id`: SA-03
- `origin_stage`: Wave 1
- `priority`: P2
- `scope`: single
- `sync_areas`: spec
- `rationale`: `state.md:108` says "Filtering always resets the current page to 1, so the `OnStateChanged` event will fire twice. First, `PropertyName` will be equal to `\"Page\"`". MariloGantt has no paging model. Leftover from DataGrid copy. First cycle edited `state.md` for Phase 1/2 split but did not remove this bullet.
- `cross_ref`: new

#### W4-INT-06 — Milestone / summary-task spec coverage gap at `overview.md` + `timeline/templates/task.md` + `gantt-tree/data-binding/overview.md`
- `id`: SRC-01 + SRC-02 + SRC-03
- `origin_stage`: Wave 1
- `priority`: P2
- `scope`: batch
- `sync_areas`: spec
- `rationale`: First-cycle E1 (SPEC-gantt-512 milestone) and E2 (SPEC-gantt-513 summary auto-aggregation) added the source behaviour and CSS classes. Wave 1 re-audit confirms the source is correct but the coverage gap remains at the three files listed — the first-cycle spec edits landed elsewhere (not in `overview.md`, not in `timeline/templates/task.md`, not in `gantt-tree/data-binding/overview.md`). This is a partial resolution — source is DONE, spec coverage at these specific files is NOT.
- `cross_ref`: partial

#### W4-INT-07 — `Data` rebind semantics / `OnParametersSet` auto-detect expanded coverage
- `id`: SRC-06
- `origin_stage`: Wave 1
- `priority`: P2
- `scope`: single
- `sync_areas`: spec
- `rationale`: First-cycle T2-12 ("Auto-detection + Rebind recomputation") closed the bulk of the observation, but Wave 1 says `refresh-data.md` still does not explain the reference-and-count detection strategy plus explicit `Rebind()` for in-place mutations. Partial first-cycle closure.
- `cross_ref`: partial

### Demo / example-UX coverage gaps (Wave 2 — this workspace never addressed demos in the first cycle)

#### W4-INT-08 — No milestone (zero-duration) demo page
- `id`: EUX-01
- `origin_stage`: Wave 2
- `priority`: P1
- `scope`: single
- `sync_areas`: demo
- `rationale`: First cycle added source + SCSS for milestone diamonds but no demo exercises the diamond rendering. Wave 2 `gantt-example-ux-gap-list.md` flagged this as P1.
- `cross_ref`: new

#### W4-INT-09 — No summary-task auto-aggregation demo page
- `id`: EUX-02
- `origin_stage`: Wave 2
- `priority`: P1
- `scope`: single
- `sync_areas`: demo
- `rationale`: Every existing Gantt demo pre-fills parent rows so the bottom-up `ComputedStart/End/PercentComplete` behaviour is invisible. Source correct per first cycle; demo coverage missing.
- `cross_ref`: new

#### W4-INT-10 — No `OnStateChanged` demo
- `id`: EUX-03
- `origin_stage`: Wave 2
- `priority`: P2
- `scope`: single
- `sync_areas`: demo
- `rationale`: Wave 2 requests a `State.razor` demo page logging `PropertyName`. First cycle wired the event; demo missing.
- `cross_ref`: new

#### W4-INT-11 — No `refresh-data.md` demo
- `id`: EUX-06
- `origin_stage`: Wave 2
- `priority`: P2
- `scope`: single
- `sync_areas`: demo
- `rationale`: `refresh-data.md` has zero demo coverage. Requested: `RefreshData.razor` page covering in-place mutation + reference-swap.
- `cross_ref`: new

#### W4-INT-12 — No column-chooser / `VisibleColumns` toggle demo
- `id`: EUX-07
- `origin_stage`: Wave 2
- `priority`: P2
- `scope`: single
- `sync_areas`: demo
- `rationale`: Source supports it (see W4-INT-01 — `FireStateChanged("VisibleColumns")`) but no demo exercises the toggle path.
- `cross_ref`: new

### Visual-parity gaps that this workspace owns (Wave 3 — first-cycle E7 created the SCSS scaffold but did not reach these rules)

#### W4-INT-13 — `.mar-gantt__bar` has NO base rule in any provider (zero-height bars)
- `id`: VP-gantt-01
- `origin_stage`: Wave 3
- `priority`: P1 (CRITICAL)
- `scope`: batch
- `sync_areas`: source, spec, tests, gap-plan
- `rationale`: First-cycle E7 created `_gantt.scss` for Fluent and `_bridge-gantt.scss` for Bootstrap with rules for `__milestone`, `__bar--summary`, `__bar-progress`, `__bar-delete`, filter-menu, command buttons, incell cursor — but NEVER a base `.mar-gantt__bar { position; height; background; border-radius; color; ... }` rule. Razor emits the class at `MariloGantt.razor:582,688`; without the base rule, bars fall back to zero-height browser default `<div>`s. Precondition for every other visual state fix (VP-gantt-06, 07, 08, 14 all depend on this). This is the single most critical source/spec fix in Wave 4.
- `cross_ref`: new

#### W4-INT-14 — Dependency SVG stroke hardcoded `#999` in razor, no arrowhead
- `id`: VP-gantt-03
- `origin_stage`: Wave 3
- `priority`: P1 (CRITICAL)
- `scope`: batch
- `sync_areas`: source, spec
- `rationale`: `MariloGantt.razor:625` (DayView) and `:731` (MonthView) both emit `<polyline … stroke="#999" stroke-width="1.5" … />`. Inline, no class, no token, no marker-end arrowhead. First cycle did not touch dependency rendering. Fix is cross-artifact: razor change + SCSS addition in all three providers + `<svg><defs><marker>` definition. (The literal `#999` is NOT the same anti-pattern as cross-component Pattern 4 `#fff` — it is a Gantt-specific inline attribute on an SVG element, which the `#fff` SCSS sweep will not catch.)
- `cross_ref`: new

#### W4-INT-15 — Today / current-date vertical line feature entirely missing
- `id`: VP-gantt-04
- `origin_stage`: Wave 3
- `priority`: P2
- `scope`: single
- `sync_areas`: source, spec, demo, tests
- `rationale`: No razor element, no SCSS rule, no parameter for a today-line marker. Wave 3 rated this MAJOR. Needs `ShowTodayMarker` parameter (default true) + `<div class="mar-gantt__today-line">` + rules in all three providers.
- `cross_ref`: new

#### W4-INT-16 — Milestone is a Unicode glyph, not a shape primitive
- `id`: VP-gantt-05
- `origin_stage`: Wave 3
- `priority`: P2
- `scope`: single
- `sync_areas`: source, spec
- `rationale`: Refines first-cycle E1 (SPEC-gantt-512): the diamond renders via `&#x25C6;` glyph at `MariloGantt.razor:573,679`. Cannot be precisely sized, subject to emoji-substitution on Android/Material. First cycle added the feature; Wave 3 says the implementation approach needs upgrading to a CSS `transform:rotate(45deg)` square or inline `<svg><rect>`.
- `cross_ref`: partial

#### W4-INT-17 — Summary bar has only opacity + floating bottom-border, no trapezoid shape
- `id`: VP-gantt-06
- `origin_stage`: Wave 3
- `priority`: P2
- `scope`: single
- `sync_areas`: source, spec
- `rationale`: Refines first-cycle E2. `mar-gantt__bar--summary` currently uses `opacity:0.85; border-bottom:2px solid …;` which assumes a base `.mar-gantt__bar` (see W4-INT-13). Must be rebuilt as a distinct trapezoid via `clip-path` after W4-INT-13 lands. Depends on W4-INT-13.
- `cross_ref`: partial

#### W4-INT-18 — Task hover has no bar-background change (only delete glyph reveal)
- `id`: VP-gantt-07
- `origin_stage`: Wave 3
- `priority`: P2
- `scope`: single
- `sync_areas`: source
- `rationale`: `.mar-gantt__bar:hover .mar-gantt__bar-delete { display:inline-flex; }` is the only hover rule. No fill darkening, no cursor change, no elevation. Primary state per rubric. Depends on W4-INT-13 base rule.
- `cross_ref`: new

#### W4-INT-19 — Task selected state has no style rule anywhere
- `id`: VP-gantt-08
- `origin_stage`: Wave 3
- `priority`: P2
- `scope`: batch
- `sync_areas`: source, spec, tests
- `rationale`: Grep for `selected` in Gantt SCSS / razor returns zero styled hits. No `.mar-gantt__bar--selected`, no `is-selected` modifier, no row-selection visual in the tree column. May involve adding a selection-state model to the razor (check during design stage); SCSS rule required in all three providers.
- `cross_ref`: new

#### W4-INT-20 — Task-list row chrome missing (no row-height, border, header background, hover)
- `id`: VP-gantt-09
- `origin_stage`: Wave 3
- `priority`: P2
- `scope`: batch
- `sync_areas`: source, spec
- `rationale`: Razor emits `.mar-gantt__tasklist`, `.mar-gantt__tasklist-header`, `.mar-gantt__task-cell` but no SCSS rule defines row-height, header background, column separator, or hover background. First-cycle E7 added feature-specific rules (filter menu, command buttons, incell cursor) but not the tree-grid chrome.
- `cross_ref`: new

#### W4-INT-21 — Timeline header has no separator, background, typography, or sticky-top
- `id`: VP-gantt-10
- `origin_stage`: Wave 3
- `priority`: P2
- `scope`: batch
- `sync_areas`: source
- `rationale`: Classes `.mar-gantt__timeline-header`, `--main`, `--secondary`, `.mar-gantt__date-label` declared in razor but unstyled in all three providers. Needs tiered heights, font weights, cell separators, sticky positioning.
- `cross_ref`: new

#### W4-INT-22 — Progress-fill formula inconsistency (Fluent `color-mix` vs Bootstrap `rgba(…,0.3)`)
- `id`: VP-gantt-11
- `origin_stage`: Wave 3
- `priority`: P3
- `scope`: single
- `sync_areas`: source, spec
- `rationale`: Two different formulas produce visibly different fills. Pick one (recommend `color-mix` everywhere, expose as `--marilo-gantt-progress-fill` token). Document in `timeline/overview.md`.
- `cross_ref`: new

#### W4-INT-23 — Tree-column indent pixel math lives in razor, not SCSS
- `id`: VP-gantt-12
- `origin_stage`: Wave 3
- `priority`: P3
- `scope`: single
- `sync_areas`: source
- `rationale`: `MariloGantt.razor:253,297,347,392` emits `padding-left:{pad}px` inline computed in code-behind. Should move to `style="--depth:{n}"` + SCSS `calc(var(--marilo-gantt-indent-per-level) * var(--depth))` so the per-level indent is theme-tokenized.
- `cross_ref`: new

#### W4-INT-24 — Filter-menu elevation uses literal `rgba(0,0,0,0.15)` instead of an elevation token
- `id`: VP-gantt-13
- `origin_stage`: Wave 3
- `priority`: P3
- `scope`: single
- `sync_areas`: source
- `rationale`: Fluent `components/_gantt.scss:124`. Related to but NOT the same as cross-component Pattern 4 (`#fff`) — this is a shadow literal, not a surface literal. Gantt-local fix. Worth flagging alongside the Pattern 4 sweep as a sibling hygiene item, but the fix belongs in this workspace because it depends on a Fluent elevation-token decision.
- `cross_ref`: new

#### W4-INT-25 — No `:focus-visible` outline on bars, rows, or milestones
- `id`: VP-gantt-14
- `origin_stage`: Wave 3
- `priority`: P2 (WCAG 2.4.7)
- `scope`: batch
- `sync_areas`: source, tests
- `rationale`: Grep for `focus-visible`/`:focus` in Gantt SCSS returns ONE hit (`.mar-gantt__skip-link:focus`). No focus ring on interactive Gantt elements. First-cycle D1 added skip-nav links but did not add focus rings elsewhere. Partial to D1 scope; new work per VP-gantt-14.
- `cross_ref`: partial

### Sync-check spec-source divergences that belong to this workspace

#### W4-INT-26 — Spec `state.md` example uses non-existent `ColumnResizable` + `@bind-TaskListWidth`
- `id`: SA-04
- `origin_stage`: Wave 1 / Wave 4 (Section 1.2)
- `priority`: P2
- `scope`: single
- `sync_areas`: spec
- `rationale`: `state.md:186-189` example uses parameters that do not exist in source. Column resize (SPEC-gantt-403) is in the first-cycle "Remaining Deferred Items" list (needs JS interop). The spec example must either be removed / commented-out or conditionally gated until the deferred feature lands. Pure spec fix — do not re-open the deferred source feature.
- `cross_ref`: new (spec-only scope)

---

## Section B — Items routed to CROSS-COMPONENT lanes (NOT this workspace)

Per `.claude/orchestrator/decisions/tick-8-2026-04-11-1830.md` Cross-Component Patterns, the following Wave 4 findings are NOT to be resolved inside `gantt-gap-analysis`. They are tagged here for traceability only.

#### W4-ROUTE-01 — Fluent provider has zero `[data-marilo-theme="dark"]` Gantt blocks
- `id`: VP-gantt-02
- `origin_stage`: Wave 3
- `priority`: P1 (CRITICAL)
- `scope`: systematic
- `sync_areas`: source (SCSS only)
- `route_to`: **Cross-Component Pattern 2 — `_dark-mode.scss` as mandatory convention.** Tick-8 promoted this to cerebrum: `_dark-mode.scss` partial becomes mandatory in every `src/Marilo.Providers.FluentUI/Styles/components/<component>/` folder.
- `rationale`: Gantt is one instance of a cross-component pattern (allocation-scheduler VP-006 invisible-text bug is a sibling). Fixing only Gantt's dark-mode gap in this workspace would leave the convention unenforced. The Fluent Gantt dark block must land as part of the repo-wide hygiene lane, not here.
- `cross_ref`: cross-component:dark-mode-hygiene

#### W4-ROUTE-02 — Duplicate `_gantt.scss` in Fluent provider (`Styles/` root vs `Styles/components/`)
- `id`: VP-gantt-15
- `origin_stage`: Wave 3
- `priority`: P3 (minor / maintenance)
- `scope`: single
- `sync_areas`: source
- `route_to`: **Cross-Component Pattern 3 — SCSS dedup lane.** Tick-8: "SCSS lives only in `Styles/components/`. Root-level `_<component>.scss` is a copy-paste artifact. Add SHA-comparison check to CI pipeline to prevent recurrence." Single orchestrator-dispatched lane with `files_owned` limited to the duplicate files to delete.
- `rationale`: Confirmed during this intake: both `src/Marilo.Providers.FluentUI/Styles/_gantt.scss` and `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss` exist. Allocation-scheduler has the same duplication. One lane deletes all provider duplicates at once.
- `cross_ref`: cross-component:scss-dedup

#### W4-ROUTE-03 — `#fff` literal fallbacks (and sibling literals) in Gantt SCSS
- `id`: (no direct VP-gantt ID; pattern match against tick-8 Pattern 4)
- `origin_stage`: Wave 3 (observed in passing during VP-gantt-01 / VP-gantt-02 analysis)
- `priority`: P3
- `scope`: systematic
- `sync_areas`: source
- `route_to`: **Cross-Component Pattern 4 — `#fff` → `var(--mar-color-surface, #fff)` sweep.** Global find-replace across `src/Marilo.Providers.FluentUI/Styles/**/*.scss`. Tick-8 promoted this as a single orchestrator-dispatched lane.
- `rationale`: Any `#fff` literal fallbacks inside Gantt SCSS are part of the 19-instance repo-wide count tick-8 identified. NOT this workspace's job. (Note: VP-gantt-03 `stroke="#999"` and VP-gantt-13 `rgba(0,0,0,0.15)` are different literals with different fix strategies — both stay in this workspace; see W4-INT-14 and W4-INT-24.)
- `cross_ref`: cross-component:fff-literal-sweep

#### W4-ROUTE-04 — Material `_gantt.scss` is a 5-line TODO stub
- `id`: VP-gantt-16
- `origin_stage`: Wave 3
- `priority`: BLOCKER (but deferred by policy)
- `scope`: systematic
- `sync_areas`: source
- `route_to`: **Cross-Component Pattern 5 — Material tech-debt tracker** at `docs/provider-material/OPEN-STUBS.md`. Tick-8: "Accepted as intentional technical debt for now. Do NOT let agents expand stubs in the current wave. Material is not Wave 4 scope."
- `rationale`: Material provider runtime project has not been scaffolded. This is a provider-wide blocker, not a Gantt-specific one. Will be resolved in a future "Material provider implementation" lane that is explicitly not part of any per-component delivery workflow.
- `cross_ref`: cross-component:material-stub-tracker

---

## Section C — Already-queued (do NOT duplicate)

#### W4-QUEUED-01 — `GanttState<TItem>` SortDescriptors / FilterDescriptors divergence
- `id`: SA-01 + SA-02 + NM-01 + NM-02 (four Wave 1 tags, single underlying decision)
- `origin_stage`: Wave 1 / Wave 4 (Section 1.3, 2.7)
- `already-queued`: **`gantt-state-shape` tick-6 decision (2026-04-11T17:20Z).**
- `decision_summary`: User resolved in tick 6: source `GanttState<TItem>` moves to spec shape (`SortDescriptors`/`FilterDescriptors` as lists, `Marilo.DataSource.*` descriptor types). This is a breaking change. It belongs to this workspace as a separate lane (already reflected in memory notes and in worker state JSON notes).
- `wave4_observations`: Wave 1 SA-01, SA-02, NM-01, NM-02 are four symptoms of the same decision. The Wave 4 sync-check report (Section 1.3, Section 5.1) also flags this as the root cause of 1.3 BLOCKED, 2.7 BLOCKED, and 5.1 BLOCKED. **Do not create a new record — this is the existing `gantt-state-shape` work.**
- `sync_areas`: source, spec, demo, docs, tests (full spread — breaking public-API change)
- `cross_ref`: already-queued:gantt-state-shape

---

## Section D — Tracked-Out-of-Session (NOT in this intake's resolution path)

These items are gated by the queued `gantt-state-shape` decision or by a deferred source feature. They are recorded here only for traceability and must NOT be picked up as independent work.

#### W4-OOS-01 — `GetState()` / `SetStateAsync()` save-restore demo and visual audit
- `id`: EUX-04 + VP-gantt-17
- `status`: DEFERRED-PENDING-SOURCE
- `blocked_on`: `gantt-state-shape` (W4-QUEUED-01) — demo cannot be built until source rewrite lands
- `sync_areas`: demo, tests
- `cross_ref`: out-of-session

#### W4-OOS-02 — `TaskListWidthChanged` / `@bind-TaskListWidth` splitter demo and visual audit
- `id`: EUX-05 + VP-gantt-18
- `status`: DEFERRED-PENDING-SOURCE
- `blocked_on`: source feature addition (same family as W4-INT-26 / SA-04 / SA-05). First-cycle closure report lists column reorder + resize in "Remaining Deferred Items" as "needs JS interop". The `TaskListWidthChanged` event is part of that same deferred bucket — it is NOT a Wave 4 regression on a previously-closed item.
- `sync_areas`: source, demo, tests
- `cross_ref`: out-of-session

#### W4-OOS-03 — `TaskListWidthChanged` / column-resize source feature
- `id`: SA-05 (Wave 1) + EUX-08 drag-to-move / drag-to-resize carryover
- `status`: first-cycle carryover — tracked in closure report "Remaining Deferred Items"
- `closure_ref`: `output/stage-06/gantt-closure-report.md:174-175` ("Column reorder + resize | 402-403 | Needs JS interop") and `:176-177` ("Timeline drag-move + resize | 501-502 | Needs JS interop")
- `rationale`: Wave 4 did not re-open these. They remain deferred. Listed here only so Wave 4 tags (SA-05, EUX-08) are accounted for.
- `cross_ref`: first-cycle-carryover

---

## Section E — Items explicitly NOT opened as new records (with reason)

| Wave 4 ID | Reason | Authority |
|---|---|---|
| SPEC-gantt-512 (milestone) | Closed as first-cycle E1. Wave 1 spec coverage gap is W4-INT-06 (partial), not a re-open. | `output/stage-06/gantt-closure-report.md` Phase E section |
| SPEC-gantt-513 (summary auto-aggregation) | Closed as first-cycle E2. Wave 1 spec coverage gap is W4-INT-06 (partial). | same |
| SPEC-gantt-719 (skip nav) | Closed as first-cycle D1. No Wave 4 regression. | closure report Phase D |
| SPEC-gantt-721 (high-contrast) + SPEC-gantt-722 (reduced motion) | Closed as first-cycle E7 (`@media` blocks in `_gantt.scss` + `_bridge-gantt.scss`). | closure report Phase E7 |
| SPEC-gantt-501/502 (bar drag-move/resize) | Explicitly deferred in first-cycle "Remaining Deferred Items". EUX-08 carries the demo-side of this forward but no new source work. | closure report "Remaining Deferred Items" rows 176-177 |
| SPEC-gantt-205-209 (OriginalEditItem, InsertedItem, ParentItem wiring) | Explicitly deferred in first-cycle (needs item-cloning strategy). SRC-05 reiterates the same observation — not a new gap. | closure report "Remaining Deferred Items" row 170 |
| SPEC-gantt-213-214 (TaskListWidth, ColumnStates) | Explicitly deferred (depends on column reorder/resize). | closure report "Remaining Deferred Items" row 171 |
| SPEC-gantt-600-618 (dependency component model) | Explicitly deferred. VP-gantt-03 (W4-INT-14) is a visual-side fix, not a resurrection of the component-model rewrite. | closure report "Remaining Deferred Items" row 172 |

---

## Routing Summary

Raw Wave 4 symptom count (before deduplication against first cycle): **33** (Wave 1 SA-01..08 + SRC-01..06 + NM-01..04 = 18 tags, Wave 2 EUX-01..08 = 8 tags, Wave 3 VP-gantt-01..16 = 16 gaps minus VP-gantt-17/18 already-classified = 16, total 42 tags; deduplicated by cross-reference → 33 distinct symptoms; many collapse to the same root cause).

Deduplicated and routed:

| Disposition | Count | IDs |
|---|---|---|
| **NEW — this workspace owns, resolve in second cycle** | **26** | W4-INT-01 through W4-INT-26 |
| — of which, `new` scope only | 20 | (see Section A: 01, 02, 03, 04, 05, 08, 09, 10, 11, 12, 13, 14, 15, 18, 19, 20, 21, 22, 23, 24) |
| — of which, `partial` — extends first-cycle work | 5 | W4-INT-06, W4-INT-07, W4-INT-16, W4-INT-17, W4-INT-25 |
| — of which, `spec-only scope` noting deferred-feature divergence | 1 | W4-INT-26 |
| **RE-OPENED (regressions on first-cycle closed items)** | **0** | None. Wave 4 did not surface any regression; every Wave 1/2/3 finding against a first-cycle closed item is either (a) a spec coverage gap at a file the first cycle did not touch, or (b) a refinement of the chosen implementation approach — neither counts as a re-open per the escalation rules. If this determination is wrong, it should be escalated as `architecture-question`. |
| **CROSS-COMPONENT routed** | **4** | W4-ROUTE-01 (VP-gantt-02 → dark-mode hygiene lane), W4-ROUTE-02 (VP-gantt-15 → SCSS dedup lane), W4-ROUTE-03 (`#fff` sweep → literal replace lane), W4-ROUTE-04 (VP-gantt-16 → Material tech-debt tracker) |
| **ALREADY-QUEUED in this workspace** | **1** | W4-QUEUED-01 (`gantt-state-shape` from tick 6 — absorbs SA-01, SA-02, NM-01, NM-02) |
| **TRACKED-OUT-OF-SESSION (blocked on queued or deferred work)** | **3** | W4-OOS-01 (EUX-04 / VP-gantt-17), W4-OOS-02 (EUX-05 / VP-gantt-18), W4-OOS-03 (SA-05 / EUX-08 source deferral) |
| **First-cycle carryover already in closure report (no new record)** | **8** | SPEC-gantt-512, -513, -719, -721, -722, -501/502, -205/206/207/208/209, -213/214, -600..618 (see Section E for full mapping) |

### Second-cycle workspace load

- Gaps this workspace will RESOLVE (new + partial): **26** (W4-INT-01..26)
- Gaps this workspace must NOT duplicate: **1** (`gantt-state-shape` — one existing lane)
- Gaps routed elsewhere: **4** (dark-mode, SCSS dedup, `#fff` sweep, Material stub)
- Gaps tracked-out-of-session: **3**
- First-cycle carryover items (no new record): **8**

### Sync-area load (for W4-INT-01..26)

| Sync area | Record count | Notes |
|---|---|---|
| spec | 17 | spec coverage / table / namespace / example rewrites |
| source | 13 | SCSS additions + razor changes for VP-gantt-01..14 refinements |
| demo | 5 | EUX-01, EUX-02, EUX-03, EUX-06, EUX-07 |
| tests | 5 | VP-gantt-01 base rule + selected state + focus-visible + today-line + state-change coverage |
| docs | 0 | (no standalone docs changes identified — all roll into spec) |
| gap-plan | 26 | Every record lands in this intake and gets prioritized in Stage 02 |

---

## Notes for orchestrator review

1. **No first-cycle file was mutated.** This intake is a single new file at `output/stage-01/gantt-wave4-intake.md`. The first-cycle files `output/stage-01/gantt-spec-gap-list.md` and `output/stage-06/gantt-closure-report.md` are untouched; their content is cited read-only.
2. **No regression claims.** Section A "partial" entries (W4-INT-06, 07, 16, 17, 25) extend first-cycle work without invalidating it. If the orchestrator judges any of these as a true regression (i.e., the first-cycle closure claimed coverage it did not actually deliver), that is an `architecture-question` escalation, not a re-open in this intake.
3. **`gantt-state-shape` collision check.** Wave 1 tags SA-01, SA-02, NM-01, NM-02 all collapse to this single decision. This intake does NOT create a new record for them — see W4-QUEUED-01.
4. **Cross-component routing assumes tick-8 cerebrum promotions are authoritative.** If the orchestrator decides a pattern should NOT become a cross-component lane (e.g., dark-mode fix is cheap enough to land per-component), re-route W4-ROUTE-01..04 at Stage 02 prioritization rather than re-opening this intake.
5. **VP-gantt-03 hardcoded `#999` vs Pattern 4 `#fff` sweep.** These are tagged separately on purpose: `#999` is an inline SVG attribute (razor source) and `#fff` is an SCSS surface literal. The cross-component sweep (W4-ROUTE-03) covers SCSS only. VP-gantt-03 (W4-INT-14) requires razor edits and a new SVG `<defs><marker>` — that is Gantt-local work.
6. **Wave 4 sync-check Section 6 cross-reference.** The Wave 4 delivery-report row "EUX-01 / EUX-02 / EUX-03 / EUX-06 / EUX-07 / EUX-08 — demo coverage gaps" maps 1:1 to W4-INT-08..12 (EUX-01, -02, -03, -06, -07) plus W4-OOS-03 (EUX-08 / deferred).
7. **Priority distribution** (for Stage 02 downstream): P1=3 (W4-INT-02, 08, 13), P1-CRITICAL-visual=2 (W4-INT-13, 14 — same root as VP-gantt-01/03), P2=16, P3=5. W4-INT-13 is the single blocker for the entire visual-parity remediation pass and should sequence first.

---

## Verification

- **File reads performed this turn** (all read-only where required):
  - `.claude/orchestration/_orchestrator/inbox/w-gantt-gap-analysis.md`
  - `.claude/orchestration/_memory/workers/w-gantt-gap-analysis.json`
  - `ICM/workspaces/gantt-gap-analysis/CLAUDE.md`
  - `ICM/workspaces/gantt-gap-analysis/output/stage-06/gantt-closure-report.md`
  - `ICM/workspaces/gantt-gap-analysis/output/stage-01/gantt-spec-gap-list.md`
  - `ICM/workspaces/gantt-delivery/stages/04-sync-check/output/gantt-delivery-report.md`
  - `ICM/workspaces/gantt-delivery/stages/01-spec-review/output/gantt-spec-gap-list.md`
  - `ICM/workspaces/gantt-delivery/stages/03-visual-parity/output/gantt-visual-parity-gaps.md`
  - `.claude/orchestration/_orchestrator/decisions/tick-8-2026-04-11-1830.md`
- **Directory scans performed:** `output/stage-01/` (confirmed only `gantt-spec-gap-list.md` existed before this turn), `stages/0{1,2,3}-*/output/` (confirmed Wave 1-3 file inventory).
- **Duplicate-SCSS physical check:** `src/Marilo.Providers.FluentUI/Styles/_gantt.scss` and `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss` both exist — VP-gantt-15 / W4-ROUTE-02 confirmed.
- **Build/test verification:** Not applicable — read-only intake, no source changes.
- **First-cycle mutation check:** `gantt-spec-gap-list.md` and `gantt-closure-report.md` were opened read-only. No Edit / Write tool calls targeted them.
- **Records written:** 26 new-owned (W4-INT-01..26), 4 cross-component routes, 1 already-queued, 3 OOS, 8 carryover references. Total disposition = 42 tag resolutions covering 33 distinct Wave 4 symptoms.

**Second-cycle intake STOP — end of Stage 01.** Next stage (02 prioritization) will sequence W4-INT-01..26 against the first-cycle "Remaining Deferred Items" list.
