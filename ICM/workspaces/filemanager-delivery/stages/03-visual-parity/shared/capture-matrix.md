# Capture Matrix -- MariloFileManager

Defines theme/mode/state combinations for FileManager visual parity review. FileManager is a dual-pane composite component requiring coverage across tree navigation, file grid, toolbar, breadcrumb, context menu, and upload area.

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
| Wide | 1600px | File grid density at larger widths |

## FileManager State Inventory

Each state below is a capture point per theme/mode combination.

### Layout Structure

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Default view | FileManager at rest with tree pane, file grid, toolbar, and breadcrumb | Yes |
| 2 | Tree navigation pane | Left tree pane showing folder hierarchy | Yes |
| 3 | File grid pane | Right pane showing files and folders as grid or list | Yes |
| 4 | Pane splitter | Splitter bar between tree and file grid panes | Yes |

### Navigation and Selection

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 5 | Breadcrumb | Path breadcrumb at rest and with multiple segments | Yes |
| 6 | Selected item | File or folder in selected state | Yes |
| 7 | Item hover | Mouse over a file or folder item | Yes |

### Toolbar and Search

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 8 | Toolbar idle | Toolbar at rest with action buttons | Yes |
| 9 | Search input | Search input field at rest and focused | Yes |

### File/Folder Presentation

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 10 | File/folder icons | File and folder icon sizing and color | Yes |
| 11 | Context menu | Right-click context menu open | Yes |

### Edge States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 12 | Upload area | File upload drop zone or upload panel | Yes |
| 13 | Empty folder | File grid showing empty folder state | Yes |
| 14 | Tree expanded/collapsed | Tree node in expanded and collapsed states | Yes |

**Total state/scenario items:** 14
**Total capture points:** 14 states x 6 theme/modes = 84 (minus N/A for unsupported states)

## FileManager-Specific Gap Categories

When scoring FileManager captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Pane splitter | Splitter bar visibility, drag handle size, alignment between tree and file grid |
| Tree indentation | Pixels per level in the folder tree, expand icon alignment |
| File grid density | Item sizing, icon-to-label spacing, grid vs. list view density |
| Icon sizing | File and folder icon dimensions and color in light and dark |
| Breadcrumb separator | Separator character/icon weight and spacing between path segments |
| Context menu chrome | Menu border, background, shadow, and item hover state |
| Toolbar spacing | Button spacing and separator weight in the toolbar |
| Search input chrome | Input border, placeholder color, focus ring in light and dark |

## Capture Priority

For first-pass review, prioritize in this order:
1. Default view + tree navigation pane (structural baseline)
2. File grid pane + selected item (primary content area)
3. Item hover + breadcrumb (navigation interaction quality)
4. Toolbar idle + pane splitter (chrome quality)
5. File/folder icons + context menu (icon and menu quality)
6. Search input + tree expanded/collapsed (input and tree states)
7. Upload area + empty folder (edge cases)
