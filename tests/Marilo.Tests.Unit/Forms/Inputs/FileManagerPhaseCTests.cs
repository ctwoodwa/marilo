using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// bUnit tests for MariloFileManager Phase C gap resolutions:
/// - SPEC-FM-022: ToolBarTemplate renders custom content when provided
/// - SPEC-FM-023: Default toolbar renders Up, breadcrumb, view toggle, search, New Folder
/// - SPEC-FM-024: Breadcrumb segments are rendered and navigate on click
/// - SPEC-FM-030: Search textbox filters items by name (case-insensitive)
/// - View toggle switches between Grid and ListView
/// - AllowCreate shows/hides New Folder button in default toolbar
/// </summary>
public class FileManagerPhaseCTests : MariloTestBase
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

    private static IEnumerable<FileManagerEntry> SampleItems() => new[]
    {
        MakeEntry("Documents", "/Documents", isDir: true),
        MakeEntry("Images", "/Images", isDir: true),
        MakeEntry("readme.txt", "/readme.txt", size: 1024),
        MakeEntry("notes.txt", "/notes.txt", size: 512),
        MakeEntry("report.pdf", "/Documents/report.pdf", size: 2048)
    };

    // ─── SPEC-FM-022: Default toolbar ──────────────────────────────────────────

    [Fact]
    public void DefaultToolbar_Renders_Up_Button()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents")
            .Add(x => x.ShowFolderTree, false));

        var buttons = cut.FindAll("button");
        Assert.Contains(buttons, b => b.TextContent.Contains("Up"));
    }

    [Fact]
    public void DefaultToolbar_Renders_Breadcrumb_Nav()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        cut.Find(".mar-filemanager__breadcrumb"); // must not throw
    }

    [Fact]
    public void DefaultToolbar_Renders_Search_Input()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        cut.Find(".mar-filemanager__search"); // must not throw
    }

    [Fact]
    public void DefaultToolbar_Renders_ViewToggle_Button()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false));

        cut.Find(".mar-filemanager__view-toggle"); // must not throw
    }

    [Fact]
    public void DefaultToolbar_AllowCreate_True_Shows_NewFolder_Button()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.AllowCreate, true)
            .Add(x => x.ShowFolderTree, false));

        var buttons = cut.FindAll("button");
        Assert.Contains(buttons, b => b.TextContent.Contains("New Folder"));
    }

    [Fact]
    public void DefaultToolbar_AllowCreate_False_Hides_NewFolder_Button()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.AllowCreate, false)
            .Add(x => x.ShowFolderTree, false));

        var buttons = cut.FindAll("button");
        Assert.DoesNotContain(buttons, b => b.TextContent.Contains("New Folder"));
    }

    // ─── SPEC-FM-022: Custom ToolBarTemplate ────────────────────────────────────

    [Fact]
    public void ToolBarTemplate_Renders_Custom_Content_When_Provided()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.ToolBarTemplate, builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "custom-toolbar");
                builder.AddContent(2, "My Custom Toolbar");
                builder.CloseElement();
            }));

        var customToolbar = cut.Find(".custom-toolbar");
        Assert.Contains("My Custom Toolbar", customToolbar.TextContent);
    }

    [Fact]
    public void ToolBarTemplate_Suppresses_DefaultToolbar_Elements()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.AllowCreate, true)
            .Add(x => x.ToolBarTemplate, builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddContent(1, "custom");
                builder.CloseElement();
            }));

        // Default toolbar elements must not be present
        Assert.Empty(cut.FindAll(".mar-filemanager__breadcrumb"));
        Assert.Empty(cut.FindAll(".mar-filemanager__search"));
        Assert.Empty(cut.FindAll(".mar-filemanager__view-toggle"));
    }

    // ─── SPEC-FM-024: Breadcrumb navigation ────────────────────────────────────

    [Fact]
    public void Breadcrumb_Root_Path_Shows_Single_Root_Segment()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        var segments = cut.FindAll(".mar-filemanager__breadcrumb-segment");
        Assert.Single(segments);
        Assert.Equal("/", segments[0].TextContent);
    }

    [Fact]
    public void Breadcrumb_SubPath_Shows_Multiple_Segments()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents")
            .Add(x => x.ShowFolderTree, false));

        var segments = cut.FindAll(".mar-filemanager__breadcrumb-segment");
        Assert.Equal(2, segments.Count);
        Assert.Equal("/", segments[0].TextContent);
        Assert.Equal("Documents", segments[1].TextContent);
    }

    [Fact]
    public void Breadcrumb_DeepPath_Shows_All_Segments()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents/Reports")
            .Add(x => x.ShowFolderTree, false));

        var segments = cut.FindAll(".mar-filemanager__breadcrumb-segment");
        Assert.Equal(3, segments.Count);
        Assert.Equal("/", segments[0].TextContent);
        Assert.Equal("Documents", segments[1].TextContent);
        Assert.Equal("Reports", segments[2].TextContent);
    }

    [Fact]
    public void Breadcrumb_Last_Segment_Has_Active_Class()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents")
            .Add(x => x.ShowFolderTree, false));

        var segments = cut.FindAll(".mar-filemanager__breadcrumb-segment");
        Assert.Contains("mar-filemanager__breadcrumb-segment--active",
            segments.Last().ClassList);
    }

    [Fact]
    public async Task Breadcrumb_Segment_Click_Navigates_To_Segment_Path()
    {
        string? navigatedPath = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.PathChanged, EventCallback.Factory.Create<string>(this, p => navigatedPath = p)));

        // Click the root "/" segment (first, non-active)
        var segments = cut.FindAll(".mar-filemanager__breadcrumb-segment");
        await cut.InvokeAsync(() => segments[0].Click());

        Assert.Equal("/", navigatedPath);
    }

    [Fact]
    public async Task Breadcrumb_Deep_Segment_Click_Navigates_To_Intermediate_Path()
    {
        string? navigatedPath = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents/Reports")
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.PathChanged, EventCallback.Factory.Create<string>(this, p => navigatedPath = p)));

        // Click the "Documents" segment (index 1)
        var segments = cut.FindAll(".mar-filemanager__breadcrumb-segment");
        await cut.InvokeAsync(() => segments[1].Click());

        Assert.Equal("/Documents", navigatedPath);
    }

    // ─── SPEC-FM-030: Search filter ─────────────────────────────────────────────

    [Fact]
    public void Search_Empty_Shows_All_Items_In_Current_Folder()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        // Root has: Documents (dir), Images (dir), readme.txt, notes.txt
        var rows = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Equal(4, rows.Count);
    }

    [Fact]
    public async Task Search_Filters_Items_By_Name_CaseInsensitive()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        // Apply search filter via component method
        await cut.InvokeAsync(() => { cut.Instance.SearchFilter = "READ"; });

        var rows = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Single(rows);
        Assert.Contains("readme.txt", rows[0].TextContent);
    }

    [Fact]
    public async Task Search_Filter_Partial_Match_Returns_Multiple_Results()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        // "notes" and "readme" both contain "e" — broader: filter by ".txt"
        await cut.InvokeAsync(() => { cut.Instance.SearchFilter = ".txt"; });

        var rows = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public async Task Search_Clear_Shows_All_Items_Again()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => { cut.Instance.SearchFilter = "readme"; });
        var filtered = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Single(filtered);

        await cut.InvokeAsync(() => { cut.Instance.SearchFilter = string.Empty; });
        var restored = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Equal(4, restored.Count);
    }

    [Fact]
    public async Task Search_NoMatch_Shows_Empty_List()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        await cut.InvokeAsync(() => { cut.Instance.SearchFilter = "xyzzynotfound"; });

        var rows = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Empty(rows);
    }

    [Fact]
    public async Task Search_Is_Applied_After_Navigation()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Data, SampleItems())
            .Add(x => x.Path, "/")
            .Add(x => x.ShowFolderTree, false));

        // Navigate to /Documents — only report.pdf is there
        await cut.InvokeAsync(() => cut.Instance.NavigateTo("/Documents"));

        // Apply filter that matches report.pdf
        await cut.InvokeAsync(() => { cut.Instance.SearchFilter = "report"; });
        var filtered = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Single(filtered);
        Assert.Contains("report.pdf", filtered[0].TextContent);

        // Apply filter that does NOT match any item at /Documents
        await cut.InvokeAsync(() => { cut.Instance.SearchFilter = "readme"; });
        var noMatch = cut.FindAll(".mar-filemanager__list tbody tr");
        Assert.Empty(noMatch); // readme.txt is at root, not /Documents
    }

    // ─── View toggle ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task ViewToggle_Button_Switches_From_List_To_Grid()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false));

        var toggleBtn = cut.Find(".mar-filemanager__view-toggle");
        await cut.InvokeAsync(() => toggleBtn.Click());

        Assert.Equal(FileManagerViewType.Grid, cut.Instance.View);
        cut.Find(".mar-filemanager__grid"); // must not throw
    }

    [Fact]
    public async Task ViewToggle_Button_Switches_From_Grid_To_List()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.View, FileManagerViewType.Grid)
            .Add(x => x.ShowFolderTree, false));

        var toggleBtn = cut.Find(".mar-filemanager__view-toggle");
        await cut.InvokeAsync(() => toggleBtn.Click());

        Assert.Equal(FileManagerViewType.ListView, cut.Instance.View);
        cut.Find(".mar-filemanager__list"); // must not throw
    }

    [Fact]
    public async Task SetViewType_Fires_ViewChanged_Event()
    {
        FileManagerViewType? captured = null;

        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.View, FileManagerViewType.ListView)
            .Add(x => x.ShowFolderTree, false)
            .Add(x => x.ViewChanged, EventCallback.Factory.Create<FileManagerViewType>(this,
                v => captured = v)));

        await cut.InvokeAsync(() => cut.Instance.SetViewType(FileManagerViewType.Grid));

        Assert.Equal(FileManagerViewType.Grid, captured);
    }

    // ─── GetBreadcrumbSegments helper ───────────────────────────────────────────

    [Fact]
    public void GetBreadcrumbSegments_Root_Returns_Single_Entry()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/"));

        var segs = cut.Instance.GetBreadcrumbSegments().ToList();
        Assert.Single(segs);
        Assert.Equal("/", segs[0].Label);
        Assert.Equal("/", segs[0].SegmentPath);
    }

    [Fact]
    public void GetBreadcrumbSegments_DeepPath_Returns_Correct_Paths()
    {
        var cut = Render<MariloFileManager<FileManagerEntry>>(p => p
            .Add(x => x.Path, "/Documents/Reports"));

        var segs = cut.Instance.GetBreadcrumbSegments().ToList();
        Assert.Equal(3, segs.Count);
        Assert.Equal("/", segs[0].SegmentPath);
        Assert.Equal("/Documents", segs[1].SegmentPath);
        Assert.Equal("/Documents/Reports", segs[2].SegmentPath);
    }
}
