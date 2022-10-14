namespace TDesignBlazor;

/// <summary>
/// 表示间距排版的项。
/// </summary>
[ChildComponent(typeof(TSpace))]
[CssClass("t-space-item")]
public class TSpaceItem : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
