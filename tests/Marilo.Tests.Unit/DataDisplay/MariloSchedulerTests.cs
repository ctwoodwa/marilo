using Bunit;
using Marilo.Components.DataDisplay;
using Marilo.Components.DataDisplay.Scheduler;
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

    // ── Child view registration ─────────────────────────────────────

    [Fact]
    public void ChildViews_Register_And_Show_In_Toolbar()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerDayView>(0);
                builder.CloseComponent();
                builder.OpenComponent<SchedulerWeekView>(1);
                builder.CloseComponent();
            }));

        // Only Day and Week buttons should appear (no Month since it was not registered)
        var buttons = cut.FindAll("button");
        var viewButtons = buttons.Where(b =>
            b.TextContent.Trim() == "Day" ||
            b.TextContent.Trim() == "Week" ||
            b.TextContent.Trim() == "Month").ToList();

        Assert.Equal(2, viewButtons.Count);
        Assert.Contains(viewButtons, b => b.TextContent.Trim() == "Day");
        Assert.Contains(viewButtons, b => b.TextContent.Trim() == "Week");
    }

    [Fact]
    public void CustomViewConfig_StartTime_Respected()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerDayView>(0);
                builder.AddAttribute(1, "StartTime", TimeSpan.FromHours(6));
                builder.AddAttribute(2, "EndTime", TimeSpan.FromHours(22));
                builder.CloseComponent();
            }));

        var timeLabels = cut.FindAll(".mar-scheduler__time-label");
        // 6..22 inclusive = 17 labels
        Assert.Equal(17, timeLabels.Count);
        Assert.Contains("06:00", cut.Markup);
        Assert.Contains("22:00", cut.Markup);
    }

    [Fact]
    public void DefaultViews_When_No_Children()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13)));

        // All three view buttons should appear by default
        var buttons = cut.FindAll("button");
        var viewButtons = buttons.Where(b =>
            b.TextContent.Trim() == "Day" ||
            b.TextContent.Trim() == "Week" ||
            b.TextContent.Trim() == "Month").ToList();

        Assert.Equal(3, viewButtons.Count);
    }

    [Fact]
    public async Task ViewSwitching_With_ChildViews()
    {
        SchedulerView? newView = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.ViewChanged, (SchedulerView v) => { newView = v; })
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerDayView>(0);
                builder.CloseComponent();
                builder.OpenComponent<SchedulerWeekView>(1);
                builder.CloseComponent();
            }));

        var weekBtn = cut.FindAll("button").First(b => b.TextContent.Trim() == "Week");
        await cut.InvokeAsync(() => weekBtn.Click());

        Assert.Equal(SchedulerView.Week, newView);
    }

    [Fact]
    public void WeekView_CustomFirstDayOfWeek_Respected()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Week)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerWeekView>(0);
                builder.AddAttribute(1, "FirstDayOfWeek", DayOfWeek.Monday);
                builder.CloseComponent();
            }));

        // Week should start on Monday - first day column header
        var dayHeaders = cut.FindAll(".mar-scheduler__day-column-header");
        Assert.StartsWith("Mon", dayHeaders[0].TextContent.Trim());
    }

    [Fact]
    public void ChildView_CustomLabel_Shows_In_Toolbar()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerDayView>(0);
                builder.AddAttribute(1, "Label", "Today");
                builder.CloseComponent();
            }));

        var buttons = cut.FindAll("button");
        Assert.Contains(buttons, b => b.TextContent.Trim() == "Today");
    }

    [Fact]
    public void Height_Width_Applied_As_Style()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.Height, "600px")
            .Add(s => s.Width, "100%"));

        var root = cut.Find(".mar-scheduler");
        var style = root.GetAttribute("style") ?? "";
        Assert.Contains("height:600px", style);
        Assert.Contains("width:100%", style);
    }

    [Fact]
    public void MonthView_CustomFirstDayOfWeek_Weekday_Headers()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerMonthView>(0);
                builder.AddAttribute(1, "FirstDayOfWeek", DayOfWeek.Monday);
                builder.CloseComponent();
            }));

        var headers = cut.FindAll(".mar-scheduler__weekday");
        Assert.Equal(7, headers.Count);
        // First header should be Mon
        Assert.Equal("Mon", headers[0].TextContent.Trim());
        // Last should be Sun
        Assert.Equal("Sun", headers[6].TextContent.Trim());
    }

    // ── CRUD Editing ────────────────────────────────────────────────

    [Fact]
    public async Task EditForm_Renders_On_DoubleClick_When_Editable()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true));

        // No edit form initially
        Assert.Empty(cut.FindAll(".mar-scheduler__edit-form"));

        // Double-click an appointment
        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        // Edit form should now render
        var editForm = cut.FindAll(".mar-scheduler__edit-form");
        Assert.Single(editForm);
        Assert.Contains("Edit Appointment", cut.Markup);
    }

    [Fact]
    public async Task EditForm_Does_Not_Render_When_Editable_Is_False()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, false));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        Assert.Empty(cut.FindAll(".mar-scheduler__edit-form"));
    }

    [Fact]
    public async Task OnUpdate_Fires_With_Updated_Appointment()
    {
        SchedulerAppointment? updated = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.OnUpdate, (SchedulerAppointment a) => { updated = a; }));

        // Double-click to open edit form
        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        // Click Save
        var saveBtn = cut.FindAll("button").First(b => b.TextContent.Trim() == "Save");
        await cut.InvokeAsync(() => saveBtn.Click());

        Assert.NotNull(updated);
        Assert.Equal("Sprint Planning", updated.Title);
    }

    [Fact]
    public async Task OnDelete_Fires_With_Correct_Appointment()
    {
        SchedulerAppointment? deleted = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.OnDelete, (SchedulerAppointment a) => { deleted = a; }));

        // Double-click to open edit form
        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        // Click Delete
        var deleteBtn = cut.FindAll("button").First(b => b.TextContent.Trim() == "Delete");
        await cut.InvokeAsync(() => deleteBtn.Click());

        Assert.NotNull(deleted);
        Assert.Equal("Sprint Planning", deleted.Title);
    }

    [Fact]
    public async Task EditForm_Closes_On_Cancel()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        Assert.Single(cut.FindAll(".mar-scheduler__edit-form"));

        var cancelBtn = cut.FindAll("button").First(b => b.TextContent.Trim() == "Cancel");
        await cut.InvokeAsync(() => cancelBtn.Click());

        Assert.Empty(cut.FindAll(".mar-scheduler__edit-form"));
    }

    // ── Appointment Template ────────────────────────────────────────

    [Fact]
    public void AppointmentTemplate_Renders_Custom_Content()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.AppointmentTemplate, (SchedulerAppointment a) => builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", "custom-appt");
                builder.AddContent(2, $"CUSTOM: {a.Title}");
                builder.CloseElement();
            }));

        var customElements = cut.FindAll(".custom-appt");
        Assert.True(customElements.Count >= 2, $"Expected at least 2 custom appointment elements, found {customElements.Count}");
        Assert.Contains("CUSTOM: Sprint Planning", cut.Markup);
    }

    [Fact]
    public void AppointmentTemplate_Used_In_WeekView()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Week)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.AppointmentTemplate, (SchedulerAppointment a) => builder =>
            {
                builder.OpenElement(0, "em");
                builder.AddAttribute(1, "class", "tpl-week");
                builder.AddContent(2, a.Title);
                builder.CloseElement();
            }));

        var tplElements = cut.FindAll(".tpl-week");
        Assert.True(tplElements.Count >= 1, "Expected at least 1 templated appointment in week view");
    }

    // ── All-day row ─────────────────────────────────────────────────

    [Fact]
    public void AllDayRow_Renders_For_AllDay_Appointments_In_MonthView()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments));

        var allDayRow = cut.FindAll(".mar-scheduler__allday-row");
        Assert.Single(allDayRow);
        Assert.Contains("Multi-day Event", cut.Find(".mar-scheduler__allday-row").TextContent);
    }

    [Fact]
    public void AllDayRow_Renders_In_WeekView()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 10))
            .Add(s => s.View, SchedulerView.Week)
            .Add(s => s.Appointments, SampleAppointments));

        var allDayRow = cut.FindAll(".mar-scheduler__allday-row");
        Assert.Single(allDayRow);
    }

    [Fact]
    public void AllDayRow_Does_Not_Render_Without_AllDay_Appointments()
    {
        var timedOnly = new List<SchedulerAppointment>
        {
            new()
            {
                Title = "Meeting",
                Start = new DateTime(2026, 4, 13, 9, 0, 0),
                End = new DateTime(2026, 4, 13, 10, 0, 0)
            }
        };

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, timedOnly));

        Assert.Empty(cut.FindAll(".mar-scheduler__allday-row"));
    }
}
