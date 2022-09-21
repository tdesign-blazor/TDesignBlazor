using ComponentBuilder;

using Microsoft.AspNetCore.Components.Rendering;

using System.Reflection.PortableExecutable;

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

        builder.CreateElement(sequence, "div", a =>
        {
            a.CreateElement(0, "div", b =>
            {
                b.CreateElement(0, "div", c =>
                {
                    c.CreateElement(0, "div", c => { }, new { @class = $"t-progress__inner {Theme.GetCssClass} {Status.GetCssClass}", style = $"width: {Percentage}%;{background}" });
                },
                new { @class = "t-progress__bar" });

                b.CreateElement(0, "div", Percentage.ToString() + "%", new { @class = "t-progress__info" }, true);
            },
            new { @class = "t-progress--thin t-progress--status--undefined" });

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
