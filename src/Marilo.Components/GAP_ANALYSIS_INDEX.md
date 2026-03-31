# Blazor Component Gap Analysis Index

This document tracks the gap analysis status for each Marilo Blazor component against its documented API specification.

**Legend:**
- Status: `[ ]` = Not evaluated, `[x]` = Evaluated
- Spec Source: Primary documentation source(s) used for analysis
- Gap File: Path to the generated gap analysis document (relative to component folder)

---

## Buttons

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloButton | [x] | `docs/component-specs/button/`, `docfx/articles/components/button/` | `Buttons/GAP_ANALYSIS.md` |
| MariloButtonGroup | [x] | `docs/component-specs/buttongroup/` | `Buttons/GAP_ANALYSIS.md` |
| MariloChip | [x] | `docs/component-specs/chip/` | `Buttons/GAP_ANALYSIS.md` |
| MariloChipSet | [x] | `docs/component-specs/chiplist/` | `Buttons/GAP_ANALYSIS.md` |
| MariloFab | [x] | `docs/component-specs/floatingactionbutton/` | `Buttons/GAP_ANALYSIS.md` |
| MariloIconButton | [x] | `docs/component-specs/button/` (icon button section) | `Buttons/GAP_ANALYSIS.md` |
| MariloSegmentedControl | [x] | *(no direct spec found)* | `Buttons/GAP_ANALYSIS.md` |
| MariloSplitButton | [x] | `docs/component-specs/splitbutton/` | `Buttons/GAP_ANALYSIS.md` |
| MariloToggleButton | [x] | `docs/component-specs/togglebutton/` | `Buttons/GAP_ANALYSIS.md` |

## Charts

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloChart | [x] | `docs/component-specs/chart/` | `Charts/GAP_ANALYSIS.md` |
| MariloChartSeries | [x] | `docs/component-specs/chart/` (series sub-docs) | `Charts/GAP_ANALYSIS.md` |

## DataDisplay

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloAvatar | [x] | `docs/component-specs/avatar/` | `DataDisplay/GAP_ANALYSIS_PART1.md` |
| MariloBadge | [x] | `docs/component-specs/badge/` | `DataDisplay/GAP_ANALYSIS_PART1.md` |
| MariloCard | [x] | `docs/component-specs/card/`, `docfx/articles/components/card/` | `DataDisplay/GAP_ANALYSIS_PART1.md` |
| MariloCardActions | [x] | `docs/component-specs/card/` (sub-component) | `DataDisplay/GAP_ANALYSIS_PART1.md` |
| MariloCardBody | [x] | `docs/component-specs/card/` (sub-component) | `DataDisplay/GAP_ANALYSIS_PART1.md` |
| MariloCardHeader | [x] | `docs/component-specs/card/` (sub-component) | `DataDisplay/GAP_ANALYSIS_PART1.md` |
| MariloCarousel | [x] | `docs/component-specs/carousel/` | `DataDisplay/GAP_ANALYSIS_PART1.md` |
| MariloHighlighter | [x] | *(no direct spec found)* | `DataDisplay/GAP_ANALYSIS_PART1.md` |
| MariloImage | [x] | *(no direct spec found)* | `DataDisplay/GAP_ANALYSIS_PART1.md` |
| MariloList | [x] | `docs/component-specs/listbox/` | `DataDisplay/GAP_ANALYSIS_PART2.md` |
| MariloListItem | [x] | `docs/component-specs/listbox/` (sub-component) | `DataDisplay/GAP_ANALYSIS_PART2.md` |
| MariloListView | [x] | `docs/component-specs/listview/` | `DataDisplay/GAP_ANALYSIS_PART2.md` |
| MariloPopover | [x] | `docs/component-specs/popover/` | `DataDisplay/GAP_ANALYSIS_PART2.md` |
| MariloTable | [x] | *(no direct spec — see grid)* | `DataDisplay/GAP_ANALYSIS_PART2.md` |
| MariloTimeline | [x] | *(no direct spec found)* | `DataDisplay/GAP_ANALYSIS_PART2.md` |
| MariloTimelineItem | [x] | *(no direct spec found — sub-component of Timeline)* | `DataDisplay/GAP_ANALYSIS_PART2.md` |
| MariloTooltip | [x] | `docs/component-specs/tooltip/`, `docfx/articles/components/tooltip/` | `DataDisplay/GAP_ANALYSIS_PART2.md` |
| MariloTypography | [x] | *(no direct spec found)* | `DataDisplay/GAP_ANALYSIS_PART2.md` |

