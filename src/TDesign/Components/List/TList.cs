using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 表示一个列表容器。配合 <see cref="TListItem"/> 组件使用。
/// </summary>
[ParentComponent]
[CssClass("t-list")]
public class TList : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 列表的尺寸。默认是 <see cref="Size.Medium"/> 。
    /// </summary>
    [Parameter] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 设置一个布尔值，表示列表项之间是否有分割线。
    /// </summary>
    [Parameter][CssClass("t-list--split")] public bool Split { get; set; }
    /// <summary>
    /// 设置一个布尔值，表示列表项之间是否有渐变色。
    /// </summary>
    [Parameter][CssClass("t-list--stripe")] public bool Stripe { get; set; }
    /// <summary>
    /// 设置在列表中顶部的内容。
    /// </summary>
    [Parameter] public RenderFragment? HeaderContent { get; set; }
    /// <summary>
    /// 设置在列表中底部的内容。
    /// </summary>
    [Parameter] public RenderFragment? FooterContent { get; set; }
    /// <summary>
    /// 设置列表加载状态时的自定义内容。
    /// </summary>
    [Parameter] public RenderFragment? LoadingContent { get; set; }
    /// <summary>
    /// 列表的最大高度。单位 px，超过高度则自动出现滚动条。
    /// </summary>
    [Parameter] public int? Height { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", HeaderContent, new { @class = "t-list__header" }, HeaderContent is not null);

        builder.CreateElement(sequence + 1, "div", ChildContent, new { @class = "t-list__inner" }, ChildContent is not null);

        builder.CreateElement(sequence + 2, "div", FooterContent, new { @class = "t-list__footer" }, FooterContent is not null);


        builder.CreateElement(sequence + 3, "div", LoadingContent, new { @class = "t-list__load" }, LoadingContent is not null);
    }
    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"max-height:{Height}px", Height.HasValue);
    }
}
