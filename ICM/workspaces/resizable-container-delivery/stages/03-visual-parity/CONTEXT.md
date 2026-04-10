# Visual Parity -- MariloResizableContainer

Compare MariloResizableContainer rendering against the internal Marilo delivery-quality baseline across Bootstrap, Fluent, and Material themes in light and dark modes. Produce structured gap records and remediation-ready outputs.

## When to Enter

- After Stage 02 (Example UX) has produced demo scenarios
- When the `parity` trigger is invoked
- When visual quality of the ResizableContainer needs evaluation against the delivery baseline

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Component identity and artifact paths |
| Stage 02 output | stages/02-example-ux/output/ | Full file | Demo scenarios to capture |
| Capture matrix | shared/capture-matrix.md | Full file | ResizableContainer-specific theme/mode/state combinations |
| Parity rubric | shared/parity-score-rubric.md | Full file | Scoring definitions |
| Gap format | shared/visual-parity-gap-format.md | Full file | Gap record template |
| Remediation template | shared/claude-remediation-template.md | Full file | Handoff format |

## Reference Strategy

**Internal Marilo delivery-quality baseline.** MariloResizableContainer is a simpler utility component with no Telerik Blazor equivalent. Visual parity review is lightweight — 5-7 states, focused on handle visibility, handle cursor, resize border, and constraint indicator across themes. Score against Marilo's delivery-quality bar: correct token usage, legible handle states, and consistent constraint feedback.

## Process

1. Read the capture matrix — MariloResizableContainer has 5-7 state/scenario combinations (lighter treatment than complex components).
2. For each theme (Fluent, Bootstrap, Material) x mode (Light, Dark) x state, capture or review the Marilo rendering.
3. Score each combination using the parity rubric (0-3) against the internal delivery-quality baseline.
4. For any score below 3, create a gap record using the gap format.
5. Classify gaps by category: handle visibility, handle cursor, resize border, constraint indicator.
6. Write output/resizable-container-visual-parity-gaps.md with all gap records.
7. Write output/resizable-container-parity-summary.md with overall scores and coverage.

## Scope

This stage owns visual comparison, scoring, and gap documentation.
This stage does NOT own source implementation changes or test writing — those are handed off via gap records to the gap-analysis workspace.
Keep scope proportionate to component complexity — do not overfit with DataGrid-level analysis.

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Visual parity gaps | output/resizable-container-visual-parity-gaps.md | visual-parity-gap-format.md |
| Parity summary | output/resizable-container-parity-summary.md | Summary with scores per theme/mode |
| Visual parity plan | output/resizable-container-visual-parity-plan.md | Starter plan (seeded at stage creation) |
