# Capture Matrix -- MariloEditor

Defines theme/mode/state combinations for Editor visual parity review. Editor is a composite component requiring coverage across toolbar states, content area, dialog chrome, and mode variants.

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
| Narrow | 900px | Toolbar wrapping and button overflow behavior |

## Editor State Inventory

Each state below is a capture point per theme/mode combination.

### Toolbar States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Toolbar default | Toolbar at rest with all buttons idle | Yes |
| 2 | Toolbar button hover | Single toolbar button in hover state | Yes |
| 3 | Toolbar button active | Toolbar button in active/pressed state | Yes |
| 4 | Toolbar button focus | Toolbar button with keyboard focus ring | Yes |
| 5 | Toolbar separator | Vertical separator between button groups | Yes |

### Content Area

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 6 | Content area idle | Editable content area at rest with sample text | Yes |
| 7 | Formatting active | Text with bold, italic, and list formatting applied | Yes |
| 8 | Placeholder | Content area empty, placeholder text visible | Yes |

### Dialogs and Panels

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 9 | Link dialog | Insert/edit link dialog open | Yes |
| 10 | Image dialog | Insert/edit image dialog open | Yes |

### Mode Variants

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 11 | Source view | HTML source view mode active | Yes |
| 12 | Readonly mode | Editor in readonly/disabled state | Yes |

**Total state/scenario items:** 12
**Total capture points:** 12 states x 6 theme/modes = 72 (minus N/A for unsupported states)

## Editor-Specific Gap Categories

When scoring Editor captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Toolbar button sizing | Button height, width, icon size, and padding within toolbar |
| Toolbar separator | Separator line weight, height, and margin |
| Content area padding | Internal padding of the editable content area |
| Dialog chrome | Dialog border, header, footer, shadow, and background in light and dark |
| Formatting indicator states | Active/toggled state of Bold, Italic, List buttons — color and background |
| Focus ring quality | Keyboard focus ring visibility and thickness on toolbar buttons |
| Readonly mode treatment | Visual distinction of readonly vs. editable state |
| Dark mode toolbar surface | Toolbar background, button hover tints in dark mode |

## Capture Priority

For first-pass review, prioritize in this order:
1. Toolbar default + content area idle (structural baseline)
2. Toolbar button hover + active + focus (interaction quality)
3. Formatting active + placeholder (content area states)
4. Toolbar separator (density detail)
5. Link dialog + image dialog (dialog chrome quality)
6. Source view + readonly mode (mode variant quality)
