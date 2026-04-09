using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace Marilo.Icons;

/// <summary>
/// Legacy icon provider that renders icons from the Marilo custom SVG sprite sheet.
/// For new projects, prefer <c>TablerIconProvider</c> from the <c>Marilo.Icons.Tabler</c> package.
/// </summary>
public class MariloIconProvider : IMariloIconProvider
{
    private const string SpriteUrl = "_content/Marilo.Icons/icons/sprite.svg";

    /// <inheritdoc />
    public IconRenderMode RenderMode => IconRenderMode.SvgSprite;

    /// <inheritdoc />
    public string LibraryName => "MariloCustom";

    /// <inheritdoc />
    public MarkupString GetIcon(string name, IconSize size = IconSize.Medium)
    {
        var px = size switch
        {
            IconSize.Small => "16",
            IconSize.Medium => "20",
            IconSize.Large => "24",
            IconSize.ExtraLarge => "32",
            _ => "20"
        };
        return new MarkupString($"""<svg class="mar-icon mar-icon--{size.ToString().ToLower()}" width="{px}" height="{px}" aria-hidden="true"><use href="{SpriteUrl}#{name}"></use></svg>""");
    }

    /// <inheritdoc />
    public string GetIconSpriteUrl() => SpriteUrl;
}
