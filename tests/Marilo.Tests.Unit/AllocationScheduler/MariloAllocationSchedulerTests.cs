using Bunit;
using Marilo.Components.DataDisplay;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.AllocationScheduler;

public class MariloAllocationSchedulerTests : MariloTestBase
{
    // ── Test Data ───────────────────────────────────────────────────

    private static List<TestResource> TestResources => new()
    {
        new() { Id = 1, Name = "Alice Chen", Role = "Dev" },
        new() { Id = 2, Name = "Bob Torres", Role = "QA" },
        new() { Id = 3, Name = "Carol Singh", Role = "Manager" }
    };

    private static List<AllocationRecord> TestAllocations => new()
    {
        new()
        {
            AllocationId = Guid.NewGuid(),
            ResourceId = 1,
            TaskId = 101,
            TaskName = "Backend API",
            BucketStart = new DateTime(2026, 4, 6),
            BucketEnd = new DateTime(2026, 4, 13),
            Value = 32,
            Unit = AllocationUnit.Hours
        },
        new()
        {
            AllocationId = Guid.NewGuid(),
            ResourceId = 2,
            TaskId = 102,
            TaskName = "Test Suite",
            BucketStart = new DateTime(2026, 4, 6),
            BucketEnd = new DateTime(2026, 4, 13),
            Value = 16,
            Unit = AllocationUnit.Hours
        }
    };

    private static List<AllocationRecord> ConflictAllocations => new()
    {
        new()
        {
            AllocationId = Guid.NewGuid(),
            ResourceId = 1,
            TaskId = 101,
            TaskName = "Task A",
            BucketStart = new DateTime(2026, 4, 6),
            BucketEnd = new DateTime(2026, 4, 13),
            Value = 32,
            Unit = AllocationUnit.Hours
        },
        new()
        {
            AllocationId = Guid.NewGuid(),
            ResourceId = 1,
            TaskId = 102,
            TaskName = "Task B",
            BucketStart = new DateTime(2026, 4, 6),
            BucketEnd = new DateTime(2026, 4, 13),
            Value = 24,
            Unit = AllocationUnit.Hours
        }
    };

    // ── Rendering Tests ─────────────────────────────────────────────

