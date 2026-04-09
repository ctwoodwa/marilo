# Gap Resolution Design — T4 Pickers Batch 8B
**Date:** 2026-04-09
**Component:** MariloTimePicker
**Batch:** 8B (4 gaps)

---

## RES-T4B8B-001 — InputMode Parameter

**Gap:** MariloTimePicker is missing an `InputMode` parameter. The spec requires a way to control the input mode on the text input, e.g., `"text"` for typed input or `"none"` to suppress the on-screen keyboard on mobile (tumbler-only mode).

**Resolution:** Add `[Parameter] public string? InputMode { get; set; }` to the component. Apply as the `inputmode` HTML attribute on the `<input>` element. Null means the attribute is omitted (browser default).

**Files changed:**
- `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor`

**Type:** new-parameter
**Priority:** P2

---

## RES-T4B8B-002 — ValidateOn Parameter

**Gap:** MariloTimePicker is missing a `ValidateOn` parameter. The spec requires control over when validation fires. The component inherits `MariloComponentBase` (not `MariloInputBase<TValue>`), so there is no inherited `ValidateOn`.

**Resolution:** Add `[Parameter] public string? ValidateOn { get; set; }` as a standalone parameter. Accepted values: `"input"`, `"blur"`, `"change"`. The parameter is stored and available for consumers to read; full EditContext/validation pipeline integration is deferred as it requires an explicit Blazor EditContext hook (spec-ahead). The parameter presence on the API surface satisfies the spec contract.

**Files changed:**
- `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor`

**Type:** new-parameter
**Priority:** P2

---

## RES-T4B8B-003 — OnChange Fires on Blur

**Gap:** The `OnChange` callback does not fire when the component loses focus (blur). The spec requires OnChange to fire on blur if the value changed since the last emitted change event.

**Resolution:** Track `_lastEmittedValue` (private field). In `OnInputBlur`, compare `Value` against `_lastEmittedValue` via `EqualityComparer<TValue?>.Default.Equals`. If they differ, emit `OnChange` with the current value and update `_lastEmittedValue`. Also update `_lastEmittedValue` in `Commit()` and `ClearValue()` to keep it in sync with explicit change events.

**Files changed:**
- `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor`

**Type:** behavior-fix
**Priority:** P1

---

## RES-T4B8B-004 — CSS Provider Integration (TimePickerClass / TimePickerPopupClass)

**Gap:** `IMariloCssProvider.TimePickerClass()` exists but the component ignores it, using the hardcoded BEM class `"mar-timepicker"` directly. The spec also requires `TimePickerPopupClass()` for the popup element, but that method does not yet exist in the interface.

**Resolution:**
1. Add `string TimePickerPopupClass()` to `IMariloCssProvider`.
2. Implement in `FluentUICssProvider` → `"mar-timepicker__popup"`.
3. Implement in `BootstrapCssProvider` → `"dropdown-menu mar-bs-timepicker__popup p-2"`.
4. Add delegation to `ProviderSwitcher`.
5. In the root `<div>` replace `"mar-timepicker"` with `CssProvider.TimePickerClass()`.
6. In the popup `<div>` replace `"mar-timepicker__popup @PopupClass"` with `@CssProvider.TimePickerPopupClass() @PopupClass`.

The hardcoded BEM class `mar-timepicker` is preserved through the FluentUI provider return value; structural behaviour is unchanged.

**Files changed:**
- `src/Marilo.Core/Contracts/IMariloCssProvider.cs`
- `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs`
- `src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs`
- `samples/Marilo.Demo/Services/ProviderSwitcher.cs`
- `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor`

**Type:** css-provider-wiring
**Priority:** P2
