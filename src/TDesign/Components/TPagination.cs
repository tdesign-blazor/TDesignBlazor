using System.Linq.Expressions;

namespace TDesign;
/// <summary>
/// 用于模块内切换内容的分页
/// </summary>
[CssClass("t-pagination")]
public partial class TPagination : TDesignComponentBase
{
    #region 参数
    /// <summary>
    /// 设置分页组件的大小。
    /// </summary>
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;

    #region Current
    /// <summary>
    /// 设置当前页码。
    /// </summary>
    [Parameter] public int PageIndex { get; set; } = 1;
    /// <summary>
    /// 设置当页码改变时的回调，通常这是双向绑定语法。
    /// </summary>
    [Parameter] public EventCallback<int> PageIndexChanged { get; set; }
    #endregion

    #region PageSize
    /// <summary>
    /// 设置每一页的数据量。默认是 10。
    /// </summary>
    [Parameter]public int PageSize { get; set; } = 10;
    /// <summary>
    /// 设置一个当每页数据量变更时的回调方法。
    /// </summary>
    [Parameter] public EventCallback<int> PageSizeChanged { get; set; }
    #endregion

    #region Total
    /// <summary>
    /// 设置分页的总数据量。必须要大于 0。
    /// </summary>
    [Parameter] public int Total { get; set; }
    /// <summary>
    /// 设置一个当总数据量变化时的回调方法。
    /// </summary>
    [Parameter] public EventCallback<int> TotalChanged { get; set; }
    /// <summary>
    /// 设置显示总数据量的任意内容。
    /// </summary>
    [Parameter] public RenderFragment<int>? TotalContent { get; set; }
    /// <summary>
    /// 设置一个布尔值，表示是否显示总数据量的内容。
    /// </summary>
    [Parameter] public bool ShowTotal { get; set; } = true;
    #endregion

    #region PageNumber
    /// <summary>
    /// 设置分页页码条的显示数量。建议范围在 5-21 之间，默认是 7。
    /// </summary>
    [Parameter] public int PageNumber { get; set; } = 7;
    /// <summary>
    /// 设置是否显示页码条。
    /// </summary>
    [Parameter] public bool ShowPageNumber { get; set; } = true;
    #endregion

    /// <summary>
    /// 是否显示首页和末页按钮。
    /// </summary>
    [Parameter] public bool ShowFirstLastBtn { get; set; }

    /// <summary>
    /// 设置当页码数量超出时，前后省略模式。
    /// </summary>
    [Parameter] public PageEllipsisMode EllipsisMode { get; set; } = PageEllipsisMode.Middle;

    #region JumpPage
    /// <summary>
    /// 设置是否显示跳转到文本框。
    /// </summary>
    [Parameter] public bool ShowJumpPage { get; set; }
    /// <summary>
    /// 设置为极简版的分页。
    /// </summary>
    [Parameter] public bool Simple { get; set; }
    #endregion
    #endregion


    #region Internal Property
    /// <summary>
    /// 获取总页数。
    /// </summary>
    int TotalPages
    {
        get
        {
            var total = Total + PageSize - 1;
            if (total <= 0)
            {
                total = 1;
            }

            var result = total / PageSize;
            if (result < 0)
            {
                result = 1;
            }
            return result;
        }
    }
    /// <summary>
    /// 跳转页码。
    /// </summary>
    int JumpPage { get; set; } = 1;
    #endregion

    #region Method

    #region Public

    /// <summary>
    /// 跳转到指定页。
    /// </summary>
    /// <param name="page">要跳转的页码。</param>
    public async Task NavigateToPage(int page)
    {
        page = page < 1 ? 1 : page;
        page = page > TotalPages ? TotalPages : page;

        PageIndex = page;
        JumpPage = page;
        await PageIndexChanged.InvokeAsync(page);
        await this.Refresh();
    }
    /// <summary>
    /// 跳转到首页。
    /// </summary>
    public Task NavigateToFirst() => NavigateToPage(1);
    /// <summary>
    /// 跳转到末页。
    /// </summary>
    public Task NavigateToLast() => NavigateToPage(TotalPages);
    /// <summary>
    /// 跳转到上一页。
    /// </summary>
    public Task NavigateToPrevious() => NavigateToPage(--PageIndex);
    /// <summary>
    /// 跳转到下一页。
    /// </summary>
    public Task NavigateToNext() => NavigateToPage(++PageIndex);
    #endregion

    #region Protected

