using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 具备悬浮提示的弹出层。
/// </summary>
[CssClass("t-popup")]
public class TPopup : TDesignComponentBase, IHasChildContent
{

    const string HIDDEN_CLASS = "visibility-hidden";

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

    [Parameter] public PopupTrigger Trigger { get; set; } = PopupTrigger.Hover;

    PopperInstance? _instance;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateCascadingComponent(this, 0, ChildContent, isFixed: true);

        base.BuildRenderTree(builder);
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", inner =>
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
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if (Visible)
        {
            builder.Remove(HIDDEN_CLASS);
        }
        else
        {
            builder.Append(HIDDEN_CLASS);
        }
    }

    public async Task Show(ElementReference objRef)
    {
        Visible = true;
        this.Refresh();
        //Task.Delay(5);
        _instance = await JS.Value.InvokePopupAsync(objRef, Ref, new()
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
            this.Refresh();
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