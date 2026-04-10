# Visual Parity Gap Format

Each visual parity gap record follows this shape. One record per theme/mode/state combination that scores below 3.

## Gap Record

**ID:** VP-allocation-scheduler-[sequence]
**Component:** MariloAllocationScheduler
**Theme:** Fluent | Bootstrap | Material
**Mode:** Light | Dark
**State/Scenario:** [e.g., default view, resource panel, timeline header, occupied cell, cell hover, cell editing, drag-fill, conflict indicator, current period highlight, disabled cell]
**Reference Source:** Telerik Scheduler | internal Marilo baseline
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
- Reference source is Telerik Scheduler for states where Telerik has a clear equivalent. Use internal Marilo baseline for states unique to MariloAllocationScheduler (e.g., conflict indicators, drag-fill preview).
- Category helps route remediation: token/color issues go to SCSS foundation; layout issues go to component template or SCSS component file.
- Acceptance criteria should be verifiable by screenshot comparison.
- Dark mode tint gaps caused by color-mix base color: note the specific variable name in Likely cause.
