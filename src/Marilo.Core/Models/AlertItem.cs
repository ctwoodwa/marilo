using Marilo.Core.Enums;

namespace Marilo.Core.Models;

public record AlertItem
{
    public required string Title { get; init; }
    public string? Detail { get; init; }
    public string? Module { get; init; }
    public AlertSeverity Severity { get; init; } = AlertSeverity.Info;
}
