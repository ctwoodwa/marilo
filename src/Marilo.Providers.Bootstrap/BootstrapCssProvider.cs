using Marilo.Core.Base;
using Marilo.Core.Contracts;
using Marilo.Core.Enums;

namespace Marilo.Providers.Bootstrap;

/// <summary>
/// Maps Marilo component state to Bootstrap 5.3 CSS classes.
/// Uses native Bootstrap classes where a direct mapping exists,
/// and Marilo bridge classes (prefixed mar-bs-) where Bootstrap
/// has no equivalent component.
/// </summary>
public class BootstrapCssProvider : IMariloCssProvider
{
    // ───────────────────────────────────────────────
    // Layout
    // ───────────────────────────────────────────────

    public string ContainerClass(string? size = null) =>
        size switch
        {
            "sm" => "container-sm",
            "md" => "container-md",
            "lg" => "container-lg",
            "xl" => "container-xl",
            "xxl" => "container-xxl",
            "fluid" => "container-fluid",
            _ => "container"
        };

    public string GridClass() => "container-fluid mar-bs-grid";

    public string RowClass() => "row";

    public string ColumnClass(int? span = null, int? offset = null) =>
        new CssClassBuilder()
            .AddClass("col", !span.HasValue)
            .AddClass($"col-{span}", span.HasValue)
            .AddClass($"offset-{offset}", offset.HasValue)
            .Build();

    public string StackClass(StackDirection orientation) =>
        new CssClassBuilder()
            .AddClass("d-flex")
            .AddClass("flex-column", orientation == StackDirection.Vertical)
            .AddClass("flex-row", orientation == StackDirection.Horizontal)
            .Build();

    public string DividerClass(bool vertical) =>
        vertical ? "vr" : "mar-bs-divider border-bottom";

    public string PanelClass() => "card mar-bs-panel";

    public string DrawerClass(DrawerPosition position, bool isOpen) =>
        new CssClassBuilder()
            .AddClass("offcanvas")
            .AddClass("offcanvas-start", position == DrawerPosition.Left)
            .AddClass("offcanvas-end", position == DrawerPosition.Right)
            .AddClass("offcanvas-top", position == DrawerPosition.Top)
            .AddClass("offcanvas-bottom", position == DrawerPosition.Bottom)
            .AddClass("show", isOpen)
            .Build();

    public string AppBarClass(AppBarPosition position) =>
        new CssClassBuilder()
            .AddClass("navbar")
            .AddClass("navbar-expand-lg")
            .AddClass("fixed-top", position == AppBarPosition.Fixed)
            .AddClass("sticky-top", position == AppBarPosition.Sticky)
            .AddClass("fixed-bottom", position == AppBarPosition.Bottom)
            .Build();

    public string AccordionClass() => "accordion";

    public string AccordionItemClass(bool isExpanded) =>
        new CssClassBuilder()
            .AddClass("accordion-item")
            .AddClass("mar-bs-accordion-item--expanded", isExpanded)
            .Build();

    public string TabsClass(TabPosition position, TabAlignment alignment, TabSize size) =>
        new CssClassBuilder()
            .AddClass("nav nav-tabs")
            .AddClass("flex-column", position == TabPosition.Left || position == TabPosition.Right)
            .AddClass("justify-content-center", alignment == TabAlignment.Center)
            .AddClass("justify-content-end", alignment == TabAlignment.End)
            .AddClass("nav-fill", alignment == TabAlignment.Stretched)
            .AddClass("nav-justified", alignment == TabAlignment.Justify)
            .AddClass("nav-sm", size == TabSize.Small)
            .AddClass("nav-lg", size == TabSize.Large)
            .Build();

