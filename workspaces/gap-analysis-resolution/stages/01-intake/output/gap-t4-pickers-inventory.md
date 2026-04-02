# Gap Inventory: T4 Components (Pickers, Upload, MultiSelect)

> Source: `GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — T4 Component Audit (2026-04-02)`
> Imported: 2026-04-02
> Total gaps: 58 (0 Critical, 18 High, 28 Medium, 12 Low)

---

## Cross-Component Gaps (Systemic)

### GAP-T4X-001: AdaptiveMode parameter missing across T4 components

**Area:** ColorPicker, DateRangePicker, DateTimePicker, TimePicker, MultiSelect
**Severity:** Low
**Theme:** missing-adaptive-mode
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — Common Cross-Component Gaps

**Target behavior:** `AdaptiveMode` parameter controls responsive layout behavior (e.g., switching to full-screen on mobile devices).

**Current behavior:** Parameter absent from all T4 picker and select components.

**Impact:** Components do not adapt layout for small screens. Low severity since most Blazor apps target desktop.

**Recommended direction:** Add `AdaptiveMode` enum parameter to all affected components. Can be implemented as a batch cross-cutting change.

**Status:** Open

---

### GAP-T4X-002: ValidateOn parameter missing from picker components

**Area:** DateTimePicker, TimePicker
**Severity:** Medium
**Theme:** missing-validate-on
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — Common Cross-Component Gaps

**Target behavior:** `ValidateOn` parameter controls when EditContext validation triggers (e.g., on change, on blur, on submit).

**Current behavior:** No `ValidateOn` parameter. Components do not integrate with EditContext validation pipeline.

**Impact:** Pickers cannot participate in form validation flows. Medium severity — workaround is manual validation.

**Recommended direction:** Add `ValidateOn` enum parameter following pattern from MariloTextField/MariloDatePicker.

**Status:** Open

---

### GAP-T4X-003: Cancellable OnOpen/OnClose events missing

**Area:** DateRangePicker, DateTimePicker, MultiSelect
**Severity:** Medium
**Theme:** missing-cancellable-events
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — Common Cross-Component Gaps

**Target behavior:** `OnOpen` and `OnClose` events fire with cancellable event args (`IsCancelled` property). Consumer can prevent popup from opening/closing.

**Current behavior:** DateRangePicker and DateTimePicker have no OnOpen/OnClose events. MultiSelect has no events at all.

**Impact:** Consumer cannot intercept or prevent popup lifecycle transitions.

**Recommended direction:** Add `EventCallback<PopupEventArgs>` with `IsCancelled` property. Shared `PopupEventArgs` type can serve all components.

**Status:** Open

---

### GAP-T4X-004: ARIA combobox role missing on picker inputs

**Area:** DateRangePicker, DateTimePicker, TimePicker
**Severity:** Low
**Theme:** missing-aria-combobox
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — Common Cross-Component Gaps

**Target behavior:** Input elements should have `role="combobox"` with `aria-controls` and `aria-activedescendant` for popup association.

**Current behavior:** Input elements lack combobox ARIA pattern.

**Impact:** Screen readers cannot announce popup association. Low severity — components are still keyboard-navigable.

**Recommended direction:** Add `role="combobox"`, `aria-haspopup="dialog"`, `aria-controls` pointing to popup ID, and `aria-activedescendant` for active selection.

**Status:** Open

---

## MariloColorPicker

### GAP-CPICK-001: FlatColorPicker standalone component missing

**Area:** MariloColorPicker
**Severity:** High
**Theme:** missing-standalone-component
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloColorPicker audit

**Target behavior:** Spec documents `MariloFlatColorPicker` as a separate inline color picker component (no popup trigger, always visible).

**Current behavior:** Only `MariloColorPicker` (popup variant) exists. FlatColorPicker is entirely absent.

**Impact:** Consumers needing an inline color picker cannot use a standalone component; must build custom workaround.

**Recommended direction:** Extract gradient/palette rendering into a `MariloFlatColorPicker` component that reuses internal picker logic without popup wrapper.

**Status:** Open

---

### GAP-CPICK-002: ColorGradient standalone component missing

