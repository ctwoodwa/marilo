# Gap Inventory: Forms/Containers

> Source: `Forms/Containers/GAP_ANALYSIS.md`
> Imported: 2026-03-31
> Total gaps: 60 (19 Critical, 24 Important, 17 Nice-to-have)

---

## MariloForm

### GAP-FORM-001: Missing Model parameter

**Area:** MariloForm
**Severity:** Critical
**Theme:** missing-model-binding
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `Model` (object) parameter binds the form to a data object. When set, the form creates an `EditContext` internally and auto-generates editors based on model property types.

**Current behavior:** No `Model` parameter exists. The form renders an empty `<form>` element with `ChildContent`.

**Impact:** Without Model binding, the form cannot auto-generate fields, cannot validate, and cannot fire typed submit events. All downstream form functionality is blocked.

**Recommended direction:** Add `Model` parameter with `EditContext` creation in `OnParametersSet`. Mutually exclusive with explicit `EditContext` parameter.

**Status:** Open

---

### GAP-FORM-002: Missing EditContext parameter

**Area:** MariloForm
**Severity:** Critical
**Theme:** missing-model-binding
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `EditContext` (EditContext) parameter allows consumers to provide their own EditContext instead of having the form create one from `Model`.

**Current behavior:** No `EditContext` parameter. No validation or field-change tracking.

**Impact:** Consumers cannot share EditContext across components or pre-configure validation. Blocks all validation integration.

**Recommended direction:** Add `EditContext` parameter, mutually exclusive with `Model`. Cascade the context to child components.

**Status:** Open

---

### GAP-FORM-003: Missing ValidationMessageType parameter

**Area:** MariloForm
**Severity:** Critical
**Theme:** missing-validation-integration
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `ValidationMessageType` (FormValidationMessageType enum: Inline/Tooltip/None) controls how validation messages display for auto-generated fields.

**Current behavior:** Not implemented. No validation message rendering of any kind.

**Impact:** No way to control validation message display mode for auto-generated form fields.

**Recommended direction:** Add enum and parameter. Wire into auto-generation logic to select between inline messages, tooltip messages, or hidden messages.

**Status:** Open

---

### GAP-FORM-004: Missing OnSubmit event

**Area:** MariloForm
**Severity:** Critical
**Theme:** missing-form-events
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Events

**Target behavior:** `OnSubmit` (`EventCallback<EditContext>`) fires when the form is submitted, before validation.

**Current behavior:** No submit event handling. The `<form>` element has no `@onsubmit` handler.

**Impact:** Consumers cannot handle form submission. The form is non-functional.

**Recommended direction:** Add `@onsubmit` handler that invokes `OnSubmit`, then runs validation and invokes `OnValidSubmit` or `OnInvalidSubmit`.

**Status:** Open

---

### GAP-FORM-005: Missing OnValidSubmit event

**Area:** MariloForm
**Severity:** Critical
**Theme:** missing-form-events
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Events

**Target behavior:** `OnValidSubmit` (`EventCallback<EditContext>`) fires when the form passes validation on submit.

**Current behavior:** Not implemented.

**Impact:** No way to distinguish valid vs invalid submissions.

**Recommended direction:** Wire to `EditContext.Validate()` result in the submit handler.

**Status:** Open

---

### GAP-FORM-006: Missing OnInvalidSubmit event

**Area:** MariloForm
**Severity:** Critical
**Theme:** missing-form-events
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Events

**Target behavior:** `OnInvalidSubmit` (`EventCallback<EditContext>`) fires when the form fails validation on submit.

**Current behavior:** Not implemented.

**Impact:** No way to handle failed validation (show errors, scroll to first error, etc.).

**Recommended direction:** Wire to `EditContext.Validate()` failure in the submit handler.

**Status:** Open

---

### GAP-FORM-007: No automatic field generation

**Area:** MariloForm
**Severity:** Critical
**Theme:** missing-auto-generation
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Behaviors

**Target behavior:** When `Model` is set and no `FormItems` are provided, the form auto-generates editors based on model property types: string→TextBox, int/double/decimal→NumericTextBox, Enum→DropDownList, DateTime→DatePicker, bool→CheckBox. Respects `[Display]`, `[Editable]`, `[Required]` attributes.

