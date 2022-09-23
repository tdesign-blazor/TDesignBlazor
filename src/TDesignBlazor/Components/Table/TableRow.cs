namespace TDesign;

/// <summary>
/// 用于渲染 tr 元素的组件。
/// </summary>
[ChildComponent(typeof(Table<>))]
[HtmlTag("tr")]
public class TableRow : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
