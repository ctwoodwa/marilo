using Bunit;
using Marilo.Components.Feedback;
using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Marilo.Tests.Unit.Feedback;

public class SignalRConnectionStatusTests : MariloTestBase
{
    private readonly MockSignalRConnectionRegistry _registry = new();

    public SignalRConnectionStatusTests()
    {
        Services.AddSingleton<ISignalRConnectionRegistry>(_registry);
    }

    [Fact]
    public void Renders_Default_WithNoHubs()
    {
        var cut = Render<MariloSignalRConnectionStatus>();

        Assert.Contains("mar-signalr-status", cut.Markup);
        Assert.Contains("mar-signalr-status--healthy", cut.Markup);
        Assert.Contains("aria-haspopup=\"dialog\"", cut.Markup);
        Assert.Contains("aria-expanded=\"false\"", cut.Markup);
    }

    [Fact]
    public void Shows_Healthy_State_When_All_Critical_Connected()
    {
        _registry.SetSnapshot([
            CreateItem("Notifications", isCritical: true, health: ConnectionHealthState.Healthy),
            CreateItem("Presence", isCritical: true, health: ConnectionHealthState.Healthy),
        ]);

        var cut = Render<MariloSignalRConnectionStatus>();

        Assert.Contains("mar-signalr-status--healthy", cut.Markup);
        Assert.Contains("2/2", cut.Markup);
    }

    [Fact]
    public void Shows_Offline_State_When_Critical_Hub_Disconnected()
    {
        _registry.SetSnapshot([
            CreateItem("Notifications", isCritical: true, health: ConnectionHealthState.Healthy),
            CreateItem("Jobs", isCritical: true, health: ConnectionHealthState.Offline),
        ], AggregateConnectionState.Offline);

        var cut = Render<MariloSignalRConnectionStatus>();

        Assert.Contains("mar-signalr-status--offline", cut.Markup);
        Assert.Contains("1/2", cut.Markup);
    }

    [Fact]
    public void Shows_Degraded_State_When_Critical_Hub_Reconnecting()
    {
        _registry.SetSnapshot([
            CreateItem("Notifications", isCritical: true, health: ConnectionHealthState.Healthy),
            CreateItem("Jobs", isCritical: true, health: ConnectionHealthState.Recovering),
        ], AggregateConnectionState.Degraded);

        var cut = Render<MariloSignalRConnectionStatus>();

        Assert.Contains("mar-signalr-status--degraded", cut.Markup);
    }

    [Fact]
    public void Hides_Counts_When_ShowCounts_False()
    {
        _registry.SetSnapshot([
            CreateItem("Notifications", isCritical: true, health: ConnectionHealthState.Healthy),
        ]);

        var cut = Render<MariloSignalRConnectionStatus>(p => p
            .Add(x => x.ShowCounts, false));

        Assert.DoesNotContain("mar-signalr-status__count", cut.Markup);
    }

    [Fact]
    public void Compact_Mode_Applies_Class()
    {
        var cut = Render<MariloSignalRConnectionStatus>(p => p
            .Add(x => x.Compact, true));

        Assert.Contains("mar-signalr-status--compact", cut.Markup);
    }

    [Fact]
    public void Popup_Not_Rendered_By_Default()
    {
        var cut = Render<MariloSignalRConnectionStatus>();

        Assert.DoesNotContain("mar-signalr-popup", cut.Markup);
    }

    [Fact]
    public void Click_Opens_Popup()
    {
        _registry.SetSnapshot([
            CreateItem("Notifications", isCritical: true, health: ConnectionHealthState.Healthy),
        ]);

        var cut = Render<MariloSignalRConnectionStatus>();

        cut.Find("button").Click();

        Assert.Contains("mar-signalr-popup", cut.Markup);
        Assert.Contains("aria-expanded=\"true\"", cut.Markup);
        Assert.Contains("role=\"dialog\"", cut.Markup);
    }

    [Fact]
    public void Popup_Shows_Hub_Rows()
    {
        _registry.SetSnapshot([
            CreateItem("Notifications", isCritical: true, health: ConnectionHealthState.Healthy),
            CreateItem("Jobs", isCritical: false, health: ConnectionHealthState.Offline, lastError: "Connection refused"),
        ]);

        var cut = Render<MariloSignalRConnectionStatus>();
        cut.Find("button").Click();

        Assert.Contains("Notifications", cut.Markup);
        Assert.Contains("Jobs", cut.Markup);
        Assert.Contains("Healthy", cut.Markup);
        Assert.Contains("Offline", cut.Markup);
        Assert.Contains("Connection refused", cut.Markup);
    }

