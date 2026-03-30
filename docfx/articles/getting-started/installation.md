---
uid: getting-started-installation
title: Installation
description: Install the Marilo component library and FluentUI provider into a Blazor project.
---

# Installation

## 1. Add the NuGet packages

Every Marilo project needs the core component package. You also need at least one provider -- here we use the Fluent UI provider.

```bash
dotnet add package Marilo.Components
dotnet add package Marilo.Providers.FluentUI
```

## 2. Register services in Program.cs

Open `Program.cs` and register Marilo with the Fluent UI provider:

```csharp
builder.Services.AddMarilo().UseFluentUI();
```

`AddMarilo()` registers core services (theming, dialogs, notifications) and returns a `MariloBuilder`. The `UseFluentUI()` extension method registers the Fluent UI implementations of `IMariloCssProvider`, `IMariloIconProvider`, and `IMariloJsInterop`.

## 3. Add the stylesheet

In your `App.razor` (or `index.html` for Blazor WebAssembly), add the provider CSS inside the `<head>` section:

```html
<link rel="stylesheet" href="_content/Marilo.Providers.FluentUI/css/marilo-fluentui.css" />
```

## 4. Add the imports

Open `_Imports.razor` and add:

```razor
@using Marilo.Components
@using Marilo.Components.Buttons
@using Marilo.Components.Forms.Inputs
@using Marilo.Components.DataDisplay
@using Marilo.Components.Feedback
@using Marilo.Components.Layout
@using Marilo.Components.Navigation
@using Marilo.Components.Utility
```

## Verify the setup

Create a quick test page to confirm everything is wired up:

```razor
@page "/test"

<MariloButton Variant="ButtonVariant.Primary" OnClick="@(() => message = "It works!")">
    Click me
</MariloButton>

<p>@message</p>

@code {
    private string message = "";
}
```

Run the application and navigate to `/test`. If you see a styled button that updates the message on click, the installation is complete.

## Next steps

- [First Component](xref:getting-started-first-component) -- a guided walkthrough of your first Marilo page.
- [Theming Overview](xref:theming-overview) -- customize colors, typography, and shape.
