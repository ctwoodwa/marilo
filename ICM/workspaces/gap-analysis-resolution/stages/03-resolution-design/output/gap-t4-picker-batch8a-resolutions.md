# Resolution Records: T4 Pickers Batch 8A — DateRangePicker (5 gaps) + DateTimePicker ValidateOn

> Date: 2026-04-09
> Source: Batch 8A task specification
> Components: MariloDateRangePicker (5 gaps), MariloDateTimePicker (1 gap)
> Scope: batch (skips Stage 04 per workspace gap-scope routing)

---

## RES-T4B8A-01: PopupClass Bug Fix — MariloDateRangePicker

**Resolves:** GAP-DRP-POPUPCLASS-001
**Status:** Implemented

### Description

`PopupClass` parameter is declared but the ternary in the root `<div>` class attribute
emits an empty string on both branches — the parameter value is never applied.
Line 3 of the razor file: `@(PopupClass is not null ? "" : "")` — both branches return `""`.

### Decision

Fix the root `<div>` ternary so the non-null branch emits the value of `PopupClass`.
The popup panel `<div>` on line 68 already correctly uses `@PopupClass` as a trailing
class token, so no change is needed there. Only the root wrapper has the bug.

### Files Modified

- `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor` (line 3)

---

## RES-T4B8A-02: ShowWeekNumbers — MariloDateRangePicker

**Resolves:** GAP-DRP-WEEKNUMBERS-001
**Status:** Implemented

### Description

`ShowWeekNumbers` parameter is declared but never rendered. The calendar panel shows
no week-number column regardless of the parameter value.

### Decision

When `ShowWeekNumbers` is true, render a leading week-number column in each calendar
panel. ISO 8601 week numbers are computed via `ISOWeek.GetWeekOfYear(date)` from
`System.Globalization`. The column renders as a `<span>` header cell (labelled "Wk")
followed by one `<span>` per row showing the ISO week number of the first day of that
row. This matches the visual pattern used in sibling components. The column is added
inside `RenderCalendarPanel` inside the `mar-calendar__weekdays` header and repeated
per calendar row.

### Files Modified

- `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`

---

## RES-T4B8A-03: Size / Rounded / FillMode Appearance Params — MariloDateRangePicker

**Resolves:** GAP-DRP-APPEARANCE-001
**Status:** Implemented

### Description

Sibling pickers (ColorPicker) expose `Size`, `Rounded`, and `FillMode` string parameters
that map to CSS modifier classes. DateRangePicker is missing all three.

### Decision

Add parameters matching the ColorPicker pattern:
- `[Parameter] public string? Size { get; set; }` — emits `mar-date-range-picker--{size}` (default `md` when null/empty)
- `[Parameter] public string? Rounded { get; set; }` — emits `mar-date-range-picker--rounded-{rounded}` (empty when null)
- `[Parameter] public string? FillMode { get; set; }` — emits `mar-date-range-picker--{fillMode}` (default `solid` when null/empty)

Private computed properties `SizeClass`, `RoundedClass`, `FillModeClass` are added and
applied to the root `<div>` class attribute — same pattern as `MariloColorPicker.razor:383-385`.

### Files Modified

- `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`

---

## RES-T4B8A-04: DebounceDelay and Title Params — MariloDateRangePicker

**Resolves:** GAP-DRP-DEBOUNCE-TITLE-001
**Status:** Implemented

### Description

`DebounceDelay` (int, ms) and `Title` (string?) parameters are missing. `DebounceDelay`
exists on `MariloTimePicker` (default 150ms). `Title` should render as a header label
inside the popup, above the calendar grids.

### Decision

- Add `[Parameter] public int DebounceDelay { get; set; } = 150;` — stored for future
  debounce use on value-change callbacks. No debounce is currently wired for the range
  picker (range selection is click-based, not text-input-based), but the parameter is
  declared at spec parity with TimePicker so consumers can pass it without error.
- Add `[Parameter] public string? Title { get; set; }` — when non-null/non-empty,
  render a `<div class="mar-date-range-picker__title">` element at the top of the
  popup `<div>`, above the `mar-date-range-picker__calendars` div. When null, nothing
  is rendered (no default text).

### Files Modified

- `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`

---

## RES-T4B8A-05: HeaderTemplate — MariloDateRangePicker

**Resolves:** GAP-DRP-HEADERTEMPLATE-001
**Status:** Implemented

### Description

`HeaderTemplate` (`RenderFragment?`) is missing. When provided, it should render at the
top of the popup above the calendar grids. When null, a default header showing the
current month/year labels for both calendars should render.

### Decision

Add `[Parameter] public RenderFragment? HeaderTemplate { get; set; }`.

Render logic in the popup:
- When `HeaderTemplate` is not null: render `@HeaderTemplate` at the top of the popup,
  before the calendars div.
- When `HeaderTemplate` is null: render a default header
  `<div class="mar-date-range-picker__default-header">` containing two `<span>` elements
  showing `_startDisplayMonth.ToString("MMMM yyyy")` and `_endDisplayMonth.ToString("MMMM yyyy")`.

Note: the per-calendar month title in `mar-calendar__header` is retained regardless —
`HeaderTemplate` is a supplementary header above the entire dual-calendar layout.

### Files Modified

- `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`

---

## RES-T4B8A-06: ValidateOn Parameter — MariloDateTimePicker

**Resolves:** GAP-DTP-VALIDATEON-001
**Status:** Implemented

### Description

`ValidateOn` parameter is missing from `MariloDateTimePicker`. `MariloTimePicker`
already declares this parameter at line 223.

### Decision

Add `[Parameter] public string? ValidateOn { get; set; }` to `MariloDateTimePicker`
following the exact declaration from `MariloTimePicker.razor:222-223`:

```csharp
/// <summary>Controls when validation fires. Accepted values: "input", "blur", "change". Null means no automatic validation trigger.</summary>
[Parameter] public string? ValidateOn { get; set; }
```

The parameter is stored for future EditContext integration (no wiring needed now —
matches the TimePicker pattern where it is also declared but not yet wired to an
EditContext).

### Files Modified

- `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor`
