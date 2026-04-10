# Visual Parity Gap Format

Each visual parity gap record follows this shape. One record per theme/mode/state combination that scores below 3.

## Gap Record

**ID:** VP-diagram-[sequence]
**Component:** MariloDiagram
**Theme:** Fluent | Bootstrap | Material
**Mode:** Light | Dark
**State/Scenario:** [e.g., node default, node selected, node hover, connector line, connector endpoint, canvas background, zoom controls, minimap, group container, text label only, empty canvas]
**Reference Source:** Telerik Diagram | internal Marilo baseline
**Parity Score:** 0 | 1 | 2 | 3
**Severity:** critical | major | minor | polish

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | [what Marilo renders] | [what the reference shows] |
| Likely cause | [token mismatch, missing SCSS rule, layout issue, etc.] | |

**Category:** token/color | typography | spacing | layout | iconography | density | elevation | state treatment
**Recommended change:** [specific CSS/token/layout adjustment]
**Acceptance criteria:** [what "fixed" looks like — tied to screenshot and state scenario]
**Remediation handoff target:** gap-analysis-resolution intake | SCSS source fix | demo update

## Usage Notes

- One gap record per distinct visual issue. If the same issue affects multiple themes, create one record per theme.
- Reference source is Telerik Diagram for states where Telerik has a clear equivalent. Use internal Marilo baseline for states unique to Marilo.
- Category helps route remediation: token/color issues go to SCSS foundation; layout issues go to component template or SCSS component file.
- Acceptance criteria should be verifiable by screenshot comparison.
