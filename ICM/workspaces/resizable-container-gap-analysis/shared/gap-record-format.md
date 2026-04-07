# Gap Record Format -- ResizableContainer

Standard shape for normalized gap records. Every gap imported or identified during intake must conform to this format.

## Record Shape

```markdown
### GAP-RESIZABLE-CONTAINER-[NNN]: [Short title]

**ID:** GAP-RESIZABLE-CONTAINER-[NNN]
**Title:** [Short descriptive title]
**Description:** [What is missing, broken, or inconsistent]
**Category:** [Feature area within ResizableContainer -- e.g., Resizing, Panes, Events, Styling, Accessibility]
**Severity:** Critical | High | Medium | Low
**Status:** Open | In Design | In Progress | Resolved | Deferred | Won't Fix
**Affected files:** [File paths in the source tree]
**Discovery source:** [How this gap was found -- spec review, testing, user report, code audit]

**Target behavior:** [What the spec/standard says should exist]

**Current behavior:** [What actually exists today]

**Impact:** [What is broken, missing, risky, or inconsistent because of this gap]

**Recommended direction:** [Brief description of the likely resolution path]
```

## ID Convention

- Format: `GAP-RESIZABLE-CONTAINER-[NNN]`
- NNN: zero-padded sequential number (e.g., 001, 002, 003)
- Examples: `GAP-RESIZABLE-CONTAINER-001`, `GAP-RESIZABLE-CONTAINER-012`

## Standard Fields

| Field | Required | Description |
|-------|----------|-------------|
| id | Yes | Unique identifier following the convention above |
| title | Yes | Short descriptive title (under 80 characters) |
| description | Yes | Full description of the gap |
| category | Yes | Feature area within the component |
| severity | Yes | Critical, High, Medium, or Low |
| status | Yes | Current lifecycle status |
| affected files | Yes | Source file paths impacted by this gap |
| discovery source | Yes | How the gap was identified |

## Severity Definitions

| Severity | Meaning |
|----------|---------|
| **Critical** | Blocks core functionality, causes data loss, or violates compliance. Must resolve before any release. |
| **High** | Missing major feature or API surface that users/consumers expect. Significant usability or integration impact. |
| **Medium** | Incomplete feature, naming mismatch, or inconsistency that causes confusion but has workarounds. |
| **Low** | Minor omission, convenience parameter, or cosmetic inconsistency. No functional impact. |

## Grouping Rules

- Group gaps by category within the ResizableContainer component.
- Within each category, sort by severity (Critical first, Low last).
- Tag cross-cutting gaps with a shared theme name so they can be resolved together.
