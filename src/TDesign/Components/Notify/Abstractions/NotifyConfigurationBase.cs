namespace TDesign.Notification;
public abstract class NotifyConfigurationBase
{
    /// <summary>
    /// Gets the key.
    /// </summary>
    internal Guid Key => Guid.NewGuid();
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
    /// <summary>
    /// 获取或设置消息提示的图标。
    /// </summary>
    public object? Icon { get; set; }

    /// <summary>
    /// 获取或设置显示的位置。
    /// </summary>
    public OneOf<Placement, (int offsetX, int offsetY)> Placement { get; set; } = OneOf<Placement, (int offsetX, int offsetY)>.FromT0(TDesign.Placement.TopRight);

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
