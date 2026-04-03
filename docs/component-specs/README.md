# Component API Specifications

This directory contains the authoritative API specifications for all Marilo components.
Each subdirectory corresponds to a component (or component group) and contains markdown
files describing parameters, events, templates, accessibility requirements, and usage patterns.

These specs drive component development — every Marilo component should implement the API
surface described in its corresponding spec directory.

## How to Use

- **Implementing a component**: Read the spec's `overview.md` first, then review
  `events.md`, `accessibility/`, and any feature-specific files.
- **Reviewing a PR**: Compare the component's parameters and behavior against its spec.
- **Adding a new component**: Create the spec directory first (or find the existing one),
  then implement to match.

## Component Index

> **Status legend**: Implemented | Partial | Planned | N/A (not applicable to Marilo)

### Layout

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [appbar](appbar/overview.md) | MariloAppBar | Implemented |
| [dialog](dialog/overview.md) | MariloDialog | Implemented |
| [drawer](drawer/overview.md) | MariloDrawer | Implemented |
| [gridlayout](gridlayout/overview.md) | MariloGrid / MariloRow / MariloColumn | Implemented |
| [panelbar](panelbar/overview.md) | MariloAccordion / MariloAccordionItem | Implemented |
| [splitter](splitter/overview.md) | MariloSplitter | Implemented |
| [stacklayout](stacklayout/overview.md) | MariloStack | Implemented |
| [tabstrip](tabstrip/overview.md) | MariloTabs / MariloTabPanel | Implemented |
| [tilelayout](tilelayout/overview.md) | — | Planned |
| [window](window/overview.md) | MariloDialog | Partial |
| [card](card/overview.md) | MariloCard | Implemented |
| [dockmanager](dockmanager/overview.md) | — | Planned |

### Navigation

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [breadcrumb](breadcrumb/overview.md) | MariloBreadcrumb | Implemented |
| [menu](menu/overview.md) | MariloMenu / MariloMenuItem | Implemented |
| [contextmenu](contextmenu/overview.md) | MariloContextMenu | Implemented |
| [pager](pager/overview.md) | MariloPagination | Implemented |
| [stepper](stepper/overview.md) | MariloStepper / MariloStep | Implemented |
| [toolbar](toolbar/overview.md) | MariloToolbar | Implemented |
| [treeview](treeview/overview.md) | MariloTreeView / MariloTreeItem | Implemented |
| [wizard](wizard/overview.md) | — | Planned |

### Buttons

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [button](button/overview.md) | MariloButton | Implemented |
| [buttongroup](buttongroup/overview.md) | MariloButtonGroup | Implemented |
| [dropdownbutton](dropdownbutton/overview.md) | MariloSplitButton | Partial |
| [floatingactionbutton](floatingactionbutton/overview.md) | MariloFab | Implemented |
| [splitbutton](splitbutton/overview.md) | MariloSplitButton | Implemented |
| [togglebutton](togglebutton/overview.md) | MariloToggleButton | Implemented |
| [chip](chip/overview.md) | MariloChip | Implemented |
| [chiplist](chiplist/overview.md) | MariloChipSet | Implemented |

