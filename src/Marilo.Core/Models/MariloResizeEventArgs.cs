using Marilo.Core.Enums;

namespace Marilo.Core.Models;

/// <summary>
/// Event arguments for resize start, resizing, and resize end events
/// raised by <see cref="MariloResizableContainer"/>.
/// </summary>
public sealed class MariloResizeEventArgs
{
    /// <summary>The current width as a CSS value (e.g., "400px").</summary>
    public string Width { get; init; } = default!;

    /// <summary>The current height as a CSS value (e.g., "300px").</summary>
    public string Height { get; init; } = default!;

    /// <summary>The current width in pixels.</summary>
    public double WidthPixels { get; init; }

    /// <summary>The current height in pixels.</summary>
    public double HeightPixels { get; init; }

    /// <summary>The edge or corner being dragged.</summary>
    public MariloResizeEdges ActiveEdge { get; init; }

    /// <summary>Whether this resize was initiated by a user action (drag or keyboard).</summary>
    public bool IsUserInitiated { get; init; }
}

/// <summary>
/// Event arguments raised when a ResizeObserver detects a size change on
/// the resizable container, including changes not caused by user drag.
/// </summary>
public sealed class MariloObservedSizeChangedEventArgs
{
    /// <summary>The observed width as a CSS value.</summary>
    public string Width { get; init; } = default!;

    /// <summary>The observed height as a CSS value.</summary>
    public string Height { get; init; } = default!;

    /// <summary>The observed width in pixels.</summary>
    public double WidthPixels { get; init; }

    /// <summary>The observed height in pixels.</summary>
    public double HeightPixels { get; init; }
}
