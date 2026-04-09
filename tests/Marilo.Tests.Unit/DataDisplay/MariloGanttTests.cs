using Bunit;
using Marilo.Components.DataDisplay;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.DataDisplay;

public class MariloGanttTests : MariloTestBase
{
    private record TaskModel
    {
        public int Id { get; init; }
        public int? ParentId { get; init; }
        public string Title { get; init; } = "";
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public double PercentComplete { get; init; }
        public List<int>? DependsOn { get; init; }
    }

    private static readonly DateTime BaseDate = new(2026, 4, 1);

    private static List<TaskModel> CreateTestData() =>
    [
        // Parent 1
        new() { Id = 1, ParentId = null, Title = "Alpha", Start = BaseDate, End = BaseDate.AddDays(10), PercentComplete = 50 },
        new() { Id = 2, ParentId = 1, Title = "Alpha-1", Start = BaseDate, End = BaseDate.AddDays(4), PercentComplete = 80 },
        new() { Id = 3, ParentId = 1, Title = "Alpha-2", Start = BaseDate.AddDays(4), End = BaseDate.AddDays(10), PercentComplete = 20 },
        // Parent 2
        new() { Id = 4, ParentId = null, Title = "Beta", Start = BaseDate.AddDays(2), End = BaseDate.AddDays(12), PercentComplete = 30 },
        new() { Id = 5, ParentId = 4, Title = "Beta-1", Start = BaseDate.AddDays(2), End = BaseDate.AddDays(7), PercentComplete = 60 },
        new() { Id = 6, ParentId = 4, Title = "Beta-2", Start = BaseDate.AddDays(7), End = BaseDate.AddDays(12), PercentComplete = 0 },
        // Parent 3
        new() { Id = 7, ParentId = null, Title = "Gamma", Start = BaseDate.AddDays(5), End = BaseDate.AddDays(15), PercentComplete = 10 },
        new() { Id = 8, ParentId = 7, Title = "Gamma-1", Start = BaseDate.AddDays(5), End = BaseDate.AddDays(10), PercentComplete = 25 },
        new() { Id = 9, ParentId = 7, Title = "Gamma-2", Start = BaseDate.AddDays(10), End = BaseDate.AddDays(15), PercentComplete = 0 },
    ];

    /// <summary>Builds a default 3-column RenderFragment (Title expandable, Start, End with date format).</summary>
    private static RenderFragment DefaultColumns() => builder =>
    {
        builder.OpenComponent<GanttColumn<TaskModel>>(0);
        builder.AddAttribute(1, nameof(GanttColumn<TaskModel>.Field), "Title");
        builder.AddAttribute(2, nameof(GanttColumn<TaskModel>.Expandable), true);
        builder.CloseComponent();

        builder.OpenComponent<GanttColumn<TaskModel>>(3);
        builder.AddAttribute(4, nameof(GanttColumn<TaskModel>.Field), "Start");
        builder.AddAttribute(5, nameof(GanttColumn<TaskModel>.DisplayFormat), "{0:d}");
        builder.CloseComponent();

        builder.OpenComponent<GanttColumn<TaskModel>>(6);
        builder.AddAttribute(7, nameof(GanttColumn<TaskModel>.Field), "End");
        builder.AddAttribute(8, nameof(GanttColumn<TaskModel>.DisplayFormat), "{0:d}");
        builder.CloseComponent();
    };

    private static RenderFragment ViewsFragment(params GanttView[] views) => builder =>
    {
        // Use distinct literal sequence numbers per view type
        foreach (var v in views)
        {
            switch (v)
            {
                case GanttView.Week:
                    builder.OpenComponent<GanttWeekView>(10);
                    builder.CloseComponent();
                    break;
                case GanttView.Month:
                    builder.OpenComponent<GanttMonthView>(20);
                    builder.CloseComponent();
                    break;
                case GanttView.Day:
                    builder.OpenComponent<GanttDayView>(30);
                    builder.CloseComponent();
                    break;
                case GanttView.Year:
                    builder.OpenComponent<GanttYearView>(40);
                    builder.CloseComponent();
                    break;
            }
        }
    };

    // ── Rendering basics ──────────────────────────────────────────────

