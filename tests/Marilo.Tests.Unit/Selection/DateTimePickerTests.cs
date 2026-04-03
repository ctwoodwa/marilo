using Bunit;
using Marilo.Components.Forms.Inputs;
using Xunit;

namespace Marilo.Tests.Unit.Selection;

public class DateTimePickerTests : MariloTestBase
{
    // ── Rendering ───────────────────────────────────────────────────

    [Fact]
    public void RendersInputWithPlaceholder()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Placeholder, "Pick date & time"));

        var input = cut.Find("input");
        Assert.Equal("Pick date & time", input.GetAttribute("placeholder"));
    }

    [Fact]
    public void DisplaysFormattedValueInInput()
    {
        var dt = new DateTime(2026, 5, 20, 14, 30, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt)
            .Add(x => x.Format, "yyyy-MM-dd HH:mm"));

        Assert.Equal("2026-05-20 14:30", cut.Find("input").GetAttribute("value"));
    }

    [Fact]
    public void RendersCalendarIcon()
    {
        var cut = Render<MariloDateTimePicker>();

        Assert.NotNull(cut.Find(".mar-datetime-picker__icon [data-icon='calendar']"));
    }

    // ── Popup open / close ──────────────────────────────────────────

    [Fact]
    public void PopupIsClosedByDefault()
    {
        var cut = Render<MariloDateTimePicker>();

        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void ClickingInputOpensPopup()
    {
        var cut = Render<MariloDateTimePicker>();

        cut.Find("input").Click();

        Assert.NotEmpty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void EscapeClosesPopup()
    {
        var cut = Render<MariloDateTimePicker>();
        cut.Find("input").Click();
        Assert.NotEmpty(cut.FindAll("[role='dialog']"));

        cut.Find(".mar-datetime-picker").KeyDown(key: "Escape");
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void CancelButtonClosesPopup()
    {
        var cut = Render<MariloDateTimePicker>();
        cut.Find("input").Click();

        // Cancel is the ghost button
        cut.Find(".mar-datetime-picker__actions .mar-btn--ghost").Click();
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void OverlayClickClosesPopup()
    {
        var cut = Render<MariloDateTimePicker>();
        cut.Find("input").Click();

        cut.Find(".mar-datepicker__overlay").Click();
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void DisabledDoesNotOpenPopup()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Enabled, false));

        cut.Find("input").Click();
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void ReadOnlyDoesNotOpenPopup()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ReadOnly, true));

        cut.Find("input").Click();
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    // ── Calendar section ────────────────────────────────────────────

    [Fact]
    public void PopupRendersCalendarWithWeekdays()
    {
        var cut = Render<MariloDateTimePicker>();
        cut.Find("input").Click();

        var weekdays = cut.FindAll(".mar-calendar__weekday");
        Assert.Equal(7, weekdays.Count);
    }

    [Fact]
    public void PopupShowsCorrectMonth()
    {
        var dt = new DateTime(2026, 8, 10, 9, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt));

        cut.Find("input").Click();

        Assert.Contains("August 2026", cut.Find(".mar-calendar__title").TextContent);
    }

    [Fact]
    public void PreviousMonthNavigates()
    {
        var dt = new DateTime(2026, 8, 10, 9, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt));

        cut.Find("input").Click();
        cut.FindAll(".mar-calendar__nav-btn")[0].Click();

        Assert.Contains("July 2026", cut.Find(".mar-calendar__title").TextContent);
    }

    [Fact]
    public void NextMonthNavigates()
    {
        var dt = new DateTime(2026, 8, 10, 9, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt));

        cut.Find("input").Click();
        cut.FindAll(".mar-calendar__nav-btn")[1].Click();

        Assert.Contains("September 2026", cut.Find(".mar-calendar__title").TextContent);
    }

    [Fact]
    public void SelectedDayHasSelectedClass()
    {
        var dt = new DateTime(2026, 4, 15, 10, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt));

        cut.Find("input").Click();

        Assert.NotEmpty(cut.FindAll(".mar-calendar__day--selected"));
    }

    [Fact]
    public void TodayHasTodayClass()
    {
        var cut = Render<MariloDateTimePicker>();
        cut.Find("input").Click();

        Assert.NotEmpty(cut.FindAll(".mar-calendar__day--today"));
    }

    // ── Time tumblers ───────────────────────────────────────────────

    [Fact]
    public void PopupRendersTimeSectionWithLabel()
    {
        var cut = Render<MariloDateTimePicker>();
        cut.Find("input").Click();

        Assert.Contains("Time", cut.Find(".mar-datetime-picker__time-label").TextContent);
    }

    [Fact]
    public void PopupRendersHourAndMinuteTumblers()
    {
        var cut = Render<MariloDateTimePicker>();
        cut.Find("input").Click();

        var tumblers = cut.FindAll(".mar-datetime-picker__tumbler");
        Assert.Equal(2, tumblers.Count); // Hour + Minute (no seconds by default)
    }

    [Fact]
    public void ShowSecondsRendersThirdTumbler()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ShowSeconds, true));

        cut.Find("input").Click();

        var tumblers = cut.FindAll(".mar-datetime-picker__tumbler");
        Assert.Equal(3, tumblers.Count);
    }

    [Fact]
    public void IncrementHourUpdatesDisplay()
    {
        var dt = new DateTime(2026, 4, 10, 14, 30, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt));

        cut.Find("input").Click();

        // Hour tumbler "Increase hour" button
        cut.Find("[aria-label='Increase hour']").Click();

        var hourValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[0].TextContent;
        Assert.Equal("15", hourValue);
    }

    [Fact]
    public void DecrementMinuteUpdatesDisplay()
    {
        var dt = new DateTime(2026, 4, 10, 14, 30, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt));

        cut.Find("input").Click();

        cut.Find("[aria-label='Decrease minute']").Click();

        var minuteValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[1].TextContent;
        Assert.Equal("29", minuteValue);
    }

    [Fact]
    public void HourWrapsAt24()
    {
        var dt = new DateTime(2026, 4, 10, 23, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt));

        cut.Find("input").Click();
        cut.Find("[aria-label='Increase hour']").Click();

        var hourValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[0].TextContent;
        Assert.Equal("00", hourValue);
    }

    [Fact]
    public void MinuteWrapsAt60()
    {
        var dt = new DateTime(2026, 4, 10, 10, 59, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt));

        cut.Find("input").Click();
        cut.Find("[aria-label='Increase minute']").Click();

        var minuteValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[1].TextContent;
        Assert.Equal("00", minuteValue);
    }

    // ── Set / Commit ────────────────────────────────────────────────

    [Fact]
    public void SetButtonCommitsValueAndClosesPopup()
    {
        DateTime? committed = null;

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ValueChanged, v => committed = v));

        cut.Find("input").Click();

        // Click "Set" (primary button)
        cut.Find(".mar-datetime-picker__actions .mar-btn--primary").Click();

        Assert.NotNull(committed);
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void SetButtonFiresOnConfirm()
    {
        DateTime? confirmed = null;

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.OnConfirm, v => confirmed = v));

        cut.Find("input").Click();
        cut.Find(".mar-datetime-picker__actions .mar-btn--primary").Click();

        Assert.NotNull(confirmed);
    }

    [Fact]
    public void NowButtonSetsCurrentTimeAndCloses()
    {
        DateTime? committed = null;

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ValueChanged, v => committed = v));

        cut.Find("input").Click();

        // "Now" is the secondary button
        cut.Find(".mar-datetime-picker__actions .mar-btn--secondary").Click();

        Assert.NotNull(committed);
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    // ── Clear button ────────────────────────────────────────────────

    [Fact]
    public void ClearButtonNotShownWhenNoValue()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ShowClearButton, true));

        Assert.Empty(cut.FindAll(".mar-datepicker__clear"));
    }

    [Fact]
    public void ClearButtonShownWhenValueSet()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ShowClearButton, true)
            .Add(x => x.Value, new DateTime(2026, 4, 1, 10, 0, 0)));

        Assert.NotEmpty(cut.FindAll(".mar-datepicker__clear"));
    }

    [Fact]
    public void ClearButtonClearsValue()
    {
        DateTime? val = new DateTime(2026, 4, 1, 10, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ShowClearButton, true)
            .Add(x => x.Value, val)
            .Add(x => x.ValueChanged, v => val = v));

        cut.Find(".mar-datepicker__clear").Click();
        Assert.Null(val);
    }

    // ── Accessibility ───────────────────────────────────────────────

    [Fact]
    public void PopupHasDialogRoleAndAriaLabel()
    {
        var cut = Render<MariloDateTimePicker>();
        cut.Find("input").Click();

        var dialog = cut.Find("[role='dialog']");
        Assert.Equal("Select date and time", dialog.GetAttribute("aria-label"));
        Assert.Equal("true", dialog.GetAttribute("aria-modal"));
    }

    [Fact]
    public void AriaExpandedReflectsState()
    {
        var cut = Render<MariloDateTimePicker>();

        Assert.Equal("false", cut.Find("input").GetAttribute("aria-expanded"));

        cut.Find("input").Click();
        Assert.Equal("true", cut.Find("input").GetAttribute("aria-expanded"));
    }

    [Fact]
    public void HourTumblerHasSpinbuttonRole()
    {
        var cut = Render<MariloDateTimePicker>();
        cut.Find("input").Click();

        var spinbutton = cut.Find("[role='spinbutton'][aria-label='Hour']");
        Assert.NotNull(spinbutton);
    }

    [Fact]
    public void MinuteTumblerHasSpinbuttonRole()
    {
        var cut = Render<MariloDateTimePicker>();
        cut.Find("input").Click();

        var spinbutton = cut.Find("[role='spinbutton'][aria-label='Minute']");
        Assert.NotNull(spinbutton);
    }

    // ── Open class modifier ─────────────────────────────────────────

    [Fact]
    public void OpenStateAppliesModifierClass()
    {
        var cut = Render<MariloDateTimePicker>();

        Assert.Empty(cut.FindAll(".mar-datetime-picker--open"));

        cut.Find("input").Click();
        Assert.NotEmpty(cut.FindAll(".mar-datetime-picker--open"));
    }

    // ── Disabled dates ──────────────────────────────────────────────

    [Fact]
    public void DisabledDatesRenderWithDisabledClass()
    {
        var today = DateTime.Today;
        var first = new DateOnly(today.Year, today.Month, 1);
        var disabled = new List<DateOnly> { first.AddDays(4) };

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.DisabledDates, disabled));

        cut.Find("input").Click();

        Assert.NotEmpty(cut.FindAll(".mar-calendar__day--disabled"));
    }

    // ── PopupClass parameter ────────────────────────────────────────

    [Fact]
    public void PopupClassIsApplied()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.PopupClass, "custom-popup"));

        cut.Find("input").Click();

        Assert.Contains("custom-popup", cut.Find(".mar-datetime-picker__popup").GetAttribute("class"));
    }
}
