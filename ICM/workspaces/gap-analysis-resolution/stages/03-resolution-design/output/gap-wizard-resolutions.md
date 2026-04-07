# Resolution Design: MariloWizard

> Date: 2026-04-04
> Source: gap-wizard-inventory.md (18 gaps), gap-wizard-priorities.md
> Stage: 03-resolution-design
> Scope: batch (Wizard component gaps)

---

## Batch 1 Resolutions: Foundational Fixes (Critical)

### RES-WIZARD-001: Fix CascadingValue and render ChildContent

**Resolves:** GAP-WIZARD-018
**Status:** Proposed

#### Target Pattern

```razor
@* In MariloWizard.razor, render ChildContent inside CascadingValue so WizardStep children can register *@
<CascadingValue Value="this" IsFixed="true">
    @ChildContent
</CascadingValue>
```

The hidden `ChildContent` renders WizardStep components that self-register via the cascading parameter. The steps themselves render nothing directly — the wizard controls their content rendering in the tabpanel area.

#### Options Considered

**Option A: Add CascadingValue wrapping ChildContent in a hidden container**
- Approach: Add `<div style="display:none"><CascadingValue Value="this" IsFixed="true">@ChildContent</CascadingValue></div>` to the wizard markup, allowing steps to register without rendering visible content.
- Pros: Steps register correctly. Minimal change. Pattern matches MariloSplitter.
- Cons: Hidden div is a DOM node that renders step content invisibly (wasted rendering).
- Effort: S

**Option B: Render ChildContent outside visible area with CSS**
- Approach: Same as A but with `position:absolute;width:0;height:0;overflow:hidden` instead of `display:none`.
- Pros: Steps register. Components inside may need to measure themselves.
- Cons: Slightly more CSS. Still renders content unnecessarily.
- Effort: S

**Option C: Override BuildRenderTree to evaluate ChildContent for registration only**
- Approach: Custom render tree logic that evaluates ChildContent to trigger OnInitialized on steps but discards the output.
- Pros: No hidden DOM nodes.
- Cons: Complex, fragile, fights Blazor's rendering model.
- Effort: L

#### Decision

**Chosen:** Option A
**Rationale:** Simple, proven pattern (used by MariloSplitter). The hidden div is a negligible DOM cost. `display:none` prevents layout participation. WizardStep already renders nothing (`@{ // Render nothing }`), so there's no wasted content rendering.

#### Consequences

- `MariloWizard.razor` adds hidden CascadingValue block
- Steps will begin registering, making the wizard functional
- All other wizard gaps depend on this fix working first

#### Success Criteria

- [ ] WizardStep components register with parent wizard when placed as children
- [ ] `_steps.Count` matches the number of WizardStep children
- [ ] Step labels appear in the stepper
- [ ] Active step content renders in the tabpanel

---

### RES-WIZARD-002: Rename ActiveStepIndex to Value for spec compliance

**Resolves:** GAP-WIZARD-001
**Status:** Proposed

#### Target Pattern

```csharp
[Parameter] public int Value { get; set; }
[Parameter] public EventCallback<int> ValueChanged { get; set; }
```

Consumer usage: `<MariloWizard @bind-Value="currentStep">`

#### Options Considered

**Option A: Rename parameters directly**
- Approach: Rename `ActiveStepIndex` → `Value`, `ActiveStepIndexChanged` → `ValueChanged`. Update all internal references.
- Pros: Clean API. Matches spec. Enables `@bind-Value`.
- Cons: Breaking change for any existing consumers.
- Effort: S

**Option B: Add Value as alias, keep ActiveStepIndex**
- Approach: Add `Value` property that delegates to `ActiveStepIndex`. Keep both.
- Pros: Non-breaking.
- Cons: Dual API surface. Confusing. Two parameters for the same thing is a Blazor anti-pattern (both will fire on SetParametersAsync).
- Effort: M

#### Decision

**Chosen:** Option A
**Rationale:** Pre-release library. The wizard is currently non-functional (GAP-018). No real consumers exist to break. Clean API from the start.

#### Consequences

