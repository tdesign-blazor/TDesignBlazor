using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TDesignBlazor;

/// <summary>
/// 滑块（滑动型输入器），是帮助用户在连续或间断的区间内，通过滑动来选择合适数值（一个数值或范围数值）的控件。
/// </summary>
[CssClass("t-slider__container")]
public class Slider : BlazorComponentBase, IHasDisabled
{
    private double _currentClientX;
    private double _currentClientY;

    /// <summary>
    /// 设置滑块的最小值。
    /// </summary>
    [Parameter]public double Min { get; set; }
    /// <summary>
    /// 设置滑块的最大值。
    /// </summary>
    [Parameter] public double Max { get; set; } = 100;
    /// <summary>
    /// 是否垂直显示。
    /// </summary>
    [Parameter][CssClass("is-vertical")]public bool Vertical { get; set; }
    /// <summary>
    /// 设置滑块每次变动的步长偏移量。
    /// </summary>
    [Parameter] public double Step { get; set; }
    /// <inheritdoc/>
    [Parameter]public bool Disabled { get; set; }
    /// <summary>
    /// 设置显示进度条的刻度。
    /// <para>
    /// 至少设置最小刻度和最大刻度，即 0 和 100 的刻度值
    /// </para>
    /// <para>
    /// 代码示例：
    /// <code language="cs">
    /// new()
    /// {
    ///     [0]="0%",
    ///     [5]="5%",
    ///     ...
    ///     [90]="90%",
    ///     [100]="100%"
    /// }
    /// </code>
    /// </para>
    /// </summary>
    [Parameter] public Dictionary<double, OneOf<string?,RenderFragment?,MarkupString?>> Marks { get; set; } = new();
    /// <summary>
    /// 支持单值和双值。
    /// </summary>
    [Parameter] public OneOf<double, (double min, double max)> Value { get; set; } = new();
    /// <summary>
    /// 当值被改变执行的回调。
    /// </summary>
    [Parameter]public EventCallback<OneOf<double, (double min, double max)>>? ValueChanged { get; set; }
    /// <summary>
    /// 获取进度条的宽度百分比。
    /// </summary>
    double Percent
    {
        get
        {
            var percent = MaxValue - MinValue;
            if ( percent <= 0 )
            {
                return MinValue;
            }
            return percent;
        }
    }

    /// <summary>
    /// 表示最小值的百分比。
    /// </summary>
    double MinValue
    {
        get
        {
            if ( IsSingleNumer )
            {
                return 0;
            }
            return Value.AsT1.min;
        }
    }

    /// <summary>
    /// 表示最大值的百分比
    /// </summary>
    double MaxValue
    {
        get
        {
            if ( IsSingleNumer )
            {
                return Value.AsT0;
            }
            return Value.AsT1.max;
        }
    }

    /// <summary>
    /// 获取一个布尔值，表示 Value 只有一个数，MinValue 是 0，MaxValue 决定了左右的宽度。
    /// </summary>
    bool IsSingleNumer => Value.IsT0;


    /// <inheritdoc/>

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if ( Min < 0 || Max > 100 || Max < Min )
        {
            throw new InvalidOperationException($"{nameof(Min)} 不能小于 0 或 {nameof(Max)} 不能大于 100 并且 {nameof(Min)} 不能大于 {nameof(Max)}");
        }

        if ( IsSingleNumer )
        {
            var value= Value.AsT0;
            if(value<Min || value > Max )
            {
                throw new InvalidOperationException($"参数 {nameof(Value)} 的值({value})必须在{Min}-{Max}之间");
            }
        }
        else
        {
            var min = Value.AsT1.min;
            var max = Value.AsT1.max;
            if(min<Min || max > Max || min>max )
            {
                throw new InvalidOperationException($"参数 {nameof(Value)} 的值({min},{max})必须在{Min}-{Max}之间，并且第一个值不能小于{Min}，第二个值不能大于{Max}，第一个值不能大于第二个值");
            }
        }

        if ( Marks is null )
        {
            throw new InvalidOperationException($"参数 {nameof(Marks)} 是 null");
        }

