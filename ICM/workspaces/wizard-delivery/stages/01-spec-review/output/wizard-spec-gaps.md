# MariloWizard Spec Gap List

> Generated: 2026-04-10
> Source branch: `workInProgress` @ `ef3b8f2`
> Spec path: `docs/component-specs/wizard/`
> Source path: `src/Marilo.Components/Layout/`

---

## Source Inventory

### MariloWizard.razor (inherits MariloComponentBase)

| Parameter | Type | Default | Notes |
|-----------|------|---------|-------|
| `Class` | `string?` | `null` | Inherited from `MariloComponentBase` |
| `Style` | `string?` | `null` | Inherited from `MariloComponentBase` |
| `ChildContent` | `RenderFragment?` | `null` | Container for WizardStep children |
| `Value` | `int` | `0` | Zero-based current step index |
| `ValueChanged` | `EventCallback<int>` | -- | Two-way binding callback |
| `Linear` | `bool` | `true` | Restricts navigation to sequential steps |
| `PreviousText` | `string?` | `null` (renders "Previous") | Custom text for Previous button |
| `NextText` | `string?` | `null` (renders "Next") | Custom text for Next button |
| `FinishText` | `string?` | `null` (renders "Finish") | Custom text for Finish/Done button |
| `OnFinish` | `EventCallback` | -- | Fires when Done/Finish button clicked |
| `OnStepChange` | `EventCallback<int>` | -- | Fires after step change (receives new index) |
| `WizardButtons` | `RenderFragment<int>?` | `null` | Custom button template (context = current step index) |
| `Width` | `string?` | `null` | CSS width |
| `Height` | `string?` | `null` | CSS height |
| `ShowPager` | `bool` | `false` | Renders "Step X of Y" label |
| `StepperPosition` | `WizardStepperPosition` | `Top` | Stepper placement (Top/Bottom/Left/Right) |

### WizardStep.razor (implements IDisposable)

| Parameter | Type | Default | Notes |
|-----------|------|---------|-------|
| `Label` | `string?` | `null` | Step label text |
| `ChildContent` | `RenderFragment?` | `null` | Fallback content if `Content` not set |
| `Content` | `RenderFragment?` | `null` | Primary step content (takes priority) |
| `Disabled` | `bool` | `false` | Disables the step |
| `Icon` | `string?` | `null` | Icon identifier |
| `Text` | `string?` | `null` | Indicator text (overrides step number) |
| `Optional` | `bool` | `false` | Shows "(Optional)" under label |
| `Valid` | `bool?` | `null` | Validation state indicator |
| `StepTemplate` | `RenderFragment?` | `null` | Full custom template for step indicator |
| `OnChange` | `EventCallback<WizardStepChangeEventArgs>` | -- | Fires before leaving this step; supports cancellation |

### MariloWizardSteps.razor

| Parameter | Type | Default | Notes |
|-----------|------|---------|-------|
| `ChildContent` | `RenderFragment?` | `null` | Pass-through wrapper, renders children directly |

### WizardTypes.cs

| Type | Members | Notes |
|------|---------|-------|
| `WizardStepperPosition` (enum) | `Top`, `Bottom`, `Left`, `Right` | Stepper placement enum |
| `WizardStepChangeEventArgs` (class) | `TargetIndex (int)`, `IsCancelled (bool)` | Cancellable step-change event args |

---

## Gap Records

### A. Undocumented (in source, not in spec)

#### A-01: `OnStepChange` event on MariloWizard
- **Priority:** P2
- **Source:** `MariloWizard.razor` line 137 -- `[Parameter] public EventCallback<int> OnStepChange { get; set; }`
- **Spec:** Not listed in the overview parameter table or the events page. The events page documents `OnChange` (on WizardStep), `ValueChanged`, and `OnFinish`. The buttons page mentions `OnStepChange` in the nav flow description (lines 27, 32) but as a slug reference to the step-level `OnChange` -- it does not document a separate wizard-level `OnStepChange` event.
- **Impact:** Consumers have no spec guidance for this wizard-level event that fires after every step change. Overlaps conceptually with `ValueChanged` but fires separately.

