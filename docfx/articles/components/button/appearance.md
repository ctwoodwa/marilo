---
uid: component-button-appearance
title: Button Appearance
description: Customize the visual appearance of MariloButton with variants, sizes, and outline mode.
---

# Button Appearance

## Variants

The `Variant` parameter controls the button's color scheme:

```razor
<MariloButton Variant="ButtonVariant.Primary">Primary</MariloButton>
<MariloButton Variant="ButtonVariant.Secondary">Secondary</MariloButton>
<MariloButton Variant="ButtonVariant.Danger">Danger</MariloButton>
```

## Sizes

The `Size` parameter controls padding and font size:

```razor
<MariloButton Size="ButtonSize.Small">Small</MariloButton>
<MariloButton Size="ButtonSize.Medium">Medium</MariloButton>
<MariloButton Size="ButtonSize.Large">Large</MariloButton>
```

## Outline mode

Set `IsOutline="true"` for a button with a transparent background and a visible border:

```razor
<MariloButton Variant="ButtonVariant.Primary" IsOutline="true">Outline</MariloButton>
```

## Disabled state

```razor
<MariloButton Disabled="true">Disabled</MariloButton>
```

## See Also

- [Button Overview](xref:component-button-overview)
