using Microsoft.AspNetCore.Components.Forms;

namespace TDesign;

/// <summary>
/// 表示作为表单的组件。
/// </summary>
[CssClass("t-form")]
[ParentComponent]
public class TForm : TDesignAdditionParameterComponentBase,IFormComponent
{
    /// <summary>
    /// 设置作为行内布局。
    /// </summary>
    [ParameterApiDoc("整个表单都是行内布局")]
    [Parameter][CssClass("t-form-inline")] public bool Inline { get; set; }

    /// <summary>
    /// 表单的对齐方式。默认时 <see cref="FormAlignment.Right"/> 。
    /// </summary>
    [ParameterApiDoc("表单的对齐方式",Value = "Right")]
    [Parameter] public FormAlignment Alignment { get; set; } = FormAlignment.Right;
    /// <inheritdoc/>
    [ParameterApiDoc("指定表单的顶级模型对象。将为模型构造一个编辑上下文")]
    [Parameter]public object? Model { get; set; }
    /// <inheritdoc/>
    [ParameterApiDoc("提交表单时将调用的回调。如果使用此参数，则由您手动触发任何验证",Type= "EventCallback<EditContext>")]
    [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
    /// <inheritdoc/>
    [ParameterApiDoc("当表单验证成功时将调用回调函数", Type = "EventCallback<EditContext>")]
    [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }
    /// <inheritdoc/>
    [ParameterApiDoc("当表单验证失败时将调用回调函数", Type = "EventCallback<EditContext>")]
    [Parameter] public EventCallback<EditContext> OnInvalidSubmit { get; set; }
    /// <inheritdoc/>
    public EditContext? FixedEditContext { get; set; }
    /// <inheritdoc/>
    [ParameterApiDoc("用于验证表单的上下文")]
    [Parameter] public EditContext? EditContext { get; set; }
    /// <inheritdoc/>
    [ParameterApiDoc("表单的内容")]
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