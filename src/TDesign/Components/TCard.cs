using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 卡片组件。
/// </summary>
[CssClass("t-card")]
public class TCard : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 设置不显示边框。
    /// </summary>
    [Parameter][BooleanCssClass("", "t-card--bordered")] public bool Borderless { get; set; }
    /// <summary>
    /// 鼠标悬停显示阴影。
    /// </summary>
    [Parameter][CssClass("t-card--shadow-hover")] public bool HoverShadow { get; set; }

    /// <summary>
    /// 设置是否显示头部内容分割线。
    /// </summary>
    [Parameter] public bool HeaderDivider { get; set; }
    /// <summary>
    /// 设置头部内容的标题部分的内容。
    /// </summary>
    [Parameter] public RenderFragment? HeaderTitleContent { get; set; }
    /// <summary>
    /// 设置头部内容的副标题部分的内容。
    /// </summary>
    [Parameter] public RenderFragment? HeaderSubTitleContent { get; set; }
    /// <summary>
    /// 设置头部内容的描述部分的内容。
    /// </summary>
    [Parameter] public RenderFragment? HeaderDescriptionContent { get; set; }
    /// <summary>
    /// 设置头部内容的操作部分的内容。
    /// </summary>
    [Parameter] public RenderFragment? HeaderActionContent { get; set; }
    /// <summary>
    /// 设置卡片的底部内容。
    /// </summary>
    [Parameter] public RenderFragment? FooterContent { get; set; }

    /// <summary>
    /// 宽度，默认400px.
    /// </summary>
    [Parameter] public int Width { get; set; } = 400;


    bool HasHeader => HeaderTitleContent is not null || HeaderActionContent is not null;

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", header =>
        {
            header.CreateElement(0, "div", inner =>
            {
                inner.CreateElement(0, "div", content =>
                {
                    content.CreateElement(0, "span", HeaderTitleContent, new { @class = "t-card__title" });
                    content.CreateElement(1, "span", HeaderSubTitleContent, new { @class = "t-card__subtitle" }, HeaderSubTitleContent is not null);
                    content.CreateElement(2, "p", HeaderDescriptionContent, new { @class = "t-card__description" }, HeaderDescriptionContent is not null);
                });

            }, new { @class = "t-card__header-wrapper" }, HeaderTitleContent is not null);

            header.CreateElement(1, "div", HeaderActionContent, new { @class = "t-card__actions" }, HeaderActionContent is not null);
        }, new
        {
            @class = HtmlHelper.Class.Append("t-card__header")
            .Append("t-card__title--bordered", HeaderDivider)
        }, HasHeader);

        builder.CreateElement(sequence + 1, "div", ChildContent, new { @class = "t-card__body" });

        builder.CreateElement(sequence + 2, "div", content =>
        {
            content.CreateElement(0, "div", FooterContent, new { @class = "t-card__footer-wrapper" });
        }, new { @class = "t-card__footer" }, FooterContent is not null);
    }

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"width:{Width}px");
    }
}
