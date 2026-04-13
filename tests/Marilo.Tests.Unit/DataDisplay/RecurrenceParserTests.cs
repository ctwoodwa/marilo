using Marilo.Core.Scheduling;
using Xunit;

namespace Marilo.Tests.Unit.DataDisplay;

/// <summary>
/// Edge-case and robustness tests for <see cref="RecurrenceParser"/>.
/// </summary>
public class RecurrenceParserTests
{
    // Wide range that won't clip any test occurrences (unless the test specifically needs clipping).
    private static readonly DateTime RangeStart = new(2026, 1, 1);
    private static readonly DateTime RangeEnd = new(2030, 12, 31);

    // ── FREQ=DAILY;COUNT=5 — exactly 5 occurrences ─────────────────

    [Fact]
    public void Daily_Count5_Returns_Exactly_5_Occurrences()
    {
        var start = new DateTime(2026, 4, 1, 9, 0, 0);
        var results = RecurrenceParser.GetOccurrences(
            "FREQ=DAILY;COUNT=5", start, RangeStart, RangeEnd).ToList();

        Assert.Equal(5, results.Count);
        Assert.Equal(new DateTime(2026, 4, 1, 9, 0, 0), results[0]);
        Assert.Equal(new DateTime(2026, 4, 2, 9, 0, 0), results[1]);
        Assert.Equal(new DateTime(2026, 4, 3, 9, 0, 0), results[2]);
        Assert.Equal(new DateTime(2026, 4, 4, 9, 0, 0), results[3]);
        Assert.Equal(new DateTime(2026, 4, 5, 9, 0, 0), results[4]);
    }

    // ── FREQ=WEEKLY;BYDAY=MO,WE,FR — only Mon/Wed/Fri ──────────────

