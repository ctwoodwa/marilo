# Gantt S05 Wave 1 Report -- TASK-W4-01 Bar Foundation

**Date:** 2026-04-12
**Worker:** `w-gantt-gap-analysis`
**Task:** TASK-W4-01 (W4-INT-13)
**Status:** COMPLETE

---

## What Was Done

### 1. FluentUI SCSS (`src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss`)

Added `.mar-gantt__bar` and `.mar-gantt__bar-row` base rules before the Milestone section (line 42). Introduced 6 design tokens:

| Token | Default |
|---|---|
| `--marilo-gantt-bar-height` | `24px` |
| `--marilo-gantt-bar-bg` | `var(--colorBrandBackground, #0078d4)` |
| `--marilo-gantt-bar-radius` | `4px` |
| `--marilo-gantt-bar-color` | `var(--colorNeutralForegroundOnBrand, #ffffff)` |
| `--marilo-gantt-bar-font-size` | `12px` |
| `--marilo-gantt-row-height` | `36px` |

### 2. Bootstrap SCSS (`src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss`)

Same structure, Bootstrap tokens:
- `--marilo-gantt-bar-bg` defaults to `var(--bs-primary, #0d6efd)`
- `--marilo-gantt-bar-radius` defaults to `var(--bs-border-radius-sm, 0.25rem)`
- `--marilo-gantt-bar-color` defaults to `#fff`

### 3. Spec (`docs/component-specs/gantt/timeline/overview.md`)

Added "Bar Rendering" section with:
- Class table (`.mar-gantt__bar-row`, `.mar-gantt__bar`)
- Design tokens table (Fluent + Bootstrap defaults)
- Accessibility notes (forced-colors, reduced-motion)

### 4. Tests (`tests/Marilo.Tests.Unit/DataDisplay/MariloGanttTests.cs`)

Added 2 bUnit tests:
- `Gantt_Renders_Bar_With_Base_Class` -- asserts `.mar-gantt__bar` elements exist
- `Gantt_Renders_BarRow_With_Base_Class` -- asserts `.mar-gantt__bar-row` elements exist

---

## Verification

| Check | Result |
|---|---|
| `dotnet build Marilo.slnx` | exit 0, 0 warnings, 0 errors |
| `dotnet test` (Gantt filter) | 94 passed, 0 failed, 0 skipped |
| SCSS compiles | Yes (verified via build) |
| No Razor changes | Confirmed -- class was already emitted |

---

## Acceptance Checklist

- [x] `.mar-gantt__bar` rule in both Fluent and Bootstrap SCSS
- [x] Bar renders with non-zero height (24px), visible background (brand color), border-radius (4px)
- [x] `.mar-gantt__bar-row` rule in both providers with `--marilo-gantt-row-height`
- [x] Spec has "Bar Rendering" section documenting base class and tokens
- [x] bUnit test asserts markup contains `class="mar-gantt__bar"`
- [x] `dotnet build Marilo.slnx` exit 0
- [x] `dotnet test` on Gantt tests passes

---

## Files Changed

| File | Change |
|---|---|
| `src/Marilo.Providers.FluentUI/Styles/components/_gantt.scss` | Added bar base + bar-row rules |
| `src/Marilo.Providers.Bootstrap/Styles/components/_gantt.scss` | Added bar base + bar-row rules |
| `docs/component-specs/gantt/timeline/overview.md` | Added Bar Rendering spec section |
| `tests/Marilo.Tests.Unit/DataDisplay/MariloGanttTests.cs` | Added 2 bUnit tests |