- `MariloWizard.razor`: `ActiveStepIndex` → `Value`, `ActiveStepIndexChanged` → `ValueChanged`
- All internal `ActiveStepIndex` references in the file update to `Value`
- Demo page must be updated to use `@bind-Value`

#### Success Criteria

- [ ] `MariloWizard` has `Value` and `ValueChanged` parameters
- [ ] `@bind-Value` works in consumer markup
- [ ] `ActiveStepIndex` / `ActiveStepIndexChanged` no longer exist

---

### RES-WIZARD-003: Add MariloWizardSteps pass-through wrapper

**Resolves:** GAP-WIZARD-002
**Status:** Proposed

#### Target Pattern

```razor
<MariloWizard @bind-Value="step">
    <MariloWizardSteps>
        <WizardStep Label="Step 1"><Content>...</Content></WizardStep>
        <WizardStep Label="Step 2"><Content>...</Content></WizardStep>
    </MariloWizardSteps>
</MariloWizard>
```

`MariloWizardSteps` is a pass-through component that renders `@ChildContent`.

#### Options Considered

**Option A: Pass-through component**
- Approach: `MariloWizardSteps.razor` renders `@ChildContent` only. Same pattern as `MariloSplitterPanes`.
- Pros: Spec-compatible markup. Consistent with Splitter pattern.
- Cons: One more file.
- Effort: S

**Option B: Named RenderFragment**
- Approach: `[Parameter] public RenderFragment? Steps { get; set; }` on MariloWizard.
- Pros: No new file.
- Cons: `<Steps>` instead of `<MariloWizardSteps>`. Doesn't match spec.
- Effort: S

#### Decision

**Chosen:** Option A
**Rationale:** Matches spec. Consistent with MariloSplitterPanes pattern.

#### Consequences

- New file: `src/Marilo.Components/Layout/MariloWizardSteps.razor`
- Direct WizardStep children still work (non-breaking)

#### Success Criteria

- [ ] `MariloWizardSteps` component exists
- [ ] Steps register correctly through the wrapper
- [ ] Direct-child pattern still works

---

### RES-WIZARD-004: Add WizardStepChangeEventArgs and per-step OnChange

**Resolves:** GAP-WIZARD-004, GAP-WIZARD-005
**Status:** Proposed

#### Target Pattern

```csharp
// New type
public class WizardStepChangeEventArgs
{
    public int TargetIndex { get; set; }
    public bool IsCancelled { get; set; }
}

// On WizardStep:
[Parameter] public EventCallback<WizardStepChangeEventArgs> OnChange { get; set; }
```

Navigation flow:
1. User clicks Next/Previous/step tab
2. Wizard creates `WizardStepChangeEventArgs { TargetIndex = newIndex }`
3. Wizard fires current step's `OnChange` event
4. If `args.IsCancelled == true`, navigation is aborted
5. Otherwise, `Value` updates and `ValueChanged` fires

#### Options Considered

**Option A: Per-step OnChange with cancellation**
- Approach: Add `OnChange` to `WizardStep`. Before any navigation, fire the current step's OnChange. Check IsCancelled.
- Pros: Matches spec exactly. Enables form validation per step.
- Cons: Slightly more complex navigation logic.
- Effort: M

**Option B: Parent-level OnStepChanging event**
- Approach: Add `OnStepChanging` EventCallback on `MariloWizard` instead of per-step.
- Pros: Simpler — one event handler for all steps.
- Cons: Doesn't match spec. Can't have different validation per step.
- Effort: S

#### Decision

**Chosen:** Option A
**Rationale:** Spec compliance. Per-step validation is the primary use case (form wizard with different validation rules per step).

#### Consequences

- New type: `WizardStepChangeEventArgs` in `src/Marilo.Components/Layout/WizardTypes.cs`
- `WizardStep` gets `OnChange` parameter
- `MariloWizard.GoToStep()` becomes async and checks cancellation
- Existing `OnStepChange` on wizard can remain as a post-navigation notification

#### Success Criteria

- [ ] `WizardStepChangeEventArgs` exists with `TargetIndex` and `IsCancelled`
- [ ] `WizardStep.OnChange` fires before navigation
- [ ] Setting `IsCancelled = true` prevents step change
- [ ] Normal navigation proceeds when IsCancelled is false
- [ ] bUnit test validates cancellation behavior

