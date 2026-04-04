# Gap Inventory: MariloWizard

> Imported: 2026-04-03
> Analysis mode: Reconstructed (code exists before gap analysis)
> Total gaps: ~18 (5 Critical, 7 High, 6 Medium)

---

## Component Inventory

| Attribute | Value |
|-----------|-------|
| **Source files** | `MariloWizard.razor` (125 lines), `WizardStep.razor` (25 lines) |
| **Code-behind partials** | None |
| **Public parameters (Wizard)** | 8 (ChildContent, ActiveStepIndex, ActiveStepIndexChanged, AllowStepClick, PreviousText, NextText, FinishText, OnFinish, OnStepChange) |
| **Public parameters (WizardStep)** | 4 (Label, ChildContent, Disabled, Icon) |
| **Internal methods** | RegisterStep, UnregisterStep |
| **Supporting types** | None (WizardStepChangeEventArgs, WizardStepperPosition, WizardStepperSettings all absent) |
| **Base class** | MariloComponentBase (provides Class, AdditionalAttributes) |
| **Tests** | None found |
| **Demos** | 1 page (`samples/Marilo.Demo/Pages/Components/Wizard/Overview.razor`) -- uses non-spec API |
| **Spec files** | `docs/component-specs/wizard/` (overview, events, layout, buttons, stepper, content, templates, form-integration, accessibility) |

---

## Gap Summary

The spec describes a Wizard with a `Value` parameter (two-way bound step index), `WizardSteps` wrapper, `WizardButtons` custom buttons RenderFragment, `WizardSettings`/`WizardStepperSettings` configuration tags, per-step `Content` RenderFragment, `StepTemplate`, per-step `OnChange` event with cancelable `WizardStepChangeEventArgs`, step validation (`Valid`), optional steps, stepper position (Top/Bottom/Left/Right), `ShowPager`, `Width`/`Height`, keyboard navigation, and full WAI-ARIA support.

The implementation is a minimal 125-line wizard with basic step registration, forward/back navigation, and a finish button. It uses `ActiveStepIndex` instead of `Value`, has no `WizardSteps` wrapper, no `WizardButtons`, no per-step `OnChange` with cancelable args, no `StepTemplate`, no layout positioning, no pager, no keyboard navigation, and incomplete ARIA. The demo page uses a `Title` parameter that does not exist on `WizardStep`.

---

### GAP-WIZARD-001: Value parameter naming mismatch

**Area:** MariloWizard
**Severity:** Critical
**Theme:** api-surface-mismatch
**Source:** wizard/overview.md -- `Value` parameter with `@bind-Value` two-way binding

**Target behavior:** `Value` parameter (int) with `ValueChanged` EventCallback for two-way binding via `@bind-Value`.
**Current behavior:** Uses `ActiveStepIndex` with `ActiveStepIndexChanged`. Consumer code using `@bind-Value` will not compile.
**Impact:** Every spec code example is incompatible with the current API.
**Recommended direction:** Rename `ActiveStepIndex` to `Value` and `ActiveStepIndexChanged` to `ValueChanged`, or add aliases.
**Status:** Open

---

### GAP-WIZARD-002: Missing WizardSteps wrapper component

**Area:** MariloWizard
**Severity:** Critical
**Theme:** api-surface-mismatch
**Source:** wizard/overview.md -- `<WizardSteps>` wrapper tag contains `<WizardStep>` children

**Target behavior:** `WizardSteps` wrapper tag groups `WizardStep` children per spec API shape.
**Current behavior:** `WizardStep` children render directly inside `ChildContent`; no `WizardSteps` wrapper exists.
**Impact:** All spec code examples use `<WizardSteps>` and will not compile against the current implementation.
**Recommended direction:** Add `WizardSteps` as a pass-through RenderFragment or wrapper component.
**Status:** Open

---

### GAP-WIZARD-003: Missing WizardButtons custom buttons RenderFragment

**Area:** MariloWizard
**Severity:** Critical
**Theme:** missing-render-fragment
**Source:** wizard/structure/buttons.md -- `<WizardButtons>` tag with `context` providing current step index

