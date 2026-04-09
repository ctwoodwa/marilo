using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// bUnit tests for MariloFileManager Phase B gap resolutions:
/// - OnEdit fires with correct FileManagerEditEventArgs
/// - OnUpdate fires with correct FileManagerUpdateEventArgs
/// - OnDownload fires with correct FileManagerDownloadEventArgs (cancellable)
/// - OnModelInit callback used in CreateFolder
/// - SelectedItems two-way binding works
/// - SelectedItemsChanged fires on selection
/// - AllowDelete gates DeleteItem
/// - Rebind triggers OnRead
/// - ViewChanged fires when view changes
/// </summary>
public class FileManagerPhaseBTests : MariloTestBase
{
    // ─── helpers ────────────────────────────────────────────────────────────────

    private static FileManagerEntry MakeEntry(string name, string path, bool isDir = false, long size = 0) =>
        new()
        {
            Id = path,
            Name = name,
            Path = path,
            IsDirectory = isDir,
            Size = size,
            DateModified = new DateTime(2025, 6, 1, 9, 0, 0)
        };

    private static FileManagerEntry[] SingleFile() =>
        new[] { MakeEntry("report.pdf", "/report.pdf", size: 2048) };

    // ─── OnEdit ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task OnEdit_Fires_With_Correct_Item()
    {
        FileManagerEditEventArgs<FileManagerEntry>? captured = null;
        var item = MakeEntry("report.pdf", "/report.pdf");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnEdit, EventCallback.Factory.Create<FileManagerEditEventArgs<FileManagerEntry>>(this, args =>
            {
                captured = args;
            })));

        await cut.InvokeAsync(() => cut.Instance.EditItem(item));

        Assert.NotNull(captured);
        Assert.Equal(item, captured!.Item);
    }

    [Fact]
    public async Task OnEdit_Does_Not_Throw_When_No_Delegate()
    {
        var item = MakeEntry("report.pdf", "/report.pdf");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.ShowFolderTree, false));

        // No OnEdit bound — should not throw
        var ex = await Record.ExceptionAsync(() =>
            cut.InvokeAsync(() => cut.Instance.EditItem(item)));

        Assert.Null(ex);
    }

    // ─── OnUpdate ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task OnUpdate_Fires_With_Correct_Item()
    {
        FileManagerUpdateEventArgs<FileManagerEntry>? captured = null;
        var item = MakeEntry("renamed.pdf", "/renamed.pdf");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnUpdate, EventCallback.Factory.Create<FileManagerUpdateEventArgs<FileManagerEntry>>(this, args =>
            {
                captured = args;
            })));

        await cut.InvokeAsync(() => cut.Instance.UpdateItem(item));

        Assert.NotNull(captured);
        Assert.Equal(item, captured!.Item);
    }

    [Fact]
    public async Task OnUpdate_Does_Not_Throw_When_No_Delegate()
    {
        var item = MakeEntry("file.txt", "/file.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.ShowFolderTree, false));

        var ex = await Record.ExceptionAsync(() =>
            cut.InvokeAsync(() => cut.Instance.UpdateItem(item)));

        Assert.Null(ex);
    }

    // ─── OnDownload ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task OnDownload_Fires_With_Correct_Item()
    {
        FileManagerDownloadEventArgs<FileManagerEntry>? captured = null;
        var item = MakeEntry("report.pdf", "/report.pdf");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnDownload, EventCallback.Factory.Create<FileManagerDownloadEventArgs<FileManagerEntry>>(this, args =>
            {
                captured = args;
            })));

        await cut.InvokeAsync(() => cut.Instance.DownloadItem(item));

        Assert.NotNull(captured);
        Assert.Equal(item, captured!.Item);
    }

    [Fact]
    public async Task OnDownload_IsCancelled_Can_Be_Set_By_Handler()
    {
        var item = MakeEntry("secret.pdf", "/secret.pdf");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnDownload, EventCallback.Factory.Create<FileManagerDownloadEventArgs<FileManagerEntry>>(this, args =>
            {
                // Handler cancels the download
                args.IsCancelled = true;
            })));

        FileManagerDownloadEventArgs<FileManagerEntry>? result = null;
        await cut.InvokeAsync(async () =>
        {
            result = await cut.Instance.DownloadItem(item);
        });

        Assert.NotNull(result);
        Assert.True(result!.IsCancelled);
    }

    [Fact]
    public async Task OnDownload_IsCancelled_False_By_Default()
    {
        var item = MakeEntry("file.pdf", "/file.pdf");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnDownload, EventCallback.Factory.Create<FileManagerDownloadEventArgs<FileManagerEntry>>(this, _ => { })));

        FileManagerDownloadEventArgs<FileManagerEntry>? result = null;
        await cut.InvokeAsync(async () =>
        {
            result = await cut.Instance.DownloadItem(item);
        });

        Assert.NotNull(result);
        Assert.False(result!.IsCancelled);
    }

    // ─── OnModelInit ────────────────────────────────────────────────────────────

    [Fact]
    public async Task OnModelInit_Callback_Used_In_CreateFolder()
    {
        FileManagerCreateEventArgs<FileManagerEntry>? captured = null;
        var template = new FileManagerEntry { Name = "New Folder", IsDirectory = true };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.AllowCreate, true)
            .Add(x => x.OnModelInit, () => template)
            .Add(x => x.OnCreate, EventCallback.Factory.Create<FileManagerCreateEventArgs<FileManagerEntry>>(this, args =>
            {
                captured = args;
            })));

        await cut.InvokeAsync(() => cut.Instance.CreateFolder());

        Assert.NotNull(captured);
        Assert.Same(template, captured!.Item);
    }

    [Fact]
    public async Task OnModelInit_Null_Uses_Default_In_CreateFolder()
    {
        FileManagerCreateEventArgs<FileManagerEntry>? captured = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.AllowCreate, true)
            // No OnModelInit bound
            .Add(x => x.OnCreate, EventCallback.Factory.Create<FileManagerCreateEventArgs<FileManagerEntry>>(this, args =>
            {
                captured = args;
            })));

        await cut.InvokeAsync(() => cut.Instance.CreateFolder());

        Assert.NotNull(captured);
        // Item should be default (null for reference types)
        Assert.Null(captured!.Item);
    }

    // ─── SelectedItems two-way binding ──────────────────────────────────────────

    [Fact]
    public async Task SelectedItems_Parameter_Syncs_To_Internal_State()
    {
        var item = MakeEntry("readme.txt", "/readme.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItems, new[] { item }));

        // After parameter sync, IsSelected should return true
        var isSelected = cut.Instance.IsSelected(item);
        Assert.True(isSelected);
    }

    [Fact]
    public async Task SelectedItems_Empty_Means_Nothing_Selected()
    {
        var item = MakeEntry("readme.txt", "/readme.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItems, Enumerable.Empty<FileManagerEntry>()));

        Assert.False(cut.Instance.IsSelected(item));
    }

    // ─── SelectedItemsChanged fires on selection ─────────────────────────────────

    [Fact]
    public async Task SelectedItemsChanged_Fires_When_Item_Is_Selected()
    {
        IEnumerable<FileManagerEntry>? capturedSelection = null;
        var item = MakeEntry("readme.txt", "/readme.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItemsChanged, EventCallback.Factory.Create<IEnumerable<FileManagerEntry>>(this, sel =>
            {
                capturedSelection = sel;
            })));

        await cut.InvokeAsync(() => cut.Instance.SelectItem(item));

        Assert.NotNull(capturedSelection);
        Assert.Contains(item, capturedSelection!);
    }

    [Fact]
    public async Task SelectItem_Updates_IsSelected_To_True()
    {
        var item = MakeEntry("notes.txt", "/notes.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        Assert.False(cut.Instance.IsSelected(item));

        await cut.InvokeAsync(() => cut.Instance.SelectItem(item));

        Assert.True(cut.Instance.IsSelected(item));
    }

    [Fact]
    public async Task SelectItem_Fires_OnSelect_And_SelectedItemsChanged()
    {
        var selectCount = 0;
        var changedCount = 0;
        var item = MakeEntry("doc.txt", "/doc.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnSelect, EventCallback.Factory.Create<FileManagerEntry>(this, _ => selectCount++))
            .Add(x => x.SelectedItemsChanged, EventCallback.Factory.Create<IEnumerable<FileManagerEntry>>(this, _ => changedCount++)));

        await cut.InvokeAsync(() => cut.Instance.SelectItem(item));

        Assert.Equal(1, selectCount);
        Assert.Equal(1, changedCount);
    }

    // ─── AllowDelete gates DeleteItem ────────────────────────────────────────────

    [Fact]
    public async Task DeleteItem_Fires_OnDelete_When_AllowDelete_Is_True()
    {
        FileManagerDeleteEventArgs<FileManagerEntry>? captured = null;
        var item = MakeEntry("old.txt", "/old.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.AllowDelete, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnDelete, EventCallback.Factory.Create<FileManagerDeleteEventArgs<FileManagerEntry>>(this, args =>
            {
                captured = args;
            })));

        await cut.InvokeAsync(() => cut.Instance.DeleteItem(item));

        Assert.NotNull(captured);
        Assert.Equal(item, captured!.Item);
    }

    [Fact]
    public async Task DeleteItem_Does_Not_Fire_OnDelete_When_AllowDelete_Is_False()
    {
        FileManagerDeleteEventArgs<FileManagerEntry>? captured = null;
        var item = MakeEntry("protected.txt", "/protected.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.AllowDelete, false)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnDelete, EventCallback.Factory.Create<FileManagerDeleteEventArgs<FileManagerEntry>>(this, args =>
            {
                captured = args;
            })));

        await cut.InvokeAsync(() => cut.Instance.DeleteItem(item));

        // OnDelete must NOT have been invoked
        Assert.Null(captured);
    }

    // ─── Rebind triggers OnRead ───────────────────────────────────────────────────

    [Fact]
    public async Task Rebind_Triggers_OnRead_When_Bound()
    {
        var readCount = 0;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.OnRead, EventCallback.Factory.Create<FileManagerReadEventArgs>(this, args =>
            {
                readCount++;
                args.Data = Array.Empty<FileManagerEntry>();
            }))
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => Task.CompletedTask); // flush init
        var countAfterInit = readCount;

        await cut.InvokeAsync(() => cut.Instance.Rebind());

        Assert.True(readCount > countAfterInit, "Rebind should trigger OnRead again");
    }

    [Fact]
    public async Task Rebind_Does_Not_Throw_When_OnRead_Not_Bound()
    {
        var items = new[] { MakeEntry("file.txt", "/file.txt") };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.ShowFolderTree, false));

        var ex = await Record.ExceptionAsync(() =>
            cut.InvokeAsync(() => cut.Instance.Rebind()));

        Assert.Null(ex);
    }

    // ─── ViewChanged two-way binding ─────────────────────────────────────────────

    [Fact]
    public async Task ViewChanged_Fires_When_View_Changes_Via_SetViewType()
    {
        FileManagerViewType? capturedView = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.ViewChanged, EventCallback.Factory.Create<FileManagerViewType>(this, v =>
            {
                capturedView = v;
            })));

        await cut.InvokeAsync(() => cut.Instance.SetViewType(FileManagerViewType.Grid));

        Assert.Equal(FileManagerViewType.Grid, capturedView);
    }

    [Fact]
    public async Task ToggleView_Switches_From_ListView_To_Grid()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.ToggleView());

        Assert.Equal(FileManagerViewType.Grid, cut.Instance.View);
    }

    [Fact]
    public async Task ToggleView_Switches_From_Grid_To_ListView()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.View, FileManagerViewType.Grid)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.ToggleView());

        Assert.Equal(FileManagerViewType.ListView, cut.Instance.View);
    }

    // ─── IsSelected reflects markup CSS classes ──────────────────────────────────

    [Fact]
    public async Task IsSelected_Item_Gets_Selected_CSS_Class_In_ListView()
    {
        var item = MakeEntry("readme.txt", "/readme.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.SelectItem(item));
        // Force re-render so the updated _selectedItems list is reflected in markup
        cut.Render(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItems, new[] { item }));

        var row = cut.Find(".mar-filemanager__row--selected");
        Assert.NotNull(row);
    }

    [Fact]
    public async Task IsSelected_Item_Gets_Selected_CSS_Class_In_Grid()
    {
        var item = MakeEntry("readme.txt", "/readme.txt");

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.View, FileManagerViewType.Grid)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.SelectItem(item));
        // Force re-render so the updated _selectedItems list is reflected in markup
        cut.Render(p => p
            .Add(x => x.Data, new[] { item })
            .Add(x => x.Path, "/")
            .Add(x => x.View, FileManagerViewType.Grid)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.SelectedItems, new[] { item }));

        var gridItem = cut.Find(".mar-filemanager__grid-item--selected");
        Assert.NotNull(gridItem);
    }
}