**Current behavior:** Not implemented.

**Impact:** Consumers must manually build every form field — a major convenience gap vs. Telerik's form.

**Recommended direction:** Implement model reflection + type-to-editor mapping. Support data annotation attributes for labels, ordering, and visibility.

**Status:** Open

---

### GAP-FORM-008: No FormValidation child support

**Area:** MariloForm
**Severity:** Critical
**Theme:** missing-child-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Behaviors

**Target behavior:** `<FormValidation>` child tag wraps `DataAnnotationsValidator` or custom validator components.

**Current behavior:** Not implemented. No structured child component support.

**Impact:** Validators cannot be plugged into the form's EditContext.

**Recommended direction:** Add `FormValidation` RenderFragment parameter that renders inside the EditForm's scope.

**Status:** Open

---

### GAP-FORM-009: No FormItems child support

**Area:** MariloForm
**Severity:** Critical
**Theme:** missing-child-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Behaviors

**Target behavior:** `<FormItems>` with `<FormItem>` children allow manual definition or customization of individual form fields, including Field expression, LabelText, EditorType, and Template.

**Current behavior:** Not implemented.

**Impact:** No way to customize individual auto-generated fields or define manual field layouts.

**Recommended direction:** Implement FormItems/FormItem as render fragment parameters with a FormItem component supporting Field, LabelText, EditorType, Template, Enabled, Visible, ColSpan.

**Status:** Open

---

### GAP-FORM-010: No data annotation attribute support

**Area:** MariloForm
**Severity:** Critical
**Theme:** missing-auto-generation
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Behaviors

**Target behavior:** `[Display(Name)]` sets label text, `[Display(AutoGenerateField=false)]` hides field, `[Editable(false)]` renders disabled, `[Required]`/`[MaxLength]`/`[Range]` drive validation.

**Current behavior:** Not implemented.

**Impact:** Standard .NET data annotation patterns don't work with the form.

**Recommended direction:** Implement reflection-based attribute reading in the auto-generation logic.

**Status:** Open

---

### GAP-FORM-011: Missing AutoComplete parameter

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-html-attributes
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `AutoComplete` (string) passes through to the form's `autocomplete` HTML attribute.

**Current behavior:** Not implemented (could be passed via AdditionalAttributes as workaround).

**Impact:** Minor — workaround exists via AdditionalAttributes.

**Recommended direction:** Add dedicated parameter for discoverability.

**Status:** Open

---

### GAP-FORM-012: Missing Id parameter

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-html-attributes
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `Id` (string) sets the form's `id` attribute. Used by external submit buttons via `form` attribute.

**Current behavior:** No dedicated parameter. Could be passed via AdditionalAttributes.

**Impact:** External submit buttons cannot target the form without a predictable id.

**Recommended direction:** Add `Id` parameter, render on `<form>` element.

**Status:** Open

---

### GAP-FORM-013: Missing Columns parameter

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-layout-params
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `Columns` (int) enables multi-column form layout for auto-generated fields.

**Current behavior:** Not implemented.

**Impact:** All auto-generated forms would be single-column only.

**Recommended direction:** Add parameter, use CSS grid with `grid-template-columns: repeat(N, 1fr)`.

**Status:** Open

---

### GAP-FORM-014: Missing Orientation parameter

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-layout-params
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `Orientation` (FormOrientation: Horizontal/Vertical) controls whether labels appear above or beside inputs.

**Current behavior:** Not implemented.

**Impact:** No horizontal form layout option.

**Recommended direction:** Add enum and parameter. Horizontal renders label + input in a row; vertical stacks them.

**Status:** Open

---

### GAP-FORM-015: Missing Size parameter

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-layout-params
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `Size` (string: sm/md/lg) controls editor size and spacing.

**Current behavior:** Not implemented.

**Impact:** No compact or spacious form variants.

**Recommended direction:** Add parameter, cascade to child inputs as a default size.

**Status:** Open

---

### GAP-FORM-016: Missing Width parameter

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-layout-params
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `Width` (string) sets CSS width of the form element.

