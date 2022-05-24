using Microsoft.AspNetCore.Components.Rendering;

namespace AnyDesignBlazor.Components.TDesign;
[CssClass("t-layout")]
[HtmlTag("section")]
[ParentComponent]
public class Layout : BlazorChildContentComponentBase
{
    [Parameter] public RenderFragment? HeaderContent { get; set; }
    [Parameter] public RenderFragment? LeftSideContent { get; set; }
    [Parameter] public RenderFragment? RightSideContent { get; set; }
    [Parameter] public RenderFragment? FooterContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "header", HeaderContent, new { @class = "t-layout__header" }, HeaderContent is not null);

        if (HasAsider)
        {
            builder.CreateElement(sequence + 1, "section", sider =>
            {
                BuildAsider(sider, 0, LeftSideContent);
                BuildContent(sider, 1);
                BuildAsider(sider, 2, RightSideContent);

            }, new { @class = "t-layout t-layout--with-sider" });
        }
        else
        {
            BuildContent(builder, sequence + 1);
        }

        builder.CreateElement(sequence + 2, "footer", FooterContent, new { @class = "t-layout__footer" }, FooterContent is not null);
    }

    void BuildContent(RenderTreeBuilder builder, int sequence)
    => builder.CreateElement(sequence, "main", ChildContent, new { @class = "t-layout__content" });

    bool HasAsider => LeftSideContent is not null || RightSideContent is not null;

    void BuildAsider(RenderTreeBuilder builder, int sequence, RenderFragment? content)
    => builder.CreateElement(sequence, "sider", content, new { @class = "t-layout__sider" }, content is not null);
}