## DataGrid

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloDataGrid | [x] | `docs/component-specs/grid/` | `DataGrid/GAP_ANALYSIS.md` |
| MariloGridColumn | [x] | `docs/component-specs/grid/` (columns sub-docs) | `DataGrid/GAP_ANALYSIS.md` |
| MariloGridToolbar | [x] | `docs/component-specs/grid/` (toolbar sub-docs) | `DataGrid/GAP_ANALYSIS.md` |

## Editors

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloEditor | [x] | `docs/component-specs/editor/` | `Editors/GAP_ANALYSIS.md` |

## Feedback

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloAlert | [x] | `docs/component-specs/notification/`, `docfx/articles/components/alert/` | `Feedback/GAP_ANALYSIS_PART1.md` |
| MariloAlertStrip | [x] | `docs/component-specs/notification/` (variant) | `Feedback/GAP_ANALYSIS_PART1.md` |
| MariloCallout | [x] | *(no direct spec found)* | `Feedback/GAP_ANALYSIS_PART1.md` |
| MariloConfirmDialog | [x] | `docs/component-specs/dialog/` (confirm variant) | `Feedback/GAP_ANALYSIS_PART1.md` |
| MariloDataBanner | [x] | *(no direct spec found)* | `Feedback/GAP_ANALYSIS_PART1.md` |
| MariloDataToast | [x] | *(no direct spec found)* | `Feedback/GAP_ANALYSIS_PART1.md` |
| MariloDialog | [x] | `docs/component-specs/dialog/`, `docfx/articles/components/dialog/` | `Feedback/GAP_ANALYSIS_PART1.md` |
| MariloProgressBar | [x] | `docs/component-specs/progressbar/` | `Feedback/GAP_ANALYSIS_PART2.md` |
| MariloProgressCircle | [x] | `docs/component-specs/loader/` | `Feedback/GAP_ANALYSIS_PART2.md` |
| MariloSkeleton | [x] | `docs/component-specs/skeleton/` | `Feedback/GAP_ANALYSIS_PART2.md` |
| MariloSnackbar | [x] | *(no direct spec found)* | `Feedback/GAP_ANALYSIS_PART2.md` |
| MariloSnackbarHost | [x] | *(no direct spec found — sub-component of Snackbar)* | `Feedback/GAP_ANALYSIS_PART2.md` |
| MariloSpinner | [x] | `docs/component-specs/loader/` | `Feedback/GAP_ANALYSIS_PART2.md` |
| MariloToast | [x] | `docs/component-specs/notification/` | `Feedback/GAP_ANALYSIS_PART2.md` |

## Forms / Containers

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloField | [x] | `docs/component-specs/floatinglabel/` | `Forms/Containers/GAP_ANALYSIS.md` |
| MariloForm | [x] | `docs/component-specs/form/` | `Forms/Containers/GAP_ANALYSIS.md` |
| MariloLabel | [x] | `docs/component-specs/floatinglabel/` | `Forms/Containers/GAP_ANALYSIS.md` |
| MariloValidation | [x] | `docs/component-specs/validation/` | `Forms/Containers/GAP_ANALYSIS.md` |

## Forms / Inputs

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloAutocomplete | [x] | `docs/component-specs/autocomplete/` | `Forms/Inputs/GAP_ANALYSIS_PART1.md` |
| MariloCheckbox | [x] | `docs/component-specs/checkbox/` | `Forms/Inputs/GAP_ANALYSIS_PART1.md` |
| MariloColorPicker | [x] | `docs/component-specs/colorpicker/` | `Forms/Inputs/GAP_ANALYSIS_PART1.md` |
| MariloComboBox | [x] | `docs/component-specs/combobox/` | `Forms/Inputs/GAP_ANALYSIS_PART1.md` |
| MariloDatePicker | [x] | `docs/component-specs/datepicker/` | `Forms/Inputs/GAP_ANALYSIS_PART1.md` |
| MariloDateRangePicker | [x] | `docs/component-specs/daterangepicker/` | `Forms/Inputs/GAP_ANALYSIS_PART1.md` |
| MariloDateTimePicker | [x] | `docs/component-specs/datetimepicker/` | `Forms/Inputs/GAP_ANALYSIS_PART1.md` |
| MariloDropDownList | [x] | `docs/component-specs/dropdownlist/` | `Forms/Inputs/GAP_ANALYSIS_PART1.md` |
| MariloFileUpload | [x] | `docs/component-specs/fileselect/`, `docs/component-specs/upload/` | `Forms/Inputs/GAP_ANALYSIS_PART2.md` |
| MariloMaskedInput | [x] | `docs/component-specs/maskedtextbox/` | `Forms/Inputs/GAP_ANALYSIS_PART2.md` |
| MariloMultiSelect | [x] | `docs/component-specs/multiselect/` | `Forms/Inputs/GAP_ANALYSIS_PART2.md` |
| MariloNumericInput | [x] | `docs/component-specs/numerictextbox/` | `Forms/Inputs/GAP_ANALYSIS_PART2.md` |
| MariloRadio | [x] | `docs/component-specs/radiogroup/` | `Forms/Inputs/GAP_ANALYSIS_PART2.md` |
| MariloRangeSlider | [x] | `docs/component-specs/rangeslider/` | `Forms/Inputs/GAP_ANALYSIS_PART2.md` |
| MariloRating | [x] | `docs/component-specs/rating/` | `Forms/Inputs/GAP_ANALYSIS_PART2.md` |
| MariloSearchBox | [x] | `docfx/articles/components/search-box/` | `Forms/Inputs/GAP_ANALYSIS_PART2.md` |
| MariloSelect | [x] | `docfx/articles/components/select/` | `Forms/Inputs/GAP_ANALYSIS_PART3.md` |
| MariloSlider | [x] | `docs/component-specs/slider/` | `Forms/Inputs/GAP_ANALYSIS_PART3.md` |
| MariloSwitch | [x] | `docs/component-specs/switch/` | `Forms/Inputs/GAP_ANALYSIS_PART3.md` |
| MariloTextArea | [x] | `docs/component-specs/textarea/` | `Forms/Inputs/GAP_ANALYSIS_PART3.md` |
| MariloTextField | [x] | `docs/component-specs/textbox/`, `docfx/articles/components/text-field/` | `Forms/Inputs/GAP_ANALYSIS_PART3.md` |
| MariloTimePicker | [x] | `docs/component-specs/timepicker/` | `Forms/Inputs/GAP_ANALYSIS_PART3.md` |
| MariloUpload | [x] | `docs/component-specs/upload/` | `Forms/Inputs/GAP_ANALYSIS_PART3.md` |

