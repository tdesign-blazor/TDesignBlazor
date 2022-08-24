using Microsoft.Extensions.DependencyInjection;

namespace TDesignBlazor;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTDesignBlazor(this IServiceCollection services) => services.AddTDesignBlazor(_ => { });

    public static IServiceCollection AddTDesignBlazor(this IServiceCollection services, Action<TDesignOptions> configure)
    {
        services.Configure(configure);
        services.AddComponentBuilder();
        return services;
    }
}
