using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;

/// <summary>
/// 导航菜单的项。
/// </summary>
[CssClass("t-menu__item")]
[ChildComponent(typeof(TSubMenu), Optional = true)]
[ChildComponent(typeof(TMenu))]
public class TMenuItem : BlazorAnchorComponentBase, IHasDisabled, IHasActive
{
    [CascadingParameter] public TMenu CascadingMenu { get; set; }
    [CascadingParameter] public TSubMenu? CascadingSubMenu { get; set; }
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
    [Parameter] public object? TIconPrefix { get; set; }
    /// <summary>
    /// 后缀图标的名称。
    /// </summary>
    [Parameter] public object? TIconSuffix { get; set; }
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
        if (TIconPrefix is not null)
        {
            builder.CreateComponent<TIcon>(sequence, attributes: new { Name = TIconPrefix });
        }

        builder.CreateElement(sequence, "span", ChildContent, new { @class = "t-menu__content" });

        if (TIconSuffix is not null)
        {
            builder.CreateComponent<TIcon>(sequence, attributes: new { Name = TIconSuffix });
        }
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        if (CanNavigationChanged && !string.IsNullOrWhiteSpace(Link))
        {
            attributes["href"] = Link;
            attributes["onclick"] = HtmlHelper.CreateCallback(this, NavigateTo);
        }
    }

    void NavigateTo()
    {
        NavigationManger.NavigateTo(Link);
        CascadingSubMenu?.CollapseSubMenuItem();
    }
}