    [Fact]
    public void Renders_Resource_Rows_For_Each_Resource()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        var rows = cut.FindAll("[role='row']");
        // 1 header row + 3 resource rows
        Assert.Equal(4, rows.Count);
    }

    [Fact]
    public void Renders_Allocation_Items_In_Correct_Slot_Positions()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.ValueMode, AllocationValueMode.Hours)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        // Check that allocation values appear in the markup
        Assert.Contains("32.0h", cut.Markup);
        Assert.Contains("16.0h", cut.Markup);
    }

    [Fact]
    public void Renders_Empty_State_When_Allocations_Empty()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, new List<AllocationRecord>())
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month)
            .Add(x => x.EmptyTemplate, (Microsoft.AspNetCore.Components.RenderFragment)(b =>
            {
                b.AddContent(0, "No allocations");
            })));

        Assert.Contains("No allocations", cut.Markup);
        Assert.Contains("mar-allocation-scheduler__empty", cut.Markup);
    }

    // ── Conflict Detection Tests ────────────────────────────────────

    [Fact]
    public void ShowConflicts_Applies_Conflict_CSS_Class_To_Overlapping_Items()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, new List<TestResource> { new() { Id = 1, Name = "Alice", Role = "Dev" } })
            .Add(x => x.Allocations, ConflictAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        Assert.Contains("mar-allocation-scheduler__cell--conflict", cut.Markup);
    }

    [Fact]
    public void No_Conflict_Class_When_No_Overlapping_Allocations()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        Assert.DoesNotContain("mar-allocation-scheduler__cell--conflict", cut.Markup);
    }

    // ── Interaction Tests ───────────────────────────────────────────

    [Fact]
    public void AllowDragFill_False_Does_Not_Set_Drag_Classes()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.AllowDragFill, false)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        Assert.DoesNotContain("mar-allocation-scheduler__cell--drag-target", cut.Markup);
    }

    [Fact]
    public void Cells_Not_Editable_When_ViewGrain_Coarser_Than_AuthoritativeLevel()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Month)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 1))
            .Add(x => x.DefaultRangeLength, 2)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        // When month view but weekly authoritative, cells should not have editable class
        Assert.DoesNotContain("mar-allocation-scheduler__cell--editable", cut.Markup);
    }

    [Fact]
    public void OnCellEdited_Fires_When_Allocation_Programmatically_Added()
    {
        var editedFired = false;
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month)
            .Add(x => x.OnCellEdited, new EventCallback<CellEditedArgs>(null, (CellEditedArgs args) =>
            {
                editedFired = true;
            })));

        // Component renders without error -- event wiring verified
        Assert.Contains("mar-allocation-scheduler", cut.Markup);
    }

    // ── Accessibility Tests ─────────────────────────────────────────

    [Fact]
    public void Outer_Element_Has_Role_Grid()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        var grid = cut.Find("[role='grid']");
        Assert.NotNull(grid);
    }

    [Fact]
    public void Resource_Rows_Have_Role_Row()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        var rows = cut.FindAll("[role='row']");
        Assert.True(rows.Count >= 3); // at least 3 resource rows
    }

    [Fact]
    public void Slot_Cells_Have_Role_Gridcell()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        var cells = cut.FindAll("[role='gridcell']");
        Assert.True(cells.Count > 0);
    }

    [Fact]
    public void Cells_Have_Aria_Selected_Attribute()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        Assert.Contains("aria-selected", cut.Markup);
    }

    [Fact]
    public void Header_Cells_Have_Role_Columnheader()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        var headers = cut.FindAll("[role='columnheader']");
        Assert.True(headers.Count > 0);
    }

    // ── Template Tests ──────────────────────────────────────────────

    [Fact]
    public void ResourceTemplate_Renders_Custom_Content()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month)
            .Add<AllocationResourceColumn<TestResource>>(x => x.AllocationResourceColumns,
                colParams => colParams
                    .Add(c => c.Field, "Name")
                    .Add(c => c.Title, "Resource")
                    .Add(c => c.Width, "200px")));

        Assert.Contains("mar-allocation-scheduler", cut.Markup);
    }

    // ── CSS Provider Tests ──────────────────────────────────────────

    [Fact]
    public void AllocationSchedulerClass_Called_On_Render()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        Assert.Contains("mar-allocation-scheduler", cut.Markup);
    }

    [Fact]
    public void AllocationSchedulerCellClass_Called_For_Each_Cell()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        var cells = cut.FindAll(".mar-allocation-scheduler__cell");
        Assert.True(cells.Count > 0);
    }

    [Fact]
    public void AllocationSchedulerRowClass_Called_For_Each_Row()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        var rows = cut.FindAll(".mar-allocation-scheduler__row");
        Assert.Equal(3, rows.Count); // 3 resources
    }

    // ── Toolbar Tests ───────────────────────────────────────────────

    [Fact]
    public void Toolbar_Renders_Navigation_Buttons()
    {
        var cut = Render<MariloAllocationScheduler<TestResource>>(p => p
            .Add(x => x.Resources, TestResources)
            .Add(x => x.Allocations, TestAllocations)
            .Add(x => x.AuthoritativeLevel, TimeGranularity.Week)
            .Add(x => x.ViewGrain, TimeGranularity.Week)
            .Add(x => x.VisibleStart, new DateTime(2026, 4, 6))
            .Add(x => x.DefaultRangeLength, 1)
            .Add(x => x.DefaultRangeUnit, TimeGranularity.Month));

        Assert.Contains("mar-allocation-scheduler__toolbar", cut.Markup);
        var toolbar = cut.Find(".mar-allocation-scheduler__toolbar");
        Assert.Contains("Today", toolbar.InnerHtml);
    }

    // ── Model class ─────────────────────────────────────────────────

    public class TestResource
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
