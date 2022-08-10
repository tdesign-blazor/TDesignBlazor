using Microsoft.Extensions.DependencyInjection;

namespace TDesignBlazor;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTDesign(this IServiceCollection services)
    {
        services.AddComponentBuilder();
        return services;
    }
}
