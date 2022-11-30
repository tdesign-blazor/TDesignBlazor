namespace TDesign;

/// <summary>
/// 表示栅格的行。
/// </summary>
[CssClass("t-row")]
[ParentComponent]
public class TRow : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// 间隔，单位 px。
    /// </summary>
    [Parameter] public int? Gutter { get; set; }
    /// <summary>
    /// 水平对齐方式。
    /// </summary>
    [Parameter][CssClass("t-row--")] public JustifyContent HorizontalAlignment { get; set; } = JustifyContent.Start;
    /// <summary>
    /// 垂直对齐方式。
    /// </summary>
    [Parameter][CssClass("t-row--")] public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"row-gap:{Gutter}px", Gutter.HasValue);
    }
}
