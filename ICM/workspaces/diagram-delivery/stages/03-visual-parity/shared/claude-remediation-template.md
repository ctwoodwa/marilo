# Claude Remediation Template -- MariloDiagram

Use this template to hand a visual parity gap to Claude for remediation. Copy, fill in the fields, and provide as a prompt.

---

## Remediation Request

**Gap ID:** VP-diagram-[sequence]
**Component:** MariloDiagram
**Theme:** [theme]
**Mode:** [light/dark]
**State:** [state/scenario]

### Problem

[One-sentence description of what looks wrong]

### Expected

[What the correct rendering should look like — reference screenshot path or description]

### Observed

[What Marilo currently renders — screenshot path or description]

### Likely Cause

[Token mismatch, missing SCSS rule, layout issue, wrong variable, etc.]

### Scope

Limit changes to the minimum required to fix this gap:

- [ ] SCSS token/variable fix (preferred for systemic issues)
- [ ] Component SCSS rule adjustment
- [ ] Component template adjustment
- [ ] Demo page update
- [ ] Other: [specify]

**Token-first rule:** If the issue is a color, spacing, or typography mismatch that could recur across components, fix the token in the SCSS foundation layer (foundation/_colors.scss, foundation/_spacing.scss, etc.) rather than patching the component file. Component-level CSS changes are only appropriate when the issue is unique to this component's layout or structure.

### Files to Check

| File | What to Look For |
|------|-----------------|
| [SCSS foundation file] | [token or variable to adjust] |
| [SCSS component file] | [specific selector or rule, e.g., .marilo-diagram__node, .marilo-diagram__connector] |
| [Component .razor/.cs file] | [specific markup or class] |

### Acceptance Criteria

[What "fixed" looks like — tied to a specific screenshot scenario and theme/mode]

### Regression Risks

Before applying this fix, verify it does not break:

- [ ] Other themes (if changing a shared token, test Fluent + Bootstrap + Material)
- [ ] Other mode (if changing a light token, verify dark mode; vice versa)
- [ ] Other states of this component (e.g., fixing node default should not break node selected or node hover)
- [ ] Other components (if changing a foundation token, scan for consumers)

### Constraints

- Do not change component API or parameters
- Do not modify unrelated styles
- Preserve all existing functionality
- Prefer token-level fixes over component-level overrides
- Test across all three themes if changing shared tokens
- Test both light and dark if changing mode-sensitive values
