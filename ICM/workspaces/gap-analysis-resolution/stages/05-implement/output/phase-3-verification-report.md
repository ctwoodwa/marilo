# Phase 3 Rollout Verification Report

> **Generated:** 2026-04-11
> **Scope:** Rollout tasks R-01 through R-28 from `stages/04-remediation-plan/output/gap-consolidated-remediation-plan.md`
> **Method:** Read-only source inspection — no code changes made.

---

## Results Table

| Task ID | Rollout Name | Status | Notes |
|---------|-------------|--------|-------|
| R-01 | ROLL-WRAPPER-WIZARD | **VERIFIED** | `MariloWizardSteps.razor` exists; pure `@ChildContent` pass-through with no logic. Pattern matches `MariloSplitterPanes`. |
| R-02 | ROLL-WRAPPER-CHART | **VERIFIED** | Covered by Pilot (P-12/P-13). `ChartSeriesItems.razor` and `ChartCategoryAxes.razor` both exist as pass-through wrappers. |
| R-03 | ROLL-CSS-DRP | **PARTIAL** | `Size`, `Rounded`, `FillMode` parameters exist and `SizeClass`/`RoundedClass`/`FillModeClass` helpers are wired. However, the component calls `CssProvider.DatePickerClass()` (line 5) — NOT `CssProvider.DateRangePickerClass()`. The `DateRangePickerClass()` method exists on `IMariloCssProvider` and all providers but is unused by this component. Done criterion requires `DateRangePickerClass()` to be called. |
| R-04 | ROLL-CSS-DTP | **PARTIAL** | `ValidateOn` parameter exists (line 228). However, component calls `CssProvider.DatePickerClass()` (line 3) — NOT `CssProvider.DateTimePickerClass()`. The dedicated method exists in the interface and all providers but is not invoked. Done criterion requires `DateTimePickerClass()` to be called. |
| R-05 | ROLL-CSS-TP | **VERIFIED** | `CssProvider.TimePickerClass()` called at root div (line 4). `InputMode` parameter present (line 220). `ValidateOn` parameter present (line 223). OnChange-on-blur logic present in `OnInputBlur` (line 468). |
| R-06 | ROLL-CSS-FU | **VERIFIED** | `CssProvider.FileUploadClass()` called at root div (line 5). `FileUploadTemplateContext` wrapper type defined in `UploadModels.cs` (line 281) and used for both `FileTemplate` and `FileInfoTemplate` parameters. |
| R-07 | ROLL-GRID-CSS | **VERIFIED** | Dual-mode rendering present: `IsGridMode` computed property (line 69) gates between flex mode (returns `CombineStyles()`) and CSS Grid mode (`BuildGridStyles()` emits `display:grid` + all gap/template styles). All 7 layout parameters wired: `Columns`, `Rows`, `ColumnSpacing`, `RowSpacing`, `Width`, `HorizontalAlign`, `VerticalAlign`. `CssProvider.GridClass()` applied. |
| R-08 | ROLL-CASCADE-MSEL-SETTINGS | **VERIFIED** | `IMultiSelectSettingsSink` interface defined in `MultiSelectSettings.cs` (line 12) with `RegisterSettings`, `UnregisterSettings`, `RegisterPopupSettings`, `UnregisterPopupSettings`. `MultiSelectSettings` and `MultiSelectPopupSettings` both exist as `ComponentBase` + `IDisposable`, register in `OnInitialized`, unregister in `Dispose`. |
| R-09 | ROLL-CASCADE-CHART-SUBTITLE | **VERIFIED** | Covered by Pilot (P-13). `ChartSubtitle.razor` exists. |
| R-10 | ROLL-DG-VALIDATION | **VERIFIED** | `EditForm` with `DataAnnotationsValidator` + `ValidationSummary` present in popup body (razor lines 332-355). Save button is `type="submit"`, cancel is `type="button"`. |
| R-11 | ROLL-DG-AUTOGEN | **VERIFIED** | `GenerateColumnsFromModel()` at razor.cs line 412. Respects `[Display(AutoGenerateField=false)]` skip logic and `[Editable(false)]` sets column Editable=false (line 447). `[Display(Name)]` used for title (line ~431 via `GetOrder()`). |
| R-12 | ROLL-DG-AGGREGATES | **VERIFIED** | `GridGroupHeaderContext<TItem>` class in `GridEventArgs.cs` (line 127). `Sum`, `Average`, `Min`, `Max` all present (lines 148-163) with decimal/int overloads and generic Min/Max. |
| R-13 | ROLL-DG-EXPORT | **VERIFIED** | `OnBeforeExport` (line 243) and `OnAfterExport` (line 246) as `EventCallback<GridExportEventArgs>`. `ExportAllPages` parameter (line 249, default `true`). `OnBeforeExport` can cancel (checked in Data.cs). `ExportAllPages=false` branches present. |
| R-14 | ROLL-DG-CANCELTOKEN | **VERIFIED** | `GridReadEventArgs.CancellationToken` property at `GridEventArgs.cs` line 38. CTS cancellation logic wired in DataGrid data pipeline. |
| R-15 | ROLL-DG-STATE | **VERIFIED** | `SetStateAsync(GridState)` at razor.cs line 518. `GetState()` returns `ExpandedItems` (line 491, `HashSet<object>` from `_expandedDetailItems`). `AddFilter` and `ClearFilters` in `MariloDataGrid.Data.cs` (lines 749, 763). |
| R-16 | ROLL-DG-FROZEN | **VERIFIED** | `Locked` and `FrozenPosition` parameters on `MariloGridColumn.razor` (lines 59, 62). Sticky CSS via `position:sticky;{side}:{offset}px;z-index:{zIndex}` computed in `razor.cs` line 409. `mar-datagrid-col--locked` CSS class applied in header render (razor line 105). `FixedWidthProvider` computes offsets for Start/End frozen columns. All render zones feed through the column width/offset system. |
| R-17 | ROLL-DG-ROWDRAGDROP | **VERIFIED** | `RowDraggable` (razor.cs line 252) and `OnRowDrop` (line 255) parameters exist. `initRowDrag()` JS function in `MariloDataGrid.Interop.cs` (line 307), called when `options.rowDraggable` (line 159). `GridRowDropEventArgs<TItem>` has `DestinationIndex`, `DropPosition` (GridEventArgs.cs lines 195, 198). `IsCancelled` can prevent drop via `GridRowDropEventArgs`. |
| R-18 | ROLL-DG-CELL-SELECT | **VERIFIED** | `SelectionUnit` parameter (razor.cs line 110, `GridSelectionUnit` enum). `SelectedCells` (line 113) and `SelectedCellsChanged` (line 116) as `IEnumerable<GridCellReference<TItem>>`. `GridCellReference<TItem>` model exists in `GridCellReference.cs`. |
| R-19 | ROLL-DG-CHECKBOXLIST | **VERIFIED** | `GridFilterMode.CheckBoxList` enum value in `GridEnums.cs` (line 51). CheckBoxList rendering branch in `MariloDataGrid.razor` (line 155). Data support in `MariloDataGrid.Data.cs` (line 643 comment). |
| R-20 | ROLL-CHART-EVENTS | **VERIFIED** | `OnRender` as `EventCallback<ChartRenderEventArgs>` (razor line 121). Fires after render with chart dimensions (line 954+). `Transitions` typed as `bool?` (line 98). |
| R-21 | ROLL-CHART-TOOLTIP | **VERIFIED** | `ChartTooltip.razor` has `Template` as `RenderFragment<ChartTooltipContext>?` (line 19) and `Shared` parameter (line 22). `ChartTooltipContext` model present. |
| R-22 | ROLL-CHART-CSS | **PARTIAL** | `--mar-chart-series-{N}` CSS variables are emitted via `GetCssVariables()` (called at razor line 6) into the chart container `<div>` style attribute — NOT directly on the `<svg>` element. The done criterion says "emitted on SVG container element." Variables ARE present in rendered output and cascadable to child SVG via CSS inheritance, but the letter of the criterion (SVG element) is not met. External CSS can still override series colors via the container div. |
| R-23 | ROLL-CHART-TESTS | **VERIFIED** | `ChartTests.cs` contains 16 `[Fact]` or `[Theory]` methods (count: 16 ≥ 15). Exceeds the 15-test threshold. |
| R-24 | ROLL-EDITOR-VALIDATION | **VERIFIED** | `ValueExpression` parameter at razor line 131. `CascadedEditContext` cascading parameter (line 128). `NotifyFieldChanged` called on debounced change (lines 782, 872). `_fieldIdentifier` built from `ValueExpression` on init (line 225). |
| R-25 | ROLL-EDITOR-CUSTOM-TOOLS | **VERIFIED** | `CustomTools` as `IEnumerable<EditorCustomTool>?` parameter (line 179). `EditorCustomTool` model referenced. Custom tools iterated into toolbar items (line 992). |
| R-26 | ROLL-EDITOR-ADAPTIVE | **VERIFIED** | `Adaptive` parameter (line 182). `IResizeObserverService` injected (file line 4). `_overflowStartIndex` field (line 122). "More" button overflow logic present with `_overflowStartIndex >= 0` guard (lines 1000-1061). `ResizeObserverService` wired in `OnAfterRenderAsync` (line 753). |
| R-27 | ROLL-EDITOR-TABLE-RESIZE | **VERIFIED** | Table column/row resize drag handles implemented in JS IIFE starting at line 500 of `MariloEditor.razor`. Image resize handles implemented at line 586+. `startImageResize` function at line 641. `onInput` sync fires after resize. |
| R-28 | ROLL-FORM-VERIFY | **VERIFIED** | All 6 component files exist in `Forms/Containers/`: `MariloForm.razor`, `MariloValidationMessage.razor`, `MariloValidationSummary.razor`, `MariloValidationTooltip.razor`, `MariloField.razor`, `MariloLabel.razor`. `MariloForm` has `EditContext` and `Model` parameters, `OnValidSubmit` and `OnInvalidSubmit` callbacks, internal `EditContext` cascade, and `CurrentEditContext` public property. |

