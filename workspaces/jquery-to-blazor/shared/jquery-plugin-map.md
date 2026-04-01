# jQuery Plugin to Telerik Blazor Component Map

Maps common jQuery plugins and libraries to their Telerik Blazor UI equivalents. Use during Stage 01 (plugin identification) and Stage 03 (component mapping).

## Plugin Inventory Checklist

Scan the source page for these common jQuery plugins. Check `<script>` tags, CDN references, `$.fn` extensions, and `node_modules`/`bower_components`.

## Data Display and Grids

| jQuery Plugin | Telerik Blazor Component | Migration Notes |
|--------------|--------------------------|-----------------|
| DataTables (`$.fn.dataTable`) | `TelerikGrid` | Map columns, sorting, paging, filtering, search. Use `OnRead` for server-side operations. Export via `GridExcelExport` or `GridPdfExport` |
| SlickGrid | `TelerikGrid` | Map custom formatters to `GridColumn Template`. Virtual scrolling via `ScrollMode="Virtual"` |
| jqGrid | `TelerikGrid` | Map inline editing to `GridCommandColumn` with edit/save/cancel. Map subgrids to detail template |
| Handsontable | `TelerikSpreadsheet` or `TelerikGrid` with inline edit | For spreadsheet-like editing. TelerikGrid handles simpler inline edit cases |
| jQuery Tablesorter | `TelerikGrid` with `Sortable="true"` | Built-in multi-column sorting |

## Forms and Inputs

| jQuery Plugin | Telerik Blazor Component | Migration Notes |
|--------------|--------------------------|-----------------|
| jQuery Validate | DataAnnotations + `TelerikForm` | Map rules to DataAnnotation attributes: `[Required]`, `[StringLength]`, `[RegularExpression]`, `[Range]` |
| jQuery Unobtrusive Validation | DataAnnotations + `TelerikValidationMessage` | Same model-based validation approach |
| Inputmask / jQuery Mask | `TelerikMaskedTextBox` | Supports phone, SSN, date, currency masks |
| jQuery Autocomplete | `TelerikAutoComplete` | Server-side filtering via `OnRead`, debounce built in |
| Select2 | `TelerikComboBox` or `TelerikMultiSelect` | ComboBox for single select with search; MultiSelect for multi-value |
| Chosen | `TelerikDropDownList` or `TelerikComboBox` | DropDownList for simple select; ComboBox for searchable |
| Bootstrap Tags Input | `TelerikMultiSelect` with `AllowCustom="true"` | Tag-style multi-value input |
| jQuery File Upload (Blueimp) | `TelerikUpload` or `TelerikFileSelect` | Upload for async server upload; FileSelect for client-side access |
| Dropzone.js | `TelerikUpload` with drag-drop | Built-in drag-and-drop upload zone |

## Date and Time

| jQuery Plugin | Telerik Blazor Component | Migration Notes |
|--------------|--------------------------|-----------------|
| jQuery UI Datepicker | `TelerikDatePicker` | Min/max dates, format, disabled dates |
| Bootstrap Datepicker | `TelerikDatePicker` | Same as above |
| jQuery Timepicker | `TelerikTimePicker` | 12/24 hour format, step interval |
| Bootstrap DateTimePicker | `TelerikDateTimePicker` | Combined date + time selection |
| Daterangepicker | `TelerikDateRangePicker` | Start/end date in one control |
| Moment.js (date formatting) | C# `DateTime.ToString(format)` | Or `DateOnly`/`TimeOnly` in .NET 8+ |
| Pikaday | `TelerikDatePicker` | Lightweight date picker replacement |

## Layout and Navigation

