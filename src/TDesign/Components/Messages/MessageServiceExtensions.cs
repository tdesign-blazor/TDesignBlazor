namespace TDesign;
/// <summary>
/// <see cref="IMessageService"/> 的扩展。
/// </summary>
public static class MessageServiceExtensions
{
    /// <summary>
    /// 执行普通的提示。
    /// </summary>
    /// <param name="messageService"><see cref="IMessageService"/> 的扩展。</param>
    /// <param name="content">提示的内容。</param>
    /// <param name="delay">持续时间，单位毫秒</param>
    /// <param name="icon">自定义图标。</param>
    /// <param name="closable">是否可以被关闭。</param>
    public static Task? Info(this IMessageService messageService,
                             string? content,
                             Placement placement = Placement.TopCenter,
                             int? delay = 5000,
                             object? icon = default,
                             bool closable = default)
    => messageService.Show(new()
    {
        Placement = placement,
        Closable = closable,
        Content = content,
        Icon = icon,
        Delay = delay,
        Theme = Theme.Primary
    });
    /// <summary>
    /// 执行成功的提示。
    /// </summary>
    /// <param name="messageService"><see cref="IMessageService"/> 的扩展。</param>
    /// <param name="content">提示的内容。</param>
    /// <param name="delay">持续时间，单位毫秒</param>
    /// <param name="icon">自定义图标。</param>
    /// <param name="closable">是否可以被关闭。</param>
    public static Task? Success(this IMessageService messageService,
                             string? content,
                             Placement placement = Placement.TopCenter,
                             int? delay = 5000,
                             object? icon = default,
                             bool closable = default)
    => messageService.Show(new()
    {
        Placement = placement,
        Closable = closable,
        Content = content,
        Icon = icon,
        Delay = delay,
        Theme = Theme.Success
    });

    /// <summary>
    /// 执行警告的提示。
    /// </summary>
    /// <param name="messageService"><see cref="IMessageService"/> 的扩展。</param>
    /// <param name="content">提示的内容。</param>
    /// <param name="delay">持续时间，单位毫秒</param>
    /// <param name="icon">自定义图标。</param>
    /// <param name="closable">是否可以被关闭。</param>
    public static Task? Warning(this IMessageService messageService,
                             string? content,
                             Placement placement = Placement.TopCenter,
                             int? delay = 5000,
                             object? icon = default,
                             bool closable = default)
    => messageService.Show(new()
    {
        Placement = placement,
        Closable = closable,
        Content = content,
        Icon = icon,
        Delay = delay,
        Theme = Theme.Warning
    });

    /// <summary>
    /// 执行错误的提示。
    /// </summary>
    /// <param name="messageService"><see cref="IMessageService"/> 的扩展。</param>
    /// <param name="content">提示的内容。</param>
    /// <param name="delay">持续时间，单位毫秒</param>
    /// <param name="icon">自定义图标。</param>
    /// <param name="closable">是否可以被关闭。</param>
    public static Task? Danger(this IMessageService messageService,
                             string? content,
                             Placement placement = Placement.TopCenter,
                             int? delay = 5000,
                             object? icon = default,
                             bool closable = default)
    => messageService.Show(new()
    {
        Placement = placement,
        Closable = closable,
        Content = content,
        Icon = icon,
        Delay = delay,
        Theme = Theme.Danger
    });

    /// <summary>
    /// 执行加载中的提示。
    /// </summary>
    /// <param name="messageService"><see cref="IMessageService"/> 的扩展。</param>
    /// <param name="content">提示的内容。</param>
    /// <param name="delay">持续时间，单位毫秒</param>
    /// <param name="icon">自定义图标。</param>
    /// <param name="closable">是否可以被关闭。</param>
    public static Task? Loading(this IMessageService messageService,
                             string? content,
                             Placement placement = Placement.TopCenter,
                             int? delay = 5000,
                             object? icon = default,
                             bool closable = default)
    => messageService.Show(new()
    {
        Placement = placement,
        Closable = closable,
        Content = content,
        Icon = icon,
        Delay = delay,
        Loading = true
    });

    /// <summary>
    /// 执行有疑问的提示。
    /// </summary>
    /// <param name="messageService"><see cref="IMessageService"/> 的扩展。</param>
    /// <param name="content">提示的内容。</param>
    /// <param name="placement">显示的位置。</param>
    /// <param name="delay">持续时间，单位毫秒</param>
    /// <param name="icon">自定义图标。</param>
    /// <param name="closable">是否可以被关闭。</param>
    public static Task? Question(this IMessageService messageService,
                             string? content,
                             Placement placement = Placement.TopCenter,
                             int? delay = 5000,
                             object? icon = default,
                             bool closable = default)
    => messageService.Show(new()
    {
        Placement = placement,
        Closable = closable,
        Content = content,
        Icon = icon,
        Delay = delay,
        Theme = MessageTheme.Question
    });
}
