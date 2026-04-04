using Bunit;
using Marilo.Components.Layout;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Layout;

public class SplitterTests : MariloTestBase
{
    // ── Helper to render a splitter with two panes ──────────────────────

    private IRenderedComponent<MariloSplitter> RenderWithTwoPanes(
        string firstSize = "40%",
        Action<ComponentParameterCollectionBuilder<MariloSplitter>>? extra = null)
    {
        return Render<MariloSplitter>(parameters =>
        {
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloSplitterPane>(0);
                builder.AddAttribute(1, "Size", firstSize);
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Left")));
                builder.CloseComponent();

                builder.OpenComponent<MariloSplitterPane>(3);
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Right")));
                builder.CloseComponent();
            }));
            extra?.Invoke(parameters);
        });
    }

    // ── 1. Default horizontal orientation CSS class ──────────────────────

    [Fact]
    public void DefaultOrientation_IsHorizontal_AppliesHorizontalClass()
    {
        var cut = RenderWithTwoPanes();
        Assert.Contains("mar-splitter--horizontal", cut.Markup);
    }

    // ── 2. Vertical orientation CSS class ───────────────────────────────

    [Fact]
    public void VerticalOrientation_AppliesVerticalClass()
    {
        var cut = Render<MariloSplitter>(parameters => parameters
            .Add(p => p.Orientation, SplitterOrientation.Vertical)
            .Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloSplitterPane>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Top")));
                builder.CloseComponent();

                builder.OpenComponent<MariloSplitterPane>(2);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Bottom")));
                builder.CloseComponent();
            })));

        Assert.Contains("mar-splitter--vertical", cut.Markup);
    }

    // ── 3. Pane registration ─────────────────────────────────────────────

    [Fact]
    public void PaneRegistration_PanesAppearAsChildren()
    {
        var cut = RenderWithTwoPanes();
        var panes = cut.FindAll(".mar-splitter__pane");
        Assert.True(panes.Count >= 2);
    }

    // ── 4. SplitterPanes wrapper ─────────────────────────────────────────

    [Fact]
    public void SplitterPanes_Wrapper_PanesRegisterThroughWrapper()
    {
        var cut = Render<MariloSplitter>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloSplitterPanes>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(inner =>
                {
                    inner.OpenComponent<MariloSplitterPane>(0);
                    inner.AddAttribute(1, "Size", "50%");
                    inner.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "A")));
                    inner.CloseComponent();

                    inner.OpenComponent<MariloSplitterPane>(3);
                    inner.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "B")));
                    inner.CloseComponent();
                }));
                builder.CloseComponent();
            })));

        var panes = cut.FindAll(".mar-splitter__pane");
        Assert.True(panes.Count >= 2);
    }

    // ── 5. Collapse button renders when Collapsible=true ─────────────────

    [Fact]
    public void CollapseButton_RendersWhenCollapsibleTrue()
    {
        var cut = Render<MariloSplitter>(parameters =>
        {
            parameters.Add(p => p.Collapsible, true);
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloSplitterPane>(0);
                builder.AddAttribute(1, "Size", "40%");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Left")));
                builder.CloseComponent();

                builder.OpenComponent<MariloSplitterPane>(3);
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Right")));
                builder.CloseComponent();
            }));
        });

        Assert.Contains("mar-splitter__collapse-btn", cut.Markup);
    }

    // ── 6. Collapse toggles pane collapsed state ──────────────────────────

    [Fact]
    public void CollapseButton_Click_TogglesCollapsedState()
    {
        var cut = Render<MariloSplitter>(parameters =>
        {
            parameters.Add(p => p.Collapsible, true);
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloSplitterPane>(0);
                builder.AddAttribute(1, "Size", "40%");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Left")));
                builder.CloseComponent();

                builder.OpenComponent<MariloSplitterPane>(3);
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Right")));
                builder.CloseComponent();
            }));
        });

        var btn = cut.Find(".mar-splitter__collapse-btn");
        btn.Click();

        Assert.Contains("mar-splitter__pane--collapsed", cut.Markup);
    }

    // ── 7. GetState returns pane sizes and collapse states ───────────────

    [Fact]
    public void GetState_ReturnsPaneSizesAndCollapsedFlags()
    {
        var cut = RenderWithTwoPanes("30%");
        var splitter = cut.Instance;

        var state = splitter.GetState();

        Assert.Equal(2, state.PaneSizes.Count);
        Assert.Equal(2, state.CollapsedPanes.Count);
        Assert.Equal("30%", state.PaneSizes[0]);
    }

    // ── 8. SetState restores pane sizes ──────────────────────────────────

    [Fact]
    public void SetState_RestoresPaneSizes()
    {
        var cut = RenderWithTwoPanes("40%");
        var splitter = cut.Instance;

        splitter.SetState(new SplitterState
        {
            PaneSizes = ["60%", ""],
            CollapsedPanes = [false, false]
        });
        cut.Render();

        var state = splitter.GetState();
        Assert.Equal("60%", state.PaneSizes[0]);
    }

    // ── 9. Width parameter renders as inline style ────────────────────────

    [Fact]
    public void Width_Parameter_RendersInlineStyle()
    {
        var cut = Render<MariloSplitter>(parameters =>
            parameters.Add(p => p.Width, "800px"));

        Assert.Contains("width:800px", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    // ── 10. Height parameter renders as inline style ──────────────────────

    [Fact]
    public void Height_Parameter_RendersInlineStyle()
    {
        var cut = Render<MariloSplitter>(parameters =>
            parameters.Add(p => p.Height, "400px"));

        Assert.Contains("height:400px", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    // ── 11. Resizable=false on pane ───────────────────────────────────────

    [Fact]
    public void Pane_Resizable_False_PaneStillRenders()
    {
        var cut = Render<MariloSplitter>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloSplitterPane>(0);
                builder.AddAttribute(1, "Size", "40%");
                builder.AddAttribute(2, "Resizable", false);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Left")));
                builder.CloseComponent();

                builder.OpenComponent<MariloSplitterPane>(4);
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Right")));
                builder.CloseComponent();
            })));

        var panes = cut.FindAll(".mar-splitter__pane");
        Assert.True(panes.Count >= 2);
    }

    // ── 12. Min/Max parameters render on pane style ───────────────────────

    [Fact]
    public void Pane_MinMax_RendersInPaneStyle()
    {
        var cut = Render<MariloSplitter>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloSplitterPane>(0);
                builder.AddAttribute(1, "Size", "40%");
                builder.AddAttribute(2, "Min", "100px");
                builder.AddAttribute(3, "Max", "600px");
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Left")));
                builder.CloseComponent();

                builder.OpenComponent<MariloSplitterPane>(5);
                builder.AddAttribute(6, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Right")));
                builder.CloseComponent();
            })));

        Assert.Contains("min-width:100px", cut.Markup.Replace(" ", "").Replace(";", ""));
        Assert.Contains("max-width:600px", cut.Markup.Replace(" ", "").Replace(";", ""));
    }

    // ── 13. OnResize event callback ───────────────────────────────────────

    [Fact]
    public void OnResize_EventCallback_CanBeSet()
    {
        SplitterResizeEventArgs? received = null;

        var cut = RenderWithTwoPanes(extra: p =>
            p.Add(x => x.OnResize, EventCallback.Factory.Create<SplitterResizeEventArgs>(
                this, args => received = args)));

        // Verify the component rendered without error; event fires during drag (JS), so just check markup
        Assert.Contains("mar-splitter", cut.Markup);
    }

    // ── 14. OnCollapse event callback ─────────────────────────────────────

    [Fact]
    public void OnCollapse_EventCallback_FiresOnCollapse()
    {
        SplitterCollapseEventArgs? received = null;

        var cut = Render<MariloSplitter>(parameters =>
        {
            parameters.Add(p => p.Collapsible, true);
            parameters.Add(p => p.OnCollapse, EventCallback.Factory.Create<SplitterCollapseEventArgs>(
                this, args => received = args));
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloSplitterPane>(0);
                builder.AddAttribute(1, "Size", "40%");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Left")));
                builder.CloseComponent();

                builder.OpenComponent<MariloSplitterPane>(3);
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Right")));
                builder.CloseComponent();
            }));
        });

        cut.Find(".mar-splitter__collapse-btn").Click();

        Assert.NotNull(received);
        Assert.Equal(0, received!.PaneIndex);
    }

    // ── 15. OnExpand event callback ───────────────────────────────────────

    [Fact]
    public void OnExpand_EventCallback_FiresOnExpand()
    {
        SplitterCollapseEventArgs? expandArgs = null;

        var cut = Render<MariloSplitter>(parameters =>
        {
            parameters.Add(p => p.Collapsible, true);
            parameters.Add(p => p.OnExpand, EventCallback.Factory.Create<SplitterCollapseEventArgs>(
                this, args => expandArgs = args));
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloSplitterPane>(0);
                builder.AddAttribute(1, "Size", "40%");
                builder.AddAttribute(2, "Collapsed", true);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Left")));
                builder.CloseComponent();

                builder.OpenComponent<MariloSplitterPane>(4);
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Right")));
                builder.CloseComponent();
            }));
        });

        // Click collapse button on pane that is already collapsed → triggers expand
        cut.Find(".mar-splitter__collapse-btn").Click();

        Assert.NotNull(expandArgs);
    }

    // ── 16. Nested splitter — inner panes register to inner splitter ──────

    [Fact]
    public void NestedSplitter_InnerPanesRegisterToInnerSplitter()
    {
        var cut = Render<MariloSplitter>(parameters =>
            parameters.Add(p => p.ChildContent, (RenderFragment)(outerBuilder =>
            {
                outerBuilder.OpenComponent<MariloSplitterPane>(0);
                outerBuilder.AddAttribute(1, "Size", "50%");
                outerBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(paneContent =>
                {
                    paneContent.OpenComponent<MariloSplitter>(0);
                    paneContent.AddAttribute(1, "ChildContent", (RenderFragment)(innerBuilder =>
                    {
                        innerBuilder.OpenComponent<MariloSplitterPane>(0);
                        innerBuilder.AddAttribute(1, "Size", "30%");
                        innerBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Inner Left")));
                        innerBuilder.CloseComponent();

                        innerBuilder.OpenComponent<MariloSplitterPane>(3);
                        innerBuilder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Inner Right")));
                        innerBuilder.CloseComponent();
                    }));
                    paneContent.CloseComponent();
                }));
                outerBuilder.CloseComponent();

                outerBuilder.OpenComponent<MariloSplitterPane>(3);
                outerBuilder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Right")));
                outerBuilder.CloseComponent();
            })));

        // Both outer and inner splitters should render their panes
        var allPanes = cut.FindAll(".mar-splitter__pane");
        Assert.True(allPanes.Count >= 3);
    }

    // ── 17. Legacy 2-pane (FirstPane/SecondPane) renders ─────────────────

    [Fact]
    public void LegacyMode_FirstAndSecondPane_Renders()
    {
        var cut = Render<MariloSplitter>(parameters =>
        {
            parameters.Add(p => p.FirstPane, (RenderFragment)(b => b.AddContent(0, "First")));
            parameters.Add(p => p.SecondPane, (RenderFragment)(b => b.AddContent(0, "Second")));
        });

        Assert.Contains("mar-splitter__pane--first", cut.Markup);
        Assert.Contains("mar-splitter__pane--second", cut.Markup);
        Assert.Contains("First", cut.Markup);
        Assert.Contains("Second", cut.Markup);
    }
}
