# Gap Analysis Resolution Plan

This document defines the resolution strategy for all 87 Marilo Blazor components evaluated in [GAP_ANALYSIS_INDEX.md](GAP_ANALYSIS_INDEX.md). The plan is organized into prioritized phases that respect open-source constraints, dependency ordering, and implementation complexity.

---

## 1. Resolution Priority Order

### Priority Criteria

| Factor | Weight | Description |
|--------|--------|-------------|
| Severity of gaps | High | Blocking gaps (High-severity) that prevent basic use take precedence |
| Number of dependents | High | Components that unblock other components are resolved first |
| Complexity | Medium | Multi-pass components need more planning; simple components batch efficiently |
| External research | Medium | Components needing OSS library evaluation start early to avoid blocking |

### Priority Tiers

| Component | Gap Count | Complexity | Needs External Research | Priority Tier | Status |
|-----------|-----------|------------|------------------------|---------------|--------|
| MariloThemeProvider | 8 | multi-pass | Yes (CSS variables, dark mode) | **T1 - Critical** | ✅ IMPLEMENTED |
| MariloForm | 33 | multi-pass | Yes (model binding, auto-generation) | **T1 - Critical** | ✅ IMPLEMENTED (22/28 gaps, 6 deferred) |
| MariloValidation | 15 | multi-pass | Yes (EditContext, 3 sub-components) | **T1 - Critical** | ✅ IMPLEMENTED (12/12 gaps + 3 new components) |
| MariloField | 7 | multi-pass | Yes (floating animation, focus) | **T1 - Critical** | ✅ IMPLEMENTED (4/7 gaps, 3 deferred) |
| MariloLabel | 5 | multi-pass | Yes (floating behavior) | **T1 - Critical** | ✅ IMPLEMENTED (4/5 gaps, 1 deferred) |
| MariloIcon | 3 | single-pass | No | **T1 - Critical** | ✅ RESOLVED (doc-only) |
| MariloGrid (Layout) | 7 | multi-pass | Yes (child component structure) | **T1 - Critical** | ✅ IMPLEMENTED |
| MariloStack | 5 | single-pass | No | **T1 - Critical** | ✅ IMPLEMENTED |
| MariloContainer | 0 | single-pass | No | **T1 - Critical** | ✅ COMPLETE (0 gaps) |
| MariloRow | 0 | single-pass | No | **T1 - Critical** | ✅ COMPLETE (0 gaps) |
| MariloColumn | 0 | single-pass | No | **T1 - Critical** | ✅ COMPLETE (0 gaps) |
| MariloDivider | 0 | single-pass | No | **T1 - Critical** | ✅ COMPLETE (0 gaps) |
| MariloDataGrid | 44 | multi-pass | Yes (virtual scroll, grouping, CRUD, export) | **T2 - High** | ✅ IMPLEMENTED (Pass 3: grouping, auto-columns, resize/reorder, keyboard nav, search, CSV export, templates, GridState enrichment) |
| MariloGridColumn | 8 | single-pass | No | **T2 - High** | ✅ IMPLEMENTED (all 4 gaps resolved Pass 1 + footer rendering) |
| MariloGridToolbar | 2 | single-pass | No | **T2 - High** | ✅ IMPLEMENTED (ARIA + GridCommandButton) |
| MariloEditor | 54 | multi-pass | Yes (ProseMirror / rich-text engine) | **T2 - High** | ✅ IMPLEMENTED (WYSIWYG contenteditable, execCommand, paste cleanup, XSS fix; 16 deferred) |
| MariloChart | 27 | multi-pass | Yes (charting engine decision) | **T2 - High** | ✅ IMPLEMENTED (tooltips, events, legend, axis config, a11y, bar fix; advanced types deferred) |
| MariloChartSeries | 17 | multi-pass | Yes (scatter/bubble data models) | **T2 - High** | ✅ IMPLEMENTED (XField/YField, primitive data, Visible, ScatterLine/Bubble types) |
| MariloWindow | 32 | multi-pass | Yes (JS interop for drag/resize) | **T2 - High** | ✅ IMPLEMENTED (drag, resize, child components, state, keyboard, a11y; 4 deferred) |
| MariloDialog | 9 | single-pass | No | **T2 - High** | ✅ IMPLEMENTED |
| MariloConfirmDialog | 8 | single-pass | No | **T2 - High** | ✅ IMPLEMENTED |
| MariloPopover | 10 | multi-pass | Yes (anchor positioning) | **T2 - High** | ✅ IMPLEMENTED (animation, keyboard, a11y) |
| MariloDrawer | 10 | multi-pass | Yes (Mode, MiniMode, data binding) | **T2 - High** | ✅ IMPLEMENTED (all 10 gaps resolved) |
| MariloList | 13 | multi-pass | Yes (data binding, selection, drag-drop) | **T2 - High** | ✅ IMPLEMENTED (toolbar, keyboard nav, size, reorder) |
| MariloTreeView | 6+16 | multi-pass | Yes (expanded/selected binding, drag-drop) | **T2 - High** | ✅ VALIDATED (21/22 gaps resolved; 1 deferred: virtualization. Stage 06 closure report complete. Follow-up: expand bUnit tests + demo page) |
| MariloMenu | 7 | multi-pass | Yes (hierarchy rendering) | **T2 - High** | ✅ IMPLEMENTED (5/7 gaps; partial file refactor, ItemTemplate, ShowOn, keyboard nav; collision/generic deferred) |
| MariloContextMenu | 8 | multi-pass | Yes (selector pattern, data binding) | **T2 - High** | ✅ IMPLEMENTED (6/8 gaps; partial file refactor, full keyboard nav, OnShow/OnHide events; Selector JS interop/collision deferred) |
| MariloAccordion | 9 | multi-pass | Yes (data binding, hierarchy) | **T2 - High** | ✅ IMPLEMENTED (all 9 gaps resolved: data binding, expandmode, templates) |
| MariloSplitter | 8 | multi-pass | Yes (interactive resize, state) | **T2 - High** | ✅ IMPLEMENTED (drag resize, keyboard resize, state, aria, collapse) |
| MariloPanel | 7 | multi-pass | Yes (data binding, expand/collapse) | **T2 - High** | ✅ IMPLEMENTED (single-panel design; templates, expand/collapse) |
| MariloStepper | 6 | multi-pass | Yes (orientation, linear flow) | **T2 - High** | ✅ IMPLEMENTED (4/7 gaps; orientation, linear, clickable nav) |
| MariloPagination | 6 | single-pass | No | **T2 - High** | ✅ IMPLEMENTED |
| MariloButton | 8 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloButtonGroup | 6 | multi-pass | Yes (child component architecture) | **T3 - Medium** | ✅ RESOLVED |
| MariloChip | 12 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloChipSet | 9 | multi-pass | Yes (generic data binding) | **T3 - Medium** | ✅ RESOLVED |
| MariloSplitButton | 9 | multi-pass | Yes (dropdown behavior) | **T3 - Medium** | ✅ RESOLVED |
| MariloFab | 9 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloToggleButton | 6 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloIconButton | 3 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloSegmentedControl | 3 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloAvatar | 7 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloBadge | 8 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloCard | 4 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloCardActions | 1 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloCardHeader | 5 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloCarousel | 10 | multi-pass | Yes (generic TItem, data-driven) | **T3 - Medium** | ✅ RESOLVED |
| MariloListItem | 2 | single-pass | No | **T3 - Medium** | ✅ RESOLVED (in T2) |
| MariloListView | 6 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloTooltip | 7 | multi-pass | Yes (selector-based targeting) | **T3 - Medium** | ✅ RESOLVED |
| MariloAlert | 5 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloAlertStrip | 3 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloCallout | 3 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloProgressBar | 4 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloSkeleton | 3 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloToast | 8 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloAutocomplete | 6 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloCheckbox | 5 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloComboBox | 6 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloDatePicker | 9 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloDropDownList | 6 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloNumericInput | 5 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloRadio | 5 | multi-pass | Yes (RadioGroup data binding) | **T3 - Medium** | ✅ RESOLVED |
| MariloTextField | 6 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloTextArea | 5 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloSelect | 2 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloSwitch | 5 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloSlider | 5 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloAccordionItem | 4 | single-pass | No | **T3 - Medium** | ✅ RESOLVED (in T2) |
| MariloAppBar | 5 | multi-pass | Yes (child components) | **T3 - Medium** | ✅ RESOLVED |
| MariloTabStrip | 4 | single-pass | No | **T3 - Medium** | ✅ RESOLVED |
| MariloStep | 5 | single-pass | No | **T3 - Medium** | ✅ RESOLVED (in T2) |
| MariloBreadcrumb | 7 | multi-pass | Yes (data binding, collapse) | **T3 - Medium** | ✅ RESOLVED |
| MariloToolbar | 5 | multi-pass | Yes (overflow, adaptive) | **T3 - Medium** | ✅ RESOLVED |
| MariloColorPicker | 8 | multi-pass | Yes (HSV canvas, palette) | **T4 - Low** | ⚠️ PARTIALLY IMPLEMENTED (core picker done; FlatColorPicker, ColorGradient, ColorPalette standalone components missing; AdaptiveMode absent) |
| MariloDateRangePicker | 10 | multi-pass | Yes (dual-calendar popup) | **T4 - Low** | ⚠️ PARTIALLY IMPLEMENTED (dual-calendar, range preview, AllowReverse done; multi-view calendar, OnOpen/OnClose/OnChange events, AdaptiveMode, Size/Rounded/FillMode, FocusAsync, HeaderTemplate missing) |
| MariloDateTimePicker | 8 | multi-pass | Yes (calendar + time tumblers) | **T4 - Low** | ⚠️ PARTIALLY IMPLEMENTED (~60% API surface; calendar+tumbler UX solid; OnChange/OnOpen/OnClose/OnBlur/OnCalendarCellRender events, ValidateOn, AdaptiveMode, DateTimePickerSteps missing) |
| MariloTimePicker | 5 | multi-pass | Yes (tumbler UI) | **T4 - Low** | ⚠️ PARTIALLY IMPLEMENTED (tumbler UX, step params, keyboard done; OnOpen/OnClose not cancellable, ValidateOn, AdaptiveMode, InputMode missing; PopupClass declared but never applied) |
| MariloFileUpload | 4 | multi-pass | Yes (async/chunk upload) | **T4 - Low** | ⚠️ PARTIALLY IMPLEMENTED (client-side file select, validation, drag-drop done; DropZoneId inert, FileTemplate/FileInfoTemplate context type mismatch vs spec) |
| MariloUpload | 5 | multi-pass | Yes (chunk upload, drop zone) | **T4 - Low** | ⚠️ PARTIALLY IMPLEMENTED (HTTP upload, chunked upload, 10 events, pause/resume done; templates missing, WithCredentials inert, DropZoneId inert, chunk resume restarts from byte 0, UploadChunkSettings API absent) |
| MariloMaskedInput | 7 | single-pass | Yes (mask enforcement) | **T4 - Low** | ✅ RESOLVED |
| MariloMultiSelect | 5 | multi-pass | Yes (filtering, virtualization) | **T4 - Low** | ⚠️ PARTIALLY IMPLEMENTED (filtering, tags, keyboard, virtualization done; OnChange/OnRead/OnOpen/OnClose/OnBlur events, AllowCustom, GroupField, 5 template slots, ValueMapper missing; actual gaps ~12-15 not 5) |
| MariloRangeSlider | 6 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloRating | 5 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloSearchBox | 3 | single-pass | Yes (debounce, suggestions) | **T4 - Low** | ✅ RESOLVED |
| MariloDataBanner | 4 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloDataToast | 4 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloProgressCircle | 2 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloSnackbar | 2 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloSnackbarHost | 1 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloSpinner | 3 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloBreadcrumbItem | 3 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloEnvironmentBadge | 2 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloMenuItem | 3 | single-pass | No | **T4 - Low** | ✅ RESOLVED (in T2) |
| MariloToolbarButton | 2 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloToolbarGroup | 1 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloToolbarSeparator | 1 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloToolbarToggleButton | 3 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloTreeItem | 3 | single-pass | No | **T4 - Low** | ✅ RESOLVED (in T2) |
| MariloTimeRangeSelector | 3 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| TabStripTab | 1 | single-pass | No | **T4 - Low** | ✅ RESOLVED |
| MariloCardBody | 0 | n/a | No | **Complete** |
| MariloHighlighter | 0 | n/a | No | **Complete** |
| MariloImage | 0 | n/a | No | **Complete** |
| MariloTable | 0 | n/a | No | **Complete** | **OBSOLETE** — removed 2026-04-03; superseded by MariloDataGrid |
| MariloTimeline | 0 | n/a | No | **Complete** |
| MariloTimelineItem | 0 | n/a | No | **Complete** |
| MariloTypography | 0 | n/a | No | **Complete** |
| MariloMenuDivider | 0 | n/a | No | **Complete** |
| MariloNavBar | 0 | n/a | No | **Complete** |
| MariloNavItem | 0 | n/a | No | **Complete** |
| MariloNavMenu | 0 | n/a | No | **Complete** |

