

namespace AnyDesignBlazor.Components;
[HtmlTag("button")]
public class ButtonBase : BlazorComponentBase, IHasOnClick, IHasChildContent
{
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public ButtonHtmlType HtmlType { get; set; } = ButtonHtmlType.Button;

    [Parameter][HtmlAttribute] public virtual bool Disabled { get; set; }
}

public enum ButtonHtmlType
{
    Button,
    Submit,
    Reset
}