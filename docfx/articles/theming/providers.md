---
uid: theming-providers
title: Providers
description: Learn about IMariloCssProvider, IMariloIconProvider, and IMariloJsInterop -- the three contracts that define a Marilo provider.
---

# Providers

A Marilo **provider** is a set of three service implementations that give components their visual identity. Each service is registered via dependency injection and consumed by the component base class at render time.

## IMariloCssProvider

This is the core contract. It contains one method per component (or component state) that returns the CSS class string to apply:

```csharp
public interface IMariloCssProvider
{
    string ButtonClass(ButtonVariant variant, ButtonSize size, bool isOutline, bool isDisabled);
    string CardClass();
    string AlertClass(AlertSeverity severity);
    string DialogClass();
    string TooltipClass(TooltipPosition position);
    // ... 80+ additional methods covering every component
}
```

The Fluent UI provider implements each method by mapping parameters to BEM-style class names defined in its SCSS:

```csharp
public class FluentUICssProvider : IMariloCssProvider
{
    public string ButtonClass(ButtonVariant variant, ButtonSize size, bool isOutline, bool isDisabled)
    {
        var css = $"mar-button mar-button--{variant.ToString().ToLowerInvariant()} mar-button--{size.ToString().ToLowerInvariant()}";
        if (isOutline) css += " mar-button--outline";
        if (isDisabled) css += " mar-button--disabled";
        return css;
    }
    // ...
}
```

## IMariloIconProvider

Responsible for resolving icon names to SVG markup:

```csharp
public interface IMariloIconProvider
{
    MarkupString GetIcon(string name, IconSize size = IconSize.Medium);
    string GetIconSpriteUrl();
}
```

- `GetIcon` returns an SVG `MarkupString` for inline rendering.
- `GetIconSpriteUrl` returns the path to an SVG sprite sheet, used by components that reference icons via `<use>` elements.

## IMariloJsInterop

Encapsulates all JavaScript interop calls the component library needs:

```csharp
public interface IMariloJsInterop : IAsyncDisposable
{
    ValueTask InitializeAsync();
    ValueTask<bool> ShowModalAsync(string modalId);
    ValueTask HideModalAsync(string modalId);
    ValueTask<BoundingBox> GetElementBoundsAsync(ElementReference element);
    ValueTask ObserveScrollAsync(ElementReference element, DotNetObjectReference<object> callback);
}
```

Each provider ships its own JavaScript module and implements this interface to bridge Blazor with the browser DOM.

## How the FluentUI provider registers everything

The `UseFluentUI()` extension method on `MariloBuilder` registers all three contracts:

```csharp
public static MariloBuilder UseFluentUI(this MariloBuilder builder, Action<FluentUIOptions>? configure = null)
{
    builder.Services.AddScoped<IMariloCssProvider, FluentUICssProvider>();
    builder.Services.AddScoped<IMariloIconProvider, FluentUIIconProvider>();
    builder.Services.AddScoped<IMariloJsInterop, FluentUIJsInterop>();
    // ...
    return builder;
}
```

This pattern makes it straightforward to create a new provider: implement the three interfaces, register them in an extension method, and every Marilo component picks them up automatically.

## Bootstrap provider

Register Bootstrap 5.3 as the active provider:

```csharp
// Program.cs
builder.Services.AddMarilo().UseBootstrap();
```

Reference the stylesheet in `App.razor`:

```html
<link rel="stylesheet" href="_content/Marilo.Providers.Bootstrap/css/marilo-bootstrap.css" />
```

The Bootstrap provider maps Marilo component state to native Bootstrap 5.3 utility classes wherever a direct equivalent exists, and falls back to BEM-style `mar-bs-*` classes for component-specific structure that Bootstrap does not cover. Bootstrap's own Sass variables remain globally available inside the provider's SCSS so `$primary`, `$border-color`, and similar variables are always in scope without `@use`.

## Material 3 provider

Register Material Design 3 as the active provider:

```csharp
// Program.cs
builder.Services.AddMarilo().UseMaterial();
```

Reference the stylesheet in `App.razor`:

```html
<link rel="stylesheet" href="_content/Marilo.Providers.Material/css/marilo-material.css" />
```

The Material 3 provider uses a two-layer token architecture. A `$material-ref-palette` Sass map holds the raw 13-stop palette; the SCSS then emits semantic role custom properties (`--md-sys-color-primary`, etc.) from that map. Marilo's `--marilo-*` tokens are mapped to the M3 role tokens so all built-in components stay consistent with Material You color expectations.

## Built-in providers

| Provider | Extension | CSS prefix | Design system | Dark mode |
| --- | --- | --- | --- | --- |
| FluentUI | `UseFluentUI()` | `mar-` | Fluent UI 2 | `[data-marilo-theme="dark"]` |
| Bootstrap | `UseBootstrap()` | `mar-bs-` | Bootstrap 5.3 | `[data-marilo-theme="dark"]` + Bootstrap color-mode |
| Material 3 | `UseMaterial()` | `mar-` | Material Design 3 | `[data-marilo-theme="dark"]` |

All three providers implement the same `IMariloCssProvider`, `IMariloIconProvider`, and `IMariloJsInterop` contracts, so switching between them requires only changing the extension method call and the stylesheet `<link>` in `App.razor`.

## See also

- [Theming Overview](xref:theming-overview) -- configure colors, typography, and shape.
- [Token Reference](xref:theming-token-reference) -- complete `--marilo-*` CSS custom property reference.
- [Dark Mode](xref:theming-dark-mode) -- enabling and toggling dark mode.
- [Runtime Provider Switching](xref:theming-runtime-switching) -- swap providers at runtime without a page reload.
- [Creating a Custom Provider](xref:theming-custom-provider) -- step-by-step guide.
