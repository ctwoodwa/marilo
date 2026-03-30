# Marilo vs Telerik Blazor UI — Gap Analysis

Based on the Telerik Blazor documentation at `/workspaces/Marilo/docs/component-specs` (101 components) compared against Marilo's current ~110 component files.

> Last updated: 2026-03-30

---

## Summary

| Category | Spec Components | Marilo Has | Missing | Partial |
|---|---|---|---|---|
| Data Grid / Tables | 3 | 1 | 2 | 0 |
| Data Visualization | 7 | 0 | 7 | 0 |
| Editors / Rich Input | 4 | 0 | 4 | 0 |
| Selection / Dropdowns | 8 | 0 | 5 | 3 |
| Date / Time | 5 | 0 | 1 | 4 |
| Layout / Containers | 11 | 7 | 4 | 0 |
| Navigation | 8 | 7 | 0 | 1 |
| Buttons / Actions | 6 | 5 | 0 | 2 |
| Feedback / Notifications | 7 | 5 | 0 | 3 |
| Inputs / Form Controls | 12 | 9 | 1 | 5 |
| File Handling | 3 | 0 | 2 | 1 |
| Popup / Overlay | 5 | 3 | 2 | 2 |
| Scheduling | 2 | 0 | 2 | 0 |
| AI Components | 4 | 0 | 4 | 0 |
| Media / Documents | 3 | 1 | 2 | 0 |
| Misc / Utility | 6 | 2 | 3 | 1 |

---

## 1. Missing Components (Not in Marilo at all)

### Critical — High-value components most apps need

| Component | Category | Description |
|---|---|---|
| **MariloGrid** | Data | Full data grid with paging, sorting, filtering, grouping, editing, column resizing, virtual scroll |
| **MariloComboBox** | Selection | Dropdown with text input, filtering, custom values, grouping, templates |
| **MariloDropDownList** | Selection | Simple dropdown selection from predefined list |
| **MariloMultiSelect** | Selection | Multiple item selection from a dropdown list |
| **MariloDialog** (full) | Overlay | Predefined dialog actions, DialogFactory for confirm/alert/prompt |
| **MariloWindow** | Overlay | Draggable, resizable popup window with title bar and actions |
| **MariloForm** (full) | Forms | Auto-generates form fields from model, validation integration, form groups, columns |

### Important — Common in business apps

| Component | Category | Description |
|---|---|---|
| **MariloTreeList** | Data | Hierarchical data in tabular format with editing, filtering, sorting |
| **MariloListView** | Data | Templated repeating layout with paging and editing |
| **MariloEditor** | Rich Input | WYSIWYG HTML editor |
| **MariloSpreadsheet** | Rich Input | Excel-like editor with formulas and formatting |
| **MariloPdfViewer** | Documents | Opens PDF files with paging, zooming, searching, text selection |
| **MariloChart** | Visualization | Line, bar, area, pie, donut, scatter charts with legends and tooltips |
| **MariloStockChart** | Visualization | Financial OHLC / candlestick charts with navigator |
| **MariloGantt** | Scheduling | Task timeline with hierarchical data, dependencies, editing |
| **MariloScheduler** | Scheduling | Calendar views (day/week/month) with appointment CRUD |
| **MariloMap** | Visualization | Geospatial map with tile layers, markers, bubble layers |
| **MariloPivotGrid** | Data | Multi-dimensional data analysis pivot table |
| **MariloArcGauge / MariloCircularGauge / MariloLinearGauge / MariloRadialGauge** | Visualization | Circular, linear, radial, arc gauge components |
| **MariloSankey** | Visualization | Flow visualization between domains |
| **MariloUpload** | Files | Async XHR file upload to server endpoint with progress, validation, chunk upload |

### Nice to Have — Specialized components

| Component | Category | Description |
|---|---|---|
| **MariloAIPrompt** | AI | Prompt input for generative AI with views for prompt/output/commands |
| **MariloInlineAIPrompt** | AI | Popup-based AI interaction within content |
| **MariloSmartPasteButton** | AI | AI-powered form fill from unstructured text |
| **MariloChat** | AI/Comms | Conversational UI with messaging and AI integration |
| **MariloDiagram** | Visualization | Shapes, connectors, and layouts for diagrams |
| **MariloBarcode** / **MariloQRCode** | Visualization | Barcode/QR code generation |
| **MariloFileManager** | Files | Explorer-like file/folder management |
| **MariloSignature** | Input | Drawing area for signatures |
| **MariloDockManager** | Layout | Dockable, floating pane management |
| **MariloTileLayout** | Layout | Draggable/resizable grid tile layout |
| **MariloWizard** | Layout | Step-by-step navigation with validation and custom buttons per step |
| **MariloAnimationContainer** | Layout | Programmatically animated show/hide container |
| **MariloMediaQuery** | Utility | React to browser size changes |
| **MariloFloatingLabel** | Utility | Animated label that floats on focus |

