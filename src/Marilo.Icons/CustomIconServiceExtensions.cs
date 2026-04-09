using Marilo.Core.Configuration;
using Marilo.Core.Contracts;
using Marilo.Core.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo.Icons;

/// <summary>
/// Extension methods for registering a custom icon provider with the DI container.
/// </summary>
public static class CustomIconServiceExtensions
{
    /// <summary>
    /// Registers a custom SVG sprite or CSS-class icon provider.
    /// Use this to integrate Font Awesome, Bootstrap Icons, or a proprietary icon set.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">A delegate to configure <see cref="IconOptions"/>.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMariloIconsCustom(
        this IServiceCollection services,
        Action<IconOptions> configure)
    {
        var opts = new IconOptions();
        configure(opts);

        IMariloIconProvider provider = opts.RenderMode switch
        {
            IconRenderMode.CssClass  => new CssIconProvider(opts),
            IconRenderMode.SvgSprite => new CustomSpriteIconProvider(opts),
            _ => throw new NotSupportedException(
                $"RenderMode {opts.RenderMode} is not supported by AddMariloIconsCustom.")
        };

        services.AddSingleton(provider);
        return services;
    }
}
