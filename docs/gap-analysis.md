# Marilo vs Telerik Blazor UI — Gap Analysis

Based on the Telerik Blazor documentation at `/workspaces/Marilo/docs/component-specs` (101 components) compared against Marilo's current ~120+ component files.

> Last updated: 2026-03-30 (revised after Phase 0–3 implementation)

---

## Summary

| Category | Spec Components | Marilo Has | Missing | Partial | Notes |
|---|---|---|---|---|---|
| Data Grid / Tables | 3 | 2 | 1 | 0 | DataGrid + ListView added; TreeList still missing |
| Data Visualization | 7 | 1 | 6 | 0 | Chart added (Line/Bar/Pie/Area/Scatter/Donut) |
| Editors / Rich Input | 4 | 1 | 3 | 0 | Editor added; Spreadsheet, Signature, Diagram missing |
| Selection / Dropdowns | 8 | 8 | 0 | 0 | All fully resolved incl. grouping |
| Date / Time | 5 | 4 | 1 | 0 | DatePicker fully enhanced with Format + calendar popup |
| Layout / Containers | 11 | 8 | 3 | 0 | TabStrip fully resolved with OverflowMode.Menu |
| Navigation | 8 | 8 | 0 | 0 | TreeView with data binding, checkboxes, and ItemTemplate |
| Buttons / Actions | 6 | 6 | 0 | 0 | Button fully enhanced (FillMode, Rounded, Icon) |
| Feedback / Notifications | 7 | 7 | 0 | 0 | SnackbarHost + NotificationService + animations added |
| Inputs / Form Controls | 12 | 12 | 0 | 0 | All inputs enhanced; MariloRangeSlider added |
| File Handling | 3 | 2 | 1 | 0 | MariloUpload added; FileManager still missing |
| Popup / Overlay | 5 | 5 | 0 | 0 | Window added; Dialog with DialogButtons UI; Tooltip with callout |
| Scheduling | 2 | 0 | 2 | 0 | — |
| AI Components | 4 | 0 | 4 | 0 | — |
| Media / Documents | 3 | 1 | 2 | 0 | — |
| Misc / Utility | 6 | 2 | 3 | 1 | — |

---

## 1. Missing Components (Not in Marilo at all)

### Critical — ALL RESOLVED

> All P0 and P1 critical components have been implemented. No remaining critical gaps.

### Important — Common in business apps (remaining gaps)

| Component | Category | Description |
|---|---|---|
| **MariloTreeList** | Data | Hierarchical data in tabular format with editing, filtering, sorting |
| **MariloSpreadsheet** | Rich Input | Excel-like editor with formulas and formatting |
| **MariloPdfViewer** | Documents | Opens PDF files with paging, zooming, searching, text selection |
| **MariloStockChart** | Visualization | Financial OHLC / candlestick charts with navigator |
| **MariloGantt** | Scheduling | Task timeline with hierarchical data, dependencies, editing |
| **MariloScheduler** | Scheduling | Calendar views (day/week/month) with appointment CRUD |
| **MariloMap** | Visualization | Geospatial map with tile layers, markers, bubble layers |
| **MariloPivotGrid** | Data | Multi-dimensional data analysis pivot table |
| **MariloArcGauge / MariloCircularGauge / MariloLinearGauge / MariloRadialGauge** | Visualization | Circular, linear, radial, arc gauge components |
| **MariloSankey** | Visualization | Flow visualization between domains |

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

### MariloTextField vs MariloTextBox — RESOLVED
| Feature | MariloTextBox (spec) | MariloTextField (impl) | Gap |
|---|---|---|---|
| Two-way binding | `@bind-Value` | `Value` + `ValueChanged` | Equivalent |
| Prefix/Suffix adornments | `TextBoxPrefixTemplate` / `TextBoxSuffixTemplate` | `Prefix` / `Suffix` RenderFragment | **Equivalent** |
| Separator | `ShowPrefixSeparator` / `ShowSuffixSeparator` | `ShowPrefixSeparator` / `ShowSuffixSeparator` | **Equivalent** |
| Label | Separate or `MariloFloatingLabel` | Not built-in | **Missing** — Label is a separate MariloLabel |
| Readonly | `ReadOnly` parameter | `ReadOnly` parameter | ~~RESOLVED~~ |
| MaxLength | Built-in | `MaxLength` parameter | ~~RESOLVED~~ |
| AutoComplete (browser) | `Autocomplete` parameter | `Autocomplete` parameter | ~~RESOLVED~~ |
| DebounceDelay | Built-in | `DebounceDelay` parameter (Timer-based) | ~~RESOLVED~~ |