## Layout

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloAccordion | [x] | `docs/component-specs/panelbar/` | `Layout/GAP_ANALYSIS_PART1.md` |
| MariloAccordionItem | [x] | `docs/component-specs/panelbar/` (sub-component) | `Layout/GAP_ANALYSIS_PART1.md` |
| MariloAppBar | [x] | `docs/component-specs/appbar/` | `Layout/GAP_ANALYSIS_PART1.md` |
| MariloColumn | [x] | `docs/component-specs/gridlayout/` (sub-component) | `Layout/GAP_ANALYSIS_PART1.md` |
| MariloContainer | [x] | *(no direct spec found)* | `Layout/GAP_ANALYSIS_PART1.md` |
| MariloDivider | [x] | *(no direct spec found)* | `Layout/GAP_ANALYSIS_PART1.md` |
| MariloDrawer | [x] | `docs/component-specs/drawer/` | `Layout/GAP_ANALYSIS_PART1.md` |
| MariloGrid | [x] | `docs/component-specs/gridlayout/` | `Layout/GAP_ANALYSIS_PART1.md` |
| MariloPanel | [x] | `docs/component-specs/panelbar/` | `Layout/GAP_ANALYSIS_PART2.md` |
| MariloRow | [x] | `docs/component-specs/gridlayout/` (sub-component) | `Layout/GAP_ANALYSIS_PART2.md` |
| MariloSplitter | [x] | `docs/component-specs/splitter/` | `Layout/GAP_ANALYSIS_PART2.md` |
| MariloStack | [x] | `docs/component-specs/stacklayout/` | `Layout/GAP_ANALYSIS_PART2.md` |
| MariloStep | [x] | `docs/component-specs/stepper/` (sub-component) | `Layout/GAP_ANALYSIS_PART2.md` |
| MariloStepper | [x] | `docs/component-specs/stepper/` | `Layout/GAP_ANALYSIS_PART2.md` |
| MariloTabStrip | [x] | `docs/component-specs/tabstrip/`, `docfx/articles/components/tabs/` | `Layout/GAP_ANALYSIS_PART2.md` |
| TabStripTab | [x] | `docs/component-specs/tabstrip/` (sub-component) | `Layout/GAP_ANALYSIS_PART2.md` |

