using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 允许用户通过单击在选中和未选中之间切换的多选框控件。
/// </summary>
[HtmlTag("input")]
[CssClass("t-checkbox__former")]
public class TInputCheckBox : BlazorInputComponentBase<bool?>, IHasChildContent, IHasDisabled
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 禁用状态。
    /// </summary>
    [Parameter] public bool Disabled { get; set; }
    /// <summary>
    /// 支持未确定状态。
    /// </summary>
    [Parameter] public bool Indeterminate { get; set; }


    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateElement(0, "label", content =>
        {
            base.BuildRenderTree(content);

            content.OpenRegion(10000);
            content.CreateElement(0, "span", attributes: new { @class = "t-checkbox__input" });
            content.CreateElement(1, "span", ChildContent, new { @class = "t-checkbox__label" });
            content.CloseRegion();
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-checkbox")
                                .Append("t-is-disabled", Disabled)
                                .Append("t-is-indeterminate", CurrentValue is null && Indeterminate)
                                .Append("t-is-checked", CurrentValue.HasValue && CurrentValue.Value)
        });
    }
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["type"] = "checkbox";
        base.BuildAttributes(attributes);
        base.AddValueChangedAttribute(attributes);
        if (Disabled)
        {
            attributes["disabled"] = true;
        }
    }
}
