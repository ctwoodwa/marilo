using Microsoft.AspNetCore.Components;

namespace Marilo.Components.Internal.Interop;

internal sealed class ElementMeasurementService : IElementMeasurementService
{
    private readonly IMariloJsModuleLoader _loader;

    public ElementMeasurementService(IMariloJsModuleLoader loader)
    {
        _loader = loader;
    }

    public async ValueTask<ElementRect> GetBoundingClientRectAsync(ElementReference element, CancellationToken cancellationToken = default)
    {
        var module = await _loader.ImportAsync("js/marilo-measurement.js", cancellationToken);
        return await module.InvokeAsync<ElementRect>("getBoundingClientRect", cancellationToken, element);
    }

    public async ValueTask<ViewportRect> GetViewportAsync(CancellationToken cancellationToken = default)
    {
        var module = await _loader.ImportAsync("js/marilo-measurement.js", cancellationToken);
        return await module.InvokeAsync<ViewportRect>("getViewport", cancellationToken);
    }
}
