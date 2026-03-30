<div align="center">

# Marilo

**Provider-first Blazor component library. Components define behavior — providers supply visual styling.  
Swap providers to change the entire look-and-feel without touching a single component.**

[![CI](https://github.com/ctwoodwa/marilo/actions/workflows/ci.yml/badge.svg)](https://github.com/ctwoodwa/marilo/actions/workflows/ci.yml)
[![Docs](https://img.shields.io/badge/docs-GitHub%20Pages-blue?style=flat-square)](https://ctwoodwa.github.io/marilo/)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg?style=flat-square)](./LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](./CONTRIBUTING.md)

[📖 Documentation](https://ctwoodwa.github.io/marilo/) · [🚀 Live Demo](https://codespaces.new/ctwoodwa/marilo?quickstart=1) · [🐛 Report a Bug](https://github.com/ctwoodwa/marilo/issues/new?template=bug_report.md) · [💡 Request a Feature](https://github.com/ctwoodwa/marilo/issues/new?template=feature_request.md)

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/ctwoodwa/marilo?quickstart=1)

</div>

---

## Overview

Marilo is an enterprise-grade Blazor component library built around a **provider pattern**: components are decoupled from their visual implementation. Register a provider (e.g., Fluent UI) at startup and all components automatically adopt that style system — without any per-component changes. This makes Marilo ideal for design system migrations, white-label products, and teams that need UI consistency across multiple applications.

> 🧩 40+ components · 360+ icons · Provider-swappable styling · .NET 10 · MIT licensed

---

## Table of Contents

- [Packages](#packages)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Component Catalog](#component-catalog)
- [Theming & Providers](#theming--providers)
- [Icons](#icons)
- [Running the Demo](#running-the-demo)
- [Documentation](#documentation)
- [Contributing](#contributing)
- [License](#license)

---

## Packages

| Package | Description | NuGet |
|---|---|---|
| `Marilo.Components` | 40+ provider-agnostic Razor components | *(coming soon)* |
| `Marilo.Providers.FluentUI` | Fluent UI visual styling provider | *(coming soon)* |
| `Marilo.Icons` | 360+ SVG icons + `MariloIcon` component | *(coming soon)* |
| `Marilo.Core` | Base classes, contracts, and enums | *(coming soon)* |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- A Blazor Server or Blazor WebAssembly project

### Installation

```bash
dotnet add package Marilo.Components
dotnet add package Marilo.Providers.FluentUI
dotnet add package Marilo.Icons   # optional — for icon support
```

---

## Usage

### 1. Register services

In `Program.cs`, register Marilo and choose your provider:

```csharp
builder.Services.AddMarilo(options =>
    options
        .UseFluentUI()
        .UseMariloIcons()  // optional
);
```

### 2. Add stylesheets

In your `App.razor` or `index.html`:

```html
<link rel="stylesheet" href="_content/Marilo.Providers.FluentUI/css/marilo-fluentui.css" />
<link rel="stylesheet" href="_content/Marilo.Icons/css/marilo-icons.css" />
```

### 3. Add global imports

In `_Imports.razor`:

```razor
@using Marilo.Components
@using Marilo.Core.Enums
```

### 4. Use a component

```razor
<MariloButton Variant="ButtonVariant.Primary" OnClick="HandleClick">
    <MariloIcon Name="download" /> Export
</MariloButton>
```

> 🔗 [Full usage guide →](https://ctwoodwa.github.io/marilo/)

---

## Component Catalog

Explore all components with live demos and API reference in the [documentation →](https://ctwoodwa.github.io/marilo/)

| Category | Components |
|---|---|
| **Buttons** | `MariloButton`, `MariloButtonGroup`, `MariloToggleButton`, `MariloFAB` |
| **Forms** | `MariloTextField`, `MariloTextArea`, `MariloCheckbox`, `MariloSwitch`, `MariloSelect`, `MariloSlider`, `MariloDatePicker`, `MariloColorPicker`, `MariloSearchBox`, `MariloRating` |
| **Navigation** | `MariloMenu`, `MariloTabs`, `MariloBreadcrumb`, `MariloTreeView`, `MariloStepper`, `MariloSegmentedControl`, `MariloPagination`, `MariloToolbar` |
| **Layout** | `MariloContainer`, `MariloStack`, `MariloGrid`, `MariloPanel`, `MariloCard`, `MariloDivider` |
| **Feedback** | `MariloAlert`, `MariloDialog`, `MariloConfirmDialog`, `MariloDrawer`, `MariloTooltip`, `MariloChip`, `MariloProgressBar`, `MariloSpinner`, `MariloSkeleton` |
| **Display** | `MariloAvatar`, `MariloBadge`, `MariloTable`, `MariloList` |
| **Icons** | `MariloIcon`, `MariloIconSprite` |

---

## Theming & Providers

Marilo's provider architecture separates **component behavior** from **visual implementation**. A provider is registered once at app startup and supplies all CSS, tokens, and rendering overrides across every component.

Currently available providers:

- **`Marilo.Providers.FluentUI`** — Microsoft Fluent UI 2 design language

Additional providers (e.g., Material, Bootstrap) can be created by implementing `IMariloProvider`. Swapping providers requires changing a single line in `Program.cs` — no component code changes needed.

> 🎨 [Theming guide →](https://ctwoodwa.github.io/marilo/)

---

## Icons

`Marilo.Icons` includes **360+ SVG icons** accessible via the `MariloIcon` component. An interactive icon browser is available in the docs.

```razor
<!-- Basic usage -->
<MariloIcon Name="settings" />

<!-- With size and color -->
<MariloIcon Name="star" Size="IconSize.Large" Color="var(--marilo-primary)" />

<!-- Optimized sprite rendering -->
<MariloIconSprite />
```

> 🔍 [Browse all icons →](https://ctwoodwa.github.io/marilo/)

---

## Running the Demo

The demo is a Blazor Server application showcasing all components interactively.

### Quickest way — GitHub Codespaces (no local setup)

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/ctwoodwa/marilo?quickstart=1)

Once the Codespace starts, run:

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

---

## Documentation

Full API reference, component demos, theming guide, and icon browser are published to GitHub Pages:

📖 **[https://ctwoodwa.github.io/marilo/](https://ctwoodwa.github.io/marilo/)**

To build and serve docs locally:

```bash
npm run docs:serve
```

Docs are generated using [DocFX](https://dotnet.github.io/docfx/) from the `docfx/` directory.

---

## Contributing

Contributions are welcome from the community. Please read the guidelines before opening a PR.

1. Fork the repo and create your branch: `git checkout -b feat/my-component`
2. Install dependencies: `npm install`
3. Build assets: `npm run build`
4. Run the demo: `dotnet run --project samples/Marilo.Demo`
5. Submit a PR against `main`

📋 [Contributing Guide →](./CONTRIBUTING.md)  
💬 [Code of Conduct →](./CODE_OF_CONDUCT.md)  
🔒 [Security Policy →](./SECURITY.md)

---

## Versioning

This project follows [Semantic Versioning](https://semver.org/).

- 🔖 [Releases →](https://github.com/ctwoodwa/marilo/releases)
- 🛣️ [Roadmap →](https://github.com/ctwoodwa/marilo/projects)

---

## License

Licensed under the [MIT License](./LICENSE).

© 2026 Christopher Wood. All rights reserved.

---

<div align="center">
  <sub>Built with ❤️ using Blazor · <a href="https://ctwoodwa.github.io/marilo/">ctwoodwa.github.io/marilo</a></sub>
</div>