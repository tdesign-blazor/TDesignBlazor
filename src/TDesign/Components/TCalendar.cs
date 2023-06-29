using TDesign.Specifications;

namespace TDesign;

/// <summary>
/// 按照日历形式展示数据或日期的容器。
/// </summary>
[CssClass("t-calendar")]
public class TCalendar : TDesignComponentBase,IHasTitleText,IHasTitleFragment
{
    #region Parameters
    /// <summary>
    /// 是否隐藏控制面板。
    /// </summary>
    [Parameter] public bool HideController { get; set; }
    /// <summary>
    /// 使用卡片模式。
    /// </summary>
    [Parameter][BooleanCssClass("t-calendar--card","t-calendar--full")]public bool Card { get; set; }
    /// <summary>
    /// 日历的模式。
    /// </summary>
    [Parameter] public CalendarMode Mode { get; set; } = CalendarMode.Month;

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
    /// <summary>
    /// 年月的开始范围。
    /// </summary>
    [Parameter]public DateTime? Start { get; set; }
    /// <summary>
    /// 年月的结束范围。
    /// </summary>
    [Parameter] public DateTime? End { get; set; }
    /// <summary>
    /// 设置标题。
    /// </summary>
    [Parameter] public string? TitleText { get; set; }
    /// <summary>
    /// 设置标题的任意代码片段。
    /// </summary>
    [Parameter] public RenderFragment? TitleContent { get; set; }

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

    #region Callback Function
    /// <summary>
    /// 切换月年模式。
    /// </summary>
    Task SwitchCalendarMode(CalendarMode mode)
    {
        Mode = mode;
        StateHasChanged();
        return Task.CompletedTask;
    }

    /// <summary>
    /// 切换周末是否显示。
    /// </summary>
    Task SwitchWeekend()
    {
        ShowWeekend = !ShowWeekend;
        StateHasChanged();
        return Task.CompletedTask;
    }

    Task ChangeDate(int year, int month)
    {
        CurrentYear = Year = year;
        CurrentMonth = Month = month;
        StateHasChanged();
        return Task.CompletedTask;
    }
    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        CurrentYear = Year;
        CurrentMonth = Month;

        Start ??= new DateTime(CurrentYear - 10, 1, 1);
        End ??= new DateTime(CurrentYear + 10, 12, 1);

        if ( Start >= End )
        {
            throw new InvalidOperationException($"{nameof(Start)} 不能大于 {nameof(End)}");
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
        if(Mode!= CalendarMode.Month )
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
                            if ( IsSkipWeekend(item) )
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
                if(Mode== CalendarMode.Month )
                {
                    BuildMonthBody(body);
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
            BuildBodyCell(builder, $"{i} 月", now: CurrentYear == Year && CurrentMonth == i);
            if ( i % 4 == 0 )
            {
                sequence += 4 * i;
                ClosePrevRowAndBuildNewRow(builder, sequence);
            }
        }
        builder.CloseElement();
    }

    /// <summary>
    /// 构建月视图。
    /// </summary>
    /// <remarks>
    /// 要计算日期。
    /// </remarks>
    /// <param name="builder"></param>
    void BuildMonthBody(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "tr");
        builder.AddAttribute(1, "class", "t-calendar__table-body-row");

        #region 构建上个月剩余的几天在当前这个月的日历
        //当前年月的第一天
        var firstDay = new DateTime(CurrentYear, CurrentMonth, 1);

        var firstDayOfWeek = firstDay.DayOfWeek; //第一天所在周几
        var firstDayOfWeekIndex = (int)firstDayOfWeek;

        var findLastMonthDayIndex = DayOfWeekList.FindIndex(m => m == firstDayOfWeek);

        var lastDays = firstDay.AddDays(-findLastMonthDayIndex);
        for ( int i = 0; i < findLastMonthDayIndex; i++ )
        {
            var day = i + lastDays.Day;

            var lastMonthDateTime = new DateTime(lastDays.Year, lastDays.Month, day);

            if( IsSkipWeekend(lastMonthDateTime.DayOfWeek) )
            {
                continue;
            }

            BuildBodyCell(builder, day.ToString(), true);
        }
        #endregion

        #region 构建这个月的日历
        var daysOfThisMonth = DateTime.DaysInMonth(CurrentYear, CurrentMonth);//这个月有多少天
        for ( int i = 0; i < daysOfThisMonth; i++ )
        {
            var day = i + 1;
            var currentDateTime = new DateTime(CurrentYear, CurrentMonth, day);
            if(IsSkipWeekend(currentDateTime.DayOfWeek) )
            {
                ClosePrevRowAndBuildNewRow(builder, i + 10);//跳过周末要换行
                continue;
            }

            var today = DateTime.Today;

            var completeDayString = FiilWithZero && day < 10 ? day.ToString().PadLeft(2, '0') : day.ToString();

            BuildBodyCell(builder, completeDayString, false, today == currentDateTime);

            if ( (i + firstDayOfWeekIndex)  % 7 == 0 )
            {
                ClosePrevRowAndBuildNewRow(builder, i + 10);
            }
        }
        #endregion

        #region 构建下个月的头几天到当前日历
        var lastDateTime = new DateTime(CurrentYear, CurrentMonth, daysOfThisMonth);//本月最后一天
        var lastDayOfWeek = lastDateTime.DayOfWeek;//最后一天是周几