#### A-02: `PreviousText` parameter on MariloWizard
- **Priority:** P2
- **Source:** `MariloWizard.razor` line 133 -- `[Parameter] public string? PreviousText { get; set; }`
- **Spec:** Not mentioned anywhere. The buttons page documents default button behavior but not how to customize button labels.
- **Impact:** Users cannot discover button text customization from the spec.

#### A-03: `NextText` parameter on MariloWizard
- **Priority:** P2
- **Source:** `MariloWizard.razor` line 134 -- `[Parameter] public string? NextText { get; set; }`
- **Spec:** Not mentioned anywhere.
- **Impact:** Same as A-02.

#### A-04: `FinishText` parameter on MariloWizard
- **Priority:** P2
- **Source:** `MariloWizard.razor` line 135 -- `[Parameter] public string? FinishText { get; set; }`
- **Spec:** Not mentioned anywhere. The spec calls the button "Done" but source defaults to "Finish".
- **Impact:** Same as A-02, plus naming inconsistency (spec says "Done", source says "Finish").

#### A-05: `ChildContent` parameter on WizardStep (fallback content)
- **Priority:** P3
- **Source:** `WizardStep.razor` line 11 -- `[Parameter] public RenderFragment? ChildContent { get; set; }`
- **Spec:** Only `Content` render fragment is documented. `ChildContent` exists as a fallback (Content takes priority over ChildContent).
- **Impact:** Minor -- the `Content` tag is the documented pattern. `ChildContent` is implicit Blazor behavior. However, the dual-content priority is undocumented.

#### A-06: Keyboard navigation implementation details
- **Priority:** P3
- **Source:** `MariloWizard.razor` lines 231-254 -- `HandleStepperKeyDown` implements Arrow keys (direction-aware for vertical/horizontal), Home, End with roving tabindex.
- **Spec:** The accessibility page (wai-aria-support.md) references keyboard navigation only via a link to a demo. No spec documents which keys are supported or the roving tabindex pattern.
- **Impact:** No testable keyboard spec. Developers and QA cannot verify keyboard behavior against written requirements.

---

### B. Spec-Ahead (in spec, not in source)

#### B-01: `WizardSettings` / `WizardStepperSettings` child component
- **Priority:** P1
- **Source:** Not implemented. No `WizardSettings.razor` or `WizardStepperSettings.razor` exists.
- **Spec:** stepper.md lines 32, 47, 88; buttons.md line 179 -- extensively documented as `<WizardSettings><WizardStepperSettings StepType="..." Linear="..." /></WizardSettings>`.
- **Impact:** Blocking. The spec documents `Linear` and `StepType` as properties of `WizardStepperSettings`, but the source puts `Linear` directly on `MariloWizard`. Consumers following the spec will get a compilation error. All spec examples using `<WizardSettings>` are broken.

#### B-02: `StepType` parameter (via WizardStepperSettings)
- **Priority:** P1
- **Source:** Not implemented anywhere. No `StepperStepType` enum exists.
- **Spec:** stepper.md lines 36-69 -- `StepType` parameter on `WizardStepperSettings` with enum `StepperStepType` (`Steps`, `Labels`). Controls whether stepper renders indicators+labels or labels only.
- **Impact:** Blocking. An entire display mode feature is specified but missing from the source.

#### B-03: `Icon` rendering in step indicator
- **Priority:** P2
- **Source:** `WizardStep` accepts an `Icon` parameter (line 14) but `MariloWizard.razor` never renders it. The step indicator area (lines 38-51) only renders `Valid` icons, `Text`, or step number. Icons are ignored entirely in the default step template.
- **Spec:** stepper.md lines 116-149 -- Icons are documented as renderable indicators, with priority rules matching the Stepper component.
- **Impact:** Consumers setting `Icon` on `WizardStep` will see no visual effect unless they provide a `StepTemplate`. The parameter exists but is non-functional.

