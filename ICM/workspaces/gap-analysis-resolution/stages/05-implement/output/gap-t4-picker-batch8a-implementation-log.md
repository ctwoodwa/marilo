# Implementation Log: T4 Pickers Batch 8A — DateRangePicker (5 gaps) + DateTimePicker ValidateOn

> Date: 2026-04-09
> Engineer: Claude (Sonnet 4.6)
> Resolution design: `stages/03-resolution-design/output/gap-t4-picker-batch8a-resolutions.md`
> Status: All 6 gaps implemented. 23 tests passing.

---

## RES-T4B8A-01: PopupClass Bug Fix

**File:** `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`

**Change:** Removed the broken ternary `@(PopupClass is not null ? "" : "")` from the
root `<div>` class attribute. Both branches emitted empty string — dead code.
The popup panel `<div>` at line 68 already correctly appended `@PopupClass`, so `PopupClass`
was effectively wired to the right element — the root div ternary was simply dead noise.
The root class attribute now uses `@SizeClass @RoundedClass @FillModeClass` (added for gap 3)
with the broken ternary fully removed.

**Test:** `DateRangePicker_PopupClass_IsAppliedToRootElement` — verifies popup panel carries
the custom class after opening; `DateRangePicker_PopupClass_Null_DoesNotBreakRender` — verifies
null PopupClass renders without error.

---

## RES-T4B8A-02: ShowWeekNumbers

**File:** `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`

**Change:** Updated `RenderCalendarPanel` to support week-number rendering:

1. Added `mar-calendar__grid--week-numbers` CSS modifier class to the grid `<div>` when
   `ShowWeekNumbers` is true.
2. Added a `mar-calendar__week-number-header` `<span>` ("Wk") as the first item in
   `.mar-calendar__weekdays` when `ShowWeekNumbers` is true.
3. Changed the `@foreach` loop over calendar days to an indexed `for` loop so that
   at the start of each 7-day row (`rowIdx % 7 == 0`), a `mar-calendar__week-number`
   `<span>` is inserted showing the ISO week number via
   `System.Globalization.ISOWeek.GetWeekOfYear(date)`.

Both calendar panels receive the same treatment (the `isStart` parameter selects
navigation buttons only; the grid code is shared).

**Tests:** 4 tests cover: no week column when false, "Wk" header present (×2) when true,
week number cells render with valid 1-53 values, grid carries the CSS modifier class.

---

## RES-T4B8A-03: Size / Rounded / FillMode Appearance Params

**File:** `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`

**Change:** Added three parameters (matching `MariloColorPicker.razor:308-385` pattern):

```csharp
[Parameter] public string? Size { get; set; }
[Parameter] public string? Rounded { get; set; }
[Parameter] public string? FillMode { get; set; }
```

Added three private computed properties:

```csharp
private string SizeClass => Size is { Length: > 0 } s ? $"mar-date-range-picker--{s}" : "mar-date-range-picker--md";
private string RoundedClass => Rounded is { Length: > 0 } r ? $"mar-date-range-picker--rounded-{r}" : "";
private string FillModeClass => FillMode is { Length: > 0 } f ? $"mar-date-range-picker--{f}" : "mar-date-range-picker--solid";
```

Applied to the root `<div>` class attribute: `@SizeClass @RoundedClass @FillModeClass`.

**Tests:** 6 tests cover: default `md`/`solid` classes, custom `sm` size, `flat` fill mode,
`pill` rounded, null rounded emits no rounded class.

---

## RES-T4B8A-04: DebounceDelay and Title

**File:** `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`

**Change:** Added two parameters:

```csharp
[Parameter] public int DebounceDelay { get; set; } = 150;
[Parameter] public string? Title { get; set; }
```

For `Title`: when non-null and non-empty (and `HeaderTemplate` is null), a
`<div class="mar-date-range-picker__title">@Title</div>` is rendered at the top of
the popup panel, above the calendars div.

`DebounceDelay` is stored only (no debounce wiring needed for click-based range selection;
declared at spec parity with `MariloTimePicker`).

**Tests:** `DebounceDelay_DefaultIs150`, `DebounceDelay_CanBeSet`, `Title_Null_DoesNotRenderTitleDiv`,
`Title_Set_RendersInPopup`.

---

## RES-T4B8A-05: HeaderTemplate

**File:** `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`

**Change:** Added parameter:

```csharp
[Parameter] public RenderFragment? HeaderTemplate { get; set; }
```

In the popup render block, above the calendars div:

```razor
@if (HeaderTemplate is not null)
{
    @HeaderTemplate
}
else if (!string.IsNullOrEmpty(Title))
{
    <div class="mar-date-range-picker__title">@Title</div>
}
```

`HeaderTemplate` takes full precedence over `Title`. Neither a default header (month/year
labels) nor the `Title` div renders when `HeaderTemplate` is provided.

**Tests:** `HeaderTemplate_Null_DoesNotRenderCustomHeader`, `HeaderTemplate_Renders_WhenProvided`,
`HeaderTemplate_TakesPrecedenceOverTitle`.

---

## RES-T4B8A-06: ValidateOn — MariloDateTimePicker

**File:** `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor`

**Change:** Added parameter after `AdaptiveMode`, matching `MariloTimePicker.razor:222-223`
exactly:

```csharp
/// <summary>Controls when validation fires. Accepted values: "input", "blur", "change". Null means no automatic validation trigger.</summary>
[Parameter] public string? ValidateOn { get; set; }
```

No EditContext wiring added — consistent with the TimePicker pattern where the parameter
is declared for spec parity but EditContext integration is deferred.

**Tests:** `ValidateOn_Null_ByDefault`, `ValidateOn_CanBeSetToBlur`, `ValidateOn_CanBeSetToChange`,
`ValidateOn_CanBeSetToInput`.

---

## Test Results

All 23 tests pass. Test file:
`tests/Marilo.Tests.Unit/Selection/T4PickerBatch8ATests.cs`

```
Test Run Successful.
Total tests: 23
     Passed: 23
 Total time: 1.13 Seconds
```
