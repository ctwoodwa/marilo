using Marilo.Core.Configuration;
using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace Marilo.Icons;

/// <summary>
/// Icon provider that renders icons from a user-supplied SVG sprite sheet.
/// Use this provider with <c>AddMariloIconsCustom</c> to integrate a
/// proprietary or third-party sprite-based icon set.
/// </summary>
public sealed class CustomSpriteIconProvider : IMariloIconProvider
{
    private readonly IconOptions _options;

    /// <summary>
    /// Initializes a new <see cref="CustomSpriteIconProvider"/> with the specified options.
    /// </summary>
    /// <param name="options">Configuration specifying the sprite URL and library name.</param>
    public CustomSpriteIconProvider(IconOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));

        if (string.IsNullOrWhiteSpace(options.SpriteUrl))
        {
            throw new ArgumentException(
                "SpriteUrl is required for SvgSprite render mode.", nameof(options));
        }
    }

    /// <inheritdoc />
    public IconRenderMode RenderMode => IconRenderMode.SvgSprite;

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

        return new MarkupString(
            $"""<svg class="mar-icon mar-icon--{size.ToString().ToLower()}" width="{px}" height="{px}" aria-hidden="true" focusable="false"><use href="{_options.SpriteUrl}#{name}"></use></svg>""");
    }

    /// <inheritdoc />
    public string GetIconSpriteUrl() => _options.SpriteUrl!;
}
