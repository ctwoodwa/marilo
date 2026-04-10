# Visual Parity -- MariloDockManager

Compare MariloDockManager rendering against the internal Marilo delivery-quality baseline across Bootstrap, Fluent, and Material themes in light and dark modes. Produce structured gap records and remediation-ready outputs.

## When to Enter

- After Stage 02 (Example UX) has produced demo scenarios
- When the `parity` trigger is invoked
- When visual quality of the DockManager needs evaluation against the delivery baseline

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Component identity and artifact paths |
| Stage 02 output | stages/02-example-ux/output/ | Full file | Demo scenarios to capture |
| Capture matrix | shared/capture-matrix.md | Full file | DockManager-specific theme/mode/state combinations |
| Parity rubric | shared/parity-score-rubric.md | Full file | Scoring definitions |
| Gap format | shared/visual-parity-gap-format.md | Full file | Gap record template |
| Remediation template | shared/claude-remediation-template.md | Full file | Handoff format |

## Reference Strategy

**Internal Marilo delivery-quality baseline.** MariloDockManager has no Telerik Blazor equivalent. Visual parity review scores against Marilo's own delivery-quality bar: consistent tokens, correct state treatment, appropriate density, and dock-manager-standard UX conventions (VS Code / JetBrains docking chrome as informal reference for panel and tab strip behavior).

## Process

1. Read the capture matrix — MariloDockManager has ~12 primary state/scenario combinations.
2. For each theme (Fluent, Bootstrap, Material) x mode (Light, Dark) x state, capture or review the Marilo rendering.
3. Score each combination using the parity rubric (0-3) against the internal delivery-quality baseline.
4. For any score below 3, create a gap record using the gap format.
5. Classify gaps by category: panel header height, tab strip styling, drag preview opacity, drop zone indicator, splitter between panels, button icon sizing, token/color, typography, density.
6. Write output/dockmanager-visual-parity-gaps.md with all gap records.
7. Write output/dockmanager-parity-summary.md with overall scores and coverage.

## Scope

This stage owns visual comparison, scoring, and gap documentation.
This stage does NOT own source implementation changes or test writing — those are handed off via gap records to the gap-analysis workspace.

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Visual parity gaps | output/dockmanager-visual-parity-gaps.md | visual-parity-gap-format.md |
| Parity summary | output/dockmanager-parity-summary.md | Summary with scores per theme/mode |
| Visual parity plan | output/dockmanager-visual-parity-plan.md | Starter plan (seeded at stage creation) |
