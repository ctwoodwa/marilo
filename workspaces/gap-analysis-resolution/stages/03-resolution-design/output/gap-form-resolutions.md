# Resolution Records: Forms/Containers Batch

## Summary

Resolved the core Form/Validation infrastructure covering backlog Phases 1-2 critical and important gaps. Created EditContext-integrated MariloForm, three new validation components, and enhanced MariloField/MariloLabel with validation-aware styling.

---

### RES-FORM-001: MariloForm with EditContext, Model binding, submit events, and child components

**Resolves:** GAP-FORM-001 (Model), GAP-FORM-002 (EditContext), GAP-FORM-003 (ValidationMessageType), GAP-FORM-004/005/006 (submit events), GAP-FORM-008/028 (FormValidation child), GAP-FORM-009 (FormItems), GAP-FORM-011 (AutoComplete), GAP-FORM-012 (Id), GAP-FORM-013 (Columns), GAP-FORM-014 (Orientation), GAP-FORM-015 (Size), GAP-FORM-016 (Width), GAP-FORM-017 (OnUpdate), GAP-FORM-018 (Refresh), GAP-FORM-019 (EditContext property), GAP-FORM-021 (FormButtons), GAP-FORM-023/024 (ColumnSpacing/RowSpacing), GAP-FORM-025 (ButtonsLayout)
**Status:** Implemented

#### Target Pattern

```razor
<MariloForm Model="@person" OnValidSubmit="@HandleValidSubmit" OnInvalidSubmit="@HandleInvalid">
    <FormValidation>
        <DataAnnotationsValidator />
    </FormValidation>
    <FormItems>
        <MariloField Text="Name" Id="name">
            <MariloTextField @bind-Value="person.Name" Id="name" />
        </MariloField>
    </FormItems>
    <FormButtons>
        <MariloButton Type="submit">Submit</MariloButton>
    </FormButtons>
</MariloForm>
```

#### Decision

Implemented Blazor's standard EditContext pattern:
- `Model` parameter creates an internal `EditContext`; `EditContext` parameter accepts an existing one
- Mutual exclusion enforced via `InvalidOperationException`
- `EditContext` cascaded as `CascadingValue` for child validation components
- Submit events follow Blazor convention: `OnSubmit` fires always; `OnValidSubmit`/`OnInvalidSubmit` fire based on `EditContext.Validate()` only when `OnSubmit` has no delegate
- `OnUpdate` fires via `EditContext.OnFieldChanged` subscription
- `FormValidation`, `FormItems`, `FormButtons` are `RenderFragment` parameters
- Layout parameters (`Columns`, `Orientation`, `Size`, `Width`, etc.) stored as parameters for CSS provider/template use

#### New Types Created

| Type | File | Purpose |
|------|------|---------|
| `FormOrientation` enum | `Core/Enums/FormEnums.cs` | Vertical/Horizontal layout |
| `FormValidationMessageType` enum | `Core/Enums/FormEnums.cs` | Inline/Tooltip/None display mode |
| `FormButtonsLayout` enum | `Core/Enums/FormEnums.cs` | Start/Center/End/Stretch alignment |
| `FormUpdateEventArgs` class | `Core/Models/FormUpdateEventArgs.cs` | Model + FieldName for OnUpdate |

---

### RES-FORM-002: Three validation components with EditContext integration

**Resolves:** GAP-VAL-001 (For parameter), GAP-VAL-002 (EditContext integration), GAP-VAL-003 (three components), GAP-VAL-004 (ValidationSummary), GAP-VAL-005/006/007 (ValidationTooltip + For + TargetSelector), GAP-VAL-008 (Template), GAP-VAL-009 (Position), GAP-VAL-010/011 (Templates), GAP-VAL-012 (multi-message)
**Status:** Implemented

#### Components Created

| Component | File | Parameters |
|-----------|------|------------|
| `MariloValidationMessage<TValue>` | `Forms/Containers/MariloValidationMessage.razor` | `For`, `Template` |
| `MariloValidationSummary` | `Forms/Containers/MariloValidationSummary.razor` | `Template` |
| `MariloValidationTooltip<TValue>` | `Forms/Containers/MariloValidationTooltip.razor` | `For`, `TargetSelector`, `Position`, `Template` |

#### Pattern

All three components:
- Require a cascading `EditContext` (throw if missing)
- Subscribe to `EditContext.OnValidationStateChanged`
- Automatically refresh when validation state changes
- Support `Template` render fragment for custom rendering
- Properly dispose event subscriptions

The existing `MariloValidation` component is retained as a low-level static validation display for manual/imperative scenarios.

---

### RES-FORM-003: MariloField and MariloLabel enhancements

**Resolves:** GAP-FIELD-001 (Text), GAP-FIELD-003 (label element), GAP-FIELD-004 (Id), GAP-FIELD-007 (validation styling), GAP-LABEL-001 (Text), GAP-LABEL-003 (Id), GAP-LABEL-004 (validation color)
**Status:** Implemented

#### Changes

**MariloField:**
- Added `Text` parameter — renders a `<label>` inside the field when set
- Added `Id` parameter — passed to label's `for` attribute
- Added validation-aware CSS class `mar-field--invalid` when associated field has errors

**MariloLabel:**
- Added `Text` parameter — takes precedence over `ChildContent`
- Added `Id` parameter — sets the label element's `id` (for `aria-labelledby`)
- Added validation-aware CSS class `mar-label--invalid` when associated field has errors
- Retains existing `For` parameter for `<label for="...">` association

#### Deferred Gaps (Phase 2+)

| Gap | Reason |
|-----|--------|
| GAP-FIELD-002 (floating animation) | Requires JS interop for focus tracking; deferred to Phase 2 |
| GAP-LABEL-002 (floating behavior) | Same as above; deferred to Phase 2 |
| GAP-FIELD-005 (compatibility enforcement) | Nice-to-have; deferred to Phase 4 |
| GAP-FIELD-006 (placeholder interaction) | Nice-to-have; deferred to Phase 4 |
| GAP-LABEL-005 (wrapper container) | Nice-to-have; deferred to Phase 4 |
| GAP-XCUT-001/002 (naming/FloatingLabel) | Design decision needed; deferred to Phase 2 |
| GAP-FORM-007/010 (auto-generation/annotations) | Complex feature; deferred to Phase 2 |
| GAP-FORM-020 (FormGroups) | Depends on FormItems; deferred to Phase 3 |
| GAP-FORM-022 (FormItemsTemplate) | Advanced template system; deferred to Phase 4 |
| GAP-FORM-026 (AutoGeneratedItems) | Depends on auto-generation; deferred to Phase 4 |

---

## Success Criteria

- [x] `MariloForm` creates EditContext from Model parameter
- [x] `MariloForm` accepts existing EditContext parameter
- [x] Mutual exclusion enforced (Model XOR EditContext)
- [x] `OnSubmit`, `OnValidSubmit`, `OnInvalidSubmit` events fire correctly
- [x] `OnUpdate` fires on field changes
- [x] `EditContext` cascaded to child components
- [x] `MariloValidationMessage<T>` displays per-field errors from EditContext
- [x] `MariloValidationSummary` displays all errors from EditContext
- [x] `MariloValidationTooltip<T>` displays per-field errors with positioning
- [x] All validation components support Template render fragment
- [x] `MariloField` renders label when Text is set
- [x] `MariloField` adds invalid CSS class from EditContext state
- [x] `MariloLabel` supports Text parameter
- [x] `MariloLabel` adds invalid CSS class from EditContext state
- [x] Existing `MariloValidation` component retained (backward compatible)
- [x] Full solution builds with zero errors
