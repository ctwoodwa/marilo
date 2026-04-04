# Wizard Gap Prioritization

> Date: 2026-04-03
> Source: gap-wizard-inventory.md (18 gaps)
> Stage: 02-prioritize

## Priority Batches

### Batch 1: Foundational Fixes (Critical)

These gaps block basic functionality or make every spec example incompatible.

| Gap | Severity | Description | Effort |
|-----|----------|-------------|--------|
| GAP-WIZARD-018 | Critical* | Missing CascadingValue for WizardStep registration -- wizard is fundamentally broken | S -- wrap ChildContent in CascadingValue |
| GAP-WIZARD-001 | Critical | Value parameter naming mismatch (ActiveStepIndex vs Value) | S -- rename parameter + EventCallback |
| GAP-WIZARD-002 | Critical | Missing WizardSteps wrapper component | S -- add pass-through wrapper |
| GAP-WIZARD-005 | Critical | Missing WizardStepChangeEventArgs type | S -- create class with TargetIndex + IsCancelled |
| GAP-WIZARD-004 | Critical | Missing per-step OnChange event with cancelable args (depends on 005) | M -- add OnChange to WizardStep, fire before navigation, check IsCancelled |
| GAP-WIZARD-003 | Critical | Missing WizardButtons custom buttons RenderFragment | M -- add RenderFragment<int>, replace default bar when provided |

*GAP-WIZARD-018 is classified Medium in the inventory but is functionally Critical -- without CascadingValue, no steps register and the wizard is completely non-functional. Promoted to Batch 1.

### Batch 2: API Completeness (High)

These gaps fill out the spec-defined API surface.

| Gap | Severity | Description | Effort |
|-----|----------|-------------|--------|
| GAP-WIZARD-006 | High | Missing Content RenderFragment on WizardStep | S -- rename ChildContent or add Content alias |
| GAP-WIZARD-009 | High | Missing Width and Height parameters | S -- add string parameters, render as inline styles |
| GAP-WIZARD-010 | High | Missing ShowPager parameter | S -- add parameter + "Step X of Y" rendering |
| GAP-WIZARD-012 | High | Missing WizardStep parameters (Text, Optional, Valid) | M -- add three parameters + stepper rendering logic |
| GAP-WIZARD-007 | High | Missing StepperPosition parameter and enum | M -- add enum + parameter + CSS layout variants |
| GAP-WIZARD-008 | High | Missing WizardSettings / WizardStepperSettings | M -- add settings components or flatten as parameters |
| GAP-WIZARD-011 | High | Missing StepTemplate RenderFragment on WizardStep | M -- add RenderFragment, render in place of default indicator |

### Batch 3: Accessibility + Testing + Polish (Medium)

| Gap | Severity | Description | Effort |
|-----|----------|-------------|--------|
| GAP-WIZARD-013 | Medium | Missing keyboard navigation | M -- add onkeydown handler, roving tabindex pattern |
| GAP-WIZARD-014 | Medium | Incomplete ARIA attributes | M -- add aria-controls, aria-current, tabindex, aria-label |
| GAP-WIZARD-015 | Medium | Disabled step does not disable adjacent navigation buttons | S -- check adjacent step disabled state on button render |
| GAP-WIZARD-016 | Medium | No test coverage | L -- bUnit tests for navigation, events, disabled, ARIA, custom buttons |
| GAP-WIZARD-017 | Medium | Demo page uses non-existent API (Title instead of Label) | M -- rewrite demo with correct API + add demos for events, layout, form integration |

## Recommended Sequence

Start with GAP-WIZARD-018 (CascadingValue fix) immediately -- without it the component is non-functional. Then resolve GAP-WIZARD-001 and GAP-WIZARD-002 to align the public API shape with the spec. Next, add the event infrastructure (005 then 004) and custom buttons (003) to complete Batch 1. Batch 2 items can proceed in parallel once the API shape is stable, prioritizing GAP-WIZARD-006 (Content RenderFragment) and GAP-WIZARD-012 (step parameters) first since other High items build on them. Batch 3 should start with accessibility (013, 014) before tests (016) so the tests can cover the final ARIA behavior, and finish with the demo rewrite (017) which validates the complete API.
