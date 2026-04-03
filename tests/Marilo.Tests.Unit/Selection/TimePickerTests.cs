using Bunit;
using Marilo.Components.Forms.Inputs;
using Xunit;

namespace Marilo.Tests.Unit.Selection;

public class TimePickerTests : MariloTestBase
{
    // ── Rendering ───────────────────────────────────────────────────

    [Fact]
    public void RendersInputWithPlaceholder()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Placeholder, "Select time"));

        var input = cut.Find("input");
        Assert.Equal("Select time", input.GetAttribute("placeholder"));
    }

    [Fact]
    public void RendersClockIcon()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();

        Assert.NotNull(cut.Find(".mar-timepicker__icon [data-icon='clock']"));
    }

    [Fact]
    public void RendersToggleButton()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();

        Assert.NotNull(cut.Find(".mar-timepicker__toggle"));
    }

    // ── Popup open / close ──────────────────────────────────────────

    [Fact]
    public void PopupIsClosedByDefault()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();

        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void ToggleButtonOpensPopup()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();

        cut.Find(".mar-timepicker__toggle").Click();

        Assert.NotEmpty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void ToggleButtonClosesOpenPopup()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();

        cut.Find(".mar-timepicker__toggle").Click();
        Assert.NotEmpty(cut.FindAll("[role='dialog']"));

        cut.Find(".mar-timepicker__toggle").Click();
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void OverlayClickClosesPopup()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();

        cut.Find(".mar-timepicker__toggle").Click();
        Assert.NotEmpty(cut.FindAll("[role='dialog']"));

        cut.Find(".mar-timepicker__overlay").Click();
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void DisabledDoesNotOpenPopup()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Enabled, false));

        cut.Find(".mar-timepicker__toggle").Click();
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void ReadOnlyDoesNotOpenPopup()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.ReadOnly, true));

        cut.Find(".mar-timepicker__toggle").Click();
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    // ── Tumbler structure ───────────────────────────────────────────

    [Fact]
    public void PopupRendersHourAndMinuteColumns()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm"));

        cut.Find(".mar-timepicker__toggle").Click();

        var columns = cut.FindAll(".mar-timepicker__column");
        Assert.Equal(2, columns.Count);
    }

    [Fact]
    public void HourColumnRendersHourCells()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm"));

        cut.Find(".mar-timepicker__toggle").Click();

        var hourCells = cut.FindAll("[aria-label='Hours'] .mar-timepicker__cell");
        Assert.Equal(24, hourCells.Count);
        Assert.Equal("00", hourCells[0].TextContent.Trim());
        Assert.Equal("23", hourCells[^1].TextContent.Trim());
    }

    [Fact]
    public void MinuteColumnRendersMinuteCells()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm"));

        cut.Find(".mar-timepicker__toggle").Click();

        var minuteCells = cut.FindAll("[aria-label='Minutes'] .mar-timepicker__cell");
        Assert.Equal(60, minuteCells.Count);
    }

    [Fact]
    public void HourStepReducesCellCount()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm")
            .Add(x => x.HourStep, 2));

        cut.Find(".mar-timepicker__toggle").Click();

        var hourCells = cut.FindAll("[aria-label='Hours'] .mar-timepicker__cell");
        Assert.Equal(12, hourCells.Count);
    }

    [Fact]
    public void MinuteStepReducesCellCount()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm")
            .Add(x => x.MinuteStep, 15));

        cut.Find(".mar-timepicker__toggle").Click();

        var minuteCells = cut.FindAll("[aria-label='Minutes'] .mar-timepicker__cell");
        Assert.Equal(4, minuteCells.Count);
    }

    // ── Arrow stepping ──────────────────────────────────────────────

    [Fact]
    public void IncreaseHourArrowStepsHour()
    {
        var dt = new DateTime(2026, 1, 1, 10, 0, 0);

        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Value, dt)
            .Add(x => x.Format, "HH:mm"));

        cut.Find(".mar-timepicker__toggle").Click();

        cut.Find("[aria-label='Increase hours']").Click();

        var selectedCell = cut.Find("[aria-label='Hours'] .mar-timepicker__cell--selected");
        Assert.Equal("11", selectedCell.TextContent.Trim());
    }

    [Fact]
    public void DecreaseMinuteArrowStepsMinute()
    {
        var dt = new DateTime(2026, 1, 1, 10, 30, 0);

        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Value, dt)
            .Add(x => x.Format, "HH:mm"));

        cut.Find(".mar-timepicker__toggle").Click();

        cut.Find("[aria-label='Decrease minutes']").Click();

        var selectedCell = cut.Find("[aria-label='Minutes'] .mar-timepicker__cell--selected");
        Assert.Equal("29", selectedCell.TextContent.Trim());
    }

    // ── Cell selection ──────────────────────────────────────────────

    [Fact]
    public void ClickingHourCellSelectsIt()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm"));

        cut.Find(".mar-timepicker__toggle").Click();

        var hourCells = cut.FindAll("[aria-label='Hours'] .mar-timepicker__cell");
        hourCells[5].Click(); // Select hour 05

        var selected = cut.Find("[aria-label='Hours'] .mar-timepicker__cell--selected");
        Assert.Equal("05", selected.TextContent.Trim());
    }

    // ── Action buttons ──────────────────────────────────────────────

    [Fact]
    public void SetButtonCommitsAndCloses()
    {
        DateTime? val = null;

        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm")
            .Add(x => x.ValueChanged, v => val = v));

        cut.Find(".mar-timepicker__toggle").Click();

        // Click "Set"
        cut.Find(".mar-timepicker__btn--set").Click();

        Assert.NotNull(val);
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void CancelButtonClosesWithoutCommitting()
    {
        DateTime? val = null;

        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm")
            .Add(x => x.ValueChanged, v => val = v));

        cut.Find(".mar-timepicker__toggle").Click();
        cut.Find(".mar-timepicker__btn--cancel").Click();

        Assert.Null(val);
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void NowButtonSetsAndCloses()
    {
        DateTime? val = null;

        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm")
            .Add(x => x.ValueChanged, v => val = v));

        cut.Find(".mar-timepicker__toggle").Click();
        cut.Find(".mar-timepicker__btn--now").Click();

        Assert.NotNull(val);
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    // ── Clear button ────────────────────────────────────────────────

    [Fact]
    public void ClearButtonNotShownWhenNoValue()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.ShowClearButton, true));

        Assert.Empty(cut.FindAll(".mar-timepicker__clear"));
    }

    [Fact]
    public void ClearButtonShownWhenValueSet()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.ShowClearButton, true)
            .Add(x => x.Value, new DateTime(2026, 1, 1, 10, 30, 0)));

        Assert.NotEmpty(cut.FindAll(".mar-timepicker__clear"));
    }

    [Fact]
    public void ClearButtonClearsValue()
    {
        DateTime? val = new DateTime(2026, 1, 1, 10, 30, 0);

        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.ShowClearButton, true)
            .Add(x => x.Value, val)
            .Add(x => x.ValueChanged, v => val = v));

        cut.Find(".mar-timepicker__clear").Click();
        Assert.Null(val);
    }

    // ── Accessibility ───────────────────────────────────────────────

    [Fact]
    public void PopupHasDialogRole()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();

        cut.Find(".mar-timepicker__toggle").Click();

        var dialog = cut.Find("[role='dialog']");
        Assert.Equal("Choose time", dialog.GetAttribute("aria-label"));
    }

    [Fact]
    public void AriaExpandedReflectsState()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();

        Assert.Equal("false", cut.Find("input").GetAttribute("aria-expanded"));

        cut.Find(".mar-timepicker__toggle").Click();
        Assert.Equal("true", cut.Find("input").GetAttribute("aria-expanded"));
    }

    [Fact]
    public void HourColumnHasGroupRole()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm"));

        cut.Find(".mar-timepicker__toggle").Click();

        Assert.NotNull(cut.Find("[role='group'][aria-label='Hours']"));
    }

    [Fact]
    public void SelectedCellHasAriaSelectedTrue()
    {
        var dt = new DateTime(2026, 1, 1, 10, 0, 0);

        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Value, dt)
            .Add(x => x.Format, "HH:mm"));

        cut.Find(".mar-timepicker__toggle").Click();

        var selectedHour = cut.Find("[aria-label='Hours'] .mar-timepicker__cell--selected");
        Assert.Equal("true", selectedHour.GetAttribute("aria-selected"));
    }

    // ── Separator ───────────────────────────────────────────────────

    [Fact]
    public void RendersColonSeparator()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Format, "HH:mm"));

        cut.Find(".mar-timepicker__toggle").Click();

        var separator = cut.Find(".mar-timepicker__separator");
        Assert.Equal(":", separator.TextContent);
    }

    // ── TimeOnly TValue support ─────────────────────────────────────

    [Fact]
    public void TimeOnlySetButtonCommitsAndCloses()
    {
        TimeOnly? val = null;

        var cut = Render<MariloTimePicker<TimeOnly?>>(p => p
            .Add(x => x.Format, "HH:mm")
            .Add(x => x.ValueChanged, v => val = v));

        cut.Find(".mar-timepicker__toggle").Click();
        cut.Find(".mar-timepicker__btn--set").Click();

        Assert.NotNull(val);
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void TimeOnlyDisplaysValue()
    {
        var time = new TimeOnly(14, 30);

        var cut = Render<MariloTimePicker<TimeOnly?>>(p => p
            .Add(x => x.Value, time)
            .Add(x => x.Format, "HH:mm"));

        Assert.Contains("14:30", cut.Find("input").GetAttribute("value"));
    }

    [Fact]
    public void TimeSpanSetButtonCommitsAndCloses()
    {
        TimeSpan? val = null;

        var cut = Render<MariloTimePicker<TimeSpan?>>(p => p
            .Add(x => x.Format, "HH:mm")
            .Add(x => x.ValueChanged, v => val = v));

        cut.Find(".mar-timepicker__toggle").Click();
        cut.Find(".mar-timepicker__btn--set").Click();

        Assert.NotNull(val);
        Assert.Empty(cut.FindAll("[role='dialog']"));
    }
}
