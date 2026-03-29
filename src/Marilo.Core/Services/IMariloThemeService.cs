using Marilo.Core.Configuration;
using Marilo.Core.Models;

namespace Marilo.Core.Services;

public interface IMariloThemeService
{
    MariloTheme CurrentTheme { get; }
    bool IsDarkMode { get; }
    bool IsRtl { get; }
    event EventHandler<ThemeChangedEventArgs>? ThemeChanged;
    Task SetThemeAsync(MariloTheme theme);
    Task ToggleDarkModeAsync();
    Task InitializeAsync();
}
