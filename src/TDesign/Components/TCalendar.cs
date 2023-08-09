using ComponentBuilder.FluentRenderTree;
using System;
using TDesign.Specifications;

namespace TDesign;

/// <summary>
/// 按照日历形式展示数据或日期的容器。
/// </summary>
[CssClass("t-calendar")]
public partial class TCalendar : TDesignComponentBase,IHasTitleText,IHasTitleFragment
{
    /// <summary>
    /// 今天的日期
    /// </summary>
    readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);

    #region Parameters
    /// <summary>
    /// 是否隐藏控制面板。
    /// </summary>
    [ParameterApiDoc("是否隐藏控制面板")]
    [Parameter] public bool HideController { get; set; }
    /// <summary>
    /// 使用卡片模式。
    /// </summary>
    [ParameterApiDoc("使用卡片模式")]
    [Parameter][BooleanCssClass("t-calendar--card","t-calendar--full")]public bool Card { get; set; }
    /// <summary>
    /// 日历的模式。
    /// </summary>
    [ParameterApiDoc("日历的模式",Value = "Month")]
    [Parameter] public CalendarMode Mode { get; set; } = CalendarMode.Month;

    /// <summary>
    /// 是否显示周末。<see cref="CalendarMode.Mounth"/> 有效。
    /// </summary>
    [ParameterApiDoc("是否显示周末",Value ="true")]
    [Parameter] public bool ShowWeekend { get; set; } = true;
    /// <summary>
    /// 日历呈现的年份。默认是当前年份。
    /// </summary>
    [ParameterApiDoc("日历呈现的年份，默认是当前年份",Value = "DateTime.Now.Year")]
    [Parameter] public int Year { get; set; } = DateTime.Now.Year;
    /// <summary>
    /// 日历呈现的月份。默认是当前月份。
    /// </summary>
    [ParameterApiDoc("日历呈现的月份。默认是当前月份", Value = "DateTime.Now.Month")]
    [Parameter] public int Month { get; set; } = DateTime.Now.Month;
    /// <summary>
    /// 小于 10 的日期，是否使用 '0' 填充。
    /// </summary>
    [ParameterApiDoc("小于 10 的日期，是否使用 '0' 填充")]
    [Parameter]public bool FiilWithZero { get; set; }
    /// <summary>
    /// 周几算第一天。
    /// </summary>
    [ParameterApiDoc("周几算第一天", Value ="Monday")]
    [Parameter] public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Monday;

    /// <summary>
    /// 设置周的自定义内容。
    /// </summary>
    [ParameterApiDoc("周的自定义内容", Type = "RenderFragment<DayOfWeek>?")]
    [Parameter]public RenderFragment<DayOfWeek>? WeekContent { get; set; }

    /// <summary>
    /// 年月的开始范围。
    /// </summary>
    [ParameterApiDoc("年月的开始范围")]
    [Parameter]public DateOnly? StartDate { get; set; }
    /// <summary>
    /// 年月的结束范围。
    /// </summary>
    [ParameterApiDoc("年月的结束范围")]
    [Parameter] public DateOnly? EndDate { get; set; }
    /// <summary>
    /// 设置标题。
    /// </summary>
    [ParameterApiDoc("日历的标题")]
    [Parameter] public string? TitleText { get; set; }
    /// <summary>
    /// 设置标题的任意代码片段。
    /// </summary>
    [ParameterApiDoc("标题的任意代码片段")]
    [Parameter] public RenderFragment? TitleContent { get; set; }
    /// <summary>
    /// 自定义日历单元格的内容。
    /// </summary>
    [ParameterApiDoc("自定义日历单元格的内容")]
    [Parameter]public RenderFragment<(DateOnly date,CalendarMode mode)>? CellContent { get; set; }
    /// <summary>
    /// 是否允许多个日期被选中。
    /// </summary>
    [ParameterApiDoc("是否允许多个日期被选中")]
    [Parameter]public bool Multiple { get; set; }

    /// <summary>
    /// 高亮显示的日期，默认时当天或当月。
    /// </summary>
    [ParameterApiDoc("高亮显示的日期，默认时当天或当月")]
    [Parameter]public List<DateOnly> SelectedDates { get; set; } = new()
    {
        DateOnly.FromDateTime(DateTime.Today)
    };
    /// <summary>
    /// 当日期改变时触发的回调。
    /// </summary>
    [ParameterApiDoc("当日期被选中时触发的回调", Type= "EventCallback<IReadOnlyList<DateOnly>>")]
    [Parameter]public EventCallback<IReadOnlyList<DateOnly>> OnDateSelected { get; set; }
    #endregion

    #region Internal
    /// <summary>
    /// 星期对应的文本。
    /// </summary>
    Dictionary<DayOfWeek, string> DayOfWeekTextMapper { get; set; } = new()
    {
        [DayOfWeek.Monday] = "周一",
        [DayOfWeek.Tuesday] = "周二",
        [DayOfWeek.Wednesday] = "周三",
        [DayOfWeek.Thursday] = "周四",
        [DayOfWeek.Friday] = "周五",
        [DayOfWeek.Saturday] = "周六",
        [DayOfWeek.Sunday] = "周日"
    };

    List<DayOfWeek> DayOfWeekList { get; set; } = new()
    {
        DayOfWeek.Monday,
        DayOfWeek.Tuesday,
        DayOfWeek.Wednesday,
        DayOfWeek.Thursday,
        DayOfWeek.Friday,
        DayOfWeek.Saturday,
        DayOfWeek.Sunday
    };


    /// <summary>
    /// 当前查看的年份。
    /// </summary>
    int CurrentViewYear { get; set; }
    /// <summary>
    /// 当前查看的月份。
    /// </summary>
    int CurrentViewMonth { get; set; }


    #endregion

    #region Callback Function
    /// <summary>
    /// 切换月年模式。
    /// </summary>
    void SwitchCalendarMode(CalendarMode mode)
    {
        Mode = mode;
        StateHasChanged();
    }

    /// <summary>
    /// 切换周末是否显示。
    /// </summary>
    void SwitchWeekend()
    {
        if ( Mode != CalendarMode.Month )
        {
            return;
        }

        ShowWeekend = !ShowWeekend;
        StateHasChanged();
    }

    void ChangeDate(int year, int month)
    {
        if ( CurrentViewYear == year && CurrentViewMonth == month )
        {
            return;
        }

        CurrentViewYear = Year = year;
        CurrentViewMonth = Month = month;
        StateHasChanged();
    }
    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        CurrentViewYear = Year;
        CurrentViewMonth = Month;

        StartDate ??= new DateOnly(CurrentViewYear - 10, 1, 1);
        EndDate ??= new DateOnly(CurrentViewYear + 10, 12, 1);

        if ( StartDate >= EndDate )
        {
            throw new InvalidOperationException($"{nameof(StartDate)} 不能大于 {nameof(EndDate)}");
        }

        #region 调整第一天是周几的顺序
        var theFirstDayIndex = DayOfWeekList.FindIndex(m => m == FirstDayOfWeek);
        var orderRange = DayOfWeekList.GetRange(theFirstDayIndex, DayOfWeekList.Count - theFirstDayIndex);
        DayOfWeekList.RemoveRange(theFirstDayIndex, DayOfWeekList.Count - theFirstDayIndex);
        DayOfWeekList.InsertRange(0, orderRange);
        #endregion
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildControl(builder);

        builder.Div("t-calendar__panel")
            .Class(Mode.GetCssClass("t-calendar__panel--"))
            .Content(BuildCalendarTable)
            .Close();
    }

        /// <summary>
    /// 是否跳过周末
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    bool IsSkipWeekend(DayOfWeek day) => !ShowWeekend && new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(day);

    /// <summary>
    /// 判断一个日期是否超出了定义的日期范围
    /// </summary>
    /// <param name="currentDate">要判断的日期。</param>
    /// <returns></returns>
    private bool IsOutOfRange(DateOnly currentDate) => StartDate.HasValue && currentDate < StartDate || EndDate.HasValue && currentDate > EndDate;
}

/// <summary>
/// 日历模式。
/// </summary>
public enum CalendarMode
{
    /// <summary>
    /// 按月显示。
    /// </summary>
    Month,
    /// <summary>
    /// 按年显示。
    /// </summary>
    Year,
}