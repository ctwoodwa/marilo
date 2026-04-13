# MariloGantt -- Wave 4 Second-Cycle Remediation Plan

**Date:** 2026-04-12
**Cycle:** SECOND -- converts S03 resolution designs into atomic implementation tasks
**Worker:** `w-gantt-gap-analysis` (tick 13, Stage 04)
**Input:** `output/stage-03/gantt-wave4-resolution-designs.md` (23 designs, 3 skipped, review PASS)
**Scope:** 23 atomic tasks across 4 implementation waves. L0 = Wave 1 (prerequisite). Skips W4-INT-19/15/16 (API decisions pending).

---

## Skipped Items (NOT in this plan)

| ID | Reason | Blocked On |
|---|---|---|
| W4-INT-19 | Selected-state model requires `SelectedItem`/`SelectedItems` public API decision | `public-api-change` escalation |
| W4-INT-15 | Today line requires `ShowTodayMarker` + possible `TodayMarkerTemplate` API decision | `public-api-change` escalation |
| W4-INT-16 | Milestone CSS shape may conflict with `MilestoneTemplate` parameter | `architecture-question` escalation |

---

## Wave Summary

| Wave | Phase | Tasks | Lane(s) | Gate |
|---|---|---|---|---|
| **1** | A | 1 | L0 | `.mar-gantt__bar` renders with visible height in Fluent + Bootstrap; bUnit test passes; SCSS compiles |
| **2** | B | 14 | L1, L2, L3 | Spec tables accurate; 5 demos build; SVG dependency styled; SCSS compiles |
| **3** | C | 5 | L4 (minus INT-19), L5 | Hover/summary/focus states visible; task-list + timeline chrome styled |
| **4** | D | 3 | L8 | No inline px/rgba/color-mix inconsistencies; all tokens documented |

**Total tasks: 23** (matches S03 designed count exactly)

---

## Wave 1 -- Bar Foundation (Phase A, prerequisite)

### TASK-W4-01: Bar base SCSS rule (W4-INT-13)

| Field | Value |
|---|---|
| **ID** | TASK-W4-01 |
| **Gap** | W4-INT-13 |
| **Lane** | L0 |
| **Wave** | 1 |
| **Priority** | P1-CRITICAL |
| **Description** | Add the missing `.mar-gantt__bar` and `.mar-gantt__bar-row` base SCSS rules to Fluent and Bootstrap providers. Introduces 6 design tokens (`--marilo-gantt-bar-height`, `--marilo-gantt-bar-bg`, `--marilo-gantt-bar-radius`, `--marilo-gantt-bar-color`, `--marilo-gantt-bar-font-size`, `--marilo-gantt-row-height`). No Razor changes -- class is already emitted. |
| **Files owned** | `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss` (bridge), `docs/component-specs/gantt/timeline/overview.md` (spec addition), bUnit test file |
| **Acceptance** | 1. `.mar-gantt__bar` rule present in both Fluent and Bootstrap SCSS. 2. Bar renders with non-zero height (24px), visible background (brand color), border-radius (4px), `position: relative`, `overflow: hidden`. 3. `.mar-gantt__bar-row` rule present in both providers with `--marilo-gantt-row-height`. 4. Spec `timeline/overview.md` has new "Bar Rendering" section documenting base class and tokens. 5. bUnit test asserts markup contains `class="mar-gantt__bar"`. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. SCSS compiles without error. `dotnet test` on Gantt-related test project passes. |
| **Effort** | S |

---

## Wave 2 -- Spec Cleanup + Demos + Dependency SVG (Phase B, parallel)

### TASK-W4-02: Add `VisibleColumns` to state enumeration (W4-INT-01)

