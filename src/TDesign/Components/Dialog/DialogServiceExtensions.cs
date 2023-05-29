using TDesign.Templates;

namespace TDesign;

/// <summary>
/// <see cref="IDialogService"/> 的扩展。
/// </summary>
public static class DialogServiceExtensions
{
    /// <summary>
    /// 打开指定模板组件的对话框。
    /// </summary>
    /// <typeparam name="TDialogTemplate">对话框模板。</typeparam>
    /// <param name="service"></param>
    /// <param name="content">内容。</param>
    /// <param name="title">标题。</param>
    /// <param name="icon">显示的图标。</param>
    /// <param name="iconTheme">图标主题颜色。</param>
    /// <param name="parameters">自定义参数。</param>
    public static Task<IDialogReference> Open<TDialogTemplate>(this IDialogService service, RenderFragment? content, RenderFragment? title, IconName? icon = default, Theme? iconTheme = default, DialogParameters? parameters = default) where TDialogTemplate : IComponent
    {
        parameters ??= new DialogParameters();

        parameters.SetTitle(title);
        parameters.SetContent(content);
        parameters.SetIcon(icon);
        parameters.SetIconTheme(iconTheme);

        return service.Open<TDialogTemplate>(parameters);
    }

    /// <summary>
    /// 打开指定模板组件的对话框。
    /// </summary>
    /// <typeparam name="TDialogTemplate">对话框模板。</typeparam>
    /// <param name="service"></param>
    /// <param name="content">内容。</param>
    /// <param name="title">标题。</param>
    /// <param name="icon">显示的图标。</param>
    /// <param name="iconTheme">图标主题颜色。</param>
    /// <param name="parameters">自定义参数。</param>
    public static Task<IDialogReference> Open<TDialogTemplate>(this IDialogService service, string? content, string? title, IconName? icon = default, Theme? iconTheme = default, DialogParameters? parameters = default) where TDialogTemplate : IComponent
        => service.Open<TDialogTemplate>(builder => builder.AddContent(0, content), builder => builder.AddContent(0, title), icon, iconTheme, parameters);

    /// <summary>
    /// 打开显示消息的对话框。
    /// </summary>
    /// <param name="content">内容。</param>
    /// <param name="title">标题。</param>
    /// <param name="icon">显示的图标。</param>
    /// <param name="iconTheme">图标主题颜色。</param>
    public static Task<IDialogReference> OpenMessage(this IDialogService dialogService, string? content = default, string? title = "提示", IconName? icon = default, Theme? iconTheme = default)
    => dialogService.Open<MessageDialogTemplate>(content, title, icon, iconTheme);
    /// <summary>
    /// 打开提示消息的对话框。
    /// </summary>
    /// <param name="content">内容。</param>
    /// <param name="title">标题。</param>
    public static Task<IDialogReference> OpenInfo(this IDialogService dialogService, string? content = default, string? title = "提示")
    => dialogService.OpenMessage(content, title, IconName.InfoCircleFilled, Theme.Primary);
    /// <summary>
    /// 打开警告消息的对话框。
    /// </summary>
    /// <param name="content">内容。</param>
    /// <param name="title">标题。</param>
    public static Task<IDialogReference> OpenWarning(this IDialogService dialogService, string? content = default, string? title = "警告")
    => dialogService.OpenMessage(content, title, IconName.InfoCircleFilled, Theme.Warning);
    /// <summary>
    /// 打开错误消息的对话框。
    /// </summary>
    /// <param name="content">内容。</param>
    /// <param name="title">标题。</param>
    public static Task<IDialogReference> OpenError(this IDialogService dialogService, string? content = default, string? title = "错误")
    => dialogService.OpenMessage(content, title, IconName.CloseCircleFilled, Theme.Danger);
    /// <summary>
    /// 打开成功消息的对话框。
    /// </summary>
    /// <param name="content">内容。</param>
    /// <param name="title">标题。</param>
    public static Task<IDialogReference> OpenSuccess(this IDialogService dialogService, string? content = default, string? title = "成功")
    => dialogService.OpenMessage(content, title, IconName.CheckCircleFilled, Theme.Success);

    /// <summary>
    /// 打开具备确认和取消操作的对话框。
    /// </summary>
    /// <param name="content">内容。</param>
    /// <param name="title">标题。</param>
    /// <param name="icon">显示的图标。</param>
    /// <param name="iconTheme">图标主题颜色。</param>
    public static Task<IDialogReference> OpenConfirm(this IDialogService service, string? content, string? title = "确定吗", IconName? icon = IconName.HelpCircleFilled, Theme? iconTheme = default)
        => service.Open<ConfirmationDialogTemplate>(content, title, icon, iconTheme ?? Theme.Primary);
}
