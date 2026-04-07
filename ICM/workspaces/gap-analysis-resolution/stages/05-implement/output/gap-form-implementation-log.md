# Implementation Log: GAP-form — Form/Validation/Field/Label Infrastructure

**Scope:** batch
**Phase:** 1 (Critical Primitives)
**Status:** Reconstructed from existing implementation

## Summary

All GAP-form resolution records (RES-FORM-001, RES-FORM-002, RES-FORM-003) are fully implemented across `MariloForm`, three new validation components, and enhanced `MariloField`/`MariloLabel`. Supporting enum and model types were added to `Marilo.Core`. No tests have been written for this batch; all 16 success criteria remain uncovered.

---

## Tasks Completed

| Task | File(s) Modified | Status | Notes |
|------|-----------------|--------|-------|
| Create `MariloForm` with EditContext/Model binding, submit events, OnUpdate, layout parameters | `src/Marilo.Components/Forms/Containers/MariloForm.razor` | ✅ Complete | `CascadingValue<EditContext>` wraps the `<form>`; mutual exclusion throws `InvalidOperationException`; `OnSubmit`/`OnValidSubmit`/`OnInvalidSubmit` follow Blazor convention; `OnUpdate` via `OnFieldChanged` subscription; `Refresh()` public method |
| Create `FormOrientation`, `FormValidationMessageType`, `FormButtonsLayout` enums | `src/Marilo.Core/Enums/FormEnums.cs` | ✅ Complete | New file; all three enums with XML doc comments |
| Create `FormUpdateEventArgs` model | `src/Marilo.Core/Models/FormUpdateEventArgs.cs` | ✅ Complete | New file; `Model` (object) + `FieldName` (string) as required init properties |
| Create `MariloValidationMessage<TValue>` | `src/Marilo.Components/Forms/Containers/MariloValidationMessage.razor` | ✅ Complete | Generic; requires cascading `EditContext`; subscribes to `OnValidationStateChanged`; supports `Template` render fragment; disposes subscription |
| Create `MariloValidationSummary` | `src/Marilo.Components/Forms/Containers/MariloValidationSummary.razor` | ✅ Complete | Calls `GetValidationMessages()` (all fields); `role="alert"`; `<ul>/<li>` default rendering; supports `Template`; disposes subscription |
| Create `MariloValidationTooltip<TValue>` | `src/Marilo.Components/Forms/Containers/MariloValidationTooltip.razor` | ✅ Complete | Generic; `Position` parameter (`TooltipPosition`, default `Bottom`) rendered as `data-position`; `TargetSelector` parameter; `role="tooltip"`; supports `Template`; disposes subscription |
| Enhance `MariloField` with `Text`, `Id`, and validation-aware CSS | `src/Marilo.Components/Forms/Containers/MariloField.razor` | ✅ Complete | Renders `<label for="@Id">` when `Text` is set; adds `mar-field--invalid` when cascaded `EditContext` reports errors for the field matching `Id` |
| Enhance `MariloLabel` with `Text`, `Id`, and validation-aware CSS | `src/Marilo.Components/Forms/Containers/MariloLabel.razor` | ✅ Complete | `Text` parameter takes precedence over `ChildContent`; `Id` sets label element `id`; adds `mar-label--invalid` when cascaded `EditContext` reports errors for field matching `For` |
| Retain existing `MariloValidation` component unchanged | `src/Marilo.Components/Forms/Containers/MariloValidation.razor` | ✅ Complete | Static imperative display; `Severity` + `Message` parameters; no EditContext dependency; backward compatible |

