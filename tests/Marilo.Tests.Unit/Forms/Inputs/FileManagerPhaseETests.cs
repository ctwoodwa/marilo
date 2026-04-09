using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// bUnit tests for MariloFileManager Phase E gap resolutions:
/// - SPEC-FM-028: FileManagerUploadSettings — Upload button visibility, dialog open/close
/// - SPEC-FM-029: Preview pane — hidden by default, toggled by Details button, shows item details,
///                shows "No item selected" when nothing is selected
/// - Details toggle button wires to preview pane
/// </summary>
public class FileManagerPhaseETests : MariloTestBase
{
    // ─── helpers ────────────────────────────────────────────────────────────────

    private static FileManagerEntry MakeFile(
        string name,
        string path,
        long size = 2048,
        string? extension = ".txt",
        DateTime? created = null,
        DateTime? modified = null) =>
        new()
        {
            Id = path,
            Name = name,
            Path = path,
            IsDirectory = false,
            Size = size,
            Extension = extension,
            DateCreated = created ?? new DateTime(2025, 1, 15, 8, 0, 0),
            DateModified = modified ?? new DateTime(2025, 6, 1, 9, 0, 0)
        };

    private static FileManagerEntry MakeDir(string name, string path) =>
        new()
        {
            Id = path,
            Name = name,
            Path = path,
            IsDirectory = true
        };

    private static IEnumerable<FileManagerEntry> SampleItems() => new[]
    {
        MakeDir("Documents", "/Documents"),
        MakeFile("readme.txt", "/readme.txt", size: 1024, extension: ".txt"),
        MakeFile("report.pdf", "/report.pdf", size: 204800, extension: ".pdf")
    };

    // ─── SPEC-FM-029: Preview pane hidden by default ─────────────────────────────