**Area:** MariloColorPicker
**Severity:** High
**Theme:** missing-standalone-component
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloColorPicker audit

**Target behavior:** Spec documents `MariloColorGradient` as a standalone HSV/HSL gradient selector component.

**Current behavior:** Gradient view is embedded inside ColorPicker but not available as a standalone component.

**Impact:** Consumers cannot use gradient selector independently.

**Recommended direction:** Extract gradient rendering into `MariloColorGradient` component with own parameters (Format, Formats, ShowOpacityEditor).

**Status:** Open

---

### GAP-CPICK-003: ColorPalette standalone component missing

**Area:** MariloColorPicker
**Severity:** High
**Theme:** missing-standalone-component
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloColorPicker audit

**Target behavior:** Spec documents `MariloColorPalette` as a standalone palette swatch component.

**Current behavior:** Palette view is embedded inside ColorPicker but not available as a standalone component.

**Impact:** Consumers cannot use palette selector independently.

**Recommended direction:** Extract palette rendering into `MariloColorPalette` component with own parameters (Columns, Colors, TileWidth, TileHeight).

**Status:** Open

---

### GAP-CPICK-004: ColorPickerViews child-tag API incomplete

**Area:** MariloColorPicker
**Severity:** Medium
**Theme:** missing-child-component-api
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloColorPicker audit

**Target behavior:** `ColorPickerGradientView` and `ColorPickerPaletteView` child components configure gradient Format/Formats/ShowOpacityEditor and palette Columns/Colors/TileWidth/TileHeight.

**Current behavior:** Child-tag syntax is a stub — component types don't exist. Views cannot be configured individually.

**Impact:** Consumers cannot customize individual picker views.

**Recommended direction:** Implement child components as `CascadingParameter` registrations following MariloGridColumn pattern.

**Status:** Open

---

### GAP-CPICK-005: Bootstrap/FluentUI CSS coverage minimal

**Area:** MariloColorPicker
**Severity:** Low
**Theme:** missing-css-provider
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloColorPicker audit

**Target behavior:** CSS providers should style popup, canvas, slider, preview, and palette tiles.

**Current behavior:** Only root class styled; internal elements use hardcoded BEM classes without provider delegation.

**Impact:** Theme switching doesn't fully apply to ColorPicker internals.

**Recommended direction:** Add CSS provider methods for picker sub-elements.

**Status:** Open

---

## MariloDateRangePicker

### GAP-DRP-001: Multi-view calendar navigation missing

**Area:** MariloDateRangePicker
**Severity:** High
**Theme:** missing-calendar-views
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateRangePicker audit

**Target behavior:** Calendar supports Year and Decade view navigation. Parameters: `BottomView`, `View`, `ViewChanged`, `OnCalendarCellRender`.

**Current behavior:** Only month-level navigation exists. No year/decade picker.

**Impact:** Users cannot quickly navigate to distant dates. Blocks spec parameters that depend on multi-view.

**Recommended direction:** Implement year grid and decade grid views with bidirectional navigation, following MariloDatePicker pattern if it exists.

**Status:** Open

---

### GAP-DRP-002: OnChange/OnOpen/OnClose events missing

**Area:** MariloDateRangePicker
**Severity:** High
**Theme:** missing-cancellable-events
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateRangePicker audit

**Target behavior:** `OnChange` fires with `DateRangePickerChangeEventArgs`. `OnOpen`/`OnClose` fire with cancellable args.

**Current behavior:** No events implemented.

**Impact:** Consumer cannot react to value changes or popup lifecycle.

**Recommended direction:** Add EventCallback parameters with appropriate event args classes.

**Status:** Open

---

### GAP-DRP-003: Appearance parameters missing

**Area:** MariloDateRangePicker
**Severity:** Medium
**Theme:** missing-appearance-params
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateRangePicker audit

**Target behavior:** `Size`, `Rounded`, `FillMode`, `DebounceDelay`, `Title` parameters control visual appearance.

**Current behavior:** None of these appearance parameters exist.

**Impact:** Component appearance cannot be customized to match other form inputs.

**Recommended direction:** Add string/enum parameters following the pattern used by other input components.

