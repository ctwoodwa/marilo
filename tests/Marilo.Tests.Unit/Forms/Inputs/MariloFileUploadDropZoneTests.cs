using Bunit;
using Marilo.Components.Forms.Inputs;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// Tests for MariloFileUpload DropZoneId JS interop wiring.
/// </summary>
public class MariloFileUploadDropZoneTests : MariloTestBase
{
    [Fact]
    public void Renders_Without_Error_When_DropZoneId_Is_Null()
    {
        var cut = Render<MariloFileUpload>();
        Assert.NotNull(cut.Markup);
        Assert.Contains("mar-file-upload", cut.Markup);
    }

    [Fact]
    public void Renders_Without_Error_When_DropZoneId_Is_Set()
    {
        var cut = Render<MariloFileUpload>(p => p.Add(c => c.DropZoneId, "my-drop-zone"));
        Assert.NotNull(cut.Markup);
        Assert.Contains("mar-file-upload", cut.Markup);
    }

    [Fact]
    public void Component_Implements_IAsyncDisposable()
    {
        var cut = Render<MariloFileUpload>();
        Assert.IsAssignableFrom<IAsyncDisposable>(cut.Instance);
    }

    [Fact]
    public void DropZoneId_Change_From_One_Value_To_Another_Does_Not_Throw()
    {
        var cut = Render<MariloFileUpload>(p => p.Add(c => c.DropZoneId, "zone1"));
        Assert.Contains("mar-file-upload", cut.Markup);

        // Re-render with a different DropZoneId — exercises OnParametersSet re-registration path
        var exception = Record.Exception(() =>
            cut.Render(p => p.Add(c => c.DropZoneId, "zone2")));

        Assert.Null(exception);
        Assert.Contains("mar-file-upload", cut.Markup);
    }

    [Fact]
    public void DropZoneId_Cleared_To_Null_After_Being_Set_Does_Not_Throw()
    {
        var cut = Render<MariloFileUpload>(p => p.Add(c => c.DropZoneId, "zone1"));
        Assert.Contains("mar-file-upload", cut.Markup);

        // Re-render with DropZoneId=null — exercises the "unregister" branch of OnParametersSet
        var exception = Record.Exception(() =>
            cut.Render(p => p.Add(c => c.DropZoneId, (string?)null)));

        Assert.Null(exception);
        Assert.Contains("mar-file-upload", cut.Markup);
    }
}
