using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloTreeListPagingOnReadTests : MariloTestBase
{
    private class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Department { get; set; } = "";
    }

    private static List<Employee> GenerateEmployees(int count) =>
        Enumerable.Range(1, count).Select(i => new Employee { Id = i, Name = $"Employee {i}", Department = $"Dept {(i % 3) + 1}" }).ToList();

    // ── Paging Tests ────────────────────────────────────────────

    [Fact]
    public void Pageable_Limits_Visible_Rows_To_PageSize()
    {
        var data = GenerateEmployees(25);
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 10)
            .Add(p => p.Page, 1)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        var rows = cut.FindAll("tr.mar-treelist__row");
        Assert.Equal(10, rows.Count);

        // Pager should be rendered
        var pager = cut.Find("nav.mar-treelist__pager");
        Assert.NotNull(pager);

        // Should have 3 pages: ceil(25/10) = 3
        var pageButtons = cut.FindAll("button.mar-treelist__pager-btn")
            .Where(b => !b.ClassList.Contains("mar-treelist__pager-btn--prev")
                     && !b.ClassList.Contains("mar-treelist__pager-btn--next"))
            .ToList();
        Assert.Equal(3, pageButtons.Count);
    }

    [Fact]
    public void Next_Button_Fires_PageChanged_Callback()
    {
        int pageChangedTo = 0;
        var data = GenerateEmployees(25);
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 10)
            .Add(p => p.Page, 1)
            .Add(p => p.PageChanged, (int pg) => pageChangedTo = pg)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        // Page 1: first row should be Employee 1
        var firstRowCells = cut.FindAll("tr.mar-treelist__row")[0].QuerySelectorAll("td");
        Assert.Contains("Employee 1", firstRowCells[0].TextContent);

        // Click the "Next" button
        var nextBtn = cut.Find("button.mar-treelist__pager-btn--next");
        nextBtn.Click();

        // PageChanged should have fired with page 2
        Assert.Equal(2, pageChangedTo);

        // After click, component internally updates: should now show page 2 items
        var rowsAfterNav = cut.FindAll("tr.mar-treelist__row");
        Assert.Equal(10, rowsAfterNav.Count);
        var page2FirstCells = rowsAfterNav[0].QuerySelectorAll("td");
        Assert.Contains("Employee 11", page2FirstCells[0].TextContent);
    }

    [Fact]
    public void Last_Page_Shows_Remaining_Items()
    {
        var data = GenerateEmployees(25);
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 10)
            .Add(p => p.Page, 3)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var rows = cut.FindAll("tr.mar-treelist__row");
        Assert.Equal(5, rows.Count); // 25 - 20 = 5 remaining
    }

    [Fact]
    public void Pager_Not_Rendered_When_Pageable_Is_False()
    {
        var data = GenerateEmployees(25);
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.Pageable, false)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var pagers = cut.FindAll("nav.mar-treelist__pager");
        Assert.Empty(pagers);

        // All 25 rows should be visible
        var rows = cut.FindAll("tr.mar-treelist__row");
        Assert.Equal(25, rows.Count);
    }

    [Fact]
    public void Active_Page_Button_Has_Active_Class()
    {
        var data = GenerateEmployees(25);
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 10)
            .Add(p => p.Page, 2)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var activeButtons = cut.FindAll("button.mar-treelist__pager-btn--active");
        Assert.Single(activeButtons);
        Assert.Equal("2", activeButtons[0].TextContent);
    }

    [Fact]
    public void Prev_Button_Disabled_On_First_Page()
    {
        var data = GenerateEmployees(25);
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 10)
            .Add(p => p.Page, 1)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var prevBtn = cut.Find("button.mar-treelist__pager-btn--prev");
        Assert.True(prevBtn.HasAttribute("disabled"));
    }

    // ── OnRead Tests ────────────────────────────────────────────

    [Fact]
    public void OnRead_Fires_With_Correct_Args()
    {
        TreeListReadEventArgs<Employee>? receivedArgs = null;

        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 5)
            .Add(p => p.Page, 2)
            .Add(p => p.OnRead, (TreeListReadEventArgs<Employee> args) =>
            {
                receivedArgs = args;
                args.Data = GenerateEmployees(5);
                args.Total = 20;
            })
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        Assert.NotNull(receivedArgs);
        Assert.Equal(2, receivedArgs!.Page);
        Assert.Equal(5, receivedArgs.PageSize);
    }

    [Fact]
    public void OnRead_Data_Displayed_In_TreeList()
    {
        var serverData = new List<Employee>
        {
            new() { Id = 100, Name = "ServerAlice", Department = "ServerDept" },
            new() { Id = 200, Name = "ServerBob", Department = "ServerDept2" },
        };

        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 10)
            .Add(p => p.Page, 1)
            .Add(p => p.OnRead, (TreeListReadEventArgs<Employee> args) =>
            {
                args.Data = serverData;
                args.Total = 2;
            })
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        var rows = cut.FindAll("tr.mar-treelist__row");
        Assert.Equal(2, rows.Count);

        var cells = cut.FindAll("td.mar-treelist__td");
        Assert.Contains("ServerAlice", cells[0].TextContent);
        Assert.Contains("ServerDept", cells[1].TextContent);
        Assert.Contains("ServerBob", cells[2].TextContent);
    }

    [Fact]
    public void OnRead_Receives_Sort_Info_After_Header_Click()
    {
        TreeListReadEventArgs<Employee>? lastArgs = null;

        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Sortable, true)
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 10)
            .Add(p => p.Page, 1)
            .Add(p => p.OnRead, (TreeListReadEventArgs<Employee> args) =>
            {
                lastArgs = args;
                args.Data = GenerateEmployees(3);
                args.Total = 3;
            })
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        // Initial load has no sort
        Assert.NotNull(lastArgs);
        Assert.Null(lastArgs!.SortField);
        Assert.Null(lastArgs.SortDirection);

        // Click header to sort
        var header = cut.Find("th.mar-treelist__th--sortable");
        header.Click();

        Assert.NotNull(lastArgs);
        Assert.Equal("Name", lastArgs!.SortField);
        Assert.Equal(SortDirection.Ascending, lastArgs.SortDirection);
    }

    [Fact]
    public void OnRead_Pager_Reflects_Server_Total()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 10)
            .Add(p => p.Page, 1)
            .Add(p => p.OnRead, (TreeListReadEventArgs<Employee> args) =>
            {
                args.Data = GenerateEmployees(10);
                args.Total = 35;
            })
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        // Should show 4 page buttons (ceil(35/10) = 4)
        var pageButtons = cut.FindAll("button.mar-treelist__pager-btn")
            .Where(b => !b.ClassList.Contains("mar-treelist__pager-btn--prev")
                     && !b.ClassList.Contains("mar-treelist__pager-btn--next"))
            .ToList();
        Assert.Equal(4, pageButtons.Count);
    }
}
