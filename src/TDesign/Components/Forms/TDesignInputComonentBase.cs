using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace TDesign;

/// <summary>
/// 输入控件的基类。
/// </summary>
/// <typeparam name="TValue">双向绑定值的类型。</typeparam>
public abstract class TDesignInputComonentBase<TValue> : TDesignAdditionParameterComponentBase,IHasInputValue<TValue?>
{
    /// <summary>
    /// 设置只读模式。
    /// </summary>
    [ParameterApiDoc("只读模式")]
    [Parameter][HtmlAttribute] public bool Readonly { get; set; }
    /// <summary>
    /// 设置禁用状态。
    /// </summary>
    [ParameterApiDoc("禁用状态")]
    [Parameter][HtmlAttribute] public bool Disabled { get; set; }
    /// <summary>
    /// 自适应宽度。
    /// </summary>
    [ParameterApiDoc("自适应宽度")]
    [Parameter] public bool AutoWidth { get; set; }
    /// <summary>
    /// 尺寸。
    /// </summary>
    [ParameterApiDoc("尺寸",Value= "Medium")]
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 自动聚焦。
    /// </summary>
    [ParameterApiDoc("自动聚焦")]
    [Parameter][HtmlAttribute] public bool AutoFocus { get; set; }
    /// <summary>
    /// 状态。
    /// </summary>
    [ParameterApiDoc("状态", Value ="Default")]
    [Parameter][CssClass("t-is-")] public Status Status { get; set; } = Status.Default;
    /// <summary>
    /// 输入框提示的内容。
    /// </summary>
    [ParameterApiDoc("提示的内容")]
    [Parameter] public RenderFragment? TipContent { get; set; }
    /// <summary>
    /// 对齐方式。
    /// </summary>
    [ParameterApiDoc("对齐方式")]
    [Parameter] public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Left;

    /// <inheritdoc/>
    [ParameterApiDoc("要绑定的值", Type="TValue?")]
    [Parameter]public TValue? Value { get; set; }
    /// <inheritdoc/>
    [ParameterApiDoc("更新绑定值的回调方法", Type = "EventCallback<TValue?>")]
    [Parameter]public EventCallback<TValue?> ValueChanged { get; set; }
    /// <inheritdoc/>
    [ParameterApiDoc("识别绑定值的表达式", Type = "Expression<Func<TValue?>>?")]
    [Parameter] public Expression<Func<TValue?>>? ValueExpression { get; set; }
    /// <inheritdoc/>
    [CascadingParameter] public EditContext? CascadedEditContext { get; set; }

    /// <summary>
    /// 获取触发数据绑定的事件名称。
    /// </summary>
    protected virtual string EventName => "oninput";
    
    protected override void AfterSetParameters(ParameterView parameters)
    {
        this.InitializeInputValue();
    }

    /// <summary>
    /// Builds the input wrapper.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="sequence">The sequence.</param>
    /// <param name="content">The content.</param>
    /// <param name="inputClass">The other css.</param>
    protected virtual void BuildInputWrapper(RenderTreeBuilder builder, int sequence, RenderFragment content, string? inputClass = default,string? wrapClass=default)
    {
        builder.CreateElement(sequence, "div", wrap =>
        {
            wrap.CreateElement(0, "div", content, new
            {
                @class = HtmlHelper.Instance.Class().Append("t-input")
                            .Append("t-is-readonly", Readonly)
                            .Append("t-is-disabled", Disabled)
                            .Append("t-input--auto-width", AutoWidth)
                            .Append("t-is-focused t-input--focused", AutoFocus)
                            .Append($"t-is-{Status.GetCssClass()}")
                            .Append($"t-align-{Alignment.GetCssClass()}")
                            .Append(Size.GetCssClass())
                            .Append(inputClass, !string.IsNullOrEmpty(inputClass))
            });

            wrap.CreateElement(1, "div", TipContent, new
            {
                @class = HtmlHelper.Instance.Class().Append("t-input__tips")
                .Append($"t-input__tips--{Status.GetCssClass()}")
            }, TipContent is not null);
        }, new
        {
            @class = HtmlHelper.Instance.Class().Append("t-input__wrap").Append(wrapClass)
        });
    }

    /// <inheritdoc/>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        base.BuildAttributes(attributes);
        BuildEventAttribute(attributes);
    }

    protected virtual void BuildEventAttribute(IDictionary<string,object> attributes)
    {
        attributes[EventName] = HtmlHelper.Instance.Callback().CreateBinder(this, _value =>
        {
            this.ChangeValue(_value);
        }, Value);
    }

    /// <inheritdoc/>
    protected override void DisposeComponentResources()
    {
        this.DisposeInputValue();
    }


}
