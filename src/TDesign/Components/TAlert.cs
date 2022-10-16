using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 警告提醒。
/// </summary>
[CssClass("t-alert")]
public class TAlert : MessageComponentBase
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
    /// <summary>
    /// 是否可以关闭。
    /// </summary>
    [Parameter] public bool Closable { get; set; }

    bool Closed { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Closed)
        {
            return;
        }
        base.BuildRenderTree(builder);
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", icon =>
        {
            icon.CreateComponent<TIcon>(0, attributes: new { Name = TIcon });
        }, new { @class = "t-alert__icon" }, TIcon is not null);

        builder.CreateElement(sequence + 1, "div",
            content =>
            {

                content.CreateElement(0, "div", TitleContent, new { @class = "t-alert__title" }, TitleContent is not null);

                content.CreateElement(0, "div", Title!, new { @class = "t-alert__title" }, Title is not null);


                content.CreateElement(1, "div", message =>
                {
                    message.CreateElement(0, "div", ChildContent, new { @class = "t-alert__description" });
                    message.CreateElement(1, "div", OperationContent, new { @class = "t-alert__operation" }, OperationContent is not null);
                }, new { @class = "t-alert__message" });
            }, new { @class = "t-alert__content" });

        builder.CreateElement(sequence + 2, "div", icon => icon.CreateComponent<TIcon>(0, attributes: new { Name = IconName.Close }), new
        {
            @class = "t-alert__close",
            onclick = HtmlHelper.CreateCallback(this, () => { Closed = true; StateHasChanged(); }, Closable)
        }, Closable);
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append($"t-alert--{GetThemeClass}");
    }
}
