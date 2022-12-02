using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 表示菜单项的分组。只能适配侧边菜单。
/// </summary>
[ChildComponent(typeof(TMenu))]
[CssClass("t-menu-group")]
public class TMenuItemGroup : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 分组标题。
    /// </summary>
    [EditorRequired][Parameter][NotNull] public string Title { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", Title, new { @class = "t-menu-group__title" });
        builder.AddContent(sequence + 1, ChildContent);
    }
}
