using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

namespace TDesign;

/// <summary>
/// 表示面包屑导航的项。必须在 <see cref="TBreadcrumb"/> 组件中使用。
/// </summary>
[CssClass("t-breadcrumb__item")]
[ChildComponent(typeof(TBreadcrumb))]
public class TBreadcrumbItem : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// 分隔符内容。
    /// </summary>
    [ParameterApiDoc("分隔符内容")]
    [Parameter] public RenderFragment? SeperatorContent { get; set; }
    /// <summary>
    /// 导航的超链接。
    /// </summary>
    [ParameterApiDoc("导航的超链接")]
    [Parameter][HtmlAttribute("href")] public string? Link { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [ParameterApiDoc("导航的任意内容")]
    [Parameter] public override RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 禁用导航。
    /// </summary>
    [ParameterApiDoc("禁用导航")]
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
        builder.CreateElement(sequence, !string.IsNullOrEmpty(Link) ? "a" : "p", overflow =>
        {
            overflow.CreateElement(0, "p", ChildContent, new { style = "max-width:150px;white-space: nowrap;overflow: hidden;text-overflow: ellipsis;", });
        }, new
        {
            @class = HtmlHelper.Instance.Class().Append("t-breadcrumb--text-overflow")
            .Append("t-link", HasLink).Append("t-is-disabled", Disabled)
        });

        builder.CreateElement(sequence + 1, "p", SeperatorContent, new { @class = "t-breadcrumb__separator" });
    }
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (SeperatorContent is null)
        {
            this.SeperatorContent = builder => builder.CreateComponent<TIcon>(0, attributes: new { Name = IconName.ChevronRight });
        }
    }

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }

}
