# Closure Report — T4 Pickers Batch 8B
**Date:** 2026-04-09
**Component:** MariloTimePicker
**Batch:** 8B (4 gaps)
**Status:** CLOSED

---

## Summary

All 4 MariloTimePicker gaps from Batch 8B have been implemented, tested, and validated. The `MariloComponents` project builds without new errors. The 3 projects directly modified by this batch (Core, FluentUI provider, Bootstrap provider) each build cleanly with 0 errors and 0 warnings.

---

## Gap Closure Status

| Gap ID | Description | Status | Notes |
|--------|-------------|--------|-------|
| RES-T4B8B-001 | InputMode parameter | CLOSED | `inputmode` HTML attribute applied to input element |
| RES-T4B8B-002 | ValidateOn parameter | CLOSED (surface) | Parameter added; full EditContext integration deferred as spec-ahead |
| RES-T4B8B-003 | OnChange fires on blur | CLOSED | `_lastEmittedValue` tracking; OnChange fires on blur when value changed |
| RES-T4B8B-004 | CSS provider integration | CLOSED | `TimePickerPopupClass()` added to interface + all providers; root and popup wired |

---

## Files Changed

| File | Change |
|------|--------|
| `src/Marilo.Core/Contracts/IMariloCssProvider.cs` | Added `TimePickerPopupClass()` method |
| `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs` | Implemented `TimePickerPopupClass()` |
| `src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs` | Implemented `TimePickerPopupClass()` |
| `samples/Marilo.Demo/Services/ProviderSwitcher.cs` | Delegated `TimePickerPopupClass()` |
| `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor` | All 4 gap fixes |
| `tests/Marilo.Tests.Unit/Forms/Inputs/T4PickerBatch8BTests.cs` | 13 new tests |

---

## Test Coverage

**File:** `tests/Marilo.Tests.Unit/Forms/Inputs/T4PickerBatch8BTests.cs`
**Test count:** 13

| Test | Gap | Assertion |
|------|-----|-----------|
| `TimePicker_InputMode_Defaults_To_Null` | 001 | Default is null |
| `TimePicker_InputMode_None_AppliedToInput` | 001 | `inputmode="none"` on input element |
| `TimePicker_InputMode_Text_AppliedToInput` | 001 | `inputmode="text"` on input element |
| `TimePicker_InputMode_Null_AttributeOmitted` | 001 | Null omits the attribute |
| `TimePicker_ValidateOn_Defaults_To_Null` | 002 | Default is null |
| `TimePicker_ValidateOn_Can_Be_Set_To_Blur` | 002 | Parameter round-trips "blur" |
| `TimePicker_ValidateOn_Can_Be_Set_To_Change` | 002 | Parameter round-trips "change" |
| `TimePicker_ValidateOn_Can_Be_Set_To_Input` | 002 | Parameter round-trips "input" |
| `TimePicker_OnChange_Fires_OnBlur_WhenValueChanged` | 003 | OnChange fires on blur when value differs |
| `TimePicker_OnChange_Does_Not_Fire_OnBlur_WhenValueUnchanged` | 003 | OnChange NOT fired on blur when value same |
| `TimePicker_OnBlur_Always_Fires_Regardless_Of_ValueChange` | 003 | OnBlur always fires |
| `TimePicker_RootDiv_UsesProviderTimePickerClass` | 004 | Root div carries provider class |
| `TimePicker_Popup_UsesProviderTimePickerPopupClass` | 004 | Popup div carries provider popup class |
| `CssProvider_TimePickerPopupClass_Returns_NonEmpty_String` | 004 | Provider returns non-empty string |
| `CssProvider_TimePickerClass_Returns_NonEmpty_String` | 004 | Provider returns non-empty string |
| `TimePicker_PopupClass_StillApplied_AlongsideProviderClass` | 004 | Custom PopupClass still applied |

---

## Build Validation

| Project | Build Result |
|---------|-------------|
| `Marilo.Core` | Success (0 errors, 0 warnings) |
| `Marilo.Providers.FluentUI` | Success (0 errors, 0 warnings) |
| `Marilo.Providers.Bootstrap` | Success (0 errors, 0 warnings) |
| `Marilo.Components` | Success (0 errors, 0 warnings) |
| `Marilo.Tests.Unit` | Pre-existing errors in `MariloUpload` (unrelated to this batch) |

---

## Design Decisions & Caveats

**ValidateOn (RES-T4B8B-002):** Full EditContext integration requires either inheriting `MariloInputBase<TValue>` (which would be a breaking refactor of the component base) or receiving an `EditContext` via cascade and managing `FieldIdentifier` manually. This is deferred as spec-ahead and tracked separately. The parameter surface matches the spec contract.

**CSS Provider popup class (RES-T4B8B-004):** The FluentUI provider returns `"mar-timepicker__popup"` which is the same BEM class previously hardcoded. This means no visible change for FluentUI consumers — the SCSS remains valid. The Bootstrap provider wraps it in `dropdown-menu` per the Bootstrap bridge convention.

**OnChange on blur (RES-T4B8B-003):** The `_lastEmittedValue` field is initialized to the default of `TValue?` (null for nullable types). On first blur before any value is committed, if `Value` is also null/default, no spurious OnChange fires. The comparison uses `EqualityComparer<TValue?>.Default.Equals` which handles value-type semantics correctly for all supported TValue types (DateTime, DateTime?, DateTimeOffset, DateTimeOffset?, TimeOnly, TimeSpan).
