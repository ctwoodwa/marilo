# Marilo vs Telerik Blazor UI — Gap Analysis

Based on the Telerik Blazor documentation at `C:\Projects\blazor-docs` (101 components) compared against Marilo's current 98 components.

---

## Summary

| Category | Telerik Components | Marilo Has | Missing | Partial |
|---|---|---|---|---|
| Data Grid / Tables | 3 | 1 | 2 | 1 |
| Data Visualization | 7 | 0 | 7 | 0 |
| Editors / Rich Input | 4 | 0 | 4 | 0 |
| Selection / Dropdowns | 7 | 2 | 5 | 1 |
| Date / Time | 5 | 4 | 1 | 3 |
| Layout / Containers | 8 | 7 | 2 | 1 |
| Navigation | 7 | 7 | 1 | 2 |
| Buttons / Actions | 5 | 5 | 0 | 2 |
| Feedback / Notifications | 6 | 6 | 0 | 3 |
| Inputs / Form Controls | 10 | 9 | 1 | 5 |
| File Handling | 3 | 1 | 2 | 0 |
| Popup / Overlay | 5 | 3 | 2 | 2 |
| Scheduling | 2 | 0 | 2 | 0 |
| AI Components | 3 | 0 | 3 | 0 |
| Media / Documents | 3 | 0 | 3 | 0 |
| Misc / Utility | 6 | 2 | 3 | 1 |

---

## 1. Missing Components (Not in Marilo at all)

### Critical — High-value components most apps need

| Telerik Component | Category | Description |
|---|---|---|
| **Grid** | Data | Full data grid with paging, sorting, filtering, grouping, editing, column resizing, virtual scroll |
| **ComboBox** | Selection | Dropdown with text input, filtering, custom values, grouping, templates |
| **DropDownList** | Selection | Simple dropdown selection from predefined list |
| **MultiSelect** | Selection | Multiple item selection from a dropdown list |
| **Dialog** (full) | Overlay | Telerik Dialog has predefined dialog actions, DialogFactory for confirm/alert/prompt |
| **Window** | Overlay | Draggable, resizable popup window with title bar and actions |
| **Form** (full) | Forms | Auto-generates form fields from model, validation integration, form groups, columns |
| **Upload** | Files | Async file upload to server endpoint with progress, validation, chunk upload |

### Important — Common in business apps

| Telerik Component | Category | Description |
|---|---|---|
| **TreeList** | Data | Hierarchical data in tabular format with editing, filtering, sorting |
| **ListView** | Data | Templated repeating layout with paging and editing |
| **Editor** | Rich Input | WYSIWYG HTML editor |
| **Spreadsheet** | Rich Input | Excel-like editor with formulas and formatting |
| **PdfViewer** | Documents | Opens PDF files with paging, zooming, searching, text selection |
| **Chart** | Visualization | Line, bar, area, pie, donut, scatter charts with legends and tooltips |
| **StockChart** | Visualization | Financial OHLC / candlestick charts with navigator |
| **Gantt** | Scheduling | Task timeline with hierarchical data, dependencies, editing |
| **Scheduler** | Scheduling | Calendar views (day/week/month) with appointment CRUD |
| **Map** | Visualization | Geospatial map with tile layers, markers, bubble layers |
| **PivotGrid** | Data | Multi-dimensional data analysis pivot table |
| **Gauges** | Visualization | Circular, linear, radial, arc gauge components |
| **Sankey** | Visualization | Flow visualization between domains |

### Nice to Have — Specialized components

| Telerik Component | Category | Description |
|---|---|---|
| **AIPrompt** | AI | Prompt input for generative AI with views for prompt/output/commands |
| **InlineAIPrompt** | AI | Popup-based AI interaction within content |
| **SmartPasteButton** | AI | AI-powered form fill from unstructured text |
| **Chat** | AI/Comms | Conversational UI with messaging and AI integration |
| **Diagram** | Visualization | Shapes, connectors, and layouts for diagrams |
| **Barcodes** | Visualization | Barcode/QR code generation |
| **FileManager** | Files | Explorer-like file/folder management |
| **Signature** | Input | Drawing area for signatures |
| **DockManager** | Layout | Dockable, floating pane management |
| **TileLayout** | Layout | Draggable/resizable grid tile layout |
| **MediaQuery** | Utility | React to browser size changes |
| **FloatingLabel** | Utility | Animated label that floats on focus |

---

## 2. Partial Matches (Marilo has it but missing Telerik features)

