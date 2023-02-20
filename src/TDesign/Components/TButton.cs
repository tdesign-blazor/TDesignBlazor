using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 表示用于开启一个闭环的操作任务的按钮。
/// </summary>
[HtmlTag("button")]
[CssClass("t-button")]
public class TButton : TDesignComponentBase, IHasOnClick, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter][HtmlAttribute("onclick")] public EventCallback<MouseEventArgs?> OnClick { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 设置按钮的 HTML 类型。默认时 <see cref="ButtonHtmlType.Button"/> 类型。
    /// </summary>
    [Parameter][HtmlAttribute("type")] public ButtonHtmlType HtmlType { get; set; } = ButtonHtmlType.Button;
    /// <summary>
    /// 按钮类型的风格。
    /// </summary>
    [Parameter][CssClass("t-button--variant-")] public ButtonVarient Varient { get; set; } = ButtonVarient.Base;

    /// <summary>
    /// 主题颜色。
    /// </summary>
    [Parameter][CssClass("t-button--theme-")] public Theme Theme { get; set; } = Theme.Default;
    /// <summary>
    /// 幽灵按钮。内容反色，背景变为透明，一般是底色透明
    /// </summary>
    [Parameter][CssClass("t-button--ghost")] public bool Ghost { get; set; }
    /// <summary>
    /// 提供大、中（默认）、小三种尺寸。
    /// </summary>
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 宽度上充满其所在的父容器。
    /// </summary>
    [Parameter][CssClass("t-size-full-width")] public bool Block { get; set; }

    /// <summary>
    /// 按钮形状。
    /// </summary>
    [Parameter][CssClass("t-button--shape-")] public ButtonShape Shape { get; set; } = ButtonShape.Rectangle;
    /// <summary>
    /// 禁用状态。
    /// </summary>
    [Parameter][CssClass("t-is-disabled")][HtmlAttribute] public bool Disabled { get; set; }
    /// <summary>
    /// 加载状态。
    /// </summary>
    [Parameter][CssClass("t-is-loading")] public bool Loading { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "span", content =>
        {
            base.AddContent(content, sequence);
        }, new { @class = "t-button__text" });
    }
}

/// <summary>
/// 按钮风格。
/// </summary>
public enum ButtonVarient
{
    /// <summary>
    /// 填充风格。
    /// </summary>
    Base,
    /// <summary>
    /// 描边风格。
    /// </summary>
    Outline,
    /// <summary>
    /// 虚框风格。
    /// </summary>
    Dashed,
    /// <summary>
    /// 文字风格。
    /// </summary>
    Text
}

/// <summary>
/// 按钮形状。
/// </summary>
public enum ButtonShape
{
    /// <summary>
    /// 长方形。
    /// </summary>
    Rectangle,
    /// <summary>
    /// 正方形。
    /// </summary>
    Square,
    /// <summary>
    /// 圆角长方形。
    /// </summary>
    Round,
    /// <summary>
    /// 圆形。
    /// </summary>
    Circle
}
/// <summary>
/// 按钮的 HTML 类型。
/// </summary>
public enum ButtonHtmlType
{
    /// <summary>
    /// 表示为 button 普通按钮。
    /// </summary>
    Button,
    /// <summary>
    /// 表示为 submit 提交按钮，可触发 form 的提交功能。
    /// </summary>
    Submit,
    /// <summary>
    /// 表示为 reset 重置按钮，可触发 form 的重置功能。
    /// </summary>
    Reset
}