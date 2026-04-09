using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// bUnit tests for MariloFileManager Phase F gap resolutions:
/// - SPEC-FM-031: Sort toolbar — sort by Name/Size/DateModified, direction toggle
/// - SPEC-FM-008: Width parameter renders in inline style
/// - SPEC-FM-006: EnableLoaderContainer shows loader overlay during async load
/// - SPEC-FM-032: ARIA roles — root, toolbar, tree, context menu, grid, alertdialog
/// - SPEC-FM-009: Class parameter from MariloComponentBase applied via CombineClasses
/// </summary>
public class FileManagerPhaseFTests : MariloTestBase
{
    // ─── helpers ────────────────────────────────────────────────────────────────

    private static FileManagerEntry MakeFile(
        string name,
        string path,
        long size = 2048,
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

    private static IEnumerable<FileManagerEntry> SortItems() => new[]
    {
        MakeDir("Zebra", "/Zebra"),
        MakeDir("Alpha", "/Alpha"),
        MakeFile("readme.txt", "/readme.txt", size: 100, extension: ".txt",
            modified: new DateTime(2025, 1, 1)),
        MakeFile("report.pdf", "/report.pdf", size: 5000, extension: ".pdf",
            modified: new DateTime(2025, 6, 1)),
        MakeFile("archive.zip", "/archive.zip", size: 300, extension: ".zip",
            modified: new DateTime(2025, 3, 1))
    };

    // ─── SPEC-FM-031: Sort by Name ascending (default) ───────────────────────────

    [Fact]
    public void Sort_ByName_Ascending_Default()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SortItems())
            .Add(x => x.ShowFolderTree, false));

        var names = cut.FindAll(".mar-filemanager__name")
            .Select(n => n.TextContent.Trim())
            .ToList();

        // Directories first (Alpha, Zebra) then files by name asc
        Assert.Equal("Alpha", names[0]);
        Assert.Equal("Zebra", names[1]);
        Assert.Equal("archive.zip", names[2]);
        Assert.Equal("readme.txt", names[3]);
        Assert.Equal("report.pdf", names[4]);
    }

    // ─── SPEC-FM-031: Sort by Name descending ────────────────────────────────────

    [Fact]
    public void Sort_ByName_Descending()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SortItems())
            .Add(x => x.ShowFolderTree, false));

        cut.InvokeAsync(() => cut.Instance.ToggleSortDirection());

        var names = cut.FindAll(".mar-filemanager__name")
            .Select(n => n.TextContent.Trim())
            .ToList();

        // Directories first (Zebra, Alpha desc) then files by name desc
        Assert.Equal("Zebra", names[0]);
        Assert.Equal("Alpha", names[1]);
        Assert.Equal("report.pdf", names[2]);
        Assert.Equal("readme.txt", names[3]);
        Assert.Equal("archive.zip", names[4]);
    }

    // ─── SPEC-FM-031: Sort by Size ascending ─────────────────────────────────────

    [Fact]
    public void Sort_BySize_Ascending()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SortItems())
            .Add(x => x.ShowFolderTree, false));

        cut.InvokeAsync(() => cut.Instance.SetSortField("Size"));

        var names = cut.FindAll(".mar-filemanager__name")
            .Select(n => n.TextContent.Trim())
            .ToList();

        // Dirs first, then files: readme(100) < archive(300) < report(5000)
        Assert.True(names.IndexOf("readme.txt") < names.IndexOf("archive.zip"));
        Assert.True(names.IndexOf("archive.zip") < names.IndexOf("report.pdf"));
    }

    // ─── SPEC-FM-031: Sort by DateModified ascending ─────────────────────────────

    [Fact]
    public void Sort_ByDateModified_Ascending()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SortItems())
            .Add(x => x.ShowFolderTree, false));

        cut.InvokeAsync(() => cut.Instance.SetSortField("DateModified"));

        var names = cut.FindAll(".mar-filemanager__name")
            .Select(n => n.TextContent.Trim())
            .ToList();

        // Dirs first, then files by date: readme(Jan) < archive(Mar) < report(Jun)
        Assert.True(names.IndexOf("readme.txt") < names.IndexOf("archive.zip"));
        Assert.True(names.IndexOf("archive.zip") < names.IndexOf("report.pdf"));
    }

    // ─── SPEC-FM-031: Directories always first regardless of sort field ───────────

    [Fact]
    public void Sort_DirectoriesAlwaysFirst()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SortItems())
            .Add(x => x.ShowFolderTree, false));

        cut.InvokeAsync(() => cut.Instance.SetSortField("Size"));

        var names = cut.FindAll(".mar-filemanager__name")
            .Select(n => n.TextContent.Trim())
            .ToList();

        // Dirs (Alpha, Zebra) must appear before any file name
        var dirIndices = new[] { names.IndexOf("Alpha"), names.IndexOf("Zebra") };
        var fileIndices = new[] { names.IndexOf("readme.txt"), names.IndexOf("archive.zip"), names.IndexOf("report.pdf") };
        Assert.True(dirIndices.Max() < fileIndices.Min());
    }

    // ─── SPEC-FM-031: Sort direction toggle updates state ────────────────────────

    [Fact]
    public void ToggleSortDirection_FlipsState()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        Assert.True(cut.Instance._sortAscending); // default

        cut.InvokeAsync(() => cut.Instance.ToggleSortDirection());
        Assert.False(cut.Instance._sortAscending);

        cut.InvokeAsync(() => cut.Instance.ToggleSortDirection());
        Assert.True(cut.Instance._sortAscending);
    }

    // ─── SPEC-FM-031: Sort controls present in default toolbar ───────────────────

    [Fact]
    public void SortSelect_PresentInDefaultToolbar()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        var select = cut.Find("select.mar-filemanager__sort-select");
        Assert.NotNull(select);
    }

    [Fact]
    public void SortDirectionButton_PresentInDefaultToolbar()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        var btn = cut.Find(".mar-filemanager__sort-dir");
        Assert.NotNull(btn);
    }

    // ─── SPEC-FM-008: Width parameter renders in inline style ────────────────────

    [Fact]
    public void Width_Parameter_Renders_In_Style()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Width, "800px")
            .Add(x => x.ShowFolderTree, false));

        var style = cut.Find(".mar-filemanager").GetAttribute("style") ?? string.Empty;
        Assert.Contains("width:800px", style);
    }

    [Fact]
    public void Width_Parameter_Null_NoWidthStyle()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        var style = cut.Find(".mar-filemanager").GetAttribute("style") ?? string.Empty;
        Assert.DoesNotContain("width:", style);
    }

    [Fact]
    public void Width_And_Height_Both_Render_In_Style()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Width, "100%")
            .Add(x => x.Height, "400px")
            .Add(x => x.ShowFolderTree, false));

        var style = cut.Find(".mar-filemanager").GetAttribute("style") ?? string.Empty;
        Assert.Contains("width:100%", style);
        Assert.Contains("height:400px", style);
    }

    // ─── SPEC-FM-006: EnableLoaderContainer hides loader by default ──────────────

    [Fact]
    public void Loader_Not_Rendered_When_NotLoading()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.EnableLoaderContainer, true)
            .Add(x => x.ShowFolderTree, false));

        // _isLoading is false after init (no OnRead bound)
        Assert.Empty(cut.FindAll(".mar-filemanager__loader"));
    }

    [Fact]
    public void Loader_Rendered_When_IsLoading_True()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.EnableLoaderContainer, true)
            .Add(x => x.ShowFolderTree, false));

        // Directly set loading state and force re-render
        cut.Instance._isLoading = true;
        cut.Render(p => p
            .Add(x => x.EnableLoaderContainer, true)
            .Add(x => x.ShowFolderTree, false));

        cut.Find(".mar-filemanager__loader"); // must not throw
    }

    [Fact]
    public void Loader_Not_Rendered_When_EnableLoaderContainer_False()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.EnableLoaderContainer, false)
            .Add(x => x.ShowFolderTree, false));

        cut.Instance._isLoading = true;
        cut.Render(p => p
            .Add(x => x.EnableLoaderContainer, false)
            .Add(x => x.ShowFolderTree, false));

        Assert.Empty(cut.FindAll(".mar-filemanager__loader"));
    }

    [Fact]
    public async Task Loader_NotLoading_After_Rebind_With_OnRead()
    {
        // Verify that after LoadDataAsync completes, _isLoading reverts to false.
        // Use a synchronous OnRead so the cycle completes within InvokeAsync.
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.EnableLoaderContainer, true)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.OnRead, EventCallback.Factory.Create<FileManagerReadEventArgs>(this, args =>
            {
                args.Data = Array.Empty<FileManagerEntry>();
            })));

        // The initial OnInitializedAsync already called LoadDataAsync.
        // After it, _isLoading must be false.
        await cut.InvokeAsync(() => cut.Instance.Rebind());

        Assert.False(cut.Instance._isLoading);
        // Loader overlay must not be visible
        Assert.Empty(cut.FindAll(".mar-filemanager__loader"));
    }

    // ─── SPEC-FM-032: ARIA — root element ────────────────────────────────────────

    [Fact]
    public void Root_Has_Role_Application()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        var root = cut.Find(".mar-filemanager");
        Assert.Equal("application", root.GetAttribute("role"));
    }

    [Fact]
    public void Root_Has_AriaLabel_FileManager()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        var root = cut.Find(".mar-filemanager");
        Assert.Equal("File Manager", root.GetAttribute("aria-label"));
    }

    // ─── SPEC-FM-032: ARIA — toolbar ─────────────────────────────────────────────

    [Fact]
    public void Toolbar_Has_Role_Toolbar()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        var toolbar = cut.Find(".mar-filemanager__toolbar");
        Assert.Equal("toolbar", toolbar.GetAttribute("role"));
    }

    [Fact]
    public void Toolbar_Has_AriaLabel()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        var toolbar = cut.Find(".mar-filemanager__toolbar");
        Assert.Equal("File manager toolbar", toolbar.GetAttribute("aria-label"));
    }

    // ─── SPEC-FM-032: ARIA — search input ────────────────────────────────────────

    [Fact]
    public void SearchInput_Has_AriaLabel()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        var search = cut.Find(".mar-filemanager__search");
        Assert.Equal("Search files", search.GetAttribute("aria-label"));
    }

    // ─── SPEC-FM-032: ARIA — folder tree ─────────────────────────────────────────

    [Fact]
    public void FolderTree_Has_Role_Tree_And_AriaLabel()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { MakeDir("Docs", "/Docs") })
            .Add(x => x.ShowFolderTree, true));

        var tree = cut.Find(".mar-filemanager__tree");
        Assert.Equal("tree", tree.GetAttribute("role"));
        Assert.Equal("Folder tree", tree.GetAttribute("aria-label"));
    }

    [Fact]
    public void TreeItems_Have_Role_Treeitem_And_Tabindex()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { MakeDir("Docs", "/Docs") })
            .Add(x => x.ShowFolderTree, true));

        var li = cut.Find(".mar-filemanager__tree li");
        Assert.Equal("treeitem", li.GetAttribute("role"));
        Assert.Equal("0", li.GetAttribute("tabindex"));
    }

    // ─── SPEC-FM-032: ARIA — file list table ─────────────────────────────────────

    [Fact]
    public void ListTable_Has_Role_Grid()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { MakeFile("a.txt", "/a.txt") })
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false));

        var table = cut.Find(".mar-filemanager__list");
        Assert.Equal("grid", table.GetAttribute("role"));
    }

    [Fact]
    public void ListTable_Rows_Have_Role_Row_And_Cells_Role_Gridcell()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { MakeFile("a.txt", "/a.txt") })
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false));

        var bodyRows = cut.FindAll("tbody tr");
        Assert.All(bodyRows, row => Assert.Equal("row", row.GetAttribute("role")));

        var cells = cut.FindAll("tbody td");
        Assert.All(cells, cell => Assert.Equal("gridcell", cell.GetAttribute("role")));
    }

    // ─── SPEC-FM-032: ARIA — grid view items ─────────────────────────────────────

    [Fact]
    public void GridItems_Have_Role_Option()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { MakeFile("a.txt", "/a.txt") })
            .Add(x => x.View, FileManagerViewType.Grid)
            .Add(x => x.ShowFolderTree, false));

        var items = cut.FindAll(".mar-filemanager__grid-item");
        Assert.All(items, item => Assert.Equal("option", item.GetAttribute("role")));
    }

    // ─── SPEC-FM-032: ARIA — context menu ────────────────────────────────────────

    [Fact]
    public async Task ContextMenu_Has_Role_Menu_And_Items_Role_Menuitem()
    {
        var file = MakeFile("a.txt", "/a.txt");
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.AllowRename, true)
            .Add(x => x.AllowDelete, true)
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.ShowContextMenu(
            file,
            new Microsoft.AspNetCore.Components.Web.MouseEventArgs { ClientX = 10, ClientY = 10 }));

        var menu = cut.Find(".mar-filemanager__context-menu");
        Assert.Equal("menu", menu.GetAttribute("role"));

        var items = menu.QuerySelectorAll("[role='menuitem']");
        Assert.NotEmpty(items);
    }

    // ─── SPEC-FM-032: ARIA — delete confirmation dialog ──────────────────────────

    [Fact]
    public async Task DeleteConfirmDialog_Has_Role_Alertdialog_And_AriaModal()
    {
        var file = MakeFile("a.txt", "/a.txt");
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, new[] { file })
            .Add(x => x.AllowDelete, true)
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => cut.Instance.ConfirmDelete(file));

        var dialog = cut.Find(".mar-filemanager__confirm-dialog");
        Assert.Equal("alertdialog", dialog.GetAttribute("role"));
        Assert.Equal("true", dialog.GetAttribute("aria-modal"));
    }

    // ─── SPEC-FM-009: Class parameter from MariloComponentBase ───────────────────

    [Fact]
    public void Class_Parameter_Applied_Via_CombineClasses()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Class, "my-custom-class")
            .Add(x => x.ShowFolderTree, false));

        var root = cut.Find(".mar-filemanager");
        Assert.Contains("my-custom-class", root.ClassList);
        Assert.Contains("mar-filemanager", root.ClassList);
    }

    [Fact]
    public void Class_Parameter_Null_OnlyBaseClass()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        var root = cut.Find(".mar-filemanager");
        Assert.Contains("mar-filemanager", root.ClassList);
    }
}
