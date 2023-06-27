namespace TDesign;
/// <summary>
/// 具备悬浮提示的弹出层。
/// </summary>
[CssClass("t-popup")]
public class TPopup : TDesignAdditionParameterWithChildContentComponentBase
{
    const string ANIMATION_ENTER = "t-popup--animation-enter";
    const string ANIMATION_ENTER_FROM = "t-popup--animation-enter-from";
    const string ANIMATION_EXITING = "t-popup--animation-exiting";
    const string ANIMATION_LEAVE_TO = "t-popup--animation-leave-to";

    const string ANIMATION_ENTER_TO = "t-popup--animation-enter-to";
    const string ANIMATION_ENTERING = "t-popup--animation-entering";
    const string ANIMATION_LEAVE_FROM = "t-popup--animation-leave-from";
    const string ANIMATION_LEAVE = "t-popup--animation-leave";

    const string ANIMATION_ENTER_ACTIVE = "t-popup--animation-enter-active";
    const string ANIMATION_LEAVE_ACTIVE = "t-popup--animation-leave-active";

    /// <summary>
    /// 初始化 <see cref="TPopup"/> 类的新实例。
    /// </summary>
    public TPopup() => CaptureReference = true;

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
    /// 设置弹出层内部的 CSS 类名称。
    /// </summary>
    [Parameter]public string? PopupContentCssClass { get; set; }

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
        => builder.Div(HtmlHelper.Instance.Class().Append("t-popup__content").Append(PopupContentCssClass!,!string.IsNullOrEmpty(PopupContentCssClass)).ToString())
            .Class("t-popup__content--arrow", Arrow)
            .Content(inner =>
            {
                inner.AddContent(0, PopupContent);

                inner.Div("t-popup__arrow", Arrow).Close();
            })
            .Close();

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if ( !builder.Contains(ANIMATION_ENTER) )
        {
            builder.Append(ANIMATION_ENTER);
        }
    }
    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append("display:none");
    }

    /// <summary>
    /// 触发指定元素引用并显示弹出层。
    /// </summary>
    /// <param name="selector">被触发弹出层的元素引用。</param>
    public async Task Show(IBlazorComponent selector)
    {
        _instance = await JS.InvokePopupAsync(selector.Reference!.Value, Reference!.Value, new()
        {
            Placement = Placement
        }, Hide);

        CssClassBuilder.Remove(ANIMATION_ENTER).Remove(ANIMATION_LEAVE_ACTIVE).Append(ANIMATION_ENTER_ACTIVE).Append(ANIMATION_LEAVE);
        Visible = true;
        StateHasChanged();
    }

    /// <summary>
    /// 隐藏弹出层。
    /// </summary>
    public async Task Hide()
    {
        if (_instance is not null)
        {
            await _instance.HideAsync(Reference);

            CssClassBuilder.Remove(ANIMATION_LEAVE).Remove(ANIMATION_ENTER_ACTIVE).Append(ANIMATION_LEAVE_ACTIVE).Append(ANIMATION_ENTER);
            Visible = false;
            StateHasChanged();
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