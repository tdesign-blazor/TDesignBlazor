namespace TDesign;

partial class TCalendar
{
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
        if ( Mode != CalendarMode.Month )
        {
            return;
        }
        builder.Element("thead")
            .Class("t-calendar__table-head", Card)
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

                            WeekContent ??= value =>builder=> builder.AddContent(0, mapperText);

                            tr.Element("th", "t-calendar__table-head-cell")
                                    .Content(cell => cell.Span().Content(week =>
                                    {
                                        week.Content(WeekContent?.Invoke(item));
                                    }).Close())
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
                if ( Mode == CalendarMode.Month )
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
            var month = i;

            var currentDate = new DateOnly(CurrentViewYear, month, 1);
            var isOutOfRange = IsOutOfRange(currentDate);

            BuildBodyCell(builder, $"{i} 月", disabled: isOutOfRange, isToday: currentDate == Today, monthOrDay: month);

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
        var firstDay = new DateTime(CurrentViewYear, CurrentViewMonth, 1);

        var firstDayOfWeek = firstDay.DayOfWeek; //第一天所在周几
        var firstDayOfWeekIndex = (int)firstDayOfWeek;

        var findLastMonthDayIndex = DayOfWeekList.FindIndex(m => m == firstDayOfWeek);

        var lastDays = firstDay.AddDays(-findLastMonthDayIndex);
        for ( int i = 0; i < findLastMonthDayIndex; i++ )
        {
            var day = i + lastDays.Day;

            var lastMonthDateTime = new DateTime(lastDays.Year, lastDays.Month, day);

            if ( IsSkipWeekend(lastMonthDateTime.DayOfWeek) )
            {
                continue;
            }

            BuildBodyCell(builder, day.ToString(), true);
        }
        #endregion

        #region 构建这个月的日历
        var daysOfThisMonth = DateTime.DaysInMonth(CurrentViewYear, CurrentViewMonth);//这个月有多少天
        for ( int i = 0; i < daysOfThisMonth; i++ )
        {
            var day = i + 1;

            var currentDate = DateOnly.FromDateTime(new DateTime(CurrentViewYear, CurrentViewMonth, day));
            if ( IsSkipWeekend(currentDate.DayOfWeek) )
            {
                if ( FirstDayOfWeek == DayOfWeek.Monday || FirstDayOfWeek== DayOfWeek.Sunday)
                {
                    ClosePrevRowAndBuildNewRow(builder, i + 10);//跳过周末要换行
                }
                continue;
            }

            //是否在日期范围内
            bool isDateOutOfRange = IsOutOfRange(currentDate);

            var completeDayString = FiilWithZero && day < 10 ? day.ToString().PadLeft(2, '0') : day.ToString();

            BuildBodyCell(builder, completeDayString, isDateOutOfRange, Today == currentDate, day);

            if ( (day + findLastMonthDayIndex) % 7 == 0 )
            {
                ClosePrevRowAndBuildNewRow(builder, i + 10);
            }
        }
        #endregion

        #region 构建下个月的头几天到当前日历
        var lastDateTime = new DateTime(CurrentViewYear, CurrentViewMonth, daysOfThisMonth);//本月最后一天
        var lastDayOfWeek = lastDateTime.DayOfWeek;//最后一天是周几

        var lastDayOfWeekIndex = DayOfWeekList.FindIndex(m => m == lastDayOfWeek);

