using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;

/// <summary>
/// 消息通知。
/// </summary>
[ParentComponent]
[CssClass("t-notification")]
public class Notification : MessageComponentBase, IHasChildContent
{

    /// <summary>
    /// 显示的标题。
    /// </summary>
    [Parameter] public string? Title { get; set; }
    /// <summary>
    /// 具备标题的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? TitleContent { get; set; }

    /// <summary>
    /// 具备操作部分的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? OperationContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", icon =>
        {
            icon.CreateComponent<Icon>(0, attributes: new { Name = Icon, AdditionalCssClass = $"t-is-{GetThemeClass}" });
        }, new { @class = "t-notification__icon" }, Icon is not null);

        builder.CreateElement(sequence + 1, "div",
            content =>
            {
                content.CreateElement(0, "div", title =>
                {
                    title.CreateElement(0, "div", TitleContent, new { @class = "t-notification__title" }, TitleContent is not null);
                    title.CreateElement(0, "div", Title!, new { @class = "t-notification__title" }, Title is not null);
                }
                , new { @class = "t-notification__title__wrap" });

                content.CreateElement(0, "div", ChildContent, new { @class = "t-notification__content" });
                content.CreateElement(1, "div", OperationContent, new { @class = "t-notification__detail" }, condition: OperationContent is not null);
            }, new { @class = "t-notification__main" });
    }
}

/// <summary>
/// 仅在 <see cref="Notification"/> 组件中的 <c>OperationContent</c> 参数中使用。
/// </summary>
[ChildComponent(typeof(Notification))]
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