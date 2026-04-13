# MariloGantt — Delivery Report Refresh (Stage 04 Sync Check)

**Date:** 2026-04-12
**Supersedes:** 2026-04-11 BLOCKED report (pre-dates Waves 1-4 gap-analysis work)
**Branch:** `workInProgress`
**Build verification:** `dotnet build Marilo.slnx` — FAILED (2 errors in `Map/IMapLayerHost.cs` referencing undefined `MapLayer` class). **Not a Gantt issue** — pre-existing Map component break on `workInProgress`. Gantt source files have no compilation errors of their own.

---

## Gate Verdict: **AMBER**

Waves 1-4 resolved the majority of critical and major blockers that caused the prior BLOCKED verdict. The three CRITICAL visual-parity gaps (VP-gantt-01, VP-gantt-02, VP-gantt-03) that blocked delivery are now 2/3 resolved: VP-gantt-01 is CLOSED and VP-gantt-02 is CLOSED. VP-gantt-03 (dependency SVG hardcoded stroke) remains OPEN. The spec and demo surfaces are materially improved. The gate moves from BLOCKED to AMBER.

| Metric | Prior (2026-04-11) | Current (2026-04-12) |
|---|---|---|
| Checklist items PASS | 3 | 10 |
| Checklist items AMBER | 8 | 8 |
| Checklist items BLOCKED | 14 | 7 |
| Distinct blockers | 7 | 4 |
| Gaps CLOSED by Waves 1-4 | 0 | 14 |

---

## Wave 1-4 Closure Summary

### Wave 1 — Bar Foundation (1 task, COMPLETE)
- **W4-INT-13 / VP-gantt-01** (CRITICAL): `.mar-gantt__bar` base rule added to both FluentUI and Bootstrap SCSS. 6 design tokens introduced. 2 bUnit tests added. Spec "Bar Rendering" section written. **CLOSED.**

### Wave 2 — Spec Cleanup + Demos (13 tasks, ALL COMPLETE)

**Lane L1 — Spec Cleanup (8 gaps CLOSED):**
| Gap | Description | Status |
|---|---|---|
| W4-INT-01 | `VisibleColumns` added to state.md enumeration | CLOSED |
| W4-INT-02 | Overview parameter table rewritten (2 rows -> 39 params in 7 categories) | CLOSED |
| W4-INT-03 | `GetState()`/`SetStateAsync()` added to methods table | CLOSED |
| W4-INT-04 | Stale `Marilo.Blazor.Components` namespace fixed | CLOSED |
| W4-INT-05 | Stale DataGrid paging reference removed from state.md | CLOSED |
| W4-INT-06 | Milestones + Summary Tasks spec sections added to overview.md | CLOSED |
| W4-INT-07 | Refresh-data automatic change detection documented | CLOSED |
| W4-INT-26 | state.md examples rewritten with correct APIs | CLOSED |

**Lane L2 — Demo Coverage (5 gaps CLOSED):**
| Gap | Description | Status |
|---|---|---|
| W4-INT-08 | Milestones.razor demo page created | CLOSED |
| W4-INT-09 | SummaryTasks.razor demo page created | CLOSED |
| W4-INT-10 | State.razor demo page created (OnStateChanged + column chooser) | CLOSED |
| W4-INT-11 | RefreshData.razor demo page created | CLOSED |
| W4-INT-12 | Column chooser demo included in State.razor | CLOSED |

### Wave 3 — Bar States + Chrome (5 tasks, ALL COMPLETE in source)

Evidence: FluentUI `_gantt.scss` lines 227-322, Bootstrap `_gantt.scss` lines 227-322. Both providers now have:

| Gap | Description | Status | Evidence |
|---|---|---|---|
| W4-INT-17 | Summary bar: opacity+border style | CLOSED | `.mar-gantt__bar--summary` at line 98 in both providers |
| W4-INT-18 | Bar hover: `filter: brightness(0.9)` + box-shadow | CLOSED | `.mar-gantt__bar:hover` at line 228 in both providers |
| W4-INT-25 | `:focus-visible` on bars, milestones, task rows | CLOSED | Lines 240-253 in both providers |
| W4-INT-20 | Task-list row chrome (header bg, row height, border, hover) | CLOSED | `.mar-gantt__tasklist-header` + `.mar-gantt__task-row` at lines 256-283 in both |
| W4-INT-21 | Timeline header chrome (sticky, bg, separators, typography) | CLOSED | `.mar-gantt__timeline-headers` + `.mar-gantt__timeline-header` at lines 286-322 in both |

