using Marilo.Core.Configuration;
using Marilo.Core.Models;
using Microsoft.JSInterop;

namespace Marilo.Core.Services;

public class ThemeService : IMariloThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private MariloTheme _currentTheme;

    public ThemeService(IJSRuntime jsRuntime, MariloOptions options)
    {
        _jsRuntime = jsRuntime;
        _currentTheme = options.Theme ?? new MariloTheme();
    }

    public MariloTheme CurrentTheme => _currentTheme;
    public bool IsDarkMode { get; private set; }
    public bool IsRtl => _currentTheme.IsRtl;

    public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;

    public async Task InitializeAsync()
    {
        try
        {
            var stored = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "marilo-theme-mode");
            IsDarkMode = stored == "dark";
        }
        catch
        {
            // localStorage may not be available during prerendering
        }
    }

    public async Task SetThemeAsync(MariloTheme theme)
    {
        var oldTheme = _currentTheme;
        _currentTheme = theme;
        ThemeChanged?.Invoke(this, new ThemeChangedEventArgs
        {
            OldTheme = oldTheme,
            NewTheme = theme,
            IsDarkMode = IsDarkMode
        });
        await Task.CompletedTask;
    }

    public async Task ToggleDarkModeAsync()
    {
        IsDarkMode = !IsDarkMode;
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "marilo-theme-mode", IsDarkMode ? "dark" : "light");
        }
        catch
        {
            // localStorage may not be available during prerendering
        }

        ThemeChanged?.Invoke(this, new ThemeChangedEventArgs
        {
            OldTheme = _currentTheme,
            NewTheme = _currentTheme,
            IsDarkMode = IsDarkMode
        });
    }
}
