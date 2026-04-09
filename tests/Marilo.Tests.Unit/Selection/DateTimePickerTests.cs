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

    // ── Tumbler step parameters (GAP-DTP-002 / RES-T4B4-02) ─────────

    [Fact]
    public void HourStep_IncrementJumpsByConfiguredAmount()
    {
        var dt = new DateTime(2026, 4, 10, 10, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt)
            .Add(x => x.HourStep, 3));

        cut.Find("input").Click();
        cut.Find("[aria-label='Increase hour']").Click();

        var hourValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[0].TextContent;
        Assert.Equal("13", hourValue);
    }

    [Fact]
    public void HourStep_DecrementJumpsByConfiguredAmount()
    {
        var dt = new DateTime(2026, 4, 10, 10, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt)
            .Add(x => x.HourStep, 4));

        cut.Find("input").Click();
        cut.Find("[aria-label='Decrease hour']").Click();

        var hourValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[0].TextContent;
        Assert.Equal("06", hourValue);
    }

    [Fact]
    public void MinuteStep_IncrementJumpsByConfiguredAmount()
    {
        var dt = new DateTime(2026, 4, 10, 10, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt)
            .Add(x => x.MinuteStep, 15));

        cut.Find("input").Click();
        cut.Find("[aria-label='Increase minute']").Click();

        var minuteValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[1].TextContent;
        Assert.Equal("15", minuteValue);
    }

    [Fact]
    public void MinuteStep_DecrementJumpsByConfiguredAmountWithWrap()
    {
        var dt = new DateTime(2026, 4, 10, 10, 5, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt)
            .Add(x => x.MinuteStep, 10));

        cut.Find("input").Click();
        cut.Find("[aria-label='Decrease minute']").Click();

        // 5 - 10 wraps to 55
        var minuteValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[1].TextContent;
        Assert.Equal("55", minuteValue);
    }

    [Fact]
    public void SecondStep_IncrementJumpsByConfiguredAmount()
    {
        var dt = new DateTime(2026, 4, 10, 10, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt)
            .Add(x => x.ShowSeconds, true)
            .Add(x => x.SecondStep, 30));

        cut.Find("input").Click();
        cut.Find("[aria-label='Increase second']").Click();

        var secondValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[2].TextContent;
        Assert.Equal("30", secondValue);
    }

    [Fact]
    public void StepDefaults_IncrementByOneWhenNotConfigured()
    {
        var dt = new DateTime(2026, 4, 10, 10, 30, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt));

        cut.Find("input").Click();
        cut.Find("[aria-label='Increase minute']").Click();

        var minuteValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[1].TextContent;
        Assert.Equal("31", minuteValue);
    }

    [Fact]
    public void HourStep_ZeroIsClampedToOne()
    {
        var dt = new DateTime(2026, 4, 10, 10, 0, 0);

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, dt)
            .Add(x => x.HourStep, 0));

        cut.Find("input").Click();
        cut.Find("[aria-label='Increase hour']").Click();

        // Zero step would have frozen the tumbler; clamp to 1 means it advances
        var hourValue = cut.FindAll(".mar-datetime-picker__tumbler-value")[0].TextContent;
        Assert.Equal("11", hourValue);
    }

    // ── Typed input parsing (GAP-DTP-003 / RES-T4B5-02) ─────────────

    [Fact]
    public void Input_IsNotReadOnlyByDefault()
    {
        var cut = Render<MariloDateTimePicker>();

        // Blazor omits bool-false attributes — readonly should be absent when ReadOnly=false
        Assert.False(cut.Find("input").HasAttribute("readonly"));
    }

    [Fact]
    public void Input_RespectsReadOnlyParameter()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ReadOnly, true));

        // When ReadOnly=true, the readonly attribute is present
        Assert.True(cut.Find("input").HasAttribute("readonly"));
    }

    [Fact]
    public void TypedValidDate_UpdatesValue()
    {
        DateTime? captured = null;

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Format, "yyyy-MM-dd HH:mm")
            .Add(x => x.ValueChanged, v => captured = v));

        cut.Find("input").Input("2026-05-20 14:30");

        Assert.NotNull(captured);
        Assert.Equal(new DateTime(2026, 5, 20, 14, 30, 0), captured);
    }

    [Fact]
    public void TypedInvalidDate_LeavesValueUnchanged()
    {
        DateTime? captured = new DateTime(2026, 1, 1, 0, 0, 0);
        var changeCount = 0;

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, captured)
            .Add(x => x.Format, "yyyy-MM-dd HH:mm")
            .Add(x => x.ValueChanged, v => { changeCount++; captured = v; }));

        cut.Find("input").Input("not-a-date");

        // No ValueChanged fired for invalid input
        Assert.Equal(0, changeCount);
        Assert.Equal(new DateTime(2026, 1, 1, 0, 0, 0), captured);
    }

    [Fact]
    public void TypedDate_ClampedToMin()
    {
        DateTime? captured = null;

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Format, "yyyy-MM-dd HH:mm")
            .Add(x => x.Min, new DateTime(2026, 1, 1, 0, 0, 0))
            .Add(x => x.ValueChanged, v => captured = v));

        cut.Find("input").Input("2025-01-01 00:00");

        Assert.NotNull(captured);
        Assert.Equal(new DateTime(2026, 1, 1, 0, 0, 0), captured);
    }

    [Fact]
    public void TypedDate_ClampedToMax()
    {
        DateTime? captured = null;

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Format, "yyyy-MM-dd HH:mm")
            .Add(x => x.Max, new DateTime(2026, 12, 31, 23, 59, 59))
            .Add(x => x.ValueChanged, v => captured = v));

        cut.Find("input").Input("2030-06-15 12:00");

        Assert.NotNull(captured);
        Assert.Equal(new DateTime(2026, 12, 31, 23, 59, 59), captured);
    }

    [Fact]
    public void ClearingInput_ClearsValue()
    {
        DateTime? captured = new DateTime(2026, 5, 20, 14, 30, 0);
        var changeCount = 0;

        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, captured)
            .Add(x => x.Format, "yyyy-MM-dd HH:mm")
            .Add(x => x.ValueChanged, v => { changeCount++; captured = v; }));

        cut.Find("input").Input("");

        Assert.Equal(1, changeCount);
        Assert.Null(captured);
    }
}
