using Bunit;
using Marilo.Components.Layout;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Xunit;

namespace Marilo.Tests.Unit.Foundation;

public class GridLayoutTests : MariloTestBase
{
    // ── 1. CSS Grid mode activated by Columns parameter ────────────────

    [Fact]
    public void MariloGridLayout_RendersAsCssGrid_WhenColumnsIsSet()
    {
        var cut = Render<MariloGridLayout>(p => p
            .Add(g => g.Columns, "200px 1fr 200px")
            .Add(g => g.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        var style = cut.Find("div.mar-grid").GetAttribute("style") ?? "";
        Assert.Contains("display: grid", style);
        Assert.Contains("grid-template-columns: 200px 1fr 200px", style);
    }

    // ── 2. CSS Grid mode activated by Rows parameter ───────────────────

    [Fact]
    public void MariloGridLayout_RendersAsCssGrid_WhenRowsIsSet()
    {
        var cut = Render<MariloGridLayout>(p => p
            .Add(g => g.Rows, "auto 1fr auto")
            .Add(g => g.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        var style = cut.Find("div.mar-grid").GetAttribute("style") ?? "";
        Assert.Contains("display: grid", style);
        Assert.Contains("grid-template-rows: auto 1fr auto", style);
    }

    // ── 3. MariloGridLayoutColumn registers Width with parent grid ─────

    [Fact]
    public void MariloGridLayoutColumn_RegistersWidth_WithParentGrid()
    {
        var cut = Render<MariloGridLayout>(p => p
            .Add(g => g.ColumnDefinitions, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloGridLayoutColumn>(0);
                builder.AddAttribute(1, "Width", "200px");
                builder.CloseComponent();
                builder.OpenComponent<MariloGridLayoutColumn>(2);
                builder.AddAttribute(3, "Width", "1fr");
                builder.CloseComponent();
            }))
            .Add(g => g.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        var style = cut.Find("div.mar-grid").GetAttribute("style") ?? "";
        Assert.Contains("display: grid", style);
        Assert.Contains("grid-template-columns: 200px 1fr", style);
    }

    // ── 4. MariloGridLayoutRow registers Height with parent grid ───────

    [Fact]
    public void MariloGridLayoutRow_RegistersHeight_WithParentGrid()
    {
        var cut = Render<MariloGridLayout>(p => p
            .Add(g => g.RowDefinitions, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloGridLayoutRow>(0);
                builder.AddAttribute(1, "Height", "100px");
                builder.CloseComponent();
                builder.OpenComponent<MariloGridLayoutRow>(2);
                builder.AddAttribute(3, "Height", "auto");
                builder.CloseComponent();
            }))
            .Add(g => g.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        var style = cut.Find("div.mar-grid").GetAttribute("style") ?? "";
        Assert.Contains("display: grid", style);
        Assert.Contains("grid-template-rows: 100px auto", style);
    }

    // ── 5. MariloGridLayoutItem positions content with Row/Column ──────

    [Fact]
    public void MariloGridLayoutItem_SetsGridPosition_WithRowAndColumn()
    {
        var cut = Render<MariloGridLayout>(p => p
            .Add(g => g.Columns, "1fr 1fr")
            .Add(g => g.Rows, "auto auto")
            .Add(g => g.ChildContent, (RenderFragment)(b =>
            {
                b.OpenComponent<MariloGridLayoutItem>(0);
                b.AddAttribute(1, "Row", 2);
                b.AddAttribute(2, "Column", 1);
                b.AddAttribute(3, "ChildContent", (RenderFragment)(inner => inner.AddContent(0, "Item A")));
                b.CloseComponent();
            }))
        );

        var item = cut.Find(".mar-grid-item");
        var style = item.GetAttribute("style") ?? "";
        Assert.Contains("grid-row: 2", style);
        Assert.Contains("grid-column: 1", style);
    }

    [Fact]
    public void MariloGridLayoutItem_SetsGridSpan_WithRowSpanAndColumnSpan()
    {
        var cut = Render<MariloGridLayout>(p => p
            .Add(g => g.Columns, "1fr 1fr 1fr")
            .Add(g => g.Rows, "auto auto auto")
            .Add(g => g.ChildContent, (RenderFragment)(b =>
            {
                b.OpenComponent<MariloGridLayoutItem>(0);
                b.AddAttribute(1, "Row", 1);
                b.AddAttribute(2, "Column", 1);
                b.AddAttribute(3, "RowSpan", 2);
                b.AddAttribute(4, "ColumnSpan", 3);
                b.AddAttribute(5, "ChildContent", (RenderFragment)(inner => inner.AddContent(0, "Spanning Item")));
                b.CloseComponent();
            }))
        );

        var item = cut.Find(".mar-grid-item");
        var style = item.GetAttribute("style") ?? "";
        Assert.Contains("grid-row: 1 / span 2", style);
        Assert.Contains("grid-column: 1 / span 3", style);
    }

    // ── 6. ColumnSpacing/RowSpacing set CSS gap properties ─────────────

    [Fact]
    public void MariloGridLayout_SetsGapProperties_WhenSpacingIsSet()
    {
        var cut = Render<MariloGridLayout>(p => p
            .Add(g => g.Columns, "1fr 1fr")
            .Add(g => g.ColumnSpacing, "16px")
            .Add(g => g.RowSpacing, "8px")
            .Add(g => g.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        var style = cut.Find("div.mar-grid").GetAttribute("style") ?? "";
        Assert.Contains("column-gap: 16px", style);
        Assert.Contains("row-gap: 8px", style);
    }

    // ── 7. Width parameter sets container width ────────────────────────

    [Fact]
    public void MariloGridLayout_SetsWidth_WhenWidthParameterIsSet()
    {
        var cut = Render<MariloGridLayout>(p => p
            .Add(g => g.Columns, "1fr")
            .Add(g => g.Width, "800px")
            .Add(g => g.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        var style = cut.Find("div.mar-grid").GetAttribute("style") ?? "";
        Assert.Contains("width: 800px", style);
    }

    // ── 8. HorizontalAlign/VerticalAlign set justify-items/align-items ─

    [Fact]
    public void MariloGridLayout_SetsAlignmentStyles_WhenAlignmentParametersAreSet()
    {
        var cut = Render<MariloGridLayout>(p => p
            .Add(g => g.Columns, "1fr 1fr")
            .Add(g => g.HorizontalAlign, StackAlignment.Center)
            .Add(g => g.VerticalAlign, StackAlignment.Start)
            .Add(g => g.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        var style = cut.Find("div.mar-grid").GetAttribute("style") ?? "";
        Assert.Contains("justify-items: center", style);
        Assert.Contains("align-items: start", style);
    }

    // ── 9. Flex container mode (no Columns/Rows) ───────────────────────

    [Fact]
    public void MariloGridLayout_RendersInFlexMode_WhenNoColumnsOrRowsAreSet()
    {
        var cut = Render<MariloGridLayout>(p => p
            .Add(g => g.ChildContent, (RenderFragment)(b => b.AddContent(0, "Flex content")))
        );

        var div = cut.Find("div.mar-grid");
        var style = div.GetAttribute("style") ?? "";

        // Should NOT contain CSS grid styles in flex mode
        Assert.DoesNotContain("display: grid", style);
        Assert.Contains("Flex content", cut.Markup);
    }
}
