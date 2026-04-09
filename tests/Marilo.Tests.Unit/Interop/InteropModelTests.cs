using Marilo.Components.Internal.Interop;
using Xunit;

namespace Marilo.Tests.Unit.Interop;

/// <summary>
/// Tests for interop DTOs, enums, and options records.
/// </summary>
public class InteropModelTests
{
    [Fact]
    public void ElementRect_Derived_Properties_Compute_Correctly()
    {
        var rect = new ElementRect(10, 20, 100, 50);
        Assert.Equal(20, rect.Top);
        Assert.Equal(10, rect.Left);
        Assert.Equal(70, rect.Bottom); // Y + Height
        Assert.Equal(110, rect.Right); // X + Width
    }

    [Fact]
    public void ResizeHandle_Flags_Compose_Correctly()
    {
        Assert.Equal(ResizeHandle.Top | ResizeHandle.Right, ResizeHandle.TopRight);
        Assert.Equal(ResizeHandle.Bottom | ResizeHandle.Left, ResizeHandle.BottomLeft);
    }

    [Fact]
    public void ResizeConstraints_Defaults_Are_Sensible()
    {
        var constraints = new ResizeConstraints();
        Assert.Equal(50, constraints.MinWidth);
        Assert.Equal(30, constraints.MinHeight);
        Assert.True(double.IsPositiveInfinity(constraints.MaxWidth));
        Assert.True(double.IsPositiveInfinity(constraints.MaxHeight));
        Assert.False(constraints.ClampToParent);
    }

    [Fact]
    public void PopupAnchorOptions_Defaults_Are_Sensible()
    {
        var options = new PopupAnchorOptions();
        Assert.Equal(PopupPlacement.Bottom, options.Placement);
        Assert.Equal(4, options.Offset);
        Assert.True(options.AutoFlip);
        Assert.Equal(8, options.ViewportMargin);
    }

    [Fact]
    public void DragStartOptions_Defaults()
    {
        var options = new DragStartOptions();
        Assert.True(options.DisableTextSelection);
        Assert.Null(options.ContainmentSelector);
    }

    [Fact]
    public void DownloadRequest_Required_Properties_Must_Be_Set()
    {
        var request = new DownloadRequest { FileName = "test.csv", Base64Content = "dGVzdA==" };
        Assert.Equal("test.csv", request.FileName);
        Assert.Equal("application/octet-stream", request.ContentType);
        Assert.Equal("dGVzdA==", request.Base64Content);
    }

    [Fact]
    public void ClipboardWriteRequest_Allows_Text_Or_Html_Or_Both()
    {
        var textOnly = new ClipboardWriteRequest { Text = "hello" };
        Assert.Equal("hello", textOnly.Text);
        Assert.Null(textOnly.Html);

        var both = new ClipboardWriteRequest { Text = "hello", Html = "<b>hello</b>" };
        Assert.Equal("hello", both.Text);
        Assert.Equal("<b>hello</b>", both.Html);
    }

    [Fact]
    public void ViewportRect_Properties()
    {
        var viewport = new ViewportRect(1920, 1080, 0, 100);
        Assert.Equal(1920, viewport.Width);
        Assert.Equal(1080, viewport.Height);
        Assert.Equal(0, viewport.ScrollX);
        Assert.Equal(100, viewport.ScrollY);
    }

    [Fact]
    public void PopupPlacement_Has_12_Values()
    {
        var values = Enum.GetValues<PopupPlacement>();
        Assert.Equal(12, values.Length);
    }
}