---

## 2. Phased Resolution Approach

### Phase 1: Critical Primitives and Foundation (T1)

**Goals:**
- Establish the component model base classes, theming infrastructure, and form/validation pipeline that all other components depend on.
- Ensure layout primitives (Grid, Stack, Container, Row, Column, Divider) are spec-complete so downstream components can compose correctly.
- Deliver the `EditContext` integration that unblocks every form input component.

**Success Criteria:**
- `MariloThemeProvider` generates CSS variables from theme tokens and supports dark mode toggling.
- `MariloForm` binds to a model via `EditContext`, fires `OnSubmit`/`OnValidSubmit`/`OnInvalidSubmit`, and supports child `FormItem`/`FormButtons` components.
- `MariloValidation` delivers `MariloValidationMessage`, `MariloValidationSummary`, and `MariloValidationTooltip` sub-components integrated with `EditContext`.
- `MariloField` and `MariloLabel` implement floating-label animation and validation-state styling.
- `MariloIcon` gaps (minor enum alignment) are resolved.
- Layout primitives (`MariloGrid`, `MariloStack`, `MariloContainer`, `MariloRow`, `MariloColumn`, `MariloDivider`) are spec-complete with child component support.

**Components:**
| Component | Path | Complexity | Notes |
|-----------|------|------------|-------|
| MariloThemeProvider | [Root](MariloThemeProvider.razor) | multi-pass | CSS variable generation, dark mode, RTL wrapper |
| MariloForm | [Forms/Containers](Forms/Containers/) | multi-pass | Model binding, EditContext, auto-generation, child components |
| MariloValidation | [Forms/Containers](Forms/Containers/) | multi-pass | 3 sub-components, EditContext integration |
| MariloField | [Forms/Containers](Forms/Containers/) | multi-pass | Floating animation, focus tracking |
| MariloLabel | [Forms/Containers](Forms/Containers/) | multi-pass | Floating behavior, validation integration |
| MariloIcon | [Utility](Utility/) | single-pass | Minor enum alignment |
| MariloGrid (Layout) | [Layout](Layout/) | multi-pass | GridLayoutColumn/Row/Item children |
| MariloStack | [Layout](Layout/) | single-pass | Spacing, Width/Height, alignment |
| MariloContainer | [Layout](Layout/) | single-pass | No gaps (no spec) |
| MariloRow | [Layout](Layout/) | single-pass | No gaps (no spec) |
| MariloColumn | [Layout](Layout/) | single-pass | No gaps (no spec) |
| MariloDivider | [Layout](Layout/) | single-pass | No gaps (no spec) |

**Dependencies:**
- MariloThemeProvider must be resolved first; its CSS variable system affects all component styling.
- MariloForm + MariloValidation must be resolved together; they share `EditContext` integration.
- Layout primitives have no internal dependencies and can be resolved in parallel.

**External Research:**
- Evaluate CSS variable generation approaches in existing OSS Blazor libraries (MudBlazor MIT, Radzen MIT, Blazorise Apache-2.0) for patterns — not code copying.
- Evaluate `EditContext` integration patterns from ASP.NET Core Blazor documentation (MIT-licensed framework code).

---

### Phase 2: Complex Data and Interaction Components (T2)

**Goals:**
- Deliver the DataGrid with paging, sorting, filtering, editing, and virtual scrolling.
- Deliver the rich-text Editor with a compatible OSS WYSIWYG engine.
- Deliver the Chart with a viable rendering strategy.
- Deliver Window, Dialog, Popover, and Drawer with proper overlay/positioning infrastructure.
- Deliver data-driven navigation components (TreeView, Menu, ContextMenu, Accordion).
- Establish shared infrastructure: data-binding patterns, JS interop for drag/resize/positioning, and virtual scrolling utilities.

**Success Criteria:**
- `MariloDataGrid` supports: Height, paging (client + server via OnRead), sorting, filtering, column templates, editing (inline + popup + incell), virtual scrolling, and public GridState API.
- `MariloEditor` provides WYSIWYG editing via an OSS-compatible JS library (e.g., Tiptap/ProseMirror, MIT) with tool system, paste cleanup, and XSS sanitization.
- `MariloChart` renders at least 12 chart types with tooltips, legends, axis configuration, and accessibility.
- `MariloWindow` supports drag, resize, minimize/maximize/restore state, and two-way position/size binding.
- `MariloDialog`/`MariloConfirmDialog` support two-way `Visible` binding, custom actions, and predefined dialogs.
- `MariloPopover` supports anchor-based positioning with Show/Hide methods.
- `MariloDrawer` supports Push/Overlay modes, MiniMode, and data binding.
- `MariloTreeView`, `MariloMenu`, `MariloContextMenu`, `MariloAccordion` support hierarchical data binding.
- `MariloSplitter` supports multi-pane interactive resize with collapse.
- `MariloPagination` supports Total/PageSize model with page-size dropdown.
- `MariloStepper` supports orientation, linear flow, and step validation.

