namespace TDesign;

/// <summary>
/// 单选按钮的组容器，支持双向绑定。
/// </summary>
/// <typeparam name="TValue">值的类型。</typeparam>
[CssClass("t-radio-group")]
[ParentComponent(IsFixed = true)]
public class TInputRadioGroup<TValue> : TInputRadioContainer<TValue>,IHasDisabled
{
    /// <summary>
    /// 设置单选框的按钮风格。
    /// </summary>
    [ParameterApiDoc("单选框的按钮风格")]
    [Parameter][CssClass] public RadioButtonStyle? ButtonStyle { get; set; }
    /// <summary>
    /// 设置禁用状态。
    /// </summary>
    [ParameterApiDoc("禁用状态")]
    [Parameter] public bool Disabled { get; set; }
    /// <summary>
    /// 设置按钮的同一尺寸。
    /// </summary>
    [ParameterApiDoc("按钮的同一尺寸", Value = "Medium")]
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;
}

/// <summary>
/// 单选按钮的风格。
/// </summary>
public enum RadioButtonStyle
{
    /// <summary>
    /// 边框风格。
    /// </summary>
    [CssClass("t-radio-group__outline")]Outline,
    /// <summary>
    /// 白色填充风格。
    /// </summary>
    [CssClass("t-radio-group--filled")] Filled,
    /// <summary>
    /// 首选主题填充风格。
    /// </summary>
    [CssClass("t-radio-group--filled t-radio-group--primary-filled")] PrimaryFilled,
}