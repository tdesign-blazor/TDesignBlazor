namespace TDesign;

/// <summary>
/// 表示间距排版的项。
/// </summary>
[ChildComponent(typeof(TSpace))]
[CssClass("t-space-item")]
public class TSpaceItem : TDesignAdditionParameterComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [ParameterApiDoc("间距内的任意内容")]
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
