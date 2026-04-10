# Visual Parity Gap Format -- MariloResizableContainer

Each visual parity gap record follows this shape. One record per theme/mode/state combination that scores below 3.

## Gap Record

**ID:** VP-resizable-container-[sequence]
**Component:** MariloResizableContainer
**Theme:** Fluent | Bootstrap | Material
**Mode:** Light | Dark
**State/Scenario:** [e.g., container default, resize handle idle, resize handle hover, resize handle active, min/max constraint visible, corner handles]
**Reference Source:** internal Marilo baseline
**Parity Score:** 0 | 1 | 2 | 3
**Severity:** critical | major | minor | polish

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | [what Marilo renders] | [what the internal Marilo delivery-quality baseline expects] |
| Likely cause | [token mismatch, missing SCSS rule, layout issue, etc.] | |

**Category:** handle visibility | handle cursor | resize border | constraint indicator | token/color | spacing | layout
**Recommended change:** [specific CSS/token/layout adjustment]
**Acceptance criteria:** [what "fixed" looks like — tied to screenshot and state scenario]
**Remediation handoff target:** gap-analysis-resolution intake | SCSS source fix | demo update

## Usage Notes

- One gap record per distinct visual issue. If the same issue affects multiple themes, create one record per theme.
- Reference source is internal Marilo baseline — there is no Telerik equivalent for ResizableContainer.
- This is a simple utility component — keep gap records proportionate. Do not overfit with DataGrid-level gap density.
- Category helps route remediation: token/color issues go to SCSS foundation; layout issues go to component template or SCSS component file.
