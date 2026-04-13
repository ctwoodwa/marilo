# MariloGantt -- Wave 4 Second-Cycle Resolution Designs

**Date:** 2026-04-12
**Cycle:** SECOND -- designs resolutions for Wave 4 gaps from `output/stage-02/gantt-wave4-priority-lanes.md`
**Worker:** `w-gantt-gap-analysis` (tick 11, Stage 03)
**Input:** `output/stage-02/gantt-wave4-priority-lanes.md` (9 lanes, 26 gaps, review PASS)
**Scope:** Design resolutions for L0, L1, L2, L3, L4, L5, L8. SKIP W4-INT-19 (selected state API), W4-INT-15 (today line API), W4-INT-16 (milestone template conflict). L6/L7 are fully composed of skipped items.

---

## Skipped Items (awaiting user decisions)

| ID | Lane | Reason | Escalation Type |
|---|---|---|---|
| W4-INT-19 | L4 | Selected state may need `SelectedItem`/`SelectedItems` public API | `public-api-change` |
| W4-INT-15 | L6 | Today line may need `TodayMarkerTemplate` API | `public-api-change` |
| W4-INT-16 | L7 | Milestone CSS shape may conflict with `MilestoneTemplate` | `architecture-question` |

These are documented but NOT designed below. When user decisions arrive, a follow-up design pass will cover them.

---

## L0 -- Bar Foundation (W4-INT-13)

**Priority:** P1-CRITICAL
**Prerequisite for:** L4, L5, L8
**Sync areas:** source (SCSS), spec, tests, gap-plan

### Problem

First-cycle E7 created `_gantt.scss` (Fluent) and `_bridge-gantt.scss` (Bootstrap) with modifier rules (`.mar-gantt__bar--summary`, `.mar-gantt__bar-progress`, `.mar-gantt__bar-delete`, `.mar-gantt__milestone`) but never wrote a base `.mar-gantt__bar` rule. The Razor emits `<div class="mar-gantt__bar ...">` at lines 578/561 (DayView) and 684/667 (MonthView) in `MariloGantt.razor`. Without the base rule, bars are zero-height unstyled `<div>`s. The existing `.mar-gantt__bar:hover .mar-gantt__bar-delete` rule (line 91 Fluent, line 91 Bootstrap) already depends on a `.mar-gantt__bar` parent, confirming the class is correct.

### Resolution Design

**No Razor changes required.** The Razor already emits the correct class. This is purely an SCSS addition in two providers.

#### Fluent Provider (`src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`)

Insert BEFORE the `// -- Milestone` comment (line 42). New block:

```scss
// -- Bar base ----------------------------------------------------------------
.mar-gantt__bar {
  position: relative;          // child positioning for progress fill, resize handles, delete
  height: var(--marilo-gantt-bar-height, 24px);
  background: var(--marilo-gantt-bar-bg, var(--colorBrandBackground, #0078d4));
  border-radius: var(--marilo-gantt-bar-radius, 4px);
  color: var(--marilo-gantt-bar-color, var(--colorNeutralForegroundOnBrand, #ffffff));
  cursor: pointer;
  overflow: hidden;            // clips progress fill to border-radius
  display: flex;
  align-items: center;
  font-size: var(--marilo-gantt-bar-font-size, 12px);
  line-height: 1;
  box-sizing: border-box;
  transition: filter 0.15s ease;
}

.mar-gantt__bar-row {
  position: relative;
  height: var(--marilo-gantt-row-height, 36px);
  display: flex;
  align-items: center;
}
```

**Tokens introduced:**
| Token | Default | Purpose |
|---|---|---|
| `--marilo-gantt-bar-height` | `24px` | Bar element height |
| `--marilo-gantt-bar-bg` | `var(--colorBrandBackground, #0078d4)` | Bar fill color |
| `--marilo-gantt-bar-radius` | `4px` | Bar corner rounding |
| `--marilo-gantt-bar-color` | `var(--colorNeutralForegroundOnBrand, #ffffff)` | Bar text/icon color |
| `--marilo-gantt-bar-font-size` | `12px` | Bar label size |
| `--marilo-gantt-row-height` | `36px` | Vertical space per bar row (grid rhythm) |

