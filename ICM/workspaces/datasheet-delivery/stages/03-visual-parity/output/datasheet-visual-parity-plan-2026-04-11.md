# DataSheet Stage 03 — Visual Parity Plan

**Plan date:** 2026-04-11
**Component:** MariloDataSheet<TItem>
**Upstream:** Stage 02 complete (4 demo pages landed); all F-series feature batches closed

## Stage 03 Contract Summary

Stage 03 (Visual Parity) compares MariloDataSheet rendering against the internal Marilo delivery-quality baseline across three CSS providers (FluentUI, Bootstrap, Material) in both light and dark modes. This stage owns visual capture, scoring against a structured rubric (0–3 scale), and gap documentation. It does NOT own remediation — gaps are handed off to the gap-analysis workspace for implementation. Deliverables include screenshots, gap records, and a parity summary with per-theme/mode scores.

## Scope — Providers and Pages to Capture

### Providers

All three CSS providers have DataSheet method implementations:

| Provider | Status | Methods |
| --- | --- | --- |
| FluentUI | Ready | DataSheetClass, DataSheetCellClass, DataSheetHeaderCellClass, DataSheetRowClass, DataSheetToolbarClass, DataSheetBulkBarClass, DataSheetSaveFooterClass |
| Bootstrap | Ready | Same 7 methods |
| Material | Ready | Same 7 methods |

**Note:** All providers implement the 7 required DataSheet CSS methods (lines 167–173 in `IMariloCssProvider.cs`). No provider gaps detected.

### Demo pages

Four demo pages exercise distinct DataSheet feature areas:

1. **Overview.razor** — Investment position editor with full feature set (add row, delete, paste, validation, saving, event logging)
2. **BulkOperations.razor** — Add row, delete/undo, bulk clipboard paste with type coercion, virtualization scenarios
3. **Editing-and-Validation.razor** — Required field validation, column-level validators, cross-row validation, loading/reset state transitions
4. **Keyboard-and-Accessibility.razor** — Tab/Shift+Tab/arrow navigation, F2 enter/Escape cancel, screen-reader support, key observer log

### Capture matrix

Per the Stage 03 `capture-matrix.md`:

**Theme/Mode combinations:** 6 (Fluent Light, Fluent Dark, Bootstrap Light, Bootstrap Dark, Material Light, Material Dark)

**Primary states (must capture):** 3

- Cell grid default
- Selected cell
- Cell editing (inline input active)

**Secondary states (should capture):** 3

- Column/row headers
- Frozen rows/columns
- Cell range selection

**Edge states (nice-to-have):** 3

- Formula bar (not yet implemented in DataSheet)
- Sheet tabs (not yet implemented)
- Empty sheet

**Total baseline capture points:** 9 states × 6 theme/modes = **54 screenshots**

**Viewports:** Desktop 1280×900 (primary); Narrow 768×900 (deferred — not configured in `playwright.config.ts` yet)

## Capture Strategy

**Recommendation: Use existing Playwright infrastructure.**

Evidence:

- `tests/visual-parity/playwright.config.ts` exists and is fully configured.
- Config targets Chromium, Desktop viewport 1280×900, with animations disabled (best for screenshot stability).
- Web server auto-start: `dotnet run --project ../../samples/Marilo.Demo` with port 5301 and `reuseExistingServer: true`.
- Snapshot baselines stored in `./baselines/` directory alongside specs.
- Playwright is initialized with Fluent/Bootstrap/Material theme switching via demo app state.

**Action:** Use Playwright-based capture. The demo app's theme switcher (available in Marilo.Demo) allows switching between providers and light/dark mode without rebuilding. Create or extend `tests/visual-parity/specs/datasheet.spec.ts` to iterate over all six theme/mode combinations and capture all nine state scenarios per the matrix.

**Implementation approach:**