**Components:**
| Component | Path | Complexity | Notes |
|-----------|------|------------|-------|
| MariloDataGrid | [DataGrid](DataGrid/) | multi-pass | Paging, sorting, filtering, editing, virtual scroll |
| MariloGridColumn | [DataGrid](DataGrid/) | single-pass | Column templates, field expressions |
| MariloGridToolbar | [DataGrid](DataGrid/) | single-pass | Search, custom tools |
| MariloEditor | [Editors](Editors/) | multi-pass | ProseMirror/Tiptap integration, tool system |
| MariloChart | [Charts](Charts/) | multi-pass | Rendering engine decision |
| MariloChartSeries | [Charts](Charts/) | multi-pass | Series types, data models |
| MariloWindow | [Overlays](Overlays/) | multi-pass | JS interop drag/resize, state management |
| MariloDialog | [Feedback](Feedback/) | single-pass | Two-way Visible, custom actions |
| MariloConfirmDialog | [Feedback](Feedback/) | single-pass | Predefined dialog patterns |
| MariloPopover | [DataDisplay](DataDisplay/) | multi-pass | Anchor positioning, Show/Hide API |
| MariloDrawer | [Layout](Layout/) | multi-pass | Push/Overlay modes, MiniMode |
| MariloList | [DataDisplay](DataDisplay/) | multi-pass | Full ListBox feature set |
| MariloTreeView | [Navigation](Navigation/) | multi-pass | Hierarchical data binding |
| MariloMenu | [Navigation](Navigation/) | multi-pass | Hierarchy rendering from data |
| MariloContextMenu | [Navigation](Navigation/) | multi-pass | Selector pattern, data binding |
| MariloAccordion | [Layout](Layout/) | multi-pass | Data binding, ExpandMode |
| MariloSplitter | [Layout](Layout/) | multi-pass | Multi-pane resize |
| MariloPanel | [Layout](Layout/) | multi-pass | PanelBar features |
| MariloStepper | [Layout](Layout/) | multi-pass | Orientation, linear flow |
| MariloPagination | [Navigation](Navigation/) | single-pass | Total/PageSize model |

**Dependencies:**
- Phase 1 (ThemeProvider, Form/Validation, Layout) must be complete.
- DataGrid depends on MariloPagination for its pager; resolve Pagination first or in parallel.
- Dialog/ConfirmDialog share overlay infrastructure with Window and Popover; design shared positioning service.
- TreeView, Menu, ContextMenu share hierarchical data-binding patterns; design once, apply to all.
- Editor depends on JS interop infrastructure that can be shared with Window drag/resize.

**External Research Required:**
| Area | Candidate Libraries | License | Purpose |
|------|-------------------|---------|---------|
| Rich-text editing | Tiptap (wraps ProseMirror) | MIT | WYSIWYG editor engine via JS interop |
| HTML sanitization | HtmlSanitizer | MIT | XSS prevention for Editor preview/output |
| Chart rendering | Option A: SVG server-side (current) | n/a | Extend existing approach |
| Chart rendering | Option B: Chart.js via JS interop | MIT | Client-side canvas rendering |
| Virtual scrolling | Blazor Virtualize (built-in) | MIT (framework) | DataGrid row virtualization |
| Drag/resize | Custom JS interop module | n/a | Window/Splitter drag and resize |
| Popover positioning | Floating UI | MIT | Anchor-based positioning calculations |

---

### Phase 3: Standard Components — Single-Pass Resolution (T3)

**Goals:**
- Resolve all remaining standard components that can be completed in a single implementation pass.
- Apply patterns established in Phases 1-2 (data binding, validation integration, accessibility, parameter naming).
- Bring all button, data-display, feedback, and standard input components to spec parity.

**Success Criteria:**
- All T3 components pass bUnit tests verifying documented API parameters, events, and rendering.
- Consistent parameter naming across all components (`Enabled` vs `Disabled`, `Value` binding patterns).
- EditContext/validation integration works on all form input components.
- ARIA roles and keyboard navigation present on interactive components.

**Components:**
| Component | Path | Complexity | Notes |
|-----------|------|------------|-------|
| MariloButton | [Buttons](Buttons/) | single-pass | Enabled, icon consistency |
| MariloButtonGroup | [Buttons](Buttons/) | multi-pass | Child component architecture |
| MariloChip | [Buttons](Buttons/) | single-pass | Selection, removable, icon |
| MariloChipSet | [Buttons](Buttons/) | multi-pass | Generic data binding |
| MariloSplitButton | [Buttons](Buttons/) | multi-pass | Dropdown behavior |
| MariloFab | [Buttons](Buttons/) | single-pass | Position, alignment, icon |
| MariloToggleButton | [Buttons](Buttons/) | single-pass | Minor parameter gaps |
| MariloIconButton | [Buttons](Buttons/) | single-pass | Minor gaps |
| MariloSegmentedControl | [Buttons](Buttons/) | single-pass | Minor gaps |
| MariloAvatar | [DataDisplay](DataDisplay/) | single-pass | Type, image fallback |
| MariloBadge | [DataDisplay](DataDisplay/) | single-pass | Position, visibility |
| MariloCard | [DataDisplay](DataDisplay/) | single-pass | Orientation, Width |
| MariloCardActions | [DataDisplay](DataDisplay/) | single-pass | Layout parameter |
| MariloCardHeader | [DataDisplay](DataDisplay/) | single-pass | Sub-components |
| MariloCarousel | [DataDisplay](DataDisplay/) | multi-pass | Generic TItem, data-driven |
| MariloListItem | [DataDisplay](DataDisplay/) | single-pass | Parent integration |
| MariloListView | [DataDisplay](DataDisplay/) | single-pass | Paging, selection |
| MariloTooltip | [DataDisplay](DataDisplay/) | multi-pass | Selector-based targeting |
| MariloAlert | [Feedback](Feedback/) | single-pass | ARIA, icon customization |
| MariloAlertStrip | [Feedback](Feedback/) | single-pass | Minor gaps |
| MariloCallout | [Feedback](Feedback/) | single-pass | Minor gaps |
| MariloProgressBar | [Feedback](Feedback/) | single-pass | Max, labels, ARIA |
| MariloSkeleton | [Feedback](Feedback/) | single-pass | AnimationType, Visible |
| MariloToast | [Feedback](Feedback/) | single-pass | Declarative vs imperative alignment |
| MariloAutocomplete | [Forms/Inputs](Forms/Inputs/) | single-pass | Filtering, templates |
| MariloCheckbox | [Forms/Inputs](Forms/Inputs/) | single-pass | Indeterminate, label position |
| MariloComboBox | [Forms/Inputs](Forms/Inputs/) | single-pass | Filtering, custom values |
| MariloDatePicker | [Forms/Inputs](Forms/Inputs/) | single-pass | DateTime type, format |
| MariloDropDownList | [Forms/Inputs](Forms/Inputs/) | single-pass | Templates, grouping |
| MariloNumericInput | [Forms/Inputs](Forms/Inputs/) | single-pass | Generic type support |
| MariloRadio | [Forms/Inputs](Forms/Inputs/) | multi-pass | RadioGroup from data |
| MariloTextField | [Forms/Inputs](Forms/Inputs/) | single-pass | ClearButton, debounce |
| MariloTextArea | [Forms/Inputs](Forms/Inputs/) | single-pass | AutoSize, debounce |
| MariloSelect | [Forms/Inputs](Forms/Inputs/) | single-pass | Minor gaps |
| MariloSwitch | [Forms/Inputs](Forms/Inputs/) | single-pass | Value binding alignment |
| MariloSlider | [Forms/Inputs](Forms/Inputs/) | single-pass | Generic, ticks, buttons |
| MariloAccordionItem | [Layout](Layout/) | single-pass | Parent integration |
| MariloAppBar | [Layout](Layout/) | multi-pass | Child component sections |
| MariloTabStrip | [Layout](Layout/) | single-pass | Scrollable, persist |
| MariloStep | [Layout](Layout/) | single-pass | Icon, validation state |
| MariloBreadcrumb | [Navigation](Navigation/) | multi-pass | Data binding, collapse |
| MariloToolbar | [Navigation](Navigation/) | multi-pass | Overflow, adaptive |

**Dependencies:**
- Phase 1 must be complete (ThemeProvider, Form/Validation).
- Phase 2 overlay infrastructure (Popover positioning) should be available for Tooltip and dropdown components.
- Input components depend on Phase 1 Form/Validation for EditContext integration.

---

### Phase 4: UX Polish, Performance, Accessibility, and Edge Cases (T4)

