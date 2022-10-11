using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TDesignBlazor;

/// <summary>
/// 滑块（滑动型输入器），是帮助用户在连续或间断的区间内，通过滑动来选择合适数值（一个数值或范围数值）的控件。
/// </summary>
[CssClass("t-slider__container")]
public class Slider : BlazorComponentBase, IHasTwoWayBinding<OneOf<double, (double min,double max)>>
{
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
    [Parameter]public bool Vertical { get; set; }
    /// <inheritdoc/>
    [Parameter]public OneOf<double, (double min, double max)> Value { get; set; }
    /// <inheritdoc/>
    [Parameter]public Expression<Func<OneOf<double, (double min, double max)>>>? ValueExpression { get; set; }
    /// <inheritdoc/>
    [Parameter]public EventCallback<OneOf<double, (double min, double max)>>? ValueChanged { get; set; }


    double Width
    {
        get
        {
            if ( Value.IsT0 )
            {
                return Left1;
            }
            var width = Left2 - Left1;
            if ( width <= 0 )
            {
                return Left1;
            }
            return width;
        }
    }
    double Left1
    {
        get
        {
            if ( Value.IsT1 )
            {
                return Value.AsT1.min;
            }
            return 0;
        }
    }
    double Left2
    {
        get
        {
            if ( Value.IsT1 )
            {
                return Value.AsT1.max;
            }
            return Max;
        }
    }

    /// <inheritdoc/>

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if(Min<0|| Max>100 || Max < Min )
        {
            throw new InvalidOperationException($"{nameof(Min)} 不能小于 0 或 {nameof(Max)} 不能大于 100 并且 {nameof(Min)} 不能大于 {nameof(Max)}");
        }
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(0, "div", slider =>
        {
            slider.CreateElement(0, "div", rail =>
            {
                rail.CreateElement(0, "div", attributes: new
                {
                    @class = "t-slider__track",
                    style = HtmlHelper.CreateStyleBuilder().Append($"width:{Width}%").Append($"left:{Left1}%")
                });

                BuildButtonWarpper(rail, 0, Left1);
                if ( Value.IsT1 )
                {
                    BuildButtonWarpper(rail, 1, Left2);
                }

            }, new { @class = "t-slider__rail" });
        }, 
        new { 
            role="slider",
            aria_valuemin= Min,
            aria_valuemax= Max,
            aria_orientation= Vertical ? "vertical": "horizontal",
            @class = "t-slider",
        });
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["aria-valuetext"] = Value.Match(value => value.ToString(), value => $"{value.min}-{value.max}");
    }

    private void BuildButtonWarpper(RenderTreeBuilder builder,int sequence,double value)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "div", attributes: new { @class = "t-slider__button" });
        }, new
        {
            tabindex = 0,
            show_tooltip = "true",
            @class = "t-slider__button-wrapper",
            style = $"left:{value}%"
        });
    }
}