**Current behavior:** Not implemented.

**Impact:** Minor — can be set via Style parameter.

**Recommended direction:** Add dedicated parameter for convenience.

**Status:** Open

---

### GAP-FORM-017: Missing OnUpdate event

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-form-events
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Events

**Target behavior:** `OnUpdate` (`EventCallback<FormUpdateEventArgs>`) fires on field value changes with `Model` and `FieldName` properties.

**Current behavior:** Not implemented.

**Impact:** No way to react to individual field changes for cross-field logic.

**Recommended direction:** Wire to `EditContext.OnFieldChanged`, create `FormUpdateEventArgs`.

**Status:** Open

---

### GAP-FORM-018: Missing Refresh() method

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-methods
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Methods

**Target behavior:** `Refresh()` triggers `StateHasChanged()` on the form, useful after programmatic model changes.

**Current behavior:** Not implemented.

**Impact:** Consumers cannot force a re-render after modifying the model programmatically.

**Recommended direction:** Add public `Refresh()` method calling `StateHasChanged()`.

**Status:** Open

---

### GAP-FORM-019: Missing EditContext property (reference)

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-methods
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Methods

**Target behavior:** `EditContext` property accessible via `@ref` for programmatic validation.

**Current behavior:** Not implemented.

**Impact:** Cannot trigger validation programmatically.

**Recommended direction:** Expose `EditContext` as a public property.

**Status:** Open

---

### GAP-FORM-020: No FormGroups support

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-child-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Behaviors

**Target behavior:** `<FormGroup>` organizes fields into labeled sections with their own `LabelText`, `Columns`, `ColumnSpacing`, `RowSpacing`.

**Current behavior:** Not implemented.

**Impact:** No way to group related fields visually.

**Recommended direction:** Implement FormGroup child component.

**Status:** Open

---

### GAP-FORM-021: No FormButtons support

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-child-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Behaviors

**Target behavior:** `<FormButtons>` child tag for custom button rendering with `ButtonsLayout` positioning.

**Current behavior:** Not implemented. No default submit button.

**Impact:** No structured button area in auto-generated forms.

**Recommended direction:** Implement FormButtons RenderFragment parameter.

**Status:** Open

---

### GAP-FORM-022: No FormItemsTemplate support

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-child-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Behaviors

**Target behavior:** `<FormItemsTemplate>` with `FormItemsTemplateContext`, `MariloFormGroupRenderer`, `MariloFormItemRenderer` for full layout control.

**Current behavior:** Not implemented.

**Impact:** No way to fully customize form layout while retaining auto-generated field logic.

**Recommended direction:** Implement as an advanced template system. Lower priority than core form features.

**Status:** Open

---

### GAP-FORM-023: Missing ColumnSpacing parameter

**Area:** MariloForm
**Severity:** Low
**Theme:** missing-layout-params
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `ColumnSpacing` (string, default "32px") controls horizontal gap between columns.

**Current behavior:** Not implemented.

**Impact:** Minor — CSS gap can be set via Style.

**Recommended direction:** Add parameter, apply as CSS `column-gap`.

**Status:** Open

---

### GAP-FORM-024: Missing RowSpacing parameter

**Area:** MariloForm
**Severity:** Low
**Theme:** missing-layout-params
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `RowSpacing` (string) controls vertical gap between rows.

**Current behavior:** Not implemented.

**Impact:** Minor — CSS gap can be set via Style.

**Recommended direction:** Add parameter, apply as CSS `row-gap`.

**Status:** Open

---

### GAP-FORM-025: Missing ButtonsLayout parameter

**Area:** MariloForm
**Severity:** Low
**Theme:** missing-layout-params
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Parameters

**Target behavior:** `ButtonsLayout` (FormButtonsLayout enum, default Start) positions buttons (start, center, end, stretch).

**Current behavior:** Not implemented.

**Impact:** Minor — can be styled manually.

**Recommended direction:** Add enum and parameter.

**Status:** Open

---

### GAP-FORM-026: No FormAutoGeneratedItems support

**Area:** MariloForm
**Severity:** Low
**Theme:** missing-child-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Behaviors

