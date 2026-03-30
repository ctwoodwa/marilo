namespace Marilo.Core.Configuration;

/// <summary>
/// Configuration options for the Marilo component library, typically set during
/// service registration in <c>Program.cs</c> or <c>Startup.cs</c>.
/// </summary>
public class MariloOptions
{
    /// <summary>
    /// Gets or sets the initial theme applied when the application starts.
    /// When <c>null</c>, the provider's default theme is used.
    /// </summary>
    public MariloTheme? Theme { get; set; }
}
