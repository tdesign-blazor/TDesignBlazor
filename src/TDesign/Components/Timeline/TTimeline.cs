namespace TDesign;

/// <summary>
/// 时间轴组件的容器，用于时间轴展示。配合 <see cref="TTimelineItem"/> 组件使用。
/// </summary>
[ParentComponent]
[CssClass("t-timeline")]
[HtmlTag("ul")]
public class TTimeline : TDesignComponentBase, IHasChildContent
{
    /// <inheritdoc/>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 设置水平显示。
    /// </summary>
    [Parameter][BooleanCssClass("t-timeline-horizontal", "t-timeline-vertical")] public bool Horizontal { get; set; }
    /// <summary>
    /// 设置标签展示在轴两侧。
    /// </summary>
    [Parameter][BooleanCssClass("t-timeline-label--alternate", "t-timeline-label--same")] public bool Alternate { get; set; }


    /// <summary>
    /// 设置时间轴的主题，默认是 <see cref="TimelineTheme.Default"/> 。
    /// </summary>
    [Parameter] public TimelineTheme Theme { get; set; } = TimelineTheme.Default;

    /// <summary>
    /// 设置标签信息放在时间轴的位置，当 <see cref="Alternate"/> 是 <c>true</c> 时有效。
    /// </summary>
    [Parameter][CssClass("t-timeline-")] public TimelineLabelAlignment LabelAlignment { get; set; } = TimelineLabelAlignment.Left;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();


        if ( ChildComponents.Any() )
        {
            if ( ChildComponents.Last() is TTimelineItem timelineItem )
            {
                //TODO：要判断最后一条并追加 CSS
                timelineItem.AdditionalClass += "t-timeline-item--last";
                timelineItem.Refresh();
            }
        }
    }


    /// <inheritdoc/>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-timeline-label");
    }
}

/// <summary>
/// 时间轴显示主题。
/// </summary>
public enum TimelineTheme
{
    /// <summary>
    /// 直线。
    /// </summary>
    Default,
    /// <summary>
    /// 虚线。
    /// </summary>
    Dot
}
/// <summary>
/// 标签信息放在时间轴的位置。
/// </summary>
public enum TimelineLabelAlignment
{
    /// <summary>
    /// 左侧。
    /// </summary>
    Left,
    /// <summary>
    /// 右侧。
    /// </summary>
    Right,
    /// <summary>
    /// 两侧。
    /// </summary>
    Alternate,
    /// <summary>
    /// 上侧。
    /// </summary>
    Top,
    /// <summary>
    /// 下侧。
    /// </summary>
    Bottom
}