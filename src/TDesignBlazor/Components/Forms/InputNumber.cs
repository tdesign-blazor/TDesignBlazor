using System.Linq.Expressions;








using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;
/// <summary>
/// 数字输入框由增加、减少按钮、数值输入组成。每次点击增加按钮（或减少按钮），数字增长（或减少）的量是恒定的。
/// <para>
/// 支持数字类型：<see cref="int"/>、<see cref="short"/>、<see cref="long"/>、<see cref="decimal"/>、<see cref="double"/>、<see cref="float"/>
/// </para>
/// </summary>
/// <typeparam name="TValue">数字类型。</typeparam>
[CssClass("t-input-number")]
public class InputNumber<TValue> : BlazorComponentBase, IHasTwoWayBinding<TValue>
{
    readonly static Type[] SupportTypes = new[] { typeof(int), typeof(short), typeof(long), typeof(decimal), typeof(double), typeof(float) };
    public InputNumber()
    {
        if (!SupportTypes.Contains(typeof(TValue)))
        {
            throw new NotSupportedException($"{nameof(TValue)} 仅支持 short, int, long, decimal, double 和 float 类型。");
        }
    }

    [Parameter] public Size Size { get; set; } = Size.Medium;
    [Parameter] public TValue? Value { get; set; }
    [Parameter] public Expression<Func<TValue?>>? ValueExpression { get; set; }
    [Parameter] public EventCallback<TValue?>? ValueChanged { get; set; }
    [Parameter] public string? Placeholder { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildButton(builder, sequence + 1, IconName.Remove);
        builder.CreateComponent<Input<TValue>>(sequence + 2, attributes: new
        {
            Value,
            ValueExpression,
            ValueChanged,
            Size,
            Placeholder
        });
        BuildButton(builder, sequence + 3, IconName.Add);
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-input-number--row");
    }

    private void BuildButton(RenderTreeBuilder builder, int sequence, object iconName, Action<MouseEventArgs>? click = default)
    {
        builder.CreateComponent<Button>(sequence + 1, content =>
        {
            content.CreateComponent<Icon>(0, attributes: new
            {
                Name = iconName,
                onclick = HtmlHelper.CreateCallback<MouseEventArgs>(this, e => click?.Invoke(e))
            });
        },
        new
        {
            Type = ButtonType.Outline,
            Shape = ButtonShape.Squre
        });
    }
}