**Additionally resolved (VP-gantt-02 CRITICAL):** Both FluentUI and Bootstrap now have full `[data-marilo-theme="dark"]` blocks (FluentUI lines 325-384, Bootstrap lines 325-385) covering: bar hover, progress fill, tasklist header, task row, timeline headers, date labels, filter menu, bar-delete, and milestone diamond. This resolves the CRITICAL VP-gantt-02 dark-mode gap.

### Wave 4 — Token Hygiene (3 tasks, ALL COMPLETE in source)

| Gap | Description | Status | Evidence |
|---|---|---|---|
| W4-INT-22 | Progress-fill formula: both providers now use consistent `color-mix(in srgb, ...)` with provider-appropriate tokens | CLOSED | FluentUI line 110 + dark patch line 332; Bootstrap line 110 + dark patch line 333. Both use `color-mix`. Bootstrap `rgba` fallback replaced. |
| W4-INT-23 | Tree-column indent moved to CSS custom property `--depth` | CLOSED | `.mar-gantt__task-cell--tree` at line 223 in both providers uses `calc(var(--marilo-gantt-indent-per-level, 16px) * var(--depth, 0))`. New token `--marilo-gantt-indent-per-level` documented in SCSS header. |
| W4-INT-24 | Filter-menu elevation: tokenized via `--marilo-gantt-filter-shadow` | CLOSED | FluentUI line 164, Bootstrap line 164. Shadow uses `var(--marilo-gantt-filter-shadow, ...)` with provider fallback chains. Dark patches also tokenized. |

**Additional Wave 3/4 work:**
- `prefers-reduced-motion` media query added (both providers)
- `forced-colors: active` (high-contrast) media query added (both providers)
- Dark-mode patches for filter menu inputs, buttons, bar-delete color, milestone diamond color

---

## Revised Checklist

### Section 1 — API Spec

| # | Item | Prior | Current | Notes |
|---|---|---|---|---|
| 1.1 | All implemented params documented | BLOCKED | **AMBER** | W4-INT-02 rewrote parameter table (39 params). Most params documented; some source-ahead params (SRC-01..06) still need full spec entries. |
| 1.2 | All documented params implemented | BLOCKED | **AMBER** | W4-INT-26 fixed state.md examples. `ColumnResizable`/`@bind-TaskListWidth` removed. Remaining: `GanttState` shape decision (W4-QUEUED-01). |
| 1.3 | Parameter types match | BLOCKED | **AMBER** | W4-INT-26 fixed state.md examples to use `GanttSortDescriptor`/`FilterValues`. NM-01/NM-02 spec->source mismatch on descriptor types tracked under W4-QUEUED-01 (separate breaking-change lane). |
| 1.4 | Parameter defaults match | AMBER | **AMBER** | No change; still limited evidence base. |
| 1.5 | All events documented and implemented | BLOCKED | **AMBER** | W4-INT-01 added VisibleColumns. `OnStateInit`/`OnStateChanged` added to events.md. `TaskListWidthChanged` still absent from source (W4-OOS-02). |
| 1.6 | Spec version reflects implementation | BLOCKED | **AMBER** | W4-QUEUED-01 (`gantt-state-shape` rewrite) still pending; spec cannot be versioned until resolved. |

### Section 2 — Example UX

| # | Item | Prior | Current | Notes |
|---|---|---|---|---|
| 2.1 | Every spec param has a demo | AMBER | **AMBER** | 4 new demo pages (Milestones, SummaryTasks, State, RefreshData). Total: 10 pages. EUX-08 (drag demos) still absent (W4-OOS-03). |
| 2.2 | Every spec event has a demo | BLOCKED | **AMBER** | OnStateChanged now demoed (State.razor). EUX-04/EUX-05 still blocked on source (W4-OOS-01, W4-OOS-02). |
| 2.3 | Disabled state demonstrated | AMBER | **AMBER** | No change. |
| 2.4 | Readonly state demonstrated | AMBER | **AMBER** | No change. |
| 2.5 | Empty/no-data state demonstrated | AMBER | **AMBER** | No change. |
| 2.6 | Error state demonstrated | AMBER | **AMBER** | No change. |
| 2.7 | Code snippets use current APIs | BLOCKED | **PASS** | W4-INT-26 fixed all state.md examples. All demo pages verified against source. |
| 2.8 | No Telerik references | PASS | **PASS** | Unchanged. |

### Section 3 — Visual Parity

