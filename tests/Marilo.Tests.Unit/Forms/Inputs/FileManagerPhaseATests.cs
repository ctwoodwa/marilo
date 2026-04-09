using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// bUnit tests for MariloFileManager Phase A gap resolutions:
/// - Generic TItem
/// - Data parameter
/// - Path two-way binding
/// - View parameter (Grid / ListView)
/// - Field-binding with custom models
/// - OnRead fires on init
/// - OnRead fires on path change
/// - Height inline style
/// - OnCreate fires with FileManagerCreateEventArgs
/// - OnDelete fires with FileManagerDeleteEventArgs
/// </summary>
public class FileManagerPhaseATests : MariloTestBase
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
            DateModified = new DateTime(2025, 1, 15, 10, 30, 0)
        };

    private static IEnumerable<FileManagerEntry> SampleItems() => new[]
    {
        MakeEntry("Documents", "/Documents", isDir: true),
        MakeEntry("readme.txt", "/readme.txt", size: 1024),
        MakeEntry("report.pdf", "/Documents/report.pdf", size: 2048)
    };

    // ─── Generic TItem ──────────────────────────────────────────────────────────

    [Fact]
    public void Renders_WithFileManagerEntry_AsGenericTItem()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems()));

        Assert.NotNull(cut.Find(".mar-filemanager"));
    }

    // ─── Data parameter ─────────────────────────────────────────────────────────

    [Fact]
    public void Data_Parameter_Binds_Items_In_ListView()
    {
        var items = new[]
        {
            MakeEntry("readme.txt", "/readme.txt", size: 512),
            MakeEntry("notes.txt", "/notes.txt", size: 256)
        };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        var rows = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public void Data_Parameter_Empty_Shows_No_Rows()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, Enumerable.Empty<FileManagerEntry>())
            .Add(x => x.ShowFolderTree, false));

        var rows = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Empty(rows);
    }

    // ─── Path two-way binding ───────────────────────────────────────────────────

    [Fact]
    public void Path_Parameter_Displays_In_Toolbar()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents"));

        var pathSpan = cut.Find(".mar-filemanager__path");
        Assert.Equal("/Documents", pathSpan.TextContent);
    }

    [Fact]
    public async Task Path_TwoWay_Binding_Fires_PathChanged_On_Navigate()
    {
        string? capturedPath = null;
        var items = new[] { MakeEntry("Documents", "/Documents", isDir: true) };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.PathChanged, EventCallback.Factory.Create<string>(this, s => capturedPath = s)));

        // Navigate programmatically (UI dbl-click opens directories)
        await cut.InvokeAsync(() => cut.Instance.NavigateTo("/Documents"));

        Assert.Equal("/Documents", capturedPath);
    }

    [Fact]
    public void CanNavigateUp_Is_False_At_Root()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/"));

        Assert.False(cut.Instance.CanNavigateUp);
    }

    [Fact]
    public void CanNavigateUp_Is_True_Below_Root()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents"));

        Assert.True(cut.Instance.CanNavigateUp);
    }

    // ─── View parameter ─────────────────────────────────────────────────────────

    [Fact]
    public void View_Defaults_To_ListView()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        Assert.Equal(FileManagerViewType.ListView, cut.Instance.View);
        cut.Find(".mar-filemanager__list"); // must not throw
    }

    [Fact]
    public void View_Grid_Renders_Grid_Container()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.View, FileManagerViewType.Grid)
            .Add(x => x.ShowFolderTree, false));

        cut.Find(".mar-filemanager__grid"); // must not throw
    }

    [Fact]
    public void View_ListView_Renders_Table()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false));

        cut.Find(".mar-filemanager__list"); // must not throw
    }

    [Fact]
    public void View_Grid_Shows_Items_As_Grid_Items()
    {
        var items = new[]
        {
            MakeEntry("readme.txt", "/readme.txt"),
            MakeEntry("notes.txt", "/notes.txt")
        };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.Path, "/")
            .Add(x => x.View, FileManagerViewType.Grid)
            .Add(x => x.ShowFolderTree, false));

        var gridItems = cut.FindAll(".mar-filemanager__grid-item");
        Assert.Equal(2, gridItems.Count);
    }

    // ─── Field-binding with custom models ───────────────────────────────────────

    private class CustomFileItem
    {
        public string FileName { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public bool Folder { get; set; }
        public long Bytes { get; set; }
        public DateTime? LastChanged { get; set; }
    }

    [Fact]
    public void FieldBinding_Resolves_CustomModel_Properties()
    {
        var items = new[]
        {
            new CustomFileItem { FileName = "custom.txt", FullPath = "/custom.txt", Folder = false, Bytes = 512 }
        };

        var cut = Render<MariloFileManager<CustomFileItem>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.NameField, "FileName")
            .Add(x => x.PathField, "FullPath")
            .Add(x => x.IsDirectoryField, "Folder")
            .Add(x => x.SizeField, "Bytes")
            .Add(x => x.DateModifiedField, "LastChanged")
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        var nameCell = cut.Find(".mar-filemanager__list tbody tr td");
        Assert.Contains("custom.txt", nameCell.TextContent);
    }

    [Fact]
    public void FieldBinding_MissingProperty_Returns_Default_Without_Throwing()
    {
        // Providing a field name that doesn't exist should not throw
        var items = new[] { new CustomFileItem { FileName = "x.txt", FullPath = "/x.txt" } };

        var cut = Render<MariloFileManager<CustomFileItem>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.NameField, "NonExistentProperty")
            .Add(x => x.PathField, "FullPath")
            .Add(x => x.IsDirectoryField, "Folder")
            .Add(x => x.SizeField, "Bytes")
            .Add(x => x.DateModifiedField, "LastChanged")
            .Add(x => x.ShowFolderTree, false));

        // Should render without throwing; name cell content is empty
        var rows = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.NotEmpty(rows);
    }

    // ─── OnRead fires on init ───────────────────────────────────────────────────

    [Fact]
    public async Task OnRead_Fires_On_Init_When_Bound()
    {
        string? capturedPath = null;
        FileManagerReadEventArgs? capturedArgs = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.OnRead, EventCallback.Factory.Create<FileManagerReadEventArgs>(this, args =>
            {
                capturedPath = args.Path;
                capturedArgs = args;
                args.Data = SampleItems();
            }))
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => Task.CompletedTask); // flush

        Assert.NotNull(capturedArgs);
        Assert.Equal("/", capturedPath);
    }

    [Fact]
    public async Task OnRead_Data_Is_Displayed_When_Populated_By_Handler()
    {
        var readItems = new[]
        {
            MakeEntry("remote.txt", "/remote.txt", size: 100)
        };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.OnRead, EventCallback.Factory.Create<FileManagerReadEventArgs>(this, args =>
            {
                args.Data = readItems;
            }))
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => Task.CompletedTask);

        var rows = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Single(rows);
        Assert.Contains("remote.txt", rows[0].TextContent);
    }

    // ─── OnRead fires on path change ────────────────────────────────────────────

    [Fact]
    public async Task OnRead_Fires_On_PathChange()
    {
        var readCount = 0;
        var lastPath = "/";

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.OnRead, EventCallback.Factory.Create<FileManagerReadEventArgs>(this, args =>
            {
                readCount++;
                lastPath = args.Path;
                args.Data = Array.Empty<FileManagerEntry>();
            }))
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => Task.CompletedTask); // flush init read
        var initCount = readCount;

        // Navigate to a sub-path
        await cut.InvokeAsync(() => cut.Instance.NavigateTo("/Documents"));

        Assert.True(readCount > initCount, "OnRead should fire again after navigation");
        Assert.Equal("/Documents", lastPath);
    }

    // ─── Height inline style ────────────────────────────────────────────────────

    [Fact]
    public void Height_Renders_In_Style_Attribute()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Height, "400px"));

        var root = cut.Find(".mar-filemanager");
        var style = root.GetAttribute("style") ?? "";
        Assert.Contains("height:400px", style);
    }

    [Fact]
    public void Height_Null_Does_Not_Add_Height_Style()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>();

        var root = cut.Find(".mar-filemanager");
        var style = root.GetAttribute("style") ?? "";
        Assert.DoesNotContain("height:", style);
    }

    // ─── OnCreate fires with FileManagerCreateEventArgs ─────────────────────────

    [Fact]
    public async Task OnCreate_Fires_When_New_Folder_Clicked()
    {
        FileManagerCreateEventArgs<FileManagerEntry>? captured = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.AllowCreate, true)
            .Add(x => x.OnCreate, EventCallback.Factory.Create<FileManagerCreateEventArgs<FileManagerEntry>>(this, args =>
            {
                captured = args;
            })));

        var btn = cut.Find("button.mar-btn:not([disabled])");
        await cut.InvokeAsync(() => btn.Click());

        Assert.NotNull(captured);
    }

    [Fact]
    public void AllowCreate_False_Does_Not_Render_NewFolder_Button()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.AllowCreate, false));

        var buttons = cut.FindAll("button");
        // Only the Up button should be present
        Assert.DoesNotContain(buttons, b => b.TextContent.Contains("New Folder"));
    }

    // ─── OnDelete fires with FileManagerDeleteEventArgs ─────────────────────────

    [Fact]
    public async Task OnDelete_Fires_With_FileManagerDeleteEventArgs()
    {
        FileManagerDeleteEventArgs<FileManagerEntry>? captured = null;
        var items = new[] { MakeEntry("readme.txt", "/readme.txt", size: 512) };

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, items)
            .Add(x => x.Path, "/")
            .Add(x => x.AllowDelete, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnDelete, EventCallback.Factory.Create<FileManagerDeleteEventArgs<FileManagerEntry>>(this, args =>
            {
                captured = args;
            })));

        // Call DeleteItem directly (UI delete trigger is Phase B)
        await cut.InvokeAsync(() => cut.Instance.DeleteItem(items[0]));

        Assert.NotNull(captured);
        Assert.Equal(items[0], captured!.Item);
    }

    // ─── Enum: FileManagerViewType values ───────────────────────────────────────

    [Fact]
    public void FileManagerViewType_Has_ListView_And_Grid_Values()
    {
        Assert.Equal(2, Enum.GetValues<FileManagerViewType>().Length);
        Assert.Contains(FileManagerViewType.ListView, Enum.GetValues<FileManagerViewType>());
        Assert.Contains(FileManagerViewType.Grid, Enum.GetValues<FileManagerViewType>());
    }

    // ─── Obsolete FileManagerViewMode backward compat ───────────────────────────

    [Fact]
    public void FileManagerViewMode_Obsolete_Still_Has_List_And_Grid()
    {
#pragma warning disable CS0618
        Assert.Contains(FileManagerViewMode.List, Enum.GetValues<FileManagerViewMode>());
        Assert.Contains(FileManagerViewMode.Grid, Enum.GetValues<FileManagerViewMode>());
#pragma warning restore CS0618
    }

    // ─── FormatSize helper ───────────────────────────────────────────────────────

    [Theory]
    [InlineData(512, "512 B")]
    [InlineData(1536, "1.5 KB")]
    [InlineData(1572864, "1.5 MB")]
    public void FormatSize_Returns_Correct_String(long bytes, string expected)
    {
        Assert.Equal(expected, MariloFileManager<FileManagerEntry>.FormatSize(bytes));
    }
}
