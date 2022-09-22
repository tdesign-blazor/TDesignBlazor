namespace TDesignBlazor;

/// <summary>
/// 表示渲染 td 元素的组件。
/// </summary>
[HtmlTag("td")]
[ChildComponent(typeof(Table<>))]
public class TableCell : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}

/// <summary>
/// 表示渲染 th 元素的组件。
/// </summary>
[HtmlTag("th")]
public class TableHeader : TableCell
{

}