        var nextMonth = lastDateTime.AddDays(1);
        for ( int i = 0; i <= (5 - lastDayOfWeekIndex); i++ )
        {
            var day = i + 1;
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
                            BuildYearSelectControl(selection);

                            //月 下拉菜单
                            BuildMonthSelectControl(selection);

                            //年月切换
                            BuildYearMonthViewModeControl(selection);

                            if ( !Card )
                            {
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
                                            .Attribute(m => m.OnClick, HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, SwitchWeekend))
                                            .Content(text)
                                            .Close();
                                }, Mode == CalendarMode.Month);

                                //回到今天/本月
                                BuildSelectionCell(selection, now =>
                                {
                                    var today = DateTime.Now;

                                    now.Component<TButton>()
                                        .Attribute(m => m.Theme, Theme.Primary)
                                        .Attribute(m => m.OnClick, HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, () => ChangeDate(today.Year, today.Month)))
                                        .Content(Mode == CalendarMode.Year ? "本月" : "今天")
                                        .Close();
                                });
                            }
                        })
                        .Close();
            })
            .Close();

    }


    void BuildSelectionCell(RenderTreeBuilder selection, RenderFragment content, Condition? condition = default) 
        => selection.Div("t-calendar__control-section-cell", condition).Content(content).Close();

    /// <summary>
    /// 构建单元格。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="content"></param>
    /// <param name="disabled"></param>
    /// <param name="isToday"></param>
    /// <param name="monthOrDay">点击单元格会触发，月份或日期，取决于当前的日历模式</param>
    void BuildBodyCell(RenderTreeBuilder builder, string content, bool disabled = false, bool isToday = default, int monthOrDay = 1)
    {
        var selectedDate = new DateOnly(CurrentViewYear, CurrentViewMonth, monthOrDay);
        if ( Mode == CalendarMode.Year )
        {
            selectedDate = new DateOnly(CurrentViewYear, monthOrDay, 1);
        }

        builder.AddContent(0, tr =>
        {
            tr.Div("t-calendar__table-body-cell")
                .Class("t-is-disabled", disabled)
                .Class("t-calendar__table-body-cell--now", isToday)
                .Class("t-is-checked",!disabled && SelectedDates.Contains(selectedDate))
                .Callback("onclick", this, (Action<MouseEventArgs>)(e =>
                {
                    ClickDateCell(selectedDate);
                }), !disabled)
                .Content(cell =>
                {
                    cell.Div()
                        .Style("display: flex; flex-direction: column; align-items: flex-end;")
                        .Content(inner =>
                        {
                            inner.Div("t-calendar__table-body-cell-display").Content(content).Close();
                            inner.Div("t-calendar__table-body-cell-content").Content(CellContent, (selectedDate, Mode)).Close();
                        })
                        .Close();
                })
                .Close();
        });
    }

    private void ClickDateCell(DateOnly selectedDate)
    {
        if ( Multiple ) //允许多选
        {
            if ( SelectedDates.Contains(selectedDate) )
            {
                SelectedDates.Remove(selectedDate);
            }
            else
            {
                SelectedDates.Add(selectedDate);
            }
        }
        else
        {
            SelectedDates.Clear();
            SelectedDates.Add(selectedDate);
        }

        OnDateSelected.InvokeAsync(SelectedDates);
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
    /// 构建年的下拉菜单控件
    /// </summary>
    /// <param name="builder"></param>
    private void BuildYearSelectControl(RenderTreeBuilder builder)
    {
        BuildSelectionCell(builder, year =>
        {
            year.Component<TSelect<int>>(sequence: 300)
                .Attribute(m => m.Value, Year)
                .Attribute(m => m.ValueExpression, () => Year)
                .Attribute(m => m.ValueChanged, HtmlHelper.Instance.Callback().Create<int>(this, value => ChangeDate(value, Month)
                ))
                .Attribute(m => m.AutoWidth, true)
                .Attribute(m => m.Size, Size.Small, Card)
                .Attribute(m => m.InputStyle, $"width:{(Card ? 44 : 55)}px")
                .Content(options =>
                {
                    for ( int i = StartDate!.Value.Year; i <= EndDate!.Value.Year; i++ )
                    {
                        var index = i;
                        options.Component<TSelectOption<int>>(sequence: 100)
                                .Attribute(m => m.Value, index)
                                .Attribute(m => m.Label, $"{index}年")
                                .Key(i)
                                .Close();
                    }
                })
                .Close();
        });
    }
    /// <summary>
    /// 构建月的下拉菜单控件
    /// </summary>
    /// <param name="builder"></param>
    private void BuildMonthSelectControl(RenderTreeBuilder builder)
    {
        BuildSelectionCell(builder, month =>
        {
            month.Component<TSelect<int>>(sequence: 400)
                .Attribute(m => m.Value, Month)
                .Attribute(m => m.ValueExpression, () => Month)
                .Attribute(m => m.ValueChanged, HtmlHelper.Instance.Callback().Create<int>(this, value => ChangeDate(Year, value)))
                .Attribute(m => m.AutoWidth, true)
                .Attribute(m => m.Size, Size.Small, Card)
                .Attribute(m => m.InputStyle, $"width:{(Card ? 44 : 55)}px")
                .Content(options =>
                {
                    for ( int i = 1; i <= 12; i++ )
                    {
                        var index = i;

                        var currentMonth = new DateOnly(CurrentViewYear, index, 1);

                        options.Component<TSelectOption<int>>(sequence: i)
                                .Attribute(m => m.Value, index)
                                .Attribute(m => m.Label, $"{index}月")
                                .Attribute(m => m.Disabled, IsOutOfRange(currentMonth))
                                .Key(i)
                                .Close();
                    }
                })
                .Close();
        }, Mode == CalendarMode.Month);
    }

    /// <summary>
    /// 构建年月切换的单选按钮控件
    /// </summary>
    /// <param name="builder"></param>
    private void BuildYearMonthViewModeControl(RenderTreeBuilder builder)
    {
        BuildSelectionCell(builder, mode =>
        {
            mode.Component<TInputRadioGroup<CalendarMode>>()
                .Attribute(m => m.Value, Mode)
                .Attribute(m => m.ValueExpression, () => Mode)
                .Attribute(m => m.ValueChanged, HtmlHelper.Instance.Callback().Create<CalendarMode>(this, SwitchCalendarMode))
                .Attribute(m => m.ButtonStyle, RadioButtonStyle.Filled)
                .Attribute(m => m.Size, Size.Small, Card)
                .Content(radio =>
                {
                    radio.Component<TInputRadio<CalendarMode>>()
                    .Attribute(m => m.Value, CalendarMode.Month)
                    .Content("月")
                    .Close();

                    radio.Component<TInputRadio<CalendarMode>>()
                    .Attribute(m => m.Value, CalendarMode.Year)
                    .Content("年")
                    .Close();
                })
                .Close();
        });
    }

}
