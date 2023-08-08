namespace TDesign;

/// <summary>
/// 表示栅格的行。
/// </summary>
[CssClass("t-row")]
[ParentComponent]
public class TRow : TDesignAdditionParameterComponentBase, IHasChildContent
{
    /// <summary>
    /// 间隔，单位 px。
    /// </summary>
    [ParameterApiDoc("行间隔，单位 px")]
    [Parameter] public int? Gutter { get; set; }
    /// <summary>
    /// 水平对齐方式。
    /// </summary>
    [ParameterApiDoc("水平对齐方式", Value = "Start")]
    [Parameter][CssClass("t-row--")] public JustifyContent HorizontalAlignment { get; set; } = JustifyContent.Start;
    /// <summary>
    /// 垂直对齐方式。
    /// </summary>
    [ParameterApiDoc("垂直对齐方式", Value = "Top")]
    [Parameter][CssClass("t-row--")] public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [ParameterApiDoc($"装载 {nameof(TColumn)} 组件的内容")]
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"row-gap:{Gutter}px", Gutter.HasValue);
    }
}
