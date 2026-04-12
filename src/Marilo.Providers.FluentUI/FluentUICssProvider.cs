using Marilo.Core.Base;
using Marilo.Core.Contracts;
using Marilo.Core.Enums;

namespace Marilo.Providers.FluentUI;

public class FluentUICssProvider : IMariloCssProvider
{
    // Layout
    public string ContainerClass(string? size = null) =>
        new CssClassBuilder()
            .AddClass("mar-container")
            .AddClass($"mar-container--{size}", size != null)
            .Build();

    public string GridClass() => "mar-grid";

    public string RowClass() => "mar-row";

    public string ColumnClass(int? span = null, int? offset = null) =>
        new CssClassBuilder()
            .AddClass("mar-col")
            .AddClass($"mar-col--{span}", span.HasValue)
            .AddClass($"mar-col--offset-{offset}", offset.HasValue)
            .Build();

    public string StackClass(StackDirection orientation) =>
        new CssClassBuilder()
            .AddClass("mar-stack")
            .AddClass($"mar-stack--{orientation.ToString().ToLower()}")
            .Build();

    public string DividerClass(bool vertical) =>
        new CssClassBuilder()
            .AddClass("mar-divider")
            .AddClass("mar-divider--vertical", vertical)
            .Build();

    public string PanelClass() => "mar-panel";

    public string DrawerClass(DrawerPosition position, bool isOpen) =>
        new CssClassBuilder()
            .AddClass("mar-drawer")
            .AddClass($"mar-drawer--{position.ToString().ToLower()}")
            .AddClass("mar-drawer--open", isOpen)
            .Build();

    public string AppBarClass(AppBarPosition position) =>
        new CssClassBuilder()
            .AddClass("mar-appbar")
            .AddClass($"mar-appbar--{position.ToString().ToLower()}")
            .Build();

    public string AccordionClass() => "mar-accordion";

    public string AccordionItemClass(bool isExpanded) =>
        new CssClassBuilder()
            .AddClass("mar-accordion-item")
            .AddClass("mar-accordion-item--expanded", isExpanded)
            .Build();

    public string TabsClass(TabPosition position, TabAlignment alignment, TabSize size) =>
        new CssClassBuilder()
            .AddClass("mar-tabs")
            .AddClass($"mar-tabs--{position.ToString().ToLower()}", position != TabPosition.Top)
            .AddClass($"mar-tabs--align-{alignment.ToString().ToLower()}", alignment != TabAlignment.Start)
            .AddClass($"mar-tabs--{size.ToString().ToLower()}", size != TabSize.Medium)
            .Build();

