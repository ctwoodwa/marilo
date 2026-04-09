using Marilo.Components.Internal.Interop;
using Marilo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Xunit;

namespace Marilo.Tests.Unit.Interop;

/// <summary>
/// Tests for shared interop service DI registration, construction, and disposal.
/// </summary>
public class SharedInteropServiceTests
{
    private static ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();
        services.AddScoped<IJSRuntime>(_ => new TestJSRuntime());
        services.AddMarilo().AddMariloCoreServices().AddMariloInteropServices();
        return services.BuildServiceProvider();
    }

    [Fact]
    public void AddMariloInteropServices_Registers_ModuleLoader()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var loader = scope.ServiceProvider.GetService<IMariloJsModuleLoader>();
        Assert.NotNull(loader);
    }

    [Fact]
    public void AddMariloInteropServices_Registers_ElementMeasurementService()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var svc = scope.ServiceProvider.GetService<IElementMeasurementService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public void AddMariloInteropServices_Registers_ResizeObserverService()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var svc = scope.ServiceProvider.GetService<IResizeObserverService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public void AddMariloInteropServices_Registers_IntersectionObserverService()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var svc = scope.ServiceProvider.GetService<IIntersectionObserverService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public void AddMariloInteropServices_Registers_PopupPositionService()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var svc = scope.ServiceProvider.GetService<IPopupPositionService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public void AddMariloInteropServices_Registers_DragService()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var svc = scope.ServiceProvider.GetService<IDragService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public void AddMariloInteropServices_Registers_ResizeInteractionService()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var svc = scope.ServiceProvider.GetService<IResizeInteractionService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public void AddMariloInteropServices_Registers_ClipboardService()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var svc = scope.ServiceProvider.GetService<IClipboardService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public void AddMariloInteropServices_Registers_DownloadService()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var svc = scope.ServiceProvider.GetService<IDownloadService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public void AddMariloInteropServices_Registers_GraphicsInteropService()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var svc = scope.ServiceProvider.GetService<IGraphicsInteropService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public void ModuleLoader_Is_Scoped_Different_Scopes_Get_Different_Instances()
    {
        using var provider = BuildProvider();
        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();
        var loader1 = scope1.ServiceProvider.GetService<IMariloJsModuleLoader>();
        var loader2 = scope2.ServiceProvider.GetService<IMariloJsModuleLoader>();
        Assert.NotSame(loader1, loader2);
    }

    [Fact]
    public async Task ModuleLoader_DisposeAsync_Does_Not_Throw()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();
        var loader = scope.ServiceProvider.GetRequiredService<IMariloJsModuleLoader>();
        // Dispose should be safe even with no modules imported
        await loader.DisposeAsync();
    }

    /// <summary>
    /// Minimal IJSRuntime stub for DI testing.
    /// </summary>
    private class TestJSRuntime : IJSRuntime
    {
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
            => default;

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
            => default;
    }
}
