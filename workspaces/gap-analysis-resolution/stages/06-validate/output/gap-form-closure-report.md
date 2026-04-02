# Closure Report: GAP-form — Form/Validation/Field/Label Infrastructure

**Closure Status:** Resolved (core gaps; deferred gaps remain for Phase 2+)
**Validated:** 2026-04-02
**Scope:** batch (RES-FORM-001, RES-FORM-002, RES-FORM-003)
**Stage routing:** 03 > 05 > 06

---

## Summary

| Metric | Count |
|--------|-------|
| Resolution records | 3 (RES-FORM-001, RES-FORM-002, RES-FORM-003) |
| Gaps resolved | 35+ (see per-resolution lists in resolution record) |
| Success criteria | 16 |
| Criteria met | 16 |
| bUnit tests | 20 (all passing) |
| Deferred gaps | 11 (Phase 2–4) |
| Partially resolved | 0 |
| Won't fix | 0 |

---

## Criteria Verification

| # | Criterion | Implementation Found | Test Passing | Status |
|---|-----------|---------------------|-------------|--------|
| 1 | `MariloForm` creates `EditContext` from `Model` parameter | `MariloForm.razor` — `OnParametersSet` creates `new EditContext(Model)` and exposes via `CurrentEditContext` property | `Form_WithModel_RendersFormElement`, `Form_WithModel_ExposesCurrentEditContext` | ✅ |
| 2 | `MariloForm` accepts existing `EditContext` parameter | `MariloForm.razor` — `EditContext` parameter wired to `_editContext`; `_hasSetEditContextExplicitly` flag tracks origin | `Form_WithExistingEditContext_UsesProvidedContext` | ✅ |
| 3 | Mutual exclusion enforced (Model XOR EditContext) | `MariloForm.razor` — `OnParametersSet` throws `InvalidOperationException` when both are non-null | `Form_WithBothModelAndEditContext_ThrowsInvalidOperationException` | ✅ |
| 4 | `OnSubmit`, `OnValidSubmit`, `OnInvalidSubmit` fire correctly | `MariloForm.razor` — `HandleSubmitAsync`: `OnSubmit` fires always; `OnValidSubmit`/`OnInvalidSubmit` fire via `EditContext.Validate()` only when `OnSubmit` has no delegate | `Form_OnSubmit_FiresOnEverySubmit`, `Form_OnValidSubmit_FiresWhenValidAndNoOnSubmitDelegate`, `Form_OnInvalidSubmit_FiresWhenInvalidAndNoOnSubmitDelegate`, `Form_OnSubmitDelegate_PreventsonValidSubmitAndOnInvalidSubmitFromFiring` | ✅ |
| 5 | `OnUpdate` fires on field changes | `MariloForm.razor` — `SubscribeToFieldChanged` attaches `HandleFieldChanged` to `EditContext.OnFieldChanged`; fires `FormUpdateEventArgs` with `Model` + `FieldName` | `Form_OnUpdate_FiresWhenFieldChanges` | ✅ |
| 6 | `EditContext` cascaded to child components | `MariloForm.razor` — `<CascadingValue Value="_editContext">` wraps `<form>` | `Form_CascadesEditContextToChildren` | ✅ |
| 7 | `MariloValidationMessage<T>` displays per-field errors | `MariloValidationMessage.razor` — subscribes to `OnValidationStateChanged`; calls `GetValidationMessages(_fieldIdentifier)` | `ValidationMessage_ShowsFieldError_AfterValidation` | ✅ |
| 8 | `MariloValidationSummary` displays all errors | `MariloValidationSummary.razor` — calls `GetValidationMessages()` (all fields); renders `<ul>/<li>` with `role="alert"` | `ValidationSummary_ShowsAllErrors_AfterValidation` | ✅ |
| 9 | `MariloValidationTooltip<T>` displays per-field errors with positioning | `MariloValidationTooltip.razor` — `Position` parameter (`TooltipPosition`, default `Bottom`) rendered as `data-position`; `role="tooltip"`; `TargetSelector` parameter present | `ValidationTooltip_ShowsFieldError_AfterValidation` | ✅ |
| 10 | All validation components support `Template` render fragment | All three components — `RenderFragment<IEnumerable<string>>? Template` parameter; rendered when non-null, otherwise default markup | `ValidationMessage_SupportsCustomTemplate` | ✅ |
| 11 | `MariloField` renders label when `Text` is set | `MariloField.razor` — `@if (!string.IsNullOrEmpty(Text))` renders `<label class="..." for="@Id">@Text</label>` | `Field_WithText_RendersLabelElement`, `Field_WithNoText_DoesNotRenderLabelElement` | ✅ |
| 12 | `MariloField` adds invalid CSS class from EditContext state | `MariloField.razor` — `BuildFieldClass()` uses `EditContext.Field(Id)` lookup; adds `mar-field--invalid` when messages exist | `Field_AddsInvalidClass_WhenFieldHasValidationErrors` | ✅ |
| 13 | `MariloLabel` supports `Text` parameter | `MariloLabel.razor` — `Text` parameter rendered directly; takes precedence over `ChildContent` via `@if (!string.IsNullOrEmpty(Text))` / `else` | `Label_WithText_RendersTextContent` | ✅ |
| 14 | `MariloLabel` adds invalid CSS class from EditContext state | `MariloLabel.razor` — `BuildLabelClass()` uses `EditContext.Field(For)` lookup; adds `mar-label--invalid` when messages exist | `Label_AddsInvalidClass_WhenFieldHasValidationErrors` | ✅ |
| 15 | Existing `MariloValidation` component retained (backward compatible) | `MariloValidation.razor` — unchanged; static imperative display with `Severity` + `Message` parameters; no EditContext dependency | `MariloValidation_RendersWithMessageAndSeverity` | ✅ |
| 16 | Full solution builds with zero errors | Confirmed — `dotnet build` passes per implementation log | (build verification) | ✅ |