1. For each theme/mode combination, navigate demo app to set theme and mode.
2. For each primary state, interact with `Overview.razor` and capture screenshot at that state.
3. For each secondary state, navigate to appropriate demo page and capture.
4. Store baselines as: `{snapshotDir}/datasheet/{theme}-{mode}/{viewport}/{scenario}.png`

**Estimated effort:** 2–3 hours for spec + captures; mature baseline by end of first pass.

## Visual States to Capture per Page

### Overview.razor

- **Cell grid default** — grid at rest, header row + column visible, sample data (6+ rows), idle state
- **Selected cell** — single cell highlighted with selection border (e.g., Ticker column, row 2)
- **Cell editing** — cell with inline text input active (caret visible, input chrome visible)
- **Column/row headers** — header styling, background color, border weight, text alignment
- **Dirty row** — row with unsaved changes (visual indicator, e.g., row highlight or marker)
- **Invalid cell** — cell with validation error state (e.g., negative Quantity, missing Ticker)
- **Empty state** — grid with no data, "No positions. Click + Add Row to begin." message

### BulkOperations.razor

- **Add Row state** — new blank row inserted, placeholders visible
- **Deleted row state** — row marked for deletion, strikethrough applied, grayed out
- **Paste in-progress** — bulk paste from clipboard with error feedback visible

### Editing-and-Validation.razor

- **Required field error** — cell with red/error border, validation message inline or tooltip
- **Column validator error** — cell rejecting negative value or out-of-range date
- **Loading state** — grid with `IsLoading="true"`, skeleton or disabled overlay visible

### Keyboard-and-Accessibility.razor

- **Focus ring on cell** — keyboard focus indicator visible on currently focused cell
- **Edit mode (F2)** — cell in edit mode (same as Overview edit state, but triggered via F2)

## Rubric

Per Stage 03 `parity-score-rubric.md`:

| Score | Label | Definition |
| --- | --- | --- |
| 0 | Materially different | Layout, structure, or behavior does not match; immediately noticeable |
| 1 | Noticeably off | Correct structure but wrong tokens, spacing, or sizing; spotted within seconds |
| 2 | Close but visible | Minor deviations in spacing, color shade, or typography; spotted on side-by-side |
| 3 | Visually equivalent | No meaningful difference at normal inspection distance |

**Severity mapping:**

- **Critical** (must fix before delivery): Score 0–1 on primary states (cell grid, selected cell, editing)
- **Major** (should fix this phase): Score 0–1 on secondary states OR score 2 on primary states
- **Minor** (fix if time): Score 2 on secondary states
- **Polish** (backlog): Score 2 on edge states

**Mismatch classification:**

- Token-level: Wrong CSS custom property or missing dark-mode override → SCSS foundation file
- Component-level: Wrong CSS rule/selector in component SCSS → Component SCSS file
- Demo: Correct render but wrong page setup → Demo page update
- Missing state: State not implemented at all → Gap-analysis intake

## Gap-Format Template

Per Stage 03 `visual-parity-gap-format.md` (abbreviated):

```text
**ID:** VP-datasheet-[sequence]
**Component:** MariloDataSheet
**Theme:** Fluent | Bootstrap | Material
**Mode:** Light | Dark
**State/Scenario:** [e.g., cell grid default, selected cell, editing]
**Parity Score:** 0 | 1 | 2 | 3
**Severity:** critical | major | minor | polish

| Field | Observed in Marilo | Expected (Baseline) |
| --- | --- | --- |
| Description | [what renders] | [what baseline expects] |
| Likely cause | [token mismatch, layout, SCSS rule, etc.] | |

**Category:** cell border weight | header background | selection highlight | editing input chrome | frozen separator | sheet tab styling | token/color | typography | spacing | layout | density

**Recommended change:** [specific CSS/token adjustment]
**Acceptance criteria:** [what "fixed" looks like]
**Remediation target:** SCSS source | demo update | gap-analysis intake
```

## Deliverables

