using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace TDesign;
/// <summary>
/// 具备悬浮提示的弹出层。
/// </summary>
[CssClass("t-popup")]
public class TPopup : BlazorComponentBase, IHasChildContent, IHasActive
{
    /// <inheritdoc/>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string Content { get; set; }

    [Parameter] public Placement Placement { get; set; } = Placement.TopCenter;
    [Parameter] public bool Active { get; set; }

    ElementReference _triggerRef;
    ElementReference _tipRef;
    object _popupRef;
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "span");
        builder.AddAttribute(3, "onmouseover", HtmlHelper.CreateCallback<MouseEventArgs>(this, Show));
        builder.AddAttribute(4, "onmouseout", HtmlHelper.CreateCallback<MouseEventArgs>(this, Hide));
        builder.AddElementReferenceCapture(1, reference =>
        {
            _triggerRef = reference;
        });
        builder.AddContent(10, ChildContent);

        builder.CloseElement();

        builder.OpenRegion(10);
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "t-popup");
        builder.AddContent(10, (RenderFragment)(content =>
        {
            content.OpenElement(0, "div");
            content.AddAttribute(1, "class", "t-popup__content");
            content.AddElementReferenceCapture(8, e => _tipRef = e);
            content.AddContent(10, Content);
            content.CloseElement();
        }));
        builder.CloseElement();
        builder.CloseRegion();

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

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", Content, new { @class = "t-popup__content" });
    }
    protected override void BuildStyle(IStyleBuilder builder)
    {
        //builder.Append("position: absolute; inset: auto auto 0px 0px; margin: 8px;", Active)
        //    .Append("display:none", !Active)
        //    ;
    }

    async Task Show(MouseEventArgs e)
    {
        _popupRef = await JS.Value.InvokeAsync<object>("tdesign.popup.show", new object[] { _triggerRef, _tipRef, "top" });
    }

    async Task Hide(MouseEventArgs e)
    {
        await JS.Value.InvokeVoidAsync("tdesign.popup.destroy", new[] { _popupRef });
    }
}
