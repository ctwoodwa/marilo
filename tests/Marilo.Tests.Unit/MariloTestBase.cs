using Bunit;
using Marilo.Components.Internal.Interop;
using Marilo.Core.Configuration;
using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Marilo.Core.Extensions;
using Marilo.Core.Models;
using Marilo.Core.Services;
using Marilo.Providers.FluentUI;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Marilo.Tests.Unit;

/// <summary>
/// Base class for Marilo component tests. Registers the FluentUI provider
/// and core services so components can render without additional setup.
/// </summary>
public abstract class MariloTestBase : BunitContext
{
    protected MariloTestBase()
    {
        Services.AddSingleton<IMariloCssProvider, FluentUICssProvider>();
        Services.AddSingleton<IMariloThemeService, TestThemeService>();
        Services.AddSingleton<IMariloIconProvider, TestIconProvider>();
        Services.AddSingleton<IMariloNotificationService, MariloNotificationService>();
        Services.AddMarilo().AddMariloInteropServices();

        // Components use JS.InvokeAsync<IJSObjectReference>("eval", ...) to
        // create inline JS modules. Use loose mode so unhandled JS calls
        // return default values instead of throwing.
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    private class TestThemeService : IMariloThemeService
    {
        public MariloTheme CurrentTheme { get; } = new();
        public bool IsDarkMode => false;
        public bool IsRtl => false;
#pragma warning disable CS0067 // Required by IMariloThemeService interface
        public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;
#pragma warning restore CS0067

        public Task SetThemeAsync(MariloTheme theme) => Task.CompletedTask;
        public Task ToggleDarkModeAsync() => Task.CompletedTask;
        public Task SetDarkModeAsync(bool dark) => Task.CompletedTask;
        public Task InitializeAsync() => Task.CompletedTask;
    }

    private class TestIconProvider : IMariloIconProvider
    {
        public MarkupString GetIcon(string name, IconSize size = IconSize.Medium) =>
            new($"<span data-icon=\"{name}\"></span>");

        public string GetIconSpriteUrl() => "/icons/sprite.svg";
        public IconRenderMode RenderMode => IconRenderMode.SvgSprite;
        public string LibraryName => "Test";
    }
}
