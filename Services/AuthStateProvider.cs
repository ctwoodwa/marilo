using System.Net.Http.Json;

namespace Marilo.Services;

/// <summary>
/// Fetches the current user's auth state from the Gateway's BFF endpoint.
/// The Gateway handles Okta OIDC and exposes user info via /api/auth/user.
/// </summary>
public class AuthStateProvider
{
    private readonly HttpClient _http;
    private UserInfo? _cachedUser;

    public AuthStateProvider(HttpClient http)
    {
        _http = http;
    }

    public string UserName => _cachedUser?.DisplayName ?? "Guest";
    public string Email => _cachedUser?.Email ?? string.Empty;
    public string UserId => _cachedUser?.UserId ?? string.Empty;
    public bool IsAuthenticated => _cachedUser?.IsAuthenticated ?? false;
    public List<string> Roles => _cachedUser?.Roles ?? new();

    public bool IsInRole(string role) => Roles.Contains(role);

    public event Action? OnAuthStateChanged;

    public async Task InitializeAsync()
    {
        try
        {
            _cachedUser = await _http.GetFromJsonAsync<UserInfo>("/api/auth/user");
        }
        catch
        {
            _cachedUser = new UserInfo();
        }
        OnAuthStateChanged?.Invoke();
    }

    public async Task RefreshAsync()
    {
        _cachedUser = null;
        await InitializeAsync();
    }

    public void TriggerLogin(string? returnUrl = null)
    {
        var url = string.IsNullOrEmpty(returnUrl)
            ? "/api/auth/login"
            : $"/api/auth/login?returnUrl={Uri.EscapeDataString(returnUrl)}";
        OnLoginRequested?.Invoke(url);
    }

    public event Action<string>? OnLoginRequested;
}

public class UserInfo
{
    public bool IsAuthenticated { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
