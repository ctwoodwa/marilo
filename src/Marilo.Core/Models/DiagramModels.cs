namespace Marilo.Core.Models;

/// <summary>
/// Represents a node in a <c>MariloDiagram</c>.
/// </summary>
public class DiagramNode
{
    /// <summary>Unique identifier for the node.</summary>
    public string Id { get; set; } = "";

    /// <summary>Text displayed inside the node shape.</summary>
    public string? Text { get; set; }

    /// <summary>X position (pixels) within the diagram canvas.</summary>
    public double X { get; set; }

    /// <summary>Y position (pixels) within the diagram canvas.</summary>
    public double Y { get; set; }

    /// <summary>Width of the node shape in pixels.</summary>
    public double Width { get; set; } = 100;

    /// <summary>Height of the node shape in pixels.</summary>
    public double Height { get; set; } = 60;

    /// <summary>Shape type (e.g., "rectangle", "ellipse").</summary>
    public string? Shape { get; set; } = "rectangle";
}

/// <summary>
/// Represents a directed edge between two <see cref="DiagramNode"/> items.
/// </summary>
public class DiagramEdge
{
    /// <summary>Unique identifier for the edge.</summary>
    public string Id { get; set; } = "";

    /// <summary>Id of the source node.</summary>
    public string FromId { get; set; } = "";

    /// <summary>Id of the target node.</summary>
    public string ToId { get; set; } = "";

    /// <summary>Optional label displayed along the edge.</summary>
    public string? Text { get; set; }
}
