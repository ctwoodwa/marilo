using Bunit;
using Marilo.Components.Forms.Inputs;
using Xunit;

namespace Marilo.Tests.Unit.P2Enhancements;

public class DatePickerTests : MariloTestBase
{
    [Fact]
    public void DatePicker_RendersCalendarPopupOnFocus()
    {
        var cut = Render<MariloDatePicker>(parameters => parameters
            .Add(p => p.Value, new DateOnly(2025, 6, 15)));

        // Calendar should not be visible initially
        var calendar = cut.FindAll(".mar-calendar");
        Assert.Empty(calendar);

        // Click input to open calendar
        var input = cut.Find("input");
        input.Click();

        // Calendar should now be visible
        calendar = cut.FindAll(".mar-calendar");
        Assert.Single(calendar);
    }

    [Fact]
    public void DatePicker_DisabledDatesAreNotClickable()
    {
        var disabledDates = new List<DateOnly>
        {
            new(2025, 6, 10),
            new(2025, 6, 11),
        };

        DateOnly? selectedDate = null;

        var cut = Render<MariloDatePicker>(parameters => parameters
            .Add(p => p.Value, new DateOnly(2025, 6, 15))
            .Add(p => p.DisabledDates, disabledDates)
            .Add(p => p.ValueChanged, (DateOnly? v) => selectedDate = v));

        // Open calendar
        var input = cut.Find("input");
        input.Click();

        // Find disabled day buttons
        var disabledButtons = cut.FindAll(".mar-calendar__day--disabled");
        Assert.NotEmpty(disabledButtons);

        // Each disabled button should have the disabled attribute
        foreach (var btn in disabledButtons)
        {
            Assert.True(btn.HasAttribute("disabled"));
        }
    }

    [Fact]
    public void DatePicker_SelectingDayClosesCalendar()
    {
        DateOnly? selectedDate = null;

        var cut = Render<MariloDatePicker>(parameters => parameters
            .Add(p => p.Value, new DateOnly(2025, 6, 15))
            .Add(p => p.ValueChanged, (DateOnly? v) => selectedDate = v));

        // Open calendar
        var input = cut.Find("input");
        input.Click();

        // Find and click a non-disabled day (the 20th)
        var dayButtons = cut.FindAll(".mar-calendar__day:not(.mar-calendar__day--disabled):not(.mar-calendar__day--outside)");
        Assert.NotEmpty(dayButtons);

        // Click the first enabled day within the current month
        dayButtons[0].Click();

        // Calendar should close
        var calendar = cut.FindAll(".mar-calendar");
        Assert.Empty(calendar);

        // Value should be updated
        Assert.NotNull(selectedDate);
    }

    [Fact]
    public void DatePicker_DisabledPreventsCalendar()
    {
        var cut = Render<MariloDatePicker>(parameters => parameters
            .Add(p => p.Disabled, true));

        var input = cut.Find("input");
        input.Click();

        var calendar = cut.FindAll(".mar-calendar");
        Assert.Empty(calendar);
    }

    [Fact]
    public void DatePicker_NavigateMonths()
    {
        var cut = Render<MariloDatePicker>(parameters => parameters
            .Add(p => p.Value, new DateOnly(2025, 6, 15)));

        var input = cut.Find("input");
        input.Click();

        // Verify we see June 2025
        var title = cut.Find(".mar-calendar__title");
        Assert.Contains("June 2025", title.TextContent);

        // Click next month
        var nextBtn = cut.Find("[aria-label='Next month']");
        nextBtn.Click();

        title = cut.Find(".mar-calendar__title");
        Assert.Contains("July 2025", title.TextContent);

        // Click previous month twice to get to May
        var prevBtn = cut.Find("[aria-label='Previous month']");
        prevBtn.Click();
        prevBtn.Click();

        title = cut.Find(".mar-calendar__title");
        Assert.Contains("May 2025", title.TextContent);
    }
}
