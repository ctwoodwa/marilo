using Marilo.Core.Enums;

namespace Marilo.Core.Contracts;

/// <summary>
/// Defines the contract for a design-system CSS provider. Each implementation
/// (e.g., Fluent UI, Material, Bootstrap) maps Marilo component states to its
/// own CSS class names, keeping component logic design-system-agnostic.
/// </summary>
public interface IMariloCssProvider
{
    // ── Layout ──────────────────────────────────────────────────────────
    string ContainerClass(string? size = null);
    string GridClass();
    string RowClass();
    string ColumnClass(int? span = null, int? offset = null);
    string StackClass(StackDirection direction, StackAlignment align);
    string DividerClass(bool vertical);
    string PanelClass();
    string DrawerClass(DrawerPosition position, bool isOpen);
    string AppBarClass(AppBarPosition position);
    string AccordionClass();
    string AccordionItemClass(bool isExpanded);
    string TabsClass();
    string TabPanelClass(bool isActive);
    string StepperClass();
    string StepClass(StepStatus status);
    string SplitterClass(StackDirection orientation);
    string DrawerOverlayClass();
    string ContextMenuClass();

    // ── Navigation ──────────────────────────────────────────────────────
    string NavBarClass();
    string NavMenuClass();
    string NavItemClass(bool isActive);
    string BreadcrumbClass();
    string BreadcrumbItemClass();
    string MenuClass();
    string MenuItemClass(bool isDisabled);
    string PaginationClass();
    string PaginationItemClass(bool isActive);
    string TreeViewClass();
    string TreeItemClass(bool isExpanded, bool isSelected);
    string ToolbarClass();
    string EnvironmentBadgeClass(string env);
    string TimeRangeSelectorClass();
    string ToolbarButtonClass(bool isDisabled = false);
    string ToolbarToggleButtonClass(bool isActive, bool isDisabled = false);
    string ToolbarSeparatorClass();
    string ToolbarGroupClass();
    string LinkClass();

    // ── Buttons ─────────────────────────────────────────────────────────
    string ButtonClass(ButtonVariant variant, ButtonSize size, bool isOutline, bool isDisabled);
    string IconButtonClass(ButtonSize size);
    string ButtonGroupClass();
    string ToggleButtonClass(bool isActive);
    string SplitButtonClass();
    string ChipClass(ChipVariant variant, bool isSelected);
    string ChipSetClass();
    string FabClass(FabSize size);

    // ── Forms — Inputs ──────────────────────────────────────────────────
    string TextFieldClass(bool isInvalid, bool isDisabled);
    string TextAreaClass(bool isInvalid);
    string NumericInputClass();
    string SearchBoxClass();
    string AutocompleteClass();
    string SelectClass(bool isInvalid);
    string CheckboxClass(bool isChecked);
    string RadioClass(bool isSelected);
    string RadioGroupClass();
    string SwitchClass(bool isOn);
    string SliderClass();
    string RatingClass();
    string ColorPickerClass();
    string DatePickerClass();
    string TimePickerClass();
    string FileUploadClass();

    // ── Forms — Containers ──────────────────────────────────────────────
    string FormClass();
    string FieldClass();
    string LabelClass();
    string InputGroupClass();
    string ValidationMessageClass(ValidationSeverity severity);

    // ── Data Display ────────────────────────────────────────────────────
    string CardClass();
    string CardHeaderClass();
    string CardBodyClass();
    string CardActionsClass();
    string ListClass();
    string ListItemClass();
    string TableClass();
    string AvatarClass(AvatarSize size);
    string BadgeClass(BadgeVariant variant);
    string TooltipClass(TooltipPosition position);
    string PopoverClass();
    string TimelineClass();
    string TimelineItemClass();
    string CarouselClass();
    string TypographyClass(TypographyVariant variant);

    // ── Feedback ────────────────────────────────────────────────────────
    string AlertClass(AlertSeverity severity);
    string AlertStripClass();
    string ToastClass(ToastSeverity severity);
    string SnackbarClass();
    string DialogClass();
    string DialogOverlayClass();
    string ProgressBarClass();
    string ProgressCircleClass();
    string SpinnerClass(SpinnerSize size);
    string SkeletonClass(SkeletonVariant variant);
    string CalloutClass(CalloutType type);

    // ── DataGrid ────────────────────────────────────────────────────────
    string DataGridClass();
    string DataGridHeaderClass();
    string DataGridRowClass(bool isSelected, bool isStriped);
    string DataGridCellClass();
    string DataGridPagerClass();

    // ── Charts ──────────────────────────────────────────────────────────
    string ChartContainerClass();
    string GaugeClass();

    // ── Scheduling ──────────────────────────────────────────────────────
    string CalendarClass();
    string SchedulerClass();

    // ── Overlays ────────────────────────────────────────────────────────
    string ModalClass(ModalSize size);
    string ModalOverlayClass();

    // ── Utility ─────────────────────────────────────────────────────────
    string IconClass(string iconName, IconSize size, IconFlip flip = IconFlip.None, IconThemeColor themeColor = IconThemeColor.Base);
    string DragDropClass();
    string DropZoneClass(bool isActive);
    string ScrollViewClass();
}
