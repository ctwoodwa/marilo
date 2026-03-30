# Marilo vs Telerik Blazor UI — Gap Analysis

Based on the Telerik Blazor documentation at `/workspaces/Marilo/docs/component-specs` (101 components) compared against Marilo's current ~120+ component files.

> Last updated: 2026-03-30 (revised after Phase 0–2 implementation)

---

## Summary

| Category | Spec Components | Marilo Has | Missing | Partial | Notes |
|---|---|---|---|---|---|
| Data Grid / Tables | 3 | 2 | 1 | 0 | DataGrid + ListView added; TreeList still missing |
| Data Visualization | 7 | 1 | 6 | 0 | Chart added (Line/Bar/Pie/Area/Scatter/Donut) |
| Editors / Rich Input | 4 | 1 | 3 | 0 | Editor added; Spreadsheet, Signature, Diagram missing |
| Selection / Dropdowns | 8 | 8 | 0 | 0 | DropDownList, ComboBox, MultiSelect added; AutoComplete fully enhanced |
| Date / Time | 5 | 4 | 1 | 0 | DatePicker fully enhanced with calendar popup |
| Layout / Containers | 11 | 8 | 3 | 0 | TabStrip fully resolved with OverflowMode.Menu |
| Navigation | 8 | 8 | 0 | 0 | TreeView data binding + checkboxes; TabStrip keyboard nav |
| Buttons / Actions | 6 | 6 | 0 | 0 | Button fully enhanced (FillMode, Rounded, Icon) |
| Feedback / Notifications | 7 | 7 | 0 | 0 | SnackbarHost + NotificationService added; Dialog enhanced |
| Inputs / Form Controls | 12 | 12 | 0 | 0 | All inputs enhanced (TextField, Checkbox, Slider, Select, DatePicker) |
| File Handling | 3 | 2 | 1 | 0 | MariloUpload added; FileManager still missing |
| Popup / Overlay | 5 | 5 | 0 | 0 | Window added; Dialog + Tooltip fully enhanced |
| Scheduling | 2 | 0 | 2 | 0 | — |
| AI Components | 4 | 0 | 4 | 0 | — |
| Media / Documents | 3 | 1 | 2 | 0 | — |
| Misc / Utility | 6 | 2 | 3 | 1 | — |

---

## 1. Missing Components (Not in Marilo at all)

### Critical — ~~High-value components most apps need~~ RESOLVED

> All P0 critical components have been implemented:
> - ~~MariloGrid~~ — **RESOLVED**: `MariloDataGrid` added with paging, sorting, filtering, selection, column templates
> - ~~MariloComboBox~~ — **RESOLVED**: Added with AllowCustom, filtering, keyboard nav, templates
> - ~~MariloDropDownList~~ — **RESOLVED**: Added with data binding, filtering, grouping, templates
> - ~~MariloMultiSelect~~ — **RESOLVED**: Added with tag display, TagMode, multi-selection
> - ~~MariloDialog (full)~~ — **RESOLVED**: Enhanced with Width/Height, Draggable, Modal toggle; `IMariloDialogService` extended with `ShowPromptAsync`/`ShowAsync`
> - ~~MariloWindow~~ — **RESOLVED**: Added with title bar actions, modal/modeless, draggable support
> - **MariloForm** (full) | Forms | Auto-generates form fields from model, validation integration, form groups, columns — *Still missing auto-generation from model*

### Important — Common in business apps (remaining gaps)

