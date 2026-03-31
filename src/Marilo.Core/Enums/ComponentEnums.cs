namespace Marilo.Core.Enums;

/// <summary>
/// Specifies the layout alignment of action buttons in dialogs, popovers, and similar containers.
/// </summary>
public enum ActionsLayout
{
    /// <summary>Actions are aligned to the start (left in LTR).</summary>
    Start,

    /// <summary>Actions are centered.</summary>
    Center,

    /// <summary>Actions are aligned to the end (right in LTR).</summary>
    End,

    /// <summary>Actions are stretched to fill the available width.</summary>
    Stretch
}

/// <summary>
/// Specifies the visual style variant of a chip.
/// </summary>
public enum ChipVariant
{
    /// <summary>Default chip styling.</summary>
    Default,

    /// <summary>Primary-colored chip.</summary>
    Primary,

    /// <summary>Secondary-colored chip.</summary>
    Secondary,

    /// <summary>Danger-colored chip for destructive context.</summary>
    Danger,

    /// <summary>Warning-colored chip for cautionary context.</summary>
    Warning,

    /// <summary>Info-colored chip for informational context.</summary>
    Info,

    /// <summary>Success-colored chip for positive context.</summary>
    Success
}

/// <summary>
/// Specifies the size of a floating action button (FAB).
/// </summary>
public enum FabSize
{
    /// <summary>A compact FAB.</summary>
    Small,

    /// <summary>The default FAB size.</summary>
    Medium,

    /// <summary>A large, prominent FAB.</summary>
    Large
}

/// <summary>
/// Specifies the severity of a form validation message.
/// </summary>
public enum ValidationSeverity
{
    /// <summary>Informational validation hint.</summary>
    Info,

    /// <summary>Non-blocking validation warning.</summary>
    Warning,

    /// <summary>Blocking validation error that prevents submission.</summary>
    Error
}

/// <summary>
/// Specifies the display size of an avatar.
/// </summary>
public enum AvatarSize
{
    /// <summary>Extra-small avatar (e.g., inline mentions).</summary>
    ExtraSmall,

    /// <summary>Small avatar.</summary>
    Small,

    /// <summary>Default avatar size.</summary>
    Medium,

    /// <summary>Large avatar.</summary>
    Large,

    /// <summary>Extra-large avatar (e.g., profile headers).</summary>
    ExtraLarge
}

/// <summary>
/// Specifies the visual style variant of a badge.
/// </summary>
public enum BadgeVariant
{
    /// <summary>Default badge styling.</summary>
    Default,

    /// <summary>Primary-colored badge.</summary>
    Primary,

    /// <summary>Secondary-colored badge.</summary>
    Secondary,

    /// <summary>Danger-colored badge.</summary>
    Danger,

    /// <summary>Warning-colored badge.</summary>
    Warning,

    /// <summary>Info-colored badge.</summary>
    Info,

    /// <summary>Success-colored badge.</summary>
    Success
}

/// <summary>
/// Specifies the preferred position of a tooltip relative to its anchor element.
/// </summary>
public enum TooltipPosition
{
    /// <summary>Tooltip appears above the anchor.</summary>
    Top,

    /// <summary>Tooltip appears below the anchor.</summary>
    Bottom,

    /// <summary>Tooltip appears to the left of the anchor.</summary>
    Left,

    /// <summary>Tooltip appears to the right of the anchor.</summary>
    Right
}

/// <summary>
/// Specifies the trigger that shows a popover or tooltip.
/// </summary>
public enum PopoverShowOn
{
    /// <summary>Show on click (toggle).</summary>
    Click,

    /// <summary>Show on mouse enter, hide on mouse leave.</summary>
    MouseEnter
}

/// <summary>
/// Specifies the collision behavior when a popover would overflow the viewport.
/// </summary>
public enum PopoverCollision
{
    /// <summary>No collision detection; popover may overflow viewport.</summary>
    None,

    /// <summary>Flip to the opposite position if the popover overflows.</summary>
    Flip,

    /// <summary>Shift the popover along the axis to stay within the viewport.</summary>
    Fit
}

/// <summary>
/// Specifies the semantic typography variant for text rendering.
/// </summary>
public enum TypographyVariant
{
    /// <summary>Heading level 1.</summary>
    H1,

    /// <summary>Heading level 2.</summary>
    H2,

    /// <summary>Heading level 3.</summary>
    H3,

    /// <summary>Heading level 4.</summary>
    H4,

    /// <summary>Heading level 5.</summary>
    H5,

