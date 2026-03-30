using Bunit;
using Marilo.Components.Buttons;
using Marilo.Components.DataDisplay;
using Marilo.Components.Feedback;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.Enhancements;

public class ComponentEnhancementTests : MariloTestBase
{
    [Fact]
    public void Button_Renders_FillMode_CssClass()
    {
        var cut = Render<MariloButton>(parameters => parameters
            .Add(p => p.FillMode, FillMode.Flat)
            .Add(p => p.Rounded, RoundedMode.Full)
            .Add(p => p.ChildContent, (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Click")))
        );

        var button = cut.Find("button");
        Assert.Contains("mar-button--fill-flat", button.GetAttribute("class"));
        Assert.Contains("mar-button--rounded-full", button.GetAttribute("class"));
    }

    [Fact]
    public void Slider_Renders_Vertical_Orientation_Class()
    {
        var cut = Render<MariloSlider>(parameters => parameters
            .Add(p => p.Orientation, SliderOrientation.Vertical)
        );

        var div = cut.Find("div");
        Assert.Contains("mar-slider--vertical", div.GetAttribute("class"));
    }

    [Fact]
    public void Dialog_Renders_Draggable_Class()
    {
        var cut = Render<MariloDialog>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Draggable, true)
            .Add(p => p.Title, "Test Dialog")
        );

        var dialog = cut.Find(".mar-dialog--draggable");
        Assert.NotNull(dialog);
    }

    [Fact]
    public void Snackbar_Renders_With_Message()
    {
        var cut = Render<MariloSnackbar>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Message, "Test notification")
        );

        Assert.Contains("Test notification", cut.Markup);
        Assert.Contains("mar-snackbar", cut.Markup);
    }

    [Fact]
    public void TextField_Renders_Readonly_Attribute()
    {
        var cut = Render<MariloTextField>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Value, "test")
        );

        var input = cut.Find("input");
        Assert.NotNull(input.GetAttribute("readonly"));
    }

    [Fact]
    public void Checkbox_Renders_Indeterminate_Aria_Attribute()
    {
        var cut = Render<MariloCheckbox>(parameters => parameters
            .Add(p => p.Indeterminate, true)
            .Add(p => p.Label, "Test")
        );

        var input = cut.Find("input[type='checkbox']");
        Assert.Equal("mixed", input.GetAttribute("aria-checked"));
    }

    [Fact]
    public void TextField_Renders_MaxLength_Attribute()
    {
        var cut = Render<MariloTextField>(parameters => parameters
            .Add(p => p.MaxLength, 50)
            .Add(p => p.Value, "test")
        );

        var input = cut.Find("input");
        Assert.Equal("50", input.GetAttribute("maxlength"));
    }

    [Fact]
    public void Dialog_Without_Modal_Does_Not_Render_Overlay()
    {
        var cut = Render<MariloDialog>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Modal, false)
            .Add(p => p.Title, "Test")
        );

        Assert.Empty(cut.FindAll(".mar-dialog-overlay"));
    }

    [Fact]
    public void Dialog_With_Modal_Renders_Overlay()
    {
        var cut = Render<MariloDialog>(parameters => parameters
            .Add(p => p.IsOpen, true)
            .Add(p => p.Modal, true)
            .Add(p => p.Title, "Test")
        );

        Assert.NotEmpty(cut.FindAll(".mar-dialog-overlay"));
    }

    [Fact]
    public void Button_Renders_Icon_Slot()
    {
        var cut = Render<MariloButton>(parameters => parameters
            .Add(p => p.Icon, (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "icon-content")))
            .Add(p => p.ChildContent, (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Click")))
        );

        var iconSpan = cut.Find(".mar-button__icon");
        Assert.Contains("icon-content", iconSpan.TextContent);
    }

    [Fact]
    public void Checkbox_Indeterminate_Adds_CssClass()
    {
        var cut = Render<MariloCheckbox>(parameters => parameters
            .Add(p => p.Indeterminate, true)
        );

        var label = cut.Find("label");
        Assert.Contains("mar-checkbox--indeterminate", label.GetAttribute("class"));
    }

    [Fact]
    public void Tooltip_Renders_ShowOn_Class()
    {
        var cut = Render<MariloTooltip>(parameters => parameters
            .Add(p => p.Text, "Tip")
            .Add(p => p.ShowOn, TooltipShowOn.Click)
            .Add(p => p.ChildContent, (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Hover me")))
        );

        var tooltipDiv = cut.Find(".mar-tooltip--show-click");
        Assert.NotNull(tooltipDiv);
    }
}
