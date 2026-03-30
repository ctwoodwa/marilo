using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace Marilo.Icons;

public class MariloIconProvider : IMariloIconProvider
{
    private const string SpriteUrl = "_content/Marilo.Icons/icons/sprite.svg";

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
