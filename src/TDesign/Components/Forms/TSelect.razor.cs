namespace TDesign;

/// <summary>
/// 用于收纳大量选项的信息录入类组件。
/// </summary>
/// <typeparam name="TValue">值的类型。</typeparam>
[ParentComponent]
public partial class TSelect<TValue>
{
    /// <summary>
    /// 自定义选项内容。
    /// </summary>
    [ParameterApiDoc("自定义选项内容")]
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 设置当选项被选中后的回调。
    /// </summary>
    [ParameterApiDoc("当选项被选中后的回调", Type= "EventCallback<TValue>")]
    [Parameter] public EventCallback<TValue> OnValueSelected { get; set; }
    /// <summary>
    /// 无边框样式。
    /// </summary>
    [ParameterApiDoc("无边框样式")]
    [Parameter][CssClass("t-select-input--borderless")] public bool Borderless { get; set; }

    /// <summary>
    /// 文本框占位字符串。
    /// </summary>
    [ParameterApiDoc("占位符",Value="请选择")]
    [Parameter] public string? Placeholder { get; set; } = "请选择";

    /// <summary>
    /// 文本框的额外 class。
    /// </summary>
    [ParameterApiDoc("文本框的额外 class")]
    [Parameter]public string? InputClass { get; set; }
    /// <summary>
    /// 文本框的额外 style。
    /// </summary>
    [ParameterApiDoc("文本框的额外 style")]
    [Parameter] public string? InputStyle { get; set; }
    /// <summary>
    /// Popup 组件的引用。
    /// </summary>
    internal TPopup? RefPopup { get; set; }

    /// <summary>
    /// 选中的标签的值。
    /// </summary>
    internal string? SelectedLabel { get; set; }

    bool _initValue;

    protected override void OnAfterRender(bool firstRender)
    {
        if ( firstRender )
        {
            if ( Value is not null && !_initValue )
            {
                var initialSelectedOption = ChildComponents.OfType<TSelectOption<TValue>>().FirstOrDefault(m => Value.Equals(m.Value));

                if ( initialSelectedOption != null )
                {
                    Value = initialSelectedOption.Value;
                    SelectedLabel = initialSelectedOption.Label;
                    _initValue = true;
                    StateHasChanged();
                }
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

    }


    /// <summary>
    /// 选择指定的值。
    /// </summary>
    /// <param name="value">选中的值。</param>
    /// <param name="label">选中的文本。</param>
    public async Task SelectValue(TValue? value, string? label)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
        await OnValueSelected.InvokeAsync(value);
        SelectedLabel = label;
        StateHasChanged();
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-select t-select-input");
    }
}


/// <summary>
/// 表示作为 <see cref="TSelect{TValue}"/> 的选项组件。
/// </summary>
/// <typeparam name="TValue">值的类型。</typeparam>
[ChildComponent(typeof(TSelect<>))]
[CascadingTypeParameter("TValue")]
[HtmlTag("li")]
[CssClass("t-select-option")]
public class TSelectOption<TValue> : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// 级联 <see cref="TSelect{TValue}"/> 组件。
    /// </summary>
    [CascadingParameter] public TSelect<TValue> CascadingSelect { get; set; }
    /// <summary>
    /// 选项的值。
    /// </summary>
    [ParameterApiDoc("选项的值")]
    [Parameter] public TValue? Value { get; set; }
    /// <summary>
    /// 选项的文本。
    /// </summary>
    [ParameterApiDoc("选项的文本")]
    [Parameter] public string? Label { get; set; }

    /// <summary>
    /// 禁用选项。
    /// </summary>
    [ParameterApiDoc("禁用选项")]
    [Parameter][CssClass("t-is-disabled")] public bool Disabled { get; set; }
    /// <summary>
    /// 自定义选项的内容。
    /// </summary>
    [ParameterApiDoc("自定义选项的内容")]
    [Parameter] public RenderFragment? ChildContent { get; set; }    

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        ChildContent ??= builder => builder.AddContent(0, Label);
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "span", ChildContent);
    }

    /// <inheritdoc/>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if (builder.Contains("t-is-selected"))
        {
            builder.Remove("t-is-selected");
        }
        builder.Append("t-is-selected", CascadingSelect.Value is not null && CascadingSelect.Value.Equals(Value));

        builder.Append(CascadingSelect.Size.GetCssClass());
    }

    /// <inheritdoc/>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["onclick"] = HtmlHelper.Instance.Callback().Create(this, async () =>
        {
            await CascadingSelect.SelectValue(Value, Label);
            if ( CascadingSelect.RefPopup is not null )
            {
                await CascadingSelect.RefPopup.Hide();
            }
        }, !Disabled);
    }
}