#### Bootstrap Provider (`src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss`)

Insert BEFORE the `// -- Milestone` comment (line 42). Same structure, Bootstrap tokens:

```scss
// -- Bar base ----------------------------------------------------------------
.mar-gantt__bar {
  position: relative;
  height: var(--marilo-gantt-bar-height, 24px);
  background: var(--marilo-gantt-bar-bg, var(--bs-primary, #0d6efd));
  border-radius: var(--marilo-gantt-bar-radius, var(--bs-border-radius-sm, 0.25rem));
  color: var(--marilo-gantt-bar-color, #fff);
  cursor: pointer;
  overflow: hidden;
  display: flex;
  align-items: center;
  font-size: var(--marilo-gantt-bar-font-size, 12px);
  line-height: 1;
  box-sizing: border-box;
  transition: filter 0.15s ease;
}

.mar-gantt__bar-row {
  position: relative;
  height: var(--marilo-gantt-row-height, 36px);
  display: flex;
  align-items: center;
}
```

#### High-Contrast Update

Both providers already have a `@media (forced-colors: active)` block with `.mar-gantt__bar { border: 1px solid ButtonText; }`. This will now correctly apply to bars that have visible height. No change needed.

#### Spec Update

`docs/component-specs/gantt/timeline/overview.md` -- add a "Bar Rendering" section documenting the base class, tokens, and default appearance.

#### Test

bUnit test: render a `MariloGantt<GanttTask>` with one task, assert the rendered markup contains `class="mar-gantt__bar"`. Verify SCSS compiles (`dotnet build` on the provider projects).

#### Breaking Changes

None. This is a purely additive SCSS rule for a class already emitted by Razor.

---

## L1 -- Spec Cleanup (W4-INT-01 through W4-INT-07, W4-INT-26)

**Priority:** P1-P2
**Sync areas:** spec, gap-plan
**No source changes.** Pure documentation fixes.

### W4-INT-01 -- Add `"VisibleColumns"` to `state.md` enumeration

**File:** `docs/component-specs/gantt/state.md` ~line 63
**Change:** Add `"VisibleColumns"` to the `PropertyName` enumeration list, between `"ExpandedItems"` and the closing of the list. One-line addition.
**Rationale:** Source fires `FireStateChanged("VisibleColumns")` at `MariloGantt.razor.cs:211`.

### W4-INT-02 -- Rewrite overview parameter table

**File:** `docs/component-specs/gantt/overview.md` ~lines 157-161
**Change:** Replace the 2-row parameter table with a comprehensive table covering all 30+ public parameters on `MariloGantt<TItem>`. Group by category: Data Binding, Appearance, State, Events, Templates, Columns, Dependencies. Each row: Parameter, Type, Default, Description. Source of truth: `MariloGantt.razor.cs` `[Parameter]` attributes.
**Effort:** M -- requires reading all parameters from source and formatting. No judgment calls; mechanical.

### W4-INT-03 -- Add `GetState()` / `SetStateAsync()` to overview methods table

**File:** `docs/component-specs/gantt/overview.md` ~lines 226-228
**Change:** Add two rows to the "Gantt Reference and Methods" table:
- `GetState()` -- Returns: `GanttState<TItem>` -- Description: "Returns a snapshot of the current component state (sort, filter, expanded items, visible columns, view)."
- `SetStateAsync(GanttState<TItem>? state)` -- Returns: `Task` -- Description: "Restores component state from a saved snapshot. Pass `null` to reset to defaults."

### W4-INT-04 -- Fix stale namespace references

**File:** `docs/component-specs/gantt/overview.md` ~lines 221, 253
**Change:** Replace `Marilo.Blazor.Components.MariloGantt-1` with `Marilo.Components.DataDisplay.MariloGantt` and `Marilo.Blazor.Components.GanttState-1` with `Marilo.Components.DataDisplay.GanttState`. Global find-replace within the file.

### W4-INT-05 -- Remove stale paging bullet

**File:** `docs/component-specs/gantt/state.md` ~line 108
**Change:** Delete the bullet that says "Filtering always resets the current page to 1, so the `OnStateChanged` event will fire twice. First, `PropertyName` will be equal to `\"Page\"`". MariloGantt has no paging model.