| Field | Value |
|---|---|
| **ID** | TASK-W4-02 |
| **Gap** | W4-INT-01 |
| **Lane** | L1 |
| **Wave** | 2 |
| **Priority** | P2 |
| **Description** | Add `"VisibleColumns"` to the `PropertyName` enumeration list in `state.md`, between `"ExpandedItems"` and the list closing. Source fires `FireStateChanged("VisibleColumns")` at `MariloGantt.razor.cs:211`. |
| **Files owned** | `docs/component-specs/gantt/state.md` |
| **Acceptance** | `state.md` PropertyName enumeration includes `"VisibleColumns"`. |
| **Build verification** | File is spec-only; no build step. Manual review confirms accuracy against source. |
| **Effort** | XS |

### TASK-W4-03: Rewrite overview parameter table (W4-INT-02)

| Field | Value |
|---|---|
| **ID** | TASK-W4-03 |
| **Gap** | W4-INT-02 |
| **Lane** | L1 |
| **Wave** | 2 |
| **Priority** | P1 |
| **Description** | Replace the 2-row parameter table in `overview.md` with a comprehensive table covering all 30+ public `[Parameter]` attributes on `MariloGantt<TItem>`. Group by category: Data Binding, Appearance, State, Events, Templates, Columns, Dependencies. Each row: Parameter, Type, Default, Description. Source of truth: `MariloGantt.razor.cs`. |
| **Files owned** | `docs/component-specs/gantt/overview.md` |
| **Acceptance** | Every `[Parameter]` public property on `MariloGantt<TItem>` has a row in the parameter table. Grouped by category. No stale or missing entries. |
| **Build verification** | Spec-only. Cross-reference with `MariloGantt.razor.cs` source confirms completeness. |
| **Effort** | M |

### TASK-W4-04: Add `GetState()`/`SetStateAsync()` to methods table (W4-INT-03)

| Field | Value |
|---|---|
| **ID** | TASK-W4-04 |
| **Gap** | W4-INT-03 |
| **Lane** | L1 |
| **Wave** | 2 |
| **Priority** | P2 |
| **Description** | Add two rows to the "Gantt Reference and Methods" table in `overview.md`: `GetState()` (returns `GanttState<TItem>`) and `SetStateAsync(GanttState<TItem>? state)` (returns `Task`). |
| **Files owned** | `docs/component-specs/gantt/overview.md` |
| **Acceptance** | Both methods appear in the methods table with correct signatures and descriptions. |
| **Build verification** | Spec-only. Signatures match source. |
| **Effort** | XS |

### TASK-W4-05: Fix stale namespace references (W4-INT-04)

| Field | Value |
|---|---|
| **ID** | TASK-W4-05 |
| **Gap** | W4-INT-04 |
| **Lane** | L1 |
| **Wave** | 2 |
| **Priority** | P2 |
| **Description** | Replace all occurrences of `Marilo.Blazor.Components.MariloGantt-1` with `Marilo.Components.DataDisplay.MariloGantt` and `Marilo.Blazor.Components.GanttState-1` with `Marilo.Components.DataDisplay.GanttState` in `overview.md`. |
| **Files owned** | `docs/component-specs/gantt/overview.md` |
| **Acceptance** | Zero occurrences of `Marilo.Blazor.Components` remain in the file. All references use `Marilo.Components.DataDisplay`. |
| **Build verification** | Spec-only. Grep confirms no stale namespace remains. |
| **Effort** | XS |

### TASK-W4-06: Remove stale paging bullet (W4-INT-05)

| Field | Value |
|---|---|
| **ID** | TASK-W4-06 |
| **Gap** | W4-INT-05 |
| **Lane** | L1 |
| **Wave** | 2 |
| **Priority** | P2 |
| **Description** | Delete the bullet in `state.md` (~line 108) referencing "Filtering always resets the current page to 1" and the double `OnStateChanged` fire for `"Page"`. MariloGantt has no paging model. |
| **Files owned** | `docs/component-specs/gantt/state.md` |
| **Acceptance** | No mention of "page" or paging behavior in `state.md` that does not apply to Gantt. |
| **Build verification** | Spec-only. |
| **Effort** | XS |

