namespace TDesign;

/// <summary>
/// 下拉菜单。用于承载过多的操作集合，通过下拉拓展的形式，收纳更多的操作。
/// </summary>
public class TDropdown : TDesignChildContentComponentBase
{
    /// <summary>
    /// 设置下拉菜单的选项集合。
    /// </summary>
    [Parameter][EditorRequired] public IEnumerable<DropdownOption> Options { get; set; } = Enumerable.Empty<DropdownOption>();
    /// <summary>
    /// 触发下拉菜单的模式。默认是 <see cref="PopupTrigger.Click"/> 模式。
    /// </summary>
    [Parameter] public PopupTrigger Trigger { get; set; } = PopupTrigger.Click;

    /// <summary>
    /// 下拉菜单选项中文字的对齐方向。
    /// </summary>
    [Parameter] public DropdownDirection Direction { get; set; } = DropdownDirection.Right;

    /// <summary>
    /// 下拉菜单层展开的方向。默认是 <see cref="PopupPlacement.Bottom"/>。
    /// </summary>
    [Parameter] public PopupPlacement Placement { get; set; } = PopupPlacement.Bottom;


    /// <summary>
    /// 下拉菜单项的最大高度。单位 px，默认 300。
    /// </summary>
    [Parameter] public int Height { get; set; } = 300;

    /// <summary>
    /// 设置当菜单选项被点击选中后的回调方法。
    /// </summary>
    [Parameter]public EventCallback<DropdownOption> OnOptionSelected { get; set; }    

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Component<TPopup>()
            .Attribute(m => m.Trigger, Trigger)
            .Attribute(m=>m.Placement,Placement)
            .Attribute(m => m.PopupContentCssClass, "t-dropdown")
            .Attribute(m => m.PopupContent, dropdown =>
            {
                dropdown.Div("t-dropdown__menu").Class(Direction.GetCssClass("t-dropdown__menu--")).Style($"max-height:{Height}px").Content(content=>BuildOptions(content,Options)).Close();
            })
            .Content(ChildContent)
            .Close();
    }

    void BuildOptions(RenderTreeBuilder builder,IEnumerable<DropdownOption> options)
    {
        foreach ( var item in options )
        {
            builder.Div().Content(div =>
            {
                div.Element("li")
                    .Key(item)
                    .Class("t-dropdown__item")
                    .Class($"t-dropdown__item--theme-{(item.Color.HasValue? item.Color.GetCssClass():"default")}")
                    .Class($"t-dropdown__item--disabled", item.Disabled)
                    .Class("t-dropdown__item--active",item.Selected)
                    .Callback<MouseEventArgs>("onclick", this, e => SelectOption(item), !item.Disabled)
                    .Content(content =>
                    {
                        if ( item.Options.Any() )
                        {
                            content.Div("t-dropdown__item-content").Content(text =>
                            {
                                text.Component<TIcon>(Direction == DropdownDirection.Left).Attribute(m => m.Name, IconName.ChevronLeft).Close();
                                BuildItemText(text, item);
                                text.Component<TIcon>(Direction== DropdownDirection.Right).Attribute(m => m.Name, IconName.ChevronRight).Close();
                            }).Close();

                            content.Div("t-dropdown__submenu-wrapper")
                                .Class(Direction.GetCssClass("t-dropdown__submenu-wrapper--"))
                                .Style("position:absolute")
                                .Content(submenu =>
                                {
                                    submenu.Div("t-dropdown__submenu")
                                            .Style("position:static")
                                            .Style($"max-height:{Height}px")
                                            .Content(menu =>menu.Element("ul").Content(ul=> BuildOptions(ul, item.Options)).Close())
                                        .Close();
                                })
                                .Close();
                        }
                        else
                        {
                            BuildItemText(content, item);
                        }
                    })
                    .Close();

                builder.Component<TDivider>(item.Divider).Close();
            })
            .Close();
        }
    }

    /// <summary>
    /// 选中指定的菜单选项。
    /// </summary>
    /// <param name="option">要选中的选项。</param>
    /// <exception cref="ArgumentNullException"><paramref name="option"/> 是 null。</exception>
    public Task SelectOption(DropdownOption option)
    {
        if ( option is null )
        {
            throw new ArgumentNullException(nameof(option));
        }

        ResetSelectedOptions(Options);

        option.Selected = true;
        return OnOptionSelected.InvokeAsync(option);
    }

    /// <summary>
    /// 重置所有的选中项。
    /// </summary>
    /// <param name="options"></param>
    void ResetSelectedOptions(IEnumerable<DropdownOption> options)
    {
        options.ForEach(item =>
        {
            if ( item.Selected )
            {
                item.Selected = false;
                ResetSelectedOptions(item.Options);
            }
        });
    }

    void BuildItemText(RenderTreeBuilder builder,DropdownOption option)
    {
        builder.Div("t-dropdown__item-icon", option.PrefixIcon is not null).Content(icon =>
        {
            icon.Component<TIcon>().Attribute(m => m.Name, option.PrefixIcon).Close();
        }).Close();
        
        builder.Span("t-dropdown__item-text").Content(option.Content).Close();
    }
}

/// <summary>
/// 下拉菜单项的文字方向。
/// </summary>
public enum DropdownDirection
{
    /// <summary>
    /// 偏左。
    /// </summary>
    Left,
    /// <summary>
    /// 偏右。
    /// </summary>
    Right
}