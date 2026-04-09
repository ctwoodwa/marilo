using Marilo.Core.Contracts;
using Marilo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo.Icons.Extensions;

/// <summary>
/// Extension methods for registering the legacy Marilo icon provider.
/// </summary>
public static class MariloBuilderExtensions
{
    /// <summary>
    /// Registers the legacy Marilo custom icon provider via the builder pattern.
    /// </summary>
    [Obsolete("Use AddMariloIconsTabler() from Marilo.Icons.Tabler instead.")]
    public static MariloBuilder UseMariloIcons(this MariloBuilder builder)
    {
        builder.Services.AddScoped<IMariloIconProvider, MariloIconProvider>();
        return builder;
    }

    /// <summary>
    /// Registers the legacy Marilo custom icon provider as a standalone service registration.
    /// </summary>
    [Obsolete("Use AddMariloIconsTabler() from Marilo.Icons.Tabler instead.")]
    public static IServiceCollection AddMariloIcons(this IServiceCollection services)
    {
        services.AddSingleton<IMariloIconProvider, MariloIconProvider>();
        return services;
    }
}
