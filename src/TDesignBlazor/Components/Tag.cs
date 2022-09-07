using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor.Components;

/// <summary>
/// 标签。
/// </summary>
[CssClass("t-tag")]
[HtmlTag("span")]
public class Tag : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 主题颜色。
    /// </summary>
    [Parameter] public Theme? Theme { get; set; }

    /// <summary>
    /// 标签的类型。
    /// </summary>
    [Parameter][CssClass("t-tag--")] public TagType Type { get; set; } = TagType.Dark;
    /// <summary>
    /// 尺寸。
    /// </summary>
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 形状。
    /// </summary>
    [Parameter][CssClass("t-tag--")] public TagShape? Shape { get; set; }
    /// <summary>
    /// 图标的名称。
    /// </summary>
    [Parameter] public object? Icon { get; set; }
    /// <summary>
    /// 是否可以被关闭。
    /// </summary>
    [Parameter][CssClass("t-tag--close")] public bool Closable { get; set; }
    /// <summary>
    /// 设置组件处于禁用状态。
    /// </summary>
    [Parameter][CssClass("t-is-disabled t-tag--disabled")] public bool Disabled { get; set; }
    /// <summary>
    /// 设置一个函数，当关闭发生时触发。
    /// </summary>
    [Parameter] public EventCallback<bool> OnClosing { get; set; }
    /// <summary>
    /// 设置标签作为复选框形式呈现。
    /// </summary>
    [Parameter][CssClass("t-tag--check")] public bool Checkbox { get; set; }
    /// <summary>
    /// 获取或设置一个布尔值，表示是否被选中。<see cref="Checkbox"/> 为 <c>true</c> 时有效。
    /// </summary>
    [Parameter] public bool Checked { get; set; }
    /// <summary>
    /// 获取或设置一个选择变更时的函数。
    /// </summary>
    [Parameter] public EventCallback<bool> CheckedChanged { get; set; }
    /// <summary>
    /// 获取或设置一个选择变更的表达式。
    /// </summary>
    [Parameter] public Expression<Func<bool>>? CheckedExpression { get; set; }
    /// <summary>
    /// 设置 <see cref="Checked"/> 是 <c>true</c> 时显示的内容。
    /// </summary>
    [Parameter] public RenderFragment? CheckedContent { get; set; }

    /// <summary>
    /// 标签固定的宽度，超长省略，单位px。
    /// </summary>
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
            builder.CreateComponent<Icon>(sequence, attributes: new { Name = Icon }, condition: Icon is not null);

            if (Width.HasValue)
            {
                builder.CreateElement(sequence + 1, "span", ChildContent, new { @class = "t-tag--text", style = $"max-width:{Width}px" });
            }
            else
            {
                builder.AddContent(sequence + 1, ChildContent);
            }

            builder.CreateComponent<Icon>(sequence, attributes: new
            {
                Name = IconName.Close,
                AdditionalCssCLass = "t-tag__icon-close",
                onclick = HtmlHelper.CreateCallback(this, () =>
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

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append($"t-tag--{Theme?.Value}", Theme is not null)
            .Append("t-tag--default", Theme is null)
            .Append("t-tag--checked", Checked)
            .Append("t-tag--ellipsis", Width.HasValue)
            ;
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["onclick"] = HtmlHelper.CreateCallback<MouseEventArgs>(this, async _ =>
        {
            Checked = !Checked;
            await CheckedChanged.InvokeAsync(Checked);
            await this.Refresh();
        }, Checkbox && !Disabled);
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