### TASK-W4-07: Add milestone/summary coverage to three spec files (W4-INT-06)

| Field | Value |
|---|---|
| **ID** | TASK-W4-07 |
| **Gap** | W4-INT-06 |
| **Lane** | L1 |
| **Wave** | 2 |
| **Priority** | P2 |
| **Description** | Add milestone and summary-task documentation to three spec files: (1) `overview.md` -- new "Milestones and Summary Tasks" subsection; (2) `timeline/templates/task.md` -- note that `TaskTemplate` applies to normal bars, milestones and summary bars use their own rendering; (3) `gantt-tree/data-binding/overview.md` -- "Hierarchical Data" note on `HasChildren`/`ParentId` driving summary-task detection. |
| **Files owned** | `docs/component-specs/gantt/overview.md`, `docs/component-specs/gantt/timeline/templates/task.md`, `docs/component-specs/gantt/gantt-tree/data-binding/overview.md` |
| **Acceptance** | All three files contain milestone/summary documentation. Milestone = zero-duration diamond. Summary = parent with auto-aggregated dates and percent. |
| **Build verification** | Spec-only. |
| **Effort** | S |

### TASK-W4-08: Expand `refresh-data.md` (W4-INT-07)

| Field | Value |
|---|---|
| **ID** | TASK-W4-08 |
| **Gap** | W4-INT-07 |
| **Lane** | L1 |
| **Wave** | 2 |
| **Priority** | P2 |
| **Description** | Add two subsections to `refresh-data.md`: (1) "Automatic Change Detection" explaining that `OnParametersSetAsync` compares the `Data` reference AND count; (2) "Explicit Rebind" explaining that in-place mutations require `Rebind()`. |
| **Files owned** | `docs/component-specs/gantt/refresh-data.md` |
| **Acceptance** | Both subsections present. Detection mechanism documented. `Rebind()` call pattern documented. |
| **Build verification** | Spec-only. Content matches source behavior at `MariloGantt.razor.cs`. |
| **Effort** | S |

### TASK-W4-09: Remove or gate `ColumnResizable` example (W4-INT-26)

| Field | Value |
|---|---|
| **ID** | TASK-W4-09 |
| **Gap** | W4-INT-26 |
| **Lane** | L1 |
| **Wave** | 2 |
| **Priority** | P2 |
| **Description** | Wrap the `ColumnResizable` / `@bind-TaskListWidth` example in `state.md` (~lines 186-189) in an HTML comment block: `<!-- DEFERRED: ColumnResizable depends on SPEC-gantt-403 (JS interop). Uncomment when the feature lands. -->`. Do not delete -- preserves example for future. |
| **Files owned** | `docs/component-specs/gantt/state.md` |
| **Acceptance** | Example wrapped in deferred comment. Not visible in rendered spec. Original content preserved for future uncomment. |
| **Build verification** | Spec-only. |
| **Effort** | XS |

### TASK-W4-10: Milestones demo page (W4-INT-08)

| Field | Value |
|---|---|
| **ID** | TASK-W4-10 |
| **Gap** | W4-INT-08 |
| **Lane** | L2 |
| **Wave** | 2 |
| **Priority** | P1 |
| **Description** | Create `Milestones.razor` demo page. Data: 3 normal tasks + 2 milestones (where `Start == End`). Demonstrates that zero-duration tasks render as diamond markers. Follows existing Gantt demo conventions (self-contained `@page` route, inline `GanttTask` model, minimal UI). |
| **Files owned** | New demo `.razor` file under Gantt demo folder |
| **Acceptance** | Page builds. Milestones render as diamonds, not zero-width bars. Normal tasks render as standard bars. |
| **Build verification** | `dotnet build` passes for the demo project. |
| **Effort** | S |

### TASK-W4-11: Summary Tasks demo page (W4-INT-09)

