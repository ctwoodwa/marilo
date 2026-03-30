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

    public string StackClass(StackDirection direction, StackAlignment align) =>
        new CssClassBuilder()
            .AddClass("mar-stack")
            .AddClass($"mar-stack--{direction.ToString().ToLower()}")
            .AddClass($"mar-stack--{align.ToString().ToLower()}")
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

    public string TabsClass() => "mar-tabs";

    public string TabPanelClass(bool isActive) =>
        new CssClassBuilder()
            .AddClass("mar-tab-panel")
            .AddClass("mar-tab-panel--active", isActive)
            .Build();

    public string StepperClass() => "mar-stepper";

    public string StepClass(StepStatus status) =>
        new CssClassBuilder()
            .AddClass("mar-step")
            .AddClass($"mar-step--{status.ToString().ToLower()}")
            .Build();

    public string SplitterClass(StackDirection orientation) =>
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

    public string IconButtonClass(ButtonSize size) =>
        new CssClassBuilder()
            .AddClass("mar-icon-button")
            .AddClass($"mar-icon-button--{size.ToString().ToLower()}")
            .Build();

    public string ButtonGroupClass() => "mar-button-group";

    public string ToggleButtonClass(bool isActive) =>
        new CssClassBuilder()
            .AddClass("mar-toggle-button")
            .AddClass("mar-toggle-button--active", isActive)
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
    public string TextFieldClass(bool isInvalid, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("mar-textfield")
            .AddClass("mar-textfield--invalid", isInvalid)
            .AddClass("mar-textfield--disabled", isDisabled)
            .Build();

    public string TextAreaClass(bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("mar-textarea")
            .AddClass("mar-textarea--invalid", isInvalid)
            .Build();

    public string NumericInputClass() => "mar-numeric-input";

    public string SearchBoxClass() => "mar-searchbox";

    public string AutocompleteClass() => "mar-autocomplete";

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

    public string RatingClass() => "mar-rating";

    public string ColorPickerClass() => "mar-color-picker";

    public string DatePickerClass() => "mar-datepicker";

    public string TimePickerClass() => "mar-timepicker";

    public string FileUploadClass() => "mar-file-upload";

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

    public string DialogClass() => "mar-dialog";

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

    public string DataGridRowClass(bool isSelected, bool isStriped) =>
        new CssClassBuilder()
            .AddClass("mar-datagrid-row")
            .AddClass("mar-datagrid-row--selected", isSelected)
            .AddClass("mar-datagrid-row--striped", isStriped)
            .Build();

    public string DataGridCellClass() => "mar-datagrid-cell";

    public string DataGridPagerClass() => "mar-datagrid-pager";

    // Charts
    public string ChartContainerClass() => "mar-chart-container";

    public string GaugeClass() => "mar-gauge";

    // Scheduling
    public string CalendarClass() => "mar-calendar";

    public string SchedulerClass() => "mar-scheduler";

    // Overlays
    public string ModalClass(ModalSize size) =>
        new CssClassBuilder()
            .AddClass("mar-modal")
            .AddClass($"mar-modal--{size.ToString().ToLower()}")
            .Build();

    public string ModalOverlayClass() => "mar-modal-overlay";

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
}
