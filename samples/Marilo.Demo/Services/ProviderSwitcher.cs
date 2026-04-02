using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Marilo.Demo.Services;

public enum DesignProvider { FluentUI, Bootstrap }

public class ProviderSwitcher : IMariloCssProvider, IMariloIconProvider, IMariloJsInterop
{
    private readonly IMariloCssProvider _fluentCss;
    private readonly IMariloCssProvider _bootstrapCss;
    private readonly IMariloIconProvider _fluentIcons;
    private readonly IMariloIconProvider _bootstrapIcons;
    private readonly IMariloJsInterop _fluentJs;
    private readonly IMariloJsInterop _bootstrapJs;

    public DesignProvider ActiveProvider { get; private set; } = DesignProvider.FluentUI;

    public event Action? OnProviderChanged;

    public ProviderSwitcher(
        Marilo.Providers.FluentUI.FluentUICssProvider fluentCss,
        Marilo.Providers.Bootstrap.BootstrapCssProvider bootstrapCss,
        Marilo.Providers.FluentUI.FluentUIIconProvider fluentIcons,
        Marilo.Providers.Bootstrap.BootstrapIconProvider bootstrapIcons,
        Marilo.Providers.FluentUI.FluentUIJsInterop fluentJs,
        Marilo.Providers.Bootstrap.BootstrapJsInterop bootstrapJs)
    {
        _fluentCss = fluentCss;
        _bootstrapCss = bootstrapCss;
        _fluentIcons = fluentIcons;
        _bootstrapIcons = bootstrapIcons;
        _fluentJs = fluentJs;
        _bootstrapJs = bootstrapJs;
    }

    private IMariloCssProvider Css => ActiveProvider == DesignProvider.FluentUI ? _fluentCss : _bootstrapCss;
    private IMariloIconProvider Icons => ActiveProvider == DesignProvider.FluentUI ? _fluentIcons : _bootstrapIcons;
    private IMariloJsInterop JsInterop => ActiveProvider == DesignProvider.FluentUI ? _fluentJs : _bootstrapJs;

    public void SetProvider(DesignProvider provider)
    {
        if (ActiveProvider == provider) return;
        ActiveProvider = provider;
        OnProviderChanged?.Invoke();
    }

    // ── IMariloIconProvider ──
    public MarkupString GetIcon(string name, IconSize size = IconSize.Medium) => Icons.GetIcon(name, size);
    public string GetIconSpriteUrl() => Icons.GetIconSpriteUrl();

    // ── IMariloJsInterop ──
    public ValueTask InitializeAsync() => JsInterop.InitializeAsync();
    public ValueTask<bool> ShowModalAsync(string modalId) => JsInterop.ShowModalAsync(modalId);
    public ValueTask HideModalAsync(string modalId) => JsInterop.HideModalAsync(modalId);
    public ValueTask<BoundingBox> GetElementBoundsAsync(ElementReference element) => JsInterop.GetElementBoundsAsync(element);
    public ValueTask ObserveScrollAsync(ElementReference element, DotNetObjectReference<object> callback) => JsInterop.ObserveScrollAsync(element, callback);
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    // ── IMariloCssProvider — Layout ──
    public string ContainerClass(string? size = null) => Css.ContainerClass(size);
    public string GridClass() => Css.GridClass();
    public string RowClass() => Css.RowClass();
    public string ColumnClass(int? span = null, int? offset = null) => Css.ColumnClass(span, offset);
    public string StackClass(StackDirection orientation) => Css.StackClass(orientation);
    public string DividerClass(bool vertical) => Css.DividerClass(vertical);
    public string PanelClass() => Css.PanelClass();
    public string DrawerClass(DrawerPosition position, bool isOpen) => Css.DrawerClass(position, isOpen);
    public string AppBarClass(AppBarPosition position) => Css.AppBarClass(position);
    public string AccordionClass() => Css.AccordionClass();
    public string AccordionItemClass(bool isExpanded) => Css.AccordionItemClass(isExpanded);
    public string TabsClass(TabPosition position, TabAlignment alignment, TabSize size) => Css.TabsClass(position, alignment, size);
    public string TabClass(bool isActive, bool isDisabled) => Css.TabClass(isActive, isDisabled);
    public string TabPanelClass(bool isActive, bool persistContent) => Css.TabPanelClass(isActive, persistContent);
    public string StepperClass() => Css.StepperClass();
    public string StepClass(StepStatus status) => Css.StepClass(status);
    public string SplitterClass(StackDirection orientation) => Css.SplitterClass(orientation);
    public string DrawerOverlayClass() => Css.DrawerOverlayClass();
    public string ContextMenuClass() => Css.ContextMenuClass();

