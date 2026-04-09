using System.Collections.Concurrent;
using Microsoft.JSInterop;

namespace Marilo.Components.Internal.Interop;

/// <summary>
/// Lazily imports and caches JS ES modules from the Marilo.Components static content path.
/// </summary>
internal sealed class MariloJsModuleLoader : IMariloJsModuleLoader
{
    private const string ContentPrefix = "./_content/Marilo.Components/";
    private readonly IJSRuntime _jsRuntime;
    private readonly ConcurrentDictionary<string, Task<IJSObjectReference>> _modules = new();
    private bool _disposed;

    public MariloJsModuleLoader(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask<IJSObjectReference> ImportAsync(string modulePath, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var fullPath = ContentPrefix + modulePath;
        var task = _modules.GetOrAdd(fullPath, path =>
            _jsRuntime.InvokeAsync<IJSObjectReference>("import", cancellationToken, path).AsTask());

        return await task;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        foreach (var kvp in _modules)
        {
            try
            {
                if (kvp.Value.IsCompletedSuccessfully)
                {
                    var module = kvp.Value.Result;
                    await module.DisposeAsync();
                }
            }
            catch (JSDisconnectedException)
            {
                // Circuit disconnected; safe to ignore.
            }
        }

        _modules.Clear();
    }
}