---

## Summary Counts

| Status | Count | Tasks |
|--------|-------|-------|
| **VERIFIED** | 24 | R-01, R-02, R-05, R-06, R-07, R-08, R-09, R-10, R-11, R-12, R-13, R-14, R-15, R-16, R-17, R-18, R-19, R-20, R-21, R-23, R-24, R-25, R-26, R-27, R-28 |
| **PARTIAL** | 3 | R-03, R-04, R-22 |
| **OPEN** | 0 | — |

> Note: The executive summary in the remediation plan flagged R-16 (frozen columns) and R-17 (row drag-drop) as expected OPEN items, but source inspection reveals both are fully implemented.

---

## PARTIAL Items — Detail and Remediation Path

### R-03 (ROLL-CSS-DRP) — `MariloDateRangePicker` CSS provider method mismatch

**File:** `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`, line 5

**Finding:** Component calls `CssProvider.DatePickerClass()`. The interface `IMariloCssProvider` exposes a dedicated `DateRangePickerClass()` method (defined at `src/Marilo.Core/Contracts/IMariloCssProvider.cs` line 90) and all three providers implement it. The method is never called by `MariloDateRangePicker`.

**Fix required:** Change line 5 from `CssProvider.DatePickerClass()` to `CssProvider.DateRangePickerClass()`. One-line fix; no provider contract change needed (method already exists).