    /// <summary>Heading level 6.</summary>
    H6,

    /// <summary>Subtitle level 1 (larger).</summary>
    Subtitle1,

    /// <summary>Subtitle level 2 (smaller).</summary>
    Subtitle2,

    /// <summary>Body text level 1 (default).</summary>
    Body1,

    /// <summary>Body text level 2 (smaller).</summary>
    Body2,

    /// <summary>Small caption text.</summary>
    Caption,

    /// <summary>All-caps overline text.</summary>
    Overline
}

/// <summary>
/// Specifies the size of a loading spinner.
/// </summary>
public enum SpinnerSize
{
    /// <summary>A compact spinner for inline use.</summary>
    Small,

    /// <summary>The default spinner size.</summary>
    Medium,

    /// <summary>A large spinner for full-page loading states.</summary>
    Large
}

/// <summary>
/// Specifies the shape of a skeleton placeholder element.
/// </summary>
public enum SkeletonVariant
{
    /// <summary>A text-line-shaped skeleton.</summary>
    Text,

    /// <summary>A rectangular skeleton block.</summary>
    Rectangular,

    /// <summary>A circular skeleton (e.g., avatar placeholder).</summary>
    Circular,

    /// <summary>A rectangle with rounded corners.</summary>
    Rounded
}

/// <summary>
/// Specifies the semantic type of a callout message.
/// </summary>
public enum CalloutType
{
    /// <summary>Informational callout.</summary>
    Info,

    /// <summary>Warning callout.</summary>
    Warning,

    /// <summary>Danger callout for critical information.</summary>
    Danger,

    /// <summary>Success callout for positive information.</summary>
    Success,

    /// <summary>Neutral note callout.</summary>
    Note
}

/// <summary>
/// Specifies the size of a modal dialog.
/// </summary>
public enum ModalSize
{
    /// <summary>A compact modal for simple confirmations.</summary>
    Small,

    /// <summary>The default modal size.</summary>
    Medium,

    /// <summary>A wide modal for complex content.</summary>
    Large,

    /// <summary>A modal that fills the entire viewport.</summary>
    FullScreen
}

/// <summary>
/// Specifies the display size of an icon.
/// </summary>
public enum IconSize
{
    /// <summary>Small icon (e.g., inline with text).</summary>
    Small,

    /// <summary>Default icon size.</summary>
    Medium,

    /// <summary>Large icon.</summary>
    Large,

    /// <summary>Extra-large icon for hero or feature sections.</summary>
    ExtraLarge
}

/// <summary>
/// Specifies the flip transformation applied to an icon.
/// </summary>
public enum IconFlip
{
    /// <summary>No flip applied.</summary>
    None,

    /// <summary>Flipped along the horizontal axis.</summary>
    Horizontal,

    /// <summary>Flipped along the vertical axis.</summary>
    Vertical,

    /// <summary>Flipped along both axes.</summary>
    Both
}

/// <summary>
/// Specifies a theme-aware color applied to an icon.
/// </summary>
public enum IconThemeColor
{
    /// <summary>The default base color.</summary>
    Base,

    /// <summary>Primary theme color.</summary>
    Primary,

    /// <summary>Secondary theme color.</summary>
    Secondary,

    /// <summary>Success/positive color.</summary>
    Success,

    /// <summary>Warning/cautionary color.</summary>
    Warning,

    /// <summary>Danger/error color.</summary>
    Danger,

    /// <summary>Informational color.</summary>
    Info,

    /// <summary>Inherits color from the parent element.</summary>
    Inherit
}

/// <summary>
/// Specifies a responsive breakpoint tier for layout decisions.
/// </summary>
public enum Breakpoint
{
    /// <summary>Extra-small viewport (mobile portrait).</summary>
    ExtraSmall,

    /// <summary>Small viewport (mobile landscape).</summary>
    Small,

    /// <summary>Medium viewport (tablet).</summary>
    Medium,

    /// <summary>Large viewport (desktop).</summary>
    Large,

    /// <summary>Extra-large viewport (wide desktop).</summary>
    ExtraLarge,

    /// <summary>Extra-extra-large viewport (ultra-wide).</summary>
    ExtraExtraLarge
}

/// <summary>
/// Specifies the checkbox mode for a tree view component.
/// </summary>
public enum CheckBoxMode
{
    /// <summary>No checkboxes shown.</summary>
    None,

    /// <summary>Single checkbox selection.</summary>
    Single,

    /// <summary>Multiple checkbox selection.</summary>
    Multiple
}
