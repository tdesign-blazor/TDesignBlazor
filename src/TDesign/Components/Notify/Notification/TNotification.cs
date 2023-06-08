using Microsoft.AspNetCore.Components.Rendering;

using TDesign.Notification;

namespace TDesign;

/// <summary>
/// 轻量级的全局消息提示和确认机制。
/// </summary>
[ParentComponent]
[CssClass("t-notification")]
public class TNotification : NotifyComponentBase
{
    /// <summary>
    /// 显示的标题。
    /// </summary>
    [Parameter][EditorRequired] public string? Title { get; set; }
    /// <summary>
    /// 显示的副标题。
    /// </summary>
    [Parameter] public string? SubTitle { get; set; }
    /// <summary>
    /// 具备操作部分的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? OperationContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", icon =>
        {
            icon.CreateComponent<TIcon>(0, attributes: new { Name = Icon, AdditionalClass = $"t-is-{GetThemeClass}" });
        }, new { @class = "t-notification__icon" }, Icon is not null);

        builder.CreateElement(sequence + 1, "div",
            content =>
            {
                content.CreateElement(0, "div", title =>
                {
                    title.CreateElement(0, "div", tc =>
                    {
                        tc.AddContent(0, Title);

                        tc.CreateElement(1, "small", sub =>
                        {
                            sub.AddContent(0, new MarkupString("&nbsp;"));
                            sub.AddContent(1, SubTitle);
                        }, condition: !string.IsNullOrEmpty(SubTitle));
                    }, new { @class = "t-notification__title" });
                }
                , new { @class = "t-notification__title__wrap" });

                content.CreateElement(0, "div", ChildContent, new { @class = "t-notification__content" });
                content.CreateElement(1, "div", OperationContent, new { @class = "t-notification__detail" }, condition: OperationContent is not null);
            }, new { @class = "t-notification__main" });
    }
}

/// <summary>
/// 用于对通知时的操作排版项。仅在 <see cref="TNotification"/> 组件中的 <see cref="TNotification.OperationContent"/> 参数中使用。
/// </summary>
[ChildComponent(typeof(TNotification))]
[CssClass("t-notification__detail-item")]
public class NotificationDetailItem : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append("display:inline-block");
    }
}