using Marilo.Core.Contracts;
using Marilo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo.Providers.Material.Extensions;

public class MaterialOptions
{
    public Marilo.Core.Configuration.MariloTheme? Theme { get; set; }
}

public static class MariloBuilderExtensions
{
    public static MariloBuilder UseMaterial(this MariloBuilder builder, Action<MaterialOptions>? configure = null)
    {
        var options = new MaterialOptions();
        configure?.Invoke(options);
        builder.Services.AddSingleton(options);
        builder.Services.AddScoped<IMariloCssProvider, MaterialCssProvider>();
        builder.Services.AddScoped<IMariloIconProvider, MaterialIconProvider>();
        builder.Services.AddScoped<IMariloJsInterop, MaterialJsInterop>();
        return builder.AddMariloCoreServices();
    }
}
