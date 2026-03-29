using Marilo.Core.Configuration;

namespace Marilo.Core.Models;

public class ThemeChangedEventArgs : EventArgs
{
    public required MariloTheme OldTheme { get; init; }
    public required MariloTheme NewTheme { get; init; }
    public bool IsDarkMode { get; init; }
}
