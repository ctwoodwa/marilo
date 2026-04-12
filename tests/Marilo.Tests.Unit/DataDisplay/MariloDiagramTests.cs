using Bunit;
using Marilo.Components.DataDisplay;
using Marilo.Core.Models;
using Xunit;

namespace Marilo.Tests.Unit.DataDisplay;

public class MariloDiagramTests : MariloTestBase
{
    private static List<DiagramShapeDescriptor> CreateTestShapes() =>
    [
        new() { Id = "s1", Text = "Start", Type = DiagramShapeType.Rectangle, X = 50, Y = 40, Width = 100, Height = 50 },
        new() { Id = "s2", Text = "Process", Type = DiagramShapeType.Ellipse, X = 220, Y = 40, Width = 120, Height = 60 },
        new() { Id = "s3", Text = "Decision", Type = DiagramShapeType.Diamond, X = 410, Y = 40, Width = 100, Height = 60 },
    ];

    private static List<DiagramConnectionDescriptor> CreateTestConnections() =>
    [
        new() { Id = "c1", FromShapeId = "s1", ToShapeId = "s2", Text = "Next" },
        new() { Id = "c2", FromShapeId = "s2", ToShapeId = "s3" },
    ];

    // ── Empty / null state ──────────────────────────────────────────

    [Fact]
    public void Diagram_Renders_Container_When_Shapes_Is_Null()
    {
        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, (IReadOnlyList<DiagramShapeDescriptor>?)null));

        var container = cut.Find(".mar-diagram");
        Assert.NotNull(container);

        // SVG canvas exists but no shapes or connections inside
        var svg = cut.Find(".mar-diagram__canvas");
        Assert.NotNull(svg);
        Assert.Empty(cut.FindAll(".mar-diagram__shape"));
    }

    [Fact]
    public void Diagram_Renders_Container_When_Shapes_Is_Empty()
    {
        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, new List<DiagramShapeDescriptor>()));

        Assert.NotNull(cut.Find(".mar-diagram"));
        Assert.Empty(cut.FindAll(".mar-diagram__shape"));
    }

    // ── Shape rendering ─────────────────────────────────────────────

    [Fact]
    public void Diagram_Renders_Shapes_From_Descriptor_List()
    {
        var shapes = CreateTestShapes();
        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes));

        var svgShapes = cut.FindAll(".mar-diagram__shape");
        Assert.Equal(3, svgShapes.Count);
    }

    [Fact]
    public void Diagram_Renders_Rectangle_As_Rect_Element()
    {
        var shapes = new List<DiagramShapeDescriptor>
        {
            new() { Id = "r1", Text = "Rect", Type = DiagramShapeType.Rectangle, X = 10, Y = 10 }
        };

        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes));

        var rect = cut.Find("rect.mar-diagram__shape");
        Assert.NotNull(rect);
    }

    [Fact]
    public void Diagram_Renders_Ellipse_As_Ellipse_Element()
    {
        var shapes = new List<DiagramShapeDescriptor>
        {
            new() { Id = "e1", Text = "Oval", Type = DiagramShapeType.Ellipse, X = 10, Y = 10 }
        };

        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes));

        var el = cut.Find("ellipse.mar-diagram__shape");
        Assert.NotNull(el);
    }

    [Fact]
    public void Diagram_Renders_Diamond_As_Polygon_Element()
    {
        var shapes = new List<DiagramShapeDescriptor>
        {
            new() { Id = "d1", Text = "Diamond", Type = DiagramShapeType.Diamond, X = 10, Y = 10 }
        };

        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes));

        var poly = cut.Find("polygon.mar-diagram__shape");
        Assert.NotNull(poly);
    }

    [Fact]
    public void Diagram_Renders_Triangle_As_Polygon_Element()
    {
        var shapes = new List<DiagramShapeDescriptor>
        {
            new() { Id = "t1", Text = "Tri", Type = DiagramShapeType.Triangle, X = 10, Y = 10 }
        };

        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes));

        var poly = cut.Find("polygon.mar-diagram__shape");
        Assert.NotNull(poly);
    }

    [Fact]
    public void Diagram_Renders_Circle_As_Circle_Element()
    {
        var shapes = new List<DiagramShapeDescriptor>
        {
            new() { Id = "c1", Text = "Circle", Type = DiagramShapeType.Circle, X = 10, Y = 10 }
        };

        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes));

        var circle = cut.Find("circle.mar-diagram__shape");
        Assert.NotNull(circle);
    }

    // ── Connection rendering ────────────────────────────────────────

    [Fact]
    public void Diagram_Renders_Connections_As_Lines()
    {
        var shapes = CreateTestShapes();
        var connections = CreateTestConnections();

        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes)
            .Add(x => x.Connections, connections));

        var lines = cut.FindAll("line.mar-diagram__connection");
        Assert.Equal(2, lines.Count);
    }

    [Fact]
    public void Diagram_Renders_Connection_Label_When_Text_Set()
    {
        var shapes = CreateTestShapes();
        var connections = CreateTestConnections();

        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes)
            .Add(x => x.Connections, connections));

        var labels = cut.FindAll(".mar-diagram__connection-label");
        // Only the first connection has Text = "Next"
        Assert.Single(labels);
        Assert.Contains("Next", labels[0].TextContent);
    }

    // ── OnShapeClick event ──────────────────────────────────────────

    [Fact]
    public void OnShapeClick_Fires_With_Correct_Shape()
    {
        DiagramShapeClickEventArgs? receivedArgs = null;
        var shapes = CreateTestShapes();

        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes)
            .Add(x => x.OnShapeClick, (DiagramShapeClickEventArgs args) => receivedArgs = args));

        // Click the first shape (a rect)
        var firstShape = cut.Find(".mar-diagram__shape");
        firstShape.Click();

        Assert.NotNull(receivedArgs);
        Assert.Equal("s1", receivedArgs!.Shape.Id);
        Assert.Equal("Start", receivedArgs.Shape.Text);
    }

    // ── CssClass passthrough ────────────────────────────────────────

    [Fact]
    public void Diagram_Applies_Shape_CssClass()
    {
        var shapes = new List<DiagramShapeDescriptor>
        {
            new() { Id = "x1", Text = "Custom", CssClass = "my-custom-class", X = 10, Y = 10 }
        };

        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes));

        var shape = cut.Find(".mar-diagram__shape.my-custom-class");
        Assert.NotNull(shape);
    }

    [Fact]
    public void Diagram_Applies_Connection_CssClass()
    {
        var shapes = new List<DiagramShapeDescriptor>
        {
            new() { Id = "a", X = 0, Y = 0 },
            new() { Id = "b", X = 100, Y = 100 }
        };
        var connections = new List<DiagramConnectionDescriptor>
        {
            new() { Id = "ab", FromShapeId = "a", ToShapeId = "b", CssClass = "highlight" }
        };

        var cut = Render<MariloDiagram>(p => p
            .Add(x => x.Shapes, shapes)
            .Add(x => x.Connections, connections));

        var conn = cut.Find("line.mar-diagram__connection.highlight");
        Assert.NotNull(conn);
    }
}
