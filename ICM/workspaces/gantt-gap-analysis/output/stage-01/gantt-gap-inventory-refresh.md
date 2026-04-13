# MariloGantt — Gap Inventory Refresh (Stage 01)

**Date:** 2026-04-12
**Purpose:** Reassess all Gantt gaps after prior-session Wave 1 (S05-W1 bar foundation SCSS) completed.
**Inputs:** `gantt-wave4-intake.md`, `gantt-wave4-priority-lanes.md`, `gantt-s05-w1-report.md`, `gantt-closure-report.md`, `gantt-delivery-report.md`
**Method:** Cross-reference every W4-INT gap against the S05-W1 deliverables to determine CLOSED vs OPEN status.

---

## Summary

| Category | Count |
|---|---|
| First-cycle gaps (Stages 01-06, 2026-04-09) | ~107 unique |
| First-cycle resolved | ~60 |
| First-cycle deferred (carryover) | ~47 |
| Wave 4 second-cycle gaps (W4-INT-01..26) | 26 |
| Cross-component routed (W4-ROUTE-01..04) | 4 |
| Already-queued (W4-QUEUED-01, gantt-state-shape) | 1 |
| Tracked-out-of-session (W4-OOS-01..03) | 3 |
| **CLOSED by S05-W1 (this refresh)** | **1** |
| **Remaining OPEN (this workspace owns)** | **25** |

---

## CLOSED Gap — Resolved by S05-W1 (Bar Foundation)

### W4-INT-13 — `.mar-gantt__bar` has NO base rule in any provider (zero-height bars) — **CLOSED**

| Field | Value |
|---|---|
| Origin | VP-gantt-01 (Wave 3) |
| Priority | P1 (CRITICAL) |
| Closed by | S05-W1 report (`gantt-s05-w1-report.md`, 2026-04-12) |
| Resolution | Added `.mar-gantt__bar` and `.mar-gantt__bar-row` base rules to both FluentUI and Bootstrap SCSS. Introduced 6 design tokens (`--marilo-gantt-bar-height`, `--marilo-gantt-bar-bg`, `--marilo-gantt-bar-radius`, `--marilo-gantt-bar-color`, `--marilo-gantt-bar-font-size`, `--marilo-gantt-row-height`). Added "Bar Rendering" spec section in `timeline/overview.md`. Added 2 bUnit tests. |
| Verification | `dotnet build Marilo.slnx` exit 0; `dotnet test` (Gantt filter) 94 passed, 0 failed |
| Files changed | `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss`, `docs/component-specs/gantt/timeline/overview.md`, `tests/Marilo.Tests.Unit/DataDisplay/MariloGanttTests.cs` |
| Confirmed | Grep verified `.mar-gantt__bar` base rule at line 43 in both provider SCSS files. Spec has "Bar Rendering" section at line 153 of `timeline/overview.md`. |

**Impact on downstream lanes:** L4 (bar states), L5 (task-list/timeline chrome), L7 (milestone upgrade), and L8 (token hygiene) all had W4-INT-13 as a prerequisite. This prerequisite is now satisfied. Phase C and D lanes are unblocked from the L0 dependency.

---

## OPEN Gaps — Grouped by Lane (25 remaining)

### Lane 1 — Spec Cleanup (8 gaps, all spec-only, Phase B)

| ID | Summary | Priority | Status |
|---|---|---|---|
| W4-INT-01 | `"VisibleColumns"` PropertyName absent from `state.md` enumeration | P2 | OPEN |
| W4-INT-02 | Overview parameter table under-populated (30+ params, only 2 listed) | P1 | OPEN |
| W4-INT-03 | Overview methods table missing `GetState()` / `SetStateAsync()` | P2 | OPEN |
| W4-INT-04 | Stale namespace `Marilo.Blazor.Components` in overview.md | P2 | OPEN |
| W4-INT-05 | Stale DataGrid paging bullet in `state.md` | P2 | OPEN |
| W4-INT-06 | Milestone/summary spec coverage gap at overview + task.md + data-binding | P2 | OPEN |
| W4-INT-07 | `refresh-data.md` missing reference-and-count detection explanation | P2 | OPEN |
| W4-INT-26 | `state.md` example uses non-existent `ColumnResizable` / `@bind-TaskListWidth` | P2 | OPEN |

### Lane 2 — Demo Coverage (5 gaps, demo-only, Phase B)

| ID | Summary | Priority | Status |
|---|---|---|---|
| W4-INT-08 | No milestone (zero-duration) demo page | P1 | OPEN |
| W4-INT-09 | No summary-task auto-aggregation demo page | P1 | OPEN |
| W4-INT-10 | No `OnStateChanged` demo | P2 | OPEN |
| W4-INT-11 | No `refresh-data.md` demo | P2 | OPEN |
| W4-INT-12 | No column-chooser / `VisibleColumns` toggle demo | P2 | OPEN |

### Lane 3 — Dependency SVG (1 gap, source+spec, Phase B)

| ID | Summary | Priority | Status |
|---|---|---|---|
| W4-INT-14 | Dependency SVG stroke hardcoded `#999`, no arrowhead marker | P1-CRITICAL | OPEN |

### Lane 4 — Bar States (3 gaps remaining after W4-INT-19 skip, Phase C)

| ID | Summary | Priority | Status | Notes |
|---|---|---|---|---|
| W4-INT-17 | Summary bar: opacity+border only, no trapezoid shape | P2 | OPEN | L0 prerequisite now satisfied |
| W4-INT-18 | Bar hover: no fill darkening, only delete glyph reveal | P2 | OPEN | L0 prerequisite now satisfied |
| W4-INT-25 | No `:focus-visible` outline on bars, rows, milestones | P2 | OPEN | L0 prerequisite now satisfied |

