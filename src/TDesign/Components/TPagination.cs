using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 用于模块内切换内容的分页
/// </summary>
[CssClass("t-pagination")]
public class TPagination:BlazorComponentBase
{
    #region 参数
    /// <summary>
    /// 设置分页组件的大小。
    /// </summary>
    [Parameter][BooleanCssClass("t-size-s","t-size-m")] public bool Small { get; set; }

    #region Current
    /// <summary>
    /// 设置当前页码。
    /// </summary>
    [Parameter][EditorRequired]public int Current { get; set; }
    /// <summary>
    /// 设置一个当页码变更时的回调方法。
    /// </summary>
    [Parameter] public EventCallback<int> CurrentChanged { get; set; }
    #endregion

    #region PageSize
    /// <summary>
    /// 设置每一页的数据量。默认是 10。
    /// </summary>
    [Parameter] [EditorRequired]public int PageSize { get; set; } = 10;
    /// <summary>
    /// 设置一个当每页数据量变更时的回调方法。
    /// </summary>
    [Parameter]public EventCallback<int> PageSizeChanged { get; set; }
    #endregion

    #region Total
    /// <summary>
    /// 设置分页的总数据量。必须要大于 0。
    /// </summary>
    [Parameter][EditorRequired]public long Total { get; set; }
    /// <summary>
    /// 设置一个当总数据量变化时的回调方法。
    /// </summary>
    [Parameter]public EventCallback<long> TotalChanged { get; set; }
    /// <summary>
    /// 设置显示总数据量的任意内容。
    /// </summary>
    [Parameter]public RenderFragment<long>? TotalContent { get; set; }
    /// <summary>
    /// 设置一个布尔值，表示是否显示总数据量的内容。
    /// </summary>
    [Parameter]public bool ShowTotal { get; set; }
    #endregion

    /// <summary>
    /// 设置分页页码条的显示数量。建议范围在 5-21 之间，默认是 7。
    /// </summary>
    [Parameter] public int PageNumber { get; set; } = 7;
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
            if ( total <= 0 )
            {
                total = 1;
            }

            var result = total / PageSize;
            if ( result < 0 )
            {
                result = 1;
            }
            return result;
        }
    }
    #endregion

    #region Method
    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if ( Current <= 0 )
        {
            throw new ArgumentException($"{nameof(Current)} 必须大于0");
        }
        if ( PageSize <= 0 )
        {
            throw new ArgumentException($"{nameof(PageSize)} 必须大于0");
        }
        if ( Total <= 0 )
        {
            throw new ArgumentException($"{nameof(Total)} 必须大于0");
        }
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildTotal(builder, sequence);
        BuildPrevOrNextBtn(builder, sequence + 1, true);
        BuildPageNumbers(builder, sequence + 2);
        BuildPrevOrNextBtn(builder, sequence + 3, false);
    }

    /// <summary>
    /// 构建总数据量的元素。
    /// </summary>
    void BuildTotal(RenderTreeBuilder builder, int sequence) => builder.CreateElement(sequence, "div", TotalContent?.Invoke(Total), new { @class = "t-pagination__total" }, ShowTotal);

    /// <summary>
    /// 构建上一页或下一页按钮。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    /// <param name="prevOrNext"><c>true</c> 表示上一页，否则是下一页。</param>
    void BuildPrevOrNextBtn(RenderTreeBuilder builder, int sequence,bool prevOrNext)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateComponent<TIcon>(0, attributes: new { Name = prevOrNext ? IconName.ChevronLeft : IconName.ChevronRight });
        }, new { @class = HtmlHelper.CreateCssBuilder().Append("t-pagination__btn").Append("t-pagination__btn-prev", prevOrNext).Append("t-pagination__btn-next") });
    }

    void BuildPageNumbers(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "ul", content =>
        {
            var (start, end) = ComputePageNumber();
            for ( long i = start; i <=end; i++ )
            {
                content.CreateElement((int)i, "li", i.ToString(), new { @class = "t-pagination__number" });
            }

        }, new { @class = "t-pagination__pager" });


        /// <summary>
        /// 计算分页页码的开始和结束的页码。
        /// </summary>
        /// <returns>开始和结束的页码。</returns>
        (long start, long end) ComputePageNumber()
        {
            var start = 0L;
            var end = 0L;

            var middle = PageNumber / 2;
            if ( Current <= middle )
            {
                start = 1;
                end = PageNumber;
            }
            else if ( Current > middle )
            {
                start = Current - middle;
                end = Current + middle;
            }
            if ( end > TotalPages )
            {
                end = TotalPages;
                if ( start + end != PageNumber - 2 )
                {
                    start = end - PageNumber + 2 - 1;
                }
            }
            if ( end <= PageNumber )
            {
                start = 1;
            }

            return (start, end);
        }
    }

    #endregion
}
