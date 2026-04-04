using Bunit;
using Marilo.Components.DataDisplay;
using Marilo.Components.Forms.Inputs;
using Marilo.Components.Overlays;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Marilo.Tests.Unit.P1Components;

public class P1ComponentTests : MariloTestBase
{
    // ── Window Tests ───────────────────────────────────────────────────

    [Fact]
    public void Window_Renders_With_Title_And_CloseButton()
    {
        var cut = Render<MariloWindow>(parameters => parameters
            .Add(p => p.Visible, true)
            .Add(p => p.Title, "Test Window")
            .Add(p => p.ChildContent, (RenderFragment)(b => b.AddContent(0, "Window body")))
        );

        var markup = cut.Markup;
        Assert.Contains("Test Window", markup);
        Assert.Contains("mar-window__action-btn--close", markup);
        Assert.Contains("Window body", markup);
        Assert.Contains("role=\"dialog\"", markup);
    }

    [Fact]
    public void Window_Overlay_Renders_When_Modal()
    {
        var cut = Render<MariloWindow>(parameters => parameters
            .Add(p => p.Visible, true)
            .Add(p => p.Modal, true)
            .Add(p => p.Title, "Modal Window")
            .Add(p => p.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        Assert.Contains("mar-window-overlay", cut.Markup);
        Assert.Contains("mar-window--modal", cut.Markup);
        Assert.Contains("aria-modal=\"true\"", cut.Markup);
    }

    [Fact]
    public void Window_No_Overlay_When_Not_Modal()
    {
        var cut = Render<MariloWindow>(parameters => parameters
            .Add(p => p.Visible, true)
            .Add(p => p.Modal, false)
            .Add(p => p.Title, "Non-Modal Window")
            .Add(p => p.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        Assert.DoesNotContain("mar-window-overlay", cut.Markup);
    }

    [Fact]
    public void Window_CloseButton_Fires_VisibleChanged()
    {
        var visibleValue = true;
        var cut = Render<MariloWindow>(parameters => parameters
            .Add(p => p.Visible, true)
            .Add(p => p.Title, "Closeable Window")
            .Add(p => p.VisibleChanged, EventCallback.Factory.Create<bool>(this, v => visibleValue = v))
            .Add(p => p.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        var closeButton = cut.Find(".mar-window__action-btn--close");
        closeButton.Click();

        Assert.False(visibleValue);
    }

    [Fact]
    public void Window_Does_Not_Render_When_Not_Visible()
    {
        var cut = Render<MariloWindow>(parameters => parameters
            .Add(p => p.Visible, false)
            .Add(p => p.Title, "Hidden Window")
            .Add(p => p.ChildContent, (RenderFragment)(b => b.AddContent(0, "Content")))
        );

        Assert.Empty(cut.Markup.Trim());
    }

    // ── ListView Tests ─────────────────────────────────────────────────

    [Fact]
    public void ListView_Renders_Items_From_Data()
    {
        var items = new List<string> { "Alpha", "Beta", "Gamma" };

        var cut = Render<MariloListView<string>>(parameters => parameters
            .Add(p => p.Data, items)
            .Add(p => p.ItemTemplate, (RenderFragment<string>)(item =>
                (RenderTreeBuilder b) => { b.AddContent(0, item); }))
        );

        Assert.Contains("Alpha", cut.Markup);
        Assert.Contains("Beta", cut.Markup);
        Assert.Contains("Gamma", cut.Markup);
    }

    [Fact]
    public void ListView_Pager_Shows_When_Pageable()
    {
        var items = Enumerable.Range(1, 25).Select(i => $"Item {i}").ToList();

        var cut = Render<MariloListView<string>>(parameters => parameters
            .Add(p => p.Data, items)
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 10)
            .Add(p => p.ItemTemplate, (RenderFragment<string>)(item =>
                (RenderTreeBuilder b) => { b.AddContent(0, item); }))
        );

        Assert.Contains("mar-listview__pager", cut.Markup);
        Assert.Contains("Page 1 of 3", cut.Markup);
    }

    [Fact]
    public void ListView_No_Pager_When_Not_Pageable()
    {
        var items = new List<string> { "One", "Two" };

        var cut = Render<MariloListView<string>>(parameters => parameters
            .Add(p => p.Data, items)
            .Add(p => p.Pageable, false)
            .Add(p => p.ItemTemplate, (RenderFragment<string>)(item =>
                (RenderTreeBuilder b) => { b.AddContent(0, item); }))
        );

        Assert.DoesNotContain("mar-listview__pager", cut.Markup);
    }

    // ── Upload Tests ───────────────────────────────────────────────────

    [Fact]
    public void Upload_Renders_DropZone()
    {
        Services.AddSingleton(new HttpClient());

        var cut = Render<MariloUpload>(parameters => parameters
            .Add(p => p.SaveUrl, "/api/upload")
        );

        Assert.Contains("mar-upload-dropzone", cut.Markup);
        Assert.Contains("mar-upload", cut.Markup);
    }

    [Fact]
    public void Upload_Renders_Custom_Content()
    {
        Services.AddSingleton(new HttpClient());

        var cut = Render<MariloUpload>(parameters => parameters
            .Add(p => p.SaveUrl, "/api/upload")
            .Add(p => p.ChildContent, (RenderFragment)(b => b.AddContent(0, "Drop files here")))
        );

        Assert.Contains("Drop files here", cut.Markup);
    }

    [Fact]
    public void Upload_SelectFilesButtonTemplate_RendersCustomContent()
    {
        Services.AddSingleton(new HttpClient());

        var cut = Render<MariloUpload>(parameters => parameters
            .Add(p => p.SaveUrl, "/api/upload")
            .Add(p => p.SelectFilesButtonTemplate, (RenderFragment)(b => b.AddContent(0, "Choose your files")))
        );

        Assert.Contains("Choose your files", cut.Markup);
        Assert.DoesNotContain("Browse files", cut.Markup);
    }

    [Fact]
    public void Upload_FileTemplate_ParameterExists()
    {
        Services.AddSingleton(new HttpClient());

        // Verify the FileTemplate parameter can be set without error.
        // Full render testing requires file selection which is not available in bUnit.
        var cut = Render<MariloUpload>(parameters => parameters
            .Add(p => p.SaveUrl, "/api/upload")
            .Add(p => p.FileTemplate, (RenderFragment<UploadFileInfo>)(file =>
                (RenderTreeBuilder b) => { b.AddContent(0, $"custom-{file.Name}"); }))
        );

        // Component renders without error — parameter is wired correctly
        Assert.Contains("mar-upload", cut.Markup);
    }

    [Fact]
    public void Upload_FileInfoTemplate_ParameterExists()
    {
        Services.AddSingleton(new HttpClient());

        // Verify the FileInfoTemplate parameter can be set without error.
        // Full render testing requires file selection which is not available in bUnit.
        var cut = Render<MariloUpload>(parameters => parameters
            .Add(p => p.SaveUrl, "/api/upload")
            .Add(p => p.FileInfoTemplate, (RenderFragment<UploadFileInfo>)(file =>
                (RenderTreeBuilder b) => { b.AddContent(0, $"info-{file.Name}"); }))
        );

        // Component renders without error — parameter is wired correctly
        Assert.Contains("mar-upload", cut.Markup);
    }
}
