using Marilo.Core.Contracts;
using Marilo.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Marilo.Icons.Extensions;

public static class MariloBuilderExtensions
{
    public static MariloBuilder UseMariloIcons(this MariloBuilder builder)
    {
        builder.Services.AddScoped<IMariloIconProvider, MariloIconProvider>();
        return builder;
    }
}
