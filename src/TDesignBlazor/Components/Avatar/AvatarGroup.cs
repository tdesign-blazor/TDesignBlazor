namespace TDesignBlazor;

/// <summary>
/// 表示头像组的容器。
/// </summary>
[ParentComponent]
[CssClass("t-avatar-group")]
public class AvatarGroup : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// 头像排列的方向。
    /// </summary>
    [Parameter][BooleanCssClass("t-avatar--offset-right", "t-avatar--offset-left")] public bool Left { get; set; }
    /// <summary>
    /// 头像组尺寸。无须再单个设置 <see cref="Avatar.Size"/> 参数。
    /// </summary>
    [Parameter] public Size? Size { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
