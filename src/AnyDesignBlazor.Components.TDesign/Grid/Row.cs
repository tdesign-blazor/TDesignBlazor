using ComponentBuilder.Abstrations;

namespace AnyDesignBlazor.Components.TDesign;

/// <summary>
/// 表示栅格的行。
/// </summary>
[CssClass("t-row")]
[ParentComponent]
public class Row : BlazorChildContentComponentBase
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

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"row-gap:{Gutter}px", Gutter.HasValue);
    }
}