---

### RES-WIZARD-005: Add WizardButtons custom buttons RenderFragment

**Resolves:** GAP-WIZARD-003
**Status:** Proposed

#### Target Pattern

```razor
<MariloWizard @bind-Value="step">
    <MariloWizardSteps>
        <WizardStep Label="Step 1"><Content>...</Content></WizardStep>
    </MariloWizardSteps>
    <WizardButtons>
        <button @onclick="() => step = 0">Reset</button>
        <button @onclick="() => step++">Custom Next</button>
    </WizardButtons>
</MariloWizard>
```

When `WizardButtons` is provided, it replaces the default Previous/Next/Finish button bar. The `context` value is the current step index.

#### Options Considered

**Option A: RenderFragment<int> parameter on MariloWizard**
- Approach: `[Parameter] public RenderFragment<int>? WizardButtons { get; set; }`. When non-null, render it instead of default buttons, passing `Value` as context.
- Pros: Simple. Matches spec pattern.
- Cons: Named RenderFragment means `<WizardButtons>` tag maps to the parameter. Clean match.
- Effort: S

**Option B: Separate WizardButtons wrapper component**
- Approach: New component that cascades up to the wizard.
- Pros: More explicit.
- Cons: Over-engineered for a render fragment. Harder to pass context back.
- Effort: M

#### Decision

**Chosen:** Option A
**Rationale:** Named RenderFragment is the idiomatic Blazor pattern for this. `<WizardButtons>` in markup maps directly to the parameter name.

#### Consequences

- `MariloWizard.razor` gets `[Parameter] public RenderFragment<int>? WizardButtons { get; set; }`
- Default button bar renders only when `WizardButtons` is null
- When provided, `@WizardButtons(Value)` renders in the actions area

#### Success Criteria

- [ ] `WizardButtons` parameter exists on MariloWizard
- [ ] Default buttons render when WizardButtons is null
- [ ] Custom content renders when WizardButtons is provided
- [ ] Context value equals current step index
- [ ] bUnit test validates custom button rendering

---

## Batch 2 Resolutions: API Completeness (High)

### RES-WIZARD-006: Add Content RenderFragment and step parameters

**Resolves:** GAP-WIZARD-006, GAP-WIZARD-012
**Status:** Proposed

#### Target Pattern

```csharp
// WizardStep parameters:
[Parameter] public RenderFragment? Content { get; set; }
[Parameter] public string? Text { get; set; }
[Parameter] public bool Optional { get; set; }
[Parameter] public bool? Valid { get; set; }
```

- `Content` is the primary render fragment for step body. `ChildContent` becomes a fallback (if `Content` is null, render `ChildContent`).
- `Text` sets custom indicator text (overrides the step number).
- `Optional` adds "(Optional)" label below the step indicator.
- `Valid` shows a check (true) or error icon (false) on the step indicator. Null = no icon.

#### Options Considered

**Option A: Add all four parameters to WizardStep**
- Approach: Direct parameter additions. `Content` takes priority over `ChildContent` for step body.
- Pros: Spec-complete. Straightforward.
- Cons: Minor complexity in render logic to check Content vs ChildContent.
- Effort: M

**Option B: Rename ChildContent to Content**
- Approach: Replace ChildContent with Content outright.
- Pros: Simpler — one render fragment.
- Cons: `ChildContent` is the Blazor convention. Removing it breaks implicit content nesting (`<WizardStep>stuff</WizardStep>` becomes `<WizardStep><Content>stuff</Content></WizardStep>` only).
- Effort: S but worse DX.

#### Decision

**Chosen:** Option A
**Rationale:** Keeping ChildContent as fallback preserves Blazor convention. Adding Content as an explicit alternative satisfies the spec. Both paths are natural.

#### Consequences

- `WizardStep` gets 4 new parameters
- `MariloWizard.razor` stepper rendering updates to show Text, Optional label, and Valid icon
- Content rendering: `_steps[i].Content ?? _steps[i].ChildContent`

#### Success Criteria

