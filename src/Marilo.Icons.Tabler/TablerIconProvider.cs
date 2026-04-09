using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace Marilo.Icons.Tabler;

/// <summary>
/// Icon provider that renders Tabler Icons from an SVG sprite sheet.
/// Tabler Icons are MIT licensed — see <see href="https://tabler.io/icons"/>.
/// </summary>
public sealed class TablerIconProvider : IMariloIconProvider
{
    private const string SpriteUrl = "_content/Marilo.Icons.Tabler/icons/tabler-sprite.svg";

    /// <inheritdoc />
    public IconRenderMode RenderMode => IconRenderMode.SvgSprite;

    /// <inheritdoc />
    public string LibraryName => "Tabler";

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

        // Tabler sprite symbol IDs use "tabler-" prefix; add it if not already present.
        var iconId = name.StartsWith("tabler-", StringComparison.Ordinal) ? name : $"tabler-{name}";

        return new MarkupString(
            $"""<svg class="mar-icon mar-icon--{size.ToString().ToLower()}" width="{px}" height="{px}" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true" focusable="false"><use href="{SpriteUrl}#{iconId}"></use></svg>""");
    }

    /// <inheritdoc />
    public string GetIconSpriteUrl() => SpriteUrl;
}
