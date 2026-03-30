---
uid: getting-started-first-component
title: First Component
description: Build a simple Blazor page using a MariloButton with an OnClick handler.
---

# First Component

This walkthrough creates a minimal Blazor page that uses `MariloButton` to demonstrate how Marilo components work.

## Create the page

Add a new Razor page to your project:

```razor
@page "/hello"

<h1>Hello Marilo</h1>

<MariloButton Variant="ButtonVariant.Primary"
              Size="ButtonSize.Medium"
              OnClick="@HandleClick">
    Say Hello
</MariloButton>

@if (!string.IsNullOrEmpty(greeting))
{
    <MariloAlert Severity="AlertSeverity.Success">
        @greeting
    </MariloAlert>
}

@code {
    private string greeting = "";

    private void HandleClick()
    {
        greeting = $"Hello from Marilo! The time is {DateTime.Now:T}.";
    }
}
```

## What is happening

1. **`MariloButton`** renders a `<button>` element whose CSS classes come from the registered `IMariloCssProvider`. The `Variant` and `Size` parameters control the visual style without you writing any CSS.
2. **`OnClick`** is an `EventCallback<MouseEventArgs>` that fires when the user clicks the button. Marilo automatically calls `StateHasChanged` after the callback completes.
3. **`MariloAlert`** shows a success banner. The `Severity` parameter maps to a provider-defined color scheme (info, success, warning, error).

## Try different variants

Swap in other parameter values to explore the component API:

```razor
<MariloButton Variant="ButtonVariant.Danger" IsOutline="true" Size="ButtonSize.Large">
    Delete
</MariloButton>

<MariloButton Variant="ButtonVariant.Secondary" Disabled="true">
    Disabled
</MariloButton>
```

## Next steps

- Browse the [Components](xref:component-button-overview) section for the full parameter reference of every component.
- Read [Theming Overview](xref:theming-overview) to learn how to customize the look and feel.