- [ ] `WizardStep.Content` renders when provided
- [ ] `WizardStep.ChildContent` renders as fallback
- [ ] `Text` overrides step number in indicator
- [ ] `Optional` shows "(Optional)" text
- [ ] `Valid=true` shows check icon, `Valid=false` shows error icon
- [ ] bUnit tests for each parameter

---

### RES-WIZARD-007: Add Width, Height, and ShowPager parameters

**Resolves:** GAP-WIZARD-009, GAP-WIZARD-010
**Status:** Proposed

#### Target Pattern

```csharp
[Parameter] public string? Width { get; set; }
[Parameter] public string? Height { get; set; }
[Parameter] public bool ShowPager { get; set; } = true;
```

Width/Height render as inline styles. ShowPager renders "Step X of Y" in the actions area.

#### Options Considered

**Option A: Direct parameters with inline style rendering**
- Approach: Add parameters. Render `style="width:{Width};height:{Height}"` on root div. Add pager span when ShowPager=true.
- Pros: Simple, spec-aligned.
- Cons: None meaningful.
- Effort: S

#### Decision

**Chosen:** Option A (single viable option)

#### Consequences

- `MariloWizard.razor` root div gets conditional inline styles
- Actions area gets pager text element
- `CombineStyles()` may need updating to include width/height

#### Success Criteria

- [ ] Width/Height render as inline styles
- [ ] ShowPager=true shows "Step X of Y" text
- [ ] ShowPager=false hides the pager
- [ ] bUnit tests verify rendered styles and pager text

---

### RES-WIZARD-008: Add StepperPosition parameter

**Resolves:** GAP-WIZARD-007
**Status:** Proposed

#### Target Pattern

```csharp
public enum WizardStepperPosition
{
    Top,    // default
    Bottom,
    Left,
    Right
}

[Parameter] public WizardStepperPosition StepperPosition { get; set; } = WizardStepperPosition.Top;
```

CSS layout changes based on position:
- Top (default): stepper above content (current layout)
- Bottom: stepper below content
- Left: stepper left of content (vertical flex)
- Right: stepper right of content (vertical flex)

#### Options Considered

**Option A: Enum + CSS flex-direction control**
- Approach: Apply CSS class `mar-wizard--stepper-{position}` to root div. Define CSS rules for each layout.
- Pros: Clean separation. CSS handles layout. Component just sets the class.
- Cons: Need CSS for 4 layouts.
- Effort: M

**Option B: Conditional render order in markup**
- Approach: Switch on StepperPosition to reorder stepper/content/actions divs in the Razor template.
- Pros: Works without CSS changes.
- Cons: Duplicated markup. Harder to maintain.
- Effort: M

#### Decision

**Chosen:** Option A
**Rationale:** CSS-based layout is cleaner and more maintainable. The component adds a CSS class; the stylesheet handles the rest.

#### Consequences

- New enum: `WizardStepperPosition` in `src/Marilo.Components/Layout/WizardTypes.cs`
- Root div class includes position modifier
- New CSS rules for left/right/bottom positions
- For left/right: stepper list renders vertically with `flex-direction: column`

#### Success Criteria

- [ ] `WizardStepperPosition` enum exists
- [ ] Top position works (default, current behavior)
- [ ] Bottom position moves stepper below content
- [ ] Left/Right positions render stepper vertically beside content
- [ ] bUnit tests verify CSS classes for each position

---

### RES-WIZARD-009: Add StepTemplate RenderFragment

**Resolves:** GAP-WIZARD-011
**Status:** Proposed

#### Target Pattern

```csharp
// On WizardStep:
[Parameter] public RenderFragment? StepTemplate { get; set; }
```

When `StepTemplate` is provided, it replaces the default step indicator (number/icon + label) in the stepper bar.

#### Options Considered

**Option A: RenderFragment parameter on WizardStep**
- Approach: `StepTemplate` replaces the default step button content when non-null.
- Pros: Simple. Consumer has full control over indicator appearance.
- Cons: None.
- Effort: S

#### Decision

**Chosen:** Option A

#### Consequences

- `WizardStep` gets `StepTemplate` parameter
- `MariloWizard.razor` stepper rendering checks `step.StepTemplate != null` and renders it instead of default number+label

#### Success Criteria

