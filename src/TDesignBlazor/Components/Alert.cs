using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor.Components;

/// <summary>
/// 警告提醒。
/// </summary>
[CssClass("t-alert")]
public class Alert : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 具备操作部分的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? OperationContent { get; set; }
    /// <summary>
    /// 具备标题的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? TitleContent { get; set; }
    /// <summary>
    /// 提醒主题。
    /// </summary>
    [Parameter] public Theme? Theme { get; set; }
    /// <summary>
    /// 图标。
    /// </summary>
    [Parameter] public object? Icon { get; set; }
    /// <summary>
    /// 是否可以关闭。
    /// </summary>
    [Parameter] public bool Closable { get; set; }

    bool Closed { get; set; }

    public string GetTheme => Theme switch
    {
        TDesignBlazor.Theme.Danger => "error",
        TDesignBlazor.Theme.Primary => "info",
        not null => Theme.GetCssClass(),
        null => ""
    };

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        Icon ??= Theme switch
        {
            TDesignBlazor.Theme.Success => IconName.CheckCircleFilled,
            TDesignBlazor.Theme.Danger => IconName.CloseCircleFilled,
            _ => IconName.InfoCircleFilled,
        };
    }

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
            icon.CreateComponent<Icon>(0, attributes: new { Name = Icon });
        }, new { @class = "t-alert__icon" });

        builder.CreateElement(sequence + 1, "div",
            content =>
            {
                content.CreateElement(0, "div", TitleContent, new { @class = "t-alert__title" }, TitleContent is not null);
                content.CreateElement(1, "div", message =>
                {
                    message.CreateElement(0, "div", ChildContent, new { @class = "t-alert__description" });
                    message.CreateElement(1, "div", OperationContent, new { @class = "t-alert__operation" }, OperationContent is not null);
                }, new { @class = "t-alert__message" });
            }, new { @class = "t-alert__content" });

        builder.CreateElement(sequence + 2, "div", icon => icon.CreateComponent<Icon>(0, attributes: new { Name = IconName.Close }), new
        {
            @class = "t-alert__close",
            onclick = HtmlHelper.CreateCallback(this, () => { Closed = true; StateHasChanged(); }, Closable)
        }, Closable);
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append($"t-alert--{GetTheme}");
    }
}
