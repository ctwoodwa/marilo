using Marilo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo.Components.Internal.Interop;

/// <summary>
/// Registers shared interop services on the <see cref="MariloBuilder"/>.
/// </summary>
public static class InteropServiceExtensions
{
    /// <summary>
    /// Adds the shared JS interop infrastructure used by complex Marilo components
    /// (Window, Popover, Splitter, DataGrid, Chart, Editor, etc.).
    /// Call after <c>AddMariloCoreServices()</c>.
    /// </summary>
    public static MariloBuilder AddMariloInteropServices(this MariloBuilder builder)
    {
        var services = builder.Services;

        // Module loader (scoped — one per circuit/connection)
        services.AddScoped<IMariloJsModuleLoader, MariloJsModuleLoader>();

        // Measurement & observation
        services.AddScoped<IElementMeasurementService, ElementMeasurementService>();
        services.AddScoped<IResizeObserverService, ResizeObserverService>();
        services.AddScoped<IIntersectionObserverService, IntersectionObserverService>();

        // Positioning
        services.AddScoped<IPopupPositionService, PopupPositionService>();

        // Drag & resize interactions
        services.AddScoped<IDragService, DragService>();
        services.AddScoped<IResizeInteractionService, ResizeInteractionService>();

        // Clipboard & download
        services.AddScoped<IClipboardService, ClipboardService>();
        services.AddScoped<IDownloadService, DownloadService>();

        // Graphics (charts, diagrams, maps)
        services.AddScoped<IGraphicsInteropService, GraphicsInteropService>();

        // Drop zones (FileUpload, Upload)
        services.AddScoped<IDropZoneService, DropZoneService>();

        return builder;
    }
}
