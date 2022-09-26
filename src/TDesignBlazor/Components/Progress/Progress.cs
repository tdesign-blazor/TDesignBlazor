using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;

/// <summary>
/// 渐变色
/// </summary>
public class LinearGradient
{
    /// <summary>
    /// 起始色
    /// </summary>
    public string? From { get; set; }

    /// <summary>
    /// 渐变色
    /// </summary>
    public string? To { get; set; }
}

/// <summary>
/// 进度条
/// </summary>
public class Progress : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// 背景色
    /// </summary>
    private string? _background;

    /// <summary>
    /// 圆环配置
    /// </summary>
    private Circle? _circle;

    /// <summary>
    /// 大小
    /// </summary>
    private int? _size;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 进度条颜色
    /// </summary>
    [Parameter] public OneOf<string, LinearGradient> Color { get; set; }

    /// <summary>
    /// 进度百分比
    /// </summary>
    [Parameter] public OneOf<string, bool> Label { get; set; } = true;

    /// <summary>
    /// 进度条百分比
    /// </summary>
    [Parameter] public int? Percentage { get; set; } = 0;

    /// <summary>
    /// 进度条尺寸
    /// </summary>
    [Parameter]
    public OneOf<int, Size> Size { get; set; } = TDesignBlazor.Size.Medium;

    /// <summary>
    /// 进度条状态
    /// </summary>
    [Parameter] public Status? Status { get; set; } = TDesignBlazor.Status.None;

    /// <summary>
    ///进度条线宽
    /// </summary>
    [Parameter] public string? StrokeWidth { get; set; }

    /// <summary>
    /// 进度条风格
    /// </summary>
    [Parameter] public ProgressThemeType? Theme { get; set; } = ProgressThemeType.Line;

    /// <summary>
    /// 进度条未完成部分颜色
    /// </summary>
    [Parameter] public string? TrackColor { get; set; }

    /// <summary>
    /// 添加内容
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        _background = GetBackGround();
        _size = GetSize();
        _circle = GetCircle();
        builder.CreateElement(sequence, "div", progress =>
        {
            switch (Theme)
            {
                case ProgressThemeType.Line:
                    BuildProgressLine(progress, 1);
                    break;

                case ProgressThemeType.Plump:
                    BuildProgressPlump(progress, 1);
                    break;

                case ProgressThemeType.Circle:
                    BuildProgressCircle(progress, 1);
                    break;
            }
        },
        new
        {
            @class = HtmlHelper.CreateCssBuilder()
                               .Append("t-progress").ToString()
        });
    }

    /// <summary>
    /// 圆环
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    private void BuildProgressCircle(RenderTreeBuilder builder, int sequence)
    {
        var circlePerimeter = GetCirclePerimeter((decimal)_circle.R);
        builder.CreateComponent<ProgressTheme>(sequence + 1, theme =>
        {
            theme.CreateComponent<ProgressInfo>(sequence + 2, $"{Percentage}%", attributes: new { Percentage, Status, Theme, Label });
            theme.CreateElement(sequence + 5, "svg", c =>
            {
                c.CreateElement(sequence + 4, "circle", d => { }, new
                {
                    cx = _circle.CX,
                    cy = _circle.CY,
                    r = _circle.R,
                    stroke_width = "6",
                    stroke = "",
                    fill = "none",
                    @class = "t-progress__circle-outer"
                });

                c.CreateElement(sequence + 5, "circle", d => { }, new
                {
                    cx = _circle.CX,
                    cy = _circle.CY,
                    r = _circle.R,
                    stroke_width = "6",
                    fill = "none",
                    stroke_linecap = "round",
                    transform = $"matrix(0,-1,1,0,0,{_size})",
                    stroke_dasharray = $"{(circlePerimeter * ((decimal)Percentage / 100M))}  {circlePerimeter + 1}",
                    @class = "t-progress__circle-inner"
                });
            }, new { width = $"{_size}", height = $"{_size}", viewBox = $"0 0 {_size} {_size}" });
        },
        attributes: new
        {
            @class = HtmlHelper.CreateCssBuilder()
                                           .Append(Theme.GetCssClass())
                                           .Append("t-progress--status--" + Status.GetCssClass()).ToString(),
            style = HtmlHelper.CreateStyleBuilder()
                                          .Append($"width:{_size}px")
                                          .Append($"height:{_size}px").ToString(),
            ChildContent = ChildContent
        });
    }

    /// <summary>
    /// 普通线状
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    private void BuildProgressLine(RenderTreeBuilder builder, int sequence)
    {
        var background = GetBackGround();
        builder.CreateComponent<ProgressTheme>(sequence + 1, theme =>
        {
            theme.CreateComponent<ProgressBar>(sequence + 1, bar =>
            {
                bar.CreateComponent<ProgressInner>(sequence + 1,
                    attributes: new
                    {
                        @style = HtmlHelper.CreateStyleBuilder()
                                           .Append($"width:{Percentage}%")
                                           .Append(_background).ToString()
                    });
            },
            attributes: new
            {
                Percentage = Percentage,
                ChildContent = ChildContent
            });

            theme.CreateComponent<ProgressInfo>(sequence + 2,
                attributes: new
                {
                    Percentage = Percentage,
                    Status = Status,
                    ChildContent = ChildContent
                });
        },
                    attributes: new
                    {
                        @class = HtmlHelper.CreateCssBuilder()
                                           .Append(Theme.GetCssClass())
                                           .Append("t-progress--status--" + Status.GetCssClass()).ToString(),
                        style = HtmlHelper.CreateStyleBuilder()
                                          .Append("width:100%").ToString(),
                        ChildContent = ChildContent
                    });
    }

    /// <summary>
    /// 加粗线状
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    private void BuildProgressPlump(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateComponent<ProgressBar>(sequence + 1, bar =>
        {
            bar.CreateComponent<ProgressInner>(sequence + 1, info =>
            {
                info.CreateComponent<ProgressInfo>(sequence + 2,
                    attributes: new
                    {
                        Percentage = Percentage,
                        Status = Status,
                        ChildContent = ChildContent
                    },
                    condition: Percentage > 10);
            },
            attributes: new
            {
                @style = HtmlHelper.CreateCssBuilder()
                                   .Append($"width:{Percentage}%")
                                   .Append(_background).ToString()
            });
            bar.CreateComponent<ProgressInfo>(sequence + 2,
                attributes: new
                {
                    Percentage = Percentage,
                    Status = Status,
                    ChildContent = ChildContent
                },
                condition: Percentage < 10 && Percentage > 0);
        },
                    attributes: new
                    {
                        @class = HtmlHelper.CreateCssBuilder()
                                           .Append("t-progress__bar")
                                           .Append(Theme.GetCssClass())
                                           .Append(" t-progress--status--" + Status.GetCssClass())
                                           .Append(Percentage < 10, "t-progress--under-ten", "t-progress--over-ten").ToString(),
                        Percentage = Percentage,
                        ChildContent = ChildContent
                    }); ;
    }

    /// <summary>
    /// 获取背景色
    /// </summary>
    /// <returns></returns>
    private string GetBackGround()
    {
        var background = Color.Match<string>(
              a =>
              {
                  if (a != null && !a.Equals(""))
                      return $"background:{a};";
                  else
                      return "";
              },
              b =>
              {
                  if (b is not null)
                  {
                      return $"background:linear-gradient(to right, {b.From}, {b.To});";
                  }
                  else
                      return "";
              });
        return background;
    }

    /// <summary>
    /// 获取svg圆形对象配置
    /// </summary>
    /// <returns></returns>
    private Circle GetCircle()
    {
        return Size.Match<Circle>(a =>
                                  {
                                      return new() { CX = a / 2, CY = a / 2, R = a / 2 - 3 };
                                  },
                                  b =>
                                  {
                                      return b switch
                                      {
                                          TDesignBlazor.Size.Small => new() { CX = 36, CY = 36, R = 33 },
                                          TDesignBlazor.Size.Medium => new() { CX = 56, CY = 56, R = 53 },
                                          TDesignBlazor.Size.Large => new() { CX = 80, CY = 80, R = 77 },
                                          _ => new() { CX = 56, CY = 56, R = 53 },
                                      };
                                  });
    }

    /// <summary>
    /// 获取圆的周长
    /// </summary>
    /// <param name="r">半径</param>
    /// <returns></returns>
    private decimal GetCirclePerimeter(decimal r)
    {
        decimal circumferenceRatio = 3.1415926m;
        return 2 * circumferenceRatio * r;
    }

    /// <summary>
    /// 获取大小
    /// </summary>
    /// <returns></returns>
    private int GetSize()
    {
        return Size.Match<int>(a => a,
                               b =>
                               {
                                   return b switch
                                   {
                                       TDesignBlazor.Size.Small => 72,
                                       TDesignBlazor.Size.Medium => 112,
                                       TDesignBlazor.Size.Large => 160,
                                       _ => 112,
                                   };
                               });
    }
}

/// <summary>
/// svg 圆型属性
/// </summary>
public class Circle
{
    /// <summary>
    /// 中心点坐标X
    /// </summary>
    public int? CX { get; set; }

    /// <summary>
    /// 中心点坐标Y
    /// </summary>
    public int? CY { get; set; }

    /// <summary>
    /// 半径
    /// </summary>
    public int? R { get; set; }
}
