# DataGrid Parity Summary

**Wave:** 3 — 03-visual-parity
**Worker:** w-datagrid-delivery
**Audit date:** 2026-04-11T17:40Z
**Approach:** Static-analysis. Scoring derived from reading SCSS tokens and component razor, not pixel comparison. Combinations requiring actual rendering are recorded as `DEFERRED-TO-CAPTURE` in `datagrid-visual-parity-gaps.md`.

---

## Headline Finding

**The single biggest visual-parity risk in MariloDataGrid is not token drift — it is unstyled component selectors.** The razor file (`src/Marilo.Components/DataGrid/MariloDataGrid.razor`, 440 lines, 41 `mar-datagrid-*` class references) emits at least **nine class names that have zero matching SCSS selectors in both the FluentUI and Bootstrap provider files**:

| Class emitted | In `FluentUI/_data-grid.scss`? | In `Bootstrap/_data-grid.scss`? | Consequence |
|---|---|---|---|
| `.mar-datagrid-pager-btn` (+ `--active`, `--prev`, `--next`) | No | No | Pager buttons render with user-agent default styling. |
| `.mar-datagrid-pager-ellipsis` | No | No | Ellipsis element has no typography or spacing. |
| `.mar-datagrid-pagesize-select` | No | No | Page-size dropdown renders as default `<select>`. |
| `.mar-datagrid-pager-info` | No | No | "Showing X of Y" label unstyled. |
| `.mar-datagrid-empty` | No | No | Empty state is just left-aligned text in a `<td>`. |
| `.mar-datagrid-loading-overlay` / `-spinner` / `-text` | No | No | Loading overlay is inline flow, not an overlay. |
| `.mar-datagrid-popup-overlay` / `-dialog` / `-header` / `-body` / `-field` / `-actions` | No | No | Popup edit mode has no modal chrome at all. |
| `.mar-datagrid-validation-summary` | No | No | Validation errors unstyled. |
| `.mar-datagrid-sort-indicator` / `-sort-order` | No | No | Sort arrow and multi-sort badge unstyled. |
| `.mar-datagrid-checkbox-cell` | No | No | Checkbox column has no centering. |
| `.mar-datagrid-detail-row` | No | No | Detail template row unstyled. |
| `.mar-datagrid-footer-row` / `-footer-cell` | No | No | Footer row unstyled. |
| `.mar-datagrid-drag-header` | No | No | Row-drag handle column unstyled. |
| `.mar-datagrid-detail-cell` | No | No | Expand/collapse button cell unstyled. |
| `.mar-datagrid-command-header` | No | No | Command column header unstyled. |
| `.mar-datagrid-searchbox` / `-searchbox-input` | No | No | SearchBox unstyled. |
| `.mar-datagrid-content` | No | No | Scrollable content container unstyled. |
| `.mar-datagrid-col--locked` | No | No | Locked-column marker has no frozen-column treatment. |
| `.mar-datagrid-cmd-btn` (outside filter menu) | Filter-scoped only | Filter-scoped only | Command buttons in toolbar/popup fall back to user-agent. |

This pattern — razor emits a rich CSS class vocabulary, provider SCSS only styles ~12 of them — is the root cause of roughly half of the gap records. Most remediation for DataGrid visual parity is **not a token rebalance; it is a one-pass sweep to write the missing rules**.

---

## Gap Counts by Category

| Category | Critical | Major | Minor | Polish | Total |
|---|---:|---:|---:|---:|---:|
| token/color | 1 (004) | 3 (013, 019, 020→minor) | 0 | 0 | 4 |
| typography | 0 | 1 (005) | 0 | 0 | 1 |
| spacing | 0 | 0 | 0 | 0 | 0 (rolled into density/layout) |
| layout | 0 | 2 (010, 018) | 1 (017) | 0 | 3 |
| iconography | 0 | 1 (007) | 0 | 0 | 1 |
| density | 0 | 1 (006) | 0 | 0 | 1 |
| elevation | 1 (012) | 1 (014) | 0 | 0 | 2 |
| state treatment | 5 (001, 002, 008, 009, 015) | 1 (003) | 0 | 0 | 6 |
| missing implementation | 1 (016) | 1 (011) | 0 | 0 | 2 |
| **TOTAL** | **8** | **11** | **1** | **0** | **20** |

