# MariloGantt — Wave 4 Second-Cycle Priority Lanes

**Date:** 2026-04-11
**Cycle:** SECOND — prioritizes the 26 W4-INT gaps from `output/stage-01/gantt-wave4-intake.md`
**Worker:** `w-gantt-gap-analysis` (tick 10, Stage 02)
**Input:** `output/stage-01/gantt-wave4-intake.md` (26 new-owned gaps, review PASS)
**Scope:** Cluster 26 gaps into sequenced remediation lanes. 4 cross-component items are OUT of scope (W4-ROUTE-01..04). 1 already-queued item (W4-QUEUED-01 / `gantt-state-shape`) is a separate lane outside this document's purview.

---

## Sequencing Principle

**W4-INT-13 (`.mar-gantt__bar` base rule) is Lane 0 / Phase A.** It is the single prerequisite for every visual-parity bar state fix. Without a base rule giving bars height, background, and position, no hover, selected, summary, or focus-visible rule can be verified visually. Lane 0 ships first; all VP bar-state lanes are gated on its completion.

Lanes are ordered by dependency, then by priority (P1 > P2 > P3), then by sync-area isolation (spec-only lanes can run in parallel with source lanes).

---

## Lane Summary

| Lane | Name | Gaps | Priority | Sync Areas | Depends On | Phase |
|------|------|------|----------|------------|------------|-------|
| **L0** | Bar Foundation | 1 | P1-CRITICAL | source, spec, tests, gap-plan | — | A |
| **L1** | Spec Cleanup | 8 | P1–P2 | spec, gap-plan | — | B (parallel) |
| **L2** | Demo Coverage | 5 | P1–P2 | demo, gap-plan | — | B (parallel) |
| **L3** | Dependency SVG | 1 | P1-CRITICAL | source, spec | — | B (parallel) |
| **L4** | Bar States (hover, selected, summary, focus) | 4 | P2 | source, spec, tests | L0 | C |
| **L5** | Task-List & Timeline Chrome | 2 | P2 | source, spec | L0 (partial) | C |
| **L6** | Today Line | 1 | P2 | source, spec, demo, tests | — | B (parallel) |
| **L7** | Milestone Upgrade | 1 | P2 | source, spec | L0 | C |
| **L8** | Token Hygiene (progress fill, indent, elevation) | 3 | P3 | source, spec | L0 (for progress fill visual check) | D |

**Total gaps in lanes: 26** (matches intake count exactly)

---

## Phase Sequencing

```
Phase A:  L0 (Bar Foundation)                    — GATE: base .mar-gantt__bar rule renders bars with visible height
    │
    ▼
Phase B:  L1 + L2 + L3 + L6 (parallel)          — GATE: spec tables accurate, 5 demos build, SVG dependency styled, today-line renders
    │
    ▼
Phase C:  L4 + L5 + L7 (parallel, all need L0)  — GATE: hover/selected/summary/focus states visible; task-list chrome styled; milestone is shape-primitive
    │
    ▼
Phase D:  L8 (token hygiene)                     — GATE: no inline px/rgba/color-mix inconsistencies remain in Gantt SCSS
```

---

## Lane Details

### Lane 0 — Bar Foundation (Phase A)

**CRITICAL PREREQUISITE — must ship before any Phase C lane.**

| Field | Value |
|---|---|
| Gaps | **W4-INT-13** |
| Priority | P1-CRITICAL |
| Sync areas | source, spec, tests, gap-plan |
| Depends on | Nothing |
| Files touched (estimated) | `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_bridge-gantt.scss`, `docs/component-specs/gantt/timeline/overview.md` (spec), bUnit test for bar rendering |
| Gate | `.mar-gantt__bar` renders with non-zero height, visible background, and border-radius in Fluent and Bootstrap providers. bUnit test asserts the class is emitted and SCSS compiles. |

**Detail:** First-cycle E7 created the SCSS scaffold with modifier rules (`__milestone`, `__bar--summary`, `__bar-progress`, `__bar-delete`) but never wrote the base `.mar-gantt__bar` rule. Razor emits the class at `MariloGantt.razor:582,688`. The base rule needs at minimum:
- `position: relative` (for child positioning of progress fill, delete button)
- `height: var(--marilo-gantt-bar-height, 24px)` (tokenized)
- `background: var(--marilo-gantt-bar-bg, var(--colorBrandBackground))`
- `border-radius: var(--marilo-gantt-bar-radius, 4px)`
- `color: var(--marilo-gantt-bar-color, var(--colorNeutralForegroundOnBrand))`
- `cursor: pointer`
- `overflow: hidden` (for progress fill clipping)