    [Fact]
    public void Popup_Filters_NonCritical_When_IncludeNonCritical_False()
    {
        _registry.SetSnapshot([
            CreateItem("Notifications", isCritical: true, health: ConnectionHealthState.Healthy),
            CreateItem("Jobs", isCritical: false, health: ConnectionHealthState.Offline),
        ]);

        var cut = Render<MariloSignalRConnectionStatus>(p => p
            .Add(x => x.IncludeNonCritical, false));
        cut.Find("button").Click();

        Assert.Contains("Notifications", cut.Markup);
        Assert.DoesNotContain("Jobs", cut.Markup);
    }

    [Fact]
    public void Popup_Shows_Reconnect_Button_For_Offline_Hub()
    {
        _registry.SetSnapshot([
            CreateItem("Jobs", isCritical: true, health: ConnectionHealthState.Offline),
        ], AggregateConnectionState.Offline);

        var cut = Render<MariloSignalRConnectionStatus>();
        cut.Find("button").Click();

        Assert.Contains("Reconnect", cut.Markup);
    }

    [Fact]
    public void Popup_Shows_Custom_Title()
    {
        var cut = Render<MariloSignalRConnectionStatus>(p => p
            .Add(x => x.Title, "Hub connections"));
        cut.Find("button").Click();

        Assert.Contains("Hub connections", cut.Markup);
    }

    [Fact]
    public void Registry_Changed_Updates_UI()
    {
        _registry.SetSnapshot([
            CreateItem("Notifications", isCritical: true, health: ConnectionHealthState.Healthy),
        ]);

        var cut = Render<MariloSignalRConnectionStatus>();
        Assert.Contains("1/1", cut.Markup);

        _registry.SetSnapshot([
            CreateItem("Notifications", isCritical: true, health: ConnectionHealthState.Healthy),
            CreateItem("Jobs", isCritical: true, health: ConnectionHealthState.Healthy),
        ]);
        _registry.RaiseChanged();

        cut.WaitForState(() => cut.Markup.Contains("2/2"));
        Assert.Contains("2/2", cut.Markup);
    }

    [Fact]
    public void Tooltip_Shows_Summary()
    {
        _registry.SetSnapshot([
            CreateItem("Hub1", isCritical: true, health: ConnectionHealthState.Healthy),
            CreateItem("Hub2", isCritical: true, health: ConnectionHealthState.Offline),
        ], AggregateConnectionState.Offline);

        var cut = Render<MariloSignalRConnectionStatus>();

        Assert.Contains("1 of 2 hubs unhealthy", cut.Markup);
    }

    // ── Helpers ─────────────────────────────────────────────────────────

    private static HubConnectionStatusItem CreateItem(
        string name,
        bool isCritical = true,
        ConnectionHealthState health = ConnectionHealthState.Healthy,
        string? lastError = null,
        int? retryCount = null)
    {
        var state = health switch
        {
            ConnectionHealthState.Healthy => HubConnectionState.Connected,
            ConnectionHealthState.Connecting => HubConnectionState.Connecting,
            ConnectionHealthState.Recovering => HubConnectionState.Reconnecting,
            ConnectionHealthState.Offline => HubConnectionState.Disconnected,
            ConnectionHealthState.Degraded => HubConnectionState.Connected,
            _ => HubConnectionState.Disconnected
        };

        return new HubConnectionStatusItem(
            Name: name,
            Endpoint: $"/hubs/{name.ToLower()}",
            IsCritical: isCritical,
            State: state,
            Health: health,
            LastConnectedAt: health == ConnectionHealthState.Healthy ? DateTimeOffset.UtcNow : null,
            LastStateChangedAt: DateTimeOffset.UtcNow,
            LastError: lastError,
            RetryCount: retryCount,
            ConnectionId: health == ConnectionHealthState.Healthy ? Guid.NewGuid().ToString("N") : null);
    }
}

// ── Mock registry for unit tests ────────────────────────────────────

internal class MockSignalRConnectionRegistry : ISignalRConnectionRegistry
{
    private IReadOnlyList<HubConnectionStatusItem> _snapshot = [];
    private AggregateConnectionState _aggregateState = AggregateConnectionState.Healthy;

    public event Action? Changed;

    public AggregateConnectionState AggregateState => _aggregateState;

    public void SetSnapshot(IReadOnlyList<HubConnectionStatusItem> items, AggregateConnectionState? aggregate = null)
    {
        _snapshot = items;
        var healthy = items.All(i => !i.IsCritical || i.Health == ConnectionHealthState.Healthy);
        _aggregateState = aggregate ?? (healthy ? AggregateConnectionState.Healthy : AggregateConnectionState.Offline);
    }

    public void RaiseChanged() => Changed?.Invoke();

    public IReadOnlyList<HubConnectionStatusItem> GetSnapshot() => _snapshot;

    public Task RegisterAsync(SignalRHubRegistration registration, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task StartAllAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task StopAllAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task RetryAsync(string name, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
