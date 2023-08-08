namespace TDesign;

/// <summary>
/// 显示程序正在处理的加载提示。
/// </summary>
[CssClass("t-loading")]
public class TLoading : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// 位置是否处于容器的正中间。
    /// </summary>
    [ParameterApiDoc("位置是否处于容器的正中间",Value ="true")]
    [Parameter][CssClass("t-loading--center")] public bool Center { get; set; } = true;

    /// <summary>
    /// 尺寸。
    /// </summary>
    [ParameterApiDoc("尺寸", Value = "Medium")]
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 设置是否显示遮罩层。
    /// </summary>
    [ParameterApiDoc("是否显示遮罩层")]
    [Parameter][CssClass("t-loading__overlay")] public bool Overlay { get; set; }
    /// <summary>
    /// 是否全屏显示。
    /// </summary>
    [ParameterApiDoc("是否全屏显示")]
    [Parameter][CssClass("t-loading__fullscreen")] public bool FullScreen { get; set; }

    /// <summary>
    /// 表示只显示文字，不显示加载图标。
    /// </summary>
    [ParameterApiDoc("只显示文字，不显示加载图标")]
    [Parameter] public bool TextOnly { get; set; }

    /// <summary>
    /// 获取或设置是否可见。
    /// </summary>
    [ParameterApiDoc("可见性设置")]
    [Parameter] public bool Visible { get; set; } = true;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Visible)
        {
            base.BuildRenderTree(builder);
        }
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateComponent<TIcon>(sequence, attributes: new { Name = IconName.Loading, AdditionalClass = "t-loading__gradient" }, condition: !TextOnly);

        builder.CreateElement(sequence + 1, "div", ChildContent, new { @class = "t-loading__text" }, ChildContent is not null);
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-loading--full", Overlay && !FullScreen);
    }
}
