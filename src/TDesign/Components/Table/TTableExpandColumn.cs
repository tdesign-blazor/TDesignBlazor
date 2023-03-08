namespace TDesign;

/// <summary>
/// 定义可展开详情行的列。
/// </summary>
[CssClass("t-table__expandable-icon-cell")]
public class TTableExpandColumn<TItem> : TTableFieldColumnBase<TItem>, IHasChildContent
{
    /// <summary>
    /// 设置要展开的图标。
    /// </summary>
    [Parameter] public object? Icon { get; set; } = IconName.ChevronRightCircle;
    /// <summary>
    /// 设置展开的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }


    /// <inheritdoc/>
    internal override RenderFragment? GetColumnContent(params object[] args)
    {
        if ( args is null )
        {
            throw new ArgumentNullException(nameof(args));
        }

        var rowData = GetRowData(args);
        var value = GetValue(rowData);


        var rowIndex = (int)args[1];

        var nextRowIndex = rowIndex + 1;

        var isExpand = CascadingGenericTable.TryGetRowData(nextRowIndex, out var result) && result.type == TableRowDataType.Expand;


        return  builder => builder.Fluent().Span("t-table__expand-box")
                                            .Class($"t-table__row--{(isExpand ? "expanded" : "collapsed")}")
                                            .Class("t-positive-rotate-90", isExpand)
                                            .Callback<MouseEventArgs>("onclick", this, e =>
                                            {
                                                var index = FindRowIndex(value);
                                                CascadingGenericTable.ExpandRow(index);
                                            })
                                            .Content(content => content.CreateComponent<TIcon>(0, attributes: new
                                            {
                                                Name = Icon
                                            }))
                                        .Close();
    }

    /// <inheritdoc/>
    internal override RenderFragment? GetHeaderContent() => builder => builder.AddContent(0, "展开/收起");

    /// <summary>
    /// 获取展开行的 UI 内容。
    /// </summary>
    internal RenderFragment? GetExpandedRow()
    => new(builder => builder.Fluent().Element("td")
                                    .Attribute("colspan", CascadingTable.ChildComponents.Count)
                                    .Content(content => content.Fluent().Div("t-table__expanded-row-inner")
                                                            .Content(div => div.Fluent().Div("t-table__row-full-element")
                                                            .Content(ChildContent).Close())
                                                        .Close())
                                  .Close());
}