    // ── Navigation ──
    public string NavBarClass() => Css.NavBarClass();
    public string NavMenuClass() => Css.NavMenuClass();
    public string NavItemClass(bool isActive) => Css.NavItemClass(isActive);
    public string BreadcrumbClass() => Css.BreadcrumbClass();
    public string BreadcrumbItemClass() => Css.BreadcrumbItemClass();
    public string MenuClass() => Css.MenuClass();
    public string MenuItemClass(bool isDisabled) => Css.MenuItemClass(isDisabled);
    public string MenuDividerClass() => Css.MenuDividerClass();
    public string PaginationClass() => Css.PaginationClass();
    public string PaginationItemClass(bool isActive) => Css.PaginationItemClass(isActive);
    public string TreeViewClass() => Css.TreeViewClass();
    public string TreeItemClass(bool isExpanded, bool isSelected) => Css.TreeItemClass(isExpanded, isSelected);
    public string ToolbarClass() => Css.ToolbarClass();
    public string EnvironmentBadgeClass(string env) => Css.EnvironmentBadgeClass(env);
    public string TimeRangeSelectorClass() => Css.TimeRangeSelectorClass();
    public string ToolbarButtonClass(bool isDisabled = false) => Css.ToolbarButtonClass(isDisabled);
    public string ToolbarToggleButtonClass(bool isActive, bool isDisabled = false) => Css.ToolbarToggleButtonClass(isActive, isDisabled);
    public string ToolbarSeparatorClass() => Css.ToolbarSeparatorClass();
    public string ToolbarGroupClass() => Css.ToolbarGroupClass();
    public string LinkClass() => Css.LinkClass();

    // ── Buttons ──
    public string ButtonClass(ButtonVariant variant, ButtonSize size, bool isOutline, bool isDisabled) => Css.ButtonClass(variant, size, isOutline, isDisabled);
    public string ButtonClass(ButtonVariant variant, ButtonSize size, FillMode fillMode, RoundedMode rounded, bool isDisabled) => Css.ButtonClass(variant, size, fillMode, rounded, isDisabled);
    public string IconButtonClass(ButtonSize size) => Css.IconButtonClass(size);
    public string ButtonGroupClass() => Css.ButtonGroupClass();
    public string ToggleButtonClass(bool selected) => Css.ToggleButtonClass(selected);
    public string SplitButtonClass() => Css.SplitButtonClass();
    public string ChipClass(ChipVariant variant, bool isSelected) => Css.ChipClass(variant, isSelected);
    public string ChipSetClass() => Css.ChipSetClass();
    public string FabClass(FabSize size) => Css.FabClass(size);

    // ── Forms — Inputs ──
    public string TextBoxClass(bool isInvalid, bool isDisabled) => Css.TextBoxClass(isInvalid, isDisabled);
    public string TextAreaClass(bool isInvalid) => Css.TextAreaClass(isInvalid);
    public string NumericInputClass() => Css.NumericInputClass();
    public string SearchBoxClass() => Css.SearchBoxClass();
    public string AutocompleteClass() => Css.AutocompleteClass();
    public string AutocompleteClass(bool isOpen, bool isDisabled, bool isInvalid) => Css.AutocompleteClass(isOpen, isDisabled, isInvalid);
    public string AutocompleteItemClass(bool isHighlighted, bool isSelected) => Css.AutocompleteItemClass(isHighlighted, isSelected);
    public string SelectClass(bool isInvalid) => Css.SelectClass(isInvalid);
    public string CheckboxClass(bool isChecked) => Css.CheckboxClass(isChecked);
    public string RadioClass(bool isSelected) => Css.RadioClass(isSelected);
    public string RadioGroupClass() => Css.RadioGroupClass();
    public string SwitchClass(bool isOn) => Css.SwitchClass(isOn);
    public string SliderClass() => Css.SliderClass();
    public string SliderClass(SliderOrientation orientation) => Css.SliderClass(orientation);
    public string RatingClass() => Css.RatingClass();
    public string ColorPickerClass() => Css.ColorPickerClass();
    public string ColorPickerPopupClass() => Css.ColorPickerPopupClass();
    public string DatePickerClass() => Css.DatePickerClass();
    public string TimePickerClass() => Css.TimePickerClass();
    public string FileUploadClass() => Css.FileUploadClass();
    public string FileUploadFileListClass() => Css.FileUploadFileListClass();

