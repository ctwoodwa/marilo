using Microsoft.AspNetCore.Components;

namespace Marilo.Components.Internal.Interop;

/// <summary>
/// Calculates popup position relative to an anchor element, handling viewport overflow and flipping.
/// </summary>
internal interface IPopupPositionService
{
    /// <summary>
    /// Computes the best position for a popup anchored to the given element.
    /// </summary>
    ValueTask<PopupPositionResult> ComputePositionAsync(
        ElementReference anchor,
        ElementReference popup,
        PopupAnchorOptions options,
        CancellationToken cancellationToken = default);
}
