namespace TDesign;

/// <summary>
/// 面包屑导航的容器。
/// </summary>
[ParentComponent]
[CssClass("t-breadcrumb")]
public class TBreadcrumb : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [ParameterApiDoc("装载 TBreadcrumbItem 组件")]
    [Parameter] public override RenderFragment? ChildContent { get; set; }
}
