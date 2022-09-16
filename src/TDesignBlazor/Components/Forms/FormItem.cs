using System.Linq.Expressions;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor.Components;
[CssClass("t-form__item")]
[ChildComponent(typeof(Form))]
public class FormItem : BlazorComponentBase, IHasChildContent
{
    [CascadingParameter] public Form CascadingForm { get; set; }
    [CascadingParameter] EditContext CascadingEditContext { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 设置表单项显示的标签文本。
    /// </summary>
    [Parameter] public string? Label { get; set; }
    /// <summary>
    /// 设置表单项文本的宽度，默认是 60px 。
    /// </summary>
    [Parameter] public string? LabelWidth { get; set; } = "60px";
    /// <summary>
    /// 设置文本自动加上冒号。
    /// </summary>
    [Parameter] public bool LabelColon { get; set; }
    /// <summary>
    /// 设置必填时自动加上红色的 “*” 符号。
    /// </summary>
    [Parameter] public bool Required { get; set; }
    /// <summary>
    /// 帮助的提示文字。
    /// </summary>
    [Parameter] public string? HelpText { get; set; }
    /// <summary>
    /// 表单字段的状态。
    /// </summary>
    [Parameter] public Status? Status { get; set; }
    /// <summary>
    /// 表单字段对应的文本。
    /// </summary>
    [Parameter] public string? StatusText { get; set; }

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
                                .Append($"t-form__label--{CascadingForm.Alignment.GetCssClass()}")
                                .Append("t-form__label--colon", LabelColon)
                                .Append("t-form__label--required", Required)
                                ,
            style = HtmlHelper.CreateStyleBuilder().Append($"width:{LabelWidth}", !string.IsNullOrEmpty(LabelWidth) && CascadingForm.Alignment != FormAlignment.Top)
        });
    }

    void BuildControl(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "div", ChildContent, new { @class = "t-form__controls-content" });
            content.CreateElement(1, "div", tip => tip.AddContent(0, HelpText), new { @class = "t-input__help" }, !string.IsNullOrEmpty(HelpText));
            content.CreateElement(1, "div", extra => extra.AddContent(0, StatusText), new { @class = "t-input__extra" }, !string.IsNullOrEmpty(StatusText));
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-form__controls")
                                                .Append($"t-is-{Status?.GetCssClass()}", Status.HasValue)
                                                .Append($"t-form--success-border", Status == TDesignBlazor.Status.Success),
            style = HtmlHelper.CreateStyleBuilder().Append("margin-left:60px", CascadingForm.Alignment != FormAlignment.Top)
        });
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-form__item-with-help", !string.IsNullOrEmpty(HelpText))
            .Append("t-form__item-with-extra", !string.IsNullOrEmpty(StatusText))
            ;
    }
}