**Target behavior:** `<FormAutoGeneratedItems>` mixes auto-generated fields with manually defined `<FormItem>` fields.

**Current behavior:** Not implemented.

**Impact:** Cannot mix auto and manual fields — all-or-nothing.

**Recommended direction:** Implement as enhancement after core auto-generation works.

**Status:** Open

---

### GAP-FORM-027: No WAI-ARIA role=form

**Area:** MariloForm
**Severity:** Low
**Theme:** missing-accessibility
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Behaviors

**Target behavior:** Spec notes `role=form` or semantic `<form>` element.

**Current behavior:** Uses `<form>` element (correct), but no additional ARIA attributes.

**Impact:** Minimal — `<form>` element provides implicit role.

**Recommended direction:** Add `aria-label` or `aria-labelledby` support via parameter.

**Status:** Open

---

### GAP-FORM-028: FormValidation child renders as RenderFragment

**Area:** MariloForm
**Severity:** Medium
**Theme:** missing-child-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §2 — Behaviors

**Target behavior:** `<FormValidation>` is a RenderFragment within the form that wraps validator components inside the EditContext scope.

**Current behavior:** No `FormValidation` render fragment parameter.

**Impact:** Cannot plug `DataAnnotationsValidator` or custom validators into the form.

**Recommended direction:** Add `FormValidation` RenderFragment parameter. Render inside cascading EditContext.

**Status:** Open

---

## MariloField

### GAP-FIELD-001: Missing Text parameter

**Area:** MariloField
**Severity:** High
**Theme:** missing-floating-label
**Source:** Forms/Containers/GAP_ANALYSIS.md §1 — Parameters

**Target behavior:** `Text` (string) defines the floating label text displayed over/above the input.

**Current behavior:** No `Text` parameter. Component renders only a `<div>` with `ChildContent`.

**Impact:** Cannot display a floating label — the core purpose of the component per spec.

**Recommended direction:** Add `Text` parameter, render a `<label>` element with the text.

**Status:** Open

---

### GAP-FIELD-002: No floating/animation behavior

**Area:** MariloField
**Severity:** High
**Theme:** missing-floating-label
**Source:** Forms/Containers/GAP_ANALYSIS.md §1 — Behaviors

**Target behavior:** Label floats over empty non-focused components and moves above on focus. Animation built-in.

**Current behavior:** Static `<div>` with no focus tracking or animation.

**Impact:** No floating label UX — fundamental gap vs. spec.

**Recommended direction:** Implement CSS transition + focus/blur tracking (CSS-only or minimal JS interop).

**Status:** Open

---

### GAP-FIELD-003: No label element rendered

**Area:** MariloField
**Severity:** High
**Theme:** missing-floating-label
**Source:** Forms/Containers/GAP_ANALYSIS.md §1 — Behaviors

**Target behavior:** Renders `<label>` inside `span.k-floating-label-container`.

**Current behavior:** Renders only a `<div>`.

**Impact:** No semantic label — breaks accessibility.

**Recommended direction:** Render proper `<label>` element with `for` attribute linking to the child input.

**Status:** Open

---

### GAP-FIELD-004: Missing Id parameter

**Area:** MariloField
**Severity:** Medium
**Theme:** missing-html-attributes
**Source:** Forms/Containers/GAP_ANALYSIS.md §1 — Parameters

**Target behavior:** `Id` parameter renders on the `<label>` element for `aria-labelledby` association.

**Current behavior:** Not implemented.

**Impact:** Accessibility link between label and input may not work.

**Recommended direction:** Add `Id` parameter.

**Status:** Open

---

### GAP-FIELD-005: No component compatibility enforcement

**Area:** MariloField
**Severity:** Medium
**Theme:** missing-floating-label
**Source:** Forms/Containers/GAP_ANALYSIS.md §1 — Behaviors

**Target behavior:** Only 12 compatible Marilo input components are supported inside the floating label.

**Current behavior:** Accepts any `ChildContent`.

**Impact:** No guidance or enforcement for consumers about supported child components.

**Recommended direction:** Document compatibility; optionally add runtime check or warning.

**Status:** Open

---

### GAP-FIELD-006: No placeholder interaction

