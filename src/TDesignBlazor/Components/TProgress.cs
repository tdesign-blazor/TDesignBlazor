using System.Drawing;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;

/// <summary>
/// 展示操作的当前进度的进度条。
/// </summary>
[CssClass("t-progress")]
public class TProgress : BlazorComponentBase,IHasActive
{
    /// <summary>
    /// 设置是否显示进度条的百分比。<c>true</c> 则显示 <see cref="Value"/> 的百分比，否则，根据状态显示对应的图标。
    /// </summary>
    [Parameter] public bool ShowLabel { get; set; }

    /// <summary>
    /// 设置是否隐藏进度条的百分比。
    /// <c>true</c> 则不显示百分比和状态图标，即使 <see cref="ShowLabel"/> 已设置。
    /// </summary>
    [Parameter]public bool HideLabel { get; set; }
    /// <summary>
    /// 设置进度条的风格。
    /// </summary>
    [Parameter] public ProgressTheme Theme { get; set; } = ProgressTheme.Line;

    /// <summary>
    /// 设置进度条的状态。
    /// </summary>
    [Parameter] public Status? Status { get; set; }
    /// <summary>
    /// 设置进度条长度的百分比。
    /// </summary>
    [Parameter] public double Value { get; set; }
    /// <summary>
    /// 自定义显示标签的内容。否则显示 <see cref="Value"/> 的百分比。
    /// </summary>
    [Parameter]public string? Label { get; set; }
    /// <summary>
    /// 设置进度条具备渐变效果。
    /// </summary>
    [Parameter] public bool Active { get; set; }

