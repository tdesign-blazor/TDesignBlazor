using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;
/// <summary>
/// 导航菜单。
/// </summary>
[ParentComponent]
public class Menu : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// 设置 <c>true</c> 表示侧边菜单栏。
    /// </summary>
    [Parameter] public bool Aside { get; set; }

    /// <summary>
    /// 是否暗色主题。
    /// </summary>
    [Parameter][BooleanCssClass("t-menu--dark", "t-menu--light")] public bool Dark { get; set; }
    /// <summary>
    /// 设置 <c>true</c> 表示下级菜单的展开模式为【弹出】方式，即鼠标悬停后展开下级菜单。否则是【鼠标点击】后展开下级菜单。
    /// </summary>
    [Parameter][BooleanCssClass("popup", "sub")] public bool Popup { get; set; }
    /// <summary>
    /// Logo 部分的内容。
    /// </summary>
    [Parameter] public RenderFragment? LogoContent { get; set; }
    /// <summary>
    /// 主体部分的内容。
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 右侧操作部分的内容。
    /// </summary>
    [Parameter] public RenderFragment? OperationContent { get; set; }
    /// <summary>
    /// 折叠菜单，侧边菜单有效。
    /// </summary>
    [Parameter][CssClass("t-is-collapsed")] public bool Collapse { get; set; }
    /// <summary>
    /// 侧边菜单的宽度。
    /// </summary>
    [Parameter] public int? Width { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        /* 顶部导航二级菜单问题
        * ***强制设成 popup 的模式显示二级菜单。
        * 原因：如果是二层导航，则布局会完全不一样，暂时无法做到在不改变现有组件调用的情况下该改写布局
        * 腾讯说实话：蛋疼的很
        */

        if (!Aside && !Popup)
        {
            Popup = true;
        }

        if (!Width.HasValue && Collapse)
        {
            Width = 64;
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", inner =>
        {
            //logo
            inner.CreateElement(0, "div", LogoContent, new { @class = "t-menu__logo" },LogoContent!=default(RenderFragment));

            builder.CreateElement(1, "ul", ChildContent, new
            {
                @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-menu")
                                .Append("t-menu--scroll narrow-scrollbar", Aside),
            });

            builder.CreateElement(2, "div", OperationContent, new { @class = "t-menu__operations" });
        },
        new
        {
            @class = GetMenuSpecificClass("t-{0}-menu__inner")
        });
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if (!Aside)
        {
            builder.Append("t-menu");
        }

        builder.Append(GetMenuSpecificClass("t-{0}-menu"));
    }

    /// <summary>
    /// 获取菜单特定的css
    /// </summary>
    /// <param name="css">用'{0}'占位可变的头部还是边</param>
    /// <returns></returns>
    string GetMenuSpecificClass(string css) => string.Format(css, (!Aside ? "head" : "default"));

    internal string GetMenuExpandClass() => $"t-menu__{(Popup ? "popup" : "sub")}";
    internal string GetMenuExapndStyle()
    {
        if (Popup)
        {
            return "--popup-max-height:144px; --popup-width:110.703px;";
        }
        return "--padding-left:44px;";
    }

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"width:{Width}px", Width.HasValue);
    }
}
