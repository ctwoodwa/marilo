using Marilo.Core.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Marilo.Providers.FluentUI;

public class FluentUIJsInterop : IMariloJsInterop
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _module;

    public FluentUIJsInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask InitializeAsync()
    {
        _module = await _jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Marilo.Providers.FluentUI/js/marilo-fluentui.js");
    }

    public async ValueTask<bool> ShowModalAsync(string modalId)
    {
        if (_module is null) await InitializeAsync();
        return await _module!.InvokeAsync<bool>("showModal", modalId);
    }

    public async ValueTask HideModalAsync(string modalId)
    {
        if (_module is null) await InitializeAsync();
        await _module!.InvokeVoidAsync("hideModal", modalId);
    }

    public async ValueTask<BoundingBox> GetElementBoundsAsync(ElementReference element)
    {
        if (_module is null) await InitializeAsync();
        return await _module!.InvokeAsync<BoundingBox>("getElementBounds", element);
    }

    public async ValueTask ObserveScrollAsync(ElementReference element, DotNetObjectReference<object> callback)
    {
        if (_module is null) await InitializeAsync();
        await _module!.InvokeVoidAsync("observeScroll", element, callback);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}
