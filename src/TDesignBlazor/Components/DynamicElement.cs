namespace TDesignBlazor;

/// <summary>
/// 表示动态的 HTML 元素。
/// </summary>
public class DynamicElement : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 获取或设置 HTML 元素的名称。默认是 "div"。
    /// </summary>
    [Parameter] public string? Name { get; set; } = "div";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override string TagName => Name ?? base.TagName;
}


/// <summary>
/// 表示作为 div 的 HTML 元素。
/// </summary>
public class Div : DynamicElement
{
}

/// <summary>
/// 表示作为 span 的 HTML 元素。
/// </summary>
public class Span : DynamicElement
{
    /// <summary>
    /// 初始化 <see cref="Span"/> 类的新实例。
    /// </summary>
    public Span()
    {
        Name = "span";
    }
}