### MariloTextField vs Telerik TextBox
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Two-way binding | `@bind-Value` | `Value` + `ValueChanged` | Equivalent |
| Prefix/Suffix adornments | `TextBoxPrefixTemplate` / `TextBoxSuffixTemplate` | `Prefix` / `Suffix` RenderFragment | **Equivalent** |
| Separator | `ShowPrefixSeparator` / `ShowSuffixSeparator` | `ShowPrefixSeparator` / `ShowSuffixSeparator` | **Equivalent** |
| Label | Separate or FloatingLabel | Not built-in | **Missing** — Label is a separate MariloLabel |
| Readonly | `ReadOnly` parameter | Not supported | **Missing** |
| MaxLength | Built-in | Not supported | **Missing** |
| AutoComplete (browser) | `Autocomplete` parameter | Not supported | **Missing** |
| DebounceDelay | Built-in | Not supported | **Missing** |

### MariloSearchBox vs Telerik TextBox (search mode)
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Composing TextField | Uses TextBox internally | Composes MariloTextField | **Equivalent** |
| Clear button | Built-in | Built-in | **Equivalent** |
| Keyboard hint | N/A | `KbdHint` | **Marilo extra** |
| OnSearch event | Built-in | `OnSearch` callback | **Equivalent** |

### MariloTooltip vs Telerik Tooltip
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Target selector | `TargetSelector` (CSS selector targets multiple) | Wraps single child | **Missing** — Can't attach to external elements |
| Position | `Position` enum | `Position` enum | **Equivalent** |
| Show/Hide events | `OnShow` / `OnHide` | Not supported | **Missing** |
| Template | `<Template>` with context | Plain `Text` string only | **Missing** — No rich content tooltip |
| Width/Height | Configurable | Not supported | **Missing** |
| Show on | Click/Hover/Focus | Hover only | **Missing** |
| Callout | Built-in callout arrow | Not supported | **Missing** |

### MariloButton vs Telerik Button
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Click event | `OnClick` | `OnClick` | **Equivalent** |
| Icon | `Icon` (ISvgIcon) | Via `ChildContent` | **Partial** — No dedicated Icon parameter |
| ThemeColor | `ThemeColor` parameter | `Variant` enum | Different approach, equivalent |
| FillMode | `Solid`, `Outline`, `Flat`, `Link`, `Clear` | `IsOutline` bool | **Missing** — Only 2 fill modes vs 5 |
| Rounded | `Rounded` parameter | Not supported | **Missing** |
| Size | `Size` (string constant) | `ButtonSize` enum | **Equivalent** |
| Enabled | `Enabled` | `Disabled` (inverted) | **Equivalent** |

### MariloDialog vs Telerik Dialog
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Visible | `Visible` two-way | `IsOpen` | **Equivalent** |
| Title | `Title` | `Title` | **Equivalent** |
| Predefined actions | `DialogButtons` (OK, Cancel, Yes, No, etc.) | Not supported | **Missing** |
| DialogFactory | `DialogFactory.ConfirmAsync()` / `AlertAsync()` / `PromptAsync()` | Separate `MariloConfirmDialog` | **Missing** programmatic API |
| Width/Height | Configurable | Not supported | **Missing** |
| Modal/Modeless | Both supported | Modal only | **Missing** |
| Draggable | Supported | Not supported | **Missing** |

### MariloSelect vs Telerik DropDownList
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Data binding | `Data` + `TextField` + `ValueField` | Raw `<option>` children | **Missing** — No data-driven binding |
| Filtering | Built-in filter | Not supported | **Missing** |
| Templates | Item, Value, Header, Footer | Not supported | **Missing** |
| Grouping | Built-in | Not supported | **Missing** |
| Virtual scroll | Built-in | Not supported | **Missing** |

### MariloDatePicker vs Telerik DatePicker
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Value binding | `Value` (DateTime?) | `Value` (DateOnly?) | Different type |
| Format | `Format` string | Not supported | **Missing** |
| Calendar popup | Full calendar | Not implemented (stub) | **Missing** — Needs visual calendar |
| Min/Max | `Min` / `Max` | `Min` / `Max` | **Equivalent** |
| Disabled dates | `DisabledDates` | Not supported | **Missing** |
| Adaptive rendering | Mobile-friendly | Not supported | **Missing** |

### MariloGrid (Layout) vs Telerik Grid (Data)
Marilo's `MariloGrid` is a CSS layout grid. Telerik's `Grid` is a full data grid component. These are **completely different components**. Marilo has no data grid equivalent.

### MariloTreeView vs Telerik TreeView
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Data binding | `Data` collection + fields | Manual `<MariloTreeItem>` children | **Missing** — No data-driven binding |
| Templates | Item, Checkbox | Not supported | **Missing** |
| Drag and Drop | Built-in | Not supported | **Missing** |
| Checkboxes | Built-in | Not supported | **Missing** |
| Load on demand | Built-in | Not supported | **Missing** |
| ExpandedItems | Collection binding | Per-item `IsExpanded` | Different approach |

