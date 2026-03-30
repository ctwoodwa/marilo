---
uid: component-icon-appearance
title: Icon Appearance
description: Customize MariloIcon size, flip direction, and theme color.
---

# Icon Appearance

## Sizes

The `Size` parameter controls the rendered dimensions of the icon:

```razor
<MariloIcon Name="star" Size="IconSize.Small" />
<MariloIcon Name="star" Size="IconSize.Medium" />
<MariloIcon Name="star" Size="IconSize.Large" />
```

## Flip

Mirror the icon along an axis:

```razor
<MariloIcon Name="arrow-right" Flip="IconFlip.Horizontal" />
<MariloIcon Name="arrow-down" Flip="IconFlip.Vertical" />
```

## Theme colors

Apply semantic colors to the icon:

```razor
<MariloIcon Name="check-circle" ThemeColor="IconThemeColor.Success" />
<MariloIcon Name="alert-circle" ThemeColor="IconThemeColor.Error" />
```

`IconThemeColor.Base` (the default) inherits the current text color from the parent element.

## See Also

- [Icon Overview](xref:component-icon-overview)