    // ── Selection / Dropdowns ──
    public string DropDownListClass(bool isOpen, bool isDisabled, bool isInvalid) => Css.DropDownListClass(isOpen, isDisabled, isInvalid);
    public string DropDownListPopupClass() => Css.DropDownListPopupClass();
    public string DropDownListItemClass(bool isHighlighted, bool isSelected) => Css.DropDownListItemClass(isHighlighted, isSelected);
    public string ComboBoxClass(bool isOpen, bool isDisabled, bool isInvalid) => Css.ComboBoxClass(isOpen, isDisabled, isInvalid);
    public string ComboBoxPopupClass() => Css.ComboBoxPopupClass();
    public string ComboBoxItemClass(bool isHighlighted, bool isSelected) => Css.ComboBoxItemClass(isHighlighted, isSelected);
    public string MultiSelectClass(bool isOpen, bool isDisabled, bool isInvalid) => Css.MultiSelectClass(isOpen, isDisabled, isInvalid);
    public string MultiSelectPopupClass() => Css.MultiSelectPopupClass();
    public string MultiSelectItemClass(bool isHighlighted, bool isSelected) => Css.MultiSelectItemClass(isHighlighted, isSelected);
    public string MultiSelectTagClass() => Css.MultiSelectTagClass();
    public string DropdownPopupClass() => Css.DropdownPopupClass();

    // ── Forms — Containers ──
    public string FormClass() => Css.FormClass();
    public string FieldClass() => Css.FieldClass();
    public string LabelClass() => Css.LabelClass();
    public string InputGroupClass() => Css.InputGroupClass();
    public string ValidationMessageClass(ValidationSeverity severity) => Css.ValidationMessageClass(severity);

    // ── Data Display ──
    public string CardClass() => Css.CardClass();
    public string CardHeaderClass() => Css.CardHeaderClass();
    public string CardBodyClass() => Css.CardBodyClass();
    public string CardActionsClass() => Css.CardActionsClass();
    public string CardFooterClass() => Css.CardFooterClass();
    public string CardImageClass() => Css.CardImageClass();
    public string ListClass() => Css.ListClass();
    public string ListItemClass() => Css.ListItemClass();
    public string TableClass() => Css.TableClass();
    public string AvatarClass(AvatarSize size) => Css.AvatarClass(size);
    public string BadgeClass(BadgeVariant variant) => Css.BadgeClass(variant);
    public string TooltipClass(TooltipPosition position) => Css.TooltipClass(position);
    public string TooltipClass(TooltipPosition position, TooltipShowOn showOn) => Css.TooltipClass(position, showOn);
    public string PopoverClass() => Css.PopoverClass();
    public string TimelineClass() => Css.TimelineClass();
    public string TimelineItemClass() => Css.TimelineItemClass();
    public string CarouselClass() => Css.CarouselClass();
    public string TypographyClass(TypographyVariant variant) => Css.TypographyClass(variant);

