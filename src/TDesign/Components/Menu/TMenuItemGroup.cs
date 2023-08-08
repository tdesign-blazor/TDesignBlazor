using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace TDesign;

/// <summary>
/// 表示菜单项的分组。只能适配侧边菜单。
/// </summary>
[ChildComponent(typeof(TMenu))]
[CssClass("t-menu-group")]
public class TMenuItemGroup : TDesignAdditionParameterComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [ParameterApiDoc("当前分组的菜单项容器")]
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 分组标题。
    /// </summary>
    [ParameterApiDoc("分组标题")]
    [EditorRequired][Parameter][NotNull] public string Title { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", Title, new { @class = "t-menu-group__title" });
        builder.AddContent(sequence + 1, ChildContent);
    }
}