**Goals:**
- Resolve remaining low-priority component gaps.
- Implement advanced input components (ColorPicker HSV canvas, DateRangePicker dual calendar, TimePicker tumbler).
- Complete file upload components with server-side and chunked upload support.
- Performance-tune virtual scrolling, large datasets, and animation.
- Full WCAG 2.1 AA accessibility audit and remediation across all components.
- API edge-case testing and documentation alignment.

**Success Criteria:**
- All components pass automated accessibility audits (axe-core or equivalent).
- File upload components support async server upload with progress tracking.
- Advanced pickers (ColorPicker, DateRangePicker, DateTimePicker, TimePicker) deliver rich UIs matching spec.
- All remaining low-severity gaps are resolved or explicitly documented as "won't fix" with rationale.

**Components:**
| Component | Path | Complexity | Notes |
|-----------|------|------------|-------|
| MariloColorPicker | [Forms/Inputs](Forms/Inputs/) | multi-pass | HSV canvas, palette |
| MariloDateRangePicker | [Forms/Inputs](Forms/Inputs/) | multi-pass | Dual-calendar popup |
| MariloDateTimePicker | [Forms/Inputs](Forms/Inputs/) | multi-pass | Calendar + time tumblers |
| MariloTimePicker | [Forms/Inputs](Forms/Inputs/) | multi-pass | Tumbler UI |
| MariloFileUpload | [Forms/Inputs](Forms/Inputs/) | multi-pass | Async/chunk upload |
| MariloUpload | [Forms/Inputs](Forms/Inputs/) | multi-pass | Chunk upload, drop zone |
| MariloMaskedInput | [Forms/Inputs](Forms/Inputs/) | single-pass | Mask enforcement |
| MariloMultiSelect | [Forms/Inputs](Forms/Inputs/) | multi-pass | Filtering, virtualization |
| MariloRangeSlider | [Forms/Inputs](Forms/Inputs/) | single-pass | Dual handles |
| MariloRating | [Forms/Inputs](Forms/Inputs/) | single-pass | Half-star, precision |
| MariloSearchBox | [Forms/Inputs](Forms/Inputs/) | single-pass | Debounce, suggestions |
| MariloDataBanner | [Feedback](Feedback/) | single-pass | Minor gaps |
| MariloDataToast | [Feedback](Feedback/) | single-pass | Minor gaps |
| MariloProgressCircle | [Feedback](Feedback/) | single-pass | Minor gaps |
| MariloSnackbar | [Feedback](Feedback/) | single-pass | Minor gaps |
| MariloSnackbarHost | [Feedback](Feedback/) | single-pass | Minor gaps |
| MariloSpinner | [Feedback](Feedback/) | single-pass | Size, ARIA |
| MariloBreadcrumbItem | [Navigation](Navigation/) | single-pass | Minor gaps |
| MariloEnvironmentBadge | [Navigation](Navigation/) | single-pass | Minor gaps |
| MariloMenuItem | [Navigation](Navigation/) | single-pass | URL, nesting |
| MariloToolbarButton | [Navigation](Navigation/) | single-pass | Minor gaps |
| MariloToolbarGroup | [Navigation](Navigation/) | single-pass | Minor gaps |
| MariloToolbarSeparator | [Navigation](Navigation/) | single-pass | Minor gaps |
| MariloToolbarToggleButton | [Navigation](Navigation/) | single-pass | Minor gaps |
| MariloTreeItem | [Navigation](Navigation/) | single-pass | Minor gaps |
| MariloTimeRangeSelector | [Navigation](Navigation/) | single-pass | Minor gaps |
| TabStripTab | [Layout](Layout/) | single-pass | Minor gaps |

**Dependencies:**
- Phases 1-3 must be substantially complete.
- Advanced picker UIs may depend on Popover positioning from Phase 2.
- File upload server integration may depend on project-specific backend decisions.

**External Research Required:**
| Area | Candidate Libraries | License | Purpose |
|------|-------------------|---------|---------|
| Color picker canvas | Custom JS interop | n/a | HSV/HSL color selection UI |
| Input masking | IMask.js | MIT | Client-side input mask enforcement |
| Debounce | System.Reactive or custom | MIT / n/a | Debounced input event handling |

---

## 3. Software Architecture Guidelines (Open-Source Focus)

### Component Model

#### Base Component Abstractions

```
MariloComponentBase (abstract)
├── Class : string
├── Style : string
├── AdditionalAttributes : Dictionary<string, object>
├── Id : string (auto-generated if not provided)
└── SetParametersAsync() — common parameter validation

MariloInputBase<TValue> : MariloComponentBase
├── Value : TValue
├── ValueChanged : EventCallback<TValue>
├── ValueExpression : Expression<Func<TValue>>
├── Enabled : bool (default true)
├── ReadOnly : bool
├── Placeholder : string
├── Label : string
├── Width : string
├── EditContext integration (cascaded from MariloForm)
└── Validation message rendering

MariloDataBoundComponent<TItem> : MariloComponentBase
├── Data : IEnumerable<TItem>
├── TextField : string (or Expression)
├── ValueField : string (or Expression)
├── ItemTemplate : RenderFragment<TItem>
└── Common data-binding infrastructure
```

#### Class/Partial Class Structure

- Each component uses a `.razor` file for markup and a `.razor.cs` partial class for logic.
- Base classes live in a shared `Base/` or `Internal/` namespace (e.g., `Marilo.Components.Internal`).
- Public API surfaces live in the component's namespace (e.g., `Marilo.Components.DataGrid`).

#### Composition vs. Inheritance

- **Inherit** when sharing core behavior: all inputs inherit `MariloInputBase<TValue>`, all data-bound components inherit `MariloDataBoundComponent<TItem>`.
- **Compose** when sharing optional behaviors: sorting, filtering, paging, and grouping are implemented as services or utility classes that components consume, not as base class features.
- **Cascade** shared state: `MariloForm` cascades `EditContext`, `MariloThemeProvider` cascades theme tokens.

### Data/Operations Model

#### Public API Design Principles

- Parameters are designed to be **conceptually similar** to Telerik UI for Blazor (for migration ease) but with **independent implementations**.
- Naming follows Telerik conventions where they align with Blazor community norms (e.g., `Data`, `Value`, `OnRead`, `PageSize`, `Sortable`, `Filterable`).
- Where Telerik naming diverges from Blazor conventions, prefer Blazor conventions (e.g., `ValueChanged` over custom event patterns).

#### Server-Side Data Operations (OnRead Pattern)

```csharp
// Consumer provides data manually via OnRead callback
[Parameter] public EventCallback<DataSourceRequestEventArgs> OnRead { get; set; }

public class DataSourceRequestEventArgs
{
    public DataSourceRequest Request { get; set; } // sort, filter, page, group descriptors
    public IEnumerable Data { get; set; }          // consumer sets this
    public int Total { get; set; }                 // consumer sets this
}
```

- When `OnRead` is bound, the component does NOT auto-sort/filter/page the `Data` collection.
- When `OnRead` is NOT bound, the component performs client-side operations on the provided `Data`.
- This dual-mode design matches Telerik's public behavior without copying implementation.

#### State Management

- Components expose a `State` object (e.g., `GridState`, `TreeViewState`) that captures current UI state (page, sort, expanded nodes, selection).
- State objects are serializable for URL persistence or session storage.
- Two-way binding via `@bind-State` is supported.

### Extensibility

#### Template Parameters

Every component that renders collections supports:
- `ItemTemplate` / `Template` — custom rendering per item.
- `HeaderTemplate`, `FooterTemplate` — custom header/footer.
- `NoDataTemplate` — empty state.
- `LoadingTemplate` — loading state.

#### Extension Points vs. New Features

- Add a template parameter when the consumer needs to control rendering.
- Add a parameter when the consumer needs to control behavior (e.g., `Sortable`, `Filterable`).
- Add an event callback when the consumer needs to react to or override behavior (e.g., `OnRead`, `OnEdit`).
- Do NOT add extension points for hypothetical use cases. Wait for actual demand.

### Licensing and Third-Party Usage

#### Evaluation Rules

1. **Preferred licenses:** MIT, Apache-2.0, BSD-2-Clause, BSD-3-Clause. These are compatible with MIT/Apache-2.0 project licensing.
2. **Acceptable with review:** MPL-2.0 (file-level copyleft — acceptable if isolated in separate files). ISC (functionally MIT).
3. **Not acceptable:** GPL, LGPL, AGPL, SSPL, BSL, proprietary, or any license requiring the consuming project to adopt the same license.
4. **Always check:** License file in the repository, not just the package metadata. Some packages have different licenses for different versions.

#### Recording Adopted Code

When external code is copied or adapted (not just used as a NuGet/npm dependency):
1. Record in the component's `RESEARCH_LOG.md`: source URL, license, what was taken, any modifications.
2. Include the original license text in a `THIRD_PARTY_LICENSES` section or file.
3. Add a comment in the source file referencing the origin: `// Adapted from [project] ([license]) — see RESEARCH_LOG.md`.

---

## 4. Class Structure, Hierarchy, and Patterns

### Base and Derived Components

