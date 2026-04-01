# Marilo Component Patterns

Authoritative reference for how Marilo Blazor components are structured. Read the relevant sections when building or testing components.

---

## Base Class

All components inherit from `MariloComponentBase` (`src/Marilo.Core/Base/MariloComponentBase.cs`).

Inherited services (injected automatically):
- `CssProvider` -- `IMariloCssProvider` for design-system-agnostic CSS classes
- `IconProvider` -- `IMariloIconProvider` for icon rendering
- `ThemeService` -- `IMariloThemeService` for theme state

Inherited helpers:
- `CombineClasses(providerClass)` -- merges provider CSS class with consumer `Class` parameter
- `CombineStyles()` -- merges computed styles with consumer `Style` parameter
- `SetAria(name, value)` -- adds aria-* attributes to the output dictionary

Inherited parameters:
- `Class` (string?) -- consumer-supplied CSS class to append
- `Style` (string?) -- consumer-supplied inline style to append
- `AdditionalAttributes` (Dictionary?) -- captures unmatched HTML attributes

---

## CssProvider Integration

Components never hardcode CSS class names. Instead they call CssProvider methods:

```csharp
var css = CssProvider.ButtonClass(Variant, Size, FillMode, Rounded, IsDisabled);
```

Each provider (FluentUI, Bootstrap) returns its own class strings. The component renders whichever classes the active provider returns.

---

## Parameter Conventions

- Use `[Parameter]` attribute on all public properties
- PascalCase names, no abbreviations
- Required parameters: document as required in XML comments
- Constrained choices use enums (e.g., `ButtonVariant`, `ButtonSize`), never raw strings
- Boolean parameters default to `false` unless the "on" state is the common case
- `[Parameter(CaptureUnmatchedValues = true)]` on `AdditionalAttributes`

---

## Event Conventions

- Use `EventCallback<T>` for typed events, `EventCallback` for parameterless
- Name pattern: `On[Action]` (e.g., `OnClick`, `OnChange`, `OnExpand`)
- Custom event args: `[Component]EventArgs` class in `Marilo.Core/Models/`
- Always use `InvokeAsync` to fire callbacks

---

## Composition Patterns

- `RenderFragment` for single content slot (`ChildContent`)
- `RenderFragment<T>` for templated content (e.g., `ItemTemplate`)
- `CascadingValue` for parent-to-child communication (e.g., DataGrid to Column)
- Child components discover parent via `[CascadingParameter]`

---

## Test Patterns

Tests use bUnit with xUnit. Base class: `MariloTestBase` (`tests/Marilo.Tests.Unit/MariloTestBase.cs`).

MariloTestBase extends `BunitContext` and registers:
- `FluentUICssProvider` as `IMariloCssProvider`
- `TestThemeService` as `IMariloThemeService`
- `TestIconProvider` as `IMariloIconProvider`
- `MariloNotificationService`

Test pattern:
```csharp
public class MariloFooTests : MariloTestBase
{
    [Fact]
    public void Foo_Renders_Default()
    {
        var cut = Render<MariloFoo>();
        Assert.Contains("mar-foo", cut.Markup);
    }

    [Fact]
    public void Foo_Applies_Variant_Class()
    {
        var cut = Render<MariloFoo>(p => p.Add(x => x.Variant, FooVariant.Primary));
        Assert.Contains("mar-foo--primary", cut.Markup);
    }
}
```
