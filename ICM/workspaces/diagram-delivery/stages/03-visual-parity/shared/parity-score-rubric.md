# Parity Score Rubric -- MariloDiagram

## Scores

| Score | Label | Definition |
|-------|-------|------------|
| 0 | Materially different | Layout, structure, or behavior does not match; immediately noticeable to any user |
| 1 | Noticeably off | Correct structure but wrong tokens, spacing, or sizing; spotted within seconds |
| 2 | Close but visible | Minor deviations in spacing, color shade, or typography weight; spotted on side-by-side comparison |
| 3 | Visually equivalent | No meaningful difference at normal inspection distance |

## Severity Levels

| Severity | Definition | Action |
|----------|------------|--------|
| Critical | Score 0-1 on a primary state (node default, node selected, node hover, connector line) in any theme | Must fix before delivery gate |
| Major | Score 0-1 on a secondary state, or score 2 on a primary state | Should fix this phase |
| Minor | Score 2 on a secondary state | Fix if time permits |
| Polish | Score 2 on edge states (minimap, text label only, empty canvas) | Backlog for next phase |

## Primary vs. Secondary vs. Edge States

- **Primary:** node default, node selected, node hover, connector line
- **Secondary:** canvas background, zoom controls, group container, connector endpoint
- **Edge:** minimap, text label only, empty canvas

## Mismatch Type Guidance

When a score is below 3, classify the root cause:

| Mismatch Type | Description | Typical Fix Path |
|---------------|-------------|------------------|
| Token-level | Wrong CSS custom property value or missing dark-mode override | SCSS foundation file (colors, spacing, typography) |
| Component-level | Correct tokens but wrong CSS rule, selector, or layout in the component SCSS | SCSS component file |
| Demo/example | Component renders correctly but the demo page sets it up in a way that looks wrong | Demo page update |
| Missing-state coverage | The state is not implemented at all (e.g., no selection handles, no minimap) | Gap-analysis intake for implementation |

## Scoring Guidelines

- Score the Marilo rendering against the reference at the same theme and mode. Do not cross-compare themes.
- If no Telerik reference exists for a state, score against internal Marilo delivery-quality expectations.
- A score of 3 does not require pixel-perfect matching — it means a user would not notice a difference at normal usage distance.
- When in doubt between two scores, choose the lower score and note "borderline" in the gap record.
- Node selected state is high-visibility — selection handles must be clearly present and correctly sized; missing or misaligned handles score 0 or 1.
- Canvas grid is a secondary concern — subtle grid presence is expected; complete absence on a themed canvas should score 1.
