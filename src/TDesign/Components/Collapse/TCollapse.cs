namespace TDesign;

/// <summary>
/// 表示具备折叠面板的组容器，并使用 <see cref="TCollapsePanel"/> 展现折叠面板的项。
/// </summary>
[CssClass("t-collapse")]
[ParentComponent]
public class TCollapse : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// 仅点击图标才可以触发展开或折叠的响应。
    /// </summary>
    [ParameterApiDoc("仅点击图标才可以触发展开或折叠的响应")]
    [Parameter] public bool IconToggle { get; set; }

    /// <summary>
    /// 设置 <c>true</c> 图标处于标题的右边，<c>null</c> 不用图标，<c>false</c> 图标处于标题左边。
    /// </summary>
    [ParameterApiDoc("设置 true 图标处于标题的右边，null 不用图标，false 图标处于标题左边。")]
    [Parameter] public bool? RightIcon { get; set; } = false;
    /// <summary>
    /// 设置为无边框。
    /// </summary>
    [ParameterApiDoc("无边框")]
    [Parameter][CssClass("t--border-less")] public bool Borderless { get; set; }
    /// <summary>
    /// 设置为手风琴模式，即只能同时展开1个面板。
    /// </summary>
    [ParameterApiDoc("是否为手风琴模式，即只能同时展开1个面板")]
    [Parameter] public bool Mutex { get; set; }
}