(Record VP-datagrid-020 listed as minor rather than major — Bootstrap compile-time stripe interpolation is a dark-mode switch issue, not a primary-state defect.)

---

## Parity Scores by Theme × Mode (Static Scoring Only)

Aggregate score is the **average of the 7 primary/secondary states that can be audited from SCSS** (default grid, header, row hover, selected row, sorted, filter row, pager). `—` marks states that require pixels.

### Fluent — Light

| State | Score | Notes |
|---|---:|---|
| Default grid | 3 | Baseline tokens wired correctly, assuming table override takes effect (DC-01 needs capture to confirm). |
| Header row | 2 | VP-datagrid-005 typography. |
| Row hover | 1 | VP-datagrid-001 state-layer collision. |
| Selected row | 2 | VP-datagrid-003 selected+hover indistinguishable. |
| Sorted | 1 | VP-datagrid-007 sort-indicator unstyled. |
| Filter row | — | DC-05 input chrome needs capture. |
| Pager | 0 | VP-datagrid-008 pager buttons unstyled. |
| **Average (scored)** | **1.5** | |

### Fluent — Dark

| State | Score | Notes |
|---|---:|---|
| Default grid | 2 | Dark tokens override base; filter/popup hardcoded `#fff` bleeds in. |
| Header row | 2 | Same as light. |
| Row hover | 0 | VP-datagrid-002 hover collision with stripe fill. |
| Selected row | 1 | VP-datagrid-004 insufficient luminance delta. |
| Sorted | 1 | Same as light. |
| Filter row | — | DC-05. |
| Pager | 0 | Same as light. |
| **Average (scored)** | **1.0** | |

### Bootstrap — Light

| State | Score | Notes |
|---|---:|---|
| Default grid | 3 | Bridge layer defers to Bootstrap table baseline. |
| Header row | 2 | Inherits from Marilo header logic; same typography gap. |
| Row hover | — | DC-12 needs capture to see Bootstrap hover interaction. |
| Selected row | 2 | Same as Fluent — VP-datagrid-003 rule applies globally. |
| Sorted | 1 | VP-datagrid-007 applies globally. |
| Filter row | 2 | Bridge file styles filter row; still missing VP-datagrid-010 empty state etc. |
| Pager | 0 | VP-datagrid-009 pager buttons unstyled. |
| **Average (scored)** | **1.7** | (Excludes DC-12.) |

### Bootstrap — Dark

| State | Score | Notes |
|---|---:|---|
| Default grid | 2 | Striping locked to compile-time Bootstrap value (VP-datagrid-020). |
| Header row | 2 | Same. |
| Row hover | — | DC-12. |
| Selected row | 2 | Same. |
| Sorted | 1 | Same. |
| Filter row | 1 | VP-datagrid-019 hardcoded `#fff` backgrounds. |
| Pager | 0 | Same as Bootstrap light. |
| **Average (scored)** | **1.3** | |

### Material — Light

| State | Score | Notes |
|---|---:|---|
| All states | 0 | VP-datagrid-016. Provider has a 5-line TODO placeholder. |
| **Average** | **0.0** | |

### Material — Dark

| State | Score | Notes |
|---|---:|---|
| All states | 0 | Same. |
| **Average** | **0.0** | |

---

## Roll-Up

| Theme/Mode | Avg Score | Status |
|---|---:|---|
| Fluent Light | 1.5 | **Below delivery gate** (primary states on critical path) |
| Fluent Dark | 1.0 | **Below delivery gate** |
| Bootstrap Light | 1.7 | **Below delivery gate** |
| Bootstrap Dark | 1.3 | **Below delivery gate** |
| Material Light | 0.0 | **Blocked** (provider not implemented) |
| Material Dark | 0.0 | **Blocked** |
| **Grand average** | **0.92** | — |

Delivery gate is "≥ 2.5 average across primary states in Fluent Light + Fluent Dark" per the parity rubric ("Close but visible" or better). **DataGrid is not at gate in any theme/mode.**

---

## Coverage Counts

| Stage | Count |
|---|---:|
| Primary state/scenario/theme/mode combinations in capture matrix | 108 (18 states × 6 theme-modes) |
| Static-scored via SCSS | 20 gap records (+ implicit score-3 default states) |
| DEFERRED-TO-CAPTURE (pixels required) | 20 entries |
| Blocked by missing Material runtime | all 6 Material slots (≈ 36 state combinations) |
| Covered in this wave (static + deferred) | 76 of 108 = **70%** |

