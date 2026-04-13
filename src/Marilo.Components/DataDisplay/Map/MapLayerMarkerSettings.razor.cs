using Marilo.Core.Base;
using Microsoft.AspNetCore.Components;

namespace Marilo.Components.DataDisplay.Map;

public partial class MapLayerMarkerSettings : MariloComponentBase
{
    /// <summary>The JS function name used as a marker template.</summary>
    [Parameter] public string? Template { get; set; }

    [CascadingParameter] internal IMapLayerSettingsHost? ParentLayer { get; set; }

    protected override void OnInitialized()
    {
        ParentLayer?.RegisterMarkerSettings(this);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) ParentLayer?.UnregisterMarkerSettings(this);
        base.Dispose(disposing);
    }
}