### Forms & Inputs

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [autocomplete](autocomplete/overview.md) | MariloAutocomplete | Implemented |
| [checkbox](checkbox/overview.md) | MariloCheckbox | Implemented |
| [colorpicker](colorpicker/overview.md) | MariloColorPicker | Implemented |
| [colorgradient](colorgradient/overview.md) | MariloColorPicker | Partial |
| [colorpalette](colorpalette/overview.md) | MariloColorPicker | Partial |
| [flatcolorpicker](flatcolorpicker/overview.md) | MariloColorPicker | Partial |
| [combobox](combobox/overview.md) | MariloSelect | Partial |
| [dateinput](dateinput/overview.md) | MariloDatePicker | Partial |
| [datepicker](datepicker/overview.md) | MariloDatePicker | Implemented |
| [daterangepicker](daterangepicker/overview.md) | MariloDateRangePicker | Implemented |
| [datetimepicker](datetimepicker/overview.md) | MariloDateTimePicker | Implemented |
| [dropdownlist](dropdownlist/overview.md) | MariloSelect | Implemented |
| [dropdowntree](dropdowntree/overview.md) | — | Planned |
| [editor](editor/overview.md) | — | Planned |
| [fileselect](fileselect/overview.md) | MariloFileUpload | Partial |
| [filter](filter/overview.md) | — | Planned |
| [floatinglabel](floatinglabel/overview.md) | MariloLabel | Partial |
| [form](form/overview.md) | MariloForm / MariloField | Implemented |
| [listbox](listbox/overview.md) | MariloList | Partial |
| [maskedtextbox](maskedtextbox/overview.md) | MariloMaskedInput | Implemented |
| [multicolumncombobox](multicolumncombobox/overview.md) | — | Planned |
| [multiselect](multiselect/overview.md) | MariloSelect | Partial |
| [numerictextbox](numerictextbox/overview.md) | MariloNumericInput | Implemented |
| [radiogroup](radiogroup/overview.md) | MariloRadio | Implemented |
| [rangeslider](rangeslider/overview.md) | MariloSlider | Partial |
| [rating](rating/overview.md) | MariloRating | Implemented |
| [signature](signature/overview.md) | — | Planned |
| [slider](slider/overview.md) | MariloSlider | Implemented |
| [switch](switch/overview.md) | MariloSwitch | Implemented |
| [textarea](textarea/overview.md) | MariloTextArea | Implemented |
| [textbox](textbox/overview.md) | MariloTextField | Implemented |
| [timepicker](timepicker/overview.md) | MariloTimePicker | Implemented |
| [upload](upload/overview.md) | MariloFileUpload | Implemented |
| [validation](validation/overview.md) | MariloValidation | Implemented |

### Data Display

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [avatar](avatar/overview.md) | MariloAvatar | Implemented |
| [badge](badge/overview.md) | MariloBadge | Implemented |
| [grid](grid/overview.md) | MariloDataGrid | Implemented |
| [listview](listview/overview.md) | MariloList / MariloListItem | Implemented |
| [pivotgrid](pivotgrid/overview.md) | — | Planned |
| [spreadsheet](spreadsheet/overview.md) | — | Planned |
| [treelist](treelist/overview.md) | MariloTreeView | Partial |
| [tooltip](tooltip/overview.md) | MariloTooltip | Implemented |
| [popover](popover/overview.md) | MariloPopover | Implemented |

### Feedback & Notifications

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [notification](notification/overview.md) | MariloToast / MariloSnackbar | Implemented |
| [progressbar](progressbar/overview.md) | MariloProgressBar | Implemented |
| [chunkprogressbar](chunkprogressbar/overview.md) | MariloProgressBar | Partial |
| [loader](loader/overview.md) | MariloSpinner | Implemented |
| [loadercontainer](loadercontainer/overview.md) | MariloSpinner | Partial |
| [skeleton](skeleton/overview.md) | MariloSkeleton | Implemented |

### Charts & Gauges

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [chart](chart/overview.md) | — | Planned |
| [stockchart](stockchart/overview.md) | — | Planned |
| [gauges](gauges/overview.md) | — | Planned |
| [sankey](sankey/overview.md) | — | Planned |

### Scheduling

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [calendar](calendar/overview.md) | MariloDatePicker | Partial |
| [gantt](gantt/overview.md) | — | Planned |
| [scheduler](scheduler/overview.md) | — | Planned |

### Barcodes & Media

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [barcodes](barcodes/barcode/overview.md) | — | Planned |
| [carousel](carousel/overview.md) | MariloCarousel | Implemented |
| [map](map/overview.md) | — | Planned |
| [pdfviewer](pdfviewer/overview.md) | — | Planned |

### AI Components

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [aiprompt](aiprompt/overview.md) | — | Planned |
| [chat](chat/overview.md) | — | Planned |
| [inlineaiprompt](inlineaiprompt/overview.md) | — | Planned |
| [promptbox](promptbox/overview.md) | — | Planned |
| [smartpastebutton](smartpastebutton/overview.md) | — | Planned |
| [speechtotextbutton](speechtotextbutton/overview.md) | — | Planned |

### Utility & Infrastructure

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [animationcontainer](animationcontainer/overview.md) | — | N/A |
| [diagram](diagram/overview.md) | — | Planned |
| [dropzone](dropzone/overview.md) | — | Planned |
| [mediaquery](mediaquery/overview.md) | — | N/A |
| [popup](popup/overview.md) | MariloPopover | Partial |
| [rootcomponent](rootcomponent/overview.md) | MariloThemeProvider | Partial |
