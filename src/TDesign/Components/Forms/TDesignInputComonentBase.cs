using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 输入控件的基类。
/// </summary>
/// <typeparam name="TValue">双向绑定值的类型。</typeparam>
public abstract class TDesignInputComonentBase<TValue> : BlazorInputComponentBase<TValue>
{
    /// <summary>
    /// 设置只读模式。
    /// </summary>
    [Parameter] public bool Readonly { get; set; }
    /// <summary>
    /// 设置禁用状态。
    /// </summary>
    [Parameter] public bool Disabled { get; set; }
    /// <summary>
    /// 自适应宽度。
    /// </summary>
    [Parameter] public bool AutoWidth { get; set; }
    /// <summary>
    /// 尺寸。
    /// </summary>
    [Parameter] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 自动聚焦。
    /// </summary>
    [Parameter] public bool AutoFocus { get; set; }
    /// <summary>
    /// 状态。
    /// </summary>
    [Parameter][CssClass("t-is-")] public Status Status { get; set; } = Status.Default;
    /// <summary>
    /// 输入框提示的内容。
    /// </summary>
    [Parameter] public RenderFragment? TipContent { get; set; }
    /// <summary>
    /// 对齐方式。
    /// </summary>
    [Parameter] public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Left;

    protected virtual void BuildInputWrapper(RenderTreeBuilder builder, int sequence, RenderFragment content, string? otherCss = default)
    {
        builder.CreateElement(sequence, "div", wrap =>
        {
            wrap.CreateElement(0, "div", content, new
            {
                @class = HtmlHelper.CreateCssBuilder().Append("t-input")
                            .Append("t-is-readonly", Readonly)
                            .Append("t-is-disabled", Disabled)
                            .Append("t-input--auto-width", AutoWidth)
                            .Append("t-is-focused t-input--focused", AutoFocus)
                            .Append($"t-is-{Status.GetCssClass()}")
                            .Append($"t-align-{Alignment.GetCssClass()}")
                            .Append(Size.GetCssClass())
                            .Append(otherCss, !string.IsNullOrEmpty(otherCss))
            });

            wrap.CreateElement(1, "div", TipContent, new
            {
                @class = HtmlHelper.CreateCssBuilder().Append("t-input__tips")
                .Append($"t-input__tips--{Status.GetCssClass()}")
            }, TipContent is not null);
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-input__wrap")
        });
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        base.BuildAttributes(attributes);
        base.AddValueChangedAttribute(attributes);
        attributes["disabled"] = Disabled;
        attributes["readonly"] = Readonly;
        attributes["autofocus"] = AutoFocus;
    }
}
