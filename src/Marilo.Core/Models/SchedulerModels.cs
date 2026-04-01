namespace Marilo.Core.Models;

/// <summary>
/// Represents an appointment in the MariloScheduler.
/// </summary>
public class SchedulerAppointment
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsAllDay { get; set; }
    public string? Color { get; set; }
    public string? RecurrenceRule { get; set; }
}

/// <summary>
/// Specifies the scheduler calendar view.
/// </summary>
public enum SchedulerView
{
    /// <summary>Day view showing hourly slots.</summary>
    Day,

    /// <summary>Week view showing 7 day columns.</summary>
    Week,

    /// <summary>Month view showing a calendar grid.</summary>
    Month
}