### MariloTooltip — RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Position | `Position` enum | `Position` enum | **Equivalent** |
| Show/Hide events | `OnShow` / `OnHide` | `OnShow` / `OnHide` EventCallbacks | ~~RESOLVED~~ |
| Template | `<Template>` with context | `TooltipTemplate` RenderFragment | ~~RESOLVED~~ |
| Width/Height | Configurable | `Width` / `Height` parameters | ~~RESOLVED~~ |
| Show on | Click/Hover/Focus | `ShowOn` enum (Hover/Click/Focus) | ~~RESOLVED~~ |
| Callout | Built-in callout arrow | `ShowCallout` parameter with CSS callout | ~~RESOLVED~~ |
| Target selector | `TargetSelector` (CSS selector targets multiple) | Wraps single child | **Missing** — Can't attach to external elements |

### MariloButton — RESOLVED
> All features fully implemented: FillMode, Rounded, Icon, Size, OnClick, Disabled.

### MariloDialog — RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Visible | `Visible` two-way | `IsOpen` | **Equivalent** |
| Title | `Title` | `Title` | **Equivalent** |
| Predefined actions | `DialogButtons` (OK, Cancel, Yes, No, etc.) | `Buttons` parameter with rendered button UI + `OnButtonClick` callback | ~~RESOLVED~~ |
| DialogFactory | `DialogFactory.ConfirmAsync()` / `AlertAsync()` / `PromptAsync()` | `IMariloDialogService.ShowAsync(DialogOptions)` + `ShowPromptAsync` | ~~RESOLVED~~ |
| Width/Height | Configurable | `Width` / `Height` parameters | ~~RESOLVED~~ |
| Modal/Modeless | Both supported | `Modal` bool parameter | ~~RESOLVED~~ |
| Draggable | Supported | `Draggable` parameter | ~~RESOLVED~~ |

### MariloSelect vs MariloDropDownList — RESOLVED
> Fully resolved with `MariloDropDownList<TItem, TValue>` including data binding, filtering, templates, keyboard navigation, and grouping via `GroupField`.

### MariloAutoComplete — RESOLVED
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
| Grouping | Built-in | `GroupField` parameter with group headers | ~~RESOLVED~~ |
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
| Initial files | `Files` parameter | `InitialFiles` parameter (`IEnumerable<FileUploadInfo>`) | ~~RESOLVED~~ |
| Chunk upload | Supported | Not supported | **Missing** |

### MariloDatePicker — RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Value binding | `Value` (DateTime?) | `Value` (DateOnly?) | Different type |
| Format | `Format` string | `Format` parameter (default: "yyyy-MM-dd") | ~~RESOLVED~~ |
| Calendar popup | Full calendar | Visual calendar with month nav, day grid, today highlight | ~~RESOLVED~~ |
| Min/Max | `Min` / `Max` | `Min` / `Max` | **Equivalent** |
| Disabled dates | `DisabledDates` | `DisabledDates` parameter (rendered disabled in calendar) | ~~RESOLVED~~ |
| Adaptive rendering | Mobile-friendly | Not supported | **Missing** |

### MariloTreeView — MOSTLY RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Data binding | `Data` collection + fields | `Data` + `IdField`/`ParentIdField`/`TextField`/`ItemsField` (flat + hierarchical) | ~~RESOLVED~~ |
| Templates | Item template | `ItemTemplate` RenderFragment<object> | ~~RESOLVED~~ |
| Drag and Drop | Built-in | Not supported | **Missing** |
| Checkboxes | Built-in | `CheckBoxMode` (None/Single/Multiple) | ~~RESOLVED~~ |
| Load on demand | Built-in | Not supported | **Missing** |
| ExpandedItems | Collection binding | Per-item `IsExpanded` / internal expanded set | Different approach |