        if ( Marks.Any() )
        {
            if ( !Marks.TryGetValue(Min,out _) )
            {
                throw new InvalidOperationException($"{nameof(Marks)} 的必须有一个最小值，且必须等于 {nameof(Min)} 参数的值");
            }

            if ( !Marks.TryGetValue(Max, out _) )
            {
                throw new InvalidOperationException($"{nameof(Marks)} 的必须有一个最大值，且必须等于 {nameof(Max)} 参数的值");
            }
        }
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildSlider(builder,sequence);

    }
    /// <inheritdoc/>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["aria-valuetext"] = GetValueString();
    }


    /// <summary>
    /// 构建 Slider 核心代码
    /// </summary>
    /// <param name="builder"></param>
    private void BuildSlider(RenderTreeBuilder builder,int sequence)
    {
        builder.CreateDiv(sequence, slider =>
        {
            slider.CreateDiv(0, rail =>
            {
                rail.CreateDiv(0, _ => { }, attributes: new
                {
                    @class = "t-slider__track",
                    style = HtmlHelper.CreateStyleBuilder()
                    .Append($"height:{Percent}%;bottom:{MinValue}%;", Vertical)
                    .Append($"width:{Percent}%;left:{MinValue}%;", !Vertical)
                });

                //if ( Vertical )
                //{
                //    BuildButtonWarpper(rail, 0, MaxValue);
                //    if ( !IsSingleNumer )
                //    {
                //        BuildButtonWarpper(rail, 1, MinValue);
                //    }
                //}
                //else
                //{
                    if ( !IsSingleNumer )
                    {
                        BuildButtonWarpper(rail, 0, MinValue);
                    }

                    BuildButtonWarpper(rail, 1, MaxValue);
                //}
                BuildMarks(rail, 2);
            },
            new
            {
                @class = HtmlHelper.CreateCssBuilder().Append("t-slider__rail")
                .Append("t-is-disabled", Disabled),
                style = HtmlHelper.CreateStyleBuilder().Append("height:100%", Vertical)
            });
        },
                new
                {
                    role = "slider",
                    aria_valuemin = Min,
                    aria_valuemax = Max,
                    aria_orientation = Vertical ? "vertical" : "horizontal",
                    @class = HtmlHelper.CreateCssBuilder().Append("t-slider").Append("is-vertical t-slider--vertical", Vertical).Append("t-is-disabled", Disabled),
                });
    }

    /// <summary>
    /// 获取 <see cref="Value"/> 的字符串形式。
    /// </summary>
    private string GetValueString()
    {
        return Value.Match(value => value.ToString(), value => $"{value.min}-{value.max}");
    }

    /// <summary>
    /// 构建拖拽的按钮。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    /// <param name="value">宽度百分比。</param>
    private void BuildButtonWarpper(RenderTreeBuilder builder,int sequence,double value)
    {
        builder.CreateDiv(sequence, content =>
        {
            content.CreateDiv(0, _ => { }, attributes: new
            {
                @class = "t-slider__button",
            });
        }, new
        {
            tabindex = 0,
            show_tooltip = "true",
            @class = "t-slider__button-wrapper",
            style = $"{(Vertical?"bottom":"left")}:{value}%;",
            disabled = Disabled,
            //按下鼠标
            onmousedown = HtmlHelper.CreateCallback<MouseEventArgs>(this, e =>
            {
                _currentClientX = e.ClientX;
                _currentClientY = e.ClientY;
            }),
            //按着移动
            onmousemove = HtmlHelper.CreateCallback<MouseEventArgs>(this, e =>
            {
            }),
            //释放定值
            onmouseup = HtmlHelper.CreateCallback<MouseEventArgs>(this, e =>
            {

            })
        });
    }

    /// <summary>
    /// 构建刻度。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    private void BuildMarks(RenderTreeBuilder builder,int sequence)
    {
        if ( !Marks.Any() )
        {
            return;
        }

        builder.CreateDiv(sequence, content =>
        {
            var marks = Vertical ? Marks.OrderByDescending(m => m.Key) : Marks.OrderBy(m => m.Key);

            content.CreateDiv(0, value =>
            {
                int index = 0;
                foreach ( var item in marks )
                {
                    value.CreateDiv(index,_=> { }, attributes: new
                    {
                        @class = "t-slider__stop t-slider__mark-stop",
                        style = HtmlHelper.CreateStyleBuilder().Append($"left:{item.Key}%;",!Vertical).Append($"top:calc({item.Key}% - 1px);",Vertical)
                    });
                    index++;
                }
            });

            content.CreateDiv(1, text =>
            {
                int index = 0;
                foreach ( var item in marks )
                {
                    text.CreateDiv(index, item.Value, new
                    {
                        @class = "t-slider__mark-text",
                        style = HtmlHelper.CreateStyleBuilder().Append($"left:{item.Key}%;", !Vertical).Append($"top:calc({item.Key}% - 1px);", Vertical)
                    });
                    index++;
                }
            }, 
            new
            {
                @class="t-slider__mark"
            });
        });
    }
}
