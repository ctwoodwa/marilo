namespace Marilo.Core.Models;

/// <summary>
/// A marker placed on a <c>MariloMap</c> at a geographic coordinate.
/// </summary>
public class MapMarker
{
    /// <summary>Tooltip or label for the marker.</summary>
    public string? Title { get; set; }

    /// <summary>Latitude coordinate (-90 to 90).</summary>
    public double Latitude { get; set; }

    /// <summary>Longitude coordinate (-180 to 180).</summary>
    public double Longitude { get; set; }
}

/// <summary>
/// Geographic center point for a <c>MariloMap</c>.
/// </summary>
public class MapCenter
{
    /// <summary>Latitude coordinate (-90 to 90).</summary>
    public double Latitude { get; set; }

    /// <summary>Longitude coordinate (-180 to 180).</summary>
    public double Longitude { get; set; }
}