### W4-INT-06 -- Add milestone/summary coverage to three spec files

**Files:**
1. `docs/component-specs/gantt/overview.md` -- Add a "Milestones and Summary Tasks" subsection explaining zero-duration = milestone diamond, parent rows = summary with auto-aggregated Start/End/PercentComplete.
2. `docs/component-specs/gantt/timeline/templates/task.md` -- Add a note that `TaskTemplate` applies to normal bars; milestones and summary bars use their own rendering (diamond / trapezoid). Document `MilestoneTemplate` if it exists.
3. `docs/component-specs/gantt/gantt-tree/data-binding/overview.md` -- Add a "Hierarchical Data" note explaining how `HasChildren`/`ParentId` drive summary-task detection and bottom-up aggregation.

### W4-INT-07 -- Expand `refresh-data.md`

**File:** `docs/component-specs/gantt/refresh-data.md`
**Change:** Add two subsections:
1. "Automatic Change Detection" -- explain that `OnParametersSetAsync` compares the `Data` reference AND count; a new reference or different count triggers re-render without `Rebind()`.
2. "Explicit Rebind" -- explain that in-place mutations (same reference, same count) require calling `Rebind()` to force recomputation of computed dates and hierarchy.

### W4-INT-26 -- Remove or gate `ColumnResizable` example

**File:** `docs/component-specs/gantt/state.md` ~lines 186-189
**Change:** Wrap the example in a `<!-- DEFERRED: ColumnResizable depends on SPEC-gantt-403 (JS interop). Uncomment when the feature lands. -->` comment block. Do not delete -- preserves the example for when the feature ships.

---

## L2 -- Demo Coverage (W4-INT-08 through W4-INT-12)

**Priority:** P1-P2
**Sync areas:** demo, gap-plan
**No source or spec changes.** New `.razor` demo pages only.

### Design Principle

Each demo page follows existing Gantt demo conventions:
- Self-contained `@page` route under the Gantt demo folder
- Inline `GanttTask` model with sample data
- Minimal UI: the Gantt component + a brief description of what to observe
- Build-verifiable: `dotnet build` must pass

### W4-INT-08 -- Milestones Demo (`Milestones.razor`)

**Target behavior:** Zero-duration tasks render as diamond markers.
**Data:** 3 normal tasks + 2 milestones (where `Start == End`).
**Assertion point:** Visual -- diamonds appear at the milestone dates, not zero-width bars.

### W4-INT-09 -- Summary Tasks Demo (`SummaryTasks.razor`)

**Target behavior:** Parent rows auto-aggregate Start/End/PercentComplete from children.
**Data:** 2 parent tasks, each with 2-3 children. Parent rows have NO explicit Start/End/PercentComplete -- they should compute from children.
**Assertion point:** Parent bar spans the union of child date ranges. Parent PercentComplete is the average of children.

### W4-INT-10 -- State Demo (`State.razor`)

**Target behavior:** `OnStateChanged` fires with correct `PropertyName` values.
**Data:** Standard tasks. UI adds a log panel that appends `PropertyName` strings when `OnStateChanged` fires.
**Assertion point:** Sorting a column logs `"SortDescriptor"`. Expanding a row logs `"ExpandedItems"`. Toggling a column logs `"VisibleColumns"`.

### W4-INT-11 -- Refresh Data Demo (`RefreshData.razor`)

