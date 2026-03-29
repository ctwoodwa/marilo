using Marilo.Core.Contracts;
using Marilo.Core.Services;
using Microsoft.AspNetCore.Components;

namespace Marilo.Core.Base;

public abstract class MariloComponentBase : ComponentBase, IDisposable
{
    [Inject] protected IMariloCssProvider CssProvider { get; set; } = default!;
    [Inject] protected IMariloIconProvider IconProvider { get; set; } = default!;
    [Inject] protected IMariloThemeService ThemeService { get; set; } = default!;

    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    protected CssClassBuilder ClassBuilder { get; } = new();
    protected StyleBuilder StyleBuilder { get; } = new();

    protected bool IsRtl => ThemeService.IsRtl;

    protected string CombineClasses(string baseClass)
    {
        return ClassBuilder.Clear()
            .AddClass(baseClass)
            .AddClass(Class)
            .Build();
    }

    protected string CombineStyles(string? baseStyle = null)
    {
        return StyleBuilder.Clear()
            .AddStyle(baseStyle)
            .AddStyle(Style)
            .Build();
    }

    protected void SetAria(string key, object? value)
    {
        if (value != null)
        {
            AdditionalAttributes ??= new Dictionary<string, object>();
            AdditionalAttributes[$"aria-{key}"] = value;
        }
    }

    private bool _disposed;

    public void Dispose()
    {
        if (!_disposed)
        {
            Dispose(disposing: true);
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
}
