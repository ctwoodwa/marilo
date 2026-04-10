# Capture Matrix -- MariloWizard

Defines theme/mode/state combinations for Wizard visual parity review. Wizard is a multi-step navigation component — the step indicator bar and its per-step states are the primary visual differentiators.

## Theme/Mode Matrix

| Theme | Light | Dark |
|-------|-------|------|
| Fluent | Required | Required |
| Bootstrap | Required | Required |
| Material | Required | Required |

**Total theme/mode combinations:** 6

## Viewport Matrix

| Viewport | Width | Use Case |
|----------|-------|----------|
| Desktop | 1280px | Primary review viewport |
| Narrow | 768px | Step indicator wrapping / compact navigation |

## Wizard State Inventory

Each state below is a capture point per theme/mode combination.

### Step Indicator States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Step indicator active | Current step — highlighted, numbered or icon, label visible | Yes |
| 2 | Step indicator completed | Past step — checkmark or filled indicator, distinct from active | Yes |
| 3 | Step indicator pending | Future step — muted, not yet reachable in linear mode | Yes |
| 4 | Connector line | Line between step indicators — weight, color, and completed fill | Yes |

### Content and Navigation

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 5 | Step content area | Content panel below/beside the step bar with padding and border | Yes |
| 6 | Navigation buttons — prev/next | Previous and Next buttons at rest in their default positions | Yes |
| 7 | Navigation buttons — finish | Finish button on the last step | Yes |
| 8 | Button alignment | Spatial relationship of prev/next/finish within the footer area | Yes |

### Step Constraint States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 9 | Disabled step | Step that cannot be navigated to — visual suppression | Yes |
| 10 | Validation error | Step with a validation error indicator on the step indicator | Yes |

### Mode Variants

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 11 | Non-linear mode | All steps clickable — active/pending distinction changes | Yes |

**Total state/scenario items:** 11
**Total capture points:** 11 states x 6 theme/modes = 66 (minus N/A for unsupported states)

## Wizard-Specific Gap Categories

When scoring Wizard captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Step indicator sizing | Circle/icon diameter, font size of step number, label font size |
| Connector line weight | Thickness and color of the line connecting step indicators |
| Active step emphasis | How the active step stands out — color, weight, scale |
| Completed step treatment | Checkmark or filled color — must be clearly distinct from pending |
| Pending step muting | Opacity or color suppression on future steps |
| Button alignment | Left/center/right placement of prev, next, and finish buttons |
| Content padding | Inner padding of the step content area |
| Validation error indicator | Error badge or color on step indicator when step has errors |
| Disabled step opacity | Visual weight reduction on unreachable steps |

## Capture Priority

For first-pass review, prioritize in this order:
1. Step indicator active + step content area (core structural baseline)
2. Navigation buttons prev/next + button alignment (primary interaction chrome)
3. Step indicator completed + connector line (progression visual)
4. Step indicator pending + disabled step (suppression states)
5. Validation error + non-linear mode (edge behavior)
6. Finish button (terminal step variant)
