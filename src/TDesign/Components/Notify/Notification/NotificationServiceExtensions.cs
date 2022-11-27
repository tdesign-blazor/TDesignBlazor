namespace TDesign;
/// <summary>
/// <see cref="INotificationService"/> 的扩展。
/// </summary>
public static class NotificationServiceExtensions
{
    /// <summary>
    /// 执行普通的提示。
    /// </summary>
    /// <param name="messageService"><see cref="INotificationService"/> 的扩展。</param>
    /// <param name="content">提示的内容。</param>
    /// <param name="title">标题。</param>
    /// <param name="subTitle">副标题。</param>
    /// <param name="placement">显示的位置。</param>
    /// <param name="delay">持续时间，单位毫秒</param>
    /// <param name="icon">自定义图标。</param>
    /// <param name="closable">是否可以被关闭。</param>
    public static Task? Info(this INotificationService messageService,
                             string? content,
                             string? title = default,
                             string? subTitle = default,
                             Placement placement = Placement.TopRight,
                             int? delay = 5000,
                             object? icon = default,
                             bool closable = default)
    => messageService.Show(new()
    {
        Placement = placement,
        Title = title,
        SubTitle = subTitle,
        Content = content,
        Icon = icon,
        Delay = delay,
        Theme = Theme.Primary
    });
    /// <summary>
    /// 执行成功的提示。
    /// </summary>
    /// <param name="messageService"><see cref="INotificationService"/> 的扩展。</param>
    /// <param name="content">提示的内容。</param>
    /// <param name="title">标题。</param>
    /// <param name="subTitle">副标题。</param>
    /// <param name="placement">显示的位置。</param>
    /// <param name="delay">持续时间，单位毫秒</param>
    /// <param name="icon">自定义图标。</param>
    public static Task? Success(this INotificationService messageService,
                             string? content,
                             string? title = default,
                             string? subTitle = default,
                             Placement placement = Placement.TopRight,
                             int? delay = 5000,
                             object? icon = default)
    => messageService.Show(new()
    {
        Placement = placement,
        Content = content,
        Title = title,
        SubTitle = subTitle,
        Icon = icon,
        Delay = delay,
        Theme = Theme.Success
    });

    /// <summary>
    /// 执行警告的提示。
    /// </summary>
    /// <param name="messageService"><see cref="INotificationService"/> 的扩展。</param>
    /// <param name="content">提示的内容。</param>
    /// <param name="title">标题。</param>
    /// <param name="subTitle">副标题。</param>
    /// <param name="placement">显示的位置。</param>
    /// <param name="delay">持续时间，单位毫秒</param>
    /// <param name="icon">自定义图标。</param>
    public static Task? Warning(this INotificationService messageService,
                             string? content,
                             string? title = default,
                             string? subTitle = default,
                             Placement placement = Placement.TopRight,
                             int? delay = 5000,
                             object? icon = default)
    => messageService.Show(new()
    {
        Placement = placement,
        Content = content,
        Title = title,
        SubTitle = subTitle,
        Icon = icon,
        Delay = delay,
        Theme = Theme.Warning
    });

    /// <summary>
    /// 执行错误的提示。
    /// </summary>
    /// <param name="messageService"><see cref="INotificationService"/> 的扩展。</param>
    /// <param name="content">提示的内容。</param>
    /// <param name="title">标题。</param>
    /// <param name="subTitle">副标题。</param>
    /// <param name="placement">显示的位置。</param>
    /// <param name="delay">持续时间，单位毫秒</param>
    /// <param name="icon">自定义图标。</param>
    public static Task? Danger(this INotificationService messageService,
                             string? content,
                             string? title = default,
                             string? subTitle = default,
                             Placement placement = Placement.TopRight,
                             int? delay = 5000,
                             object? icon = default)
    => messageService.Show(new()
    {
        Placement = placement,
        Content = content,
        Title = title,
        SubTitle = subTitle,
        Icon = icon,
        Delay = delay,
        Theme = Theme.Danger
    });
}
