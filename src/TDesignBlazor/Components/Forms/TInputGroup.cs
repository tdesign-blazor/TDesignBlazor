namespace TDesignBlazor;

/// <summary>
/// 可以将表单输入组件进行组合。
/// </summary>
[CssClass("t-input-group")]
public class TInputGroup : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 设置组合起来的输入框之间具备一点空隙。
    /// </summary>
    [Parameter][CssClass("t-input-group--separate")] public bool Seperate { get; set; }
}