    public string TabClass(bool isActive, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("nav-link")
            .AddClass("active", isActive)
            .AddClass("disabled", isDisabled)
            .Build();

    public string TabPanelClass(bool isActive, bool persistContent) =>
        new CssClassBuilder()
            .AddClass("tab-pane")
            .AddClass("fade")
            .AddClass("show active", isActive)
            .AddClass("d-none", persistContent && !isActive)
            .Build();

    public string StepperClass() => "mar-bs-stepper d-flex";

    public string StepClass(StepStatus status) =>
        new CssClassBuilder()
            .AddClass("mar-bs-step")
            .AddClass("mar-bs-step--active", status == StepStatus.Active)
            .AddClass("mar-bs-step--completed", status == StepStatus.Completed)
            .AddClass("mar-bs-step--error text-danger", status == StepStatus.Error)
            .AddClass("text-body-secondary", status == StepStatus.Pending)
            .Build();

    public string SplitterClass(SplitterOrientation orientation) =>
        new CssClassBuilder()
            .AddClass("mar-bs-splitter d-flex")
            .AddClass("flex-row", orientation == SplitterOrientation.Horizontal)
            .AddClass("flex-column", orientation == SplitterOrientation.Vertical)
            .Build();

    public string DrawerOverlayClass() => "offcanvas-backdrop fade show";

    public string ContextMenuClass() => "dropdown-menu show mar-bs-context-menu";

    // ───────────────────────────────────────────────
    // Navigation
    // ───────────────────────────────────────────────

    public string NavBarClass() => "navbar navbar-expand-lg";

    public string NavMenuClass() => "navbar-nav";

    public string NavItemClass(bool isActive) =>
        new CssClassBuilder()
            .AddClass("nav-link")
            .AddClass("active", isActive)
            .Build();

    public string BreadcrumbClass() => "mar-bs-breadcrumb";

    public string BreadcrumbItemClass() => "breadcrumb-item";

    public string MenuClass() => "dropdown-menu show";

    public string MenuItemClass(bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("dropdown-item")
            .AddClass("disabled", isDisabled)
            .Build();

    public string MenuDividerClass() => "dropdown-divider";

    public string PaginationClass() => "pagination";

    public string PaginationItemClass(bool isActive) =>
        new CssClassBuilder()
            .AddClass("page-item")
            .AddClass("active", isActive)
            .Build();

    public string TreeViewClass() => "list-group mar-bs-treeview";

    public string TreeItemClass(bool isExpanded, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("list-group-item list-group-item-action")
            .AddClass("active", isSelected)
            .AddClass("mar-bs-tree-item--expanded", isExpanded)
            .Build();

    public string ToolbarClass() => "btn-toolbar";

    public string EnvironmentBadgeClass(string env) =>
        new CssClassBuilder()
            .AddClass("badge")
            .AddClass(env.ToLower() switch
            {
                "production" or "prod" => "bg-danger",
                "staging" or "stage" => "bg-warning",
                "development" or "dev" => "bg-info",
                "test" or "qa" => "bg-success",
                _ => "bg-secondary"
            })
            .Build();

    public string TimeRangeSelectorClass() => "mar-bs-time-range-selector d-inline-flex";

    public string ToolbarButtonClass(bool isDisabled = false) =>
        new CssClassBuilder()
            .AddClass("btn btn-outline-secondary btn-sm")
            .AddClass("disabled", isDisabled)
            .Build();

    public string ToolbarToggleButtonClass(bool isActive, bool isDisabled = false) =>
        new CssClassBuilder()
            .AddClass("btn btn-sm")
            .AddClass("btn-secondary", isActive)
            .AddClass("btn-outline-secondary", !isActive)
            .AddClass("disabled", isDisabled)
            .Build();

    public string ToolbarSeparatorClass() => "vr mx-1";

    public string ToolbarGroupClass() => "btn-group btn-group-sm";

    public string LinkClass() => "mar-bs-link";

    // ───────────────────────────────────────────────
    // Buttons
    // ───────────────────────────────────────────────

    private static string BootstrapVariant(ButtonVariant variant) =>
        variant switch
        {
            ButtonVariant.Primary => "primary",
            ButtonVariant.Secondary => "secondary",
            ButtonVariant.Danger => "danger",
            ButtonVariant.Warning => "warning",
            ButtonVariant.Info => "info",
            ButtonVariant.Success => "success",
            _ => "primary"
        };

    private static string BootstrapSize(ButtonSize size) =>
        size switch
        {
            ButtonSize.Small => "btn-sm",
            ButtonSize.Large => "btn-lg",
            _ => ""
        };

    public string ButtonClass(ButtonVariant variant, ButtonSize size, bool isOutline, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("btn")
            .AddClass($"btn-outline-{BootstrapVariant(variant)}", isOutline)
            .AddClass($"btn-{BootstrapVariant(variant)}", !isOutline)
            .AddClass(BootstrapSize(size), size != ButtonSize.Medium)
            .AddClass("disabled", isDisabled)
            .Build();

    public string ButtonClass(ButtonVariant variant, ButtonSize size, FillMode fillMode, RoundedMode rounded, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("btn")
            .AddClass($"btn-{BootstrapVariant(variant)}", fillMode == FillMode.Solid)
            .AddClass($"btn-outline-{BootstrapVariant(variant)}", fillMode == FillMode.Outline)
            .AddClass("btn-link", fillMode == FillMode.Link)
            .AddClass($"btn-light border-0 text-{BootstrapVariant(variant)}", fillMode == FillMode.Flat)
            .AddClass("btn-link text-decoration-none", fillMode == FillMode.Clear)
            .AddClass(BootstrapSize(size), size != ButtonSize.Medium)
            .AddClass("rounded-pill", rounded == RoundedMode.Full)
            .AddClass("rounded-1", rounded == RoundedMode.Small)
            .AddClass("rounded-3", rounded == RoundedMode.Large)
            .AddClass("disabled", isDisabled)
            .Build();

    public string IconButtonClass(ButtonSize size) =>
        new CssClassBuilder()
            .AddClass("btn btn-icon mar-bs-icon-button")
            .AddClass(BootstrapSize(size), size != ButtonSize.Medium)
            .Build();

    public string ButtonGroupClass() => "btn-group";

    public string ToggleButtonClass(bool selected) =>
        new CssClassBuilder()
            .AddClass("btn")
            .AddClass("btn-primary", selected)
            .AddClass("btn-outline-primary", !selected)
            .Build();

    public string SplitButtonClass() => "btn-group mar-bs-split-button";

    public string ChipClass(ChipVariant variant, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("badge rounded-pill mar-bs-chip")
            .AddClass(variant switch
            {
                ChipVariant.Primary => "bg-primary",
                ChipVariant.Secondary => "bg-secondary",
                ChipVariant.Danger => "bg-danger",
                ChipVariant.Warning => "bg-warning",
                ChipVariant.Info => "bg-info",
                ChipVariant.Success => "bg-success",
                _ => "bg-light border"
            })
            .AddClass("mar-bs-chip--selected", isSelected)
            .Build();

    public string ChipSetClass() => "d-flex flex-wrap gap-1 mar-bs-chip-set";

    public string FabClass(FabSize size) =>
        new CssClassBuilder()
            .AddClass("btn btn-primary rounded-circle mar-bs-fab")
            .AddClass("mar-bs-fab--small", size == FabSize.Small)
            .AddClass("mar-bs-fab--large", size == FabSize.Large)
            .Build();

    // ───────────────────────────────────────────────
    // Forms - Inputs
    // ───────────────────────────────────────────────

    public string TextBoxClass(bool isInvalid, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("input-group")
            .AddClass("mar-bs-textbox")
            .AddClass("is-invalid", isInvalid)
            .AddClass("mar-bs-textbox--disabled", isDisabled)
            .Build();

    public string TextAreaClass(bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("form-control")
            .AddClass("is-invalid", isInvalid)
            .Build();

    public string NumericInputClass() => "form-control mar-bs-numeric-input";

    public string SearchBoxClass() => "mar-search-box";

    public string AutocompleteClass() => "mar-bs-autocomplete";

    public string AutocompleteClass(bool isOpen, bool isDisabled, bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("mar-bs-autocomplete")
            .AddClass("mar-bs-autocomplete--open", isOpen)
            .AddClass("disabled", isDisabled)
            .AddClass("is-invalid", isInvalid)
            .Build();

    public string AutocompleteItemClass(bool isHighlighted, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("dropdown-item")
            .AddClass("active", isHighlighted || isSelected)
            .Build();

    public string SelectClass(bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("form-select")
            .AddClass("is-invalid", isInvalid)
            .Build();

    public string CheckboxClass(bool isChecked) =>
        new CssClassBuilder()
            .AddClass("form-check")
            .AddClass("mar-bs-checkbox--checked", isChecked)
            .Build();

    public string RadioClass(bool isSelected) =>
        new CssClassBuilder()
            .AddClass("form-check")
            .AddClass("mar-bs-radio--selected", isSelected)
            .Build();

    public string RadioGroupClass() => "mar-bs-radio-group";

    public string SwitchClass(bool isOn) =>
        new CssClassBuilder()
            .AddClass("form-check form-switch")
            .AddClass("mar-bs-switch--on", isOn)
            .Build();

    public string SliderClass() => "form-range";

    public string SliderClass(SliderOrientation orientation) =>
        new CssClassBuilder()
            .AddClass("form-range")
            .AddClass("mar-bs-slider--vertical", orientation == SliderOrientation.Vertical)
            .Build();

    public string RatingClass() => "mar-bs-rating d-inline-flex";

    public string ColorPickerClass() => "form-control form-control-color mar-bs-color-picker";
    public string ColorPickerPopupClass() => "dropdown-menu mar-bs-color-picker__popup p-2";

    public string DatePickerClass() => "form-control mar-bs-datepicker";

    public string TimePickerClass() => "form-control mar-bs-timepicker";
    public string TimePickerPopupClass() => "dropdown-menu mar-bs-timepicker__popup p-2";

    public string DateRangePickerClass() => "mar-date-range-picker";

    public string DateRangePickerPopupClass() => "mar-date-range-picker__popup";

    public string DateTimePickerClass() => "mar-datetime-picker";

    public string DateTimePickerPopupClass() => "mar-datetime-picker__popup";

    public string FileUploadClass() => "form-control mar-bs-file-upload";

    public string FileUploadFileListClass() => "list-group list-group-flush mar-bs-file-upload-list";

    public string FileUploadDropZoneClass(bool isDragOver, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("border border-2 rounded-3 p-4 text-center mar-bs-file-upload__zone")
            .AddClass("border-primary bg-body-secondary", isDragOver)
            .AddClass("border-secondary", !isDragOver)
            .AddClass("opacity-50 pe-none", isDisabled)
            .Build();

    // ───────────────────────────────────────────────
    // Selection / Dropdowns
    // ───────────────────────────────────────────────

    public string DropDownListClass(bool isOpen, bool isDisabled, bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("form-select mar-bs-dropdownlist")
            .AddClass("mar-bs-dropdownlist--open", isOpen)
            .AddClass("disabled", isDisabled)
            .AddClass("is-invalid", isInvalid)
            .Build();

    public string DropDownListPopupClass() => "dropdown-menu show";

    public string DropDownListItemClass(bool isHighlighted, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("dropdown-item")
            .AddClass("active", isSelected)
            .AddClass("mar-bs-item--highlighted", isHighlighted && !isSelected)
            .Build();

    public string ComboBoxClass(bool isOpen, bool isDisabled, bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("mar-bs-combobox")
            .AddClass("mar-bs-combobox--open", isOpen)
            .AddClass("disabled", isDisabled)
            .AddClass("is-invalid", isInvalid)
            .Build();

    public string ComboBoxPopupClass() => "dropdown-menu show";

    public string ComboBoxItemClass(bool isHighlighted, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("dropdown-item")
            .AddClass("active", isSelected)
            .AddClass("mar-bs-item--highlighted", isHighlighted && !isSelected)
            .Build();

    public string MultiSelectClass(bool isOpen, bool isDisabled, bool isInvalid) =>
        new CssClassBuilder()
            .AddClass("mar-bs-multiselect")
            .AddClass("mar-bs-multiselect--open", isOpen)
            .AddClass("disabled", isDisabled)
            .AddClass("is-invalid", isInvalid)
            .Build();

    public string MultiSelectPopupClass() => "dropdown-menu show";

    public string MultiSelectItemClass(bool isHighlighted, bool isSelected) =>
        new CssClassBuilder()
            .AddClass("dropdown-item")
            .AddClass("active", isSelected)
            .AddClass("mar-bs-item--highlighted", isHighlighted && !isSelected)
            .Build();

    public string MultiSelectTagClass() => "badge bg-primary me-1 mar-bs-multiselect-tag";

    public string DropdownPopupClass() => "dropdown-menu show";

    // ───────────────────────────────────────────────
    // Forms - Containers
    // ───────────────────────────────────────────────

    public string FormClass() => "mar-bs-form";

    public string FieldClass() => "mb-3";

    public string LabelClass() => "form-label";

    public string InputGroupClass() => "input-group";

    public string ValidationMessageClass(ValidationSeverity severity) =>
        new CssClassBuilder()
            .AddClass(severity switch
            {
                ValidationSeverity.Error => "invalid-feedback d-block",
                ValidationSeverity.Warning => "text-warning small",
                ValidationSeverity.Info => "form-text",
                _ => "invalid-feedback d-block"
            })
            .Build();

    // ───────────────────────────────────────────────
    // Data Display
    // ───────────────────────────────────────────────

    public string CardClass() => "card";

    public string CardHeaderClass() => "card-header";

    public string CardBodyClass() => "card-body";

    public string CardActionsClass() => "card-footer d-flex gap-2";

    public string CardFooterClass() => "card-footer";

    public string CardImageClass() => "card-img-top";

    public string ListClass() => "list-group";

    public string ListItemClass() => "list-group-item";

    public string TableClass() => "table";

    public string AvatarClass(AvatarSize size) =>
        new CssClassBuilder()
            .AddClass("rounded-circle mar-bs-avatar")
            .AddClass(size switch
            {
                AvatarSize.ExtraSmall => "mar-bs-avatar--xs",
                AvatarSize.Small => "mar-bs-avatar--sm",
                AvatarSize.Medium => "mar-bs-avatar--md",
                AvatarSize.Large => "mar-bs-avatar--lg",
                AvatarSize.ExtraLarge => "mar-bs-avatar--xl",
                _ => "mar-bs-avatar--md"
            })
            .Build();

    public string BadgeClass(BadgeVariant variant) =>
        new CssClassBuilder()
            .AddClass("badge")
            .AddClass(variant switch
            {
                BadgeVariant.Primary => "bg-primary",
                BadgeVariant.Secondary => "bg-secondary",
                BadgeVariant.Danger => "bg-danger",
                BadgeVariant.Warning => "bg-warning",
                BadgeVariant.Info => "bg-info",
                BadgeVariant.Success => "bg-success",
                _ => "bg-secondary"
            })
            .Build();

    public string TooltipClass(TooltipPosition position) =>
        new CssClassBuilder()
            .AddClass("tooltip bs-tooltip-auto mar-bs-tooltip")
            .AddClass($"bs-tooltip-{BootstrapTooltipPlacement(position)}")
            .Build();

    public string TooltipClass(TooltipPosition position, TooltipShowOn showOn) =>
        new CssClassBuilder()
            .AddClass("tooltip bs-tooltip-auto mar-bs-tooltip")
            .AddClass($"bs-tooltip-{BootstrapTooltipPlacement(position)}")
            .AddClass($"mar-bs-tooltip--show-{showOn.ToString().ToLower()}", showOn != TooltipShowOn.Hover)
            .Build();

    private static string BootstrapTooltipPlacement(TooltipPosition pos) =>
        pos switch
        {
            TooltipPosition.Top => "top",
            TooltipPosition.Bottom => "bottom",
            TooltipPosition.Left => "start",
            TooltipPosition.Right => "end",
            _ => "top"
        };

    public string PopoverClass() => "popover mar-bs-popover";

    public string TimelineClass() => "mar-bs-timeline";

    public string TimelineItemClass() => "mar-bs-timeline-item";

    public string CarouselClass() => "carousel slide";

    public string TypographyClass(TypographyVariant variant) =>
        variant switch
        {
            TypographyVariant.H1 => "h1",
            TypographyVariant.H2 => "h2",
            TypographyVariant.H3 => "h3",
            TypographyVariant.H4 => "h4",
            TypographyVariant.H5 => "h5",
            TypographyVariant.H6 => "h6",
            TypographyVariant.Subtitle1 => "lead",
            TypographyVariant.Subtitle2 => "fs-5",
            TypographyVariant.Body1 => "",
            TypographyVariant.Body2 => "small",
            TypographyVariant.Caption => "small text-body-secondary",
            TypographyVariant.Overline => "text-uppercase small text-body-secondary",
            _ => ""
        };

    // ───────────────────────────────────────────────
    // Feedback
    // ───────────────────────────────────────────────

    private static string BootstrapAlertVariant(AlertSeverity sev) =>
        sev switch
        {
            AlertSeverity.Info => "info",
            AlertSeverity.Warning => "warning",
            AlertSeverity.Critical => "danger",
            AlertSeverity.Success => "success",
            _ => "info"
        };

    public string AlertClass(AlertSeverity severity) =>
        $"alert alert-{BootstrapAlertVariant(severity)}";

    public string AlertStripClass() => "alert mar-bs-alert-strip";

    public string ToastClass(ToastSeverity severity) =>
        new CssClassBuilder()
            .AddClass("toast show")
            .AddClass(severity switch
            {
                ToastSeverity.Info => "border-info",
                ToastSeverity.Warning => "border-warning",
                ToastSeverity.Error => "border-danger",
                ToastSeverity.Success => "border-success",
                _ => "border-info"
            })
            .Build();

    public string SnackbarClass() => "toast show mar-bs-snackbar";

    public string SnackbarClass(NotificationVerticalPosition vertical, NotificationHorizontalPosition horizontal) =>
        new CssClassBuilder()
            .AddClass("toast show mar-bs-snackbar")
            .AddClass(vertical == NotificationVerticalPosition.Top ? "mar-bs-snackbar--top" : "mar-bs-snackbar--bottom")
            .AddClass(horizontal switch
            {
                NotificationHorizontalPosition.Left => "mar-bs-snackbar--start",
                NotificationHorizontalPosition.Center => "mar-bs-snackbar--center",
                NotificationHorizontalPosition.Right => "mar-bs-snackbar--end",
                _ => "mar-bs-snackbar--end"
            })
            .Build();

    public string SnackbarHostClass() => "toast-container position-fixed p-3";

    public string DialogClass() => "modal-dialog";

    public string DialogClass(bool isDraggable) =>
        new CssClassBuilder()
            .AddClass("modal-dialog")
            .AddClass("mar-bs-dialog--draggable", isDraggable)
            .Build();

    public string DialogOverlayClass() => "modal-backdrop fade show";

    public string ProgressBarClass() => "progress";

    public string ProgressCircleClass() => "mar-bs-progress-circle";

    public string SpinnerClass(SpinnerSize size) =>
        new CssClassBuilder()
            .AddClass("spinner-border")
            .AddClass("spinner-border-sm", size == SpinnerSize.Small)
            .AddClass("mar-bs-spinner--lg", size == SpinnerSize.Large)
            .Build();

    public string SkeletonClass(SkeletonVariant variant) =>
        new CssClassBuilder()
            .AddClass("placeholder-glow mar-bs-skeleton")
            .AddClass(variant switch
            {
                SkeletonVariant.Text => "mar-bs-skeleton--text",
                SkeletonVariant.Rectangular => "mar-bs-skeleton--rect",
                SkeletonVariant.Circular => "rounded-circle mar-bs-skeleton--circle",
                SkeletonVariant.Rounded => "rounded mar-bs-skeleton--rounded",
                _ => "mar-bs-skeleton--text"
            })
            .Build();

    public string CalloutClass(CalloutType type) =>
        new CssClassBuilder()
            .AddClass("mar-bs-callout")
            .AddClass(type switch
            {
                CalloutType.Info => "mar-bs-callout--info",
                CalloutType.Warning => "mar-bs-callout--warning",
                CalloutType.Danger => "mar-bs-callout--danger",
                CalloutType.Success => "mar-bs-callout--success",
                CalloutType.Note => "mar-bs-callout--note",
                _ => "mar-bs-callout--info"
            })
            .Build();

    // ───────────────────────────────────────────────
    // DataGrid
    // ───────────────────────────────────────────────

    public string DataGridClass() => "table-responsive mar-bs-datagrid";

    public string DataGridHeaderClass() => "mar-bs-datagrid-header";

    public string DataGridHeaderCellClass(bool isSortable, bool isSorted) =>
        new CssClassBuilder()
            .AddClass("mar-bs-datagrid-header-cell")
            .AddClass("mar-bs-datagrid-header-cell--sortable", isSortable)
            .AddClass("mar-bs-datagrid-header-cell--sorted", isSorted)
            .Build();

    public string DataGridRowClass(bool isSelected, bool isStriped) =>
        new CssClassBuilder()
            .AddClass("mar-bs-datagrid-row")
            .AddClass("table-active", isSelected)
            .AddClass("mar-bs-datagrid-row--striped", isStriped)
            .Build();

    public string DataGridCellClass() => "mar-bs-datagrid-cell";

    public string DataGridPagerClass() => "d-flex justify-content-between align-items-center mar-bs-datagrid-pager";

    public string DataGridToolbarClass() => "d-flex gap-2 mb-2 mar-bs-datagrid-toolbar";

    public string DataGridFilterRowClass() => "mar-bs-datagrid-filter-row";

    public string DataGridFilterCellClass() => "mar-bs-datagrid-filter-cell";

    public string DataGridGroupHeaderClass() => "table-secondary fw-bold mar-bs-datagrid-group-header";

    // ───────────────────────────────────────────────
    // DataSheet
    // ───────────────────────────────────────────────

    public string DataSheetClass(bool isLoading) =>
        new CssClassBuilder()
            .AddClass("table-responsive mar-bs-datasheet")
            .AddClass("mar-bs-datasheet--loading", isLoading)
            .Build();

    public string DataSheetCellClass(CellState state, bool isActive, bool isEditable) =>
        new CssClassBuilder()
            .AddClass("mar-bs-datasheet__cell")
            .AddClass("table-primary", isActive)
            .AddClass("mar-bs-datasheet__cell--readonly", !isEditable)
            .AddClass("table-warning", state == CellState.Dirty)
            .AddClass("table-danger", state == CellState.Invalid)
            .Build();

    public string DataSheetHeaderCellClass(bool isSortable) =>
        new CssClassBuilder()
            .AddClass("mar-bs-datasheet__header-cell")
            .AddClass("mar-bs-datasheet__header-cell--sortable", isSortable)
            .Build();

    public string DataSheetRowClass(bool isDirty, bool isSelected, bool isDeleted) =>
        new CssClassBuilder()
            .AddClass("mar-bs-datasheet__row")
            .AddClass("table-warning", isDirty)
            .AddClass("table-active", isSelected)
            .AddClass("text-decoration-line-through", isDeleted)
            .Build();

    public string DataSheetToolbarClass() => "d-flex gap-2 mb-2 mar-bs-datasheet__toolbar";

    public string DataSheetBulkBarClass(bool isVisible) =>
        new CssClassBuilder()
            .AddClass("d-flex gap-2 mar-bs-datasheet__bulk-bar")
            .AddClass("d-none", !isVisible)
            .Build();

    public string DataSheetSaveFooterClass(int dirtyCount) =>
        new CssClassBuilder()
            .AddClass("d-flex justify-content-between align-items-center p-2 mar-bs-datasheet__save-footer")
            .AddClass("mar-bs-datasheet__save-footer--has-changes", dirtyCount > 0)
            .Build();

    // ───────────────────────────────────────────────
    // ListView
    // ───────────────────────────────────────────────

    public string ListViewClass() => "list-group mar-bs-listview";

    public string ListViewItemClass(bool isSelected) =>
        new CssClassBuilder()
            .AddClass("list-group-item list-group-item-action")
            .AddClass("active", isSelected)
            .Build();

    // ───────────────────────────────────────────────
    // Window
    // ───────────────────────────────────────────────

    public string WindowClass(bool isModal) =>
        new CssClassBuilder()
            .AddClass("mar-bs-window card shadow")
            .AddClass("mar-bs-window--modal", isModal)
            .Build();

    public string WindowTitleBarClass() => "card-header d-flex justify-content-between align-items-center mar-bs-window-titlebar";

    public string WindowContentClass() => "card-body mar-bs-window-content";

    public string WindowActionsClass() => "mar-bs-window-actions";

    public string WindowOverlayClass() => "modal-backdrop fade show";

    public string WindowFooterClass() => "card-footer mar-bs-window-footer";

    // ───────────────────────────────────────────────
    // Editor
    // ───────────────────────────────────────────────

    public string EditorClass() => "mar-bs-editor border rounded";

    public string EditorToolbarClass() => "btn-toolbar p-1 border-bottom mar-bs-editor-toolbar";

    public string EditorContentClass() => "p-2 mar-bs-editor-content";

    // ───────────────────────────────────────────────
    // Upload
    // ───────────────────────────────────────────────

    public string UploadClass() => "mar-bs-upload";

    public string UploadFileListClass() => "list-group list-group-flush mt-2";

    public string UploadFileItemClass() => "list-group-item d-flex justify-content-between align-items-center";

    public string UploadDropZoneClass(bool isActive) =>
        new CssClassBuilder()
            .AddClass("border border-2 border-dashed rounded p-4 text-center mar-bs-upload-dropzone")
            .AddClass("border-primary bg-light", isActive)
            .AddClass("border-secondary", !isActive)
            .Build();

    // ───────────────────────────────────────────────
    // Charts
    // ───────────────────────────────────────────────

    public string ChartContainerClass() => "mar-bs-chart-container";

    public string GaugeClass() => "mar-bs-gauge";

    // ───────────────────────────────────────────────
    // Scheduling
    // ───────────────────────────────────────────────

    public string CalendarClass() => "mar-bs-calendar";

    public string SchedulerClass() => "mar-bs-scheduler";

    // AllocationScheduler
    // Note: table-responsive removed — it adds overflow-x:auto which conflicts with internal scroll.
    public string AllocationSchedulerClass() => "mar-bs-allocation-scheduler";

    // Note: d-flex/gap moved to SCSS; mb-2 removed to avoid gap between toolbar and grid.
    public string AllocationSchedulerToolbarClass() => "mar-bs-allocation-scheduler__toolbar";

    public string AllocationSchedulerResourceColumnClass(bool isPinned) =>
        new CssClassBuilder()
            .AddClass("mar-bs-allocation-scheduler__resource-col")
            .AddClass("position-sticky start-0", isPinned)
            .Build();

    public string AllocationSchedulerTimeHeaderClass(TimeGranularity grain) =>
        new CssClassBuilder()
            .AddClass("mar-bs-allocation-scheduler__time-header")
            .AddClass($"mar-bs-allocation-scheduler__time-header--{grain.ToString().ToLower()}")
            .Build();

    public string AllocationSchedulerRowClass(bool isSelected, bool isOverAllocated, bool isStriped = false) =>
        new CssClassBuilder()
            .AddClass("mar-bs-allocation-scheduler__row")
            .AddClass("mar-bs-allocation-scheduler__row--selected", isSelected)
            .AddClass("mar-bs-allocation-scheduler__row--over-allocated", isOverAllocated)
            .AddClass("mar-bs-allocation-scheduler__row--striped", isStriped)
            .Build();

    public string AllocationSchedulerCellClass(bool isEditable, bool isSelected, bool isConflict, bool isDisabled, bool isDragTarget) =>
        new CssClassBuilder()
            .AddClass("mar-bs-allocation-scheduler__cell")
            .AddClass("mar-bs-allocation-scheduler__cell--editable", isEditable)
            .AddClass("mar-bs-allocation-scheduler__cell--selected", isSelected)
            .AddClass("mar-bs-allocation-scheduler__cell--conflict", isConflict)
            .AddClass("mar-bs-allocation-scheduler__cell--disabled", isDisabled)
            .AddClass("mar-bs-allocation-scheduler__cell--drag-target", isDragTarget)
            .Build();

    public string AllocationSchedulerCellValueClass(AllocationValueMode mode) =>
        new CssClassBuilder()
            .AddClass("mar-bs-allocation-scheduler__cell-value")
            .AddClass(mode == AllocationValueMode.Currency ? "mar-bs-allocation-scheduler__cell-value--currency" : "")
            .Build();

    public string AllocationSchedulerDeltaClass(DeltaDisplayMode mode, bool isOver, bool isUnder) =>
        new CssClassBuilder()
            .AddClass("mar-bs-allocation-scheduler__delta")
            .AddClass("text-danger", isOver)
            .AddClass("text-warning", isUnder)
            .Build();

    // Note: d-flex/gap moved to SCSS; mb-2 removed.
    public string AllocationSchedulerScenarioStripClass() => "mar-bs-allocation-scheduler__scenario-strip";

    public string AllocationSchedulerScenarioChipClass(bool isActive, bool isLocked) =>
        new CssClassBuilder()
            .AddClass("badge rounded-pill mar-bs-allocation-scheduler__scenario-chip")
            .AddClass(isActive ? "bg-primary" : "bg-secondary")
            .AddClass("opacity-75", isLocked)
            .Build();

    public string AllocationSchedulerGhostBarClass() => "text-muted opacity-50 mar-bs-allocation-scheduler__ghost-bar";

    public string AllocationSchedulerContextMenuClass() => "dropdown-menu mar-bs-allocation-scheduler__context-menu";

    public string AllocationSchedulerEmptyClass() => "text-center text-muted p-4 mar-bs-allocation-scheduler__empty";

    public string AllocationSchedulerLoaderClass() => "d-flex justify-content-center p-4 mar-bs-allocation-scheduler__loader";

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

    // ───────────────────────────────────────────────
    // ResizableContainer
    // ───────────────────────────────────────────────

    public string ResizableContainerClass(bool isResizing, bool isDisabled) =>
        new CssClassBuilder()
            .AddClass("mar-bs-resizable-container")
            .AddClass("mar-bs-resizable-container--resizing", isResizing)
            .AddClass("mar-bs-resizable-container--disabled", isDisabled)
            .Build();

    public string ResizableContainerContentClass() => "mar-bs-resizable-container__content";

    public string ResizableContainerHandleClass(MariloResizeEdges edge, bool isActive, bool isFocused) =>
        new CssClassBuilder()
            .AddClass("mar-bs-resizable-container__handle")
            .AddClass("mar-bs-resizable-container__handle--right", edge == MariloResizeEdges.Right)
            .AddClass("mar-bs-resizable-container__handle--bottom", edge == MariloResizeEdges.Bottom)
            .AddClass("mar-bs-resizable-container__handle--bottom-right", edge == MariloResizeEdges.BottomRight)
            .AddClass("mar-bs-resizable-container__handle--left", edge == MariloResizeEdges.Left)
            .AddClass("mar-bs-resizable-container__handle--top", edge == MariloResizeEdges.Top)
            .AddClass("mar-bs-resizable-container__handle--top-left", edge == MariloResizeEdges.TopLeft)
            .AddClass("mar-bs-resizable-container__handle--top-right", edge == MariloResizeEdges.TopRight)
            .AddClass("mar-bs-resizable-container__handle--bottom-left", edge == MariloResizeEdges.BottomLeft)
            .AddClass("mar-bs-resizable-container__handle--active", isActive)
            .AddClass("mar-bs-resizable-container__handle--focused", isFocused)
            .Build();

    // ───────────────────────────────────────────────
    // Overlays
    // ───────────────────────────────────────────────

    public string ModalClass(ModalSize size) =>
        new CssClassBuilder()
            .AddClass("modal fade show")
            .AddClass(size switch
            {
                ModalSize.Small => "modal-sm",
                ModalSize.Large => "modal-lg",
                ModalSize.FullScreen => "modal-fullscreen",
                _ => ""
            })
            .Build();

    public string ModalOverlayClass() => "modal-backdrop fade show";

    // ───────────────────────────────────────────────
    // Utility
    // ───────────────────────────────────────────────

    public string IconClass(string iconName, IconSize size, IconFlip flip = IconFlip.None, IconThemeColor themeColor = IconThemeColor.Base) =>
        new CssClassBuilder()
            .AddClass("bi mar-icon")
            .AddClass($"bi-{iconName}")
            .AddClass($"mar-icon--{size.ToString().ToLower()}")
            .AddClass($"mar-icon--flip-{flip.ToString().ToLower()}", flip != IconFlip.None)
            .AddClass(themeColor switch
            {
                IconThemeColor.Primary => "text-primary",
                IconThemeColor.Secondary => "text-secondary",
                IconThemeColor.Success => "text-success",
                IconThemeColor.Warning => "text-warning",
                IconThemeColor.Danger => "text-danger",
                IconThemeColor.Info => "text-info",
                IconThemeColor.Inherit => "",
                _ => ""
            }, themeColor != IconThemeColor.Base && themeColor != IconThemeColor.Inherit)
            .Build();

    public string DragDropClass() => "mar-bs-dragdrop";

    public string DropZoneClass(bool isActive) =>
        new CssClassBuilder()
            .AddClass("border border-2 border-dashed rounded p-3 mar-bs-dropzone")
            .AddClass("border-primary bg-light", isActive)
            .AddClass("border-secondary", !isActive)
            .Build();

    public string ScrollViewClass() => "overflow-auto mar-bs-scrollview";

    // SignalR Status
    public string SignalRStatusClass(AggregateConnectionState state, bool isCompact) =>
        new CssClassBuilder()
            .AddClass("btn btn-sm mar-signalr-status")
            .AddClass(state switch
            {
                AggregateConnectionState.Healthy => "btn-outline-success",
                AggregateConnectionState.Degraded => "btn-outline-warning",
                AggregateConnectionState.Offline => "btn-outline-danger",
                AggregateConnectionState.Partial => "btn-outline-info",
                _ => "btn-outline-secondary"
            })
            .AddClass("mar-signalr-status--compact", isCompact)
            .Build();

    public string SignalRPopupClass() => "card shadow-lg border mar-signalr-popup";

    public string SignalRRowClass(ConnectionHealthState health) =>
        new CssClassBuilder()
            .AddClass("list-group-item d-flex align-items-center mar-signalr-row")
            .AddClass(health switch
            {
                ConnectionHealthState.Healthy => "list-group-item-success",
                ConnectionHealthState.Recovering => "list-group-item-warning",
                ConnectionHealthState.Offline => "list-group-item-danger",
                ConnectionHealthState.Degraded => "list-group-item-danger",
                ConnectionHealthState.Connecting => "list-group-item-info",
                _ => ""
            })
            .Build();

    public string SignalRBadgeClass(ConnectionHealthState health) =>
        new CssClassBuilder()
            .AddClass("badge mar-signalr-badge")
            .AddClass(health switch
            {
                ConnectionHealthState.Healthy => "bg-success",
                ConnectionHealthState.Recovering => "bg-warning text-dark",
                ConnectionHealthState.Offline => "bg-danger",
                ConnectionHealthState.Degraded => "bg-danger",
                ConnectionHealthState.Connecting => "bg-info text-dark",
                _ => "bg-secondary"
            })
            .Build();
}
