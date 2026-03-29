using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Marilo.Models;

namespace Marilo.Services;

/// <summary>
/// Base GraphQL client with shared query infrastructure and common queries
/// used by both the Web and Admin frontends.
/// </summary>
public abstract class GraphQLClientBase
{
    private readonly HttpClient _http;

    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    protected GraphQLClientBase(HttpClient http)
    {
        _http = http;
    }

    protected async Task<T?> QueryAsync<T>(string query, object? variables = null)
    {
        var request = new { query, variables };
        var json = JsonSerializer.Serialize(request, JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync("/graphql", content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<T>>(JsonOptions);
        return result != null ? result.Data : default;
    }

    // ── Shared queries available in both Web and Admin ───────────

    public async Task<List<UserModel>> GetUsersAsync()
    {
        var result = await QueryAsync<UsersResponse>(
            "query { users { id email displayName isActive createdAt lastLoginAt userRoles { role { name } } } }");
        return result?.Users ?? new();
    }

    public async Task<List<RoleModel>> GetRolesAsync()
    {
        var result = await QueryAsync<RolesResponse>(
            "query { roles { id name description permissions } }");
        return result?.Roles ?? new();
    }

    public async Task<List<SettingModel>> GetSettingsAsync()
    {
        var result = await QueryAsync<SettingsResponse>(
            "query { settings { id key value category description valueType isReadOnly updatedAt } }");
        return result?.Settings ?? new();
    }

    public async Task<List<AuditLogModel>> GetRecentAuditLogsAsync(int count = 50)
    {
        var result = await QueryAsync<RecentAuditLogsResponse>(
            $"query {{ recentAuditLogs(count: {count}) {{ id userId userEmail action entityType entityId timestamp module severity }} }}");
        return result?.RecentAuditLogs ?? new();
    }

    public async Task<AuditLogStatsModel> GetAuditLogStatsAsync()
    {
        var result = await QueryAsync<AuditLogStatsResponse>(
            "query { auditLogStats { totalEntries todayEntries uniqueUsers topActions { action count } } }");
        return result?.AuditLogStats ?? new();
    }

    public async Task<List<HelpArticleModel>> GetHelpArticlesAsync()
    {
        var result = await QueryAsync<HelpArticlesResponse>(
            "query { helpArticles { id title slug content category tags sortOrder } }");
        return result?.HelpArticles ?? new();
    }
}
