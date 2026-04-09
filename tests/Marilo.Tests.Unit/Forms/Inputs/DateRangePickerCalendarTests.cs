using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// bUnit tests for MariloDateRangePicker calendar views (Year, Decade, drill-down)
/// and FocusStartAsync/FocusEndAsync JS interop methods.
/// </summary>
public class DateRangePickerCalendarTests : MariloTestBase
{
    // ══════════════════════════════════════════════════════════════════
    // B1 — Calendar Views
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void Year_View_Renders_Month_Tiles()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.View, CalendarView.Year));

        // Open the popup so the calendar panels render
        cut.Find("input").Click();

        var tiles = cut.FindAll(".mar-date-range-picker__month-tile");
        // Two panels x 12 months = 24 tiles
        Assert.Equal(24, tiles.Count);
    }

    [Fact]
    public void Decade_View_Renders_Year_Tiles()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.View, CalendarView.Decade));

        cut.Find("input").Click();

        var tiles = cut.FindAll(".mar-date-range-picker__year-tile");
        // Two panels x 12 years (decade start-1 .. decade start+10) = 24 tiles
        Assert.Equal(24, tiles.Count);
    }

    [Fact]
    public void Click_Month_Header_Drills_To_Year_View()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.View, CalendarView.Month));

        // Open popup
        cut.Find("input").Click();

        // In month view, the header title is a clickable button
        var headerBtn = cut.Find(".mar-calendar__title--clickable");
        Assert.NotNull(headerBtn);

        // Click to drill up to Year view
        headerBtn.Click();

        // Now year grid should be visible with month tiles
        var tiles = cut.FindAll(".mar-date-range-picker__month-tile");
        Assert.True(tiles.Count >= 12, $"Expected at least 12 month tiles, got {tiles.Count}");
    }

    // ══════════════════════════════════════════════════════════════════
    // B2 — FocusStartAsync / FocusEndAsync
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public async Task FocusStartAsync_Exists()
    {
        var cut = Render<MariloDateRangePicker>();

        // JSInterop is in loose mode — the call will succeed without a handler
        await cut.InvokeAsync(() => cut.Instance.FocusStartAsync());
    }

    [Fact]
    public async Task FocusEndAsync_Exists()
    {
        var cut = Render<MariloDateRangePicker>();

        await cut.InvokeAsync(() => cut.Instance.FocusEndAsync());
    }
}
