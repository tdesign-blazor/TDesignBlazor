namespace TDesignBlazor;

/// <summary>
/// 组件之间具备一定间距的布局组件。
/// </summary>
[ParentComponent]
[CssClass("t-space")]
public class Space : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 是否为竖向排列。
    /// </summary>
    [Parameter][BooleanCssClass("t-space-vertical", "t-space-horizontal")] public bool Vertical { get; set; }
    /// <summary>
    /// <see cref="SpaceItem"/> 之间的间距。
    /// </summary>
    [Parameter] public string? Gap { get; set; } = "16px";
    /// <summary>
    /// 水平排列时是否可以被自动换行。
    /// </summary>
    [Parameter] public bool BreakLine { get; set; }

    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append($"gap:{Gap}", !string.IsNullOrEmpty(Gap)).Append("flex-wrap:wrap", BreakLine);
    }
}
