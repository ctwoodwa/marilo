using Bunit;
using Marilo.Components;
using Marilo.Core.Configuration;
using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Marilo.Core.Services;
using Marilo.Providers.FluentUI;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Marilo.Tests.Unit.Foundation;

public class ThemeProviderTests : MariloTestBase
{
    // ---------------------------------------------------------------
    // Dark-mode helper: overrides TestThemeService with IsDarkMode = true
    // ---------------------------------------------------------------
    private class DarkModeThemeService : IMariloThemeService
    {
        public MariloTheme CurrentTheme { get; } = new();
        public bool IsDarkMode => true;
        public bool IsRtl => false;
#pragma warning disable CS0067
        public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;
#pragma warning restore CS0067

        public Task SetThemeAsync(MariloTheme theme) => Task.CompletedTask;
        public Task ToggleDarkModeAsync() => Task.CompletedTask;
        public Task InitializeAsync() => Task.CompletedTask;
    }

    // ---------------------------------------------------------------
    // 1. Wrapper div renders with class="marilo-theme-provider"
    // ---------------------------------------------------------------
    [Fact]
    public void RendersWrapperDiv_WithMariloThemeProviderClass()
    {
        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme()));

        var div = cut.Find("div.marilo-theme-provider");
        Assert.NotNull(div);
    }

    // ---------------------------------------------------------------
    // 2. CSS custom properties for colors, typography, and shape
    //    are emitted as inline styles
    // ---------------------------------------------------------------
    [Fact]
    public void RendersInlineStyles_WithColorTokens()
    {
        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme()));

        Assert.Contains("--marilo-color-primary", cut.Markup);
        Assert.Contains("--marilo-color-secondary", cut.Markup);
        Assert.Contains("--marilo-color-danger", cut.Markup);
        Assert.Contains("--marilo-color-bg", cut.Markup);
        Assert.Contains("--marilo-color-surface", cut.Markup);
    }

    [Fact]
    public void RendersInlineStyles_WithTypographyTokens()
    {
        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme()));

        Assert.Contains("--marilo-font-family", cut.Markup);
        Assert.Contains("--marilo-font-size-base", cut.Markup);
    }

    [Fact]
    public void RendersInlineStyles_WithShapeTokens()
    {
        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme()));

        Assert.Contains("--marilo-radius-md", cut.Markup);
        Assert.Contains("--marilo-radius-lg", cut.Markup);
        Assert.Contains("--marilo-shadow-sm", cut.Markup);
    }

    [Fact]
    public void RendersInlineStyles_WithDefaultColorValues()
    {
        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme()));

        // Default primary colour from MariloColorPalette
        Assert.Contains("#0078d4", cut.Markup);
        // Default font family from MariloTypographyScale
        Assert.Contains("Segoe UI", cut.Markup);
        // Default border radius from MariloShape
        Assert.Contains("4px", cut.Markup);
    }

    // ---------------------------------------------------------------
    // 3. data-marilo-theme="light" by default (TestThemeService.IsDarkMode = false)
    // ---------------------------------------------------------------
    [Fact]
    public void Sets_DataMariloTheme_Light_ByDefault()
    {
        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme()));

        var div = cut.Find("div.marilo-theme-provider");
        Assert.Equal("light", div.GetAttribute("data-marilo-theme"));
    }

    // ---------------------------------------------------------------
    // 3. data-marilo-theme="dark" when IsDarkMode = true
    // ---------------------------------------------------------------
    [Fact]
    public void Sets_DataMariloTheme_Dark_WhenDarkModeServiceIsUsed()
    {
        // Override the singleton registration with the dark-mode service
        Services.AddSingleton<IMariloThemeService, DarkModeThemeService>();

        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme()));

        var div = cut.Find("div.marilo-theme-provider");
        Assert.Equal("dark", div.GetAttribute("data-marilo-theme"));
    }

    // ---------------------------------------------------------------
    // 4. dir="rtl" when Theme.IsRtl = true
    // ---------------------------------------------------------------
    [Fact]
    public void Sets_DirRtl_WhenThemeIsRtlIsTrue()
    {
        var rtlTheme = new MariloTheme { IsRtl = true };

        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, rtlTheme));

        var div = cut.Find("div.marilo-theme-provider");
        Assert.Equal("rtl", div.GetAttribute("dir"));
    }

    [Fact]
    public void DoesNotSet_Dir_WhenThemeIsRtlIsFalse()
    {
        var ltrTheme = new MariloTheme { IsRtl = false };

        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, ltrTheme));

        var div = cut.Find("div.marilo-theme-provider");
        // Blazor omits the attribute entirely when the value is null
        Assert.Null(div.GetAttribute("dir"));
    }

    // ---------------------------------------------------------------
    // 5. Class, Style, and AdditionalAttributes pass through
    // ---------------------------------------------------------------
    [Fact]
    public void PassesThrough_ClassParameter()
    {
        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme())
            .Add(p => p.Class, "my-custom-class"));

        var div = cut.Find("div.marilo-theme-provider");
        Assert.Contains("my-custom-class", div.GetAttribute("class"));
    }

    [Fact]
    public void PassesThrough_StyleParameter()
    {
        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme())
            .Add(p => p.Style, "color: red;"));

        var div = cut.Find("div.marilo-theme-provider");
        Assert.Contains("color: red;", div.GetAttribute("style"));
    }

    [Fact]
    public void PassesThrough_AdditionalAttributes()
    {
        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme())
            .AddUnmatched("data-testid", "theme-root"));

        var div = cut.Find("div[data-testid='theme-root']");
        Assert.NotNull(div);
    }

    // ---------------------------------------------------------------
    // Bonus: ChildContent is rendered inside the wrapper
    // ---------------------------------------------------------------
    [Fact]
    public void RendersChildContent_InsideWrapper()
    {
        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, new MariloTheme())
            .Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.AddContent(0, "hello from child");
            })));

        Assert.Contains("hello from child", cut.Markup);
    }

    // ---------------------------------------------------------------
    // Custom theme values appear in inline styles
    // ---------------------------------------------------------------
    [Fact]
    public void RendersCustomColorValues_InInlineStyles()
    {
        var customTheme = new MariloTheme
        {
            Colors = new MariloColorPalette { Primary = "#ff0000" }
        };

        var cut = Render<MariloThemeProvider>(parameters => parameters
            .Add(p => p.Theme, customTheme));

        Assert.Contains("#ff0000", cut.Markup);
    }
}
