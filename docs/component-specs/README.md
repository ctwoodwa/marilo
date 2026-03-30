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
| [appbar](appbar/) | MariloAppBar | Implemented |
| [dialog](dialog/) | MariloDialog | Implemented |
| [drawer](drawer/) | MariloDrawer | Implemented |
| [gridlayout](gridlayout/) | MariloGrid / MariloRow / MariloColumn | Implemented |
| [panelbar](panelbar/) | MariloAccordion / MariloAccordionItem | Implemented |
| [splitter](splitter/) | MariloSplitter | Implemented |
| [stacklayout](stacklayout/) | MariloStack | Implemented |
| [tabstrip](tabstrip/) | MariloTabs / MariloTabPanel | Implemented |
| [tilelayout](tilelayout/) | — | Planned |
| [window](window/) | MariloDialog | Partial |
| [card](card/) | MariloCard | Implemented |
| [dockmanager](dockmanager/) | — | Planned |

### Navigation

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [breadcrumb](breadcrumb/) | MariloBreadcrumb | Implemented |
| [menu](menu/) | MariloMenu / MariloMenuItem | Implemented |
| [contextmenu](contextmenu/) | MariloContextMenu | Implemented |
| [pager](pager/) | MariloPagination | Implemented |
| [stepper](stepper/) | MariloStepper / MariloStep | Implemented |
| [toolbar](toolbar/) | MariloToolbar | Implemented |
| [treeview](treeview/) | MariloTreeView / MariloTreeItem | Implemented |
| [wizard](wizard/) | — | Planned |

### Buttons

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [button](button/) | MariloButton | Implemented |
| [buttongroup](buttongroup/) | MariloButtonGroup | Implemented |
| [dropdownbutton](dropdownbutton/) | MariloSplitButton | Partial |
| [floatingactionbutton](floatingactionbutton/) | MariloFab | Implemented |
| [splitbutton](splitbutton/) | MariloSplitButton | Implemented |
| [togglebutton](togglebutton/) | MariloToggleButton | Implemented |
| [chip](chip/) | MariloChip | Implemented |
| [chiplist](chiplist/) | MariloChipSet | Implemented |

### Forms & Inputs

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [autocomplete](autocomplete/) | MariloAutocomplete | Implemented |
| [checkbox](checkbox/) | MariloCheckbox | Implemented |
| [colorpicker](colorpicker/) | MariloColorPicker | Implemented |
| [colorgradient](colorgradient/) | MariloColorPicker | Partial |
| [colorpalette](colorpalette/) | MariloColorPicker | Partial |
| [flatcolorpicker](flatcolorpicker/) | MariloColorPicker | Partial |
| [combobox](combobox/) | MariloSelect | Partial |
| [dateinput](dateinput/) | MariloDatePicker | Partial |
| [datepicker](datepicker/) | MariloDatePicker | Implemented |
| [daterangepicker](daterangepicker/) | MariloDateRangePicker | Implemented |
| [datetimepicker](datetimepicker/) | MariloDateTimePicker | Implemented |
| [dropdownlist](dropdownlist/) | MariloSelect | Implemented |
| [dropdowntree](dropdowntree/) | — | Planned |
| [editor](editor/) | — | Planned |
| [fileselect](fileselect/) | MariloFileUpload | Partial |
| [filter](filter/) | — | Planned |
| [floatinglabel](floatinglabel/) | MariloLabel | Partial |
| [form](form/) | MariloForm / MariloField | Implemented |
| [listbox](listbox/) | MariloList | Partial |
| [maskedtextbox](maskedtextbox/) | MariloMaskedInput | Implemented |
| [multicolumncombobox](multicolumncombobox/) | — | Planned |
| [multiselect](multiselect/) | MariloSelect | Partial |
| [numerictextbox](numerictextbox/) | MariloNumericInput | Implemented |
| [radiogroup](radiogroup/) | MariloRadio | Implemented |
| [rangeslider](rangeslider/) | MariloSlider | Partial |
| [rating](rating/) | MariloRating | Implemented |
| [signature](signature/) | — | Planned |
| [slider](slider/) | MariloSlider | Implemented |
| [switch](switch/) | MariloSwitch | Implemented |
| [textarea](textarea/) | MariloTextArea | Implemented |
| [textbox](textbox/) | MariloTextField | Implemented |
| [timepicker](timepicker/) | MariloTimePicker | Implemented |
| [upload](upload/) | MariloFileUpload | Implemented |
| [validation](validation/) | MariloValidation | Implemented |

### Data Display

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [avatar](avatar/) | MariloAvatar | Implemented |
| [badge](badge/) | MariloBadge | Implemented |
| [grid](grid/) | MariloTable | Partial |
| [listview](listview/) | MariloList / MariloListItem | Implemented |
| [pivotgrid](pivotgrid/) | — | Planned |
| [spreadsheet](spreadsheet/) | — | Planned |
| [treelist](treelist/) | MariloTreeView | Partial |
| [tooltip](tooltip/) | MariloTooltip | Implemented |
| [popover](popover/) | MariloPopover | Implemented |

### Feedback & Notifications

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [notification](notification/) | MariloToast / MariloSnackbar | Implemented |
| [progressbar](progressbar/) | MariloProgressBar | Implemented |
| [chunkprogressbar](chunkprogressbar/) | MariloProgressBar | Partial |
| [loader](loader/) | MariloSpinner | Implemented |
| [loadercontainer](loadercontainer/) | MariloSpinner | Partial |
| [skeleton](skeleton/) | MariloSkeleton | Implemented |

### Charts & Gauges

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [chart](chart/) | — | Planned |
| [stockchart](stockchart/) | — | Planned |
| [gauges](gauges/) | — | Planned |
| [sankey](sankey/) | — | Planned |

### Scheduling

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [calendar](calendar/) | MariloDatePicker | Partial |
| [gantt](gantt/) | — | Planned |
| [scheduler](scheduler/) | — | Planned |

### Barcodes & Media

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [barcodes](barcodes/) | — | Planned |
| [carousel](carousel/) | MariloCarousel | Implemented |
| [map](map/) | — | Planned |
| [pdfviewer](pdfviewer/) | — | Planned |

### AI Components

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [aiprompt](aiprompt/) | — | Planned |
| [chat](chat/) | — | Planned |
| [inlineaiprompt](inlineaiprompt/) | — | Planned |
| [promptbox](promptbox/) | — | Planned |
| [smartpastebutton](smartpastebutton/) | — | Planned |
| [speechtotextbutton](speechtotextbutton/) | — | Planned |

### Utility & Infrastructure

| Spec Directory | Marilo Component | Status |
|---|---|---|
| [animationcontainer](animationcontainer/) | — | N/A |
| [diagram](diagram/) | — | Planned |
| [dropzone](dropzone/) | — | Planned |
| [mediaquery](mediaquery/) | — | N/A |
| [popup](popup/) | MariloPopover | Partial |
| [rootcomponent](rootcomponent/) | MariloThemeProvider | Partial |
