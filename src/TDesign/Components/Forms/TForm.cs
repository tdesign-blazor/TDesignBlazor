using Microsoft.AspNetCore.Components.Forms;

namespace TDesign;

/// <summary>
/// 表示作为表单的组件。
/// </summary>
[CssClass("t-form")]
[ParentComponent]
public class TForm : TDesignComponentBase,IHasForm
{
    /// <summary>
    /// 设置作为行内布局。
    /// </summary>
    [Parameter][CssClass("t-form-inline")] public bool Inline { get; set; }

    /// <summary>
    /// 表单的对齐方式。默认时 <see cref="FormAlignment.Right"/> 。
    /// </summary>
    [Parameter] public FormAlignment Alignment { get; set; } = FormAlignment.Right;
    /// <inheritdoc/>
    [Parameter]public object? Model { get; set; }
    /// <inheritdoc/>
    [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
    /// <inheritdoc/>
    [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }
    /// <inheritdoc/>
    [Parameter] public EventCallback<EditContext> OnInvalidSubmit { get; set; }
    /// <inheritdoc/>
    public EditContext? FixedEditContext { get; set; }
    /// <inheritdoc/>
    [Parameter] public EditContext? EditContext { get; set; }
    /// <inheritdoc/>
    [Parameter] public RenderFragment<EditContext>? ChildContent { get; set; }
}

/// <summary>
/// 表单文本的对齐方式。
/// </summary>
public enum FormAlignment
{
    /// <summary>
    /// 居顶对齐。
    /// </summary>
    Top,
    /// <summary>
    /// 居左对齐。
    /// </summary>
    Left,
    /// <summary>
    /// 居右对齐。
    /// </summary>
    Right,
}