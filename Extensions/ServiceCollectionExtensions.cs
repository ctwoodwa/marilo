using Marilo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo;

/// <summary>
/// Extension methods for registering Marilo UI services.
/// Usage: builder.Services.AddMarilo();
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Marilo UI shared services:
    /// AuthStateProvider, ThemeService, DataChangeService, and MarkdownService.
    /// </summary>
    public static IServiceCollection AddMarilo(this IServiceCollection services)
    {
        services.AddScoped<AuthStateProvider>();
        services.AddScoped<ThemeService>();
        services.AddScoped<DataChangeService>();
        services.AddSingleton<MarkdownService>();

        return services;
    }
}
