# Telerik Blazor UI Component Selection Guide

Maps common ASP Classic UI patterns to Telerik Blazor UI components. Use this during Stage 02 (architecture decisions) and Stage 04 (page conversion) to select the right component.

## Data Display

| ASP UI Pattern | Telerik Component | Key Features |
|---------------|-------------------|--------------|
| HTML table with manual sorting/paging | `TelerikGrid` | Built-in sorting, paging, filtering, grouping, column resizing, export |
| Read-only data list | `TelerikListView` | Template-based rendering with paging |
| Hierarchical data display | `TelerikTreeList` | Parent-child rows with expand/collapse |
| Key-value detail view | `TelerikForm` (read-only mode) | Structured layout without edit capability |
| Card/tile layout | `TelerikTileLayout` | Draggable, resizable card containers |

## Forms and Input

| ASP UI Pattern | Telerik Component | Key Features |
|---------------|-------------------|--------------|
| HTML form with validation | `TelerikForm` | Auto-generates fields from model, DataAnnotations validation |
| Text input | `TelerikTextBox` | Floating label, placeholder, validation |
| Multi-line text | `TelerikTextArea` | Auto-resize, character count |
| Number input | `TelerikNumericTextBox` | Min/max, step, format, decimals |
| Password field | `TelerikTextBox` with `Password=true` | Toggle visibility |
| Checkbox | `TelerikCheckBox` | Label, indeterminate state |
| Radio buttons | `TelerikRadioGroup` | Horizontal or vertical layout |

## Selection Controls

| ASP UI Pattern | Telerik Component | Key Features |
|---------------|-------------------|--------------|
| `<select>` dropdown | `TelerikDropDownList` | Data binding, templates, virtualization |
| Searchable dropdown | `TelerikComboBox` | Filtering, custom values, auto-complete |
| Type-ahead search | `TelerikAutoComplete` | Server-side filtering, debounce |
| Multi-select list | `TelerikMultiSelect` | Tags, grouping, templates |
| Cascading dropdowns | Multiple `TelerikDropDownList` with `ValueChanged` | Chain selections via event handlers |

## Date and Time

| ASP UI Pattern | Telerik Component | Key Features |
|---------------|-------------------|--------------|
| Date picker | `TelerikDatePicker` | Min/max dates, format, navigation |
| Date + time picker | `TelerikDateTimePicker` | Combined date and time selection |
| Date range selection | `TelerikDateRangePicker` | Start/end date in one control |
| Time-only input | `TelerikTimePicker` | 12/24 hour format, step |

## Layout and Navigation

| ASP UI Pattern | Telerik Component | Key Features |
|---------------|-------------------|--------------|
| Tab panels | `TelerikTabStrip` | Lazy loading, closable tabs |
| Accordion sections | `TelerikPanelBar` | Expand/collapse, single or multi-expand |
| Navigation menu | `TelerikMenu` | Horizontal/vertical, sub-menus, templates |
| Side drawer/panel | `TelerikDrawer` | Overlay or push mode, mini view |
| Breadcrumb trail | `TelerikBreadcrumb` | Navigation hierarchy |
| Tree navigation | `TelerikTreeView` | Checkboxes, drag-drop, lazy load |
| Toolbar/action bar | `TelerikToolBar` | Buttons, separators, overflow menu |

## Feedback and Dialogs

| ASP UI Pattern | Telerik Component | Key Features |
|---------------|-------------------|--------------|
| Alert/confirm dialogs | `TelerikDialog` | Predefined alert, confirm, prompt |
| Modal window | `TelerikWindow` | Draggable, resizable, modal or modeless |
| Toast notifications | `TelerikNotification` | Auto-dismiss, stacking, templates |
| Progress bar | `TelerikProgressBar` | Determinate and indeterminate |
| Loading spinner | `TelerikLoader` | Overlay, inline, custom size |
| Tooltip | `TelerikTooltip` | Position, template, delay |

## File and Media

| ASP UI Pattern | Telerik Component | Key Features |
|---------------|-------------------|--------------|
| File upload form | `TelerikUpload` | Async, chunk upload, validation |
| File select (no auto-upload) | `TelerikFileSelect` | Client-side file access, drag-drop |
| Image display | `TelerikMediaQuery` + standard `<img>` | Responsive image handling |

## Data Visualization

| ASP UI Pattern | Telerik Component | Key Features |
|---------------|-------------------|--------------|
| Bar/line/pie charts | `TelerikChart` | 30+ chart types, tooltips, legends |
| Gauges/KPI indicators | `TelerikArcGauge`, `TelerikLinearGauge` | Min/max, color ranges |
| Rich text editing | `TelerikEditor` | WYSIWYG, formatting, tables, images |
| Scheduler/calendar view | `TelerikScheduler` | Day/week/month views, recurring events |

## Validation

| ASP UI Pattern | Telerik Component | Key Features |
|---------------|-------------------|--------------|
| Server-side ASP validation | DataAnnotations on model + `TelerikValidationMessage` | Per-field messages |
| Client-side JS validation | `TelerikForm` built-in validation | No custom JS needed |
| Summary of all errors | `TelerikValidationSummary` | Lists all validation failures |

## Usage Notes

- Always wrap Telerik components in a `TelerikRootComponent` in the main layout.
- Use `OnRead` event for server-side data operations on grids and lists to avoid loading all data at once.
- Apply a consistent theme via `TelerikLayout` in `MainLayout.razor`. Default themes: Default, Bootstrap, Material, Fluent.
- Telerik components support `Class` parameter for custom CSS. Avoid overriding internal Telerik CSS selectors.
