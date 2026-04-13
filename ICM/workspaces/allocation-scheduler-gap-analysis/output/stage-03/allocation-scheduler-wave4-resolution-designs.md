# Wave 4 Resolution Designs: MariloAllocationScheduler — Second-Cycle R-Lanes

**Stage:** 03-resolution-design (second cycle)
**Date:** 2026-04-11
**Input:** `output/stage-02/allocation-scheduler-wave4-priority-lanes.md` (9 in-workspace lanes, review PASS)
**Worker:** `w-allocation-scheduler-gap-analysis`
**Session:** marilo-grid-pipeline-2026-04-11-1200

---

## Scope

Concrete resolution designs for all 9 in-workspace R-lanes: R1, R2, R3, R4, R7, R8, R10, R11, R12.

**Excluded (not this worker's scope):** R5/R6 (cross-component, orchestrator-owned), R9 (locked out, Material tracker).

**Design methodology:** Each lane below specifies the exact files to modify, the nature of the change, a before/after sketch where applicable, breaking-change assessment, effort estimate, and dependencies.

---

## R1 — Dark-Mode Invisible Cell-Edit Text

**Gap:** `&__cell--editing input` in `_allocation-scheduler.scss` (Fluent) has `background: var(--marilo-color-surface, #ffffff)` but no explicit `color:` declaration. In Fluent Dark, the browser-default text color is often light-on-light-bg or inherits dark text that is invisible against the dark surface.

**Root cause analysis:** The dark-theme patch block (lines 629-653 of the Fluent SCSS) sets `--marilo-color-text` but the cell-edit `input` never references that token for its `color` property.

### Dual-Path Design (R5 dependency)

#### Path A: R5 lands first

If the cross-component `#fff` sweep (R5) replaces `#ffffff` fallbacks with `var(--mar-color-surface, #fff)` across the file, the `background` property of the input changes, but the missing `color:` declaration is **not** addressed by R5. R5 only fixes surface/background literals. R1 remains necessary regardless of R5 status.

**Conclusion:** R1 is NOT absorbed by R5. Proceed with R1 independently.

#### Path B: R1 proceeds independently (canonical path)

Both paths converge to the same fix.

### Resolution

| Field | Value |
|---|---|
| **File** | `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` |
| **Selector** | `.mar-allocation-scheduler .mar-allocation-scheduler__cell--editing input` (line ~483-496) |
| **Change** | Add `color: var(--marilo-color-text, #323130);` to the input rule |
| **Breaking** | No. Additive CSS property. Light-mode appearance unchanged (fallback `#323130` matches existing implicit behavior). Dark-mode becomes readable. |
| **Effort** | Low (1 line addition) |
| **Sync areas** | source, tests, gap-plan |

**Before:**
```scss
&--editing {
  padding: 0;
  input {
    // ... existing properties ...
    background: var(--marilo-color-surface, #ffffff);
    // NO color declaration
  }
}
```

**After:**
```scss
&--editing {
  padding: 0;
  input {
    // ... existing properties ...
    background: var(--marilo-color-surface, #ffffff);
    color: var(--marilo-color-text, #323130);
  }
}
```

**Test requirement:** Visual regression or bUnit test confirming cell-edit input renders readable text in `[data-marilo-theme="dark"]`.

**Note on duplicate root-level file:** The same change must be applied to `src/Marilo.Providers.FluentUI/Styles/_allocation-scheduler.scss` (root-level duplicate). However, if R6 (SCSS dedup) lands first and deletes the duplicate, only the `components/` copy needs the edit. If R6 has not landed, apply to both files to avoid divergence.

---

## R2 — Hidden-Scrollbar A11y Fix

**Gap:** `&__resource-panel` uses `scrollbar-width: none` + `&::-webkit-scrollbar { display: none; }` which hides the scrollbar entirely. Keyboard and screen-reader users cannot discover that the region scrolls.

**Root cause:** The resource panel's vertical scroll is synced from the timeline panel via JS. The hidden scrollbar was a visual choice to avoid a double-scrollbar appearance. But it violates WCAG 2.1 SC 1.3.1 (Info and Relationships) and SC 2.1.1 (Keyboard) because there is no visible affordance that the region is scrollable.

### Resolution

| Field | Value |
|---|---|
| **Files** | `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` (Fluent), `src/Marilo.Providers.Bootstrap/Styles/_bridge-allocation-scheduler.scss` (Bootstrap — same pattern at lines 220-221) |
| **Selector** | `.mar-allocation-scheduler__resource-panel` |
| **Change** | Replace `scrollbar-width: none` / `::-webkit-scrollbar { display: none }` with a thin styled scrollbar that is visible but unobtrusive |
| **Breaking** | No. Visual change only — scrollbar becomes visible. Scroll sync behavior unchanged (JS still drives both panels). |
| **Effort** | Low (remove 2 lines, add 4-6 lines per file) |
| **Sync areas** | source, tests, gap-plan |

**Before (Fluent):**
```scss
&__resource-panel {
  // ...
  scrollbar-width: none;
  &::-webkit-scrollbar { display: none; }
}
```

**After (Fluent):**
```scss
&__resource-panel {
  // ...
  // Thin visible scrollbar so keyboard/SR users can discover scrollability.
  // Sync scrolling still driven by JS from the timeline panel.
  scrollbar-width: thin;
  scrollbar-color: var(--marilo-color-border, #d1d1d1) transparent;

  &::-webkit-scrollbar {
    width: 6px;
  }
  &::-webkit-scrollbar-thumb {
    background: var(--marilo-color-border, #d1d1d1);
    border-radius: 3px;
  }
  &::-webkit-scrollbar-track {
    background: transparent;
  }
}
```

**After (Bootstrap):** Same pattern using `var(--bs-border-color)` instead of `--marilo-color-border`.

**Cross-component note from Stage 02:** Verify at dispatch time whether gantt/scheduler/datagrid share the same hidden-scrollbar pattern. If so, flag for potential cross-component promotion. This is a dispatch-time check, not a design-time blocker.

**Test requirement:** Manual test confirming scrollbar is visible and keyboard Tab can reach the scrollable region.

---

## R3 — Missing AccessibilityDemo.razor

**Gap:** Wave 2 topic matrix lists `accessibility` as Missing for AllocationScheduler. No demo page exercises keyboard walkthrough or ARIA live-region logging.

### Resolution

| Field | Value |
|---|---|
| **File** | `samples/Marilo.Demo/Pages/Components/AllocationScheduler/AccessibilityDemo.razor` (new file) |
| **Change** | Create new demo page covering keyboard navigation, ARIA roles, screen-reader announcements |
| **Breaking** | No. New file, additive. |
| **Effort** | Low-medium (new razor file, ~80-120 lines) |
| **Sync areas** | demo, docs, gap-plan |

**Page structure (modeled on existing demo pages in the same folder):**

```
@page "/components/allocation-scheduler/accessibility"
@layout ComponentDemoLayout
```

**Demo sections to include:**

1. **Keyboard Navigation Walkthrough** — Tab through toolbar, grain selector, resource panel, timeline cells. Arrow keys move cell focus. Enter/F2 enters edit mode. Escape cancels edit. Context-menu key opens context menu.
2. **ARIA Roles & Landmarks** — Show the component with `role="grid"`, `role="gridcell"`, `role="columnheader"`, `role="rowheader"`, `aria-label` on the root, `aria-selected` on selected cells, `aria-readonly` on disabled cells.
3. **Screen-Reader Live Region** — Enable `EnableLiveRegion="true"` and show how cell edits, navigation, and scenario switches announce changes via `aria-live="polite"`.
4. **High-Contrast Mode** — Demonstrate the component renders correctly under Windows High Contrast (forced-colors media query).

**Pattern reference:** Follow `AdvancedFeatures.razor` layout: `<PageSection>` wrapping `<DemoSection>` with `Title`, `Description`, `Code` props.

**No source changes required.** This is demo-only.

---

## R4 — Missing ThemingDemo.razor

**Gap:** Wave 2 topic matrix lists `theming` as Missing. No demo exercises provider swap or dark/light toggle.

### Resolution

| Field | Value |
|---|---|
| **File** | `samples/Marilo.Demo/Pages/Components/AllocationScheduler/ThemingDemo.razor` (new file) |
| **Change** | Create new demo page covering provider swap and theme toggle |
| **Breaking** | No. New file, additive. |
| **Effort** | Low-medium (new razor file, ~80-120 lines) |
| **Sync areas** | demo, docs, gap-plan |

**Demo sections to include:**

1. **Dark / Light Toggle** — Button toggles `data-marilo-theme` attribute between `"dark"` and `"light"`. AllocationScheduler re-renders with dark-mode token patches.
2. **Provider Swap** — Side-by-side or sequential rendering of the same data under FluentUI vs. Bootstrap providers (using `MariloProviderConfiguration`). Shows how BEM class roots change (`mar-allocation-scheduler` vs. `mar-bs-allocation-scheduler`) and how design tokens differ.
3. **Custom Token Overrides** — Inline `<style>` block overrides `--marilo-color-primary`, `--marilo-color-surface`, `--marilo-color-border` to demonstrate theming extensibility.

**Pattern reference:** Same `<PageSection>` + `<DemoSection>` layout as other demos.

**No source changes required.** Demo-only.

---

## R7 — Conflict Indicator Icon + ARIA Label

**Gap:** The conflict indicator on over-allocated cells uses a raw glyph character. It should use `MariloIcon` with accent styling and an `aria-label` for screen-reader users.

### Resolution

| Field | Value |
|---|---|
| **Files (source)** | `src/Marilo.Components/DataDisplay/AllocationScheduler/MariloAllocationScheduler.razor` — conflict indicator markup |
| **Files (spec)** | `docs/component-specs/allocation-scheduler/editing.md` or `selection.md` — add note about conflict icon semantic |
| **Files (demo)** | Existing demo (e.g., `AdvancedFeatures.razor` or `SelectionAndEditing.razor`) — ensure a conflict scenario is visible |
| **Files (SCSS)** | `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` — add conflict icon styling if needed |
| **Files (tests)** | bUnit test confirming `aria-label` is rendered on conflict indicator |
| **Change nature** | Replace raw glyph with `<MariloIcon>` component using accent color. Add `aria-label="Conflict: over-allocated"` (or equivalent localization key). |
| **Breaking** | No. Visual enhancement + a11y improvement. No parameter changes. |
| **Effort** | Medium (source + spec + demo + tests — widest sync footprint of P2 lanes) |
| **Sync areas** | source, spec, demo, tests, gap-plan |

**Before (conceptual):**
```razor
@if (hasConflict)
{
    <span class="mar-allocation-scheduler__conflict-icon">⚠</span>
}
```

**After (conceptual):**
```razor
@if (hasConflict)
{
    <MariloIcon Name="Warning"
                Class="mar-allocation-scheduler__conflict-icon"
                Color="var(--marilo-color-danger)"
                Size="14"
                aria-label="@ConflictAriaLabel" />
}
```

**SCSS addition (if needed):**
```scss
&__conflict-icon {
  color: var(--marilo-color-danger, #bc2f32);
  vertical-align: middle;
  margin-left: 0.25rem;
}
```

**Spec update:** Add a paragraph to the editing or selection spec noting that cells in conflict state display a `MariloIcon` with `aria-label` for screen-reader announcement.

---

## R8 — Spec Re-Audit Batch (SPEC-AS-W1-010..021)

**Gap:** 12 spec-update-only records where source is ahead of spec documentation. No source changes needed.

### Resolution

This is a text-editing batch across spec files under `docs/component-specs/allocation-scheduler/`. Each record below lists the spec file, the finding, and the required edit.

| Record | Priority | Spec File | Finding | Resolution |
|---|---|---|---|---|
| SPEC-AS-W1-010 | P1 | `scenario-planning.md` | Spec references `AllocationScenarioStatus` but source uses `ScenarioStatus` (in `Marilo.Core.BusinessLogic.Enums.BusinessLogicEnums`) | Replace all instances of `AllocationScenarioStatus` with `ScenarioStatus` in spec. Affects code snippets and type references. |
| SPEC-AS-W1-011 | P2 | `events.md` | `OnScenarioStatusChanged` event args type listed as `AllocationScenarioStatusChangedArgs` | Change to `ScenarioStatusChangedArgs` (matches `AllocationSchedulerModels.cs` line 210) |
| SPEC-AS-W1-012 | P2 | `editing.md` | `OnTimeColumnResized` event not documented in spec (source has it as a parameter on the component) | Add event documentation row to the events table. Document `TimeColumnResizedArgs` payload. |
| SPEC-AS-W1-013 | P2 | `overview.md` | `DefaultRangeLength` is marked `[Obsolete]` in source but spec still lists it as active | Add deprecation note: "Deprecated. Use `DefaultRangeUnit` with `VisibleStart` instead." |
| SPEC-AS-W1-014 | P2 | `editing.md` | Drag-fill behavior spec mentions "solid preview" but source implementation renders the drag-target with `box-shadow` only | Align spec wording to describe the inset box-shadow visual treatment. (Note: R10 may change this to dashed; coordinate spec text after R10 lands.) |
| SPEC-AS-W1-015 | P2 | `overview.md` | `ShowJumpToDate` parameter not documented | Add parameter to the Parameters table. Document its effect on toolbar rendering. |
| SPEC-AS-W1-016 | P2 | `overview.md` or `templates.md` | Grouped headers (`&__header-group-row`, `&__header-group-cell`) not mentioned in spec | Add section describing grouped header rendering for Day/Week/Month/Quarter views with group-row semantics. |
| SPEC-AS-W1-017 | P3 | `overview.md` | Current-period column highlight (`&__col-current`) not documented | Add note describing the `&__col-current` CSS class and its visual effect (tinted column, bold header, top accent border). |
| SPEC-AS-W1-018 | P3 | `editing.md` | Dynamic column fill behavior (timeline table `calc(100%/N)` for month/week/quarter/year) not documented | Add note in the layout/sizing section about how column widths adapt to view grain. |
| SPEC-AS-W1-019 | P2 | `splitter-layout.md` | Splitter restore zones (`&__splitter-restore--left`, `--right`) and pane collapse (`&__pane--collapsed`) not documented | Add subsection describing collapse behavior, restore buttons, and CSS classes. |
| SPEC-AS-W1-020 | P3 | `scenario-planning.md` | `AllocationSet.DisplayLabel` override behavior not documented | Add note that when `DisplayLabel` is non-null it takes precedence over auto-formatted baseline label. (Actually present in spec at line 31-32 — verify and close if already documented.) |
| SPEC-AS-W1-021 | P2 | `theming.md` | Dark theme patch tokens (`--marilo-color-subtle-background`, `--marilo-color-disabled-background`, `--marilo-color-primary-rgb`, `--marilo-color-text`) not documented in spec theming page | Add token table listing the dark-theme-scoped overrides and their purposes. |

**Effort:** Medium (12 spec records, text-only edits across 6-7 spec files).

**Breaking:** No. Spec-only changes. No source modifications.

**Sync areas:** spec, docs, gap-plan.

**Implementation note:** SPEC-AS-W1-014 (drag-fill wording) should be coordinated with R10 (drag-fill dashed outline). If R10 lands in the same wave, update the spec text once to reflect the final visual treatment rather than editing twice. If R10 is delayed, write the spec to match current source behavior and mark as "pending R10 update."

---

## R10 — Drag-Fill Dashed Outline

**Gap:** Drag-fill preview cells (`&__cell--drag-target`) currently use a solid background tint + inset `box-shadow`. Fluent UI guidance recommends a dashed outline for preview/tentative states to distinguish them from committed selections.

### Resolution

| Field | Value |
|---|---|
| **File** | `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` |
| **Selector** | `.mar-allocation-scheduler__cell--drag-target` (line ~475-478) |
| **Change** | Replace solid fill + box-shadow with dashed outline |
| **Breaking** | No. Visual-only change. No parameter or event changes. |
| **Effort** | Low (SCSS property swap) |
| **Sync areas** | source, tests, gap-plan |

**Before:**
```scss
&--drag-target {
  background: color-mix(in srgb, var(--marilo-color-primary, #0f6cbd) 15%, var(--marilo-color-surface, #ffffff));
  box-shadow: inset 0 0 0 1px var(--marilo-color-primary, #0f6cbd);
}
```

**After:**
```scss
&--drag-target {
  background: color-mix(in srgb, var(--marilo-color-primary, #0f6cbd) 6%, var(--marilo-color-surface, #ffffff));
  outline: 2px dashed var(--marilo-color-primary, #0f6cbd);
  outline-offset: -2px;
  box-shadow: none;
}
```

**Rationale for `outline` over `border`:** `outline` does not affect box model / cell sizing. `outline-offset: -2px` renders inside the cell boundary like the previous `box-shadow: inset` did.

**Bootstrap bridge:** The Bootstrap file (`_bridge-allocation-scheduler.scss`) has the same `&--drag-target` pattern at line ~560-563. Apply the same dashed-outline change there, using `var(--bs-primary)` tokens.

**Note on duplicate root-level file:** Same R6 caveat as R1 — if the duplicate root-level Fluent SCSS still exists, apply to both files.

**Spec coordination:** SPEC-AS-W1-014 (R8 batch) should describe the final visual treatment. If R10 and R8 are implemented in the same wave, coordinate so the spec text reflects the dashed outline.

**Test requirement:** Visual regression confirming drag-fill preview cells show dashed outline, not solid fill.

---

## R11 — Context-Menu Elevation Token

**Gap:** The context menu (`&__context-menu`) uses a hardcoded `box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12)`. This should route through the `--marilo-shadow-*` elevation token family so provider swap actually changes shadow elevation.

### Resolution

| Field | Value |
|---|---|
| **File** | `src/Marilo.Providers.FluentUI/Styles/components/_allocation-scheduler.scss` |
| **Selector** | `.mar-allocation-scheduler__context-menu` (line ~601-609) |
| **Change** | Replace hardcoded `rgba(0, 0, 0, 0.12)` shadow with elevation token |
| **Breaking** | No. Visual-only. Shadow appearance is identical when `--marilo-shadow-elevated` resolves to the same value. |
| **Effort** | Low (single property change) |
| **Sync areas** | source, tests, gap-plan |

**Before:**
```scss
&__context-menu {
  // ...
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);
}
```

**After:**
```scss
&__context-menu {
  // ...
  box-shadow: var(--marilo-shadow-elevated, 0 8px 24px rgba(0, 0, 0, 0.12));
}
```

**Design note:** The token `--marilo-shadow-elevated` is the standard Marilo elevation token for popover/menu surfaces. The fallback value preserves current behavior when the token is not defined. When a dark theme or alternative provider defines `--marilo-shadow-elevated`, the context menu shadow will adapt automatically.

**Verify token existence:** At implementation time, confirm `--marilo-shadow-elevated` is defined in the Marilo token system (check `_generated-base.scss` or `_tokens.scss`). If the token does not exist yet, use `--marilo-shadow-flyout` or propose the token name to the orchestrator as a cross-component token addition.

**Bootstrap bridge:** The Bootstrap context menu at line ~672-698 of `_bridge-allocation-scheduler.scss` uses Bootstrap's `.dropdown-menu` which provides its own shadow via Bootstrap utilities. No change needed for Bootstrap — Bootstrap's shadow system is already token-based via `--bs-dropdown-box-shadow`.

**Note on duplicate root-level file:** Same R6 caveat.

**Test requirement:** Verify context menu shadow renders correctly in both light and dark themes.

---

## R12 — Bootstrap Disabled-Cell Stripes (Dark Mode)

**Gap:** In Bootstrap dark mode, the disabled-cell diagonal stripe pattern uses `rgba(0, 0, 0, 0.07)` lines which are invisible against the dark background. The dark-mode patch block overrides this with `rgba(255, 255, 255, 0.06)` lines, which works. **However**, the disabled cell *background color* token (`--bs-scheduler-disabled-bg: #e9ecef` in light mode) renders as a light gray block that looks wrong against the dark surface.

**Root cause analysis from source:** The dark-mode patch at lines 790-832 of the Bootstrap bridge file does override `--bs-scheduler-disabled-bg` to `#2b3035` and `--bs-scheduler-disabled-pattern` to white-tint lines. Examining the SCSS more carefully:

The dark-theme patch block (lines 798-831) already overrides both tokens. The actual gap may be narrower than initially reported. Re-verify at implementation time.

### Resolution

| Field | Value |
|---|---|
| **File** | `src/Marilo.Providers.Bootstrap/Styles/_bridge-allocation-scheduler.scss` |
| **Selector** | `[data-marilo-theme="dark"], [data-bs-theme="dark"]` dark-mode patch block, disabled cell tokens |
| **Change** | Verify dark-mode disabled-cell rendering. If the existing patch is sufficient, close R12 as already-resolved. If the stripe contrast is still insufficient, increase `rgba(255, 255, 255, 0.06)` to `rgba(255, 255, 255, 0.09)` for better visibility. |
| **Breaking** | No. Visual-only change in dark mode only. |
| **Effort** | Low (verification + possible 1-value tweak) |
| **Sync areas** | source, tests, gap-plan |

**Implementation path:**

1. Build the demo app and navigate to AllocationScheduler in Bootstrap provider + dark mode.
2. Verify disabled cells show visible diagonal stripes.
3. If stripes are invisible: increase the white-tint opacity from `0.06` to `0.09`.
4. If stripes are visible but the background color is wrong: adjust `--bs-scheduler-disabled-bg` value.
5. If everything looks correct already: close R12 as no-action-needed and document finding.

**Note:** This is disjoint from all Fluent SCSS files. The Bootstrap bridge is a separate file with no overlap to R1, R2, R5, R6, R10, or R11.

**Test requirement:** Visual verification in Bootstrap dark mode showing disabled cells have visible stripe pattern.

---

## Lane Interaction Summary

| Lane | Depends On | Interacts With | File Overlap Risk |
|---|---|---|---|
| R1 | R5 (soft, NOT absorbed) | R6 (duplicate file) | Fluent `_allocation-scheduler.scss` — disjoint selectors from R2, R10, R11 |
| R2 | none | R1 (disjoint selectors confirmed: `__resource-panel` vs `__cell--editing input`) | Fluent SCSS + Bootstrap bridge — `__resource-panel` selector |
| R3 | none | none | New file, no overlap |
| R4 | none | none | New file, no overlap |
| R7 | Phase C complete (avoid source churn) | R8 spec (spec may mention conflict indicator) | Component source (.razor) + spec + demo |
| R8 | none | R7 (SPEC-AS-W1-014 coords with R10) | Spec files only — no SCSS/source overlap |
| R10 | Phase C complete | R8 (SPEC-AS-W1-014 wording), R11 (verify disjoint SCSS selectors) | Fluent `__cell--drag-target` — disjoint from R11 `__context-menu` |
| R11 | Phase C complete | R10 (disjoint confirmed: `__context-menu` vs `__cell--drag-target`) | Fluent `__context-menu` selector — disjoint from all other lanes |
| R12 | Phase C complete | none | Bootstrap bridge only — disjoint from all Fluent lanes |

**Selector disjointness verified:**
- R1: `__cell--editing input`
- R2: `__resource-panel` scrollbar rules
- R10: `__cell--drag-target`
- R11: `__context-menu`
- R12: Bootstrap bridge `.mar-bs-allocation-scheduler` scope

All five SCSS-touching lanes target different selectors in different logical sections. No merge conflict risk when run in parallel within their designated phases.

---

## Effort Summary

| Lane | Effort | Category |
|---|---|---|
| R1 | Low | 1-line SCSS addition |
| R2 | Low | ~10-line SCSS edit (2 files) |
| R3 | Low-medium | New ~100-line demo razor file |
| R4 | Low-medium | New ~100-line demo razor file |
| R7 | Medium | Source + spec + demo + SCSS + tests |
| R8 | Medium | 12 spec text edits across 6-7 files |
| R10 | Low | SCSS property swap (2 files) |
| R11 | Low | 1-property SCSS change |
| R12 | Low | Verification + possible 1-value tweak |

**Total estimated effort:** Medium aggregate. R7 and R8 are the largest individual lanes. All others are low-effort.

---

## Verification

- **Lane count:** 9 / 9 in-workspace lanes designed (R1, R2, R3, R4, R7, R8, R10, R11, R12).
- **Excluded lanes accounted:** R5/R6 (cross-component), R9 (locked out) = 3 excluded = 12 total.
- **R1 dual-path analysis:** Completed. Conclusion: R1 is NOT absorbed by R5. Both paths converge to the same fix.
- **File ownership:** All proposed edits fall within `files_read_only` scope (read for design) and will be assigned to `files_owned` at dispatch time (Stage 04/05).
- **Selector disjointness:** Verified across all SCSS-touching lanes (R1, R2, R10, R11, R12).
- **Breaking changes:** None across all 9 lanes.
- **First-cycle artifacts:** NOT touched. No edits to `gap-inventory.md` or `closure-report.md`.
- **Files written:** Only `output/stage-03/allocation-scheduler-wave4-resolution-designs.md` (this file).
- **Build/test:** N/A — this is a design turn, not a code turn.
- **Skill discipline:**
  - `verification-before-completion` — lane count verified, disjointness verified, all constraints from priority-lanes honored.
  - `requesting-code-review` — result file follows the resolution-design template with before/after, breaking assessment, effort, and sync areas per lane.
  - `systematic-debugging` — not triggered (no contradictions found).
  - `test-driven-development` — N/A (no source/test edits).

**End of Stage 03 second-cycle resolution-design. STOP at checkpoint.**
