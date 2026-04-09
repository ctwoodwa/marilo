using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Selection;

/// <summary>
/// Tests for T4 Pickers Batch 8A gaps:
/// MariloDateRangePicker: PopupClass bug, ShowWeekNumbers, Size/Rounded/FillMode,
/// DebounceDelay/Title, HeaderTemplate.
/// MariloDateTimePicker: ValidateOn parameter.
/// </summary>
public class T4PickerBatch8ATests : MariloTestBase
{
    // ── RES-T4B8A-01: PopupClass bug fix ──────────────────────────────

    [Fact]
    public void DateRangePicker_PopupClass_IsAppliedToRootElement()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.PopupClass, "my-custom-popup"));

        // Open the popup
        var input = cut.Find("input");
        input.Click();

        // The popup panel should carry the custom class
        var popup = cut.Find(".mar-date-range-picker__popup");
        Assert.Contains("my-custom-popup", popup.ClassList);
    }

    [Fact]
    public void DateRangePicker_PopupClass_Null_DoesNotBreakRender()
    {
        // PopupClass is null by default — component should render without exception
        var cut = Render<MariloDateRangePicker>();
        Assert.NotNull(cut.Find(".mar-date-range-picker"));
    }

    // ── RES-T4B8A-02: ShowWeekNumbers ─────────────────────────────────

    [Fact]
    public void DateRangePicker_ShowWeekNumbers_False_RendersNoWeekColumn()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.ShowWeekNumbers, false));

        // Open popup
        cut.Find("input").Click();

        var weekNumbers = cut.FindAll(".mar-calendar__week-number");
        Assert.Empty(weekNumbers);

        var weekHeader = cut.FindAll(".mar-calendar__week-number-header");
        Assert.Empty(weekHeader);
    }

    [Fact]
    public void DateRangePicker_ShowWeekNumbers_True_RendersWeekNumberHeader()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.ShowWeekNumbers, true));

        // Open popup
        cut.Find("input").Click();

        // Both calendars should show a "Wk" header
        var weekHeaders = cut.FindAll(".mar-calendar__week-number-header");
        Assert.Equal(2, weekHeaders.Count);
        Assert.All(weekHeaders, h => Assert.Equal("Wk", h.TextContent.Trim()));
    }

    [Fact]
    public void DateRangePicker_ShowWeekNumbers_True_RendersWeekNumberCells()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.StartValue, new DateTime(2026, 1, 5))
            .Add(x => x.ShowWeekNumbers, true));

        // Open popup
        cut.Find("input").Click();

        // Each calendar has 6 rows = 6 week-number cells per calendar; 2 calendars = 12
        var weekCells = cut.FindAll(".mar-calendar__week-number");
        Assert.True(weekCells.Count >= 2, "Expected at least 2 week-number cells");

        // All displayed week numbers should be parseable positive integers
        foreach (var cell in weekCells)
        {
            var ok = int.TryParse(cell.TextContent.Trim(), out var wk);
            Assert.True(ok, $"Week number '{cell.TextContent.Trim()}' is not an integer");
            Assert.InRange(wk, 1, 53);
        }
    }

    [Fact]
    public void DateRangePicker_ShowWeekNumbers_Grid_HasWeekNumbersClass()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.ShowWeekNumbers, true));

        cut.Find("input").Click();

        var grids = cut.FindAll(".mar-calendar__grid--week-numbers");
        Assert.Equal(2, grids.Count);
    }

    // ── RES-T4B8A-03: Size / Rounded / FillMode ───────────────────────

    [Fact]
    public void DateRangePicker_Size_Null_AppliesDefaultMdClass()
    {
        var cut = Render<MariloDateRangePicker>();
        var root = cut.Find(".mar-date-range-picker");
        Assert.Contains("mar-date-range-picker--md", root.ClassList);
    }

    [Fact]
    public void DateRangePicker_Size_Sm_AppliesSmClass()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.Size, "sm"));
        var root = cut.Find(".mar-date-range-picker");
        Assert.Contains("mar-date-range-picker--sm", root.ClassList);
    }

    [Fact]
    public void DateRangePicker_Rounded_Pill_AppliesRoundedClass()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.Rounded, "pill"));
        var root = cut.Find(".mar-date-range-picker");
        Assert.Contains("mar-date-range-picker--rounded-pill", root.ClassList);
    }

    [Fact]
    public void DateRangePicker_Rounded_Null_EmitsNoRoundedClass()
    {
        var cut = Render<MariloDateRangePicker>();
        var root = cut.Find(".mar-date-range-picker");
        Assert.DoesNotContain(root.ClassList, c => c.StartsWith("mar-date-range-picker--rounded-"));
    }

    [Fact]
    public void DateRangePicker_FillMode_Null_AppliesDefaultSolidClass()
    {
        var cut = Render<MariloDateRangePicker>();
        var root = cut.Find(".mar-date-range-picker");
        Assert.Contains("mar-date-range-picker--solid", root.ClassList);
    }

    [Fact]
    public void DateRangePicker_FillMode_Flat_AppliesFlatClass()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.FillMode, "flat"));
        var root = cut.Find(".mar-date-range-picker");
        Assert.Contains("mar-date-range-picker--flat", root.ClassList);
    }

    // ── RES-T4B8A-04: DebounceDelay and Title ─────────────────────────

    [Fact]
    public void DateRangePicker_DebounceDelay_DefaultIs150()
    {
        var cut = Render<MariloDateRangePicker>();
        Assert.Equal(150, cut.Instance.DebounceDelay);
    }

    [Fact]
    public void DateRangePicker_DebounceDelay_CanBeSet()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.DebounceDelay, 300));
        Assert.Equal(300, cut.Instance.DebounceDelay);
    }

    [Fact]
    public void DateRangePicker_Title_Null_DoesNotRenderTitleDiv()
    {
        var cut = Render<MariloDateRangePicker>();
        cut.Find("input").Click();

        var titleDivs = cut.FindAll(".mar-date-range-picker__title");
        Assert.Empty(titleDivs);
    }

    [Fact]
    public void DateRangePicker_Title_Set_RendersInPopup()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.Title, "Select your vacation dates"));

        // Title is in the popup — open it
        cut.Find("input").Click();

        var titleDiv = cut.Find(".mar-date-range-picker__title");
        Assert.Contains("Select your vacation dates", titleDiv.TextContent);
    }

    // ── RES-T4B8A-05: HeaderTemplate ──────────────────────────────────

    [Fact]
    public void DateRangePicker_HeaderTemplate_Null_DoesNotRenderCustomHeader()
    {
        var cut = Render<MariloDateRangePicker>();
        cut.Find("input").Click();

        // No custom header element — only the per-calendar header
        var customHeaders = cut.FindAll("[data-test-header]");
        Assert.Empty(customHeaders);
    }

    [Fact]
    public void DateRangePicker_HeaderTemplate_Renders_WhenProvided()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.HeaderTemplate, (RenderFragment)(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "data-test-header", "true");
                builder.AddContent(2, "Custom Header");
                builder.CloseElement();
            })));

        cut.Find("input").Click();

        var customHeader = cut.Find("[data-test-header]");
        Assert.Contains("Custom Header", customHeader.TextContent);
    }

    [Fact]
    public void DateRangePicker_HeaderTemplate_TakesPrecedenceOverTitle()
    {
        // When HeaderTemplate is set, Title div should NOT be rendered
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.Title, "Should not show")
            .Add(x => x.HeaderTemplate, (RenderFragment)(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "data-test-header", "true");
                builder.AddContent(2, "Template wins");
                builder.CloseElement();
            })));

        cut.Find("input").Click();

        // Template is rendered
        Assert.NotNull(cut.Find("[data-test-header]"));

        // Title div should not be rendered because HeaderTemplate takes priority
        var titleDivs = cut.FindAll(".mar-date-range-picker__title");
        Assert.Empty(titleDivs);
    }

    // ── RES-T4B8A-06: ValidateOn — MariloDateTimePicker ───────────────

    [Fact]
    public void DateTimePicker_ValidateOn_Null_ByDefault()
    {
        var cut = Render<MariloDateTimePicker>();
        Assert.Null(cut.Instance.ValidateOn);
    }

    [Fact]
    public void DateTimePicker_ValidateOn_CanBeSetToBlur()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ValidateOn, "blur"));
        Assert.Equal("blur", cut.Instance.ValidateOn);
    }

    [Fact]
    public void DateTimePicker_ValidateOn_CanBeSetToChange()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ValidateOn, "change"));
        Assert.Equal("change", cut.Instance.ValidateOn);
    }

    [Fact]
    public void DateTimePicker_ValidateOn_CanBeSetToInput()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.ValidateOn, "input"));
        Assert.Equal("input", cut.Instance.ValidateOn);
    }
}
