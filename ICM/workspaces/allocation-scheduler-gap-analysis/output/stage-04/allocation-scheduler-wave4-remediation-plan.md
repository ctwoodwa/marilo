# Wave 4 Remediation Plan: MariloAllocationScheduler — Second-Cycle Atomic Tasks

**Stage:** 04-remediation-plan (second cycle)
**Date:** 2026-04-11
**Input:** `output/stage-03/allocation-scheduler-wave4-resolution-designs.md` (9 in-workspace lanes, review PASS)
**Worker:** `w-allocation-scheduler-gap-analysis`
**Session:** marilo-grid-pipeline-2026-04-11-1200

---

## Scope

9 in-workspace R-lanes converted to 12 atomic tasks across 4 dispatch waves (B, C, D-source, D-spec).

**Excluded (not this worker's scope):** R5/R6 (cross-component, orchestrator-owned), R9 (locked out, Material tracker).

**Ordering rationale:** Follows the Phase B > C > D sequencing from S02 priority-lanes. Within each phase, tasks are independently parallelizable (disjoint files_owned confirmed in S03). R1 dual-path design means R1 proceeds independently regardless of R5 status.

---

## Wave B — Immediate Work (no external deps)

### Task B-01: R2 — Hidden-Scrollbar A11y Fix (Fluent)

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-B01` |
| **R-Lane** | R2 |
| **Description** | Replace `scrollbar-width: none` and `::-webkit-scrollbar { display: none }` on `.mar-allocation-scheduler__resource-panel` with thin visible scrollbar using design tokens. Fluent provider file. |
| **files_owned** | `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` (selector: `&__resource-panel` scrollbar rules only) |
| **Acceptance** | (1) `scrollbar-width: thin` replaces `scrollbar-width: none`. (2) `scrollbar-color` uses `var(--marilo-color-border, #d1d1d1) transparent`. (3) WebKit fallback provides 6px thumb with `--marilo-color-border` and `border-radius: 3px`. (4) Hidden scrollbar CSS fully removed. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Visual check: resource panel scrollbar visible in Fluent light and dark modes. |
| **Wave** | B |
| **Effort** | Low (~10 lines changed) |
| **Parallel with** | B-02, B-03, B-04, B-05 |

### Task B-02: R2 — Hidden-Scrollbar A11y Fix (Bootstrap)

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-B02` |
| **R-Lane** | R2 |
| **Description** | Same scrollbar fix as B-01 but in Bootstrap bridge file. Replace hidden scrollbar with thin visible scrollbar using `var(--bs-border-color)` tokens. |
| **files_owned** | `src/Marilo.Providers.Bootstrap/Styles/_bridge-allocation-scheduler.scss` (selector: `&__resource-panel` scrollbar rules only) |
| **Acceptance** | (1) `scrollbar-width: thin` replaces `scrollbar-width: none`. (2) `scrollbar-color` uses `var(--bs-border-color)`. (3) WebKit fallback with `--bs-border-color` thumb. (4) Hidden scrollbar CSS fully removed. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Visual check: resource panel scrollbar visible in Bootstrap light and dark modes. |
| **Wave** | B |
| **Effort** | Low (~10 lines changed) |
| **Parallel with** | B-01, B-03, B-04, B-05 |

### Task B-03: R3 — AccessibilityDemo.razor

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-B03` |
| **R-Lane** | R3 |
| **Description** | Create new demo page at `samples/Marilo.Demo/Pages/Components/AllocationScheduler/AccessibilityDemo.razor`. Sections: keyboard navigation walkthrough, ARIA roles/landmarks, screen-reader live region, high-contrast mode. Follow existing `AdvancedFeatures.razor` layout pattern (`<PageSection>` + `<DemoSection>`). |
| **files_owned** | `samples/Marilo.Demo/Pages/Components/AllocationScheduler/AccessibilityDemo.razor` (new file) |
| **Acceptance** | (1) Page routable at `/components/allocation-scheduler/accessibility`. (2) Contains 4 demo sections (keyboard, ARIA, live-region, high-contrast). (3) Uses `ComponentDemoLayout`. (4) Follows `<PageSection>` + `<DemoSection>` pattern from sibling demos. (5) Builds without error. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Page loads in browser at the route. |
| **Wave** | B |
| **Effort** | Low-medium (~80-120 lines, new file) |
| **Parallel with** | B-01, B-02, B-04, B-05 |

### Task B-04: R4 — ThemingDemo.razor

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-B04` |
| **R-Lane** | R4 |
| **Description** | Create new demo page at `samples/Marilo.Demo/Pages/Components/AllocationScheduler/ThemingDemo.razor`. Sections: dark/light toggle, provider swap (FluentUI vs Bootstrap), custom token overrides. Follow existing demo layout pattern. |
| **files_owned** | `samples/Marilo.Demo/Pages/Components/AllocationScheduler/ThemingDemo.razor` (new file) |
| **Acceptance** | (1) Page routable at `/components/allocation-scheduler/theming`. (2) Contains 3 demo sections (dark/light toggle, provider swap, custom tokens). (3) Uses `ComponentDemoLayout`. (4) Follows `<PageSection>` + `<DemoSection>` pattern. (5) Builds without error. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Page loads in browser at the route. |
| **Wave** | B |
| **Effort** | Low-medium (~80-120 lines, new file) |
| **Parallel with** | B-01, B-02, B-03, B-05 |

### Task B-05: R8 — Spec Re-Audit Batch (12 Records)

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-B05` |
| **R-Lane** | R8 |
| **Description** | Apply 12 spec-text corrections across `docs/component-specs/allocation-scheduler/` files. Records SPEC-AS-W1-010 through SPEC-AS-W1-021. Covers: type-name mismatches (`AllocationScenarioStatus` to `ScenarioStatus`), undocumented events/parameters, deprecation notes, missing CSS class docs, dark-theme token table. |
| **files_owned** | `docs/component-specs/allocation-scheduler/scenario-planning.md`, `docs/component-specs/allocation-scheduler/events.md`, `docs/component-specs/allocation-scheduler/editing.md`, `docs/component-specs/allocation-scheduler/overview.md`, `docs/component-specs/allocation-scheduler/templates.md`, `docs/component-specs/allocation-scheduler/splitter-layout.md`, `docs/component-specs/allocation-scheduler/theming.md` |
| **Acceptance** | Per-record acceptance: |
| | (010) All `AllocationScenarioStatus` references replaced with `ScenarioStatus` in `scenario-planning.md`. |
| | (011) `OnScenarioStatusChanged` args type corrected to `ScenarioStatusChangedArgs` in `events.md`. |
| | (012) `OnTimeColumnResized` event + `TimeColumnResizedArgs` payload documented in `editing.md`. |
| | (013) `DefaultRangeLength` marked deprecated in `overview.md` with migration note. |
| | (014) Drag-fill wording in `editing.md` updated to match current source (inset box-shadow). Mark "pending R10 update" if R10 not yet landed. |
| | (015) `ShowJumpToDate` parameter added to Parameters table in `overview.md`. |
| | (016) Grouped headers (`&__header-group-row`, `&__header-group-cell`) section added to `overview.md` or `templates.md`. |
| | (017) `&__col-current` CSS class documented in `overview.md`. |
| | (018) Dynamic column fill (`calc(100%/N)`) documented in `editing.md` layout/sizing section. |
| | (019) Splitter restore zones + pane collapse documented in `splitter-layout.md`. |
| | (020) `DisplayLabel` override verified in `scenario-planning.md`; closed if already documented. |
| | (021) Dark theme token table added to `theming.md`. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0 (spec files are not compiled, but verify no broken cross-references). |
| **Wave** | B |
| **Effort** | Medium (12 records across 6-7 files, text-only) |
| **Parallel with** | B-01, B-02, B-03, B-04 |

**Wave B Gate:** All 5 tasks complete or review-pending. R5 status confirmed by orchestrator (landed or deferred with R1 dispatch-decision recorded).

---

## Wave C — P1 Source Change (after Wave B gate)

### Task C-01: R1 — Dark-Mode Invisible Cell-Edit Text (Fluent)

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-C01` |
| **R-Lane** | R1 |
| **Description** | Add `color: var(--marilo-color-text, #323130);` to the cell-edit input rule (selector `&__cell--editing input`) in Fluent SCSS. This is the canonical path (Path B from S03 dual-path analysis). R1 is NOT absorbed by R5 — the `#fff` sweep addresses `background` tokens, not `color` declarations. |
| **files_owned** | `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` (selector: `&__cell--editing input` only) |
| **Acceptance** | (1) `color: var(--marilo-color-text, #323130);` present in `&__cell--editing input` rule. (2) Light-mode fallback `#323130` matches Fluent neutral primary text. (3) Dark-mode renders readable text (light text on dark surface). (4) No other properties in the rule changed. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Visual check: cell-edit input text visible in Fluent dark mode. |
| **Wave** | C |
| **Effort** | Low (1 line addition) |
| **Parallel with** | none (sole Wave C task) |
| **Dual-path note** | If R5 has landed: verify `&__cell--editing input` still lacks explicit `color:`. If color was somehow added by R5 (not expected), mark R1 absorbed. If R6 (SCSS dedup) has NOT landed: also apply to root-level `src/Marilo.Providers.FluentUI/Styles/_allocation-scheduler.scss` to prevent divergence. |

**Wave C Gate:** R1 complete or confirmed absorbed. All P1 source lanes done.

---

## Wave D — P2 Polish (after Wave C gate)

### Task D-01: R7 — Conflict Indicator Icon + ARIA Label (Source)

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-D01` |
| **R-Lane** | R7 |
| **Description** | Replace raw glyph conflict indicator in `MariloAllocationScheduler.razor` with `<MariloIcon Name="Warning">` component. Add `Color="var(--marilo-color-danger)"`, `Size="14"`, and `aria-label="Conflict: over-allocated"` (or localization key). |
| **files_owned** | `src/Marilo.Components/DataDisplay/AllocationScheduler/MariloAllocationScheduler.razor` (conflict indicator markup only) |
| **Acceptance** | (1) Raw glyph character removed. (2) `<MariloIcon>` used with `Name="Warning"`. (3) `aria-label` present on the icon element. (4) `Class="mar-allocation-scheduler__conflict-icon"` preserved for styling hook. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. bUnit test confirms `aria-label` renders on conflict indicator element. |
| **Wave** | D |
| **Effort** | Medium (source change + test) |
| **Parallel with** | D-02, D-03, D-04, D-05, D-06, D-07 |

### Task D-02: R7 — Conflict Indicator SCSS

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-D02` |
| **R-Lane** | R7 |
| **Description** | Add or update `&__conflict-icon` SCSS rule in Fluent provider. Set `color: var(--marilo-color-danger, #bc2f32)`, `vertical-align: middle`, `margin-left: 0.25rem`. |
| **files_owned** | `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` (selector: `&__conflict-icon` only) |
| **Acceptance** | (1) `&__conflict-icon` rule exists with danger-color token. (2) Vertical alignment and spacing set. (3) No other selectors modified. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. |
| **Wave** | D |
| **Effort** | Low (3-4 SCSS lines) |
| **Parallel with** | D-01, D-03, D-04, D-05, D-06, D-07 |

### Task D-03: R7 — Conflict Indicator Spec + Demo Update

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-D03` |
| **R-Lane** | R7 |
| **Description** | Add paragraph to `docs/component-specs/allocation-scheduler/editing.md` (or `selection.md`) documenting that conflict-state cells display `MariloIcon` with `aria-label`. Verify existing demo (e.g. `AdvancedFeatures.razor` or `SelectionAndEditing.razor`) includes a conflict scenario. |
| **files_owned** | `docs/component-specs/allocation-scheduler/editing.md` (conflict icon section only), `docs/component-specs/allocation-scheduler/selection.md` (conflict icon section only) |
| **Acceptance** | (1) Spec paragraph describes MariloIcon usage for conflict indicator. (2) `aria-label` behavior documented. (3) Existing demo verified to show conflict scenario (or note filed if demo needs separate task). |
| **Build verification** | `dotnet build Marilo.slnx` exit 0 (spec files not compiled). |
| **Wave** | D |
| **Effort** | Low (text additions) |
| **Parallel with** | D-01, D-02, D-04, D-05, D-06, D-07 |

### Task D-04: R10 — Drag-Fill Dashed Outline (Fluent)

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-D04` |
| **R-Lane** | R10 |
| **Description** | Replace solid fill + `box-shadow` on `&__cell--drag-target` with dashed outline. Set `background` to 6% primary tint, `outline: 2px dashed var(--marilo-color-primary)`, `outline-offset: -2px`, `box-shadow: none`. Fluent SCSS. |
| **files_owned** | `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` (selector: `&__cell--drag-target` only) |
| **Acceptance** | (1) `background` uses 6% color-mix. (2) `outline` is `2px dashed` with primary token. (3) `outline-offset: -2px` renders inside cell. (4) Old `box-shadow: inset` removed, replaced with `box-shadow: none`. (5) Selector disjoint from R1, R2, R11 confirmed. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Visual check: drag-fill preview shows dashed outline. |
| **Wave** | D |
| **Effort** | Low (SCSS property swap) |
| **Parallel with** | D-01, D-02, D-03, D-05, D-06, D-07 |

### Task D-05: R10 — Drag-Fill Dashed Outline (Bootstrap)

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-D05` |
| **R-Lane** | R10 |
| **Description** | Same dashed-outline change as D-04 but in Bootstrap bridge file using `var(--bs-primary)` tokens. |
| **files_owned** | `src/Marilo.Providers.Bootstrap/Styles/_bridge-allocation-scheduler.scss` (selector: `&--drag-target` only) |
| **Acceptance** | (1) Dashed outline with `--bs-primary` token. (2) Old box-shadow removed. (3) Outline renders inside cell boundary. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Visual check: drag-fill in Bootstrap provider shows dashed outline. |
| **Wave** | D |
| **Effort** | Low (SCSS property swap) |
| **Parallel with** | D-01, D-02, D-03, D-04, D-06, D-07 |

### Task D-06: R11 — Context-Menu Elevation Token

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-D06` |
| **R-Lane** | R11 |
| **Description** | Replace hardcoded `box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12)` on `&__context-menu` with `box-shadow: var(--marilo-shadow-elevated, 0 8px 24px rgba(0, 0, 0, 0.12))`. Preserves current visual as fallback while enabling token-based theming. |
| **files_owned** | `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` (selector: `&__context-menu` box-shadow only) |
| **Acceptance** | (1) `box-shadow` uses `var(--marilo-shadow-elevated, ...)` with current value as fallback. (2) Visual appearance unchanged in light mode (fallback matches). (3) Dark theme adapts if `--marilo-shadow-elevated` is defined. (4) Selector `&__context-menu` disjoint from R1 `&__cell--editing`, R2 `&__resource-panel`, R10 `&__cell--drag-target` confirmed. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Visual check: context menu shadow renders in light and dark modes. |
| **Pre-implementation check** | Verify `--marilo-shadow-elevated` exists in token system (`_generated-base.scss` or `_tokens.scss`). If missing, use `--marilo-shadow-flyout` or escalate token name to orchestrator. |
| **Wave** | D |
| **Effort** | Low (1-property SCSS change) |
| **Parallel with** | D-01, D-02, D-03, D-04, D-05, D-07 |

### Task D-07: R12 — Bootstrap Disabled-Cell Stripes (Dark Mode)

| Field | Value |
|---|---|
| **Task ID** | `ASC-W4-D07` |
| **R-Lane** | R12 |
| **Description** | Verify Bootstrap dark-mode disabled-cell rendering. If existing dark-theme patch (`rgba(255, 255, 255, 0.06)` stripes + `--bs-scheduler-disabled-bg: #2b3035`) is sufficient, close R12 as already-resolved. If stripes are invisible, increase opacity to `0.09`. If background color is wrong, adjust `--bs-scheduler-disabled-bg`. |
| **files_owned** | `src/Marilo.Providers.Bootstrap/Styles/_bridge-allocation-scheduler.scss` (dark-mode patch block, disabled-cell tokens only) |
| **Acceptance** | (1) Disabled cells show visible diagonal stripes in Bootstrap dark mode. (2) Background color contrasts properly with dark surface. (3) If no change needed, documented as verified-no-action in result. (4) If changed, only opacity value or `--bs-scheduler-disabled-bg` modified. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Visual check in Bootstrap dark mode. |
| **Wave** | D |
| **Effort** | Low (verification + possible 1-value tweak) |
| **Parallel with** | D-01, D-02, D-03, D-04, D-05, D-06 |

**Wave D Gate:** All 7 tasks complete. Full second-cycle in-workspace remediation done.

---

## Spec Coordination Notes

1. **R8 record SPEC-AS-W1-014 + R10 coordination:** If B-05 (R8 spec batch) and D-04/D-05 (R10 dashed outline) run in different waves (they do), B-05 should write spec text matching current source behavior ("inset box-shadow") and mark "pending R10 update." When D-04 lands, update the spec wording to "dashed outline" in the same wave or a follow-up.

2. **R7 spec (D-03) + R8 batch (B-05):** B-05 may touch `editing.md` for records 012/014/018. D-03 also touches `editing.md` for conflict icon docs. These target different sections (events table vs. conflict indicator paragraph) so they are content-disjoint, but if assigned to the same worker the edits should be sequenced to avoid stale-file conflicts.

3. **R6 duplicate-file caveat for R1 (C-01):** If R6 (SCSS dedup) has not landed when C-01 executes, apply the `color:` fix to both `Styles/components/_allocation-scheduler.scss` and `Styles/_allocation-scheduler.scss`. C-01 acceptance includes this conditional.

---

## Task-to-Lane Traceability

| R-Lane | Tasks | Total Effort |
|---|---|---|
| R1 | C-01 | Low |
| R2 | B-01, B-02 | Low (x2 files) |
| R3 | B-03 | Low-medium |
| R4 | B-04 | Low-medium |
| R7 | D-01, D-02, D-03 | Medium (3 tasks, widest sync) |
| R8 | B-05 | Medium (12 records) |
| R10 | D-04, D-05 | Low (x2 files) |
| R11 | D-06 | Low |
| R12 | D-07 | Low |
| **Total** | **12 tasks** | **Medium aggregate** |

---

## Wave Summary

| Wave | Tasks | R-Lanes Covered | Parallelism | Gate |
|---|---|---|---|---|
| **B** | B-01, B-02, B-03, B-04, B-05 | R2, R3, R4, R8 | All 5 fully parallel (disjoint files) | All complete or review-pending |
| **C** | C-01 | R1 | Single task | R1 complete or absorbed |
| **D** | D-01..D-07 | R7, R10, R11, R12 | All 7 fully parallel (disjoint selectors/files) | All complete |

**Total tasks:** 12
**Total R-lanes covered:** 9 / 9 in-workspace
**Excluded:** R5 (cross-component), R6 (cross-component), R9 (locked out)

---

## Verification

- **Task count:** 12 tasks covering all 9 in-workspace R-lanes.
- **Lane traceability:** Every in-workspace R-lane (R1, R2, R3, R4, R7, R8, R10, R11, R12) maps to at least one task.
- **Excluded lanes accounted:** R5/R6 (cross-component, orchestrator-owned), R9 (locked out) = 3 excluded = 12 R-lanes total.
- **R1 dual-path honored:** C-01 proceeds independently per Path B. Conditional logic for R5/R6 status documented in task.
- **R6-to-R5 ordering honored:** Wave structure follows Phase B > C > D from S02. R1 (Wave C) runs after Wave B. P2 polish (Wave D) runs after Wave C.
- **Selector disjointness:** All SCSS-touching tasks target different selectors (verified in S03). No merge conflict risk within any wave.
- **files_owned per task:** Each task declares specific files and selector scope. No two tasks within the same wave write to the same file+selector.
- **Acceptance criteria:** Every task has measurable, binary acceptance criteria.
- **Build verification:** Every task requires `dotnet build Marilo.slnx` exit 0.
- **First-cycle artifacts:** NOT touched. No edits to `gap-inventory.md` or `closure-report.md`.
- **Files written:** Only `output/stage-04/allocation-scheduler-wave4-remediation-plan.md` (this file).
- **Build/test:** N/A — this is a planning turn, not a code turn.
- **Skill discipline:**
  - `verification-before-completion` — task count verified, lane traceability verified, all S02/S03 constraints honored.
  - `requesting-code-review` — result file follows remediation-plan template with ID, description, files_owned, acceptance, build verification, wave, effort per task.
  - `systematic-debugging` — not triggered (no contradictions found).
  - `test-driven-development` — N/A (no source/test edits).

**End of Stage 04 second-cycle remediation-plan. STOP at checkpoint.**
