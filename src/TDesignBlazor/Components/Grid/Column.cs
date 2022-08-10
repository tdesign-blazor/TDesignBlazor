using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;

/// <summary>
/// 表示栅格的列。
/// </summary>
[ChildComponent(typeof(Row))]
[CssClass("t-col")]
public class Column : BlazorChildContentComponentBase, IHasCascadingParameter<Row>
{
    /// <summary>
    /// 获取或设置父组件 <see cref="Row"/> 的级联参数的值。
    /// </summary>
    [CascadingParameter] public Row? CascadingValue { get; set; }
    /// <summary>
    /// 获取或设置栅格的宽度。
    /// </summary>
    [Parameter][CssClass("t-col-")] public ColumnSize Span { get; set; } = ColumnSize.Is1;
    /// <summary>
    /// 获取或设置栅格的偏移量。
    /// </summary>
    [Parameter][CssClass("t-col-offset-")] public ColumnSize? Offset { get; set; }
    /// <summary>
    /// 获取或设置向左边拉动的单元格宽度。
    /// </summary>
    [Parameter][CssClass("t-col-pull-")] public ColumnSize? Pull { get; set; }
    /// <summary>
    /// 获取或设置向右边推动的单元格宽度。
    /// </summary>
    [Parameter][CssClass("t-col-push-")] public ColumnSize? Push { get; set; }
    /// <summary>
    /// 获取或设置单元格的排序。
    /// </summary>
    [Parameter][CssClass("t-col-order-")] public ColumnSize? Order { get; set; }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", ChildContent);
    }
}