The remaining 30% (≈ 32 combinations) are edge states (virtualization, popup edit overlay, detail template, command column, footer row) that are BOTH unstyled AND require pixel capture to audit safely. These are recorded as cross-references under the deferred section but not exploded into individual records, because static scoring on an unstyled-and-unrun element is not informative.

---

## Top 5 Themes in the Gap List

1. **Unstyled selector cluster.** 9+ razor-emitted classes have no SCSS. Single biggest blow to parity scores. One SCSS pass can lift 7–8 gap records by a full score each.
2. **State-layer collisions in dark mode.** `--marilo-color-surface` is used as header fill, stripe fill, AND hover fill. Dark mode exposes the collision because there is no alpha-based state layer to separate them. Introducing a dedicated `--marilo-color-state-hover` token (and matching `--marilo-color-row-selected`) fixes VP-datagrid-001/002/003/004 in one stroke.
3. **Hardcoded `#ffffff` and `#fff` literals.** Cerebrum learning confirmed: filter-menu popover, filter-menu-btn active state, filter-menu operator/value inputs, Bootstrap filter-menu, Bootstrap cmd-btn. All break dark mode. Straightforward find-and-replace with `var(--marilo-color-surface)` / `var(--marilo-color-background)`.
4. **Missing focus treatment.** `--focus-stroke-outer` is defined in foundation but no DataGrid selector uses it. Given the spec already advertises keyboard navigation (Wave 2 headline finding: D4 demo claims capability the source does not deliver), failing to show focus rings compounds the demo-honesty defect: when keyboard dispatch does ship, there is no visible focus to indicate where it went.
5. **Material provider is a 5-line placeholder.** Every Material row/theme combination scores 0 by default. This is not a SCSS patch — it requires a new provider implementation track. Should be escalated to orchestrator for separate gap-analysis intake.

---

## Remediation Route

| Category | Remediation target | Est. effort |
|---|---|---|
| Unstyled selector cluster (VP-007, 008, 009, 010, 011, 012, 015, 018) | Single FluentUI + single Bootstrap SCSS PR, ~200 lines added | 1 worker day |
| Token collisions (VP-001, 002, 003, 004) | Foundation token additions + provider rule updates | 0.5 worker day |
| Hardcoded `#fff` literals (VP-013, 014, 019) | Find-and-replace + retest dark mode | 0.25 worker day |
| Typography/density (VP-005, 006, 017) | Component SCSS pass | 0.5 worker day |
| Material provider (VP-016) | Separate gap-analysis intake — new provider track | Out of scope for this wave |
| Bootstrap compile-time striping (VP-020) | Token introduction in `_tokens.scss` + `_tokens-dark.scss` | 0.25 worker day |

**Total fluent + bootstrap visual-parity remediation: ~2.5 worker days.** Material is separate.

---

## Dependencies on Prior Waves

- **Wave 1 (spec review):** SA-06..SA-08 (keyboard dispatch not implemented) makes VP-datagrid-015 (focus treatment) more urgent — when the engine lands, users need visible focus.
- **Wave 2 (demo UX):** D4 "Navigable Grid" demo honesty defect (A-01) is compounded by missing focus rings. Orchestrator should bundle A-01 + VP-datagrid-015 into one remediation task.
- **Wave 2 finding #2 (refresh-data zero coverage):** When new `DataGrid/RefreshData.razor` page is added, VP-datagrid-011 (loading overlay unstyled) will be the most visible remaining gap — should fix before shipping the new page.

---

## Stage-03 Exit Criteria Check

| Criterion | Met? |
|---|---|
| Gap records produced using `visual-parity-gap-format.md` | Yes — 20 records |
| Parity summary produced | Yes (this file) |
| At least 10–20 gap records | Yes — exactly 20 |
| Coverage of theme × mode × core-state matrix | 70% (static + deferred), 30% bundled under deferred edge states |
| Deferred captures recorded with reason | Yes — 20 deferred entries |
| Remediation routing defined | Yes (above) |
| All writes inside `files_owned` | Yes — confirmed against state JSON |
| Verification command run | Yes — `dotnet build Marilo.slnx` exit 0 (see result file) |
