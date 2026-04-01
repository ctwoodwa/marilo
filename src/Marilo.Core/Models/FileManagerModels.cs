namespace Marilo.Core.Models;

/// <summary>
/// Represents a file or folder entry in the MariloFileManager.
/// </summary>
public class FileManagerEntry
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public bool IsDirectory { get; set; }
    public long Size { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateCreated { get; set; }
    public string? Extension { get; set; }
}

/// <summary>
/// Specifies the view mode for the file manager.
/// </summary>
public enum FileManagerViewMode
{
    /// <summary>List view with details.</summary>
    List,

    /// <summary>Grid/thumbnail view.</summary>
    Grid
}
