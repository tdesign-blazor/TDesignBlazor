namespace TDesign;

/// <summary>
/// 组件之间具备一定间距的布局组件。
/// </summary>
[ParentComponent]
[CssClass("t-space")]
public class TSpace : TDesignAdditionParameterComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [ParameterApiDoc($"装载 {nameof(TSpaceItem)} 组件的内容")]
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 是否为竖向排列。
    /// </summary>
    [ParameterApiDoc($"是否为竖向排列")]
    [Parameter][BooleanCssClass("t-space-vertical", "t-space-horizontal")] public bool Vertical { get; set; }
    /// <summary>
    /// <see cref="TSpaceItem"/> 之间的间距。
    /// </summary>
    [ParameterApiDoc($"每一个 {nameof(TSpaceItem)} 的间距", Value ="16px")]
    [Parameter] public string? Gap { get; set; } = "16px";
    /// <summary>
    /// 水平排列时是否可以被自动换行。
    /// </summary>
    [ParameterApiDoc($"水平排列时是否可以被自动换行")]
    [Parameter] public bool BreakLine { get; set; }

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"gap:{Gap}", !string.IsNullOrEmpty(Gap)).Append("flex-wrap:wrap", BreakLine);
    }
}
