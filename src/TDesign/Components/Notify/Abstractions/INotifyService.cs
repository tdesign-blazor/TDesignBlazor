namespace TDesign.Notification;
/// <summary>
/// 提供可以弹出通知提示的服务。
/// </summary>
/// <typeparam name="TConfiguration">配置的类型。</typeparam>
public interface INotifyService<TConfiguration> where TConfiguration : NotifyConfigurationBase
{
    /// <summary>
    /// 显示指定消息配置的全局提示。
    /// </summary>
    /// <param name="configuration">消息服务的配置。</param>
    Task? Show(TConfiguration configuration);
    /// <summary>
    /// 当消息被关闭后时触发的事件。
    /// </summary>
    event Action? OnClosed;
    /// <summary>
    /// 当消息正在显示时触发的事件。
    /// </summary>

    event Func<TConfiguration, Task>? OnShowing;
}