All three providers need the rule; Material stub is cross-component (W4-ROUTE-04), so only Fluent + Bootstrap + base are in scope.

---

### Lane 1 — Spec Cleanup (Phase B, parallel)

**All spec-only changes. No source edits. Can run in parallel with L2, L3, L6.**

| Field | Value |
|---|---|
| Gaps | **W4-INT-01, W4-INT-02, W4-INT-03, W4-INT-04, W4-INT-05, W4-INT-06, W4-INT-07, W4-INT-26** |
| Priority | P1 (INT-02), P2 (all others) |
| Sync areas | spec, gap-plan |
| Depends on | Nothing (pure documentation) |
| Files touched (estimated) | `docs/component-specs/gantt/overview.md`, `docs/component-specs/gantt/state.md`, `docs/component-specs/gantt/timeline/templates/task.md`, `docs/component-specs/gantt/gantt-tree/data-binding/overview.md`, `docs/component-specs/gantt/refresh-data.md` |
| Gate | Every parameter on `MariloGantt<TItem>` has a row in the overview parameter table. `state.md` enumeration includes `"VisibleColumns"`. Stale paging bullet removed. Namespace references use `Marilo.Components.DataDisplay`. `GetState()`/`SetStateAsync()` in methods table. `ColumnResizable` example removed or gated. Milestone/summary coverage at the three files listed in INT-06. `refresh-data.md` explains reference-and-count detection. |

**Breakdown:**

| Gap | Summary | Est. Effort |
|---|---|---|
| W4-INT-01 | Add `"VisibleColumns"` to `state.md` PropertyName enumeration | XS |
| W4-INT-02 | Rewrite `overview.md` parameter table to cover 30+ params | M |
| W4-INT-03 | Add `GetState()` / `SetStateAsync()` to overview methods table | XS |
| W4-INT-04 | Fix stale namespace refs (`Marilo.Blazor.Components` → `Marilo.Components.DataDisplay`) | XS |
| W4-INT-05 | Remove DataGrid paging copy-paste bullet from `state.md` | XS |
| W4-INT-06 | Add milestone/summary-task coverage to `overview.md`, `task.md`, `data-binding/overview.md` | S |
| W4-INT-07 | Expand `refresh-data.md` with reference-and-count detection + explicit Rebind semantics | S |
| W4-INT-26 | Remove or gate `ColumnResizable` / `@bind-TaskListWidth` example in `state.md` | XS |

---

### Lane 2 — Demo Coverage (Phase B, parallel)

**Demo-only changes. No source or spec edits. Can run in parallel with L1, L3, L6.**

| Field | Value |
|---|---|
| Gaps | **W4-INT-08, W4-INT-09, W4-INT-10, W4-INT-11, W4-INT-12** |
| Priority | P1 (INT-08, INT-09), P2 (INT-10, INT-11, INT-12) |
| Sync areas | demo, gap-plan |
| Depends on | Nothing (existing source supports all these scenarios) |
| Files touched (estimated) | New `.razor` demo pages under the Gantt demo folder |
| Gate | All 5 demo pages build, render, and exercise their target behavior. `dotnet build` passes. |

**Breakdown:**

| Gap | Demo Page | Target Behavior |
|---|---|---|
| W4-INT-08 | `Milestones.razor` | Zero-duration tasks render as diamonds |
| W4-INT-09 | `SummaryTasks.razor` | Parent rows auto-aggregate Start/End/PercentComplete from children |
| W4-INT-10 | `State.razor` | Logs `OnStateChanged` PropertyName values |
| W4-INT-11 | `RefreshData.razor` | In-place mutation vs reference-swap with `Rebind()` |
| W4-INT-12 | `ColumnChooser.razor` | Toggle column visibility via `VisibleColumns` state |

---

### Lane 3 — Dependency SVG (Phase B, parallel)

**Gantt-local source change: inline `#999` SVG → class-based + arrowhead marker.**

| Field | Value |
|---|---|
| Gaps | **W4-INT-14** |
| Priority | P1-CRITICAL |
| Sync areas | source, spec |
| Depends on | Nothing (independent of bar base rule — dependency lines are separate SVG elements) |
| Files touched (estimated) | `MariloGantt.razor` (DayView ~625, MonthView ~731), Fluent `_gantt.scss`, Bootstrap `_bridge-gantt.scss`, spec `timeline/overview.md` or `dependencies/overview.md` |
| Gate | Dependency polylines use a CSS class (`mar-gantt__dependency-line`), no inline `stroke="#999"`, arrowhead via `<marker>` element. SCSS compiles. |