    [Fact]
    public void Gantt_Renders_Container_With_Class()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData()));

        var container = cut.Find(".mar-gantt");
        Assert.NotNull(container);
    }

    [Fact]
    public void Gantt_Renders_Correct_Number_Of_Visible_Rows()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var rows = cut.FindAll(".mar-gantt__task-row");
        Assert.Equal(9, rows.Count);
    }

    [Fact]
    public void Gantt_Renders_Width_And_Height()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.Width, "900px")
            .Add(x => x.Height, "600px"));

        var container = cut.Find(".mar-gantt");
        var style = container.GetAttribute("style") ?? "";
        Assert.Contains("width:900px", style);
        Assert.Contains("height:600px", style);
    }

    [Fact]
    public void Gantt_Renders_TaskList_And_Timeline_Panes()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData()));

        Assert.NotNull(cut.Find(".mar-gantt__tasklist"));
        Assert.NotNull(cut.Find(".mar-gantt__timeline"));
    }

    [Fact]
    public void Gantt_Without_Data_Renders_Empty()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, new List<TaskModel>())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var rows = cut.FindAll(".mar-gantt__task-row");
        Assert.Empty(rows);
    }

    // ── Hierarchy ─────────────────────────────────────────────────────

    [Fact]
    public void Tree_Shows_Correct_Depth_Indentation()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var rows = cut.FindAll(".mar-gantt__task-row");
        // First row is parent (depth 0), second is child (depth 1)
        var parentSpan = rows[0].QuerySelector(".mar-gantt__task-cell span[style]");
        var childSpan = rows[1].QuerySelector(".mar-gantt__task-cell span[style]");
        Assert.NotNull(parentSpan);
        Assert.NotNull(childSpan);

        // Parent at depth 0 with children: pad = 0*16 + 0 = 0px
        // Child at depth 1 with no children: pad = 1*16 + 16 = 32px
        var parentStyle = parentSpan!.GetAttribute("style") ?? "";
        var childStyle = childSpan!.GetAttribute("style") ?? "";
        Assert.Contains("padding-left: 0px", parentStyle);
        Assert.Contains("padding-left: 32px", childStyle);
    }

    [Fact]
    public void Collapse_Hides_Children()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        Assert.Equal(9, cut.FindAll(".mar-gantt__task-row").Count);

        // Click the first parent's chevron to collapse
        var chevron = cut.Find(".mar-gantt__chevron");
        chevron.Click();

        // Alpha's 2 children should be hidden -> 7 rows
        Assert.Equal(7, cut.FindAll(".mar-gantt__task-row").Count);
    }

    [Fact]
    public void Expand_Shows_Children()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        // Collapse first
        var chevron = cut.Find(".mar-gantt__chevron");
        chevron.Click();
        Assert.Equal(7, cut.FindAll(".mar-gantt__task-row").Count);

        // Expand again
        chevron = cut.Find(".mar-gantt__chevron");
        chevron.Click();
        Assert.Equal(9, cut.FindAll(".mar-gantt__task-row").Count);
    }

    [Fact]
    public void Orphan_Items_Render_As_Roots()
    {
        var data = new List<TaskModel>
        {
            new() { Id = 1, ParentId = null, Title = "Root", Start = BaseDate, End = BaseDate.AddDays(5) },
            new() { Id = 2, ParentId = 999, Title = "Orphan", Start = BaseDate, End = BaseDate.AddDays(3) },
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, DefaultColumns()));

        var rows = cut.FindAll(".mar-gantt__task-row");
        Assert.Equal(2, rows.Count);

        // Both should be at aria-level 1 (depth 0)
        foreach (var row in rows)
        {
            Assert.Equal("1", row.GetAttribute("aria-level"));
        }
    }

    // ── Columns ───────────────────────────────────────────────────────

    [Fact]
    public void Columns_Render_Header_Cells()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var headers = cut.FindAll(".mar-gantt__header-cell");
        Assert.Equal(3, headers.Count);
        Assert.Contains("Title", headers[0].TextContent);
        Assert.Contains("Start", headers[1].TextContent);
        Assert.Contains("End", headers[2].TextContent);
    }

    [Fact]
    public void Column_Visible_False_Hides_Column()
    {
        RenderFragment cols = builder =>
        {
            builder.OpenComponent<GanttColumn<TaskModel>>(0);
            builder.AddAttribute(1, nameof(GanttColumn<TaskModel>.Field), "Title");
            builder.AddAttribute(2, nameof(GanttColumn<TaskModel>.Expandable), true);
            builder.CloseComponent();

            builder.OpenComponent<GanttColumn<TaskModel>>(3);
            builder.AddAttribute(4, nameof(GanttColumn<TaskModel>.Field), "Start");
            builder.AddAttribute(5, nameof(GanttColumn<TaskModel>.Visible), false);
            builder.CloseComponent();
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, cols));

        var headers = cut.FindAll(".mar-gantt__header-cell");
        Assert.Single(headers);
        Assert.Contains("Title", headers[0].TextContent);
    }

    [Fact]
    public void Expandable_Column_Shows_Chevron()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        // 3 parent tasks = 3 chevrons
        var chevrons = cut.FindAll(".mar-gantt__chevron");
        Assert.Equal(3, chevrons.Count);
    }

    [Fact]
    public void Column_DisplayFormat_Applied()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        // Each task row has 3 cells. Second cell is Start. Use cells within task rows to skip filter row.
        var firstRow = cut.Find(".mar-gantt__task-row");
        var cells = firstRow.QuerySelectorAll(".mar-gantt__task-cell");
        var startCell = cells[1]; // second cell = Start column
        var formatted = BaseDate.ToString("d");
        Assert.Contains(formatted, startCell.TextContent);
    }

    // ── Views ─────────────────────────────────────────────────────────

    [Fact]
    public void Default_View_Is_Week()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData()));

        Assert.Equal(GanttView.Week, cut.Instance.View);
    }

    [Fact]
    public void View_Selector_Renders_For_Multiple_Views()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week, GanttView.Month)));

        var viewBtns = cut.FindAll(".mar-gantt__view-btn");
        Assert.Equal(2, viewBtns.Count);
    }

    [Fact]
    public void Click_View_Button_Switches_View()
    {
        GanttView? changedView = null;
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week, GanttView.Month))
            .Add(x => x.View, GanttView.Week)
            .Add(x => x.ViewChanged, (GanttView v) => changedView = v));

        var viewBtns = cut.FindAll(".mar-gantt__view-btn");
        // Click the Month button (second)
        viewBtns[1].Click();

        Assert.Equal(GanttView.Month, changedView);
    }

    // ── Sorting ───────────────────────────────────────────────────────

    [Fact]
    public void Click_Header_Sorts_Ascending()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        // Click the Title header (sortable by default)
        var header = cut.Find(".mar-gantt__header-cell");
        header.Click();

        // After ascending sort: Alpha is still first (already alphabetically first among roots)
        var rows = cut.FindAll(".mar-gantt__task-row");
        Assert.Contains("Alpha", rows[0].TextContent);
    }

    [Fact]
    public void Double_Click_Header_Sorts_Descending()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var header = cut.Find(".mar-gantt__header-cell");
        header.Click(); // ascending
        header.Click(); // descending

        // After descending sort: Gamma first among roots
        var rows = cut.FindAll(".mar-gantt__task-row");
        Assert.Contains("Gamma", rows[0].TextContent);
    }

    [Fact]
    public void Triple_Click_Clears_Sort()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var header = cut.Find(".mar-gantt__header-cell");
        header.Click(); // ascending
        header.Click(); // descending
        header.Click(); // clear

        // Back to original order: Alpha first
        var rows = cut.FindAll(".mar-gantt__task-row");
        Assert.Contains("Alpha", rows[0].TextContent);
    }

    // ── Filtering ─────────────────────────────────────────────────────

    [Fact]
    public void Filter_Input_Filters_Rows()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        // Filter by "Beta" in the Title column filter input
        var filterInput = cut.Find(".mar-gantt__filter-input");
        filterInput.Input("Beta");

        var rows = cut.FindAll(".mar-gantt__task-row");
        // Beta (parent) + Beta-1 + Beta-2 = 3 visible
        Assert.Equal(3, rows.Count);
    }

    [Fact]
    public void Filter_Preserves_Parent_Context()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        // Filter for "Alpha-1" — should show Alpha (parent) + Alpha-1 (match)
        var filterInput = cut.Find(".mar-gantt__filter-input");
        filterInput.Input("Alpha-1");

        var rows = cut.FindAll(".mar-gantt__task-row");
        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public void Clear_Filter_Restores_All_Rows()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var filterInput = cut.Find(".mar-gantt__filter-input");
        filterInput.Input("Beta");
        Assert.Equal(3, cut.FindAll(".mar-gantt__task-row").Count);

        // Clear filter
        filterInput.Input("");
        Assert.Equal(9, cut.FindAll(".mar-gantt__task-row").Count);
    }

    // ── Events ────────────────────────────────────────────────────────

    [Fact]
    public void OnTaskClick_Fires_On_Row_Click()
    {
        TaskModel? clicked = null;
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.OnTaskClick, (TaskModel t) => clicked = t));

        var rows = cut.FindAll(".mar-gantt__task-row");
        rows[3].Click(); // Click Beta (4th row, index 3)

        Assert.NotNull(clicked);
        Assert.Equal("Beta", clicked!.Title);
    }

    [Fact]
    public void OnExpand_Fires_When_Expanding()
    {
        object? expandedItem = null;
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.OnExpand, (GanttExpandEventArgs args) => expandedItem = args.Item));

        // Collapse first, then expand
        var chevron = cut.Find(".mar-gantt__chevron");
        chevron.Click(); // collapse Alpha
        chevron = cut.Find(".mar-gantt__chevron");
        chevron.Click(); // expand Alpha

        Assert.NotNull(expandedItem);
    }

    [Fact]
    public void OnCollapse_Fires_When_Collapsing()
    {
        object? collapsedItem = null;
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.OnCollapse, (GanttCollapseEventArgs args) => collapsedItem = args.Item));

        // Click first parent's chevron to collapse
        var chevron = cut.Find(".mar-gantt__chevron");
        chevron.Click();

        Assert.NotNull(collapsedItem);
    }

    // ── Templates ─────────────────────────────────────────────────────

    [Fact]
    public void TaskTemplate_Renders_Custom_Content()
    {
        RenderFragment<TaskModel> taskTemplate = item => builder =>
        {
            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "class", "custom-bar");
            builder.AddContent(2, $"CUSTOM:{item.Title}");
            builder.CloseElement();
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.TaskTemplate, taskTemplate)
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week)));

        var customBars = cut.FindAll(".custom-bar");
        Assert.True(customBars.Count > 0);
        Assert.Contains("CUSTOM:Alpha", customBars[0].TextContent);
    }

    [Fact]
    public void Column_Template_Renders_Custom_Cell()
    {
        RenderFragment<TaskModel> cellTemplate = item => builder =>
        {
            builder.OpenElement(0, "strong");
            builder.AddAttribute(1, "class", "custom-cell");
            builder.AddContent(2, $"[{item.Title}]");
            builder.CloseElement();
        };

        RenderFragment cols = builder =>
        {
            builder.OpenComponent<GanttColumn<TaskModel>>(0);
            builder.AddAttribute(1, nameof(GanttColumn<TaskModel>.Field), "Title");
            builder.AddAttribute(2, nameof(GanttColumn<TaskModel>.Expandable), true);
            builder.AddAttribute(3, nameof(GanttColumn<TaskModel>.Template), cellTemplate);
            builder.CloseComponent();
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, cols));

        var customCells = cut.FindAll(".custom-cell");
        Assert.True(customCells.Count > 0);
        Assert.Contains("[Alpha]", customCells[0].TextContent);
    }

    // ── Toolbar ───────────────────────────────────────────────────────

    [Fact]
    public void Toolbar_Template_Renders()
    {
        RenderFragment toolbarContent = builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "class", "custom-toolbar-btn");
            builder.AddContent(2, "Export");
            builder.CloseElement();
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttToolBarTemplate, toolbarContent));

        var toolbar = cut.Find(".mar-gantt__toolbar");
        Assert.NotNull(toolbar);
        var btn = cut.Find(".custom-toolbar-btn");
        Assert.Equal("Export", btn.TextContent);
    }

    // ── ARIA ──────────────────────────────────────────────────────────

    [Fact]
    public void TreeGrid_Has_Role_Attribute()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData()));

        var treegrid = cut.Find("[role='treegrid']");
        Assert.NotNull(treegrid);
    }

    [Fact]
    public void Rows_Have_AriaLevel()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var rows = cut.FindAll(".mar-gantt__task-row");

        // Parent rows (index 0,3,6) should be aria-level="1"
        Assert.Equal("1", rows[0].GetAttribute("aria-level"));
        Assert.Equal("1", rows[3].GetAttribute("aria-level"));
        Assert.Equal("1", rows[6].GetAttribute("aria-level"));

        // Child rows (index 1,2,4,5,7,8) should be aria-level="2"
        Assert.Equal("2", rows[1].GetAttribute("aria-level"));
        Assert.Equal("2", rows[2].GetAttribute("aria-level"));
        Assert.Equal("2", rows[4].GetAttribute("aria-level"));
    }

    // ── Dependencies ──────────────────────────────────────────────────

    [Fact]
    public void Dependency_Lines_Render_SVG()
    {
        var data = new List<TaskModel>
        {
            new() { Id = 1, ParentId = null, Title = "Task A", Start = BaseDate, End = BaseDate.AddDays(3) },
            new() { Id = 2, ParentId = null, Title = "Task B", Start = BaseDate.AddDays(3), End = BaseDate.AddDays(6), DependsOn = [1] },
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week)));

        var svg = cut.Find(".mar-gantt__dependency-svg");
        Assert.NotNull(svg);
        var lines = cut.FindAll(".mar-gantt__dependency-svg line");
        Assert.Single(lines);
    }
}