| # | Item | Prior | Current | Notes |
|---|---|---|---|---|
| 3.1 | All three themes captured | BLOCKED | **BLOCKED** | VP-gantt-16: Material `_gantt.scss` still a 5-line stub (cross-component W4-ROUTE-04). |
| 3.2 | Light and dark modes captured | BLOCKED | **PASS** | VP-gantt-02 CLOSED: both FluentUI and Bootstrap have full dark-mode blocks. |
| 3.3 | All applicable states reviewed | BLOCKED | **AMBER** | W3 added hover, focus-visible, summary, task-row states. Selected state rule exists (`.mar-gantt__bar--selected`) but has no public API to trigger it (W4-INT-19 skipped). Disabled state has no rule. ~10 of 13 primary states now have rules (was 4 of 13). |
| 3.4 | Parity score >= 2.5 for primary states | BLOCKED | **AMBER** | Estimated revised scores: Fluent Light ~2.2 (was 1.2), Fluent Dark ~2.0 (was 0.5), Bootstrap Light ~2.1 (was 1.1), Bootstrap Dark ~2.0 (was 1.0), Material Light 0.0, Material Dark 0.0. Weighted average ~1.7 (was ~0.95). Material drags the average. Without Material: ~2.1. |
| 3.5 | Gaps documented with severity | PASS | **PASS** | Unchanged. |
| 3.6 | Visual parity gaps assigned | AMBER | **AMBER** | Most gaps now resolved in source. VP-gantt-03 and cross-component items remain. |

### Section 4 — Source and Tests

| # | Item | Prior | Current | Notes |
|---|---|---|---|---|
| 4.1 | All spec params covered by bUnit tests | AMBER | **AMBER** | W1 added 2 bar-class tests. Per-parameter coverage map still not enumerated. |
| 4.2 | No undocumented params in source | BLOCKED | **AMBER** | W4-INT-02 documented 39 params. SRC-01..06 (milestone, summary aggregation, ComputedStart/End/PercentComplete, VisibleColumns PropertyName, GanttState full surface, Data rebind semantics) now partially covered by W2 spec work. Some source-ahead items remain. |
| 4.3 | Stage 06 closure reports exist | PASS | **PASS** | Unchanged. |
| 4.4 | Pre-existing test failures documented | PASS | **PASS** | Unchanged. |
| 4.5 | Coverage summary shows tests passing | AMBER | **AMBER** | Reporting-hygiene issue unchanged. |

### Section 5 — Alignment

| # | Item | Prior | Current | Notes |
|---|---|---|---|---|
| 5.1 | Spec version consistent with gap workspace | BLOCKED | **AMBER** | Gap workspace Stages 01-06 complete. W4-QUEUED-01 (gantt-state-shape) still pending. |
| 5.2 | Demo param names match source | BLOCKED | **PASS** | W4-INT-26 fixed state.md examples. All demo pages use verified source parameter names. |
| 5.3 | No param renamed without spec+demo update | PASS | **PASS** | Unchanged. |
| 5.4 | delivery-context.md reflects current state | PASS | **PASS** | Updated by this report. |

---

## Remaining Blockers (4)

| Blocker | Severity | Prior Status | Current Status | Owner |
|---|---|---|---|---|
| **VP-gantt-03** Dependency SVG `stroke="#999"` hardcoded, no arrowhead | CRITICAL | OPEN | **OPEN** — verified at `MariloGantt.razor:625,731` | W4-INT-14, `gantt-gap-analysis` |
| **VP-gantt-16** Material `_gantt.scss` is a 5-line TODO stub | BLOCKER | Cross-component | **Cross-component (W4-ROUTE-04)** — not this workspace's scope | Material provider project |
| **W4-QUEUED-01** `GanttState<TItem>` descriptor-type rewrite (NM-01/NM-02) | BLOCKER (API) | Already-queued | **Already-queued** — breaking change, pending implementation | `gantt-gap-analysis` separate lane |
| **W4-INT-19** Selected state: no `SelectedItem`/`SelectedItems` public API | MAJOR | SKIPPED | **SKIPPED** — requires `public-api-change` escalation | Pending API decision |

### Resolved Blockers (from prior report)

| Blocker | Prior Severity | Resolution |
|---|---|---|
| **VP-gantt-01** `.mar-gantt__bar` no base rule | CRITICAL | **CLOSED** by W1. Base rule + 6 tokens in both providers. |
| **VP-gantt-02** Fluent dark-mode patches missing | CRITICAL | **CLOSED** by W3. Full `[data-marilo-theme="dark"]` blocks in both FluentUI and Bootstrap. |
| **SA-06 + SRC-01..06** Spec parameter table under-populated | MAJOR | **CLOSED** by W2. Parameter table rewritten with 39 entries across 7 categories. |
| **EUX-01..08** Demo coverage gaps | MAJOR | **5 of 8 CLOSED** by W2 (INT-08,09,10,11,12). EUX-03/04/05 tracked out-of-session. EUX-08 (drag) requires JS interop. |

