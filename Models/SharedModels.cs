using System.Text.Json.Serialization;

namespace Marilo.Models;

// ── GraphQL infrastructure ──────────────────────────────────────

public class GraphQLResponse<T>
{
    public T? Data { get; set; }
    public List<GraphQLError>? Errors { get; set; }
}

public class GraphQLError
{
    public string Message { get; set; } = string.Empty;
}

// ── User / Role ─────────────────────────────────────────────────

public class UserModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public List<UserRoleModel> UserRoles { get; set; } = new();
}

public class UserRoleModel
{
    public RoleModel Role { get; set; } = new();
}

public class RoleModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
}

// ── Settings ────────────────────────────────────────────────────

public class SettingModel
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public bool IsReadOnly { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// ── Audit Log ───────────────────────────────────────────────────

public class AuditLogModel
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Module { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
}

public class AuditLogStatsModel
{
    public int TotalEntries { get; set; }
    public int TodayEntries { get; set; }
    public int UniqueUsers { get; set; }
    public List<ActionCountModel> TopActions { get; set; } = new();
}

public class ActionCountModel
{
    public string Action { get; set; } = string.Empty;
    public int Count { get; set; }
}

// ── Help Center ─────────────────────────────────────────────────

public class HelpArticleModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public int SortOrder { get; set; }
    public List<Guid> RelatedArticleIds { get; set; } = new();
    public bool IsPublished { get; set; } = true;
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public class HelpArticleRevisionModel
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public int VersionNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? EditedBy { get; set; }
    public string? EditSummary { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ArticleFeedbackSummaryModel
{
    public Guid ArticleId { get; set; }
    public int TotalFeedback { get; set; }
    public int HelpfulCount { get; set; }
    public int NotHelpfulCount { get; set; }
}

// ── Dashboard ───────────────────────────────────────────────────

public class WidgetModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string WidgetType { get; set; } = string.Empty;
    public string DataSource { get; set; } = string.Empty;
    public int Column { get; set; }
    public int Row { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsVisible { get; set; }
    public int SortOrder { get; set; }
    public string? Configuration { get; set; }
}

public class DashboardSummaryModel
{
    public int TotalWidgets { get; set; }
    public int VisibleWidgets { get; set; }
    public List<string> WidgetTypes { get; set; } = new();
}

// ── System Status ───────────────────────────────────────────────

public class SystemStatusModel
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int AuditEventsToday { get; set; }
    public int EnabledFeatureFlags { get; set; }
    public int TotalFeatureFlags { get; set; }
    public int UnreadNotifications { get; set; }
    public int ActiveWebhooks { get; set; }
    public int FailedWebhookDeliveries24h { get; set; }
    public int ActiveApiKeys { get; set; }
    public int PendingApprovals { get; set; }
    public int ActiveSessions { get; set; }
    public List<SystemAlertModel> Alerts { get; set; } = new();
    public DateTime Timestamp { get; set; }
}

public class SystemAlertModel
{
    public string Severity { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
}

// ── Reports ─────────────────────────────────────────────────────

public class ReportModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OutputFormat { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

// ── Shared GraphQL response wrappers ────────────────────────────

public class UsersResponse { public List<UserModel> Users { get; set; } = new(); }
public class RolesResponse { public List<RoleModel> Roles { get; set; } = new(); }
public class SettingsResponse { public List<SettingModel> Settings { get; set; } = new(); }
public class RecentAuditLogsResponse { public List<AuditLogModel> RecentAuditLogs { get; set; } = new(); }
public class AuditLogStatsResponse { public AuditLogStatsModel AuditLogStats { get; set; } = new(); }
public class HelpArticlesResponse { public List<HelpArticleModel> HelpArticles { get; set; } = new(); }
