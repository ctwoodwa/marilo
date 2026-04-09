using Marilo.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo.Icons.Tabler;

/// <summary>
/// Extension methods for registering the Tabler icon provider with the DI container.
/// </summary>
public static class TablerIconServiceExtensions
{
    /// <summary>
    /// Registers the Tabler SVG sprite icon provider as the <see cref="IMariloIconProvider"/>.
    /// Tabler Icons are MIT licensed (<see href="https://tabler.io/icons"/>).
    /// </summary>
    public static IServiceCollection AddMariloIconsTabler(this IServiceCollection services)
    {
        services.AddSingleton<IMariloIconProvider, TablerIconProvider>();
        return services;
    }
}