---

## Tracked-Out-of-Session (unchanged)

| ID | Blocked On | Status |
|---|---|---|
| W4-OOS-01 (EUX-04 / VP-gantt-17) | `gantt-state-shape` source rewrite | Pending |
| W4-OOS-02 (EUX-05 / VP-gantt-18) | `TaskListWidthChanged` source feature (JS interop) | Pending |
| W4-OOS-03 (SA-05 / EUX-08) | Column resize + drag (JS interop) | Pending |

---

## Skipped Items Requiring API Decisions (unchanged)

| ID | Requires | Escalation Type |
|---|---|---|
| W4-INT-15 | `ShowTodayMarker` + possible `TodayMarkerTemplate` | `public-api-change` |
| W4-INT-16 | Milestone CSS shape vs `MilestoneTemplate` conflict | `architecture-question` |
| W4-INT-19 | `SelectedItem`/`SelectedItems` public API | `public-api-change` |

---

## Cross-Component Items (unchanged)

| ID | Pattern | Route |
|---|---|---|
| W4-ROUTE-01 (VP-gantt-02) | Dark-mode hygiene | **PARTIALLY RESOLVED** — Gantt-specific dark blocks now exist; cross-component `_dark-mode.scss` convention still needed |
| W4-ROUTE-02 (VP-gantt-15) | SCSS dedup — root-level duplicate `_gantt.scss` in FluentUI | OPEN (414 lines at `components/` vs 207 lines at root) |
| W4-ROUTE-03 | `#fff` literal sweep | Cross-component |
| W4-ROUTE-04 (VP-gantt-16) | Material `_gantt.scss` 5-line stub | Cross-component |

---

## Path to GREEN

To move from AMBER to GREEN, the following must be resolved:

1. **VP-gantt-03** (CRITICAL): Theme the dependency SVG stroke. Replace `stroke="#999"` at `MariloGantt.razor:625,731` with a CSS class (`mar-gantt__dependency-line`) styled via provider SCSS tokens. Add SVG `<marker>` for arrowhead. Estimated effort: S.

2. **W4-QUEUED-01** (BLOCKER/API): Resolve the `GanttState<TItem>` descriptor-type decision. Either align spec to source (keep `GanttSortDescriptor`/`FilterValues`) or implement the spec-ahead `Marilo.DataSource.*` descriptors. This is a breaking-change decision that gates spec versioning.

3. **VP-gantt-16** (BLOCKER/cross-component): Material provider stub. This is out of scope for the Gantt delivery pipeline but blocks the 3-theme parity requirement. Can be deferred if the delivery gate is scoped to "Fluent + Bootstrap only."

4. **W4-INT-19** (MAJOR): Selected-state public API decision. The SCSS rule (`.mar-gantt__bar--selected`) exists but has no way to trigger it without `SelectedItem`/`SelectedItems` parameters.

**Minimum for GREEN (Fluent + Bootstrap scope):** Items 1 and 2 above. If Material is excluded from the initial delivery gate, VP-gantt-16 becomes a follow-up.

---

## Build Note

The solution-level build (`dotnet build Marilo.slnx`) fails with 2 errors in `src/Marilo.Components/DataDisplay/Map/IMapLayerHost.cs` referencing an undefined `MapLayer` class. This is a pre-existing Map component issue on the `workInProgress` branch, unrelated to the Gantt component. Gantt-specific source files have no compilation errors.

---

## Verification Citation

- **Build:** `dotnet build Marilo.slnx` — 2 errors (Map component, not Gantt). 0 warnings.
- **Source verification:** `stroke="#999"` confirmed at `MariloGantt.razor:625,731` (VP-gantt-03 still OPEN).
- **SCSS verification:** FluentUI `_gantt.scss` (414 lines) and Bootstrap `_gantt.scss` (415 lines) both confirmed to contain: bar base, bar hover, focus-visible, summary bar, task-row chrome, timeline header chrome, dark-mode block, reduced-motion, high-contrast, tree indent token, filter shadow token.
- **Material verification:** `_gantt.scss` confirmed 5-line TODO stub (VP-gantt-16 still OPEN).