| Component | Category | Description |
|---|---|---|
| **MariloTreeList** | Data | Hierarchical data in tabular format with editing, filtering, sorting |
| ~~MariloListView~~ | ~~Data~~ | ~~RESOLVED: Added with ItemTemplate, paging, selection, CRUD callbacks~~ |
| ~~MariloEditor~~ | ~~Rich Input~~ | ~~RESOLVED: Added with toolbar, preview, tool configuration~~ |
| **MariloSpreadsheet** | Rich Input | Excel-like editor with formulas and formatting |
| **MariloPdfViewer** | Documents | Opens PDF files with paging, zooming, searching, text selection |
| ~~MariloChart~~ | ~~Visualization~~ | ~~RESOLVED: Added with Line, Bar, Column, Area, Pie, Donut, Scatter; SVG-based~~ |
| **MariloStockChart** | Visualization | Financial OHLC / candlestick charts with navigator |
| **MariloGantt** | Scheduling | Task timeline with hierarchical data, dependencies, editing |
| **MariloScheduler** | Scheduling | Calendar views (day/week/month) with appointment CRUD |
| **MariloMap** | Visualization | Geospatial map with tile layers, markers, bubble layers |
| **MariloPivotGrid** | Data | Multi-dimensional data analysis pivot table |
| **MariloArcGauge / MariloCircularGauge / MariloLinearGauge / MariloRadialGauge** | Visualization | Circular, linear, radial, arc gauge components |
| **MariloSankey** | Visualization | Flow visualization between domains |
| ~~MariloUpload~~ | ~~Files~~ | ~~RESOLVED: Added with SaveUrl/RemoveUrl, progress tracking, drag-drop, file validation~~ |

### Nice to Have — Specialized components (unchanged)

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

### MariloTextField vs MariloTextBox — MOSTLY RESOLVED
| Feature | MariloTextBox (spec) | MariloTextField (impl) | Gap |
|---|---|---|---|
| Two-way binding | `@bind-Value` | `Value` + `ValueChanged` | Equivalent |
| Prefix/Suffix adornments | `TextBoxPrefixTemplate` / `TextBoxSuffixTemplate` | `Prefix` / `Suffix` RenderFragment | **Equivalent** |
| Separator | `ShowPrefixSeparator` / `ShowSuffixSeparator` | `ShowPrefixSeparator` / `ShowSuffixSeparator` | **Equivalent** |
| Label | Separate or `MariloFloatingLabel` | Not built-in | **Missing** — Label is a separate MariloLabel |
| Readonly | `ReadOnly` parameter | `ReadOnly` parameter | ~~RESOLVED~~ |
| MaxLength | Built-in | `MaxLength` parameter | ~~RESOLVED~~ |
| AutoComplete (browser) | `Autocomplete` parameter | Not supported | **Missing** |
| DebounceDelay | Built-in | `DebounceDelay` parameter (Timer-based) | ~~RESOLVED~~ |

### MariloSearchBox vs MariloTextBox (search mode)
| Feature | MariloTextBox (spec) | MariloSearchBox (impl) | Gap |
|---|---|---|---|
| Composing TextField | Uses MariloTextBox internally | Composes MariloTextField | **Equivalent** |
| Clear button | Built-in | Built-in | **Equivalent** |
| Keyboard hint | N/A | `KbdHint` | **Marilo extra** |
| OnSearch event | Built-in | `OnSearch` callback | **Equivalent** |

### MariloTooltip — MOSTLY RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Target selector | `TargetSelector` (CSS selector targets multiple) | Wraps single child | **Missing** — Can't attach to external elements |
| Position | `Position` enum | `Position` enum | **Equivalent** |
| Show/Hide events | `OnShow` / `OnHide` | Not supported | **Missing** |
| Template | `<Template>` with context | `TooltipTemplate` RenderFragment | ~~RESOLVED~~ |
| Width/Height | Configurable | `Width` / `Height` parameters | ~~RESOLVED~~ |
| Show on | Click/Hover/Focus | `ShowOn` enum (Hover/Click/Focus) | ~~RESOLVED~~ |
| Callout | Built-in callout arrow | Not supported | **Missing** |

### MariloButton — RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Click event | `OnClick` | `OnClick` | **Equivalent** |
| Icon | `Icon` (ISvgIcon) | `Icon` RenderFragment | ~~RESOLVED~~ |
| ThemeColor | `ThemeColor` parameter | `Variant` enum | Different approach, equivalent |
| FillMode | `Solid`, `Outline`, `Flat`, `Link`, `Clear` | `FillMode` enum (all 5 modes) | ~~RESOLVED~~ |
| Rounded | `Rounded` parameter | `Rounded` (`RoundedMode` enum) | ~~RESOLVED~~ |
| Size | `Size` (string constant) | `ButtonSize` enum | **Equivalent** |
| Enabled | `Enabled` | `Disabled` (inverted) | **Equivalent** |

