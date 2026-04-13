# MariloGantt — Gap Prioritization Refresh (Stage 02)

**Date:** 2026-04-12
**Purpose:** Reprioritize remaining 25 open gaps after S05-W1 closed L0 (bar foundation).
**Input:** `gantt-gap-inventory-refresh.md` (Stage 01 refresh output)
**Prior prioritization:** `gantt-wave4-priority-lanes.md` — 9 lanes, 4 phases (A/B/C/D)

---

## What Changed Since Last Prioritization

### L0 / Phase A — COMPLETE

**W4-INT-13 (`.mar-gantt__bar` base rule) is CLOSED.** S05-W1 delivered:
- Base `.mar-gantt__bar` and `.mar-gantt__bar-row` rules in Fluent and Bootstrap SCSS
- 6 design tokens introduced
- "Bar Rendering" spec section added to `timeline/overview.md`
- 2 bUnit tests added and passing

**Impact:** Phase A is done. Phase C lanes (L4, L5, L7) that depended on L0 are now unblocked. Phase D (L8) token hygiene prerequisite is also satisfied.

### No Other Gaps Resolved

S05-W1 was scoped only to TASK-W4-01 (L0). All 25 remaining gaps are unchanged.

### API Decisions Still Pending

Three gaps remain skipped pending user decisions (unchanged from S03/S04):
- **W4-INT-19** (L4, selected state) — `public-api-change` escalation
- **W4-INT-15** (L6, today line) — `public-api-change` escalation
- **W4-INT-16** (L7, milestone upgrade) — `architecture-question` escalation

---

## Revised Phase Sequencing

Phase A is removed (complete). Remaining phases renumber:

```
Phase B (next):  L1 + L2 + L3  (parallel, no dependencies)
    │            8 spec + 5 demo + 1 SVG source = 14 tasks
    │
    ▼
Phase C:         L4 (minus INT-19) + L5  (parallel, L0 satisfied)
    │            3 bar-state + 2 chrome = 5 tasks
    │
    ▼
Phase D:         L8  (token hygiene)
    │            3 tasks
    │
    ▼
[PENDING]        L6 (INT-15) + L7 (INT-16) + L4 remainder (INT-19)
                 3 tasks, blocked on API/architecture decisions
```

**Total implementable now: 22 tasks across Phases B/C/D.**
**Total blocked on decisions: 3 tasks.**

---

## Wave Plan (Implementation Order)

### Wave 2 — Spec + Demo + SVG (Phase B, parallel lanes)

**14 tasks. All parallelizable. No inter-lane dependencies.**

| Lane | Tasks | Sync Areas | Priority Range |
|---|---|---|---|
| L1 (Spec Cleanup) | W4-INT-01, 02, 03, 04, 05, 06, 07, 26 | spec, gap-plan | P1-P2 |
| L2 (Demo Coverage) | W4-INT-08, 09, 10, 11, 12 | demo, gap-plan | P1-P2 |
| L3 (Dependency SVG) | W4-INT-14 | source, spec | P1-CRITICAL |

**Wave 2 gate:** Every `MariloGantt<TItem>` parameter has a row in `overview.md`. `state.md` enumeration includes `"VisibleColumns"`. Stale paging/namespace/ColumnResizable content removed. 5 demo pages build. SVG dependency lines use CSS class + arrowhead marker with no inline stroke.

**Priority sequencing within Wave 2:**
1. **W4-INT-14** (L3, P1-CRITICAL) — single most impactful visual fix remaining after L0
2. **W4-INT-02** (L1, P1) — overview parameter table rewrite, unblocks delivery checklist items 1.1/1.4
3. **W4-INT-08, W4-INT-09** (L2, P1) — milestone and summary demo pages
4. Remaining P2 items in parallel

### Wave 3 — Bar States + Chrome (Phase C)

**5 tasks. L4 and L5 can run in parallel. All depend on L0 (now satisfied).**

| Lane | Tasks | Sync Areas | Priority |
|---|---|---|---|
| L4 (Bar States, partial) | W4-INT-17, 18, 25 | source, spec, tests | P2 |
| L5 (Chrome) | W4-INT-20, 21 | source, spec | P2 |

**Wave 3 gate:** Hover darkens bar fill. Summary bar has trapezoid clip-path. Interactive elements have `:focus-visible` outline. Task-list rows have defined height/border/hover. Timeline header has sticky positioning, separators, typography.

### Wave 4 — Token Hygiene (Phase D)

**3 tasks. Sequential cleanup.**

| Lane | Tasks | Sync Areas | Priority |
|---|---|---|---|
| L8 (Token Hygiene) | W4-INT-22, 23, 24 | source, spec | P3 |

