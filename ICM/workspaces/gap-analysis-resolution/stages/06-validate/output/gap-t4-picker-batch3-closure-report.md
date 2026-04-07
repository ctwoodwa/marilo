# Closure Report: T4 Pickers Batch 3 — Cross-Cutting Polish

**Date:** 2026-04-05
**Scope:** batch (cross-cutting across 7 components)
**Component:** T4 Pickers — `src/Marilo.Components/Forms/Inputs/`
**Implementation log:** `stages/05-implement/output/gap-t4-picker-batch3-implementation-log.md`
**Resolution records:** `stages/03-resolution-design/output/gap-t4-picker-batch3-resolutions.md`

## Summary

13 gaps addressed: 12 resolved, 1 won't fix (naming is already correct). All 17 new bUnit tests pass. Full suite 547/547, zero regressions.

## Per-Gap Closure

---

### AdaptiveMode — 7 components

**GAP-*-AdaptiveMode: Shared AdaptiveMode parameter on all T4 pickers**
- Status: **Resolved** (7 components)
- Changed: `ComponentEnums.cs`, all 7 picker files
- Tests: `T4PickerBatch3Tests.cs` :: `AdaptiveMode_Enum_HasExpectedValues`, `TimePicker_AdaptiveMode_Defaults_To_None`, `TimePicker_AdaptiveMode_Can_Be_Set_To_Auto`, `DateTimePicker_AdaptiveMode_Defaults_To_None`, `DateRangePicker_AdaptiveMode_Defaults_To_None`, `ColorPicker_AdaptiveMode_Defaults_To_None`, `MultiSelect_AdaptiveMode_Defaults_To_None`
- Enforcement: 7 bUnit tests; shared enum constrains values; default `None` preserves behavior

---

### ARIA combobox — 3 components (4 inputs)

**GAP-TP-ARIA: TimePicker ARIA combobox compliance**
- Status: **Resolved**
- Changed: `MariloTimePicker.razor`
- Tests: `T4PickerBatch3Tests.cs` :: `TimePicker_Input_Has_Combobox_Role`, `TimePicker_Input_Has_AriaHaspopup_Dialog`, `TimePicker_Input_AriaExpanded_False_WhenClosed`
- Enforcement: 3 bUnit tests verify `role="combobox"`, `aria-haspopup="dialog"`, `aria-expanded`

**GAP-DTP-ARIA: DateTimePicker ARIA combobox compliance**
- Status: **Resolved**
- Changed: `MariloDateTimePicker.razor`
- Tests: `T4PickerBatch3Tests.cs` :: `DateTimePicker_Input_Has_Combobox_Role`
- Enforcement: bUnit test; popup has unique id for `aria-controls`

**GAP-DRP-ARIA: DateRangePicker ARIA combobox compliance**
- Status: **Resolved**
- Changed: `MariloDateRangePicker.razor`
- Tests: `T4PickerBatch3Tests.cs` :: `DateRangePicker_StartInput_Has_Combobox_Role`, `DateRangePicker_BothInputs_Have_Combobox_Role`
- Enforcement: 2 bUnit tests; both start and end inputs have combobox role

---

### CSS provider — 2 components, 4 methods

**GAP-DRP-CSS: DateRangePicker CSS provider methods**
- Status: **Resolved**
- Changed: `IMariloCssProvider.cs`, `FluentUICssProvider.cs`, `BootstrapCssProvider.cs`
- Tests: `T4PickerBatch3Tests.cs` :: `CssProvider_DateRangePickerClass_Returns_String`, `CssProvider_DateRangePickerPopupClass_Returns_String`
- Enforcement: bUnit tests verify non-empty return values from FluentUI provider

**GAP-DTP-CSS: DateTimePicker CSS provider methods**
- Status: **Resolved**
- Changed: same 3 files
- Tests: `T4PickerBatch3Tests.cs` :: `CssProvider_DateTimePickerClass_Returns_String`, `CssProvider_DateTimePickerPopupClass_Returns_String`
- Enforcement: bUnit tests verify non-empty return values from FluentUI provider

---

### Naming

**GAP-MSEL-Naming: MaxVisibleTags vs MaxAllowedTags**
- Status: **Won't fix**
- Rationale: `MaxVisibleTags` is more accurate than spec's `MaxAllowedTags` — it controls tag *visibility*, not selection limit. Users can still select unlimited items.

---

## Aggregate

| Status | Count |
|--------|-------|
| Resolved | 12 |
| Won't fix | 1 |
| **Total** | **13** |

## Test Coverage

- Test file: `tests/Marilo.Tests.Unit/Selection/T4PickerBatch3Tests.cs`
- New tests: 17 bUnit tests
- Full suite: **547/547 passing (zero regressions)**
- Runtime execution: ✅ verified

## Enforcement Guardrails

1. **bUnit tests** — 17 tests covering all resolved gaps
2. **Type safety** — Shared `AdaptiveMode` enum constrains values at compile time
3. **ARIA compliance** — WAI-ARIA 1.2 combobox pattern with `role`, `aria-haspopup`, `aria-expanded`, `aria-controls`
4. **Interface contract** — CSS provider methods on `IMariloCssProvider` ensure all providers must implement them

## Follow-up Items

- AdaptiveMode rendering behavior (Auto mode action sheet on small screens) — deferred until responsive layout infrastructure exists
- `aria-activedescendant` on TimePicker/DateTimePicker — deferred (requires tracked active element in popup)
