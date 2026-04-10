# Visual Parity Gap Format -- MariloDockManager

Each visual parity gap record follows this shape. One record per theme/mode/state combination that scores below 3.

## Gap Record

**ID:** VP-dockmanager-[sequence]
**Component:** MariloDockManager
**Theme:** Fluent | Bootstrap | Material
**Mode:** Light | Dark
**State/Scenario:** [e.g., docked panel, floating panel, tab strip, panel header, drag preview, drop indicator, split layout, close/minimize buttons, empty dock zone]
**Reference Source:** internal Marilo baseline
**Parity Score:** 0 | 1 | 2 | 3
**Severity:** critical | major | minor | polish

| Field | Observed in Marilo | Expected (Reference) |
|-------|-------------------|----------------------|
| Description | [what Marilo renders] | [what the internal Marilo delivery-quality baseline expects] |
| Likely cause | [token mismatch, missing SCSS rule, layout issue, etc.] | |

**Category:** panel header height | tab strip styling | drag preview opacity | drop zone indicator | splitter between panels | button icon sizing | token/color | typography | spacing | layout | density | elevation
**Recommended change:** [specific CSS/token/layout adjustment]
**Acceptance criteria:** [what "fixed" looks like — tied to screenshot and state scenario]
**Remediation handoff target:** gap-analysis-resolution intake | SCSS source fix | demo update

## Usage Notes

- One gap record per distinct visual issue. If the same issue affects multiple themes, create one record per theme.
- Reference source is internal Marilo baseline — there is no Telerik Blazor equivalent for DockManager.
- Category helps route remediation: token/color issues go to SCSS foundation; layout issues go to component template or SCSS component file.
- Acceptance criteria should be verifiable by screenshot comparison.
- Use VS Code / JetBrains docking chrome as informal reference when the internal baseline is ambiguous.
