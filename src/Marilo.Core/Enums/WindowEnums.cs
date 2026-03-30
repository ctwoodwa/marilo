namespace Marilo.Core.Enums;

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
