using Marilo.Core.Configuration;
using Marilo.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo.Core.Extensions;

public class MariloBuilder
{
    public IServiceCollection Services { get; }

    public MariloBuilder(IServiceCollection services)
    {
        Services = services;
    }
}

public static class ServiceCollectionExtensions
{
    public static MariloBuilder AddMarilo(this IServiceCollection services, Action<MariloOptions>? configure = null)
    {
        var options = new MariloOptions();
        configure?.Invoke(options);
        services.AddSingleton(options);
        return new MariloBuilder(services);
    }

    public static MariloBuilder AddMariloCoreServices(this MariloBuilder builder)
    {
        builder.Services.AddScoped<IMariloThemeService, ThemeService>();
        builder.Services.AddScoped<IMariloNotificationService, MariloNotificationService>();
        return builder;
    }
}
