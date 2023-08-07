namespace TDesign;
/// <summary>
/// 超链接文本。
/// </summary>
[CssClass("t-link")]
[HtmlTag("a")]
public class TLink : TDesignAdditionParameterWithChildContentComponentBase, IHasDisabled
{
    /// <summary>
    /// 文字尺寸。
    /// </summary>
    [ParameterApiDoc("文字的尺寸",Value =$"{nameof(Size.Medium)}")]
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 文字颜色。
    /// </summary>
    [ParameterApiDoc("文字的颜色")]
    [Parameter][CssClass("t-link--theme-")] public Theme? Theme { get; set; }
    /// <summary>
    /// 显示超链接的下划线。
    /// </summary>
    [ParameterApiDoc("显示下划线")]
    [Parameter][CssClass("t-is-underline")] public bool Underline { get; set; }
    /// <summary>
    /// 设置鼠标悬停时的效果。
    /// </summary>
    [ParameterApiDoc("鼠标悬停时的效果")]
    [Parameter][CssClass("t-link--hover-")] public LinkHover? Hover { get; set; }
    /// <summary>
    /// 禁用超链接。
    /// </summary>
    [ParameterApiDoc("禁用超链接")]
    [Parameter][CssClass("t-is-disabled")] public bool Disabled { get; set; }
    /// <summary>
    /// 超链接的地址。同 <c>href</c> 属性。
    /// </summary>
    [ParameterApiDoc("超链接的地址")]
    [Parameter][HtmlAttribute] public string? Href { get; set; }


}
/// <summary>
/// 链接的悬停效果。
/// </summary>
public enum LinkHover
{
    /// <summary>
    /// 下划线模式。
    /// </summary>
    Underline,
    /// <summary>
    /// 高亮颜色模式。
    /// </summary>
    Color
}