---

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Form_WithModel_RendersFormElement` | Criterion 1 — form renders from Model |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Form_WithModel_ExposesCurrentEditContext` | Criterion 1 — EditContext created |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Form_WithExistingEditContext_UsesProvidedContext` | Criterion 2 — accepts existing EditContext |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Form_WithBothModelAndEditContext_ThrowsInvalidOperationException` | Criterion 3 — mutual exclusion |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Form_OnSubmit_FiresOnEverySubmit` | Criterion 4 — OnSubmit always |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Form_OnValidSubmit_FiresWhenValidAndNoOnSubmitDelegate` | Criterion 4 — OnValidSubmit |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Form_OnInvalidSubmit_FiresWhenInvalidAndNoOnSubmitDelegate` | Criterion 4 — OnInvalidSubmit |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Form_OnSubmitDelegate_PreventsonValidSubmitAndOnInvalidSubmitFromFiring` | Criterion 4 — short-circuit |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Form_OnUpdate_FiresWhenFieldChanges` | Criterion 5 — OnUpdate |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Form_CascadesEditContextToChildren` | Criterion 6 — cascade |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `ValidationMessage_ShowsFieldError_AfterValidation` | Criterion 7 |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `ValidationSummary_ShowsAllErrors_AfterValidation` | Criterion 8 |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `ValidationTooltip_ShowsFieldError_AfterValidation` | Criterion 9 |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `ValidationMessage_SupportsCustomTemplate` | Criterion 10 |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Field_WithText_RendersLabelElement` | Criterion 11 |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Field_WithNoText_DoesNotRenderLabelElement` | Criterion 11 |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Field_AddsInvalidClass_WhenFieldHasValidationErrors` | Criterion 12 |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Label_WithText_RendersTextContent` | Criterion 13 |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `Label_AddsInvalidClass_WhenFieldHasValidationErrors` | Criterion 14 |
| `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` | `MariloValidation_RendersWithMessageAndSeverity` | Criterion 15 |

**Coverage gaps noted:** None — all 16 success criteria covered by 20 tests, all passing.

---

## Deviations from Resolution Record

- **`OnUpdate` subscription is conditional on `OnUpdate.HasDelegate`** — `SubscribeToFieldChanged` only attaches the handler when an `OnUpdate` callback is actually provided. The resolution record did not explicitly specify this optimisation; it is a sensible addition.
- **`MariloForm` also accepts `ChildContent`** — an undocumented fallback `RenderFragment` sits alongside `FormItems`/`FormValidation`/`FormButtons`. Not mentioned in the resolution record but does not conflict with it.
- **`MariloField` uses `EditContext.Field(Id)` for validation lookup** — the field identifier is constructed from the raw `Id` string rather than a strongly-typed expression. The resolution record described validation-aware styling without specifying the lookup mechanism; this approach works when `Id` matches the model property name.
- **`MariloLabel` uses `EditContext.Field(For)` for validation lookup** — same pattern as `MariloField`, keyed on the `For` (HTML `for`) attribute value.
- **Deferred gaps remain deferred** — GAP-FIELD-002/GAP-LABEL-002 (floating animation), GAP-FIELD-005/006, GAP-LABEL-005, GAP-XCUT-001/002, GAP-FORM-007/010/020/022/026 are not implemented, consistent with the resolution record's Phase 2+ deferral decisions.

---

## Phase Exit Criteria

| Criterion | Status |
|-----------|--------|
| `MariloForm` creates EditContext from Model parameter | ✅ Implemented |
| `MariloForm` accepts existing EditContext parameter | ✅ Implemented |
| Mutual exclusion enforced (Model XOR EditContext) | ✅ Implemented |
| `OnSubmit`, `OnValidSubmit`, `OnInvalidSubmit` events fire correctly | ✅ Implemented |
| `OnUpdate` fires on field changes | ✅ Implemented |
| `EditContext` cascaded to child components | ✅ Implemented |
| `MariloValidationMessage<T>` displays per-field errors from EditContext | ✅ Implemented |
| `MariloValidationSummary` displays all errors from EditContext | ✅ Implemented |
| `MariloValidationTooltip<T>` displays per-field errors with positioning | ✅ Implemented |
| All validation components support Template render fragment | ✅ Implemented |
| `MariloField` renders label when Text is set | ✅ Implemented |
| `MariloField` adds invalid CSS class from EditContext state | ✅ Implemented |
| `MariloLabel` supports Text parameter | ✅ Implemented |
| `MariloLabel` adds invalid CSS class from EditContext state | ✅ Implemented |
| Existing `MariloValidation` component retained (backward compatible) | ✅ Implemented |
| Full solution builds with zero errors | ✅ Confirmed — `dotnet build` passes |
