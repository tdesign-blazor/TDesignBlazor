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
                                        BuildBody(table, 1);
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
            builder.CreateElement(sequence + 1, "thead", content =>
            {
                content.CreateComponent<TTableRow>(0, tr =>
                {
                    tr.CreateCascadingComponent(true, sequence, ChildContent?.Invoke(default), "IsHeader");
                });

            }, new
            {
                @class = HtmlHelper.Class.Append("t-table__header")
                                                        .Append("t-table__header--fixed", FixedHeader)
            });
        }
        /// <summary>
        /// 构建 tbody 部分。
        /// </summary>
        /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
        /// <param name="sequence">源代码的位置。</param>
        void BuildBody(RenderTreeBuilder builder, int sequence)
        {
            builder.Fluent().Element("tbody", "t-table__body")
                    .Content(content =>
                    {
                        var index = 0;
                        foreach ( var item in TableData )
                        {
                            var key = index;
                            content.CreateComponent<TTableRow>(index, ChildContent!.Invoke(item), new
                            {
                                onclick = HtmlHelper.Event.Create<MouseEventArgs>(this, e => SelectRow(key), RowSelection)
                            }, key: key);
                            index++;
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

    /// <summary>
    /// 获取列集合。
    /// </summary>
    internal IEnumerable<TTableColumnBase> GetColumns() => GetColumns<TTableColumnBase>();
    /// <summary>
    /// 获取指定类型的列集合。
    /// </summary>
    /// <typeparam name="TTableColumn">列的类型。</typeparam>
    internal IEnumerable<TTableColumn> GetColumns<TTableColumn>() where TTableColumn : TTableColumnBase => ChildComponents.OfType<TTableColumn>();

}
