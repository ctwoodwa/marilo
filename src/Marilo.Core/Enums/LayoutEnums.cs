namespace Marilo.Core.Enums;

/// <summary>
/// Specifies the layout direction of a stack container.
/// </summary>
public enum StackDirection
{
    /// <summary>Items are arranged in a horizontal row.</summary>
    Horizontal,

    /// <summary>Items are arranged in a vertical column.</summary>
    Vertical
}

/// <summary>
/// Specifies how items are aligned within a stack container.
/// </summary>
public enum StackAlignment
{
    /// <summary>Items are aligned to the start of the container.</summary>
    Start,

    /// <summary>Items are centered within the container.</summary>
    Center,

    /// <summary>Items are aligned to the end of the container.</summary>
    End,

    /// <summary>Items are stretched to fill the container.</summary>
    Stretch,

    /// <summary>Items are evenly distributed with space between them.</summary>
    SpaceBetween,

    /// <summary>Items are evenly distributed with space around them.</summary>
    SpaceAround
}

/// <summary>
/// Specifies the position from which a drawer panel slides in.
/// </summary>
public enum DrawerPosition
{
    /// <summary>Drawer slides in from the left edge.</summary>
    Left,

    /// <summary>Drawer slides in from the right edge.</summary>
    Right,

    /// <summary>Drawer slides in from the top edge.</summary>
    Top,

    /// <summary>Drawer slides in from the bottom edge.</summary>
    Bottom
}

/// <summary>
/// Specifies the positioning behavior of an application bar.
/// </summary>
public enum AppBarPosition
{
    /// <summary>Positioned at the top of the page in normal flow.</summary>
    Top,

    /// <summary>Positioned at the bottom of the page in normal flow.</summary>
    Bottom,

    /// <summary>Fixed to the viewport and does not scroll with content.</summary>
    Fixed,

    /// <summary>Sticks to the top of the viewport when scrolled past.</summary>
    Sticky
}

/// <summary>
/// Specifies the current status of a step in a stepper control.
/// </summary>
public enum StepStatus
{
    /// <summary>The step has not yet been reached.</summary>
    Pending,

    /// <summary>The step is currently in progress.</summary>
    Active,

    /// <summary>The step has been successfully completed.</summary>
    Completed,

    /// <summary>The step encountered an error.</summary>
    Error
}