---

## 2. Partial Matches (Marilo has it but missing spec features)

### MariloTextField vs MariloTextBox
| Feature | MariloTextBox (spec) | MariloTextField (impl) | Gap |
|---|---|---|---|
| Two-way binding | `@bind-Value` | `Value` + `ValueChanged` | Equivalent |
| Prefix/Suffix adornments | `TextBoxPrefixTemplate` / `TextBoxSuffixTemplate` | `Prefix` / `Suffix` RenderFragment | **Equivalent** |
| Separator | `ShowPrefixSeparator` / `ShowSuffixSeparator` | `ShowPrefixSeparator` / `ShowSuffixSeparator` | **Equivalent** |
| Label | Separate or `MariloFloatingLabel` | Not built-in | **Missing** — Label is a separate MariloLabel |
| Readonly | `ReadOnly` parameter | Not supported | **Missing** |
| MaxLength | Built-in | Not supported | **Missing** |
| AutoComplete (browser) | `Autocomplete` parameter | Not supported | **Missing** |
| DebounceDelay | Built-in | Not supported | **Missing** |

### MariloSearchBox vs MariloTextBox (search mode)
| Feature | MariloTextBox (spec) | MariloSearchBox (impl) | Gap |
|---|---|---|---|
| Composing TextField | Uses MariloTextBox internally | Composes MariloTextField | **Equivalent** |
| Clear button | Built-in | Built-in | **Equivalent** |
| Keyboard hint | N/A | `KbdHint` | **Marilo extra** |
| OnSearch event | Built-in | `OnSearch` callback | **Equivalent** |

### MariloTooltip
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Target selector | `TargetSelector` (CSS selector targets multiple) | Wraps single child | **Missing** — Can't attach to external elements |
| Position | `Position` enum | `Position` enum | **Equivalent** |
| Show/Hide events | `OnShow` / `OnHide` | Not supported | **Missing** |
| Template | `<Template>` with context | Plain `Text` string only | **Missing** — No rich content tooltip |
| Width/Height | Configurable | Not supported | **Missing** |
| Show on | Click/Hover/Focus | Hover only | **Missing** |
| Callout | Built-in callout arrow | Not supported | **Missing** |

### MariloButton
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Click event | `OnClick` | `OnClick` | **Equivalent** |
| Icon | `Icon` (ISvgIcon) | Via `ChildContent` | **Partial** — No dedicated Icon parameter |
| ThemeColor | `ThemeColor` parameter | `Variant` enum | Different approach, equivalent |
| FillMode | `Solid`, `Outline`, `Flat`, `Link`, `Clear` | `IsOutline` bool | **Missing** — Only 2 fill modes vs 5 |
| Rounded | `Rounded` parameter | Not supported | **Missing** |
| Size | `Size` (string constant) | `ButtonSize` enum | **Equivalent** |
| Enabled | `Enabled` | `Disabled` (inverted) | **Equivalent** |

### MariloDialog
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Visible | `Visible` two-way | `IsOpen` | **Equivalent** |
| Title | `Title` | `Title` | **Equivalent** |
| Predefined actions | `DialogButtons` (OK, Cancel, Yes, No, etc.) | Not supported | **Missing** |
| DialogFactory | `DialogFactory.ConfirmAsync()` / `AlertAsync()` / `PromptAsync()` | Separate `MariloConfirmDialog` | **Missing** programmatic API |
| Width/Height | Configurable | Not supported | **Missing** |
| Modal/Modeless | Both supported | Modal only | **Missing** |
| Draggable | Supported | Not supported | **Missing** |

### MariloSelect vs MariloDropDownList
| Feature | MariloDropDownList (spec) | MariloSelect (impl) | Gap |
|---|---|---|---|
| Data binding | `Data` + `TextField` + `ValueField` | Raw `<option>` children | **Missing** — No data-driven binding |
| Filtering | Built-in filter | Not supported | **Missing** |
| Templates | Item, Value, Header, Footer | Not supported | **Missing** |
| Grouping | Built-in | Not supported | **Missing** |
| Virtual scroll | Built-in | Not supported | **Missing** |

