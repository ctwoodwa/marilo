namespace Marilo.Core.Configuration;

public record MariloTheme
{
    public MariloColorPalette Colors { get; init; } = new();
    public MariloTypographyScale Typography { get; init; } = new();
    public MariloShape Shape { get; init; } = new();
    public bool IsRtl { get; init; }
}
