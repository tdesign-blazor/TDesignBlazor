using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;

/// <summary>
/// 选项卡
/// </summary>
[ParentComponent]
[CssClass("t-tabs")]
public class Tab : BlazorComponentBase, IHasChildContent, IHasOnSwitch
{
    public Tab()
    {
        SwitchIndex = 0;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 选项卡标题呈现的位置。
    /// </summary>
    [Parameter] public Position Position { get; set; } = Position.Top;
    /// <summary>
    /// 卡片模式。
    /// </summary>
    [Parameter] public bool Card { get; set; }
    /// <summary>
    /// 选项卡的尺寸。
    /// </summary>
    [Parameter] public TabSize Size { get; set; } = TabSize.Medium;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public EventCallback<int?> OnSwitch { get; set; }
    public int? SwitchIndex { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        BuildTabHeader(builder, sequence);
        BuildTabContent(builder, sequence + 1);
    }

    void BuildTabHeader(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", BuildTabHeaderNav, new
        {
            @class = HtmlHelper.CreateCssBuilder()
            .Append("t-tabs__header")
            .Append(Position.GetCssClass("t-is-")),
        });
    }

    void BuildTabHeaderNav(RenderTreeBuilder builder)
    {
        builder.CreateElement(0, "div", content =>
        {
            BuildOperation(content, 0, true);//左按钮
            BuildOperation(content, 1, false);//右按钮

            content.CreateElement(5, "div", container =>
            {
                container.CreateElement(0, "div", scroll =>
                {
                    scroll.CreateElement(0, "div", wrap =>
                    {
                        BuildTabHeaderItem(wrap, 0);
                    }, new { @class = "t-tabs__nav-wrap t-is-smooth", style = "transform: translate(0px, 0px);" });
                }, new { @class = "t-tabs__nav-scroll" });
            }, new
            {
                @class = HtmlHelper.CreateCssBuilder()
                            .Append("t-tabs__nav-container")
                            .Append(CardCss, Card)
                            .Append(Position.GetCssClass("t-is-"))
            });

        }, new { @class = "t-tabs__nav" });
    }

    /// <summary>
    /// 选项卡的内容
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    void BuildTabContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", ChildContent, new { @class = "t-tabs__content" });
    }

    void BuildOperation(RenderTreeBuilder builder, int sequence, bool leftOrRight)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            //左右按钮
        }, new { @class = $"t-tabs__operations t-tabs__operations--{GetLeftOrRightString(leftOrRight)}" });
    }

    /// <summary>
    /// 选项卡头部的项。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    void BuildTabHeaderItem(RenderTreeBuilder builder, int sequence)
    {
        for (int i = 0; i < ChildComponents.Count; i++)
        {
            var tabItem = (TabItem)ChildComponents[i];
            var index = i;
            if (i == SwitchIndex)
            {
                builder.CreateElement(0, "div", attributes: new
                {
                    @class = HtmlHelper.CreateCssBuilder()
                                    .Append("t-tabs__bar")
                                    .Append(Position.GetCssClass("t-is-")),
                    //style = HtmlHelper.CreateStyleBuilder()
                    //                .Append($"left:{tabItem.Width * SwitchIndex}px")
                    //                .Append($"width:{tabItem?.Width}px", tabItem!.Width.HasValue)
                });
            }
            builder.CreateElement(index + 1, "div", content =>
            {
                builder.CreateElement(0, "div", wrapper =>
                {
                    wrapper.CreateElement(0, "span", title =>
                    {
                        title.CreateComponent<Icon>(0, attributes: new { Name = tabItem.Icon, style = "margin-right:4px" }, condition: tabItem!.Icon is not null);
                        title.AddContent(0, tabItem!.Title);
                    }, new { @class = "t-tabs__nav-item-text-wrapper" });
                }, new
                {
                    @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-tabs__nav-item-wrapper")
                                .Append(GetActiveCss(index))
                });
            }, new
            {
                @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-tabs__nav-item")
                                .Append(CardCss, Card)
                                .Append(GetActiveCss(index))
                                .Append("t-is-disabled", tabItem!.Disabled)
                                .Append(Size.GetCssClass("t-size-")),
                onclick = HtmlHelper.CreateCallback(this, () =>
                {
                    if (!tabItem!.Disabled)
                    {
                        this.SwitchTo(index);
                    }
                })
            }, appendFunc: (b, s) =>
            {
                b.SetKey(index);
                return s;
            });
        };
    }

    string GetLeftOrRightString(bool leftOrRight) => leftOrRight ? "left" : "right";

    string GetActiveCss(int index) => index == SwitchIndex ? "t-is-active" : string.Empty;

    string CardCss => "t-tabs__nav--card";
}
/// <summary>
/// 选项卡尺寸。
/// </summary>
public enum TabSize
{
    /// <summary>
    /// 中等尺寸。
    /// </summary>
    [CssClass("m")] Medium,
    /// <summary>
    /// 大型尺寸。
    /// </summary>
    [CssClass("l")] Large
}