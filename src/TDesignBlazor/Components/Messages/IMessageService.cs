namespace TDesignBlazor;
/// <summary>
/// 提供全局提示的功能。
/// </summary>
public interface IMessageService : IDisposable
{
    /// <summary>
    /// 显示指定消息配置的全局提示。
    /// </summary>
    /// <param name="configuration">执行消息服务的配置。</param>
    Task? Show(MessageConfiguration configuration);
    /// <summary>
    /// 当消息被关闭后时触发的事件。
    /// </summary>
    event Action? OnClosed;
    /// <summary>
    /// 当消息正在显示时触发的事件。
    /// </summary>

    event Func<MessageConfiguration, Task>? OnShowing;
}
