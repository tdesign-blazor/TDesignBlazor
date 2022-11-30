namespace TDesign;

/// <summary>
/// 用于渲染 tr 元素的组件。
/// </summary>
[ChildComponent(typeof(TTable<>))]
[HtmlTag("tr")]
public class TTableRow : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
