namespace TDesign.Notification;
/// <summary>
/// 通知服务的默认实现基类。
/// </summary>
/// <typeparam name="TConfiguration"></typeparam>
internal abstract class NotifyServiceBase<TConfiguration> : INotifyService<TConfiguration> where TConfiguration : NotifyConfigurationBase
{

    public event Action? OnClosed;
    public event Func<TConfiguration, Task>? OnShowing;
    /// <inheritdoc/>
    public void Dispose() => OnClosed?.Invoke();

    /// <summary>
    /// 显示指定消息配置的全局提示。
    /// </summary>
    /// <param name="configuration">消息服务的配置。</param>
    public virtual async Task Show(TConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (OnShowing is not null)
        {
            await OnShowing.Invoke(configuration);
        }
    }
}