### MariloTabs vs Telerik TabStrip
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Tab content | `<TabStripTab>` children | `<MariloTabPanel>` children | **Equivalent** |
| Closeable tabs | Built-in | Not supported | **Missing** |
| Tab position | Top, Bottom, Left, Right | Not configurable | **Missing** |
| Scrollable tabs | Built-in | Not supported | **Missing** |
| Disabled tabs | Built-in | Not supported | **Missing** |

### MariloSlider vs Telerik Slider
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Value | `Value` | `Value` | **Equivalent** |
| Min/Max/Step | Built-in | Built-in | **Equivalent** |
| LargeStep | Built-in tick marks | Not supported | **Missing** |
| Label template | Built-in | Not supported | **Missing** |
| Orientation | Horizontal/Vertical | Not configurable | **Missing** |
| **RangeSlider** | Separate component with `StartValue`/`EndValue` | Not available | **Missing component** |

### MariloCheckbox vs Telerik Checkbox
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Value | `Value` | `Checked` | **Equivalent** |
| Indeterminate | `Indeterminate` parameter | Not supported | **Missing** |
| Label | Via `<label>` | `Label` parameter | **Equivalent** |

### MariloIcon vs Telerik TelerikSvgIcon
| Feature | Telerik | Marilo | Gap |
|---|---|---|---|
| Icon parameter | `Icon` (ISvgIcon) | `Icon` (RenderFragment) | Different approach |
| ChildContent | Custom SVG | Custom SVG | **Equivalent** |
| Size | `Size` string | `Size` enum | **Equivalent** |
| Flip | `Flip` enum | `Flip` enum | **Equivalent** |
| ThemeColor | `ThemeColor` string | `ThemeColor` enum | **Equivalent** |
| Name lookup | N/A | `Name` string | **Marilo extra** |
| AriaLabel | N/A | `AriaLabel` | **Marilo extra** |

---

## 3. Components Marilo Has That Telerik Doesn't

| Marilo Component | Description |
|---|---|
| MariloAlertStrip | Multi-alert severity strip |
| MariloDataBanner | Real-time data change banner |
| MariloDataToast | Real-time data change toast notifications |
| MariloEnvironmentBadge | Environment indicator (DEV/STAGING/PROD) |
| MariloTimeRangeSelector | Time range quick-select (Now, 1h, 24h, 7d, 30d) |
| MariloHighlighter | Text highlighting/search-match display |
| MariloCallout | Styled callout blocks (info/warning/danger) |
| MariloSegmentedControl | iOS-style segmented selector |
| MariloChipSet | Chip collection container |

---

## 4. Priority Recommendations

### P0 — Add immediately (blocks most business apps)
1. **MariloDataGrid** — Full data grid with paging, sorting, filtering, column templates, editing
2. **MariloComboBox** — Filterable dropdown with data binding, templates, custom values
3. **MariloDropDownList** — Simple data-bound dropdown (MariloSelect upgrade)
4. **MariloMultiSelect** — Multi-item selection dropdown

### P1 — Add soon (common requirements)
5. **MariloWindow** — Draggable/resizable popup window
6. **MariloChart** — Line, bar, pie, donut charts
7. **MariloEditor** — WYSIWYG HTML editor
8. **MariloListView** — Templated data-bound repeating layout
9. **MariloUpload** — Async file upload with progress
10. **MariloNotification** — Programmatic notification service

### P2 — Enhance existing components
11. MariloButton — Add `FillMode` (Flat, Link, Clear), `Rounded`, dedicated `Icon` parameter
12. MariloTextField — Add `ReadOnly`, `MaxLength`, `DebounceDelay`
13. MariloTooltip — Add `TargetSelector`, template content, show-on modes, width/height
14. MariloDialog — Add `DialogFactory` service, predefined actions, width/height, draggable
15. MariloSelect — Upgrade to data-bound with filtering, templates, grouping
16. MariloDatePicker — Add visual calendar, format string, disabled dates
17. MariloTreeView — Add data binding, checkboxes, drag-and-drop
18. MariloCheckbox — Add `Indeterminate` state
19. MariloTabs — Add closeable, scrollable, disabled, position options
20. MariloSlider — Add orientation, large step, label template

### P3 — Future roadmap
21. Scheduler / Gantt
22. PdfViewer / Spreadsheet
23. AI components (AIPrompt, Chat)
24. Map / Diagram / Barcodes
25. DockManager / TileLayout
