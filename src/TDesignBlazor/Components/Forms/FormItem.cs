using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor.Components;
[CssClass("t-form__item")]
[ChildComponent(typeof(Form))]
public class FormItem : BlazorComponentBase, IHasChildContent
{
    [CascadingParameter] public Form CascadingForm { get; set; }
    [CascadingParameter] EditContext CascadingEditContext { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 设置表单项显示的标签文本。
    /// </summary>
    [Parameter] public string? Label { get; set; }
    /// <summary>
    /// 设置表单项文本的宽度，默认是 60px 。
    /// </summary>
    [Parameter] public string? LabelWidth { get; set; } = "60px";

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildLabel(builder, sequence);
        BuildControl(builder, sequence + 1);
    }

    void BuildLabel(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "label", Label, new { @for = "" });
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-form__label")
                                .Append($"t-form__label--{CascadingForm.Alignment.GetCssClass()}"),
            style = HtmlHelper.CreateStyleBuilder().Append($"width:{LabelWidth}", !string.IsNullOrEmpty(LabelWidth) && CascadingForm.Alignment != FormAlignment.Top)
        });
    }

    void BuildControl(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "div", ChildContent, new { @class = "t-form__controls-content" });
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-form__controls"),
            style = HtmlHelper.CreateStyleBuilder().Append("margin-left:60px", CascadingForm.Alignment != FormAlignment.Top)
        });
    }
}
