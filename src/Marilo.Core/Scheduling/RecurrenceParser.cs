using System.Globalization;

namespace Marilo.Core.Scheduling;

/// <summary>
/// Parses iCalendar RRULE strings (RFC 5545 subset) and generates occurrence dates.
/// Supported properties: FREQ (DAILY, WEEKLY, MONTHLY, YEARLY), INTERVAL, COUNT, UNTIL, BYDAY, BYMONTHDAY, BYMONTH.
/// </summary>
public static class RecurrenceParser
{
    private static readonly Dictionary<string, DayOfWeek> DayMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["MO"] = DayOfWeek.Monday,
        ["TU"] = DayOfWeek.Tuesday,
        ["WE"] = DayOfWeek.Wednesday,
        ["TH"] = DayOfWeek.Thursday,
        ["FR"] = DayOfWeek.Friday,
        ["SA"] = DayOfWeek.Saturday,
        ["SU"] = DayOfWeek.Sunday
    };

    /// <summary>
    /// Generates occurrence dates for a recurrence rule within the specified date range.
    /// </summary>
    /// <param name="rrule">An iCalendar RRULE string (e.g. "FREQ=WEEKLY;BYDAY=MO,WE,FR;COUNT=10").</param>
    /// <param name="start">The start date/time of the original appointment.</param>
    /// <param name="rangeStart">The beginning of the visible date range.</param>
    /// <param name="rangeEnd">The end of the visible date range.</param>
    /// <param name="exceptions">Optional list of dates to exclude from the recurrence pattern.</param>
    /// <returns>An enumerable of occurrence start dates within the range.</returns>
    public static IEnumerable<DateTime> GetOccurrences(
        string rrule, DateTime start, DateTime rangeStart, DateTime rangeEnd,
        IReadOnlyList<DateTime>? exceptions = null)
    {
        if (string.IsNullOrWhiteSpace(rrule))
            yield break;

        var rule = ParseRule(rrule);
        if (rule.Frequency == null)
            yield break;

        var exceptionSet = exceptions is { Count: > 0 }
            ? new HashSet<DateTime>(exceptions.Select(e => e.Date))
            : null;

        var count = 0;
        var maxIterations = 10_000; // Safety limit
        var iterationCount = 0;

        foreach (var candidate in GenerateCandidates(rule, start))
        {
            if (++iterationCount > maxIterations)
                yield break;

            if (rule.Until.HasValue && candidate > rule.Until.Value)
                yield break;

            if (rule.Count.HasValue && count >= rule.Count.Value)
                yield break;

            count++;

            // Skip excluded dates
            if (exceptionSet != null && exceptionSet.Contains(candidate.Date))
                continue;

            if (candidate > rangeEnd)
                yield break;

            if (candidate >= rangeStart)
                yield return candidate;
        }
    }

    private static IEnumerable<DateTime> GenerateCandidates(ParsedRule rule, DateTime start)
    {
        var interval = Math.Max(1, rule.Interval);

        switch (rule.Frequency)
        {
            case RecurrenceFrequency.Daily:
                return GenerateDaily(start, interval);

            case RecurrenceFrequency.Weekly:
                return rule.ByDay.Count > 0
                    ? GenerateWeeklyByDay(start, interval, rule.ByDay)
                    : GenerateDaily(start, interval * 7);

            case RecurrenceFrequency.Monthly:
                return rule.ByMonthDay.Count > 0
                    ? GenerateMonthlyByMonthDay(start, interval, rule.ByMonthDay)
                    : GenerateMonthly(start, interval);

            case RecurrenceFrequency.Yearly:
                return rule.ByMonth.Count > 0 && rule.ByMonthDay.Count > 0
                    ? GenerateYearlyByMonthAndDay(start, interval, rule.ByMonth, rule.ByMonthDay)
                    : rule.ByMonth.Count > 0
                        ? GenerateYearlyByMonth(start, interval, rule.ByMonth)
                        : GenerateYearly(start, interval);

            default:
                return Enumerable.Empty<DateTime>();
        }
    }

    private static IEnumerable<DateTime> GenerateDaily(DateTime start, int interval)
    {
        var current = start;
        while (true)
        {
            yield return current;
            current = current.AddDays(interval);
        }
    }

    private static IEnumerable<DateTime> GenerateWeeklyByDay(
        DateTime start, int weekInterval, List<DayOfWeek> byDay)
    {
        // Start from the beginning of the week containing 'start'
        var weekStart = start.AddDays(-(((int)start.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7));
        var sortedDays = byDay.OrderBy(d => ((int)d - (int)DayOfWeek.Monday + 7) % 7).ToList();
        var isFirstWeek = true;

        while (true)
        {
            foreach (var day in sortedDays)
            {
                var offset = ((int)day - (int)DayOfWeek.Monday + 7) % 7;
                var candidate = weekStart.AddDays(offset);
                candidate = new DateTime(candidate.Year, candidate.Month, candidate.Day,
                    start.Hour, start.Minute, start.Second, start.Kind);

                if (candidate >= start)
                    yield return candidate;
            }

            if (isFirstWeek)
            {
                isFirstWeek = false;
                weekStart = weekStart.AddDays(7 * weekInterval);
            }
            else
            {
                weekStart = weekStart.AddDays(7 * weekInterval);
            }
        }
    }

    private static IEnumerable<DateTime> GenerateMonthly(DateTime start, int interval)
    {
        var current = start;
        while (true)
        {
            yield return current;
            current = AddMonthsClamped(current, interval);
        }
    }

    private static IEnumerable<DateTime> GenerateMonthlyByMonthDay(
        DateTime start, int interval, List<int> monthDays)
    {
        var year = start.Year;
        var month = start.Month;
        var sortedDays = monthDays.OrderBy(d => d).ToList();
        var isFirst = true;

        while (true)
        {
            var daysInMonth = DateTime.DaysInMonth(year, month);
            foreach (var day in sortedDays)
            {
                if (day < 1 || day > daysInMonth) continue;
                var candidate = new DateTime(year, month, day,
                    start.Hour, start.Minute, start.Second, start.Kind);

                if (isFirst && candidate < start) continue;
                yield return candidate;
            }

            isFirst = false;
            month += interval;
            while (month > 12)
            {
                month -= 12;
                year++;
            }
        }
    }

    private static IEnumerable<DateTime> GenerateYearly(DateTime start, int interval)
    {
        var current = start;
        while (true)
        {
            yield return current;
            var nextYear = current.Year + interval;
            var day = Math.Min(current.Day, DateTime.DaysInMonth(nextYear, current.Month));
            current = new DateTime(nextYear, current.Month, day,
                current.Hour, current.Minute, current.Second, current.Kind);
        }
    }

    private static IEnumerable<DateTime> GenerateYearlyByMonth(
        DateTime start, int interval, List<int> byMonth)
    {
        var year = start.Year;
        var sortedMonths = byMonth.OrderBy(m => m).ToList();
        var isFirst = true;

        while (true)
        {
            foreach (var month in sortedMonths)
            {
                if (month < 1 || month > 12) continue;
                var day = Math.Min(start.Day, DateTime.DaysInMonth(year, month));
                var candidate = new DateTime(year, month, day,
                    start.Hour, start.Minute, start.Second, start.Kind);

                if (isFirst && candidate < start) continue;
                yield return candidate;
            }

            isFirst = false;
            year += interval;
        }
    }

    private static IEnumerable<DateTime> GenerateYearlyByMonthAndDay(
        DateTime start, int interval, List<int> byMonth, List<int> byMonthDay)
    {
        var year = start.Year;
        var sortedMonths = byMonth.OrderBy(m => m).ToList();
        var sortedDays = byMonthDay.OrderBy(d => d).ToList();
        var isFirst = true;

        while (true)
        {
            foreach (var month in sortedMonths)
            {
                if (month < 1 || month > 12) continue;
                var daysInMonth = DateTime.DaysInMonth(year, month);
                foreach (var day in sortedDays)
                {
                    if (day < 1 || day > daysInMonth) continue;
                    var candidate = new DateTime(year, month, day,
                        start.Hour, start.Minute, start.Second, start.Kind);

                    if (isFirst && candidate < start) continue;
                    yield return candidate;
                }
            }

            isFirst = false;
            year += interval;
        }
    }

    private static DateTime AddMonthsClamped(DateTime dt, int months)
    {
        var newDate = dt.AddMonths(months);
        var maxDay = DateTime.DaysInMonth(newDate.Year, newDate.Month);
        if (newDate.Day > maxDay)
            newDate = new DateTime(newDate.Year, newDate.Month, maxDay,
                dt.Hour, dt.Minute, dt.Second, dt.Kind);
        return newDate;
    }

    internal static ParsedRule ParseRule(string rrule)
    {
        var result = new ParsedRule();

        // Strip "RRULE:" prefix if present
        if (rrule.StartsWith("RRULE:", StringComparison.OrdinalIgnoreCase))
            rrule = rrule.Substring(6);

        var parts = rrule.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var eqIndex = part.IndexOf('=');
            if (eqIndex < 0) continue;

            var key = part.Substring(0, eqIndex).Trim().ToUpperInvariant();
            var value = part.Substring(eqIndex + 1).Trim();

            switch (key)
            {
                case "FREQ":
                    result.Frequency = value.ToUpperInvariant() switch
                    {
                        "DAILY" => RecurrenceFrequency.Daily,
                        "WEEKLY" => RecurrenceFrequency.Weekly,
                        "MONTHLY" => RecurrenceFrequency.Monthly,
                        "YEARLY" => RecurrenceFrequency.Yearly,
                        _ => null
                    };
                    break;

                case "INTERVAL":
                    if (int.TryParse(value, out var interval))
                        result.Interval = interval;
                    break;

                case "COUNT":
                    if (int.TryParse(value, out var count))
                        result.Count = count;
                    break;

                case "UNTIL":
                    result.Until = ParseUntilDate(value);
                    break;

                case "BYDAY":
                    foreach (var dayStr in value.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        // Strip any numeric prefix (e.g., "1MO" -> "MO")
                        var trimmed = dayStr.Trim();
                        var dayCode = trimmed.Length > 2 ? trimmed.Substring(trimmed.Length - 2) : trimmed;
                        if (DayMap.TryGetValue(dayCode, out var dow))
                            result.ByDay.Add(dow);
                    }
                    break;

                case "BYMONTHDAY":
                    foreach (var dayStr in value.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (int.TryParse(dayStr.Trim(), out var monthDay))
                            result.ByMonthDay.Add(monthDay);
                    }
                    break;

                case "BYMONTH":
                    foreach (var monthStr in value.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (int.TryParse(monthStr.Trim(), out var month))
                            result.ByMonth.Add(month);
                    }
                    break;
            }
        }

        return result;
    }

    private static DateTime? ParseUntilDate(string value)
    {
        // Handle formats: "20261231T235959Z", "20261231T235959", "20261231", "2026-12-31T23:59:59"
        var cleaned = value.Replace("-", "").TrimEnd('Z', 'z');

        if (cleaned.Length >= 15 && DateTime.TryParseExact(cleaned.Substring(0, 15),
            "yyyyMMdd'T'HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtFull))
            return dtFull;

        if (cleaned.Length >= 8 && DateTime.TryParseExact(cleaned.Substring(0, 8),
            "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtDate))
            return dtDate.Date.AddDays(1).AddTicks(-1); // End of day

        // Fallback: try general parse
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtGeneral))
            return dtGeneral;

        return null;
    }

    internal enum RecurrenceFrequency
    {
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    internal class ParsedRule
    {
        public RecurrenceFrequency? Frequency { get; set; }
        public int Interval { get; set; } = 1;
        public int? Count { get; set; }
        public DateTime? Until { get; set; }
        public List<DayOfWeek> ByDay { get; } = new();
        public List<int> ByMonthDay { get; } = new();
        public List<int> ByMonth { get; } = new();
    }
}
