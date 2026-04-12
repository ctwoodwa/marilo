using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Models;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloTreeListColumnTests : MariloTestBase
{
    private record Employee(int Id, string Name, string Department, int? ParentId = null);

    private static readonly List<Employee> FlatData =
    [
        new(1, "Alice", "Engineering"),
        new(2, "Bob", "Marketing"),
        new(3, "Carol", "Engineering"),
    ];

    private static readonly List<Employee> HierarchicalFlatData =
    [
        new(1, "Alice", "Engineering"),
        new(2, "Bob", "Engineering", 1),
        new(3, "Carol", "Marketing"),
        new(4, "David", "Marketing", 3),
    ];

    [Fact]
    public void TreeList_Renders_Columns_From_Child_MariloTreeListColumn()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Employee Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")
                .Add(c => c.Title, "Dept")));

        var headers = cut.FindAll("th.mar-treelist__th");
        Assert.Equal(2, headers.Count);
        Assert.Contains("Employee Name", headers[0].TextContent);
        Assert.Contains("Dept", headers[1].TextContent);
    }

    [Fact]
    public void TreeList_Renders_Data_Rows_With_Child_Columns()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        var rows = cut.FindAll("tr.mar-treelist__row");
        Assert.Equal(3, rows.Count);

        var cells = cut.FindAll("td.mar-treelist__td");
        Assert.Equal(6, cells.Count); // 3 rows x 2 columns
    }

    [Fact]
    public void TreeList_Cell_Values_Rendered_From_Child_Columns()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        var cells = cut.FindAll("td.mar-treelist__td");
        Assert.Contains("Alice", cells[0].TextContent);
        Assert.Contains("Engineering", cells[1].TextContent);
        Assert.Contains("Bob", cells[2].TextContent);
        Assert.Contains("Marketing", cells[3].TextContent);
    }

#pragma warning disable CS0618 // Testing obsolete Columns parameter intentionally
    [Fact]
    public void TreeList_Renders_Columns_From_Legacy_Columns_Parameter()
    {
        var columns = new List<TreeListColumn>
        {
            new() { Field = "Name", Title = "Employee" },
            new() { Field = "Department", Title = "Dept" },
        };

        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Columns, columns));

        var headers = cut.FindAll("th.mar-treelist__th");
        Assert.Equal(2, headers.Count);
        Assert.Contains("Employee", headers[0].TextContent);
        Assert.Contains("Dept", headers[1].TextContent);

        var rows = cut.FindAll("tr.mar-treelist__row");
        Assert.Equal(3, rows.Count);
    }

    [Fact]
    public void Child_Columns_Take_Precedence_Over_Legacy_Columns_Parameter()
    {
        var legacyColumns = new List<TreeListColumn>
        {
            new() { Field = "Id", Title = "ID" },
        };

        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Columns, legacyColumns)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Full Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        var headers = cut.FindAll("th.mar-treelist__th");
        Assert.Equal(2, headers.Count);
        // Child columns should win: "Full Name" and "Department"
        Assert.Contains("Full Name", headers[0].TextContent);
        Assert.Contains("Department", headers[1].TextContent);
    }
#pragma warning restore CS0618

    [Fact]
    public void Column_Width_Applied_To_Header()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Width, "200px"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        var headers = cut.FindAll("th.mar-treelist__th");
        Assert.Contains("width:200px", headers[0].GetAttribute("style"));
        // Second column has no width set
        var secondStyle = headers[1].GetAttribute("style") ?? "";
        Assert.DoesNotContain("width:", secondStyle);
    }

    [Fact]
    public void Column_Title_Defaults_To_Field_Name()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var header = cut.Find("th.mar-treelist__th");
        Assert.Contains("Name", header.TextContent);
    }

    [Fact]
    public void Empty_TreeList_No_Columns_No_Data_Renders_Without_Error()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, Array.Empty<Employee>()));

        // Should render the container and empty table structure
        var table = cut.Find("table.mar-treelist__table");
        Assert.NotNull(table);

        var rows = cut.FindAll("tr.mar-treelist__row");
        Assert.Empty(rows);

        var headers = cut.FindAll("th.mar-treelist__th");
        Assert.Empty(headers);
    }

    [Fact]
    public void TreeList_Flat_Data_With_Child_Columns_Renders_Hierarchy()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, HierarchicalFlatData)
            .Add(p => p.IdField, "Id")
            .Add(p => p.ParentIdField, "ParentId")
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        // Only root rows visible initially (Alice, Carol)
        var rows = cut.FindAll("tr.mar-treelist__row");
        Assert.Equal(2, rows.Count);

        // Root rows have aria-level 1
        Assert.Equal("1", rows[0].GetAttribute("aria-level"));
        Assert.Equal("1", rows[1].GetAttribute("aria-level"));
    }

    [Fact]
    public void TreeList_Has_Treegrid_Role()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        Assert.Equal("treegrid", table.GetAttribute("role"));
    }
}
