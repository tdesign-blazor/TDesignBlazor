namespace TDesign;

/// <summary>
/// 风格主题的配色方案。
/// </summary>
public class Theme : Enumeration
{
    /// <summary>
    /// 初始化 <see cref="Theme"/> 类的新实例。
    /// </summary>
    /// <param name="value">配色的指。</param>
    protected internal Theme(string value) : base(value)
    {
    }
    /// <summary>
    /// 主配色。
    /// </summary>
    public static readonly Theme Primary = nameof(Primary);
    /// <summary>
    /// 危险级别的配色。
    /// </summary>
    public static readonly Theme Danger = nameof(Danger);
    /// <summary>
    /// 警告级别的配色。
    /// </summary>
    public static readonly Theme Warning = nameof(Warning);
    /// <summary>
    /// 成功级别的配色。
    /// </summary>
    public static readonly Theme Success = nameof(Success);
    /// <summary>
    /// 自定义的配色方案。
    /// </summary>
    /// <param name="name">配色方案的名称。</param>
    public static implicit operator Theme(string name) => new(name.ToLower());
}

/// <summary>
/// 消息主题配色方案。
/// </summary>
public class MessageTheme : Theme
{
    /// <summary>
    /// 初始化 <see cref="MessageTheme"/> 类的新实例。
    /// </summary>
    /// <param name="value"></param>
    internal protected MessageTheme(string value) : base(value)
    {
    }
    /// <summary>
    /// 疑问的主题配色。
    /// </summary>
    public static readonly Theme Question = nameof(Question);
}
/// <summary>
/// <see cref="TTooltip"/> 组件的主题配色。
/// </summary>
public class TooltipTheme : Theme
{
    /// <summary>
    /// 初始化 <see cref="TooltipTheme"/> 类的新实例。
    /// </summary>
    /// <param name="value"></param>
    internal protected TooltipTheme(string value) : base(value)
    {
    }
    /// <summary>
    /// 默认配色。
    /// </summary>
    public static readonly Theme Default = nameof(Default);
    /// <summary>
    /// 轻量级配色。
    /// </summary>
    public static readonly Theme Light = nameof(Light);
}