    /// <summary>
    /// 进度条的大小。
    /// </summary>
    [Parameter] public Size Size { get; set; } = Size.Medium;

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (Value < 0 || Value > 100)
        {
            throw new ArgumentException($"{nameof(Value)} 的值必须在 0-100 之间，当前的值是 {Value}。");
        }
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        switch (Theme)
        {
            case ProgressTheme.Line:
                BuildLine(builder);
                break;
            case ProgressTheme.Plump:
                BuildPlump(builder);
                break;
            case ProgressTheme.Circle:
                BuildCircle(builder);
                break;
        }
    }

    /// <summary>
    /// 根据 <see cref="Size"/> 获取 style 的宽高。
    /// </summary>
    private (int? size, int? fontSize) GetSizeStyle()
    => Size switch
    {
        TDesignBlazor.Size.Small => (72, 14),
        TDesignBlazor.Size.Medium => (112, 20),
        TDesignBlazor.Size.Large => (160, 36),
        _ => (default, default),
    };

    /// <summary>
    /// 构造 <see cref="ProgressTheme.Line"/> 的 HTML 代码。
    /// </summary>
    private void BuildLine(RenderTreeBuilder builder)
    {
        builder.CreateDiv(0, (RenderFragment)(content =>
        {
            content.CreateElement(0, "div", bar =>
            {
                BuildProgressInner(bar);
            }, new { @class = "t-progress__bar" });

            BuildProgressInfo(content, 1);
        }), 
        new
        {
            @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-progress--thin")
                                .Append(Status is null, "t-progress--status--undefined", $"t-progress--status--{Status?.GetCssClass()}")
                                .Append("t-progress--status--active", Active)
        });
    }

    /// <summary>
    /// 构造 <see cref="ProgressTheme.Plump"/> 的 HTML 代码。
    /// </summary>
    private void BuildPlump(RenderTreeBuilder builder)
    {
        builder.CreateElement(0, "div", bar =>
        {
            if (Value > 10)
            {
                BuildProgressInner(bar,0, info =>
                {
                    BuildProgressInfo(info, 0);
                });
            }
            else
            {
                BuildProgressInner(bar);
                BuildProgressInfo(bar, 1);
            }
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-progress__bar")
                                .Append("t-progress--plump")
                                .Append(Status is null, "t-progress--status--undefined", $"t-progress--status--{Status?.GetCssClass()}")
                                .Append("t-progress--status--active", Active)
                                .Append(Value <= 10, "t-progress--under-ten", "t-progress--over-ten")
        });
    }

    private void BuildCircle(RenderTreeBuilder builder)
    {
        (int? size, int? fontSize) = GetSizeStyle();
        builder.CreateElement(0, "div", circle =>
        {
            BuildProgressInfo(circle,0,true);
            circle.CreateElement(1, "svg", svg =>
            {
                var param = GetCircleParameter();
                var array = GetDashData(param.r);
                var size = GetSizeStyle();
                svg.CreateElement(0, "circle", attributes: new
                {
                    param.cx, 
                    param.cy,
                    param.r,
                    param.stroke_width,
                    fill="none",
                    @class= "t-progress__circle-outer"
                });

                svg.CreateElement(0, "circle", attributes: new
                {
                    param.cx,
                    param.cy,
                    param.r,
                    param.stroke_width,
                    fill = "none",
                    stroke_linecap="round",
                    transform=$"matrix(0,-1,1,0,0,{size.size})",
                    stroke_dasharray=$"{array.d1} {array.d2}",
                    @class = "t-progress__circle-inner"
                });
            }, new
            {
                width=size,
                height=size,
                viewBox=string.Format("0 0 {0} {0}",size)
            });
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-progress--circle")
                                .Append(Status is null, "t-progress--status--undefined", $"t-progress--status--{Status?.GetCssClass()}")
                                .Append("t-progress--status--active", Active),
                                style=HtmlHelper.CreateStyleBuilder()
                                                .Append($"width:{size}px",size.HasValue)
                                                .Append($"height:{size}px", size.HasValue)
                                                .Append($"font-size:{fontSize}px",fontSize.HasValue)
        });

        (int cx, int cy, int r,int stroke_width) GetCircleParameter()
        => Size switch
        {
            Size.Small => (36, 36, 34,4),
            Size.Medium => (56, 56, 53,6),
            Size.Large => (80, 80, 77,6)
        };

        // 返回 stroke_dasharray 属性
        (double d1,double d2) GetDashData(double radius)
        {
            var circlePerimeter = 2 * Math.PI * radius;

            return (circlePerimeter * (Value / 100D), circlePerimeter + 1);
        }
    }

    /// <summary>
    /// 构建 <c>&lt;div class="t-progress__inner">xx&lt;/div></c> 这段代码
    /// </summary>
    /// <param name="content"></param>
    /// <param name="sequence"></param>
    /// <param name="childContent"></param>
    /// <param name="additionalStyle"></param>
    private void BuildProgressInner( RenderTreeBuilder content, int sequence=0, RenderFragment? childContent = default, string? additionalStyle = default)
    {
        content.CreateElement(sequence, "div", childContent,
                        new
                        {
                            @class = "t-progress__inner",
                            style = HtmlHelper.CreateStyleBuilder().Append($"width:{Value}%").Append(additionalStyle ??= string.Empty),
                        });
    }

    /// <summary>
    /// 构建 <c>&lt;div class="t-progress__info">xx&lt;/div></c> 这段代码
    /// </summary>
    private void BuildProgressInfo(RenderTreeBuilder builder, int sequence,bool circle=false)
    {
        builder.CreateElement(sequence, "div", content =>
            {
                if (HideLabel)
                {
                    return;
                }

                if (ShowLabel)
                {
                    content.AddContent(0, $"{GetLabel()}");
                }
                else if (Status is not null)
                {
                    content.CreateComponent<TIcon>(0, attributes: new
                    {
                        Name = Status?.GetStatusTIconName(!circle?default: s => s switch
                        {
                            TDesignBlazor.Status.Success => IconName.Check,
                            TDesignBlazor.Status.Error => IconName.Close,
                            TDesignBlazor.Status.Warning => IconName.Error,
                            _ => default
                        }),
                        AdditionalCssClass = "t-progress__icon"
                    });
                }
            }, new { @class = "t-progress__info" });

        string? GetLabel()
        {
            return Label ?? $"{Value}%";
        }
    }
}

/// <summary>
/// 进度条的类型。
/// </summary>
public enum ProgressTheme
{
    /// <summary>
    /// 瘦小的线，很细的线。
    /// </summary>
    Line,
    /// <summary>
    /// 丰满的线，比较粗的线。
    /// </summary>
    Plump,
    /// <summary>
    /// 圆形。像仪表盘一样。
    /// </summary>
    Circle
}