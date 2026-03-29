# Marilo

A Blazor component library built on [Fluent UI](https://www.fluentui-blazor.net/) with real-time data change notifications, feedback components, and navigation helpers.

## Installation

```bash
dotnet add package Marilo
```

## Setup

### 1. Register services

```csharp
// Program.cs
using Marilo;

builder.Services.AddMarilo();
```

### 2. Add the stylesheet

```html
<!-- index.html or _Host.cshtml -->
<link href="_content/Marilo/css/marilo.css" rel="stylesheet" />
```

### 3. Add imports

```razor
<!-- _Imports.razor -->
@using Marilo.Services
@using Marilo.Models
@using Marilo.Components
```

## Components

### Feedback

| Component | Description |
|---|---|
| `<MarAlertStrip>` | Displays a list of system alerts with severity-based styling |
| `<MarConfirmDialog>` | Modal confirmation dialog with dangerous-action styling support |
| `<MarDataChangeBanner>` | Banner showing pending real-time data changes with refresh/dismiss |
| `<MarDataChangeToast>` | Toast notifications for real-time data changes via SignalR |

### Navigation

| Component | Description |
|---|---|
| `<MarEnvironmentBadge>` | Displays the current environment (DEV/STAGING/PROD) as a badge |
| `<MarTimeRangeSelector>` | Time range picker (Now, 1h, 24h, 7d, 30d) with two-way binding |

## Usage Examples

```razor
<!-- Alert strip -->
<MarAlertStrip Alerts="@alerts" />

<!-- Confirmation dialog -->
<MarConfirmDialog IsOpen="@showDialog"
                  Title="Delete Item"
                  ConfirmText="Delete"
                  IsDangerous="true"
                  OnConfirm="HandleDelete"
                  OnCancel="CloseDialog">
    <p>Are you sure you want to delete this item?</p>
</MarConfirmDialog>

<!-- Real-time data change banner -->
<MarDataChangeBanner EntityTypes="@(new[] { "User" })"
                     OnRefreshRequested="@LoadData" />

<!-- Toast notifications (typically in MainLayout) -->
<MarDataChangeToast />

<!-- Environment badge -->
<MarEnvironmentBadge />

<!-- Time range selector with two-way binding -->
<MarTimeRangeSelector @bind-SelectedRange="timeRange"
                      OnRangeChanged="HandleRangeChange" />
```

## Services

| Service | Lifetime | Description |
|---|---|---|
| `AuthStateProvider` | Scoped | Fetches auth state from a BFF endpoint |
| `ThemeService` | Scoped | Dark/light theme toggle with localStorage persistence |
| `DataChangeService` | Scoped | SignalR-based real-time data change notifications |
| `MarkdownService` | Singleton | Markdown-to-HTML rendering via Markdig |
| `GraphQLClientBase` | Abstract | Base class for app-specific GraphQL clients |

## CSS Prefix

All Marilo CSS classes use the `mar-` prefix to avoid collisions with your application styles.

## License

MIT
