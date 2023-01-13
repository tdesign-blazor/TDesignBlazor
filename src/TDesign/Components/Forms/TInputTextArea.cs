using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 用于承载用户多行信息录入的组件。
/// </summary>
[HtmlTag("textarea")]
[CssClass("t-textarea__inner")]
public class TInputTextArea : TDesignInputComonentBase<string>
{
    /// <summary>
    /// 设置只读模式。
    /// </summary>
    [Parameter] public bool Readonly { get; set; }
    /// <summary>
    /// 设置禁用状态。
    /// </summary>
    [Parameter][CssClass("t-is-disabled")] public bool Disabled { get; set; }
    /// <summary>
    /// 状态。
    /// </summary>
    [Parameter][CssClass("t-is-")] public Status Status { get; set; } = Status.Default;
    /// <summary>
    /// 设置是否自动聚焦。
    /// </summary>
    [Parameter] public bool AutoFocus { get; set; }
    /// <summary>
    /// 文本域的最大行数。
    /// </summary>
    [Parameter] public int? Rows { get; set; }
    /// <summary>
    /// 禁用重新绘制尺寸的功能。
    /// </summary>
    [Parameter][CssClass("t-resize-none")] public bool DisableResize { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateElement(0, "div", content =>
        {
            base.BuildRenderTree(content);
        }, new
        {
            @class = HtmlHelper.Class.Append("t-textarea")
                                                .Append("t-is-disabled", Disabled)
                                                .Append("t-is-readonly", Readonly)
        });
    }


    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"min-height:{Rows * 24}px", Rows.HasValue);
    }
}
