# Visual Parity Gap Format -- MariloDataSheet

Each visual parity gap record follows this shape. One record per theme/mode/state combination that scores below 3.

## Gap Record

**ID:** VP-datasheet-[sequence]
**Component:** MariloDataSheet
**Theme:** Fluent | Bootstrap | Material
**Mode:** Light | Dark
**State/Scenario:** [e.g., cell grid default, selected cell, cell range selection, cell editing, column/row headers, frozen rows/columns, formula bar, sheet tabs, scrollbar]
**Reference Source:** internal Marilo baseline
**Parity Score:** 0 | 1 | 2 | 3
**Severity:** critical | major | minor | polish

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | [what Marilo renders] | [what the internal Marilo delivery-quality baseline expects] |
| Likely cause | [token mismatch, missing SCSS rule, layout issue, etc.] | |

**Category:** cell border weight | header background | selection highlight | editing input chrome | frozen separator | sheet tab styling | token/color | typography | spacing | layout | density | elevation
**Recommended change:** [specific CSS/token/layout adjustment]
**Acceptance criteria:** [what "fixed" looks like — tied to screenshot and state scenario]
**Remediation handoff target:** gap-analysis-resolution intake | SCSS source fix | demo update

## Usage Notes

- One gap record per distinct visual issue. If the same issue affects multiple themes, create one record per theme.
- Reference source is internal Marilo baseline — there is no Telerik equivalent for DataSheet.
- Category helps route remediation: token/color issues go to SCSS foundation; layout issues go to component template or SCSS component file.
- Acceptance criteria should be verifiable by screenshot comparison.
- Use spreadsheet-standard UX conventions (Excel/Google Sheets visual grammar) as informal reference when the internal baseline is ambiguous.
