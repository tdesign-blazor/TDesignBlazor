namespace TDesign;

/// <summary>
/// 用于包裹其他内容和 <see cref="Loading"/> 的容器。
/// </summary>
[CssClass("t-loading__parent")]
public class LoadingContainer : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
