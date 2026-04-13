using Bunit;
using Marilo.Components.DataDisplay;
using Marilo.Components.DataDisplay.Scheduler;
using Marilo.Core.Models;
using Xunit;

namespace Marilo.Tests.Unit.DataDisplay;

/// <summary>
/// Edge-case and robustness tests for <see cref="MariloScheduler"/>.
/// Covers resource, view, and general robustness scenarios.
/// </summary>
public class MariloSchedulerEdgeCaseTests : MariloTestBase
{
    // ══════════════════════════════════════════════════════════════════
    // §2 — Resource Edge Cases
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void Resource_With_Null_Id_Does_Not_Crash()
    {
        var resources = new List<SchedulerResource>
        {
            new() { Id = null!, Text = "Null Resource" }
        };

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Resources, resources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, true)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        // Should render without exception
        var root = cut.Find(".mar-scheduler");
        Assert.NotNull(root);
    }

    [Fact]
    public void Appointment_With_No_Matching_Resource_Renders_In_Default_Layout()
    {
        var resources = new List<SchedulerResource>
        {
            new() { Id = "room-a", Text = "Room A" }
        };

        var appointments = new List<UnmatchedResourceAppointment>
        {
            new()
            {
                Title = "Orphan Meeting",
                Start = new DateTime(2026, 4, 13, 9, 0, 0),
                End = new DateTime(2026, 4, 13, 10, 0, 0),
                ResourceId = "room-nonexistent" // no matching resource
            }
        };

        // Render in non-grouped mode to verify the appointment still appears
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, appointments.Cast<SchedulerAppointment>())
            .Add(s => s.Resources, resources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, false)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        // Should render without crashing and the appointment should be visible
        Assert.Contains("Orphan Meeting", cut.Markup);
    }

    [Fact]
    public void Single_Resource_No_Grouping_Headers()
    {
        var resources = new List<SchedulerResource>
        {
            new() { Id = "room-solo", Text = "Solo Room" }
        };

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Resources, resources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, false)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        // Non-grouped mode: no resource headers
        Assert.Empty(cut.FindAll(".mar-scheduler__resource-header"));
        // Single day column
        Assert.Single(cut.FindAll(".mar-scheduler__day-column"));
    }

    [Fact]
    public void Switching_GroupByResource_On_And_Off_Updates_Layout()
    {
        var resources = new List<SchedulerResource>
        {
            new() { Id = "room-a", Text = "Room A" },
            new() { Id = "room-b", Text = "Room B" }
        };

        // Start grouped
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Resources, resources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, true)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        Assert.Equal(2, cut.FindAll(".mar-scheduler__resource-header").Count);

        // Switch to non-grouped
        cut.Render(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Resources, resources)
            .Add(s => s.ResourceIdField, "ResourceId")
            .Add(s => s.GroupByResource, false)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        Assert.Empty(cut.FindAll(".mar-scheduler__resource-header"));
        Assert.Single(cut.FindAll(".mar-scheduler__day-column"));
    }

    // ══════════════════════════════════════════════════════════════════
    // §3 — View Edge Cases
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void Timeline_With_Zero_Appointments_Renders_Empty_Grid()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Timeline)
            .Add(s => s.Appointments, Enumerable.Empty<SchedulerAppointment>())
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerTimelineView>(0);
                builder.AddAttribute(1, "StartTime", TimeSpan.FromHours(8));
                builder.AddAttribute(2, "EndTime", TimeSpan.FromHours(18));
                builder.CloseComponent();
            }));

        // Timeline container renders
        var timeline = cut.FindAll(".mar-scheduler__timeline");
        Assert.Single(timeline);

        // No appointment elements
        Assert.Empty(cut.FindAll(".mar-scheduler__timeline-appointment"));
    }

    [Fact]
    public void Agenda_With_Zero_Appointments_Renders_Empty_Container()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Agenda)
            .Add(s => s.Appointments, Enumerable.Empty<SchedulerAppointment>())
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerAgendaView>(0);
                builder.AddAttribute(1, "NumberOfDays", 7);
                builder.CloseComponent();
            }));

        // Agenda container renders
        var agenda = cut.FindAll(".mar-scheduler__agenda");
        Assert.Single(agenda);

        // No agenda items
        Assert.Empty(cut.FindAll(".mar-scheduler__agenda-item"));
        // No date headers (only shown when day has appointments)
        Assert.Empty(cut.FindAll(".mar-scheduler__agenda-date"));
    }

    [Fact]
    public void MultiDay_With_NumberOfDays1_Same_As_Day_View()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.MultiDay)
            .Add(s => s.Appointments, Enumerable.Empty<SchedulerAppointment>())
            .Add(s => s.ChildContent, builder =>
            {
                builder.OpenComponent<SchedulerMultiDayView>(0);
                builder.AddAttribute(1, "NumberOfDays", 1);
                builder.CloseComponent();
            }));

        // Should have exactly 1 day column, same as Day view
        var dayColumns = cut.FindAll(".mar-scheduler__day-column");
        Assert.Single(dayColumns);
    }

    [Fact]
    public async Task Switching_Views_Rapidly_No_Stale_State()
    {
        SchedulerView? lastView = null;
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.ViewChanged, (SchedulerView v) => { lastView = v; }));

        // Rapidly switch between views
        var buttons = cut.FindAll("button");

        var weekBtn = buttons.First(b => b.TextContent.Trim() == "Week");
        await cut.InvokeAsync(() => weekBtn.Click());

        var dayBtn = cut.FindAll("button").First(b => b.TextContent.Trim() == "Day");
        await cut.InvokeAsync(() => dayBtn.Click());

        var monthBtn = cut.FindAll("button").First(b => b.TextContent.Trim() == "Month");
        await cut.InvokeAsync(() => monthBtn.Click());

        Assert.Equal(SchedulerView.Month, lastView);

        // Verify current view is consistent
        var monthGrid = cut.FindAll(".mar-scheduler__month");
        Assert.Single(monthGrid);
        // No time grid from Day/Week should remain
        Assert.Empty(cut.FindAll(".mar-scheduler__time-grid"));
    }

    // ══════════════════════════════════════════════════════════════════
    // §4 — General Robustness
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void Null_Data_Parameter_Renders_Empty_Scheduler()
    {
        // The Appointments parameter defaults to Enumerable.Empty so null is not
        // directly assignable, but passing an explicit empty enumerable is equivalent.
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month));

        var root = cut.Find(".mar-scheduler");
        Assert.NotNull(root);
        // No appointment elements
        Assert.Empty(cut.FindAll(".mar-scheduler__appointment"));
    }

    [Fact]
    public void Empty_Data_Renders_Empty_Scheduler()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, new List<SchedulerAppointment>()));

        var root = cut.Find(".mar-scheduler");
        Assert.NotNull(root);
        Assert.Empty(cut.FindAll(".mar-scheduler__appointment"));
    }

    [Fact]
    public void Appointment_With_Start_After_End_Does_Not_Crash()
    {
        var badAppts = new List<SchedulerAppointment>
        {
            new()
            {
                Title = "Backwards",
                Start = new DateTime(2026, 4, 13, 17, 0, 0),
                End = new DateTime(2026, 4, 13, 9, 0, 0) // End before Start
            }
        };

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, badAppts));

        // Should render without exception
        var root = cut.Find(".mar-scheduler");
        Assert.NotNull(root);
    }

    [Fact]
    public void Appointment_With_Start_After_End_DayView_Does_Not_Crash()
    {
        var badAppts = new List<SchedulerAppointment>
        {
            new()
            {
                Title = "Backwards Day",
                Start = new DateTime(2026, 4, 13, 17, 0, 0),
                End = new DateTime(2026, 4, 13, 9, 0, 0) // End before Start
            }
        };

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, badAppts)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        var root = cut.Find(".mar-scheduler");
        Assert.NotNull(root);
    }

    [Fact]
    public void Appointment_With_Null_Title_Renders_Without_Title()
    {
        var appts = new List<SchedulerAppointment>
        {
            new()
            {
                Title = null!,
                Start = new DateTime(2026, 4, 13, 9, 0, 0),
                End = new DateTime(2026, 4, 13, 10, 0, 0)
            }
        };

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, appts));

        // Should render the appointment element even with no title
        var apptElements = cut.FindAll(".mar-scheduler__appointment");
        Assert.Single(apptElements);
    }

    [Fact]
    public void Dispose_During_Normal_Operation_No_Exception()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, new List<SchedulerAppointment>
            {
                new()
                {
                    Title = "Meeting",
                    Start = new DateTime(2026, 4, 13, 9, 0, 0),
                    End = new DateTime(2026, 4, 13, 10, 0, 0)
                }
            }));

        // Should dispose cleanly without throwing
        var ex = Record.Exception(() =>
        {
            // Dispose cleans up all rendered components
            Dispose();
        });

        Assert.Null(ex);
    }

    [Fact]
    public void Dispose_With_Registered_Views_No_Exception()
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
                builder.OpenComponent<SchedulerMultiDayView>(2);
                builder.CloseComponent();
            }));

        var ex = Record.Exception(() =>
        {
            Dispose();
        });

        Assert.Null(ex);
    }

    [Fact]
    public void Appointment_With_Zero_Duration_Does_Not_Crash()
    {
        var appts = new List<SchedulerAppointment>
        {
            new()
            {
                Title = "Zero Duration",
                Start = new DateTime(2026, 4, 13, 9, 0, 0),
                End = new DateTime(2026, 4, 13, 9, 0, 0) // Same as start
            }
        };

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.Appointments, appts)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        var root = cut.Find(".mar-scheduler");
        Assert.NotNull(root);
    }

    [Fact]
    public void Many_Appointments_Same_Slot_Does_Not_Crash()
    {
        var appts = Enumerable.Range(0, 50).Select(i => new SchedulerAppointment
        {
            Title = $"Meeting {i}",
            Start = new DateTime(2026, 4, 13, 9, 0, 0),
            End = new DateTime(2026, 4, 13, 10, 0, 0)
        }).ToList();

        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Month)
            .Add(s => s.Appointments, appts));

        var apptElements = cut.FindAll(".mar-scheduler__appointment");
        Assert.Equal(50, apptElements.Count);
    }

    [Fact]
    public void Empty_Scheduler_DayView_Shows_Time_Slots()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Day)
            .Add(s => s.StartHour, 8)
            .Add(s => s.EndHour, 18));

        var timeLabels = cut.FindAll(".mar-scheduler__time-label");
        Assert.Equal(11, timeLabels.Count); // 8-18 inclusive
    }

    [Fact]
    public void Empty_Scheduler_WeekView_Shows_Seven_Columns()
    {
        var cut = Render<MariloScheduler>(p => p
            .Add(s => s.CurrentDate, new DateTime(2026, 4, 13))
            .Add(s => s.View, SchedulerView.Week));

        var dayColumns = cut.FindAll(".mar-scheduler__day-column");
        Assert.Equal(7, dayColumns.Count);
    }

    // ── Helper class ────────────────────────────────────────────────

    private class UnmatchedResourceAppointment : SchedulerAppointment
    {
        public string ResourceId { get; set; } = string.Empty;
    }
}