| Field | Value |
|---|---|
| **ID** | TASK-W4-11 |
| **Gap** | W4-INT-09 |
| **Lane** | L2 |
| **Wave** | 2 |
| **Priority** | P1 |
| **Description** | Create `SummaryTasks.razor` demo page. Data: 2 parent tasks, each with 2-3 children. Parent rows have NO explicit Start/End/PercentComplete -- they compute from children. |
| **Files owned** | New demo `.razor` file under Gantt demo folder |
| **Acceptance** | Page builds. Parent bar spans the union of child date ranges. Parent PercentComplete is average of children. |
| **Build verification** | `dotnet build` passes for the demo project. |
| **Effort** | S |

### TASK-W4-12: State demo page (W4-INT-10)

| Field | Value |
|---|---|
| **ID** | TASK-W4-12 |
| **Gap** | W4-INT-10 |
| **Lane** | L2 |
| **Wave** | 2 |
| **Priority** | P2 |
| **Description** | Create `State.razor` demo page. Standard tasks. UI includes a log panel that appends `PropertyName` strings when `OnStateChanged` fires. Demonstrates sort, expand, and column-visibility state changes. |
| **Files owned** | New demo `.razor` file under Gantt demo folder |
| **Acceptance** | Page builds. Sorting a column logs `"SortDescriptor"`. Expanding a row logs `"ExpandedItems"`. Toggling a column logs `"VisibleColumns"`. |
| **Build verification** | `dotnet build` passes for the demo project. |
| **Effort** | S |

### TASK-W4-13: Refresh Data demo page (W4-INT-11)

| Field | Value |
|---|---|
| **ID** | TASK-W4-13 |
| **Gap** | W4-INT-11 |
| **Lane** | L2 |
| **Wave** | 2 |
| **Priority** | P2 |
| **Description** | Create `RefreshData.razor` demo page. 3 tasks. Two buttons: "Mutate In-Place + Rebind" (changes PercentComplete, calls `Rebind()`) and "Replace Collection" (swaps the entire list reference). |
| **Files owned** | New demo `.razor` file under Gantt demo folder |
| **Acceptance** | Page builds. Both buttons update the Gantt display. Demonstrates the two data-refresh patterns. |
| **Build verification** | `dotnet build` passes for the demo project. |
| **Effort** | S |

### TASK-W4-14: Column Chooser demo page (W4-INT-12)

| Field | Value |
|---|---|
| **ID** | TASK-W4-14 |
| **Gap** | W4-INT-12 |
| **Lane** | L2 |
| **Wave** | 2 |
| **Priority** | P2 |
| **Description** | Create `ColumnChooser.razor` demo page. Standard tasks with 4+ columns. Checkboxes toggle `VisibleColumns` state entries. |
| **Files owned** | New demo `.razor` file under Gantt demo folder |
| **Acceptance** | Page builds. Unchecking a column hides it from the task list. Re-checking restores it. |
| **Build verification** | `dotnet build` passes for the demo project. |
| **Effort** | S |

### TASK-W4-15: Dependency SVG class migration (W4-INT-14)

| Field | Value |
|---|---|
| **ID** | TASK-W4-15 |
| **Gap** | W4-INT-14 |
| **Lane** | L3 |
| **Wave** | 2 |
| **Priority** | P1-CRITICAL |
| **Description** | Replace inline `stroke="#999"` / `fill="#999"` on dependency `<line>` and `<marker><path>` elements with CSS classes `mar-gantt__dependency-line` and `mar-gantt__dependency-arrow`. Add SCSS rules to Fluent and Bootstrap providers with `--marilo-gantt-dependency-color` token. Add high-contrast `forced-colors` rules. Update spec. Two Razor locations: DayView (~line 625) and MonthView (~line 731). |
| **Files owned** | `src/Marilo.Components/Gantt/MariloGantt.razor`, `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss`, `docs/component-specs/gantt/dependencies/overview.md` or `docs/component-specs/gantt/timeline/overview.md` |
| **Acceptance** | 1. No inline `stroke="#999"` or `fill="#999"` on dependency elements. 2. `<line>` elements have `class="mar-gantt__dependency-line"`. 3. `<path>` in `<marker>` has `class="mar-gantt__dependency-arrow"`. 4. SCSS compiles in both providers. 5. `--marilo-gantt-dependency-color` token documented. 6. High-contrast rules present. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. SCSS compiles. Grep confirms zero inline `#999` on dependency elements. |
| **Effort** | S |

