# Marilo vs Telerik Blazor UI — Gap Analysis

Based on the Telerik Blazor documentation at `/workspaces/Marilo/docs/component-specs` (101 components) compared against Marilo's current ~120+ component files.

> Last updated: 2026-03-30 (revised after Phase 0–3 implementation — all P0/P1/P2 gaps resolved)

---

## Summary

| Category | Spec Components | Marilo Has | Missing | Partial | Notes |
|---|---|---|---|---|---|
| Data Grid / Tables | 3 | 2 | 1 | 0 | DataGrid + ListView added; TreeList still missing |
| Data Visualization | 7 | 1 | 6 | 0 | Chart added (Line/Bar/Pie/Area/Scatter/Donut) |
| Editors / Rich Input | 4 | 1 | 3 | 0 | Editor added; Spreadsheet, Signature, Diagram missing |
| Selection / Dropdowns | 8 | 8 | 0 | 0 | All fully resolved incl. grouping + virtualization |
| Date / Time | 5 | 5 | 0 | 0 | DatePicker fully enhanced with Format, calendar, adaptive |
| Layout / Containers | 11 | 8 | 3 | 0 | TabStrip fully resolved with OverflowMode.Menu |
| Navigation | 8 | 8 | 0 | 0 | TreeView with data binding, checkboxes, ItemTemplate, DnD, load-on-demand |
| Buttons / Actions | 6 | 6 | 0 | 0 | Button fully enhanced (FillMode, Rounded, Icon) |
| Feedback / Notifications | 7 | 7 | 0 | 0 | SnackbarHost + NotificationService + animations + ThemeColor |
| Inputs / Form Controls | 12 | 12 | 0 | 0 | All inputs enhanced; MariloRangeSlider added |
| File Handling | 3 | 2 | 1 | 0 | MariloUpload with chunk upload; FileManager still missing |
| Popup / Overlay | 5 | 5 | 0 | 0 | Dialog with DialogButtons UI; Tooltip with callout + TargetSelector |
| Scheduling | 2 | 0 | 2 | 0 | — |
| AI Components | 4 | 0 | 4 | 0 | — |
| Media / Documents | 3 | 1 | 2 | 0 | — |
| Misc / Utility | 6 | 2 | 3 | 1 | — |

---

## 1. Missing Components (Not in Marilo at all)

### Critical / Important — ALL RESOLVED

> All P0 and P1 critical/important components have been implemented. No remaining gaps.

### Remaining missing — Specialized/future roadmap components

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
| **MariloArcGauge / MariloCircularGauge / MariloLinearGauge / MariloRadialGauge** | Visualization | Gauge components |
| **MariloSankey** | Visualization | Flow visualization between domains |
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
| **MariloForm** (auto-gen) | Forms | Auto-generates form fields from model |

---

## 2. Existing Component Feature Status — ALL RESOLVED

All partial matches from the spec have been fully resolved. Below is a summary of the final feature parity:

### MariloTextField — RESOLVED
- Two-way binding, Prefix/Suffix, Separator, ReadOnly, MaxLength, DebounceDelay, **Autocomplete** browser hint

### MariloTooltip — RESOLVED
- Position, ShowOn (Hover/Click/Focus), Template, Width/Height, **OnShow/OnHide** events, **ShowCallout** arrow, **TargetSelector** (CSS selector via JS interop for external elements)

### MariloButton — RESOLVED
- OnClick, Icon, FillMode (Solid/Outline/Flat/Link/Clear), Rounded, Size, Disabled

### MariloDialog — RESOLVED
- IsOpen, Title, Width/Height, Draggable, Modal, **DialogButtons** (Ok/OkCancel/YesNo/YesNoCancel/RetryCancel) rendered with **OnButtonClick** callback, `IMariloDialogService` with ShowAsync/ShowConfirmAsync/ShowAlertAsync/ShowPromptAsync

### MariloDropDownList — RESOLVED
- Data binding, Filtering, ItemTemplate/ValueTemplate, Keyboard nav, **GroupField** for grouped rendering

### MariloAutoComplete — RESOLVED
- Data binding (generic TItem), FilterOperator, DebounceDelay, MinLength, ShowClearButton, ReadOnly, ItemTemplate/HeaderTemplate/FooterTemplate, **GroupField**, **EnableVirtualization** (Blazor `<Virtualize>`), **AdaptiveMode** (action sheet on mobile)

### MariloFileUpload / MariloUpload — RESOLVED
- File selection, drag-and-drop, Multiple, Accept, MaxFileSize, file list UI, **InitialFiles** parameter, async server upload (SaveUrl/RemoveUrl), progress tracking, **ChunkSize** for chunked uploads with X-Chunk-Index/X-Total-Chunks headers

### MariloDatePicker — RESOLVED
- Value binding (DateOnly?), **Format** parameter, calendar popup with month nav, Min/Max, DisabledDates, **AdaptiveMode** (full-screen calendar on mobile)

### MariloTreeView — RESOLVED
- Data binding (flat + hierarchical), IdField/ParentIdField/TextField/ItemsField, CheckBoxMode, **ItemTemplate**, **OnExpand** load-on-demand callback with loading indicator, **EnableDragDrop** with **OnDrop** callback (`TreeDragDropEventArgs`)

### MariloTabStrip — RESOLVED
- Tab content, closeable, position, scrollable, disabled, reorder, state management, keyboard nav, overflow menu

### MariloSlider — RESOLVED
- Value, Min/Max/Step, LargeStep with ticks, LabelTemplate, Orientation, **MariloRangeSlider** component (StartValue/EndValue)

### MariloCheckBox — RESOLVED
- Value, Indeterminate with `aria-checked="mixed"`, Label

### MariloSnackbar / MariloNotification — RESOLVED
- `IMariloNotificationService` with programmatic API, position (Vertical/Horizontal), auto-close delay, ContentTemplate, stacked notifications, **AnimationType** (None/Fade/SlideIn) + **AnimationDurationMs**, **ThemeColor** per-notification custom color

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

## 4. Priority Recommendations — Final Status

### P0 — ALL RESOLVED
### P1 — ALL RESOLVED
### P2 — ALL RESOLVED

All feature-level gaps in existing components have been addressed.

### P3 — Future roadmap (entirely new components)
1. MariloScheduler / MariloGantt
2. MariloPdfViewer / MariloSpreadsheet
3. MariloAIPrompt / MariloChat
4. MariloMap / MariloDiagram / MariloBarcode / MariloQRCode
5. MariloDockManager / MariloTileLayout / MariloWizard / MariloAnimationContainer
6. MariloTreeList / MariloPivotGrid
7. MariloStockChart / Gauges / MariloSankey
8. MariloFileManager / MariloSignature
9. MariloForm (auto-generation from model)
10. MariloFloatingLabel / MariloMediaQuery
