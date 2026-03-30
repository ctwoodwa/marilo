namespace Marilo.Core.Configuration;

/// <summary>
/// Immutable snapshot of a Marilo theme, including color palette, typography scale,
/// shape tokens, and layout direction. Passed to <see cref="Services.IMariloThemeService"/>
/// to apply a new visual appearance at runtime.
/// </summary>
public record MariloTheme
{
    /// <summary>
    /// Gets the color palette (primary, secondary, surface, semantic colors, etc.).
    /// </summary>
    public MariloColorPalette Colors { get; init; } = new();

    /// <summary>
    /// Gets the typography scale (font families, sizes, weights, line heights).
    /// </summary>
    public MariloTypographyScale Typography { get; init; } = new();

    /// <summary>
    /// Gets the shape tokens (border radius values, elevation levels).
    /// </summary>
    public MariloShape Shape { get; init; } = new();

    /// <summary>
    /// Gets a value indicating whether the theme uses right-to-left layout direction.
    /// </summary>
    public bool IsRtl { get; init; }
}