        var lastDayOfWeekIndex = DayOfWeekList.FindIndex(m => m == lastDayOfWeek);

        var nextMonth = lastDateTime.AddDays(1);
        for ( int i = 0; i <= (5 - lastDayOfWeekIndex); i++ )
        {
           var day=i + 1;
            var nextMonthDateTime = new DateTime(nextMonth.Year, nextMonth.Month, day);
            if ( IsSkipWeekend(nextMonthDateTime.DayOfWeek) )
            {
                continue;
            }

            BuildBodyCell(builder, day.ToString(), true);
        }
        #endregion

        builder.CloseElement();
    }

    void BuildControl(RenderTreeBuilder builder)
    {
        builder.Div("t-calendar__control", !HideController)
            .Content(control =>
            {
                control.Div("t-calendar__title").Content(TitleContent).Close();
                control.Div("t-calendar__control-section")
                        .Content(selection =>
                        {
                            //年 下拉菜单
                            BuildSelectionCell(selection, year =>
                            {
                                year.Component<TSelect<int>>()
                                    .Attribute(m => m.Value, Year)
                                    .Attribute(m => m.ValueExpression, () => Year)
                                    .Attribute(m => m.ValueChanged, HtmlHelper.Instance.Callback().Create<int>(this, value => ChangeDate(value, Month)))
                                    .Content(options =>
                                    {
                                        for ( int i = Start!.Value.Year; i <= End!.Value.Year; i++ )
                                        {
                                            options.Component<TSelectOption<int>>()
                                                    .Attribute(m => m.Value, i)
                                                    .Attribute(m => m.Label, $"{i}年")
                                                    .Close();
                                        }
                                    })
                                    .Close();
                            });
                            //月 下拉菜单
                            BuildSelectionCell(selection, month =>
                            {
                                month.Component<TSelect<int>>()
                                    .Attribute(m => m.Value, Month)
                                    .Attribute(m => m.ValueExpression, () => Month)
                                    .Attribute(m => m.ValueChanged, HtmlHelper.Instance.Callback().Create<int>(this, value => ChangeDate(Year, value)))
                                    .Content(options =>
                                    {
                                        for ( int i = 1; i <= 12; i++ )
                                        {
                                            options.Component<TSelectOption<int>>()
                                                    .Attribute(m => m.Value, i)
                                                    .Attribute(m => m.Label, $"{i}月")
                                                    .Attribute(m => m.Disabled, i <= 6 && i < Start!.Value.Month)
                                                    .Attribute(m => m.Disabled, i > 6 && i > End!.Value.Month)
                                                    .Close();
                                        }
                                    })
                                    .Close();
                            }, Mode == CalendarMode.Month);
                            //年月切换
                            BuildSelectionCell(selection, mode =>
                            {
                                mode.Component<TInputRadioGroup<CalendarMode>>()
                                    .Attribute(m=>m.Value,Mode)
                                    .Attribute(m=>m.ValueExpression,()=>Mode)
                                    .Attribute(m=>m.ValueChanged,HtmlHelper.Instance.Callback().Create<CalendarMode>(this,SwitchCalendarMode))
                                    .Attribute(m=>m.ButtonStyle, RadioButtonStyle.Filled)
                                    .Content(radio =>
                                    {
                                        radio.Component<TInputRadio<CalendarMode>>()
                                        .Attribute(m=>m.Value,CalendarMode.Month)
                                        .Content("月")
                                        .Close();

                                        radio.Component<TInputRadio<CalendarMode>>()
                                        .Attribute(m => m.Value, CalendarMode.Year)
                                        .Content("年")
                                        .Close();
                                    })
                                    .Close();
                            });
                            //切换周末显示
                            BuildSelectionCell(selection, weekend =>
                            {
                                var theme = Theme.Primary;
                                var text = "显示周末";

                                if ( !ShowWeekend )
                                {
                                    theme = Theme.Default;
                                    text = "隐藏周末";
                                }
                                weekend.Component<TButton>()
                                        .Attribute(m => m.Theme, theme)
                                        .Attribute(m => m.OnClick, HtmlHelper.Instance.Callback().Create<MouseEventArgs?>(this, SwitchWeekend))
                                        .Content(text)
                                        .Close();
                            }, Mode == CalendarMode.Month);

                            //回到今天/本月
                            BuildSelectionCell(selection, now =>
                            {
                                var today = DateTime.Now;

                                now.Component<TButton>()
                                    .Attribute(m => m.Theme, Theme.Primary)
                                    .Attribute(m => m.OnClick, HtmlHelper.Instance.Callback().Create<MouseEventArgs?>(this, () => ChangeDate(today.Year, today.Month)))
                                    .Content(Mode == CalendarMode.Year ? "本月" : "今天")
                                    .Close();
                            });
                        })
                        .Close();
            })
            .Close();

        void BuildSelectionCell(RenderTreeBuilder selection,RenderFragment content,Condition? condition=default)
        {
            selection.Div("t-calendar__control-section-cell", condition).Content(content).Close();
        }
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
        builder.AddContent(0, tr =>
        {
            tr.Div("t-calendar__table-body-cell")
                .Class("t-is-disabled", disabled)
                .Class("t-calendar__table-body-cell--now", now)
                .Class("t-is-checked", now)
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
        });
    }

    /// <summary>
    /// 是否跳过周末
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    bool IsSkipWeekend(DayOfWeek day) => !ShowWeekend && new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(day);
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