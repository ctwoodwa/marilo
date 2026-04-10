using Bunit;
using Marilo.Components.DataGrid;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloDataGridRowDragTests : MariloTestBase
{
    private record Product(int Id, string Name, string Category, decimal Price);

    private static readonly List<Product> Products =
    [
        new(1, "Widget A", "Tools", 10.0m),
        new(2, "Widget B", "Tools", 20.0m),
        new(3, "Gadget C", "Electronics", 30.0m),
    ];

    // ── RowDraggable=false (default): no drag handle column ────────────

    [Fact]
    public void RowDraggable_False_By_Default_No_DragColumn()
    {
        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Name");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Name");
                builder.CloseComponent();
            }));

        Assert.False(cut.Instance.RowDraggable);
        Assert.Empty(cut.FindAll(".mar-datagrid-drag-header"));
        Assert.Empty(cut.FindAll(".mar-datagrid-drag-cell"));
    }

    // ── RowDraggable=true: drag header appears ─────────────────────────

    [Fact]
    public void RowDraggable_True_Renders_DragHeader()
    {
        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.RowDraggable, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Name");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Name");
                builder.CloseComponent();
            }));

        var dragHeaders = cut.FindAll(".mar-datagrid-drag-header");
        Assert.Single(dragHeaders);
    }

    // ── RowDraggable=true: drag cells appear in body rows ──────────────

    [Fact]
    public void RowDraggable_True_Renders_DragCells_In_Body()
    {
        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.RowDraggable, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Name");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Name");
                builder.CloseComponent();
            }));

        var dragCells = cut.FindAll(".mar-datagrid-drag-cell");
        Assert.Equal(Products.Count, dragCells.Count);

        // Each drag cell should have draggable="true" and data-row-index
        for (var i = 0; i < dragCells.Count; i++)
        {
            Assert.Equal("true", dragCells[i].GetAttribute("draggable"));
            Assert.Equal(i.ToString(), dragCells[i].GetAttribute("data-row-index"));
        }
    }

    // ── GridRowDropEventArgs has correct property types and defaults ────

    [Fact]
    public void GridRowDropEventArgs_HasCorrectDefaults()
    {
        var args = new GridRowDropEventArgs<Product>();

        Assert.Equal(default, args.Item);
        Assert.Null(args.DestinationItem);
        Assert.Equal(0, args.DestinationIndex);
        Assert.Equal(GridRowDropPosition.Before, args.DropPosition);
        Assert.False(args.IsCancelled);
    }

    [Fact]
    public void GridRowDropEventArgs_Properties_Settable()
    {
        var source = Products[0];
        var dest = Products[2];

        var args = new GridRowDropEventArgs<Product>
        {
            Item = source,
            DestinationItem = dest,
            DestinationIndex = 2,
            DropPosition = GridRowDropPosition.After
        };

        Assert.Equal(source, args.Item);
        Assert.Equal(dest, args.DestinationItem);
        Assert.Equal(2, args.DestinationIndex);
        Assert.Equal(GridRowDropPosition.After, args.DropPosition);

        args.IsCancelled = true;
        Assert.True(args.IsCancelled);
    }

    // ── OnRowDrop callback type is correct ─────────────────────────────

    [Fact]
    public void OnRowDrop_EventCallback_Parameter_Accepted()
    {
        GridRowDropEventArgs<Product>? received = null;

        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.RowDraggable, true)
            .Add(x => x.OnRowDrop, EventCallback.Factory.Create<GridRowDropEventArgs<Product>>(this, args => received = args))
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Name");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Name");
                builder.CloseComponent();
            }));

        // Grid renders successfully with the callback wired up
        Assert.NotNull(cut.Find("table"));
    }

    // ── GridRowDropPosition enum values ────────────────────────────────

    [Fact]
    public void GridRowDropPosition_Enum_HasExpectedValues()
    {
        Assert.Equal(0, (int)GridRowDropPosition.Before);
        Assert.Equal(1, (int)GridRowDropPosition.After);
    }
}
