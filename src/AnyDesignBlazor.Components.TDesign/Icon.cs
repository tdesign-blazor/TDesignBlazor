using Microsoft.AspNetCore.Components.Rendering;

namespace AnyDesignBlazor.Components.TDesign;


/// <summary>
/// 图标组件。
/// </summary>
[HtmlTag("svg")]
[CssClass("t-icon")]
public class Icon : BlazorChildContentComponentBase
{
    /// <summary>
    /// 图标名称。参见 https://tdesign.tencent.com/vue/components/icon 。
    /// </summary>
    [Parameter][CssClass("t-icon-")] public string Name { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "use", attributes: new { href = $"#t-icon-{Name}" });
    }
}
