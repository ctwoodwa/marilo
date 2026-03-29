using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace Marilo.Core.Contracts;

public interface IMariloIconProvider
{
    MarkupString GetIcon(string name, IconSize size = IconSize.Medium);
    string GetIconSpriteUrl();
}
