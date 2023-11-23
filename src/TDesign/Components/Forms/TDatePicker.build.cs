namespace TDesign;

partial class TDatePicker<TValue>
{
    void BuildCalendarTable(RenderTreeBuilder builder)
    {
        builder.Div("t-date-picker__table")
            .Content(table =>
            {
                table.Element("table")
                    .Content(body =>
                    {
                        BuildCalendarHeader(body);
                        BuildCalendarBody(body);
                    })
                    .Close();
            })
            .Close();
    }

    /// <summary>
    /// 构建日历的头部。
    /// </summary>
    /// <param name="builder"></param>
    void BuildCalendarHeader(RenderTreeBuilder builder)
    {
        if (ViewMode != DatePickerViewMode.Date)
        {
            return;
        }
        builder.Element("thead")
            .Content(head =>
            {
                head.Element("tr")
                    .Content(tr =>
                    {
                        foreach (var item in DayOfWeekList)
                        {
                            var mapperText = DayOfWeekTextMapper[item];

                            WeekContent = value => builder => builder.AddContent(0, mapperText);

                            tr.Element("th")
                                    .Content(cell => cell.Content(week =>
                                    {
                                        week.Content(WeekContent, item);
                                    }))
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
                switch (ViewMode)
                {
                    case DatePickerViewMode.Year:
                        BuildYearBody(body);
                        break;
                    case DatePickerViewMode.Month:
                        BuildMonthBody(body);
                        break;
                    case DatePickerViewMode.Date:
                        BuildDateBody(body);
                        break;
                    case DatePickerViewMode.Week:
                        break;
                    case DatePickerViewMode.Quater:
                        break;
                    default:
                        break;
                }

            })
            .Close();
    }
    /// <summary>
    /// 构建月份的单元格和行
    /// </summary>
    /// <param name="builder"></param>
    void BuildYearBody(RenderTreeBuilder builder)
    {
        //1年12个月，4列3行

        builder.OpenElement(0, "tr");
        builder.AddAttribute(1, "class", "t-date-picker__table-month-row");
        var sequence = 0;
        for (int i = CurrentYear - 5; i <= CurrentYear + 5; i++)
        {
            var currentDate = new DateTime(CurrentYear, 1, 1);
            var year = i;

            BuildBodyCell(builder, $"{year}", currentDate, isNow: IsToday(currentDate));

            if (i % 4 == 0)
            {
                sequence += 4 * i;
                ClosePrevRowAndBuildNewRow(builder, sequence);
            }
        }
        builder.CloseElement();
    }
    /// <summary>
    /// 构建月份的单元格和行
    /// </summary>
    /// <param name="builder"></param>
    void BuildMonthBody(RenderTreeBuilder builder)
    {
        //1年12个月，4列3行

        builder.OpenElement(0, "tr");
        builder.AddAttribute(1, "class", "t-date-picker__table-month-row");
        var sequence = 0;
        for (int i = 1; i <= 12; i++)
        {
            var month = i;

            var currentDate = new DateTime(CurrentYear, month, 1);

            BuildBodyCell(builder, $"{i} 月", value: currentDate, isNow: IsToday(currentDate));

            if (i % 4 == 0)
            {
                sequence += 4 * i;
                ClosePrevRowAndBuildNewRow(builder, sequence);
            }
        }
        builder.CloseElement();
    }

    /// <summary>
    /// 构建日期视图。
    /// </summary>
    /// <remarks>
    /// 要计算日期。
    /// </remarks>
    /// <param name="builder"></param>
    void BuildDateBody(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "tr");
        builder.AddAttribute(1, "class", $"t-date-picker__table-{ViewMode.ToString().ToLower()}-row");

        #region 构建上个月剩余的几天在当前这个月的日历
        //当前年月的第一天
        var firstDay = new DateTime(CurrentYear, CurrentMonth, 1);

        var firstDayOfWeek = firstDay.DayOfWeek; //第一天所在周几


        var findLastMonthDayIndex = DayOfWeekList.FindIndex(m => m == firstDayOfWeek);

        var lastDays = firstDay.AddDays(-findLastMonthDayIndex);
        for (int i = 0; i < findLastMonthDayIndex; i++)
        {
            var day = i + lastDays.Day;

            BuildBodyCell(builder, day.ToString(),new(lastDays.Year, lastDays.Month, day), true);
        }
        #endregion

        #region 构建这个月的日历
        var daysOfThisMonth = DateTime.DaysInMonth(CurrentYear, CurrentMonth);//这个月有多少天
        for (int i = 0; i < daysOfThisMonth; i++)
        {
            var day = i + 1;

            BuildBodyCell(builder, day.ToString(), isNow: IsToday(day), value: new(CurrentYear, CurrentMonth, day));

            if ((day + findLastMonthDayIndex) % 7 == 0)
            {
                ClosePrevRowAndBuildNewRow(builder, i + 10);
            }
        }
        #endregion

        #region 构建下个月的头几天到当前日历
        var lastDateTime = new DateTime(CurrentYear, CurrentMonth, daysOfThisMonth);//本月最后一天
        var lastDayOfWeek = lastDateTime.DayOfWeek;//最后一天是周几

        var lastDayOfWeekIndex = DayOfWeekList.FindIndex(m => m == lastDayOfWeek);

        for (int i = 0; i <= (5 - lastDayOfWeekIndex); i++)
        {
            var day = i + 1;

            BuildBodyCell(builder, day.ToString(), new(CurrentYear, CurrentMonth, day), true);
        }
        #endregion

        builder.CloseElement();
    }


    void BuildPanelHeader(RenderTreeBuilder builder)
    {
        builder.Div("t-date-picker__header")
            .Content(controller =>
            {
                controller.Div("t-date-picker__header-controller")
                        .Content(controller =>
                        {
                            switch (ViewMode)
                            {
                                case DatePickerViewMode.Year:
                                case DatePickerViewMode.Month:
                                case DatePickerViewMode.Quater:
                                    BuildYearSelectControl(controller);
                                    break;
                                case DatePickerViewMode.Date:
                                case DatePickerViewMode.Week:
                                    BuildYearSelectControl(controller);
                                    BuildMonthSelectControl(controller);
                                    break;
                                default:
                                    break;
                            }
                        })
                        .Close();

                controller.Div("t-pagination-mini")
                        .Content(pagination =>
                        {
                            BuildButton(pagination, IconName.ChevronLeft, Prev);
                            BuildButton(pagination, IconName.Round, Next);
                            BuildButton(pagination, IconName.ChevronRight, Current);
                        })
                        .Close();
            })
            .Close();

        void BuildButton(RenderTreeBuilder bulder,IconName icon,Func<MouseEventArgs,Task> callback)
        {

            bulder.Component<TButton>()
                        .Attribute(m => m.Varient, ButtonVarient.Text)
                        .Attribute(m => m.Size, Size.Small)
                        .Attribute(m => m.Shape, ButtonShape.Square)
                        .Attribute(m => m.Icon, icon)
                        .Attribute(m => m.OnClick, HtmlHelper.Instance.Callback().Create(this,callback))
                        .Close();
        }

        Task Prev(MouseEventArgs e)
        {
            switch (ViewMode)
            {
                case DatePickerViewMode.Year:
                case DatePickerViewMode.Month:
                    CurrentValueAsDateTime.AddYears(-1);
                    break;
                case DatePickerViewMode.Date:
                case DatePickerViewMode.Week:
                case DatePickerViewMode.Quater:
                    CurrentValueAsDateTime.AddMonths(-1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ViewMode), $"不在可选的视图范围");
            }
            return this.Refresh();
        }
        Task Next(MouseEventArgs e)
        {
            switch (ViewMode)
            {
                case DatePickerViewMode.Year:
                case DatePickerViewMode.Month:
                    CurrentValueAsDateTime.AddYears(1);
                    break;
                case DatePickerViewMode.Date:
                case DatePickerViewMode.Week:
                case DatePickerViewMode.Quater:
                    CurrentValueAsDateTime.AddMonths(1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ViewMode), $"不在可选的视图范围");
            }
            return this.Refresh();
        }
        Task Current(MouseEventArgs e)
        {
            CurrentValueAsDateTime = DateTime.Now;
            return this.Refresh();
        }


        /// <summary>
        /// 构建年的下拉菜单控件
        /// </summary>
        /// <param name="builder"></param>
        void BuildYearSelectControl(RenderTreeBuilder builder)
        {
            builder.Component<TSelect<int>>(sequence: 300)
                .Attribute(m => m.AdditionalClass, $"t-select__wrap t-date-picker__header-controller-year")
                .Attribute(m => m.Value, CurrentYear)
                .Attribute(m => m.ValueExpression, () => CurrentYear)
                .Attribute(m => m.ValueChanged, HtmlHelper.Instance.Callback().Create<int>(this, value => ChangeYearMonth(value, CurrentMonth)))
                .Content(options =>
                {
                    for (int i = CurrentValueAsDateTime.AddYears(-5).Year; i <= CurrentValueAsDateTime.AddYears(5).Year; i++)
                    {
                        var index = i;
                        options.Component<TSelectOption<int>>(sequence: 100)
                                .Attribute(m => m.Value, index)
                                .Attribute(m => m.Label, $"{index}")
                                .Key(i)
                                .Close();
                    }
                })
            .Close();
        }
        /// <summary>
        /// 构建月的下拉菜单控件
        /// </summary>
        /// <param name="builder"></param>
        void BuildMonthSelectControl(RenderTreeBuilder builder)
        {
            builder.Component<TSelect<int>>(sequence: 400)
                    .Attribute(m => m.AdditionalClass, $"t-select__wrap t-date-picker__header-controller-month")
                    .Attribute(m => m.Value, CurrentMonth)
                    .Attribute(m => m.ValueExpression, () => CurrentMonth)
                    .Attribute(m => m.ValueChanged, HtmlHelper.Instance.Callback().Create<int>(this, value => ChangeYearMonth(CurrentYear, value)))
                    .Content(options =>
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            var index = i;

                            options.Component<TSelectOption<int>>(sequence: i)
                                    .Attribute(m => m.Value, index)
                                    .Attribute(m => m.Label, $"{index}月")
                                    .Key(i)
                                    .Close();
                        }
                    })
                .Close();
        }

    }


    /// <summary>
    /// 构建单元格。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="content"></param>
    /// <param name="disabled"></param>
    /// <param name="isNow"></param>
    /// <param name="value">取决于视图模式的值。</param>
    void BuildBodyCell(RenderTreeBuilder builder, string content, DateTime value, bool disabled = false, bool isNow = default)
    {

        builder.AddContent(0, tr =>
        {
            tr.Element("td", "t-date-picker__cell")
                        .Class("t-date-picker__cell--now", isNow)
                        .Class("t-date-picker__cell--active", !disabled && SelectedDates.Contains(value))
                        .Callback<MouseEventArgs>("onclick", this, e => SelectItems(new List<DateTime>() { value }))
                        .Content(inner =>
                        {
                            inner.Div("t-date-picker__cell-inner").Content(content).Close();
                        })
           .Close();
        });
    }

    /// <summary>
    /// 关闭上一个 tr 并开始新的 tr。用于动态换行。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    private void ClosePrevRowAndBuildNewRow(RenderTreeBuilder builder, int sequence)
    {
        builder.CloseElement();
        builder.OpenElement(sequence, "tr");
        builder.AddAttribute(sequence + 1, "class", $"t-date-picker__table-{ViewMode.ToString().ToLower()}-row");
    }
}
