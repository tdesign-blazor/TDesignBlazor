/* 用于定义表格中各种代码片段的文件 */

namespace TDesign;
partial class TTable<TItem>
{
    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateCascadingComponent(this, 0, base.BuildRenderTree,"Table", true);
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.AddContent(sequence, ChildContent);
        BuildTable(builder, sequence + 1);
        BuildPagination(builder, 10);
    }

    /// <summary>
    /// 构建表格。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence"></param>
    void BuildTable(RenderTreeBuilder builder,int sequence)
    {
        if ( Loading )
        {
            builder.CreateComponent<LoadingContainer>(sequence, container =>
            {
                BuildTableContent(container);
                container.CreateComponent<TLoading>(1, attributes: new { Overlay = true });
            });
        }
        else
        {
            BuildTableContent(builder);
        }
    }

    /// <summary>
    /// 构建表格。
    /// </summary>
    void BuildTableContent(RenderTreeBuilder builder)
    => builder.Div("t-table__content").Style($"max-height:{FixedHeight}px", FixedHeight.HasValue)
                .Content(content =>
                {
                    content.Element("table").Class($"t-table--layout-{(AutoWidth?"auto":"fixed")}")
                    .Content(table =>
                    {
                        table.AddContent(0, BuildTableHeader());
                        table.AddContent(1, BuildTableBody());
                        table.AddContent(2, BuildTableFooter());
                    })
                    .Close();

                })
        .Close();

    /// <summary>
    /// 构建 tbody 部分。
    /// </summary>
    RenderFragment? BuildTableBody() 
        => builder => builder.Element("tbody", "t-table__body")
                                .Content(content =>
                                {
                                    var rowIndex = 0;

                                    foreach ( var item in TableData )
                                    {
                                        content.OpenElement(0, "tr");
                                        content.SetKey(ItemKey);
                                        BuildTableRow(content, rowIndex, item);
                                        content.CloseElement();

                                        rowIndex++;
                                    }

                                    if ( DataLoadedComplete && !TableData.Any() )
                                    {
                                        content.AddContent(10, BuildEmptyContent());
                                    }
                                })
                            .Close();

    /// <summary>
    /// 构建空表格的内容。
    /// </summary>
    RenderFragment BuildEmptyContent()
        => builder => builder.Element("tr", "t-table__empty-row")
                                .Content(td => td.Element("td")
                                                    .Attribute("colspan", ChildComponents.Count)
                                                    .Content(content => content.Div("t-table__empty").Content(EmptyContent).Close())
                                                .Close())
                            .Close();

    /// <summary>
    /// 构建 thead 部分。
    /// </summary>
    RenderFragment? BuildTableHeader()
    {
      return builder=>  builder.Element("thead", "t-table__header")
                            .Class("t-table__header--fixed", FixedHeader)
                            .Content(content =>
                            {
                                content.Element("tr").Content(tr =>
                                {
                                    var index = 0;
                                    foreach ( var column in GetColumns() )
                                    {
                                        var headerContent = column.GetHeaderContent();
                                        tr.CreateElement(index, "th", headerContent, new { @class = column.HeaderClass }, key: column);
                                        index++;
                                    }
                                }).Close();
                            })
                        .Close();
    }

    #region tfoot 部分
    /// <summary>
    /// 构建底部，自定义 FooterContent 作为通栏表底，对列的 FooterContent 作为当列的表底。
    /// </summary>
    private RenderFragment? BuildTableFooter()
    {
        if ( !DataLoadedComplete )
        {
            return default;
        }

        if ( FooterContent is null && GetColumns().All(m => m.FooterContent is null) )
        {
            return default;
        }

       return builder=> builder.Element("tfoot", "t-table__footer")
                .Class("t-table__footer--fixed", FixedFooter)
                .Content(content =>
                {
                    content.Element("tr", "t-table__row--full", FooterContent is not null)
                                .Content(tr => tr.CreateElement(0, "td", inner =>
                                {
                                    inner
                                    .Div("t-table__row-full-element")
                                    .Content(div =>
                                    {
                                        div.Div("t-table__row-filter-inner")
                                            .Content(FooterContent)
                                           .Close();
                                    })
                                    .Close();
                                }, new { colspan = ChildComponents.Count }))
                            .Close()
                            .Element("tr", "t-tdesign__custom-footer-tr")
                                .Content(tr =>
                                {
                                    tr.ForEach("td", ChildComponents.Count, e =>
                                    {
                                        e.builder.Content(((TTableColumnBase<TItem>)ChildComponents[e.index]).FooterContent);
                                    });
                                })
                            .Close();
                })
                .Close();
    }

    #endregion

    /// <summary>
    /// 构建表格的行数据。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="rowIndex">行索引。</param>
    /// <param name="item">数据。</param>
    private void BuildTableRow(RenderTreeBuilder builder,int rowIndex, (TableRowDataType type, TItem data) item)
    {
        switch ( item.type )
        {
            case TableRowDataType.Expand:
                var expandColumn = GetColumns<TTableExpandColumn<TItem>>().FirstOrDefault();
                if ( expandColumn is null )
                {
                    break;
                }
                builder.AddAttribute(0, "class", "t-table__expanded-row");
                builder.AddContent(1, expandColumn.GetExpandedRow(item.data));
                break;
            case TableRowDataType.Data:
                builder.AddContent(0,row =>
                    {
                        foreach ( var column in GetColumns() )
                        {
                            row.CreateElement(0, "td", column.GetCellContent(rowIndex, item.data), new { @class = column.GetCssClassString() }, key: column);
                        }
                    })
                        ;
                break;
            default: //TODO 如何处理未知数据？不渲染还是渲染一个错误？
                break;
        }
    }

    /// <summary>
    /// 构建分页。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">源代码的位置。</param>
    void BuildPagination(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "div", pagination =>
            {
                builder.CreateComponent<TPagination>(sequence, attributes: new
                {
                    PageIndex,
                    PageIndexChanged = HtmlHelper.Event.Create<int>(this, value => QueryData(value, PageSize)),
                    PageSize,
                    PageSizeChanged = HtmlHelper.Event.Create<int>(this, value => QueryData(1, value)),
                    Total = TotalCount,
                    TotalChanged = HtmlHelper.Event.Create<int>(this, value => QueryData(1, PageSize)),
                    ShowJumpPage = true
                });
            }, new { @class = "t-table__pagination" });
        }, new { @class = "t-table__pagination-wrap", style = "opacity:1" }, condition: Pagination);
    }
}

