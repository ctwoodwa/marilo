# API Design: SignalRConnectionStatus

---

## 1. Enums

### ConnectionHealthState

**File:** `src/Marilo.Core/Enums/ConnectionHealthState.cs`

```csharp
public enum ConnectionHealthState
{
    Healthy,
    Connecting,
    Recovering,
    Degraded,
    Offline
}
```

### AggregateConnectionState

**File:** `src/Marilo.Core/Enums/AggregateConnectionState.cs`

```csharp
public enum AggregateConnectionState
{
    Healthy,
    Degraded,
    Offline,
    Partial
}
```

### ConnectionPopupPlacement

**File:** `src/Marilo.Core/Enums/ConnectionPopupPlacement.cs`

```csharp
public enum ConnectionPopupPlacement
{
    BottomStart,
    BottomEnd,
    TopStart,
    TopEnd
}
```

---

## 2. Models

### HubConnectionStatusItem

**File:** `src/Marilo.Core/Models/HubConnectionStatusItem.cs`

```csharp
public sealed record HubConnectionStatusItem(
    string Name,
    string Endpoint,
    bool IsCritical,
    HubConnectionState State,
    ConnectionHealthState Health,
    DateTimeOffset? LastConnectedAt,
    DateTimeOffset LastStateChangedAt,
    string? LastError,
    int? RetryCount,
    string? ConnectionId);
```

### SignalRHubRegistration

**File:** `src/Marilo.Core/Models/SignalRHubRegistration.cs`

```csharp
public sealed record SignalRHubRegistration(
    string Name,
    string Endpoint,
    bool IsCritical,
    Func<IServiceProvider, HubConnection> Factory);
```

---

## 3. Service Contract

### ISignalRConnectionRegistry

**File:** `src/Marilo.Core/Contracts/ISignalRConnectionRegistry.cs`

```csharp
public interface ISignalRConnectionRegistry : IAsyncDisposable
{
    IReadOnlyList<HubConnectionStatusItem> GetSnapshot();
    AggregateConnectionState AggregateState { get; }
    event Action? Changed;

    Task RegisterAsync(SignalRHubRegistration registration,
                       CancellationToken cancellationToken = default);
    Task StartAllAsync(CancellationToken cancellationToken = default);
    Task StopAllAsync(CancellationToken cancellationToken = default);
    Task RetryAsync(string name, CancellationToken cancellationToken = default);
}
```

**Implementation notes:**
- Build each connection via the `Factory` delegate so auth tokens, headers, and URLs can be injected
- Attach `Reconnecting`, `Reconnected`, and `Closed` handlers **before** calling `StartAsync`
- Handle initial `StartAsync` failure with explicit retry loop (auto-reconnect does not cover first connect)
- Use `lock` or thread-safe collection for snapshot state (follows `MariloNotificationService` pattern)
- Raise `Changed` on every lifecycle transition, retry count change, or health recalculation

**Aggregate state computation:**
- **Healthy:** all critical hubs in `Connected` state with no stale heartbeat
- **Degraded:** at least one critical hub `Reconnecting` or `Connecting`, none `Disconnected`
- **Offline:** at least one critical hub `Disconnected`
- **Partial:** all critical hubs healthy, at least one noncritical hub unhealthy

---

## 4. Component Parameters

### MariloSignalRConnectionStatus

**File:** `src/Marilo.Components/Feedback/MariloSignalRConnectionStatus.razor` + `.razor.cs`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ShowCounts` | `bool` | `true` | Show healthy/total count beside the icon |
| `IncludeNonCritical` | `bool` | `true` | Include optional hubs in the popup |
| `PopupPlacement` | `ConnectionPopupPlacement` | `BottomEnd` | Flyout alignment relative to trigger |
| `Title` | `string` | `"Real-time connections"` | Popup header text |
| `Compact` | `bool` | `false` | Smaller visual density for toolbar/header use |
| `Class` | `string?` | `null` | (inherited) Consumer CSS class |
| `Style` | `string?` | `null` | (inherited) Consumer inline style |
| `AdditionalAttributes` | `Dictionary<string, object>?` | `null` | (inherited) Unmatched HTML attributes |

### SignalRConnectionPopup (child, internal)

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `IsOpen` | `bool` | `false` | Whether popup is visible |
| `Title` | `string` | required | Header text |
| `Items` | `IReadOnlyList<HubConnectionStatusItem>` | required | Hub list to render |
| `AggregateState` | `AggregateConnectionState` | required | For header summary |
| `Placement` | `ConnectionPopupPlacement` | `BottomEnd` | Alignment |
| `OnClose` | `EventCallback` | -- | Fires when popup should close |
| `OnRetry` | `EventCallback<string>` | -- | Fires with hub name to retry |

### HubConnectionRow (child, internal)

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Item` | `HubConnectionStatusItem` | required | Hub data to render |
| `OnRetry` | `EventCallback<string>` | -- | Fires with hub name when user clicks reconnect |

---

## 5. Events

| Component | Event | Type | When |
|-----------|-------|------|------|
| `MariloSignalRConnectionStatus` | (none outward) | -- | State is read from registry service |
| `SignalRConnectionPopup` | `OnClose` | `EventCallback` | User presses Escape, clicks outside, or clicks close |
| `SignalRConnectionPopup` | `OnRetry` | `EventCallback<string>` | User clicks reconnect on a disconnected row |
| `HubConnectionRow` | `OnRetry` | `EventCallback<string>` | Bubbles up hub name |

