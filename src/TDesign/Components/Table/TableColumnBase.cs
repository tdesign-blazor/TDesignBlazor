using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 表示表格的单元格的基类。
/// </summary>
public abstract class TableColumnBase : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// 获取当前是否是顶部单元格。
    /// </summary>
    [CascadingParameter(Name = "IsHeader")] protected bool IsHeader { get; set; }
    /// <summary>
    /// 设置列标题。若设置了 <see cref="TitleContent"/> 参数，则该参数无效。
    /// </summary>
    [Parameter] public string? Title { get; set; }
    /// <summary>
    /// 设置列标题部分的任意 UI 片段。
    /// </summary>
    [Parameter] public RenderFragment? TitleContent { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 获取顶部标题内容。
    /// </summary>
    /// <returns></returns>
    protected RenderFragment? GetHeaderContent() => TitleContent ??= b => b.AddContent(0, Title);

    /// <summary>
    /// 获取表格列的内容。
    /// </summary>
    /// <returns></returns>
    protected abstract RenderFragment? GetColumnContent();

    /// <inheritdoc/>
    public override string? GetTagName() => IsHeader ? "th" : "td";

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        if (IsHeader)
        {
            builder.AddContent(0, GetHeaderContent());
        }
        else
        {
            builder.AddContent(0, ChildContent ??= GetColumnContent());
        }
    }
}