### MariloAutoComplete
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Value binding | `@bind-Value` (string) | `Value` + `ValueChanged` (string) | **Equivalent** |
| Data binding | `Data` (IEnumerable<TItem>) + `ValueField` | `Items` (IEnumerable<string> only) | **Missing** — Object binding not supported |
| Keyboard navigation | Arrow keys, Enter, Escape | Arrow keys, Enter, Escape | **Equivalent** |
| Filtering | `Filterable` + `FilterOperator` | Always contains-filter | **Partial** — No FilterOperator control |
| DebounceDelay | `DebounceDelay` (ms) | Not supported | **Missing** |
| MinLength | `MinLength` | Not supported | **Missing** |
| ShowClearButton | `ShowClearButton` | Not supported | **Missing** |
| ReadOnly | `ReadOnly` | Not supported | **Missing** |
| Grouping | Built-in | Not supported | **Missing** |
| Templates | Item, Header, Footer | Not supported | **Missing** |
| Virtualization | Built-in | Not supported | **Missing** |
| AdaptiveMode | Mobile action sheet | Not supported | **Missing** |

### MariloFileUpload vs MariloFileSelect / MariloUpload
| Feature | MariloFileSelect (spec) | MariloFileUpload (impl) | Gap |
|---|---|---|---|
| File selection | `InputFile`-based | Blazor `InputFile` | **Equivalent** |
| Drag-and-drop zone | Built-in | Built-in | **Equivalent** |
| Multiple files | `Multiple` | `Multiple` | **Equivalent** |
| Accept filter | `Accept` | `Accept` | **Equivalent** |
| MaxFileSize validation | Client-side | `MaxFileSize` (not enforced) | **Missing** — No validation feedback |
| File list UI | Shows selected files with remove | Shows comma-joined names only | **Missing** — No individual file remove |
| Async server upload | MariloUpload — `SaveUrl` / `RemoveUrl` | Not supported (client-side only) | **Missing** — Full MariloUpload requires server endpoint |
| Upload progress | Built-in progress per file | Not supported | **Missing** |
| Chunk upload | Supported | Not supported | **Missing** |
| Initial files | `Files` parameter | Not supported | **Missing** |

### MariloDatePicker
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Value binding | `Value` (DateTime?) | `Value` (DateOnly?) | Different type |
| Format | `Format` string | Not supported | **Missing** |
| Calendar popup | Full calendar | Not implemented (stub) | **Missing** — Needs visual calendar |
| Min/Max | `Min` / `Max` | `Min` / `Max` | **Equivalent** |
| Disabled dates | `DisabledDates` | Not supported | **Missing** |
| Adaptive rendering | Mobile-friendly | Not supported | **Missing** |

### MariloGridLayout vs MariloGrid
Marilo's current `MariloGrid` component is a CSS layout grid — matching the spec's `MariloGridLayout`. The spec's `MariloGrid` is a full data grid component. These are **completely different components**. Marilo has no data grid equivalent and the current `MariloGrid` is misnamed relative to the spec.

### MariloTreeView
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Data binding | `Data` collection + fields | Manual `<MariloTreeItem>` children | **Missing** — No data-driven binding |
| Templates | Item, Checkbox | Not supported | **Missing** |
| Drag and Drop | Built-in | Not supported | **Missing** |
| Checkboxes | Built-in | Not supported | **Missing** |
| Load on demand | Built-in | Not supported | **Missing** |
| ExpandedItems | Collection binding | Per-item `IsExpanded` | Different approach |

### MariloTabStrip
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Tab content | `<TabStripTab>` children | `<TabStripTab>` children | **Equivalent** |
| Closeable tabs | Built-in | `Closeable` per tab | **Equivalent** |
| Tab position | Top, Bottom, Left, Right | `TabPosition` enum | **Equivalent** |
| Scrollable tabs | Built-in | `OverflowMode.Scroll` | **Equivalent** |
| Disabled tabs | Built-in | `Disabled` per tab | **Equivalent** |
| Tab reorder | Drag-and-drop | `EnableTabReorder` | **Equivalent** |
| State management | N/A | `OnStateInit` / `OnStateChanged` / `GetState()` / `SetState()` | **Marilo extra** |
| Keyboard navigation | Full ARIA | `role="tablist"` / `role="tab"` | **Partial** — Missing keyboard arrow nav |
| Overflow menu | Built-in | `OverflowMode.Menu` (UI not impl.) | **Missing** — `OverflowMode.Menu` has no menu rendering |

### MariloSlider
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Value | `Value` | `Value` | **Equivalent** |
| Min/Max/Step | Built-in | Built-in | **Equivalent** |
| LargeStep | Built-in tick marks | Not supported | **Missing** |
| Label template | Built-in | Not supported | **Missing** |
| Orientation | Horizontal/Vertical | Not configurable | **Missing** |
| **MariloRangeSlider** | Separate component with `StartValue`/`EndValue` | Not available | **Missing component** |

