using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 开关组件
/// </summary>
[CssClass("t-switch")]
public class TSwitch : BlazorInputComponentBase<bool>, IHasDisabled
{
    /// <summary>
    /// 是否禁用
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// 是否加载中
    /// </summary>
    [Parameter] public bool Loading { get; set; }

    /// <summary>
    /// 尺寸。
    /// </summary>
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;

    /// <summary>
    ///  执行当 <see cref="TSwitch"/> 触发的事件。
    /// </summary>
    [Parameter] public EventCallback<MouseEventArgs?> OnChange { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {

        builder.CreateElement(++sequence, "span",
            span =>
            {
                span.CreateComponent<TIcon>(++sequence, attributes: new { Name = IconName.Loading }, condition: Loading);
            },
            attributes: new
            {
                @class = HtmlHelper.CreateCssBuilder().Append("t-switch__handle").Append("t-is-loading", Loading)
            });
        builder.CreateElement(++sequence, "div", attributes: new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-switch__content").Append(Size.GetCssClass())
        });

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attributes"></param>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["onclick"] = HtmlHelper.CreateCallback(this, () =>
        {
            CurrentValue = !Value;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if (builder.Contains("t-is-checked") && !CurrentValue)
        {
            builder.Remove("t-is-checked");
        }
        else
        {
            builder.Append("t-is-checked", CurrentValue);
        }
    }
}