**Status:** Open

---

### GAP-DRP-004: FocusStartAsync/FocusEndAsync methods missing

**Area:** MariloDateRangePicker
**Severity:** Medium
**Theme:** missing-focus-method
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateRangePicker audit

**Target behavior:** Public `FocusStartAsync()`/`FocusEndAsync()` methods programmatically focus the start/end date inputs.

**Current behavior:** No focus methods.

**Impact:** Consumer cannot programmatically direct user attention to a specific date field.

**Recommended direction:** Add JS interop for element focus.

**Status:** Open

---

### GAP-DRP-005: HeaderTemplate and ShowWeekNumbers gaps

**Area:** MariloDateRangePicker
**Severity:** Medium
**Theme:** missing-template
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateRangePicker audit

**Target behavior:** `HeaderTemplate` RenderFragment for custom popup header. `ShowWeekNumbers` renders ISO week numbers.

**Current behavior:** `HeaderTemplate` missing. `ShowWeekNumbers` declared but never rendered.

**Impact:** Calendar popup header cannot be customized. Week numbers don't appear despite parameter existing.

**Recommended direction:** Implement HeaderTemplate slot. Wire ShowWeekNumbers to render week column.

**Status:** Open

---

### GAP-DRP-006: PopupClass wrapper-div bug

**Area:** MariloDateRangePicker
**Severity:** Medium
**Theme:** bug
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateRangePicker audit

**Target behavior:** `PopupClass` parameter adds CSS class to the popup container.

**Current behavior:** Both ternary branches emit empty string — PopupClass is never applied.

**Impact:** Consumer cannot style the popup container.

**Recommended direction:** Fix ternary expression to actually use PopupClass value.

**Status:** Open

---

### GAP-DRP-007: No dedicated CSS provider method

**Area:** MariloDateRangePicker
**Severity:** Low
**Theme:** missing-css-provider
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateRangePicker audit

**Target behavior:** Dedicated `DateRangePickerClass()` method in CSS provider.

**Current behavior:** Reuses `DatePickerClass()`.

**Impact:** Cannot style DateRangePicker differently from DatePicker via theme provider.

**Recommended direction:** Add `DateRangePickerClass()` to IMariloCssProvider and implementations.

**Status:** Open

---

## MariloDateTimePicker

### GAP-DTP-001: All spec events missing

**Area:** MariloDateTimePicker
**Severity:** High
**Theme:** missing-events
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateTimePicker audit

**Target behavior:** `OnChange`, `OnOpen` (cancellable), `OnClose` (cancellable), `OnBlur`, `OnCalendarCellRender` events.

**Current behavior:** Only bespoke `OnConfirm` event exists (not in spec). No spec events implemented.

**Impact:** Consumer cannot react to any lifecycle or value change events.

**Recommended direction:** Add EventCallback parameters. Replace or supplement `OnConfirm` with spec-aligned `OnChange`.

**Status:** Open

---

### GAP-DTP-002: DateTimePickerSteps child component missing

**Area:** MariloDateTimePicker
**Severity:** Medium
**Theme:** missing-child-component-api
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateTimePicker audit

**Target behavior:** `<DateTimePickerSteps>` child component configures tumbler increments (e.g., 15-minute steps).

**Current behavior:** Tumblers always increment by 1. No step configuration.

**Impact:** Users must scroll through every minute/second value.

**Recommended direction:** Add `DateTimePickerSteps` child component or flat step parameters (HourStep, MinuteStep, SecondStep).

**Status:** Open

---

### GAP-DTP-003: Input is readonly — no typed input support

**Area:** MariloDateTimePicker
**Severity:** Medium
**Theme:** missing-input-behavior
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateTimePicker audit

**Target behavior:** Spec allows typing date/time values directly into the input field.

**Current behavior:** Input has `readonly="true"`. Users must use popup.

**Impact:** Power users cannot type dates quickly.

**Recommended direction:** Remove readonly, add input parsing with format validation.

**Status:** Open

---

### GAP-DTP-004: No dedicated CSS provider method

**Area:** MariloDateTimePicker
**Severity:** Low
**Theme:** missing-css-provider
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloDateTimePicker audit