    [Fact]
    public void PreviewPane_Hidden_By_Default()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false));

        // Pane must not be in DOM when _previewPaneVisible is false
        Assert.Empty(cut.FindAll(".mar-filemanager__preview"));
    }

    [Fact]
    public void PreviewPane_Not_Rendered_When_ShowPreviewPane_False()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowPreviewPane, false)
            .Add(x => x.ShowFolderTree, false));

        // Even after toggling, pane should remain hidden
        cut.Instance._previewPaneVisible = true;
        cut.Render(p => p
            .Add(x => x.ShowPreviewPane, false)
            .Add(x => x.ShowFolderTree, false));

        Assert.Empty(cut.FindAll(".mar-filemanager__preview"));
    }

    // ─── SPEC-FM-029: Preview pane shows when toggled ────────────────────────────

    [Fact]
    public void PreviewPane_Shows_When_TogglePreviewPane_Called()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false));

        cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());

        cut.Find(".mar-filemanager__preview"); // must not throw
    }

    [Fact]
    public void PreviewPane_Hides_When_TogglePreviewPane_Called_Twice()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false));

        cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());
        cut.Find(".mar-filemanager__preview"); // visible after first toggle

        cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());
        Assert.Empty(cut.FindAll(".mar-filemanager__preview")); // hidden after second toggle
    }

    // ─── Details toolbar button ───────────────────────────────────────────────────

    [Fact]
    public void DetailsButton_NotRendered_When_ShowPreviewPane_False()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowPreviewPane, false)
            .Add(x => x.ShowFolderTree, false));

        Assert.Empty(cut.FindAll(".mar-filemanager__details-btn"));
    }

    [Fact]
    public void DetailsButton_Rendered_When_ShowPreviewPane_True()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false));

        cut.Find(".mar-filemanager__details-btn"); // must not throw
    }

    [Fact]
    public async Task DetailsButton_Click_Opens_PreviewPane()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false));

        var detailsBtn = cut.Find(".mar-filemanager__details-btn");
        await cut.InvokeAsync(() => detailsBtn.Click());

        cut.Find(".mar-filemanager__preview"); // must not throw
        Assert.True(cut.Instance._previewPaneVisible);
    }

    // ─── SPEC-FM-029: Preview pane shows "No item selected" placeholder ──────────

    [Fact]
    public void PreviewPane_Shows_Placeholder_When_No_Selection()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false));

        cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());

        var pane = cut.Find(".mar-filemanager__preview");
        Assert.Contains("No item selected", pane.TextContent);
    }

    // ─── SPEC-FM-029: Preview pane displays selected item details ────────────────

    [Fact]
    public async Task PreviewPane_Shows_FileName_Of_Selected_Item()
    {
        var items = SampleItems().ToList();
        var file = items.First(e => !e.IsDirectory);

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.Path, "/")
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItems, new[] { file }));

        await cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());

        var pane = cut.Find(".mar-filemanager__preview");
        Assert.Contains(file.Name, pane.TextContent);
    }

    [Fact]
    public async Task PreviewPane_Shows_Extension_Of_Selected_File()
    {
        var items = SampleItems().ToList();
        var file = items.First(e => !e.IsDirectory);

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.Path, "/")
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItems, new[] { file }));

        await cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());

        var pane = cut.Find(".mar-filemanager__preview");
        Assert.Contains(file.Extension!, pane.TextContent);
    }

    [Fact]
    public async Task PreviewPane_Shows_Formatted_Size_Of_Selected_File()
    {
        var file = MakeFile("big.pdf", "/big.pdf", size: 2048, extension: ".pdf");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItems, new[] { file }));

        await cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());

        var pane = cut.Find(".mar-filemanager__preview");
        // 2048 bytes = 2.0 KB
        Assert.Contains("KB", pane.TextContent);
    }

    [Fact]
    public async Task PreviewPane_Shows_DateModified_Of_Selected_Item()
    {
        var modified = new DateTime(2025, 6, 1, 9, 0, 0);
        var file = MakeFile("readme.txt", "/readme.txt", modified: modified);

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItems, new[] { file }));

        await cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());

        var pane = cut.Find(".mar-filemanager__preview");
        Assert.Contains("2025-06-01", pane.TextContent);
    }

    [Fact]
    public async Task PreviewPane_Shows_DateCreated_Of_Selected_Item()
    {
        var created = new DateTime(2025, 1, 15, 8, 0, 0);
        var file = MakeFile("readme.txt", "/readme.txt", created: created);

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItems, new[] { file }));

        await cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());

        var pane = cut.Find(".mar-filemanager__preview");
        Assert.Contains("2025-01-15", pane.TextContent);
    }

    [Fact]
    public async Task PreviewPane_Shows_Folder_Type_For_Directory()
    {
        var dir = MakeDir("Documents", "/Documents");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { dir })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItems, new[] { dir }));

        await cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());

        var pane = cut.Find(".mar-filemanager__preview");
        Assert.Contains("Folder", pane.TextContent);
    }

    // ─── SPEC-FM-028: Upload button visibility ────────────────────────────────────

    [Fact]
    public void UploadButton_Hidden_When_UploadSettings_Null()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        Assert.Empty(cut.FindAll(".mar-filemanager__upload-btn"));
    }

    [Fact]
    public void UploadButton_Visible_When_UploadSettings_Provided()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.UploadSettings, new FileManagerUploadSettings
            {
                SaveUrl = "/api/upload",
                Multiple = true
            }));

        cut.Find(".mar-filemanager__upload-btn"); // must not throw
    }

    // ─── SPEC-FM-028: Upload dialog open/close ────────────────────────────────────

    [Fact]
    public async Task UploadDialog_Opens_On_Upload_Button_Click()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.UploadSettings, new FileManagerUploadSettings { SaveUrl = "/api/upload" }));

        var uploadBtn = cut.Find(".mar-filemanager__upload-btn");
        await cut.InvokeAsync(() => uploadBtn.Click());

        cut.Find(".mar-filemanager__upload-dialog"); // must not throw
        Assert.True(cut.Instance._uploadDialogVisible);
    }

    [Fact]
    public async Task UploadDialog_Closes_When_CloseUploadDialog_Called()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.UploadSettings, new FileManagerUploadSettings { SaveUrl = "/api/upload" }));

        await cut.InvokeAsync(() => cut.Instance.ShowUploadDialog());
        cut.Find(".mar-filemanager__upload-dialog"); // open

        await cut.InvokeAsync(() => cut.Instance.CloseUploadDialog());
        Assert.Empty(cut.FindAll(".mar-filemanager__upload-dialog")); // closed
    }

    [Fact]
    public async Task UploadDialog_Contains_File_Input()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.UploadSettings, new FileManagerUploadSettings
            {
                SaveUrl = "/api/upload",
                Multiple = true
            }));

        await cut.InvokeAsync(() => cut.Instance.ShowUploadDialog());

        cut.Find(".mar-filemanager__upload-input"); // must not throw
        var fileInput = cut.Find("input[type='file']");
        Assert.NotNull(fileInput);
    }

    [Fact]
    public async Task UploadDialog_Hidden_By_Default()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.UploadSettings, new FileManagerUploadSettings { SaveUrl = "/api/upload" }));

        // Dialog must not appear before the button is clicked
        Assert.Empty(cut.FindAll(".mar-filemanager__upload-dialog"));
        Assert.False(cut.Instance._uploadDialogVisible);
    }

    // ─── FileManagerUploadSettings defaults ───────────────────────────────────────

    [Fact]
    public void UploadSettings_Multiple_Defaults_To_True()
    {
        var settings = new FileManagerUploadSettings();
        Assert.True(settings.Multiple);
    }

    [Fact]
    public void UploadSettings_Properties_Are_Settable()
    {
        var settings = new FileManagerUploadSettings
        {
            SaveUrl = "/api/upload",
            AllowedExtensions = new[] { ".jpg", ".png" },
            MaxFileSize = 5_000_000,
            Multiple = false
        };

        Assert.Equal("/api/upload", settings.SaveUrl);
        Assert.Equal(new[] { ".jpg", ".png" }, settings.AllowedExtensions);
        Assert.Equal(5_000_000, settings.MaxFileSize);
        Assert.False(settings.Multiple);
    }

    // ─── Preview pane: files area has --with-preview modifier ────────────────────

    [Fact]
    public async Task FilesArea_HasWithPreview_Class_When_PaneOpen()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.TogglePreviewPane());

        var filesDiv = cut.Find(".mar-filemanager__files");
        Assert.Contains("mar-filemanager__files--with-preview", filesDiv.ClassList);
    }

    [Fact]
    public void FilesArea_DoesNotHave_WithPreview_Class_When_PaneClosed()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowPreviewPane, true)
            .Add(x => x.ShowFolderTree, false));

        var filesDiv = cut.Find(".mar-filemanager__files");
        Assert.DoesNotContain("mar-filemanager__files--with-preview", filesDiv.ClassList);
    }
}