### MariloDialog — MOSTLY RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Visible | `Visible` two-way | `IsOpen` | **Equivalent** |
| Title | `Title` | `Title` | **Equivalent** |
| Predefined actions | `DialogButtons` (OK, Cancel, Yes, No, etc.) | `DialogButtons` enum available in `DialogOptions` model | **Partial** — model exists, not yet wired into component UI |
| DialogFactory | `DialogFactory.ConfirmAsync()` / `AlertAsync()` / `PromptAsync()` | `IMariloDialogService.ShowAsync(DialogOptions)` + `ShowPromptAsync` | ~~RESOLVED~~ (interface extended) |
| Width/Height | Configurable | `Width` / `Height` parameters | ~~RESOLVED~~ |
| Modal/Modeless | Both supported | `Modal` bool parameter | ~~RESOLVED~~ |
| Draggable | Supported | `Draggable` parameter | ~~RESOLVED~~ |

### MariloSelect vs MariloDropDownList — RESOLVED (new component)
> The gap between MariloSelect and the spec's MariloDropDownList has been **resolved** by creating a new `MariloDropDownList<TItem, TValue>` component with full data binding, filtering, templates, and keyboard navigation. MariloSelect has also been enhanced with `Filterable` support.
>
> | Feature | Status |
> |---|---|
> | Data binding (`Data`/`TextField`/`ValueField`) | ~~RESOLVED~~ — MariloDropDownList |
> | Filtering | ~~RESOLVED~~ — MariloDropDownList + MariloSelect |
> | Templates (Item, Value, Header, Footer) | ~~RESOLVED~~ — MariloDropDownList |
> | Grouping | **Missing** — Not yet implemented in MariloDropDownList |
> | Virtual scroll | **Missing** — Not yet implemented |

### MariloAutoComplete — MOSTLY RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Value binding | `@bind-Value` (string) | `Value` + `ValueChanged` (string) | **Equivalent** |
| Data binding | `Data` (IEnumerable<TItem>) + `ValueField` | `Data` + `TextField` + `ValueField` (generic TItem) | ~~RESOLVED~~ |
| Keyboard navigation | Arrow keys, Enter, Escape | Arrow keys, Enter, Escape | **Equivalent** |
| Filtering | `Filterable` + `FilterOperator` | `FilterOperator` parameter (Contains/StartsWith/etc.) | ~~RESOLVED~~ |
| DebounceDelay | `DebounceDelay` (ms) | `DebounceDelay` parameter (Timer-based) | ~~RESOLVED~~ |
| MinLength | `MinLength` | `MinLength` parameter | ~~RESOLVED~~ |
| ShowClearButton | `ShowClearButton` | `ShowClearButton` parameter | ~~RESOLVED~~ |
| ReadOnly | `ReadOnly` | `ReadOnly` parameter | ~~RESOLVED~~ |
| Grouping | Built-in | Not supported | **Missing** |
| Templates | Item, Header, Footer | `ItemTemplate`, `HeaderTemplate`, `FooterTemplate` | ~~RESOLVED~~ |
| Virtualization | Built-in | Not supported | **Missing** |
| AdaptiveMode | Mobile action sheet | Not supported | **Missing** |

### MariloFileUpload vs MariloFileSelect / MariloUpload — MOSTLY RESOLVED
| Feature | MariloFileSelect (spec) | MariloFileUpload (impl) | Gap |
|---|---|---|---|
| File selection | `InputFile`-based | Blazor `InputFile` | **Equivalent** |
| Drag-and-drop zone | Built-in | Built-in | **Equivalent** |
| Multiple files | `Multiple` | `Multiple` | **Equivalent** |
| Accept filter | `Accept` | `Accept` | **Equivalent** |
| MaxFileSize validation | Client-side | `MaxFileSize` with error messages | ~~RESOLVED~~ |
| File list UI | Shows selected files with remove | Per-file list with remove buttons | ~~RESOLVED~~ |
| Async server upload | MariloUpload — `SaveUrl` / `RemoveUrl` | New `MariloUpload` component added | ~~RESOLVED~~ |
| Upload progress | Built-in progress per file | Progress tracking in MariloUpload | ~~RESOLVED~~ |
| Chunk upload | Supported | Not supported | **Missing** |
| Initial files | `Files` parameter | Not supported | **Missing** |

