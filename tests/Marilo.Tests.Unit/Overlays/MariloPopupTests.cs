using Bunit;
using Marilo.Components.Overlays;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Overlays;

public class MariloPopupTests : MariloTestBase
{
    [Fact]
    public void Renders_ChildContent_When_IsOpen_True()
    {
        var cut = Render<MariloPopup>(p => p
            .Add(x => x.IsOpen, true)
            .Add(x => x.ChildContent, (RenderFragment)(b =>
            {
                b.OpenElement(0, "span");
                b.AddContent(1, "Hello Popup");
                b.CloseElement();
            })));

        Assert.Contains("Hello Popup", cut.Markup);
    }

    [Fact]
    public void Does_Not_Render_When_IsOpen_False()
    {
        var cut = Render<MariloPopup>(p => p
            .Add(x => x.IsOpen, false)
            .Add(x => x.ChildContent, (RenderFragment)(b =>
            {
                b.OpenElement(0, "span");
                b.AddContent(1, "Hidden Content");
                b.CloseElement();
            })));

        Assert.DoesNotContain("Hidden Content", cut.Markup);
        Assert.DoesNotContain("mar-popup", cut.Markup);
    }

    [Fact]
    public void Has_Dialog_Role_When_FocusTrap_True()
    {
        var cut = Render<MariloPopup>(p => p
            .Add(x => x.IsOpen, true)
            .Add(x => x.FocusTrap, true));

        Assert.Contains("role=\"dialog\"", cut.Markup);
        Assert.Contains("aria-modal=\"true\"", cut.Markup);
    }

    [Fact]
    public void Does_Not_Have_Dialog_Role_When_FocusTrap_False()
    {
        var cut = Render<MariloPopup>(p => p
            .Add(x => x.IsOpen, true)
            .Add(x => x.FocusTrap, false));

        Assert.DoesNotContain("role=\"dialog\"", cut.Markup);
        Assert.DoesNotContain("aria-modal", cut.Markup);
    }

    [Fact]
    public void Applies_CssClass_Via_Class_Parameter()
    {
        var cut = Render<MariloPopup>(p => p
            .Add(x => x.IsOpen, true)
            .Add(x => x.Class, "my-custom-popup"));

        Assert.Contains("my-custom-popup", cut.Markup);
    }

    [Fact]
    public void Contains_Placement_Modifier_Class()
    {
        var cut = Render<MariloPopup>(p => p
            .Add(x => x.IsOpen, true)
            .Add(x => x.Placement, PopupPlacement.Top));

        Assert.Contains("mar-popup--top", cut.Markup);
    }

    [Fact]
    public void Contains_Open_Class_When_Visible()
    {
        var cut = Render<MariloPopup>(p => p
            .Add(x => x.IsOpen, true));

        Assert.Contains("mar-popup--open", cut.Markup);
        Assert.Contains("mar-popup", cut.Markup);
    }

    [Fact]
    public void Escape_Key_Closes_Popup_When_CloseOnEscape_True()
    {
        var isOpenChanged = false;
        var newValue = true;

        var cut = Render<MariloPopup>(p => p
            .Add(x => x.IsOpen, true)
            .Add(x => x.CloseOnEscape, true)
            .Add(x => x.IsOpenChanged, EventCallback.Factory.Create<bool>(this, v =>
            {
                isOpenChanged = true;
                newValue = v;
            })));

        cut.Find("div.mar-popup").KeyDown(new Microsoft.AspNetCore.Components.Web.KeyboardEventArgs
        {
            Key = "Escape"
        });

        Assert.True(isOpenChanged);
        Assert.False(newValue);
    }

    [Fact]
    public void Escape_Key_Does_Not_Close_When_CloseOnEscape_False()
    {
        var isOpenChanged = false;

        var cut = Render<MariloPopup>(p => p
            .Add(x => x.IsOpen, true)
            .Add(x => x.CloseOnEscape, false)
            .Add(x => x.IsOpenChanged, EventCallback.Factory.Create<bool>(this, v =>
            {
                isOpenChanged = true;
            })));

        cut.Find("div.mar-popup").KeyDown(new Microsoft.AspNetCore.Components.Web.KeyboardEventArgs
        {
            Key = "Escape"
        });

        Assert.False(isOpenChanged);
    }

    [Fact]
    public void Default_Placement_Is_Bottom()
    {
        var cut = Render<MariloPopup>(p => p
            .Add(x => x.IsOpen, true));

        Assert.Contains("mar-popup--bottom", cut.Markup);
    }
}
