using TDesignBlazor.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor.Components;
[CssClass("t-menu__item")]
[ChildComponent(typeof(SubMenu), Optional = true)]
[ChildComponent(typeof(Menu))]
public class MenuItem : BlazorAnchorComponentBase, IHasDisabled
{
    [CascadingParameter] public Menu CascadingMenu { get; set; }
    [CascadingParameter] public SubMenu? CascadingSubMenu { get; set; }
    protected override string TagName => CascadingSubMenu is not null ? "div" : "li";

    [Parameter][CssClass("t-is-disabled")] public bool Disabled { get; set; }
    [Parameter] public string? Link { get; set; }

    [Parameter] public object? IconPrefix { get; set; }
    [Parameter] public object? IconSuffix { get; set; }


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
        builder.Append("t-is-active", IsActive)
            .Append("t-menu__item--plain");
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        if (IconPrefix is not null)
        {
            builder.CreateComponent<Icon>(sequence, attributes: new { Name = IconPrefix });
        }

        builder.CreateElement(sequence, "span", ChildContent, new { @class = "t-menu__content" });

        if (IconSuffix is not null)
        {
            builder.CreateComponent<Icon>(sequence, attributes: new { Name = IconSuffix });
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