**Skipped from L4:** W4-INT-19 (selected state) — requires `SelectedItem`/`SelectedItems` public API decision. Escalation type: `public-api-change`. Remains pending.

### Lane 5 — Task-List & Timeline Chrome (2 gaps, Phase C)

| ID | Summary | Priority | Status | Notes |
|---|---|---|---|---|
| W4-INT-20 | Task-list row chrome missing (height, border, header bg, hover) | P2 | OPEN | L0 partial prerequisite now satisfied |
| W4-INT-21 | Timeline header: no separator, background, typography, sticky-top | P2 | OPEN | |

### Lane 6 — Today Line (1 gap, Phase B — SKIPPED pending API decision)

| ID | Summary | Priority | Status | Notes |
|---|---|---|---|---|
| W4-INT-15 | Today/current-date line feature entirely missing | P2 | OPEN (SKIPPED) | Requires `ShowTodayMarker` + possible `TodayMarkerTemplate` API decision |

### Lane 7 — Milestone Upgrade (1 gap, Phase C — SKIPPED pending architecture question)

| ID | Summary | Priority | Status | Notes |
|---|---|---|---|---|
| W4-INT-16 | Milestone is Unicode glyph, not CSS shape primitive | P2 | OPEN (SKIPPED) | May conflict with `MilestoneTemplate` parameter |

### Lane 8 — Token Hygiene (3 gaps, Phase D)

| ID | Summary | Priority | Status |
|---|---|---|---|
| W4-INT-22 | Progress-fill formula inconsistency (Fluent color-mix vs Bootstrap rgba) | P3 | OPEN |
| W4-INT-23 | Tree-column indent pixel math in razor, not SCSS | P3 | OPEN |
| W4-INT-24 | Filter-menu elevation uses literal `rgba(0,0,0,0.15)` | P3 | OPEN |

---

## Items NOT in This Workspace's Scope (unchanged from W4 intake)

### Cross-component routed (4 items)

| ID | Pattern | Route |
|---|---|---|
| W4-ROUTE-01 (VP-gantt-02) | Dark-mode hygiene — `_dark-mode.scss` convention | Cross-Component Pattern 2 |
| W4-ROUTE-02 (VP-gantt-15) | SCSS dedup — root-level duplicate `_gantt.scss` | Cross-Component Pattern 3 |
| W4-ROUTE-03 | `#fff` literal sweep | Cross-Component Pattern 4 |
| W4-ROUTE-04 (VP-gantt-16) | Material `_gantt.scss` 5-line stub | Cross-Component Pattern 5 |

### Already-queued (1 item)

| ID | Decision | Status |
|---|---|---|
| W4-QUEUED-01 | `gantt-state-shape` (SA-01/SA-02/NM-01/NM-02) — `GanttState<TItem>` descriptor-type rewrite | Separate breaking-change lane, pending implementation |

### Tracked-out-of-session (3 items)

| ID | Blocked On |
|---|---|
| W4-OOS-01 (EUX-04 / VP-gantt-17) | `gantt-state-shape` source rewrite |
| W4-OOS-02 (EUX-05 / VP-gantt-18) | `TaskListWidthChanged` source feature (JS interop) |
| W4-OOS-03 (SA-05 / EUX-08) | Column resize + drag (JS interop) |

### First-cycle deferred items (unchanged, ~47 items)

These remain as documented in `gantt-closure-report.md` "Remaining Deferred Items" section. No changes from this refresh. Key deferred buckets:
- GanttState partial wiring (OriginalEditItem clone, InsertedItem, ParentItem) — SPEC-gantt-205-209
- GanttState ColumnStates/TaskListWidth — SPEC-gantt-213-214
- GanttDependencies component model — SPEC-gantt-600-618
- Column reorder + resize — SPEC-gantt-402-403
- Column menu + chooser — SPEC-gantt-404, 426-427
- Popup edit mode — SPEC-gantt-409
- Filter checkbox list — SPEC-gantt-415-417
- Timeline drag-move + resize — SPEC-gantt-501-502
- RangeSnapTo / zooming — SPEC-gantt-500
- Screen reader drag announcements — SPEC-gantt-720

---

## Delivery Report Blockers Cross-Reference

The delivery report cited 7 distinct blockers. Updated status after S05-W1:

| Blocker | Delivery Report Severity | Current Status |
|---|---|---|
| VP-gantt-01 (bar base rule) | CRITICAL | **CLOSED** (S05-W1) |
| VP-gantt-02 (Fluent dark-mode) | CRITICAL | Cross-component (W4-ROUTE-01) |
| VP-gantt-03 (dependency SVG) | CRITICAL | OPEN (W4-INT-14) |
| VP-gantt-16 (Material stub) | BLOCKER | Cross-component (W4-ROUTE-04) |
| NM-01/NM-02/SA-01/SA-02 (GanttState shape) | BLOCKER (spec/API) | Already-queued (W4-QUEUED-01) |
| SA-06+SRC-01..06 (spec table) | MAJOR | OPEN (W4-INT-02, W4-INT-06) |
| EUX-01..08 (demo gaps) | MAJOR | OPEN (W4-INT-08..12) |

**Net delivery-report blocker movement:** 1 of 7 blockers resolved by S05-W1. 2 blockers routed cross-component. 1 blocker already-queued. 3 blockers remain OPEN in this workspace.