```
Marilo.Components.Internal
├── MariloComponentBase              — common parameters (Class, Style, Id, AdditionalAttributes)
├── MariloInputBase<TValue>          — value binding, validation, enabled/readonly, label
├── MariloDataBoundComponent<TItem>  — data binding, field expressions, templates
├── MariloPopupBase                  — popup/overlay positioning, Show/Hide, anchor
└── MariloCompositeComponent         — child component registration, cascading

Marilo.Components.DataGrid
├── MariloDataGrid : MariloDataBoundComponent<TItem>
│   ├── GridState management
│   ├── Paging, sorting, filtering, grouping engines (internal services)
│   └── Editing coordinator
├── MariloGridColumn : MariloComponentBase
│   └── Column metadata, templates, field binding
└── MariloGridToolbar : MariloComponentBase

Marilo.Components.Layout
├── MariloAccordion : MariloDataBoundComponent<TItem>  (or MariloCompositeComponent)
├── MariloDrawer : MariloComponentBase
├── MariloSplitter : MariloCompositeComponent
├── MariloStepper : MariloCompositeComponent
└── MariloTabStrip : MariloCompositeComponent

Marilo.Components.Navigation
├── MariloTreeView : MariloDataBoundComponent<TItem>
├── MariloMenu : MariloDataBoundComponent<TItem>
└── MariloContextMenu : MariloDataBoundComponent<TItem>
```

### Shared Utility Services

```
Marilo.Components.Services
├── ISortingService          — sort descriptor application to IEnumerable/IQueryable
├── IFilteringService        — filter descriptor application, expression building
├── IGroupingService         — group descriptor application, aggregate computation
├── IPagingService           — page slicing, total computation
├── IVirtualScrollService    — viewport calculation, item height estimation, placeholder management
├── IPopupPositionService    — anchor-relative positioning (wraps Floating UI or custom logic)
├── IDragService             — JS interop for drag operations (Window, Splitter)
├── IColumnMetadataService   — column width, visibility, order tracking
└── IThemeService            — CSS variable resolution, theme switching
```

These services are registered via `AddMariloComponents()` in DI and are `internal` — not part of the public API. Components consume them via `[Inject]`.

### Reusable Render Fragments and Definitions

- **Column definitions** (`MariloGridColumn`) use `CascadingParameter` to register with the parent grid. The grid collects columns during `OnParametersSet`.
- **Tab definitions** (`TabStripTab`) follow the same pattern with `MariloTabStrip`.
- **Menu items** can be defined declaratively (`MariloMenuItem` children) or via data binding (`Data` + field expressions). Both paths produce the same internal `MenuItemModel`.

### Naming and Namespaces

```
Marilo.Components                — root namespace, MariloComponentBase, service registration
Marilo.Components.Buttons        — Button, ButtonGroup, Chip, ChipSet, Fab, SplitButton, etc.
Marilo.Components.Charts         — Chart, ChartSeries
Marilo.Components.DataDisplay    — Avatar, Badge, Card, Carousel, List, Popover, Tooltip, etc.
Marilo.Components.DataGrid       — DataGrid, GridColumn, GridToolbar, GridState
Marilo.Components.Editors        — Editor
Marilo.Components.Feedback       — Alert, Dialog, ProgressBar, Toast, Snackbar, etc.
Marilo.Components.Forms          — Form, Field, Label, Validation
Marilo.Components.Forms.Inputs   — all input components
Marilo.Components.Layout         — Accordion, AppBar, Drawer, Grid, Panel, Splitter, etc.
Marilo.Components.Navigation     — Breadcrumb, Menu, ContextMenu, Pagination, Toolbar, TreeView
Marilo.Components.Overlays       — Window
Marilo.Components.Utility        — Icon
Marilo.Components.Internal       — base classes, services, shared utilities (not public API)
```

---

## 5. Testing Strategy (bUnit and Beyond)

### Unit Tests with bUnit

Every component gets a test class in a parallel `Marilo.Components.Tests` project mirroring the source structure.

#### Core Test Categories

1. **Rendering tests** — verify that given specific parameters, the component renders the expected HTML structure, CSS classes, and attributes.
2. **Parameter tests** — verify that each public parameter is applied correctly (e.g., setting `Disabled="true"` adds `disabled` attribute).
3. **Event tests** — verify that user interactions trigger the correct `EventCallback` parameters with the expected arguments.
4. **Two-way binding tests** — verify that `@bind-Value` updates both directions correctly.
5. **Validation integration tests** — verify that input components display validation messages from `EditContext`.
6. **Template tests** — verify that `RenderFragment` and `RenderFragment<TItem>` parameters are rendered in the correct locations.
7. **Accessibility tests** — verify ARIA attributes, roles, and keyboard interaction patterns.

#### bUnit Test Patterns

```csharp
// Rendering test
[Fact]
public void DataGrid_RendersWithHeight()
{
    var cut = RenderComponent<MariloDataGrid<Person>>(p => p
        .Add(g => g.Data, testData)
        .Add(g => g.Height, "400px"));

    cut.Find("div.marilo-datagrid").GetAttribute("style")
        .Should().Contain("height: 400px");
}

// Event test
[Fact]
public void DataGrid_PageChanged_FiresCallback()
{
    var pageChanged = false;
    var cut = RenderComponent<MariloDataGrid<Person>>(p => p
        .Add(g => g.Data, testData)
        .Add(g => g.Pageable, true)
        .Add(g => g.PageSize, 10)
        .Add(g => g.OnPageChanged, args => pageChanged = true));

    // Simulate page navigation
    cut.Find(".marilo-pager-next").Click();
    pageChanged.Should().BeTrue();
}

// Validation integration test
[Fact]
public void TextField_ShowsValidationMessage_WhenInvalid()
{
    var model = new TestModel();
    var cut = RenderComponent<MariloForm>(p => p
        .Add(f => f.Model, model)
        .AddChildContent<MariloTextField>(t => t
            .Add(t => t.ValueExpression, () => model.Name)));

    cut.Find("form").Submit();
    cut.Find(".validation-message").TextContent
        .Should().Contain("Name is required");
}
```

### Behavioral Parity Tests

For each component with a documented Telerik API equivalent, write tests that verify:
- The same set of public parameters exists and accepts the documented types.
- The same events fire under the documented conditions.
- The same visual states are achievable (e.g., a grid with `Sortable="true"` renders sort indicators and responds to header clicks).

These tests do NOT compare output HTML to Telerik output — they verify **behavioral equivalence** against the documented API.

### JS Interop Mocking

```csharp
// Mock JS interop for components that use it (Window, Editor, Popover)
var jsRuntime = new BunitJSInterop();
jsRuntime.SetupModule("./js/marilo-window.js")
    .Setup<BoundingClientRect>("getElementPosition", _ => true)
    .SetResult(new BoundingClientRect { Top = 100, Left = 200 });
```

### Cross-Browser and Visual Testing (Optional)

- Playwright-based visual regression tests can be added for complex components (DataGrid, Editor, Charts).
- Use screenshot comparison with a tolerance threshold.
- Run in CI against Chromium; cross-browser runs are optional and configurable.

### Test Coverage Targets

| Category | Target |
|----------|--------|
| Public parameters | 100% — every parameter has at least one test |
| Event callbacks | 100% — every event fires correctly |
| Core behaviors | 90%+ — sorting, filtering, paging, editing, validation |
| Edge cases | Best-effort — null data, empty collections, extreme values |
| Accessibility | All interactive components have ARIA and keyboard tests |

---

## 6. Per-Component Workflow Tracking

### Resolution Folder Structure

Each component (or component group sharing a gap analysis file) has a resolution folder at:

```
/workspaces/Marilo/src/Marilo.Components/{ComponentPath}/resolution/
```

### Required Files Per Component

| File | Purpose |
|------|---------|
| `RESOLUTION_STATUS.md` | Frontmatter-driven status tracking |
| `RESEARCH_LOG.md` | External OSS research and license decisions |
| `IMPLEMENTATION_NOTES.md` | Design decisions, approach, code notes |
| `TEST_PLAN.md` | Specific test cases planned for this component |
| `PROTOTYPE_NOTES.md` | Multi-pass components only: experiment log per pass |

### Component Resolution Tracking Table

