using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor.Components;
[CssClass("t-submenu")]
[HtmlTag("li")]
[ParentComponent]
[ChildComponent(typeof(Menu))]
public class SubMenu : BlazorComponentBase, IHasCascadingParameter<Menu>, IHasChildContent
{
    [CascadingParameter] public Menu CascadingValue { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public object? Icon { get; set; }

    internal bool IsOpened { get; set; }

    internal bool IsActive { get; set; }

    internal string IsOpenCssClass => IsOpened ? "t-is-opened" : string.Empty;

    internal bool CanClose { get; set; } = true;

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append(IsOpenCssClass, IsOpened)
            .Append("t-is-active", IsActive);


    }
    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append("z-index:5000");
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        if (CascadingValue.ExpandType == MenuExpandType.Popup)
        {
            attributes["onmouseleave"] = HtmlHelper.CreateCallback(this, async () =>
            {
                await Task.Delay(100);
                if (CanClose)
                {
                    CollapseSubMenuItem();
                }
            });
        }
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        Dictionary<string, object> callbackAttributes = new();

        BuildCallbackAttributes(callbackAttributes);

        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateComponent<MenuItem>(0, Title, new
            {
                IconPrefix = Icon,
                IconSuffix = IsOpened ? IconName.ChevronUp : IconName.ChevronDown,
            });

        }, callbackAttributes);


        Dictionary<string, object> htmlAttributes = new()
        {
            ["class"] = HtmlHelper.CreateCssBuilder().Append(CascadingValue.GetMenuExpandClass())
            .Append(IsOpenCssClass, IsOpened),
            ["style"] = HtmlHelper.CreateStyleBuilder().Append(CascadingValue.GetMenuExapndStyle())
        };

        var eventAttribute = new Dictionary<string, object>();

        if (CascadingValue.ExpandType == MenuExpandType.Popup)
        {
            htmlAttributes["onmouseenter"] = HtmlHelper.CreateCallback(this, () =>
            {
                CanClose = false;
            });
            htmlAttributes["onmouseleave"] = HtmlHelper.CreateCallback(this, () =>
            {
                CanClose = true;
                CollapseSubMenuItem();
            });
        }

        builder.CreateElement(sequence + 1, "div", content =>
        {
            if (CascadingValue.ExpandType == MenuExpandType.Popup)
            {
                eventAttribute["class"] = "t-menu__popup-wrapper";
                builder.CreateElement(0, "ul", ChildContent, eventAttribute);
            }
            else
            {
                builder.AddContent(0, ChildContent);
            }
        }, htmlAttributes);
    }

    private void BuildCallbackAttributes(Dictionary<string, object> htmlAttributes)
    {
        if (CascadingValue.ExpandType == MenuExpandType.Popup)
        {
            htmlAttributes["onmouseenter"] = HtmlHelper.CreateCallback(this, ExpandSubMenuItem);
        }
        else
        {
            htmlAttributes["onclick"] = HtmlHelper.CreateCallback(this, () =>
            {
                if (IsOpened)
                {
                    CollapseSubMenuItem();
                }
                else
                {
                    ExpandSubMenuItem();
                }
            });
        }
    }

    //private bool LookUpActiveMenuItem()
    //{
    //    foreach (var item in ChildComponents.Where(m => m is MenuItem).Select(m => m as MenuItem))
    //    {
    //        if (item.HasActived)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    internal Task Active()
    {
        return InvokeAsync(() =>
        {
            IsActive = true;
            //StateHasChanged();
        });
    }

    internal void ExpandSubMenuItem()
    {
        IsOpened = true;
    }

    internal void CollapseSubMenuItem()
    {
        IsOpened = false;
        CanClose = true;
    }
}
