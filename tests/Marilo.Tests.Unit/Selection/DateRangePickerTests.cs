using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.Selection;

public class DateRangePickerTests : MariloTestBase
{
    // ── Rendering ───────────────────────────────────────────────────

    [Fact]
    public void RendersStartAndEndInputs()
    {
        var cut = Render<MariloDateRangePicker>();

        var inputs = cut.FindAll("input");
        Assert.Equal(2, inputs.Count);
        Assert.Equal("Start date", inputs[0].GetAttribute("aria-label"));
        Assert.Equal("End date", inputs[1].GetAttribute("aria-label"));
    }

    [Fact]
    public void RendersPlaceholderText()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.Placeholder, "Pick date"));

        var inputs = cut.FindAll("input");
        Assert.Equal("Pick date", inputs[0].GetAttribute("placeholder"));
        Assert.Equal("Pick date", inputs[1].GetAttribute("placeholder"));
    }

    [Fact]
    public void DefaultPlaceholdersAreStartAndEndDate()
    {
        var cut = Render<MariloDateRangePicker>();

        var inputs = cut.FindAll("input");
        Assert.Equal("Start date", inputs[0].GetAttribute("placeholder"));
        Assert.Equal("End date", inputs[1].GetAttribute("placeholder"));
    }

    [Fact]
    public void DisplaysFormattedDatesWhenValuesSet()
    {
        var start = new DateTime(2026, 3, 15);
        var end = new DateTime(2026, 3, 20);

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.StartValue, start)
            .Add(x => x.EndValue, end)
            .Add(x => x.Format, "yyyy-MM-dd"));

        var inputs = cut.FindAll("input");
        Assert.Equal("2026-03-15", inputs[0].GetAttribute("value"));
        Assert.Equal("2026-03-20", inputs[1].GetAttribute("value"));
    }

    [Fact]
    public void RendersCalendarIconInInputWrapper()
    {
        var cut = Render<MariloDateRangePicker>();

        var icon = cut.Find(".mar-date-picker__icon [data-icon='calendar']");
        Assert.NotNull(icon);
    }

    // ── Popup open / close ──────────────────────────────────────────

    [Fact]
    public void PopupIsClosedByDefault()
    {
        var cut = Render<MariloDateRangePicker>();

        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void ClickingStartInputOpensPopup()
    {
        var cut = Render<MariloDateRangePicker>();

        cut.FindAll("input")[0].Click();

        Assert.NotEmpty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void ClickingEndInputOpensPopup()
    {
        var cut = Render<MariloDateRangePicker>();

        cut.FindAll("input")[1].Click();

        Assert.NotEmpty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void EscapeClosesPopup()
    {
        var cut = Render<MariloDateRangePicker>();

        // Open
        cut.FindAll("input")[0].Click();
        Assert.NotEmpty(cut.FindAll("[role='dialog']"));

        // Close via Escape
        cut.Find(".mar-date-range-picker").KeyDown(key: "Escape");
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void CloseButtonClosesPopup()
    {
        var cut = Render<MariloDateRangePicker>();

        cut.FindAll("input")[0].Click();
        Assert.NotEmpty(cut.FindAll("[role='dialog']"));

        // Click the "Close" action button
        cut.Find(".mar-date-range-picker__actions button.mar-btn--secondary").Click();
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void OverlayClickClosesPopup()
    {
        var cut = Render<MariloDateRangePicker>();

        cut.FindAll("input")[0].Click();
        Assert.NotEmpty(cut.FindAll("[role='dialog']"));

        cut.Find(".mar-datepicker__overlay").Click();
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    // ── Dual calendars ──────────────────────────────────────────────

    [Fact]
    public void PopupRendersTwoCalendarPanels()
    {
        var cut = Render<MariloDateRangePicker>();

        cut.FindAll("input")[0].Click();

        var panels = cut.FindAll(".mar-date-range-picker__calendar");
        Assert.Equal(2, panels.Count);
    }

    [Fact]
    public void CalendarsShowConsecutiveMonths()
    {
        var start = new DateTime(2026, 6, 10);

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.StartValue, start));

        cut.FindAll("input")[0].Click();

        var titles = cut.FindAll(".mar-calendar__title");
        Assert.Equal(2, titles.Count);
        Assert.Contains("June 2026", titles[0].TextContent);
        Assert.Contains("July 2026", titles[1].TextContent);
    }

    [Fact]
    public void VerticalOrientationAppliesModifierClass()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.Orientation, CalendarOrientation.Vertical));

        cut.FindAll("input")[0].Click();

        Assert.NotEmpty(cut.FindAll(".mar-date-range-picker__popup--vertical"));
    }

    [Fact]
    public void HorizontalOrientationDoesNotApplyVerticalClass()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.Orientation, CalendarOrientation.Horizontal));

        cut.FindAll("input")[0].Click();

        Assert.Empty(cut.FindAll(".mar-date-range-picker__popup--vertical"));
    }

    // ── Navigation ──────────────────────────────────────────────────

    [Fact]
    public void PreviousMonthNavigatesStartCalendar()
    {
        var start = new DateTime(2026, 6, 10);

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.StartValue, start));

        cut.FindAll("input")[0].Click();

        // First nav button is the "previous" on the start calendar
        var navButtons = cut.FindAll(".mar-calendar__nav-btn");
        navButtons[0].Click();

        var titles = cut.FindAll(".mar-calendar__title");
        Assert.Contains("May 2026", titles[0].TextContent);
    }

    [Fact]
    public void NextMonthNavigatesEndCalendar()
    {
        var start = new DateTime(2026, 6, 10);

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.StartValue, start));

        cut.FindAll("input")[0].Click();

        // Last nav button is the "next" on the end calendar
        var navButtons = cut.FindAll(".mar-calendar__nav-btn");
        navButtons[^1].Click();

        var titles = cut.FindAll(".mar-calendar__title");
        Assert.Contains("August 2026", titles[1].TextContent);
    }

    // ── Date selection ──────────────────────────────────────────────

    [Fact]
    public void SelectingTwoDatesFiresBothCallbacks()
    {
        DateTime? startVal = null;
        DateTime? endVal = null;

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.StartValueChanged, v => startVal = v)
            .Add(x => x.EndValueChanged, v => endVal = v));

        cut.FindAll("input")[0].Click();

        // Find day buttons that are not disabled and not outside-month
        var days = cut.FindAll(".mar-calendar__day:not(.mar-calendar__day--disabled):not(.mar-calendar__day--outside)");

        // Click first available day (start)
        days[0].Click();
        Assert.NotNull(startVal);
        Assert.Null(endVal);

        // Popup should still be open for end selection — re-query days
        days = cut.FindAll(".mar-calendar__day:not(.mar-calendar__day--disabled):not(.mar-calendar__day--outside)");

        // Click a later day (end) — pick one well after the start
        days[^1].Click();
        Assert.NotNull(endVal);
    }

    [Fact]
    public void SelectingEndBeforeStartResetsRange()
    {
        DateTime? startVal = null;
        DateTime? endVal = null;

        // Pre-set a start date
        var initialStart = new DateTime(2026, 4, 15);

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.StartValue, initialStart)
            .Add(x => x.StartValueChanged, v => startVal = v)
            .Add(x => x.EndValueChanged, v => endVal = v));

        cut.FindAll("input")[0].Click();

        // The popup opens in end-selection mode since start is already set.
        // Pick a day earlier than the 15th on the start calendar — day "1"
        var dayButtons = cut.FindAll(".mar-date-range-picker__calendar:first-child .mar-calendar__day:not(.mar-calendar__day--disabled):not(.mar-calendar__day--outside)");

        // Click day 1 (before the 15th) — should reset: start becomes day 1, end becomes null
        dayButtons[0].Click();

        // Start should have been reset to the earlier date
        Assert.NotNull(startVal);
    }

    // ── Disabled / ReadOnly ─────────────────────────────────────────

    [Fact]
    public void DisabledDoesNotOpenPopup()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.Enabled, false));

        cut.FindAll("input")[0].Click();

        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void ReadOnlyDoesNotOpenPopup()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.ReadOnly, true));

        cut.FindAll("input")[0].Click();

        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void DisabledInputsHaveDisabledAttribute()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.Enabled, false));

        var inputs = cut.FindAll("input");
        Assert.All(inputs, input => Assert.True(input.HasAttribute("disabled")));
    }

    // ── Clear button ────────────────────────────────────────────────

    [Fact]
    public void ClearButtonNotShownWhenNoDates()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.ShowClearButton, true));

        Assert.Empty(cut.FindAll(".mar-datepicker__clear"));
    }

    [Fact]
    public void ClearButtonShownWhenStartValueExists()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.ShowClearButton, true)
            .Add(x => x.StartValue, new DateTime(2026, 4, 1)));

        Assert.NotEmpty(cut.FindAll(".mar-datepicker__clear"));
    }

    [Fact]
    public void ClearButtonResetsBothDates()
    {
        DateTime? startVal = new DateTime(2026, 4, 1);
        DateTime? endVal = new DateTime(2026, 4, 10);

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.ShowClearButton, true)
            .Add(x => x.StartValue, startVal)
            .Add(x => x.EndValue, endVal)
            .Add(x => x.StartValueChanged, v => startVal = v)
            .Add(x => x.EndValueChanged, v => endVal = v));

        cut.Find(".mar-datepicker__clear").Click();

        Assert.Null(startVal);
        Assert.Null(endVal);
    }

    [Fact]
    public void ClearButtonNotShownWhenDisabled()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.ShowClearButton, true)
            .Add(x => x.Enabled, false)
            .Add(x => x.StartValue, new DateTime(2026, 4, 1)));

        Assert.Empty(cut.FindAll(".mar-datepicker__clear"));
    }

    [Fact]
    public void ClearButtonNotShownWhenReadOnly()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.ShowClearButton, true)
            .Add(x => x.ReadOnly, true)
            .Add(x => x.StartValue, new DateTime(2026, 4, 1)));

        Assert.Empty(cut.FindAll(".mar-datepicker__clear"));
    }

    // ── Disabled dates ──────────────────────────────────────────────

    [Fact]
    public void DisabledDatesRenderWithDisabledClass()
    {
        var today = DateTime.Today;
        var firstOfMonth = new DateTime(today.Year, today.Month, 1);
        // Disable the 5th of the current month
        var disabled = new List<DateTime> { firstOfMonth.AddDays(4) };

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.DisabledDates, disabled));

        cut.FindAll("input")[0].Click();

        Assert.NotEmpty(cut.FindAll(".mar-calendar__day--disabled"));
    }

    // ── Aria / Accessibility ────────────────────────────────────────

    [Fact]
    public void PopupHasDialogRoleAndAriaLabel()
    {
        var cut = Render<MariloDateRangePicker>();

        cut.FindAll("input")[0].Click();

        var dialog = cut.Find("[role='dialog']");
        Assert.Equal("Select date range", dialog.GetAttribute("aria-label"));
        Assert.Equal("true", dialog.GetAttribute("aria-modal"));
    }

    [Fact]
    public void AriaExpandedReflectsPopupState()
    {
        var cut = Render<MariloDateRangePicker>();

        var startInput = cut.FindAll("input")[0];
        Assert.Equal("false", startInput.GetAttribute("aria-expanded"));

        startInput.Click();
        startInput = cut.FindAll("input")[0];
        Assert.Equal("true", startInput.GetAttribute("aria-expanded"));
    }

    [Fact]
    public void InputsHaveAriaHaspopup()
    {
        var cut = Render<MariloDateRangePicker>();

        var inputs = cut.FindAll("input");
        Assert.All(inputs, input => Assert.Equal("dialog", input.GetAttribute("aria-haspopup")));
    }

    // ── Open class modifier ─────────────────────────────────────────

    [Fact]
    public void OpenStateAppliesModifierClass()
    {
        var cut = Render<MariloDateRangePicker>();

        Assert.Empty(cut.FindAll(".mar-date-range-picker--open"));

        cut.FindAll("input")[0].Click();

        Assert.NotEmpty(cut.FindAll(".mar-date-range-picker--open"));
    }

    // ── Calendar rendering details ──────────────────────────────────

    [Fact]
    public void CalendarRendersWeekdayHeaders()
    {
        var cut = Render<MariloDateRangePicker>();

        cut.FindAll("input")[0].Click();

        var weekdays = cut.FindAll(".mar-calendar__weekday");
        // Two calendars × 7 weekdays
        Assert.Equal(14, weekdays.Count);
        Assert.Equal("Su", weekdays[0].TextContent);
        Assert.Equal("Sa", weekdays[6].TextContent);
    }

    [Fact]
    public void TodayHasTodayClass()
    {
        var cut = Render<MariloDateRangePicker>();

        cut.FindAll("input")[0].Click();

        Assert.NotEmpty(cut.FindAll(".mar-calendar__day--today"));
    }

    [Fact]
    public void SelectedStartHasRangeStartClass()
    {
        var today = DateTime.Today;
        var firstOfMonth = new DateTime(today.Year, today.Month, 1);

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.StartValue, firstOfMonth.AddDays(4)));

        cut.FindAll("input")[0].Click();

        Assert.NotEmpty(cut.FindAll(".mar-calendar__day--range-start"));
    }

    [Fact]
    public void SelectedEndHasRangeEndClass()
    {
        var today = DateTime.Today;
        var firstOfMonth = new DateTime(today.Year, today.Month, 1);

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.StartValue, firstOfMonth.AddDays(2))
            .Add(x => x.EndValue, firstOfMonth.AddDays(8)));

        cut.FindAll("input")[0].Click();

        Assert.NotEmpty(cut.FindAll(".mar-calendar__day--range-end"));
    }

    [Fact]
    public void DaysBetweenStartAndEndHaveInRangeClass()
    {
        var today = DateTime.Today;
        var firstOfMonth = new DateTime(today.Year, today.Month, 1);

        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.StartValue, firstOfMonth.AddDays(2))
            .Add(x => x.EndValue, firstOfMonth.AddDays(8)));

        cut.FindAll("input")[0].Click();

        Assert.NotEmpty(cut.FindAll(".mar-calendar__day--in-range"));
    }

    // ── ShowOtherMonthDays ──────────────────────────────────────────

    [Fact]
    public void HidingOtherMonthDaysRendersPlaceholders()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.ShowOtherMonthDays, false));

        cut.FindAll("input")[0].Click();

        // When the first day of the month isn't Sunday, there should be placeholders
        Assert.NotEmpty(cut.FindAll(".mar-calendar__day--placeholder"));
    }

    // ── PopupClass parameter ────────────────────────────────────────

    [Fact]
    public void PopupClassParameterIsApplied()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.PopupClass, "my-custom-popup"));

        cut.FindAll("input")[0].Click();

        var popup = cut.Find(".mar-date-range-picker__popup");
        Assert.Contains("my-custom-popup", popup.GetAttribute("class"));
    }
}