**Area:** MariloField
**Severity:** Medium
**Theme:** missing-floating-label
**Source:** Forms/Containers/GAP_ANALYSIS.md §1 — Behaviors

**Target behavior:** Placeholder hidden when floating label is over the component; shown when label floats away.

**Current behavior:** Not implemented.

**Impact:** Visual confusion when both placeholder and label text are visible.

**Recommended direction:** Implement via CSS `:placeholder-shown` pseudo-class coordination.

**Status:** Open

---

### GAP-FIELD-007: No validation-aware styling

**Area:** MariloField
**Severity:** Medium
**Theme:** missing-validation-integration
**Source:** Forms/Containers/GAP_ANALYSIS.md §1 — Behaviors

**Target behavior:** Floating label changes color when the associated field is invalid.

**Current behavior:** Not implemented.

**Impact:** No visual feedback on validation state at the field container level.

**Recommended direction:** Read validation state from cascaded EditContext, toggle error CSS class.

**Status:** Open

---

## MariloLabel

### GAP-LABEL-001: Missing Text parameter

**Area:** MariloLabel
**Severity:** High
**Theme:** missing-floating-label
**Source:** Forms/Containers/GAP_ANALYSIS.md §3 — Parameters

**Target behavior:** `Text` (string) defines the floating label text.

**Current behavior:** Uses `ChildContent` instead. No `Text` parameter.

**Impact:** API mismatch with spec.

**Recommended direction:** Add `Text` parameter. Support both `Text` and `ChildContent` (Text takes precedence).

**Status:** Open

---

### GAP-LABEL-002: No floating behavior

**Area:** MariloLabel
**Severity:** High
**Theme:** missing-floating-label
**Source:** Forms/Containers/GAP_ANALYSIS.md §3 — Behaviors

**Target behavior:** Label floats over empty inputs and moves above on focus with animation.

**Current behavior:** Static `<label>` element.

**Impact:** Core feature of the spec missing.

**Recommended direction:** Implement via shared floating-label infrastructure with MariloField.

**Status:** Open

---

### GAP-LABEL-003: Parameter mismatch — For vs Id

**Area:** MariloLabel
**Severity:** Medium
**Theme:** missing-html-attributes
**Source:** Forms/Containers/GAP_ANALYSIS.md §3 — Parameters

**Target behavior:** Spec documents `Id` on the label element itself (for `aria-labelledby`). MariloLabel has `For` (the `for` attribute, not in spec).

**Current behavior:** `For` parameter renders as HTML `for` attribute. No `Id` parameter.

**Impact:** Spec's `Id` parameter missing. `For` serves a different purpose.

**Recommended direction:** Keep `For`, add `Id`. Both are useful.

**Status:** Open

---

### GAP-LABEL-004: No validation color change

**Area:** MariloLabel
**Severity:** Medium
**Theme:** missing-validation-integration
**Source:** Forms/Containers/GAP_ANALYSIS.md §3 — Behaviors

**Target behavior:** Label changes color when the form field is invalid.

**Current behavior:** Not implemented.

**Impact:** No visual indication of validation state on the label.

**Recommended direction:** Read validation state from EditContext, toggle CSS class.

**Status:** Open

---

### GAP-LABEL-005: No wrapper container

**Area:** MariloLabel
**Severity:** Low
**Theme:** missing-floating-label
**Source:** Forms/Containers/GAP_ANALYSIS.md §3 — Behaviors

**Target behavior:** FloatingLabel renders `span.k-floating-label-container` wrapping the label.

**Current behavior:** Renders only `<label>`.

**Impact:** CSS targeting differences; minor structural gap.

**Recommended direction:** Address as part of floating-label implementation.

**Status:** Open

---

## MariloValidation

### GAP-VAL-001: Missing For parameter (ValidationMessage)

**Area:** MariloValidation → MariloValidationMessage
**Severity:** Critical
**Theme:** missing-editcontext-integration
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Parameters (ValidationMessage)

**Target behavior:** `For` (`Expression<Func<TValue>>`) binds the message to a specific model property via EditContext.

**Current behavior:** Uses static `Message` string parameter. No expression binding.

