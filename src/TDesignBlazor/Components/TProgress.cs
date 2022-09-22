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
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public OneOf<string, LinearGradient> Color { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public OneOf<string, bool> Label { get; set; } = true;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public int? Percentage { get; set; } = 0;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public OneOf<int, Size> Size { get; set; } = 112;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter][CssClass("t-progress--status--")] public Status? Status { get; set; } = TDesignBlazor.Status.Default;// [
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public string? StrokeWidth { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public ProgressTheme? Theme { get; set; } = ProgressTheme.line;
    /// <summary>
    /// <inheritdoc/>
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
        int cx = 0, cy = 0, r = 0;
        var icon = "";
        switch (Status)
        {
            case TDesignBlazor.Status.Default:
                //icon = "check";
                break;
            case TDesignBlazor.Status.Warning:
                icon = "error";
                break;
            case TDesignBlazor.Status.Error:
                icon = "close";
                break;
            case TDesignBlazor.Status.Success:
                icon = "check";
                break;
            case TDesignBlazor.Status.None:
                break;
            default:
                //icon = "check";
                break;
        }
        var isLable = Label.Match<bool>(a => a != "",
        b => b);
        var lableText = Label.Match<string>(a => a,
        b => Percentage.ToString() + "%");
        var size = Size.Match<int>(
    a =>
    {
        switch (a)
        {
            case 72:
                cx = 36;
                cy = 36;
                r = 33;
                return 72;
            case 112:
                cx = 56;
                cy = 56;
                r = 53;
                return 112;
            case 160:
                cx = 80;
                cy = 80;
                r = 77;
                return 160;
            default:
                cx = 56;
                cy = 56;
                r = 53;
                return 112;
        }
    },
    b =>
    {
        switch (b)
        {
            case TDesignBlazor.Size.Small:
                cx = 36;
                cy = 36;
                r = 33;
                return 72;
            case TDesignBlazor.Size.Medium:
                cx = 56;
                cy = 56;
                r = 53;
                return 112;
            case TDesignBlazor.Size.Large:
                cx = 80;
                cy = 80;
                r = 77;
                return 160;
            default:
                cx = 56;
                cy = 56;
                r = 53;
                return 112;
        }
    });
        switch (Theme)
        {
            case ProgressTheme.line:
                builder.CreateElement(sequence, "div", a =>
                {
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

                },
                new { @class = "t-progress" });
                break;
            case ProgressTheme.plump:
                builder.CreateElement(sequence, "div", a =>
                {
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
                },
                new { @class = "t-progress" });
                break;
            case ProgressTheme.circle:

                builder.CreateElement(sequence, "div", a =>
                {
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
                                cx = cx,
                                cy = cy,
                                r = r,
                                stroke_width = "6",
                                stroke = "",
                                fill = "none",
                                @class = "t-progress__circle-outer"
                            });

                            c.CreateElement(sequence + 5, "circle", d => { }, new
                            {
                                cx = cx,
                                cy = cy,
                                r = r,
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
                },
                new { @class = "t-progress" });
                break;
            default:
                break;
        }

    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
    }
    /// <summary>
    /// 获取背景色
    /// </summary>
    /// <returns></returns>
    public string GetBackGround()
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


}
public class LinearGradient
{
    public string From { get; set; }
    public string To { get; set; }
}
