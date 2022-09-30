using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace TDesignBlazor;

/// <summary>
/// 导航菜单的项。
/// </summary>
[CssClass("t-menu__item")]
[ChildComponent(typeof(SubMenu), Optional = true)]
[ChildComponent(typeof(Menu))]
public class MenuItem : BlazorAnchorComponentBase, IHasDisabled, IHasActive
{
    [CascadingParameter] public Menu CascadingMenu { get; set; }
    [CascadingParameter] public SubMenu? CascadingSubMenu { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override string TagName => CascadingSubMenu is not null ? "div" : "li";

    /// <summary>
    /// 禁用状态。
    /// </summary>
    [Parameter][CssClass("t-is-disabled")] public bool Disabled { get; set; }

    /// <summary>
    /// 导航的超链接。
    /// </summary>
    [Parameter] public string? Link { get; set; }

    /// <summary>
    /// 前缀图标的名称。
    /// </summary>
    [Parameter] public object? IconPrefix { get; set; }

    /// <summary>
    /// 后缀图标的名称。
    /// </summary>
    [Parameter] public object? IconSuffix { get; set; }

    /// <summary>
    /// 选中状态。若为 <c>false</c> 则根据导航自动判断。
    /// </summary>
    [Parameter] public bool Active { get; set; }

    internal bool CanNavigationChanged { get; set; } = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (CascadingSubMenu is not null)
        {
            await CascadingSubMenu?.Active();
        }
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-is-active", Active || IsActive)
            .Append("t-menu__item--plain");
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        if (IconPrefix is not null)
        {
            builder.CreateComponent<Icon>(sequence, attributes: new { Name = IconPrefix });
        }

        builder.CreateElement(sequence, "span", Link is not { Length: > 0 } ? ChildContent : buider =>
        {
            builder.CreateComponent<NavLink>(0, ChildContent, attributes: new { href = Link, Match });
        },
        new { @class = "t-menu__content" });

        if (IconSuffix is not null)
        {
            builder.CreateComponent<Icon>(sequence, attributes: new { Name = IconSuffix });
        }
    }
}