**Target behavior:** `WizardButtons` RenderFragment<int> allows full custom button rendering with access to current step index via `context`.
**Current behavior:** Only hardcoded Previous/Next/Finish buttons with text customization. No way to inject custom buttons.
**Impact:** Cannot implement custom navigation patterns (e.g., "Go to first page", conditional buttons, custom Done logic).
**Recommended direction:** Add `WizardButtons` as `RenderFragment<int>` parameter; when provided, replace default button bar.
**Status:** Open

---

### GAP-WIZARD-004: Missing per-step OnChange event with cancelable args

**Area:** WizardStep
**Severity:** Critical
**Theme:** missing-event
**Source:** wizard/events.md -- per-step `OnChange` with `WizardStepChangeEventArgs` { TargetIndex, IsCancelled }

**Target behavior:** Each `WizardStep` has an `OnChange` event that fires before navigation away from that step. The handler receives `WizardStepChangeEventArgs` with `TargetIndex` and `IsCancelled`. Setting `IsCancelled = true` prevents navigation.
**Current behavior:** Parent wizard has `OnStepChange` EventCallback<int> that fires after navigation with no cancellation support. No per-step event exists. `WizardStepChangeEventArgs` type does not exist.
**Impact:** Cannot validate or cancel step transitions. Form integration pattern from spec is impossible.
**Recommended direction:** Add `OnChange` EventCallback<WizardStepChangeEventArgs> to WizardStep. Create `WizardStepChangeEventArgs` class. Fire before navigation; check IsCancelled before proceeding.
**Status:** Open

---

### GAP-WIZARD-005: Missing WizardStepChangeEventArgs type

**Area:** Supporting types
**Severity:** Critical
**Theme:** missing-type
**Source:** wizard/events.md -- `WizardStepChangeEventArgs` with `TargetIndex` (int) and `IsCancelled` (bool)

**Target behavior:** `WizardStepChangeEventArgs` class exists with `TargetIndex` and `IsCancelled` properties.
**Current behavior:** Type does not exist anywhere in the codebase.
**Impact:** Blocks GAP-WIZARD-004 (cancelable step navigation) and form integration patterns.
**Recommended direction:** Create `WizardStepChangeEventArgs` class in the component library.
**Status:** Open

---

### GAP-WIZARD-006: Missing Content RenderFragment on WizardStep

**Area:** WizardStep
**Severity:** High
**Theme:** api-surface-mismatch
**Source:** wizard/structure/content.md -- `<Content>` tag inside each `<WizardStep>`

**Target behavior:** Each `WizardStep` has a `Content` RenderFragment where step body is placed.
**Current behavior:** Uses `ChildContent` directly. Spec examples use `<Content>` tag inside `<WizardStep>` which will not compile.
**Impact:** All spec code examples placing content in `<Content>` tags are incompatible.
**Recommended direction:** Rename `ChildContent` to `Content` on WizardStep, or add `Content` as an alias RenderFragment.
**Status:** Open

---

### GAP-WIZARD-007: Missing StepperPosition parameter and WizardStepperPosition enum

**Area:** MariloWizard
**Severity:** High
**Theme:** missing-parameter
**Source:** wizard/layout.md -- `StepperPosition` parameter with `WizardStepperPosition` enum (Top, Bottom, Left, Right)

**Target behavior:** `StepperPosition` parameter controls layout: stepper on Top (default), Bottom, Left, or Right of content.
**Current behavior:** Stepper is always rendered on top. No `StepperPosition` parameter. No `WizardStepperPosition` enum.
**Impact:** Cannot create left/right/bottom stepper layouts.
**Recommended direction:** Add `StepperPosition` parameter, create `WizardStepperPosition` enum, adjust CSS layout accordingly.
**Status:** Open

---

### GAP-WIZARD-008: Missing WizardSettings / WizardStepperSettings

**Area:** MariloWizard
**Severity:** High
**Theme:** missing-component
**Source:** wizard/structure/stepper.md -- `<WizardSettings>` / `<WizardStepperSettings>` tags with `StepType` and `Linear` parameters