**Impact:** Cannot auto-display validation errors from EditContext. Manual message passing required.

**Recommended direction:** Implement `MariloValidationMessage<TValue>` with `For` parameter and cascaded `EditContext`.

**Status:** Open

---

### GAP-VAL-002: No EditContext integration

**Area:** MariloValidation
**Severity:** Critical
**Theme:** missing-editcontext-integration
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Behaviors

**Target behavior:** All three validation components receive `EditContext` via cascading parameter and auto-retrieve validation messages.

**Current behavior:** No EditContext awareness. Static `Message` string only.

**Impact:** The fundamental purpose of the validation components — automatic EditContext-driven message display — is missing.

**Recommended direction:** Add `[CascadingParameter] EditContext` to all validation components.

**Status:** Open

---

### GAP-VAL-003: Single component instead of three

**Area:** MariloValidation
**Severity:** Critical
**Theme:** missing-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Behaviors

**Target behavior:** Three distinct components: `MariloValidationMessage` (per-field), `MariloValidationSummary` (all errors), `MariloValidationTooltip` (popup per-field).

**Current behavior:** Single `MariloValidation` component displaying one static message.

**Impact:** Missing two of three documented validation components entirely.

**Recommended direction:** Create all three as new components. Optionally retain `MariloValidation` as a low-level static display.

**Status:** Open

---

### GAP-VAL-004: No ValidationSummary implementation

**Area:** MariloValidation → MariloValidationSummary
**Severity:** Critical
**Theme:** missing-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Parameters (ValidationSummary)

**Target behavior:** `MariloValidationSummary` displays all validation errors from EditContext with optional Template.

**Current behavior:** Component does not exist.

**Impact:** No aggregate error display for forms.

**Recommended direction:** Implement `MariloValidationSummary` with cascaded `EditContext` and `Template` parameter.

**Status:** Open

---

### GAP-VAL-005: No ValidationTooltip implementation

**Area:** MariloValidation → MariloValidationTooltip
**Severity:** Critical
**Theme:** missing-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Parameters (ValidationTooltip)

**Target behavior:** `MariloValidationTooltip` shows per-field errors in a positioned tooltip popup.

**Current behavior:** Component does not exist.

**Impact:** No tooltip-style validation message display.

**Recommended direction:** Implement `MariloValidationTooltip` with `For`, `TargetSelector`, `Position`, `Template`.

**Status:** Open

---

### GAP-VAL-006: Missing For parameter (ValidationTooltip)

**Area:** MariloValidation → MariloValidationTooltip
**Severity:** Critical
**Theme:** missing-editcontext-integration
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Parameters (ValidationTooltip)

**Target behavior:** `For` expression binding for tooltip, same as ValidationMessage.

**Current behavior:** Component does not exist.

**Impact:** Blocked by GAP-VAL-005.

**Recommended direction:** Part of MariloValidationTooltip implementation.

**Status:** Open

---

### GAP-VAL-007: Missing TargetSelector parameter

**Area:** MariloValidation → MariloValidationTooltip
**Severity:** High
**Theme:** missing-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Parameters (ValidationTooltip)

**Target behavior:** `TargetSelector` (CSS selector) positions the tooltip relative to a target element.

**Current behavior:** Component does not exist.

**Impact:** Blocked by GAP-VAL-005.

**Recommended direction:** Part of MariloValidationTooltip implementation. May share positioning with Popover/Tooltip.

**Status:** Open

---

### GAP-VAL-008: Missing Template parameter (ValidationMessage)

**Area:** MariloValidation → MariloValidationMessage
**Severity:** Medium
**Theme:** missing-templates
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Parameters (ValidationMessage)

**Target behavior:** `Template` (RenderFragment with IEnumerable<string> context) for custom rendering of messages.

**Current behavior:** No template support. Renders static message string.

**Impact:** Cannot customize validation message appearance.

**Recommended direction:** Add `Template` RenderFragment<IEnumerable<string>> parameter.

**Status:** Open

---

### GAP-VAL-009: Missing Position parameter (ValidationTooltip)

**Area:** MariloValidation → MariloValidationTooltip
**Severity:** Medium
**Theme:** missing-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Parameters (ValidationTooltip)

