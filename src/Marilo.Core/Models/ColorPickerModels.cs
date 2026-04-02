namespace Marilo.Core.Models;

/// <summary>
/// Event arguments for the <c>OnOpen</c> event of a <c>MariloColorPicker</c>.
/// Set <see cref="IsCancelled"/> to <c>true</c> to prevent the popup from opening.
/// </summary>
public class ColorPickerOpenEventArgs
{
    /// <summary>Set to <c>true</c> to cancel the popup opening.</summary>
    public bool IsCancelled { get; set; }
}

/// <summary>
/// Event arguments for the <c>OnClose</c> event of a <c>MariloColorPicker</c>.
/// Set <see cref="IsCancelled"/> to <c>true</c> to prevent the popup from closing.
/// </summary>
public class ColorPickerCloseEventArgs
{
    /// <summary>Set to <c>true</c> to cancel the popup closing.</summary>
    public bool IsCancelled { get; set; }
}

/// <summary>
/// Provides preset color palette collections for the <c>MariloColorPicker</c> palette view.
/// </summary>
public static class ColorPalettePresets
{
    /// <summary>
    /// A basic palette of 20 common colors.
    /// </summary>
    public static readonly IEnumerable<string> Basic = new[]
    {
        "#ffffff", "#c0c0c0", "#808080", "#000000",
        "#ff0000", "#800000", "#ffff00", "#808000",
        "#00ff00", "#008000", "#00ffff", "#008080",
        "#0000ff", "#000080", "#ff00ff", "#800080",
        "#ff8040", "#804000", "#ff80c0", "#804060"
    };

    /// <summary>
    /// An extended palette of 40 colors covering a broad spectrum.
    /// </summary>
    public static readonly IEnumerable<string> Extended = new[]
    {
        "#ffffff", "#e0e0e0", "#c0c0c0", "#a0a0a0", "#808080", "#404040", "#202020", "#000000",
        "#ffcccc", "#ff8080", "#ff0000", "#cc0000", "#800000", "#400000",
        "#ffddbb", "#ffaa55", "#ff8000", "#cc6600", "#804000",
        "#ffff99", "#ffff00", "#cccc00", "#808000", "#404000",
        "#ccffcc", "#80ff80", "#00ff00", "#00cc00", "#008000", "#004000",
        "#ccffff", "#80ffff", "#00ffff", "#00cccc", "#008080", "#004040",
        "#ccccff", "#8080ff", "#0000ff", "#0000cc", "#000080", "#000040"
    };

    /// <summary>
    /// A flat design-inspired palette of material-style colors.
    /// </summary>
    public static readonly IEnumerable<string> Flat = new[]
    {
        "#e74c3c", "#c0392b", "#e67e22", "#d35400", "#f1c40f", "#f39c12",
        "#2ecc71", "#27ae60", "#1abc9c", "#16a085", "#3498db", "#2980b9",
        "#9b59b6", "#8e44ad", "#34495e", "#2c3e50", "#95a5a6", "#7f8c8d",
        "#ecf0f1", "#bdc3c7"
    };
}
