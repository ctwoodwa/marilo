using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Marilo.Core.Contracts;

public interface IMariloJsInterop : IAsyncDisposable
{
    ValueTask InitializeAsync();
    ValueTask<bool> ShowModalAsync(string modalId);
    ValueTask HideModalAsync(string modalId);
    ValueTask<BoundingBox> GetElementBoundsAsync(ElementReference element);
    ValueTask ObserveScrollAsync(ElementReference element, DotNetObjectReference<object> callback);
}

public record BoundingBox(double X, double Y, double Width, double Height);
