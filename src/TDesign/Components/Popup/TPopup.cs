using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 具备悬浮提示的弹出层。
/// </summary>
[CssClass("t-popup")]
public class TPopup : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// 隐藏的 class。
    /// </summary>
    const string HIDDEN_CLASS = "hide";

    /// <inheritdoc/>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 设置弹出层的显示位置。
    /// </summary>
    [Parameter] public PopupPlacement Placement { get; set; } = PopupPlacement.Top;

    /// <summary>
    /// 设置弹出层的内容。
    /// </summary>
    [Parameter] public string? Content { get; set; }
    /// <summary>
    /// 设置弹出层内容的模板。
    /// </summary>
    [Parameter] public RenderFragment? PopupContent { get; set; }

    /// <summary>
    /// 设置弹出层是否具备箭头指向。
    /// </summary>
    [Parameter] public bool Arrow { get; set; }
    /// <summary>
    /// 设置是否显示弹出层。
    /// </summary>
    [Parameter] public bool Visible { get; set; }

    /// <summary>
    /// 触发弹出的方式。
    /// </summary>
    [Parameter] public PopupTrigger Trigger { get; set; } = PopupTrigger.Hover;

    Popper? _instance;

    protected override void OnComponentParameterSet()
    {
        base.OnComponentParameterSet();

        PopupContent ??= builder => builder.AddContent(0, Content);
    }

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (ChildContent != null)
        {
            builder.CreateCascadingComponent(this, 0, ChildContent, isFixed: true);
        }

        base.BuildRenderTree(builder);
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", inner =>
        {
            inner.AddContent(0, PopupContent);

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

    /// <inheritdoc/>
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

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        base.BuildAttributes(attributes);

        if (Visible)
        {
            attributes["autofocus"] = true;
        }
    }

    /// <summary>
    /// 触发指定元素引用并显示弹出层。
    /// </summary>
    /// <param name="selector">被触发弹出层的元素引用。</param>
    public async Task Show(ElementReference selector)
    {
        Visible = true;
        StateHasChanged();
        _instance = await JS.Value.InvokePopupAsync(selector, Ref, new()
        {
            Placement = Placement
        });
    }

    /// <summary>
    /// 隐藏弹出层。
    /// </summary>
    public async Task Hide()
    {
        Visible = false;
        StateHasChanged();
        if (_instance is not null)
        {
            await _instance.DestroyAsync();
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
    /// <summary>
    /// 手动调用。
    /// </summary>
    Manual,
}