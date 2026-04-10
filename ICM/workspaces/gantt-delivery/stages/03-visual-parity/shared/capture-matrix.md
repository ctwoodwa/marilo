# Capture Matrix -- MariloGantt

Defines theme/mode/state combinations for Gantt visual parity review. Gantt is a timeline-intensive component requiring coverage across task bar types, tree column, dependency lines, and splitter layout.

## Theme/Mode Matrix

| Theme | Light | Dark |
|-------|-------|------|
| Fluent | Required | Required |
| Bootstrap | Required | Required |
| Material | Required | Required |

**Total theme/mode combinations:** 6

## Viewport Matrix

| Viewport | Width | Use Case |
|----------|-------|----------|
| Desktop | 1280px | Primary review viewport |
| Wide | 1600px | Timeline density and dependency line clarity |

## Gantt State Inventory

Each state below is a capture point per theme/mode combination.

### Task Bar Types

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Default task bar | Standard task bar at rest with label | Yes |
| 2 | Summary bar | Parent/summary task bar spanning child range | Yes |
| 3 | Milestone diamond | Zero-duration milestone marker | Yes |
| 4 | Progress indicator | Task bar with progress fill overlay | Yes |

### Tree Column

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 5 | Tree column idle | Task name column with expand/collapse icons | Yes |
| 6 | Expanded row | Parent row in expanded state, children visible | Yes |
| 7 | Collapsed row | Parent row in collapsed state | Yes |

### Timeline and Header

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 8 | Timeline header | Date/period header tiers above task bars | Yes |
| 9 | Current date line | Vertical line marking today's date | Yes |

### Interaction States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 10 | Task hover | Mouse over a task bar | Yes |
| 11 | Task selected | Task bar in selected state | Yes |
| 12 | Editing row | Row in inline edit mode | Yes |
| 13 | Dependency lines | Finish-to-start or other dependency connectors | Yes |

**Total state/scenario items:** 13
**Total capture points:** 13 states x 6 theme/modes = 78 (minus N/A for unsupported states)

## Gantt-Specific Gap Categories

When scoring Gantt captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Bar height | Task bar height relative to row height — compactness vs. Telerik |
| Milestone sizing | Diamond size, border weight, fill color at rest and hover |
| Dependency line weight | Line thickness, arrowhead size, color in light and dark |
| Tree column indentation | Pixels per level, alignment with expand icon |
| Timeline density | Header cell width, typography size, tier separator lines |
| Progress fill color | Fill color contrast against task bar background in both modes |
| Splitter | Splitter bar between tree column and timeline pane |
| Current date line | Color and opacity of today marker in light and dark |

## Capture Priority

For first-pass review, prioritize in this order:
1. Default task bar + tree column idle (structural baseline)
2. Timeline header + current date line (layout anchors)
3. Task hover + task selected (interaction quality)
4. Summary bar + milestone diamond (bar type variety)
5. Progress indicator + dependency lines (detail quality)
6. Expanded/collapsed rows + editing row (tree and edit states)
