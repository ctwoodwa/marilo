using Marilo.Core.Contracts;
using Marilo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo.Providers.FluentUI.Extensions;

public class FluentUIOptions
{
    public Marilo.Core.Configuration.MariloTheme? Theme { get; set; }
}

public static class MariloBuilderExtensions
{
    public static MariloBuilder UseFluentUI(this MariloBuilder builder, Action<FluentUIOptions>? configure = null)
    {
        var options = new FluentUIOptions();
        configure?.Invoke(options);
        builder.Services.AddSingleton(options);
        builder.Services.AddScoped<IMariloCssProvider, FluentUICssProvider>();
        builder.Services.AddScoped<IMariloIconProvider, FluentUIIconProvider>();
        builder.Services.AddScoped<IMariloJsInterop, FluentUIJsInterop>();
        return builder.AddMariloCoreServices();
    }
}
