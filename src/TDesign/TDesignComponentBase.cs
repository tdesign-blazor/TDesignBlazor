using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Options;

namespace TDesign;
/// <summary>
/// 表示 TDesign 组件的基类。
/// </summary>
public abstract class TDesignComponentBase : BlazorComponentBase
{
    /// <summary>
    /// 获取注入的配置。
    /// </summary>
    [Inject]protected IOptions<TDesignOptions> Options { get; set; }
    /// <summary>
    /// 级联 TPopup 组件。
    /// </summary>
    [CascadingParameter] TPopup? CascadingPopup { get; set; }

    /// <summary>
    /// 获取一个布尔值，表示是否可以具备弹出层的功能。
    /// </summary>
    protected bool CanPopup => CascadingPopup != null;

    /// <summary>
    /// 如果重写，请手动调用 <see cref="BuildPopupAttributes(IDictionary{string, object})"/> 方法。
    /// </summary>
    /// <param name="attributes">The attributes for components.</param>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        BuildPopupAttributes(attributes);
    }

    public override void AddChildComponent(IBlazorComponent component)
    {
        base.AddChildComponent(component);
        StateHasChanged();
    }

    //TODO 换成 Interceptor 实现
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
                //attributes["onblur"] = HtmlHelper.Event.Create<FocusEventArgs>(this, HidePopup);
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

    protected override void CaptureElementReference(RenderTreeBuilder builder, int sequence)
    {
        if ( CanPopup )
        {
            builder.AddElementReferenceCapture(sequence, element => Reference = element);
        }
        else
        {
            base.CaptureElementReference(builder, sequence);
        }
    }

    protected virtual Task ShowPopup()
    {
        if (!CanPopup)
        {
            return Task.CompletedTask;
        }
        return CascadingPopup.Show(this);
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

/// <summary>
/// 带 ChildContent 参数的 TDesign 组件基类。
/// </summary>
public abstract class TDesignChildContentComponentBase : TDesignComponentBase, IHasChildContent
{
    /// <inheritdoc/>
    [Parameter]public RenderFragment? ChildContent { get; set; }
}

/// <summary>
/// 带 AdditionalClass 和 AdditionalStyle 参数的 TDesign 组件基类。
/// </summary>
public abstract class TDesignAdditionParameterComponentBase : TDesignComponentBase, IHasAdditionalClass, IHasAdditionalStyle
{
    /// <inheritdoc/>
    [Parameter]public string? AdditionalStyle { get; set; }
    /// <inheritdoc/>
    [Parameter]public string? AdditionalClass { get; set; }
}
/// <summary>
/// 带 ChildContent AdditionalClass 和 AdditionalStyle 参数的 TDesign 组件基类。
/// </summary>
public abstract class TDesignAdditionParameterWithChildContentComponentBase : TDesignChildContentComponentBase, IHasAdditionalClass, IHasAdditionalStyle
{
    /// <inheritdoc/>
    [Parameter] public string? AdditionalStyle { get; set; }
    /// <inheritdoc/>
    [Parameter] public string? AdditionalClass { get; set; }
}