using Marilo.Core.Configuration;
using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace Marilo.Icons;

/// <summary>
/// Reusable icon provider base for CSS/font-based icon libraries
/// (e.g., Bootstrap Icons, Font Awesome). Renders icons as
/// <c>&lt;i class="{prefix} {prefix}-{name}"&gt;</c> elements.
/// </summary>
public class CssIconProvider : IMariloIconProvider
{
    private readonly IconOptions _options;

    /// <summary>
    /// Initializes a new <see cref="CssIconProvider"/> with the specified options.
    /// </summary>
    /// <param name="options">Configuration specifying the CSS class prefix and library name.</param>
    public CssIconProvider(IconOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));

        if (string.IsNullOrWhiteSpace(options.CssClassPrefix))
        {
            throw new ArgumentException(
                "CssClassPrefix is required for CssClass render mode.", nameof(options));
        }
    }

    /// <inheritdoc />
    public IconRenderMode RenderMode => IconRenderMode.CssClass;

    /// <inheritdoc />
    public string LibraryName => _options.LibraryName;

    /// <inheritdoc />
    public MarkupString GetIcon(string name, IconSize size = IconSize.Medium)
    {
        var px = size switch
        {
            IconSize.Small      => "16",
            IconSize.Medium     => "20",
            IconSize.Large      => "24",
            IconSize.ExtraLarge => "32",
            _                   => "20"
        };

        var prefix = _options.CssClassPrefix;
        return new MarkupString(
            $"""<i class="{prefix} {prefix}-{name} mar-icon mar-icon--{size.ToString().ToLower()}" style="font-size:{px}px" aria-hidden="true"></i>""");
    }

    /// <inheritdoc />
    public string GetIconSpriteUrl() => string.Empty;
}
