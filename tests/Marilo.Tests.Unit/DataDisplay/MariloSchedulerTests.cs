using Bunit;
using Marilo.Components.DataDisplay;
using Marilo.Core.Models;
using Xunit;

namespace Marilo.Tests.Unit.DataDisplay;

public class MariloSchedulerTests : MariloTestBase
{
    private static readonly List<SchedulerAppointment> SampleAppointments =
    [
        new()
        {
            Title = "Sprint Planning",
            Start = new DateTime(2026, 4, 13, 9, 0, 0),
            End = new DateTime(2026, 4, 13, 11, 0, 0),
            Color = "#0078d4"
        },
        new()
        {
            Title = "Design Review",
            Start = new DateTime(2026, 4, 14, 14, 0, 0),
            End = new DateTime(2026, 4, 14, 15, 30, 0),
        },
        new()
        {
            Title = "Multi-day Event",
            Start = new DateTime(2026, 4, 10),
            End = new DateTime(2026, 4, 12),
            IsAllDay = true,
        }
    ];

    // ── Renders without error ────────────────────────────────────────

    [Fact]
    public void Scheduler_Renders_Without_Error()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13)));

        var root = cut.Find(".mar-scheduler");
        Assert.NotNull(root);
    }

    [Fact]
    public void Scheduler_Renders_With_Empty_Appointments()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.Appointments, Enumerable.Empty<SchedulerAppointment>()));

        var root = cut.Find(".mar-scheduler");
        Assert.NotNull(root);
    }

    // ── Month view shows appointments ────────────────────────────────

    [Fact]
    public void MonthView_Shows_Appointment_Titles()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments));

        var appts = cut.FindAll(".mar-scheduler__appointment");
        // "Sprint Planning" and "Design Review" are in April;
        // "Multi-day Event" spans Apr 10-12 so shows on 10, 11, 12 = 3 cells
        Assert.True(appts.Count >= 2, $"Expected at least 2 appointment elements, found {appts.Count}");

        var markup = cut.Markup;
        Assert.Contains("Sprint Planning", markup);
        Assert.Contains("Design Review", markup);
    }

    [Fact]
    public void MonthView_Renders_WeekdayHeaders()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month));

        var headers = cut.FindAll(".mar-scheduler__weekday");
        Assert.Equal(7, headers.Count);
        Assert.Contains("Sun", cut.Markup);
        Assert.Contains("Sat", cut.Markup);
    }

    [Fact]
    public void MonthView_Header_Shows_MonthYear()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month));

        var title = cut.Find(".mar-scheduler__title");
        Assert.Contains("April 2026", title.TextContent);
    }

    // ── View switching ───────────────────────────────────────────────

    [Fact]
    public void DefaultView_Is_Month()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13)));

        // Month view renders the month grid
        var monthGrid = cut.FindAll(".mar-scheduler__month");
        Assert.Single(monthGrid);
    }

    [Fact]
    public void WeekView_Renders_TimeGrid()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Week)
            .Add(s => s.Appointments, SampleAppointments));

        var timeGrid = cut.FindAll(".mar-scheduler__time-grid");
        Assert.Single(timeGrid);

        // Week view should show 7 day columns
        var dayColumns = cut.FindAll(".mar-scheduler__day-column");
        Assert.Equal(7, dayColumns.Count);
    }

    [Fact]
    public void DayView_Renders_SingleDayColumn()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, SampleAppointments));

        var dayColumns = cut.FindAll(".mar-scheduler__day-column");
        Assert.Single(dayColumns);
    }

    [Fact]
    public async Task ViewChanged_Fires_On_Button_Click()
    {
        SchedulerView? newView = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.ViewChanged, (SchedulerView v) => { newView = v; }));

        // Click the "Week" button (third toolbar button, after prev/next arrows)
        var buttons = cut.FindAll("button");
        var weekBtn = buttons.First(b => b.TextContent.Trim() == "Week");
        await cut.InvokeAsync(() => weekBtn.Click());

        Assert.Equal(SchedulerView.Week, newView);
    }

    // ── Navigation ───────────────────────────────────────────────────

    [Fact]
    public async Task NavigateNext_In_MonthView_Advances_By_Month()
    {
        DateTime? newDate = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.CurrentDateChanged, (DateTime d) => { newDate = d; }));

        // First button is the "previous" nav, second is "next"
        var buttons = cut.FindAll("button");
        var nextBtn = buttons[1]; // ► button
        await cut.InvokeAsync(() => nextBtn.Click());

        Assert.NotNull(newDate);
        Assert.Equal(5, newDate.Value.Month); // May 2026
    }

    // ── StartHour / EndHour ──────────────────────────────────────────

    [Fact]
    public void WeekView_Respects_StartHour_EndHour()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Week)
            .Add(s => s.StartHour, 9)
            .Add(s => s.EndHour, 17));

        var timeLabels = cut.FindAll(".mar-scheduler__time-label");
        // 9, 10, 11, 12, 13, 14, 15, 16, 17 = 9 labels
        Assert.Equal(9, timeLabels.Count);
        Assert.Contains("09:00", cut.Markup);
        Assert.Contains("17:00", cut.Markup);
    }

    // ── Appointment click ────────────────────────────────────────────

    [Fact]
    public async Task OnAppointmentClick_Fires_With_Correct_Appointment()
    {
        SchedulerAppointment? clicked = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.OnAppointmentClick, (SchedulerAppointment a) => { clicked = a; }));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.Click());

        Assert.NotNull(clicked);
        Assert.Equal("Sprint Planning", clicked.Title);
    }

    // ── Appointment color ────────────────────────────────────────────

    [Fact]
    public void Appointment_Renders_Custom_Color()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));

        Assert.Contains("background-color:#0078d4", apptEl.GetAttribute("style"));
    }
}
