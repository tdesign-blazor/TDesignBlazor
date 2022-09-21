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
    [Parameter] public string? Label { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public int? Percentage { get; set; } = 0;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public string? Size { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter][CssClass("t-progress--status--")] public Status? Status { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public string? StrokeWidth { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter][CssClass] public ProgressTheme? Theme { get; set; } = ProgressTheme.line;
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
        var s = Percentage<10 && Percentage>0? "t-progress--under-ten": "t-progress--over-ten";
        switch (Theme)
        {
            case ProgressTheme.line:
                builder.CreateElement(sequence, "div", a =>
                {
                    a.CreateElement(sequence+1, "div", b =>
                    {
                        b.CreateElement(sequence+2, "div", c =>
                        {
                            c.CreateElement(sequence+3, "div", c => { }, new { @class = $"t-progress__inner {Theme.GetCssClass} {Status.GetCssClass}", style = $"width: {Percentage}%;{background}" });
                        },
                        new { @class = "t-progress__bar" ,style="width:720px"});

                        b.CreateElement(sequence+4, "div", Percentage.ToString() + "%", new { @class = "t-progress__info" }, true);
                    },
                    new { @class = "t-progress--thin t-progress--status--undefined" });

                },
                new { @class = "t-progress" });
                break;
            case ProgressTheme.plump:
                builder.CreateElement(sequence, "div", a =>
                {
                    a.CreateElement(sequence+1, "div", b =>
                    {
                        b.CreateElement(sequence + 2, "div", c =>
                        {
                            c.CreateElement(sequence + 3, "div", Percentage.ToString() + "%", 
                                new { @class = "t-progress__info" }, Percentage>10);
                        }, 
                        new { @class = "t-progress__inner", style = $"width: {Percentage}%;" });

                        b.CreateElement(sequence + 3, "div", Percentage.ToString() + "%", new { @class = "t-progress__info" }, Percentage < 10 && Percentage > 0);
                        
                    }, 
                    new { @class = $"t-progress__bar t-progress--plump  {s}", style = "width:720px" });
                },
                new { @class = "t-progress" });
                break;
            case ProgressTheme.circle:
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