**Target behavior:** In-place mutation vs reference-swap with `Rebind()`.
**Data:** 3 tasks. Two buttons: "Mutate In-Place + Rebind" (changes a task's PercentComplete, calls `Rebind()`), "Replace Collection" (swaps the entire list reference).
**Assertion point:** Both buttons update the Gantt display.

### W4-INT-12 -- Column Chooser Demo (`ColumnChooser.razor`)

**Target behavior:** Toggle column visibility via state.
**Data:** Standard tasks with 4+ columns. Checkboxes toggle `VisibleColumns` state entries.
**Assertion point:** Unchecking a column hides it from the task list. Re-checking restores it.

---

## L3 -- Dependency SVG (W4-INT-14)

**Priority:** P1-CRITICAL
**Sync areas:** source (Razor + SCSS), spec

### Problem

`MariloGantt.razor` at lines 615-627 (DayView) and 721-733 (MonthView) renders dependency lines with:
- Inline `stroke="#999"` and `stroke-width="1.5"` on `<line>` elements
- `<marker>` arrowhead with inline `fill="#999"`
- No CSS class on the `<line>` elements (only the parent `<svg>` has `class="mar-gantt__dependency-svg"`)

### Resolution Design

#### Razor Changes (`MariloGantt.razor`)

Both DayView (~line 615) and MonthView (~line 721) blocks. Replace:

```html
<line x1="..." y1="..." x2="..." y2="..."
      stroke="#999" stroke-width="1.5"
      marker-end="url(#gantt-arrow-{_instanceId})" />
```

With:

```html
<line x1="..." y1="..." x2="..." y2="..."
      class="mar-gantt__dependency-line"
      marker-end="url(#gantt-arrow-{_instanceId})" />
```

Also update the `<marker>` `<path>` to remove inline `fill="#999"`:

```html
<marker id="@($"gantt-arrow-{_instanceId}")" markerWidth="10" markerHeight="8" refX="10" refY="4" orient="auto">
    <path d="M0,0 L10,4 L0,8 Z" class="mar-gantt__dependency-arrow" />
</marker>
```

#### Fluent SCSS (`src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`)

Add after the bar base rule block:

```scss
// -- Dependency lines ---------------------------------------------------------
.mar-gantt__dependency-line {
  stroke: var(--marilo-gantt-dependency-color, var(--colorNeutralStroke1, #999));
  stroke-width: 1.5px;
  fill: none;
}

.mar-gantt__dependency-arrow {
  fill: var(--marilo-gantt-dependency-color, var(--colorNeutralStroke1, #999));
}
```

#### Bootstrap SCSS (`src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss`)

```scss
// -- Dependency lines ---------------------------------------------------------
.mar-gantt__dependency-line {
  stroke: var(--marilo-gantt-dependency-color, var(--bs-secondary-color, #6c757d));
  stroke-width: 1.5px;
  fill: none;
}

.mar-gantt__dependency-arrow {
  fill: var(--marilo-gantt-dependency-color, var(--bs-secondary-color, #6c757d));
}
```

#### High-Contrast Addition

Add to both providers' `@media (forced-colors: active)` blocks:

```scss
.mar-gantt__dependency-line {
  stroke: LinkText;
}
.mar-gantt__dependency-arrow {
  fill: LinkText;
}
```

#### Token Introduced

| Token | Default (Fluent) | Default (Bootstrap) | Purpose |
|---|---|---|---|
| `--marilo-gantt-dependency-color` | `var(--colorNeutralStroke1, #999)` | `var(--bs-secondary-color, #6c757d)` | Dependency line and arrow color |

#### Spec Update

Document the dependency line class and token in `docs/component-specs/gantt/dependencies/overview.md` or `timeline/overview.md`.

#### Breaking Changes

**Visual only.** The lines will look the same by default (#999 fallback). The structural change (class instead of inline) is non-breaking for consumers. No public API change.

---

## L4 -- Bar States (W4-INT-17, W4-INT-18, W4-INT-25)

**Note:** W4-INT-19 (selected state) is SKIPPED. This section covers the remaining 3 gaps in L4.

### W4-INT-17 -- Summary Bar Trapezoid

**Priority:** P2
**Depends on:** L0 (bar base rule must exist)

#### Problem

`.mar-gantt__bar--summary` currently uses `opacity: 0.85; border-bottom: 2px solid ...;` which is a flat overlay, not a recognizable Gantt summary shape. Industry standard is a trapezoid (wider at top, narrower at bottom) or inverted bracket.

#### Resolution Design

Replace the current `.mar-gantt__bar--summary` rule in both providers.

**Fluent:**
```scss
.mar-gantt__bar--summary {
  background: var(--marilo-gantt-summary-bg, var(--colorNeutralBackground3, #f0f0f0));
  color: var(--marilo-gantt-summary-color, var(--colorNeutralForeground1, #242424));
  clip-path: polygon(0 0, 100% 0, calc(100% - 4px) 100%, 4px 100%);
  border-radius: 0;
  opacity: 1;
  border-bottom: none;
}
```

**Bootstrap:**
```scss
.mar-gantt__bar--summary {
  background: var(--marilo-gantt-summary-bg, var(--bs-secondary-bg, #e9ecef));
  color: var(--marilo-gantt-summary-color, var(--bs-body-color, #212529));
  clip-path: polygon(0 0, 100% 0, calc(100% - 4px) 100%, 4px 100%);
  border-radius: 0;
  opacity: 1;
  border-bottom: none;
}
```

**Tokens:**
| Token | Purpose |
|---|---|
| `--marilo-gantt-summary-bg` | Summary bar background (lighter than normal bar) |
| `--marilo-gantt-summary-color` | Summary bar text color |

**No Razor changes.** The `mar-gantt__bar--summary` modifier is already emitted correctly.

### W4-INT-18 -- Bar Hover Fill

**Priority:** P2
**Depends on:** L0

#### Resolution Design

Add to both providers after the bar base rule:

**Fluent:**
```scss
.mar-gantt__bar:hover {
  filter: brightness(0.92);
}
```

**Bootstrap:**
```scss
.mar-gantt__bar:hover {
  filter: brightness(0.92);
}
```

Simple and provider-agnostic. `filter: brightness()` darkens any background color without needing to know its value. The existing `.mar-gantt__bar:hover .mar-gantt__bar-delete { display: inline-flex; }` rule stays as-is (delete button reveal on hover).

**No Razor changes. No new tokens.** The `cursor: pointer` is already in the L0 base rule. `transition: filter 0.15s ease` in L0 base rule provides smooth animation.

### W4-INT-25 -- `:focus-visible` Outline (WCAG 2.4.7)

**Priority:** P2
**Sync areas:** source (SCSS), tests

#### Resolution Design

Add to both providers:

**Fluent:**
```scss
// -- Focus visible (WCAG 2.4.7) ----------------------------------------------
.mar-gantt__bar:focus-visible,
.mar-gantt__milestone:focus-visible,
.mar-gantt__task-row:focus-visible {
  outline: 2px solid var(--colorStrokeFocus2, #0078d4);
  outline-offset: 1px;
}
```

**Bootstrap:**
```scss
// -- Focus visible (WCAG 2.4.7) ----------------------------------------------
.mar-gantt__bar:focus-visible,
.mar-gantt__milestone:focus-visible,
.mar-gantt__task-row:focus-visible {
  outline: 2px solid var(--bs-focus-ring-color, rgba(13, 110, 253, 0.25));
  outline-offset: 1px;
}
```

**Razor check:** The bar `<div>` and milestone `<div>` need `tabindex="0"` to be focusable. Check if already present:
- Bar at line 578: no `tabindex`. **Razor change needed:** add `tabindex="0"` to the bar `<div>`.
- Milestone at line 567: no `tabindex`. **Razor change needed:** add `tabindex="0"` to the milestone `<div>`.
- Task row: already has `tabindex` from keyboard navigation code (line 480).

**High-contrast addition:** Already covered by `@media (forced-colors: active)` -- `outline` in forced-colors mode automatically uses system colors.

---

## L5 -- Task-List and Timeline Chrome (W4-INT-20, W4-INT-21)

**Priority:** P2
**Depends on:** L0 (partial -- row-height token alignment)

### W4-INT-20 -- Task-List Row Chrome

#### Problem

Razor emits `.mar-gantt__tasklist`, `.mar-gantt__tasklist-header`, `.mar-gantt__task-row`, `.mar-gantt__task-cell`, `.mar-gantt__header-cell` but no SCSS rule defines row height, header background, column separators, or row hover.

#### Resolution Design

**Fluent (`_gantt.scss`):**

```scss
// -- Task list chrome ---------------------------------------------------------
.mar-gantt__tasklist {
  border-right: 1px solid var(--colorNeutralStroke2, #e0e0e0);
}

.mar-gantt__tasklist-header {
  display: flex;
  background: var(--colorNeutralBackground3, #f5f5f5);
  border-bottom: 1px solid var(--colorNeutralStroke2, #e0e0e0);
  font-weight: 600;
  font-size: 12px;
}

.mar-gantt__header-cell {
  // position: relative already exists (line 97)
  padding: 6px 8px;
  border-right: 1px solid var(--colorNeutralStroke3, #ebebeb);

  &:last-child {
    border-right: none;
  }
}

.mar-gantt__task-row {
  display: flex;
  align-items: center;
  height: var(--marilo-gantt-row-height, 36px);
  border-bottom: 1px solid var(--colorNeutralStroke3, #ebebeb);

  &:hover {
    background: var(--colorNeutralBackground1Hover, #f5f5f5);
  }

  &--focused {
    background: var(--colorNeutralBackground1Selected, #ebebeb);
  }

  &--editing {
    background: var(--colorNeutralBackground4, #fafafa);
  }
}

.mar-gantt__task-cell {
  padding: 4px 8px;
  font-size: 13px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
```

**Bootstrap (`_bridge-gantt.scss`):** Same structure, Bootstrap tokens:

```scss
// -- Task list chrome ---------------------------------------------------------
.mar-gantt__tasklist {
  border-right: 1px solid var(--bs-border-color, #dee2e6);
}

.mar-gantt__tasklist-header {
  display: flex;
  background: var(--bs-tertiary-bg, #f8f9fa);
  border-bottom: 1px solid var(--bs-border-color, #dee2e6);
  font-weight: 600;
  font-size: 12px;
}

.mar-gantt__header-cell {
  padding: 6px 8px;
  border-right: 1px solid var(--bs-border-color-translucent, rgba(0, 0, 0, 0.175));

  &:last-child {
    border-right: none;
  }
}

.mar-gantt__task-row {
  display: flex;
  align-items: center;
  height: var(--marilo-gantt-row-height, 36px);
  border-bottom: 1px solid var(--bs-border-color-translucent, rgba(0, 0, 0, 0.175));

  &:hover {
    background: var(--bs-tertiary-bg, #f8f9fa);
  }

  &--focused {
    background: var(--bs-secondary-bg, #e9ecef);
  }

  &--editing {
    background: var(--bs-light, #f8f9fa);
  }
}

.mar-gantt__task-cell {
  padding: 4px 8px;
  font-size: 13px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
```

**No Razor changes.** All classes are already emitted.

### W4-INT-21 -- Timeline Header Chrome

#### Problem

`.mar-gantt__timeline-header`, `--main`, `--secondary`, `.mar-gantt__date-label` emitted by Razor but unstyled.

#### Resolution Design

**Fluent (`_gantt.scss`):**

```scss
// -- Timeline header chrome ---------------------------------------------------
.mar-gantt__timeline-headers {
  position: sticky;
  top: 0;
  z-index: 5;
  background: var(--colorNeutralBackground1, #ffffff);
}

.mar-gantt__timeline-header {
  display: flex;
  border-bottom: 1px solid var(--colorNeutralStroke2, #e0e0e0);

  &--main {
    font-weight: 600;
    font-size: 13px;
  }

  &--secondary {
    font-weight: 400;
    font-size: 11px;
    color: var(--colorNeutralForeground2, #616161);
  }
}

.mar-gantt__date-label {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 4px 2px;
  border-right: 1px solid var(--colorNeutralStroke3, #ebebeb);
  box-sizing: border-box;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;

  &--main {
    font-weight: 600;
  }
}
```

**Bootstrap (`_bridge-gantt.scss`):**

```scss
// -- Timeline header chrome ---------------------------------------------------
.mar-gantt__timeline-headers {
  position: sticky;
  top: 0;
  z-index: 5;
  background: var(--bs-body-bg, #ffffff);
}

.mar-gantt__timeline-header {
  display: flex;
  border-bottom: 1px solid var(--bs-border-color, #dee2e6);

  &--main {
    font-weight: 600;
    font-size: 13px;
  }

  &--secondary {
    font-weight: 400;
    font-size: 11px;
    color: var(--bs-secondary-color, #6c757d);
  }
}

.mar-gantt__date-label {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 4px 2px;
  border-right: 1px solid var(--bs-border-color-translucent, rgba(0, 0, 0, 0.175));
  box-sizing: border-box;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;

  &--main {
    font-weight: 600;
  }
}
```

**No Razor changes.** Classes already emitted. The `position: sticky; top: 0` on `.mar-gantt__timeline-headers` (the wrapper div at line 507) will pin the dual-row header during vertical scroll.

---

## L8 -- Token Hygiene (W4-INT-22, W4-INT-23, W4-INT-24)

**Priority:** P3
**Depends on:** L0 (for progress-fill visual verification)

### W4-INT-22 -- Unify Progress-Fill Formula

#### Problem

Fluent uses `color-mix(in srgb, var(--marilo-color-primary) 40%, var(--marilo-color-surface))`.
Bootstrap uses `rgba(var(--bs-primary-rgb), 0.3)`.
Different formulas produce different fill appearances.

#### Resolution Design

Standardize on `color-mix` in both providers (wider browser support since 2023, all target browsers support it). Expose a token for override.

**Fluent:**
```scss
.mar-gantt__bar-progress {
  // ... existing positioning ...
  background: var(--marilo-gantt-progress-fill, color-mix(in srgb, var(--colorBrandBackground, #0078d4) 40%, var(--colorNeutralBackground1, #ffffff)));
}
```

**Bootstrap:**
```scss
.mar-gantt__bar-progress {
  // ... existing positioning ...
  background: var(--marilo-gantt-progress-fill, color-mix(in srgb, var(--bs-primary, #0d6efd) 40%, var(--bs-body-bg, #ffffff)));
}
```

**Token:** `--marilo-gantt-progress-fill` -- allows full override of the progress fill color without understanding `color-mix`.

**Spec update:** Document in `timeline/overview.md`.

### W4-INT-23 -- Tree-Column Indent via CSS Custom Property

#### Problem

Razor at lines 251/297/347/392 computes `padding-left: {depth * 16 + offset}px` inline. This prevents theming the indent depth.

#### Resolution Design

**Razor change:** Replace inline `padding-left` with a `--depth` custom property.

Current (4 locations):
```csharp
var pad = (int)(n.Depth * 16 + (hasChildren ? 0 : 16));
<span style="padding-left: @(pad)px; ...">
```

Proposed:
```csharp
<span style="--depth: @(n.Depth); --leaf-offset: @(hasChildren ? 0 : 1); ..." class="mar-gantt__indent">
```

**SCSS addition (both providers):**
```scss
.mar-gantt__indent {
  padding-left: calc(
    var(--marilo-gantt-indent-per-level, 16px) * var(--depth, 0)
    + var(--marilo-gantt-indent-per-level, 16px) * var(--leaf-offset, 0)
  );
  display: inline-flex;
  align-items: center;
}
```

**Token:** `--marilo-gantt-indent-per-level` (default `16px`).

Also applies to line 481 (`style="padding-left: @(pad)px;"` on the read-only task row span).

**Breaking changes:** None externally. Internal: the pixel-identical output is preserved because `16px * depth + 16px * leafOffset` equals the old formula.

### W4-INT-24 -- Filter-Menu Elevation Token

#### Problem

Fluent `_gantt.scss` line 124: `box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);` is a literal instead of a Fluent elevation token.

#### Resolution Design

**Fluent only** (Bootstrap already uses `var(--bs-box-shadow-sm)` at line 124):

Replace:
```scss
box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
```

With:
```scss
box-shadow: var(--shadow4, 0 2px 8px rgba(0, 0, 0, 0.15));
```

`--shadow4` is the Fluent UI 2 elevation token for menus/popovers. The `rgba` value stays as fallback.

**No Razor changes. No new custom tokens.**

---

## Cross-Reference: All Tokens Introduced

| Token | Introduced In | Default (Fluent) | Default (Bootstrap) |
|---|---|---|---|
| `--marilo-gantt-bar-height` | L0 | `24px` | `24px` |
| `--marilo-gantt-bar-bg` | L0 | `var(--colorBrandBackground)` | `var(--bs-primary)` |
| `--marilo-gantt-bar-radius` | L0 | `4px` | `var(--bs-border-radius-sm)` |
| `--marilo-gantt-bar-color` | L0 | `var(--colorNeutralForegroundOnBrand)` | `#fff` |
| `--marilo-gantt-bar-font-size` | L0 | `12px` | `12px` |
| `--marilo-gantt-row-height` | L0 | `36px` | `36px` |
| `--marilo-gantt-dependency-color` | L3 | `var(--colorNeutralStroke1)` | `var(--bs-secondary-color)` |
| `--marilo-gantt-summary-bg` | L4 (INT-17) | `var(--colorNeutralBackground3)` | `var(--bs-secondary-bg)` |
| `--marilo-gantt-summary-color` | L4 (INT-17) | `var(--colorNeutralForeground1)` | `var(--bs-body-color)` |
| `--marilo-gantt-progress-fill` | L8 (INT-22) | `color-mix(...)` | `color-mix(...)` |
| `--marilo-gantt-indent-per-level` | L8 (INT-23) | `16px` | `16px` |

---

## Razor Changes Summary

| File | Lane | Change |
|---|---|---|
| `MariloGantt.razor` ~line 625, 731 | L3 | Remove inline `stroke="#999"` / `fill="#999"`, add classes |
| `MariloGantt.razor` ~line 618, 724 | L3 | Add class to `<path>` in `<marker>` |
| `MariloGantt.razor` ~line 578, 684 (bar div) | L4 (INT-25) | Add `tabindex="0"` |
| `MariloGantt.razor` ~line 567, 673 (milestone div) | L4 (INT-25) | Add `tabindex="0"` |
| `MariloGantt.razor` ~lines 253, 297, 347, 392, 481 | L8 (INT-23) | Replace inline `padding-left` with `--depth` / `--leaf-offset` + class |

---

## SCSS Files Summary

| File | Lanes | Changes |
|---|---|---|
| `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss` | L0, L3, L4, L5, L8 | Bar base, dependency lines, hover/focus/summary, task-list chrome, timeline header, progress-fill, indent, elevation |
| `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss` | L0, L3, L4, L5, L8 | Same structure, Bootstrap tokens |

---

## Spec Files Summary

| File | Lanes | Changes |
|---|---|---|
| `docs/component-specs/gantt/overview.md` | L1 (INT-02, INT-03, INT-04, INT-06) | Parameter table rewrite, methods table, namespace fix, milestone/summary section |
| `docs/component-specs/gantt/state.md` | L1 (INT-01, INT-05, INT-26) | VisibleColumns enum, paging bullet removal, ColumnResizable gate |
| `docs/component-specs/gantt/timeline/templates/task.md` | L1 (INT-06) | Milestone/summary template note |
| `docs/component-specs/gantt/gantt-tree/data-binding/overview.md` | L1 (INT-06) | Hierarchical data note |
| `docs/component-specs/gantt/refresh-data.md` | L1 (INT-07) | Auto-detect + explicit Rebind sections |
| `docs/component-specs/gantt/timeline/overview.md` | L0 (spec), L3 (spec), L8 (INT-22 spec) | Bar rendering, dependency line, progress-fill |
| `docs/component-specs/gantt/dependencies/overview.md` | L3 | Dependency class + token |

---

## Gap Count Verification

| Category | Count | IDs |
|---|---|---|
| **Designed (this document)** | 20 | W4-INT-01..14, 17, 18, 20..25, 26 |
| **Skipped (API decisions)** | 3 | W4-INT-15, 16, 19 |
| **Demo coverage (designed)** | 5 | W4-INT-08..12 |
| **Cross-component (out of scope)** | 4 | W4-ROUTE-01..04 |
| **Already-queued (out of scope)** | 1 | W4-QUEUED-01 |
| **Total accounted** | 26 + 4 + 1 = 31 | Matches intake sections A + B + C |

Designed + Skipped = 20 + 3 = 23. This covers all 26 W4-INT items minus the 3 skipped, giving 23 designed. Wait -- recount:

- L0: 1 (INT-13)
- L1: 8 (INT-01..07, INT-26)
- L2: 5 (INT-08..12)
- L3: 1 (INT-14)
- L4 designed: 3 (INT-17, INT-18, INT-25) -- INT-19 skipped
- L5: 2 (INT-20, INT-21)
- L8: 3 (INT-22, INT-23, INT-24)
- **Designed total: 23**
- **Skipped: 3** (INT-15, INT-16, INT-19)
- **23 + 3 = 26** -- matches intake exactly.

---

## Stage 03 STOP -- end of resolution design. Ready for orchestrator review.