1. **Screenshot baselines** — `tests/visual-parity/baselines/datasheet/{theme}-{mode}/{viewport}/{scenario}.png` (54 files total if all edge states land; ~36 for primary+secondary only)
2. **Gap list** — `ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/datasheet-visual-parity-gaps.md` (one gap record per score < 3)
3. **Parity summary** — `ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/datasheet-parity-summary.md` (overall scores, coverage table, blockers)
4. **Playwright spec** — `tests/visual-parity/specs/datasheet.spec.ts` (new or extended with DataSheet capture logic)

## Open Questions

1. **Formula bar & sheet tabs:** The capture matrix references these edge states, but DataSheet does not implement them. Should Stage 03 skip those states or document as "not yet implemented" and defer capture?
   - **Recommendation:** Skip in Stage 03. Focus on the 9 states available now (cell grid, selected cell, editing, headers, frozen, range, empty). Add formula bar and tabs when implemented.
2. **Cell range selection (V03 deferral):** The plan notes state "V03 cell range selection (deferred)". Is range selection implemented enough to capture, or should we skip it?
   - **Recommendation:** Check current implementation status. If range selection is present but incomplete, capture at "partially visible" state. If absent, skip and note in parity summary.
3. **Dark mode theme switcher:** Does the demo app have a built-in dark mode toggle, or must the Playwright test emulate CSS dark mode? (Playwright supports `emulate({ colorScheme: 'dark' })`.)
   - **Recommendation:** Check Marilo.Demo app structure. If no built-in switcher, use Playwright's `emulate` in the spec to toggle dark mode per test.
4. **Frozen columns/rows:** Are frozen rows/columns actually implemented in DataSheet, or is this a placeholder state?
   - **Recommendation:** Verify implementation status. If not implemented, skip and note as "deferred to future phase."
5. **Narrow viewport (768×900):** The `playwright.config.ts` includes a commented-out narrow project. Should Stage 03 capture both desktop and narrow, or desktop-only for now?
   - **Recommendation:** Desktop-only for Stage 03. Uncomment narrow project and add to Stage 04 future work if narrow viewport parity is a delivery requirement.

## Recommendation

**Stage 03 should proceed immediately.** All prerequisites are in place:

- Four demo pages are implemented and stable (Stage 02 complete).
- Playwright infrastructure exists and is correctly configured.
- CSS provider implementations are complete (all 7 DataSheet methods present across all three themes).
- Capture matrix is defined and specific.
- Rubric and gap format are documented.

**Start with primary states only** (cell grid default, selected cell, editing) across all 6 theme/modes (18 screenshots). Capture secondaries after validating primary baselines. Edge states (formula bar, sheet tabs) are deferred pending implementation status clarification.

**Expected timeline:** 1–2 iterations of the loop to baseline primary states + score, classify gaps, and write parity summary. Remediation handoff to gap-analysis workspace follows Stage 03 output.

**Blocking condition:** Answer the open questions above (especially formula bar/sheet tabs and frozen column/row status) before commencing captures. If both are not implemented, reduce total capture points from 54 to ~36 (6 theme/modes × 6 states).

## Implementation sequencing for future iterations

Given the 54-screenshot target and the orchestrator's tight per-iteration context budget, Stage 03 implementation should split into sub-batches:

- **03a — Playwright spec + primary state captures** (18 screenshots across 6 theme/modes × 3 primary states). One iteration.
- **03b — Secondary state captures** (18 screenshots across 6 theme/modes × 3 secondary states), with frozen/range states conditional on implementation status. One iteration.
- **03c — Scoring, gap classification, parity summary** — pure analysis, no new captures. One iteration.
- **03d — Gap remediation handoff** to gap-analysis workspace (if any score < 3 gaps surface). One iteration.

Total: ~4 iterations to complete Stage 03. Each iteration has a bounded scope suitable for a focused implementer subagent dispatch.