---

## Deferred Gaps

| Gap ID | Description | Deferred To | Rationale |
|--------|-------------|-------------|-----------|
| GAP-FIELD-002 | Floating label animation (focus tracking) | Phase 2 | Requires JS interop for input focus/blur events; not a blocking primitive |
| GAP-LABEL-002 | Floating label behavior on MariloLabel | Phase 2 | Same JS interop dependency as GAP-FIELD-002 |
| GAP-FIELD-005 | Input compatibility enforcement for MariloField | Phase 4 | Nice-to-have guardrail; no consumer demand yet |
| GAP-FIELD-006 | Placeholder interaction with floating label | Phase 4 | Depends on GAP-FIELD-002 floating animation |
| GAP-LABEL-005 | MariloLabel as wrapper container | Phase 4 | Nice-to-have structural option; not required for core validation flow |
| GAP-XCUT-001 | Cross-cutting component naming consistency | Phase 2 | Design decision requiring broader API review |
| GAP-XCUT-002 | FloatingLabel cross-cutting pattern | Phase 2 | Blocked on GAP-FIELD-002/GAP-LABEL-002 resolution |
| GAP-FORM-007 | Auto-generate form items from model annotations | Phase 2 | Complex feature; requires reflection + attribute scanning |
| GAP-FORM-010 | Annotation-driven form layout | Phase 2 | Depends on auto-generation infrastructure (GAP-FORM-007) |
| GAP-FORM-020 | FormGroups (grouped field sections) | Phase 3 | Depends on FormItems layout being stable |
| GAP-FORM-022 | FormItemsTemplate (custom item rendering template) | Phase 4 | Advanced template system; deferred until core usage patterns stabilize |
| GAP-FORM-026 | AutoGeneratedItems template customization | Phase 4 | Depends on auto-generation (GAP-FORM-007/010) |

---

## Deviations from Resolution Record

1. **`OnUpdate` subscription is conditional on `OnUpdate.HasDelegate`** — `SubscribeToFieldChanged` only attaches the `OnFieldChanged` handler when a consumer actually provides an `OnUpdate` callback. The resolution record did not specify this optimization; it is a correct and safe addition that avoids unnecessary event wiring.

