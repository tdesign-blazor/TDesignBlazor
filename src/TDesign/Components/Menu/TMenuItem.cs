using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace TDesign;

/// <summary>
/// 导航菜单的项。
/// </summary>
[CssClass("t-menu__item")]
[ChildComponent(typeof(TSubMenu), Optional = true)]
[ChildComponent(typeof(TMenu))]
public class TMenuItem : TDesignComponentBase, IHasNavLink, IHasDisabled, IHasActive
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [CascadingParameter] public TMenu CascadingMenu { get; set; }
    [CascadingParameter] public TSubMenu? CascadingSubMenu { get; set; }
    [Parameter] public NavLinkMatch Match { get; set; } = NavLinkMatch.All;

    string? IHasNavLink.ActiveCssClass => "t-is-active";

    public bool IsActive { get; set; }
    internal bool CanNavigationChanged { get; set; } = true;

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
    /// <inheritdoc/>
    [Parameter]public RenderFragment? ChildContent { get; set; }

    public override string? GetTagName() => CascadingSubMenu is not null ? "div" : "li";


    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (CascadingSubMenu is not null)
        {
            await CascadingSubMenu?.Active();
        }
    }


    /// <inheritdoc/>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-is-active", Active || IsActive)
            .Append("t-menu__item--plain");
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateComponent<TIcon>(sequence, attributes: new { Name = IconPrefix }, condition: IconPrefix is not null);

        builder.CreateElement(sequence, "span", ChildContent, new { @class = "t-menu__content" });

        builder.CreateComponent<TIcon>(sequence, attributes: new { Name = IconSuffix }, condition: IconSuffix is not null);
    }

    /// <inheritdoc/>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        if (CanNavigationChanged && !string.IsNullOrWhiteSpace(Link))
        {
            attributes["href"] = Link;
            attributes["onclick"] = HtmlHelper.Event.Create(this, NavigateTo);
        }
    }

    /// <summary>
    /// 跳转到指定的连接地址。
    /// </summary>
    void NavigateTo()
    {
        NavigationManager.NavigateTo(Link);
        CascadingSubMenu?.CollapseSubMenuItem();
    }
}
