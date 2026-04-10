# Parity Score Rubric -- MariloResizableContainer

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
| Critical | Score 0-1 on a primary state (container default, resize handle hover, resize handle active) in any theme | Must fix before delivery gate |
| Major | Score 0-1 on a secondary state, or score 2 on a primary state | Should fix this phase |
| Minor | Score 2 on a secondary state (min/max constraint) | Fix if time permits |
| Polish | Score 2 on edge states (corner handles) | Backlog for next phase |

## Primary vs. Secondary vs. Edge States

- **Primary:** container default, resize handle hover, resize handle active
- **Secondary:** min/max constraint visible
- **Edge:** corner handles

## Mismatch Type Guidance

When a score is below 3, classify the root cause:

| Mismatch Type | Description | Typical Fix Path |
|---------------|-------------|------------------|
| Token-level | Wrong CSS custom property value or missing dark-mode override | SCSS foundation file (colors, spacing) |
| Component-level | Correct tokens but wrong CSS rule or selector in the component SCSS | SCSS component file |
| Demo/example | Component renders correctly but demo page sets it up in a way that looks wrong | Demo page update |
| Missing-state coverage | The state is not implemented (e.g., no constraint indicator) | Gap-analysis intake for implementation |

## Scoring Guidelines

- Score the Marilo rendering against the internal Marilo delivery-quality baseline. There is no Telerik equivalent for ResizableContainer.
- Keep scoring proportionate to component complexity — this is a utility component, not a data-dense grid.
- A score of 3 does not require pixel-perfect matching — it means a user would not notice a difference at normal usage distance.
- When in doubt between two scores, choose the lower score and note "borderline" in the gap record.