    /// <inheritdoc/>
    protected override void AfterSetParameters(ParameterView parameters)
    {
        TotalContent ??= value => new RenderFragment(builder => builder.AddContent(0, $"共 {Total} 项数据"));
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildTotal(builder);
        BuildPageSizeSelect(builder);

        BuildPageBehaviorBtn(builder, PageButtonBehavior.First, IconName.PageFirst, ShowFirstLastBtn, PageIndex <= 1);
        BuildPageBehaviorBtn(builder, PageButtonBehavior.Previous, IconName.ChevronLeft, disabled: PageIndex <= 1);

        if (Simple)
        {
            BuildJumper(builder);
        }
        else
        {
            BuildPageNumbers(builder);
        }

        BuildPageBehaviorBtn(builder, PageButtonBehavior.Next, IconName.ChevronRight, disabled: PageIndex >= TotalPages);
        BuildPageBehaviorBtn(builder, PageButtonBehavior.Last, IconName.PageLast, ShowFirstLastBtn, PageIndex >= TotalPages);

        if (!Simple)
        {
            BuildJumper(builder);
        }
    }
    #endregion

    #region Private
    /// <summary>
    /// 构建总数据量的元素。
    /// </summary>
    void BuildTotal(RenderTreeBuilder builder)
        => builder.Div("t-pagination__total", ShowTotal).Content(TotalContent?.Invoke(Total)).Close();

    /// <summary>
    /// 构建每页数据量的下拉菜单。
    /// TODO，等待 Select 组件完成
    /// </summary>
    /// <param name="builder"></param>
    void BuildPageSizeSelect(RenderTreeBuilder builder)
        => builder.Div("t-select__wrap t-pagination__select").Content(HtmlHelper.Instance.CreateContent($"{PageSize} 条/页")).Close();

    /// <summary>
    /// 构建上一页或下一页按钮。
    /// <param name="prevOrNext"><c>true</c> 表示上一页，否则是下一页。</param>
    /// <param name="disabled">是否被禁用。</param>
    void BuildPageBehaviorBtn(RenderTreeBuilder builder, PageButtonBehavior behavior, object iconName, bool show = true, bool disabled = default)
        => builder
            .Div("t-pagination__btn", show)
                .Class("t-pagination__btn-prev", behavior is PageButtonBehavior.First or PageButtonBehavior.Previous)
                .Class("t-pagination__btn-next", behavior is PageButtonBehavior.Next or PageButtonBehavior.Last)
                .Class("t-is-disabled", disabled)
                .Callback("onclick", this, () =>
                {
                    if (disabled)
                    {
                        return Task.CompletedTask;
                    }
                    return behavior switch
                    {
                        PageButtonBehavior.First => NavigateToFirst(),
                        PageButtonBehavior.Last => NavigateToLast(),
                        PageButtonBehavior.Previous => NavigateToPrevious(),
                        _ => NavigateToNext()
                    };
                })
                .Content(icon => icon.Component<TIcon>().Attribute(new { Name = iconName }).Close())
            .Close()
            ;

    /// <summary>
    /// 该方法用于构建分页的页码条的各项，例如上一页按钮，分页页码，末页按钮等。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    /// <param name="content">显示的文本。</param>
    /// <param name="callback">点击的回调。</param>
    /// <param name="disabled">是否禁用。</param>
    /// <param name="additionalClass">附加的 class 样式。</param>
    void BuildPageItem(RenderTreeBuilder builder, int sequence, RenderFragment content, Func<Task>? callback = default, bool disabled = default, string? additionalClass = default)
        => builder.Element("li","t-pagination__number")
                             .Class("t-is-disabled", disabled)
                             .Class(additionalClass, !string.IsNullOrEmpty(additionalClass))
                             .Callback("onclick", this, () =>
                             {
                                 callback?.Invoke();
                             }, callback is not null && !disabled)
                            .Content(content)
                            .Close();
    /// <summary>
    /// 构建分页数字。
    /// </summary>
    /// <param name="pageNumber">分页数字。</param>
    void BuildPageNumerItem(RenderTreeBuilder builder, int sequence, int pageNumber)
    {
        if (pageNumber == PageIndex)
        {
            BuildPageItem(builder, sequence, number => number.AddContent(0, pageNumber), additionalClass: "t-is-current");
        }
        else
        {
            BuildPageItem(builder, sequence, number => number.AddContent(0, pageNumber), () => NavigateToPage(pageNumber));
        }
    }

