using TDesign.Notification;

namespace TDesign;

/// <summary>
/// 表示全局消息的配置。
/// </summary>
/// <remarks>该对象用于 <see cref="IMessageService"/> 调用时传值给 <see cref="TMessage"/> 组件。</remarks>
public class MessageConfiguration : NotifyConfigurationBase
{
    /// <summary>
    /// 获取或设置消息提示具备加载中的状态。
    /// </summary>
    public bool Loading { get; set; }
    /// <summary>
    /// 获取或设置消息提示可以被用户关闭。
    /// </summary>
    public bool Closable { get; set; }
}
