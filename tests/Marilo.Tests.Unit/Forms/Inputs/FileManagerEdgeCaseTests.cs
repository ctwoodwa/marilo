using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// Edge-case tests for MariloFileManager to close remaining coverage gaps:
/// - HandleRenameKeyDown Enter/Escape paths
/// - ContextMenuDownload via context menu
/// - Sort by Extension
/// - OpenItem for files fires OnOpen (not navigation)
/// - OpenItem for directories navigates (fires PathChanged, not OnOpen)
/// - ShowFolderTree false hides sidebar
/// - NavigateUp from nested path
/// - ShowUploadDialog no-op when UploadSettings null
/// - Dispose cancels pending reads
/// - Sort by Type field
/// - Empty rename text commit
/// - Double-toggle preview pane state
/// - SelectedItem returns first item or default
/// </summary>
public class FileManagerEdgeCaseTests : MariloTestBase
{
    // ─── helpers ────────────────────────────────────────────────────────────────

    private static FileManagerEntry MakeFile(
        string name,
        string path,
        long size = 1024,
        string? extension = ".txt",
        DateTime? modified = null) =>
        new()
        {
            Id = path,
            Name = name,
            Path = path,
            IsDirectory = false,
            Size = size,
            Extension = extension,
            DateModified = modified ?? new DateTime(2025, 6, 1)
        };

    private static FileManagerEntry MakeDir(string name, string path) =>
        new()
        {
            Id = path,
            Name = name,
            Path = path,
            IsDirectory = true
        };

    // ─── HandleRenameKeyDown: Enter commits ───��─────────────────────────────────

