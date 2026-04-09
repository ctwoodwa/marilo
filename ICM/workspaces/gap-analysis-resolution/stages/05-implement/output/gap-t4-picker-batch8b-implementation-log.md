# Implementation Log — T4 Pickers Batch 8B
**Date:** 2026-04-09
**Component:** MariloTimePicker
**Batch:** 8B (4 gaps)

---

## RES-T4B8B-001 — InputMode Parameter

**Status:** Implemented

**Changes:**
- `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor`
  - Added `[Parameter] public string? InputMode { get; set; }` with XML doc comment
  - Added `inputmode="@InputMode"` attribute to the `<input>` element

**Notes:** Null value causes the attribute to be omitted (standard Blazor attribute omission behaviour). When set to `"none"`, mobile browsers suppress the on-screen keyboard, making the tumbler the sole interaction mode.

---

## RES-T4B8B-002 — ValidateOn Parameter

**Status:** Implemented (parameter surface)

**Changes:**
- `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor`
  - Added `[Parameter] public string? ValidateOn { get; set; }` with XML doc comment

**Notes:** Full EditContext pipeline integration (i.e., calling `EditContext.NotifyFieldChanged`) is deferred as spec-ahead — it requires the component to be inside a `<EditForm>` and hold a `FieldIdentifier`, which needs a refactor to support `MariloInputBase<TValue>` inheritance or an explicit EditContext cascade. The parameter is present on the public API surface so consumers can inspect it. This matches the pattern used by `MariloDateTimePicker` where `ValidateOn` is also listed as a gap awaiting full integration.

---

## RES-T4B8B-003 — OnChange Fires on Blur

**Status:** Implemented

**Changes:**
- `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor`
  - Added `private TValue? _lastEmittedValue;` field
  - Updated `Commit()` to set `_lastEmittedValue = newValue` after emitting OnChange
  - Updated `ClearValue()` to set `_lastEmittedValue = default` after emitting OnChange
  - Updated `OnInputBlur()` to compare `Value` vs `_lastEmittedValue` and conditionally emit `OnChange` before calling `OnBlur`

**Notes:** Comparison uses `EqualityComparer<TValue?>.Default.Equals` to handle value-type TValues correctly (TimeSpan, DateTime, DateTimeOffset, TimeOnly). The OnBlur event still fires unconditionally after the conditional OnChange, preserving existing behaviour for OnBlur consumers.

---

## RES-T4B8B-004 — CSS Provider Integration

**Status:** Implemented

**Changes:**
- `src/Marilo.Core/Contracts/IMariloCssProvider.cs`
  - Added `string TimePickerPopupClass();` after `TimePickerClass()`

- `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs`
  - Added `public string TimePickerPopupClass() => "mar-timepicker__popup";`

- `src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs`
  - Added `public string TimePickerPopupClass() => "dropdown-menu mar-bs-timepicker__popup p-2";`

- `samples/Marilo.Demo/Services/ProviderSwitcher.cs`
  - Added `public string TimePickerPopupClass() => Css.TimePickerPopupClass();`

- `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor`
  - Root `<div>`: replaced `"mar-timepicker"` with `CssProvider.TimePickerClass()`
  - Popup `<div>`: replaced `"mar-timepicker__popup @PopupClass"` with `@CssProvider.TimePickerPopupClass() @PopupClass`

**Notes:** The FluentUI provider returns the same class name (`mar-timepicker`, `mar-timepicker__popup`) so existing SCSS and tests are unaffected. The Bootstrap provider prepends Bootstrap utility classes per the established `mar-bs-*` bridge convention.