### MariloCheckBox
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Value | `Value` | `Checked` | **Equivalent** |
| Indeterminate | `Indeterminate` parameter | Not supported | **Missing** |
| Label | Via `<label>` | `Label` parameter | **Equivalent** |

### MariloIcon
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Icon parameter | `Icon` (ISvgIcon) | `Icon` (RenderFragment) | Different approach |
| ChildContent | Custom SVG | Custom SVG | **Equivalent** |
| Size | `Size` string | `Size` enum | **Equivalent** |
| Flip | `Flip` enum | `Flip` enum | **Equivalent** |
| ThemeColor | `ThemeColor` string | `ThemeColor` enum | **Equivalent** |
| Name lookup | N/A | `Name` string | **Marilo extra** |
| AriaLabel | N/A | `AriaLabel` | **Marilo extra** |

### MariloSnackbar vs MariloNotification
| Feature | MariloNotification (spec) | MariloSnackbar (impl) | Gap |
|---|---|---|---|
| Programmatic API | `NotificationRef.Show(model)` | Not supported | **Missing** — No `@ref` Show/Hide API |
| Position | `VerticalPosition` + `HorizontalPosition` | Not configurable | **Missing** |
| Auto-close delay | `CloseAfter` (ms) | Not supported | **Missing** |
| ThemeColor | Per-notification color | Built-in severity variants | **Partial** |
| Animation | `AnimationType` + `AnimationDuration` | Not supported | **Missing** |
| Templates | Custom render template | Not supported | **Missing** |
| Stacked notifications | Multiple instances stack | Single message only | **Missing** |

---

## 3. Components Marilo Has That the Spec Doesn't

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
| MariloTimeline / MariloTimelineItem | Vertical event timeline display |
| MariloImage | Image component with aspect ratio and fit controls |
| MariloTypography | Semantic text rendering with preset size/weight variants |

---

## 4. Priority Recommendations

### P0 — Add immediately (blocks most business apps)
1. **MariloGrid** — Full data grid with paging, sorting, filtering, column templates, editing
2. **MariloComboBox** — Filterable dropdown with data binding, templates, custom values
3. **MariloDropDownList** — Simple data-bound dropdown (replaces MariloSelect)
4. **MariloMultiSelect** — Multi-item selection dropdown

### P1 — Add soon (common requirements)
5. **MariloWindow** — Draggable/resizable popup window
6. **MariloChart** — Line, bar, pie, donut charts
7. **MariloEditor** — WYSIWYG HTML editor
8. **MariloListView** — Templated data-bound repeating layout
9. **MariloUpload** — Server-endpoint upload with progress, queue, validation, chunk upload

### P2 — Enhance existing components
10. MariloAutoComplete — Add object binding (`TItem`/`ValueField`), `FilterOperator`, `DebounceDelay`, `MinLength`, `ShowClearButton`, `ReadOnly`, templates
11. MariloFileUpload — Add per-file list UI, validation feedback, initial files list; rename to `MariloFileSelect`
12. MariloSnackbar — Add programmatic `Show()`/`Hide()` API via `@ref`, position, auto-close delay, stacking; rename to `MariloNotification`
13. MariloButton — Add `FillMode` (Flat, Link, Clear), `Rounded`, dedicated `Icon` parameter
14. MariloTextField — Add `ReadOnly`, `MaxLength`, `DebounceDelay`; rename to `MariloTextBox`
15. MariloTooltip — Add `TargetSelector`, template content, show-on modes, width/height
16. MariloDialog — Add `DialogFactory` service, predefined actions, width/height, draggable
17. MariloSelect — Upgrade to data-bound with filtering, templates, grouping; rename to `MariloDropDownList`
18. MariloDatePicker — Add visual calendar, format string, disabled dates
19. MariloTreeView — Add data binding, checkboxes, drag-and-drop
20. MariloCheckBox — Add `Indeterminate` state
21. MariloTabStrip — Implement `OverflowMode.Menu` rendering, keyboard arrow navigation
22. MariloSlider — Add orientation, large step, label template

### P3 — Future roadmap
23. MariloScheduler / MariloGantt
24. MariloPdfViewer / MariloSpreadsheet
25. MariloAIPrompt / MariloChat
26. MariloMap / MariloDiagram / MariloBarcode / MariloQRCode
27. MariloDockManager / MariloTileLayout / MariloWizard / MariloAnimationContainer
