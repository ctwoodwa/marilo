# Visual Parity -- MariloChart

Compare MariloChart rendering against Telerik Chart visual baseline across Bootstrap, Fluent, and Material themes in light and dark modes. Produce structured gap records and remediation-ready outputs.

## When to Enter

- After Stage 02 (Example UX) has produced demo scenarios
- When the `parity` trigger is invoked
- When visual quality of the Chart needs evaluation against reference

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Component identity and artifact paths |
| Stage 02 output | stages/02-example-ux/output/ | Full file | Demo scenarios to capture |
| Capture matrix | shared/capture-matrix.md | Full file | Chart-specific theme/mode/state combinations |
| Parity rubric | shared/parity-score-rubric.md | Full file | Scoring definitions |
| Gap format | shared/visual-parity-gap-format.md | Full file | Gap record template |
| Remediation template | shared/claude-remediation-template.md | Full file | Handoff format |

## Reference Strategy

**Telerik Chart parity.** The Telerik Blazor Chart is the visual reference baseline for Chart scenarios including line, bar, pie, and area series, axis labels, gridlines, tooltips, legends, and interactive states. Marilo does not need to clone every Telerik behavior — the comparison targets visual quality, series color fidelity, and state treatment.

## Process

1. Read the capture matrix — Chart has ~14 primary state/scenario combinations.
2. For each theme (Fluent, Bootstrap, Material) x mode (Light, Dark) x state, capture or review the Marilo rendering.
3. Compare against Telerik Chart reference for the same theme/mode/state where applicable.
4. Score each combination using the parity rubric (0-3).
5. For any score below 3, create a gap record using the gap format.
6. Classify gaps by category: token/color, typography, spacing, layout, iconography, density, elevation, state treatment.
7. Write output/chart-visual-parity-gaps.md with all gap records.
8. Write output/chart-parity-summary.md with overall scores and coverage.

## Scope

This stage owns visual comparison, scoring, and gap documentation.
This stage does NOT own source implementation changes or test writing — those are handed off via gap records to the gap-analysis workspace.

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Visual parity gaps | output/chart-visual-parity-gaps.md | visual-parity-gap-format.md |
| Parity summary | output/chart-parity-summary.md | Summary with scores per theme/mode |
| Visual parity plan | output/chart-visual-parity-plan.md | Starter plan (seeded at stage creation) |
