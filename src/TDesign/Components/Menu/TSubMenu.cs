using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 表示具备下级菜单项的二级菜单。
/// </summary>
[CssClass("t-submenu")]
[HtmlTag("li")]
[ParentComponent]
[ChildComponent(typeof(TMenu))]
public class TSubMenu : TDesignComponentBase, IHasChildContent
{
    [CascadingParameter] public TMenu CascadingMenu { get; set; }
    /// <summary>
    /// 菜单内容。
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 显示下级菜单的当前菜单标题。
    /// </summary>
    [Parameter] public string? Title { get; set; }
    /// <summary>
    /// 图标名称。
    /// </summary>
    [Parameter] public object? TIcon { get; set; }

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
        if (CascadingMenu.Popup)
        {
            attributes["onmouseleave"] = HtmlHelper.Instance.Callback().Create(this, async () =>
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
            content.CreateComponent<TMenuItem>(0, Title, new
            {
                TIconPrefix = TIcon,
                TIconSuffix = IsOpened ? IconName.ChevronUp : IconName.ChevronDown,
            });

        }, callbackAttributes);


        Dictionary<string, object> htmlAttributes = new()
        {
            ["class"] = HtmlHelper.Instance.Class().Append(CascadingMenu.GetMenuExpandClass())
            .Append(IsOpenCssClass, IsOpened),
            ["style"] = HtmlHelper.Instance.Style().Append(CascadingMenu.GetMenuExapndStyle())
        };

        var eventAttribute = new Dictionary<string, object>();

        if (CascadingMenu.Popup)
        {
            htmlAttributes["onmouseenter"] = HtmlHelper.Instance.Callback().Create(this, () =>
            {
                CanClose = false;
            });
            htmlAttributes["onmouseleave"] = HtmlHelper.Instance.Callback().Create(this, () =>
            {
                CanClose = true;
                CollapseSubMenuItem();
            });
        }

        builder.CreateElement(sequence + 1, "div", content =>
        {
            if (CascadingMenu.Popup)
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
        if (CascadingMenu.Popup)
        {
            htmlAttributes["onmouseenter"] = HtmlHelper.Instance.Callback().Create(this, ExpandSubMenuItem);
        }
        else
        {
            htmlAttributes["onclick"] = HtmlHelper.Instance.Callback().Create(this, () =>
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