**Detail:** Replace inline `stroke="#999" stroke-width="1.5"` with:
- `<svg><defs><marker id="mar-gantt-arrow" …><path d="…"/></marker></defs>`
- `<polyline class="mar-gantt__dependency-line" marker-end="url(#mar-gantt-arrow)" …/>`
- SCSS: `.mar-gantt__dependency-line { stroke: var(--marilo-gantt-dependency-color, var(--colorNeutralStroke1)); stroke-width: 1.5px; fill: none; }`

---

### Lane 4 — Bar States (Phase C)

**Hover, selected, summary trapezoid, focus-visible. All depend on L0 base rule.**

| Field | Value |
|---|---|
| Gaps | **W4-INT-17, W4-INT-18, W4-INT-19, W4-INT-25** |
| Priority | P2 |
| Sync areas | source, spec, tests |
| Depends on | **L0 (W4-INT-13)** — bar must have visible base styling before states can be verified |
| Files touched (estimated) | Fluent `_gantt.scss`, Bootstrap `_bridge-gantt.scss`, `MariloGantt.razor`/`.razor.cs` (for selected-state class binding), spec files, bUnit tests |
| Gate | Hover darkens bar fill. Selected state applies `--selected` modifier with visible ring/outline. Summary bar has trapezoid `clip-path`. All interactive elements have `:focus-visible` outline (WCAG 2.4.7). Tests cover each state. |

**Breakdown:**

| Gap | Summary | Notes |
|---|---|---|
| W4-INT-17 | Summary bar trapezoid via `clip-path` | Replaces current `opacity:0.85 + border-bottom` approach |
| W4-INT-18 | Bar hover fill darkening + cursor change | Add `.mar-gantt__bar:hover` rule with `filter:brightness(0.9)` or `background` shift |
| W4-INT-19 | Selected-state model + `.mar-gantt__bar--selected` | May need razor `@class` binding for selection; SCSS rule in all providers; bUnit test |
| W4-INT-25 | `:focus-visible` outline on bars, rows, milestones | Extend D1 skip-nav work; add `outline: 2px solid var(--colorStrokeFocus2)` on interactive elements |

**Design note for W4-INT-19:** If a selection-state model does not exist in source, this gap may require a new `SelectedItem` / `SelectedItems` parameter on `MariloGantt<TItem>`. That would be a public API addition — if the design stage confirms this, escalate as `public-api-change` before implementing.

---

### Lane 5 — Task-List & Timeline Chrome (Phase C)

**Structural chrome: row heights, header backgrounds, separators, sticky positioning.**

| Field | Value |
|---|---|
| Gaps | **W4-INT-20, W4-INT-21** |
| Priority | P2 |
| Sync areas | source, spec |
| Depends on | **L0 (partial)** — row-height token should align with bar-height token from L0 |
| Files touched (estimated) | Fluent `_gantt.scss`, Bootstrap `_bridge-gantt.scss`, spec `gantt-tree/overview.md`, spec `timeline/overview.md` |
| Gate | Task-list rows have defined height, header background, column separator, hover background. Timeline header has tiered heights, font weights, cell separators, `position: sticky; top: 0`. |

**Breakdown:**

| Gap | Summary |
|---|---|
| W4-INT-20 | Task-list row chrome: `--marilo-gantt-row-height`, header bg, column separators, row hover |
| W4-INT-21 | Timeline header chrome: tiered height for main/secondary, font weights, separators, sticky-top |

---

### Lane 6 — Today Line (Phase B, parallel)

**Standalone feature: new parameter + new DOM element + SCSS + demo + test.**

| Field | Value |
|---|---|
| Gaps | **W4-INT-15** |
| Priority | P2 |
| Sync areas | source, spec, demo, tests |
| Depends on | Nothing (renders in the timeline area independently of bar styling) |
| Files touched (estimated) | `MariloGantt.razor` / `.razor.cs` (new `ShowTodayMarker` parameter + `<div class="mar-gantt__today-line">`), Fluent/Bootstrap SCSS, spec `timeline/overview.md`, demo page, bUnit test |
| Gate | Today-line renders as a vertical marker at the current date. `ShowTodayMarker` defaults to `true`. SCSS applies `position:absolute`, token color, `z-index`. bUnit test for parameter + DOM output. |

**Design note:** `ShowTodayMarker` is a new public parameter. This is an additive API change (no breaking change). If the design stage determines it needs a more complex API (e.g., `TodayMarkerTemplate`), escalate as `public-api-change`.

---

### Lane 7 — Milestone Upgrade (Phase C)

**Upgrade milestone rendering from Unicode glyph to CSS shape primitive.**

