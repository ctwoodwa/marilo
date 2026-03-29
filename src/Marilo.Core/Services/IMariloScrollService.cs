using Microsoft.AspNetCore.Components;

namespace Marilo.Core.Services;

public interface IMariloScrollService
{
    ValueTask ScrollToAsync(string elementId, bool smooth = true);
    ValueTask ScrollToAsync(ElementReference element, bool smooth = true);
    ValueTask ScrollToTopAsync(bool smooth = true);
}
