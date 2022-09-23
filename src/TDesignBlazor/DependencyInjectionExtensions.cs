using Microsoft.Extensions.DependencyInjection;

namespace TDesignBlazor;
/// <summary>
/// 依赖注入的扩展。
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// 添加 TDesignBlazor 的默认配置。
    /// </summary>
    public static IServiceCollection AddTDesignBlazor(this IServiceCollection services) => services.AddTDesignBlazor(_ => { });

    /// <summary>
    /// 添加 TDesignBlazor 的默认配置。
    /// </summary>
    /// <param name="configure">个性化配置。</param>
    internal static IServiceCollection AddTDesignBlazor(this IServiceCollection services, Action<TDesignOptions> configure)
    {
        services.Configure(configure);
        services.AddComponentBuilder();
        return services;
    }
}
