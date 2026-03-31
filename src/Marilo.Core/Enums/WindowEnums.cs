namespace Marilo.Core.Enums;

/// <summary>
/// Specifies the visual state of a window.
/// </summary>
public enum WindowState
{
    /// <summary>Normal sized and positioned window.</summary>
    Normal,

    /// <summary>Minimized to a collapsed state.</summary>
    Minimized,

    /// <summary>Maximized to fill the viewport.</summary>
    Maximized
}

/// <summary>
/// Specifies the available actions for a Window title bar.
/// </summary>
[Flags]
public enum WindowAction
{
    /// <summary>No actions.</summary>
    None = 0,

    /// <summary>The window can be closed.</summary>
    Close = 1,

    /// <summary>The window can be minimized.</summary>
    Minimize = 2,

    /// <summary>The window can be maximized or restored.</summary>
    Maximize = 4
}
