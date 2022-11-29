namespace TDesign;

/// <summary>
/// 表示间距排版的项。
/// </summary>
[ChildComponent(typeof(TSpace))]
[CssClass("t-space-item")]
public class TSpaceItem : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
