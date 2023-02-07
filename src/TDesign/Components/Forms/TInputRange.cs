using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace TDesign;
/// <summary>
/// 范围输入框，用于输入范围文本。
/// </summary>
/// <typeparam name="TValue">值的类型。</typeparam>
[CssClass("t-range-input")]
public class TInputRange<TValue> : BlazorComponentBase
{

    /// <summary>
    /// 设置开始值。
    /// </summary>
    [Parameter] public TValue? StartValue { get; set; }

    /// <summary>
    /// 设置开始值的表达式。
    /// </summary>
    [Parameter] public Expression<Func<TValue?>>? StartValueExpression { get; set; }

    /// <summary>
    /// 设置开始值变化触发的回调。
    /// </summary>
    [Parameter] public EventCallback<TValue?> StartValueChanged { get; set; }


    /// <summary>
    /// 设置结束值。
    /// </summary>
    [Parameter] public TValue? EndValue { get; set; }

    /// <summary>
    /// 设置结束值的表达式。
    /// </summary>
    [Parameter] public Expression<Func<TValue?>>? EndValueExpression { get; set; }

    /// <summary>
    /// 设置结束值变化触发的回调。
    /// </summary>
    [Parameter] public EventCallback<TValue?> EndValueChanged { get; set; }

    /// <summary>
    /// 设置大小。
    /// </summary>
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;


    /// <summary>
    /// 设置分隔符文本。
    /// </summary>
    [Parameter] public string Seperator { get; set; } = "-";


    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", inner =>
        {
            BuildRangeInput(inner, 0, true, StartValue, StartValueExpression, StartValueChanged);
            BuildRnageSeperator(inner, 1);
            BuildRangeInput(inner, 3, false, EndValue, EndValueExpression, EndValueChanged);
        },
        new
        {
            @class = "t-range-input__inner"
        });
    }

    /// <summary>
    /// 构建分隔符。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> instance.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    void BuildRnageSeperator(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", Seperator, new { @class = "t-range-input__inner-separator" });
    }

    /// <summary>
    /// 构建输入框。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> instance.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="leftOrRight"><c>true</c>是左边，否则为右边。</param>
    /// <param name="value">值</param>
    /// <param name="expression">值的表达式</param>
    /// <param name="changed">改变事件。</param>
    void BuildRangeInput(RenderTreeBuilder builder, int sequence, bool leftOrRight, TValue? value, Expression<Func<TValue?>>? expression, EventCallback<TValue?> changed)
    {
        builder.CreateComponent<TInputText<TValue>>(sequence, attributes: new
        {
            Value = value,
            ValueExpression = expression,
            ValueChanged = changed,
            Size,
            AdditionalCssClass = $"t-range-input__inner-{(leftOrRight ? "left" : "right")}"
        });
    }
}