| Component | Phase | Complexity | Resolution Folder |
|-----------|-------|------------|-------------------|
| MariloThemeProvider | 1 | multi-pass | [Root/resolution](resolution/) |
| MariloForm | 1 | multi-pass | [Forms/Containers/resolution](Forms/Containers/resolution/) |
| MariloValidation | 1 | multi-pass | [Forms/Containers/resolution](Forms/Containers/resolution/) |
| MariloField | 1 | multi-pass | [Forms/Containers/resolution](Forms/Containers/resolution/) |
| MariloLabel | 1 | multi-pass | [Forms/Containers/resolution](Forms/Containers/resolution/) |
| MariloIcon | 1 | single-pass | [Utility/resolution](Utility/resolution/) |
| MariloGrid (Layout) | 1 | multi-pass | [Layout/resolution](Layout/resolution/) |
| MariloStack | 1 | single-pass | [Layout/resolution](Layout/resolution/) |
| MariloContainer | 1 | single-pass | [Layout/resolution](Layout/resolution/) |
| MariloRow | 1 | single-pass | [Layout/resolution](Layout/resolution/) |
| MariloColumn | 1 | single-pass | [Layout/resolution](Layout/resolution/) |
| MariloDivider | 1 | single-pass | [Layout/resolution](Layout/resolution/) |
| MariloDataGrid | 2 | multi-pass | [DataGrid/resolution](DataGrid/resolution/) |
| MariloGridColumn | 2 | single-pass | [DataGrid/resolution](DataGrid/resolution/) |
| MariloGridToolbar | 2 | single-pass | [DataGrid/resolution](DataGrid/resolution/) |
| MariloEditor | 2 | multi-pass | [Editors/resolution](Editors/resolution/) |
| MariloChart | 2 | multi-pass | [Charts/resolution](Charts/resolution/) |
| MariloChartSeries | 2 | multi-pass | [Charts/resolution](Charts/resolution/) |
| MariloWindow | 2 | multi-pass | [Overlays/resolution](Overlays/resolution/) |
| MariloDialog | 2 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloConfirmDialog | 2 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloPopover | 2 | multi-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloDrawer | 2 | multi-pass | [Layout/resolution](Layout/resolution/) |
| MariloList | 2 | multi-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloTreeView | 2 | multi-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloMenu | 2 | multi-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloContextMenu | 2 | multi-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloAccordion | 2 | multi-pass | [Layout/resolution](Layout/resolution/) |
| MariloSplitter | 2 | multi-pass | [Layout/resolution](Layout/resolution/) |
| MariloPanel | 2 | multi-pass | [Layout/resolution](Layout/resolution/) |
| MariloStepper | 2 | multi-pass | [Layout/resolution](Layout/resolution/) |
| MariloPagination | 2 | single-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloButton | 3 | single-pass | [Buttons/resolution](Buttons/resolution/) |
| MariloButtonGroup | 3 | multi-pass | [Buttons/resolution](Buttons/resolution/) |
| MariloChip | 3 | single-pass | [Buttons/resolution](Buttons/resolution/) |
| MariloChipSet | 3 | multi-pass | [Buttons/resolution](Buttons/resolution/) |
| MariloSplitButton | 3 | multi-pass | [Buttons/resolution](Buttons/resolution/) |
| MariloFab | 3 | single-pass | [Buttons/resolution](Buttons/resolution/) |
| MariloToggleButton | 3 | single-pass | [Buttons/resolution](Buttons/resolution/) |
| MariloIconButton | 3 | single-pass | [Buttons/resolution](Buttons/resolution/) |
| MariloSegmentedControl | 3 | single-pass | [Buttons/resolution](Buttons/resolution/) |
| MariloAvatar | 3 | single-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloBadge | 3 | single-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloCard | 3 | single-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloCardActions | 3 | single-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloCardHeader | 3 | single-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloCarousel | 3 | multi-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloListItem | 3 | single-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloListView | 3 | single-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloTooltip | 3 | multi-pass | [DataDisplay/resolution](DataDisplay/resolution/) |
| MariloAlert | 3 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloAlertStrip | 3 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloCallout | 3 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloProgressBar | 3 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloSkeleton | 3 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloToast | 3 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloAutocomplete | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloCheckbox | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloComboBox | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloDatePicker | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloDropDownList | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloNumericInput | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloRadio | 3 | multi-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloTextField | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloTextArea | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloSelect | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloSwitch | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloSlider | 3 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloAccordionItem | 3 | single-pass | [Layout/resolution](Layout/resolution/) |
| MariloAppBar | 3 | multi-pass | [Layout/resolution](Layout/resolution/) |
| MariloTabStrip | 3 | single-pass | [Layout/resolution](Layout/resolution/) |
| MariloStep | 3 | single-pass | [Layout/resolution](Layout/resolution/) |
| MariloBreadcrumb | 3 | multi-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloToolbar | 3 | multi-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloColorPicker | 4 | multi-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloDateRangePicker | 4 | multi-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloDateTimePicker | 4 | multi-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloTimePicker | 4 | multi-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloFileUpload | 4 | multi-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloUpload | 4 | multi-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloMaskedInput | 4 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloMultiSelect | 4 | multi-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloRangeSlider | 4 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloRating | 4 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloSearchBox | 4 | single-pass | [Forms/Inputs/resolution](Forms/Inputs/resolution/) |
| MariloDataBanner | 4 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloDataToast | 4 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloProgressCircle | 4 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloSnackbar | 4 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloSnackbarHost | 4 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloSpinner | 4 | single-pass | [Feedback/resolution](Feedback/resolution/) |
| MariloBreadcrumbItem | 4 | single-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloEnvironmentBadge | 4 | single-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloMenuItem | 4 | single-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloToolbarButton | 4 | single-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloToolbarGroup | 4 | single-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloToolbarSeparator | 4 | single-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloToolbarToggleButton | 4 | single-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloTreeItem | 4 | single-pass | [Navigation/resolution](Navigation/resolution/) |
| MariloTimeRangeSelector | 4 | single-pass | [Navigation/resolution](Navigation/resolution/) |
| TabStripTab | 4 | single-pass | [Layout/resolution](Layout/resolution/) |

### Open-Source Licensing in Research and Implementation

All external code and library evaluation follows these rules:

1. **Research phase:** Every OSS project inspected is logged in the component's `RESEARCH_LOG.md` with: project name, URL, license, fitness assessment, and adoption decision.
2. **NuGet/npm dependencies:** Only packages with MIT, Apache-2.0, BSD-2-Clause, or BSD-3-Clause licenses may be added. MPL-2.0 is acceptable with isolation. GPL-family licenses are prohibited.
3. **Copied/adapted code:** When code is copied or adapted from an external source (not just used as a package dependency), the source file must include a comment citing the origin, and the license text must be included in the project's `THIRD_PARTY_LICENSES` file.
4. **Review gate:** Before any external dependency or adapted code is merged, the `RESEARCH_LOG.md` entry must have `approved: true` in the component's `RESOLUTION_STATUS.md` external-resources section.
5. **Periodic audit:** Licenses of all external dependencies are audited at each phase boundary to ensure ongoing compliance.

---

## 7. T4 Component Audit — Detailed Findings (2026-04-02)

This section records the results of a source-code audit of all T4 "IMPLEMENTED" components, comparing actual implementation against the API specifications in `/docs/component-specs/`. The audit found that while core UX functionality is solid across all components, the plan's status markers were overly optimistic. All T4 components are **PARTIALLY IMPLEMENTED**, not fully implemented.

### Common Cross-Component Gaps

The following gaps are systemic across most/all T4 components:

| Gap | Affected Components | Severity |
|-----|-------------------|----------|
| `AdaptiveMode` parameter missing | All T4 pickers, MultiSelect | Low |
| `ValidateOn` / EditContext integration missing | DateTimePicker, TimePicker | Medium |
| Cancellable `OnOpen`/`OnClose` event args missing | DateRangePicker, DateTimePicker, MultiSelect | Medium |
| `role=combobox` on input elements missing | DateRangePicker, DateTimePicker, TimePicker | Low |
| `aria-controls`/`aria-activedescendant` missing | DateRangePicker, DateTimePicker, TimePicker | Low |
| CSS provider methods not component-specific | DateRangePicker reuses `DatePickerClass()`, DateTimePicker reuses `DatePickerClass()` | Low |

### Per-Component Audit Results

#### MariloColorPicker (Plan: 8 gaps, ✅ IMPLEMENTED → Actual: ⚠️ PARTIALLY IMPLEMENTED)

**What works:** HSV canvas with JS interop, hue/opacity sliders, palette view, popup trigger, `ValueFormat`, `ShowButtons`/`ShowPreview`/`ShowClearButton`, `Size`/`Rounded`/`FillMode` appearance params, cancellable `OnOpen`/`OnClose`, `Open()`/`Close()`/`FocusAsync()`, keyboard nav, WAI-ARIA (`role=combobox`, `aria-haspopup`), `ColorPickerClass()`/`ColorPickerPopupClass()` in CSS providers.

**Remaining gaps:**
- `MariloFlatColorPicker` standalone component — entirely absent (spec documents a separate inline variant)
- `MariloColorGradient` standalone component — entirely absent (only embedded inside ColorPicker)
- `MariloColorPalette` standalone component — entirely absent (only embedded inside ColorPicker)
- `ColorPickerViews` child-tag syntax is a stub — `ColorPickerGradientView`/`ColorPickerPaletteView` components don't exist as types; child tags can't configure gradient `Format`/`Formats`/`ShowOpacityEditor` or palette `Columns`/`Colors`/`TileWidth`/`TileHeight`
- `AdaptiveMode` parameter absent
- Bootstrap/FluentUI CSS coverage minimal (only root class styled; no popup, canvas, slider, preview, palette tile styles)