### MariloDatePicker — MOSTLY RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Value binding | `Value` (DateTime?) | `Value` (DateOnly?) | Different type |
| Format | `Format` string | `Format` parameter | ~~RESOLVED~~ |
| Calendar popup | Full calendar | Visual calendar with month nav, day grid, today highlight | ~~RESOLVED~~ |
| Min/Max | `Min` / `Max` | `Min` / `Max` | **Equivalent** |
| Disabled dates | `DisabledDates` | `DisabledDates` parameter (rendered disabled in calendar) | ~~RESOLVED~~ |
| Adaptive rendering | Mobile-friendly | Not supported | **Missing** |

### MariloGridLayout vs MariloGrid
Marilo's current `MariloGrid` component is a CSS layout grid — matching the spec's `MariloGridLayout`. The spec's `MariloGrid` is a full data grid component. These are **completely different components**. Marilo has no data grid equivalent and the current `MariloGrid` is misnamed relative to the spec.

### MariloTreeView — MOSTLY RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Data binding | `Data` collection + fields | `Data` + `IdField`/`ParentIdField`/`TextField`/`ItemsField` (flat + hierarchical) | ~~RESOLVED~~ |
| Templates | Item, Checkbox | Not supported | **Missing** |
| Drag and Drop | Built-in | Not supported | **Missing** |
| Checkboxes | Built-in | `CheckBoxMode` (None/Single/Multiple) | ~~RESOLVED~~ |
| Load on demand | Built-in | Not supported | **Missing** |
| ExpandedItems | Collection binding | Per-item `IsExpanded` / internal expanded set | Different approach |

### MariloTabStrip — RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Tab content | `<TabStripTab>` children | `<TabStripTab>` children | **Equivalent** |
| Closeable tabs | Built-in | `Closeable` per tab | **Equivalent** |
| Tab position | Top, Bottom, Left, Right | `TabPosition` enum | **Equivalent** |
| Scrollable tabs | Built-in | `OverflowMode.Scroll` | **Equivalent** |
| Disabled tabs | Built-in | `Disabled` per tab | **Equivalent** |
| Tab reorder | Drag-and-drop | `EnableTabReorder` | **Equivalent** |
| State management | N/A | `OnStateInit` / `OnStateChanged` / `GetState()` / `SetState()` | **Marilo extra** |
| Keyboard navigation | Full ARIA | Arrow key navigation with wrap-around | ~~RESOLVED~~ |
| Overflow menu | Built-in | `OverflowMode.Menu` with `MaxVisibleTabs` and dropdown | ~~RESOLVED~~ |

### MariloSlider — MOSTLY RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Value | `Value` | `Value` | **Equivalent** |
| Min/Max/Step | Built-in | Built-in | **Equivalent** |
| LargeStep | Built-in tick marks | `LargeStep` with tick marks | ~~RESOLVED~~ |
| Label template | Built-in | `LabelTemplate` RenderFragment | ~~RESOLVED~~ |
| Orientation | Horizontal/Vertical | `Orientation` enum | ~~RESOLVED~~ |
| **MariloRangeSlider** | Separate component with `StartValue`/`EndValue` | Not available | **Missing component** |

### MariloCheckBox — RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Value | `Value` | `Checked` | **Equivalent** |
| Indeterminate | `Indeterminate` parameter | `Indeterminate` with `aria-checked="mixed"` | ~~RESOLVED~~ |
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

### MariloSnackbar vs MariloNotification — MOSTLY RESOLVED
| Feature | MariloNotification (spec) | MariloSnackbar (impl) | Gap |
|---|---|---|---|
| Programmatic API | `NotificationRef.Show(model)` | `IMariloNotificationService` + `MariloNotificationService` impl | ~~RESOLVED~~ |
| Position | `VerticalPosition` + `HorizontalPosition` | `MariloSnackbarHost` with position parameters | ~~RESOLVED~~ |
| Auto-close delay | `CloseAfter` (ms) | Auto-dismiss via `CloseAfterMs` on `NotificationModel` | ~~RESOLVED~~ |
| ThemeColor | Per-notification color | Built-in severity variants | **Partial** |
| Animation | `AnimationType` + `AnimationDuration` | Not supported | **Missing** |
| Templates | Custom render template | `ContentTemplate` RenderFragment on Snackbar | ~~RESOLVED~~ |
| Stacked notifications | Multiple instances stack | `MariloSnackbarHost` renders stacked notifications | ~~RESOLVED~~ |

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

