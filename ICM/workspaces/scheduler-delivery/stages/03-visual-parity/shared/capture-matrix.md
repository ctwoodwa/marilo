# Capture Matrix -- MariloScheduler

Defines theme/mode/state combinations for Scheduler visual parity review. Scheduler is a calendar-style component with multiple views, appointment rendering, and time-based interactions.

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
| Wide | 1440px | Timeline view if supported |

## Scheduler State Inventory

Each state below is a capture point per theme/mode combination.

### Calendar Views

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Day view | Single-day calendar with time slots | Yes |
| 2 | Week view | 7-day calendar with time grid | Yes |
| 3 | Month view | Monthly calendar grid (if supported) | Yes |
| 4 | Timeline view | Horizontal timeline layout (if supported) | Yes |

### Time Regions

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 5 | Work-hours region | Visual distinction between work and non-work hours | Yes |
| 6 | All-day area | All-day event row above the time grid | Yes |
| 7 | Current time indicator | Horizontal line marking current time | Yes |
| 8 | Empty timeslot | Unoccupied time slot at rest | Yes |

### Appointments

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 9 | Appointment card default | Standard appointment rendered in a time slot | Yes |
| 10 | Appointment hover | Mouse over an appointment | Yes |
| 11 | Appointment selected | Clicked/selected appointment | Yes |
| 12 | Overlapping appointments | Multiple appointments in the same time slot | Yes |

### Editing and Interaction

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 13 | Popup editor | Appointment edit form in overlay | Yes |
| 14 | Drag preview | Visual feedback during appointment drag (if supported) | Partial |
| 15 | Resize affordance | Drag handle for appointment resize (if supported) | Partial |

### Navigation

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 16 | View-switcher buttons | Day/Week/Month/Timeline tab bar | Yes |
| 17 | Disabled/read-only slot | Non-editable time slot (if supported) | Partial |

**Total state/scenario items:** 17
**Total capture points:** 17 states x 6 theme/modes = 102 (minus N/A for unsupported views/states)

## Scheduler-Specific Gap Categories

When scoring Scheduler captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Calendar grid density | Time slot height, day column width, grid line weight |
| Appointment color treatment | Background color, text contrast, category color bars |
| Border clarity | Grid borders, appointment borders, header separators |
| Time-label readability | Font size, weight, color of hour labels |
| Overlay/popup chrome | Edit popup borders, shadows, background in light/dark |
| Current-day/current-time emphasis | Today highlight, current-time line color and weight |
| Event typography | Appointment title size, weight, truncation |
| Spacing in dense scenarios | Overlapping appointments, narrow time slots |
| View-switcher button styling | Active/inactive tab states, hover, focus ring |

## Capture Priority

For first-pass review, prioritize in this order:
1. Week view + day view (most-used calendar layouts)
2. Appointment card default + hover + selected (core interaction states)
3. Current time indicator + work-hours region (temporal context)
4. All-day area + overlapping appointments (layout complexity)
5. Popup editor (overlay chrome quality)
6. View-switcher buttons (navigation UX)
7. Month/timeline views, drag/resize affordances, disabled slots
