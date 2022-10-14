namespace TDesignBlazor;

/// <summary>
/// 用于包裹其他内容和 <see cref="TLoading"/> 的容器。
/// </summary>
[CssClass("t-loading__parent")]
public class LoadingContainer : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
