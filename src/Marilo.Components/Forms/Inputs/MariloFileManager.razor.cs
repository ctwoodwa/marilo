using Marilo.Core.Base;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Marilo.Components.Forms.Inputs;

/// <summary>
/// A generic file manager component that displays and navigates a file/folder tree.
/// Bind your own data model via <c>Data</c> and field-mapping parameters, or use
/// <see cref="FileManagerEntry"/> directly as <typeparamref name="TItem"/>.
/// </summary>
public partial class MariloFileManager<TItem> : MariloComponentBase
{
    // ── Internal state ──────────────────────────────────────────────────────────

    private string? _selectedItemPath;
    private IEnumerable<TItem> _resolvedItems = Enumerable.Empty<TItem>();
    private CancellationTokenSource? _readCts;

    // ── Parameters: Data ────────────────────────────────────────────────────────

    /// <summary>
    /// The data items to display. For server-side loading, use <see cref="OnRead"/> instead.
    /// </summary>
    [Parameter] public IEnumerable<TItem> Data { get; set; } = Enumerable.Empty<TItem>();

    /// <summary>
    /// Fires when the component needs data (on init and path changes).
    /// Assign <see cref="FileManagerReadEventArgs.Data"/> in the handler.
    /// When bound, the <see cref="Data"/> parameter is ignored.
    /// </summary>
    [Parameter] public EventCallback<FileManagerReadEventArgs> OnRead { get; set; }

    // ── Parameters: Field Bindings ──────────────────────────────────────────────

    /// <summary>Property name for the item id. Default: "Id".</summary>
    [Parameter] public string IdField { get; set; } = "Id";

    /// <summary>Property name for the parent item id. Default: "ParentId".</summary>
    [Parameter] public string ParentIdField { get; set; } = "ParentId";

    /// <summary>Property name for the display name. Default: "Name".</summary>
    [Parameter] public string NameField { get; set; } = "Name";

    /// <summary>Property name for the full path. Default: "Path".</summary>
    [Parameter] public string PathField { get; set; } = "Path";

    /// <summary>Property name for the file extension. Default: "Extension".</summary>
    [Parameter] public string ExtensionField { get; set; } = "Extension";

    /// <summary>Property name for the directory flag. Default: "IsDirectory".</summary>
    [Parameter] public string IsDirectoryField { get; set; } = "IsDirectory";

    /// <summary>Property name for the has-directories flag. Default: "HasDirectories".</summary>
    [Parameter] public string HasDirectoriesField { get; set; } = "HasDirectories";

    /// <summary>Property name for the file size. Default: "Size".</summary>
    [Parameter] public string SizeField { get; set; } = "Size";

    /// <summary>Property name for the local date created. Default: "DateCreated".</summary>
    [Parameter] public string DateCreatedField { get; set; } = "DateCreated";

    /// <summary>Property name for the UTC date created. Default: "DateCreatedUtc".</summary>
    [Parameter] public string DateCreatedUtcField { get; set; } = "DateCreatedUtc";

    /// <summary>Property name for the local date modified. Default: "DateModified".</summary>
    [Parameter] public string DateModifiedField { get; set; } = "DateModified";

    /// <summary>Property name for the UTC date modified. Default: "DateModifiedUtc".</summary>
    [Parameter] public string DateModifiedUtcField { get; set; } = "DateModifiedUtc";

    /// <summary>Property name for the child directories collection. Default: "Directories".</summary>
    [Parameter] public string DirectoriesField { get; set; } = "Directories";

    /// <summary>Property name for the child items collection. Default: "Items".</summary>
    [Parameter] public string ItemsField { get; set; } = "Items";

    // ── Parameters: Navigation ──────────────────────────────────────────────────

    /// <summary>The current navigation path. Supports two-way binding.</summary>
    [Parameter] public string Path { get; set; } = "/";

    /// <summary>Fires when the path changes.</summary>
    [Parameter] public EventCallback<string> PathChanged { get; set; }

    // ── Parameters: View ────────────────────────────────────────────────────────

    /// <summary>The current view type. Supports two-way binding.</summary>
    [Parameter] public FileManagerViewType View { get; set; } = FileManagerViewType.ListView;

    /// <summary>Fires when the view type changes.</summary>
    [Parameter] public EventCallback<FileManagerViewType> ViewChanged { get; set; }

    // ── Parameters: Layout ──────────────────────────────────────────────────────

    /// <summary>Optional height (e.g. "400px", "60vh"). Applied as inline style.</summary>
    [Parameter] public string? Height { get; set; }

    /// <summary>When true, displays the folder-tree sidebar.</summary>
    [Parameter] public bool ShowFolderTree { get; set; } = true;

    // ── Parameters: Permissions ─────────────────────────────────────────────────

    /// <summary>Enables the New Folder button.</summary>
    [Parameter] public bool AllowCreate { get; set; }

    /// <summary>Enables the Delete action.</summary>
    [Parameter] public bool AllowDelete { get; set; }

    /// <summary>Enables the Rename action.</summary>
    [Parameter] public bool AllowRename { get; set; }

    // ── Parameters: Events ──────────────────────────────────────────────────────

    /// <summary>Fires when the user single-clicks an item.</summary>
    [Parameter] public EventCallback<TItem> OnSelect { get; set; }

    /// <summary>Fires when the user double-clicks an item (opens directories, opens files).</summary>
    [Parameter] public EventCallback<TItem> OnOpen { get; set; }