#### MariloDateRangePicker (Plan: 10 gaps, ✅ IMPLEMENTED → Actual: ⚠️ PARTIALLY IMPLEMENTED)

**What works:** `StartValue`/`EndValue` two-way binding, `Min`/`Max`/`DisabledDates`, `AllowReverse`, `Format`, `ShowClearButton`, `ShowOtherMonthDays`, `Orientation`, dual-calendar popup with independent navigation, hover-range preview, `Open()`/`Close()`/`NavigateTo()`/`Refresh()`, Escape-to-close, ARIA on popup/inputs/day buttons.

**Remaining gaps:**
- Multi-view calendar navigation (Year/Decade views) absent — blocks `BottomView`, `View`, `ViewChanged`, `OnCalendarCellRender`
- `OnChange` (with `DateRangePickerChangeEventArgs`), `OnOpen` (cancellable), `OnClose` (cancellable) events missing
- `AdaptiveMode`, `DebounceDelay`, `Title`, `Size`, `Rounded`, `FillMode` parameters missing
- `FocusStartAsync()`/`FocusEndAsync()` methods missing (require JS interop)
- `HeaderTemplate` missing
- `ShowWeekNumbers` declared but never rendered
- `PopupClass` wrapper-div bug (both ternary branches emit empty string)
- No dedicated `DateRangePickerClass()` in CSS provider (reuses `DatePickerClass()`)

#### MariloDateTimePicker (Plan: 8 gaps, ✅ IMPLEMENTED → Actual: ⚠️ PARTIALLY IMPLEMENTED ~60%)

**What works:** `Value` two-way binding, `Min`/`Max`, `DisabledDates`, `Format`, `ShowClearButton`, `ShowOtherMonthDays`, `ShowSeconds`, calendar popup with month nav, hour/minute/second tumblers, Now/Set/Cancel buttons, pending-date staging, Escape-to-close, `Open()`/`Close()`, ARIA on tumblers/input/popup.

**Remaining gaps:**
- All spec events missing: `OnChange`, `OnOpen` (cancellable), `OnClose` (cancellable), `OnBlur`, `OnCalendarCellRender`
- Only bespoke `OnConfirm` event exists (not in spec)
- `ValidateOn`, `AdaptiveMode`, `DebounceDelay` parameters missing
- `<DateTimePickerSteps>` child component not implemented (tumblers always increment by 1)
- Input is `readonly="true"` (no typed input support, contradicts spec)
- No dedicated `DateTimePickerClass()` in CSS provider

#### MariloTimePicker (Plan: 5 gaps, ✅ IMPLEMENTED → Actual: ⚠️ PARTIALLY IMPLEMENTED, ~8-10 gaps)

**What works:** Generic `TValue` support, tumbler dropdown (hour/minute/second/AM-PM), `HourStep`/`MinuteStep`/`SecondStep`, `Format`-driven column visibility, `Min`/`Max`, `ShowClearButton`, `DebounceDelay` (150ms default), `OnChange`/`OnOpen`/`OnClose`/`OnBlur` events (basic form), mouse-wheel scroll on tumblers, Now/Set/Cancel, keyboard (Enter/Escape/Tab), `Open()`/`Close()`.

**Remaining gaps:**
- `OnOpen`/`OnClose` not cancellable (no `IsCancelled` args)
- `AdaptiveMode`, `InputMode`, `ValidateOn` parameters missing
- `PopupClass` declared but **never applied** to popup div (bug)
- `<TimePickerSteps>` child component not implemented (flat params instead)
- `role=combobox` missing on input; tumbler `role=option` lacks parent `role=listbox`
- `OnChange` doesn't fire on blur (spec requires it)
- `TimePickerClass()` exists in providers but component ignores it (uses hardcoded BEM classes)

#### MariloFileUpload (Plan: 4 gaps, ✅ IMPLEMENTED → Actual: ⚠️ PARTIALLY IMPLEMENTED)

**What works:** `Accept`, `AllowedExtensions`, `MaxFileSize`/`MinFileSize`, `Capture`, `Multiple`, `Enabled`, `Files` (initial), drag-and-drop with visual state, client-side validation (extension/size), file list with remove, `OnSelect`/`OnRemove` events (cancellable), `ClearFiles()`/`RemoveFileAsync()`/`OpenSelectFilesDialog()`, three templates (`SelectFilesButtonTemplate`/`FileTemplate`/`FileInfoTemplate`), CSS provider integration.

**Remaining gaps:**
- `DropZoneId` parameter declared but inert (no JS interop to wire external drop zone)
- `FileTemplate`/`FileInfoTemplate` context type mismatch vs spec (passes raw `FileSelectFileInfo` instead of `FileTemplateContext` wrapper)
- Drop-zone CSS not delegated to CSS provider (hardcoded inline)

#### MariloUpload (Plan: 5 gaps, ✅ IMPLEMENTED → Actual: ⚠️ PARTIALLY IMPLEMENTED)

**What works:** `SaveUrl`/`RemoveUrl`/`SaveField`/`RemoveField`, `AutoUpload`, `ChunkSize` with chunked upload loop, `Multiple`, `Accept`, `AllowedExtensions`, `MaxFileSize`/`MinFileSize`, `Enabled`/`Capture`, `WithCredentials` (declared), pause/resume/cancel/retry per file, progress bars and status badges, all 10 events (`OnSelect`/`OnUpload`/`OnSuccess`/`OnError`/`OnProgress`/`OnRemove`/`OnCancel`/`OnClear`/`OnPause`/`OnResume`), `ClearFiles()`/`UploadFiles()`/`CancelFile()`/`PauseFile()`/`ResumeFile()`/`RetryFile()`/`RemoveFile()`/`OpenSelectFilesDialog()`, CSS provider integration (4 methods).

