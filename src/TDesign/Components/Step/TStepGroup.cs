namespace TDesign;

/// <summary>
/// 步骤组件的容器。配合 <see cref="TStepItem"/> 组件使用步骤项。
/// </summary>
[CssClass("t-steps")]
[ParentComponent]
public class TStepGroup : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [ParameterApiDoc($"装载 {nameof(TStepItem)} 组件的内容")]
    [Parameter] public override RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 是否为垂直排列。
    /// </summary>
    [ParameterApiDoc("是否为垂直排列")]
    [Parameter][BooleanCssClass("t-steps--vertical", "t-steps--horizontal")] public bool Vertical { get; set; }
    /// <summary>
    /// 设置一个布尔值，表示步骤的顺序是否为倒序。
    /// </summary>
    [ParameterApiDoc("步骤的顺序是否为倒序")]
    [Parameter][BooleanCssClass("t-steps--reserve", "t-steps--positive")] public bool Reserve { get; set; }
    /// <summary>
    /// 设置 <c>true</c> 使用 “.” 作为步骤项的连接标识。否则使用序号。
    /// </summary>
    [ParameterApiDoc("使用 “.” 作为步骤项的连接标识。否则使用序号")]
    [Parameter][BooleanCssClass("t-steps--dot-anchor", "t-steps--default-anchor")] public bool Dot { get; set; }
    /// <summary>
    /// 步骤项分割线风格，默认是 <see cref="StepSeperator.Line"/> 。
    /// </summary>
    [ParameterApiDoc("步骤项分割线风格", Value = "Line")]
    [Parameter] public StepSeperator Seperator { get; set; } = StepSeperator.Line;

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append($"t-steps--{Seperator.GetCssClass()}-separator");
    }
}

/// <summary>
/// 步骤条分隔符。
/// </summary>
public enum StepSeperator
{
    /// <summary>
    /// 线条。
    /// </summary>
    Line,
    /// <summary>
    /// 虚线。
    /// </summary>
    Dash,
    /// <summary>
    /// 箭头。
    /// </summary>
    Arrow
}