# Implementation Log: MariloWizard

> Date: 2026-04-04
> Stage: 05-implement
> Source: gap-wizard-resolutions.md (14 resolutions covering 18 gaps)
> Scope: batch (Wizard component gaps)

---

## Resolutions Implemented

### RES-WIZARD-001: Fix CascadingValue and render ChildContent (GAP-018)

**Status:** Implemented — CRITICAL FIX
**Change:** Added hidden `<div style="display:none"><CascadingValue Value="this" IsFixed="true">@ChildContent</CascadingValue></div>` to MariloWizard.razor. WizardStep children now register successfully via cascading parameter.

### RES-WIZARD-002: Rename ActiveStepIndex to Value (GAP-001)

**Status:** Implemented
**Change:** `ActiveStepIndex` → `Value`, `ActiveStepIndexChanged` → `ValueChanged`. Breaking change (pre-release, acceptable). Wizard was non-functional before, so no real consumers broken.

### RES-WIZARD-003: MariloWizardSteps pass-through wrapper (GAP-002)

**Status:** Implemented
**File:** Created `src/Marilo.Components/Layout/MariloWizardSteps.razor`

### RES-WIZARD-004: WizardStepChangeEventArgs and per-step OnChange (GAP-004, GAP-005)

**Status:** Implemented
**Change:** Added `WizardStepChangeEventArgs` to `WizardTypes.cs`. Added `OnChange` parameter to `WizardStep`. `GoToStep()` fires current step's `OnChange` before navigation; if `IsCancelled == true`, navigation is aborted.

### RES-WIZARD-005: WizardButtons custom RenderFragment (GAP-003)

**Status:** Implemented
**Change:** Added `[Parameter] public RenderFragment<int>? WizardButtons` to MariloWizard. When non-null, replaces default button bar. Context value is current step index.

### RES-WIZARD-006: Content, Text, Optional, Valid parameters (GAP-006, GAP-012)

**Status:** Implemented
**Change:** Added `Content`, `Text`, `Optional`, `Valid` parameters to WizardStep. `EffectiveContent` property returns `Content ?? ChildContent`. Step indicator shows Valid icons, Text override, and "(Optional)" label.

### RES-WIZARD-007: Width, Height, ShowPager (GAP-009, GAP-010)

**Status:** Implemented
**Change:** Width/Height render as inline styles on root div. ShowPager renders "Step X of Y" in actions area.

### RES-WIZARD-008: StepperPosition (GAP-007)

**Status:** Implemented
**Change:** Added `WizardStepperPosition` enum (Top/Bottom/Left/Right) to `WizardTypes.cs`. Root div gets `mar-wizard--stepper-{position}` CSS class. Left/Right positions add `mar-wizard__steps--vertical` class.

### RES-WIZARD-009: StepTemplate (GAP-011)

**Status:** Implemented
**Change:** Added `StepTemplate` RenderFragment to WizardStep. When non-null, replaces default step indicator (number/icon + label).

### RES-WIZARD-010: Linear parameter (GAP-008)

**Status:** Implemented
**Change:** Replaced `AllowStepClick` with `Linear` (default true). When Linear=true, only current and completed steps are clickable. Breaking change (acceptable).

### RES-WIZARD-011: Keyboard navigation and ARIA (GAP-013, GAP-014)

**Status:** Implemented
**Change:** Added `HandleStepperKeyDown` with Arrow/Home/End navigation (roving tabindex). Added `aria-controls`, `aria-current="step"`, `aria-label` on tabpanel, `tabindex` management.

### RES-WIZARD-012: Disabled step disables adjacent buttons (GAP-015)

**Status:** Implemented
**Change:** Next button checks `_steps[Value + 1].Disabled`. Previous button checks `_steps[Value - 1].Disabled`.

### RES-WIZARD-013: bUnit test suite (GAP-016)

**Status:** Implemented
**File:** Created `tests/Marilo.Tests.Unit/Layout/WizardTests.cs`
30 test methods covering all Batch 1, 2, and 3 features.

### RES-WIZARD-014: Fix demo page (GAP-017)

**Status:** Implemented
**Change:** Updated `samples/Marilo.Demo/Pages/Components/Wizard/Overview.razor` to use `Label` parameter instead of `Title`.

## Tests

- 30 bUnit tests in `WizardTests.cs`
- Covers: step registration, labels, content rendering, Value binding, navigation, Linear mode, disabled steps, WizardButtons, WizardSteps wrapper, Width/Height, ShowPager, StepperPosition, Content priority, Text/Optional/Valid, StepTemplate, OnChange cancellation, ARIA attributes, OnFinish, custom button text
- Cannot run `dotnet test` in this environment (no .NET SDK)

## Files Changed

| File | Action |
|------|--------|
| `src/Marilo.Components/Layout/MariloWizard.razor` | Rewritten (all 14 resolutions) |
| `src/Marilo.Components/Layout/WizardStep.razor` | Modified (7 new parameters) |
| `src/Marilo.Components/Layout/WizardTypes.cs` | Created |
| `src/Marilo.Components/Layout/MariloWizardSteps.razor` | Created |
| `tests/Marilo.Tests.Unit/Layout/WizardTests.cs` | Created |
| `samples/Marilo.Demo/Pages/Components/Wizard/Overview.razor` | Updated |
