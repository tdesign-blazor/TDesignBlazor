using ComponentBuilder;

using Microsoft.AspNetCore.Components.Rendering;

using System.Reflection.PortableExecutable;
using System.Security.Claims;

namespace TDesignBlazor;

public class TProgress : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 进度条颜色。示例：'#ED7B2F' 或 'orange' 或 ['#f00', '#0ff', '#f0f'] 或 { '0%': '#f00', '100%': '#0ff' } 或 { from: '#000', to: '#000' } 等。
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
    [Parameter] public OneOf<int, Size> Size { get; set; } = 112;
    /// <summary>
    /// 进度条状态
    /// </summary>
    [Parameter][CssClass("t-progress--status--")] public Status? Status { get; set; } = TDesignBlazor.Status.Default;// [
    /// <summary>
    ///进度条线宽。宽度数值不能超过 size 的一半，否则不能输出环形进度
    /// </summary>
    [Parameter] public string? StrokeWidth { get; set; }
    /// <summary>
    /// 进度条风格。值为 line，标签（label）显示在进度条右侧；值为 plump，标签（label）显示在进度条里面；值为 circle，标签（label）显示在进度条正中间。可选项：line/plump/circle。
    /// </summary>
    [Parameter] public ProgressTheme? Theme { get; set; } = ProgressTheme.Line;
    /// <summary>
    /// 进度条未完成部分颜色
    /// </summary>
    [Parameter] public string? TrackColor { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {

        var background = GetBackGround();
        var progressTypeClass = Percentage < 10 && Percentage > 0 ? "t-progress--under-ten" : "t-progress--over-ten";

        var icon = GetIcon();
        var isLable = GetIsLabel();
        var lableText = GetLable();
        var size = GetSize();
        var circle = GetCircle();

        builder.CreateElement(sequence, "div", a =>
        {
            switch (Theme)
            {
                case ProgressTheme.Line:
                    a.CreateElement(sequence + 1, "div", b =>
                    {
                        b.CreateElement(sequence + 2, "div", c =>
                        {
                            c.CreateElement(sequence + 3, "div", c => { }, new { @class = $"t-progress__inner {Theme.GetCssClass} {Status.GetCssClass}", style = $"width: {Percentage}%;{background}" });
                        },
                        new { @class = $"t-progress__bar {progressTypeClass}", style = "width:720px" });

                        b.CreateElement(sequence + 4, "div", Percentage.ToString() + "%", new { @class = "t-progress__info" }, Status == TDesignBlazor.Status.Default || Status == TDesignBlazor.Status.None);
                        b.CreateElement(sequence + 5, "div", c =>
                        {
                            c.CreateComponent<Icon>(0, attributes: new { Class = $"t-icon t-icon-{icon}-circle-filled t-progress__icon" });
                        }, new { @class = "t-progress__info" }, Status != TDesignBlazor.Status.Default && Status != TDesignBlazor.Status.None);
                    },
                    new { @class = $"t-progress--thin {Status.GetCssClass} " });
                    break;
                case ProgressTheme.Plump:
                    a.CreateElement(sequence + 1, "div", b =>
                    {
                        b.CreateElement(sequence + 2, "div", c =>
                        {
                            c.CreateElement(sequence + 3, "div", Percentage.ToString() + "%",
                                new { @class = "t-progress__info" }, Percentage > 10);
                        },
                        new { @class = "t-progress__inner", style = $"width: {Percentage}%;" });

                        b.CreateElement(sequence + 3, "div", Percentage.ToString() + "%", new { @class = "t-progress__info" }, Percentage < 10 && Percentage > 0);

                    },
                    new { @class = $"t-progress__bar t-progress--plump  {progressTypeClass}", style = "width:720px" });
                    break;
                case ProgressTheme.Circle:
                    a.CreateElement(sequence + 1, "div", b =>
                    {
                        b.CreateElement(sequence + 3, "div", lableText,
                        new { @class = "t-progress__info" }, Percentage > 10 && isLable && Status == TDesignBlazor.Status.Default);

                        b.CreateElement(sequence + 4, "div", c =>
                        {
                            c.CreateComponent<Icon>(0, attributes: new { Class = $"t-icon t-icon-{icon} t-progress__icon" });
                        },
                        new { @class = "t-progress__info" }, Percentage > 10 && Status != TDesignBlazor.Status.Default);

                        b.CreateElement(sequence + 5, "svg", c =>
                        {
                            c.CreateElement(sequence + 4, "circle", d => { }, new
                            {
                                cx = circle.CX,
                                cy = circle.CY,
                                r = circle.R,
                                stroke_width = "6",
                                stroke = "",
                                fill = "none",
                                @class = "t-progress__circle-outer"
                            });

                            c.CreateElement(sequence + 5, "circle", d => { }, new
                            {
                                cx = circle.CX,
                                cy = circle.CY,
                                r = circle.R,
                                stroke_width = "6",
                                fill = "none",
                                strokeLinecap = "round",
                                transform = $"matrix(0,-1,1,0,0,{size})",
                                strokeDasharray = "99.90264638415542  233.10617489636263",
                                @class = "t-progress__circle-inner"
                            });
                        }, new { width = $"{size}", height = $"{size}", viewBox = $"0 0 {size} {size}" });
                    },
                   new { @class = $"t-progress--circle {Status.GetCssClass} ", style = $"width: {size}px; height: {size}px; font-size: 20px" });
                    break;
                default:
                    break;
            }
        },
        new { @class = "t-progress" });



    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
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
    /// 获取大小
    /// </summary>
    /// <returns></returns>
    private int GetSize()
    {
        return Size.Match<int>(
        a =>
        {
            switch (a)
            {
                case 72:
                    return 72;
                case 112:
                    return 112;
                case 160:
                    return 160;
                default:
                    return 112;
            }
        },
        b =>
        {
            switch (b)
            {
                case TDesignBlazor.Size.Small:
                    return 72;
                case TDesignBlazor.Size.Medium:
                    return 112;
                case TDesignBlazor.Size.Large:
                    return 160;
                default:
                    return 112;
            }
        });
    }
    /// <summary>
    /// 获取是否显示Lable
    /// </summary>
    /// <returns></returns>
    private bool GetIsLabel()
    {
        return Label.Match<bool>(
            a => a != "",
            b => b);
    }
    /// <summary>
    /// 获取Lable文本
    /// </summary>
    /// <returns></returns>
    private string GetLable()
    {
        return Label.Match<string>(
            a => a,
            b => Percentage.ToString() + "%");
    }
    /// <summary>
    /// 获取状态图标
    /// </summary>
    /// <returns></returns>
    private string GetIcon()
    {
        switch (Status)
        {
            case TDesignBlazor.Status.Default:
                return "";
            case TDesignBlazor.Status.Warning:
                return "error";
            case TDesignBlazor.Status.Error:
                return "close";
            case TDesignBlazor.Status.Success:
                return "check";
            case TDesignBlazor.Status.None:
                return "";
            default:
                return "";
        }
    }
    /// <summary>
    /// 获取svg圆形对象配置
    /// </summary>
    /// <returns></returns>
    private Circle GetCircle()
    {
        return Size.Match<Circle>(
        a =>
        {
            switch (a)
            {
                case 72:
                    return new() { CX = 36, CY = 36, R = 33 };
                case 112:
                    return new() { CX = 56, CY = 56, R = 53 };
                case 160:
                    return new() { CX = 80, CY = 80, R = 77 };
                default:
                    return new() { CX = 56, CY = 56, R = 53 };
            }
        },
        b =>
        {
            switch (b)
            {
                case TDesignBlazor.Size.Small:
                    return new() { CX = 36, CY = 36, R = 33 };
                case TDesignBlazor.Size.Medium:
                    return new() { CX = 56, CY = 56, R = 53 };
                case TDesignBlazor.Size.Large:
                    return new() { CX = 80, CY = 80, R = 77 };
                default:
                    return new() { CX = 56, CY = 56, R = 53 };
            }
        });
    }
}
public class LinearGradient
{
    public string From { get; set; }
    public string To { get; set; }
}

public class Circle
{
    public int CX { get; set; }
    public int CY { get; set; }
    public int R { get; set; }
}
