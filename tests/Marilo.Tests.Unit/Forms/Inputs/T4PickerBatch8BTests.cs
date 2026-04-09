using Bunit;
using Marilo.Components.Forms.Inputs;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// Tests for T4 Pickers Batch 8B gap resolutions on MariloTimePicker:
/// - RES-T4B8B-001: InputMode parameter
/// - RES-T4B8B-002: ValidateOn parameter
/// - RES-T4B8B-003: OnChange fires on blur
/// - RES-T4B8B-004: CSS provider integration (TimePickerClass / TimePickerPopupClass)
/// </summary>
public class T4PickerBatch8BTests : MariloTestBase
{
    // ══════════════════════════════════════════════════════════════════
    // RES-T4B8B-001: InputMode parameter
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void TimePicker_InputMode_Defaults_To_Null()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();
        Assert.Null(cut.Instance.InputMode);
    }

    [Fact]
    public void TimePicker_InputMode_None_AppliedToInput()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.InputMode, "none"));

        var input = cut.Find("input");
        Assert.Equal("none", input.GetAttribute("inputmode"));
    }

    [Fact]
    public void TimePicker_InputMode_Text_AppliedToInput()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.InputMode, "text"));

        var input = cut.Find("input");
        Assert.Equal("text", input.GetAttribute("inputmode"));
    }

    [Fact]
    public void TimePicker_InputMode_Null_AttributeOmitted()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();
        var input = cut.Find("input");
        // When InputMode is null, the inputmode attribute should be absent or null
        var attr = input.GetAttribute("inputmode");
        Assert.True(attr is null || attr == string.Empty);
    }

    // ══════════════════════════════════════════════════════════════════
    // RES-T4B8B-002: ValidateOn parameter
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void TimePicker_ValidateOn_Defaults_To_Null()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();
        Assert.Null(cut.Instance.ValidateOn);
    }

    [Fact]
    public void TimePicker_ValidateOn_Can_Be_Set_To_Blur()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.ValidateOn, "blur"));
        Assert.Equal("blur", cut.Instance.ValidateOn);
    }

    [Fact]
    public void TimePicker_ValidateOn_Can_Be_Set_To_Change()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.ValidateOn, "change"));
        Assert.Equal("change", cut.Instance.ValidateOn);
    }

    [Fact]
    public void TimePicker_ValidateOn_Can_Be_Set_To_Input()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.ValidateOn, "input"));
        Assert.Equal("input", cut.Instance.ValidateOn);
    }

    // ══════════════════════════════════════════════════════════════════
    // RES-T4B8B-003: OnChange fires on blur
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void TimePicker_OnChange_Fires_OnBlur_WhenValueChanged()
    {
        object? capturedChange = null;
        var initialValue = new DateTime(1900, 1, 1, 9, 30, 0);

        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Value, initialValue)
            .Add(x => x.OnChange, (object? v) => { capturedChange = v; return Task.CompletedTask; }));

        // Open popup and commit a new time
        cut.Find(".mar-timepicker__toggle").Click();
        cut.Find(".mar-timepicker__btn--set").Click();

        // Reset capture so we can test blur-triggered OnChange
        // Now re-render with a different value without emitting OnChange (simulates external update)
        var newValue = new DateTime(1900, 1, 1, 10, 0, 0);
        capturedChange = "sentinel"; // mark as not yet triggered by blur

        cut.Render(p => p
            .Add(x => x.Value, newValue)
            .Add(x => x.OnChange, (object? v) => { capturedChange = v; return Task.CompletedTask; }));

        // Trigger blur — since Value != _lastEmittedValue, OnChange should fire
        cut.Find("input").Blur();

        Assert.NotEqual("sentinel", capturedChange);
    }

    [Fact]
    public void TimePicker_OnChange_Does_Not_Fire_OnBlur_WhenValueUnchanged()
    {
        var fireCount = 0;
        var value = new DateTime(1900, 1, 1, 9, 30, 0);

        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.Value, value)
            .Add(x => x.OnChange, (object? v) => { fireCount++; return Task.CompletedTask; }));

        // Open and commit same value — this fires OnChange once
        cut.Find(".mar-timepicker__toggle").Click();
        cut.Find(".mar-timepicker__btn--set").Click();

        var countAfterCommit = fireCount;

        // Blur immediately — value is the same as _lastEmittedValue, OnChange must NOT fire again
        cut.Find("input").Blur();

        Assert.Equal(countAfterCommit, fireCount);
    }

    [Fact]
    public void TimePicker_OnBlur_Always_Fires_Regardless_Of_ValueChange()
    {
        var blurCount = 0;

        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.OnBlur, Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, () => blurCount++)));

        cut.Find("input").Blur();

        Assert.Equal(1, blurCount);
    }

    // ══════════════════════════════════════════════════════════════════
    // RES-T4B8B-004: CSS provider integration
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void TimePicker_RootDiv_UsesProviderTimePickerClass()
    {
        var provider = Services.GetRequiredService<Marilo.Core.Contracts.IMariloCssProvider>();
        var expectedClass = provider.TimePickerClass();

        var cut = Render<MariloTimePicker<DateTime?>>();
        var root = cut.Find("div");

        Assert.Contains(expectedClass, root.GetAttribute("class") ?? string.Empty);
    }

    [Fact]
    public void TimePicker_Popup_UsesProviderTimePickerPopupClass()
    {
        var provider = Services.GetRequiredService<Marilo.Core.Contracts.IMariloCssProvider>();
        var expectedClass = provider.TimePickerPopupClass().Split(' ')[0]; // first token

        var cut = Render<MariloTimePicker<DateTime?>>();
        cut.Find(".mar-timepicker__toggle").Click();

        var popup = cut.Find("[role='dialog']");
        Assert.Contains(expectedClass, popup.GetAttribute("class") ?? string.Empty);
    }

    [Fact]
    public void CssProvider_TimePickerPopupClass_Returns_NonEmpty_String()
    {
        var provider = Services.GetRequiredService<Marilo.Core.Contracts.IMariloCssProvider>();
        var result = provider.TimePickerPopupClass();
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void CssProvider_TimePickerClass_Returns_NonEmpty_String()
    {
        var provider = Services.GetRequiredService<Marilo.Core.Contracts.IMariloCssProvider>();
        var result = provider.TimePickerClass();
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void TimePicker_PopupClass_StillApplied_AlongsideProviderClass()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.PopupClass, "custom-popup-class"));

        cut.Find(".mar-timepicker__toggle").Click();

        var popup = cut.Find("[role='dialog']");
        var classes = popup.GetAttribute("class") ?? string.Empty;
        Assert.Contains("custom-popup-class", classes);
    }
}