**Wave 4 gate:** Single progress-fill formula with `--marilo-gantt-progress-fill` token. Tree indent via CSS custom property `--depth` + `calc()`. Filter-menu elevation uses Fluent elevation token.

### Decision-Blocked Items (no wave assigned)

| ID | Lane | Decision Needed | Escalation Type |
|---|---|---|---|
| W4-INT-19 | L4 | `SelectedItem`/`SelectedItems` public API | `public-api-change` |
| W4-INT-15 | L6 | `ShowTodayMarker` + `TodayMarkerTemplate` API | `public-api-change` |
| W4-INT-16 | L7 | CSS shape vs `MilestoneTemplate` conflict | `architecture-question` |

These slot into Wave 3 (INT-19) or Wave 2 (INT-15, INT-16) once decisions are made.

---

## Priority Distribution

| Priority | Open Count | Wave |
|---|---|---|
| P1-CRITICAL | 1 (INT-14) | Wave 2 |
| P1 | 3 (INT-02, INT-08, INT-09) | Wave 2 |
| P2 | 16 (INT-01, 03, 04, 05, 06, 07, 10, 11, 12, 17, 18, 20, 21, 25, 26 + skipped INT-15) | Waves 2-3 + pending |
| P3 | 3 (INT-22, 23, 24) | Wave 4 |
| Skipped (API) | 2 (INT-19, INT-16) | Pending |

---

## Effort Estimates by Wave

| Wave | Tasks | Est. Effort | Notes |
|---|---|---|---|
| ~~Wave 1~~ | ~~1~~ | ~~XS~~ | **COMPLETE** (S05-W1, bar foundation) |
| Wave 2 | 14 | M-L | Bulk is spec table rewrite (INT-02) + 5 new demo pages |
| Wave 3 | 5 | M | SCSS states + chrome; may surface additional design tokens |
| Wave 4 | 3 | S | Cleanup / refactoring |
| Pending | 3 | S-M each | Depend on API decisions |

---

## Delivery Report Impact Projection

If all 22 implementable tasks complete (Waves 2-4):

| Delivery Checklist Area | Current | Projected |
|---|---|---|
| 1.1 All params documented | BLOCKED | PASS (after INT-02) |
| 1.2 All documented params implemented | BLOCKED | Remains BLOCKED (gantt-state-shape) |
| 1.5 All events documented | BLOCKED | Partial improvement (INT-01 adds VisibleColumns) |
| 2.1 Every param has demo | AMBER | Improved (5 new demos) |
| 2.2 Every event has demo | BLOCKED | Partial (INT-10 adds OnStateChanged demo) |
| 3.3 All states reviewed | BLOCKED | Improved to AMBER (hover, summary, focus-visible added) |
| 3.4 Parity score >= 2.5 | BLOCKED | Improved but dark-mode (cross-component) keeps it below target |

**Net: 3 of 7 delivery blockers addressed by this workspace. Remaining 4 depend on cross-component lanes (2) + gantt-state-shape (1) + Material provider (1).**

---

## Recommendations

1. **Start Wave 2 immediately.** L0 prerequisite is satisfied. L1/L2/L3 are independent and can run in parallel.
2. **Escalate the 3 API decisions** (INT-15, INT-16, INT-19) to the user/orchestrator. The sooner these resolve, the sooner they slot into Waves 2-3.
3. **gantt-state-shape lane** (W4-QUEUED-01) remains the single largest remaining blocker for the delivery gate. It is a separate lane with a full sync-area spread (source+spec+demo+tests). Prioritize it alongside Wave 2.
4. **Cross-component lanes** (dark-mode, SCSS dedup, `#fff` sweep, Material stub) are outside this workspace but affect the overall Gantt delivery score. Flag to orchestrator for scheduling.

---

## Verification

- **Inventory refresh input:** `gantt-gap-inventory-refresh.md` — 1 CLOSED, 25 OPEN
- **Gap count check:** 25 OPEN = 8 (L1) + 5 (L2) + 1 (L3) + 3 (L4 partial) + 2 (L5) + 1 (L6 skipped) + 1 (L7 skipped) + 3 (L8) = 24 in lanes + INT-19 (skipped from L4) = 25 total. Matches.
- **Phase ordering:** L0 dependency removed. No circular dependencies in remaining phases.
- **Priority ordering:** P1-CRITICAL (Wave 2) > P1 (Wave 2) > P2 (Waves 2-3) > P3 (Wave 4). Correct.
- **No first-cycle artifact mutated.** This is a new file.
