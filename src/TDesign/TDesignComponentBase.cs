using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
public abstract class TDesignComponentBase : BlazorComponentBase
{
    protected internal ElementReference Ref { get; set; }

    [CascadingParameter] TPopup? CascadingPopup { get; set; }

    protected bool CanPopup => CascadingPopup != null;

    protected override void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
    {
        base.BuildComponentAttributes(builder, out sequence);

        builder.AddElementReferenceCapture(++sequence, e => Ref = e);
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        //base.BuildAttributes(attributes);

        BuildPopupAttributes(attributes);
    }

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
