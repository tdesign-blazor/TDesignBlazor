using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 头像。
/// </summary>
[CssClass("t-avatar")]
[ChildComponent(typeof(TAvatarGroup), Optional = true)]
public class TAvatar : TDesignAdditionParameterWithChildContentComponentBase
{
    [CascadingParameter] public TAvatarGroup? CascadingAvatarGroup { get; set; }
    /// <summary>
    /// 头像形状。
    /// </summary>
    [ParameterApiDoc("头像形状", Value =$"{nameof(Shape.Circle)}")]
    [Parameter][CssClass("t-avatar--")] public Shape? Shape { get; set; } = Shape.Circle;
    /// <summary>
    /// 设置头像的图片地址。
    /// </summary>
    [ParameterApiDoc("头像的图片地址")]
    [Parameter] public string? Url { get; set; }
    /// <summary>
    /// 设置头像的字体名称。
    /// </summary>
    [ParameterApiDoc("头像的字体名称")]
    [Parameter] public object? Icon { get; set; }
    /// <summary>
    /// 头像的尺寸。
    /// </summary>
    [ParameterApiDoc("头像的尺寸")]
    [Parameter][CssClass] public Size? Size { get; set; }

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

    protected override void AfterSetParameters(ParameterView parameters)
    {
        if ( CascadingAvatarGroup is not null )
        {
            Size = CascadingAvatarGroup.Size;
        }
    }
}
