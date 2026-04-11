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
    public async Task CheckboxRequired_FalseValue_ProducesError()
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

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "IsActive", false));

        Assert.Equal(CellState.Invalid, cut.Instance.GetCellState(data[0], "IsActive"));
        var error = cut.Instance.GetCellError(data[0], "IsActive");
        Assert.NotNull(error);
        Assert.Contains("required", error!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CheckboxRequired_TrueValue_Passes()
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

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "IsActive", true));

        Assert.Null(cut.Instance.GetCellError(data[0], "IsActive"));
        Assert.NotEqual(CellState.Invalid, cut.Instance.GetCellState(data[0], "IsActive"));
    }

    [Fact]
    public async Task CheckboxNotRequired_FalseValue_Passes()
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

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "IsActive", false));

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
    public async Task CellRevert_RemovesFieldFromDirtySet()
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

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Changed"));
        Assert.NotEmpty(cut.Instance.GetDirtyRows());

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", original));

        Assert.Empty(cut.Instance.GetDirtyRows());
        Assert.Equal(CellState.Pristine, cut.Instance.GetCellState(data[0], "Name"));
    }

    [Fact]
    public async Task CellRevert_LeavesOtherDirtyFieldsIntact()
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
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Changed"));
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Amount", 999m));

        // Revert only Name.
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", originalName));

        // Row is still dirty because Amount remains modified.
        Assert.Single(cut.Instance.GetDirtyRows());
        Assert.Equal(CellState.Pristine, cut.Instance.GetCellState(data[0], "Name"));
        Assert.Equal(CellState.Dirty, cut.Instance.GetCellState(data[0], "Amount"));
    }

    [Fact]
    public async Task CellRevert_WithValidationError_StillTracksState()
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
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", ""));
        Assert.Equal(CellState.Invalid, cut.Instance.GetCellState(data[0], "Name"));

        // Revert to original — should be pristine, not invalid.
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", original));

        Assert.Equal(CellState.Pristine, cut.Instance.GetCellState(data[0], "Name"));
        Assert.Null(cut.Instance.GetCellError(data[0], "Name"));
        Assert.Empty(cut.Instance.GetDirtyRows());
    }

    // ─────────────────────────────────────────────────────────────────────
    // Batch F3 — Paste hardening (GAP-DATASHEET-V04.1/V04.2/V04.3/V04.4)
    // ─────────────────────────────────────────────────────────────────────

    // ── Fix V04.1: Normalize \r\n line endings during paste ────────────

    [Fact]
    public void Paste_WithCrlfLineEndings_ParsesNumberCellsCorrectly()
    {
        var data = SeedData();
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Amount");
                builder.AddAttribute(2, "Title", "Amount");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Number);
                builder.CloseComponent();
            }));

        // Activate first cell so PasteFromClipboard has an anchor.
        cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "Amount"));
        cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Escape", false, false));

        // TSV with Windows-style CRLF line endings — the trailing "\r" on the
        // last cell of each row would break decimal.TryParse without V04.1.
        cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("11.50\r\n22.75\r\n33.00"));

        Assert.Equal(11.50m, data[0].Amount);
        Assert.Equal(22.75m, data[1].Amount);
        Assert.Equal(33.00m, data[2].Amount);
        // No cells should be marked invalid.
        Assert.Null(cut.Instance.GetCellError(data[0], "Amount"));
        Assert.Null(cut.Instance.GetCellError(data[1], "Amount"));
        Assert.Null(cut.Instance.GetCellError(data[2], "Amount"));
    }

    [Fact]
    public void Paste_WithCrlfLineEndings_MultiColumn_LastCellNotCorrupted()
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

                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(10);
                builder.AddAttribute(11, "Field", "Amount");
                builder.AddAttribute(12, "Title", "Amount");
                builder.AddAttribute(13, "ColumnType", DataSheetColumnType.Number);
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "Name"));
        cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Escape", false, false));

        // Two rows, two columns, CRLF between rows. The numeric cell at the
        // end of each line must parse cleanly (no trailing "\r").
        cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("Row1\t10.5\r\nRow2\t20.5"));

        Assert.Equal("Row1", data[0].Name);
        Assert.Equal(10.5m, data[0].Amount);
        Assert.Equal("Row2", data[1].Name);
        Assert.Equal(20.5m, data[1].Amount);
    }

    // ── Fix V04.2: Parse failures do not write raw string to model ─────

    [Fact]
    public void Paste_InvalidNumber_MarksCellInvalid_AndPreservesOriginalValue()
    {
        var data = SeedData();
        var originalAmount = data[0].Amount; // 100m
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Amount");
                builder.AddAttribute(2, "Title", "Amount");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Number);
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "Amount"));
        cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Escape", false, false));

        cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("abc"));

        // The raw string was NOT written to the model.
        Assert.Equal(originalAmount, data[0].Amount);
        // The cell is marked invalid with the spec-required message.
        Assert.Equal(CellState.Invalid, cut.Instance.GetCellState(data[0], "Amount"));
        var error = cut.Instance.GetCellError(data[0], "Amount");
        Assert.NotNull(error);
        Assert.Contains("Invalid number", error!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Paste_InvalidDate_MarksCellInvalid_AndPreservesOriginalValue()
    {
        var data = new List<DateRow>
        {
            new() { Name = "A", When = new DateTime(2025, 1, 1) },
            new() { Name = "B", When = new DateTime(2025, 2, 1) },
        };

        var cut = Render<MariloDataSheet<DateRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<DateRow>>(0);
                builder.AddAttribute(1, "Field", "When");
                builder.AddAttribute(2, "Title", "When");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Date);
                builder.CloseComponent();
            }));

        var originalDate = data[0].When;

        cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "When"));
        cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Escape", false, false));

        cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("not-a-date"));

        Assert.Equal(originalDate, data[0].When);
        Assert.Equal(CellState.Invalid, cut.Instance.GetCellState(data[0], "When"));
        var error = cut.Instance.GetCellError(data[0], "When");
        Assert.NotNull(error);
        Assert.Contains("Invalid date", error!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Paste_MixedValidAndInvalid_CommitsValidAndMarksInvalid()
    {
        var data = SeedData();
        var originalRow1Amount = data[1].Amount; // 200m

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Amount");
                builder.AddAttribute(2, "Title", "Amount");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Number);
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "Amount"));
        cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Escape", false, false));

        // Row 0 gets a valid number, row 1 gets garbage.
        cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("11.5\nbad"));

        Assert.Equal(11.5m, data[0].Amount); // committed
        Assert.Equal(originalRow1Amount, data[1].Amount); // preserved
        Assert.Equal(CellState.Dirty, cut.Instance.GetCellState(data[0], "Amount"));
        Assert.Equal(CellState.Invalid, cut.Instance.GetCellState(data[1], "Amount"));
    }

    // ── Fix V04.3: Paste skips rows marked for deletion ────────────────

    [Fact]
    public void Paste_SkipsDeletedRow_AdvancesToNextLiveRow()
    {
        var data = new List<TestRow>
        {
            new() { Name = "Row0", Amount = 0m, IsActive = true },
            new() { Name = "Row1", Amount = 0m, IsActive = true },
            new() { Name = "Row2", Amount = 0m, IsActive = true },
            new() { Name = "Row3", Amount = 0m, IsActive = true },
        };

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.AllowDeleteRow, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        // Mark row index 1 for deletion.
        cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(data[1]));

        // Anchor paste at row 0.
        cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "Name"));
        cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Escape", false, false));

        // 3 TSV rows. Expected mapping: A→Row0, B→Row2 (skipping deleted Row1), C→Row3.
        cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("A\nB\nC"));

        Assert.Equal("A", data[0].Name);
        Assert.Equal("Row1", data[1].Name); // unchanged — still deleted, still original name
        Assert.Equal("B", data[2].Name);
        Assert.Equal("C", data[3].Name);
        Assert.True(cut.Instance.IsRowDeleted(data[1]));
    }

    // ── Fix V04.4: Copy honors Format via data-raw-value ───────────────

    [Fact]
    public void DataCell_HasDataRawValueAttribute_WithRawPropertyValue()
    {
        var data = new List<TestRow>
        {
            new() { Name = "Widget", Amount = 25.99m, IsActive = true }
        };

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Amount");
                builder.AddAttribute(2, "Title", "Amount");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Number);
                builder.AddAttribute(4, "Format", (Func<TestRow, string>)(r => r.Amount.ToString("C2", System.Globalization.CultureInfo.InvariantCulture)));
                builder.CloseComponent();
            }));

        var cells = cut.FindAll("td[data-field='Amount']");
        Assert.NotEmpty(cells);

        // Raw value attribute contains the untyped/unformatted property value.
        var rawAttr = cells[0].GetAttribute("data-raw-value");
        Assert.Equal("25.99", rawAttr);

        // Rendered text shows the Format-delegate output, not the raw value.
        Assert.Contains("¤25.99", cells[0].TextContent);
    }

    [Fact]
    public void ComputedCell_DoesNotEmitDataRawValue_DisplaysFormattedValue()
    {
        var data = new List<TestRow>
        {
            new() { Name = "Widget", Amount = 10m, IsActive = true }
        };

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Total");
                builder.AddAttribute(2, "Title", "Total");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Computed);
                builder.AddAttribute(4, "Format", (Func<TestRow, string>)(r => $"[{r.Total}]"));
                builder.CloseComponent();
            }));

        var cells = cut.FindAll("td[data-field='Total']");
        Assert.NotEmpty(cells);

        // Per spec (bulk-paste-and-clipboard.md:36), computed cells copy their
        // *displayed* (formatted) value. Since the JS copy handler falls back
        // to textContent when data-raw-value is absent, computed cells deliberately
        // omit the attribute — their visible text already IS the formatted value.
        Assert.Null(cells[0].GetAttribute("data-raw-value"));
        Assert.Contains("[20]", cells[0].TextContent);
    }

    [Fact]
    public void DataCell_WithoutFormatDelegate_OmitsDataRawValue()
    {
        var data = new List<TestRow>
        {
            new() { Name = "Alpha", Amount = 42m, IsActive = true }
        };

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

        var cells = cut.FindAll("td[data-field='Name']");
        Assert.NotEmpty(cells);

        // When there is no Format delegate, the cell's textContent already equals
        // the raw value, so the attribute is omitted to avoid per-cell DOM bloat.
        // JS copy handlers should read data-raw-value with a textContent fallback.
        Assert.Null(cells[0].GetAttribute("data-raw-value"));
        Assert.Contains("Alpha", cells[0].TextContent);
    }

    private record DateRow
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public DateTime When { get; set; }
    }

    // ── Additional Batch F3 coverage per spec ──────────────────────────

    [Fact]
    public void Paste_WithLoneCrLineEndings_NormalizesCorrectly()
    {
        // Old Mac / legacy TSV: single \r between rows.
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

                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(10);
                builder.AddAttribute(11, "Field", "Amount");
                builder.AddAttribute(12, "Title", "Amount");
                builder.AddAttribute(13, "ColumnType", DataSheetColumnType.Number);
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Name"));
        cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("A\t1\rB\t2"));

        Assert.Equal("A", data[0].Name);
        Assert.Equal(1m, data[0].Amount);
        Assert.Equal("B", data[1].Name);
        Assert.Equal(2m, data[1].Amount);
    }

    [Fact]
    public void Paste_ValidNumber_DoesNotProduceError()
    {
        // Regression guard: a valid number commits cleanly (no leftover
        // ValidationErrors entry from the V04.2 plumbing).
        var data = SeedData();
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Amount");
                builder.AddAttribute(2, "Title", "Amount");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Number);
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Amount"));
        cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("42"));

        Assert.Equal(42m, data[0].Amount);
        Assert.Null(cut.Instance.GetCellError(data[0], "Amount"));
        Assert.NotEqual(CellState.Invalid, cut.Instance.GetCellState(data[0], "Amount"));
    }

    [Fact]
    public void Paste_SelectValueNotInOptions_SetsInvalidCellStateWithMessage()
    {
        var data = new List<SelectRow>
        {
            new() { Name = "Row0", Status = "new" }
        };
        var options = new List<DataSheetSelectOption>
        {
            new() { Value = "new", Label = "New" },
            new() { Value = "active", Label = "Active" },
            new() { Value = "done", Label = "Done" },
        };

        var cut = Render<MariloDataSheet<SelectRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<SelectRow>>(0);
                builder.AddAttribute(1, "Field", "Status");
                builder.AddAttribute(2, "Title", "Status");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Select);
                builder.AddAttribute(4, "Options", (IEnumerable<DataSheetSelectOption>)options);
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Status"));
        cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("bogus"));

        // Raw string not written to the model.
        Assert.Equal("new", data[0].Status);
        Assert.Equal(CellState.Invalid, cut.Instance.GetCellState(data[0], "Status"));
        var error = cut.Instance.GetCellError(data[0], "Status");
        Assert.NotNull(error);
        Assert.Contains("Value not in options", error!);
    }

    [Fact]
    public void Paste_AllDeletedRows_NoOp()
    {
        var data = new List<TestRow>
        {
            new() { Name = "Row0", Amount = 1m, IsActive = true },
            new() { Name = "Row1", Amount = 2m, IsActive = true },
        };

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.AllowDeleteRow, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(data[0]));
        cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(data[1]));

        cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Name"));
        cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("A\nB"));

        // Both rows untouched — the row cursor walks off the end without
        // landing on any live row.
        Assert.Equal("Row0", data[0].Name);
        Assert.Equal("Row1", data[1].Name);
    }

    // ─────────────────────────────────────────────────────────────────────
    // Batch F2 — Save/Reset lifecycle (GAP-DATASHEET-V02.2/V05.1..V05.5)
    // ─────────────────────────────────────────────────────────────────────

    // ── Fix V02.2 / V05.1: CellState.Saving / CellState.Saved transitions ─

    [Fact]
    public async Task SaveAll_DirtyCells_TransitionThroughSavingAndSaved()
    {
        var data = SeedData();
        var onSaveAllObservedState = CellState.Pristine;
        Bunit.IRenderedComponent<MariloDataSheet<TestRow>>? cutRef = null;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.OnSaveAll, (DataSheetSaveArgs<TestRow> args) =>
            {
                // While OnSaveAll is in flight, the cell should be Saving.
                onSaveAllObservedState = cutRef!.Instance.GetCellState(data[0], "Name");
            })
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));
        cutRef = cut;

        // Shrink the Saved visual-indicator duration so the test runs fast.
        cut.Instance._savedStateDurationMs = 0;

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Edited"));
        Assert.Equal(CellState.Dirty, cut.Instance.GetCellState(data[0], "Name"));

        await cut.InvokeAsync(() => cut.Instance.SaveAllAsync());

        // OnSaveAll must have observed the cell as Saving.
        Assert.Equal(CellState.Saving, onSaveAllObservedState);

        // After Save All completes and the brief Saved window elapses,
        // the cell transitions to Pristine.
        Assert.Equal(CellState.Pristine, cut.Instance.GetCellState(data[0], "Name"));
    }

    [Fact]
    public async Task SaveAll_NonDirtyFields_StayPristineDuringSave()
    {
        var data = SeedData();
        var amountStateDuringSave = CellState.Pristine;
        var nameStateDuringSave = CellState.Pristine;
        var onSaveAllFired = false;
        Bunit.IRenderedComponent<MariloDataSheet<TestRow>>? cutRef = null;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.OnSaveAll, (DataSheetSaveArgs<TestRow> args) =>
            {
                onSaveAllFired = true;
                nameStateDuringSave = cutRef!.Instance.GetCellState(data[0], "Name");
                amountStateDuringSave = cutRef!.Instance.GetCellState(data[0], "Amount");
            })
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
        cutRef = cut;
        cut.Instance._savedStateDurationMs = 0;

        // Only Name is dirty on row[0]. Amount should not flip to Saving.
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Edited"));
        await cut.InvokeAsync(() => cut.Instance.SaveAllAsync());

        Assert.True(onSaveAllFired);
        Assert.Equal(CellState.Saving, nameStateDuringSave);
        Assert.Equal(CellState.Pristine, amountStateDuringSave);
    }

    // ── Fix V05.2: Deleted rows removed from _displayRows after save ────

    [Fact]
    public async Task SaveAll_DeletedRows_AreRemovedFromDisplayRowsAfterSuccess()
    {
        var data = SeedData();
        var captured = new List<TestRow>();

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.AllowDeleteRow, true)
            .Add(x => x.OnSaveAll, (DataSheetSaveArgs<TestRow> args) =>
            {
                captured.AddRange(args.DeletedRows);
            })
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        cut.Instance._savedStateDurationMs = 0;

        var targetRow = data[1];
        await cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(targetRow));
        Assert.True(cut.Instance.IsRowDeleted(targetRow));
        Assert.Contains(targetRow, cut.Instance._displayRows);

        await cut.InvokeAsync(() => cut.Instance.SaveAllAsync());

        Assert.Single(captured);
        Assert.Same(targetRow, captured[0]);
        Assert.DoesNotContain(targetRow, cut.Instance._displayRows);
    }

    [Fact]
    public async Task SaveAll_UpdatesOriginalSnapshot_SoRevertToPreSaveValueIsDetectedAsDirty()
    {
        var data = SeedData();

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.OnSaveAll, (DataSheetSaveArgs<TestRow> _) => { })
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        cut.Instance._savedStateDurationMs = 0;

        // "Alpha" -> "Beta", save. Then edit back to "Alpha" — which was the
        // pre-save original. Without V05.2's snapshot update this would
        // incorrectly register as "reverted to original" and drop dirty.
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Beta"));
        await cut.InvokeAsync(() => cut.Instance.SaveAllAsync());
        Assert.Equal(CellState.Pristine, cut.Instance.GetCellState(data[0], "Name"));

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Alpha"));
        Assert.Equal(CellState.Dirty, cut.Instance.GetCellState(data[0], "Name"));
    }

    // ── Fix V05.3: ResetAsync removes newly-added rows ─────────────────

    [Fact]
    public async Task AddRow_IsIncludedInDirtyRows()
    {
        var data = SeedData();

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.AllowAddRow, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        var initialDirty = cut.Instance.GetDirtyRows().Count;
        Assert.Equal(0, initialDirty);

        await cut.InvokeAsync(() => cut.Instance.AddRowAsync());

        var dirty = cut.Instance.GetDirtyRows();
        Assert.Single(dirty);
    }

    [Fact]
    public async Task ResetAsync_RemovesNewlyAddedRowsFromDisplayRows()
    {
        var data = SeedData();
        var originalCount = data.Count;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.AllowAddRow, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.AddRowAsync());
        await cut.InvokeAsync(() => cut.Instance.AddRowAsync());
        Assert.Equal(originalCount + 2, cut.Instance._displayRows.Count);

        await cut.InvokeAsync(() => cut.Instance.ResetAsync());

        Assert.Equal(originalCount, cut.Instance._displayRows.Count);
        Assert.Empty(cut.Instance.GetDirtyRows());
    }

    [Fact]
    public async Task ResetAsync_RevertsEditedRows_AndAlsoRemovesNewlyAddedRows()
    {
        var data = SeedData();
        var originalCount = data.Count;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.AllowAddRow, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Edited"));
        await cut.InvokeAsync(() => cut.Instance.AddRowAsync());

        Assert.Equal("Edited", data[0].Name);
        Assert.Equal(originalCount + 1, cut.Instance._displayRows.Count);

        await cut.InvokeAsync(() => cut.Instance.ResetAsync());

        Assert.Equal("Alpha", data[0].Name);
        Assert.Equal(originalCount, cut.Instance._displayRows.Count);
        Assert.Empty(cut.Instance.GetDirtyRows());
    }

    // ── Fix V05.4: Delete toggle (un-delete on re-click) ────────────────

    [Fact]
    public async Task MarkRowDeleted_CalledTwice_UndeletesTheRow()
    {
        var data = SeedData();

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.AllowDeleteRow, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(data[0]));
        Assert.True(cut.Instance.IsRowDeleted(data[0]));

        await cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(data[0]));
        Assert.False(cut.Instance.IsRowDeleted(data[0]));
    }

    [Fact]
    public async Task MarkRowDeleted_Untoggle_PreservesDirtyFieldEdits()
    {
        var data = SeedData();

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.AllowDeleteRow, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        // Edit, then delete, then undelete — the edit should survive.
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Edited"));
        await cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(data[0]));
        await cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(data[0]));

        Assert.False(cut.Instance.IsRowDeleted(data[0]));
        Assert.Equal("Edited", data[0].Name);
        Assert.Equal(CellState.Dirty, cut.Instance.GetCellState(data[0], "Name"));
    }

    [Fact]
    public async Task MarkRowDeleted_UntoggleWithoutEdits_ReturnsRowToPristine()
    {
        var data = SeedData();

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.AllowDeleteRow, true)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(data[0]));
        await cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(data[0]));

        Assert.False(cut.Instance.IsRowDeleted(data[0]));
        Assert.False(cut.Instance.IsRowDirty(data[0]));
        Assert.Equal(CellState.Pristine, cut.Instance.GetCellState(data[0], "Name"));
    }

    // ── Fix V05.5: BulkResetAsync restores original field values ────────

    [Fact]
    public async Task BulkResetAsync_RestoresFieldValuesOnSelectedRows()
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

                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(10);
                builder.AddAttribute(11, "Field", "Amount");
                builder.AddAttribute(12, "Title", "Amount");
                builder.AddAttribute(13, "ColumnType", DataSheetColumnType.Number);
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Changed"));
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Amount", 999m));
        Assert.Equal("Changed", data[0].Name);
        Assert.Equal(999m, data[0].Amount);

        cut.Instance._selectedRows.Add(data[0]);
        await cut.InvokeAsync(() => cut.Instance.BulkResetAsync());

        Assert.Equal("Alpha", data[0].Name);
        Assert.Equal(100m, data[0].Amount);
        Assert.Empty(cut.Instance.GetDirtyRows());
    }

    [Fact]
    public async Task BulkResetAsync_OnlyAffectsSelectedRows()
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

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Edit0"));
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[1], "Name", "Edit1"));

        cut.Instance._selectedRows.Add(data[0]);
        await cut.InvokeAsync(() => cut.Instance.BulkResetAsync());

        Assert.Equal("Alpha", data[0].Name);
        Assert.Equal("Edit1", data[1].Name);
        Assert.Single(cut.Instance.GetDirtyRows());
    }

    private record SelectRow
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string Status { get; set; } = "";
    }

    // ── Polish: DateTime paste round-trip under non-en-US culture ──────
    // Regression guard for the latent bug where V04.4 emits dates via
    // InvariantCulture into data-raw-value but the paste path parsed with
    // CurrentCulture. On de-DE this broke copy/paste round-trip.
    [Fact]
    public async Task Paste_DateInInvariantCulture_ParsesRegardlessOfCurrentCulture()
    {
        var original = System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo.CurrentCulture =
                new System.Globalization.CultureInfo("de-DE");

            var data = new List<DateRow>
            {
                new() { Name = "A", When = new DateTime(2025, 1, 1) },
                new() { Name = "B", When = new DateTime(2025, 2, 1) },
            };

            var cut = Render<MariloDataSheet<DateRow>>(p => p
                .Add(x => x.Data, data)
                .Add(x => x.EnableVirtualization, false)
                .Add(x => x.ChildContent, builder =>
                {
                    builder.OpenComponent<MariloDataSheetColumn<DateRow>>(0);
                    builder.AddAttribute(1, "Field", "When");
                    builder.AddAttribute(2, "Title", "When");
                    builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Date);
                    builder.CloseComponent();
                }));

            await cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "When"));
            await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Escape", false, false));

            // InvariantCulture m/d/y — would be rejected by de-DE d.m.y parser.
            await cut.InvokeAsync(() => cut.Instance.PasteFromClipboard("4/10/2026"));

            Assert.Equal(new DateTime(2026, 4, 10), data[0].When);
            Assert.Equal(CellState.Dirty, cut.Instance.GetCellState(data[0], "When"));
            Assert.Null(cut.Instance.GetCellError(data[0], "When"));
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentCulture = original;
        }
    }
}