---

## 6. CSS Provider Methods

**Add to `IMariloCssProvider.cs`:**

```csharp
// Root indicator
string SignalRStatusClass(AggregateConnectionState state, bool isCompact);

// Popup container
string SignalRPopupClass();

// Individual hub row
string SignalRRowClass(ConnectionHealthState health);

// Health badge
string SignalRBadgeClass(ConnectionHealthState health);
```

**CSS class patterns:**

| Method | FluentUI Output Example | Bootstrap Output Example |
|--------|------------------------|-------------------------|
| `SignalRStatusClass(Healthy, false)` | `mar-signalr-status mar-signalr-status--healthy` | `mar-signalr-status mar-signalr-status--healthy` |
| `SignalRStatusClass(Offline, true)` | `mar-signalr-status mar-signalr-status--offline mar-signalr-status--compact` | `mar-signalr-status mar-signalr-status--offline mar-signalr-status--compact` |
| `SignalRPopupClass()` | `mar-signalr-popup` | `mar-signalr-popup` |
| `SignalRRowClass(Recovering)` | `mar-signalr-row mar-signalr-row--recovering` | `mar-signalr-row mar-signalr-row--recovering` |
| `SignalRBadgeClass(Healthy)` | `mar-signalr-badge mar-signalr-badge--healthy` | `mar-signalr-badge badge badge-success mar-signalr-badge` |

---

## 7. RenderFragment Slots

None. The component is self-contained -- content is driven entirely by the registry service data.

---

## 8. Accessibility Contract

```html
<!-- Trigger button -->
<button class="@CombineClasses(CssProvider.SignalRStatusClass(state, Compact))"
        aria-label="SignalR connection status"
        aria-haspopup="dialog"
        aria-expanded="@_isOpen"
        title="@_tooltipSummary">
    <!-- wireless or wireless-slash icon -->
    <!-- optional count badge -->
</button>

<!-- Popup -->
<div role="dialog" aria-labelledby="signalr-popup-header">
    <h3 id="signalr-popup-header">@Title</h3>
    <!-- hub rows with textual status, not color alone -->
</div>
```

---

## 9. Health Mapping Rules

```
HubConnectionState.Connected + no recent error + heartbeat fresh → Healthy
HubConnectionState.Connected + stale heartbeat                  → Degraded
HubConnectionState.Connecting                                   → Connecting
HubConnectionState.Reconnecting                                 → Recovering
HubConnectionState.Disconnected                                 → Offline
```

---

## 10. File Layout

```
src/Marilo.Core/
  Enums/
    ConnectionHealthState.cs
    AggregateConnectionState.cs
    ConnectionPopupPlacement.cs
  Models/
    HubConnectionStatusItem.cs
    SignalRHubRegistration.cs
  Contracts/
    ISignalRConnectionRegistry.cs

src/Marilo.Components/Feedback/
  MariloSignalRConnectionStatus.razor
  MariloSignalRConnectionStatus.razor.cs
  SignalRConnectionPopup.razor
  HubConnectionRow.razor

src/Marilo.Core/Services/
  SignalRConnectionRegistry.cs

src/Marilo.Providers.FluentUI/
  FluentUICssProvider.cs              (add SignalR methods)
  Styles/_signalr-status.scss         (new)

src/Marilo.Providers.Bootstrap/
  BootstrapCssProvider.cs             (add SignalR methods)
  Styles/_bridge-signalr-status.scss  (new)
```

---

## 11. Example Usage

```razor
@* Minimal -- in app shell or toolbar *@
<MariloSignalRConnectionStatus />

@* Customized *@
<MariloSignalRConnectionStatus ShowCounts="true"
                                IncludeNonCritical="true"
                                PopupPlacement="ConnectionPopupPlacement.BottomEnd"
                                Title="Hub connections"
                                Compact="true" />
```

**Service registration:**

```csharp
// Program.cs
builder.Services.AddSingleton<ISignalRConnectionRegistry, SignalRConnectionRegistry>();

// Register hubs
var registry = app.Services.GetRequiredService<ISignalRConnectionRegistry>();
await registry.RegisterAsync(new SignalRHubRegistration(
    Name: "Notifications",
    Endpoint: "/hubs/notifications",
    IsCritical: true,
    Factory: sp => new HubConnectionBuilder()
        .WithUrl(navigationManager.ToAbsoluteUri("/hubs/notifications"))
        .WithAutomaticReconnect()
        .Build()));

await registry.StartAllAsync();
```

---

## 12. Acceptance Criteria

1. All critical hubs connected -- wireless icon, popup shows all healthy
2. Hub enters `Reconnecting` -- popup updates within one render cycle, row shows "Recovering"
3. Critical hub disconnected + retries exhausted -- wireless-slash icon, popup shows error
4. User opens popup -- sees every registered hub with criticality and health
5. Initial `StartAsync` fails -- registry retries per configured strategy (separate from auto-reconnect)
6. Reconnect succeeds -- row updates with new ConnectionId
7. Stale heartbeat -- row degrades to "Degraded" even if `HubConnectionState` is `Connected`
