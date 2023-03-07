/* 用于定义表格中各种代码片段的文件 */

using ComponentBuilder;

namespace TDesign;
partial class TTable<TItem>
{
    /// <summary>
    /// 构建表格。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">源代码的位置。</param>
    void BuildTable(RenderTreeBuilder builder, int sequence)
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
    /// 构造表格的 tbody 内容部分。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    void BuildTableContent(RenderTreeBuilder builder)
    {
        builder.Fluent()
            .Div().Class("t-table__content").Style($"max-height:{FixedHeight}px", FixedHeight.HasValue)
            .Content(content =>
            {
                content.Fluent().Element("table")
                                    .Class("t-table--layout-auto", AutoWidth)
                                    .Class("t-table--layout-fixed", !AutoWidth)
                                    .Content(table =>
                                    {
                                        BuildHeader(table, 0);
                                        BuildBody(table);
                                        BuildFooter(table);
                                    })
                                .Close();
            })
            .Close();


        /// <summary>
        /// 构建 theader 部分。
        /// </summary>
        /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
        /// <param name="sequence">源代码的位置。</param>
        void BuildHeader(RenderTreeBuilder builder, int sequence)
        {
            builder.Fluent().Element("thead", "t-table__header")
                                .Class("t-table__header--fixed", FixedHeader)
                                .Content(content =>
                                {
                                    content.CreateComponent<TTableRow>(0, tr =>
                                    {
                                        tr.AddContent(0, ChildContent!.Invoke(default));

                                        tr.OpenRegion(1);
                                        var index = 0;
                                        foreach ( var column in GetColumns() )
                                        {
                                            var key = index;
                                            var headerContent = column.GetHeaderContent();

                                            tr.CreateElement(index, "th", headerContent, new { @class = column.HeaderClass }, key: key);
                                            index++;
                                        }
                                        tr.CloseRegion();
                                    });
                                })
                            .Close();
        }
        /// <summary>
        /// 构建 tbody 部分。
        /// </summary>
        /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
        /// <param name="sequence">源代码的位置。</param>
        void BuildBody(RenderTreeBuilder builder)
        {
            builder.Fluent().Element("tbody", "t-table__body")
                    .Content(content =>
                    {
                        var rowIndex = 0;

                        foreach ( var (type, data) in TableData )
                        {
                            content.AddContent(0, ChildContent!.Invoke(data));

                            var rowKey = rowIndex;

                            content.OpenRegion(rowIndex);

                            content.OpenElement(1,"tr");
                            BuildRow(content,rowIndex, type, data);
                            content.SetKey(rowKey);
                            content.CloseElement();

                            content.CloseRegion();

                            rowIndex++;
                        }

                        if ( DataLoadedComplete && !TableData.Any() )
                        {
                            BuildEmptyContent(content);
                        }
                    })
                .Close();

            #region 空表格的内容
            void BuildEmptyContent(RenderTreeBuilder builder)
                => builder.Fluent().Element("tr", "t-table__empty-row")
                .Content(td => td.Fluent().Element("td")
                                        .Attribute("colspan", ChildComponents.Count)
                                        .Content(content => content.Fluent()
                                                                .Div("t-table__empty")
                                                                .Content(EmptyContent)
                                                                .Close())
                                        .Close())
                .Close();
            #endregion
        }
        #region tfoot 部分
        // 自定义 FooterContent 作为通栏表底，对列的 FooterContent 作为当列的表底
        void BuildFooter(RenderTreeBuilder builder)
        {
            if ( !DataLoadedComplete )
            {
                return;
            }

            using var footer = builder.Fluent();

            if ( FooterContent is null && GetColumns().All(m => m.FooterContent is null) )
            {
                return;
            }

            footer.Element("tfoot", "t-table__footer")
                    .Class("t-table__footer--fixed", FixedFooter)
                    .Content(content =>
                    {
                        content.Fluent()
                                .Element("tr", "t-table__row--full", FooterContent is not null)
                                    .Content(tr => tr.CreateElement(0, "td", inner =>
                                    {
                                        inner.Fluent()
                                        .Div("t-table__row-full-element")
                                        .Content(div =>
                                        {
                                            div.Fluent()
                                                        .Div("t-table__row-filter-inner")
                                                        .Content(FooterContent)
                                                        .Close();
                                        })
                                        .Close();
                                    }, new { colspan = ChildComponents.Count }))
                                .Close()
                                .Element("tr", "t-tdesign__custom-footer-tr")
                                    .Content(tr =>
                                    {
                                        tr.Fluent()
                                            .ForEach("td", ChildComponents.Count, (content, index) =>
                                            {
                                                content.Content(((TTableColumnBase)ChildComponents[index]).FooterContent);
                                            });
                                    })
                                .Close();
                    });
        }

        #endregion
    }

    private void BuildRow(RenderTreeBuilder builder,int rowIndex, TableRowDataType type, TItem? data)
    {
        switch ( type )
        {
            case TableRowDataType.Expand:
                var expandColumn = GetColumns<TTableExpandColumn<TItem>>().FirstOrDefault();
                if ( expandColumn is null )
                {
                    break;
                }
                builder.AddAttribute(0, "class", "t-table__expanded-row");
                builder.AddContent(1, expandColumn.GetExpandedRow());
                break;
            case TableRowDataType.Data:
                builder.AddContent(0,row =>
                    {
                        var columnIndex = 0;
                        foreach ( var column in GetColumns() )
                        {
                            var key = columnIndex;

                            RenderFragment? columnContent = column switch
                            {
                                //TTableFieldColumn<TItem> fieldColumn => fieldColumn.GetColumnContent(data!, type),
                                TTableExpandColumn<TItem> expandColumn => expandColumn.GetColumnContent(data!, rowIndex),
                                _ => builder => builder.AddContent(0, column.GetColumnContent(data!, type)),
                            };

                            row.CreateElement(0, "td", columnContent, new { columnIndex, @class = column.GetCssClassString() }, key: key);
                            columnIndex++;
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
