# Component Requirements: SignalRConnectionStatus

## Overview

A reusable Blazor component that displays aggregate SignalR hub health as a toolbar icon (wireless / wireless-slash) with an expandable popup listing every registered hub connection, its state, health, errors, and reconnect activity. Connection orchestration is owned by a companion `ISignalRConnectionRegistry` service; the UI component renders derived state only.

---

## Use Cases

| # | Scenario | Behavior |
|---|----------|----------|
| 1 | All critical hubs healthy | Wireless icon, green state. Popup lists all hubs as "Healthy." |
| 2 | One or more hubs reconnecting | Wireless icon with amber badge/pulse. Popup shows affected rows as "Recovering." |
| 3 | Critical hub disconnected | Wireless-slash icon, red state. Popup shows latest error and manual retry action. |
| 4 | Only noncritical hubs unhealthy | Wireless icon, partial/info state. Popup distinguishes critical vs noncritical. |
| 5 | Initial startup failure | Registry retries initial `StartAsync` separately from `WithAutomaticReconnect` (auto-reconnect does not cover first connect). |
| 6 | Reconnect assigns new ConnectionId | Row updates to show new ID; optional "New connection id assigned" note. |
| 7 | Stale heartbeat threshold exceeded | Health degrades to "Degraded" even though `HubConnectionState` is still `Connected`. |

---

## Visual States

### Aggregate Icon States

| State | Icon | Badge | Condition |
|-------|------|-------|-----------|
| Healthy | wireless | none | All critical hubs connected |
| Degraded | wireless | amber dot/pulse | At least one critical hub reconnecting/connecting, none fully failed |
| Offline | wireless-slash | red dot | At least one critical hub disconnected |
| Partial | wireless | info dot | Only noncritical hubs are unhealthy |

### Per-Row Status Mapping

| HubConnectionState | Health Label | Badge Color | UI Enum |
|--------------------|-------------|-------------|---------|
| Connected (no recent error) | Healthy | Green | `ConnectionHealthState.Healthy` |
| Connecting | Starting | Neutral/Info | `ConnectionHealthState.Connecting` |
| Reconnecting | Recovering | Amber | `ConnectionHealthState.Recovering` |
| Disconnected | Offline | Red | `ConnectionHealthState.Offline` |
| Connected + stale heartbeat | Degraded | Red | `ConnectionHealthState.Degraded` |

---

## Interactive Behavior

- Click icon toggles popup open/closed
- Escape closes popup
- Click outside closes popup
- Focus moves into popup on open, returns to trigger on close
- Rows sorted by criticality first, then unhealthy before healthy
- Tooltip on icon: single-line summary (e.g., "SignalR: 2 of 3 hubs healthy")
- Optional "Reconnect" action button on disconnected rows

---

## Composition

| Part | Role |
|------|------|
| `MariloSignalRConnectionStatus` | Root icon-button trigger + popup host |
| `SignalRConnectionPopup` | Popup panel with header summary and hub list |
| `HubConnectionRow` | Single hub row: name, endpoint, state badge, error, retry info |

The root component is placed in the app shell, top bar, or status area. It renders the icon-button and conditionally renders the popup.

---

## Service Dependencies

| Dependency | Type | Purpose |
|------------|------|---------|
| `ISignalRConnectionRegistry` | DI (required) | Owns all tracked HubConnections, exposes snapshot + Changed event |
| `IMariloCssProvider` | DI (inherited) | CSS class generation from MariloComponentBase |
| `IMariloIconProvider` | DI (inherited) | Icon rendering |
| `TimeProvider` | DI (optional) | "Last updated" formatting; defaults to `TimeProvider.System` |

---

## Accessibility

- Trigger: `<button>` with `aria-label="SignalR connection status"`, `aria-haspopup="dialog"`, `aria-expanded` bound to open state
- Popup: `role="dialog"`, `aria-labelledby` pointing to header
- Each row exposes textual status (not color alone)
- Keyboard: Tab through rows, Escape to close, Enter/Space to activate reconnect

---

## SignalR Lifecycle Integration

Per Microsoft docs (ASP.NET Core SignalR .NET Client):

1. **`WithAutomaticReconnect()`** -- default retry delays: 0s, 2s, 10s, 30s. Stops after four failed attempts.
2. **`Reconnecting` event** -- fires before reconnect attempts begin; `HubConnectionState` transitions to `Reconnecting`.
3. **`Reconnected` event** -- fires on successful reconnect; receives new `ConnectionId` (may be null if negotiation skipped).
4. **`Closed` event** -- fires when all reconnect attempts exhausted or connection permanently lost; `HubConnectionState` transitions to `Disconnected`.
5. **Initial `StartAsync` is NOT covered** by `WithAutomaticReconnect`. Must be handled with explicit retry loop.
6. **Handlers must be registered after Build, before StartAsync.**
7. **Custom retry** via `WithAutomaticReconnect(TimeSpan[])` or `IRetryPolicy`.

---

## Theme Considerations

| Property | FluentUI | Bootstrap |
|----------|----------|-----------|
| Icon style | Fluent system icons (wireless/wireless-slash) | Bootstrap Icons equivalent |
| Badge colors | Fluent design tokens | Bootstrap contextual colors |
| Popup shadow | Fluent elevation | Bootstrap shadow-lg |
| Row spacing | Fluent spacing scale | Bootstrap spacing utilities |
| Separator | Subtle divider between critical/noncritical | `<hr>` or border-bottom |
| Timestamps | Monospace/tabular numerals | Monospace font |

---

## Hosting Note

This component is for **application-level hub connections** (Notifications, Presence, Jobs, etc.). It must not replace or conflict with Blazor's built-in circuit reconnect UI, which handles the framework-level SignalR circuit separately.