**Target behavior:** `Position` (TooltipPosition enum) controls tooltip placement.

**Current behavior:** Component does not exist.

**Impact:** Blocked by GAP-VAL-005.

**Recommended direction:** Part of MariloValidationTooltip implementation.

**Status:** Open

---

### GAP-VAL-010: Missing Template parameter (ValidationTooltip)

**Area:** MariloValidation → MariloValidationTooltip
**Severity:** Medium
**Theme:** missing-templates
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Parameters (ValidationTooltip)

**Target behavior:** Template for custom tooltip content rendering.

**Current behavior:** Component does not exist.

**Impact:** Blocked by GAP-VAL-005.

**Recommended direction:** Part of MariloValidationTooltip implementation.

**Status:** Open

---

### GAP-VAL-011: Missing Template parameter (ValidationSummary)

**Area:** MariloValidation → MariloValidationSummary
**Severity:** Medium
**Theme:** missing-templates
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Parameters (ValidationSummary)

**Target behavior:** Template for custom summary rendering.

**Current behavior:** Component does not exist.

**Impact:** Blocked by GAP-VAL-004.

**Recommended direction:** Part of MariloValidationSummary implementation.

**Status:** Open

---

### GAP-VAL-012: No multi-message support

**Area:** MariloValidation
**Severity:** Medium
**Theme:** missing-editcontext-integration
**Source:** Forms/Containers/GAP_ANALYSIS.md §4 — Behaviors

**Target behavior:** ValidationMessage/Tooltip show all validation messages for a field (IEnumerable<string>).

**Current behavior:** Single `Message` string only.

**Impact:** Only one validation message shown even if multiple rules fail.

**Recommended direction:** Implement as part of EditContext integration.

**Status:** Open

---

## Cross-Cutting

### GAP-XCUT-001: Naming mismatch — spec vs implementation

**Area:** Forms/Containers (all)
**Severity:** High
**Theme:** naming-mismatch
**Source:** Forms/Containers/GAP_ANALYSIS.md §5

**Target behavior:** Spec names: `MariloFloatingLabel`, `MariloValidationMessage`, `MariloValidationSummary`, `MariloValidationTooltip`.

**Current behavior:** Implementation names: `MariloField`, `MariloLabel`, `MariloValidation`.

**Impact:** Documentation/implementation mismatch confuses consumers.

**Recommended direction:** Create the spec-named components as new additions. Retain existing names as basic utility components with their own documentation.

**Status:** Open

---

### GAP-XCUT-002: Missing MariloFloatingLabel component

**Area:** Forms/Containers
**Severity:** High
**Theme:** missing-components
**Source:** Forms/Containers/GAP_ANALYSIS.md §5

**Target behavior:** Fully documented floating label with Text, Id, animation, focus tracking, validation-aware styling, and component compatibility.

**Current behavior:** `MariloField` + `MariloLabel` together do not cover FloatingLabel functionality.

**Impact:** A key spec'd component does not exist in any form.

**Recommended direction:** Implement `MariloFloatingLabel` as a new component. MariloField serves as field container, MariloLabel as basic label.

**Status:** Open

---

## Summary by Theme

| Theme | Gap Count | Severity Breakdown |
|-------|-----------|-------------------|
| missing-model-binding | 2 | 2 Critical |
| missing-form-events | 4 | 3 Critical, 1 Medium |
| missing-auto-generation | 2 | 2 Critical |
| missing-child-components | 7 | 2 Critical, 4 Medium, 1 Low |
| missing-editcontext-integration | 4 | 3 Critical, 1 Medium |
| missing-components | 5 | 3 Critical, 2 High |
| missing-floating-label | 5 | 3 High, 1 Medium, 1 Low |
| missing-validation-integration | 2 | 2 Medium |
| missing-templates | 3 | 3 Medium |
| missing-layout-params | 6 | 3 Medium, 3 Low |
| missing-html-attributes | 3 | 3 Medium |
| missing-methods | 2 | 2 Medium |
| naming-mismatch | 1 | 1 High |
| missing-accessibility | 1 | 1 Low |