#### B-04: `ShowPager` default value mismatch
- **Priority:** P2
- **Source:** `MariloWizard.razor` line 141 -- `public bool ShowPager { get; set; }` defaults to `false` (C# default for bool).
- **Spec:** overview.md line 85 -- `ShowPager | bool (true)` -- spec says default is `true`.
- **Impact:** Out-of-box behavior differs from spec. Users expecting the pager to appear by default will be confused.

#### B-05: WAI-ARIA selector/attribute table references non-existent CSS classes
- **Priority:** P3
- **Source:** Source uses classes like `mar-wizard__step`, `mar-wizard__step--active`, `mar-wizard__step--completed`, `mar-wizard__content`.
- **Spec:** wai-aria-support.md uses Kendo-style selectors: `.k-step-list`, `.k-step`, `.k-step-link`, `.k-step-disabled`, `.k-step-current`, `.k-wizard-step`. None of these exist in the source.
- **Impact:** The ARIA documentation table is entirely wrong for the actual implementation. Not functionally blocking but makes accessibility audits against the spec impossible.

---

### C. Mismatch (both exist but differ)

#### C-01: `Linear` parameter location
- **Priority:** P1
- **Source:** `Linear` is a direct parameter on `MariloWizard` (line 132).
- **Spec:** `Linear` is documented as a parameter on `WizardStepperSettings` (stepper.md line 75), not on `MariloWizard` directly.
- **Impact:** Blocking API shape mismatch. Source: `<MariloWizard Linear="false">`. Spec: `<WizardStepperSettings Linear="false">`. Both work differently but only the source pattern compiles.

#### C-02: Done/Finish button label naming
- **Priority:** P2
- **Source:** Default button text is "Finish" (line 115: `@(FinishText ?? "Finish")`). The event is `OnFinish`. The parameter is `FinishText`.
- **Spec:** buttons.md line 36 calls it "Done" button. events.md line 127 says "the Done button". The `OnFinish` event name matches between source and spec.
- **Impact:** Cosmetic inconsistency. Users reading the spec expect a "Done" label but see "Finish" at runtime.

#### C-03: `OnChange` event handler signature in spec examples
- **Priority:** P2
- **Source:** `OnChange` on `WizardStep` is `EventCallback<WizardStepChangeEventArgs>` (always receives args).
- **Spec:** stepper.md line 320 shows `void OnChangeHandler1()` with no parameters. This is valid C# (EventCallback<T> allows parameterless handlers) but misleading -- the handler won't receive the args object for cancellation. Other spec examples correctly show the args parameter.
- **Impact:** Inconsistent spec examples may confuse consumers about the correct handler signature.

#### C-04: `WizardButtons` context type documentation
- **Priority:** P3
- **Source:** `WizardButtons` is `RenderFragment<int>` -- context is the current step index as `int`.
- **Spec:** buttons.md lines 100-123 show `WizardButtons` usage with `var index = context;` which is correct, but the parameter type and context type are never formally documented in the overview parameter table.
- **Impact:** Minor -- the examples are correct but the parameter table in overview.md omits `WizardButtons` entirely.

---

## Summary

| Category | Count | P1 | P2 | P3 |
|----------|-------|----|----|----|
| A. Undocumented | 6 | 0 | 4 | 2 |
| B. Spec-Ahead | 5 | 2 | 2 | 1 |
| C. Mismatch | 4 | 1 | 2 | 1 |
| **Total** | **15** | **3** | **8** | **4** |

### P1 Blockers (must resolve before next phase)
1. **B-01** -- `WizardSettings`/`WizardStepperSettings` component missing from source
2. **B-02** -- `StepType` display mode feature missing from source
3. **C-01** -- `Linear` parameter location mismatch (direct vs. nested settings)

### Recommendation
The 3 P1 gaps represent an architectural decision: either implement `WizardSettings`/`WizardStepperSettings` to match the spec, or update the spec to match the current flat-parameter pattern on `MariloWizard`. Given the cerebrum note about rejecting nested child-component patterns (cf. picker tumbler step convention), the recommended path is to **update the spec** to reflect the flat-parameter API and add `StepType` as a direct parameter on `MariloWizard`.
