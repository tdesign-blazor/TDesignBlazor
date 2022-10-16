namespace TDesign;
/// <summary>
/// 图标组件。内置图标要手动引入样式 <c>tdesign-icons.css</c>
/// </summary>
[HtmlTag("i")]
[CssClass("t-icon")]
[ChildComponent(typeof(TMenu), Optional = true)]
[ChildComponent(typeof(TSubMenu), Optional = true)]
public class TIcon : BlazorComponentBase
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (IsInMenuOperation)
        {
            Size = "25px";
        }
    }

    /// <summary>
    /// 图标放在 <see cref="TMenu"/> 组件中会有特定样式。
    /// </summary>
    [CascadingParameter] public TMenu? CascadingMenu { get; set; }
    [CascadingParameter] public TSubMenu? CascadingSubMenu { get; set; }

    /// <summary>
    /// 图标名称。参见 https://tdesign.tencent.com/vue/components/icon 。
    /// <para>可使用 <see cref="IconName"/> 枚举。</para>
    /// </summary>
    [Parameter] public object? Name { get; set; }
    /// <summary>
    /// 图标的颜色。自由填写支持 color 的字符串，例如颜色名称或16进制表达式（#xxxxxx）。
    /// </summary>
    [Parameter] public string? Color { get; set; }
    /// <summary>
    /// 图标的大小。自由填写支持 font-size 的字符串，例如 32px 或 2rem 等。
    /// </summary>
    [Parameter] public string? Size { get; set; }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-menu__operations-icon", IsInMenuOperation);

        if (Name is IconName iconName)
        {
            builder.Append($"t-icon-{iconName.GetCssClass()}");
        }
        else
        {
            builder.Append(Name?.ToString());
        }
    }

    /// <summary>
    /// 判断 TIcon 是否在 <see cref="TMenu"/> 组件的 <see cref="TMenu.OperationContent"/> 内。
    /// </summary>
    private bool IsInMenuOperation => CascadingMenu is not null && CascadingMenu.OperationContent is not null && CascadingSubMenu is null;

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"color:{Color}", !string.IsNullOrEmpty(Color))
            .Append($"font-size:{Size}", !string.IsNullOrEmpty(Size))
            ;
    }
}
