namespace TDesign;

/// <summary>
/// 日期选择器，用于选择某一具体日期或某一段日期区间。
/// </summary>
public partial class TDatePicker<TValue> : TDesignInputComonentBase<TValue>
{
    readonly Type[] Support_Types = new[] { typeof(DateOnly), typeof(DateTime), typeof(DateTimeOffset?), typeof(DateOnly?), typeof(DateTime?), typeof(DateTimeOffset?) };
    /// <summary>
    /// 前缀图标，默认是 <see cref="IconName.Calendar"/> 。
    /// </summary>
    [ParameterApiDoc("输入框的前缀图标", Value = "IconName.Calendar")]
    [Parameter] public object? PrefixIcon { get; set; } = IconName.Calendar;
    /// <summary>
    /// 后缀图标。
    /// </summary>
    [ParameterApiDoc("输入框的后缀图标")]
    [Parameter] public object? SuffixIcon { get; set; }

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


    string? InputValue { get; set; }
    EventCallback<string?> InputValueChanged { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if ( !Support_Types.Contains(typeof(TValue)) )
        {
            throw new ArgumentException($"仅支持 {nameof(DateOnly)}、{nameof(DateTime)}、{nameof(DateTimeOffset)} 的类型");
        }
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Component<TPopup>()
            .Attribute(m => m.Trigger, PopupTrigger.Click)
            .Attribute(m => m.PopupContent, content =>
            {
                content.Component<TCalendar>()
                        .Attribute(m => m.Card, true)
                        .Attribute(m => m.OnDateSelected, HtmlHelper.Instance.Callback().Create<IReadOnlyList<DateOnly>>(this, ChangeDate))
                        .Close();
            })
            .Attribute(m => m.ChildContent, input =>
            {
                input.Component<TInputText<string>>()
                    .Attribute(m => m.Value, InputValue)
                    .Attribute(m => m.ValueExpression,()=>Value.ToString())
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

    async Task ChangeDate(IReadOnlyList<DateOnly> selectedDates)
    {
        if ( selectedDates is null )
        {
            throw new ArgumentNullException(nameof(selectedDates));
        }

        if ( !selectedDates.Any() )
        {
            return;
        }

        var value = selectedDates.First();
        Value = value.To<TValue>();
        await ValueChanged.InvokeAsync(Value);

        InputValue = value.ToString(Format);
        await InputValueChanged.InvokeAsync(InputValue);
    }
}