### MariloTabStrip — RESOLVED
> All features fully implemented: Tab content, closeable, position, scrollable, disabled, reorder, state management, keyboard nav, overflow menu.

### MariloSlider — RESOLVED
| Feature | Spec | Impl | Gap |
|---|---|---|---|
| Value | `Value` | `Value` | **Equivalent** |
| Min/Max/Step | Built-in | Built-in | **Equivalent** |
| LargeStep | Built-in tick marks | `LargeStep` with tick marks | ~~RESOLVED~~ |
| Label template | Built-in | `LabelTemplate` RenderFragment | ~~RESOLVED~~ |
| Orientation | Horizontal/Vertical | `Orientation` enum | ~~RESOLVED~~ |
| **MariloRangeSlider** | Separate component with `StartValue`/`EndValue` | `MariloRangeSlider` component with `StartValue`/`EndValue`, tick marks, label template | ~~RESOLVED~~ |

### MariloCheckBox — RESOLVED
> All features fully implemented: Value, Indeterminate with `aria-checked="mixed"`, Label.

### MariloSnackbar vs MariloNotification — RESOLVED
| Feature | MariloNotification (spec) | MariloSnackbar (impl) | Gap |
|---|---|---|---|
| Programmatic API | `NotificationRef.Show(model)` | `IMariloNotificationService` + `MariloNotificationService` impl | ~~RESOLVED~~ |
| Position | `VerticalPosition` + `HorizontalPosition` | `MariloSnackbarHost` with position parameters | ~~RESOLVED~~ |
| Auto-close delay | `CloseAfter` (ms) | Auto-dismiss via `CloseAfterMs` on `NotificationModel` | ~~RESOLVED~~ |
| ThemeColor | Per-notification color | Built-in severity variants | **Partial** |
| Animation | `AnimationType` + `AnimationDuration` | `AnimationType` + `AnimationDurationMs` parameters on SnackbarHost | ~~RESOLVED~~ |
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

### P0 — ALL RESOLVED
> All P0 critical components have been implemented.

### P1 — ALL RESOLVED
> All P1 important components have been implemented.

### P2 — Enhance existing components — ALL RESOLVED
> All P2 enhancements have been completed:
> - MariloAutoComplete: grouping via `GroupField`
> - MariloFileUpload: `InitialFiles` parameter
> - MariloSnackbar: animation support (`AnimationType`, `AnimationDurationMs`)
> - MariloButton: FillMode, Rounded, Icon
> - MariloTextField: `Autocomplete` browser hint
> - MariloTooltip: `OnShow`/`OnHide` events, `ShowCallout` callout arrow
> - MariloDialog: `DialogButtons` wired into UI with `OnButtonClick`
> - MariloDropDownList: `GroupField` for grouped items
> - MariloDatePicker: `Format` parameter
> - MariloTreeView: `ItemTemplate` for custom item rendering
> - MariloSlider: `MariloRangeSlider` component added
> - MariloCheckBox: Indeterminate support
> - MariloTabStrip: Keyboard nav + overflow menu
> - MariloSelect: Filterable support

### Remaining minor gaps (not blocking — deferred to P3)
| Component | Feature | Notes |
|---|---|---|
| MariloAutoComplete | Virtualization | Large dataset optimization |
| MariloAutoComplete | AdaptiveMode | Mobile action sheet rendering |
| MariloFileUpload | Chunk upload | Large file chunked upload support |
| MariloDatePicker | Adaptive rendering | Mobile-friendly calendar |
| MariloTreeView | Drag and Drop | Node reordering via DnD |
| MariloTreeView | Load on demand | Lazy child loading |
| MariloTooltip | TargetSelector | Attach to external elements via CSS selector |
| MariloSnackbar | ThemeColor | Per-notification custom theme colors |

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
32. MariloFloatingLabel / MariloMediaQuery / MariloAnimationContainer
