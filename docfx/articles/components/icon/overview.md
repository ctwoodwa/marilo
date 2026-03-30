---
uid: component-icon-overview
title: Icon
description: The MariloIcon component renders SVG icons by name, render fragment, or inline child content.
---

# Icon

## Overview

The `MariloIcon` component renders an SVG icon inside a `<span>` element. It supports three ways to specify the icon: by `Name` (resolved through `IMariloIconProvider`), by a predefined `Icon` render fragment, or via inline `ChildContent` markup. It includes size, flip, theme color, and accessibility options.

## Creating an Icon

````razor
@* By name -- resolved by the icon provider *@
<MariloIcon Name="home" />

@* By render fragment *@
<MariloIcon Icon="@myCustomIcon" />

@* By inline SVG *@
<MariloIcon>
    <svg viewBox="0 0 24 24"><path d="M12 2L2 22h20L12 2z"/></svg>
</MariloIcon>
````

## Features

- **Named icons** -- Pass an icon name and the registered `IMariloIconProvider` resolves it to SVG markup.
- **Render fragment icons** -- Use the `Icon` parameter to pass a pre-built `RenderFragment` (takes precedence over `Name`).
- **Inline SVG** -- Place custom SVG directly as `ChildContent`.
- **Sizes** -- Small, Medium, Large, and ExtraLarge via `IconSize`. See [Appearance](appearance.md).
- **Flip** -- Mirror the icon horizontally or vertically via `IconFlip`.
- **Theme colors** -- Apply semantic colors via `IconThemeColor`.
- **Accessibility** -- Set `AriaLabel` to give the icon an accessible name; when set, `role="img"` is used instead of `role="presentation"`.
- **Validation** -- The component throws if none of `Name`, `Icon`, or `ChildContent` is provided.

## Parameters

| Name | Type | Default | Description |
|---|---|---|---|
| `Name` | `string?` | `null` | The name of a predefined icon from the icon provider. |
| `Icon` | `RenderFragment?` | `null` | A predefined icon render fragment. Takes precedence over `Name` and `ChildContent`. |
| `ChildContent` | `RenderFragment?` | `null` | Custom SVG markup rendered as the icon content. |
| `Size` | `IconSize` | `IconSize.Medium` | The size of the icon. |
| `Flip` | `IconFlip` | `IconFlip.None` | The flip direction (None, Horizontal, Vertical). |
| `ThemeColor` | `IconThemeColor` | `IconThemeColor.Base` | The theme color applied to the icon. Base inherits the current text color. |
| `AriaLabel` | `string?` | `null` | Accessible label. When set, the icon uses `role="img"` instead of `role="presentation"`. |

## See Also

- [API Reference](xref:Marilo.Components.Utility.MariloIcon)
