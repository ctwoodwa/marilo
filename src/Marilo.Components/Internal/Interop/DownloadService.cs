using Microsoft.JSInterop;

namespace Marilo.Components.Internal.Interop;

internal sealed class DownloadService : IDownloadService
{
    private readonly IMariloJsModuleLoader _loader;

    public DownloadService(IMariloJsModuleLoader loader)
    {
        _loader = loader;
    }

    public async ValueTask DownloadAsync(DownloadRequest request, CancellationToken cancellationToken = default)
    {
        var module = await _loader.ImportAsync("js/marilo-clipboard-download.js", cancellationToken);
        await module.InvokeVoidAsync("download", cancellationToken, request);
    }
}
