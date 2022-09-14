namespace TDesignBlazor.Components;

/// <summary>
/// 表示作为表单的组件。
/// </summary>
[CssClass("t-form")]
public class Form : BlazorFormComponentBase<Form>
{
    /// <summary>
    /// 设置作为行内布局。
    /// </summary>
    [Parameter] public bool Inline { get; set; }

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
    Top,
    Left,
    Right,
}