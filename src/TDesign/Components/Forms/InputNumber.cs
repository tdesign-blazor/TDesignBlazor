using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http;

using ComponentBuilder;

using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace TDesign;
/// <summary>
/// 数字输入框由增加、减少按钮、数值输入组成。每次点击增加按钮（或减少按钮），数字增长（或减少）的量是恒定的。
/// <para>
/// 支持数字类型：<see cref="int"/>、<see cref="short"/>、<see cref="long"/>、<see cref="decimal"/>、<see cref="double"/>、<see cref="float"/>
/// </para>
/// </summary>
/// <typeparam name="TValue">数字类型。</typeparam>
[CssClass("t-input-number")]
public class TInputNumber<TValue> : BlazorComponentBase, IHasTwoWayBinding<TValue>
{

    readonly static Type[] SupportTypes = new[] { typeof(int), typeof(short), typeof(long), typeof(decimal), typeof(double), typeof(float) };
    public TInputNumber()
    {
        if (!SupportTypes.Contains(typeof(TValue)))
        {
            throw new NotSupportedException($"{nameof(TValue)} 仅支持 short, int, long, decimal, double 和 float 类型。");
        }
    }
    /// <summary>
    /// 大小
    /// </summary>
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 值，仅支持 short, int, long, decimal, double 和 float 类型。
    /// </summary>
    [Parameter] public TValue? Value { get; set; }
    [Parameter] public Expression<Func<TValue?>>? ValueExpression { get; set; }
    [Parameter] public EventCallback<TValue?>? ValueChanged { get; set; }
    /// <summary>
    /// 占位符
    /// </summary>
    [Parameter] public string? Placeholder { get; set; }
    /// <summary>
    /// 对齐方式
    /// </summary>
    [Parameter] public HorizontalAlignment Align { get; set; } = HorizontalAlignment.Center;
    /// <summary>
    /// 步救
    /// </summary>
    [Parameter] public TValue? Step { get; set; } = (TValue)Convert.ChangeType(1, typeof(TValue));
    /// <summary>
    /// 最大值，
    /// </summary>
    [Parameter] public TValue? Max { get; set; } = (TValue)Convert.ChangeType(int.MaxValue, typeof(TValue));
    /// <summary>
    /// 类型
    /// </summary>
    [Parameter][CssClass("t-input-number--")] public InputNumberTheme Theme { get; set; } = InputNumberTheme.Row;
    /// <summary>
    /// 自适应宽度
    /// </summary>
    [Parameter][CssClass("t-input-number--auto-width")] public bool AutoWidth { get; set; }
    /// <summary>
    /// 状态。
    /// </summary>
    [Parameter][CssClass("t-is-")] public Status Status { get; set; } = Status.Default;
    /// <summary>
    /// 输入框后缀显示的文本。
    /// </summary>
    [Parameter] public string? SuffixText { get; set; }
    /// <summary>
    /// 标签
    /// </summary>
    [Parameter] public string? Label { get; set; }
    /// <summary>
    /// tipcontent标签
    /// </summary>
    [Parameter] public RenderFragment? TipContent1 { get; set; }
    /// <summary>
    /// inputnumber tips
    /// </summary>
    [Parameter] public string? Tips { get; set; }
    /// <summary>
    /// 只读状态
    /// </summary>
    [Parameter] public bool Readonly { get; set; }
    /// <summary>
    /// 禁用状态
    /// </summary>
    [Parameter] public bool Disabled { get; set; }
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {

        dynamic _value = Value;
        dynamic _step = Step;
        dynamic _max = Max;
        var _disabled = Max != null ? (bool)(_value >= _max) : false;

        BuildButton(builder, sequence + 1, IconName.Remove, Disabled, Theme != InputNumberTheme.Normal, a =>
        {
            Value = (TValue)(_value - _step);
        });

        builder.CreateComponent<TInputText<TValue>>(sequence + 2, attributes: new
        {
            Value,
            ValueExpression,
            ValueChanged,
            Size,
            Placeholder,
            Alignment = Align,
            Status = _disabled ? Status.Error : Status,
            SuffixText,
            PrefixText = Label,
            oninput = "OnInput",
            onkeydown = HtmlHelper.CreateCallback<KeyboardEventArgs>(this, e =>
            {
                if (e.Key == "ArrowUp" && !_disabled)
                    Value = (TValue)(_value + _step);
                else if (e.Key == "ArrowDown")
                    Value = (TValue)(_value - _step);

            }),
            onfocus = "@OnFocusAsync",
            onblur = "@OnBlurAsync",
            AutoWidth,
            Readonly,
            Disabled
            //TipContent=TipContent1
        });

        BuildButton(builder, sequence + 3, IconName.Add, _disabled||Disabled, Theme != InputNumberTheme.Normal, a =>
        {
            Value = (TValue)(_value + _step);
        });
        builder.CreateElement(sequence + 4, "div", Tips, new { @class = $"t-input__tips t-input__tips--{Status.GetCssClass()}" }, Tips != null && TipContent1 is null);

        //
        //this.Refresh();
    }

    private void BuildButton(RenderTreeBuilder builder, int sequence, object iconName, bool disabled, bool condition, Action<MouseEventArgs>? click = default)
    {
        builder.CreateComponent<TButton>(sequence + 1, content =>
        {
            content.CreateComponent<TIcon>(0, attributes: new
            {
                Name = iconName,
            });
        },
        new
        {
            Varient = ButtonVarient.Outline,
            Shape = ButtonShape.Square,
            @Class = HtmlHelper.CreateCssBuilder()
            .Append($"t-input-number__decrease", iconName.ToString() == IconName.Remove.ToString())
            .Append($"t-input-number__increase", iconName.ToString() == IconName.Add.ToString())
            .Append($"t-is-disabled", iconName.ToString() == IconName.Remove.ToString() && disabled)
            .Append($"t-is-disabled", iconName.ToString() == IconName.Add.ToString() && disabled),
            Onclick = HtmlHelper.CreateCallback<MouseEventArgs>(this, e => click?.Invoke(e)),
            Disabled = disabled,

        }, condition);
    }


}

public enum InputNumberTheme
{
    /// <summary>
    /// 按钮横向排列
    /// </summary>
    Row,
    /// <summary>
    /// 按钮行内纵向排列
    /// </summary>
    [CssClass("column t-is-controls-right ")] Column,
    /// <summary>
    /// 没有按钮，通过上下键控制
    /// </summary>
    Normal
}