using TDesign;
using TDesign.Interceptors;

namespace Microsoft.Extensions.DependencyInjection;
/// <summary>
/// 依赖注入的扩展。
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// 添加 TDesign 组件的默认配置。
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> 实例。</param>
    public static IServiceCollection AddTDesign(this IServiceCollection services) => services.AddTDesign(_ => { });

    /// <summary>
    /// 添加 TDesign 组件的全局配置。
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> 实例。</param>
    /// <param name="configure">个性化配置。</param>
    ///<remarks>暂不对外开放。</remarks>
    internal static IServiceCollection AddTDesign(this IServiceCollection services, Action<TDesignOptions> configure)
    {
        services.Configure(configure);
        services.AddComponentBuilder(cfg =>
        {
            cfg.AddDefaultConfigurations().AddInterceptor<SpecificationInterceptor>();
        });

        services.AddScoped<IMessageService, MessageService>()
            .AddScoped<INotificationService, NotificationService>()
            .AddScoped<IDialogService, DialogService>()
            ;
        return services;
    }
}