| Field | Value |
|---|---|
| Gaps | **W4-INT-16** |
| Priority | P2 |
| Sync areas | source, spec |
| Depends on | **L0** — milestone diamond sizing may reference `--marilo-gantt-bar-height` token |
| Files touched (estimated) | `MariloGantt.razor` (replace `&#x25C6;` with `<div class="mar-gantt__milestone-diamond">`), Fluent/Bootstrap SCSS, spec `timeline/templates/task.md` |
| Gate | Milestone renders as a CSS `transform:rotate(45deg)` square (or inline SVG rect). No Unicode glyph. Consistent sizing across platforms. |

---

### Lane 8 — Token Hygiene (Phase D)

**Low-priority cleanup: progress-fill formula, indent pixel math, elevation literal.**

| Field | Value |
|---|---|
| Gaps | **W4-INT-22, W4-INT-23, W4-INT-24** |
| Priority | P3 |
| Sync areas | source, spec |
| Depends on | **L0 (for progress-fill visual verification)** |
| Files touched (estimated) | Fluent `_gantt.scss`, Bootstrap `_bridge-gantt.scss`, `MariloGantt.razor` (indent calc), spec `timeline/overview.md` |
| Gate | Single progress-fill formula (`color-mix` with `--marilo-gantt-progress-fill` token). Indent uses `style="--depth:{n}"` + SCSS `calc()`. Filter-menu elevation uses a Fluent elevation token, not `rgba()` literal. |

**Breakdown:**

| Gap | Summary |
|---|---|
| W4-INT-22 | Unify progress-fill formula: `color-mix` everywhere, expose `--marilo-gantt-progress-fill` token |
| W4-INT-23 | Move tree-column indent from inline `padding-left:{px}` to CSS custom property `--depth` + `calc()` |
| W4-INT-24 | Replace `rgba(0,0,0,0.15)` filter-menu elevation with Fluent elevation token |

---

## Cross-Component Items (OUT OF SCOPE — recorded for traceability)

These 4 items are NOT planned in any lane above. They belong to cross-component orchestrator lanes per tick-8 decisions.

| ID | Pattern | Route |
|---|---|---|
| W4-ROUTE-01 (VP-gantt-02) | Dark-mode hygiene — `_dark-mode.scss` convention | Cross-Component Pattern 2 |
| W4-ROUTE-02 (VP-gantt-15) | SCSS dedup — root-level `_gantt.scss` duplicate | Cross-Component Pattern 3 |
| W4-ROUTE-03 | `#fff` literal sweep in SCSS | Cross-Component Pattern 4 |
| W4-ROUTE-04 (VP-gantt-16) | Material `_gantt.scss` 5-line stub | Cross-Component Pattern 5 |

---

## Already-Queued Item (separate lane, NOT in scope)

| ID | Decision | Lane |
|---|---|---|
| W4-QUEUED-01 | `gantt-state-shape` tick-6 decision (SA-01/SA-02/NM-01/NM-02) | Separate breaking-change lane; full sync-area spread |

---

## Potential Escalation Points

1. **W4-INT-19 (selected state):** If design stage reveals that a `SelectedItem`/`SelectedItems` public API addition is needed, escalate as `public-api-change` before L4 implementation.
2. **W4-INT-15 (today line):** If `ShowTodayMarker` needs a template variant (`TodayMarkerTemplate`), escalate as `public-api-change`.
3. **W4-INT-16 (milestone upgrade):** If the CSS approach conflicts with existing razor template customization (`MilestoneTemplate` parameter), escalate as `architecture-question`.

---

## Verification

- **Input file:** `output/stage-01/gantt-wave4-intake.md` — 26 W4-INT gaps (PASS review)
- **Gaps in lanes:** 1 + 8 + 5 + 1 + 4 + 2 + 1 + 1 + 3 = **26** (matches intake exactly)
- **Cross-component excluded:** 4 (W4-ROUTE-01..04)
- **Already-queued excluded:** 1 (W4-QUEUED-01)
- **OOS excluded:** 3 (W4-OOS-01..03) — not mentioned in lanes, correct
- **First-cycle artifacts:** NOT touched. `gantt-gap-priorities.md` (first-cycle stage-02 output) is untouched.
- **Dependency ordering:** L0 (Phase A) → Phases B/C/D respect the prerequisite chain. No circular dependencies.
- **Priority distribution in lanes:** P1-CRITICAL in L0 and L3 (Phase A and B). P1 in L1 and L2. P2 bulk in Phases B–C. P3 in Phase D.

---

## Stage 02 STOP — end of prioritization. Ready for orchestrator review.