    [Fact]
    public void Weekly_ByDay_MoWeFr_Returns_Only_Matching_Days()
    {
        var start = new DateTime(2026, 4, 6, 10, 0, 0); // Monday
        var rangeEnd = new DateTime(2026, 4, 19); // ~2 weeks

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=WEEKLY;BYDAY=MO,WE,FR", start, start, rangeEnd).ToList();

        Assert.True(results.Count >= 4, $"Expected at least 4 occurrences, got {results.Count}");

        foreach (var dt in results)
        {
            Assert.Contains(dt.DayOfWeek, new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday });
        }
    }

    // ── FREQ=MONTHLY;BYMONTHDAY=15 — 15th of each month ────────────

    [Fact]
    public void Monthly_ByMonthDay15_Returns_15th_Each_Month()
    {
        var start = new DateTime(2026, 1, 15, 8, 0, 0);
        var rangeEnd = new DateTime(2026, 6, 30);

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=MONTHLY;BYMONTHDAY=15", start, start, rangeEnd).ToList();

        Assert.Equal(6, results.Count);
        for (int i = 0; i < results.Count; i++)
        {
            Assert.Equal(15, results[i].Day);
            Assert.Equal(i + 1, results[i].Month);
        }
    }

    // ── FREQ=YEARLY;BYMONTH=6 — once per year in June ──────────────

    [Fact]
    public void Yearly_ByMonth6_Returns_Once_Per_Year_In_June()
    {
        var start = new DateTime(2026, 6, 1, 12, 0, 0);
        var rangeEnd = new DateTime(2030, 12, 31);

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=YEARLY;BYMONTH=6", start, start, rangeEnd).ToList();

        Assert.Equal(5, results.Count); // 2026-2030
        foreach (var dt in results)
        {
            Assert.Equal(6, dt.Month);
        }
    }

    // ── FREQ=DAILY;INTERVAL=2 — every other day ────────────────────

    [Fact]
    public void Daily_Interval2_Returns_Every_Other_Day()
    {
        var start = new DateTime(2026, 4, 1, 9, 0, 0);
        var rangeEnd = new DateTime(2026, 4, 10);

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=DAILY;INTERVAL=2", start, start, rangeEnd).ToList();

        Assert.Equal(5, results.Count); // Apr 1,3,5,7,9
        for (int i = 0; i < results.Count; i++)
        {
            Assert.Equal(1 + i * 2, results[i].Day);
        }
    }

    // ── FREQ=WEEKLY;UNTIL=20260501T000000Z — stops at date ─────────

    [Fact]
    public void Weekly_Until_Stops_At_Specified_Date()
    {
        var start = new DateTime(2026, 4, 6, 10, 0, 0); // Monday
        var rangeEnd = new DateTime(2026, 12, 31);

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=WEEKLY;UNTIL=20260501T000000Z", start, start, rangeEnd).ToList();

        // All results must be before or equal to May 1
        foreach (var dt in results)
        {
            Assert.True(dt <= new DateTime(2026, 5, 1), $"Occurrence {dt} exceeds UNTIL date");
        }

        Assert.True(results.Count >= 3, $"Expected at least 3 weekly occurrences, got {results.Count}");
    }

    // ── Invalid/empty RRULE — no crash, returns empty ───────────────

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("INVALID_RULE")]
    [InlineData("FREQ=BOGUS")]
    [InlineData("NOT_A_RULE;AT=ALL")]
    public void Invalid_Or_Empty_Rrule_Returns_Empty(string? rrule)
    {
        var start = new DateTime(2026, 4, 1);
        var results = RecurrenceParser.GetOccurrences(
            rrule!, start, RangeStart, RangeEnd).ToList();

        Assert.Empty(results);
    }

    // ── RecurrenceExceptions — specific dates excluded ──────────────

    [Fact]
    public void Exceptions_Exclude_Specific_Dates()
    {
        var start = new DateTime(2026, 4, 1, 9, 0, 0);
        var exceptions = new List<DateTime>
        {
            new(2026, 4, 3), // Exclude Apr 3
            new(2026, 4, 5)  // Exclude Apr 5
        };

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=DAILY;COUNT=7", start, RangeStart, RangeEnd, exceptions).ToList();

        // COUNT=7 means 7 iterations, but 2 are excluded = 5 returned
        Assert.Equal(5, results.Count);
        Assert.DoesNotContain(results, r => r.Date == new DateTime(2026, 4, 3));
        Assert.DoesNotContain(results, r => r.Date == new DateTime(2026, 4, 5));
    }

    [Fact]
    public void Null_Exceptions_Returns_All_Occurrences()
    {
        var start = new DateTime(2026, 4, 1, 9, 0, 0);

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=DAILY;COUNT=3", start, RangeStart, RangeEnd, null).ToList();

        Assert.Equal(3, results.Count);
    }

    [Fact]
    public void Empty_Exceptions_Returns_All_Occurrences()
    {
        var start = new DateTime(2026, 4, 1, 9, 0, 0);

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=DAILY;COUNT=3", start, RangeStart, RangeEnd, new List<DateTime>()).ToList();

        Assert.Equal(3, results.Count);
    }

    // ── Occurrence outside view range — not returned ────────────────

    [Fact]
    public void Occurrences_Outside_ViewRange_Not_Returned()
    {
        var start = new DateTime(2026, 4, 1, 9, 0, 0);
        // Occurrences land at 9:00 each day, so range must cover the time component
        var narrowRangeStart = new DateTime(2026, 4, 3);
        var narrowRangeEnd = new DateTime(2026, 4, 5, 23, 59, 59);

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=DAILY;COUNT=10", start, narrowRangeStart, narrowRangeEnd).ToList();

        // Apr 3, 4, 5 at 9:00 = 3 occurrences
        Assert.Equal(3, results.Count);
        Assert.All(results, r =>
        {
            Assert.True(r >= narrowRangeStart, $"Occurrence {r} before range start");
            Assert.True(r <= narrowRangeEnd, $"Occurrence {r} after range end");
        });
    }

    // ── Large COUNT (1000) — doesn't hang (perf test with timeout) ──

    [Fact(Timeout = 5000)] // 5-second timeout
    public async Task Large_Count_Does_Not_Hang()
    {
        await Task.Run(() =>
        {
            var start = new DateTime(2026, 1, 1, 8, 0, 0);

            var results = RecurrenceParser.GetOccurrences(
                "FREQ=DAILY;COUNT=1000", start, RangeStart, RangeEnd).ToList();

            Assert.Equal(1000, results.Count);
        });
    }

    [Fact(Timeout = 5000)]
    public async Task Large_Count_Weekly_Does_Not_Hang()
    {
        await Task.Run(() =>
        {
            var start = new DateTime(2026, 1, 5, 8, 0, 0); // Monday
            var wideEnd = new DateTime(2050, 12, 31);

            var results = RecurrenceParser.GetOccurrences(
                "FREQ=WEEKLY;BYDAY=MO,WE,FR;COUNT=1000", start, start, wideEnd).ToList();

            Assert.Equal(1000, results.Count);
        });
    }

    // ── RRULE: prefix stripping ─────────────────────────────────────

    [Fact]
    public void Rrule_Prefix_Stripped_Correctly()
    {
        var start = new DateTime(2026, 4, 1, 9, 0, 0);

        var results = RecurrenceParser.GetOccurrences(
            "RRULE:FREQ=DAILY;COUNT=3", start, RangeStart, RangeEnd).ToList();

        Assert.Equal(3, results.Count);
    }

    // ── BYMONTHDAY with months of varying length ────────────────────

    [Fact]
    public void Monthly_ByMonthDay31_Skips_Short_Months()
    {
        var start = new DateTime(2026, 1, 31, 10, 0, 0);
        var rangeEnd = new DateTime(2026, 6, 30);

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=MONTHLY;BYMONTHDAY=31", start, start, rangeEnd).ToList();

        // Months with 31 days in Jan-Jun: Jan(31), Mar(31), May(31) = 3
        Assert.Equal(3, results.Count);
        Assert.All(results, r => Assert.Equal(31, r.Day));
    }

    // ── Yearly by month and day ─────────────────────────────────────

    [Fact]
    public void Yearly_ByMonth_And_ByMonthDay_Combined()
    {
        var start = new DateTime(2026, 3, 15, 9, 0, 0);

        var results = RecurrenceParser.GetOccurrences(
            "FREQ=YEARLY;BYMONTH=3;BYMONTHDAY=15", start, RangeStart, RangeEnd).ToList();

        Assert.Equal(5, results.Count); // 2026-2030
        Assert.All(results, r =>
        {
            Assert.Equal(3, r.Month);
            Assert.Equal(15, r.Day);
        });
    }

    // ── Safety limit prevents infinite loops ────────────────────────

    [Fact(Timeout = 5000)]
    public async Task Safety_Limit_Caps_Unbounded_Daily()
    {
        await Task.Run(() =>
        {
            var start = new DateTime(2026, 1, 1);
            var wideEnd = new DateTime(2100, 12, 31);

            // No COUNT, no UNTIL — relies on maxIterations safety limit
            var results = RecurrenceParser.GetOccurrences(
                "FREQ=DAILY", start, start, wideEnd).ToList();

            // Should be capped by the 10_000 iteration limit or range end
            Assert.True(results.Count <= 10_000, $"Expected <=10000, got {results.Count}");
            Assert.True(results.Count > 0, "Expected at least 1 occurrence");
        });
    }
}
