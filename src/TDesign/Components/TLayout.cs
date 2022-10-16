using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 布局组件。
/// </summary>
[CssClass("t-layout")]
[HtmlTag("section")]
[ParentComponent]
public class TLayout : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// 布局的顶部内容。
    /// </summary>
    [Parameter] public RenderFragment? HeaderContent { get; set; }
    /// <summary>
    /// 布局的右侧内容。
    /// </summary>
    [Parameter] public RenderFragment? LeftSideContent { get; set; }
    /// <summary>
    /// 布局的左侧部分。
    /// </summary>
    [Parameter] public RenderFragment? RightSideContent { get; set; }
    /// <summary>
    /// 布局的底部部分。
    /// </summary>
    [Parameter] public RenderFragment? FooterContent { get; set; }
    /// <summary>
    /// 布局的主体部分。
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "header", HeaderContent, new { @class = "t-layout__header" }, HeaderContent is not null);

        if (HasAsider)
        {
            builder.CreateElement(sequence + 1, "section", sider =>
            {
                BuildAsider(sider, 0, LeftSideContent);
                builder.CreateElement(1, "section", content =>
                {
                    BuildContent(content, 0);
                    BuildFooter(content, 1);
                }, new { @class = "t-layout" });
                BuildAsider(sider, 2, RightSideContent);

            }, new { @class = "t-layout t-layout--with-sider" });
        }
        else
        {
            BuildContent(builder, sequence + 1);

            BuildFooter(builder, sequence);
        }
    }

    private void BuildFooter(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence + 2, "footer", FooterContent, new { @class = "t-layout__footer" }, FooterContent is not null);
    }

    void BuildContent(RenderTreeBuilder builder, int sequence)
    => builder.CreateElement(sequence, "main", ChildContent, new { @class = "t-layout__content" });

    bool HasAsider => LeftSideContent is not null || RightSideContent is not null;

    void BuildAsider(RenderTreeBuilder builder, int sequence, RenderFragment? content)
    => builder.CreateElement(sequence, "aside", content, new { @class = "t-layout__sider" }, content is not null);
}
