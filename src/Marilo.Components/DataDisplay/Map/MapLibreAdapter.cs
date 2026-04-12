using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Marilo.Components.DataDisplay.Map;

/// <summary>
/// MapLibre GL JS adapter implementing <see cref="IMapEngineAdapter"/>.
/// Owns the JS module reference and translates adapter calls into JS interop calls.
/// Wave 2: stub implementation — real MapLibre integration is Wave 3.
/// </summary>
internal sealed class MapLibreAdapter : IMapEngineAdapter
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _jsModule;
    private DotNetObjectReference<MapLibreAdapter>? _dotNetRef;
    private bool _disposed;

    public MapLibreAdapter(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    /// <inheritdoc />
    public async Task InitializeAsync(ElementReference container, MapInitOptions options)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./_content/Marilo.Components/js/marilo-map.js");

        _dotNetRef = DotNetObjectReference.Create(this);

        await _jsModule.InvokeVoidAsync("init", container, _dotNetRef, new
        {
            centerLat = options.CenterLat,
            centerLng = options.CenterLng,
            zoom = options.Zoom,
            minZoom = options.MinZoom,
            maxZoom = options.MaxZoom,
            zoomable = options.Zoomable,
            pannable = options.Pannable
        });
    }

    /// <inheritdoc />
    public async Task UpdateViewportAsync(double centerLat, double centerLng, double zoom)
    {
        if (_jsModule is null) return;
        await _jsModule.InvokeVoidAsync("updateViewport", centerLat, centerLng, zoom);
    }

    /// <inheritdoc />
    public async Task AddLayerAsync(MapLayerDescriptor layer)
    {
        if (_jsModule is null) return;
        await _jsModule.InvokeVoidAsync("addLayer", layer);
    }

    /// <inheritdoc />
    public async Task RemoveLayerAsync(string layerId)
    {
        if (_jsModule is null) return;
        await _jsModule.InvokeVoidAsync("removeLayer", layerId);
    }

    /// <inheritdoc />
    public async Task UpdateLayerAsync(MapLayerDescriptor layer)
    {
        if (_jsModule is null) return;
        await _jsModule.InvokeVoidAsync("updateLayer", layer);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("destroy");
                await _jsModule.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Circuit may already be gone in Blazor Server.
            }
        }

        _dotNetRef?.Dispose();
    }
}
