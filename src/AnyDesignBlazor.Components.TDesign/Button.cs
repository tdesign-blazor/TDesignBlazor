using ComponentBuilder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AnyDesignBlazor.Components.TDesign;
[CssClass("t-button")]
public class Button : ButtonBase
{
    [Parameter][CssClass("t-button--variant-")] public ButtonVariant Variant { get; set; } = ButtonVariant.Base;

    [Parameter][CssClass("t-button--theme-")] public ButtonTheme Theme { get; set; } = ButtonTheme.Default;

    [Parameter][CssClass("t-button--ghost")] public bool Ghost { get; set; }
    [Parameter][CssClass("t-button--size-")] public ButtonSize Size { get; set; } = ButtonSize.Small;
    [Parameter][CssClass("t-size-full-width")] public bool Block { get; set; }

    [Parameter][CssClass("t-button--shape-")] public ButtonShape? Shape { get; set; }

    [Parameter][CssClass("t-is-disabled")][HtmlAttribute] public override bool Disabled { get => base.Disabled; set => base.Disabled = value; }

    [Parameter][CssClass("t-is-loading")] public bool Loading { get; set; }


    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "span", content =>
        {

            base.AddContent(content, sequence);
        }, new { @class = "t-button__text" });
    }
}

public enum ButtonVariant
{
    Base,
    Outline,
    Dashed,
    Text
}

public enum ButtonTheme
{
    Default,
    Primary,
    Danger,
    Warning,
    Success
}

public enum ButtonSize
{
    Small,
    Medium,
    Large
}

public enum ButtonShape
{
    Squre,
    Round,
    Circle
}