namespace Marilo.Components.Forms.Inputs;

/// <summary>
/// Represents the status of a file in the upload process.
/// </summary>
public enum UploadFileStatus
{
    /// <summary>File has been selected but not yet uploaded.</summary>
    Selected,

    /// <summary>File is currently being uploaded.</summary>
    Uploading,

    /// <summary>File has been successfully uploaded.</summary>
    Uploaded,

    /// <summary>File upload failed.</summary>
    Failed
}

/// <summary>
/// Represents information about a file being uploaded.
/// </summary>
public class UploadFileInfo
{
    /// <summary>The name of the file.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The size of the file in bytes.</summary>
    public long Size { get; set; }

    /// <summary>The current upload status.</summary>
    public UploadFileStatus Status { get; set; }

    /// <summary>The upload progress percentage (0-100).</summary>
    public int Progress { get; set; }
}

/// <summary>
/// Event arguments for upload and remove events.
/// </summary>
public class UploadEventArgs
{
    /// <summary>The file associated with this event.</summary>
    public UploadFileInfo File { get; set; } = default!;
}

/// <summary>
/// Event arguments for upload error events.
/// </summary>
public class UploadErrorEventArgs : UploadEventArgs
{
    /// <summary>The error message describing the failure.</summary>
    public string ErrorMessage { get; set; } = string.Empty;
}

/// <summary>
/// Event arguments for file selection events.
/// </summary>
public class UploadSelectEventArgs
{
    /// <summary>The collection of files that were selected.</summary>
    public IEnumerable<UploadFileInfo> Files { get; set; } = [];
}
