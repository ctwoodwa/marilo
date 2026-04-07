# Gap Record Format

Standard shape for normalized gap records. Every gap imported or identified during intake must conform to this format.

## Record Shape

```markdown
### GAP-[AREA]-[NNN]: [Short title]

**Area:** [Component, module, or architectural area]
**Severity:** Critical | High | Medium | Low
**Theme:** [Cross-cutting theme, if any]
**Source:** [File path or reference to original gap analysis]

**Target behavior:** [What the spec/standard/target state says should exist]

**Current behavior:** [What actually exists today]

**Impact:** [What is broken, missing, risky, or inconsistent because of this gap]

**Recommended direction:** [Brief description of the likely resolution path]

**Status:** Open | In Design | In Progress | Resolved | Deferred | Won't Fix
```

## ID Convention

- Format: `GAP-[AREA]-[NNN]`
- AREA: uppercase short name for the component or module (e.g., BUTTON, CHIP, AUTH, NAV)
- NNN: zero-padded sequential number within the area (e.g., 001, 002)
- Examples: `GAP-BUTTON-001`, `GAP-CHIPSET-003`, `GAP-GRID-012`

## Severity Definitions

| Severity | Meaning |
|----------|---------|
| **Critical** | Blocks core functionality, causes data loss, or violates compliance. Must resolve before any release. |
| **High** | Missing major feature or API surface that users/consumers expect. Significant usability or integration impact. |
| **Medium** | Incomplete feature, naming mismatch, or inconsistency that causes confusion but has workarounds. |
| **Low** | Minor omission, convenience parameter, or cosmetic inconsistency. No functional impact. |

## Grouping Rules

- Group gaps by area (component/module) in the inventory.
- Within each area, sort by severity (Critical first, Low last).
- Tag cross-cutting gaps (gaps that appear identically across multiple areas) with a shared theme name so they can be resolved together.

## Theme Examples

Themes capture patterns that repeat. Common examples:

- `missing-enabled-parameter` -- Multiple components lack an Enabled/Disabled toggle.
- `icon-type-inconsistency` -- Different components use different types for their Icon property.
- `missing-focus-method` -- Multiple interactive components lack a FocusAsync() method.
- `naming-mismatch` -- Component name in code differs from spec name.
- `missing-accessibility-params` -- AriaLabel, TabIndex, etc. missing across components.
