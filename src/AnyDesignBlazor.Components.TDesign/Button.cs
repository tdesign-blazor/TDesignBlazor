using Microsoft.AspNetCore.Components.Rendering;

namespace AnyDesignBlazor.Components.TDesign;

/// <summary>
/// 表示用于开启一个闭环的操作任务的按钮。
/// </summary>
[CssClass("t-button")]
public class Button : ButtonBase
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter][CssClass("t-button--variant-")] public ButtonType Type { get; set; } = ButtonType.Base;

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
    [Parameter][CssClass("t-button--shape-")] public ButtonShape? Shape { get; set; }
    /// <summary>
    /// 禁用状态。
    /// </summary>
    [Parameter][CssClass("t-is-disabled")][HtmlAttribute] public override bool Disabled { get => base.Disabled; set => base.Disabled = value; }
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

public enum ButtonType
{
    Base,
    Outline,
    Dashed,
    Text
}


public enum ButtonShape
{
    Squre,
    Round,
    Circle
}