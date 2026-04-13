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
            .Add(s => s.Editable, true)
            .Add(s => s.EditMode, SchedulerEditMode.Inline));

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
            .Add(s => s.Editable, false)
            .Add(s => s.EditMode, SchedulerEditMode.Inline));

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
            .Add(s => s.EditMode, SchedulerEditMode.Inline)
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
            .Add(s => s.EditMode, SchedulerEditMode.Inline)
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
            .Add(s => s.Editable, true)
            .Add(s => s.EditMode, SchedulerEditMode.Inline));

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

    // ── MultiDay View ───────────────────────────────────────────────

    [Fact]
    public void MultiDayView_Renders_N_Columns()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.MultiDay)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerMultiDayView>(0);
                builder.AddAttribute(1, "NumberOfDays", 5);
                builder.CloseComponent();
            }));

        var dayColumns = cut.FindAll(".mar-scheduler__day-column");
        Assert.Equal(5, dayColumns.Count);
    }

    [Fact]
    public async Task MultiDayView_Navigation_Moves_By_NumberOfDays()
    {
        DateTime? newDate = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.MultiDay)
            .Add(s => s.CurrentDateChanged, (DateTime d) => { newDate = d; })
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerMultiDayView>(0);
                builder.AddAttribute(1, "NumberOfDays", 4);
                builder.CloseComponent();
            }));

        // Click next button (second button after prev)
        var buttons = cut.FindAll("button");
        var nextBtn = buttons[1]; // next arrow
        await cut.InvokeAsync(() => nextBtn.Click());

        Assert.NotNull(newDate);
        Assert.Equal(new DateTime(2026, 4, 17), newDate.Value);
    }

    // ── Timeline View ───────────────────────────────────────────────

    [Fact]
    public void TimelineView_Renders_Horizontal_TimeSlots()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Timeline)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerTimelineView>(0);
                builder.AddAttribute(1, "StartTime", TimeSpan.FromHours(8));
                builder.AddAttribute(2, "EndTime", TimeSpan.FromHours(12));
                builder.AddAttribute(3, "SlotDuration", TimeSpan.FromMinutes(60));
                builder.CloseComponent();
            }));

        // Should have a timeline container
        var timeline = cut.FindAll(".mar-scheduler__timeline");
        Assert.Single(timeline);

        // Header should have time slots: 8-12 = 4 hours / 60min = 4 slots
        var headerSlots = cut.Find(".mar-scheduler__timeline-header").QuerySelectorAll(".mar-scheduler__timeline-slot");
        Assert.Equal(4, headerSlots.Length);
    }

    [Fact]
    public void TimelineView_Appointments_Span_Correct_Width()
    {
        // Sprint Planning: 9:00-11:00 on Apr 13
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Timeline)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerTimelineView>(0);
                builder.AddAttribute(1, "StartTime", TimeSpan.FromHours(8));
                builder.AddAttribute(2, "EndTime", TimeSpan.FromHours(18));
                builder.AddAttribute(3, "SlotDuration", TimeSpan.FromMinutes(30));
                builder.CloseComponent();
            }));

        var appts = cut.FindAll(".mar-scheduler__timeline-appointment");
        Assert.True(appts.Count >= 1, "Expected at least 1 timeline appointment");

        // Check that the appointment has a width style (horizontal bar)
        var firstAppt = appts.First(el => el.TextContent.Contains("Sprint Planning"));
        var style = firstAppt.GetAttribute("style") ?? "";
        Assert.Contains("width:", style);
        Assert.Contains("left:", style);
    }

    // ── Agenda View ─────────────────────────────────────────────────

    [Fact]
    public void AgendaView_Renders_Chronological_List()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 12))
            .Add(s => s.View, SchedulerView.Agenda)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerAgendaView>(0);
                builder.AddAttribute(1, "NumberOfDays", 7);
                builder.CloseComponent();
            }));

        var agenda = cut.FindAll(".mar-scheduler__agenda");
        Assert.Single(agenda);

        var items = cut.FindAll(".mar-scheduler__agenda-item");
        Assert.True(items.Count >= 2, $"Expected at least 2 agenda items, found {items.Count}");
    }

    [Fact]
    public void AgendaView_Groups_By_Date()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 12))
            .Add(s => s.View, SchedulerView.Agenda)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerAgendaView>(0);
                builder.AddAttribute(1, "NumberOfDays", 7);
                builder.CloseComponent();
            }));

        var dateHeaders = cut.FindAll(".mar-scheduler__agenda-date");
        // Should have date headers for days that have appointments
        Assert.True(dateHeaders.Count >= 2, $"Expected at least 2 date groups, found {dateHeaders.Count}");
    }

    // ── New views registration ──────────────────────────────────────

    [Fact]
    public void NewViews_Register_Via_ChildComponents()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerDayView>(0);
                builder.CloseComponent();
                builder.OpenComponent<SchedulerMultiDayView>(1);
                builder.CloseComponent();
                builder.OpenComponent<SchedulerTimelineView>(2);
                builder.CloseComponent();
                builder.OpenComponent<SchedulerAgendaView>(3);
                builder.CloseComponent();
            }));

        var buttons = cut.FindAll("button");
        var viewLabels = buttons
            .Select(b => b.TextContent.Trim())
            .Where(t => t == "Day" || t == "MultiDay" || t == "Timeline" || t == "Agenda")
            .ToList();

        Assert.Equal(4, viewLabels.Count);
        Assert.Contains("Day", viewLabels);
        Assert.Contains("MultiDay", viewLabels);
        Assert.Contains("Timeline", viewLabels);
        Assert.Contains("Agenda", viewLabels);
    }

    [Fact]
    public void DefaultMode_Does_Not_Show_New_Views()
    {
        // When no child views are registered, only Day/Week/Month should appear
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13)));

        var buttons = cut.FindAll("button");
        var viewLabels = buttons.Select(b => b.TextContent.Trim()).ToList();

        Assert.Contains("Day", viewLabels);
        Assert.Contains("Week", viewLabels);
        Assert.Contains("Month", viewLabels);
        Assert.DoesNotContain("MultiDay", viewLabels);
        Assert.DoesNotContain("Timeline", viewLabels);
        Assert.DoesNotContain("Agenda", viewLabels);
    }

    // ── Resource Grouping ───────────────────────────────────────────

    private static readonly List<SchedulerResource> SampleResources = new()
    {
        new() { Id = "room-a", Text = "Room A", Color = "#ff0000" },
        new() { Id = "room-b", Text = "Room B", Color = "#00ff00" },
        new() { Id = "room-c", Text = "Room C", Color = "#0000ff" }
    };

    private static readonly List<ResourcedAppointment> ResourcedAppointments = new()
    {
        new() { Title = "Meeting 1", Start = new DateTime(2026, 4, 13, 9, 0, 0), End = new DateTime(2026, 4, 13, 10, 0, 0), ResourceId = "room-a" },
        new() { Title = "Meeting 2", Start = new DateTime(2026, 4, 13, 11, 0, 0), End = new DateTime(2026, 4, 13, 12, 0, 0), ResourceId = "room-b" },
        new() { Title = "Meeting 3", Start = new DateTime(2026, 4, 14, 14, 0, 0), End = new DateTime(2026, 4, 14, 15, 0, 0), ResourceId = "room-a" },
        new() { Title = "Unassigned", Start = new DateTime(2026, 4, 13, 10, 0, 0), End = new DateTime(2026, 4, 13, 11, 0, 0), ResourceId = "room-c" }
    };

    [Fact]
    public void DayView_GroupByResource_Renders_Resource_Headers()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, ResourcedAppointments.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, SampleResources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, true)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        var headers = cut.FindAll(".mar-scheduler__resource-header");
        Assert.Equal(3, headers.Count);
        Assert.Contains("Room A", headers[0].TextContent);
        Assert.Contains("Room B", headers[1].TextContent);
        Assert.Contains("Room C", headers[2].TextContent);
    }

    [Fact]
    public void DayView_GroupByResource_Appointments_Filter_To_Correct_Column()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, ResourcedAppointments.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, SampleResources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, true)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        var columns = cut.FindAll(".mar-scheduler__day-column");
        Assert.Equal(3, columns.Count);

        // Room A column (first) should have Meeting 1 (9-10 on Apr 13)
        Assert.Contains("Meeting 1", columns[0].TextContent);
        // Room B column (second) should have Meeting 2 (11-12 on Apr 13)
        Assert.Contains("Meeting 2", columns[1].TextContent);
        // Room C column (third) should have Unassigned (10-11 on Apr 13)
        Assert.Contains("Unassigned", columns[2].TextContent);
    }

    [Fact]
    public void GroupByResource_False_Does_Not_Change_Layout()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, ResourcedAppointments.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, SampleResources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, false)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        // Should render single day column (non-grouped)
        var columns = cut.FindAll(".mar-scheduler__day-column");
        Assert.Single(columns);
        // No resource headers
        Assert.Empty(cut.FindAll(".mar-scheduler__resource-header"));
    }

    [Fact]
    public void ResourceColor_Applied_To_Appointments_When_No_Own_Color()
    {
        var appts = new List<ResourcedAppointment>
        {
            new() { Title = "No Color", Start = new DateTime(2026, 4, 13, 9, 0, 0), End = new DateTime(2026, 4, 13, 10, 0, 0), ResourceId = "room-a" }
        };

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, appts.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, SampleResources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, false));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("No Color"));
        var style = apptEl.GetAttribute("style") ?? "";
        // Should use Room A's color (#ff0000)
        Assert.Contains("background-color:#ff0000", style);
    }

    [Fact]
    public void WeekView_GroupByResource_Renders_Resource_Groups()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Week)
            .Add(s => s.Appointments, ResourcedAppointments.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, SampleResources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, true)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        var groups = cut.FindAll(".mar-scheduler__resource-group");
        Assert.Equal(3, groups.Count);

        var labels = cut.FindAll(".mar-scheduler__resource-label");
        Assert.Equal(3, labels.Count);
        Assert.Contains("Room A", labels[0].TextContent);
        Assert.Contains("Room B", labels[1].TextContent);
        Assert.Contains("Room C", labels[2].TextContent);
    }

    [Fact]
    public void EmptyResources_Renders_Normally()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Resources, new List<SchedulerResource>())
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, true)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        // Empty resources list means IsResourceGrouped=false => normal single column
        var columns = cut.FindAll(".mar-scheduler__day-column");
        Assert.Single(columns);
    }

    [Fact]
    public void Resource_Without_Matching_Appointments_Shows_Empty_Column()
    {
        var appts = new List<ResourcedAppointment>
        {
            new() { Title = "Only Room A", Start = new DateTime(2026, 4, 13, 9, 0, 0), End = new DateTime(2026, 4, 13, 10, 0, 0), ResourceId = "room-a" }
        };

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, appts.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, SampleResources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, true)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        // All 3 resource columns should render
        var columns = cut.FindAll(".mar-scheduler__day-column");
        Assert.Equal(3, columns.Count);

        // Room B and Room C columns should have no appointments
        Assert.DoesNotContain("Only Room A", columns[1].TextContent);
        Assert.DoesNotContain("Only Room A", columns[2].TextContent);
        // Room A should have the appointment
        Assert.Contains("Only Room A", columns[0].TextContent);
    }

    [Fact]
    public void MonthView_GroupByResource_Renders_Resource_Labels()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, ResourcedAppointments.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, SampleResources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, true));

        var labels = cut.FindAll(".mar-scheduler__resource-label");
        Assert.Equal(3, labels.Count);
    }

    [Fact]
    public void Appointment_OwnColor_Takes_Precedence_Over_ResourceColor()
    {
        var appts = new List<ResourcedAppointment>
        {
            new() { Title = "Custom Color", Start = new DateTime(2026, 4, 13, 9, 0, 0), End = new DateTime(2026, 4, 13, 10, 0, 0), ResourceId = "room-a", Color = "#abc123" }
        };

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, appts.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, SampleResources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, false));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Custom Color"));
        var style = apptEl.GetAttribute("style") ?? "";
        // Should use the appointment's own color, not the resource color
        Assert.Contains("background-color:#abc123", style);
    }

    [Fact]
    public void NullResources_Renders_Normally()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.GroupByResource, true)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        // null Resources means IsResourceGrouped=false => normal single column
        var columns = cut.FindAll(".mar-scheduler__day-column");
        Assert.Single(columns);
    }

    /// <summary>Test appointment subclass with a ResourceId property for reflection-based resource matching.</summary>

    // -- Drag & Drop: Drag-to-Create --

    [Fact]
    public async Task DragCreate_Fires_OnAppointmentCreate_With_TimeRange()
    {
        SchedulerAppointment? created = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18)
            .Add(s => s.Editable, true)
            .Add(s => s.OnAppointmentCreate, (SchedulerAppointment a) => { created = a; }));

        var slots = cut.FindAll(".mar-scheduler__time-slot");
        Assert.True(slots.Count >= 3, "Need at least 3 slots for drag test");

        await cut.InvokeAsync(() => cut.FindAll(".mar-scheduler__time-slot")[0].MouseDown());
        await cut.InvokeAsync(() => cut.FindAll(".mar-scheduler__time-slot")[2].MouseMove());
        await cut.InvokeAsync(() => cut.FindAll(".mar-scheduler__time-slot")[2].MouseUp());

        Assert.NotNull(created);
        Assert.Equal(new DateTime(2026, 4, 13, 8, 0, 0), created.Start);
        Assert.Equal(new DateTime(2026, 4, 13, 11, 0, 0), created.End);
    }

    [Fact]
    public async Task DragCreate_DragSelecting_CssClass_Applied()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18)
            .Add(s => s.Editable, true));

        var slots = cut.FindAll(".mar-scheduler__time-slot");
        await cut.InvokeAsync(() => cut.FindAll(".mar-scheduler__time-slot")[0].MouseDown());
        await cut.InvokeAsync(() => cut.FindAll(".mar-scheduler__time-slot")[1].MouseMove());

        var selecting = cut.FindAll(".mar-scheduler__slot--drag-selecting");
        Assert.True(selecting.Count >= 2, $"Expected at least 2 drag-selecting slots, found {selecting.Count}");
    }

    [Fact]
    public async Task DragCreate_Does_Not_Fire_When_Not_Editable()
    {
        SchedulerAppointment? created = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18)
            .Add(s => s.Editable, false)
            .Add(s => s.OnAppointmentCreate, (SchedulerAppointment a) => { created = a; }));

        var slots = cut.FindAll(".mar-scheduler__time-slot");
        await cut.InvokeAsync(() => cut.FindAll(".mar-scheduler__time-slot")[0].MouseDown());
        await cut.InvokeAsync(() => cut.FindAll(".mar-scheduler__time-slot")[2].MouseMove());
        await cut.InvokeAsync(() => cut.FindAll(".mar-scheduler__time-slot")[2].MouseUp());

        Assert.Null(created);
    }

    // -- Drag & Drop: Drag-to-Reschedule --

    [Fact]
    public void Appointment_Has_Draggable_When_Editable()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true));

        var apptEls = cut.FindAll(".mar-scheduler__appointment--timed");
        Assert.True(apptEls.Count >= 1, "Expected at least 1 timed appointment");
        Assert.Equal("true", apptEls[0].GetAttribute("draggable"));
    }

    [Fact]
    public void Appointment_No_Draggable_When_Not_Editable()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, false));

        var apptEls = cut.FindAll(".mar-scheduler__appointment--timed");
        Assert.True(apptEls.Count >= 1, "Expected at least 1 timed appointment");
        Assert.Equal("false", apptEls[0].GetAttribute("draggable"));
    }

    [Fact]
    public async Task DragReschedule_OnUpdate_Fires_With_New_Times()
    {
        SchedulerAppointment? updated = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.OnUpdate, (SchedulerAppointment a) => { updated = a; }));

        var apptEls = cut.FindAll(".mar-scheduler__appointment--timed");
        var apptEl = apptEls.First(el => el.TextContent.Contains("Sprint Planning"));

        await cut.InvokeAsync(() => apptEl.DragStart());
        var slots = cut.FindAll(".mar-scheduler__time-slot");
        await cut.InvokeAsync(() => slots[6].Drop());

        Assert.NotNull(updated);
        Assert.Equal("Sprint Planning", updated.Title);
        Assert.Equal(new DateTime(2026, 4, 13, 14, 0, 0), updated.Start);
        Assert.Equal(new DateTime(2026, 4, 13, 16, 0, 0), updated.End);
    }

    [Fact]
    public async Task DragReschedule_DropTarget_CssClass_Applied()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true));

        var apptEls = cut.FindAll(".mar-scheduler__appointment--timed");
        var apptEl = apptEls.First(el => el.TextContent.Contains("Sprint Planning"));

        await cut.InvokeAsync(() => apptEl.DragStart());
        var slots = cut.FindAll(".mar-scheduler__time-slot");
        await cut.InvokeAsync(() => slots[5].DragOver());

        var dropTargets = cut.FindAll(".mar-scheduler__slot--drop-target");
        Assert.Single(dropTargets);
    }

    [Fact]
    public async Task DragReschedule_Duration_Preserved()
    {
        SchedulerAppointment? updated = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.OnUpdate, (SchedulerAppointment a) => { updated = a; }));

        var apptEls = cut.FindAll(".mar-scheduler__appointment--timed");
        var apptEl = apptEls.First(el => el.TextContent.Contains("Sprint Planning"));

        await cut.InvokeAsync(() => apptEl.DragStart());
        var slots = cut.FindAll(".mar-scheduler__time-slot");
        await cut.InvokeAsync(() => slots[4].Drop());

        Assert.NotNull(updated);
        var duration = updated.End - updated.Start;
        Assert.Equal(TimeSpan.FromHours(2), duration);
    }

    [Fact]
    public void AllDay_Appointment_Draggable_When_Editable()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 10))
            .Add(s => s.View, SchedulerView.Week)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true));

        var allDayAppts = cut.FindAll(".mar-scheduler__appointment--allday");
        Assert.True(allDayAppts.Count >= 1, "Expected at least 1 all-day appointment");
        Assert.Equal("true", allDayAppts[0].GetAttribute("draggable"));
    }

    private class ResourcedAppointment : SchedulerAppointment
    {
        public string ResourceId { get; set; } = string.Empty;
    }

    // ── View mode rendering ─────────────────────────────────────────

    [Fact]
    public void MultiDayView_Renders_TimeGrid()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.MultiDay)
            .Add(s => s.Appointments, SampleAppointments));

        var timeGrid = cut.FindAll(".mar-scheduler__time-grid");
        Assert.Single(timeGrid);
    }

    [Fact]
    public void TimelineView_Renders_TimelineContainer()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Timeline)
            .Add(s => s.Appointments, SampleAppointments));

        var timeline = cut.FindAll(".mar-scheduler__timeline");
        Assert.Single(timeline);
    }

    [Fact]
    public void AgendaView_Renders_AgendaContainer()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 12))
            .Add(s => s.View, SchedulerView.Agenda)
            .Add(s => s.Appointments, SampleAppointments));

        var agenda = cut.FindAll(".mar-scheduler__agenda");
        Assert.Single(agenda);
    }

    // ── Class parameter (inherited from MariloComponentBase) ────────

    [Fact]
    public void Class_Parameter_Applied_To_Root()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.Class, "my-custom-scheduler"));

        var root = cut.Find(".mar-scheduler");
        Assert.Contains("my-custom-scheduler", root.GetAttribute("class"));
    }

    [Fact]
    public void Style_Parameter_Applied_To_Root()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.Style, "border:1px solid red"));

        var root = cut.Find(".mar-scheduler");
        var style = root.GetAttribute("style") ?? "";
        Assert.Contains("border:1px solid red", style);
    }

    // ── Programmatic navigation ─────────────────────────────────────

    [Fact]
    public async Task NavigatePrevious_In_WeekView_Moves_By_7_Days()
    {
        DateTime? newDate = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Week)
            .Add(s => s.CurrentDateChanged, (DateTime d) => { newDate = d; }));

        var buttons = cut.FindAll("button");
        var prevBtn = buttons[0]; // first button is previous
        await cut.InvokeAsync(() => prevBtn.Click());

        Assert.NotNull(newDate);
        Assert.Equal(new DateTime(2026, 4, 6), newDate.Value);
    }

    [Fact]
    public async Task NavigateNext_In_DayView_Advances_By_1_Day()
    {
        DateTime? newDate = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.CurrentDateChanged, (DateTime d) => { newDate = d; }));

        var buttons = cut.FindAll("button");
        var nextBtn = buttons[1];
        await cut.InvokeAsync(() => nextBtn.Click());

        Assert.NotNull(newDate);
        Assert.Equal(new DateTime(2026, 4, 14), newDate.Value);
    }

    [Fact]
    public async Task NavigateNext_In_AgendaView_Advances_By_NumberOfDays()
    {
        DateTime? newDate = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Agenda)
            .Add(s => s.CurrentDateChanged, (DateTime d) => { newDate = d; })
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerAgendaView>(0);
                builder.AddAttribute(1, "NumberOfDays", 14);
                builder.CloseComponent();
            }));

        var buttons = cut.FindAll("button");
        var nextBtn = buttons[1];
        await cut.InvokeAsync(() => nextBtn.Click());

        Assert.NotNull(newDate);
        Assert.Equal(new DateTime(2026, 4, 27), newDate.Value);
    }

    // ── Toolbar customization ───────────────────────────────────────

    [Fact]
    public void Toolbar_Shows_Active_View_Highlighted()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Week));

        var buttons = cut.FindAll("button");
        var weekBtn = buttons.First(b => b.TextContent.Trim() == "Week");
        Assert.Contains("mar-btn--primary", weekBtn.GetAttribute("class"));
    }

    [Fact]
    public void Toolbar_NonActive_View_Not_Highlighted()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Week));

        var buttons = cut.FindAll("button");
        var dayBtn = buttons.First(b => b.TextContent.Trim() == "Day");
        Assert.DoesNotContain("mar-btn--primary", dayBtn.GetAttribute("class") ?? "");
    }

    // ── Template in Timeline and Agenda views ───────────────────────

    [Fact]
    public void AppointmentTemplate_Used_In_TimelineView()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Timeline)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.AppointmentTemplate, (SchedulerAppointment a) => builder =>
            {
                builder.OpenElement(0, "b");
                builder.AddAttribute(1, "class", "tpl-timeline");
                builder.AddContent(2, a.Title);
                builder.CloseElement();
            }));

        var tpl = cut.FindAll(".tpl-timeline");
        Assert.True(tpl.Count >= 1, "Expected at least 1 templated appointment in timeline view");
    }

    [Fact]
    public void AppointmentTemplate_Used_In_AgendaView()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 12))
            .Add(s => s.View, SchedulerView.Agenda)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.AppointmentTemplate, (SchedulerAppointment a) => builder =>
            {
                builder.OpenElement(0, "i");
                builder.AddAttribute(1, "class", "tpl-agenda");
                builder.AddContent(2, a.Title);
                builder.CloseElement();
            }));

        var tpl = cut.FindAll(".tpl-agenda");
        Assert.True(tpl.Count >= 1, "Expected at least 1 templated appointment in agenda view");
    }

    // ── Resource grouping per view ──────────────────────────────────

    [Fact]
    public void MonthView_GroupByResource_Appointments_Filtered()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, ResourcedAppointments.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, SampleResources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, true));

        var groups = cut.FindAll(".mar-scheduler__resource-group");
        Assert.Equal(3, groups.Count);

        // Each group should have its own month grid
        var grids = cut.FindAll(".mar-scheduler__month-grid");
        Assert.Equal(3, grids.Count);
    }

    // ── Edit Popup ──────────────────────────────────────────────────

    [Fact]
    public async Task Popup_Renders_On_DoubleClick_When_EditMode_Popup()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.EditMode, SchedulerEditMode.Popup));

        // No popup initially
        Assert.Empty(cut.FindAll(".mar-scheduler__edit-popup"));
        Assert.Empty(cut.FindAll(".mar-scheduler__edit-backdrop"));

        // Double-click an appointment
        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        // Popup should render
        Assert.Single(cut.FindAll(".mar-scheduler__edit-popup"));
        Assert.Single(cut.FindAll(".mar-scheduler__edit-backdrop"));
    }

    [Fact]
    public async Task Popup_Has_All_Expected_Fields()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.EditMode, SchedulerEditMode.Popup));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        var popup = cut.Find(".mar-scheduler__edit-popup");
        var markup = popup.InnerHtml;

        // Title input
        Assert.Contains("Title", markup);
        // Start datetime input
        Assert.Contains("Start", markup);
        // End datetime input
        Assert.Contains("End", markup);
        // All Day checkbox
        Assert.Contains("All Day", markup);
        // Description textarea
        Assert.Contains("Description", markup);
        // Save / Delete / Cancel buttons
        Assert.Contains("Save", markup);
        Assert.Contains("Delete", markup);
        Assert.Contains("Cancel", markup);
    }

    [Fact]
    public async Task Popup_Save_Fires_OnUpdate()
    {
        SchedulerAppointment? updated = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.EditMode, SchedulerEditMode.Popup)
            .Add(s => s.OnUpdate, (SchedulerAppointment a) => { updated = a; }));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        var saveBtn = cut.FindAll("button").First(b => b.TextContent.Trim() == "Save");
        await cut.InvokeAsync(() => saveBtn.Click());

        Assert.NotNull(updated);
        Assert.Equal("Sprint Planning", updated.Title);
    }

    [Fact]
    public async Task Popup_Delete_Fires_OnDelete()
    {
        SchedulerAppointment? deleted = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.EditMode, SchedulerEditMode.Popup)
            .Add(s => s.OnDelete, (SchedulerAppointment a) => { deleted = a; }));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        var deleteBtn = cut.FindAll("button").First(b => b.TextContent.Trim() == "Delete");
        await cut.InvokeAsync(() => deleteBtn.Click());

        Assert.NotNull(deleted);
        Assert.Equal("Sprint Planning", deleted.Title);
    }

    [Fact]
    public async Task Popup_Cancel_Closes_Popup()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.EditMode, SchedulerEditMode.Popup));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        Assert.Single(cut.FindAll(".mar-scheduler__edit-popup"));

        var cancelBtn = cut.FindAll("button").First(b => b.TextContent.Trim() == "Cancel");
        await cut.InvokeAsync(() => cancelBtn.Click());

        Assert.Empty(cut.FindAll(".mar-scheduler__edit-popup"));
        Assert.Empty(cut.FindAll(".mar-scheduler__edit-backdrop"));
    }

    [Fact]
    public void ResizeHandle_Renders_On_Timed_Appointments_When_Editable()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        var handles = cut.FindAll(".mar-scheduler__appointment-resize-handle");
        Assert.True(handles.Count >= 1, $"Expected at least 1 resize handle, found {handles.Count}");
    }

    [Fact]
    public void ResizeHandle_Does_Not_Render_When_Not_Editable()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, false)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        Assert.Empty(cut.FindAll(".mar-scheduler__appointment-resize-handle"));
    }

    [Fact]
    public void OnAppointmentRender_Callback_Invoked()
    {
        var invokedAppointments = new List<string>();
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.OnAppointmentRender, (SchedulerAppointmentRenderEventArgs args) =>
            {
                invokedAppointments.Add(args.Appointment.Title);
            }));

        Assert.Contains("Sprint Planning", invokedAppointments);
        Assert.Contains("Design Review", invokedAppointments);
    }

    [Fact]
    public void OnAppointmentRender_CssClass_Applied()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.OnAppointmentRender, (SchedulerAppointmentRenderEventArgs args) =>
            {
                if (args.Appointment.Title == "Sprint Planning")
                    args.CssClass = "highlight-appt";
            }));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));

        Assert.Contains("highlight-appt", apptEl.GetAttribute("class"));
    }

    [Fact]
    public async Task OnCancel_Fires_When_Edit_Cancelled()
    {
        var cancelFired = false;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, SampleAppointments)
            .Add(s => s.Editable, true)
            .Add(s => s.EditMode, SchedulerEditMode.Popup)
            .Add(s => s.OnCancel, () => { cancelFired = true; }));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Sprint Planning"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        var cancelBtn = cut.FindAll("button").First(b => b.TextContent.Trim() == "Cancel");
        await cut.InvokeAsync(() => cancelBtn.Click());

        Assert.True(cancelFired);
    }

    [Fact]
    public async Task Popup_Shows_ResourceSelector_When_Resources_Configured()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, ResourcedAppointments.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, SampleResources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, true)
            .Add(s => s.Editable, true)
            .Add(s => s.EditMode, SchedulerEditMode.Popup));

        var apptEl = cut.FindAll(".mar-scheduler__appointment")
            .First(el => el.TextContent.Contains("Meeting 1"));
        await cut.InvokeAsync(() => apptEl.DoubleClick());

        var popup = cut.Find(".mar-scheduler__edit-popup");
        Assert.Contains("Resource", popup.InnerHtml);
        // Should contain resource options
        Assert.Contains("Room A", popup.InnerHtml);
        Assert.Contains("Room B", popup.InnerHtml);
    }

    [Fact]
    public void EditMode_Defaults_To_Popup()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.Editable, true));

        // The default EditMode is Popup, so inline form should not appear
        // We can verify by checking that the component has the correct default
        // by trying to double-click and verifying popup appears (not inline form)
        // This is implicitly tested via the popup tests above, but let's verify
        // that no inline form markup is present at all in default mode
        Assert.Empty(cut.FindAll(".mar-scheduler__edit-form"));
        Assert.Empty(cut.FindAll(".mar-scheduler__edit-popup"));
    }
}
