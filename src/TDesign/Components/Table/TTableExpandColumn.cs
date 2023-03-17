namespace TDesign;

/// <summary>
/// 定义可展开详情行的列。
/// </summary>
/// <typeparam name="TItem">数据的类型。</typeparam>
[CssClass("t-table__expandable-icon-cell")]
public class TTableExpandColumn<TItem> : TTableColumnBase<TItem>, IHasChildContent<TItem>
{
    /// <summary>
    /// 设置要展开的图标。
    /// </summary>
    [Parameter] public object? Icon { get; set; } = IconName.ChevronRightCircle;
    /// <summary>
    /// 设置展开的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment<TItem>? ChildContent { get; set; }


    /// <inheritdoc/>
    protected internal override RenderFragment? GetCellContent(int rowIndex, TItem item)
    {
        var nextRowIndex = rowIndex + 1;

        var isExpand = Table.TryGetRowData(nextRowIndex, out var result) && result.type == TableRowDataType.Expand;


        return builder => builder.Span("t-table__expand-box")
                                            .Class($"t-table__row--{(isExpand ? "expanded" : "collapsed")}")
                                            .Class("t-positive-rotate-90", isExpand)
                                            .Callback<MouseEventArgs>("onclick", this, e =>
                                            {
                                                Table.ExpandRow(rowIndex);
                                            })
                                            .Content(content => content.CreateComponent<TIcon>(0, attributes: new
                                            {
                                                Name = Icon
                                            }))
                                        .Close();
    }

    /// <inheritdoc/>
    protected internal override RenderFragment? GetHeaderContent() => builder => builder.AddContent(0, "展开/收起");

    /// <summary>
    /// 获取展开行的 UI 内容。
    /// </summary>
    internal RenderFragment? GetExpandedRow(TItem item)
    => new(builder => builder.Element("td")
                                    .Attribute("colspan", Table.ChildComponents.Count)
                                    .Content(content => content.Div("t-table__expanded-row-inner")
                                                            .Content(div => div.Div("t-table__row-full-element")
                                                            .Content(ChildContent?.Invoke(item)).Close())
                                                        .Close())
                                  .Close());
}

