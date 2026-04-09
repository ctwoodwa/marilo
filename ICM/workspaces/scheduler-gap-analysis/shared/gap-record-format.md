# Gap Record Format

Standard shape for normalized gap records. Every gap imported or identified during intake must conform to this format.

## Record Shape

```markdown
### GAP-SCHEDULER-[NNN]: [Short title]

**Area:** [Feature area, e.g., Views, Editing, Recurrence, Resources]
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

- Format: `GAP-SCHEDULER-[NNN]`
- NNN: zero-padded sequential number (e.g., 001, 002)
- Examples: `GAP-SCHEDULER-001`, `GAP-SCHEDULER-012`

## Severity Definitions

| Severity | Meaning |
|----------|---------|
| **Critical** | Blocks core functionality, causes data loss, or violates compliance. Must resolve before any release. |
| **High** | Missing major feature or API surface that users/consumers expect. Significant usability or integration impact. |
| **Medium** | Incomplete feature, naming mismatch, or inconsistency that causes confusion but has workarounds. |
| **Low** | Minor omission, convenience parameter, or cosmetic inconsistency. No functional impact. |

## Scheduler-Specific Area Examples

Common area tags for Scheduler gaps:

- `Views` -- Day, week, month, timeline view rendering and configuration.
- `Editing` -- Create, edit, delete appointment workflows.
- `Recurrence` -- Recurring event rules and exceptions.
- `Resources` -- Resource grouping, assignment, and display.
- `DragDrop` -- Drag-and-drop move and resize interactions.
- `TimeZones` -- Time zone handling and display.
- `Templates` -- Custom templates for slots, headers, and appointments.
- `Toolbar` -- Toolbar commands and navigation controls.
- `DataBinding` -- Data source binding and refresh.
- `Accessibility` -- Keyboard navigation, ARIA attributes, screen reader support.

## Grouping Rules

- Group gaps by area in the inventory.
- Within each area, sort by severity (Critical first, Low last).
- Tag cross-cutting gaps with a shared theme name so they can be resolved together.
