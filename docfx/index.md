---
_layout: landing
---

# Marilo Documentation

Marilo is a provider-first Blazor component library. Components define behavior; providers supply visual styling. Swap providers to change the entire look-and-feel without touching component code.

## Quick Links

- [Getting Started](articles/getting-started/overview.md) - Installation and first component
- [Components](articles/components/) - Component documentation and usage guides
- [Theming](articles/theming/overview.md) - Provider system and custom themes
- [API Reference](api/) - Auto-generated API docs from source code

## Architecture

```
Marilo.Core          - Contracts, base classes, enums, configuration
Marilo.Components    - Provider-agnostic Razor components
Marilo.Icons         - Custom SVG icon set + icon provider
Marilo.Providers.*   - Provider implementations (FluentUI, Bootstrap, etc.)
```

## Current Providers

| Provider | Package | Status |
|---|---|---|
| Fluent UI | `Marilo.Providers.FluentUI` | Available |
| Bootstrap 5 | `Marilo.Providers.Bootstrap` | Planned |
| Material Design 3 | `Marilo.Providers.Material` | Planned |