---

### R-04 (ROLL-CSS-DTP) — `MariloDateTimePicker` CSS provider method mismatch

**File:** `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor`, line 3

**Finding:** Component calls `CssProvider.DatePickerClass()`. The interface exposes `DateTimePickerClass()` (`IMariloCssProvider.cs` line 92) and all three providers implement it. The method is never called by `MariloDateTimePicker`.

**Fix required:** Change line 3 from `CssProvider.DatePickerClass()` to `CssProvider.DateTimePickerClass()`. One-line fix; no provider contract change needed.

---

### R-22 (ROLL-CHART-CSS) — CSS variables on container div, not SVG element

**File:** `src/Marilo.Components/Charts/MariloChart.razor`, line 6

**Finding:** `--mar-chart-series-{N}` variables are emitted via `GetCssVariables()` into the `style` attribute of the outer `<div class="mar-chart-container">` element, not on the `<svg>` element. CSS inheritance means variables cascade into the SVG child, so external overrides work functionally. The done criterion specifies "emitted on SVG container element."

**Severity:** Low. Functional behavior matches intent (variables accessible to series rendering). The criterion wording is the only gap.

**Fix options:**
- A (spec side): Update done criterion to say "emitted on the chart root element" — no code change.
- B (source side): Move `GetCssVariables()` call to the `<svg>` element's style attribute — requires locating the SVG element in `MariloChart.razor`.

Path A is recommended — the functional requirement is met.

---

## Prioritized Action List

| Priority | Task | Action | File |
|----------|------|--------|------|
| 1 | R-03 | Call `DateRangePickerClass()` instead of `DatePickerClass()` | `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor:5` |
| 2 | R-04 | Call `DateTimePickerClass()` instead of `DatePickerClass()` | `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor:3` |
| 3 | R-22 | Update done criterion (spec-side) OR move CSS var emission to SVG element (source-side) | `stages/04-remediation-plan/output/gap-consolidated-remediation-plan.md` or `MariloChart.razor` |
