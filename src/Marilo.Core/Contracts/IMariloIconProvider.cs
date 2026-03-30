using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace Marilo.Core.Contracts;

/// <summary>
/// Provides design-system-specific icon rendering. Implementations resolve icon
/// names to the appropriate markup (inline SVG, sprite reference, icon font, etc.).
/// </summary>
public interface IMariloIconProvider
{
    /// <summary>
    /// Returns the markup for the requested icon at the given size.
    /// </summary>
    /// <param name="name">The logical icon name (e.g., "home", "settings").</param>
    /// <param name="size">The desired icon size.</param>
    /// <returns>A <see cref="MarkupString"/> containing the rendered icon markup.</returns>
    MarkupString GetIcon(string name, IconSize size = IconSize.Medium);

    /// <summary>
    /// Gets the URL of the SVG sprite sheet used by this icon provider.
    /// </summary>
    /// <returns>A relative or absolute URL pointing to the sprite SVG file.</returns>
    string GetIconSpriteUrl();
}
