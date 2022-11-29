using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 输入控件的基类。
/// </summary>
/// <typeparam name="TValue">双向绑定值的类型。</typeparam>
public abstract class TDesignInputComonentBase<TValue> : BlazorInputComponentBase<TValue>, IHasAdditionalCssClass
{
    /// <summary>
    /// 获取当前组件的元素引用。
    /// </summary>
    protected ElementReference Ref { get; private set; }
    /// <summary>
    /// 设置只读模式。
    /// </summary>
    [Parameter] public bool Readonly { get; set; }
    /// <summary>
    /// 设置禁用状态。
    /// </summary>
    [Parameter] public bool Disabled { get; set; }
    /// <summary>
    /// 自适应宽度。
    /// </summary>
    [Parameter] public bool AutoWidth { get; set; }
    /// <summary>
    /// 尺寸。
    /// </summary>
    [Parameter] public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 自动聚焦。
    /// </summary>
    [Parameter] public bool AutoFocus { get; set; }
    /// <summary>
    /// 状态。
    /// </summary>
    [Parameter][CssClass("t-is-")] public Status Status { get; set; } = Status.Default;
    /// <summary>
    /// 输入框提示的内容。
    /// </summary>
    [Parameter] public RenderFragment? TipContent { get; set; }
    /// <summary>
    /// 对齐方式。
    /// </summary>
    [Parameter] public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Left;
    /// <inheritdoc/>    
    [Parameter] public string? AdditionalCssClass { get; set; }

    /// <summary>
    /// Builds the input wrapper.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="sequence">The sequence.</param>
    /// <param name="content">The content.</param>
    /// <param name="otherCss">The other css.</param>
    protected virtual void BuildInputWrapper(RenderTreeBuilder builder, int sequence, RenderFragment content, string? otherCss = default)
    {
        builder.CreateElement(sequence, "div", wrap =>
        {
            wrap.CreateElement(0, "div", content, new
            {
                @class = HtmlHelper.Class.Append("t-input")
                            .Append("t-is-readonly", Readonly)
                            .Append("t-is-disabled", Disabled)
                            .Append("t-input--auto-width", AutoWidth)
                            .Append("t-is-focused t-input--focused", AutoFocus)
                            .Append($"t-is-{Status.GetCssClass()}")
                            .Append($"t-align-{Alignment.GetCssClass()}")
                            .Append(Size.GetCssClass())
                            .Append(otherCss, !string.IsNullOrEmpty(otherCss))
            });

            wrap.CreateElement(1, "div", TipContent, new
            {
                @class = HtmlHelper.Class.Append("t-input__tips")
                .Append($"t-input__tips--{Status.GetCssClass()}")
            }, TipContent is not null);
        }, new
        {
            @class = HtmlHelper.Class.Append("t-input__wrap").Append(AdditionalCssClass)
        });
    }

    /// <inheritdoc/>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        base.BuildAttributes(attributes);
        base.AddValueChangedAttribute(attributes);
        attributes["disabled"] = Disabled;
        attributes["readonly"] = Readonly;
        attributes["autofocus"] = AutoFocus;

        BuildPopupAttributes(attributes);
    }



    /// <summary>
    /// 级联 TPopup 组件。
    /// </summary>
    [CascadingParameter] TPopup? CascadingPopup { get; set; }

    /// <summary>
    /// 获取一个布尔值，表示是否可以具备弹出层的功能。
    /// </summary>
    protected bool CanPopup => CascadingPopup != null;

    /// <inheritdoc/>
    protected override void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
    {
        base.BuildComponentAttributes(builder, out sequence);

        builder.AddElementReferenceCapture(++sequence, e => Ref = e);
    }

    /// <summary>
    /// 构建 Popup 相关的属性。
    /// </summary>
    /// <param name="attributes">The attributes for components.</param>
    protected virtual void BuildPopupAttributes(IDictionary<string, object> attributes)
    {
        if (!CanPopup)
        {
            return;
        }

        switch (CascadingPopup!.Trigger)
        {
            case PopupTrigger.Click:
                attributes["onclick"] = HtmlHelper.Event.Create<MouseEventArgs>(this, TogglePopup);
                break;
            case PopupTrigger.Hover:
                attributes["onmouseenter"] = HtmlHelper.Event.Create<MouseEventArgs>(this, ShowPopup);
                attributes["onmouseleave"] = HtmlHelper.Event.Create<MouseEventArgs>(this, HidePopup);
                break;
            case PopupTrigger.Focus:
                attributes["onfocus"] = HtmlHelper.Event.Create<FocusEventArgs>(this, ShowPopup);
                attributes["onblur"] = HtmlHelper.Event.Create<FocusEventArgs>(this, HidePopup);
                break;
            case PopupTrigger.ContextMenu:
                attributes["onclick"] = HtmlHelper.Event.Create<MouseEventArgs>(this, e =>
                {
                    if (e.Button != 2)
                    {
                        return Task.CompletedTask;
                    }

                    return TogglePopup();
                });
                break;
            default:
                break;
        }
    }

    protected virtual Task ShowPopup()
    {
        if (!CanPopup)
        {
            return Task.CompletedTask;
        }
        return CascadingPopup.Show(Ref);
    }

    protected virtual Task HidePopup()
    {
        if (!CanPopup)
        {
            return Task.CompletedTask;
        }
        return CascadingPopup.Hide();
    }

    protected virtual async Task TogglePopup()
    {
        if (!CanPopup)
        {
            return;
        }

        if (!CascadingPopup.Visible)
        {
            await ShowPopup();
        }
        else
        {
            await HidePopup();
        }
    }
}
