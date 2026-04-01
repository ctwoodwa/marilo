---
title: SignalR Connection Status Appearance
page_title: Appearance | SignalR Connection Status | Marilo Blazor
description: Customization options for the SignalR connection status indicator appearance.
slug: components/signalr-connection-status/appearance
tags: marilo,blazor,signalr,appearance
published: True
position: 1
components: ["signalr-connection-status"]
---

# Appearance

## Compact Mode

Use `Compact="true"` for toolbar or header placement with smaller visual density:

````RAZOR
<MariloSignalRConnectionStatus Compact="true" />
````

## Hide Counts

Disable the healthy/total count label:

````RAZOR
<MariloSignalRConnectionStatus ShowCounts="false" />
````

## Popup Placement

Control where the popup appears relative to the trigger:

````RAZOR
<MariloSignalRConnectionStatus PopupPlacement="ConnectionPopupPlacement.TopEnd" />
````

Options: `BottomStart`, `BottomEnd` (default), `TopStart`, `TopEnd`.

## Per-Row Health Badges

Each hub row shows a color-coded badge:

| Health | Badge | Color |
|--------|-------|-------|
| Healthy | "Healthy" | Green |
| Starting | "Starting" | Blue/Info |
| Recovering | "Recovering" | Amber |
| Degraded | "Degraded" | Red |
| Offline | "Offline" | Red |

## Critical vs Noncritical

The popup separates hubs into "Critical" and "Optional" groups. Use `IncludeNonCritical="false"` to hide optional hubs.

## Custom Title

````RAZOR
<MariloSignalRConnectionStatus Title="Hub connections" />
````
