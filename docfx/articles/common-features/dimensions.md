---
uid: common-features-dimensions
title: Dimensions
description: How to control the size and dimensions of Marilo components.
---

# Dimensions

Marilo components provide a consistent set of parameters for controlling their physical size. Most components expand to fill their container by default and accept explicit overrides through `Width`, `Height`, and provider-aware CSS custom properties.

## Width and Height Parameters

The majority of Marilo components accept `Width` and `Height` as string parameters. These accept any valid CSS size value.

```razor
<!-- Fixed pixel width -->
<MariloTextBox Width="300px" />

<!-- Percentage of container -->
<MariloDropDown Width="100%" />

<!-- Viewport unit -->
<MariloDataGrid Height="60vh" />

<!-- CSS calc() -->
<MariloMultiSelect Width="calc(100% - 48px)" />
```

These values are applied as inline `style` attributes on the component root element. You can combine them with the `Style` parameter for additional inline styles:

```razor
<MariloDataGrid
    Width="100%"
    Height="400px"
    Style="border-radius: var(--marilo-radius-medium);" />
```

## Responsive Behavior

By default, most block-level Marilo components use `width: 100%` and inherit height from their content. This means they adapt to their container without any explicit sizing.

To constrain a component within a flexible layout, wrap it or set `Width` to a specific value:

```razor
<div style="display: flex; gap: 16px;">
    <MariloTextBox Width="240px" Placeholder="Search..." />
    <MariloButton>Search</MariloButton>
</div>
```

For responsive grid layouts, rely on the container's grid or flex rules and leave `Width` unset:

```razor
<div style="display: grid; grid-template-columns: 1fr 1fr; gap: 16px;">
    <MariloTextBox @bind-Value="model.FirstName" />
    <MariloTextBox @bind-Value="model.LastName" />
</div>
```

## CSS Custom Properties

Marilo exposes a set of `--marilo-*` spacing tokens that you can use for consistent sizing across components. These are set at the `:root` level and adapt to the active provider.

| Token | Purpose |
|---|---|
| `--marilo-spacing-xs` | Extra-small gap (4 px) |
| `--marilo-spacing-sm` | Small gap (8 px) |
| `--marilo-spacing-md` | Medium gap (16 px) |
| `--marilo-spacing-lg` | Large gap (24 px) |
| `--marilo-spacing-xl` | Extra-large gap (32 px) |

Use these tokens in `Width`, `Height`, or custom container styles to align sizing with the rest of the design system:

```razor
<MariloCard Style="padding: var(--marilo-spacing-md);">
    <MariloTextBox Width="100%" />
</MariloCard>
```

## Component-Specific Sizing

Some components have their own discrete size scales rather than free-form strings.

### Button Sizes

`MariloButton` accepts a `Size` parameter of type `ButtonSize`:

```razor
<MariloButton Size="ButtonSize.Small">Small</MariloButton>
<MariloButton Size="ButtonSize.Medium">Medium</MariloButton>
<MariloButton Size="ButtonSize.Large">Large</MariloButton>
```

### Avatar Sizes

`MariloAvatar` accepts a `Size` parameter of type `AvatarSize`:

```razor
<MariloAvatar Name="Jane Smith" Size="AvatarSize.Small" />
<MariloAvatar Name="Jane Smith" Size="AvatarSize.Medium" />
<MariloAvatar Name="Jane Smith" Size="AvatarSize.Large" />
```

### Icon Sizes

`MariloIcon` accepts `IconSize.Small` (16 px), `IconSize.Medium` (20 px), `IconSize.Large` (24 px), and `IconSize.ExtraLarge` (32 px). See <xref:common-features-icons> for details.

### DataGrid Row Height

`MariloDataGrid` exposes a `RowHeight` parameter (integer, pixels) that controls the height of each data row. The default is 36 px.

```razor
<MariloDataGrid TItem="Order" RowHeight="48" OnRead="@LoadOrders">
    ...
</MariloDataGrid>
```

## Setting Width and Height via Style Parameter

For cases where `Width` and `Height` are not available on a particular component, use the `Style` parameter directly:

```razor
<MariloCard Style="width: 320px; min-height: 200px;">
    ...
</MariloCard>
```

All Marilo components inherit from `MariloComponentBase`, which merges the `Style` parameter with any internally generated inline styles via the `CombineStyles()` helper.