## 4. Priority Recommendations (Updated)

### P0 — ~~Add immediately~~ ALL RESOLVED
1. ~~**MariloGrid**~~ — **DONE**: `MariloDataGrid<TItem>` with paging, sorting, filtering, column templates, selection
2. ~~**MariloComboBox**~~ — **DONE**: `MariloComboBox<TItem, TValue>` with AllowCustom, filtering, templates
3. ~~**MariloDropDownList**~~ — **DONE**: `MariloDropDownList<TItem, TValue>` with data binding, filtering
4. ~~**MariloMultiSelect**~~ — **DONE**: `MariloMultiSelect<TItem, TValue>` with tag display, multi-selection

### P1 — ~~Add soon~~ ALL RESOLVED
5. ~~**MariloWindow**~~ — **DONE**: Modal/modeless, draggable, title bar actions
6. ~~**MariloChart**~~ — **DONE**: SVG-based (Line, Bar, Column, Area, Pie, Donut, Scatter)
7. ~~**MariloEditor**~~ — **DONE**: Toolbar, preview, tool configuration
8. ~~**MariloListView**~~ — **DONE**: ItemTemplate, paging, selection, CRUD callbacks
9. ~~**MariloUpload**~~ — **DONE**: SaveUrl/RemoveUrl, progress tracking, drag-drop, file validation

### P2 — Enhance existing components — ALL RESOLVED
| # | Component | Status | Minor remaining gaps |
|---|-----------|--------|---------------------|
| 10 | MariloAutoComplete | **Done** | Generic TItem, FilterOperator, DebounceDelay, MinLength, ShowClearButton, templates all added. Missing: grouping, virtualization |
| 11 | MariloFileUpload | **Done** | File list UI, MaxFileSize validation added. Missing: chunk upload, initial files |
| 12 | MariloSnackbar | **Done** | `MariloSnackbarHost` + `MariloNotificationService` added. Missing: animation |
| 13 | MariloButton | **Done** | FillMode, Rounded, Icon all added |
| 14 | MariloTextField | **Done** | ReadOnly, MaxLength, DebounceDelay all added. Missing: Autocomplete browser hint |
| 15 | MariloTooltip | **Done** | Template, ShowOn, Width/Height added. Missing: TargetSelector, callout arrow |
| 16 | MariloDialog | **Done** | Width/Height, Draggable, Modal added; `IMariloDialogService` extended. Missing: predefined buttons UI |
| 17 | MariloSelect | **Done** | Filterable added. New `MariloDropDownList` replaces data-bound need |
| 18 | MariloDatePicker | **Done** | Visual calendar popup, Format, DisabledDates all added. Missing: adaptive rendering |
| 19 | MariloTreeView | **Done** | Data binding (flat + hierarchical), CheckBoxMode added. Missing: drag-and-drop, item templates |
| 20 | MariloCheckBox | **Done** | Indeterminate with aria-checked="mixed" added |
| 21 | MariloTabStrip | **Done** | Keyboard nav + OverflowMode.Menu with MaxVisibleTabs added |
| 22 | MariloSlider | **Done** | Orientation, LargeStep, LabelTemplate all added. Missing: MariloRangeSlider |

### P3 — Future roadmap (unchanged)
23. MariloScheduler / MariloGantt
24. MariloPdfViewer / MariloSpreadsheet
25. MariloAIPrompt / MariloChat
26. MariloMap / MariloDiagram / MariloBarcode / MariloQRCode
27. MariloDockManager / MariloTileLayout / MariloWizard / MariloAnimationContainer
28. MariloTreeList / MariloPivotGrid
29. MariloStockChart / Gauges / MariloSankey
30. MariloFileManager / MariloSignature
31. MariloForm (auto-generation from model)
32. MariloRangeSlider
33. MariloFloatingLabel / MariloMediaQuery / MariloAnimationContainer
