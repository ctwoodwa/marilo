# Marilo

[![CI](https://github.com/ctwoodwa/marilo/actions/workflows/ci.yml/badge.svg)](https://github.com/ctwoodwa/marilo/actions/workflows/ci.yml)
[![Docs](https://img.shields.io/badge/docs-ctwoodwa.github.io%2Fmarilo-blue)](https://ctwoodwa.github.io/marilo/)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Provider-first Blazor component library. Components define behavior; providers supply visual styling — swap providers to change the entire look-and-feel without touching component code.

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/ctwoodwa/marilo?quickstart=1)
&nbsp;&nbsp;**[View Documentation →](https://ctwoodwa.github.io/marilo/)**

---

## Packages

| Package | Description |
|---|---|
| `Marilo.Components` | 40+ provider-agnostic Razor components |
| `Marilo.Providers.FluentUI` | Fluent UI visual styling provider |
| `Marilo.Icons` | 360+ SVG icons + `MariloIcon` component |
| `Marilo.Core` | Base classes, contracts, and enums |

## Quick Start

### 1. Install packages

```bash
dotnet add package Marilo.Components
dotnet add package Marilo.Providers.FluentUI
dotnet add package Marilo.Icons        # optional
```

### 2. Register services

```csharp
// Program.cs
builder.Services.AddMarilo(options => options
    .UseFluentUI()
    .UseMariloIcons()   // optional
);
```

### 3. Add stylesheets

```html
<!-- App.razor or index.html -->
<link rel="stylesheet" href="_content/Marilo.Providers.FluentUI/css/marilo-fluentui.css" />
<link rel="stylesheet" href="_content/Marilo.Icons/css/marilo-icons.css" />
```

### 4. Add global imports

```razor
<!-- _Imports.razor -->
@using Marilo.Components
@using Marilo.Core.Enums
```

### 5. Use a component

```razor
<MariloButton Variant="ButtonVariant.Primary" OnClick="HandleClick">
    <MariloIcon Name="download" /> Export
</MariloButton>
```

## Components

| Category | Components |
|---|---|
| Buttons | `MariloButton`, `MariloButtonGroup`, `MariloToggleButton`, `MariloFAB` |
| Forms | `MariloTextField`, `MariloTextArea`, `MariloCheckbox`, `MariloSwitch`, `MariloSelect`, `MariloSlider`, `MariloDatePicker`, `MariloColorPicker`, `MariloSearchBox`, `MariloRating` |
| Navigation | `MariloMenu`, `MariloTabs`, `MariloBreadcrumb`, `MariloTreeView`, `MariloStepper`, `MariloSegmentedControl`, `MariloPagination`, `MariloToolbar` |
| Layout | `MariloContainer`, `MariloStack`, `MariloGrid`, `MariloPanel`, `MariloCard`, `MariloDivider` |
| Feedback | `MariloAlert`, `MariloDialog`, `MariloConfirmDialog`, `MariloDrawer`, `MariloTooltip`, `MariloChip`, `MariloProgressBar`, `MariloSpinner`, `MariloSkeleton` |
| Display | `MariloAvatar`, `MariloBadge`, `MariloTable`, `MariloList` |
| Icons | `MariloIcon`, `MariloIconSprite` |

## Running the Demo

The demo is a Blazor Server application. The quickest way to try it is **GitHub Codespaces** — no local setup required:

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/ctwoodwa/marilo?quickstart=1)

Once the Codespace starts, run the demo from the terminal:

```bash
dotnet run --project samples/Marilo.Demo
```

Then open the forwarded port in your browser.

### Run locally

```bash
git clone https://github.com/ctwoodwa/marilo.git
cd marilo
npm install && npm run build
dotnet run --project samples/Marilo.Demo
```

Open `http://localhost:8080`.

## Documentation

Full docs including API reference, theming guide, and icon browser are published to GitHub Pages:

**[https://ctwoodwa.github.io/marilo/](https://ctwoodwa.github.io/marilo/)**

To build and serve docs locally:

```bash
npm run docs:serve
```

## License

MIT
