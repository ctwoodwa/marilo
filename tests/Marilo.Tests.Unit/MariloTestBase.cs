using Bunit;
using Marilo.Core.Configuration;
using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Marilo.Core.Services;
using Marilo.Providers.FluentUI;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo.Tests.Unit;

/// <summary>
/// Base class for Marilo component tests. Registers the FluentUI provider
/// and core services so components can render without additional setup.
/// </summary>
public abstract class MariloTestBase : TestContext
{
    protected MariloTestBase()
    {
        Services.AddSingleton<IMariloCssProvider, FluentUICssProvider>();
        Services.AddSingleton<IMariloThemeService, TestThemeService>();
        Services.AddSingleton<IMariloIconProvider, TestIconProvider>();
        Services.AddSingleton<IMariloNotificationService, MariloNotificationService>();
    }

    private class TestThemeService : IMariloThemeService
    {
        public MariloTheme CurrentTheme { get; } = new();
        public bool IsDarkMode => false;
        public bool IsRtl => false;
        public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;

        public Task SetThemeAsync(MariloTheme theme) => Task.CompletedTask;
        public Task ToggleDarkModeAsync() => Task.CompletedTask;
        public Task InitializeAsync() => Task.CompletedTask;
    }

    private class TestIconProvider : IMariloIconProvider
    {
        public MarkupString GetIcon(string name, IconSize size = IconSize.Medium) =>
            new($"<span data-icon=\"{name}\"></span>");

        public string GetIconSpriteUrl() => "/icons/sprite.svg";
    }
}
