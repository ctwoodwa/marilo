using Bunit;
using Marilo.Components.DataDisplay;
using Marilo.Components.Overlays;
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

    // ── MariloGanttDependencies component model ───────────────────────────

    [Fact]
    public void GanttDependencies_CanRender_AsChildComponent()
    {
        var deps = new List<GanttDependency>
        {
            new() { Id = 1, PredecessorId = 1, SuccessorId = 2, Type = GanttDependencyType.FinishToStart },
        };

        RenderFragment depsSlot = builder =>
        {
            builder.OpenComponent<MariloGanttDependencies<TaskModel>>(0);
            builder.AddAttribute(1, nameof(MariloGanttDependencies<TaskModel>.Data), (IEnumerable<GanttDependency>)deps);
            builder.CloseComponent();
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttDependenciesSlot, depsSlot));

        // Component renders without error; the gantt container is present
        Assert.NotNull(cut.Find(".mar-gantt"));
    }

    [Fact]
    public void GanttDependencies_RegistersWithParent_ViaCascadingParameter()
    {
        var deps = new List<GanttDependency>
        {
            new() { Id = 1, PredecessorId = 1, SuccessorId = 2 },
        };

        RenderFragment depsSlot = builder =>
        {
            builder.OpenComponent<MariloGanttDependencies<TaskModel>>(0);
            builder.AddAttribute(1, nameof(MariloGanttDependencies<TaskModel>.Data), (IEnumerable<GanttDependency>)deps);
            builder.CloseComponent();
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttDependenciesSlot, depsSlot));

        // Access the MariloGantt component instance and verify _dependencies was set
        var gantt = cut.Instance;
        Assert.NotNull(gantt._dependencies);
    }

    [Fact]
    public void GanttDependencies_DataParameter_BindsCorrectly()
    {
        var deps = new List<GanttDependency>
        {
            new() { Id = 10, PredecessorId = 1, SuccessorId = 2, Type = GanttDependencyType.StartToStart },
            new() { Id = 11, PredecessorId = 2, SuccessorId = 3, Type = GanttDependencyType.FinishToFinish },
        };

        RenderFragment depsSlot = builder =>
        {
            builder.OpenComponent<MariloGanttDependencies<TaskModel>>(0);
            builder.AddAttribute(1, nameof(MariloGanttDependencies<TaskModel>.Data), (IEnumerable<GanttDependency>)deps);
            builder.CloseComponent();
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttDependenciesSlot, depsSlot));

        var gantt = cut.Instance;
        Assert.NotNull(gantt._dependencies);
        Assert.Equal(2, gantt._dependencies!.Data.Count());
        Assert.Equal(GanttDependencyType.StartToStart, gantt._dependencies.Data.First().Type);
    }

    [Fact]
    public void GanttDependencies_OnCreate_EventCallback_CanBeInvoked()
    {
        GanttDependencyCreateEventArgs? receivedArgs = null;

        var newDep = new GanttDependency { Id = 99, PredecessorId = 1, SuccessorId = 2 };

        RenderFragment depsSlot = builder =>
        {
            builder.OpenComponent<MariloGanttDependencies<TaskModel>>(0);
            builder.AddAttribute(1, nameof(MariloGanttDependencies<TaskModel>.Data),
                (IEnumerable<GanttDependency>)Enumerable.Empty<GanttDependency>());
            builder.AddAttribute(2, nameof(MariloGanttDependencies<TaskModel>.OnCreate),
                EventCallback.Factory.Create<GanttDependencyCreateEventArgs>(this,
                    args => receivedArgs = args));
            builder.CloseComponent();
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttDependenciesSlot, depsSlot));

        var gantt = cut.Instance;
        Assert.NotNull(gantt._dependencies);

        var createArgs = new GanttDependencyCreateEventArgs { Dependency = newDep };
        cut.InvokeAsync(() => gantt._dependencies!.OnCreate.InvokeAsync(createArgs));

        Assert.NotNull(receivedArgs);
        Assert.Equal(99, (int)receivedArgs!.Dependency.Id);
    }

    [Fact]
    public void GanttDependencies_OnDelete_EventCallback_CanBeInvoked()
    {
        GanttDependencyDeleteEventArgs? receivedArgs = null;

        var existingDep = new GanttDependency { Id = 42, PredecessorId = 3, SuccessorId = 4 };
        var depsList = new List<GanttDependency> { existingDep };

        RenderFragment depsSlot = builder =>
        {
            builder.OpenComponent<MariloGanttDependencies<TaskModel>>(0);
            builder.AddAttribute(1, nameof(MariloGanttDependencies<TaskModel>.Data), (IEnumerable<GanttDependency>)depsList);
            builder.AddAttribute(2, nameof(MariloGanttDependencies<TaskModel>.OnDelete),
                EventCallback.Factory.Create<GanttDependencyDeleteEventArgs>(this,
                    args => receivedArgs = args));
            builder.CloseComponent();
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttDependenciesSlot, depsSlot));

        var gantt = cut.Instance;
        Assert.NotNull(gantt._dependencies);

        var deleteArgs = new GanttDependencyDeleteEventArgs { Dependency = existingDep };
        cut.InvokeAsync(() => gantt._dependencies!.OnDelete.InvokeAsync(deleteArgs));

        Assert.NotNull(receivedArgs);
        Assert.Equal(42, (int)receivedArgs!.Dependency.Id);
    }

    // ── E9: OriginalEditItem deep clone ───────────────────────────────

    private class CloneableTask : IGanttCloneable<CloneableTask>
    {
        public static int CloneCallCount;

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; } = "";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double PercentComplete { get; set; }

        public CloneableTask Clone()
        {
            CloneCallCount++;
            return new CloneableTask
            {
                Id = Id, ParentId = ParentId, Title = Title,
                Start = Start, End = End, PercentComplete = PercentComplete
            };
        }
    }

    private static RenderFragment EditableColumns() => builder =>
    {
        builder.OpenComponent<GanttColumn<TaskModel>>(0);
        builder.AddAttribute(1, nameof(GanttColumn<TaskModel>.Field), "Title");
        builder.AddAttribute(2, nameof(GanttColumn<TaskModel>.Editable), true);
        builder.CloseComponent();

        builder.OpenComponent<GanttColumn<TaskModel>>(3);
        builder.AddAttribute(4, nameof(GanttColumn<TaskModel>.Field), "Start");
        builder.AddAttribute(5, nameof(GanttColumn<TaskModel>.Editable), true);
        builder.CloseComponent();

        builder.OpenComponent<GanttColumn<TaskModel>>(6);
        builder.AddAttribute(7, nameof(GanttColumn<TaskModel>.Field), "End");
        builder.AddAttribute(8, nameof(GanttColumn<TaskModel>.Editable), true);
        builder.CloseComponent();
    };

    private static RenderFragment CloneableTaskColumns() => builder =>
    {
        builder.OpenComponent<GanttColumn<CloneableTask>>(0);
        builder.AddAttribute(1, nameof(GanttColumn<CloneableTask>.Field), "Title");
        builder.AddAttribute(2, nameof(GanttColumn<CloneableTask>.Editable), true);
        builder.CloseComponent();

        builder.OpenComponent<GanttColumn<CloneableTask>>(3);
        builder.AddAttribute(4, nameof(GanttColumn<CloneableTask>.Field), "Start");
        builder.AddAttribute(5, nameof(GanttColumn<CloneableTask>.Editable), true);
        builder.CloseComponent();
    };

    [Fact]
    public async Task BeginEdit_StoresNonNullOriginalEditItem()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns()));

        await cut.Instance.BeginEdit(0);

        var state = cut.Instance.GetState();
        Assert.NotNull(state.OriginalEditItem);
    }

    [Fact]
    public async Task BeginEdit_OriginalEditItem_IsDistinctCloneOfEditItem()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns()));

        await cut.Instance.BeginEdit(0);

        var state = cut.Instance.GetState();
        Assert.NotNull(state.EditItem);
        Assert.NotNull(state.OriginalEditItem);
        // Must be a distinct clone — not the same reference
        Assert.False(ReferenceEquals(state.EditItem, state.OriginalEditItem));
        // Values match
        Assert.Equal(state.EditItem!.Id, state.OriginalEditItem!.Id);
        Assert.Equal(state.EditItem.Title, state.OriginalEditItem.Title);
    }

    [Fact]
    public async Task BeginEdit_UsesIGanttCloneable_WhenImplemented()
    {
        CloneableTask.CloneCallCount = 0;

        var data = new List<CloneableTask>
        {
            new() { Id = 1, ParentId = null, Title = "Task A", Start = BaseDate, End = BaseDate.AddDays(5) },
            new() { Id = 2, ParentId = null, Title = "Task B", Start = BaseDate.AddDays(5), End = BaseDate.AddDays(10) },
        };

        var cut = Render<MariloGantt<CloneableTask>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.IdField, "Id")
            .Add(x => x.ParentIdField, "ParentId")
            .Add(x => x.TitleField, "Title")
            .Add(x => x.StartField, "Start")
            .Add(x => x.EndField, "End")
            .Add(x => x.GanttColumns, CloneableTaskColumns()));

        await cut.Instance.BeginEdit(0);

        // Clone() must have been called exactly once via the interface path
        Assert.Equal(1, CloneableTask.CloneCallCount);
        var state = cut.Instance.GetState();
        Assert.NotNull(state.OriginalEditItem);
    }

    [Fact]
    public async Task BeginEdit_JsonFallback_WhenNotIGanttCloneable()
    {
        // TaskModel is a record (no IGanttCloneable) — JSON roundtrip fallback is used.
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns()));

        await cut.Instance.BeginEdit(0);

        var state = cut.Instance.GetState();
        Assert.NotNull(state.OriginalEditItem);
        Assert.Equal(data[0].Id, state.OriginalEditItem!.Id);
        Assert.Equal(data[0].Title, state.OriginalEditItem.Title);
    }

    [Fact]
    public async Task CancelEdit_ClearsOriginalEditItem()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns()));

        await cut.Instance.BeginEdit(0);
        Assert.NotNull(cut.Instance.GetState().OriginalEditItem);

        await cut.Instance.CancelEdit();

        Assert.Null(cut.Instance.GetState().OriginalEditItem);
    }

    [Fact]
    public async Task CommitEdit_ClearsOriginalEditItem()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns()));

        await cut.Instance.BeginEdit(0);
        Assert.NotNull(cut.Instance.GetState().OriginalEditItem);

        await cut.Instance.CommitEdit();

        Assert.Null(cut.Instance.GetState().OriginalEditItem);
    }

    // ── E12: CheckboxList filter ──────────────────────────────────────

    private record StatusTaskModel
    {
        public int Id { get; init; }
        public int? ParentId { get; init; }
        public string Title { get; init; } = "";
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public double PercentComplete { get; init; }
        public string Status { get; init; } = "";
    }

    private static List<StatusTaskModel> CreateStatusData() =>
    [
        new() { Id = 1, ParentId = null, Title = "Task A", Start = BaseDate, End = BaseDate.AddDays(5), Status = "Active" },
        new() { Id = 2, ParentId = null, Title = "Task B", Start = BaseDate, End = BaseDate.AddDays(5), Status = "Active" },
        new() { Id = 3, ParentId = null, Title = "Task C", Start = BaseDate, End = BaseDate.AddDays(5), Status = "Pending" },
        new() { Id = 4, ParentId = null, Title = "Task D", Start = BaseDate, End = BaseDate.AddDays(5), Status = "Closed" },
    ];

    private static RenderFragment StatusColumns() => builder =>
    {
        builder.OpenComponent<GanttColumn<StatusTaskModel>>(0);
        builder.AddAttribute(1, nameof(GanttColumn<StatusTaskModel>.Field), "Title");
        builder.AddAttribute(2, nameof(GanttColumn<StatusTaskModel>.Expandable), true);
        builder.CloseComponent();

        builder.OpenComponent<GanttColumn<StatusTaskModel>>(3);
        builder.AddAttribute(4, nameof(GanttColumn<StatusTaskModel>.Field), "Status");
        builder.AddAttribute(5, nameof(GanttColumn<StatusTaskModel>.FilterType), GanttColumnFilterType.CheckboxList);
        builder.CloseComponent();
    };

    [Fact]
    public void CheckboxFilter_FilterButton_RendersForCheckboxListColumn()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Both filterable columns should render a filter button
        var filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        Assert.True(filterBtns.Count >= 1);
    }

    [Fact]
    public void CheckboxFilter_OpenDrawer_ShowsOptions()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Find the Status column filter button (second filterable column)
        var filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        // Status column is second; click its filter button
        filterBtns[^1].Click();

        // The checkbox filter drawer should now be visible
        var drawer = cut.Find(".mar-gantt__checkbox-filter");
        Assert.NotNull(drawer);

        // All 3 distinct Status values should appear as checkboxes
        var checkboxes = drawer.QuerySelectorAll("input[type='checkbox']");
        Assert.Equal(3, checkboxes.Length);
    }

    [Fact]
    public void CheckboxFilter_Apply_FiltersRows()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Open the Status checkbox drawer
        var filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        filterBtns[^1].Click();

        // Uncheck "Active" (the first checkbox alphabetically = "Active")
        var checkboxes = cut.FindAll(".mar-gantt__checkbox-filter-item input[type='checkbox']");
        // Active, Closed, Pending — uncheck "Active"
        checkboxes[0].Change(false);

        // Click Apply
        var applyBtn = cut.Find(".mar-gantt__checkbox-filter-actions .mar-gantt__filter-menu-btn");
        applyBtn.Click();

        // Only Pending (1) + Closed (1) = 2 rows should remain
        var rows = cut.FindAll(".mar-gantt__task-row");
        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public void CheckboxFilter_Clear_RemovesFilter()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Open and apply a partial filter first
        var filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        filterBtns[^1].Click();

        var checkboxes = cut.FindAll(".mar-gantt__checkbox-filter-item input[type='checkbox']");
        checkboxes[0].Change(false); // uncheck Active

        var applyBtn = cut.Find(".mar-gantt__checkbox-filter-actions .mar-gantt__filter-menu-btn");
        applyBtn.Click();
        Assert.Equal(2, cut.FindAll(".mar-gantt__task-row").Count);

        // Re-open drawer and click Clear
        filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        filterBtns[^1].Click();

        var actionBtns = cut.FindAll(".mar-gantt__checkbox-filter-actions .mar-gantt__filter-menu-btn");
        actionBtns[^1].Click(); // Clear button is last

        // All 4 rows restored
        Assert.Equal(4, cut.FindAll(".mar-gantt__task-row").Count);
    }

    // ── Screen reader announcements (E11) ─────────────────────────────

    [Fact]
    public void Announcer_Region_Exists_With_AriaLive_Polite()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var announcer = cut.Find(".mar-gantt__announcer");
        Assert.NotNull(announcer);
        Assert.Equal("polite", announcer.GetAttribute("aria-live"));
        Assert.Equal("true", announcer.GetAttribute("aria-atomic"));
        Assert.Equal("status", announcer.GetAttribute("role"));
    }

    [Fact]
    public async Task Expand_Node_Announces_Expanded_Message()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        // All nodes start expanded; collapse Alpha first so we can expand it
        var chevron = cut.Find(".mar-gantt__chevron");
        chevron.Click(); // collapses Alpha
        cut.Render();

        // Now expand Alpha again
        chevron = cut.Find(".mar-gantt__chevron");
        chevron.Click(); // expands Alpha

        cut.Render();
        var announcer = cut.Find(".mar-gantt__announcer");
        Assert.Contains("expanded", announcer.TextContent);
        Assert.Contains("Alpha", announcer.TextContent);
    }

    [Fact]
    public async Task Collapse_Node_Announces_Collapsed_Message()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        // Click first chevron to collapse Alpha
        var chevron = cut.Find(".mar-gantt__chevron");
        chevron.Click();

        cut.Render();
        var announcer = cut.Find(".mar-gantt__announcer");
        Assert.Contains("collapsed", announcer.TextContent);
        Assert.Contains("Alpha", announcer.TextContent);
    }

    [Fact]
    public async Task BeginCellEdit_Announces_Field_And_Task_Name()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, EditableColumns())
            .Add(x => x.TreeListEditMode, GanttTreeListEditMode.Incell));

        await cut.Instance.BeginCellEdit(0, "Title");

        cut.Render();
        var announcer = cut.Find(".mar-gantt__announcer");
        Assert.Contains("Editing", announcer.TextContent);
        Assert.Contains("Title", announcer.TextContent);
        Assert.Contains("Alpha", announcer.TextContent);
    }

    [Fact]
    public async Task CancelEdit_Announces_Edit_Cancelled()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, EditableColumns())
            .Add(x => x.TreeListEditMode, GanttTreeListEditMode.Incell));

        await cut.Instance.BeginCellEdit(0, "Title");
        await cut.Instance.CancelEdit();

        cut.Render();
        var announcer = cut.Find(".mar-gantt__announcer");
        Assert.Equal("Edit cancelled", announcer.TextContent);
    }

    // ── E16: FilterPopupMode (Drawer vs Popup) ────────────────────────

    [Fact]
    public void FilterPopupMode_Default_IsDrawer()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Default FilterPopupMode is Drawer — verify the parameter default
        Assert.Equal(GanttFilterPopupMode.Drawer, cut.Instance.FilterPopupMode);
    }

    [Fact]
    public void FilterPopupMode_Drawer_RendersDrawerOnOpen()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.FilterPopupMode, GanttFilterPopupMode.Drawer)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Open the checkbox filter
        var filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        filterBtns[^1].Click();

        // Should render MariloDrawer (has class mar-drawer or mar-drawer__panel)
        var checkboxFilter = cut.Find(".mar-gantt__checkbox-filter");
        Assert.NotNull(checkboxFilter);

        // Should NOT render a MariloPopup (mar-popup class)
        var popup = cut.FindAll(".mar-popup");
        Assert.Empty(popup);
    }

    [Fact]
    public void FilterPopupMode_Popup_RendersPopupOnOpen()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.FilterPopupMode, GanttFilterPopupMode.Popup)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Open the checkbox filter
        var filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        filterBtns[^1].Click();

        // Should render the popup (mar-popup class)
        var popup = cut.Find(".mar-popup");
        Assert.NotNull(popup);

        // The checkbox filter content should be inside the popup
        var checkboxFilter = cut.Find(".mar-gantt__checkbox-filter");
        Assert.NotNull(checkboxFilter);
    }

    [Fact]
    public void FilterPopupMode_Popup_FilterButtonHasAnchorId()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.FilterPopupMode, GanttFilterPopupMode.Popup)
            .Add(x => x.GanttColumns, StatusColumns()));

        // The Status column filter button should have id="mar-gantt-filter-Status"
        var btn = cut.Find("#mar-gantt-filter-Status");
        Assert.NotNull(btn);
        Assert.Equal("button", btn.TagName.ToLowerInvariant());
    }

    [Fact]
    public void FilterPopupMode_Popup_Apply_FiltersRows()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.FilterPopupMode, GanttFilterPopupMode.Popup)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Open the Status checkbox popup
        var filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        filterBtns[^1].Click();

        // Uncheck "Active" (first alphabetically)
        var checkboxes = cut.FindAll(".mar-gantt__checkbox-filter-item input[type='checkbox']");
        checkboxes[0].Change(false);

        // Click Apply
        var applyBtn = cut.Find(".mar-gantt__checkbox-filter-actions .mar-gantt__filter-menu-btn");
        applyBtn.Click();

        // Only Pending (1) + Closed (1) = 2 rows should remain
        var rows = cut.FindAll(".mar-gantt__task-row");
        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public void FilterPopupMode_Popup_OutsideClick_ClosesPopup()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.FilterPopupMode, GanttFilterPopupMode.Popup)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Open the checkbox filter popup
        var filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        filterBtns[^1].Click();

        // Confirm popup is open
        cut.Find(".mar-popup");

        // Simulate outside click by invoking OnOutsideClick on the MariloPopup instance
        var popup = cut.FindComponent<MariloPopup>();
        popup.Instance.OnOutsideClickInternal();
        cut.Render();

        // Popup (and checkbox filter) should no longer be visible
        var popups = cut.FindAll(".mar-popup");
        Assert.Empty(popups);
    }

    // ── E15: Popup Edit Mode ──────────────────────────────────────────

    [Fact]
    public async Task PopupEdit_ClickCell_OpensPopup()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns())
            .Add(x => x.TreeListEditMode, GanttTreeListEditMode.Popup));

        // Trigger popup edit programmatically (simulates cell click)
        await cut.Instance.BeginPopupEdit(0, "Title");
        cut.Render();

        // The popup container should be visible
        var popup = cut.Find(".mar-gantt__popup-edit");
        Assert.NotNull(popup);
    }

    [Fact]
    public async Task PopupEdit_ContainsEditableFields()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns())
            .Add(x => x.TreeListEditMode, GanttTreeListEditMode.Popup));

        await cut.Instance.BeginPopupEdit(0, "Title");
        cut.Render();

        var popup = cut.Find(".mar-gantt__popup-edit");

        // All three editable fields (Title, Start, End) should have inputs in the popup
        var inputs = popup.QuerySelectorAll("input");
        Assert.Equal(3, inputs.Length);
    }

    [Fact]
    public async Task PopupEdit_HasRoleDialog_WhenFocusTrap()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns())
            .Add(x => x.TreeListEditMode, GanttTreeListEditMode.Popup));

        await cut.Instance.BeginPopupEdit(0, "Title");
        cut.Render();

        // MariloPopup with FocusTrap=true renders role="dialog"
        var dialog = cut.Find("[role='dialog']");
        Assert.NotNull(dialog);
    }

    [Fact]
    public async Task PopupEdit_SaveButton_CommitsEdit()
    {
        var data = CreateTestData();
        GanttUpdateEventArgs? updatedArgs = null;

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns())
            .Add(x => x.TreeListEditMode, GanttTreeListEditMode.Popup)
            .Add(x => x.OnUpdate, EventCallback.Factory.Create<GanttUpdateEventArgs>(this, args => updatedArgs = args)));

        await cut.Instance.BeginPopupEdit(0, "Title");
        cut.Render();

        // Click the Save button
        var saveBtn = cut.Find(".mar-gantt__edit-btn--save");
        await cut.InvokeAsync(() => saveBtn.Click());
        cut.Render();

        // Popup should be closed
        var popups = cut.FindAll(".mar-gantt__popup-edit");
        Assert.Empty(popups);

        // OnUpdate should have fired
        Assert.NotNull(updatedArgs);
    }

    [Fact]
    public async Task PopupEdit_CancelButton_CancelsEdit()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns())
            .Add(x => x.TreeListEditMode, GanttTreeListEditMode.Popup));

        await cut.Instance.BeginPopupEdit(0, "Title");
        cut.Render();

        // Click the Cancel button
        var cancelBtn = cut.Find(".mar-gantt__edit-btn--cancel");
        await cut.InvokeAsync(() => cancelBtn.Click());
        cut.Render();

        // Popup should be closed and edit state cleared
        var popups = cut.FindAll(".mar-gantt__popup-edit");
        Assert.Empty(popups);

        // EditItem should be null after cancel
        Assert.Null(cut.Instance.GetState().EditItem);
    }

    [Fact]
    public async Task PopupEdit_Cells_HaveIdAttributes()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns())
            .Add(x => x.TreeListEditMode, GanttTreeListEditMode.Popup));

        // The first row's Title cell should have id="mar-gantt-cell-0-Title"
        var titleCell = cut.Find("#mar-gantt-cell-0-Title");
        Assert.NotNull(titleCell);
        Assert.Equal("div", titleCell.TagName.ToLowerInvariant());
    }

    [Fact]
    public async Task PopupEdit_DoesNotOpenInInlineMode()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns())
            .Add(x => x.TreeListEditMode, GanttTreeListEditMode.Inline));

        // BeginPopupEdit should be a no-op in Inline mode
        await cut.Instance.BeginPopupEdit(0, "Title");
        cut.Render();

        var popups = cut.FindAll(".mar-gantt__popup-edit");
        Assert.Empty(popups);
    }

    [Fact]
    public async Task PopupEdit_DoesNotOpenInIncellMode()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns())
            .Add(x => x.TreeListEditMode, GanttTreeListEditMode.Incell));

        // BeginPopupEdit should be a no-op in Incell mode
        await cut.Instance.BeginPopupEdit(0, "Title");
        cut.Render();

        var popups = cut.FindAll(".mar-gantt__popup-edit");
        Assert.Empty(popups);
    }

    // ── E17: Column Chooser ───────────────────────────────────────────

    [Fact]
    public void ColumnChooser_Button_Renders_When_ShowColumnChooser_True()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.ShowColumnChooser, true));

        var btn = cut.Find(".mar-gantt__column-chooser-btn");
        Assert.NotNull(btn);
    }

    [Fact]
    public void ColumnChooser_Button_Not_Rendered_When_ShowColumnChooser_False()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.ShowColumnChooser, false));

        var btns = cut.FindAll(".mar-gantt__column-chooser-btn");
        Assert.Empty(btns);
    }

    [Fact]
    public void ColumnChooser_Opens_On_Button_Click()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.ShowColumnChooser, true));

        // Panel not visible initially
        Assert.Empty(cut.FindAll(".mar-gantt__column-chooser"));

        cut.Find(".mar-gantt__column-chooser-btn").Click();

        Assert.NotNull(cut.Find(".mar-gantt__column-chooser"));
    }

    [Fact]
    public void ColumnChooser_Shows_All_Column_Entries()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.ShowColumnChooser, true));

        cut.Find(".mar-gantt__column-chooser-btn").Click();

        var items = cut.FindAll(".mar-gantt__column-chooser-item");
        Assert.Equal(3, items.Count); // Title, Start, End
    }

    [Fact]
    public async Task ColumnChooser_Unchecking_Column_Hides_It_From_Headers()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.ShowColumnChooser, true));

        // Open chooser
        cut.Find(".mar-gantt__column-chooser-btn").Click();

        // All 3 column headers visible before toggle
        var headersBefore = cut.FindAll(".mar-gantt__header-cell");
        Assert.Equal(3, headersBefore.Count);

        // Uncheck the "End" column (third checkbox)
        var checkboxes = cut.FindAll(".mar-gantt__column-chooser-item input[type=checkbox]");
        await checkboxes[2].ChangeAsync(new Microsoft.AspNetCore.Components.ChangeEventArgs { Value = false });

        cut.Render();

        var headersAfter = cut.FindAll(".mar-gantt__header-cell");
        Assert.Equal(2, headersAfter.Count);
    }

    [Fact]
    public async Task ColumnChooser_Rechecking_Column_Restores_Header()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.ShowColumnChooser, true));

        // Open chooser and uncheck "End" column
        cut.Find(".mar-gantt__column-chooser-btn").Click();
        var checkboxes = cut.FindAll(".mar-gantt__column-chooser-item input[type=checkbox]");
        await checkboxes[2].ChangeAsync(new Microsoft.AspNetCore.Components.ChangeEventArgs { Value = false });
        cut.Render();

        // Re-check it
        checkboxes = cut.FindAll(".mar-gantt__column-chooser-item input[type=checkbox]");
        await checkboxes[2].ChangeAsync(new Microsoft.AspNetCore.Components.ChangeEventArgs { Value = true });
        cut.Render();

        var headers = cut.FindAll(".mar-gantt__header-cell");
        Assert.Equal(3, headers.Count);
    }

    [Fact]
    public void ColumnChooser_GetState_VisibleColumns_Null_When_All_Visible()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var state = cut.Instance.GetState();

        Assert.Null(state.VisibleColumns);
    }

    [Fact]
    public void ColumnChooser_GetState_VisibleColumns_Contains_Only_Visible_Fields_When_Some_Hidden()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.ShowColumnChooser, true));

        // Hide the Start column directly via the instance
        var col = cut.Instance.VisibleColumns.First(c => c.Field == "Start");
        col.SetVisible(false);

        var state = cut.Instance.GetState();

        Assert.NotNull(state.VisibleColumns);
        Assert.DoesNotContain("Start", state.VisibleColumns!);
        Assert.Contains("Title", state.VisibleColumns!);
        Assert.Contains("End", state.VisibleColumns!);
    }

    // ── Task A: OriginalEditItem clone lifecycle ─────────────────────

    [Fact]
    public async Task OriginalEditItem_PreservesOriginalValues_AfterEditValuesMutated()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns()));

        await cut.Instance.BeginEdit(0);

        var stateBeforeCommit = cut.Instance.GetState();
        // OriginalEditItem captures original values
        Assert.Equal("Alpha", stateBeforeCommit.OriginalEditItem!.Title);
        Assert.Equal(data[0].Start, stateBeforeCommit.OriginalEditItem.Start);

        // Commit (writes _editValues back to item)
        await cut.Instance.CommitEdit();

        // After commit, OriginalEditItem is cleared
        var stateAfterCommit = cut.Instance.GetState();
        Assert.Null(stateAfterCommit.OriginalEditItem);
        Assert.Null(stateAfterCommit.EditItem);
    }

    [Fact]
    public async Task OriginalEditItem_PreservesValues_ThroughCancelFlow()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns()));

        var originalTitle = data[0].Title;
        await cut.Instance.BeginEdit(0);

        // Verify original is captured
        var state = cut.Instance.GetState();
        Assert.Equal(originalTitle, state.OriginalEditItem!.Title);

        // Cancel
        await cut.Instance.CancelEdit();

        // After cancel, both are cleared
        state = cut.Instance.GetState();
        Assert.Null(state.EditItem);
        Assert.Null(state.OriginalEditItem);
    }

    [Fact]
    public async Task GetState_InsertedItem_NullByDefault()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var state = cut.Instance.GetState();
        Assert.Null(state.InsertedItem);
        Assert.Null(state.ParentItem);
    }

    [Fact]
    public async Task SetStateAsync_AppliesInsertedItemAndParentItem()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, DefaultColumns()));

        var newItem = new TaskModel { Id = 99, Title = "New Task", Start = BaseDate, End = BaseDate.AddDays(3) };
        var parentItem = data[0]; // Alpha

        await cut.Instance.SetStateAsync(new GanttState<TaskModel>
        {
            InsertedItem = newItem,
            ParentItem = parentItem
        });

        var state = cut.Instance.GetState();
        Assert.NotNull(state.InsertedItem);
        Assert.Equal(99, state.InsertedItem!.Id);
        Assert.NotNull(state.ParentItem);
        Assert.Equal(parentItem.Id, state.ParentItem!.Id);
    }

    [Fact]
    public async Task GanttCloneHelper_ReturnsNull_ForNullInput()
    {
        var result = GanttCloneHelper.DeepClone<TaskModel>(null);
        Assert.Null(result);
    }

    // ── Task B: GanttDependencies component model stub ───────────────

    private static RenderFragment DependenciesSlot(IEnumerable<GanttDependency> deps) => builder =>
    {
        builder.OpenComponent<MariloGanttDependencies<TaskModel>>(0);
        builder.AddAttribute(1, nameof(MariloGanttDependencies<TaskModel>.Data), deps);
        builder.CloseComponent();
    };

    [Fact]
    public void Dependencies_Component_Registers_With_Parent()
    {
        var deps = new List<GanttDependency>
        {
            new() { Id = 1, PredecessorId = 1, SuccessorId = 2, Type = GanttDependencyType.FinishToStart }
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttDependenciesSlot, DependenciesSlot(deps)));

        // The dependency component should register itself and render dependency lines
        var depsComponent = cut.FindComponent<MariloGanttDependencies<TaskModel>>();
        Assert.NotNull(depsComponent);
    }

    [Fact]
    public void Dependencies_Component_GetDependencies_ReturnsData()
    {
        var deps = new List<GanttDependency>
        {
            new() { Id = 1, PredecessorId = 1, SuccessorId = 2, Type = GanttDependencyType.FinishToStart },
            new() { Id = 2, PredecessorId = 2, SuccessorId = 3, Type = GanttDependencyType.StartToStart }
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttDependenciesSlot, DependenciesSlot(deps)));

        var depsComponent = cut.FindComponent<MariloGanttDependencies<TaskModel>>();
        var result = depsComponent.Instance.GetDependencies();
        Assert.Equal(2, result.Count);
        Assert.Equal(GanttDependencyType.FinishToStart, result[0].Type);
    }

    [Fact]
    public void Dependencies_Component_Default_FieldMapping_MatchesGanttDependencyProperties()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttDependenciesSlot, DependenciesSlot(Enumerable.Empty<GanttDependency>())));

        var depsComponent = cut.FindComponent<MariloGanttDependencies<TaskModel>>();
        Assert.Equal("Id", depsComponent.Instance.IdField);
        Assert.Equal("PredecessorId", depsComponent.Instance.PredecessorIdField);
        Assert.Equal("SuccessorId", depsComponent.Instance.SuccessorIdField);
        Assert.Equal("Type", depsComponent.Instance.TypeField);
    }

    [Fact]
    public void GanttDependency_Record_HasCorrectDefaults()
    {
        var dep = new GanttDependency
        {
            Id = 1,
            PredecessorId = 10,
            SuccessorId = 20
        };

        Assert.Equal(GanttDependencyType.FinishToStart, dep.Type); // default
        Assert.Equal(1, dep.Id);
        Assert.Equal(10, dep.PredecessorId);
        Assert.Equal(20, dep.SuccessorId);
    }

    [Fact]
    public void GanttDependencyCreateEventArgs_ConvenienceProperties_MatchDependency()
    {
        var dep = new GanttDependency
        {
            Id = 1, PredecessorId = 10, SuccessorId = 20, Type = GanttDependencyType.StartToStart
        };
        var args = new GanttDependencyCreateEventArgs { Dependency = dep };

        Assert.Equal(10, args.PredecessorId);
        Assert.Equal(20, args.SuccessorId);
        Assert.Equal(GanttDependencyType.StartToStart, args.Type);
    }

    [Fact]
    public void GanttDependencyDeleteEventArgs_Item_ReturnsDependency()
    {
        var dep = new GanttDependency
        {
            Id = 5, PredecessorId = 1, SuccessorId = 2
        };
        var args = new GanttDependencyDeleteEventArgs { Dependency = dep };

        Assert.Same(dep, args.Item);
    }

    [Fact]
    public void GanttDependencyType_HasAllFourTypes()
    {
        var values = Enum.GetValues<GanttDependencyType>();
        Assert.Equal(4, values.Length);
        Assert.Contains(GanttDependencyType.FinishToStart, values);
        Assert.Contains(GanttDependencyType.StartToStart, values);
        Assert.Contains(GanttDependencyType.FinishToFinish, values);
        Assert.Contains(GanttDependencyType.StartToFinish, values);
    }

    // ── Task C: Accessibility announcements (additional) ─────────────

    [Fact]
    public async Task CommitEdit_Announces_FieldUpdated()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, EditableColumns()));

        await cut.Instance.BeginEdit(0);
        await cut.Instance.CommitEdit();

        cut.Render();
        var announcer = cut.Find(".mar-gantt__announcer");
        Assert.Contains("updated", announcer.TextContent);
    }

    [Fact]
    public async Task KeyboardNavigation_ArrowDown_Announces_TaskNameAndPosition()
    {
        var data = CreateTestData();
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, DefaultColumns()));

        // Simulate ArrowDown key on the treegrid
        var treegrid = cut.Find("[role='treegrid']");
        await treegrid.KeyDownAsync(new Microsoft.AspNetCore.Components.Web.KeyboardEventArgs { Key = "ArrowDown" });

        cut.Render();
        var announcer = cut.Find(".mar-gantt__announcer");
        // Should announce the task and position
        Assert.Contains("Task", announcer.TextContent);
        Assert.Contains("row", announcer.TextContent);
    }

    [Fact]
    public void SkipLinks_Render_For_TasklistAndTimeline()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData()));

        var skipLinks = cut.FindAll(".mar-gantt__skip-link");
        Assert.Equal(2, skipLinks.Count);
        Assert.Contains("task list", skipLinks[0].TextContent, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("timeline", skipLinks[1].TextContent, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Announcer_Starts_Empty()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        var announcer = cut.Find(".mar-gantt__announcer");
        Assert.Equal("", announcer.TextContent);
    }

    // ── Task D: CheckboxFilter state integration ─────────────────────

    [Fact]
    public void CheckboxFilter_GetState_ReflectsAppliedFilter()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Open and apply a filter selecting only "Active"
        var filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        filterBtns[^1].Click();

        // Uncheck Closed and Pending (re-find after each change to avoid stale handlers)
        cut.FindAll(".mar-gantt__checkbox-filter-item input[type='checkbox']")[1].Change(false); // Uncheck "Closed"
        cut.FindAll(".mar-gantt__checkbox-filter-item input[type='checkbox']")[2].Change(false); // Uncheck "Pending"

        var applyBtn = cut.Find(".mar-gantt__checkbox-filter-actions .mar-gantt__filter-menu-btn");
        applyBtn.Click();

        var state = cut.Instance.GetState();
        Assert.NotNull(state.FilterValues);
        Assert.True(state.FilterValues!.ContainsKey("Status"));
        Assert.Contains("Active", state.FilterValues["Status"]);
    }

    [Fact]
    public void CheckboxFilter_SelectAll_EqualsNoFilter()
    {
        var cut = Render<MariloGantt<StatusTaskModel>>(p => p
            .Add(x => x.Data, CreateStatusData())
            .Add(x => x.FilterMode, GanttFilterMode.FilterMenu)
            .Add(x => x.GanttColumns, StatusColumns()));

        // Open drawer, leave all checked, apply
        var filterBtns = cut.FindAll(".mar-gantt__filter-btn");
        filterBtns[^1].Click();

        var applyBtn = cut.Find(".mar-gantt__checkbox-filter-actions .mar-gantt__filter-menu-btn");
        applyBtn.Click();

        // All selected = no filter applied
        var state = cut.Instance.GetState();
        Assert.True(state.FilterValues is null || !state.FilterValues.ContainsKey("Status"));
        Assert.Equal(4, cut.FindAll(".mar-gantt__task-row").Count);
    }

    [Fact]
    public void CheckboxFilter_ColumnFilterType_DefaultIsText()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns()));

        // Default column FilterType is Text
        var col = cut.Instance.VisibleColumns.First();
        Assert.Equal(GanttColumnFilterType.Text, col.FilterType);
    }

    // ── Bar base rendering (W4-INT-13) ───────────────────────────────

    [Fact]
    public void Gantt_Renders_Bar_With_Base_Class()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week)));

        // The Razor emits <div class="mar-gantt__bar ..."> for each task bar.
        // At least one bar should be present for the 9-item test data set.
        var bars = cut.FindAll(".mar-gantt__bar");
        Assert.NotEmpty(bars);
    }

    [Fact]
    public void Gantt_Renders_BarRow_With_Base_Class()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week)));

        // Each bar sits inside a bar-row container.
        var barRows = cut.FindAll(".mar-gantt__bar-row");
        Assert.NotEmpty(barRows);
    }

    // ── VP-gantt-03: Dependency line uses CSS class ───────────────────────

    [Fact]
    public void Dependency_Line_Has_CSS_Class_Not_Hardcoded_Stroke()
    {
        var data = new List<TaskModel>
        {
            new() { Id = 1, ParentId = null, Title = "Task A", Start = BaseDate, End = BaseDate.AddDays(3) },
            new() { Id = 2, ParentId = null, Title = "Task B", Start = BaseDate.AddDays(3), End = BaseDate.AddDays(6), DependsOn = [1] },
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week)));

        var lines = cut.FindAll(".mar-gantt__dependency-svg line");
        Assert.Single(lines);

        var line = lines[0];
        // Must have the CSS class
        Assert.Contains("mar-gantt__dependency", line.GetAttribute("class") ?? "");
        // Must NOT have hardcoded stroke attribute
        Assert.Null(line.GetAttribute("stroke"));
    }

    // ── W4-INT-19: Selection API ─────────────────────────────────────────

    [Fact]
    public void Task_Bar_Click_Sets_SelectedTask()
    {
        TaskModel? selected = null;
        var data = CreateTestData();

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week))
            .Add(x => x.OnTaskSelect, EventCallback.Factory.Create<TaskModel?>(this, val => selected = val)));

        // Click on the first task row to trigger selection
        var rows = cut.FindAll(".mar-gantt__task-row");
        Assert.NotEmpty(rows);
        rows[0].Click();

        Assert.NotNull(selected);
    }

    [Fact]
    public void Selected_Task_Bar_Gets_Selected_CSS_Class()
    {
        var data = CreateTestData();
        var firstTask = data[0];

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week))
            .Add(x => x.SelectedTask, firstTask));

        // The bar for the selected (first) task should have the --selected class
        var selectedBars = cut.FindAll(".mar-gantt__bar--selected");
        Assert.NotEmpty(selectedBars);
    }

    // ── Sortable=false disables sorting ──────────────────────────────

    [Fact]
    public void Sortable_False_Prevents_Sort_On_Header_Click()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.Sortable, false)
            .Add(x => x.GanttColumns, DefaultColumns()));

        var header = cut.Find(".mar-gantt__header-cell");
        header.Click();

        // No sort indicator should appear when Sortable is false
        var indicators = cut.FindAll(".mar-gantt__sort-indicator");
        Assert.Empty(indicators);
    }

    // ── FilterMode.None hides filter row ─────────────────────────────

    [Fact]
    public void FilterMode_None_Hides_FilterRow()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.FilterMode, GanttFilterMode.None)
            .Add(x => x.GanttColumns, DefaultColumns()));

        var filterRows = cut.FindAll(".mar-gantt__filter-row");
        Assert.Empty(filterRows);
        var filterInputs = cut.FindAll(".mar-gantt__filter-input");
        Assert.Empty(filterInputs);
    }

    // ── SelectedTaskChanged fires on selection ───────────────────────

    [Fact]
    public void SelectedTaskChanged_Fires_On_Row_Click()
    {
        TaskModel? changedTask = null;
        var data = CreateTestData();

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week))
            .Add(x => x.SelectedTaskChanged, EventCallback.Factory.Create<TaskModel?>(this, val => changedTask = val)));

        var rows = cut.FindAll(".mar-gantt__task-row");
        rows[0].Click();

        Assert.NotNull(changedTask);
        Assert.Equal("Alpha", changedTask!.Title);
    }

    // ── NewRowPosition default ───────────────────────────────────────

    [Fact]
    public void NewRowPosition_Default_IsTop()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData()));

        Assert.Equal(GanttNewRowPosition.Top, cut.Instance.NewRowPosition);
    }

    // ── Milestone rendering ──────────────────────────────────────────

    [Fact]
    public void Milestone_Task_Renders_Diamond()
    {
        var data = new List<TaskModel>
        {
            new() { Id = 1, ParentId = null, Title = "Milestone", Start = BaseDate, End = BaseDate, PercentComplete = 0 },
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week)));

        var milestones = cut.FindAll(".mar-gantt__milestone");
        Assert.NotEmpty(milestones);

        var diamonds = cut.FindAll(".mar-gantt__milestone-diamond");
        Assert.NotEmpty(diamonds);
    }

    // ── Summary task rendering ───────────────────────────────────────

    [Fact]
    public void Summary_Task_Gets_Summary_CSS_Class()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttColumns, DefaultColumns())
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week)));

        // Parent tasks (Alpha, Beta, Gamma) have children and should get --summary
        var summaryBars = cut.FindAll(".mar-gantt__bar--summary");
        Assert.Equal(3, summaryBars.Count);
    }

    // ── TooltipTemplate renders custom content ───────────────────────

    [Fact]
    public void TooltipTemplate_Renders_Custom_Content()
    {
        RenderFragment<TaskModel> tooltipTpl = item => builder =>
        {
            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "class", "custom-tooltip");
            builder.AddContent(2, $"TIP:{item.Title}");
            builder.CloseElement();
        };

        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData())
            .Add(x => x.GanttViews, ViewsFragment(GanttView.Week))
            .Add(x => x.TooltipTemplate, tooltipTpl));

        var customTooltips = cut.FindAll(".custom-tooltip");
        Assert.True(customTooltips.Count > 0);
        Assert.Contains("TIP:Alpha", customTooltips[0].TextContent);
    }

    // ── FilterRowDebounceDelay parameter ─────────────────────────────

    [Fact]
    public void FilterRowDebounceDelay_Default_IsZero()
    {
        var cut = Render<MariloGantt<TaskModel>>(p => p
            .Add(x => x.Data, CreateTestData()));

        Assert.Equal(0, cut.Instance.FilterRowDebounceDelay);
    }

}
