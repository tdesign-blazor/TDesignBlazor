namespace TDesignBlazor;

/// <summary>
/// 表示作为表单的组件。
/// </summary>
[CssClass("t-form")]
public class Form : BlazorFormComponentBase<Form>
{
    /// <summary>
    /// 设置作为行内布局。
    /// </summary>
    [Parameter][CssClass("t-form-inline")] public bool Inline { get; set; }

    /// <summary>
    /// 表单的对齐方式。默认时 <see cref="FormAlignment.Right"/> 。
    /// </summary>
    [Parameter] public FormAlignment Alignment { get; set; } = FormAlignment.Right;
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