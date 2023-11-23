namespace TDesign;

/// <summary>
/// 日期选择器，用于选择某一具体日期或某一段日期区间。
/// </summary>
public partial class TDatePicker<TValue> : TDesignInputComonentBase<TValue>
{
    readonly static Type[] Support_Types = new[] { typeof(DateOnly), typeof(DateTime), typeof(DateTimeOffset?), typeof(DateOnly?), typeof(DateTime?), typeof(DateTimeOffset?) };

    readonly static DateTime TODAY = DateTime.Now;

    /// <summary>
    /// 前缀图标，默认是 <see cref="IconName.Calendar"/> 。
    /// </summary>
    [ParameterApiDoc("输入框的前缀图标", Value = "IconName.Calendar")]
    [Parameter] public object? PrefixIcon { get; set; }
    /// <summary>
    /// 后缀图标。
    /// </summary>
    [ParameterApiDoc("输入框的后缀图标")]
    [Parameter] public object? SuffixIcon { get; set; } = IconName.Calendar;

    /// <summary>
    /// 提示文本。
    /// </summary>
    [ParameterApiDoc("提示文本")]
    [Parameter] public string? Placeholder { get; set; } = "请选择日期";

    /// <summary>
    /// 日期格式。
    /// </summary>
    [ParameterApiDoc("日期的格式")]
    [Parameter] public string Format { get; set; } = "yyyy/MM/dd";

    /// <summary>
    /// 周几算第一天。
    /// </summary>
    [ParameterApiDoc("周几算第一天", Value = "Monday")]
    [Parameter] public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Monday;

    /// <summary>
    /// 设置周的自定义内容。
    /// </summary>
    [ParameterApiDoc("周的自定义内容", Type = "RenderFragment<DayOfWeek>?")]
    [Parameter] public RenderFragment<DayOfWeek>? WeekContent { get; set; }

    /// <summary>
    /// 视图模式。
    /// </summary>
    [ParameterApiDoc("选择器的视图模式", Value= "Date")]
    [Parameter] public DatePickerViewMode ViewMode { get; set; } = DatePickerViewMode.Date;

    /// <summary>
    /// 高亮显示的日期，默认时当天或当月。
    /// </summary>
    [ParameterApiDoc("高亮显示的日期，默认时当天或当月")]
    [Parameter]
    public List<DateTime> SelectedDates { get; set; } = new() { TODAY };
    /// <summary>
    /// 当日期改变时触发的回调。
    /// </summary>
    [ParameterApiDoc("当日期被选中时触发的回调", Type = "EventCallback<IReadOnlyList<DateTime>>")]
    [Parameter] public EventCallback<IReadOnlyList<DateTime>> OnSelected { get; set; }
    #region Internal



    DateTime CurrentValueAsDateTime { get; set; } = TODAY;

    /// <summary>
    /// 文本框的输入值
    /// </summary>
    string? InputValue { get; set; }
    EventCallback<string?> InputValueChanged { get; set; }


