namespace TDesign;
/// <summary>
/// 具备悬浮提示的弹出层。
/// </summary>
[CssClass("t-popup")]
public class TPopup : TDesignComponentBase, IHasChildContent
{
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
    /// 触发弹出的方式。
    /// </summary>
    [Parameter] public PopupTrigger Trigger { get; set; } = PopupTrigger.Hover;

    /// <summary>
    /// 设置弹出延迟的时间，单位毫秒，默认400毫秒。
    /// </summary>
    [Parameter] public int? Timeout { get; set; } = 400;

    /// <summary>
    /// 获取一个布尔值，表示弹出框是否显示。
    /// </summary>
    public bool Visible { get; private set; }

    Popper? _instance;

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
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
                inner.Fluent().Div().Class("t-popup__arrow").Close();
            }
        },
        new
        {
            @class = HtmlHelper.Instance.Class().Append("t-popup__content").Append("t-popup__content--arrow", Arrow)
        });
    }

    /// <inheritdoc/>
    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append("display:none");
    }

    /// <summary>
    /// 捕获 <see cref="TPopup"/> 的元素引用。
    /// </summary>
    /// <param name="builder"><inheritdoc/></param>
    /// <param name="sequence"><inheritdoc/></param>
    protected override void CaptureElementReference(RenderTreeBuilder builder, int sequence)
    {
        builder.AddElementReferenceCapture(sequence, element => Reference = element);
    }

    /// <summary>
    /// 触发指定元素引用并显示弹出层。
    /// </summary>
    /// <param name="selector">被触发弹出层的元素引用。</param>
    public async Task Show(IBlazorComponent selector)
    {
        _instance = await JS.InvokePopupAsync(selector.Reference!.Value, Reference!.Value, new()
        {
            Timeout = Timeout ?? Options.Value.PopupTimeout ?? 400,
            Placement = Placement
        });
        Visible = _instance is not null;
    }

    /// <summary>
    /// 隐藏弹出层。
    /// </summary>
    public async Task Hide()
    {
        if (_instance is not null)
        {
            await _instance.HideAsync(Reference);
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
    /// <summary>
    /// 手动调用。
    /// </summary>
    Manual,
}