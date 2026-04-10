---
uid: common-features-icons
title: Icons
description: Using SVG icons in Marilo components — built-in Tabler Icons, custom icons, and provider icon architecture.
---

# Icons

Marilo ships with built-in SVG icon support through the `MariloIcon` component and a provider-aware icon architecture that lets each UI provider supply its own icon implementation.

## MariloIcon Component

`MariloIcon` renders a single SVG icon by name.

```razor
<MariloIcon Name="home" />

<MariloIcon Name="user" Size="IconSize.Large" />

<MariloIcon Name="settings" Color="var(--marilo-color-primary)" />
```

### Parameters

| Parameter | Type | Default | Description |
|---|---|---|---|
| `Name` | `string` | — | The icon name (see Tabler Icons). |
| `Size` | `IconSize` | `IconSize.Medium` | Predefined size. |
| `Width` | `string?` | `null` | Explicit width, overrides `Size`. |
| `Height` | `string?` | `null` | Explicit height, overrides `Size`. |
| `Color` | `string?` | `null` | SVG stroke or fill color. Accepts any CSS color value. |
| `Title` | `string?` | `null` | Accessible title for the SVG element (`aria-label`). |
| `Class` | `string?` | `null` | Additional CSS classes. |
| `Style` | `string?` | `null` | Inline styles applied to the SVG element. |

## Tabler Icons

The default icon set is [Tabler Icons](https://tabler.io/icons), an open-source collection of 5,000+ SVG icons distributed under the MIT license.

Icons are referenced by their Tabler name — lowercase, hyphen-separated:

```razor
<MariloIcon Name="brand-github" />
<MariloIcon Name="arrow-up-right" />
<MariloIcon Name="circle-check" />
<MariloIcon Name="alert-triangle" />
```

Icons are delivered as an SVG sprite embedded in the Marilo bundle. No additional CDN requests are made at runtime.

## Icon Sizes

`IconSize` is an enum with four predefined values:

| Value | Pixel size |
|---|---|
| `IconSize.Small` | 16 px |
| `IconSize.Medium` | 20 px |
| `IconSize.Large` | 24 px |
| `IconSize.ExtraLarge` | 32 px |

For sizes outside these steps, set `Width` and `Height` directly:

```razor
<MariloIcon Name="star" Width="48px" Height="48px" />
```

## Provider Icon Architecture

Each UI provider (FluentUI, Bootstrap, Material 3) registers an `IMariloIconProvider` implementation. This interface controls how `MariloIcon` resolves and renders its output — allowing providers to use their own icon font, SVG system, or native icon component while keeping the `MariloIcon` API stable.

```csharp
public interface IMariloIconProvider
{
    RenderFragment Render(string name, IconSize size, string? color, string? title);
}
```

The active provider is resolved from DI. When the FluentUI provider is active, icons use Fluent's SVG sprite; the Bootstrap provider falls back to the same Tabler sprites with Bootstrap-compatible sizing tokens.

You can register a custom provider in `Program.cs`:

```csharp
builder.Services.AddSingleton<IMariloIconProvider, MyCustomIconProvider>();
```

## Custom Icons

To use icons outside the Tabler set, implement `IMariloIconProvider` or use `MariloIcon` with `ChildContent` when you need a fully custom SVG inline:

```razor
<MariloIcon Name="my-logo" Size="IconSize.Large">
    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
        <path d="M12 2 ..." />
    </svg>
</MariloIcon>
```

Alternatively, render the SVG directly and apply the same spacing classes manually for consistency.

## Icons in Components

Many Marilo components accept icon parameters directly, so you rarely need to nest `MariloIcon` by hand.

```razor
<!-- Button with icon -->
<MariloButton Icon="download" Variant="ButtonVariant.Primary">Export</MariloButton>

<!-- NavLink with icon -->
<MariloNavLink Href="/dashboard" Icon="dashboard">Dashboard</MariloNavLink>

<!-- Menu item with icon -->
<MariloMenuItem Icon="edit" OnClick="Edit">Edit</MariloMenuItem>

<!-- Alert with icon -->
<MariloAlert Severity="AlertSeverity.Warning" Icon="alert-triangle">
    Check your input before proceeding.
</MariloAlert>
```

These parameters accept the same Tabler icon name strings used with `MariloIcon`.
