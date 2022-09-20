using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;

/// <summary>
/// 表示用于开启一个闭环的操作任务的按钮。
/// </summary>
[HtmlTag("button")]
[CssClass("t-button")]
public class Button : BlazorComponentBase, IHasOnClick, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter][HtmlEvent("onclick")] public EventCallback<MouseEventArgs?> OnClick { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 设置按钮的 HTML 类型。默认时 <see cref="ButtonHtmlType.Button"/> 类型。
    /// </summary>
    [Parameter][HtmlAttribute("type")] public ButtonHtmlType HtmlType { get; set; } = ButtonHtmlType.Button;
    /// <summary>
    /// 
    /// </summary>
    [Parameter][CssClass("t-button--variant-")] public ButtonType Type { get; set; } = ButtonType.Base;

    /// <summary>
    /// 主题颜色。
    /// </summary>
    [Parameter][CssClass("t-button--theme-")] public Theme? Theme { get; set; }
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
    [Parameter][CssClass("t-button--shape-")] public ButtonShape? Shape { get; set; }
    /// <summary>
    /// 禁用状态。
    /// </summary>
    [Parameter][CssClass("t-is-disabled")] public bool Disabled { get; set; }
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

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["disabled"] = Disabled;
    }
}

/// <summary>
/// 按钮风格。
/// </summary>
public enum ButtonType
{
    Base,
    Outline,
    Dashed,
    Text
}

/// <summary>
/// 按钮形状。
/// </summary>
public enum ButtonShape
{
    Squre,
    Round,
    Circle
}
/// <summary>
/// 按钮的 HTML 类型。
/// </summary>
public enum ButtonHtmlType
{
    Button,
    Submit,
    Reset
}