**Target behavior:** Dedicated `DateTimePickerClass()` in CSS provider.

**Current behavior:** Reuses `DatePickerClass()`.

**Impact:** Cannot style differently from DatePicker.

**Recommended direction:** Add to IMariloCssProvider.

**Status:** Open

---

## MariloTimePicker

### GAP-TP-001: OnOpen/OnClose not cancellable

**Area:** MariloTimePicker
**Severity:** Medium
**Theme:** missing-cancellable-events
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloTimePicker audit

**Target behavior:** `OnOpen`/`OnClose` fire with `IsCancelled` args allowing consumer to prevent transition.

**Current behavior:** Events fire but with no cancellation mechanism.

**Impact:** Consumer cannot prevent popup from opening/closing.

**Recommended direction:** Add `PopupEventArgs` with `IsCancelled` property.

**Status:** Open

---

### GAP-TP-002: InputMode parameter missing

**Area:** MariloTimePicker
**Severity:** Medium
**Theme:** missing-input-behavior
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloTimePicker audit

**Target behavior:** `InputMode` parameter controls whether time can be typed directly or only selected via popup.

**Current behavior:** No InputMode parameter.

**Impact:** No control over input method.

**Recommended direction:** Add enum parameter.

**Status:** Open

---

### GAP-TP-003: PopupClass declared but never applied (bug)

**Area:** MariloTimePicker
**Severity:** Medium
**Theme:** bug
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloTimePicker audit

**Target behavior:** `PopupClass` parameter applies CSS class to the popup container.

**Current behavior:** Parameter declared but never applied to popup div.

**Impact:** Consumer sets PopupClass but it has no effect.

**Recommended direction:** Apply PopupClass to popup container div in markup.

**Status:** Open

---

### GAP-TP-004: TimePickerSteps child component missing

**Area:** MariloTimePicker
**Severity:** Low
**Theme:** missing-child-component-api
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloTimePicker audit

**Target behavior:** `<TimePickerSteps>` child component as alternative API for step configuration.

**Current behavior:** Only flat HourStep/MinuteStep/SecondStep parameters. No child-tag API.

**Impact:** Minor API inconsistency with spec. Flat params are functional.

**Recommended direction:** Low priority — flat params are sufficient. Add child component only if other pickers adopt child-tag pattern.

**Status:** Open

---

### GAP-TP-005: ARIA listbox role missing on tumblers

**Area:** MariloTimePicker
**Severity:** Low
**Theme:** missing-aria-combobox
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloTimePicker audit

**Target behavior:** Tumbler columns should have `role="listbox"` with `role="option"` on each item.

**Current behavior:** Tumbler items have `role="option"` but parent lacks `role="listbox"`.

**Impact:** ARIA tree incomplete for screen readers.

**Recommended direction:** Add `role="listbox"` to tumbler container divs.

**Status:** Open

---

### GAP-TP-006: OnChange doesn't fire on blur

**Area:** MariloTimePicker
**Severity:** Medium
**Theme:** missing-event-behavior
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloTimePicker audit

**Target behavior:** Spec requires `OnChange` to fire when component loses focus (blur).

**Current behavior:** `OnChange` only fires on explicit value selection.

**Impact:** Form validation on blur doesn't trigger.

**Recommended direction:** Add `onblur` handler that fires `OnChange` if value changed.

**Status:** Open

---

### GAP-TP-007: TimePickerClass ignored by component

**Area:** MariloTimePicker
**Severity:** Low
**Theme:** missing-css-provider
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloTimePicker audit

**Target behavior:** Component uses `CssProvider.TimePickerClass()` for styling.

**Current behavior:** `TimePickerClass()` exists in providers but component uses hardcoded BEM classes.

**Impact:** Theme switching doesn't affect TimePicker styling.

**Recommended direction:** Wire component to use CssProvider method.

**Status:** Open

---

## MariloFileUpload

### GAP-FU-001: DropZoneId parameter inert

**Area:** MariloFileUpload
**Severity:** Medium
**Theme:** missing-js-interop
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloFileUpload audit

**Target behavior:** `DropZoneId` parameter identifies an external element as a drop zone for file drag-drop.

