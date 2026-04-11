# Workspace Status: DataSheet Delivery

**Last updated:** 2026-04-11 (Stage 02 Example UX impl COMPLETE — 02a landed across commits `5abf813`/`6ce6d65`/`1021175`, and **Batch 02b Keyboard-and-Accessibility.razor landed as commit `e48d08f` in Fire #26**; 1160/1160 full suite passing across all four demo pages)

## Pipeline Status

```text
  [01-spec-review]  ------>  [01b verification]  ------>  [02-example-ux]                  ------>  [03-visual-parity]  ------>  [04-sync-check]
    COMPLETE 2026-04-10       COMPLETE 2026-04-10         COMPLETE 2026-04-11                        PENDING                    PENDING
                              (10 sub-tasks audited;      (4 demo pages, 1490 LOC total:
                               23 gaps surfaced +          Overview + Editing-and-Validation
                               V03 large-feature)          + BulkOperations +
                                                           Keyboard-and-Accessibility)
```

## Iteration 15 — Stage 03a Open Questions Resolved (Complete 2026-04-11)

Loop iteration 15. Research-only probe dispatched to answer the 5 open questions from iteration 14's Stage 03 plan. No code changes this iteration.

**Deliverable:** 5 definitive answers that unblock Stage 03a for immediate start.

### Answers

| Q | Topic | Answer | Stage 03a action |
|---|---|---|---|
| Q1 | Formula bar & sheet tabs | **NOT IMPLEMENTED** — belong on MariloSpreadsheet, not DataSheet. Spec explicitly contrasts the two. | **Skip both states** |
| Q2 | Cell range selection (V03) | **NOT IMPLEMENTED** — source has only `_selectedRows` HashSet and scalar active-cell fields; no range state | **Skip state** |
| Q3 | Dark mode switcher | **IMPLEMENTED** — `MainLayout.razor` has `SetDarkMode(true/false)`, `ToggleDarkMode()`, `_isDark` cascading value, persistent theme storage. `ThemePresets.cs` defines light+dark palettes for all 5 theme presets. | **Use built-in toggle** via JS interop / click the toggle button. No `emulateMedia` needed. |
| Q4 | Frozen columns/rows | **NOT IMPLEMENTED** — no `frozen`/`sticky`/`locked` references in DataSheet source. Frozen columns are a DataGrid feature (JS Interop Batch 2), not shared with DataSheet. Default `<thead>` sticky behavior is part of the standard grid render. | **Skip dedicated state**; the standard sticky-thead behavior is naturally captured as part of "grid default" |
| Q5 | Playwright narrow viewport | **UN-EXERCISED** — `playwright.config.ts` has narrow project commented out; zero narrow baselines across all CDW baselines; no spec references it. | **Stay desktop-only**; defer narrow to Stage 04+ |

### Stage 03a Status: ✅ READY TO PROCEED

**Reduced capture count:** 18 primary screenshots (confirmed; down from 54-state maximum)

- 6 theme/mode combinations (Fluent/Bootstrap/Material × Light/Dark)
- 3 primary states per combination (cell grid default, selected cell, cell editing)
- 1 viewport (Desktop 1280×900)

**Blocking conditions — ALL RESOLVED:**

1. ✅ Formula bar / sheet tabs → skip (not implemented)
2. ✅ Cell range selection → skip (V03 deferred)
3. ✅ Dark mode → use built-in toggle (implemented)
4. ✅ Frozen columns → skip (not implemented; `<thead>` sticky captured as part of default)
5. ✅ Narrow viewport → skip (deferred to Stage 04+)

**Next iteration should:** Extend `tests/visual-parity/specs/datasheet.spec.ts` (or create it if absent) to iterate 6 theme/mode combinations, navigate the Marilo.Demo DataSheet pages, and capture the 18 primary-state baselines. Use the existing Playwright infrastructure (`tests/visual-parity/playwright.config.ts`, port 5301, `reuseExistingServer: true`).

**Key evidence file paths for the next implementer:**

- `samples/Marilo.Demo/Layout/MainLayout.razor` — dark mode toggle implementation
- `samples/Marilo.Demo/Data/ThemePresets.cs` — 5 themes × 2 modes
- `samples/Marilo.Demo/Services/ProviderSwitcher.cs` — provider switching API
- `tests/visual-parity/playwright.config.ts` — existing infrastructure
- `tests/visual-parity/specs/datagrid.spec.ts` — closest sibling spec to mirror
- `ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/datasheet-visual-parity-plan-2026-04-11.md` — iteration 14 plan

## Iteration 14 — Stage 03 Visual Parity Plan (Complete 2026-04-11)

Loop iteration 14. Audit-only deliverable — no code changes this iteration.

**Deliverable:** `stages/03-visual-parity/output/datasheet-visual-parity-plan-2026-04-11.md` — comprehensive Stage 03 plan sourced from reading the Stage 03 CONTEXT.md, capture-matrix.md, parity-score-rubric.md, and the four DataSheet demo pages.

### Key findings

- **Playwright infrastructure EXISTS** — `tests/visual-parity/playwright.config.ts` is fully configured with Chromium, Desktop 1280×900 viewport, animations disabled, auto-start webServer on port 5301 (`dotnet run --project samples/Marilo.Demo`), and `reuseExistingServer: true`. Baselines stored in `./baselines/` alongside specs. Stage 03 does NOT need to build new infrastructure — it can extend the existing framework.
- **All 3 CSS providers implement the 7 DataSheet methods** — FluentUI, Bootstrap, Material all have `DataSheetClass`, `DataSheetCellClass`, `DataSheetHeaderCellClass`, `DataSheetRowClass`, `DataSheetToolbarClass`, `DataSheetBulkBarClass`, `DataSheetSaveFooterClass`. No provider gaps.
- **Capture matrix is defined** — 6 theme/modes × 9 states = 54 screenshots max, or ~36 if edge states (formula bar, sheet tabs) are skipped as not-yet-implemented.
- **Rubric is defined** — 0-3 parity score with critical/major/minor/polish severity mapping, plus mismatch classification (token/component/demo/missing).
- **Gap format is defined** — structured `VP-datasheet-[N]` records with theme, mode, state, score, severity, category, recommended change, acceptance criteria, remediation target.

### Recommended sequencing

- **Stage 03a** — Playwright spec + primary state captures (18 screenshots across 6 theme/modes × 3 primary states: cell grid default, selected cell, editing). One loop iteration.
- **Stage 03b** — Secondary state captures (18 screenshots across 6 theme/modes × 3 secondary states: headers, frozen rows/columns, cell range selection). One loop iteration. Frozen/range states conditional on implementation status.
- **Stage 03c** — Scoring, gap classification, parity summary. Pure analysis. One loop iteration.
- **Stage 03d** — Gap remediation handoff to `datasheet-gap-analysis` workspace (if any score < 3 gaps surface). One loop iteration.

**Estimated total:** ~4 iterations to complete Stage 03.

### Open questions documented in plan

5 open questions identified (formula bar implementation status, range selection capture status, dark mode switcher existence, frozen column implementation status, narrow viewport scope). All have recommendations — only the "formula bar / sheet tabs implementation status" and "frozen column implementation status" are true blockers for Stage 03a start. Plan includes recommendations to skip unimplemented states.

### Stage 03 readiness

**✅ READY TO PROCEED.** All prerequisites (Playwright infra, CSS providers, demo pages, rubric, gap format) are in place. Next iteration can start Stage 03a directly by extending the Playwright test suite.

## Iteration 13 — ComponentRegistry + eab6997 ResetAsync doc note (Complete 2026-04-11)

Loop iteration 13. Small, focused mop-up commit addressing two pending items:

1. **ComponentRegistry DataSheet entry** — previously missing; unblocks breadcrumb/header rendering for all 4 DataSheet demo pages
2. **`eab6997` ResetAsync edge case** — iteration 11's flagged concern, addressed via XML documentation

**Commit:** `ca71e0a` — "fix(datasheet): ComponentRegistry entry + eab6997 ResetAsync doc note"

**Files changed (2):**

- `samples/Marilo.Demo/Data/ComponentRegistry.cs` — new `DataSheet` entry in the "Data Display" category between `DataGrid` and `TreeList`; new `DataSheetSubPages` array listing the 4 actual demo pages. Slug `"DataSheet"` exact-case matches the folder. ApiPath reuses `ApiNs("DataGrid")` since `MariloDataSheet<TItem>` lives in the same namespace.
- `src/Marilo.Components/DataGrid/MariloDataSheet.Data.cs` — new XML `<summary>` + `<remarks>` on `RestoreEntryOrRemoveNewRow` documenting the helper's two normal cases and the `IsNewlyAdded && IsDeleted` edge case. Inline NOTE at the `ResetAsync` loop guard points readers to the helper remarks.

### Important correction: iteration 11 finding was inaccurate

During implementation, the subagent discovered that iteration 11's code-quality review finding was factually incorrect. The stated concern was:

> `ResetAsync` now routes `IsNewlyAdded && IsDeleted` entries through `RestoreEntryOrRemoveNewRow` because the loop guard was changed from `IsNewlyAdded || IsDeleted` to `IsDeleted`.

Actual state: With the current guard `if (entry.IsDeleted) continue;`, a `new+deleted` entry hits the `continue` (because `IsDeleted=true`) and **never reaches the helper**. No behavioral change via `ResetAsync`.

The **real** asymmetry lives in `BulkResetAsync`, which does NOT guard on `IsDeleted` and passes every selected row straight through the helper. So a `new+deleted` entry reset via `BulkResetAsync` IS removed from `_displayRows`.

The XML `<remarks>` documents both call sites honestly, explaining the asymmetry is intentional (`BulkResetAsync` targets an explicit user selection, so removing the row entirely matches user intent).

### Reviews

- **Spec compliance:** ✅ APPROVED. ComponentRegistry entry matches sibling pattern exactly. XML doc matches spec including the iteration 11 correction. Zero behavioral changes.
- **Code quality:** ⚠️ APPROVED WITH NITS. Zero blockers. Three minor items:
  - `DataSheetSubPages` array is currently unconsumed (same status as sibling sub-page arrays). A `// Consumed by ComponentDemoLayout once sub-page routing lands` header comment would prevent future dead-code deletion.
  - Inline NOTE at ResetAsync loop guard partially duplicates XML remarks. Could collapse to 2 lines pointing to the remarks.
  - A pre-existing 3-line comment above the new `<summary>` partially overlaps with it. Low-cost cleanup opportunity but not introduced by this commit.
- **Full suite:** 1160/1160 unchanged (docs-only changes on the source side).

## Iteration 12 — e65b081 Review (Complete 2026-04-11)

One new parallel-process commit landed since iteration 11. Iteration 12 ran a combined spec + code-quality catch-up review.

| Commit | Scope | Spec | Code Quality |
|---|---|---|---|
| `e65b081` | Keyboard-and-Accessibility polish follow-up (removes 3 unused `@ref` fields, adds `.ks-*` scoping comment) | ✅ APPROVED | ✅ APPROVED |

**Details:** Net `-3 LOC`. Scoped to `Keyboard-and-Accessibility.razor` only. Removes unused `@ref` fields (dead code from the initial draft) while retaining `_cmdSheet` with a commit-message justification (symmetry + future extension). Adds a scoping comment above the `<style>` block explaining the `.ks-*` prefix avoids collisions with `.ds-*` on sibling demo pages.

**Deferred nits carried forward** (not addressed in e65b081):

- The 374-LOC single-file size from iteration 11 remains (not a regression).
- Scenario A's passive `onkeydown` side-panel listener verification (mentioned in iteration 11 review) is still pending.
- One minor self-documentation opportunity: `_cmdSheet` retention rationale lives only in git history, not at the code site. An inline `// TODO` or comment would make the asymmetry self-documenting.

## Iteration 11 — Review Sweep (Complete 2026-04-11)

Four parallel-process commits that landed between iterations 8 and 11 were catch-up reviewed in iteration 11 to comply with the `superpowers:subagent-driven-development` skill's two-stage review rule.

| Commit | Scope | Spec | Code Quality |
|---|---|---|---|
| `e48d08f` | Stage 02b Keyboard-and-Accessibility demo | ✅ APPROVED | ✅ APPROVED |
| `1021175` | BulkOperations.razor spec-alignment (Required flag, field rename, heights) | ✅ APPROVED | ✅ APPROVED |
| `eab6997` | F4+F2 cosmetic refactor (`RestoreEntryOrRemoveNewRow` helper, switch-expression restore, `GetColumnClrType` centralization, Step 6 LINQ) | ✅ APPROVED | ⚠️ APPROVED WITH NITS (1 edge-case concern) |
| `d693573` | BulkResetAsync re-entrancy guard + 2 regression tests | ✅ APPROVED | ✅ APPROVED |

**Edge-case concern on `eab6997`:** The `ResetAsync` refactor's loop guard changed from `IsNewlyAdded || IsDeleted` to `IsDeleted` only. An entry with both flags true is now routed through `RestoreEntryOrRemoveNewRow` (which sees `IsNewlyAdded` first and removes from `_displayRows`) instead of being skipped. This is a behavioral change in a "cosmetic" commit — arguably more correct (reset should remove new rows even if also deleted), but the combined state is unusual. **Severity: low.** Either restore the original guard or add an XML doc note to `RestoreEntryOrRemoveNewRow`. Logged as a future polish follow-up.

**Iteration 11 nit collection (all non-blocking):**

- `ParseNumberForCell` / `TryParseNumberCell` naming pair could use one-line XML summaries.
- `GetColumnClrType` `typeof(decimal)` fallback could use a stronger comment.
- Tests reaching into `_savedStateDurationMs`/`_selectedRows` could note the `[InternalsVisibleTo]` dependency.
- Regression-test comments reference internal IDs (V02.2/V05.1) without plain-English explainers.
- `BulkOperations.razor` camelCase filename preserved intentionally for concurrent-loop stability — rename to `Bulk-Operations.razor` acceptable in a future cleanup.

## Stage 02b — Keyboard-and-Accessibility.razor (Complete 2026-04-11)

Loop iteration Fire #26. Commit `e48d08f` on `workInProgress`. Closes the final Priority-1 scenario block from the Stage 02 audit.

**Deliverable:** `samples/Marilo.Demo/Pages/Components/DataSheet/Keyboard-and-Accessibility.razor` (NEW, 374 lines). Five scenarios:

- **Scenario A — Keyboard navigation:** 3-row grid with Text/Number/Date columns wrapped in `<div @onkeydown="...">` capture. "Last key observed" display logs Tab/Shift+Tab/Arrow/Enter passively; real navigation handled by MariloDataSheet internals.
- **Scenario B — Edit mode shortcuts:** Same grid style with a key-log filtered to F2/Enter/Escape presses only, matching the pedagogical focus of the scenario.
- **Scenario C — Command keys:** 4-row grid with `AllowBulkPaste="true"` and `OnSaveAll` wired to an event log. Instruction panel explains Ctrl+S/Ctrl+C/Ctrl+V/Ctrl+D/Ctrl+Z. No programmatic interception — MariloDataSheet handles the combos; demo observes via `OnSaveAll`.
- **Scenario D — Space toggles checkbox:** 3-row grid with 1 Text + 1 Checkbox column. No event log (single-gesture scenario per spec).
- **Scenario E — ARIA attributes inspection:** `<AccessibilityInfo>` block populated with 12 keyboard rows, 10 ARIA attribute rows (role=grid/row/gridcell, aria-rowindex, aria-colindex, aria-readonly, aria-invalid, aria-busy, aria-live, aria-describedby), and 3 screen-reader notes.

**Files changed (commit `e48d08f`):** 1 new file only. No component source touched, no tests, no other demo pages, no ComponentRegistry.cs.
**Build:** 0 warnings, 0 errors.
**Tests:** 1160/1160 (unchanged; no test files touched).

**Scope notes:** CSS class prefix is `.ks-*` (keyboard-scenarios) instead of `.ds-*` to allow cohabitation with sibling demo pages without style bleed. onkeydown wrapper uses `tabindex="-1"` + `outline: none` so the wrapper captures bubbling key events without stealing focus or showing a focus ring.

## Stage 02a — Editing-and-Validation.razor + Overview.razor fix (Complete 2026-04-11)

Loop iteration 8 follow-up. Commit `6ef8d89` on `workInProgress`. Absorbed Fire #25's rename (`BulkOperations.razor` → `Bulk-Operations.razor`) plus small spec-matching edits (TSV years `2025`, `Required="true"` on Quantity, `OrderDate` field/title, virtualization heights `400px`), AND added a **brand-new Editing-and-Validation.razor demo page (~465 lines)**, plus a concurrent **ResetAsync-in-OnSaveAll footgun fix**.

**Deliverable (1/2) — `samples/Marilo.Demo/Pages/Components/DataSheet/Editing-and-Validation.razor`** (NEW, ~465 lines). Five Priority-1 scenarios:

- **Scenario A — Required field validation:** `RequiredRow` grid with Name + Amount both `Required="true"`; `OnValidate` wires "Name is required" error via `DataSheetValidationError<RequiredRow>`; event log reports save-blocked count.
- **Scenario B — Column-level validators:** `ColumnRow` grid with `Validate=(r => r.Amount < 0 ? "Must be >= 0" : null)` and a Date range rule (`Year != 2025 → "Must be in 2025"`); 4-row dataset.
- **Scenario C — Cross-row OnValidate:** `CrossRow` grid with 5 SKUs; `OnValidate` enforces `sum(Quantity across DirtyRows) ≤ 1000` and populates `args.Errors` with one error per offending row, each quoting the total and the row's contribution.
- **Scenario D — IsLoading skeleton state:** `LoadingRow` grid with Load Data / Clear Data buttons; `_loadingInFlight` bound to `IsLoading`; `await Task.Delay(1500)` simulates async fetch; `EmptyStateMessage` covers the cleared state.
- **Scenario E — Reset / Discard all changes:** `ResetRow` grid with 4 seeded rows; "Discard All Changes" button reads `GetDirtyRows().Count` then calls `_resetSheet.ResetAsync()` (public API); event log records the discard count. Deliberately uses `ResetAsync` (not `BulkResetAsync`, which is internal).

**Deliverable (2/2) — `samples/Marilo.Demo/Pages/Components/DataSheet/Bulk-Operations.razor`** (RENAMED from `BulkOperations.razor`, same 5 scenarios as the prior 02a partial, with small spec-matching edits applied during Fire #25 review).

**Deliverable (3/3) — ResetAsync-in-OnSaveAll footgun fix:**

- `Overview.razor` — removed the `await _grid!.ResetAsync()` call from `HandleSaveAll` (lines 115-128) and added an inline comment explaining why the consumer must not clean up here.
- `src/Marilo.Components/DataGrid/MariloDataSheet.Data.cs` — added an XML doc `<remarks>` warning on `SaveAllAsync` and an inline comment next to the `OnSaveAll.InvokeAsync` call so both reviewers and maintainers see the contract.
- `tests/Marilo.Tests.Unit/DataGrid/MariloDataSheetTests.cs` — new test `SaveAll_WithDeletedRows_RemovesThemFromDisplayRowsAfterHandler` that locks in the correct post-save cleanup behavior (deleted row gone from `_displayRows`, remaining rows intact, `GetDirtyRows()` empty). Closes the follow-up item from the partial-02a review (#1 and #4 of the "Follow-up owner" list).

**Files changed (commit `6ef8d89`):** 5 files total (1 rename with edits, 1 new demo page, 1 existing demo fix, 1 component source doc tweak, 1 test addition). 558 insertions, 10 deletions.
**Build:** 0 warnings, 0 errors.
**Tests:** **1160/1160** (was 1159; +1 new regression test).

**Reviews:**

- Spec compliance: ✅ APPROVED via independent subagent review. All 10 Priority-1 scenarios present across the two pages. Real tab characters verified in TSV block via hex dump. `ResetAsync` correctly used (public); `BulkResetAsync`/`BulkDeleteAsync`/`AddRowAsync` (all internal) correctly avoided.
- Code quality: ⚠️ APPROVED WITH NITS via independent subagent review. Zero blockers. Nits: verbose Year-range predicate in Scenario B (`d.Year < 2025 || d.Year > 2025` vs `d.Year != 2025`); unused `@ref` fields kept for symmetry with Overview pattern; cross-page duplication of `AddLog` helper and `.ds-demo-*` CSS (acceptable for standalone demo pages).

**Follow-up:** Stage 02b remains: `Keyboard-and-Accessibility.razor` + Overview.razor refactor (scenarios 6 per the audit). Next loop iteration should pick that up.

## Stage 02a — Editing-and-Validation.razor (Complete 2026-04-10)

Loop iteration 10. Landed concurrently via a parallel subagent dispatch — commit `6ce6d65` on `workInProgress`. This iteration ran catch-up reviews.

**Deliverable:** `samples/Marilo.Demo/Pages/Components/DataSheet/Editing-and-Validation.razor` — NEW, 465 lines, 5 Priority-1 scenarios stacked vertically.

**Scenarios implemented:**

- **Scenario A — Required Field Validation.** `RequiredRow` grid (Text + Number, both Required). `HandleRequiredValidate` iterates `args.DirtyRows`, pushes `DataSheetValidationError` entries for empty values, blocks save. Event log records attempts.
- **Scenario B — Column-Level Validators.** `ColumnRow` grid with per-column `Validate` delegates: `Amount < 0` rejected, `TradeDate` must be in 2025. Invalid cells flagged inline.
- **Scenario C — Cross-Row OnValidate.** `CrossRow` grid enforcing "sum of dirty Quantity ≤ 1000" via `HandleCrossValidate` summing `args.DirtyRows` and populating `args.Errors` with one error per dirty row when the aggregate exceeds the limit.
- **Scenario D — IsLoading Skeleton State.** `LoadingRow` grid with Load/Clear Data buttons that toggle `_loadingInFlight`. 1500ms async fetch simulation. Status indicator ("Loading…" / "Empty" / "N rows loaded").
- **Scenario E — Reset / Discard All Changes.** `ResetRow` grid with a "Discard All Changes" button calling `await _resetSheet.ResetAsync()`. Reads `GetDirtyRows().Count` **before** reset so the log is meaningful.

**Files changed:** 1 new file. Zero component-source touches, zero test changes, zero nav changes.
**Build:** solution builds clean (0 warnings, 0 errors). Full test suite 1160/1160 unchanged.

### Editing-and-Validation reviews — **Completed 2026-04-10 (loop iteration 10)**

Catch-up reviews run on commit `6ce6d65` since it landed outside the standard implementer-dispatch flow.

- **Spec compliance:** ✅ APPROVED. All 5 scenarios match the iteration-7 audit spec exactly. `DataSheetValidateArgs<T>.Errors` mutation is correct (mutable `List<>`, `.Add()` pattern). Scenario C correctly sums `args.DirtyRows` and short-circuits on pass. Scenario E correctly awaits `ResetAsync()` and null-guards `_resetSheet`. No out-of-scope changes.
- **Code quality:** ⚠️ APPROVED WITH NITS. Zero blockers. 6 non-blocking minor items:
  - Date Validate predicate `d.Year < 2025 || d.Year > 2025` could be `d.Year != 2025`. Cosmetic.
  - `_loadingSheet` `@ref` captured but never read — safe but deletable for symmetry.
  - `AddLog` cap uses `log.RemoveAt(10)`; `log.RemoveAt(log.Count - 1)` reads more clearly as "drop the oldest".
  - `ClearLoadingData` is sync without explicit `StateHasChanged()` — works via click-handler render, mildly asymmetric with `LoadLoadingDataAsync`.
  - Inline `<style>` block duplicates `BulkOperations.razor`'s `.ds-demo-log` / `.ds-demo-btn` / `.ds-demo-toolbar` CSS — future refactor opportunity to hoist into a shared partial.
  - Scenario D `_loadingInFlight` disables BOTH buttons during load; disabling "Clear Data" during load is mildly surprising UX.

**Rename red herring:** Commit `6ce6d65`'s subject mentioned a "Bulk-Operations rename" but `git show --stat` confirms only `Editing-and-Validation.razor` was actually staged. `BulkOperations.razor` (PascalCase from iteration 8) is unchanged at HEAD. No cleanup needed.

### Stage 02a — Status: ✅ **COMPLETE**

Both Stage 02a demo pages are landed and reviewed:

- `BulkOperations.razor` (iteration 8, commit `0ba0eff3`)
- `Editing-and-Validation.razor` (iteration 10, commit `6ce6d65`)

Next: **Stage 02b — Keyboard-and-Accessibility.razor + Overview refactor** remains pending.

## Stage 02a (partial) — Bulk-Operations.razor (Complete 2026-04-10)

Loop iteration 8 (earlier phase). First of three Stage 02 demo pages. Commit `0ba0eff3` on `workInProgress`.

**Deliverable:** `samples/Marilo.Demo/Pages/Components/DataSheet/BulkOperations.razor` — NEW, ~440 lines, 5 Priority-1 scenarios stacked vertically.

**Scenarios implemented:**

- **Scenario A** — Add Row + Save. `ProductRow` grid (Text+Required, Number, Checkbox) with `AllowAddRow=true`. `OnSaveAll` diffs `DirtyRows` against a seeded-id HashSet to report "(N new, M edited)". Event log panel below.
- **Scenario B** — Delete toggle (F2 V05.4). `InventoryRow` grid with `AllowDeleteRow=true`. Uses the component's built-in per-row delete toggle (click to mark, click again to undo). `OnSaveAll` logs `args.DeletedRows.Count`.
- **Scenario C** — Bulk select + multi-row delete. `TaskRow` grid with 6 rows; uses the built-in header select-all checkbox and the built-in bulk action bar (Delete Selected / Reset Selected). Small `Priority` Select column added as harmless enrichment.
- **Scenario D** — Clipboard paste with type coercion. `PasteRow` grid (Text, Number, Date). Visible `<pre>` TSV block with deliberate invalid values (`abc` in Quantity, `invalid-date` in Date). Numbered instructions.
- **Scenario E** — Virtualization comparison. Two `MetricRow` grids side-by-side (responsive; stacks below 900px). Left: 50 rows + `EnableVirtualization=false`. Right: 500 rows + `EnableVirtualization=true`. Both `Height="300px"`. Data seeded in `OnInitialized`.

**Files changed:** 1 new file (`BulkOperations.razor`). Zero component-source touches, zero test changes, zero nav changes.
**Build:** solution builds clean. 0 errors, 0 warnings.

**Reviews:**

- Spec compliance: ✅ APPROVED. All 5 scenarios match spec exactly. Route `@page "/components/DataSheet/bulk-operations"` matches existing demo convention. Section separators via `PageSection` + `DemoSection`. Commit is cleanly scoped.
- Code quality: ⚠️ APPROVED WITH NITS. Zero blockers. 8 non-blocking polish items:
  - Magic number `Task.Delay(600)` repeated three times → extract `SimulatedSaveDelayMs` constant.
  - Three-line save-simulation pattern (`setFlag=true; StateHasChanged(); await Task.Delay(...); setFlag=false;`) repeated → extract `SimulateSaveAsync` helper.
  - Comment "`SaveAllAsync Step 7`" references internal step a new reader can't look up — drop or link.
  - Scenario D has no event log even though it demonstrates invalid-cell state — small log showing "Pasted N cells, M invalid" would make the teaching point explicit.
  - Scenario D's "Copy this block:" is a bare `<strong>` where other scenarios use `<h4>` — match the hierarchy.
  - `MetricRow.Id` is `{ get; set; }` while other row types use `{ get; init; }` — inconsistent.
  - `@using Marilo.Components.DataGrid` looks stale — page uses MariloDataSheet, not DataGrid. Worth confirming the types live in that namespace or adding a one-line comment.
  - File mixes `new() { ... }` and `[ ... ]` collection-expression styles — pick one for the file.
  - `log.RemoveAt(10)` in `AddLog` is correct but unclear — `log.RemoveAt(log.Count - 1)` is more intent-revealing.

### 🐛 Significant finding: ResetAsync-in-OnSaveAll clobbers pending deletions — **RESOLVED iteration 9 (commit `5abf8130`)**

**Resolution (2026-04-10 iteration 9):** Overview.razor no longer calls `ResetAsync()` in `HandleSaveAll`. `SaveAllAsync` now carries a prominent `<remarks>` XML doc warning explicitly forbidding consumers from calling `ResetAsync`/`BulkResetAsync` from within their handler, plus a belt-and-braces inline comment at the `OnSaveAll.InvokeAsync` call site. New regression test `SaveAll_WithDeletedRows_RemovesThemFromDisplayRowsAfterHandler` locks in strict post-conditions (surviving-row identity + order, empty `GetDirtyRows()`, deletion tracking cleared). Full suite: **1160/1160**. Reviews: spec ✅ APPROVED, code quality ⚠️ APPROVED WITH NITS (3 minor non-blocking items: test block comment could cross-reference XML doc more explicitly; inline `Step 6` comment could be paired with a matching label at the target block; two deleted-row tests could eventually be consolidated via `[Theory]`). Step ordering in `SaveAllAsync` deliberately left unchanged — larger design decision deferred; documentation + test combination is the load-bearing prevention mechanism.

**Original finding (preserved below for history):**

The implementer discovered that the existing `samples/Marilo.Demo/Pages/Components/DataSheet/Overview.razor` follows a pattern of calling `await _grid!.ResetAsync()` inside the `OnSaveAll` handler. Because `SaveAllAsync` awaits the consumer's handler **before** running its Step 6/7 cleanup (which is where deleted rows are actually removed from `_displayRows` and the dirty dictionary), calling `ResetAsync()` inside the handler clears `_dirtyRows` early. The subsequent Step 6 then re-computes `deletedKeys` from the now-empty dictionary and removes nothing.

**Effect:** Overview.razor's delete flow would silently drop pending deletions if a user ever deleted rows before saving. (The demo never exercises this path, so the bug is latent.)

**Root cause (unconfirmed):** ordering contract between `SaveAllAsync` Step 6 (deleted-row pruning) and consumer's `OnSaveAll` handler ability to mutate state.

**Resolution:** BulkOperations.razor deliberately omits the post-save `ResetAsync()` call and relies on the component's internal Step 7 cleanup. Comments in both Scenario B and Scenario C save handlers document the rationale.

**Follow-up owner:** future loop iteration — should:
1. Fix `Overview.razor` by removing the post-save `ResetAsync()` call (or replacing it with a `StateHasChanged()` call if the original intent was to refresh the view).
2. Add an XML doc comment on `SaveAllAsync` explicitly documenting that consumers must NOT call `ResetAsync` from within `OnSaveAll`.
3. Consider whether `SaveAllAsync` should reorder its steps so Step 6 runs before the `OnSaveAll.InvokeAsync` call — but that changes the semantics of "what's in DirtyRows when the consumer sees it" and may not be desirable.
4. Add a bUnit regression test: render a sheet, delete a row, call `SaveAllAsync` with a handler that awaits a `Task.Delay(10)`, assert the deleted row is gone from `_displayRows` afterwards.

### Internal-visibility finding

`AddRowAsync`, `MarkRowDeleted`, and `BulkDeleteAsync` are `internal` rather than `public`. The implementer correctly used the component's built-in UI controls (toolbar Add Row button, per-row delete button, select-all + bulk action bar) instead of `@ref` method invocations. **This is the correct pattern** — these methods exist to serve the component's internal UX and should not be exposed as external API unless there's a specific need. Flag for confirmation that the `internal` visibility is intentional.

### ComponentRegistry finding

`samples/Marilo.Demo/Data/ComponentRegistry.cs` does not register DataSheet under slug `"DataSheet"`, so `ComponentDemoLayout` cannot look up a `ComponentInfo` and renders no breadcrumb/header. Pre-existing issue affecting Overview.razor. **Not fixed in this task** — out of scope, would affect routes beyond DataSheet demos.

## Stage 02 — Example UX Audit (Complete 2026-04-10)

Loop iteration 7. Audit-only deliverable — no code changes this iteration.

**Deliverable:** `stages/02-example-ux/output/datasheet-example-ux-audit-2026-04-10.md` (~1900 words)

**Headline findings:**

- Current demo `samples/Marilo.Demo/Pages/Components/DataSheet/Overview.razor` (206 lines, "Investment Position Editor") covers **~55% of the API surface**. Strong on core editing/validation/save; weak on clipboard operations, keyboard interactivity, bulk operations, and theming.
- **Top-level parameters:** 13/18 demonstrated (72%) — missing IsLoading, AriaLabel, ToolbarTemplate, Class, Style.
- **Column parameters:** 10/11 demonstrated (91%) — missing MinWidth, CellTemplate.
- **Behavioral features:** 17/25 demonstrated (68%) — missing worked clipboard example, interactive keyboard demo, virtualization comparison, bulk select, IsLoading skeleton, user-initiated Reset, CellTemplate, cross-row validation, theme switching.

**Recommended demo structure: multi-page (follows DataGrid precedent).** Four pages totaling 16 scenarios:

| Page | Scenarios | New LOC |
| --- | --- | --- |
| **Overview.razor** (refactor existing) | 1 headline scenario | ~80 |
| **Editing-and-Validation.razor** (NEW) | Required, custom validator, cross-row validation, IsLoading, Reset | ~120 |
| **Bulk-Operations.razor** (NEW) | Add row, delete toggle, bulk select+delete, clipboard paste, virtualization comparison | ~140 |
| **Keyboard-and-Accessibility.razor** (NEW) | Keyboard nav, edit mode keys, command keys, Space toggle, ARIA inspection | ~140 |
| **Total** | **16 scenarios** | **~480** |

**Priority-1 gaps (6, must-have):**

1. Clipboard paste with type coercion and error handling
2. Bulk select all + multi-row delete (with F2 V05.4 delete toggle)
3. Virtualization performance comparison (small vs large dataset)
4. IsLoading skeleton state
5. Reset/Discard changes (user-initiated)
6. Keyboard navigation and edit shortcuts (interactive demo with key log)

**Priority-2 gaps (6, should-have):** ToolbarTemplate, CellTemplate, cross-row OnValidate, error handling/retry, MarkRowDeleted toggle explicit demo, theme switching.

**Priority-3 gaps (4, nice-to-have):** AriaLabel customization, Class/Style injection, MinWidth, programmatic AddRowAsync/BulkDeleteAsync.

**Suggested implementation approach:** two sub-batches across future iterations

- **02a** — page scaffolding + Editing-and-Validation + Bulk-Operations (scenarios 1–5 from P1 set)
- **02b** — Keyboard-and-Accessibility + Overview refactor (scenario 6 + polish)

**Effort estimate:** 3-4 development days total for the P1 set.

**Open design questions (4):** clipboard data source pattern, keystroke logger overlay shape, theme switching scope (defer to Stage 03), cross-row validation scale. All have recommendations in the audit doc; only clipboard source is a true blocker that needs quick confirmation (recommendation: pre-formatted text block).

## Stage 01b — Verification (Complete 2026-04-10)

All 10 sub-tasks (GAP-DATASHEET-V01 through V10) audited in one loop iteration via parallel read-only subagents.

| Sub-task | Area | Verdict | Gap count |
| --- | --- | --- | --- |
| V01 | columns-and-schema | MINOR_GAPS | 2 |
| V02 | editing-and-validation | REAL_GAPS | 3 |
| V03 | **selection-and-ranges** | **REAL_GAPS_LARGE** | **10 (cohesive feature)** |
| V04 | bulk-paste-and-clipboard | MINOR_GAPS | 4 |
| V05 | bulk-operations-and-saveall | REAL_GAPS | 5 |
| V06 | virtualization-and-performance | CLEAN | 0 |
| V07 | keyboard-and-accessibility | REAL_GAPS | 9 |
| V08 | theming-and-css-provider | CLEAN | 0 |
| V09 | DataSheetColumnType enum | CLEAN | 0 |
| V10 | CellState enum | CLEAN | 0 |

**Aggregate output:** `stages/01-spec-review/output/datasheet-spec-gaps-verified-2026-04-10.md`

**Stage 03 resolution design for V03 (large):** `../datasheet-gap-analysis/stages/03-resolution-design/output/gap-datasheet-v03-selection-ranges-resolution.md`

## Batch F1 Implementation (Complete 2026-04-10)

Landed via subagent-driven development (implementer + spec review + code-quality review). Commit `6794644` on `workInProgress`.

- ✅ V01.1 — Checkbox `Required=true` rejects `false`/`null`
- ✅ V01.2 — Number parsing fallback via `ParseNumericValue(string, Type)` helper supporting int/long/short/byte/decimal/double/float and nullables
- ✅ V02.1 — Field-level dirty state cleared when cell reverts to original; row dropped from `_dirtyRows` when all fields revert

**Tests:** 9 new bUnit tests (all passing). Full suite: **1106/1106** ✓
**Files changed:** `MariloDataSheet.Data.cs`, `MariloDataSheet.Editing.cs`, `MariloDataSheet.Rendering.cs`, `tests/Marilo.Tests.Unit/DataGrid/MariloDataSheetTests.cs`
**Reviews:** spec-compliance ✅ APPROVED; code-quality ⚠️ APPROVED WITH NITS (non-blocking — listed below)

### Code-quality nits to address in a follow-up loop

- `Editing.cs:336` — narrow the bare `catch` around `Convert.ChangeType` to `catch (Exception ex) when (ex is InvalidCastException or FormatException or OverflowException)`.
- `Editing.cs:283-303` — restore switch-expression idiom; extract a small `ParseNumberForCell(column, text)` helper so the Number branch fits.
- `Editing.cs:325` — add `using System.Globalization;` at file top; remove fully-qualified references.
- `Rendering.cs:176` + `Editing.cs:289` — duplicated `typeof(TItem).GetProperty(column.Field)?.PropertyType` lookup; extract a cached `GetColumnClrType(column)` helper.
- `Data.cs:60` — add a short code comment noting `object.Equals` does reference-equality for user POCO types without overridden `Equals`.
- `Data.cs:44` — invert `entryIsNew` flag logic: compute `originalValue` before `TryGetValue` insert and always run match check.
- Tests — add `await` on `InvokeAsync(...)` calls to match surrounding convention.
- Tests — extract `RenderCheckboxSheet(bool required)` helper to deduplicate three test setups.
- Tests — move `ParseNumericValue` to non-generic `DataSheetNumericParser` internal static class so tests don't need to reach through `MariloDataSheet<NumericRow>`.

## Batch F3 Implementation (Complete 2026-04-10)

Landed via subagent-driven development (single implementer + self-review). Commit `3071c39` on `workInProgress`.

- ✅ V04.1 — CRLF line endings normalized in `PasteFromClipboard` via `.Replace("\r\n", "\n").Replace("\r", "\n")` before splitting; last-cell-of-row corruption for Number/Date columns eliminated.
- ✅ V04.2 — Parse-error paste cells no longer write raw string to model. New `TryParseCellValue` + `MarkPasteCellInvalid` helpers surface `CellState.Invalid` via `entry.ValidationErrors[field]`, leaving the underlying `TItem` property at its pre-paste value. Error text matches spec: "Invalid number", "Invalid date", "Value not in options". Select columns gained parity coercion (value must exist in `Options`).
- ✅ V04.3 — Deleted rows skipped during paste. Independent `rowCursor` advances through `_displayRows` past any row returning `true` from `IsRowDeleted`; TSV row index `r` only increments when a live row consumes a TSV row.
- ✅ V04.4 — Copy honors `Format` via `data-raw-value`. Non-`Computed` cells with a `Format` delegate emit `data-raw-value` using `InvariantCulture` (`.Rendering.cs:73-86`). JS interop `copyToClipboard` handler in `marilo-datasheet.js:81-93` prefers `data-raw-value` over `textContent`. Computed and unformatted cells deliberately omit the attribute (their `textContent` already holds the correct copy value, matching spec line 36 for Computed).

**Tests:** 9 new bUnit tests (2 V04.1 + 3 V04.2 + 1 V04.3 + 3 V04.4). Full suite: **1119/1119** ✓ (verified independently; subagent's internal count of 1115 was a minor undercount).
**Files changed:** `MariloDataSheet.Editing.cs`, `MariloDataSheet.Rendering.cs`, `wwwroot/js/marilo-datasheet.js`, `tests/Marilo.Tests.Unit/DataGrid/MariloDataSheetTests.cs`. 416 insertions, 14 deletions.
**Build:** 0 warnings, 0 errors.

### Scope notes (from subagent's DONE_WITH_CONCERNS)

1. **One extra file touched beyond the original scope:** `wwwroot/js/marilo-datasheet.js` received 5 new lines in the `Ctrl+C` handler. Without this JS change, the `data-raw-value` attribute from V04.4 has no copy-behavior effect and would be untestable end-to-end. Judgment call — the JS addition was necessary to make V04.4 a complete fix rather than dead HTML markup.
2. **V04.4 narrower than the task prompt.** The prompt suggested emitting `data-raw-value` on all data cells (including Computed, with Format-result as the value). The landed implementation only emits it on non-`Computed` cells that have a `Format` delegate — reasoning: cells without a Format delegate already have `textContent` == raw value, Computed cells' `textContent` is the spec-correct copy value, and the JS handler cleanly falls back to `textContent` when the attribute is null. Semantically equivalent for copy behavior, smaller DOM footprint. **Flag for code review:** if the reviewer wants the attribute emitted universally (e.g., to support future JS consumers beyond copy), the C# side should be broadened and two tests flipped (`ComputedCell_DoesNotEmitDataRawValue_DisplaysFormattedValue`, `DataCell_WithoutFormatDelegate_OmitsDataRawValue`).
3. **F1 code-quality nits intentionally untouched.** Per batch scope rules.
4. **Internal rename:** `ParseCellValue` → `TryParseCellValue` (private static, single call-site updated, no API impact).

### F3 reviews (2026-04-10)

- **Spec compliance:** ✅ APPROVED. All four fixes match the spec exactly. Error strings verbatim. Model stays at pre-paste value on parse failure. F1 `// TODO(V04.2)` removed. All 9 required tests + 4 acceptable extras present. No out-of-scope changes.
- **Code quality:** ⚠️ APPROVED WITH NITS. No blocking issues. Non-blocking items logged below.

### F3 code-quality nits to address in a follow-up loop

- **`Editing.cs:355` — Date parsing uses current culture, round-trip bug on non-`en-US` locales.** V04.4 emits dates via `Convert.ToString(rawValue, CultureInfo.InvariantCulture)` but `DateTime.TryParse(text, out var dt)` on the paste side uses the current culture. On `de-DE` (and most non-US locales), copy-emits `4/10/2026` but paste expects `10.04.2026`, breaking the round-trip. **Fix:** `DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)`. This is worth bundling into the next polish pass because it's directly in-scope for F3's stated goal.
- `Rendering.cs:85` — Blazor `RenderTreeBuilder` sequence numbers are non-monotonic: `data-raw-value` uses `29`, next `onclick` uses `28`, `CellTemplate` content uses `30`. Blazor tolerates it but it's a diff-algorithm footgun. Renumber so sequence is strictly increasing.
- `Editing.cs:363` — `column.Options.Any(o => o.Value == text)` uses ordinal case-sensitive compare implicitly. Add `StringComparer.Ordinal` or a comment documenting the case-sensitivity decision.
- `Editing.cs:367` — `TryParseCellValue` default switch arm silently returns `(true, text, null)` for unknown column types. Add `Debug.Assert` or enumerate expected types explicitly to harden against future additions.
- `marilo-datasheet.js:87` — simplify `(rawValue !== null ? rawValue : (activeCell.textContent || '')).trim()` to `(rawValue ?? activeCell.textContent ?? '').trim()` using nullish coalescing.
- Tests — 9 F3 tests use `EnterEditMode`+`Escape` to anchor active cell; 4 supplementary tests use cleaner `ActivateCell(...)`. Migrate for consistency.
- Tests — structural boilerplate duplication across 13 F3 tests; extract a `BuildSheet(params (string field, DataSheetColumnType type)[] cols)` helper to cut ~60% of test LOC.

## Batch F2 Implementation (Complete 2026-04-10)

Landed via subagent-driven development (single implementer + self-review). Commit `7a40055` on `workInProgress`.

- ✅ **V02.2 / V05.1 — CellState.Saving/Saved transitions.** Added `DirtyRowEntry.TransientState` nullable field + component-level `_savedStateDurationMs` (default 1000ms). `SaveAllAsync` now marks non-deleted dirty entries `Saving` → fires `OnSaveAll` → flips to `Saved` → awaits delay → clears override so cells report `Pristine`. `GetCellState` honors `TransientState` only for fields in `DirtyFields` so non-dirty fields stay Pristine during a save. No new `CellState` enum values added.
- ✅ **V05.2 — Deleted rows removed from `_displayRows` after save.** After successful `OnSaveAll.InvokeAsync`, iterates dirty entries with `IsDeleted=true`, extracts row keys, and removes matching rows from `_displayRows` + drops their `_dirtyRows` entries. **Critical correctness fix included:** refreshes `entry.Original = DeepClone(entry.Current)` for non-deleted saved rows so round-trip edits (save → edit back to pre-save value) are correctly detected as dirty rather than silently dropped.
- ✅ **V05.3 — ResetAsync removes newly-added rows + AddRowAsync tracks them as dirty.** Added `DirtyRowEntry.IsNewlyAdded` flag. `AddRowAsync` now creates a `DirtyRowEntry` with `IsNewlyAdded=true` and seeds `DirtyFields` with every editable non-computed column so the new row appears in `GetDirtyRows()` and `DataSheetSaveArgs.DirtyRows`. `CommitCellEdit` preserves dirty fields on newly-added rows regardless of value comparison (the default Original snapshot would otherwise drop them when the user types a default value). `ResetAsync` collects `IsNewlyAdded` entries and removes them from `_displayRows` instead of attempting to revert nonexistent originals; other entries still get field-level Original restoration. **This also resolves V05-06 (data-loss risk for newly-added rows) from my fire #17 findings.**
- ✅ **V05.4 — Delete toggle (un-delete on re-click).** `MarkRowDeleted` now toggles `entry.IsDeleted` rather than unconditional set. If the row ends up un-deleted AND has no remaining dirty state (no dirty fields, no validation errors, not newly added), the entry is dropped so state queries report Pristine. Pre-existing dirty edits on the row are preserved across the toggle.
- ✅ **V05.5 — BulkReset restores original values.** `BulkResetAsync` now iterates each selected row's `DirtyFields` and restores field values via `GridReflectionHelper.SetValue(entry.Current, field, originalValue)` BEFORE dropping the dirty entry. Previously it only removed entries from `_dirtyRows` without reverting the data — a silent data state bug. Newly-added selected rows are removed from `_displayRows` entirely, mirroring `ResetAsync` semantics.

**Tests:** 12 new bUnit tests (2 V02.2/V05.1 + 2 V05.2 + 3 V05.3 + 3 V05.4 + 2 V05.5). Full suite: **1132/1132** ✓ (up from 1119 post-F3; independently verified).
**Files changed:** `MariloDataSheet.Data.cs` (+164 / -23), `MariloDataSheet.razor.cs` (+48 / -5), `tests/Marilo.Tests.Unit/DataGrid/MariloDataSheetTests.cs` (+409 / -1). Scope discipline held: zero touches to `.Editing.cs`, `.Rendering.cs`, JS, or any non-DataSheet source.
**Build:** 0 warnings, 0 errors.

### F2 scope notes (from subagent's DONE_WITH_CONCERNS)

1. **Pre-existing unrelated work in working tree at start:** The working tree had uncommitted changes in `.Editing.cs`, `.Rendering.cs`, `wwwroot/js/marilo-datasheet.js`, and one test for an "F3 polish / culture-aware date parse" fix (likely the F3 review nit #1 about `DateTime.TryParse` culture). The subagent stashed the non-F2 source changes to measure a clean F2 baseline (1116/1119 — the 3 failures were pre-existing paste tests depending on the uncommitted `Editing.cs` culture fix), committed F2 cleanly, then restored the polish test to the working tree so the other work isn't lost. The `Editing.cs` change remains as an uncommitted working-tree modification for the next F3-polish batch to pick up.
2. **Timing-test discipline:** All Saving/Saved tests use `cut.Instance._savedStateDurationMs = 0` rather than `Task.Delay(1000)` waits, so the Saved visual-indicator window collapses to zero in tests; the tests verify state *ordering* (Dirty → Saving during `OnSaveAll` → Pristine after completion), not wall-clock timing.
3. **F1 + F3 code-quality nits intentionally untouched.** Per batch scope rules.
4. **No new CellState enum values added.** Used the existing `Saving` / `Saved` values.
5. **Option A chosen for V02.2/V05.1** (`TransientState` field on `DirtyRowEntry`) over Option B (component-level `HashSet<object>`). Cleaner cohesion with the existing entry-scoped state model.

### F2 reviews — **Completed 2026-04-10 (loop iteration 4)**

Catch-up reviews run on commit `7a40055` during loop iteration 4.

- **Spec compliance:** ✅ APPROVED. All 5 F2 fixes match the spec exactly; polish items 4/5/6 bundled correctly; 12 tests covering all fix areas; no out-of-scope changes. The V05.2 `entry.Original = DeepClone(entry.Current)` refresh after save is a spec-aligned refinement that is itself test-covered.
- **Code quality:** ⚠️ APPROVED WITH NITS. No blockers. **Two notable save-path nits flagged as realistic production concerns** (see below).

#### F2 code-quality nits (resolved or pending)

1. ✅ **RESOLVED in commit `6322198` (loop iteration 5).** `SaveAllAsync` now wraps Steps 5-7 in a `try`/`catch`/`finally` block. On exception, snapshotted `savingEntries` have `TransientState` cleared, `StateHasChanged` fires, and the exception re-throws. Regression test `SaveAll_OnSaveAllThrows_ClearsTransientStateAndRethrows` added.
2. ✅ **RESOLVED in commit `6322198` (loop iteration 5).** Added private `bool _isSaving` field. Silent `if (_isSaving) return;` at top of `SaveAllAsync` (after the `_dirtyRows.Count == 0` early return). Cleared in `finally`. Follows the `_isXxx` convention used elsewhere in the partial.

#### F2 code-quality minor nits (truly optional)

- `ResetAsync` + `BulkResetAsync` duplicate restore-then-remove logic — extract `RestoreEntryOrRemoveNewRow(DirtyRowEntry<TItem>)` helper.
- `AddRowAsync` seeding `DirtyFields` with all editable columns works because `CommitCellEdit` guards on `!entry.IsNewlyAdded`. Invariant lives in two files — add an inline comment at `AddRowAsync` pointing at the guard.
- `DeepClone` on `entry.Current` after save breaks consumers whose `TItem` holds non-serializable members. Not new in this commit but newly reachable from the save path. Consider a doc comment on `SaveAllAsync` noting the requirement, or try/catch fallback to `MemberwiseClone`.
- `_savedStateDurationMs` internal field is pragmatic test injection but leaky. Future: `TimeSpan SavedIndicatorDuration` public parameter or `internal ITimeProvider`.
- `MariloDataSheet.Data.cs:272-291` two-loop pattern (collect keys then remove) could be one `foreach`. Micro-nit.
- Test file `CaptureDuringSave<T>(Func<T> probe)` helper would simplify future transient-state observation tests.

## Polish Pass (F1+F3 nits) — Complete 2026-04-10

Landed via subagent-driven development (implementer + spec review + code-quality review). Commit `7b5e217` on `workInProgress`. Items 4, 5, 6 from the polish pass were concurrently swept into commit `7a40055` (Batch F2) by a parallel process — functionally identical net result, though git history attributes those three items to F2 rather than the polish commit.

Ten items addressed across commits `7b5e217` + `7a40055`:

- ✅ ITEM 1 (F3 review #1) — `DateTime.TryParse` now uses `InvariantCulture + DateTimeStyles.None`, fixing the round-trip bug on non-`en-US` locales. Regression test `Paste_DateInInvariantCulture_ParsesRegardlessOfCurrentCulture` added (flips `CurrentCulture` to `de-DE` in try/finally, pastes `"4/10/2026"`, asserts correct `DateTime` + `Dirty` state).
- ✅ ITEM 2 (F1 review #1) — `ParseNumericValue`'s `Convert.ChangeType` catch narrowed to `catch (Exception ex) when (ex is InvalidCastException or FormatException or OverflowException)`.
- ✅ ITEM 3 (F1 review #3) — `using System.Globalization;` + `using System.Diagnostics;` added to `Editing.cs`; fully-qualified references dropped.
- ✅ ITEM 4 (F1 review #5) — `object.Equals` comment added to `CommitCellEdit` explaining reference-equality caveat for user POCOs. [Landed in `7a40055`.]
- ✅ ITEM 5 (F1 review #6) — `entryIsNew` flag eliminated; `CommitCellEdit` restructured to compute `originalValue` up-front and use `hadExistingEntry` (the `TryGetValue` bool) directly. [Landed in `7a40055`.]
- ✅ ITEM 6 (F1 review #7) — Six F1 bUnit tests converted to `async Task` with explicit `await` on `InvokeAsync(...)`. [Landed in `7a40055`.]
- ✅ ITEM 7 (F3 review #3) — `column.Options.Any(o => string.Equals(o.Value, text, StringComparison.Ordinal))` for Select comparison.
- ✅ ITEM 8 (F3 review #4) — `TryParseCellValue` default switch arm now `Debug.Assert(false, …)`. Required an acceptable scope expansion: explicit `case DataSheetColumnType.Text:` and `case DataSheetColumnType.Computed:` arms added so the assertion only fires on genuinely unknown enum values.
- ✅ ITEM 9 (F3 review #5) — `marilo-datasheet.js` copy handler simplified to `(rawValue ?? activeCell.textContent ?? '').trim()`.
- ✅ ITEM 10 (F3 review #2) — `Rendering.cs` `data-raw-value`/`onclick` sequence numbers swapped (28↔29) for strictly monotonic block ordering. Clean 2-line swap, no cascade.

## Polish Follow-up (F2 save-path + residual nits) — Complete 2026-04-10

Loop iteration 5. Landed via subagent-driven development (implementer + spec review + code-quality review). Commit `6322198` on `workInProgress`. Five items: two F2 save-path correctness fixes (high priority) + three small residual polish nits.

- ✅ **F2 save-path #1** — `SaveAllAsync` try/catch/finally around `OnSaveAll.InvokeAsync`. Rollback clears `TransientState` on snapshotted `savingEntries`, fires `StateHasChanged`, re-throws. `DirtyFields` and `ValidationErrors` preserved so rows return to `Dirty` and are retryable. Regression test: `SaveAll_OnSaveAllThrows_ClearsTransientStateAndRethrows`.
- ✅ **F2 save-path #2** — Private `bool _isSaving` re-entrancy guard at top of `SaveAllAsync`. Silent return on re-entry (matches UX convention). Cleared in `finally`.
- ✅ **Polish-pass nit #1** — `Debug.Assert(false, ...)` → `Debug.Fail(...)` in `TryParseCellValue` default arm (idiomatic one-arg form for unreachable branches).
- ✅ **Polish-pass nit #3** — `using System.Globalization;` added to `MariloDataSheetTests.cs`; 5 fully-qualified `System.Globalization.CultureInfo` references unqualified (including a format lambda at line 737 and 4 in the V04.4 date-culture regression test).
- ✅ **Polish-pass nit #4** — Comment added above `decimal.TryParse` in `ParseNumericValue` explaining the intentional culture split (numeric input = `CurrentCulture` because user-typed is locale-aware; date paste = `InvariantCulture` because V04.4 emits via `Convert.ToString(…, InvariantCulture)`).

**Tests:** 1 new regression test (the save-path throw rollback). Full suite: **1156/1156** ✓ (baseline 1132 + 23 F4 + 1 polish = 1156 — F4 landed concurrently in commit `95f7f17`).
**Files changed:** `MariloDataSheet.Data.cs` (save-path restructure + `_isSaving`), `MariloDataSheet.Editing.cs` (Debug.Fail + culture comment), `MariloDataSheetTests.cs` (using + regression test).
**Reviews:**

- Spec compliance: ✅ APPROVED. Five items match spec exactly. `savingEntries` snapshot precisely scopes the rollback set; catch doesn't touch `DirtyFields`/`ValidationErrors`; regression test asserts both post-conditions.
- Code quality: ⚠️ APPROVED WITH NITS. Zero blockers. Three non-blocking minor suggestions:
  - `MariloDataSheet.Data.cs:357` — catch's `StateHasChanged()` is null-guarded on `savingEntries`; if an exception fires before Step 4 there's no visible rollback. Arguably correct; add a one-line comment noting intent.
  - Re-entrancy test (second `SaveAllAsync` call returning silently while first awaits) would round out coverage — not blocking.
  - `savingEntries` could be initialized non-null upfront to simplify the catch. Taste call.

## Batch F4 — Keyboard and Accessibility — Complete 2026-04-10

Loop iteration 5. **Landed in parallel with the polish pass via a concurrent subagent dispatch** — commit `95f7f17` on `workInProgress` immediately precedes commit `6322198`. Both commits reviewed in the same iteration.

Nine V07 gaps + Tab row wrapping:

- ✅ **V07.1** — Enter on active non-editing cell enters edit mode (same as F2). Placed before the existing in-edit Enter-commit branch.
- ✅ **V07.2** — Printable characters trigger edit mode and replace value (Text/Number columns only). JS side uses `isInEditor` detection so `preventDefault` on printable keys doesn't break open editors. Number branch routes through `ParseNumericValue` with silent fall-through on unparseable input.
- ✅ **V07.3** — Space toggles checkbox cell value, fires `CommitCellEdit`, no edit mode transition. JS `preventDefault` on Space prevents page scroll.
- ✅ **V07.5** — `aria-rowindex` on every `<tr>` (header = 1, data rows = `dataRowPosition + 2`).
- ✅ **V07.6** — `aria-colindex` on every cell via single `nextAriaColIndex` counter that walks select + data + delete columns in render order. Header mirrors with `headerColIndex`.
- ✅ **V07.7** — `aria-describedby` links invalid cell to a visually-hidden `<span id="{_gridId}-err-{rowKey}-{field}">{error}</span>`. Inline `clip:rect(0,0,0,0)` styles guarantee sr-only behavior without depending on SCSS.
- ✅ **V07.8** — `aria-busy="@((IsLoading || IsSaving) ? "true" : null)"`.
- ✅ **V07.9** — `_ariaAnnouncement` populated on dirty-count transitions (diff prior/post), save success ("Changes saved successfully."), validation-block ("Save blocked: fix validation errors first."), and reset.
- ✅ **Tab row wrapping** — new `MoveToNextEditableCell` helper builds editable-column index list (skips non-editable + Computed), handles forward/reverse traversal with row wrapping, handles "currently on Computed column" edge case, returns `false` at grid boundary so caller `ClearActiveCell`s for exit.

**Tests:** 23 new bUnit tests across all 9 V07 fix areas + Tab wrap. Full suite: 1155/1155 (F4 standalone) → 1156/1156 (after polish commit).

### F4 reviews — **Completed 2026-04-10 (loop iteration 5)**

Catch-up reviews run on commit `95f7f17` during loop iteration 5 (F4 landed in parallel with the polish commit).

- **Spec compliance:** ✅ APPROVED. All 9 V07 gaps + Tab row wrapping implemented per spec. 23 tests across every fix area, assertions meaningful (DOM aria-* reads, `_ariaAnnouncement` direct checks, a V07.9 sentinel pattern proving non-re-firing on same-row edits). No out-of-scope changes, no public API changes (`IsSaving` pre-existed).
- **Code quality:** ⚠️ APPROVED WITH NITS. No blockers.

#### F4 code-quality nits (resolved or pending)

1. ✅ **RESOLVED in commit `0c70a69` (loop iteration 6).** `SanitizeIdPart(object?)` helper added to `MariloDataSheet.Rendering.cs` — replaces any char outside `[A-Za-z0-9_-]` with `_`, null → `"null"`, empty → `"empty"`. Applied at the shared `cellErrorId` variable so the span `id` and cell `aria-describedby` are structurally guaranteed to match. Regression test `InvalidCell_WithRowKeyContainingSpecialChars_ProducesValidDescribedbyId` added — uses row key `abc def "#%"`, asserts char-level safety AND `cut.Find($"#{describedBy}")` round-trips.
2. ⏸️ **Deferred — SKIPPED in commit `0c70a69`.** Attempted in iteration 6 but no DataSheet SCSS file exists in any provider (`FluentUI`, `Material`, `Bootstrap`). The `mar-datasheet__*` classes are emitted from `FluentUICssProvider.cs` (C#) without matching SCSS selectors. Inline sr-only fallback style preserved. Future pass should either create `_data-sheet.scss` per provider or add the rule to an existing shared SCSS file.
3. ⏸️ **Pending.** V07.2 uncached reflection — route through `GridReflectionHelper` caching.
4. ⏸️ **Pending.** V07.2 numeric non-parseable keystroke silently falls through; trace/debug log would help.
5. ⏸️ **Pending.** V07.9 twice-iteration of `_dirtyRows.Values`; `_dirtyRowCount` cache would be cleaner.
6. ⏸️ **Pending.** `marilo-datasheet.js` `isInEditor` — `closest('input, select, textarea')` would be marginally more robust.
7. ✅ **RESOLVED in commit `0c70a69`.** Renamed `EnterKey_OnInactiveCell_EntersEditMode` → `EnterKey_OnActiveNonEditingCell_EntersEditMode`.
8. ✅ **RESOLVED in commit `0c70a69`.** Tab-exit comment rewritten to accurately describe: native browser Tab handles focus, upstream JS `preventDefault` leaves focus stuck on grid root `<div>` as a known limitation, interop layer should not `preventDefault` on boundary Tab.

### Polish Follow-up #2 (F4 mop-up) — Complete 2026-04-10

Loop iteration 6. Commit `0c70a69` on `workInProgress`. Five in-scope items plus bonus test-only refactors:

- ✅ ITEM 1 — Row-key sanitization for `aria-describedby` IDs (semi-important a11y correctness fix) + regression test
- ⏸️ ITEM 2 — SCSS sr-only class LEGITIMATELY SKIPPED (no DataSheet SCSS file exists to edit)
- ✅ ITEM 3 — Test rename
- ✅ ITEM 4 — SaveAllAsync catch null-guard comment
- ✅ ITEM 5 — Tab-exit focus comment rewrite
- ✅ **Bonus (deferred refactors from iterations 2-3):** `RenderCheckboxSheet(bool required, List<TestRow>)` test helper extracted to deduplicate checkbox test setup (F1.N8); V04 paste tests migrated from `EnterEditMode`+`Escape` dance to direct `ActivateCell(...)` call (F3.N6).

**Tests:** 1 new regression test. Full suite: **1159/1159** ✓
**Files changed:** `MariloDataSheet.Rendering.cs`, `MariloDataSheet.Data.cs`, `MariloDataSheet.Editing.cs`, `MariloDataSheetTests.cs`.
**Reviews:**

- Spec compliance: ✅ APPROVED. All 4 in-scope items match spec exactly; ITEM 2 skip legitimate per escape clause; regression test asserts both spec claims (char whitelist AND `#id` round-trip); bonus test refactors noted as minor acceptable scope creep.
- Code quality: ⚠️ APPROVED WITH NITS. Zero blockers. `SanitizeIdPart` is allocation-lean (single char[] buffer, no regex, no reflection). Nits: optional `string` overload for the hot path; collision behavior (`"a b"` and `"a_b"` both sanitize to `"a_b"`) could use an xmldoc note; `[Theory]` tests for edge cases (all-invalid, null sentinel, empty sentinel) would lock the contract more tightly.

### Remaining F4 nits (pending, for a future mop-up)

Items 3-6 from the F4 code-quality review: reflection caching, numeric-fall-through tracing, `_dirtyRowCount` cache field, `marilo-datasheet.js` `closest()` selector. Plus ITEM 2 when the DataSheet SCSS file exists.

**Tests:** 1 new regression test (ITEM 1). Full suite: **1132/1132** ✓ (up from 1119 post-F3 + 12 F2 tests + 1 polish test = 1132, confirmed).
**Files changed (polish commit `7b5e217`):** `MariloDataSheet.Editing.cs`, `MariloDataSheet.Rendering.cs`, `wwwroot/js/marilo-datasheet.js`, `tests/Marilo.Tests.Unit/DataGrid/MariloDataSheetTests.cs`.
**Reviews:** spec-compliance ✅ APPROVED; code-quality ⚠️ APPROVED WITH NITS (4 non-blocking — listed below).

### Polish-pass nits (from code-quality review 2026-04-10) — non-blocking

- `Editing.cs:377` — `Debug.Assert(false, ...)` could be the idiomatic `Debug.Fail("...")` (one-arg form for unreachable branches). Behaviorally identical.
- Test file culture manipulation (`MariloDataSheetTests.cs:1346`) uses `CultureInfo.CurrentCulture` on the ambient thread. Works today because in-assembly parallelism is disabled, but consider `CultureInfo.DefaultThreadCurrentCulture` or a `[Collection]` annotation if parallelism is later enabled.
- Test file has 4 fully-qualified `System.Globalization.CultureInfo` references; add `using System.Globalization;` at the top.
- `Editing.cs:400-401` — `ParseNumericValue` still uses `CultureInfo.CurrentCulture` for decimals (intentional — user-typed input is culture-sensitive; pasted dates come from code-formatted raw values) but worth a one-line comment to preempt the same confusion this commit just fixed.

All four are follow-ups for a future pass, not blockers.

## Batch F4 Implementation (Complete 2026-04-10)

Landed via subagent-driven development (single implementer + self-review). Commit `95f7f17` on `workInProgress`. **This completes the final feature-level batch for DataSheet — all code-level gaps from Stage 01b verification are now resolved except the deferred V03 cell range selection subsystem.**

- ✅ **V07.1 — Enter → edit mode.** New branch in `HandleKeyDown` at `MariloDataSheet.Editing.cs:174-183`: Enter on active cell when NOT in edit mode calls `EnterEditMode`. Existing in-edit-mode Enter (commit + move down) preserved; last-row path now commits in place rather than attempting an out-of-bounds move.
- ✅ **V07.2 — Printable character → edit mode.** C# branch at `Editing.cs:247-287` handles `key.Length == 1 && !ctrl && key != " "` on Text/Number cells. Number columns parse via existing `ParseNumericValue`. Non-matching input enters edit mode with empty value (Text) or falls through (Number). JS handler at `marilo-datasheet.js:77-99` calls `preventDefault` on printable keys only when focus is NOT inside an input/select/textarea so open editors still type normally. **No new [JSInvokable] method needed** — existing `HandleKeyDown(key, ctrl, shift)` route already forwards single-character keys.
- ✅ **V07.3 — Space → checkbox toggle.** New Space branch at `Editing.cs:185-202` tests column type, reads current value, commits flip. Non-checkbox cells are no-op. JS handler at `marilo-datasheet.js:87-90` suppresses page-scroll default on Space when not in an editor.
- ✅ **V07.5 — `aria-rowindex`.** Data rows at `Rendering.cs:25-39` use `_displayRows.IndexOf(row) + 2` (header is `aria-rowindex=1`). Header row explicitly `aria-rowindex="1"` at `.razor:97-103`.
- ✅ **V07.6 — `aria-colindex`.** Single `nextAriaColIndex` counter at `Rendering.cs:41-87, 164-166` runs across select-checkbox column, data cells, and delete column. Header uses a parallel `headerColIndex` counter at `.razor:100-133`.
- ✅ **V07.7 — `aria-describedby` on invalid cells.** Invalid cells at `Rendering.cs:74-98, 143-153` emit `aria-describedby="{_gridId}-err-{rowKey}-{field}"` pointing to a visually-hidden `<span>` containing the error text (inline sr-only style). Existing `title` + `aria-invalid="true"` attributes retained alongside.
- ✅ **V07.8 — `aria-busy` + `IsSaving`.** One-line fix at `MariloDataSheet.razor:13`: `aria-busy="@((IsLoading || IsSaving) ? "true" : null)"`.
- ✅ **V07.9 — `aria-live` dirty count.** `CommitCellEdit` at `Data.cs:59-67, 135-150` snapshots dirty-row count before mutation, compares after, sets `_ariaAnnouncement = "1 row modified"` or `$"{N} rows modified"` **only when the count actually changes**. Edits within an already-dirty row do NOT fire a new announcement (no spam).
- ✅ **Tab row wrapping.** New `MoveToNextEditableCell(reverse)` helper at `Editing.cs:235-258, 310-401` skips Computed/non-editable columns, wraps from last editable cell to first of next row (or last of previous row for Shift+Tab), and returns `false` at grid boundary. Tab handler clears active cell on boundary so browser focus leaves the grid. Tab now works in both edit and non-edit modes per spec "Any" context.

**Tests:** 23 new bUnit tests (2 V07.1 + 3 V07.3 + 3 V07.5/6 + 2 V07.7 + 3 V07.8 + 2 V07.9 + 3 V07.2 + 5 Tab-wrap). Full suite: **1155/1155** ✓ (up from 1132 post-polish; independently verified).
**Files changed:** `MariloDataSheet.Editing.cs`, `MariloDataSheet.Rendering.cs`, `MariloDataSheet.razor`, `MariloDataSheet.Data.cs`, `wwwroot/js/marilo-datasheet.js`, `tests/Marilo.Tests.Unit/DataGrid/MariloDataSheetTests.cs`.
**Build:** 0 warnings, 0 errors.

### F4 scope notes (from subagent's DONE_WITH_CONCERNS)

1. **Sequence number renumbering in `.Rendering.cs`.** Because V07.7 added `aria-describedby` and V07.6 added `aria-colindex` on the `<td>` open block, existing sequence numbers (28 data-raw-value, 29 onclick, 30 content, 98 button text) collided with the new attributes. The subagent renumbered the cell attribute block (28 = aria-describedby, 29 = data-field, 30 = data-raw-value, 31 = onclick, 32 = CellTemplate content, 33-37 = sr-only error span) and the delete-button subtree (100-105) to restore monotonic ordering. Blazor tolerates non-monotonic, but this keeps the diff-algorithm path optimal and follows the F3-polish item #10 style. One unchanged existing render path, no cascade to providers or tests.
2. **JS interop path for V07.2 routed through existing `HandleKeyDown`, not a new `[JSInvokable]`.** The JS handler already forwards unhandled keys to `HandleKeyDown(key, ctrl, shift)` — single-character keys (e.g., `"a"`, `"5"`) arrive as standard keydown events. `preventDefault` added in JS only when focus is NOT inside an input/select/textarea so open editors still receive keystrokes normally. Avoids a second `[JSInvokable]` method round-trip.
3. **`_ariaAnnouncement` dirty-count test uses a SENTINEL string.** Test sets `cut.Instance._ariaAnnouncement = "SENTINEL"` between commits to detect whether a second `CommitCellEdit` fires a new announcement; if the field still reads "SENTINEL" after the second commit, the announcement was correctly suppressed (no dirty-count change). Internal field accessible via existing `InternalsVisibleTo` setup.
4. **Tab in edit mode now also routed through `MoveToNextEditableCell`.** Previously the in-edit-mode Tab branch walked all columns (including non-editable). Consolidating gives consistent behavior: Tab from an edit in a row whose next column is non-editable now skips to the next editable column rather than landing on it. This aligns with spec `keyboard-and-accessibility.md:104` ("skipping non-editable columns") — Tab has always been documented as editable-only, so this is a **bugfix to match spec**, not a behavior change the user notices negatively.
5. **One JS file touched** (`marilo-datasheet.js`). Already anticipated in the task prompt as V07.2/V07.3 requirement.

### No F4 deferrals

All 9 F4 gaps are fully implemented with tests. No BLOCKED states. No scope creep beyond the expected files.

## Batch F2-polish Implementation (Complete 2026-04-10)

Landed as two commits on `workInProgress` — a parallel commit `6322198` and my follow-up commit `d693573`. Together these resolve the two High-priority production-correctness concerns flagged in the F2 code-quality review.

- ✅ **F2P.1 — `try`/`catch`/re-throw around `OnSaveAll.InvokeAsync`.** Landed in parallel commit `6322198` (`fix(datasheet): save-path robustness + polish nits (F2 follow-up)`). The Step 5-7 block in `SaveAllAsync` is now wrapped in a try/catch that, on exception, clears `TransientState = null` for all entries that were marked `CellState.Saving`, sets an `_ariaAnnouncement = "Save failed."`, calls `StateHasChanged`, and **re-throws** so the exception still propagates to the caller per the spec's "Retry and Error Handling Guidance". Dirty state is NOT cleared on failure (spec-compliant). Test: `SaveAll_OnSaveAllThrows_ClearsTransientStateAndRethrows`.
- ✅ **F2P.2 — `_isSaving` re-entrancy guard.** Split across two commits:
  - **`6322198`** added the `_isSaving` internal field and the early-return guard at the top of `SaveAllAsync`, preventing a second `SaveAllAsync` call during the `Task.Delay(_savedStateDurationMs)` window from interleaving with the first call's Step 7 cleanup.
  - **`d693573`** (my subagent) added the matching guard at the top of `BulkResetAsync` (`MariloDataSheet.razor.cs:171-180`). BulkResetAsync writes to `_dirtyRows` too, so it was also subject to the race — now it early-returns when `_isSaving` is true with a comment explaining the race. Uses the same `_isSaving` field defined in `Data.cs:52`.

**Important naming clarification:** The internal `_isSaving` field is **separate from** the public `IsSaving` parameter. `IsSaving` is a `[Parameter]` the consumer sets to signal "I am persisting to the backend", which disables the Save All button externally. `_isSaving` is the component's internal re-entrancy guard for the `Task.Delay` dwell window. Both are needed; both serve different concerns.

**Tests:** 3 new bUnit tests total — 1 from `6322198` (OnSaveAll throws → rollback), 2 from `d693573`:

1. `SaveAll_OnSaveAllThrows_ClearsTransientStateAndRethrows` — wires `OnSaveAll` to a throwing handler, invokes `SaveAllAsync`, asserts the exception propagates AND `TransientState` is `null` on all entries AND `DirtyFields` survives (dirty state preserved for retry).
2. `SaveAll_ReentrantCall_IsNoOp` — sets `_savedStateDurationMs = 200`, kicks off two consecutive `SaveAllAsync` calls within the same dispatcher pass, asserts `OnSaveAll` was invoked exactly once and dirty state is cleared after the window closes.
3. `BulkResetAsync_WhileSaveInFlight_IsNoOp` — commits two edits, selects row[1], starts `SaveAllAsync` without awaiting, calls `BulkResetAsync()` during the `Task.Delay` window, awaits the save. Asserts row[1] is still selected, its edited value survives the reset attempt, and both rows clean up correctly after the save completes.

**Existing F2 transition tests confirmed green:** `SaveAll_DirtyCells_TransitionThroughSavingAndSaved` and `SaveAll_NonDirtyFields_StayPristineDuringSave` still pass — no regression in the F2 Saving→Saved→Pristine flow.

**Final suite:** **1158/1158** ✓ (up from 1155 post-F4: +1 from `6322198` OnSaveAll throws test, +2 from `d693573` re-entrancy tests).

**Files changed:**

- `6322198`: `MariloDataSheet.Data.cs` (try/catch + `_isSaving` field + SaveAllAsync guard + polish nits: `Debug.Fail`, using directives, numeric-parse comment), test file
- `d693573`: `MariloDataSheet.razor.cs` (BulkResetAsync guard), test file

**Build:** 0 warnings, 0 errors.

### F2-polish scope notes

1. **Parallel commit `6322198` landed while the F2-polish subagent was reading files.** The subagent adapted its scope to complete only the remaining gap (BulkResetAsync guard) rather than duplicating work. Clean cooperation between the two concurrent implementation paths.
2. **Polish nits bundled into `6322198`** include `Debug.Fail` replacement for the `Debug.Assert(false, ...)` from the polish-pass review (F4-polish item from the remaining work queue — partially resolved early), `using` directive cleanup, and a numeric-parse comment. These were slightly out of F2-polish scope but are non-blocking improvements aligned with the F4-polish/F2 minor nits queue.
3. **No naming collision.** `_isSaving` (internal, re-entrancy) and `IsSaving` (public, parameter) live side by side without ambiguity.

## Batch F4-polish + F2 minor nits — Cosmetic Cleanup (Complete 2026-04-10)

Landed via subagent-driven development (single implementer + self-review). Commit `eab6997` on `workInProgress`. **This is the final cron-tractable DataSheet batch — all feature, hardening, and cosmetic cleanup work is now complete except the deferred V03 cell range selection subsystem.**

### Items addressed (checklist by ID)

| Item ID | Source | Status | Notes |
|---|---|---|---|
| **F1.N4** | F1 review | ✅ DONE | `TryParseCellValue` restored to switch-expression with per-arm helpers (`TryParseNumberCell`, `TryParseDateCell`, `TryParseCheckboxCell`, `TryParseSelectCell`, `UnknownColumnTypeFallback`) keeping every branch on one line. |
| **F1.N5** | F1 review | ✅ DONE (partial) | `GetColumnClrType(column)` helper added; V07.2 printable-char path and paste path share it via `ParseNumberForCell`. `Rendering.cs` intentionally left untouched (out of batch scope for the rendering refactor side of the duplication). |
| **F1.N8** | F1 review | ✅ ALREADY_DONE | `RenderCheckboxSheet(bool required, List<TestRow> data)` helper landed in parallel commit `0c70a69` (row-key sanitization + F4 polish nits) before this batch started. Subagent confirmed via git diff. |
| **F1.N9** | F1 review | ⏭ SKIPPED | Generic-to-non-generic `DataSheetNumericParser` refactor ripples into the test helper's reach-through logic — too broad for a cosmetic pass. Deferred. |
| **F3.N6** | F3 review | ✅ ALREADY_DONE | 6 F3 tests migrated from `EnterEditMode + HandleKeyDown("Escape")` to `ActivateCell` in commit `0c70a69`. |
| **F3.N7** | F3 review | ⏭ SKIPPED | `BuildSheet(params …)` helper would rewrite 13 F3 tests — too churny for a cosmetic pass. Explicit skip per the batch's scope-discipline rules. |
| **F2.M1** | F2 review | ✅ DONE | `RestoreEntryOrRemoveNewRow(DirtyRowEntry<TItem>)` helper on `Data.cs`. `ResetAsync` (Data.cs) and `BulkResetAsync` (razor.cs) both route through it. Callers still own their own `_dirtyRows` cleanup because the cleanup shapes differ (bulk `Clear()` vs per-key `Remove`). |
| **F2.M2** | F2 review | ✅ DONE | Inline comment at `AddRowAsync` seed loop pointing at the `!entry.IsNewlyAdded` guard in `CommitCellEdit`, documenting the cross-file invariant so future changes don't break the contract silently. |
| **F2.M3** | F2 review | ✅ DONE | Added `<remarks>` block on `SaveAllAsync` XML doc noting the `DeepClone`-on-save requirement and the `TItem` deep-cloneability contract. **No try/catch fallback** — behavior-changing and potentially wrong, explicitly out of scope. |
| **F2.M4** | F2 review | ⏭ SKIPPED | `SavedIndicatorDuration` public parameter is an API surface change — belongs in Stage 03 resolution design, not a cosmetic cleanup batch. Documented as deferred. |
| **F2.M5** | F2 review | ✅ DONE | Deleted-keys collection in `SaveAllAsync` Step 6 replaced with a single `Where().Select().ToHashSet()` LINQ pass. Two-loop structure preserved for the removal phase with an explanatory comment — the two-loop exists because mutating `_dirtyRows` during iteration is not allowed. |
| **F2.M6** | F2 review | ⏭ SKIPPED | `CaptureDuringSave<T>` helper-for-possible-future-use is speculative. |
| **P.N2** | Polish review | ⚠ DONE_WITH_CONCERNS | Initially attempted the `CultureInfo.DefaultThreadCurrentCulture` fix — **it made things worse.** `de-DE` leaked into the parallel-running `DatePickerTests.DatePicker_NavigateMonths` (xUnit default in-assembly parallelism means thread-pool threads are shared across test classes), failing that test (1158/1159). Reverted to `CurrentCulture`-only and added an explanatory comment preempting a future attempt. **The real fix requires a `[Collection("NoParallel")]` annotation or an `AssemblyFixture`** to serialise culture-sensitive tests across classes — a broader test-infrastructure change than a cosmetic pass warrants. Flagged for a dedicated follow-up if xUnit test-parallelism is ever actively considered. |
| **P.N3** | Polish review | ✅ ALREADY_DONE | `using System.Globalization;` present at top of the test file; no fully-qualified `System.Globalization.CultureInfo` references remain. Landed in earlier polish work. |
| **P.N4** | Polish review | ✅ ALREADY_DONE | `ParseNumericValue` already has the 6-line comment block explaining the `CurrentCulture` vs `InvariantCulture` split (user-typed input is culture-sensitive; pasted dates come from code-formatted raw values). |

**Summary:** 8 items DONE, 4 ALREADY_DONE (landed in parallel commit `0c70a69` before this batch), 3 SKIPPED (correctly scoped out), 1 DONE_WITH_CONCERNS (P.N2 — real trap, explanatory comment added as the safe resolution).

### Stats

- **Net code changes:** 4 files changed, 148 insertions, 84 deletions
  - `MariloDataSheet.Data.cs` — +83/-47 (F2.M1 helper + ResetAsync rewrite, F2.M3 doc, F2.M5 LINQ)
  - `MariloDataSheet.Editing.cs` — +77/-27 (F1.N4 switch-expression + helpers, F1.N5 GetColumnClrType)
  - `MariloDataSheet.razor.cs` — +8/-16 (F2.M1 BulkResetAsync simplification, F2.M2 invariant comment)
  - `tests/Marilo.Tests.Unit/DataGrid/MariloDataSheetTests.cs` — +7/-0 (P.N2 explanatory comment only)
- **New tests:** 0 (cosmetic batch; existing 1159 tests cover the refactors; all passed on first run)
- **Final suite:** **1159/1159** passing (no delta; subagent caught that the prior-fire baseline of 1158 was stale because parallel commit `0c70a69` had added one test before this batch started)
- **Build:** 0 warnings, 0 errors

### Key findings from the cosmetic pass

1. **Parallel commit `0c70a69` discovered.** A sister commit `fix(datasheet): row-key sanitization + F4 polish nits` landed externally between F2-polish and this batch. It absorbed F1.N8 (RenderCheckboxSheet helper) and F3.N6 (ActivateCell migration) with the exact expected comment tags, plus an **additional bug fix** (row-key sanitization for special characters in aria-describedby IDs) that included a new test `InvalidCell_WithRowKeyContainingSpecialChars_ProducesValidDescribedbyId`. This is why the true test baseline for this batch was 1159, not 1158 as my fire #21 report stated.

2. **P.N2 is a real cross-test-class trap.** The obvious `DefaultThreadCurrentCulture` fix leaks across xUnit test classes because in-assembly parallelism is enabled and thread-pool threads are shared. The safer fix (test collection annotation or fixture) is broader test-infrastructure work. **The explanatory comment I added should prevent the next session from repeating the failed attempt.**

3. **Scope discipline held.** 4 files touched (the exact 4 in-scope files: Data.cs, Editing.cs, razor.cs, test file). No JS, no Rendering.cs, no new files. 8 items addressed, 4 already-done skipped cleanly, 3 explicitly deferred with reasons.

## Remaining DataSheet work (deferred batches)

| Batch | Gaps | Effort | Priority |
| --- | --- | --- | --- |
| ~~**F1** — Checkbox Required, Number parse, dirty revert~~ | ~~V01.1/V01.2/V02.1~~ | Small | ✅ **COMPLETE (commit `6794644`, 9 tests)** |
| ~~**F3** — Paste hardening~~ | ~~V04.1/V04.2/V04.3/V04.4~~ | Small | ✅ **COMPLETE (commit `3071c39`, 9 tests)** |
| ~~**F2** — Save/Reset lifecycle~~ | ~~V02.2/V05.1/V05.2/V05.3/V05.4/V05.5~~ | Medium | ✅ **COMPLETE (commit `7a40055`, 12 tests)** |
| ~~**Polish pass** — F1+F3 review nits~~ | ~~Date culture, narrow catches, readability, ordinal compare, ten items~~ | Small | ✅ **COMPLETE (commit `7b5e217` + items folded into F2, 1 regression test)** |
| ~~**F4** — Accessibility~~ | ~~V07.1/V07.2/V07.3/V07.5/V07.6/V07.7/V07.8/V07.9/Tab-wrap~~ | Medium | ✅ **COMPLETE (commit `95f7f17`, 23 tests)** |
| ~~**F2-polish** — Save-lifecycle production hardening~~ | ~~try/catch around OnSaveAll.InvokeAsync; `_isSaving` re-entrancy guard on SaveAllAsync + BulkResetAsync~~ | Small | ✅ **COMPLETE (commits `6322198` + `d693573`, 3 tests)** |
| ~~**Row-key sanitization + F4 polish nits**~~ | ~~Row-key special-char escaping for aria-describedby IDs + F1.N8 (RenderCheckboxSheet helper) + F3.N6 (ActivateCell migration)~~ | Small | ✅ **COMPLETE (commit `0c70a69`, 1 new test)** |
| ~~**F4-polish + F2 minor nits** — cosmetic cleanup~~ | ~~F1.N4 switch-expression restoration + per-arm helpers; F1.N5 GetColumnClrType helper; F2.M1 RestoreEntryOrRemoveNewRow helper; F2.M2 invariant comment; F2.M3 DeepClone doc comment; F2.M5 LINQ simplification; P.N2 culture-trap explanatory comment~~ | Small | ✅ **COMPLETE (commit `eab6997`, 0 new tests, 8 items DONE / 4 ALREADY_DONE / 3 SKIPPED / 1 DONE_WITH_CONCERNS)** |
| **V03** | Cell range selection (full feature subsystem) | ~3.5 days, 27 tests | Critical but deferred — Stage 03 design doc complete at `datasheet-gap-analysis/stages/03-resolution-design/output/gap-datasheet-v03-selection-ranges-resolution.md` |

**Status:** **ALL DataSheet feature, hardening, and cosmetic cleanup batches are now complete.** The component is code-complete at every level except the deferred V03 cell range selection subsystem. Full suite 1097 → **1159** (+62 total). **No remaining cron-tractable work exists for DataSheet** — V03 is a 3.5-day standalone build that needs deliberate session time, not autonomous cron dispatch.

**Items skipped / deferred** during the cosmetic cleanup:

- **F1.N9** (DataSheetNumericParser non-generic extraction) — too broad for a cosmetic pass; ripples into test helper reach-through logic.
- **F3.N7** (BuildSheet test helper to cut ~60% of test LOC) — would rewrite 13 F3 tests; correct scope-discipline skip.
- **F2.M4** (SavedIndicatorDuration public parameter) — API surface change that should go through Stage 03 resolution design.
- **F2.M6** (CaptureDuringSave test helper) — speculative helper for possible future tests.
- **P.N2** (CultureInfo.DefaultThreadCurrentCulture) — real trap; leaks across test classes due to xUnit in-assembly parallelism. Explanatory comment added to preempt future attempts. The real fix requires `[Collection("NoParallel")]` or `AssemblyFixture` — broader test-infrastructure work.

**Known outstanding bug finds still deferred** (not in any cron-tractable batch):

- **V03-07** (Ctrl+D Fill Down uses wrong scope — row selection instead of cell range) — part of the V03 cell range selection subsystem, deferred with V03.
- **V01-01** (Tab navigation filter includes Computed columns) — partially addressed in F4's Tab-wrap fix (Tab now correctly skips Computed via `MoveToNextEditableCell`), so the original concern is resolved in practice.

Next loop iteration has no more DataSheet work to do. The cron's DataSheet autonomous work pool is **fully drained**.

## Architecture Decision — Resolved 2026-04-09

The blocker from the 2026-04-03 Stage 01 audit ("Spec-Implementation Architecture Mismatch — is MariloDataSheet intended to become the full Spreadsheet, or is it a deliberately simpler component that needs its own spec?") was resolved by the 2026-04-09 human decisions session:

> **"DataSheet: True spreadsheet with its own architecture (not DataGrid reuse)"**
> — per `ICM/workspaces/gap-analysis-resolution/_status/workspace-status.md:112` "Human Decisions Resolved (2026-04-09)"

**Outcome:** Two components are now tracked as distinct deliverables:

- **`MariloSpreadsheet`** (Editors folder) — Excel-clone direction, spec at `docs/component-specs/spreadsheet/` (5 files: overview, events, functions-formulas, tools, accessibility/wai-aria-support). Tracked in the main plan's API Completion Pass table (line 59) as `COMPLETE | PRESENT_PARTIAL | Editable grid; formula engine deferred`.
- **`MariloDataSheet<TItem>`** (DataGrid folder) — strongly-typed editable data grid, spec at `docs/component-specs/datasheet/` (9 files: overview, columns-and-schema, editing-and-validation, selection-and-ranges, bulk-paste-and-clipboard, bulk-operations-and-saveall, virtualization-and-performance, keyboard-and-accessibility, theming-and-css-provider). **Owned by this delivery workspace.**

## Stage 01 — Spec Review

- **Status:** ✅ **Complete (re-run 2026-04-10)**
- **Current output:** `stages/01-spec-review/output/datasheet-spec-gaps-2026-04-10.md` — new audit against the correct spec
- **Superseded output:** `stages/01-spec-review/output/datasheet-spec-gaps.md` (2026-04-03) — marked with SUPERSEDED banner; retained for historical value
- **Headline finding:** Broad-surface alignment is excellent. 53 distinct API elements verified present (18 parameters + 11 column params + 6 event args + 2 enums + 9 methods + 7 CSS provider methods). Demo exists at `samples/Marilo.Demo/Pages/Components/DataSheet/Overview.razor` (205-line "Investment Position Editor"). **Zero confirmed missing features.**
- **Remaining:** 10 Stage 01b verification sub-tasks (GAP-DATASHEET-V01 through -V10) against the 8 feature-area detail spec files + the 2 enum value lists. Each is a small independent audit. V03 (cell range selection) has the highest probability of surfacing a real implementation gap.

## Current DataSheet Source Footprint

1,324 lines across 7 files in `src/Marilo.Components/DataGrid/`:

- `MariloDataSheet.razor` (166 lines — markup)
- `MariloDataSheet.razor.cs` (191 lines — parameters, state, lifecycle)
- `MariloDataSheet.Data.cs` (287 lines — dirty tracking, row key resolution)
- `MariloDataSheet.Editing.cs` (294 lines — cell editing, paste handling)
- `MariloDataSheet.Interop.cs` (87 lines — clipboard JS interop)
- `MariloDataSheet.Rendering.cs` (244 lines — RenderTreeBuilder)
- `MariloDataSheetColumn.razor` (55 lines — column child component)

Architecturally consistent with the DataGrid partial-class pattern (`MariloDataGrid.Data.cs`, `.Editing.cs`, `.Interop.cs`, `.Rendering.cs`).

## Key Open Issues (post-2026-04-09)

1. ~~Resolve architecture direction: Spreadsheet vs DataSheet~~ → **RESOLVED 2026-04-09:** both tracked as distinct components
2. ~~If DataSheet: write new spec for actual API surface~~ → **RESOLVED:** new spec at `docs/component-specs/datasheet/` (9 files)
3. ~~If Spreadsheet: plan phased XLSX engine implementation~~ → **DEFERRED:** MariloSpreadsheet formula engine tracked as deferred work in main plan's API Completion Pass table
4. **Re-run Stage 01 spec review** against the new `docs/component-specs/datasheet/` spec to produce a superseding gaps file
5. **Sanity-check the 9 new spec files** against the current 1,324-line source for coverage (every documented feature in spec has an implementation touch point; every non-trivial implementation surface has a spec reference)

## Next Trigger

Run Stage 01b — execute the 10 verification sub-tasks from `datasheet-spec-gaps-2026-04-10.md`. Each sub-task is an independent small session. The highest-priority one is **V03 (cell range selection)** — the source has row selection but cell range selection (shift-click, drag-select, Ctrl-click) is unverified and likely to surface a real gap. V07 (keyboard and accessibility) is a close second for thoroughness. The other eight are lower-risk verification passes that can run in parallel.

**Skipping Stage 02 prioritization** is recommended because the gap surface is already tiny (10 verification sub-tasks vs Scheduler's 32 or TreeList's 43 implementation gaps). The effective pipeline becomes: `01 → 01b → 03 → 05 → 06`.
