# Visual Parity -- MariloTreeList

Compare MariloTreeList rendering against Telerik TreeList visual baseline across Bootstrap, Fluent, and Material themes in light and dark modes. Produce structured gap records and remediation-ready outputs.

## When to Enter

- After Stage 02 (Example UX) has produced demo scenarios
- When the `parity` trigger is invoked
- When visual quality of the TreeList needs evaluation against reference

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Component identity and artifact paths |
| Stage 02 output | stages/02-example-ux/output/ | Full file | Demo scenarios to capture |
| Capture matrix | shared/capture-matrix.md | Full file | TreeList-specific theme/mode/state combinations |
| Parity rubric | shared/parity-score-rubric.md | Full file | Scoring definitions |
| Gap format | shared/visual-parity-gap-format.md | Full file | Gap record template |
| Remediation template | shared/claude-remediation-template.md | Full file | Handoff format |

## Reference Strategy

**Telerik TreeList parity.** The Telerik Blazor TreeList is the visual reference baseline for TreeList scenarios including hierarchical rows, expand/collapse, inline editing, sorting, filtering, paging, checkbox selection, and row interaction states. Marilo does not need to clone every Telerik behavior — the comparison targets visual quality, density, and state treatment.

## Process

1. Read the capture matrix — TreeList has ~15 primary state/scenario combinations.
2. For each theme (Fluent, Bootstrap, Material) x mode (Light, Dark) x state, capture or review the Marilo rendering.
3. Compare against Telerik TreeList reference for the same theme/mode/state where applicable.
4. Score each combination using the parity rubric (0-3).
5. For any score below 3, create a gap record using the gap format.
6. Classify gaps by category: token/color, typography, spacing, layout, iconography, density, elevation, state treatment.
7. Write output/treelist-visual-parity-gaps.md with all gap records.
8. Write output/treelist-parity-summary.md with overall scores and coverage.

## Scope

This stage owns visual comparison, scoring, and gap documentation.
This stage does NOT own source implementation changes or test writing — those are handed off via gap records to the gap-analysis workspace.

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Visual parity gaps | output/treelist-visual-parity-gaps.md | visual-parity-gap-format.md |
| Parity summary | output/treelist-parity-summary.md | Summary with scores per theme/mode |
| Visual parity plan | output/treelist-visual-parity-plan.md | Starter plan (seeded at stage creation) |