**Current behavior:** Parameter declared but no JS interop wires the external element.

**Impact:** Consumer sets DropZoneId but external drop zones don't work.

**Recommended direction:** Add JS interop module to register external drop zone elements.

**Status:** Open

---

### GAP-FU-002: Template context type mismatch

**Area:** MariloFileUpload
**Severity:** Medium
**Theme:** template-api-mismatch
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloFileUpload audit

**Target behavior:** `FileTemplate`/`FileInfoTemplate` context should be `FileTemplateContext` wrapper with file info + actions.

**Current behavior:** Templates pass raw `FileSelectFileInfo` instead of the spec's wrapper type.

**Impact:** Template consumers cannot access file actions (remove, retry) from within the template.

**Recommended direction:** Create `FileTemplateContext` wrapper class that includes both file info and action delegates.

**Status:** Open

---

### GAP-FU-003: Drop-zone CSS not delegated to provider

**Area:** MariloFileUpload
**Severity:** Low
**Theme:** missing-css-provider
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloFileUpload audit

**Target behavior:** Drop zone styling should use CSS provider methods.

**Current behavior:** Drop-zone CSS hardcoded inline.

**Impact:** Theme switching doesn't affect drop zone appearance.

**Recommended direction:** Add drop zone CSS provider methods.

**Status:** Open

---

## MariloUpload

### GAP-UPL-001: Template slots missing

**Area:** MariloUpload
**Severity:** High
**Theme:** missing-template
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloUpload audit

**Target behavior:** `SelectFilesButtonTemplate`, `FileTemplate`, `FileInfoTemplate` RenderFragment parameters for custom rendering.

**Current behavior:** Only `ChildContent` exists. No template customization.

**Impact:** Consumer cannot customize file list appearance or upload button.

**Recommended direction:** Add RenderFragment parameters following MariloFileUpload's template pattern.

**Status:** Open

---

### GAP-UPL-002: WithCredentials declared but inert

**Area:** MariloUpload
**Severity:** Medium
**Theme:** inert-parameter
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloUpload audit

**Target behavior:** `WithCredentials` parameter adds credentials to HTTP upload requests (for cross-origin auth).

**Current behavior:** Parameter declared but never applied to HttpClient requests.

**Impact:** Cross-origin uploads with authentication fail silently.

**Recommended direction:** Apply `WithCredentials` to `HttpRequestMessage` configuration.

**Status:** Open

---

### GAP-UPL-003: DropZoneId declared but inert

**Area:** MariloUpload
**Severity:** Medium
**Theme:** missing-js-interop
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloUpload audit

**Target behavior:** External drop zone element identified by CSS selector.

**Current behavior:** Parameter declared but not wired.

**Impact:** External drop zones don't work.

**Recommended direction:** Share JS interop module with MariloFileUpload's DropZoneId implementation.

**Status:** Open

---

### GAP-UPL-004: Chunk resume restarts from byte 0

**Area:** MariloUpload
**Severity:** High
**Theme:** bug
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloUpload audit

**Target behavior:** Resuming a paused chunked upload should continue from the last successfully uploaded byte offset.

**Current behavior:** Resume always restarts from byte 0, re-uploading previously sent chunks.

**Impact:** Large file uploads waste bandwidth and time on resume. Data integrity risk if server doesn't deduplicate.

**Recommended direction:** Track `_pausedByteOffset` per file and resume from that position.

**Status:** Open

---

### GAP-UPL-005: UploadChunkSettings child component missing

**Area:** MariloUpload
**Severity:** Medium
**Theme:** missing-child-component-api
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloUpload audit

**Target behavior:** `<UploadChunkSettings>` child component with `AutoRetryAfter`, `MaxAutoRetries`, `MetadataField`, `Resumable` parameters.

**Current behavior:** Only flat `ChunkSize` parameter. No retry or metadata configuration.

**Impact:** Consumer cannot configure chunk retry behavior or metadata.

**Recommended direction:** Add child component or flat parameters for retry configuration.

**Status:** Open

---

## MariloMultiSelect

### GAP-MSEL-001: Core events missing

