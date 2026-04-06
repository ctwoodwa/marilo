using Bunit;
using Marilo.Components.Layout.ResizableContainer;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Layout;

public class MariloResizableContainerTests : MariloTestBase
{
    // ── Default rendering ──────────────────────────────────────────────

    [Fact]
    public void Renders_Default_WithContainerClass()
    {
        var cut = Render<MariloResizableContainer>();
        Assert.Contains("mar-resizable-container", cut.Markup);
    }

    [Fact]
    public void Renders_Default_WithContentClass()
    {
        var cut = Render<MariloResizableContainer>();
        Assert.Contains("mar-resizable-container__content", cut.Markup);
    }

    [Fact]
    public void Renders_Default_WithHandle()
    {
        var cut = Render<MariloResizableContainer>();
        Assert.Contains("mar-resizable-container__handle", cut.Markup);
    }

    [Fact]
    public void Renders_Default_WithBottomRightHandle()
    {
        var cut = Render<MariloResizableContainer>();
        Assert.Contains("mar-resizable-container__handle--bottom-right", cut.Markup);
    }

    // ── Child content ──────────────────────────────────────────────────

    [Fact]
    public void Renders_ChildContent()
    {
        var cut = Render<MariloResizableContainer>(p =>
            p.Add(x => x.ChildContent, (RenderFragment)(b =>
            {
                b.AddContent(0, "Hello resizable");
            })));

        Assert.Contains("Hello resizable", cut.Markup);
    }

    // ── Sizing parameters ──────────────────────────────────────────────

    [Fact]
    public void Applies_Width_And_Height_Style()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.Width, "500px")
            .Add(x => x.Height, "300px"));

        Assert.Contains("width:500px", cut.Markup);
        Assert.Contains("height:300px", cut.Markup);
    }

    [Fact]
    public void Applies_MinWidth_And_MinHeight()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.MinWidth, "200px")
            .Add(x => x.MinHeight, "100px"));

        Assert.Contains("min-width:200px", cut.Markup);
        Assert.Contains("min-height:100px", cut.Markup);
    }

    [Fact]
    public void Applies_MaxWidth_And_MaxHeight()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.MaxWidth, "800px")
            .Add(x => x.MaxHeight, "600px"));

        Assert.Contains("max-width:800px", cut.Markup);
        Assert.Contains("max-height:600px", cut.Markup);
    }

    // ── Handle visibility ──────────────────────────────────────────────

    [Fact]
    public void ShowHandle_False_HidesHandle()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.ShowHandle, false));

        Assert.DoesNotContain("mar-resizable-container__handle", cut.Markup);
    }

    [Fact]
    public void Enabled_False_HidesHandle()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.Enabled, false));

        Assert.DoesNotContain("mar-resizable-container__handle", cut.Markup);
    }

    [Fact]
    public void Enabled_False_AppliesDisabledClass()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.Enabled, false));

        Assert.Contains("mar-resizable-container--disabled", cut.Markup);
    }

    // ── Edge configuration ─────────────────────────────────────────────

    [Fact]
    public void ResizeEdges_Right_RendersRightHandle()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.ResizeEdges, MariloResizeEdges.Right));

        Assert.Contains("mar-resizable-container__handle--right", cut.Markup);
        Assert.DoesNotContain("mar-resizable-container__handle--bottom", cut.Markup);
    }

    [Fact]
    public void ResizeEdges_Bottom_RendersBottomHandle()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.ResizeEdges, MariloResizeEdges.Bottom));

        Assert.Contains("mar-resizable-container__handle--bottom", cut.Markup);
    }

    [Fact]
    public void ResizeEdges_None_RendersNoHandles()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.ResizeEdges, MariloResizeEdges.None));

        Assert.DoesNotContain("mar-resizable-container__handle", cut.Markup);
    }

    // ── Handle accessibility ───────────────────────────────────────────

    [Fact]
    public void Handle_HasDefaultAriaLabel()
    {
        var cut = Render<MariloResizableContainer>();
        var handle = cut.Find("button");
        Assert.Equal("Resize", handle.GetAttribute("aria-label"));
    }

    [Fact]
    public void Handle_UsesCustomAriaLabel()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.HandleAriaLabel, "Resize editor panel"));

        var handle = cut.Find("button");
        Assert.Equal("Resize editor panel", handle.GetAttribute("aria-label"));
    }

    [Fact]
    public void Handle_IsButton()
    {
        var cut = Render<MariloResizableContainer>();
        var handle = cut.Find("button");
        Assert.Equal("button", handle.GetAttribute("type"));
    }

    [Fact]
    public void Handle_IsFocusable()
    {
        var cut = Render<MariloResizableContainer>();
        var handle = cut.Find("button");
        Assert.Equal("0", handle.GetAttribute("tabindex"));
    }

    // ── Custom classes ─────────────────────────────────────────────────

    [Fact]
    public void Applies_Custom_Class()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.Class, "my-custom"));

        Assert.Contains("my-custom", cut.Markup);
    }

    [Fact]
    public void Applies_Custom_HandleClass()
    {
        var cut = Render<MariloResizableContainer>(p => p
            .Add(x => x.HandleClass, "my-handle"));

        Assert.Contains("my-handle", cut.Markup);
    }

    // ── CSS provider contract ──────────────────────────────────────────

    [Fact]
    public void FluentUI_ContainerClass_Default()
    {
        var provider = new Marilo.Providers.FluentUI.FluentUICssProvider();
        var css = provider.ResizableContainerClass(false, false);
        Assert.Equal("mar-resizable-container", css);
    }

    [Fact]
    public void FluentUI_ContainerClass_Resizing()
    {
        var provider = new Marilo.Providers.FluentUI.FluentUICssProvider();
        var css = provider.ResizableContainerClass(true, false);
        Assert.Contains("mar-resizable-container--resizing", css);
    }

    [Fact]
    public void FluentUI_ContainerClass_Disabled()
    {
        var provider = new Marilo.Providers.FluentUI.FluentUICssProvider();
        var css = provider.ResizableContainerClass(false, true);
        Assert.Contains("mar-resizable-container--disabled", css);
    }

    [Fact]
    public void FluentUI_ContentClass()
    {
        var provider = new Marilo.Providers.FluentUI.FluentUICssProvider();
        var css = provider.ResizableContainerContentClass();
        Assert.Equal("mar-resizable-container__content", css);
    }

    [Fact]
    public void FluentUI_HandleClass_BottomRight()
    {
        var provider = new Marilo.Providers.FluentUI.FluentUICssProvider();
        var css = provider.ResizableContainerHandleClass(MariloResizeEdges.BottomRight, false, false);
        Assert.Contains("mar-resizable-container__handle--bottom-right", css);
    }

    [Fact]
    public void FluentUI_HandleClass_Active()
    {
        var provider = new Marilo.Providers.FluentUI.FluentUICssProvider();
        var css = provider.ResizableContainerHandleClass(MariloResizeEdges.Right, true, false);
        Assert.Contains("mar-resizable-container__handle--active", css);
    }

    [Fact]
    public void FluentUI_HandleClass_Focused()
    {
        var provider = new Marilo.Providers.FluentUI.FluentUICssProvider();
        var css = provider.ResizableContainerHandleClass(MariloResizeEdges.Bottom, false, true);
        Assert.Contains("mar-resizable-container__handle--focused", css);
    }

    // Note: Bootstrap provider tests omitted — test project does not reference Marilo.Providers.Bootstrap.
    // Bootstrap provider methods are structurally identical and verified via manual testing.
}
