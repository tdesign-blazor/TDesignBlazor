using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor.Components;
/// <summary>
/// 菜单。
/// </summary>
[ParentComponent]
public class Menu : TDesignComponentBase, IHasChildContent
{
    [Parameter] public MenuType Type { get; set; } = MenuType.Header;
    [Parameter][CssClass("t-menu--")] public MenuTheme Theme { get; set; } = MenuTheme.Light;

    [Parameter] public MenuExpandType ExpandType { get; set; } = MenuExpandType.Popup;

    [Parameter] public RenderFragment? LogoContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? OperationContent { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (Type == MenuType.Header && ExpandType == MenuExpandType.Default)
        {
            ExpandType = MenuExpandType.Popup;
        }
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", inner =>
        {
            //logo
            inner.CreateElement(0, "div", LogoContent, new { @class = "t-menu__logo" });

            builder.CreateElement(1, "ul", ChildContent, new
            {
                @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-menu")
                                .Append("t-menu--scroll narrow-scrollbar", Type == MenuType.Aside)
            });

            builder.CreateElement(2, "div", OperationContent, new { @class = "t-menu__operations" });
        },
        new
        {
            @class = GetMenuSpecificClass("t-{0}-menu__inner")
        });
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if (Type == MenuType.Header)
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
    string GetMenuSpecificClass(string css) => string.Format(css, (Type == MenuType.Header ? "head" : "default"));

    internal string GetMenuExpandClass() => $"t-menu__{(ExpandType.GetCssClass())}";
    internal string GetMenuExapndStyle()
    {
        switch (ExpandType)
        {
            case MenuExpandType.Default:
                return "--padding-left:44px;";
            case MenuExpandType.Popup:
                return "--popup-max-height:144px; --popup-width:110.703px;";
            default:
                throw new IndexOutOfRangeException($"No such enum member of '{nameof(MenuExpandType)}'");
        }
    }

}

public enum MenuTheme
{
    Light,
    Dark
}

public enum MenuType
{
    [CssClass("default")] Aside,
    Header
}

public enum MenuExpandType
{
    [CssClass("sub")] Default,
    [CssClass("popup")] Popup
}