    /// <summary>Fires when the user creates a new folder.</summary>
    [Parameter] public EventCallback<FileManagerCreateEventArgs<TItem>> OnCreate { get; set; }

    /// <summary>Fires when the user deletes an item.</summary>
    [Parameter] public EventCallback<FileManagerDeleteEventArgs<TItem>> OnDelete { get; set; }

    // ── Derived state ───────────────────────────────────────────────────────────

    internal bool CanNavigateUp => Path != "/" && Path.Contains('/');

    // ── PropertyInfo cache ──────────────────────────────────────────────────────

    private readonly Dictionary<string, PropertyInfo?> _propCache = new();

    private PropertyInfo? GetProp(string fieldName)
    {
        if (!_propCache.TryGetValue(fieldName, out var prop))
        {
            prop = typeof(TItem).GetProperty(fieldName);
            _propCache[fieldName] = prop;
        }
        return prop;
    }

    private T? GetFieldValue<T>(TItem item, string fieldName)
    {
        if (item is null) return default;
        var prop = GetProp(fieldName);
        if (prop is null) return default;
        var val = prop.GetValue(item);
        if (val is T typed) return typed;
        return default;
    }

    // ── Field accessor helpers ──────────────────────────────────────────────────

    internal string GetName(TItem item) => GetFieldValue<string>(item, NameField) ?? string.Empty;
    internal string GetPath(TItem item) => GetFieldValue<string>(item, PathField) ?? string.Empty;
    internal bool GetIsDirectory(TItem item) => GetFieldValue<bool>(item, IsDirectoryField);
    internal long GetSize(TItem item) => GetFieldValue<long>(item, SizeField);
    internal DateTime? GetDateModified(TItem item) => GetFieldValue<DateTime?>(item, DateModifiedField);
    internal string? GetExtension(TItem item) => GetFieldValue<string>(item, ExtensionField);

    // ── Lifecycle ───────────────────────────────────────────────────────────────

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadDataAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        // If OnRead is not bound, sync _resolvedItems from the Data parameter
        if (!OnRead.HasDelegate)
            _resolvedItems = Data;
    }

    // ── Data loading ────────────────────────────────────────────────────────────

    /// <summary>
    /// Triggers a data reload. Call this from outside the component to force a rebind.
    /// </summary>
    public Task Rebind() => LoadDataAsync();

    private async Task LoadDataAsync()
    {
        if (OnRead.HasDelegate)
        {
            _readCts?.Cancel();
            _readCts = new CancellationTokenSource();

            var args = new FileManagerReadEventArgs
            {
                Path = Path,
                CancellationToken = _readCts.Token
            };

            await OnRead.InvokeAsync(args);

            if (!_readCts.Token.IsCancellationRequested && args.Data is not null)
            {
                _resolvedItems = args.Data.OfType<TItem>();
                await InvokeAsync(StateHasChanged);
            }
        }
        else
        {
            _resolvedItems = Data;
        }
    }

    // ── Navigation ──────────────────────────────────────────────────────────────

    internal IEnumerable<TItem> GetCurrentItems()
    {
        return _resolvedItems
            .Where(i => GetParentPath(GetPath(i)) == Path.TrimEnd('/'))
            .OrderByDescending(GetIsDirectory)
            .ThenBy(GetName);
    }

    internal IEnumerable<TItem> GetRootFolders()
    {
        return _resolvedItems
            .Where(GetIsDirectory)
            .OrderBy(GetPath);
    }

    private static string GetParentPath(string path)
    {
        var lastSlash = path.TrimEnd('/').LastIndexOf('/');
        return lastSlash <= 0 ? "" : path[..lastSlash];
    }

    internal async Task NavigateTo(string path)
    {
        Path = path;
        await PathChanged.InvokeAsync(Path);
        await LoadDataAsync();
    }

    internal async Task NavigateUp()
    {
        var parent = GetParentPath(Path);
        await NavigateTo(string.IsNullOrEmpty(parent) ? "/" : parent);
    }

    // ── Item actions ────────────────────────────────────────────────────────────

    internal async Task SelectItem(TItem item)
    {
        _selectedItemPath = GetPath(item);
        await OnSelect.InvokeAsync(item);
    }

    internal async Task OpenItem(TItem item)
    {
        if (GetIsDirectory(item))
            await NavigateTo(GetPath(item));
        else
            await OnOpen.InvokeAsync(item);
    }

    internal async Task CreateFolder()
    {
        var args = new FileManagerCreateEventArgs<TItem>();
        await OnCreate.InvokeAsync(args);
    }

    internal async Task DeleteItem(TItem item)
    {
        var args = new FileManagerDeleteEventArgs<TItem> { Item = item };
        await OnDelete.InvokeAsync(args);
    }

    // ── Helpers ─────────────────────────────────────────────────────────────────

    internal bool IsSelected(TItem item) => GetPath(item) == _selectedItemPath;

    internal static string FormatSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        return $"{bytes / (1024.0 * 1024):F1} MB";
    }

    internal string GetHeightStyle()
    {
        return string.IsNullOrEmpty(Height) ? "" : $"height:{Height};";
    }

    // ── IDisposable ─────────────────────────────────────────────────────────────

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _readCts?.Cancel();
            _readCts?.Dispose();
        }
        base.Dispose(disposing);
    }
}