**Target behavior:** `WizardSettings` contains `WizardStepperSettings` with `StepType` (StepperStepType enum: Steps, Labels) and `Linear` (bool, default true) parameters.
**Current behavior:** No settings components exist. `AllowStepClick` on wizard is a partial substitute for `Linear=false` but inverted semantics.
**Impact:** Cannot configure stepper display mode or linear flow behavior per spec.
**Recommended direction:** Add `WizardSettings` and `WizardStepperSettings` components, or flatten as parameters on MariloWizard.
**Status:** Open

---

### GAP-WIZARD-009: Missing Width and Height parameters

**Area:** MariloWizard
**Severity:** High
**Theme:** missing-parameter
**Source:** wizard/overview.md -- `Width` and `Height` parameters

**Target behavior:** `Width` and `Height` string parameters apply inline styles to the wizard container.
**Current behavior:** Not present on MariloWizard (base class does not provide them either).
**Impact:** Cannot size the wizard component. Many spec examples set `Width="600px" Height="300px"`.
**Recommended direction:** Add `Width` and `Height` string parameters, render as inline styles.
**Status:** Open

---

### GAP-WIZARD-010: Missing ShowPager parameter

**Area:** MariloWizard
**Severity:** High
**Theme:** missing-parameter
**Source:** wizard/overview.md -- `ShowPager` (bool, default true) renders "Step X of Y" label

**Target behavior:** When `ShowPager` is true (default), a "Step X of Y" label renders at the bottom of the component.
**Current behavior:** No pager exists. No `ShowPager` parameter.
**Impact:** No step progress indicator for users.
**Recommended direction:** Add `ShowPager` parameter with pager rendering in the actions area or content footer.
**Status:** Open

---

### GAP-WIZARD-011: Missing StepTemplate RenderFragment on WizardStep

**Area:** WizardStep
**Severity:** High
**Theme:** missing-render-fragment
**Source:** wizard/templates.md -- `<StepTemplate>` inside `<WizardStep>` for custom step indicator rendering

**Target behavior:** Each `WizardStep` has a `StepTemplate` RenderFragment that overrides the default step indicator (number/icon + label).
**Current behavior:** No `StepTemplate` parameter. Step indicators are hardcoded in MariloWizard.
**Impact:** Cannot customize step indicator appearance (e.g., Roman numerals, custom icons, badges).
**Recommended direction:** Add `StepTemplate` RenderFragment to WizardStep; render it in place of the default indicator when provided.
**Status:** Open

---

### GAP-WIZARD-012: Missing WizardStep parameters (Text, Optional, Valid)

**Area:** WizardStep
**Severity:** High
**Theme:** missing-parameter
**Source:** wizard/structure/stepper.md -- `Text`, `Optional`, `Valid` parameters on WizardStep

**Target behavior:** `Text` (string) sets indicator text. `Optional` (bool) marks step as optional with "(Optional)" label. `Valid` (bool?) shows success/error icon on step indicator.
**Current behavior:** None of these parameters exist. WizardStep only has Label, ChildContent, Disabled, Icon.
**Impact:** Cannot set custom indicator text, mark steps as optional, or show validation state on step indicators.
**Recommended direction:** Add `Text`, `Optional`, and `Valid` parameters to WizardStep. Render optional text and valid/invalid icons in the stepper.
**Status:** Open

---

### GAP-WIZARD-013: Missing keyboard navigation

**Area:** MariloWizard
**Severity:** Medium
**Theme:** missing-accessibility
**Source:** wizard/accessibility/wai-aria-support.md -- keyboard navigation for tablist role

**Target behavior:** Arrow keys navigate between step tabs. Tab key moves focus between stepper and content. Enter/Space activates a step. Focus management follows WAI-ARIA tablist pattern.
**Current behavior:** No keyboard event handlers. Step buttons are native `<button>` elements (basic keyboard activation works via browser defaults) but no arrow key navigation between tabs.
**Impact:** Partially accessible but does not meet WAI-ARIA tablist keyboard interaction pattern.
**Recommended direction:** Add `@onkeydown` handler for arrow key navigation between step tabs; manage `tabindex` per the roving tabindex pattern.
**Status:** Open

---

### GAP-WIZARD-014: Incomplete ARIA attributes

**Area:** MariloWizard
**Severity:** Medium
**Theme:** missing-accessibility
**Source:** wizard/accessibility/wai-aria-support.md -- full ARIA attribute table