- [ ] Custom template renders in place of default indicator
- [ ] Default indicator still works when StepTemplate is null
- [ ] bUnit test validates template rendering

---

### RES-WIZARD-010: Flatten WizardSettings as parameters

**Resolves:** GAP-WIZARD-008
**Status:** Proposed

#### Target Pattern

Rather than creating separate `WizardSettings`/`WizardStepperSettings` wrapper components, flatten the settings as direct parameters on `MariloWizard`:

```csharp
[Parameter] public bool Linear { get; set; } = true;
```

`Linear=true` (default) means non-visited steps cannot be clicked (equivalent to current `AllowStepClick=false`). `Linear=false` means any step is clickable. This replaces `AllowStepClick` with inverted semantics to match the spec.

The `StepType` (Steps vs Labels) concept can be handled by combining `StepperPosition` with CSS rather than a separate enum, since the stepper already shows both numbers and labels. If a consumer wants labels-only or numbers-only, they can use `StepTemplate`.

#### Options Considered

**Option A: Flatten as parameters (Linear replaces AllowStepClick)**
- Approach: Add `Linear` parameter. Remove `AllowStepClick` (replace with `!Linear`).
- Pros: Simpler API. No wrapper components. Spec-compatible name.
- Cons: Breaking change (AllowStepClick removal).
- Effort: S

**Option B: Create WizardSettings/WizardStepperSettings components**
- Approach: Full spec implementation with cascading settings components.
- Pros: Exact spec match.
- Cons: Over-engineered for two boolean parameters. Unintuitive for consumers.
- Effort: M

#### Decision

**Chosen:** Option A
**Rationale:** The settings components in the spec contain only `StepType` and `Linear`. Flattening is cleaner and more idiomatic for Blazor. `StepType` is handled by existing template support.

#### Consequences

- `AllowStepClick` removed from MariloWizard
- `Linear` added (default true)
- Step click logic: `disabled="@(Linear && !isCurrent && !isCompleted)"`

#### Success Criteria

- [ ] `Linear=true` prevents clicking non-visited steps
- [ ] `Linear=false` allows clicking any step
- [ ] `AllowStepClick` no longer exists
- [ ] bUnit test validates both modes

---

## Batch 3 Resolutions: Accessibility + Testing + Polish (Medium)

### RES-WIZARD-011: Keyboard navigation and ARIA compliance

**Resolves:** GAP-WIZARD-013, GAP-WIZARD-014
**Status:** Proposed

#### Target Pattern

Keyboard interactions (WAI-ARIA tablist pattern):
- **Left/Right arrows**: Move focus between step tabs
- **Home**: Focus first step tab
- **End**: Focus last step tab
- **Enter/Space**: Activate focused step (if not disabled and Linear allows it)
- **Tab**: Move focus from stepper to content panel

ARIA attributes:
- Step tabs: `aria-controls="panel-{id}"`, `aria-current="step"` on active, `tabindex=0` on current / `tabindex=-1` on others
- Tabpanel: `id="panel-{id}"`, `aria-label="Step {n} of {total}"`, `tabindex=0`

#### Options Considered

**Option A: @onkeydown handler with roving tabindex**
- Approach: Add `@onkeydown` on the tablist container. Track focused tab index. Manage tabindex attributes per the roving tabindex pattern.
- Pros: Standard WAI-ARIA pattern. Keyboard nav works correctly.
- Cons: More code in the wizard. Need to manage a `_focusedIndex` internal state.
- Effort: M

#### Decision

**Chosen:** Option A (standard approach, no alternatives)

#### Consequences

- Internal `_focusedIndex` state added
- Step buttons get dynamic `tabindex` and `aria-controls`
- Tabpanel gets `id`, `aria-label`, `tabindex=0`
- `aria-current="step"` replaces or supplements `aria-selected`

#### Success Criteria

- [ ] Arrow keys navigate between step tabs
- [ ] Home/End jump to first/last step
- [ ] Enter/Space activates the focused step
- [ ] `aria-controls` links tabs to panels
- [ ] `aria-current="step"` set on active tab
- [ ] Tabpanel has `aria-label` with step count
- [ ] bUnit tests verify ARIA attributes and keyboard event handling

---

