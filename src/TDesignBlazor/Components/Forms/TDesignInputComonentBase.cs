using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor.Components;

/// <summary>
/// 输入空间的基类。
/// </summary>
/// <typeparam name="TValue">双向绑定值的类型。</typeparam>
public abstract class TDesignInputComonentBase<TValue> : BlazorInputComponentBase<TValue>
{
    [Parameter] public bool Readonly { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool AutoWidth { get; set; }
    [Parameter] public Size Size { get; set; } = Size.Medium;
    [Parameter] public bool AutoFocus { get; set; }
    [Parameter] public Status Status { get; set; } = Status.Default;
    [Parameter] public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Left;

    protected void BuildInputWrapper(RenderTreeBuilder builder, int sequence, RenderFragment content, string? otherCss = default)
    {
        builder.CreateElement(sequence, "div", wrap =>
        {
            wrap.CreateElement(0, "div", content, new
            {
                @class = HtmlHelper.CreateCssBuilder().Append("t-input")
                            .Append("t-is-readonly", Readonly)
                            .Append("t-is-disabled", Disabled)
                            .Append("t-input--auto-width", AutoWidth)
                            .Append("t-is-focused t-input--focused", AutoFocus)
                            .Append(Status.GetCssClass())
                            .Append($"t-align-{Alignment.GetCssClass()}")
                            .Append(Size.GetCssClass())
                            .Append(otherCss, !string.IsNullOrEmpty(otherCss))
            });
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-input__wrap")
        });
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        base.BuildAttributes(attributes);
        base.AddValueChangedAttribute(attributes);
        if (Disabled)
        {
            attributes["disabled"] = true;
        }
        if (Readonly)
        {
            attributes["readonly"] = true;
        }
        if (AutoFocus)
        {
            attributes["autofocus"] = true;
        }
    }
}
