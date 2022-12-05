using TDesign.Notification;

namespace TDesign;

/// <summary>
/// 表示消息通知的配置。
/// </summary>
public class NotificationConfiguration : NotifyConfigurationBase
{
    /// <summary>
    /// 获取或设置标题。
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// 获取或设置副标题。
    /// </summary>
    public string? SubTitle { get; set; }
}
