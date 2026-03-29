using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.SignalR.Client;

namespace Marilo.Services;

/// <summary>
/// Manages a SignalR connection to the DataChangeHub on the server.
/// Pages subscribe to entity types they're displaying and receive
/// real-time change events when other users modify that data.
/// </summary>
public class DataChangeService : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;
    private readonly AuthStateProvider _auth;
    private readonly HashSet<string> _subscribedEntityTypes = new();
    private readonly List<DataChangeAlert> _recentChanges = new();
    private const int MaxRecentChanges = 50;

    public DataChangeService(NavigationManager nav, AuthStateProvider auth)
    {
        _auth = auth;

        var hubUrl = nav.ToAbsoluteUri("/hubs/data-changes");
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.HttpMessageHandlerFactory = innerHandler =>
                    new IncludeCredentialsHandler(innerHandler);
            })
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) })
            .Build();

        _hubConnection.On<DataChangeAlert>("DataChanged", OnDataChanged);
        _hubConnection.Reconnected += OnReconnected;
        _hubConnection.Closed += OnClosed;
    }

    public event Action<DataChangeAlert>? OnChange;
    public event Action<bool>? OnConnectionStateChanged;

    public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;
    public IReadOnlyList<DataChangeAlert> RecentChanges => _recentChanges;

    public async Task StartAsync()
    {
        if (_hubConnection.State != HubConnectionState.Disconnected)
            return;

        if (!_auth.IsAuthenticated)
            return;

        try
        {
            await _hubConnection.StartAsync();
            OnConnectionStateChanged?.Invoke(true);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[DataChangeService] Failed to connect: {ex.Message}");
            OnConnectionStateChanged?.Invoke(false);
        }
    }

    public async Task SubscribeAsync(string entityType)
    {
        if (_subscribedEntityTypes.Add(entityType) && IsConnected)
            await _hubConnection.InvokeAsync("SubscribeToEntity", entityType);
    }

    public async Task UnsubscribeAsync(string entityType)
    {
        if (_subscribedEntityTypes.Remove(entityType) && IsConnected)
            await _hubConnection.InvokeAsync("UnsubscribeFromEntity", entityType);
    }

    public async Task SubscribeToInstanceAsync(string entityType, string entityId)
    {
        if (IsConnected)
            await _hubConnection.InvokeAsync("SubscribeToEntityInstance", entityType, entityId);
    }

    public async Task UnsubscribeFromInstanceAsync(string entityType, string entityId)
    {
        if (IsConnected)
            await _hubConnection.InvokeAsync("UnsubscribeFromEntityInstance", entityType, entityId);
    }

    public void Dismiss(Guid eventId)
    {
        var alert = _recentChanges.FirstOrDefault(c => c.EventId == eventId);
        if (alert != null) alert.IsDismissed = true;
    }

    public void DismissAll()
    {
        foreach (var alert in _recentChanges)
            alert.IsDismissed = true;
    }

    private void OnDataChanged(DataChangeAlert change)
    {
        if (!string.IsNullOrEmpty(_auth.UserId) && change.ChangedBy == _auth.UserId)
            return;

        change.ReceivedAt = DateTime.UtcNow;
        _recentChanges.Insert(0, change);
        if (_recentChanges.Count > MaxRecentChanges)
            _recentChanges.RemoveAt(_recentChanges.Count - 1);

        OnChange?.Invoke(change);
    }

    private async Task OnReconnected(string? connectionId)
    {
        OnConnectionStateChanged?.Invoke(true);
        foreach (var entityType in _subscribedEntityTypes)
            await _hubConnection.InvokeAsync("SubscribeToEntity", entityType);
    }

    private Task OnClosed(Exception? error)
    {
        OnConnectionStateChanged?.Invoke(false);
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        _hubConnection.Reconnected -= OnReconnected;
        _hubConnection.Closed -= OnClosed;

        if (_hubConnection.State != HubConnectionState.Disconnected)
            await _hubConnection.StopAsync();

        await _hubConnection.DisposeAsync();
    }
}

internal class IncludeCredentialsHandler : DelegatingHandler
{
    public IncludeCredentialsHandler(HttpMessageHandler inner) : base(inner) { }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.SameOrigin);
        return base.SendAsync(request, cancellationToken);
    }
}

public class DataChangeAlert
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    public string EntityType { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string ChangeType { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ChangedBy { get; set; } = string.Empty;
    public string ChangedByName { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public string Module { get; set; } = string.Empty;
    public string? AffectedEntityName { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public bool IsDismissed { get; set; }
}