### RES-WIZARD-012: Disabled step disables adjacent navigation buttons

**Resolves:** GAP-WIZARD-015
**Status:** Proposed

#### Target Pattern

```razor
@* Next button disabled when next step is disabled *@
<button disabled="@(Value >= _steps.Count - 1 || _steps[Value + 1].Disabled)">Next</button>

@* Previous button disabled when previous step is disabled *@
<button disabled="@(Value <= 0 || _steps[Value - 1].Disabled)">Previous</button>
```

#### Decision

Single viable approach: check adjacent step's Disabled state when rendering buttons.

#### Success Criteria

- [ ] Next button disabled when next step has `Disabled=true`
- [ ] Previous button disabled when previous step has `Disabled=true`
- [ ] bUnit test validates disabled button rendering

---

### RES-WIZARD-013: bUnit test suite

**Resolves:** GAP-WIZARD-016
**Status:** Proposed

#### Target Pattern

`WizardTests.cs` covering:
1. Step registration via CascadingValue
2. Navigation (Next/Previous/GoToStep)
3. `@bind-Value` two-way binding
4. Disabled step blocking
5. `OnChange` cancellation
6. Custom WizardButtons rendering
7. StepperPosition CSS classes
8. Width/Height inline styles
9. ShowPager rendering
10. Content vs ChildContent priority
11. Text/Optional/Valid indicators
12. StepTemplate custom rendering
13. Linear mode step click restriction
14. Keyboard navigation
15. ARIA attributes
16. Adjacent button disabling
17. WizardSteps wrapper compatibility

#### Decision

Single test class, consistent with project conventions (matches TreeView and planned Splitter test patterns).

#### Success Criteria

- [ ] WizardTests.cs exists with ≥20 test methods
- [ ] All tests pass
- [ ] Coverage includes all Batch 1, 2, and 3 features

---

### RES-WIZARD-014: Fix demo page

**Resolves:** GAP-WIZARD-017
**Status:** Proposed

#### Target Pattern

Update `samples/Marilo.Demo/Pages/Components/Wizard/Overview.razor`:
- Use `@bind-Value` instead of `@bind-ActiveStepIndex`
- Use `<MariloWizardSteps>` wrapper
- Use `Label` parameter (not `Title`)
- Add additional demos: form validation per step, custom buttons, stepper positions

#### Decision

Rewrite the existing demo page with spec-compliant API. Add 2-3 additional demo pages.

#### Success Criteria

- [ ] Demo compiles and renders correctly
- [ ] Demo uses spec-compliant API (`@bind-Value`, `<MariloWizardSteps>`, `Label`)
- [ ] At least one demo shows form validation with OnChange cancellation

---

## Summary

| Resolution | Gaps Resolved | Batch | Status | Effort |
|------------|--------------|-------|--------|--------|
| RES-WIZARD-001 | GAP-018 | 1 | Proposed | S |
| RES-WIZARD-002 | GAP-001 | 1 | Proposed | S |
| RES-WIZARD-003 | GAP-002 | 1 | Proposed | S |
| RES-WIZARD-004 | GAP-004, GAP-005 | 1 | Proposed | M |
| RES-WIZARD-005 | GAP-003 | 1 | Proposed | S |
| RES-WIZARD-006 | GAP-006, GAP-012 | 2 | Proposed | M |
| RES-WIZARD-007 | GAP-009, GAP-010 | 2 | Proposed | S |
| RES-WIZARD-008 | GAP-007 | 2 | Proposed | M |
| RES-WIZARD-009 | GAP-011 | 2 | Proposed | S |
| RES-WIZARD-010 | GAP-008 | 2 | Proposed | S |
| RES-WIZARD-011 | GAP-013, GAP-014 | 3 | Proposed | M |
| RES-WIZARD-012 | GAP-015 | 3 | Proposed | S |
| RES-WIZARD-013 | GAP-016 | 3 | Proposed | L |
| RES-WIZARD-014 | GAP-017 | 3 | Proposed | M |

Total: 14 resolutions covering all 18 gaps.
Implementation priority: Batch 1 (critical fixes, wizard is non-functional without them) → Batch 2 (API completeness) → Batch 3 (polish, tests, demos).
