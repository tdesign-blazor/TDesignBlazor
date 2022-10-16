using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 具备悬浮提示的弹出层。
/// </summary>
[CssClass("t-popup")]
public class TPopup : BlazorComponentBase, IHasChildContent, IHasActive
{
    /// <inheritdoc/>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public OneOf<string?, RenderFragment?, MarkupString?> Content { get; set; }

    [Parameter] public Placement Placement { get; set; } = Placement.TopCenter;
    [Parameter] public bool Active { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateElement(0, "div", content =>
        {
            content.AddContent(0, ChildContent);
            builder.CreateElement(1, "div", content =>
            {
                base.BuildRenderTree(content);
            }, new
            {
                style = "width: 100%;top:0px",
                @class = HtmlHelper.CreateCssBuilder().Append(Placement.GetCssClass())
            });
        }, new
        {
            style = "position:relative",
            onmouseover = HtmlHelper.CreateCallback<MouseEventArgs>(this, Toggle),
            onmouseout = HtmlHelper.CreateCallback<MouseEventArgs>(this, Toggle)
        }); ;
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", Content, new { @class = "t-popup__content" });
    }
    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append("position: absolute; inset: auto auto 0px 0px; margin: 8px;", Active)
            .Append("display:none", !Active)
            ;
    }

    public Task Toggle(MouseEventArgs e)
    {
        _ = new Timer(_ =>
        {
            Active = !Active;
            this.Refresh();

        }, Active, 100, Timeout.Infinite);
        return Task.CompletedTask;
    }
}
