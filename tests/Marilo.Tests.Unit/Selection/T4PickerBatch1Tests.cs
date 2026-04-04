using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Models;
using Xunit;

namespace Marilo.Tests.Unit.Selection;

/// <summary>
/// Tests for T4 Picker Batch 1 gap resolutions:
/// - PopupEventArgs cancellation (RES-T4B1-001)
/// - TimePicker PopupClass bug fix (RES-T4B1-005)
/// - MultiSelect OnOpen/OnClose/OnBlur (RES-T4B1-002)
/// - DateTimePicker OnOpen/OnClose/OnBlur/OnCalendarCellRender (RES-T4B1-003)
/// - DateRangePicker OnOpen/OnClose (RES-T4B1-004)
/// </summary>
public class T4PickerBatch1Tests : MariloTestBase
{
    // ══════════════════════════════════════════════════════════════════
    // RES-T4B1-005: TimePicker PopupClass bug fix
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void TimePicker_PopupClass_AppliedToPopupDiv()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.PopupClass, "my-custom-popup"));

        cut.Find(".mar-timepicker__toggle").Click();

        var popup = cut.Find("[role='dialog']");
        Assert.Contains("my-custom-popup", popup.GetAttribute("class"));
    }

    [Fact]
    public void TimePicker_PopupClass_Null_NoExtraClass()
    {
        var cut = Render<MariloTimePicker<DateTime?>>();

        cut.Find(".mar-timepicker__toggle").Click();

        var popup = cut.Find("[role='dialog']");
        Assert.Contains("mar-timepicker__popup", popup.GetAttribute("class"));
    }

    // ══════════════════════════════════════════════════════════════════
    // RES-T4B1-001/005: TimePicker cancellable OnOpen/OnClose
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void TimePicker_OnOpen_Fires_WithPopupEventArgs()
    {
        PopupEventArgs? receivedArgs = null;
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.OnOpen, (PopupEventArgs args) => { receivedArgs = args; }));

        cut.Find(".mar-timepicker__toggle").Click();

        Assert.NotNull(receivedArgs);
        Assert.False(receivedArgs!.IsCancelled);
    }

    [Fact]
    public void TimePicker_OnOpen_Cancelled_PopupStaysClosed()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.OnOpen, (PopupEventArgs args) => { args.IsCancelled = true; }));

        cut.Find(".mar-timepicker__toggle").Click();

        Assert.Empty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void TimePicker_OnClose_Fires_OnCommit()
    {
        PopupEventArgs? receivedArgs = null;
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.OnClose, (PopupEventArgs args) => { receivedArgs = args; }));

        // Open then commit via the Set button
        cut.Find(".mar-timepicker__toggle").Click();
        cut.Find(".mar-timepicker__btn--set").Click();

        Assert.NotNull(receivedArgs);
    }

    [Fact]
    public void TimePicker_OnClose_Cancelled_PopupStaysOpen()
    {
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.OnClose, (PopupEventArgs args) => { args.IsCancelled = true; }));

        // Open the popup
        cut.Find(".mar-timepicker__toggle").Click();
        Assert.NotEmpty(cut.FindAll("[role='dialog']"));

        // Try to close via Set — should stay open because OnClose is cancelled
        cut.Find(".mar-timepicker__btn--set").Click();
        Assert.NotEmpty(cut.FindAll("[role='dialog']"));
    }

    [Fact]
    public void TimePicker_OnBlur_Fires()
    {
        var blurFired = false;
        var cut = Render<MariloTimePicker<DateTime?>>(p => p
            .Add(x => x.OnBlur, () => { blurFired = true; }));

        cut.Find("input").Blur();

        Assert.True(blurFired);
    }

    // ══════════════════════════════════════════════════════════════════
    // RES-T4B1-002: MultiSelect OnOpen/OnClose/OnBlur
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void MultiSelect_OnOpen_Fires()
    {
        PopupEventArgs? receivedArgs = null;
        var cut = Render<MariloMultiSelect<string, string>>(p => p
            .Add(x => x.Data, new List<string> { "A", "B" })
            .Add(x => x.OnOpen, (PopupEventArgs args) => { receivedArgs = args; }));

        cut.Find(".mar-multiselect__input-area").Click();

        Assert.NotNull(receivedArgs);
    }

    [Fact]
    public void MultiSelect_OnOpen_Cancelled_PopupStaysClosed()
    {
        var cut = Render<MariloMultiSelect<string, string>>(p => p
            .Add(x => x.Data, new List<string> { "A", "B" })
            .Add(x => x.OnOpen, (PopupEventArgs args) => { args.IsCancelled = true; }));

        cut.Find(".mar-multiselect__input-area").Click();

        Assert.Empty(cut.FindAll("[role='listbox']"));
    }

    [Fact]
    public void MultiSelect_OnClose_Fires_ViaToggle()
    {
        PopupEventArgs? receivedArgs = null;
        var cut = Render<MariloMultiSelect<string, string>>(p => p
            .Add(x => x.Data, new List<string> { "A", "B" })
            .Add(x => x.OnClose, (PopupEventArgs args) => { receivedArgs = args; }));

        // Open via input area
        cut.Find(".mar-multiselect__input-area").Click();
        Assert.NotEmpty(cut.FindAll("[role='listbox']"));
        // Close via arrow toggle button
        cut.Find(".mar-multiselect__arrow").Click();

        Assert.NotNull(receivedArgs);
    }

    [Fact]
    public void MultiSelect_OnBlur_Fires()
    {
        var blurFired = false;
        var cut = Render<MariloMultiSelect<string, string>>(p => p
            .Add(x => x.Data, new List<string> { "A", "B" })
            .Add(x => x.OnBlur, () => { blurFired = true; }));

        cut.Find(".mar-multiselect").FocusOut(new Microsoft.AspNetCore.Components.Web.FocusEventArgs());

        Assert.True(blurFired);
    }

    // ══════════════════════════════════════════════════════════════════
    // RES-T4B1-003: DateTimePicker OnOpen/OnClose/OnBlur
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void DateTimePicker_OnOpen_Fires()
    {
        PopupEventArgs? receivedArgs = null;
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.OnOpen, (PopupEventArgs args) => { receivedArgs = args; }));

        // DateTimePicker opens on input click
        cut.Find("input").Click();

        Assert.NotNull(receivedArgs);
    }

    [Fact]
    public void DateTimePicker_OnOpen_Cancelled_PopupStaysClosed()
    {
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.OnOpen, (PopupEventArgs args) => { args.IsCancelled = true; }));

        cut.Find("input").Click();

        Assert.Empty(cut.FindAll(".mar-datetime-picker__popup"));
    }

    [Fact]
    public void DateTimePicker_OnBlur_Fires()
    {
        var blurFired = false;
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.OnBlur, () => { blurFired = true; }));

        cut.Find("input").Blur();

        Assert.True(blurFired);
    }

    [Fact]
    public void DateTimePicker_OnCalendarCellRender_Fires()
    {
        var renderCount = 0;
        var cut = Render<MariloDateTimePicker>(p => p
            .Add(x => x.OnCalendarCellRender, (CalendarCellRenderEventArgs args) => { renderCount++; }));

        // Open the popup to trigger calendar render
        cut.Find("input").Click();

        // Should fire for each visible day cell (typically 28-42)
        Assert.True(renderCount > 0);
    }

    // ══════════════════════════════════════════════════════════════════
    // RES-T4B1-004: DateRangePicker OnOpen/OnClose
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void DateRangePicker_OnOpen_Fires()
    {
        PopupEventArgs? receivedArgs = null;
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.OnOpen, (PopupEventArgs args) => { receivedArgs = args; }));

        // DateRangePicker opens on input focus/click
        cut.Find("input").Click();

        Assert.NotNull(receivedArgs);
    }

    [Fact]
    public void DateRangePicker_OnOpen_Cancelled_PopupStaysClosed()
    {
        var cut = Render<MariloDateRangePicker>(p => p
            .Add(x => x.OnOpen, (PopupEventArgs args) => { args.IsCancelled = true; }));

        cut.Find("input").Click();

        Assert.Empty(cut.FindAll(".mar-date-range-picker__popup"));
    }
}
