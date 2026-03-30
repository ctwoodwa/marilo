---
uid: component-tabs-overview
title: Tabs
description: The MariloTabs component organizes content into tabbed panels with automatic tab strip rendering.
---

# Tabs

## Overview

The `MariloTabs` component displays a horizontal tab strip and shows the content of the active panel. Each panel is defined with `MariloTabPanel`. The tab strip is generated automatically from the panel titles.

## Creating Tabs

````razor
<MariloTabs @bind-ActiveIndex="activeTab">
    <MariloTabPanel Title="Profile">
        <p>Profile content goes here.</p>
    </MariloTabPanel>
    <MariloTabPanel Title="Settings">
        <p>Settings content goes here.</p>
    </MariloTabPanel>
    <MariloTabPanel Title="Notifications">
        <p>Notifications content goes here.</p>
    </MariloTabPanel>
</MariloTabs>

@code {
    private int activeTab = 0;
}
````

## Features

- **Automatic tab strip** -- Tab buttons are rendered from the `Title` property of each `MariloTabPanel`.
- **Two-way binding** -- Use `@bind-ActiveIndex` to synchronize the selected tab with your code.
- **Dynamic panels** -- Add or remove `MariloTabPanel` children at runtime; the tab strip updates automatically.
- **Accessibility** -- Uses `role="tablist"` and `aria-selected` attributes.

## Parameters

### MariloTabs

| Name | Type | Default | Description |
|---|---|---|---|
| `ActiveIndex` | `int` | `0` | The zero-based index of the currently active tab. |
| `ActiveIndexChanged` | `EventCallback<int>` | -- | Callback fired when the active tab changes. Used by `@bind-ActiveIndex`. |
| `ChildContent` | `RenderFragment?` | `null` | The `MariloTabPanel` children. |

### MariloTabPanel

| Name | Type | Default | Description |
|---|---|---|---|
| `Title` | `string` | `""` | The text displayed on the tab button. |
| `ChildContent` | `RenderFragment?` | `null` | The panel content shown when this tab is active. |

## See Also

- [API Reference](xref:Marilo.Components.Layout.MariloTabs)
