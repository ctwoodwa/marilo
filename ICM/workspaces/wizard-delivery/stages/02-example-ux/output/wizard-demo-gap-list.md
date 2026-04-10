# MariloWizard — Demo Gap List

**Audit date:** 2026-04-10
**Existing demo page:** `samples/Marilo.Demo/Pages/Components/Wizard/Overview.razor`
**Current scenario count:** 9 (updated from 1)
**Target scenario count:** 9

---

## Demo Scenarios

| # | Section | Scenario | Features Covered |
|---|---------|---------|-----------------|
| 1 | Overview | Basic Wizard | Value binding, WizardStep, Label |
| 2 | Overview | Non-Linear Navigation | Linear=false, click-any-step |
| 3 | Step Configuration | Icons & Optional Steps | Icon, Optional |
| 4 | Step Configuration | Disabled Steps | Disabled parameter |
| 5 | Step Configuration | Step Validation | Valid=true/false/null |
| 6 | Layout | Stepper Position | WizardStepperPosition (Top/Bottom/Left/Right), Height |
| 7 | Layout | Custom Button Text & Pager | PreviousText, NextText, FinishText, ShowPager |
| 8 | Events | OnFinish & OnStepChange | OnFinish, OnStepChange events |
| 9 | Events | Custom Buttons | WizardButtons RenderFragment template |

## Assessment

All implemented parameters, events, and features are now demonstrated with interactive controls. No gaps remain.
