# Implementation Log: T4 Pickers Batch 3 — Cross-Cutting Polish

**Scope:** batch (cross-cutting across 7 components)
**Phase:** Batch 3 (Low-severity polish)
**Status:** Complete
**Date:** 2026-04-05

## Summary

Resolved 13 gaps across 7 components: AdaptiveMode (7), ARIA combobox (3 components, 4 inputs), CSS provider (2 components, 4 methods), naming (1, won't fix). All changes build clean. 547/547 tests pass (17 new + 530 existing, zero regressions).

## Tasks Completed

| Task | File(s) Modified | Status | Notes |
|------|-----------------|--------|-------|
| Create shared `AdaptiveMode` enum | `src/Marilo.Core/Enums/ComponentEnums.cs` | ✅ | None, Auto values |
| AdaptiveMode → MariloColorPicker | `src/Marilo.Components/Forms/Inputs/MariloColorPicker.razor` | ✅ | Parameter added |
| AdaptiveMode → MariloDateRangePicker | `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor` | ✅ | Parameter added |
| AdaptiveMode → MariloDateTimePicker | `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor` | ✅ | Parameter added |
| AdaptiveMode → MariloTimePicker | `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor` | ✅ | Parameter added |
| AdaptiveMode → MariloFileUpload | `src/Marilo.Components/Forms/Inputs/MariloFileUpload.razor.cs` | ✅ | Added using + parameter |
| AdaptiveMode → MariloUpload | `src/Marilo.Components/Forms/Inputs/MariloUpload.razor.cs` | ✅ | Added using + parameter |
| AdaptiveMode → MariloMultiSelect | `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` | ✅ | Parameter added |
| ARIA combobox → TimePicker | `MariloTimePicker.razor` | ✅ | role="combobox", aria-controls, popup id |
| ARIA combobox → DateTimePicker | `MariloDateTimePicker.razor` | ✅ | role="combobox", aria-controls, popup id |
| ARIA combobox → DateRangePicker | `MariloDateRangePicker.razor` | ✅ | Both inputs: role="combobox", aria-controls, popup id |
| CSS provider: DateRangePicker methods | IMariloCssProvider + FluentUI + Bootstrap | ✅ | 4 new interface methods + 2 implementations |
| CSS provider: DateTimePicker methods | IMariloCssProvider + FluentUI + Bootstrap | ✅ | Same |
| MaxVisibleTags naming | — | Won't fix | Current name is more accurate than spec's MaxAllowedTags |
| Fix Transitions bool? build error | `MariloChart.razor` | ✅ | `Transitions ?? true` (from prior session) |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| `T4PickerBatch3Tests.cs` | `AdaptiveMode_Enum_HasExpectedValues` | Enum values (None=0, Auto=1) |
| | `TimePicker_AdaptiveMode_Defaults_To_None` | TimePicker default |
| | `TimePicker_AdaptiveMode_Can_Be_Set_To_Auto` | TimePicker explicit set |
| | `DateTimePicker_AdaptiveMode_Defaults_To_None` | DateTimePicker default |
| | `DateRangePicker_AdaptiveMode_Defaults_To_None` | DateRangePicker default |
| | `ColorPicker_AdaptiveMode_Defaults_To_None` | ColorPicker default |
| | `MultiSelect_AdaptiveMode_Defaults_To_None` | MultiSelect default |
| | `TimePicker_Input_Has_Combobox_Role` | ARIA combobox role |
| | `TimePicker_Input_Has_AriaHaspopup_Dialog` | aria-haspopup upgrade |
| | `TimePicker_Input_AriaExpanded_False_WhenClosed` | aria-expanded state |
| | `DateTimePicker_Input_Has_Combobox_Role` | DTP ARIA combobox |
| | `DateRangePicker_StartInput_Has_Combobox_Role` | DRP start input ARIA |
| | `DateRangePicker_BothInputs_Have_Combobox_Role` | DRP both inputs ARIA |
| | `CssProvider_DateRangePickerClass_Returns_String` | Provider method exists |
| | `CssProvider_DateTimePickerClass_Returns_String` | Provider method exists |
| | `CssProvider_DateRangePickerPopupClass_Returns_String` | Popup provider method |
| | `CssProvider_DateTimePickerPopupClass_Returns_String` | Popup provider method |

**Total new tests:** 17 bUnit tests (all passing)
**Full suite:** 547/547 passing (zero regressions)

## Files Changed

| File | Change Type | Reason |
|------|-------------|--------|
| `src/Marilo.Core/Enums/ComponentEnums.cs` | edit | Added `AdaptiveMode` enum |
| `src/Marilo.Core/Contracts/IMariloCssProvider.cs` | edit | 4 new interface methods |
| `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs` | edit | 4 new implementations |
| `src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs` | edit | 4 new implementations |
| `src/Marilo.Components/Forms/Inputs/MariloColorPicker.razor` | edit | AdaptiveMode param |
| `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor` | edit | AdaptiveMode + ARIA combobox |
| `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor` | edit | AdaptiveMode + ARIA combobox |
| `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor` | edit | AdaptiveMode + ARIA combobox |
| `src/Marilo.Components/Forms/Inputs/MariloFileUpload.razor.cs` | edit | AdaptiveMode param |
| `src/Marilo.Components/Forms/Inputs/MariloUpload.razor.cs` | edit | AdaptiveMode param |
| `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` | edit | AdaptiveMode param |
| `src/Marilo.Components/Charts/MariloChart.razor` | edit | Fix Transitions bool? build error |
| `tests/Marilo.Tests.Unit/Selection/T4PickerBatch3Tests.cs` | new | 17 bUnit tests |