**Target behavior:** Step links have `aria-controls` pointing to tabpanel ID, `aria-current=true` on active step, `tabindex=-1` on non-current tabs and `tabindex=0` on current tab. Tabpanel has `aria-label` with "Step X of Y" text and `tabindex=0`.
**Current behavior:** Has `role="tablist"`, `role="tab"`, `aria-selected`, `role="tabpanel"`. Missing: `aria-controls`, `aria-current`, `tabindex` management on tabs, `aria-label` on tabpanel, `tabindex=0` on tabpanel, `aria-disabled` (uses HTML `disabled` attribute instead).
**Impact:** Screen readers cannot associate tabs with panels or announce step progress.
**Recommended direction:** Add IDs to tabpanels, `aria-controls` to tabs, `aria-current` on active tab, `aria-label` on tabpanel, and roving `tabindex`.
**Status:** Open

---

### GAP-WIZARD-015: Disabled step does not disable adjacent navigation buttons

**Area:** MariloWizard
**Severity:** Medium
**Theme:** missing-behavior
**Source:** wizard/structure/stepper.md -- "If the next step is disabled, the Next button on the current step will also be marked as disabled"

**Target behavior:** When the next step is disabled, the Next button is disabled. When the previous step is disabled, the Previous button is disabled.
**Current behavior:** Next and Previous buttons do not check whether the adjacent step is disabled. `GoToStep` checks `Disabled` and silently no-ops, but the button remains enabled and clickable.
**Impact:** Misleading UX -- user can click Next/Previous into a disabled step with no visible feedback.
**Recommended direction:** Add disabled attribute to Next/Previous buttons when adjacent step has `Disabled=true`.
**Status:** Open

---

### GAP-WIZARD-016: No test coverage

**Area:** MariloWizard, WizardStep
**Severity:** Medium
**Theme:** missing-tests
**Source:** No test files found in any test project

**Target behavior:** Tests covering step registration, navigation, disabled steps, cancelable OnChange, finish event, keyboard navigation, ARIA attributes, custom buttons, stepper position, pager.
**Current behavior:** Zero tests.
**Impact:** All functionality untested; regressions undetectable.
**Recommended direction:** Create WizardTests.cs with bUnit coverage of core scenarios.
**Status:** Open

---

### GAP-WIZARD-017: Demo page uses non-existent API

**Area:** Demo
**Severity:** Medium
**Theme:** demo-api-mismatch
**Source:** `samples/Marilo.Demo/Pages/Components/Wizard/Overview.razor`

**Target behavior:** Demo should use the spec-compliant API (`@bind-Value`, `<WizardSteps>`, `<Content>`, `Label`).
**Current behavior:** Demo uses `Title` parameter (does not exist on WizardStep -- the actual parameter is `Label`). Demo does not use `<WizardSteps>` wrapper or `<Content>` tag. Demo likely does not compile or renders incorrectly.
**Impact:** Demo is broken or misleading; does not showcase spec-compliant usage.
**Recommended direction:** Update demo to use correct parameter names and spec-compliant structure. Add additional demos for events, form integration, layout, custom buttons.
**Status:** Open

---

### GAP-WIZARD-018: Missing CascadingValue for WizardStep registration

**Area:** MariloWizard
**Severity:** Medium
**Theme:** missing-cascading-value
**Source:** WizardStep.razor -- `[CascadingParameter] public MariloWizard? ParentWizard`

**Target behavior:** MariloWizard cascades itself to child WizardStep components for registration.
**Current behavior:** WizardStep expects a `CascadingParameter` of type `MariloWizard`, but MariloWizard.razor does not wrap `ChildContent` in a `<CascadingValue>`. Steps will never register because `ParentWizard` will always be null.
**Impact:** The wizard is fundamentally broken -- no steps will appear in the stepper or content area.
**Recommended direction:** Wrap `ChildContent` rendering in `<CascadingValue Value="this">@ChildContent</CascadingValue>`.
**Status:** Open

---

## Severity Breakdown

| Severity | Count |
|----------|-------|
| Critical | 5 |
| High | 7 |
| Medium | 6 |
| Low | 0 |
| **Total** | **18** |