    // ── Feedback ──
    public string AlertClass(AlertSeverity severity) => Css.AlertClass(severity);
    public string AlertStripClass() => Css.AlertStripClass();
    public string ToastClass(ToastSeverity severity) => Css.ToastClass(severity);
    public string SnackbarClass() => Css.SnackbarClass();
    public string SnackbarClass(NotificationVerticalPosition vertical, NotificationHorizontalPosition horizontal) => Css.SnackbarClass(vertical, horizontal);
    public string SnackbarHostClass() => Css.SnackbarHostClass();
    public string DialogClass() => Css.DialogClass();
    public string DialogClass(bool isDraggable) => Css.DialogClass(isDraggable);
    public string DialogOverlayClass() => Css.DialogOverlayClass();
    public string ProgressBarClass() => Css.ProgressBarClass();
    public string ProgressCircleClass() => Css.ProgressCircleClass();
    public string SpinnerClass(SpinnerSize size) => Css.SpinnerClass(size);
    public string SkeletonClass(SkeletonVariant variant) => Css.SkeletonClass(variant);
    public string CalloutClass(CalloutType type) => Css.CalloutClass(type);

    // ── DataGrid ──
    public string DataGridClass() => Css.DataGridClass();
    public string DataGridHeaderClass() => Css.DataGridHeaderClass();
    public string DataGridHeaderCellClass(bool isSortable, bool isSorted) => Css.DataGridHeaderCellClass(isSortable, isSorted);
    public string DataGridRowClass(bool isSelected, bool isStriped) => Css.DataGridRowClass(isSelected, isStriped);
    public string DataGridCellClass() => Css.DataGridCellClass();
    public string DataGridPagerClass() => Css.DataGridPagerClass();
    public string DataGridToolbarClass() => Css.DataGridToolbarClass();
    public string DataGridFilterRowClass() => Css.DataGridFilterRowClass();
    public string DataGridFilterCellClass() => Css.DataGridFilterCellClass();
    public string DataGridGroupHeaderClass() => Css.DataGridGroupHeaderClass();

    // ── ListView ──
    public string ListViewClass() => Css.ListViewClass();
    public string ListViewItemClass(bool isSelected) => Css.ListViewItemClass(isSelected);

    // ── Window ──
    public string WindowClass(bool isModal) => Css.WindowClass(isModal);
    public string WindowTitleBarClass() => Css.WindowTitleBarClass();
    public string WindowContentClass() => Css.WindowContentClass();
    public string WindowActionsClass() => Css.WindowActionsClass();
    public string WindowOverlayClass() => Css.WindowOverlayClass();
    public string WindowFooterClass() => Css.WindowFooterClass();

    // ── Editor ──
    public string EditorClass() => Css.EditorClass();
    public string EditorToolbarClass() => Css.EditorToolbarClass();
    public string EditorContentClass() => Css.EditorContentClass();

    // ── Upload ──
    public string UploadClass() => Css.UploadClass();
    public string UploadFileListClass() => Css.UploadFileListClass();
    public string UploadFileItemClass() => Css.UploadFileItemClass();
    public string UploadDropZoneClass(bool isActive) => Css.UploadDropZoneClass(isActive);

    // ── Charts ──
    public string ChartContainerClass() => Css.ChartContainerClass();
    public string GaugeClass() => Css.GaugeClass();

    // ── Scheduling ──
    public string CalendarClass() => Css.CalendarClass();
    public string SchedulerClass() => Css.SchedulerClass();

    // ── Overlays ──
    public string ModalClass(ModalSize size) => Css.ModalClass(size);
    public string ModalOverlayClass() => Css.ModalOverlayClass();

    // ── SignalR Status ──
    public string SignalRStatusClass(AggregateConnectionState state, bool isCompact) => Css.SignalRStatusClass(state, isCompact);
    public string SignalRPopupClass() => Css.SignalRPopupClass();
    public string SignalRRowClass(ConnectionHealthState health) => Css.SignalRRowClass(health);
    public string SignalRBadgeClass(ConnectionHealthState health) => Css.SignalRBadgeClass(health);

    // ── Utility ──
    public string IconClass(string iconName, IconSize size, IconFlip flip = IconFlip.None, IconThemeColor themeColor = IconThemeColor.Base) => Css.IconClass(iconName, size, flip, themeColor);
    public string DragDropClass() => Css.DragDropClass();
    public string DropZoneClass(bool isActive) => Css.DropZoneClass(isActive);
    public string ScrollViewClass() => Css.ScrollViewClass();
}
