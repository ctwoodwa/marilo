using System.Globalization;
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

    // F1.N8 — Single-column checkbox sheet helper used by the three V01.1
    // Required tests below. Each test provides its own `data` list so the
    // helper does not impose any initial IsActive value.
    private IRenderedComponent<MariloDataSheet<TestRow>> RenderCheckboxSheet(
        bool required, List<TestRow> data)
    {
        return Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "IsActive");
                builder.AddAttribute(2, "Title", "Active");
                builder.AddAttribute(3, "ColumnType", DataSheetColumnType.Checkbox);
                builder.AddAttribute(4, "Required", required);
                builder.CloseComponent();
            }));
    }

    [Fact]
    public async Task CheckboxRequired_FalseValue_ProducesError()
    {
        var data = SeedData();
        data[0].IsActive = false;

        var cut = RenderCheckboxSheet(required: true, data);

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

        var cut = RenderCheckboxSheet(required: true, data);

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "IsActive", true));

        Assert.Null(cut.Instance.GetCellError(data[0], "IsActive"));
        Assert.NotEqual(CellState.Invalid, cut.Instance.GetCellState(data[0], "IsActive"));
    }

    [Fact]
    public async Task CheckboxNotRequired_FalseValue_Passes()
    {
        var data = SeedData();
        data[0].IsActive = true;

        var cut = RenderCheckboxSheet(required: false, data);

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

        // F3.N6 — ActivateCell anchors the paste cursor without the
        // EnterEditMode+Escape pair the first V04.1 iteration used.
        cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Amount"));

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

        // F3.N6 — ActivateCell anchors the paste cursor.
        cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Name"));

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

        // F3.N6 — ActivateCell anchors the paste cursor.
        cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Amount"));

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

        // F3.N6 — ActivateCell anchors the paste cursor.
        cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "When"));

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

        // F3.N6 — ActivateCell anchors the paste cursor.
        cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Amount"));

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

        // Anchor paste at row 0. F3.N6 — ActivateCell is cleaner than the
        // EnterEditMode+Escape pair the original V04.3 iteration used.
        cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Name"));

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
                builder.AddAttribute(4, "Format", (Func<TestRow, string>)(r => r.Amount.ToString("C2", CultureInfo.InvariantCulture)));
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

    // ── Fix V02.2 / V05.1: OnSaveAll exception rolls back transient state ─
    // Regression guard: if the consumer OnSaveAll handler throws, every
    // entry flipped to CellState.Saving must be rolled back to Dirty so the
    // grid is editable again and the user can retry. Before the fix the
    // exception left cells stuck in Saving forever with no recovery path.
    [Fact]
    public async Task SaveAll_OnSaveAllThrows_ClearsTransientStateAndRethrows()
    {
        var data = SeedData();

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.OnSaveAll, (DataSheetSaveArgs<TestRow> args) =>
            {
                throw new InvalidOperationException("boom");
            })
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        cut.Instance._savedStateDurationMs = 0;

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Edited"));
        Assert.Equal(CellState.Dirty, cut.Instance.GetCellState(data[0], "Name"));

        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await cut.InvokeAsync(() => cut.Instance.SaveAllAsync()));

        // After the exception the cell must be Dirty (not stuck in Saving)
        // and the row must still appear in GetDirtyRows() so the user can
        // retry Save All.
        Assert.Equal(CellState.Dirty, cut.Instance.GetCellState(data[0], "Name"));
        Assert.Single(cut.Instance.GetDirtyRows());
    }

    // ── Fix V02.2 / V05.1: Re-entrancy guard on SaveAllAsync ─────────────
    // Regression guard: a second SaveAllAsync call that lands during the
    // Saved-indicator Task.Delay window of the first call must be a no-op.
    // Before the guard, the first call's Step 7 cleanup would _dirtyRows.
    // Remove(key) rows that the second call had just re-dirtied, silently
    // losing edits.
    [Fact]
    public async Task SaveAll_ReentrantCall_IsNoOp()
    {
        var data = SeedData();
        var onSaveAllCallCount = 0;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.OnSaveAll, (DataSheetSaveArgs<TestRow> args) =>
            {
                onSaveAllCallCount++;
            })
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        // Keep the Saved-indicator window open long enough to start a second
        // SaveAllAsync call while the first is still parked in Task.Delay.
        cut.Instance._savedStateDurationMs = 200;

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Edited"));

        Task? firstCall = null;
        await cut.InvokeAsync(() =>
        {
            // Kick off the first SaveAllAsync without awaiting it so we can
            // observe the _isSaving window from the caller's perspective.
            firstCall = cut.Instance.SaveAllAsync();
        });

        // Second call lands while the first is still inside Task.Delay.
        // The re-entrancy guard must short-circuit it before OnSaveAll fires.
        await cut.InvokeAsync(() => cut.Instance.SaveAllAsync());

        // Let the first call finish its Saved-indicator window.
        if (firstCall != null)
        {
            await cut.InvokeAsync(() => firstCall);
        }

        // OnSaveAll was invoked exactly once — the second call was a no-op.
        Assert.Equal(1, onSaveAllCallCount);
        // The first call completed successfully, so the row is clean.
        Assert.Empty(cut.Instance.GetDirtyRows());
    }

    // ── Fix V02.2 / V05.1: BulkResetAsync guarded while save in flight ───
    // Regression guard: BulkResetAsync mutates the same _dirtyRows
    // dictionary that SaveAllAsync walks during its Step 7 cleanup. If a
    // user clicks Reset Selected during the Saved-indicator window the
    // reset must drop silently — stomping on rows that are mid-save would
    // corrupt the dirty-tracking state.
    [Fact]
    public async Task BulkResetAsync_WhileSaveInFlight_IsNoOp()
    {
        var data = SeedData();

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.OnSaveAll, (DataSheetSaveArgs<TestRow> args) => { })
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        // Non-zero Saved-indicator window so BulkResetAsync can land while
        // SaveAllAsync is parked in Task.Delay.
        cut.Instance._savedStateDurationMs = 200;

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Edited"));
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[1], "Name", "AlsoEdited"));
        cut.Instance._selectedRows.Add(data[1]);

        Task? saveCall = null;
        await cut.InvokeAsync(() =>
        {
            saveCall = cut.Instance.SaveAllAsync();
        });

        // BulkResetAsync lands while SaveAllAsync is in its Task.Delay window.
        // The guard must drop the reset without touching _selectedRows or
        // _dirtyRows so the save completes cleanly.
        await cut.InvokeAsync(() => cut.Instance.BulkResetAsync());

        // The reset should have been a no-op: row[1] stays selected and its
        // edited value survives the reset attempt.
        Assert.Contains(data[1], cut.Instance._selectedRows);
        Assert.Equal("AlsoEdited", data[1].Name);

        // Let the save complete.
        if (saveCall != null)
        {
            await cut.InvokeAsync(() => saveCall);
        }

        // After save completes both rows are clean (BulkResetAsync was a
        // no-op so it did not interfere with the Step 7 cleanup).
        Assert.Empty(cut.Instance.GetDirtyRows());
        Assert.Equal("Edited", data[0].Name);
        Assert.Equal("AlsoEdited", data[1].Name);
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

    // ── Regression: Iteration 8 — ResetAsync-in-OnSaveAll footgun ────────
    // The Overview demo used to call ResetAsync() from inside its OnSaveAll
    // handler. Because SaveAllAsync awaits OnSaveAll BEFORE running its
    // Step 6 cleanup (which removes deleted rows from _displayRows), the
    // handler's ResetAsync call cleared _dirtyRows early — so Step 6
    // recomputed deletedKeys from an empty dictionary and silently dropped
    // the pending deletion. This test locks in the correct post-save state
    // when a well-behaved handler simply returns. See
    // datasheet-delivery workspace status iteration 8 and the warning
    // XML doc on SaveAllAsync.
    [Fact]
    public async Task SaveAll_WithDeletedRows_RemovesThemFromDisplayRowsAfterHandler()
    {
        var data = SeedData();
        var handlerInvoked = false;

        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.AllowDeleteRow, true)
            .Add(x => x.OnSaveAll, (DataSheetSaveArgs<TestRow> _) =>
            {
                // Well-behaved handler: no state mutation, no ResetAsync.
                handlerInvoked = true;
            })
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        cut.Instance._savedStateDurationMs = 0;

        var alpha = data[0];
        var beta = data[1];
        var gamma = data[2];

        await cut.InvokeAsync(() => cut.Instance.MarkRowDeleted(beta));
        Assert.True(cut.Instance.IsRowDeleted(beta));
        Assert.Equal(3, cut.Instance._displayRows.Count);

        await cut.InvokeAsync(() => cut.Instance.SaveAllAsync());

        Assert.True(handlerInvoked);

        // Step 6 cleanup must have removed the deleted row from _displayRows.
        Assert.DoesNotContain(beta, cut.Instance._displayRows);

        // Remaining rows are the exact correct ones, in order.
        Assert.Equal(2, cut.Instance._displayRows.Count);
        Assert.Same(alpha, cut.Instance._displayRows[0]);
        Assert.Same(gamma, cut.Instance._displayRows[1]);

        // Step 7 cleanup must have cleared all dirty tracking.
        Assert.Empty(cut.Instance.GetDirtyRows());
        Assert.False(cut.Instance.IsRowDeleted(beta));
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
        // P.N2 — Deliberately use ONLY CurrentCulture (thread-local) for
        // the swap. Setting DefaultThreadCurrentCulture here was attempted
        // in the F4-polish batch but leaks into other test classes running
        // in parallel (DatePickerTests hit a de-DE month string). xUnit's
        // default in-assembly parallelism keeps same-class tests serialised,
        // so scoping the culture override to CurrentCulture is sufficient
        // for this single-class regression guard.
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

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
            CultureInfo.CurrentCulture = original;
        }
    }

    // ─────────────────────────────────────────────────────────────────────
    // Batch F4 — Keyboard and accessibility (GAP-DATASHEET-V07.1-9 + Tab wrap)
    // ─────────────────────────────────────────────────────────────────────

    // ── V07.1: Enter key enters edit mode when not already editing ────

    [Fact]
    public async Task EnterKey_OnActiveNonEditingCell_EntersEditMode()
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

        // Activate a cell but don't enter edit mode.
        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Name"));
        Assert.False(cut.Instance.IsCellEditing(data[0], "Name"));

        // Enter key should enter edit mode (V07.1).
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Enter", false, false));

        Assert.True(cut.Instance.IsCellEditing(data[0], "Name"));
    }

    [Fact]
    public async Task EnterKey_InEditMode_CommitsAndMovesDown()
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

        await cut.InvokeAsync(() => cut.Instance.EnterEditMode(data[0], "Name"));
        Assert.True(cut.Instance.IsCellEditing(data[0], "Name"));

        // Enter in edit mode commits (exits edit mode) and moves down.
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Enter", false, false));

        Assert.False(cut.Instance.IsCellEditing(data[0], "Name"));
        Assert.True(cut.Instance.IsCellActive(data[1], "Name"));
    }

    // ── V07.3: Space toggles checkbox without edit mode ────────────────

    [Fact]
    public async Task Space_OnCheckboxCell_TogglesValue()
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
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "IsActive"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown(" ", false, false));

        Assert.True(data[0].IsActive);
        Assert.Equal(CellState.Dirty, cut.Instance.GetCellState(data[0], "IsActive"));
    }

    [Fact]
    public async Task Space_OnCheckboxCell_TogglesBackAndForth()
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
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "IsActive"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown(" ", false, false));
        Assert.False(data[0].IsActive);

        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown(" ", false, false));
        Assert.True(data[0].IsActive);
    }

    [Fact]
    public async Task Space_OnNonCheckboxCell_DoesNotToggle()
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
            }));

        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Name"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown(" ", false, false));

        // Space on a text cell should not commit anything; value stays.
        Assert.Equal(originalName, data[0].Name);
    }

    // ── V07.5 / V07.6: aria-rowindex / aria-colindex ───────────────────

    [Fact]
    public void DataRows_HaveAriaRowIndex_StartingAtTwo()
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

        var dataRows = cut.FindAll("tbody tr[role='row']");
        Assert.Equal(3, dataRows.Count);
        // Header row is aria-rowindex=1, so first data row is 2.
        Assert.Equal("2", dataRows[0].GetAttribute("aria-rowindex"));
        Assert.Equal("3", dataRows[1].GetAttribute("aria-rowindex"));
        Assert.Equal("4", dataRows[2].GetAttribute("aria-rowindex"));
    }

    [Fact]
    public void HeaderRow_HasAriaRowIndexOne()
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

        var header = cut.Find("thead tr[role='row']");
        Assert.Equal("1", header.GetAttribute("aria-rowindex"));
    }

    [Fact]
    public void DataCells_HaveAriaColIndex_StartingAtOne()
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

        var firstDataRow = cut.FindAll("tbody tr[role='row']").First();
        var cells = firstDataRow.QuerySelectorAll("td[role='gridcell']");
        Assert.Equal(2, cells.Length);
        Assert.Equal("1", cells[0].GetAttribute("aria-colindex"));
        Assert.Equal("2", cells[1].GetAttribute("aria-colindex"));
    }

    // ── V07.7: aria-describedby on invalid cells ───────────────────────

    [Fact]
    public async Task InvalidCell_HasAriaDescribedBy_PointingToErrorText()
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
                builder.AddAttribute(3, "Required", true);
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", ""));
        Assert.Equal(CellState.Invalid, cut.Instance.GetCellState(data[0], "Name"));

        var invalidCell = cut.Find("td[aria-invalid='true']");
        var describedBy = invalidCell.GetAttribute("aria-describedby");
        Assert.NotNull(describedBy);

        var errorSpan = cut.Find($"#{describedBy}");
        Assert.NotNull(errorSpan);
        Assert.Contains("required", errorSpan.TextContent, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ValidCell_HasNoAriaDescribedBy()
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

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Changed"));
        var cell = cut.Find("td[data-field='Name']");
        Assert.Null(cell.GetAttribute("aria-describedby"));
    }

    private class KeyedTestRow
    {
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
    }

    [Fact]
    public async Task InvalidCell_WithRowKeyContainingSpecialChars_ProducesValidDescribedbyId()
    {
        // V07.7 polish — rowKey.ToString() may contain whitespace, quotes,
        // `#`, or other characters that are invalid in an HTML id attribute
        // or CSS id selector. The sanitizer must replace those with '_' so
        // the cell's aria-describedby and the error span's id match exactly
        // and form a valid id selector.
        var data = new List<KeyedTestRow>
        {
            new() { Key = "abc def \"#%\"", Name = "Alpha" },
        };

        var cut = Render<MariloDataSheet<KeyedTestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.KeyField, "Key")
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<KeyedTestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.AddAttribute(3, "Required", true);
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", ""));
        Assert.Equal(CellState.Invalid, cut.Instance.GetCellState(data[0], "Name"));

        var invalidCell = cut.Find("td[aria-invalid='true']");
        var describedBy = invalidCell.GetAttribute("aria-describedby");
        Assert.NotNull(describedBy);

        // The id must contain ONLY [A-Za-z0-9_-] so it is valid in an HTML
        // id attribute and in a CSS id selector.
        foreach (var c in describedBy!)
        {
            var isSafe = (c >= 'A' && c <= 'Z')
                      || (c >= 'a' && c <= 'z')
                      || (c >= '0' && c <= '9')
                      || c == '_'
                      || c == '-';
            Assert.True(
                isSafe,
                $"aria-describedby contains unsafe char '{c}' (U+{(int)c:X4}) in value '{describedBy}'.");
        }

        // The error span's id must match the cell's aria-describedby
        // exactly so screen readers can locate the described element.
        var errorSpan = cut.Find($"#{describedBy}");
        Assert.NotNull(errorSpan);
        Assert.Contains("required", errorSpan.TextContent, StringComparison.OrdinalIgnoreCase);
    }

    // ── V07.8: aria-busy includes IsSaving ─────────────────────────────

    [Fact]
    public void AriaBusy_IsTrue_WhenIsSaving()
    {
        var data = SeedData();
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.IsSaving, true)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        var root = cut.Find("div[role='grid']");
        Assert.Equal("true", root.GetAttribute("aria-busy"));
    }

    [Fact]
    public void AriaBusy_IsTrue_WhenIsLoading()
    {
        var data = SeedData();
        var cut = Render<MariloDataSheet<TestRow>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.IsLoading, true)
            .Add(x => x.EnableVirtualization, false)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(0);
                builder.AddAttribute(1, "Field", "Name");
                builder.AddAttribute(2, "Title", "Name");
                builder.CloseComponent();
            }));

        var root = cut.Find("div[role='grid']");
        Assert.Equal("true", root.GetAttribute("aria-busy"));
    }

    [Fact]
    public void AriaBusy_IsNull_WhenIdle()
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

        var root = cut.Find("div[role='grid']");
        Assert.Null(root.GetAttribute("aria-busy"));
    }

    // ── V07.9: aria-live dirty count announcement ──────────────────────

    [Fact]
    public async Task DirtyCountChange_Announces_RowsModified()
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

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Changed1"));
        Assert.Equal("1 row modified", cut.Instance._ariaAnnouncement);

        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[1], "Name", "Changed2"));
        Assert.Equal("2 rows modified", cut.Instance._ariaAnnouncement);
    }

    [Fact]
    public async Task DirtyCountUnchanged_DoesNotReAnnounce()
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

        // First edit brings row count to 1 and fires an announcement.
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Name", "Changed"));
        Assert.Equal("1 row modified", cut.Instance._ariaAnnouncement);

        // Manually reset the announcement so we can detect a re-fire.
        cut.Instance._ariaAnnouncement = "SENTINEL";

        // Second edit on the SAME row doesn't change the dirty row count,
        // so the announcement should NOT re-fire.
        await cut.InvokeAsync(() => cut.Instance.CommitCellEdit(data[0], "Amount", 999m));
        Assert.Equal("SENTINEL", cut.Instance._ariaAnnouncement);
    }

    // ── V07.2: Printable character begins edit mode ────────────────────

    [Fact]
    public async Task PrintableChar_OnTextCell_EntersEditModeWithChar()
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

        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Name"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("x", false, false));

        Assert.True(cut.Instance.IsCellEditing(data[0], "Name"));
        Assert.Equal("x", data[0].Name);
    }

    [Fact]
    public async Task PrintableChar_OnNumberCell_EntersEditModeWithParsedValue()
    {
        var data = SeedData();
        data[0].Amount = 0m;

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

        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Amount"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("5", false, false));

        Assert.True(cut.Instance.IsCellEditing(data[0], "Amount"));
        Assert.Equal(5m, data[0].Amount);
    }

    [Fact]
    public async Task PrintableChar_OnComputedCell_DoesNothing()
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
                builder.CloseComponent();
            }));

        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Total"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("x", false, false));

        // Computed cells cannot enter edit mode.
        Assert.False(cut.Instance.IsCellEditing(data[0], "Total"));
    }

    // ── Tab row wrapping ───────────────────────────────────────────────

    [Fact]
    public async Task Tab_AtLastEditableColumnOfRow_WrapsToFirstColumnOfNextRow()
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

        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Amount"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Tab", false, false));

        Assert.True(cut.Instance.IsCellActive(data[1], "Name"));
    }

    [Fact]
    public async Task ShiftTab_AtFirstEditableColumnOfRow_WrapsToLastColumnOfPrevRow()
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

        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[1], "Name"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Tab", false, true));

        Assert.True(cut.Instance.IsCellActive(data[0], "Amount"));
    }

    [Fact]
    public async Task Tab_OnLastCellOfLastRow_ExitsGrid()
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

        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[2], "Amount"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Tab", false, false));

        // At the last editable cell of the last row, Tab clears the
        // active cell so browser focus leaves the grid.
        Assert.Null(cut.Instance._activeCellRow);
        Assert.Null(cut.Instance._activeCellField);
    }

    [Fact]
    public async Task ShiftTab_OnFirstCellOfFirstRow_ExitsGrid()
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

        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Name"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Tab", false, true));

        Assert.Null(cut.Instance._activeCellRow);
        Assert.Null(cut.Instance._activeCellField);
    }

    [Fact]
    public async Task Tab_SkipsComputedColumns()
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
                builder.AddAttribute(11, "Field", "Total");
                builder.AddAttribute(12, "Title", "Total");
                builder.AddAttribute(13, "ColumnType", DataSheetColumnType.Computed);
                builder.CloseComponent();

                builder.OpenComponent<MariloDataSheetColumn<TestRow>>(20);
                builder.AddAttribute(21, "Field", "Amount");
                builder.AddAttribute(22, "Title", "Amount");
                builder.AddAttribute(23, "ColumnType", DataSheetColumnType.Number);
                builder.CloseComponent();
            }));

        // Activate Name (editable) — Tab should skip Total (computed) and
        // land directly on Amount.
        await cut.InvokeAsync(() => cut.Instance.ActivateCell(data[0], "Name"));
        await cut.InvokeAsync(() => cut.Instance.HandleKeyDown("Tab", false, false));

        Assert.True(cut.Instance.IsCellActive(data[0], "Amount"));
    }
}
