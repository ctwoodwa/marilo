# Closure Report: T4 Pickers Batch 8A

> Date: 2026-04-09
> Components: MariloDateRangePicker (5 gaps), MariloDateTimePicker (1 gap)
> Total gaps: 6
> Status: **CLOSED**

---

## Summary

All 6 gaps in Batch 8A are resolved and verified with passing bUnit tests.

| Gap ID | Description | Status | Tests |
|--------|-------------|--------|-------|
| RES-T4B8A-01 | PopupClass bug — dead ternary on root div | Closed | 2 |
| RES-T4B8A-02 | ShowWeekNumbers — not rendered | Closed | 4 |
| RES-T4B8A-03 | Size/Rounded/FillMode missing | Closed | 6 |
| RES-T4B8A-04 | DebounceDelay and Title missing | Closed | 4 |
| RES-T4B8A-05 | HeaderTemplate missing | Closed | 3 |
| RES-T4B8A-06 | ValidateOn missing on DateTimePicker | Closed | 4 |

**Total tests: 23 — all passing.**

---

## Validation Details

### RES-T4B8A-01: PopupClass Bug

- Root cause: `@(PopupClass is not null ? "" : "")` — both branches returned empty string.
- Fix: removed the broken ternary from the root `<div>`. The popup panel `<div>` already
  correctly applied `@PopupClass` so no functional regression is possible.
- Verification: `PopupClass_IsAppliedToRootElement` opens the popup and confirms the
  custom class is on `.mar-date-range-picker__popup`. `PopupClass_Null_DoesNotBreakRender`
  confirms null renders cleanly.

### RES-T4B8A-02: ShowWeekNumbers

- ISO week numbers rendered via `System.Globalization.ISOWeek.GetWeekOfYear`.
- Both calendar panels (start and end) receive the week column.
- Tests verify: no column by default, "Wk" header × 2 when enabled, cell values in
  range 1–53, CSS modifier class on grid.

### RES-T4B8A-03: Size / Rounded / FillMode

- Pattern matches `MariloColorPicker` exactly (nullable string → CSS modifier class).
- Default values: `md` for Size, `solid` for FillMode, empty for Rounded.
- Tests cover all three params and their null/default behaviors.

### RES-T4B8A-04: DebounceDelay and Title

- `DebounceDelay` defaults to 150ms (matches TimePicker).
- `Title` renders in popup only when non-null and `HeaderTemplate` is absent.
- Tests confirm defaults and rendering behavior.

### RES-T4B8A-05: HeaderTemplate

- `RenderFragment?` parameter — when set, replaces the Title div.
- Priority order: `HeaderTemplate` > `Title` > nothing.
- Three tests cover null (no custom element), provided (renders), and precedence over Title.

### RES-T4B8A-06: ValidateOn (DateTimePicker)

- Declaration matches `MariloTimePicker.razor:222-223` exactly.
- No EditContext wiring (consistent with TimePicker — deferred to a future batch).
- Four tests confirm null default and all three accepted string values.

---

## Files Modified

| File | Change |
|------|--------|
| `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor` | 5 gaps resolved |
| `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor` | 1 gap resolved |

## Files Created

| File | Purpose |
|------|---------|
| `tests/Marilo.Tests.Unit/Selection/T4PickerBatch8ATests.cs` | 23 bUnit tests |
| `ICM/workspaces/gap-analysis-resolution/stages/03-resolution-design/output/gap-t4-picker-batch8a-resolutions.md` | Resolution design |
| `ICM/workspaces/gap-analysis-resolution/stages/05-implement/output/gap-t4-picker-batch8a-implementation-log.md` | Implementation log |
| `ICM/workspaces/gap-analysis-resolution/stages/06-validate/output/gap-t4-picker-batch8a-closure-report.md` | This file |

---

## Guardrails Check

- Every change traces to a documented gap. No opportunistic changes.
- All patterns match sibling components (ColorPicker for Size/Rounded/FillMode, TimePicker for DebounceDelay/ValidateOn).
- No existing parameter names modified.
- `PopupClass` behavior preserved — popup panel still receives the class as before; only the dead root-div ternary was removed.
- ISO week numbers use `System.Globalization.ISOWeek` (correct ISO 8601 — not `Calendar.GetWeekOfYear`).
