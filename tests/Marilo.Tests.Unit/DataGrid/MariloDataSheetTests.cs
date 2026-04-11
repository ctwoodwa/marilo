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

    // ─────────────────────────────────────────────────────────────────────
    // Batch F1 — Validation & dirty tracking correctness (GAP-DATASHEET-V01/V02)
    // ─────────────────────────────────────────────────────────────────────

    private record NumericRow
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public int IntValue { get; set; }
        public double DoubleValue { get; set; }
        public int? NullableInt { get; set; }
    }

    // ── Fix 1: Checkbox Required enforcement (V01.1) ───────────────────

    [Fact]
    public void CheckboxRequired_FalseValue_ProducesError()
    {
        var data = SeedData();
        data[0].IsActive = false;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "IsActive");
                builder.AddAttribute(2, "Title", "Active");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Checkbox);
                builder.AddAttribute(4, "Required", true);
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "IsActive", false));

        Assert.Equal(CellState.Invalid, cut.Instance.GetCellState(data[0], "IsActive"));
        var error = cut.Instance.GetCellError(data[0], "IsActive");
        Assert.NotNull(error);
        Assert.Contains("required", error!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CheckboxRequired_TrueValue_Passes()
    {
        var data = SeedData();
        data[0].IsActive = false;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "IsActive");
                builder.AddAttribute(2, "Title", "Active");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Checkbox);
                builder.AddAttribute(4, "Required", true);
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "IsActive", true));

        Assert.Null(cut.Instance.GetCellError(data[0], "IsActive"));
        Assert.NotEqual(CellState.Invalid, cut.Instance.GetCellState(data[0], "IsActive"));
    }

    [Fact]
    public void CheckboxNotRequired_FalseValue_Passes()
    {
        var data = SeedData();
        data[0].IsActive = true;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "IsActive");
                builder.AddAttribute(2, "Title", "Active");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Checkbox);
                builder.AddAttribute(4, "Required", false);
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "IsActive", false));

        Assert.Null(cut.Instance.GetCellError(data[0], "IsActive"));
        Assert.NotEqual(CellState.Invalid, cut.Instance.GetCellState(data[0], "IsActive"));
    }

    // ── Fix 2: Number parsing fallback for non-decimal types (V01.2) ───

    [Fact]
    public void NumberColumn_IntProperty_ParsesCorrectly()
    {
        var (success, value) = MariloDataSheet<NumericRow>.ParseNumericValue("42", typeof(int));

        Assert.True(success);
        Assert.IsType<int>(value);
        Assert.Equal(42, value);
    }

    [Fact]
    public void NumberColumn_DoubleProperty_ParsesCorrectly()
    {
        var (success, value) = MariloDataSheet<NumericRow>.ParseNumericValue("3.14", typeof(double));

        Assert.True(success);
        Assert.IsType<double>(value);
        Assert.Equal(3.14, (double)value!, 5);
    }

    [Fact]
    public void NumberColumn_NullableInt_EmptyStringWritesNull()
    {
        var (success, value) = MariloDataSheet<NumericRow>.ParseNumericValue("", typeof(int?));

        Assert.True(success);
        Assert.Null(value);
    }

    // ── Fix 3: Field-level dirty cleared on revert to original (V02.1) ─

    [Fact]
    public void CellRevert_RemovesFieldFromDirtySet()
    {
        var data = SeedData();
        var original = data[0].Name; // "Alpha"

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

        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", original));

        Assert.Empty(cut.Instance.GetDirtyRows());
        Assert.Equal(CellState.Pristine, cut.Instance.GetCellState(data[0], "Name"));
    }

    [Fact]
    public void CellRevert_LeavesOtherDirtyFieldsIntact()
    {
        var data = SeedData();
        var originalName = data[0].Name;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();

                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(10);
                builder.AddAttribute(11, "Field", "Amount");
                builder.AddAttribute(12, "Title", "Amount");
                builder.AddAttribute(13, "ColumnType", DataSheetColumnType.Number);
                builder.CloseComponent();
            }));

        // Edit two fields on the same row.
        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Changed"));
        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Amount", 999m));

        // Revert only Name.
        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", originalName));

        // Row is still dirty because Amount remains modified.
        Assert.Single(cut.Instance.GetDirtyRows());
        Assert.Equal(CellState.Pristine, cut.Instance.GetCellState(data[0], "Name"));
        Assert.Equal(CellState.Dirty, cut.Instance.GetCellState(data[0], "Amount"));
    }

    [Fact]
    public void CellRevert_WithValidationError_StillTracksState()
    {
        var data = SeedData();
        var original = data[0].Name; // "Alpha"

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.AddAttribute(3, "Required", true);
                builder.CloseComponent();
            }));

        // Edit to empty string — invalid (required).
        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", ""));
        Assert.Equal(CellState.Invalid, cut.Instance.GetCellState(data[0], "Name"));

        // Revert to original — should be pristine, not invalid.
        cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", original));

        Assert.Equal(CellState.Pristine, cut.Instance.GetCellState(data[0], "Name"));
        Assert.Null(cut.Instance.GetCellError(data[0], "Name"));
        Assert.Empty(cut.Instance.GetDirtyRows());
    }
}