    [Fact]
    public async Task HandleRenameKeyDown_Enter_Commits_Rename()
    {
        FileManagerUpdateEventArgs<FileManagerEntry>? captured = null;
        var file = MakeFile("readme.txt", "/readme.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.AllowRename, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnUpdate, EventCallback.Factory.Create<FileManagerUpdateEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        await cut.InvokeAsync(() => cut.Instance.StartRename(file));
        cut.Instance._renameText = "renamed.txt";
        await cut.InvokeAsync(() => cut.Instance.HandleRenameKeyDown(new KeyboardEventArgs { Key = "Enter" }));

        Assert.NotNull(captured);
        Assert.Equal("renamed.txt", file.Name);
        Assert.Empty(cut.FindAll(".mar-filemanager__rename-input"));
    }

    // ─── HandleRenameKeyDown: Escape cancels ────────────────────────────────────

    [Fact]
    public async Task HandleRenameKeyDown_Escape_Cancels_Rename()
    {
        FileManagerUpdateEventArgs<FileManagerEntry>? captured = null;
        var file = MakeFile("readme.txt", "/readme.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.AllowRename, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnUpdate, EventCallback.Factory.Create<FileManagerUpdateEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        await cut.InvokeAsync(() => cut.Instance.StartRename(file));
        cut.Instance._renameText = "should-not-apply.txt";
        await cut.InvokeAsync(() => cut.Instance.HandleRenameKeyDown(new KeyboardEventArgs { Key = "Escape" }));

        Assert.Null(captured);
        Assert.Equal("readme.txt", file.Name);
        Assert.Empty(cut.FindAll(".mar-filemanager__rename-input"));
    }

    // ��── HandleRenameKeyDown: Other key does nothing ────────────────────────────

    [Fact]
    public async Task HandleRenameKeyDown_OtherKey_Does_Nothing()
    {
        var file = MakeFile("readme.txt", "/readme.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.AllowRename, true)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.StartRename(file));
        await cut.InvokeAsync(() => cut.Instance.HandleRenameKeyDown(new KeyboardEventArgs { Key = "a" }));

        // Rename input should still be visible
        Assert.NotEmpty(cut.FindAll(".mar-filemanager__rename-input"));
    }

    // ─── ContextMenuDownload fires OnDownload then closes menu ───────────────────

    [Fact]
    public async Task ContextMenuDownload_FiresOnDownload_And_ClosesMenu()
    {
        FileManagerDownloadEventArgs<FileManagerEntry>? captured = null;
        var file = MakeFile("report.pdf", "/report.pdf");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnDownload, EventCallback.Factory.Create<FileManagerDownloadEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        // Open the context menu then invoke download
        await cut.InvokeAsync(() => cut.Instance.ShowContextMenu(
            file, new MouseEventArgs { ClientX = 50, ClientY = 50 }));
        await cut.InvokeAsync(() => cut.Instance.ContextMenuDownload());

        Assert.NotNull(captured);
        Assert.Equal(file, captured!.Item);
        // Menu must be closed after download
        Assert.Empty(cut.FindAll(".mar-filemanager__context-menu"));
    }

    // ─── Sort by Extension ────────���─────────────────────────────────────────────

    [Fact]
    public void Sort_ByExtension_Ascending()
    {
        var items = new[]
        {
            MakeFile("archive.zip", "/archive.zip", extension: ".zip"),
            MakeFile("readme.txt", "/readme.txt", extension: ".txt"),
            MakeFile("photo.jpg", "/photo.jpg", extension: ".jpg")
        };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.ShowFolderTree, false));

        cut.InvokeAsync(() => cut.Instance.SetSortField("Extension"));

        var names = cut.FindAll(".mar-filemanager__name")
            .Select(n => n.TextContent.Trim())
            .ToList();

        // .jpg < .txt < .zip alphabetically
        Assert.True(names.IndexOf("photo.jpg") < names.IndexOf("readme.txt"));
        Assert.True(names.IndexOf("readme.txt") < names.IndexOf("archive.zip"));
    }

    // ─── OpenItem: file fires OnOpen, does NOT navigate ─────────────────────────

    [Fact]
    public async Task OpenItem_File_Fires_OnOpen_Not_PathChanged()
    {
        FileManagerEntry? openedItem = null;
        string? navigatedPath = null;
        var file = MakeFile("readme.txt", "/readme.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnOpen, EventCallback.Factory.Create<FileManagerEntry>(this, item => openedItem = item))
            .Add(x => x.PathChanged, EventCallback.Factory.Create<string>(this, p => navigatedPath = p)));

        await cut.InvokeAsync(() => cut.Instance.OpenItem(file));

        Assert.NotNull(openedItem);
        Assert.Equal(file, openedItem);
        Assert.Null(navigatedPath); // should NOT navigate
    }

    // ─── OpenItem: directory navigates, does NOT fire OnOpen ─────────────────────

    [Fact]
    public async Task OpenItem_Directory_Navigates_Not_OnOpen()
    {
        FileManagerEntry? openedItem = null;
        string? navigatedPath = null;
        var dir = MakeDir("Documents", "/Documents");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { dir })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnOpen, EventCallback.Factory.Create<FileManagerEntry>(this, item => openedItem = item))
            .Add(x => x.PathChanged, EventCallback.Factory.Create<string>(this, p => navigatedPath = p)));

        await cut.InvokeAsync(() => cut.Instance.OpenItem(dir));

        Assert.Null(openedItem); // should NOT fire OnOpen for directories
        Assert.Equal("/Documents", navigatedPath);
    }

    // ─── ShowFolderTree false hides sidebar ─────────────────────────────────────

    [Fact]
    public void ShowFolderTree_False_Hides_Sidebar()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { MakeDir("Docs", "/Docs") })
            .Add(x => x.ShowFolderTree, false));

        Assert.Empty(cut.FindAll(".mar-filemanager__sidebar"));
        Assert.Empty(cut.FindAll(".mar-filemanager__tree"));
    }

    [Fact]
    public void ShowFolderTree_True_Shows_Sidebar()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { MakeDir("Docs", "/Docs") })
            .Add(x => x.ShowFolderTree, true));

        cut.Find(".mar-filemanager__sidebar"); // must not throw
        cut.Find(".mar-filemanager__tree");
    }

    // ─── NavigateUp from nested path ────────────────────────────────────────────

    [Fact]
    public async Task NavigateUp_From_Nested_Path_Goes_To_Parent()
    {
        string? navigatedPath = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents/Reports")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.PathChanged, EventCallback.Factory.Create<string>(this, p => navigatedPath = p)));

        await cut.InvokeAsync(() => cut.Instance.NavigateUp());

        Assert.Equal("/Documents", navigatedPath);
    }

    [Fact]
    public async Task NavigateUp_From_First_Level_Goes_To_Root()
    {
        string? navigatedPath = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.PathChanged, EventCallback.Factory.Create<string>(this, p => navigatedPath = p)));

        await cut.InvokeAsync(() => cut.Instance.NavigateUp());

        Assert.Equal("/", navigatedPath);
    }

    // ─── ShowUploadDialog no-op when UploadSettings null ────────────────────────

    [Fact]
    public void ShowUploadDialog_NoOp_When_UploadSettings_Null()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        cut.InvokeAsync(() => cut.Instance.ShowUploadDialog());

        Assert.False(cut.Instance._uploadDialogVisible);
        Assert.Empty(cut.FindAll(".mar-filemanager__upload-dialog"));
    }

    // ─── Sort by Type field ─────────────────────────────────────────────────────

    [Fact]
    public void Sort_ByType_Ascending_Groups_By_IsDirectory_ThenByName()
    {
        var items = new[]
        {
            MakeFile("readme.txt", "/readme.txt"),
            MakeDir("Zebra", "/Zebra"),
            MakeFile("alpha.pdf", "/alpha.pdf"),
            MakeDir("Alpha", "/Alpha")
        };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.ShowFolderTree, false));

        cut.InvokeAsync(() => cut.Instance.SetSortField("Type"));

        var names = cut.FindAll(".mar-filemanager__name")
            .Select(n => n.TextContent.Trim())
            .ToList();

        // Type ascending: OrderBy(IsDirectory) puts files (false) first, then dirs (true)
        // Within each group, sorted by name ascending
        Assert.Equal("alpha.pdf", names[0]);
        Assert.Equal("readme.txt", names[1]);
        Assert.Equal("Alpha", names[2]);
        Assert.Equal("Zebra", names[3]);
    }

    // ─── SelectedItem returns first or default ──���───────────────────────────────

    [Fact]
    public void SelectedItem_Returns_Null_When_Nothing_Selected()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        Assert.Null(cut.Instance.SelectedItem);
    }

    [Fact]
    public async Task SelectedItem_Returns_First_Selected_Item()
    {
        var file = MakeFile("readme.txt", "/readme.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.SelectItem(file));

        Assert.Equal(file, cut.Instance.SelectedItem);
    }

    // ─── UpdateRenameText updates internal text ─────────────────────────────────

    [Fact]
    public void UpdateRenameText_Sets_Internal_Value()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        cut.Instance.UpdateRenameText("new-name.txt");

        Assert.Equal("new-name.txt", cut.Instance._renameText);
    }

    // ─── GetContainerStyle combines height and width ────────────────────────────

    [Fact]
    public void GetContainerStyle_Returns_Combined_Height_And_Width()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Height, "400px")
            .Add(x => x.Width, "800px")
            .Add(x => x.ShowFolderTree, false));

        var style = cut.Instance.GetContainerStyle();
        Assert.Contains("height:400px", style);
        Assert.Contains("width:800px", style);
    }

    [Fact]
    public void GetContainerStyle_Returns_Empty_When_No_Dimensions()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        Assert.Equal("", cut.Instance.GetContainerStyle());
    }

    // ─── CommitRename with no active rename is no-op ──────────────────────���─────

    [Fact]
    public async Task CommitRename_NoOp_When_No_Active_Rename()
    {
        FileManagerUpdateEventArgs<FileManagerEntry>? captured = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnUpdate, EventCallback.Factory.Create<FileManagerUpdateEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        await cut.InvokeAsync(() => cut.Instance.CommitRename());

        Assert.Null(captured); // no update fired
    }

    // ─── Breadcrumb separator rendered between non-last segments ─────────────────

    [Fact]
    public void Breadcrumb_Separator_Rendered_Between_Segments()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents/Reports")
            .Add(x => x.ShowFolderTree, false));

        var separators = cut.FindAll(".mar-filemanager__breadcrumb-separator");
        // For 3 segments (/, Documents, Reports), there should be 2 separators
        Assert.Equal(2, separators.Count);
    }

    // ─── Folder tree item click navigates to folder ─────────────────────────────

    [Fact]
    public async Task FolderTree_Item_Click_Navigates()
    {
        string? navigatedPath = null;
        var dir = MakeDir("Documents", "/Documents");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { dir })
            .Add(x => x.ShowFolderTree, true)
            .Add(x => x.PathChanged, EventCallback.Factory.Create<string>(this, p => navigatedPath = p)));

        var treeItem = cut.Find(".mar-filemanager__tree li");
        await cut.InvokeAsync(() => treeItem.Click());

        Assert.Equal("/Documents", navigatedPath);
    }
}
