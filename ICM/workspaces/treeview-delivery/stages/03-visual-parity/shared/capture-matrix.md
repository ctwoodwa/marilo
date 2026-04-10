# Capture Matrix -- TreeView

Defines theme/mode/state combinations for TreeView visual parity review. TreeView is a hierarchical component with node-level interactions and indentation structure.

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

## TreeView State Inventory

Each state below is a capture point per theme/mode combination.

### Node Structure

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Default hierarchy | Tree with 2-3 levels expanded, mixed leaf and parent nodes | Yes |
| 2 | Expanded node | Parent node in expanded state with children visible | Yes |
| 3 | Collapsed node | Parent node in collapsed state, disclosure icon pointing right/down | Yes |

### Node Interactions

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 4 | Hovered node | Mouse over a tree node | Yes |
| 5 | Selected node | Single node in selected state | Yes |
| 6 | Focused node | Keyboard focus visible on a node | Yes |
| 7 | Disabled node | Node rendered in disabled/dimmed state | Yes |

### Checkboxes

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 8 | Checkbox unchecked | Node with unchecked checkbox | Yes |
| 9 | Checkbox checked | Node with checked checkbox | Yes |
| 10 | Checkbox indeterminate | Parent node with mixed children (tri-state) | Yes |

### Content Layout

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 11 | Icon + text alignment | Nodes with icons and text, verifying baseline alignment | Yes |
| 12 | Nested indentation rhythm | 3+ levels deep, verifying consistent indent spacing | Yes |

### Advanced States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 13 | Load-on-demand indicator | Loading spinner/indicator during async child load | Yes |
| 14 | Templated node | Custom template rendering within node if supported | Partial |
| 15 | Empty tree | Tree with no data, empty state message | N/A |

**Total state/scenario items:** 15
**Total capture points:** 15 states x 6 theme/modes = 90 (minus N/A for unsupported states)

## TreeView-Specific Gap Categories

When scoring TreeView captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Indentation spacing | Consistent indent per level, matching reference rhythm |
| Disclosure icon size/alignment | Expand/collapse arrow size, vertical centering with text |
| Checkbox geometry | Size, border-radius, check mark weight and centering |
| Tri-state visuals | Indeterminate dash vs partial fill, color treatment |
| Node hover/selected treatment | Background tint, border, contrast against siblings |
| Icon/text baseline alignment | Icon and text vertically aligned on the same baseline |
| Density and nesting rhythm | Vertical spacing between nodes, padding within nodes |
| Contrast in dark mode | Node text, icons, and borders readable on dark backgrounds |

## Capture Priority

For first-pass review, prioritize in this order:
1. Default hierarchy + expanded/collapsed nodes (structural baseline)
2. Hovered node + selected node (interaction states)
3. Checkbox states including indeterminate (tri-state quality)
4. Icon + text alignment + indentation rhythm (layout precision)
5. Focused node + disabled node (accessibility states)
6. Load-on-demand + templated node + empty tree
