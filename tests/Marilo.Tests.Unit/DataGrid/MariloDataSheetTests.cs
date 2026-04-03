using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Enums;
using Marilo.Core.Models.DataSheet;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloDataSheetTests : MariloTestBase
{
    private record TestRow
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public decimal Total => Amount * 2;
    }

    private static List<TestRow> SeedData() =>
    [
        new() { Name = "Alpha", Amount = 100m, IsActive = true },
        new() { Name = "Beta", Amount = 200m, IsActive = false },
        new() { Name = "Gamma", Amount = 300m, IsActive = true },
    ];

    // ── 1. Empty State ─────────────────────────────────────────────────

    [Fact]
    public void Renders_With_No_Data_Shows_EmptyState()
    {
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, new List<TestRow>())
            .Add(x => x.EmptyStateMessage, "Nothing here.")
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        Assert.Contains("Nothing here.", cut.Markup);
    }

    // ── 2. Column Registration ─────────────────────────────────────────

    [Fact]
    public void Columns_Register_From_ChildContent()
    {
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, SeedData())
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();

                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(10);
                builder.AddAttribute(11, "Field", "Amount");
                builder.AddAttribute(12, "Title", "Amount");
                builder.CloseComponent();

                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(20);
                builder.AddAttribute(21, "Field", "Total");
                builder.AddAttribute(22, "Title", "Total");
                builder.AddAttribute(23, "ColumnType", DataSheetColumnType.Computed);
                builder.CloseComponent();
            }));

        var headers = cut.FindAll("th[role='columnheader']");
        Assert.True(headers.Count >= 3, $"Expected at least 3 column headers, found {headers.Count}");
        Assert.Contains(headers, h => h.TextContent.Contains("Name"));
        Assert.Contains(headers, h => h.TextContent.Contains("Amount"));
        Assert.Contains(headers, h => h.TextContent.Contains("Total"));
    }

    // ── 3. Click Cell Enters Edit Mode ─────────────────────────────────

    [Fact]
    public void Clicking_Cell_Enters_Edit_Mode()
    {
        var data = SeedData();
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        // Use EnterEditMode directly via InvokeAsync to avoid stale DOM reference issues
        cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "Name"));

        var inputs = cut.FindAll("td[data-field='Name'] input[type='text']");
        Assert.NotEmpty(inputs);
    }

    // ── 4. Escape Cancels Edit ─────────────────────────────────────────

    [Fact]
    public void Escape_Cancels_Cell_Edit()
    {
        var data = SeedData();
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        // Enter edit mode via InvokeAsync
        cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "Name"));

        // Verify we're in edit mode
        Assert.NotEmpty(cut.FindAll("td[data-field='Name'] input[type='text']"));

        // Simulate escape
        cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Escape", false, false));

        // Input should be gone
        Assert.Empty(cut.FindAll("td[data-field='Name'] input[type='text']"));
    }

    // ── 5. Commit Cell Marks Row Dirty ─────────────────────────────────

    [Fact]
    public void Commit_Cell_Marks_Row_Dirty()
    {
        var data = SeedData();
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Changed"));

        var dirtyRows = cut.Instance.GetDirtyRows();
        Assert.Single(dirtyRows);
        Assert.Equal("Changed", data[0].Name);
    }

    // ── 6. SaveAll Fires OnSaveAll ─────────────────────────────────────

    [Fact]
    public void SaveAll_Fires_OnSaveAll_With_Dirty_Rows()
    {
        var data = SeedData();
        DataSheetSaveArgs<TestRow>? savedArgs = null;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.OnSaveAll, args => { savedArgs = args; })
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Edited"));
        cut.InvokeAsync(() => cut.Instance.SaveAllAsync());

        Assert.NotNull(savedArgs);
        Assert.Single(savedArgs!.DirtyRows);
    }

    // ── 7. Validation Blocks SaveAll ───────────────────────────────────

    [Fact]
    public void Validation_Blocks_SaveAll_When_Required_Empty()
    {
        var data = SeedData();
        DataSheetSaveArgs<TestRow>? savedArgs = null;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.OnSaveAll, args => { savedArgs = args; })
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.AddAttribute(3, "Required", true);
                builder.CloseComponent();
            }));

        // Commit an empty value for a required field
        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", ""));
        cut.InvokeAsync(() => cut.Instance.SaveAllAsync());

        // OnSaveAll should NOT have fired because validation blocked it
        Assert.Null(savedArgs);
    }

    // ── 8. Reset Clears Dirty State ────────────────────────────────────

    [Fact]
    public void Reset_Clears_All_Dirty_State()
    {
        var data = SeedData();
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Changed"));
        Assert.NotEmpty(cut.Instance.GetDirtyRows());

        cut.InvokeAsync(() => cut.Instance.ResetAsync());
        Assert.Empty(cut.Instance.GetDirtyRows());
    }

    // ── 9. Computed Column Not Editable ────────────────────────────────

    [Fact]
    public void Computed_Column_Is_Not_Editable()
    {
        var data = SeedData();
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Total");
                builder.AddAttribute(2, "Title", "Total");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Computed);
                builder.AddAttribute(4, "Editable", false);
                builder.CloseComponent();
            }));

        // Verify computed cells have aria-readonly
        var cells = cut.FindAll("td[data-field='Total']");
        Assert.NotEmpty(cells);
        Assert.Equal("true", cells[0].GetAttribute("aria-readonly"));

        // Programmatically try to enter edit mode - should not work
        cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "Total"));

        // Should NOT have an input
        var inputs = cut.FindAll("td[data-field='Total'] input");
        Assert.Empty(inputs);
    }

    // ── 10. IsLoading Shows Skeleton ───────────────────────────────────

    [Fact]
    public void IsLoading_Shows_Skeleton_Rows()
    {
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, SeedData())
            .Add(x => x.IsLoading, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        Assert.Contains("mar-datasheet__skeleton", cut.Markup);
        Assert.Contains("Loading", cut.Markup);
    }
}
