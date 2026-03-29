using Microsoft.JSInterop;

namespace Marilo.Services;

public class ThemeService
{
    private readonly IJSRuntime _js;

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    public bool IsDarkMode { get; private set; } = true;
    public float BaseLayerLuminance => IsDarkMode ? 0.15f : 0.98f;

    public event Action? OnThemeChanged;

    public async Task InitializeAsync()
    {
        try
        {
            var saved = await _js.InvokeAsync<string?>("localStorage.getItem", "theme");
            if (saved is not null)
            {
                IsDarkMode = saved == "dark";
            }
        }
        catch
        {
            // localStorage may not be available (e.g., during prerendering)
        }
    }

    public async Task ToggleThemeAsync()
    {
        IsDarkMode = !IsDarkMode;
        try
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "theme", IsDarkMode ? "dark" : "light");
        }
        catch { }
        OnThemeChanged?.Invoke();
    }

    public void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        OnThemeChanged?.Invoke();
    }

    public void SetDarkMode(bool isDark)
    {
        IsDarkMode = isDark;
        OnThemeChanged?.Invoke();
    }
}
