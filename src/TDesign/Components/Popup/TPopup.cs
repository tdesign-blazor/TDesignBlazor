using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 具备悬浮提示的弹出层。
/// </summary>
[CssClass("t-popup")]
public class TPopup : BlazorComponentBase, IHasChildContent
{
    /// <inheritdoc/>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 设置弹出层的显示位置。
    /// </summary>
    [Parameter] public PopperPlacement Placement { get; set; } = PopperPlacement.Auto;


    /// <summary>
    /// 获取火设置弹出层的内容。
    /// </summary>
    [Parameter] public OneOf<string?, RenderFragment?, MarkupString?> Content { get; set; }
    /// <summary>
    /// 设置弹出层是否具备箭头指向。
    /// </summary>
    [Parameter] public bool Arrow { get; set; }
    /// <summary>
    /// 设置是否显示弹出层。
    /// </summary>
    [Parameter] public bool Visible { get; set; }


    ElementReference _tipRef;
    PopperInstance? _instance;


    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        this.CreateCascadingComponent(builder, 0, inner =>
        {
            inner.AddContent(0, ChildContent);
        }, "Popup", true);

        builder.CreateElement(0, "div", content =>
        {
            builder.CreateElement(0, "div", inner =>
            {
                Content.Switch(str => inner.AddContent(0, str),
                                fragment => inner.AddContent(0, fragment),
                                markup => inner.AddContent(0, markup));

                if (Arrow)
                {
                    inner.Div().Class("t-popup__arrow").Close();
                }
            },
            new
            {
                @class = HtmlHelper.Class.Append("t-popup__content").Append("t-popup__content--arrow", Arrow)
            });
        },
        new
        {
            @class = HtmlHelper.Class.Append("t-popup").Append("visibility-hidden", !Visible)
        },
        captureReference: e => _tipRef = e);
    }

    public async Task Show(string selector)
    {
        Visible = true;
        Task.Delay(5);
        _instance = await JS.Value.InvokePopupAsync(selector, _tipRef, new()
        {
            Placement = Placement
        });

    }

    public async Task Hide()
    {
        if (_instance is not null)
        {
            await _instance.DestroyAsync();
            Visible = false;
        }
    }
}

/// <summary>
/// 弹出层的触发选项。
/// </summary>
public enum PopupTrigger
{
    /// <summary>
    /// 左键点击。
    /// </summary>
    Click,
    /// <summary>
    /// 鼠标悬停。
    /// </summary>
    Hover,
    /// <summary>
    /// 焦点集中。
    /// </summary>
    Focus,
    /// <summary>
    /// 右键点击。
    /// </summary>
    ContextMenu,

}