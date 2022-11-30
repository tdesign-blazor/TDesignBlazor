using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 头像。
/// </summary>
[CssClass("t-avatar")]
[ChildComponent(typeof(TAvatarGroup), Optional = true)]
public class TAvatar : TDesignComponentBase, IHasChildContent
{
    [CascadingParameter] public TAvatarGroup? CascadingAvatarGroup { get; set; }
    /// <summary>
    /// 头像形状。
    /// </summary>
    [Parameter][CssClass("t-avatar--")] public Shape? Shape { get; set; } = Shape.Circle;
    /// <summary>
    /// 设置头像的图片地址。
    /// </summary>
    [Parameter] public string? Url { get; set; }
    /// <summary>
    /// 设置头像的字体名称。
    /// </summary>
    [Parameter] public object? Icon { get; set; }
    /// <summary>
    /// 头像的尺寸。
    /// </summary>
    [Parameter][CssClass] public Size? Size { get; set; }
    /// <summary>
    /// 头像显示的自定义内容。
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "img", attributes: new { src = Url }, condition: !string.IsNullOrEmpty(Url));
        builder.CreateComponent<TIcon>(sequence + 1, attributes: new { Name = Icon }, condition: Icon is not null);
        builder.CreateElement(sequence + 2, "span", ChildContent, new { style = "transform:scale(1)" }, ChildContent is not null);
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-avatar__icon", Icon is not null);
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (CascadingAvatarGroup is not null && CascadingAvatarGroup.Size is not null)
        {
            Size = CascadingAvatarGroup.Size;
        }
    }
}
