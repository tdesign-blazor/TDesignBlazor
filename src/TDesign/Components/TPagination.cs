using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 用于模块内切换内容的分页
/// </summary>
[CssClass("t-pagination")]
public class TPagination : BlazorComponentBase
{
    #region 参数
    /// <summary>
    /// 设置分页组件的大小。
    /// </summary>
    [Parameter][BooleanCssClass("t-size-s", "t-size-m")] public bool Small { get; set; }

    #region Current
    /// <summary>
    /// 设置当前页码。
    /// </summary>
    [Parameter][EditorRequired] public long Current { get; set; }
    /// <summary>
    /// 设置一个当页码变更时的回调方法。
    /// </summary>
    [Parameter] public EventCallback<long> CurrentChanged { get; set; }
    #endregion

    #region PageSize
    /// <summary>
    /// 设置每一页的数据量。默认是 10。
    /// </summary>
    [Parameter][EditorRequired] public int PageSize { get; set; } = 10;
    /// <summary>
    /// 设置一个当每页数据量变更时的回调方法。
    /// </summary>
    [Parameter] public EventCallback<int> PageSizeChanged { get; set; }
    #endregion

    #region Total
    /// <summary>
    /// 设置分页的总数据量。必须要大于 0。
    /// </summary>
    [Parameter][EditorRequired] public long Total { get; set; }
    /// <summary>
    /// 设置一个当总数据量变化时的回调方法。
    /// </summary>
    [Parameter] public EventCallback<long> TotalChanged { get; set; }
    /// <summary>
    /// 设置显示总数据量的任意内容。
    /// </summary>
    [Parameter] public RenderFragment<long>? TotalContent { get; set; }
    /// <summary>
    /// 设置一个布尔值，表示是否显示总数据量的内容。
    /// </summary>
    [Parameter] public bool ShowTotal { get; set; } = true;
    #endregion

    /// <summary>
    /// 设置分页页码条的显示数量。建议范围在 5-21 之间，默认是 7。
    /// </summary>
    [Parameter] public int PageNumber { get; set; } = 7;
    /// <summary>
    /// 设置是否显示页码条。
    /// </summary>
    [Parameter]public bool ShowPageNumber { get; set; }
    #endregion


    #region Internal Property
    /// <summary>
    /// 获取总页数。
    /// </summary>
    long TotalPages
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
    #endregion

    #region Method

    #region Public

    /// <summary>
    /// 跳转到指定页。
    /// </summary>
    /// <param name="page">要跳转的页码。</param>
    public async Task NavigateToPage(long page)
    {
        page = page < 1 ? 1 : page;
        page = page > TotalPages ? TotalPages : page;

        Current = page;
        await CurrentChanged.InvokeAsync(page);
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
    public Task NavigateToPrevious() => NavigateToPage(--Current);
    /// <summary>
    /// 跳转到下一页。
    /// </summary>
    public Task NavigateToNext() => NavigateToPage(++Current);
    #endregion

    #region Protected
    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (Current <= 0)
        {
            throw new ArgumentException($"{nameof(Current)} 必须大于0");
        }
        if (PageSize <= 0)
        {
            throw new ArgumentException($"{nameof(PageSize)} 必须大于0");
        }
        if (Total <= 0)
        {
            throw new ArgumentException($"{nameof(Total)} 必须大于0");
        }

        TotalContent ??= value => new RenderFragment(builder => builder.AddContent(0, $"共 {Total} 项数据"));
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildTotal(builder, sequence);

        //上一页
        BuildPrevOrNextBtn(builder, sequence + 1, true, Current <= 1);

        BuildPageNumbers(builder, sequence + 2);

        //下一页
        BuildPrevOrNextBtn(builder, sequence + 3, false, Current >= TotalPages);
    }
    #endregion

    #region Private
    /// <summary>
    /// 构建总数据量的元素。
    /// </summary>
    void BuildTotal(RenderTreeBuilder builder, int sequence)
        => builder.CreateElement(sequence, "div", TotalContent?.Invoke(Total), new { @class = "t-pagination__total" }, ShowTotal);

    /// <summary>
    /// 构建上一页或下一页按钮。
    /// <param name="prevOrNext"><c>true</c> 表示上一页，否则是下一页。</param>
    /// <param name="disabled">是否被禁用。</param>
    void BuildPrevOrNextBtn(RenderTreeBuilder builder, int sequence, bool prevOrNext, bool disabled = default)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateComponent<TIcon>(0, attributes: new { Name = prevOrNext ? IconName.ChevronLeft : IconName.ChevronRight });
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-pagination__btn")
                                .Append("t-pagination__btn-prev", prevOrNext)
                                .Append("t-pagination__btn-next", !prevOrNext)
                                .Append("t-is-disabled", disabled)
                                ,
            onclick = HtmlHelper.CreateCallback(this, async () =>
            {
                if (prevOrNext)//上一页
                {
                    await NavigateToPrevious();
                }
                else
                {
                    await NavigateToNext();
                }
            }, !disabled)
        });
    }

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
    {
        builder.CreateElement(sequence, "li", content, new
        {
            @class = HtmlHelper.CreateCssBuilder()
                        .Append("t-pagination__number")
                        .Append("t-is-disabled", disabled)
                        .Append(additionalClass, !string.IsNullOrEmpty(additionalClass)),
            onclick = HtmlHelper.CreateCallback(this, callback, callback is not null && !disabled),
            disabled
        });
    }

    void BuildPageNumerItem(RenderTreeBuilder builder, int sequence, long pageNumber)
    {
        if (pageNumber == Current)
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
    void BuildPageNumbers(RenderTreeBuilder builder, int sequence)
    {

        //分页条
        builder.CreateElement(sequence + 1, "ul", content =>
        {
            #region 第一页

            //第一页永远显示
            BuildPageNumerItem(content, 0, 1);

            #endregion

            #region 前5页
            if (Current > PageNumber / 2)
            {
                var backTo = PageNumber - 5;
                if (backTo <= 1)
                {
                    backTo = 1;
                }
                BuildPageItem(content, 1, text => text.CreateComponent<TIcon>(0, attributes: new { Name = IconName.Ellipsis }), () => NavigateToPage(backTo));
            }
            #endregion

            #region 页码条

            var (start, end) = ComputePageNumber();

            //页码1 永远显示，所有从2开始
            //最后一页永远显示，所以结束要少一个索引

            for (long i = start + 1; i <= end - 1; i++)
            {
                var current = i;
                var contentSequence = (int)i + 30;

                BuildPageNumerItem(content, contentSequence, current);
            }
            #endregion

            #region 后5页
            if (Current < TotalPages - PageNumber / 2)
            {
                var nextTo = Current + 5;
                if (nextTo >= TotalPages)
                {
                    nextTo = TotalPages;
                }
                BuildPageItem(content, 90, text => text.CreateComponent<TIcon>(0, attributes: new { Name = IconName.Ellipsis }), () => NavigateToPage(nextTo));
            }
            #endregion

            #region 末页
            BuildPageNumerItem(content, 100, TotalPages);
            #endregion

        }, new { @class = "t-pagination__pager" }, ShowPageNumber);

        /// <summary>
        /// 计算分页页码的开始和结束的页码。
        /// </summary>
        /// <returns>开始和结束的页码。</returns>
        (long start, long end) ComputePageNumber()
        {
            var start = 0L;
            var end = 0L;

            var middle = PageNumber / 2;
            if (Current <= middle)
            {
                start = 1;
                end = PageNumber;
            }
            else if (Current > middle)
            {
                start = Current - middle;
                end = Current + middle;
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
    }
    #endregion

    #endregion
}
