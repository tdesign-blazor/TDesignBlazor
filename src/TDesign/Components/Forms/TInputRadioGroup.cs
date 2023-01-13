namespace TDesign;

/// <summary>
/// 单选按钮的组容器，支持双向绑定。
/// </summary>
/// <typeparam name="TValue">值的类型。</typeparam>
[CssClass("t-radio-group t-radio-group__outline")]
[ParentComponent(IsFixed = true)]
public class TInputRadioGroup<TValue> : TDesignInputComonentBase<TValue>, IHasChildContent, IHasDisabled
{
    /// <summary>
    /// 分组名称。
    /// </summary>
    [Parameter] public string Name { get; set; } = $"radio_group_{Guid.NewGuid()}";
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 当 <see cref="TButton"/> 是 <c>true</c> 时的按钮风格。
    /// </summary>
    [Parameter] public RadioButtonStyle? ButtonStyle { get; set; }

    /// <summary>
    ///  执行当 <see cref="TInputRadio{TValue}"/> 触发的事件。
    /// </summary>
    [Parameter] public EventCallback<string> OnValueSelected { get; set; }

    /// <summary>
    /// 内部事件，当 <see cref="TInputRadio{TValue}"/> 组件被点击发生改变时触发。
    /// </summary>
    internal EventCallback<ChangeEventArgs> ChangeEventCallback { get; set; }

    /// <summary>
    /// 设置按钮风格的单选框。
    /// </summary>
    internal bool TButton => ButtonStyle.HasValue;

    /// <summary>
    /// Gets the selected value.
    /// </summary>
    internal TValue? SelectedValue => this.Value;

    internal event Action RerenderRadioBoxes;
    string _oldValue = "";

    /// <summary>
    /// Method invoked when the component has received parameters from its parent in
    /// the render tree, and the incoming values have been assigned to properties.
    /// </summary>
    protected override void OnParametersSet()
    {
        var newValue = this.FormatValueAsString();
        ChangeEventCallback = EventCallback.Factory.CreateBinder<string?>(this, __value =>
        {
            this.GetCurrentValueAsString(__value);
            _ = OnValueSelected.InvokeAsync(__value);
        }
        , newValue);

        if (_oldValue != newValue)
        {
            _oldValue = newValue;   
            RerenderRadioBoxes?.Invoke();
        }
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if (ButtonStyle.HasValue)
        {
            builder.Append(ButtonStyle.GetCssClass(), TButton);
        }
    }
}

/// <summary>
/// 单选按钮的风格。
/// </summary>
public enum RadioButtonStyle
{
    /// <summary>
    /// 边框风格。
    /// </summary>
    Outline,
    /// <summary>
    /// 白色填充风格。
    /// </summary>
    [CssClass("t-radio-group--filled")] Filled,
    /// <summary>
    /// 首选主题填充风格。
    /// </summary>
    [CssClass("t-radio-group--filled t-radio-group--primary-filled")] PrimaryFilled,
}