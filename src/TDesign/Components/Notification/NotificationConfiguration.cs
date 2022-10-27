using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign;

/// <summary>
/// 表示消息通知的配置。
/// </summary>
public class NotificationConfiguration
{
    /// <summary>
    /// Gets the key.
    /// </summary>
    internal Guid Key => Guid.NewGuid();
    /// <summary>
    /// 获取或设置标题。
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// 获取或设置内容文本字符串。
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// 获取或设置主题风格。
    /// </summary>
    public Theme Theme { get; set; } = Theme.Primary;
    /// <summary>
    /// 获取或设置消息持续多久自动关闭，单位是毫秒，默认 5 秒，即 5000 毫秒。
    /// </summary>
    public int? Delay { get; set; } = 3000;
}
