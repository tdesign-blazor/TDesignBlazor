using Microsoft.AspNetCore.Components.Rendering;
using TDesign.Notification;
using TDesign.Specifications;

namespace TDesign;

/// <summary>
/// 警告提醒。
/// </summary>
[CssClass("t-alert")]
public class TAlert : NotifyComponentBase,IHasTitleFragment,IHasTitleText
{
    /// <inheritdoc/>
    [Parameter]public string? TitleText { get; set; }
    /// <summary>
    /// 具备标题的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? TitleContent { get; set; }
    /// <summary>
    /// 具备操作部分的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? OperationContent { get; set; }

    /// <summary>
    /// 主题颜色，默认是 <see cref="Theme.Primary"/>。
    /// </summary>
    [Parameter]public override Theme? Theme { get; set; } = Theme.Primary;
    /// <summary>
    /// 显示的警告图标。
    /// </summary>
    [Parameter]public override object? Icon { get; set; } = IconName.InfoCircleFilled;
    /// <summary>
    /// 是否可以关闭。
    /// </summary>
    [Parameter] public bool Closable { get; set; }

    /// <summary>
    /// 当警告消息被关闭后触发的回调方法。
    /// </summary>
    [Parameter]public EventCallback<bool> OnClosed { get; set; }

    bool Closed { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if ( Closed )
        {
            return;
        }

        base.BuildRenderTree(builder);
    }


    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Div("t-alert__icon", Icon is not null)
                .Content(icon => icon.Component<TIcon>().Attribute(m => m.Name, Icon).Close())
                .Close();

        builder.Div("t-alert__content")
                .Content(content =>
                {
                    content.Div("t-alert__title", TitleContent is not null).Content(TitleContent).Close();

                    content.Div("t-alert__message")
                            .Content(message =>
                            {
                                message.Div("t-alert__description").Content(ChildContent).Close();
                                message.Div("t-alert__operation", OperationContent is not null).Content(OperationContent).Close();
                            })
                            .Close();
                })
                .Close();

        builder.Div("t-alert__close",Closable).Content(icon=>icon.Component<TIcon>().Attribute(m=>m.Name,IconName.Close).Callback("onclick",this,Close).Close()).Close();   
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append($"t-alert--{GetThemeClass}");
    }
    /// <summary>
    /// 关闭消息警告。
    /// </summary>
    public async Task Close()
    {
        Closed = true;
        await OnClosed.InvokeAsync(true);
        await this.Refresh();
    }
}