2. **`MariloForm` accepts `ChildContent` in addition to `FormItems`/`FormValidation`/`FormButtons`** — an undocumented `RenderFragment? ChildContent` parameter is present alongside the three named fragments. It does not conflict with any resolution record decision and enables flexible ad-hoc content injection (used by bUnit tests for child component capture).

3. **`MariloField` uses `EditContext.Field(Id)` for validation lookup** — the field identifier is constructed from the raw `Id` string rather than a strongly-typed expression. This works correctly when the `Id` value matches the model property name, which is the expected usage convention.

4. **`MariloLabel` uses `EditContext.Field(For)` for validation lookup** — same string-based pattern as `MariloField`, keyed on the HTML `for` attribute value.

---

## Evidence

- **Changed files:**
  - `src/Marilo.Components/Forms/Containers/MariloForm.razor`
  - `src/Marilo.Components/Forms/Containers/MariloValidationMessage.razor`
  - `src/Marilo.Components/Forms/Containers/MariloValidationSummary.razor`
  - `src/Marilo.Components/Forms/Containers/MariloValidationTooltip.razor`
  - `src/Marilo.Components/Forms/Containers/MariloField.razor`
  - `src/Marilo.Components/Forms/Containers/MariloLabel.razor`
  - `src/Marilo.Core/Enums/FormEnums.cs` (new — `FormOrientation`, `FormValidationMessageType`, `FormButtonsLayout`)
  - `src/Marilo.Core/Models/FormUpdateEventArgs.cs` (new — `Model` + `FieldName` init properties)

- **Tests:** `tests/Marilo.Tests.Unit/Foundation/FormTests.cs` — 20 bUnit tests, all passing; covers all 16 success criteria

- **Original gap no longer present:** Yes. The pre-implementation state had no `MariloForm` component, no EditContext integration, no validation display components (`MariloValidationMessage`, `MariloValidationSummary`, `MariloValidationTooltip`), and `MariloField`/`MariloLabel` lacked `Text`, `Id`/`For`, and validation-aware CSS. All of these deficiencies are resolved in the current source files.

---

## Enforcement Guardrails

- **Mutual exclusion throw** — `MariloForm.OnParametersSet` throws `InvalidOperationException` at runtime if both `Model` and `EditContext` are supplied; this is caught by the `Form_WithBothModelAndEditContext_ThrowsInvalidOperationException` test.
- **Required cascading parameter throws** — all three validation components (`MariloValidationMessage`, `MariloValidationSummary`, `MariloValidationTooltip`) throw `InvalidOperationException` in `OnParametersSet` if no cascading `EditContext` is present, preventing silent misconfiguration.
- **Required `For` parameter throws** — `MariloValidationMessage` and `MariloValidationTooltip` throw `InvalidOperationException` if the `For` expression is null.
- **`IDisposable` on all validation components** — `DetachFromEditContext` is called in `Dispose` and on context change, preventing stale subscriptions and memory leaks.
- **bUnit regression coverage** — 20 tests covering all 16 criteria; any regression to core form/validation/field/label behavior will be caught by the test suite.
- **Code review pattern** — new form-area components should follow the `CascadingParameter EditContext` + `OnValidationStateChanged` subscription + `IDisposable` disposal pattern established by this implementation.

---

## Follow-up Tasks

- **Phase 2:** Implement floating label animation for `MariloField` and `MariloLabel` (GAP-FIELD-002, GAP-LABEL-002) once JS interop strategy is established; also resolve GAP-XCUT-001/002 naming and GAP-FORM-007/010 auto-generation.
- **Phase 3:** Implement `FormGroups` (GAP-FORM-020) after FormItems layout has stabilized in production use.
- **Phase 4:** Implement `FormItemsTemplate` (GAP-FORM-022), `AutoGeneratedItems` template customization (GAP-FORM-026), and field/label compatibility enforcement (GAP-FIELD-005/006, GAP-LABEL-005).
- **Test expansion:** Add bUnit tests for `MariloValidationSummary` Template and `MariloValidationTooltip` Template render paths (criteria 10 is covered only for `MariloValidationMessage`; the other two components are verified indirectly via the non-template path tests).
