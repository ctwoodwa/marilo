using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace Marilo.Providers.Material;

public class MaterialIconProvider : IMariloIconProvider
{
    // Material reuses the Tabler icon sprite (same as FluentUI)
    // A dedicated Material icon set can be added later
    private const string SpriteUrl = "_content/Marilo.Providers.FluentUI/icons/fluent-icons.svg";

    public IconRenderMode RenderMode => IconRenderMode.SvgSprite;
    public string LibraryName => "Material";

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

    public string GetIconSpriteUrl() => SpriteUrl;
}