**Remaining gaps:**
- `SelectFilesButtonTemplate`, `FileTemplate`, `FileInfoTemplate` all missing (only `ChildContent` exists)
- `WithCredentials` declared but never applied to `HttpClient` requests
- `DropZoneId` declared but inert
- Chunk resume restarts from byte 0 (doesn't track paused chunk offset)
- `UploadChunkSettings` nested tag API absent (only flat `ChunkSize` param; `AutoRetryAfter`/`MaxAutoRetries`/`MetadataField`/`Resumable` missing)

#### MariloMultiSelect (Plan: 5 gaps, ✅ IMPLEMENTED → Actual: ⚠️ PARTIALLY IMPLEMENTED, ~12-15 gaps)

**What works:** Two-way `Value` binding (`List<TValue>`), `Data`/`TextField`/`ValueField`, `Filterable`/`FilterOperator`/`MinLength`/`DebounceDelay`/`PersistFilterOnSelect`, `OnFilter` event, `TagMode` (Single/Multiple), `MaxVisibleTags`, `ShowClearButton`/`ShowArrowButton`, `AutoClose`, keyboard nav (arrows/enter/space/escape/backspace/ctrl+A), `EnableVirtualization` + `<Virtualize>`, `ItemTemplate`, full WAI-ARIA, `Open()`/`Close()`/`Refresh()`, CSS provider (4 methods).

**Remaining gaps:**
- Events: `OnChange`, `OnRead`, `OnOpen` (cancellable), `OnClose` (cancellable), `OnItemRender`, `OnBlur` — all missing
- `AllowCustom` (freeform values) missing
- `GroupField` (sticky group headers) missing
- Templates: `SummaryTagTemplate`, `TagTemplate`, `HeaderTemplate`, `FooterTemplate`, `NoDataTemplate` — all missing
- `<MultiSelectSettings>`/`<MultiSelectPopupSettings>` child component API missing
- `AdaptiveMode`, `InputMode`, `LoaderShowDelay` parameters missing
- `Rebind()` method (triggers `OnRead`) missing
- `ValueMapper` (remote virtualization pre-selection) missing
- `ScrollMode`/`ItemHeight`/`PageSize` virtual scroll params missing
- `MaxVisibleTags` vs spec's `MaxAllowedTags` naming mismatch

### Summary: T4 Actual Status

| Component | Plan Status | Actual Status | Core UX | Spec API Coverage |
|-----------|------------|---------------|---------|-------------------|
| MariloColorPicker | ✅ IMPLEMENTED | ⚠️ PARTIAL | Solid | ~70% (standalone sub-components missing) |
| MariloDateRangePicker | ✅ IMPLEMENTED | ⚠️ PARTIAL | Solid | ~55% (events, multi-view, appearance missing) |
| MariloDateTimePicker | ✅ IMPLEMENTED | ⚠️ PARTIAL | Solid | ~60% (all spec events missing) |
| MariloTimePicker | ✅ IMPLEMENTED | ⚠️ PARTIAL | Solid | ~70% (event args, PopupClass bug) |
| MariloFileUpload | ✅ IMPLEMENTED | ⚠️ PARTIAL | Solid | ~80% (DropZoneId inert, template context types) |
| MariloUpload | ✅ IMPLEMENTED | ⚠️ PARTIAL | Solid | ~75% (templates, chunk resume, WithCredentials) |
| MariloMultiSelect | ✅ IMPLEMENTED | ⚠️ PARTIAL | Solid | ~50% (many events/templates/params missing) |

**Key takeaway:** All T4 components deliver functional core UX (the hard parts — canvases, tumblers, dual calendars, chunked uploads, filtering). The remaining work is primarily: (1) spec-aligned event signatures with cancellable args, (2) missing template slots, (3) `AdaptiveMode`/`ValidateOn` cross-cutting params, and (4) WAI-ARIA completeness.

---

## Phase 2.5 — Post-Reconstruction Fixes

Gaps discovered during Phase 2 pipeline reconstruction. Intake complete; resolution pending.

| Gap ID | Title | Severity | Scope | Status |
|--------|-------|----------|-------|--------|
| GAP-readonly-guards | ReadOnly parameter missing from ExpandOnClick and DragDrop interaction guards | Medium | single | 📋 Intake complete |
| GAP-expandall-lazyload | ExpandAllAsync does not trigger LoadChildrenAsync for unloaded nodes | High | single | 📋 Intake complete |

### GAP-readonly-guards

`HandleDrop()` and the DragDrop render guard (line 480) do not check `ReadOnly`. A `ReadOnly=true` tree with `EnableDragDrop=true` fires `OnItemDrop` callbacks — the only true state-mutation leak. Three additional cosmetic inconsistencies (ExpandOnClick handler attached, toggle button appears enabled, title click handler attached when ReadOnly). `HandleKeyDown` omission of ReadOnly is intentional (accessibility).

**Must fix:** Add `ReadOnly` guard to `HandleDrop()` and DragDrop handler attachment.
**Should fix:** Add `ReadOnly` to ExpandOnClick guard, toggle button `disabled` attr, title click guard.

**Intake record:** [`stages/01-intake/output/gap-readonly-guards-inventory.md`](../workspaces/gap-analysis-resolution/stages/01-intake/output/gap-readonly-guards-inventory.md)

### GAP-expandall-lazyload

`ExpandAllAsync()` calls `CollectAllIds(GetTree(), _expandedIds)` which only walks already-loaded nodes. For trees using `LoadChildrenAsync`, nodes with `HasChildren=true` but no fetched children produce `TreeNode(..., Children=[], HasChildren=true)`. `CollectAllIds` adds the parent ID but finds zero child IDs. No `LoadChildrenAsync` call is made. The tree renders with expanded parents showing no children and no loading indicator — **silent data loss**.

**Must fix:** `ExpandAllAsync` must detect and fetch unloaded lazy nodes before collecting IDs. Recommended: sequential `LoadChildrenAsync` calls with per-node loading indicators.
**Effort:** Medium — requires iterating unloaded nodes, calling async fetch, invalidating cache between levels.

**Intake record:** [`stages/01-intake/output/gap-expandall-lazyload-inventory.md`](../workspaces/gap-analysis-resolution/stages/01-intake/output/gap-expandall-lazyload-inventory.md)

---

## Pre-existing Regressions

13 pre-existing test failures exist in the suite (173 passing / 13 failing). None were introduced by the gap-analysis-resolution work. Full triage: [`stages/05-implement/output/gap-regression-triage.md`](../workspaces/gap-analysis-resolution/stages/05-implement/output/gap-regression-triage.md).

| Component | Failures | Root Cause | Fix Effort |
|-----------|:--------:|------------|:----------:|
| MariloWindow | 4 | Inline `eval()` JS interop not mocked in bUnit tests | Small — add `JSRuntimeMode.Loose` |
| MariloEditor | 7 | Inline `eval()` JS interop not mocked in bUnit tests | Small — add `JSRuntimeMode.Loose` |
| MariloMultiSelect | 2 | Popup-based dropdown restructured; test selectors find `div[role='listbox']` before popup opens | Small — open combobox trigger first |

---

## Test Coverage Status

Tracks test coverage for each gap slug that has a Stage 03 resolution record in the gap pipeline. A row is ✅ only when a `## Tests` section exists in the corresponding Stage 05 implementation log with confirmed passing tests.

| Gap ID | Title | Tests Written | Tests Passing | Notes |
|--------|-------|:-------------:|:-------------:|-------|
| GAP-themeprovider | Wrapper element, CSS variables, dark mode, RTL, async handler, doc | ✅ | ✅ | 14 tests in `Foundation/ThemeProviderTests.cs`, all passing (test-coverage-pass) |
| GAP-icon | Document IconFlip.Both, IconSize.ExtraLarge, IconThemeColor.Danger | ✅ | ✅ | Doc-only resolution; Stage 06 closure confirmed; enum values verified (test-coverage-pass) |
| GAP-stack | Spacing, Width/Height, two-axis alignment, orientation | ✅ | ✅ | 11 tests in `Foundation/StackTests.cs`, all passing; Stage 06 Resolved (test-coverage-pass) |
| GAP-grid | CSS Grid Layout with child components, spacing, alignment | ✅ | ✅ | 10 tests in `Foundation/GridLayoutTests.cs`, all passing; Stage 06 Resolved (test-coverage-pass) |
| GAP-form | Form EditContext, validation components, Field/Label enhancements | ✅ | ✅ | 20 tests in `Foundation/FormTests.cs`, all passing; Stage 06 Resolved with deferred gaps (test-coverage-pass) |
| GAP-treeview | TreeView/TreeItem — 22 gaps (tri-state, checked binding, multi-select, lazy load, keyboard, Phase 2+3) | ✅ (67 tests) | ✅ | Phase 1: 17 + Phase 2: 28 + Phase 3: 22 in `P2Enhancements/TreeViewTests.cs`; all passing (test-coverage-pass) |
| GAP-expand-onclick | ExpandOnClick / ExpandOnDoubleClick | ✅ (5 tests) | ✅ | Phase 2; Stage 06 Resolved (reconstructed pipeline) |
| GAP-single-expand | SingleExpand (accordion mode) | ✅ (3 tests) | ✅ | Phase 2; Stage 06 Resolved (reconstructed pipeline) |
| GAP-auto-expand | AutoExpand ancestors of selected items | ✅ (3 tests) | ✅ | Phase 2; Stage 06 Resolved (reconstructed pipeline) |
| GAP-batch-expand | ExpandAllAsync / CollapseAllAsync | ✅ (4 tests) | ✅ | Phase 2; Stage 06 Resolved (reconstructed pipeline) |
| GAP-filter | FilterFunc / Search with ancestor preservation | ✅ (4 tests) | ✅ | Phase 2; Stage 06 Resolved (reconstructed pipeline) |
| GAP-disabled | Disabled / ReadOnly state guards | ✅ (9 tests) | ✅ | Phase 2; Stage 06 Resolved; ReadOnly/ExpandOnClick interaction flagged for future review |
| GAP-programmatic-nav | SelectNodeAsync — expand ancestors, select, focus | ✅ (5 tests) | ✅ | Phase 3; Stage 06 Resolved (reconstructed pipeline) |
| GAP-item-context-menu | OnItemContextMenu event on right-click | ✅ (2 tests) | ✅ | Phase 3; Stage 06 Resolved; preventDefault ⚠️ code inspection only |
| GAP-checkbox-template | CheckboxTemplate RenderFragment<CheckboxContext> | ✅ (3 tests) | ✅ | Phase 3; Stage 06 Resolved; Disabled/OnChange ⚠️ code inspection only |
| GAP-node-editing | AllowEditing — inline rename via double-click/F2 | ✅ (12 tests) | ✅ | Phase 3; Stage 06 Resolved; ExpandOnDoubleClick suppression documented |

### Deferred / Partial Coverage

- GAP-form GAP-FIELD-002 (floating label animation) → Phase 2 — requires JS interop for focus tracking
- GAP-form GAP-LABEL-002 (floating label behavior) → Phase 2 — same as above
- GAP-form GAP-FORM-007/010 (form auto-generation from model annotations) → Phase 2 — complex feature
- GAP-form GAP-XCUT-001/002 (cross-cutting naming/FloatingLabel) → Phase 2 — design decision needed
- GAP-form GAP-FORM-020 (FormGroups) → Phase 3 — depends on FormItems
- GAP-form GAP-FORM-022/026 (FormItemsTemplate/AutoGeneratedItems) → Phase 4 — advanced template system
- GAP-form GAP-FIELD-005/006, GAP-LABEL-005 → Phase 4 — nice-to-have enhancements
- GAP-treeview Gap 18 (Virtualization) → Future iteration — requires flatten-and-virtualize architecture change
- GAP-disabled ReadOnly interaction with ExpandOnClick and EnableDragDrop → Future review — ReadOnly does not block these paths (only Disabled does); intentional or oversight TBD
