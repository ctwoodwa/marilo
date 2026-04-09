using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace Marilo.Providers.Bootstrap;

public class BootstrapIconProvider : IMariloIconProvider
{
    private const string SpriteUrl = "_content/Marilo.Icons/icons/sprite.svg";

    /// <inheritdoc />
    public IconRenderMode RenderMode => IconRenderMode.SvgSprite;

    /// <inheritdoc />
    public string LibraryName => "Bootstrap";

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
        // Marilo Icons sprite uses "marilo-" prefix for all icon IDs
        var iconId = name.StartsWith("marilo-") ? name : $"marilo-{name}";
        return new MarkupString($"""<svg class="mar-icon mar-icon--{size.ToString().ToLower()}" width="{px}" height="{px}" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><use href="{SpriteUrl}#{iconId}"></use></svg>""");
    }

    public string GetIconSpriteUrl() => SpriteUrl;
}
