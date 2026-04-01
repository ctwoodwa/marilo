---
title: Icons
description: Browse all 362 hand-crafted SVG icons included with Marilo.
---

# Marilo Icons

362 essential icons across 18 categories. 24×24 grid, 2px stroke, round joins, `currentColor` for effortless theming.

> **Interactive demo** — For live component examples (sizing, coloring, icon-in-button, etc.), see the [Marilo demo site](https://localhost:5301/icons).

## Installation

Install the `Marilo.Icons` package and call `UseMariloIcons()` during setup:

```csharp
// Program.cs
builder.Services.AddMarilo(options => options
    .UseMariloIcons()
);
```

Include the stylesheet in your `App.razor` or `index.html`:

```html
<link rel="stylesheet" href="_content/Marilo.Icons/css/marilo-icons.css" />
```

Include the SVG sprite (required for icon rendering):

```html
<!-- Razor component approach — place once in App.razor / MainLayout.razor -->
<MariloIconSprite />
```

## Basic usage

```razor
<MariloIcon Name="search" />
```

## Parameters

| Parameter | Type | Default | Description |
|---|---|---|---|
| `Name` | `string` | — | Icon name (e.g. `"search"`, `"download"`) |
| `Size` | `IconSize` | `Medium` | `Small` (16px), `Medium` (20px), `Large` (24px), `ExtraLarge` (32px) |
| `ThemeColor` | `IconThemeColor` | `Base` | `Primary`, `Secondary`, `Success`, `Warning`, `Danger`, `Info`, `Inherit` |
| `Flip` | `IconFlip` | `None` | `Horizontal`, `Vertical`, `Both` |
| `AriaLabel` | `string?` | `null` | Accessible label. Set when the icon is the sole content of an interactive element. |

## Common patterns

**Icon with a label** — default for most use cases:

```razor
<MariloButton>
    <MariloIcon Name="download" /> Export
</MariloButton>
```

**Icon-only button** — always provide an `aria-label`:

```razor
<MariloButton aria-label="Delete item">
    <MariloIcon Name="trash" />
</MariloButton>
```

**Sized icon:**

```razor
<MariloIcon Name="settings" Size="IconSize.Large" />
```

**Themed icon:**

```razor
<MariloIcon Name="check-circle" ThemeColor="IconThemeColor.Success" />
```

**Flipped icon:**

```razor
<MariloIcon Name="arrow-right" Flip="IconFlip.Horizontal" />
```

## Icon browser

Search, filter by category, and copy ready-to-use Blazor or SVG snippets.

<div id="marilo-icon-browser"></div>

<script>window.MARILO_ICONS_URL = '../marilo-icons.json';</script>
<script src="../marilo-icon-browser.js"></script>