    /// <summary>
    /// 星期对应的文本。
    /// </summary>
    Dictionary<DayOfWeek, string> DayOfWeekTextMapper { get; set; } = new()
    {
        [DayOfWeek.Monday] = "一",
        [DayOfWeek.Tuesday] = "二",
        [DayOfWeek.Wednesday] = "三",
        [DayOfWeek.Thursday] = "四",
        [DayOfWeek.Friday] = "五",
        [DayOfWeek.Saturday] = "六",
        [DayOfWeek.Sunday] = "日"
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
    int CurrentYear
    {
        get => CurrentValueAsDateTime.Year;
        set => CurrentValueAsDateTime = new(value, CurrentMonth, CurrentDay);
    }
    /// <summary>
    /// 当前查看的月份。
    /// </summary>
    int CurrentMonth
    {
        get => CurrentValueAsDateTime.Month;
        set => CurrentValueAsDateTime = new(CurrentYear, value, CurrentDay);
    }

    /// <summary>
    /// 获取当前选中的天。
    /// </summary>
    int CurrentDay
    {
        get => CurrentValueAsDateTime.Day;
    }


    #endregion

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if ( !Support_Types.Contains(typeof(TValue)) )
        {
            throw new NotSupportedException($"仅支持 {nameof(DateOnly)}、{nameof(DateTime)}、{nameof(DateTimeOffset)} 的类型");
        }

        switch (Value)
        {
            case DateOnly date:
                CurrentValueAsDateTime = date.ToDateTime(new());
                break;
            case DateTime dateTime:
                CurrentValueAsDateTime = dateTime;
                break;
            case DateTimeOffset offset:
                CurrentValueAsDateTime = offset.DateTime;
                break;
        }
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        #region 调整第一天是周几的顺序
        var theFirstDayIndex = DayOfWeekList.FindIndex(m => m == FirstDayOfWeek);
        var orderRange = DayOfWeekList.GetRange(theFirstDayIndex, DayOfWeekList.Count - theFirstDayIndex);
        DayOfWeekList.RemoveRange(theFirstDayIndex, DayOfWeekList.Count - theFirstDayIndex);
        DayOfWeekList.InsertRange(0, orderRange);
        #endregion
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Component<TPopup>()
            .Attribute(m => m.Trigger, PopupTrigger.Click)
            .Attribute(m => m.PopupContent, content =>
            {
                content.Div("t-date-picker__panel")
                    .Content(panel =>
                    {
                        content.Div("t-date-picker__panel-content")
                            .Content(p =>
                            {
                                p.Div($"t-date-picker__panel-{ViewMode.ToString().ToLower()}")
                                    .Content(inner =>
                                    {
                                        BuildPanelHeader(inner);
                                        BuildCalendarTable(inner);
                                    })
                                .Close();
                            })
                            .Close();
                    })
                    .Close();

            })
            .Attribute(m => m.ChildContent, input =>
            {
                input.Component<TInputText<string>>()
                    .Attribute(m => m.Value, InputValue)
                    .Attribute(m => m.ValueExpression,()=>InputValue)
                    .Attribute(m => m.ValueChanged, InputValueChanged)
                    .Attribute(m => m.AutoWidth, AutoWidth)
                    .Attribute(m => m.Alignment, Alignment)
                    .Attribute(m => m.Disabled, Disabled)
                    .Attribute(m => m.PrefixIcon, PrefixIcon)
                    .Attribute(m => m.SuffixIcon, SuffixIcon)
                    .Attribute(m => m.Readonly, true)
                    .Attribute("placeholder", Placeholder)
                    .Close();
            })
            .Close()
            ;
    }

    /// <summary>
    /// 选中
    /// </summary>
    /// <param name="selectedItems">选中的时间。</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    async Task SelectItems(IReadOnlyList<DateTime> selectedItems)
    {
        if (selectedItems is null)
        {
            throw new ArgumentNullException(nameof(selectedItems));
        }

        if (!selectedItems.Any())
        {
            return;
        }

        var value = selectedItems[0];
        CurrentValueAsDateTime = value;

        SelectedDates.Clear();
        SelectedDates.Add(value);

        InputValue = CurrentValueAsDateTime.ToString(Format);
        await InputValueChanged.InvokeAsync(InputValue);


        Value = InputValue.To(value =>
        {
            value = value switch
            {
                DateOnly => DateOnly.FromDateTime(CurrentValueAsDateTime),
                DateTimeOffset => new DateTimeOffset(CurrentValueAsDateTime),
                _ => CurrentValueAsDateTime,
            };

            var nullableValueType = Nullable.GetUnderlyingType(typeof(TValue));
            if (nullableValueType is not null)
            {

                if (nullableValueType == typeof(DateOnly))
                {
                    return (TValue?)typeof(DateOnly)?.GetMethod(nameof(DateOnly.FromDateTime))?.Invoke(value, new object?[] { CurrentValueAsDateTime });
                }
                else if (nullableValueType == typeof(DateTimeOffset))
                {
                    return (TValue?)typeof(DateTimeOffset)?.GetMethod(nameof(DateTimeOffset.FromFileTime))?.Invoke(value, new object?[] { CurrentValueAsDateTime.ToFileTime() });
                }
            }
            return (TValue)value;
        });
        await ValueChanged.InvokeAsync(Value);
    }

    Task ChangeYearMonth(int year,int month)
    {
        CurrentYear = year;
        CurrentMonth = month;
        return this.Refresh();
    }

    bool IsToday(int day) => DateOnly.FromDateTime(DateTime.Today) == new DateOnly(CurrentYear, CurrentMonth, day);
    bool IsToday(DateTime date) => DateTime.Today == date;
}

/// <summary>
/// 日期选择器的视图模式。
/// </summary>
public enum DatePickerViewMode
{
    /// <summary>
    /// 年份视图。
    /// </summary>
    Year,
    /// <summary>
    /// 月份视图。
    /// </summary>
    Month,
    /// <summary>
    /// 日期视图。
    /// </summary>
    Date,
    /// <summary>
    /// 周视图。
    /// </summary>
    Week,
    /// <summary>
    /// 季度视图。
    /// </summary>
    Quater
}