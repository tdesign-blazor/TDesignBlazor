using OneOf;

namespace TDesignBlazor;

/// <summary>
/// 表示全局消息的配置。
/// </summary>
/// <remarks>该对象用于 <see cref="IMessageService"/> 调用时传值给 <see cref="Message"/> 组件。</remarks>
public class MessageConfiguration
{
    internal Guid Key => Guid.NewGuid();
    /// <summary>
    /// 获取或设置消息提示的内容文本字符串。
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// 获取或设置消息提示的图标。
    /// </summary>
    public object? Icon { get; set; }
    /// <summary>
    /// 获取或设置消息提示的主题风格。
    /// </summary>
    public Theme Theme { get; set; } = Theme.Primary;

    /// <summary>
    /// 获取或设置消息提示具备加载中的状态。
    /// </summary>
    public bool Loading { get; set; }
    /// <summary>
    /// 获取或设置消息提示可以被用户关闭。
    /// </summary>
    public bool Closable { get; set; }

    /// <summary>
    /// 获取或设置消息提示持续多久自动关闭，单位是毫秒，默认 5 秒，即 5000 毫秒。
    /// </summary>
    public int? Delay { get; set; } = 3000;

    /// <summary>
    /// 获取或设置显示的位置。
    /// </summary>
    public OneOf<Placement, (int offsetX, int offsetY)> Placement { get; set; } = OneOf<Placement, (int offsetX, int offsetY)>.FromT0(TDesignBlazor.Placement.TopRight);

    /// <summary>
    /// 获取显示的位置。
    /// </summary>
    /// <returns>class 或 style</returns>
    public static (bool classOrStyle, string value) GetPlacement(OneOf<Placement, (int offsetX, int offsetY)> placement)
        => placement.Match(
            p => (true, p.GetCssClass()),
            value => new(false, $"left:{value.offsetX}px;top:{value.offsetY}")
        );
}