---

## Wave 3 -- Bar States + Chrome (Phase C, depends on Wave 1)

### TASK-W4-16: Summary bar trapezoid clip-path (W4-INT-17)

| Field | Value |
|---|---|
| **ID** | TASK-W4-16 |
| **Gap** | W4-INT-17 |
| **Lane** | L4 |
| **Wave** | 3 |
| **Priority** | P2 |
| **Description** | Replace the current `.mar-gantt__bar--summary` rule (opacity + border-bottom) with a `clip-path: polygon()` trapezoid shape in both Fluent and Bootstrap providers. Introduce `--marilo-gantt-summary-bg` and `--marilo-gantt-summary-color` tokens. No Razor changes -- modifier class already emitted. |
| **Files owned** | `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss` |
| **Acceptance** | 1. `.mar-gantt__bar--summary` uses `clip-path: polygon(0 0, 100% 0, calc(100% - 4px) 100%, 4px 100%)`. 2. No `opacity: 0.85` or `border-bottom` on summary bar. 3. Tokens `--marilo-gantt-summary-bg` and `--marilo-gantt-summary-color` present. 4. SCSS compiles. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. SCSS compiles. |
| **Effort** | S |

### TASK-W4-17: Bar hover fill darkening (W4-INT-18)

| Field | Value |
|---|---|
| **ID** | TASK-W4-17 |
| **Gap** | W4-INT-18 |
| **Lane** | L4 |
| **Wave** | 3 |
| **Priority** | P2 |
| **Description** | Add `.mar-gantt__bar:hover { filter: brightness(0.92); }` to both Fluent and Bootstrap providers. The `transition: filter 0.15s ease` in the L0 base rule (TASK-W4-01) provides smooth animation. No Razor changes. No new tokens. |
| **Files owned** | `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss` |
| **Acceptance** | 1. `.mar-gantt__bar:hover` rule present in both providers with `filter: brightness(0.92)`. 2. Existing `:hover .mar-gantt__bar-delete` rule unchanged. 3. SCSS compiles. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. SCSS compiles. |
| **Effort** | XS |

### TASK-W4-18: Focus-visible outline WCAG 2.4.7 (W4-INT-25)

| Field | Value |
|---|---|
| **ID** | TASK-W4-18 |
| **Gap** | W4-INT-25 |
| **Lane** | L4 |
| **Wave** | 3 |
| **Priority** | P2 |
| **Description** | Add `:focus-visible` outline rules for `.mar-gantt__bar`, `.mar-gantt__milestone`, `.mar-gantt__task-row` to both providers. Fluent uses `--colorStrokeFocus2`; Bootstrap uses `--bs-focus-ring-color`. Also add `tabindex="0"` to bar `<div>` (~lines 578, 684) and milestone `<div>` (~lines 567, 673) in `MariloGantt.razor`. Task row already has `tabindex`. |
| **Files owned** | `src/Marilo.Components/Gantt/MariloGantt.razor`, `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss` |
| **Acceptance** | 1. `:focus-visible` rules present for bar, milestone, task-row in both providers. 2. `tabindex="0"` on bar and milestone `<div>` elements. 3. Outline is 2px solid with 1px offset. 4. High-contrast mode auto-uses system colors (no additional rule needed). 5. SCSS compiles. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. SCSS compiles. `dotnet test` passes. |
| **Effort** | S |

### TASK-W4-19: Task-list row chrome (W4-INT-20)

