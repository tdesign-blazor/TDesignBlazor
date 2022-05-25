using ComponentBuilder.Abstrations;

using Microsoft.AspNetCore.Components.Rendering;

namespace AnyDesignBlazor.Components.TDesign;


/// <summary>
/// 分割线。
/// </summary>
[CssClass("t-divider")]
public class Divider : BlazorComponentBase, IHasChildContent
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter][CssClass("t-divider--")] public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// 是否为虚线。
    /// </summary>
    [Parameter][CssClass("t-divider-dashed")] public bool Dashed { get; set; }

    [Parameter][CssClass("t-divider--with-text-")] public HorizontalAlignment Alignment { get; set; }

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
