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

## See also

- [Theming Overview](xref:theming-overview) -- configure colors, typography, and shape.
- [Creating a Custom Provider](xref:theming-custom-provider) -- step-by-step guide.