| jQuery Plugin | Telerik Blazor Component | Migration Notes |
|--------------|--------------------------|-----------------|
| jQuery UI Tabs | `TelerikTabStrip` | Lazy loading, closable tabs, keyboard nav |
| jQuery UI Accordion | `TelerikPanelBar` | Single or multi-expand mode |
| jQuery UI Sortable (lists) | `TelerikListBox` with reorder | Or `TelerikGrid` with `RowDraggable` |
| jQuery UI Draggable | `TelerikSortable` or custom JSInterop | Limited native drag support; JSInterop for complex cases |
| jQuery UI Resizable | `TelerikSplitter` for panel resizing | Or CSS `resize` property for simple cases |
| jQuery Scrollspy | Blazor `NavigationManager` + scroll event via JSInterop | No native equivalent; consider `TelerikBreadcrumb` for section navigation |
| jQuery Sticky / Affix | CSS `position: sticky` | No plugin needed in modern browsers |

## Dialogs and Feedback

| jQuery Plugin | Telerik Blazor Component | Migration Notes |
|--------------|--------------------------|-----------------|
| jQuery UI Dialog | `TelerikDialog` or `TelerikWindow` | Dialog for confirmations; Window for floating panels |
| Bootstrap Modal | `TelerikDialog` | Map `show`/`hide` to `Visible` parameter binding |
| SweetAlert / SweetAlert2 | `TelerikDialog` with custom template | Map alert types to Dialog icons/styling |
| Toastr | `TelerikNotification` | Map notification types (success, error, warning, info) |
| Noty / Notyf | `TelerikNotification` | Auto-dismiss, stacking, position |
| jQuery UI Tooltip | `TelerikTooltip` | Position, template, delay |
| Bootstrap Tooltip / Popover | `TelerikTooltip` or `TelerikPopover` | Tooltip for hover; Popover for click-triggered rich content |
| jQuery UI Progressbar | `TelerikProgressBar` | Determinate and indeterminate modes |
| NProgress | `TelerikLoader` or custom loading bar | Global loading indicator |
| Spin.js | `TelerikLoader` | Inline or overlay loading spinner |

## Charts and Visualization

| jQuery Plugin | Telerik Blazor Component | Migration Notes |
|--------------|--------------------------|-----------------|
| Chart.js | `TelerikChart` | 30+ chart types, tooltips, legends. Map chart type and dataset config |
| Highcharts | `TelerikChart` | Map series, axes, tooltips to Telerik Chart API |
| Flot | `TelerikChart` | Basic line/bar/pie charts |
| D3.js (simple charts) | `TelerikChart` | For standard chart types. Complex D3 visualizations may need JSInterop |
| jQuery Sparkline | `TelerikSparkline` | Inline mini-charts |
| Gauge.js | `TelerikArcGauge` or `TelerikLinearGauge` | KPI indicators with color ranges |

## Rich Content

| jQuery Plugin | Telerik Blazor Component | Migration Notes |
|--------------|--------------------------|-----------------|
| TinyMCE | `TelerikEditor` | WYSIWYG rich text. Map toolbar configuration |
| CKEditor | `TelerikEditor` | Same as TinyMCE migration |
| Quill | `TelerikEditor` | Map delta format to HTML content |
| jQuery UI Slider | `TelerikSlider` or `TelerikRangeSlider` | Single or range value selection |
| noUiSlider | `TelerikSlider` | Step, min/max, format |
| FullCalendar | `TelerikScheduler` | Day/week/month views, event CRUD |

## Utility Libraries

| Library | .NET / Blazor Equivalent | Migration Notes |
|---------|-------------------------|-----------------|
| Lodash / Underscore | LINQ | `_.map` to `.Select()`, `_.filter` to `.Where()`, `_.find` to `.FirstOrDefault()`, `_.groupBy` to `.GroupBy()` |
| Axios | `HttpClient` | Registered via DI; handles JSON serialization |
| Fetch polyfill | Not needed | Blazor uses `HttpClient` which abstracts transport |
| jQuery Cookie | `ProtectedBrowserStorage` or `IJSRuntime` | Async API for cookie/storage access |
| Store.js | `ProtectedLocalStorage` | Blazor Server protected storage |
| clipboard.js | `IJSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text)` | JSInterop wrapper |
| html2canvas | JSInterop | No native Blazor equivalent; keep as JS with interop |