## Navigation

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloBreadcrumb | [x] | `docs/component-specs/breadcrumb/` | `Navigation/GAP_ANALYSIS_PART1.md` |
| MariloBreadcrumbItem | [x] | `docs/component-specs/breadcrumb/` (sub-component) | `Navigation/GAP_ANALYSIS_PART1.md` |
| MariloContextMenu | [x] | `docs/component-specs/contextmenu/` | `Navigation/GAP_ANALYSIS_PART1.md` |
| MariloEnvironmentBadge | [x] | *(no direct spec found)* | `Navigation/GAP_ANALYSIS_PART1.md` |
| MariloMenu | [x] | `docs/component-specs/menu/` | `Navigation/GAP_ANALYSIS_PART1.md` |
| MariloMenuDivider | [x] | `docs/component-specs/menu/` (sub-component) | `Navigation/GAP_ANALYSIS_PART1.md` |
| MariloMenuItem | [x] | `docs/component-specs/menu/` (sub-component) | `Navigation/GAP_ANALYSIS_PART1.md` |
| MariloNavBar | [x] | *(no direct spec found)* | `Navigation/GAP_ANALYSIS_PART1.md` |
| MariloNavItem | [x] | *(no direct spec found — sub-component of NavMenu)* | `Navigation/GAP_ANALYSIS_PART1.md` |
| MariloNavMenu | [x] | *(no direct spec found)* | `Navigation/GAP_ANALYSIS_PART1.md` |
| MariloPagination | [x] | `docs/component-specs/pager/` | `Navigation/GAP_ANALYSIS_PART2.md` |
| MariloTimeRangeSelector | [x] | *(no direct spec found)* | `Navigation/GAP_ANALYSIS_PART2.md` |
| MariloToolbar | [x] | `docs/component-specs/toolbar/` | `Navigation/GAP_ANALYSIS_PART2.md` |
| MariloToolbarButton | [x] | `docs/component-specs/toolbar/` (sub-component) | `Navigation/GAP_ANALYSIS_PART2.md` |
| MariloToolbarGroup | [x] | `docs/component-specs/toolbar/` (sub-component) | `Navigation/GAP_ANALYSIS_PART2.md` |
| MariloToolbarSeparator | [x] | `docs/component-specs/toolbar/` (sub-component) | `Navigation/GAP_ANALYSIS_PART2.md` |
| MariloToolbarToggleButton | [x] | `docs/component-specs/toolbar/` (sub-component) | `Navigation/GAP_ANALYSIS_PART2.md` |
| MariloTreeItem | [x] | `docs/component-specs/treeview/` (sub-component) | `Navigation/GAP_ANALYSIS_PART2.md` |
| MariloTreeView | [x] | `docs/component-specs/treeview/` | `Navigation/GAP_ANALYSIS_PART2.md` |

## Overlays

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloWindow | [x] | `docs/component-specs/window/` | `Overlays/GAP_ANALYSIS.md` |

## Utility / Root

| Component | Status | Spec Source | Gap File |
|-----------|--------|------------|----------|
| MariloIcon | [x] | `docfx/articles/components/icon/` | `Utility/GAP_ANALYSIS.md` |
| MariloThemeProvider | [x] | `docfx/articles/theming/` | `MariloThemeProvider_GAP_ANALYSIS.md` |

---

## Summary Statistics

- **Total Components:** 87
- **Evaluated:** 87
- **Remaining:** 0
- **Components with no spec found:** ~19 (SegmentedControl, Highlighter, Image, Table, Timeline, TimelineItem, Typography, Callout, DataBanner, DataToast, Snackbar, SnackbarHost, Container, Divider, EnvironmentBadge, NavBar, NavItem, NavMenu, TimeRangeSelector)

## Gap Analysis Files

| Folder | File(s) |
|--------|---------|
| Buttons | `GAP_ANALYSIS.md` |
| Charts | `GAP_ANALYSIS.md` |
| DataDisplay | `GAP_ANALYSIS_PART1.md`, `GAP_ANALYSIS_PART2.md` |
| DataGrid | `GAP_ANALYSIS.md` |
| Editors | `GAP_ANALYSIS.md` |
| Feedback | `GAP_ANALYSIS_PART1.md`, `GAP_ANALYSIS_PART2.md` |
| Forms/Containers | `GAP_ANALYSIS.md` |
| Forms/Inputs | `GAP_ANALYSIS_PART1.md`, `GAP_ANALYSIS_PART2.md`, `GAP_ANALYSIS_PART3.md` |
| Layout | `GAP_ANALYSIS_PART1.md`, `GAP_ANALYSIS_PART2.md` |
| Navigation | `GAP_ANALYSIS_PART1.md`, `GAP_ANALYSIS_PART2.md` |
| Overlays | `GAP_ANALYSIS.md` |
| Utility | `GAP_ANALYSIS.md` |
| Root | `MariloThemeProvider_GAP_ANALYSIS.md` |

## Notes

- Components inheriting from `MariloComponentBase` share common parameters: `Class`, `Style`, and `AdditionalAttributes` (splatted). These base parameters are documented implicitly across all components.
- Sub-components (e.g., `MariloCardHeader`, `MariloMenuItem`) are analyzed in the context of their parent component's spec when a standalone spec does not exist.
- Spec sources include both `docs/component-specs/` (detailed reference specs) and `docfx/articles/components/` (user-facing DocFX docs). Both are used when available.
- Components marked *(no direct spec found)* may be custom additions not yet documented or may map to a spec under a different name.
