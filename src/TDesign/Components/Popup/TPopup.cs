using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace TDesign;
/// <summary>
/// 具备悬浮提示的弹出层。
/// </summary>
public class TPopup : BlazorComponentBase, IHasChildContent
{
    /// <inheritdoc/>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string Content { get; set; }

    [Parameter] public PopupPlacement Placement { get; set; } = PopupPlacement.Top;
    bool Active { get; set; }

    ElementReference _triggerRef;
    ElementReference _tipRef;
    PopperInstance _instance;
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "span");
        builder.AddAttribute(3, "onmouseenter", HtmlHelper.CreateCallback<MouseEventArgs>(this, Show));
        builder.AddAttribute(4, "onmouseleave", HtmlHelper.CreateCallback<MouseEventArgs>(this, Hide));
        builder.AddElementReferenceCapture(1, reference =>
        {
            _triggerRef = reference;
        });
        builder.AddContent(10, ChildContent);

        if ( Active )
        {
            builder.OpenRegion(10);
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "t-popup");
            //builder.AddAttribute(2, "style", "position:absolute");
            builder.AddElementReferenceCapture(8, e => _tipRef = e);
            builder.AddContent(10, content =>
            {
                content.OpenElement(0, "div");
                content.AddAttribute(1, "class", "t-popup__content");
                //content.AddAttribute(2, "style", $"visibility:hidden");
                content.AddContent(10, Content);
                content.CloseElement();
            });
            builder.CloseElement();
            builder.CloseRegion();

        }

        builder.CloseElement();
        //builder.CreateElement(0, "div", content =>
        //{
        //    content.AddContent(0, ChildContent);
        //    builder.CreateElement(1, "div", content =>
        //    {
        //        base.BuildRenderTree(content);
        //    }, new
        //    {
        //        style = "width: 100%;top:0px",
        //        @class = HtmlHelper.CreateCssBuilder().Append(Placement.GetCssClass())
        //    });
        //},
        //new
        //{
        //    style = "position:relative",
        //    //onmouseover = HtmlHelper.CreateCallback<MouseEventArgs>(this, Toggle),
        //    //onmouseout = HtmlHelper.CreateCallback<MouseEventArgs>(this, Toggle)
        //}, appendFunc: (b, i) =>
        //{
        //    b.SetKey(this);
        //    b.AddElementReferenceCapture(i + 1, reference =>
        //    {
        //        _elementReference = reference;
        //    });
        //    return i;
        //});
    }

    async Task Show(MouseEventArgs e)
    {
        _instance = await JS.Value.InvokePopupAsync(_triggerRef, _tipRef, new()
        {
            Placement = Placement
        });
        Active = true;
        StateHasChanged();
    }

    async Task Hide(MouseEventArgs e)
    {
        if ( _instance is not null )
        {
            await _instance.Destroy();
            //Active = false;
            StateHasChanged();
        }
    }
}
