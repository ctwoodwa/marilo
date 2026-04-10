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
    public async Task AddMariloInteropServices_Registers_ModuleLoader()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var loader = scope.ServiceProvider.GetService<IMariloJsModuleLoader>();
        Assert.NotNull(loader);
    }

    [Fact]
    public async Task AddMariloInteropServices_Registers_ElementMeasurementService()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var svc = scope.ServiceProvider.GetService<IElementMeasurementService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public async Task AddMariloInteropServices_Registers_ResizeObserverService()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var svc = scope.ServiceProvider.GetService<IResizeObserverService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public async Task AddMariloInteropServices_Registers_IntersectionObserverService()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var svc = scope.ServiceProvider.GetService<IIntersectionObserverService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public async Task AddMariloInteropServices_Registers_PopupPositionService()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var svc = scope.ServiceProvider.GetService<IPopupPositionService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public async Task AddMariloInteropServices_Registers_DragService()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var svc = scope.ServiceProvider.GetService<IDragService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public async Task AddMariloInteropServices_Registers_ResizeInteractionService()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var svc = scope.ServiceProvider.GetService<IResizeInteractionService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public async Task AddMariloInteropServices_Registers_ClipboardService()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var svc = scope.ServiceProvider.GetService<IClipboardService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public async Task AddMariloInteropServices_Registers_DownloadService()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var svc = scope.ServiceProvider.GetService<IDownloadService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public async Task AddMariloInteropServices_Registers_GraphicsInteropService()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var svc = scope.ServiceProvider.GetService<IGraphicsInteropService>();
        Assert.NotNull(svc);
    }

    [Fact]
    public async Task ModuleLoader_Is_Scoped_Different_Scopes_Get_Different_Instances()
    {
        await using var provider = BuildProvider();
        await using var scope1 = provider.CreateAsyncScope();
        await using var scope2 = provider.CreateAsyncScope();
        var loader1 = scope1.ServiceProvider.GetService<IMariloJsModuleLoader>();
        var loader2 = scope2.ServiceProvider.GetService<IMariloJsModuleLoader>();
        Assert.NotSame(loader1, loader2);
    }

    [Fact]
    public async Task ModuleLoader_DisposeAsync_Does_Not_Throw()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
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
