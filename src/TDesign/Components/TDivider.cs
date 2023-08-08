using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;


/// <summary>
/// 分割线。
/// </summary>
[CssClass("t-divider")]
public class TDivider : TDesignAdditionParameterComponentBase, IHasChildContent
{
    /// <summary>
    /// 分割线的文本。
    /// </summary>
    [ParameterApiDoc("分割线的任意内容")]
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 是否垂直分割线。
    /// </summary>
    [ParameterApiDoc("是否为垂直分割线")]
    [Parameter][CssClass("t-divider--vertical")] public bool Vertical { get; set; }

    /// <summary>
    /// 是否为虚线。
    /// </summary>
    [ParameterApiDoc("是否为虚线")]
    [Parameter][CssClass("t-divider--dashed")] public bool Dashed { get; set; }

    /// <summary>
    /// 文字对齐方式。
    /// </summary>
    [ParameterApiDoc("文字对齐方式", Value = "Center")]
    [Parameter][CssClass("t-divider--with-text-")] public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        if (ChildContent is not null)
        {
            builder.CreateElement(sequence, "span", ChildContent, new { @class = "t-divider__inner-text" });
        }
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-divider--with-text", ChildContent is not null);
    }
}
