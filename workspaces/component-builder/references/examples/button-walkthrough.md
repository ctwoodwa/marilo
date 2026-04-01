# Example Component: MariloButton

A condensed walkthrough of the MariloButton component to show what a completed Marilo component looks like across all artifact types.

---

## Component Source

**Location:** `src/Marilo.Components/Buttons/MariloButton.razor`
**Type:** Simple (single .razor file with @code block)
**Inherits:** MariloComponentBase

**Key parameters:**
- `Variant` (ButtonVariant) -- Primary, Secondary, Tertiary, Outline, Flat
- `Size` (ButtonSize) -- Sm, Md, Lg
- `FillMode` (ButtonFillMode) -- Solid, Outline, Flat
- `Rounded` (RoundedMode) -- Sm, Md, Lg, Full
- `Disabled` (bool) -- Disables interaction
- `Icon` (string?) -- Icon name to render
- `ChildContent` (RenderFragment?) -- Button label content

**Key events:**
- `OnClick` (EventCallback<MouseEventArgs>) -- Fires on click

**Rendering pattern:**
```razor
<button class="@CombineClasses(CssProvider.ButtonClass(Variant, Size, FillMode, Rounded, Disabled))"
        style="@CombineStyles()"
        disabled="@Disabled"
        @onclick="OnClick"
        @attributes="AdditionalAttributes">
    @if (Icon is not null)
    {
        <MariloIcon Name="@Icon" />
    }
    @ChildContent
</button>
```

---

## Core Infrastructure

**Enums (in `src/Marilo.Core/Enums/`):**
- `ButtonVariant.cs` -- Primary, Secondary, Tertiary, Outline, Flat
- `ButtonSize.cs` -- Sm, Md, Lg
- `ButtonFillMode.cs` -- Solid, Outline, Flat

**CSS Provider Method (in `IMariloCssProvider.cs`):**
```csharp
string ButtonClass(ButtonVariant variant, ButtonSize size, ButtonFillMode fillMode,
                   RoundedMode rounded, bool isDisabled);
```

---

## Theme Providers

**FluentUI (`FluentUICssProvider.cs`):**
```csharp
public string ButtonClass(ButtonVariant variant, ButtonSize size, ...) =>
    new CssClassBuilder()
        .AddClass("mar-button")
        .AddClass($"mar-button--{variant.ToString().ToLower()}")
        .AddClass($"mar-button--{size.ToString().ToLower()}")
        .Build();
```

**Bootstrap (`BootstrapCssProvider.cs`):**
Maps to Bootstrap `btn btn-primary btn-sm` classes plus `mar-button` bridge class.

**SCSS files:**
- FluentUI: `_buttons.scss` with `mar-button`, `mar-button--primary`, etc.
- Bootstrap: `_bridge-buttons.scss` bridging Bootstrap button classes

---

## Documentation

**Spec folder:** `docs/component-specs/button/`
- `overview.md` -- Purpose, basic usage, parameters table
- `appearance.md` -- Variants, sizes, fill modes, rounded modes
- `events.md` -- OnClick handler examples
- `icons.md` -- Icon button usage
- `disabled-button.md` -- Disabled state
- `accessibility/overview.md` -- Keyboard (Enter/Space), role="button", aria-disabled
- `toc.yml` -- DocFx table of contents

---

## Demo Page

**Location:** `samples/Marilo.Demo/Pages/Components/Button/Overview.razor`

Sections: Basic Usage, Variants, Sizes, Fill Modes, Icon Buttons, Disabled, Events, Accessibility

Each section uses `<DemoSection>` with embedded code samples and live component rendering.

---

## Tests

**Location:** `tests/Marilo.Tests.Unit/`

Test cases:
- Default render (has `mar-button` class)
- Each variant applies correct CSS class
- Each size applies correct CSS class
- Disabled attribute renders correctly
- OnClick callback fires
- Icon renders when provided
- ChildContent renders
- AdditionalAttributes pass through
- ARIA attributes present
- FluentUI provider returns expected classes
- Bootstrap provider returns expected classes
