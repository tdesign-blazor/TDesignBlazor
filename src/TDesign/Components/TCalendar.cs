namespace TDesign;

/// <summary>
/// 按照日历形式展示数据或日期的容器。
/// </summary>
[CssClass("t-calendar")]
public class TCalendar : TDesignComponentBase
{
    #region Parameters
    /// <summary>
    /// 使用卡片模式。
    /// </summary>
    [Parameter][BooleanCssClass("t-calendar--card","t-calendar--full")]public bool Card { get; set; }
    /// <summary>
    /// 日历的模式。
    /// </summary>
    [Parameter] public CalendarMode Mode { get; set; } = CalendarMode.Year;

    /// <summary>
    /// 是否显示周末。<see cref="CalendarMode.Mounth"/> 有效。
    /// </summary>
    [Parameter]public bool ShowWeekend { get; set; }
    /// <summary>
    /// 日历呈现的年份。默认是当前年份。
    /// </summary>
    [Parameter] public int Year { get; set; } = DateTime.Now.Year;
    /// <summary>
    /// 日历呈现的月份。默认是当前月份。
    /// </summary>
    [Parameter] public int Month { get; set; } = DateTime.Now.Month;
    /// <summary>
    /// 小于 10 的日期，是否使用 '0' 填充。
    /// </summary>
    [Parameter]public bool FiilWithZero { get; set; }
    /// <summary>
    /// 周几算第一天。
    /// </summary>
    [Parameter] public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Monday;
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
    /// 当前年。
    /// </summary>
    int CurrentYear { get; set; }
    /// <summary>
    /// 当前月。
    /// </summary>
    int CurrentMonth { get; set; }
    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        CurrentYear = Year;
        CurrentMonth = Month;

        #region 调整第一天是周几的顺序
        var theFirstDayIndex = DayOfWeekList.FindIndex(m => m == FirstDayOfWeek);
        var orderRange = DayOfWeekList.GetRange(theFirstDayIndex, DayOfWeekList.Count - theFirstDayIndex);
        DayOfWeekList.RemoveRange(theFirstDayIndex, DayOfWeekList.Count - theFirstDayIndex);
        DayOfWeekList.InsertRange(0, orderRange);
        #endregion
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Div("t-calendar__panel")
            .Class(Mode.GetCssClass("t-calendar__panel--"))
            .Content(BuildCalendarTable)
            .Close();
    }

    void BuildCalendarTable(RenderTreeBuilder builder)
    {
        builder.Element("table", "t-calendar__table")
            .Content(table =>
            {
                BuildCalendarHeader(table);
                BuildCalendarBody(table);
            })
            .Close();
    }

    /// <summary>
    /// 构建日历的头部。
    /// </summary>
    /// <param name="builder"></param>
    void BuildCalendarHeader(RenderTreeBuilder builder)
    {
        if(Mode!= CalendarMode.Mounth )
        {
            return;
        }
        builder.Element("thead")
            .Content(head =>
            {
                head.Element("tr", "t-calendar__table-head-row")
                    .Content(tr =>
                    {
                        foreach ( var item in DayOfWeekList )
                        {
                            //不显示周末
                            if ( !ShowWeekend && new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(item) )
                            {
                                continue;
                            }

                            var mapperText = DayOfWeekTextMapper[item];

                            tr.Element("th", "t-calendar__table-head-cell")
                                    .Content(cell => cell.Span().Content(mapperText).Close())
                                    .Close();
                        }
                    })
                    .Close();
            })
            .Close();
    }

    /// <summary>
    /// 构建日历的主体。
    /// </summary>
    /// <param name="builder"></param>
    void BuildCalendarBody(RenderTreeBuilder builder)
    {
        builder.Element("tbody", "t-calendar__table-body")
            .Content(body =>
            {
                if(Mode== CalendarMode.Mounth )
                {

                }
                else
                {
                    BuildYearBody(body);
                }
            })
            .Close();
    }

    /// <summary>
    /// 构建年的单元格和行
    /// </summary>
    /// <param name="builder"></param>
    void BuildYearBody(RenderTreeBuilder builder)
    {
        //1年12个月，4列3行

        builder.OpenElement(0, "tr");
        builder.AddAttribute(1, "class", "t-calendar__table-body-row");
        var sequence = 0;
        for ( int i = 1; i <= 12; i++ )
        {
            builder.AddContent(sequence + 2, cell =>
            {
                BuildBodyCell(cell, $"{i} 月", now: CurrentYear == Year && CurrentMonth == i);
            });
            if ( i % 4 == 0 )
            {
                sequence += 4 * i;
                ClosePrevRowAndBuildNewRow(builder, sequence);
            }
        }
        builder.CloseElement();
    }

    /// <summary>
    /// 关闭上一个 tr 并开始新的 tr。用于动态换行。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    private static void ClosePrevRowAndBuildNewRow(RenderTreeBuilder builder, int sequence)
    {
        builder.CloseElement();
        builder.OpenElement(sequence, "tr");
        builder.AddAttribute(sequence + 1, "class", "t-calendar__table-body-row");
    }

    /// <summary>
    /// 构建单元格。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="content"></param>
    /// <param name="disabled"></param>
    /// <param name="now"></param>
    static void BuildBodyCell(RenderTreeBuilder builder,string content,bool disabled = false,bool now=default)
    {
        builder.Div("t-calendar__table-body-cell")
            .Class("t-is-disabled", disabled)
            .Class("t-calendar__table-body-cell--now", now)
            .Class("t-is-checked",now)
            .Content(cell =>
            {
                cell.Div()
                    .Style("display: flex; flex-direction: column; align-items: flex-end;")
                    .Content(inner =>
                    {
                        inner.Div("t-calendar__table-body-cell-display").Content(content).Close();
                        inner.Div("t-calendar__table-body-cell-content").Close();
                    })
                    .Close();
            })
            .Close();
    }
}

/// <summary>
/// 日历模式。
/// </summary>
public enum CalendarMode
{
    /// <summary>
    /// 按月显示。
    /// </summary>
    Mounth,
    /// <summary>
    /// 按年显示。
    /// </summary>
    Year,
}