# Visual Parity Gap Format

Each visual parity gap record follows this shape. One record per theme/mode/state combination that scores below 3.

## Gap Record

**ID:** VP-gantt-[sequence]
**Component:** MariloGantt
**Theme:** Fluent | Bootstrap | Material
**Mode:** Light | Dark
**State/Scenario:** [e.g., default task bar, summary bar, milestone diamond, progress indicator, tree column idle, expanded row, collapsed row, timeline header, current date line, task hover, task selected, editing row, dependency lines]
**Reference Source:** Telerik Gantt | internal Marilo baseline
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
- Reference source is Telerik Gantt for states where Telerik has a clear equivalent. Use internal Marilo baseline for states unique to MariloGantt.
- Category helps route remediation: token/color issues go to SCSS foundation; layout issues go to component template or SCSS component file.
- Acceptance criteria should be verifiable by screenshot comparison.
- Bar height and milestone sizing gaps should note the specific pixel measurements observed vs. expected.
