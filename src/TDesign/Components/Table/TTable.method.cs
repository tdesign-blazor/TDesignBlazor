/*
 * TTable 的部分类，该cs文件用于对方法的归类
 */

namespace TDesign;

partial class TTable<TItem>
{
    /// <inheritdoc/>
    protected override void AfterSetParameters(ParameterView parameters)
    {
        if (DataQuery is null && Data is null)
        {
            throw new InvalidOperationException($"必须提供 {nameof(Data)} 或 {nameof(DataQuery)} 参数，但不能两个同时提供");
        }
    }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        EmptyContent ??= builder => builder.AddContent(0, "暂无数据");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
            await QueryData(PageIndex, PageSize);
        }
    }

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateCascadingComponent(this, 0, base.BuildRenderTree, "Table");
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildTable(builder, sequence + 1);
        BuildPagination(builder, sequence + 2);
    }

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
                BuildTableContent(container, 0);
                container.CreateComponent<TLoading>(1, attributes: new { Overlay = true });
            });
        }
        else
        {
            BuildTableContent(builder, sequence);
        }
    }

    /// <summary>
    /// 构造表格的 tbody 内容部分。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    void BuildTableContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "table", table =>
            {
                BuildTableHeader(table, 0);
                BuildTableBody(table, 1);
                BuildTableFooter(table, 2);
            },
            new
            {
                @class = HtmlHelper.Class.Append("t-table--layout-auto", AutoWidth, "t-table--layout-fixed")
            });
        }, new
        {
            @class = "t-table__content",
            style = HtmlHelper.Style.Append($"max-height:{FixedHeight}px", FixedHeight.HasValue)
        });
    }

    /// <summary>
    /// 构建分页。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">源代码的位置。</param>
    void BuildPagination(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateComponent<TPagination>(sequence, attributes: new
        {
            Current = PageIndex,
            CurrentChanged = PageIndexChanged,
            PageSize,
            PageSizeChanged,
            Total=TotalCount,
            TotalChanged=TotalCountChanged
        });
    }
    /// <summary>
    /// 构建 theader 部分。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">源代码的位置。</param>
    void BuildTableHeader(RenderTreeBuilder builder, int sequence)
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
    void BuildTableBody(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "tbody", content =>
        {
            if ( DataLoaded.Any() )
            {
                var index = 0;
                foreach ( var item in DataLoaded! )
                {
                    var key = index;
                    content.CreateComponent<TTableRow>(index + 1, tr =>
                    {
                        tr.AddContent(0, ChildContent!.Invoke(item));
                    }, key: key);

                    index++;
                }
            }
            else
            {
                BuildEmptyContent(content);
            }
        }, new
        {
            @class = HtmlHelper.Class.Append("t-table__body")
        });
    }

    /// <summary>
    /// 构建空表格的内容。
    /// </summary>
    /// <param name="builder"></param>
    void BuildEmptyContent(RenderTreeBuilder builder)
    {
        builder.CreateElement(0, "tr", content =>
        {
            content.CreateElement(0, "td", empty =>
            {
                empty.CreateElement(0, "div", EmptyContent, new { @class = "t-table__empty" });
            }, new { colspan = ChildComponents.Count });

        }, new { @class = "t-table__empty-row" });
    }

    /// <summary>
    /// 构建 tfooter 部分。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">源代码的位置。</param>
    void BuildTableFooter(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "tfoot", content =>
        {

        }, new
        {
            @class = HtmlHelper.Class.Append("t-table__footer")
                                                    .Append("t-table__footer--fixed", FixedFooter)
        });
    }

    /// <summary>
    /// 以异步的方式查询数据。
    /// </summary>
    /// <param name="page">页码。</param>
    /// <param name="size">数据量。</param>
    public async Task QueryData(int page=1,int size=10)
    {
        if ( page < 1 )
        {
            throw new InvalidOperationException("页码不能小于1");
        }
        if(size < 0 )
        {
            throw new InvalidOperationException("数据量不能小于0");
        }

        if ( Data is not null )
        {
            DataLoaded = Data.Skip((page - 1)*size).Take(page * size);
            TotalCount = !Data.Any() ? 1 : Data.Count();
            await TotalCountChanged.InvokeAsync(TotalCount);
        }
        if ( DataQuery is not null )
        {
            Loading = true;
            var queryTask = DataQuery.Invoke(new(page, size));
            if ( queryTask.IsCompleted )
            {
                var result = await queryTask;
                DataLoaded = result.Data;
                TotalCount = result.Count;
            }
            Loading = false;
        }
        await this.Refresh();
    }
}