**Area:** MariloMultiSelect
**Severity:** High
**Theme:** missing-events
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloMultiSelect audit

**Target behavior:** `OnChange`, `OnRead`, `OnOpen` (cancellable), `OnClose` (cancellable), `OnItemRender`, `OnBlur` events.

**Current behavior:** None of these events exist.

**Impact:** Consumer cannot react to any component lifecycle events. No server-side data support via OnRead.

**Recommended direction:** Add EventCallback parameters for all spec events.

**Status:** Open

---

### GAP-MSEL-002: AllowCustom parameter missing

**Area:** MariloMultiSelect
**Severity:** High
**Theme:** missing-parameter
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloMultiSelect audit

**Target behavior:** `AllowCustom` parameter lets users enter freeform values not in the dropdown list.

**Current behavior:** Parameter absent. Users can only select from provided data.

**Impact:** Common use case (tags, custom values) not supported.

**Recommended direction:** Add bool parameter, extend selection logic to accept non-data values.

**Status:** Open

---

### GAP-MSEL-003: GroupField parameter missing

**Area:** MariloMultiSelect
**Severity:** Medium
**Theme:** missing-parameter
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloMultiSelect audit

**Target behavior:** `GroupField` parameter enables sticky group headers in the dropdown.

**Current behavior:** No grouping support.

**Impact:** Large item lists cannot be organized visually.

**Recommended direction:** Add GroupField with group header rendering.

**Status:** Open

---

### GAP-MSEL-004: Template slots missing

**Area:** MariloMultiSelect
**Severity:** High
**Theme:** missing-template
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloMultiSelect audit

**Target behavior:** `SummaryTagTemplate`, `TagTemplate`, `HeaderTemplate`, `FooterTemplate`, `NoDataTemplate` RenderFragment parameters.

**Current behavior:** Only `ItemTemplate` exists. Five template slots missing.

**Impact:** Consumers cannot customize tags, header, footer, or empty state rendering.

**Recommended direction:** Add RenderFragment parameters following established template pattern.

**Status:** Open

---

### GAP-MSEL-005: MultiSelectSettings child component missing

**Area:** MariloMultiSelect
**Severity:** Medium
**Theme:** missing-child-component-api
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloMultiSelect audit

**Target behavior:** `<MultiSelectSettings>` and `<MultiSelectPopupSettings>` child components for advanced configuration.

**Current behavior:** No child component API.

**Impact:** Advanced popup configuration not available.

**Recommended direction:** Add child component with CascadingParameter pattern.

**Status:** Open

---

### GAP-MSEL-006: Rebind and ValueMapper methods missing

**Area:** MariloMultiSelect
**Severity:** Medium
**Theme:** missing-public-method
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloMultiSelect audit

**Target behavior:** `Rebind()` triggers `OnRead` re-fetch. `ValueMapper` resolves pre-selected values during remote virtualization.

**Current behavior:** Neither method exists.

**Impact:** Remote data scenarios cannot refresh data or pre-resolve selections.

**Recommended direction:** Add public methods matching TreeView's Rebind pattern.

**Status:** Open

---

### GAP-MSEL-007: Virtual scroll configuration parameters missing

**Area:** MariloMultiSelect
**Severity:** Medium
**Theme:** missing-parameter
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloMultiSelect audit

**Target behavior:** `ScrollMode`, `ItemHeight`, `PageSize` parameters for virtual scrolling configuration.

**Current behavior:** `EnableVirtualization` exists but no configuration parameters.

**Impact:** Virtual scrolling uses defaults only; cannot tune for variable item heights.

**Recommended direction:** Add configuration parameters.

**Status:** Open

---

### GAP-MSEL-008: MaxVisibleTags naming mismatch

**Area:** MariloMultiSelect
**Severity:** Low
**Theme:** naming-mismatch
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md §7 — MariloMultiSelect audit

**Target behavior:** Spec uses `MaxAllowedTags` parameter name.

**Current behavior:** Component uses `MaxVisibleTags`.

**Impact:** API surface doesn't match spec. Minor inconsistency — both names are descriptive.

**Recommended direction:** Low priority. Either rename to match spec or document the deviation.

**Status:** Open
