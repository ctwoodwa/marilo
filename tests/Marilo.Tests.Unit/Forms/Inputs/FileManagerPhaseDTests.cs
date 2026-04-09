using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// bUnit tests for MariloFileManager Phase D gap resolutions:
/// - SPEC-FM-025: Right-click context menu appears with correct items
/// - SPEC-FM-026: Inline rename UI — start, commit, cancel
/// - SPEC-FM-027: Delete confirmation dialog — confirm, cancel
/// - SPEC-FM-012: OnCreate wired to toolbar New Folder button (verify)
/// - AllowRename gates rename menu item and rename action
/// - AllowDelete gates delete menu item and delete action
/// </summary>
public class FileManagerPhaseDTests : MariloTestBase
{
    // ─── helpers ────────────────────────────────────────────────────────────────

    private static FileManagerEntry MakeFile(string name, string path, long size = 1024) =>
        new()
        {
            Id = path,
            Name = name,
            Path = path,
            IsDirectory = false,
            Size = size,
            DateModified = new DateTime(2025, 6, 1, 9, 0, 0)
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
        MakeFile("readme.txt", "/readme.txt"),
        MakeFile("notes.txt", "/notes.txt")
    };

    private static MouseEventArgs At(double x, double y) =>
        new() { ClientX = x, ClientY = y };

    // ─── SPEC-FM-025: Context menu visibility ───────────────────────────────────