    public string TabClass(bool isActive, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("mar-tab")
            .AddClass("mar-tab--active", isActive)
            .AddClass("mar-tab--disabled", isDisabled)
            .Build();

    public string TabPanelClass(bool isActive, bool persistContent) =>
        new CssClassBuilder()
            .AddClass("mar-tab-panel")
            .AddClass("mar-tab-panel--active", isActive)
            .AddClass("mar-tab-panel--hidden", persistContent && !isActive)
            .Build();

    public string StepperClass() => "mar-stepper";

    public string StepClass(StepStatus status) =>
        new CssClassBuilder()
            .AddClass("mar-step")
            .AddClass($"mar-step--{status.ToString().ToLower()}")
            .Build();

    public string SplitterClass(SplitterOrientation orientation) =>
        new CssClassBuilder()
            .AddClass("mar-splitter")
            .AddClass($"mar-splitter--{orientation.ToString().ToLower()}")
            .Build();

    public string DrawerOverlayClass() => "mar-drawer-overlay";

    public string ContextMenuClass() => "mar-context-menu";

    // Navigation
    public string NavBarClass() => "mar-navbar";

    public string NavMenuClass() => "mar-navmenu";

    public string NavItemClass(bool isActive) =>
        new CssClassBuilder()
            .AddClass("mar-navitem")
            .AddClass("mar-navitem--active", isActive)
            .Build();

    public string BreadcrumbClass() => "mar-breadcrumb";

    public string BreadcrumbItemClass() => "mar-breadcrumb-item";

    public string MenuClass() => "mar-menu";

    public string MenuItemClass(bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("mar-menu-item")
            .AddClass("mar-menu-item--disabled", isDisabled)
            .Build();

    public string MenuDividerClass() => "mar-menu-divider";

    public string PaginationClass() => "mar-pagination";

    public string PaginationItemClass(bool isActive) =>
        new CssClassBuilder()
            .AddClass("mar-pagination-item")
            .AddClass("mar-pagination-item--active", isActive)
            .Build();

    public string TreeViewClass() => "mar-treeview";

    public string TreeItemClass(bool isExpanded, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("mar-tree-item")
            .AddClass("mar-tree-item--expanded", isExpanded)
            .AddClass("mar-tree-item--selected", isSelected)
            .Build();

    public string ToolbarClass() => "mar-toolbar";

    public string EnvironmentBadgeClass(string env) =>
        new CssClassBuilder()
            .AddClass("mar-env-badge")
            .AddClass($"mar-env-badge--{env.ToLower()}")
            .Build();

    public string TimeRangeSelectorClass() => "mar-time-range-selector";


    public string ToolbarButtonClass(bool isDisabled = false) =>
        new CssClassBuilder()
            .AddClass("mar-toolbar-btn")
            .AddClass("mar-toolbar-btn--disabled", isDisabled)
            .Build();

    public string ToolbarToggleButtonClass(bool isActive, bool isDisabled = false) =>
        new CssClassBuilder()
            .AddClass("mar-toolbar-btn")
            .AddClass("mar-toolbar-btn--active", isActive)
            .AddClass("mar-toolbar-btn--disabled", isDisabled)
            .Build();

    public string ToolbarSeparatorClass() => "mar-toolbar-sep";

    public string ToolbarGroupClass() => "mar-toolbar-group";

    public string LinkClass() => "mar-link";

    // Buttons
    public string ButtonClass(ButtonVariant variant, ButtonSize size, bool isOutline, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("mar-button")
            .AddClass($"mar-button--{variant.ToString().ToLower()}")
            .AddClass($"mar-button--{size.ToString().ToLower()}")
            .AddClass("mar-button--outline", isOutline)
            .AddClass("mar-button--disabled", isDisabled)
            .Build();

    public string ButtonClass(ButtonVariant variant, ButtonSize size, FillMode fillMode, RoundedMode rounded, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("mar-button")
            .AddClass($"mar-button--{variant.ToString().ToLower()}")
            .AddClass($"mar-button--{size.ToString().ToLower()}")
            .AddClass($"mar-button--fill-{fillMode.ToString().ToLower()}", fillMode != FillMode.Solid)
            .AddClass($"mar-button--rounded-{rounded.ToString().ToLower()}", rounded != RoundedMode.Medium)
            .AddClass("mar-button--disabled", isDisabled)
            .Build();

    public string IconButtonClass(ButtonSize size) =>
        new CssClassBuilder()
            .AddClass("mar-icon-button")
            .AddClass($"mar-icon-button--{size.ToString().ToLower()}")
            .Build();

    public string ButtonGroupClass() => "mar-button-group";

    public string ToggleButtonClass(bool selected) =>
        new CssClassBuilder()
            .AddClass("mar-toggle-button")
            .AddClass("mar-toggle-button--active", selected)
            .Build();

    public string SplitButtonClass() => "mar-split-button";

    public string ChipClass(ChipVariant variant, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("mar-chip")
            .AddClass($"mar-chip--{variant.ToString().ToLower()}")
            .AddClass("mar-chip--selected", isSelected)
            .Build();

    public string ChipSetClass() => "mar-chip-set";

    public string FabClass(FabSize size) =>
        new CssClassBuilder()
            .AddClass("mar-fab")
            .AddClass($"mar-fab--{size.ToString().ToLower()}")
            .Build();

    // Forms - Inputs
    public string TextBoxClass(bool isInvalid, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("mar-textbox")
            .AddClass("mar-textbox--invalid", isInvalid)
            .AddClass("mar-textbox--disabled", isDisabled)
            .Build();

    public string TextAreaClass(bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("mar-textarea")
            .AddClass("mar-textarea--invalid", isInvalid)
            .Build();

    public string NumericInputClass() => "mar-numeric-input";

    public string SearchBoxClass() => "mar-search-box";

    public string AutocompleteClass() => "mar-autocomplete";

    public string AutocompleteClass(bool isOpen, bool isDisabled, bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("mar-autocomplete")
            .AddClass("mar-autocomplete--open", isOpen)
            .AddClass("mar-autocomplete--disabled", isDisabled)
            .AddClass("mar-autocomplete--invalid", isInvalid)
            .Build();

    public string AutocompleteItemClass(bool isHighlighted, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("mar-autocomplete-item")
            .AddClass("mar-autocomplete-item--highlighted", isHighlighted)
            .AddClass("mar-autocomplete-item--selected", isSelected)
            .Build();

    public string SelectClass(bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("mar-select")
            .AddClass("mar-select--invalid", isInvalid)
            .Build();

    public string CheckboxClass(bool isChecked) =>
        new CssClassBuilder()
            .AddClass("mar-checkbox")
            .AddClass("mar-checkbox--checked", isChecked)
            .Build();

    public string RadioClass(bool isSelected) =>
        new CssClassBuilder()
            .AddClass("mar-radio")
            .AddClass("mar-radio--selected", isSelected)
            .Build();

    public string RadioGroupClass() => "mar-radio-group";

    public string SwitchClass(bool isOn) =>
        new CssClassBuilder()
            .AddClass("mar-switch")
            .AddClass("mar-switch--on", isOn)
            .Build();

    public string SliderClass() => "mar-slider";

    public string SliderClass(SliderOrientation orientation) =>
        new CssClassBuilder()
            .AddClass("mar-slider")
            .AddClass($"mar-slider--{orientation.ToString().ToLower()}")
            .Build();

    public string RatingClass() => "mar-rating";

    public string ColorPickerClass() => "mar-color-picker";
    public string ColorPickerPopupClass() => "mar-color-picker__popup";
    public string ColorGradientClass() => "fui-colorgradient";
    public string ColorPaletteClass() => "fui-colorpalette";
    public string FlatColorPickerClass() => "fui-flatcolorpicker";

    public string DatePickerClass() => "mar-datepicker";

    public string TimePickerClass() => "mar-timepicker";
    public string TimePickerPopupClass() => "mar-timepicker__popup";

    public string DateRangePickerClass() => "mar-date-range-picker";

    public string DateRangePickerPopupClass() => "mar-date-range-picker__popup";

    public string DateTimePickerClass() => "mar-datetime-picker";

    public string DateTimePickerPopupClass() => "mar-datetime-picker__popup";

    public string FileUploadClass() => "mar-file-upload";

    public string FileUploadFileListClass() => "mar-file-upload-file-list";

    public string FileUploadDropZoneClass(bool isDragOver, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("mar-file-upload__zone")
            .AddClass("mar-file-upload__zone--dragover", isDragOver)
            .AddClass("mar-file-upload__zone--disabled", isDisabled)
            .Build();

    // Selection / Dropdowns
    public string DropDownListClass(bool isOpen, bool isDisabled, bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("mar-dropdownlist")
            .AddClass("mar-dropdownlist--open", isOpen)
            .AddClass("mar-dropdownlist--disabled", isDisabled)
            .AddClass("mar-dropdownlist--invalid", isInvalid)
            .Build();

    public string DropDownListPopupClass() => "mar-dropdownlist-popup";

    public string DropDownListItemClass(bool isHighlighted, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("mar-dropdownlist-item")
            .AddClass("mar-dropdownlist-item--highlighted", isHighlighted)
            .AddClass("mar-dropdownlist-item--selected", isSelected)
            .Build();

    public string ComboBoxClass(bool isOpen, bool isDisabled, bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("mar-combobox")
            .AddClass("mar-combobox--open", isOpen)
            .AddClass("mar-combobox--disabled", isDisabled)
            .AddClass("mar-combobox--invalid", isInvalid)
            .Build();

    public string ComboBoxPopupClass() => "mar-combobox-popup";

    public string ComboBoxItemClass(bool isHighlighted, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("mar-combobox-item")
            .AddClass("mar-combobox-item--highlighted", isHighlighted)
            .AddClass("mar-combobox-item--selected", isSelected)
            .Build();

    public string MultiSelectClass(bool isOpen, bool isDisabled, bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("mar-multiselect")
            .AddClass("mar-multiselect--open", isOpen)
            .AddClass("mar-multiselect--disabled", isDisabled)
            .AddClass("mar-multiselect--invalid", isInvalid)
            .Build();

    public string MultiSelectPopupClass() => "mar-multiselect-popup";

    public string MultiSelectItemClass(bool isHighlighted, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("mar-multiselect-item")
            .AddClass("mar-multiselect-item--highlighted", isHighlighted)
            .AddClass("mar-multiselect-item--selected", isSelected)
            .Build();

    public string MultiSelectTagClass() => "mar-multiselect-tag";

    public string DropdownPopupClass() => "mar-dropdown-popup";

    // Forms - Containers
    public string FormClass() => "mar-form";

    public string FieldClass() => "mar-field";

    public string LabelClass() => "mar-label";

    public string InputGroupClass() => "mar-input-group";

    public string ValidationMessageClass(ValidationSeverity severity) =>
        new CssClassBuilder()
            .AddClass("mar-validation-message")
            .AddClass($"mar-validation-message--{severity.ToString().ToLower()}")
            .Build();

    // Data Display
    public string CardClass() => "mar-card";

    public string CardHeaderClass() => "mar-card-header";

    public string CardBodyClass() => "mar-card-body";

    public string CardActionsClass() => "mar-card-actions";

    public string CardFooterClass() => "mar-card-footer";

    public string CardImageClass() => "mar-card-image";

    public string ListClass() => "mar-list";

    public string ListItemClass() => "mar-list-item";

    public string TableClass() => "mar-table";

    public string AvatarClass(AvatarSize size) =>
        new CssClassBuilder()
            .AddClass("mar-avatar")
            .AddClass($"mar-avatar--{size.ToString().ToLower()}")
            .Build();

    public string BadgeClass(BadgeVariant variant) =>
        new CssClassBuilder()
            .AddClass("mar-badge")
            .AddClass($"mar-badge--{variant.ToString().ToLower()}")
            .Build();

    public string TooltipClass(TooltipPosition position) =>
        new CssClassBuilder()
            .AddClass("mar-tooltip")
            .AddClass($"mar-tooltip--{position.ToString().ToLower()}")
            .Build();

    public string TooltipClass(TooltipPosition position, TooltipShowOn showOn) =>
        new CssClassBuilder()
            .AddClass("mar-tooltip")
            .AddClass($"mar-tooltip--{position.ToString().ToLower()}")
            .AddClass($"mar-tooltip--show-{showOn.ToString().ToLower()}", showOn != TooltipShowOn.Hover)
            .Build();

    public string PopoverClass() => "mar-popover";

    public string TimelineClass() => "mar-timeline";

    public string TimelineItemClass() => "mar-timeline-item";

    public string CarouselClass() => "mar-carousel";

    public string TypographyClass(TypographyVariant variant) =>
        new CssClassBuilder()
            .AddClass("mar-typography")
            .AddClass($"mar-typography--{variant.ToString().ToLower()}")
            .Build();

    // Feedback
    public string AlertClass(AlertSeverity severity) =>
        new CssClassBuilder()
            .AddClass("mar-alert")
            .AddClass($"mar-alert--{severity.ToString().ToLower()}")
            .Build();

    public string AlertStripClass() => "mar-alert-strip";

    public string ToastClass(ToastSeverity severity) =>
        new CssClassBuilder()
            .AddClass("mar-toast")
            .AddClass($"mar-toast--{severity.ToString().ToLower()}")
            .Build();

    public string SnackbarClass() => "mar-snackbar";

    public string SnackbarClass(NotificationVerticalPosition vertical, NotificationHorizontalPosition horizontal) =>
        new CssClassBuilder()
            .AddClass("mar-snackbar")
            .AddClass($"mar-snackbar--{vertical.ToString().ToLower()}")
            .AddClass($"mar-snackbar--{horizontal.ToString().ToLower()}")
            .Build();

    public string SnackbarHostClass() => "mar-snackbar-host";

    public string DialogClass() => "mar-dialog";

    public string DialogClass(bool isDraggable) =>
        new CssClassBuilder()
            .AddClass("mar-dialog")
            .AddClass("mar-dialog--draggable", isDraggable)
            .Build();

    public string DialogOverlayClass() => "mar-dialog-overlay";

    public string ProgressBarClass() => "mar-progress-bar";

    public string ProgressCircleClass() => "mar-progress-circle";

    public string SpinnerClass(SpinnerSize size) =>
        new CssClassBuilder()
            .AddClass("mar-spinner")
            .AddClass($"mar-spinner--{size.ToString().ToLower()}")
            .Build();

    public string SkeletonClass(SkeletonVariant variant) =>
        new CssClassBuilder()
            .AddClass("mar-skeleton")
            .AddClass($"mar-skeleton--{variant.ToString().ToLower()}")
            .Build();

    public string CalloutClass(CalloutType type) =>
        new CssClassBuilder()
            .AddClass("mar-callout")
            .AddClass($"mar-callout--{type.ToString().ToLower()}")
            .Build();

    // DataGrid
    public string DataGridClass() => "mar-datagrid";

    public string DataGridHeaderClass() => "mar-datagrid-header";

    public string DataGridHeaderCellClass(bool isSortable, bool isSorted) =>
        new CssClassBuilder()
            .AddClass("mar-datagrid-header-cell")
            .AddClass("mar-datagrid-header-cell--sortable", isSortable)
            .AddClass("mar-datagrid-header-cell--sorted", isSorted)
            .Build();

    public string DataGridRowClass(bool isSelected, bool isStriped) =>
        new CssClassBuilder()
            .AddClass("mar-datagrid-row")
            .AddClass("mar-datagrid-row--selected", isSelected)
            .AddClass("mar-datagrid-row--striped", isStriped)
            .Build();

    public string DataGridCellClass() => "mar-datagrid-cell";

    public string DataGridPagerClass() => "mar-datagrid-pager";

    public string DataGridToolbarClass() => "mar-datagrid-toolbar";

    public string DataGridFilterRowClass() => "mar-datagrid-filter-row";

    public string DataGridFilterCellClass() => "mar-datagrid-filter-cell";

    public string DataGridGroupHeaderClass() => "mar-datagrid-group-header";

    // DataSheet
    public string DataSheetClass(bool isLoading) =>
        new CssClassBuilder()
            .AddClass("mar-datasheet")
            .AddClass("mar-datasheet--loading", isLoading)
            .Build();

    public string DataSheetCellClass(CellState state, bool isActive, bool isEditable) =>
        new CssClassBuilder()
            .AddClass("mar-datasheet__cell")
            .AddClass("mar-datasheet__cell--active", isActive)
            .AddClass("mar-datasheet__cell--readonly", !isEditable)
            .AddClass("mar-datasheet__cell--dirty", state == CellState.Dirty)
            .AddClass("mar-datasheet__cell--invalid", state == CellState.Invalid)
            .AddClass("mar-datasheet__cell--saving", state == CellState.Saving)
            .AddClass("mar-datasheet__cell--saved", state == CellState.Saved)
            .Build();

    public string DataSheetHeaderCellClass(bool isSortable) =>
        new CssClassBuilder()
            .AddClass("mar-datasheet__header-cell")
            .AddClass("mar-datasheet__header-cell--sortable", isSortable)
            .Build();

    public string DataSheetRowClass(bool isDirty, bool isSelected, bool isDeleted) =>
        new CssClassBuilder()
            .AddClass("mar-datasheet__row")
            .AddClass("mar-datasheet__row--dirty", isDirty)
            .AddClass("mar-datasheet__row--selected", isSelected)
            .AddClass("mar-datasheet__row--deleted", isDeleted)
            .Build();

    public string DataSheetToolbarClass() => "mar-datasheet__toolbar";

    public string DataSheetBulkBarClass(bool isVisible) =>
        new CssClassBuilder()
            .AddClass("mar-datasheet__bulk-bar")
            .AddClass("mar-datasheet__bulk-bar--visible", isVisible)
            .Build();

    public string DataSheetSaveFooterClass(int dirtyCount) =>
        new CssClassBuilder()
            .AddClass("mar-datasheet__save-footer")
            .AddClass("mar-datasheet__save-footer--has-changes", dirtyCount > 0)
            .Build();

    public string DataSheetAddButtonClass() => "mar-datasheet__add-btn";
    public string DataSheetSaveButtonClass() => "mar-datasheet__save-btn";
    public string DataSheetResetButtonClass() => "mar-datasheet__reset-btn";
    public string DataSheetSpinnerClass() => "mar-datasheet__spinner";
    public string DataSheetDirtyBadgeClass() => "mar-datasheet__dirty-badge";
    public string DataSheetSkeletonClass() => "mar-datasheet__skeleton";
    public string DataSheetSkeletonRowClass() => "mar-datasheet__skeleton-row";
    public string DataSheetSkeletonCellClass() => "mar-datasheet__skeleton-cell";
    public string DataSheetLoadingTextClass() => "mar-datasheet__loading-text";
    public string DataSheetEmptyClass() => "mar-datasheet__empty";
    public string DataSheetSelectHeaderClass() => "mar-datasheet__select-header";
    public string DataSheetActionsHeaderClass() => "mar-datasheet__actions-header";
    public string DataSheetAriaLiveClass() => "mar-datasheet__aria-live";
    public string DataSheetSelectCellClass() => "mar-datasheet__select-cell";
    public string DataSheetActionsCellClass() => "mar-datasheet__actions-cell";
    public string DataSheetDeleteButtonClass() => "mar-datasheet__delete-btn";
    public string DataSheetCellTextClass() => "mar-datasheet__cell-text";
    public string DataSheetEditorInputClass() => "mar-datasheet__editor-input";
    public string DataSheetEditorSelectClass() => "mar-datasheet__editor-select";
    public string DataSheetContentClass() => "mar-datasheet__content";
    public string DataSheetScreenReaderOnlyClass() => "mar-datasheet__sr-only";

    // ListView
    public string ListViewClass() => "mar-listview";

    public string ListViewItemClass(bool isSelected) =>
        new CssClassBuilder()
            .AddClass("mar-listview-item")
            .AddClass("mar-listview-item--selected", isSelected)
            .Build();

    // Window
    public string WindowClass(bool isModal) =>
        new CssClassBuilder()
            .AddClass("mar-window")
            .AddClass("mar-window--modal", isModal)
            .Build();

    public string WindowTitleBarClass() => "mar-window-titlebar";

    public string WindowContentClass() => "mar-window-content";

    public string WindowActionsClass() => "mar-window-actions";

    public string WindowOverlayClass() => "mar-window-overlay";

    public string WindowFooterClass() => "mar-window-footer";

    // Editor
    public string EditorClass() => "mar-editor";

    public string EditorToolbarClass() => "mar-editor-toolbar";

    public string EditorContentClass() => "mar-editor-content";

    // Upload
    public string UploadClass() => "mar-upload";

    public string UploadFileListClass() => "mar-upload-file-list";

    public string UploadFileItemClass() => "mar-upload-file-item";

    public string UploadDropZoneClass(bool isActive) =>
        new CssClassBuilder()
            .AddClass("mar-upload-dropzone")
            .AddClass("mar-upload-dropzone--active", isActive)
            .Build();

    // Charts
    public string ChartContainerClass() => "mar-chart-container";

    public string GaugeClass() => "mar-gauge";

    // Scheduling
    public string CalendarClass() => "mar-calendar";

    public string SchedulerClass() => "mar-scheduler";

    // AllocationScheduler
    public string AllocationSchedulerClass() => "mar-allocation-scheduler";

    public string AllocationSchedulerToolbarClass() => "mar-allocation-scheduler__toolbar";

    public string AllocationSchedulerResourceColumnClass(bool isPinned) =>
        new CssClassBuilder()
            .AddClass("mar-allocation-scheduler__resource-col")
            .AddClass("mar-allocation-scheduler__resource-col--pinned", isPinned)
            .Build();

    public string AllocationSchedulerTimeHeaderClass(TimeGranularity grain) =>
        new CssClassBuilder()
            .AddClass("mar-allocation-scheduler__time-header")
            .AddClass($"mar-allocation-scheduler__time-header--{grain.ToString().ToLower()}")
            .Build();

    public string AllocationSchedulerRowClass(bool isSelected, bool isOverAllocated, bool isStriped = false) =>
        new CssClassBuilder()
            .AddClass("mar-allocation-scheduler__row")
            .AddClass("mar-allocation-scheduler__row--selected", isSelected)
            .AddClass("mar-allocation-scheduler__row--over-allocated", isOverAllocated)
            .AddClass("mar-allocation-scheduler__row--striped", isStriped)
            .Build();

    public string AllocationSchedulerCellClass(bool isEditable, bool isSelected, bool isConflict, bool isDisabled, bool isDragTarget) =>
        new CssClassBuilder()
            .AddClass("mar-allocation-scheduler__cell")
            .AddClass("mar-allocation-scheduler__cell--editable", isEditable)
            .AddClass("mar-allocation-scheduler__cell--selected", isSelected)
            .AddClass("mar-allocation-scheduler__cell--conflict", isConflict)
            .AddClass("mar-allocation-scheduler__cell--disabled", isDisabled)
            .AddClass("mar-allocation-scheduler__cell--drag-target", isDragTarget)
            .Build();

    public string AllocationSchedulerCellValueClass(AllocationValueMode mode) =>
        new CssClassBuilder()
            .AddClass("mar-allocation-scheduler__cell-value")
            .AddClass($"mar-allocation-scheduler__cell-value--{mode.ToString().ToLower()}")
            .Build();

    public string AllocationSchedulerDeltaClass(DeltaDisplayMode mode, bool isOver, bool isUnder) =>
        new CssClassBuilder()
            .AddClass("mar-allocation-scheduler__delta")
            .AddClass($"mar-allocation-scheduler__delta--{mode.ToString().ToLower()}")
            .AddClass("mar-allocation-scheduler__delta--over", isOver)
            .AddClass("mar-allocation-scheduler__delta--under", isUnder)
            .Build();

    public string AllocationSchedulerScenarioStripClass() => "mar-allocation-scheduler__scenario-strip";

    public string AllocationSchedulerScenarioChipClass(bool isActive, bool isLocked) =>
        new CssClassBuilder()
            .AddClass("mar-allocation-scheduler__scenario-chip")
            .AddClass("mar-allocation-scheduler__scenario-chip--active", isActive)
            .AddClass("mar-allocation-scheduler__scenario-chip--locked", isLocked)
            .Build();

    public string AllocationSchedulerGhostBarClass() => "mar-allocation-scheduler__ghost-bar";

    public string AllocationSchedulerContextMenuClass() => "mar-allocation-scheduler__context-menu";

    public string AllocationSchedulerEmptyClass() => "mar-allocation-scheduler__empty";

    public string AllocationSchedulerLoaderClass() => "mar-allocation-scheduler__loader";

    public string AllocationSchedulerSplitterClass(bool isDragging, bool isFocused) =>
        new CssClassBuilder()
            .AddClass("mar-allocation-scheduler__splitter")
            .AddClass("mar-allocation-scheduler__splitter--dragging", isDragging)
            .AddClass("mar-allocation-scheduler__splitter--focused", isFocused)
            .Build();

    public string AllocationSchedulerSplitterRestoreClass(SplitterSide collapsedSide) =>
        new CssClassBuilder()
            .AddClass("mar-allocation-scheduler__splitter-restore")
            .AddClass($"mar-allocation-scheduler__splitter-restore--{collapsedSide.ToString().ToLower()}")
            .Build();

    // Overlays
    public string ModalClass(ModalSize size) =>
        new CssClassBuilder()
            .AddClass("mar-modal")
            .AddClass($"mar-modal--{size.ToString().ToLower()}")
            .Build();

    public string ModalOverlayClass() => "mar-modal-overlay";

    // ResizableContainer
    public string ResizableContainerClass(bool isResizing, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("mar-resizable-container")
            .AddClass("mar-resizable-container--resizing", isResizing)
            .AddClass("mar-resizable-container--disabled", isDisabled)
            .Build();

    public string ResizableContainerContentClass() => "mar-resizable-container__content";

    public string ResizableContainerHandleClass(MariloResizeEdges edge, bool isActive, bool isFocused) =>
        new CssClassBuilder()
            .AddClass("mar-resizable-container__handle")
            .AddClass("mar-resizable-container__handle--right", edge == MariloResizeEdges.Right)
            .AddClass("mar-resizable-container__handle--bottom", edge == MariloResizeEdges.Bottom)
            .AddClass("mar-resizable-container__handle--bottom-right", edge == MariloResizeEdges.BottomRight)
            .AddClass("mar-resizable-container__handle--left", edge == MariloResizeEdges.Left)
            .AddClass("mar-resizable-container__handle--top", edge == MariloResizeEdges.Top)
            .AddClass("mar-resizable-container__handle--top-left", edge == MariloResizeEdges.TopLeft)
            .AddClass("mar-resizable-container__handle--top-right", edge == MariloResizeEdges.TopRight)
            .AddClass("mar-resizable-container__handle--bottom-left", edge == MariloResizeEdges.BottomLeft)
            .AddClass("mar-resizable-container__handle--active", isActive)
            .AddClass("mar-resizable-container__handle--focused", isFocused)
            .Build();

    // Utility
    public string IconClass(string iconName, IconSize size, IconFlip flip = IconFlip.None, IconThemeColor themeColor = IconThemeColor.Base) =>
        new CssClassBuilder()
            .AddClass("mar-icon")
            .AddClass($"mar-icon--{size.ToString().ToLower()}")
            .AddClass($"mar-icon--{iconName}")
            .AddClass($"mar-icon--flip-{flip.ToString().ToLower()}", flip != IconFlip.None)
            .AddClass($"mar-icon--{themeColor.ToString().ToLower()}", themeColor != IconThemeColor.Base)
            .Build();

    public string DragDropClass() => "mar-dragdrop";

    public string DropZoneClass(bool isActive) =>
        new CssClassBuilder()
            .AddClass("mar-dropzone")
            .AddClass("mar-dropzone--active", isActive)
            .Build();

    public string ScrollViewClass() => "mar-scrollview";

    // SignalR Status
    public string SignalRStatusClass(AggregateConnectionState state, bool isCompact) =>
        new CssClassBuilder()
            .AddClass("mar-signalr-status")
            .AddClass($"mar-signalr-status--{state.ToString().ToLower()}")
            .AddClass("mar-signalr-status--compact", isCompact)
            .Build();

    public string SignalRPopupClass() => "mar-signalr-popup";

    public string SignalRRowClass(ConnectionHealthState health) =>
        new CssClassBuilder()
            .AddClass("mar-signalr-row")
            .AddClass($"mar-signalr-row--{health.ToString().ToLower()}")
            .Build();

    public string SignalRBadgeClass(ConnectionHealthState health) =>
        new CssClassBuilder()
            .AddClass("mar-signalr-badge")
            .AddClass($"mar-signalr-badge--{health.ToString().ToLower()}")
            .Build();
}
