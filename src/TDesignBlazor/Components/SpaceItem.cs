namespace TDesignBlazor.Components;

/// <summary>
/// 表示间距排版的项。
/// </summary>
[ChildComponent(typeof(Space))]
[CssClass("t-space-item")]
public class SpaceItem : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