    [Fact]
    public async Task ContextMenu_NotVisible_By_Default()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => { });

        Assert.Empty(cut.FindAll(".mar-filemanager__context-menu"));
    }

    [Fact]
    public async Task ContextMenu_Appears_After_ShowContextMenu_Call()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.ShowFolderTree, false));

        var entry = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ShowContextMenu(entry, At(100, 200)));

        var menu = cut.Find(".mar-filemanager__context-menu");
        Assert.NotNull(menu);
    }

    [Fact]
    public async Task ContextMenu_CloseContextMenu_Hides_Menu()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.ShowFolderTree, false));

        var entry = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ShowContextMenu(entry, At(100, 200)));
        await cut.InvokeAsync(() => cut.Instance.CloseContextMenu());

        Assert.Empty(cut.FindAll(".mar-filemanager__context-menu"));
    }

    // ─── SPEC-FM-025: Context menu items gated by permissions ──────────────────

    [Fact]
    public async Task ContextMenu_File_AllPermissions_Shows_Rename_Download_Delete()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowRename, true)
            .Add(x => x.AllowDelete, true)
            .Add(x => x.ShowFolderTree, false));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ShowContextMenu(file, At(0, 0)));

        Assert.NotEmpty(cut.FindAll(".mar-filemanager__context-menu-item--rename"));
        Assert.NotEmpty(cut.FindAll(".mar-filemanager__context-menu-item--download"));
        Assert.NotEmpty(cut.FindAll(".mar-filemanager__context-menu-item--delete"));
    }

    [Fact]
    public async Task ContextMenu_AllowRename_False_Hides_Rename_Item()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowRename, false)
            .Add(x => x.AllowDelete, true)
            .Add(x => x.ShowFolderTree, false));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ShowContextMenu(file, At(0, 0)));

        Assert.Empty(cut.FindAll(".mar-filemanager__context-menu-item--rename"));
    }

    [Fact]
    public async Task ContextMenu_AllowDelete_False_Hides_Delete_Item()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowRename, true)
            .Add(x => x.AllowDelete, false)
            .Add(x => x.ShowFolderTree, false));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ShowContextMenu(file, At(0, 0)));

        Assert.Empty(cut.FindAll(".mar-filemanager__context-menu-item--delete"));
    }

    [Fact]
    public async Task ContextMenu_Directory_Hides_Download_Item()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowRename, true)
            .Add(x => x.AllowDelete, true)
            .Add(x => x.ShowFolderTree, false));

        var dir = SampleItems().First(e => e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ShowContextMenu(dir, At(0, 0)));

        Assert.Empty(cut.FindAll(".mar-filemanager__context-menu-item--download"));
    }

    [Fact]
    public async Task ContextMenu_NoPermissions_Shows_Only_Download_For_File()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowRename, false)
            .Add(x => x.AllowDelete, false)
            .Add(x => x.ShowFolderTree, false));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ShowContextMenu(file, At(0, 0)));

        Assert.Empty(cut.FindAll(".mar-filemanager__context-menu-item--rename"));
        Assert.NotEmpty(cut.FindAll(".mar-filemanager__context-menu-item--download"));
        Assert.Empty(cut.FindAll(".mar-filemanager__context-menu-item--delete"));
    }

    // ─── SPEC-FM-026: Inline rename ─────────────────────────────────────────────

    [Fact]
    public async Task StartRename_Shows_Rename_Input_For_Item()
    {
        var items = SampleItems().ToList();
        var file = items.First(e => !e.IsDirectory);

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.AllowRename, true)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.StartRename(file));

        var inputs = cut.FindAll(".mar-filemanager__rename-input");
        Assert.Single(inputs);
        Assert.Equal(file.Name, inputs[0].GetAttribute("value"));
    }

    [Fact]
    public async Task StartRename_AllowRename_False_Does_Not_Show_Input()
    {
        var items = SampleItems().ToList();
        var file = items.First(e => !e.IsDirectory);

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.AllowRename, false)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.StartRename(file));

        Assert.Empty(cut.FindAll(".mar-filemanager__rename-input"));
    }

    [Fact]
    public async Task StartRename_Fires_OnEdit_Event()
    {
        FileManagerEditEventArgs<FileManagerEntry>? captured = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowRename, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnEdit, EventCallback.Factory.Create<FileManagerEditEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.StartRename(file));

        Assert.NotNull(captured);
        Assert.Equal(file, captured!.Item);
    }

    [Fact]
    public async Task CommitRename_Fires_OnUpdate_Event()
    {
        FileManagerUpdateEventArgs<FileManagerEntry>? captured = null;

        // Use a mutable copy so the name can be written back
        var file = MakeFile("readme.txt", "/readme.txt");
        var items = new[] { file };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.AllowRename, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnUpdate, EventCallback.Factory.Create<FileManagerUpdateEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        await cut.InvokeAsync(() => cut.Instance.StartRename(file));
        cut.Instance._renameText = "renamed.txt";
        await cut.InvokeAsync(() => cut.Instance.CommitRename());

        Assert.NotNull(captured);
        Assert.Equal(file, captured!.Item);
    }

    [Fact]
    public async Task CommitRename_Updates_Item_Name_Property()
    {
        var file = MakeFile("readme.txt", "/readme.txt");
        var items = new[] { file };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.AllowRename, true)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.StartRename(file));
        cut.Instance._renameText = "renamed.txt";
        await cut.InvokeAsync(() => cut.Instance.CommitRename());

        Assert.Equal("renamed.txt", file.Name);
    }

    [Fact]
    public async Task CommitRename_Hides_Rename_Input()
    {
        var file = MakeFile("readme.txt", "/readme.txt");
        var items = new[] { file };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.AllowRename, true)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.StartRename(file));
        await cut.InvokeAsync(() => cut.Instance.CommitRename());

        Assert.Empty(cut.FindAll(".mar-filemanager__rename-input"));
    }

    [Fact]
    public async Task CancelRename_Hides_Input_Without_Firing_OnUpdate()
    {
        FileManagerUpdateEventArgs<FileManagerEntry>? captured = null;

        var file = MakeFile("readme.txt", "/readme.txt");
        var items = new[] { file };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.AllowRename, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnUpdate, EventCallback.Factory.Create<FileManagerUpdateEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        await cut.InvokeAsync(() => cut.Instance.StartRename(file));
        await cut.InvokeAsync(() => cut.Instance.CancelRename());

        Assert.Empty(cut.FindAll(".mar-filemanager__rename-input"));
        Assert.Null(captured);
        Assert.Equal("readme.txt", file.Name); // name unchanged
    }

    // ─── SPEC-FM-027: Delete confirmation ──────────────────────────────────────

    [Fact]
    public async Task ConfirmDelete_Shows_Confirmation_Dialog()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowDelete, true)
            .Add(x => x.ShowFolderTree, false));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ConfirmDelete(file));

        var dialog = cut.Find(".mar-filemanager__confirm-dialog");
        Assert.NotNull(dialog);
        Assert.Contains(file.Name, dialog.TextContent);
    }

    [Fact]
    public async Task ConfirmDelete_AllowDelete_False_Does_Not_Show_Dialog()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowDelete, false)
            .Add(x => x.ShowFolderTree, false));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ConfirmDelete(file));

        Assert.Empty(cut.FindAll(".mar-filemanager__confirm-dialog"));
    }

    [Fact]
    public async Task ExecuteDelete_Fires_OnDelete_Event()
    {
        FileManagerDeleteEventArgs<FileManagerEntry>? captured = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowDelete, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnDelete, EventCallback.Factory.Create<FileManagerDeleteEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ConfirmDelete(file));
        await cut.InvokeAsync(() => cut.Instance.ExecuteDelete());

        Assert.NotNull(captured);
        Assert.Equal(file, captured!.Item);
    }

    [Fact]
    public async Task ExecuteDelete_Closes_Confirmation_Dialog()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowDelete, true)
            .Add(x => x.ShowFolderTree, false));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ConfirmDelete(file));
        await cut.InvokeAsync(() => cut.Instance.ExecuteDelete());

        Assert.Empty(cut.FindAll(".mar-filemanager__confirm-dialog"));
    }

    [Fact]
    public async Task CancelDelete_Closes_Dialog_Without_Firing_OnDelete()
    {
        FileManagerDeleteEventArgs<FileManagerEntry>? captured = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.AllowDelete, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnDelete, EventCallback.Factory.Create<FileManagerDeleteEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.ConfirmDelete(file));
        await cut.InvokeAsync(() => cut.Instance.CancelDelete());

        Assert.Empty(cut.FindAll(".mar-filemanager__confirm-dialog"));
        Assert.Null(captured);
    }

    // ─── SPEC-FM-012: OnCreate wired to toolbar ─────────────────────────────────

    [Fact]
    public async Task CreateFolder_Fires_OnCreate_Event()
    {
        FileManagerCreateEventArgs<FileManagerEntry>? captured = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.AllowCreate, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnCreate, EventCallback.Factory.Create<FileManagerCreateEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        var newFolderBtn = cut.FindAll("button").First(b => b.TextContent.Contains("New Folder"));
        await cut.InvokeAsync(() => newFolderBtn.Click());

        Assert.NotNull(captured);
    }

    [Fact]
    public async Task CreateFolder_Uses_OnModelInit_When_Provided()
    {
        FileManagerCreateEventArgs<FileManagerEntry>? captured = null;
        var template = new FileManagerEntry { Name = "New Folder", Path = "/New Folder", IsDirectory = true };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.AllowCreate, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnModelInit, () => template)
            .Add(x => x.OnCreate, EventCallback.Factory.Create<FileManagerCreateEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        await cut.InvokeAsync(() => cut.Instance.CreateFolder());

        Assert.NotNull(captured);
        Assert.Same(template, captured!.Item);
    }

    // ─── Download event ─────────────────────────────────────────────────────────

    [Fact]
    public async Task DownloadItem_Fires_OnDownload_Event()
    {
        FileManagerDownloadEventArgs<FileManagerEntry>? captured = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnDownload, EventCallback.Factory.Create<FileManagerDownloadEventArgs<FileManagerEntry>>(
                this, args => captured = args)));

        var file = SampleItems().First(e => !e.IsDirectory);
        await cut.InvokeAsync(() => cut.Instance.DownloadItem(file));

        Assert.NotNull(captured);
        Assert.Equal(file, captured!.Item);
    }

    // ─── IsRenaming helper ──────────────────────────────────────────────────────

    [Fact]
    public async Task IsRenaming_Returns_True_Only_For_Active_Rename_Item()
    {
        var file1 = MakeFile("readme.txt", "/readme.txt");
        var file2 = MakeFile("notes.txt", "/notes.txt");
        var items = new[] { file1, file2 };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.AllowRename, true)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.StartRename(file1));

        Assert.True(cut.Instance.IsRenaming(file1));
        Assert.False(cut.Instance.IsRenaming(file2));
    }
}