    /// <summary>
    /// 构建分页条。
    /// </summary>
    void BuildPageNumbers(RenderTreeBuilder builder)
        => builder.Element("ul","t-pagination__pager", ShowPageNumber)
                        .Content(content =>
                        {
                            //总页数不足2页，就显示1页的分页条
                            if ( TotalPages < 2 )
                            {
                                BuildPageNumerItem(content, 100, 1);
                            }
                            else
                            {
                                if ( EllipsisMode == PageEllipsisMode.Middle )
                                {
                                    #region 第一页

                                    //第一页永远显示
                                    BuildPageNumerItem(content, 0, 1);

                                    #endregion

                                    #region 前5页
                                    if ( PageIndex > PageNumber / 2 )
                                    {
                                        var backTo = PageNumber - 5;
                                        if ( backTo <= 1 )
                                        {
                                            backTo = 1;
                                        }
                                        BuildPageItem(content, 1, text => text.CreateComponent<TIcon>(0, attributes: new { Name = IconName.Ellipsis }), () => NavigateToPage(backTo));
                                    }
                                    #endregion
                                }

                                #region 页码条

                                var (start, end) = ComputePageNumber();

                                //页码1 永远显示，所有从2开始
                                //最后一页永远显示，所以结束要少一个索引
                                var offset = (EllipsisMode == PageEllipsisMode.Middle ? 1 : 0);
                                for ( var i = start + offset; i <= end - offset; i++ )
                                {
                                    var current = i;
                                    var contentSequence = (int)i + 30;

                                    BuildPageNumerItem(content, contentSequence, current);
                                }
                                #endregion

                                if ( EllipsisMode == PageEllipsisMode.Middle )
                                {
                                    #region 后5页
                                    if ( PageIndex < TotalPages - PageNumber / 2 )
                                    {
                                        var nextTo = PageIndex + 5;
                                        if ( nextTo >= TotalPages )
                                        {
                                            nextTo = TotalPages;
                                        }
                                        BuildPageItem(content, 90, text => text.CreateComponent<TIcon>(0, attributes: new { Name = IconName.Ellipsis }), () => NavigateToPage(nextTo));
                                    }
                                    #endregion

                                    #region 末页
                                    BuildPageNumerItem(content, 100, TotalPages);
                                    #endregion
                                }
                            }
                        })
                        .Close();


    /// <summary>
    /// 计算分页页码的开始和结束的页码。
    /// </summary>
    /// <returns>开始和结束的页码。</returns>
    (int start, int end) ComputePageNumber()
    {
        var start = 0;
        var end = 0;

        var middle = PageNumber / 2;
        if (PageIndex <= middle)
        {
            start = 1;
            end = PageNumber;
        }
        else if (PageIndex > middle)
        {
            start = PageIndex - middle;
            end = PageIndex + middle;
        }
        if (end > TotalPages)
        {
            end = TotalPages;
            if (start + end != PageNumber - 2)
            {
                start = end - PageNumber + 2 - 1;
            }
        }
        if (end <= PageNumber)
        {
            start = 1;
        }

        return (start, end);
    }

    /// <summary>
    /// 构建 跳转到 文本框。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    void BuildJumper(RenderTreeBuilder builder)
        => builder.Div("t-pagination__jump", ShowJumpPage)
                            .Content(content =>
                            {
                                content.AddContent(0, "跳至");

                                content
                                        .Component<TInputAdornment>()
                                        .Attribute(m=>m.Append, $"/{TotalPages} 页")
                                        .Content(input => input.CreateComponent<TInputNumber<int>>(0, attributes: new
                                        {
                                            AdditionalClass = "t-pagination__input",
                                            Theme = InputNumberTheme.Normal,
                                            Min = 1,
                                            Max = TotalPages,
                                            Size,
                                            Value = JumpPage,
                                            ValueExpression = (Expression<Func<int>>)(() => JumpPage),
                                            ValueChanged = HtmlHelper.Instance.Callback().Create<int>(this, NavigateToPage)
                                        }))
                                        .Close();
                            })
                            .Close();
    #endregion

    #endregion

    /// <summary>
    /// 分页按钮的行为。
    /// </summary>
    enum PageButtonBehavior
    {
        /// <summary>
        /// 首页。
        /// </summary>
        First,
        /// <summary>
        /// 上一页。
        /// </summary>
        Previous,
        /// <summary>
        /// 下一页。
        /// </summary>
        Next,
        /// <summary>
        /// 末页。
        /// </summary>
        Last
    }
}

/// <summary>
/// 页码条省略模式。
/// </summary>
public enum PageEllipsisMode
{
    /// <summary>
    /// 中间省略。
    /// </summary>
    Middle,
    /// <summary>
    /// 两端省略。
    /// </summary>
    BothEnds
}