| Field | Value |
|---|---|
| **ID** | TASK-W4-19 |
| **Gap** | W4-INT-20 |
| **Lane** | L5 |
| **Wave** | 3 |
| **Priority** | P2 |
| **Description** | Add SCSS rules for `.mar-gantt__tasklist`, `.mar-gantt__tasklist-header`, `.mar-gantt__header-cell`, `.mar-gantt__task-row` (with hover, `--focused`, `--editing` modifiers), `.mar-gantt__task-cell` to both Fluent and Bootstrap providers. Uses `--marilo-gantt-row-height` token from L0. No Razor changes -- all classes already emitted. |
| **Files owned** | `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss` |
| **Acceptance** | 1. Task-list has right border separator. 2. Header row has background, bottom border, `font-weight: 600`. 3. Header cells have padding and column separators. 4. Task rows have defined height via `--marilo-gantt-row-height`, bottom border, hover background, focused/editing states. 5. Task cells have padding, ellipsis overflow. 6. SCSS compiles. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. SCSS compiles. |
| **Effort** | M |

### TASK-W4-20: Timeline header chrome (W4-INT-21)

| Field | Value |
|---|---|
| **ID** | TASK-W4-20 |
| **Gap** | W4-INT-21 |
| **Lane** | L5 |
| **Wave** | 3 |
| **Priority** | P2 |
| **Description** | Add SCSS rules for `.mar-gantt__timeline-headers` (sticky top, z-index, background), `.mar-gantt__timeline-header` (with `--main`/`--secondary` modifiers), `.mar-gantt__date-label` (with `--main` modifier) to both Fluent and Bootstrap providers. No Razor changes -- all classes already emitted. |
| **Files owned** | `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss` |
| **Acceptance** | 1. Timeline headers use `position: sticky; top: 0; z-index: 5`. 2. Main header row has `font-weight: 600; font-size: 13px`. 3. Secondary header row has lighter weight and smaller font. 4. Date labels have padding, border-right separator, ellipsis. 5. SCSS compiles. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. SCSS compiles. |
| **Effort** | M |

---

## Wave 4 -- Token Hygiene (Phase D, depends on Wave 1 for visual verification)

### TASK-W4-21: Unify progress-fill formula (W4-INT-22)

