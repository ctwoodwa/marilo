---
uid: theming-overview
title: Theming Overview
description: Understand the Marilo provider system and learn how to customize colors, typography, and shape with MariloTheme.
---

# Theming Overview

Marilo separates component behavior from visual styling through its **provider system**. Every component asks a provider for its CSS classes, icons, and JS interop behavior at render time. You control the look and feel by configuring a `MariloTheme` and wrapping your application in a `MariloThemeProvider`.

## The MariloTheme record

`MariloTheme` is a C# record that holds all design tokens:

```csharp
public record MariloTheme
{
    public MariloColorPalette Colors { get; init; } = new();
    public MariloTypographyScale Typography { get; init; } = new();
    public MariloShape Shape { get; init; } = new();
    public bool IsRtl { get; init; }
}
```

### Color palette

`MariloColorPalette` defines the semantic colors used throughout the component library -- primary, secondary, success, warning, error, surface, and background tokens. Each provider maps these tokens to its own design system values.

### Typography scale

`MariloTypographyScale` provides font family, size, weight, and line-height values for headings, body text, captions, and other typographic roles.

### Shape

`MariloShape` controls border radius values at small, medium, and large scales, giving you consistent rounding across all components.

## Using MariloThemeProvider

Wrap your application layout in `MariloThemeProvider` to make the theme available to all child components:

```razor
<MariloThemeProvider Theme="@customTheme">
    <Router AppAssembly="@typeof(App).Assembly">
        <!-- ... -->
    </Router>
</MariloThemeProvider>

@code {
    private MariloTheme customTheme = new()
    {
        Colors = new MariloColorPalette
        {
            Primary = "#0078D4",
            Secondary = "#2B88D8",
            Success = "#107C10",
            Warning = "#FFB900",
            Error = "#D13438"
        }
    };
}
```

When no custom theme is supplied, `MariloThemeProvider` uses the provider's default theme values.

## Runtime theme switching

Inject `IMariloThemeService` to change the theme at runtime:

```csharp
@inject IMariloThemeService ThemeService

private void SwitchToDark()
{
    ThemeService.SetTheme(new MariloTheme
    {
        Colors = new MariloColorPalette { Background = "#1E1E1E", Surface = "#252525" }
    });
}
```

All components re-render automatically when the theme changes.

## See also

- [Providers](xref:theming-providers) -- the three provider contracts in detail.
- [Creating a Custom Provider](xref:theming-custom-provider) -- build your own provider from scratch.
