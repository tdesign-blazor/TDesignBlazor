using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace TDesign;

/// <summary>
/// 标签。
/// </summary>
[CssClass("t-tag")]
[HtmlTag("span")]
public class TTag : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// 主题颜色。
    /// </summary>
    [ParameterApiDoc("主题颜色")]
    [Parameter] public Theme? Theme { get; set; }

    /// <summary>
    /// 标签的类型。
    /// </summary>
    [ParameterApiDoc("标签的类型", Value = "Dark")]
    [Parameter][CssClass("t-tag--")] public TagType Type { get; set; } = TagType.Dark;
    /// <summary>
    /// 尺寸。
    /// </summary>
    [ParameterApiDoc("尺寸", Value = "Medium")]
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 形状。
    /// </summary>
    [ParameterApiDoc("形状")]
    [Parameter][CssClass("t-tag--")] public TagShape? Shape { get; set; }
    /// <summary>
    /// 图标的名称。
    /// </summary>
    [ParameterApiDoc("图标的名称")]
    [Parameter] public object? Icon { get; set; }
    /// <summary>
    /// 是否可以被关闭。
    /// </summary>
    [ParameterApiDoc("是否可以被关闭")]
    [Parameter][CssClass("t-tag--close")] public bool Closable { get; set; }
    /// <summary>
    /// 禁用状态。
    /// </summary>
    [ParameterApiDoc("禁用状态")]
    [Parameter][CssClass("t-is-disabled t-tag--disabled")] public bool Disabled { get; set; }
    /// <summary>
    /// 设置一个函数，当关闭发生时触发。
    /// </summary>
    [ParameterApiDoc("设置一个函数，当关闭发生时触发", Type= "EventCallback<bool>")]
    [Parameter] public EventCallback<bool> OnClosing { get; set; }
    /// <summary>
    /// 设置标签作为复选框形式呈现。
    /// </summary>
    [ParameterApiDoc("标签作为复选框形式呈现")]
    [Parameter][CssClass("t-tag--check")] public bool Checkbox { get; set; }
    /// <summary>
    /// 获取或设置一个布尔值，表示是否被选中。<see cref="Checkbox"/> 为 <c>true</c> 时有效。
    /// </summary>
    [ParameterApiDoc("是否被选中，Checkbox 为 true 时有效")]
    [Parameter] public bool Checked { get; set; }
    /// <summary>
    /// 获取或设置一个选择变更时的函数。
    /// </summary>
    [ParameterApiDoc("设置一个选择变更时的函数", Type = "EventCallback<bool>")]
    [Parameter] public EventCallback<bool> CheckedChanged { get; set; }
    /// <summary>
    /// 获取或设置一个选择变更的表达式。
    /// </summary>
    [ParameterApiDoc("一个选择变更的表达式", Type = "Expression<Func<bool>>")]
    [Parameter] public Expression<Func<bool>>? CheckedExpression { get; set; }
    /// <summary>
    /// 设置 <see cref="Checked"/> 是 <c>true</c> 时显示的内容。
    /// </summary>
    [ParameterApiDoc("Checked 是 <c>true</c> 时显示的内容")]
    [Parameter] public RenderFragment? CheckedContent { get; set; }

    /// <summary>
    /// 标签固定的宽度，超长省略，单位px。
    /// </summary>
    [ParameterApiDoc("签固定的宽度，超长省略，单位px")]
    [Parameter] public int? Width { get; set; }
    bool IsClosed { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (IsClosed)
        {
            return;
        }
        base.BuildRenderTree(builder);
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        if (Checkbox && Checked)
        {
            builder.AddContent(0, CheckedContent);
        }
        else
        {
            builder.CreateComponent<TIcon>(sequence, attributes: new { Name = Icon }, condition: Icon is not null);

            if (Width.HasValue)
            {
                builder.CreateElement(sequence + 1, "span", ChildContent, new { @class = "t-tag--text", style = $"max-width:{Width}px" });
            }
            else
            {
                builder.AddContent(sequence + 1, ChildContent);
            }

            builder.CreateComponent<TIcon>(sequence, attributes: new
            {
                Name = IconName.Close,
                AdditionalCLass = "t-tag__icon-close",
                onclick = HtmlHelper.Instance.Callback().Create(this, () =>
                {
                    if (Disabled)
                    {
                        return;
                    }
                    IsClosed = true;
                    OnClosing.InvokeAsync(IsClosed);
                    this.Refresh();
                })
            }, condition: Closable);
        }
    }

    /// <inheritdoc/>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append($"t-tag--{Theme?.Value}", Theme is not null)
            .Append("t-tag--default", Theme is null)
            .Append("t-tag--checked", Checked)
            .Append("t-tag--ellipsis", Width.HasValue)
            ;
    }

    /// <inheritdoc/>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["onclick"] = HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, async _ =>
        {
            if (Checkbox && !Disabled)
            {
                Checked = !Checked;
                await CheckedChanged.InvokeAsync(Checked);
                await this.Refresh();
            }
        });
    }
}

/// <summary>
/// 标签类型。
/// </summary>
public enum TagType
{
    Light,
    Dark,
    Outline
}

/// <summary>
/// 标签形状。
/// </summary>
public enum TagShape
{
    /// <summary>
    /// 椭圆。
    /// </summary>
    Round,
    /// <summary>
    /// 半椭圆。
    /// </summary>
    Mark
}