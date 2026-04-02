using Bunit;
using Marilo.Components.Layout;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Foundation;

public class StackTests : MariloTestBase
{
    [Fact]
    public void DefaultOrientation_IsHorizontal()
    {
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => { })));

        Assert.Contains("mar-stack--horizontal", cut.Markup);
    }

    [Fact]
    public void VerticalOrientation_AppliesVerticalClass()
    {
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.Orientation, StackDirection.Vertical));

        Assert.Contains("mar-stack--vertical", cut.Markup);
    }

    [Fact]
    public void Spacing_SetsGapStyle()
    {
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.Spacing, "16px"));

        Assert.Contains("gap:16px", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    [Fact]
    public void Width_SetsWidthStyle()
    {
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.Width, "100%"));

        Assert.Contains("width:100%", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    [Fact]
    public void Height_SetsHeightStyle()
    {
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.Height, "300px"));

        Assert.Contains("height:300px", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    [Fact]
    public void HorizontalAlign_Center_OnHorizontalStack_SetsJustifyContent()
    {
        // Horizontal stack: HorizontalAlign maps to justify-content (main axis)
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.Orientation, StackDirection.Horizontal)
            .Add(p => p.HorizontalAlign, StackAlignment.Center));

        Assert.Contains("justify-content:center", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    [Fact]
    public void VerticalAlign_Center_OnHorizontalStack_SetsAlignItems()
    {
        // Horizontal stack: VerticalAlign maps to align-items (cross axis)
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.Orientation, StackDirection.Horizontal)
            .Add(p => p.VerticalAlign, StackAlignment.Center));

        Assert.Contains("align-items:center", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    [Fact]
    public void VerticalAlign_Center_OnVerticalStack_SetsJustifyContent()
    {
        // Vertical stack: VerticalAlign maps to justify-content (main axis)
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.Orientation, StackDirection.Vertical)
            .Add(p => p.VerticalAlign, StackAlignment.Center));

        Assert.Contains("justify-content:center", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    [Fact]
    public void HorizontalAlign_End_OnVerticalStack_SetsAlignItems()
    {
        // Vertical stack: HorizontalAlign maps to align-items (cross axis)
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.Orientation, StackDirection.Vertical)
            .Add(p => p.HorizontalAlign, StackAlignment.End));

        Assert.Contains("align-items:flex-end", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    [Fact]
    public void ChildContent_IsRendered()
    {
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "hello");
                builder.CloseElement();
            })));

        Assert.Contains("<span>hello</span>", cut.Markup);
    }

    [Fact]
    public void DefaultAlignments_DoNotEmitJustifyContentOrAlignItems()
    {
        // Start is the default; StyleBuilder skips Start values so no justify-content or align-items in style
        var cut = Render<MariloStack>(parameters => parameters
            .Add(p => p.Orientation, StackDirection.Horizontal));

        var style = cut.Find("div").GetAttribute("style") ?? "";
        Assert.DoesNotContain("justify-content", style);
        Assert.DoesNotContain("align-items", style);
    }
}
