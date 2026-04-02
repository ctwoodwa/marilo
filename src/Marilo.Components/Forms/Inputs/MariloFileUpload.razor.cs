using Marilo.Core.Base;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Marilo.Components.Forms.Inputs;

/// <summary>
/// A client-side file selection component (FileSelect) that wraps Blazor's InputFile
/// with drag-and-drop support, client-side validation, and a file list with remove buttons.
/// For server-upload functionality, use <see cref="MariloUpload"/> instead.
/// </summary>
public partial class MariloFileUpload : MariloComponentBase
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    // ── Parameters: Core ────────────────────────────────────────────────────

    /// <summary>The accept attribute passed to the underlying input element.</summary>
    [Parameter] public string? Accept { get; set; }

    /// <summary>List of allowed file extensions (e.g. ".pdf", ".jpg"). Validated after selection.</summary>
    [Parameter] public List<string>? AllowedExtensions { get; set; }

    /// <summary>The capture attribute for the file input (e.g. "user", "environment").</summary>
    [Parameter] public string? Capture { get; set; }

    /// <summary>When true, allows multiple files to be selected at once.</summary>
    [Parameter] public bool Multiple { get; set; } = true;

    /// <summary>Maximum allowed file size in bytes. Null means no limit.</summary>
    [Parameter] public long? MaxFileSize { get; set; }

    /// <summary>Minimum allowed file size in bytes. Null means no limit.</summary>
    [Parameter] public long? MinFileSize { get; set; }

    /// <summary>When false, disables file selection.</summary>
    [Parameter] public bool Enabled { get; set; } = true;

    /// <summary>An id attribute to render on the file input element.</summary>
    [Parameter] public string? Id { get; set; }

    /// <summary>
    /// The id of an external drop zone element. When set, the component connects to
    /// the external drop zone for drag-and-drop events.
    /// </summary>
    [Parameter] public string? DropZoneId { get; set; }

    /// <summary>Pre-populated file entries shown in the file list on load.</summary>
    [Parameter] public IEnumerable<FileSelectFileInfo>? Files { get; set; }

    // ── Parameters: Templates ───────────────────────────────────────────────

    /// <summary>Custom template for the select-files button.</summary>
    [Parameter] public RenderFragment? SelectFilesButtonTemplate { get; set; }

    /// <summary>Custom template for each file item in the list. Context is FileSelectFileInfo.</summary>
    [Parameter] public RenderFragment<FileSelectFileInfo>? FileTemplate { get; set; }

    /// <summary>Custom template for the file info section within each built-in file item.</summary>
    [Parameter] public RenderFragment<FileSelectFileInfo>? FileInfoTemplate { get; set; }

    // ── Parameters: Events ──────────────────────────────────────────────────

    /// <summary>Fires when the user selects one or more files.</summary>
    [Parameter] public EventCallback<FileSelectEventArgs> OnSelect { get; set; }

    /// <summary>Fires when the user removes a file from the list.</summary>
    [Parameter] public EventCallback<FileSelectEventArgs> OnRemove { get; set; }

    // ── Internal state ───────────────────────────────────────────────────────

    private readonly List<FileSelectFileInfo> _files = [];
    private bool _isDragOver;
    private bool _filesParamInitialized;
    private string InputId => Id ?? $"mar-fileupload-{_componentId}";
    private readonly string _componentId = Guid.NewGuid().ToString("N")[..8];

    // ── Lifecycle ────────────────────────────────────────────────────────────

    protected override void OnParametersSet()
    {
        // Load Files parameter on first render only (behaves like initial files)
        if (!_filesParamInitialized && Files != null)
        {
            foreach (var f in Files)
            {
                if (!_files.Any(x => x.Id == f.Id))
                    _files.Add(f);
            }
            _filesParamInitialized = true;
        }
    }

    // ── Public API (programmatic methods) ───────────────────────────────────

    /// <summary>Clears all files from the list.</summary>
    public void ClearFiles()
    {
        _files.Clear();
        StateHasChanged();
    }

    /// <summary>Removes the file with the given id from the list.</summary>
    public async Task RemoveFileAsync(string fileId)
    {
        var file = _files.FirstOrDefault(f => f.Id == fileId);
        if (file != null)
            await RemoveFileInternalAsync(file);
    }

    /// <summary>
    /// Opens the browser file selection dialog programmatically.
    /// Note: Does not work in Safari due to browser security restrictions.
    /// </summary>
    public async Task OpenSelectFilesDialog()
    {
        await JS.InvokeVoidAsync("eval", $"document.getElementById('{InputId}')?.click()");
    }

    // ── Event handlers ───────────────────────────────────────────────────────

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        if (!Enabled) return;

        const int maxFiles = 1000;
        var browserFiles = Multiple
            ? e.GetMultipleFiles(maxFiles).ToList()
            : [e.File];

        var newEntries = new List<FileSelectFileInfo>();

        foreach (var bf in browserFiles)
        {
            var ext = System.IO.Path.GetExtension(bf.Name)?.ToLowerInvariant() ?? string.Empty;
            var entry = new FileSelectFileInfo
            {
                Name = bf.Name,
                Size = bf.Size,
                Extension = ext,
                BrowserFile = bf,
                InvalidExtension = IsInvalidExtension(ext),
                InvalidMaxFileSize = MaxFileSize.HasValue && bf.Size > MaxFileSize.Value,
                InvalidMinFileSize = MinFileSize.HasValue && bf.Size < MinFileSize.Value
            };

            // Open a stream for the consumer to read file contents
            var maxSize = MaxFileSize ?? 2L * 1024 * 1024 * 1024; // 2 GB default cap
            try
            {
                entry.Stream = bf.OpenReadStream(maxSize);
            }
            catch
            {
                // Stream will be null; consumer must handle
            }

            _files.Add(entry);
            newEntries.Add(entry);
        }

        var args = new FileSelectEventArgs { Files = newEntries };
        await OnSelect.InvokeAsync(args);

        if (args.IsCancelled)
        {
            // Remove the just-added files
            foreach (var e2 in newEntries)
                _files.Remove(e2);
        }
    }

    private async Task HandleDrop(Microsoft.AspNetCore.Components.Web.DragEventArgs e)
    {
        _isDragOver = false;
        // Drag-and-drop file reading is handled by the InputFile inside the label;
        // when the user drops onto the label element the browser fires the change event.
    }

    private void HandleDragEnter(Microsoft.AspNetCore.Components.Web.DragEventArgs e) => _isDragOver = true;
    private void HandleDragOver(Microsoft.AspNetCore.Components.Web.DragEventArgs e) { }
    private void HandleDragLeave(Microsoft.AspNetCore.Components.Web.DragEventArgs e) => _isDragOver = false;

    private async Task RemoveFileInternalAsync(FileSelectFileInfo file)
    {
        var args = new FileSelectEventArgs { Files = [file] };
        await OnRemove.InvokeAsync(args);

        if (!args.IsCancelled)
        {
            file.Stream?.Dispose();
            _files.Remove(file);
        }
    }

    // ── Validation helpers ───────────────────────────────────────────────────

    private bool IsInvalidExtension(string ext)
    {
        if (AllowedExtensions == null || !AllowedExtensions.Any()) return false;
        return !AllowedExtensions.Select(e => e.ToLowerInvariant()).Contains(ext);
    }

    // ── Formatting helpers ───────────────────────────────────────────────────

    private static string FormatFileSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        if (bytes < 1024L * 1024 * 1024) return $"{bytes / (1024.0 * 1024):F1} MB";
        return $"{bytes / (1024.0 * 1024 * 1024):F1} GB";
    }

    private static string ValidationSummary(FileSelectFileInfo f)
    {
        var msgs = new List<string>();
        if (f.InvalidExtension) msgs.Add("invalid file type");
        if (f.InvalidMaxFileSize) msgs.Add("file too large");
        if (f.InvalidMinFileSize) msgs.Add("file too small");
        return string.Join(", ", msgs);
    }

    // ── CSS helpers ──────────────────────────────────────────────────────────

    private string DropZoneCssClass()
    {
        var cls = "mar-file-upload__zone";
        if (_isDragOver) cls += " mar-file-upload__zone--dragover";
        if (!Enabled) cls += " mar-file-upload__zone--disabled";
        return cls;
    }
}
