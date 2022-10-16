namespace TDesign;

/// <summary>
/// 面包屑导航的容器。
/// </summary>
[ParentComponent]
[CssClass("t-breadcrumb")]
public class TBreadcrumb : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
