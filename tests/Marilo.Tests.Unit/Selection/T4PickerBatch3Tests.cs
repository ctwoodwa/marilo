using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Marilo.Tests.Unit.Selection;

/// <summary>
/// Tests for T4 Pickers Batch 3 cross-cutting gaps:
/// AdaptiveMode parameter, ARIA combobox pattern, CSS provider methods.
/// </summary>
public class T4PickerBatch3Tests : MariloTestBase
{
    // ── AdaptiveMode Enum ──────────────────────────────────────────────

    [Fact]
    public void AdaptiveMode_Enum_HasExpectedValues()
    {
        Assert.Equal(0, (int)AdaptiveMode.None);
        Assert.Equal(1, (int)AdaptiveMode.Auto);
    }

    // ── AdaptiveMode: TimePicker ───────────────────────────────────────

    [Fact]
    public void TimePicker_AdaptiveMode_Defaults_To_None()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();
        Assert.Equal(AdaptiveMode.None, cut.Instance.AdaptiveMode);
    }

    [Fact]
    public void TimePicker_AdaptiveMode_Can_Be_Set_To_Auto()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.AdaptiveMode, AdaptiveMode.Auto));
        Assert.Equal(AdaptiveMode.Auto, cut.Instance.AdaptiveMode);
    }

    // ── AdaptiveMode: DateTimePicker ───────────────────────────────────

    [Fact]
    public void DateTimePicker_AdaptiveMode_Defaults_To_None()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, DateTime.Now));
        Assert.Equal(AdaptiveMode.None, cut.Instance.AdaptiveMode);
    }

    // ── AdaptiveMode: DateRangePicker ──────────────────────────────────

    [Fact]
    public void DateRangePicker_AdaptiveMode_Defaults_To_None()
    {
        var cut = Render<MariloDateRangePicker>();
        Assert.Equal(AdaptiveMode.None, cut.Instance.AdaptiveMode);
    }

    // ── AdaptiveMode: ColorPicker ──────────────────────────────────────

    [Fact]
    public void ColorPicker_AdaptiveMode_Defaults_To_None()
    {
        var cut = Render<MariloColorPicker>();
        Assert.Equal(AdaptiveMode.None, cut.Instance.AdaptiveMode);
    }

    // ── AdaptiveMode: MultiSelect ──────────────────────────────────────

    private record SelectItem(int Id, string Name);

    private static readonly List<SelectItem> _selectData = [new(1, "A"), new(2, "B")];

    [Fact]
    public void MultiSelect_AdaptiveMode_Defaults_To_None()
    {
        var cut = Render<MariloMultiSelect<SelectItem, int>>(p => p
            .Add(x => x.Data, _selectData)
            .Add(x => x.TextField, "Name")
            .Add(x => x.ValueField, "Id")
            .Add(x => x.Value, new List<int>()));
        Assert.Equal(AdaptiveMode.None, cut.Instance.AdaptiveMode);
    }

    // ── ARIA: TimePicker combobox ──────────────────────────────────────

    [Fact]
    public void TimePicker_Input_Has_Combobox_Role()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();
        var input = cut.Find("input");
        Assert.Equal("combobox", input.GetAttribute("role"));
    }

    [Fact]
    public void TimePicker_Input_Has_AriaHaspopup_Dialog()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();
        var input = cut.Find("input");
        Assert.Equal("dialog", input.GetAttribute("aria-haspopup"));
    }

    [Fact]
    public void TimePicker_Input_AriaExpanded_False_WhenClosed()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();
        var input = cut.Find("input");
        Assert.Equal("false", input.GetAttribute("aria-expanded"));
    }

    // ── ARIA: DateTimePicker combobox ──────────────────────────────────

    [Fact]
    public void DateTimePicker_Input_Has_Combobox_Role()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.Value, DateTime.Now));
        var input = cut.Find("input");
        Assert.Equal("combobox", input.GetAttribute("role"));
    }

    // ── ARIA: DateRangePicker combobox ─────────────────────────────────

    [Fact]
    public void DateRangePicker_StartInput_Has_Combobox_Role()
    {
        var cut = Render<MariloDateRangePicker>();
        var inputs = cut.FindAll("input[role='combobox']");
        Assert.True(inputs.Count >= 1, "At least start input should have combobox role");
    }

    [Fact]
    public void DateRangePicker_BothInputs_Have_Combobox_Role()
    {
        var cut = Render<MariloDateRangePicker>();
        var inputs = cut.FindAll("input[role='combobox']");
        Assert.Equal(2, inputs.Count);
    }

    // ── CSS Provider: DateRangePicker/DateTimePicker ────────────────────

    [Fact]
    public void CssProvider_DateRangePickerClass_Returns_String()
    {
        var provider = Services.GetService<Marilo.Core.Contracts.IMariloCssProvider>();
        Assert.NotNull(provider);
        var result = provider!.DateRangePickerClass();
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void CssProvider_DateTimePickerClass_Returns_String()
    {
        var provider = Services.GetService<Marilo.Core.Contracts.IMariloCssProvider>();
        Assert.NotNull(provider);
        var result = provider!.DateTimePickerClass();
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void CssProvider_DateRangePickerPopupClass_Returns_String()
    {
        var provider = Services.GetService<Marilo.Core.Contracts.IMariloCssProvider>();
        Assert.NotNull(provider);
        var result = provider!.DateRangePickerPopupClass();
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void CssProvider_DateTimePickerPopupClass_Returns_String()
    {
        var provider = Services.GetService<Marilo.Core.Contracts.IMariloCssProvider>();
        Assert.NotNull(provider);
        var result = provider!.DateTimePickerPopupClass();
        Assert.False(string.IsNullOrEmpty(result));
    }
}
