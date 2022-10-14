using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Options;

namespace TDesignBlazor;

/// <summary>
/// 表示面包屑导航的项。必须在 <see cref="TBreadcrumb"/> 组件中使用。
/// </summary>
[CssClass("t-breadcrumb__item")]
[ChildComponent(typeof(TBreadcrumb))]
public class TBreadcrumbItem : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// 获取注入的配置。
    /// </summary>
    [Inject] IOptions<TDesignOptions> Options { get; set; }
    /// <summary>
    /// 分隔符内容。
    /// </summary>
    [Parameter] public RenderFragment? SeperatorContent { get; set; }
    /// <summary>
    /// 导航的超链接。
    /// </summary>
    [Parameter][HtmlAttribute("href")] public string? Link { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 禁用导航。
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// 表示是否右超链接。
    /// </summary>
    protected bool HasLink => !string.IsNullOrEmpty(Link);

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("dark", Options.Value.Dark).Append("light", !Options.Value.Dark);
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, !string.IsNullOrEmpty(Link) ? "a" : "span", overflow =>
        {
            overflow.CreateElement(0, "span", ChildContent, new { @class = "t-breadcrumb__inner", style = "max-width:120px" });
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-breadcrumb--text-overflow")
            .Append("t-link", HasLink).Append("t-is-disabled", Disabled)
        });

        builder.CreateElement(sequence + 1, "span", SeperatorContent, new { @class = "t-breadcrumb__separator" });
    }
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (SeperatorContent is null)
        {
            this.SeperatorContent = builder => builder.CreateComponent<TIcon>(0, attributes: new { Name = IconName.ChevronRight });
        }
    }

}