| Field | Value |
|---|---|
| **ID** | TASK-W4-21 |
| **Gap** | W4-INT-22 |
| **Lane** | L8 |
| **Wave** | 4 |
| **Priority** | P3 |
| **Description** | Standardize `.mar-gantt__bar-progress` background to use `color-mix(in srgb, ...)` in both providers (replacing Bootstrap's `rgba()` approach). Expose `--marilo-gantt-progress-fill` token for full override. Document in `timeline/overview.md`. |
| **Files owned** | `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss`, `docs/component-specs/gantt/timeline/overview.md` |
| **Acceptance** | 1. Both providers use `color-mix(in srgb, ...)` for progress-fill background. 2. `--marilo-gantt-progress-fill` token present for override. 3. No `rgba(var(--bs-primary-rgb), 0.3)` remains in Bootstrap. 4. Spec documents the token. 5. SCSS compiles. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. SCSS compiles. |
| **Effort** | S |

### TASK-W4-22: Tree-column indent via CSS custom property (W4-INT-23)

| Field | Value |
|---|---|
| **ID** | TASK-W4-22 |
| **Gap** | W4-INT-23 |
| **Lane** | L8 |
| **Wave** | 4 |
| **Priority** | P3 |
| **Description** | Replace inline `padding-left: {depth * 16 + offset}px` at 5 Razor locations (~lines 251, 297, 347, 392, 481) with `style="--depth:{n}; --leaf-offset:{0|1}"` + `class="mar-gantt__indent"`. Add `.mar-gantt__indent` SCSS rule to both providers using `calc(var(--marilo-gantt-indent-per-level, 16px) * var(--depth, 0) + ...)`. Pixel-identical output preserved. |
| **Files owned** | `src/Marilo.Components/Gantt/MariloGantt.razor`, `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`, `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss` |
| **Acceptance** | 1. No inline `padding-left: {px}px` on tree indent spans. 2. `--depth` and `--leaf-offset` custom properties set on indent elements. 3. `.mar-gantt__indent` rule in both providers. 4. `--marilo-gantt-indent-per-level` token (default 16px). 5. Visual output identical to previous inline calculation. 6. SCSS compiles. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. SCSS compiles. `dotnet test` passes. |
| **Effort** | S |

### TASK-W4-23: Filter-menu elevation token (W4-INT-24)

| Field | Value |
|---|---|
| **ID** | TASK-W4-23 |
| **Gap** | W4-INT-24 |
| **Lane** | L8 |
| **Wave** | 4 |
| **Priority** | P3 |
| **Description** | Replace `box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15)` in Fluent `_gantt.scss` (~line 124) with `box-shadow: var(--shadow4, 0 2px 8px rgba(0, 0, 0, 0.15))`. Bootstrap already uses `var(--bs-box-shadow-sm)` so no change needed there. Fluent-only fix. No Razor changes. No new custom tokens. |
| **Files owned** | `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss` |
| **Acceptance** | 1. Fluent `_gantt.scss` uses `var(--shadow4, ...)` for filter-menu box-shadow. 2. No raw `rgba(0, 0, 0, 0.15)` for elevation (the value remains only as fallback inside `var()`). 3. SCSS compiles. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. SCSS compiles. |
| **Effort** | XS |

---

## Task Index

| Task ID | Gap ID | Lane | Wave | Priority | Effort | Sync Areas |
|---|---|---|---|---|---|---|
| TASK-W4-01 | W4-INT-13 | L0 | 1 | P1-CRITICAL | S | source, spec, tests, gap-plan |
| TASK-W4-02 | W4-INT-01 | L1 | 2 | P2 | XS | spec, gap-plan |
| TASK-W4-03 | W4-INT-02 | L1 | 2 | P1 | M | spec, gap-plan |
| TASK-W4-04 | W4-INT-03 | L1 | 2 | P2 | XS | spec, gap-plan |
| TASK-W4-05 | W4-INT-04 | L1 | 2 | P2 | XS | spec, gap-plan |
| TASK-W4-06 | W4-INT-05 | L1 | 2 | P2 | XS | spec, gap-plan |
| TASK-W4-07 | W4-INT-06 | L1 | 2 | P2 | S | spec, gap-plan |
| TASK-W4-08 | W4-INT-07 | L1 | 2 | P2 | S | spec, gap-plan |
| TASK-W4-09 | W4-INT-26 | L1 | 2 | P2 | XS | spec, gap-plan |
| TASK-W4-10 | W4-INT-08 | L2 | 2 | P1 | S | demo, gap-plan |
| TASK-W4-11 | W4-INT-09 | L2 | 2 | P1 | S | demo, gap-plan |
| TASK-W4-12 | W4-INT-10 | L2 | 2 | P2 | S | demo, gap-plan |
| TASK-W4-13 | W4-INT-11 | L2 | 2 | P2 | S | demo, gap-plan |
| TASK-W4-14 | W4-INT-12 | L2 | 2 | P2 | S | demo, gap-plan |
| TASK-W4-15 | W4-INT-14 | L3 | 2 | P1-CRITICAL | S | source, spec, gap-plan |
| TASK-W4-16 | W4-INT-17 | L4 | 3 | P2 | S | source, spec |
| TASK-W4-17 | W4-INT-18 | L4 | 3 | P2 | XS | source |
| TASK-W4-18 | W4-INT-25 | L4 | 3 | P2 | S | source, tests |
| TASK-W4-19 | W4-INT-20 | L5 | 3 | P2 | M | source, spec |
| TASK-W4-20 | W4-INT-21 | L5 | 3 | P2 | M | source, spec |
| TASK-W4-21 | W4-INT-22 | L8 | 4 | P3 | S | source, spec |
| TASK-W4-22 | W4-INT-23 | L8 | 4 | P3 | S | source |
| TASK-W4-23 | W4-INT-24 | L8 | 4 | P3 | XS | source |

---

## Effort Summary

| Effort | Count | Tasks |
|---|---|---|
| XS | 7 | TASK-W4-02, 04, 05, 06, 09, 17, 23 |
| S | 12 | TASK-W4-01, 07, 08, 10, 11, 12, 13, 14, 15, 16, 18, 21, 22 |
| M | 4 | TASK-W4-03, 19, 20 |
| **Total** | **23** | |

---

## Wave Dependency Graph

```
Wave 1:  TASK-W4-01 (L0 bar foundation)
   │
   ├─── GATE: .mar-gantt__bar renders with visible height; bUnit passes; SCSS compiles
   │
   ▼
Wave 2:  TASK-W4-02..09 (L1 spec) + TASK-W4-10..14 (L2 demo) + TASK-W4-15 (L3 SVG)
   │     [14 tasks, parallel — no L0 dependency]
   │
   ├─── GATE: all spec tables accurate; 5 demos build; SVG classes applied; SCSS compiles
   │
   ▼
Wave 3:  TASK-W4-16..18 (L4 bar states) + TASK-W4-19..20 (L5 chrome)
   │     [5 tasks — depend on Wave 1 for base bar rule]
   │
   ├─── GATE: hover/summary/focus visible; task-list + timeline chrome styled; SCSS compiles
   │
   ▼
Wave 4:  TASK-W4-21..23 (L8 token hygiene)
         [3 tasks — depend on Wave 1 for visual verification]

         GATE: no inline px/rgba inconsistencies; all tokens documented; SCSS compiles
```

---

## Files Touched Summary (across all tasks)

| File | Tasks | Sync Area |
|---|---|---|
| `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss` | 01, 15, 16, 17, 18, 19, 20, 21, 22, 23 | source |
| `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss` | 01, 15, 16, 17, 18, 19, 20, 21, 22 | source |
| `src/Marilo.Components/Gantt/MariloGantt.razor` | 15, 18, 22 | source |
| `docs/component-specs/gantt/overview.md` | 03, 04, 05, 07 | spec |
| `docs/component-specs/gantt/state.md` | 02, 06, 09 | spec |
| `docs/component-specs/gantt/timeline/overview.md` | 01, 21 | spec |
| `docs/component-specs/gantt/timeline/templates/task.md` | 07 | spec |
| `docs/component-specs/gantt/gantt-tree/data-binding/overview.md` | 07 | spec |
| `docs/component-specs/gantt/refresh-data.md` | 08 | spec |
| `docs/component-specs/gantt/dependencies/overview.md` | 15 | spec |
| 5 new demo `.razor` pages | 10, 11, 12, 13, 14 | demo |
| bUnit test file(s) | 01, 18 | tests |

---

## Verification Checklist

- [x] 23 tasks match 23 S03 designed gaps exactly
- [x] 3 skipped items (W4-INT-15, 16, 19) documented and excluded
- [x] L0 (TASK-W4-01) is Wave 1 prerequisite
- [x] Wave 2 tasks (L1/L2/L3) have no L0 dependency -- can run in parallel
- [x] Wave 3 tasks (L4/L5) depend on Wave 1 completion
- [x] Wave 4 tasks (L8) depend on Wave 1 for visual verification
- [x] Every task has: ID, description, files_owned, acceptance, build verification, wave, effort
- [x] No overlap with first-cycle `gantt-remediation-plan.md`
- [x] Cross-component items (W4-ROUTE-01..04) and already-queued (W4-QUEUED-01) remain out of scope

---

## Stage 04 STOP -- end of remediation plan. Ready for orchestrator review.
