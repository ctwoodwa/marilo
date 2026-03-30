---
uid: component-tooltip-appearance
title: Tooltip Appearance
description: Customize MariloTooltip position and presentation.
---

# Tooltip Appearance

## Position

The `Position` parameter controls where the tooltip appears relative to its trigger content:

```razor
<MariloTooltip Text="Top" Position="TooltipPosition.Top">
    <span>Hover me</span>
</MariloTooltip>

<MariloTooltip Text="Bottom" Position="TooltipPosition.Bottom">
    <span>Hover me</span>
</MariloTooltip>

<MariloTooltip Text="Left" Position="TooltipPosition.Left">
    <span>Hover me</span>
</MariloTooltip>

<MariloTooltip Text="Right" Position="TooltipPosition.Right">
    <span>Hover me</span>
</MariloTooltip>
```

## See Also

- [Tooltip Overview](xref:component-tooltip-overview)
