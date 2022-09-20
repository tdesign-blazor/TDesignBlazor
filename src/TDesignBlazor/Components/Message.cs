using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;

/// <summary>
/// 全局提示。
/// </summary>
[CssClass("t-message")]
public class Message : MessageComponentBase
{
    /// <summary>
    /// 加载中的状态。
    /// </summary>
    [Parameter][CssClass("t-is-loading")] public bool Loading { get; set; }
    /// <summary>
    /// 可关闭的功能。
    /// </summary>
    [Parameter][CssClass("t-is-closable")] public bool Closable { get; set; }

    /// <summary>
    /// 获取一个布尔值，表示组件是否被关闭。
    /// </summary>
    public bool Closed { get; private set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Closed)
        {
            return;
        }
        base.BuildRenderTree(builder);
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append($"t-is-{GetThemeClass}", !string.IsNullOrEmpty(GetThemeClass));
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        if (Loading)
        {
            builder.CreateElement(sequence, "div", loading =>
            {
                loading.CreateComponent<Icon>(0, attributes: new { Name = IconName.Loading });
            }, new { @class = "t-loading--center t-size-m t-loading" });
        }
        else
        {
            builder.CreateComponent<Icon>(sequence, attributes: new { Name = GetIconByTheme }, condition: Icon is not null);
        }
        builder.AddContent(sequence + 1, ChildContent);

        builder.CreateElement(sequence + 2, "span", close =>
            {
                close.CreateComponent<Icon>(0, attributes: new { Name = IconName.Close });
            }, attributes: new
            {
                @class = "t-message__close",
                onclick = HtmlHelper.CreateCallback(this, () =>
                {
                    Closed = true;
                    StateHasChanged();
                })
            }, condition: Closable);
    }

    protected override IconName? GetIconByTheme
    {
        get
        {
            if (Theme == TDesignBlazor.MessageTheme.Question)
            {
                return IconName.HelpCircleFilled;
            }
            return base.GetIconByTheme;
        }
    }
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        Icon ??= GetIconByTheme;
    }
}
