namespace TDesignBlazor.Components;
/// <summary>
/// 图标组件。内置图标要手动引入样式 <c>tdesign-icons.css</c>
/// </summary>
[HtmlTag("i")]
[CssClass("t-icon")]
public class Icon : TDesignComponentBase
{
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
        if (Name is IconName iconName)
        {
            builder.Append($"t-icon-{iconName.GetCssClass()}");
        }
        else
        {
            builder.Append(Name?.ToString());
        }
    }

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"color:{Color}", !string.IsNullOrEmpty(Color))
            .Append($"font-size:{Size}", !string.IsNullOrEmpty(Size))
            ;
    }
}
