# Telerik Blazor UI Component Selection Guide

Maps common .NET Framework UI patterns to Telerik Blazor UI components. Use during Stage 02 (architecture decisions) and Stage 04 (UI conversion) to select the right component.

## Data Display

| Framework UI Pattern | Telerik Component | Key Features |
|---------------------|-------------------|--------------| 
| `GridView` (WebForms) | `TelerikGrid` | Built-in sorting, paging, filtering, grouping, column resizing, export |
| `@Html.Grid()` / HTML table (MVC) | `TelerikGrid` | Built-in sorting, paging, filtering, grouping, column resizing, export |
| `Repeater` / `DataList` (WebForms) | `TelerikListView` | Template-based rendering with paging |
| `TreeView` (WebForms) | `TelerikTreeList` | Parent-child rows with expand/collapse |
| `DetailsView` / `FormView` (read-only) | `TelerikForm` (read-only mode) | Structured layout without edit capability |
| `ListView` (WebForms) | `TelerikListView` | Template-based with paging and sorting |

## Forms and Input

| Framework UI Pattern | Telerik Component | Key Features |
|---------------------|-------------------|--------------| 
| `<asp:TextBox>` / `@Html.TextBoxFor()` | `TelerikTextBox` | Floating label, placeholder, validation |
| `<asp:TextBox TextMode="MultiLine">` / `@Html.TextAreaFor()` | `TelerikTextArea` | Auto-resize, character count |
| `<asp:TextBox TextMode="Number">` | `TelerikNumericTextBox` | Min/max, step, format, decimals |
| `<asp:TextBox TextMode="Password">` | `TelerikTextBox` with `Password=true` | Toggle visibility |
| `<asp:CheckBox>` / `@Html.CheckBoxFor()` | `TelerikCheckBox` | Label, indeterminate state |
| `<asp:RadioButtonList>` / radio inputs | `TelerikRadioGroup` | Horizontal or vertical layout |
| `<asp:Panel>` / `<asp:PlaceHolder>` | Standard `<div>` or Blazor `RenderFragment` | No direct component needed |
| WebForms `EditItemTemplate` | `TelerikGrid` inline/popup editing | Built-in CRUD editing modes |

## Selection Controls

| Framework UI Pattern | Telerik Component | Key Features |
|---------------------|-------------------|--------------| 
| `<asp:DropDownList>` / `@Html.DropDownListFor()` | `TelerikDropDownList` | Data binding, templates, virtualization |
| Searchable dropdown / autocomplete | `TelerikComboBox` | Filtering, custom values, auto-complete |
| `<asp:ListBox>` / multi-select | `TelerikMultiSelect` | Tags, grouping, templates |
| Cascading `<asp:DropDownList>` | Multiple `TelerikDropDownList` with `ValueChanged` | Chain selections via event handlers |

## Date and Time

| Framework UI Pattern | Telerik Component | Key Features |
|---------------------|-------------------|--------------| 
| `<asp:Calendar>` / jQuery UI datepicker | `TelerikDatePicker` | Min/max dates, format, navigation |
| Date + time input | `TelerikDateTimePicker` | Combined date and time selection |
| Date range selection | `TelerikDateRangePicker` | Start/end date in one control |

## Layout and Navigation

| Framework UI Pattern | Telerik Component | Key Features |
|---------------------|-------------------|--------------| 
| `<asp:Menu>` / `@Html.ActionLink()` nav | `TelerikMenu` | Horizontal/vertical, sub-menus, templates |
| `<asp:SiteMapPath>` / breadcrumbs | `TelerikBreadcrumb` | Navigation hierarchy |
| Tab panels / `<asp:MultiView>` | `TelerikTabStrip` | Lazy loading, closable tabs |
| Accordion / `<asp:Accordion>` | `TelerikPanelBar` | Expand/collapse, single or multi-expand |
| `<asp:TreeView>` | `TelerikTreeView` | Checkboxes, drag-drop, lazy load |
| Side navigation drawer | `TelerikDrawer` | Overlay or push mode, mini view |
| Toolbar / button bar | `TelerikToolBar` | Buttons, separators, overflow menu |

## Feedback and Dialogs

| Framework UI Pattern | Telerik Component | Key Features |
|---------------------|-------------------|--------------| 
| JavaScript `alert()`/`confirm()` | `TelerikDialog` | Predefined alert, confirm, prompt |
| Modal popup / `<asp:ModalPopupExtender>` | `TelerikWindow` | Draggable, resizable, modal or modeless |
| Toast/notification messages | `TelerikNotification` | Auto-dismiss, stacking, templates |
| `<asp:UpdateProgress>` / loading indicator | `TelerikLoader` | Overlay, inline, custom size |
| Tooltip / `<asp:HoverMenuExtender>` | `TelerikTooltip` | Position, template, delay |

## File and Media

| Framework UI Pattern | Telerik Component | Key Features |
|---------------------|-------------------|--------------| 
| `<asp:FileUpload>` / `<input type="file">` | `TelerikUpload` | Async, chunk upload, validation |
| `HtmlEditorExtender` / rich text | `TelerikEditor` | WYSIWYG, formatting, tables, images |

## Data Visualization

| Framework UI Pattern | Telerik Component | Key Features |
|---------------------|-------------------|--------------| 
| `<asp:Chart>` / Chart.js | `TelerikChart` | 30+ chart types, tooltips, legends |
| KPI gauges | `TelerikArcGauge`, `TelerikLinearGauge` | Min/max, color ranges |
| Scheduler / calendar view | `TelerikScheduler` | Day/week/month views, recurring events |

## Validation

| Framework UI Pattern | Telerik Component | Key Features |
|---------------------|-------------------|--------------| 
| `<asp:RequiredFieldValidator>` | DataAnnotations `[Required]` + `TelerikValidationMessage` | Per-field messages |
| `<asp:ValidationSummary>` | `TelerikValidationSummary` | Lists all validation failures |
| `@Html.ValidationMessageFor()` (MVC) | `TelerikValidationMessage` | Per-field inline validation |
| jQuery Validate (unobtrusive) | `TelerikForm` built-in validation | No custom JS needed |
| `<asp:RegularExpressionValidator>` | `[RegularExpression]` DataAnnotation | Server and client validation |
| `<asp:CompareValidator>` | `[Compare]` DataAnnotation | Cross-field validation |
| `<asp:CustomValidator>` | Custom `ValidationAttribute` or `EditContext` validation | Flexible server-side rules |

## Usage Notes

- Always wrap Telerik components in a `TelerikRootComponent` in the main layout.
- Use `OnRead` event for server-side data operations on grids and lists to avoid loading all data at once.
- Apply a consistent theme via `TelerikLayout` in `MainLayout.razor`. Default themes: Default, Bootstrap, Material, Fluent.
- Telerik components support `Class` parameter for custom CSS. Avoid overriding internal Telerik CSS selectors.
