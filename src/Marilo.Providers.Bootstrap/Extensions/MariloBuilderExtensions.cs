using Marilo.Core.Contracts;
using Marilo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo.Providers.Bootstrap.Extensions;

public class BootstrapOptions
{
    public Marilo.Core.Configuration.MariloTheme? Theme { get; set; }
}

public static class MariloBuilderExtensions
{
    public static MariloBuilder UseBootstrap(this MariloBuilder builder, Action<BootstrapOptions>? configure = null)
    {
        var options = new BootstrapOptions();
        configure?.Invoke(options);
        builder.Services.AddSingleton(options);
        builder.Services.AddScoped<IMariloCssProvider, BootstrapCssProvider>();
        builder.Services.AddScoped<IMariloIconProvider, BootstrapIconProvider>();
        builder.Services.AddScoped<IMariloJsInterop, BootstrapJsInterop>();
        return builder.AddMariloCoreServices();
    }
}
