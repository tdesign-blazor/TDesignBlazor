using Microsoft.JSInterop;
using System.Text.Json.Serialization;

namespace TDesign;
/// <summary>
/// 具备悬浮提示的弹出层。
/// </summary>
[CssClass("t-popup")]
public class TPopup : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// 初始化 <see cref="TPopup"/> 类的新实例。
    /// </summary>
    public TPopup() => CaptureReference = true;

    /// <summary>
    /// 设置弹出层的显示位置。
    /// </summary>
    [ParameterApiDoc("弹出层的显示位置", Value = "Top")]
    [Parameter] public PopupPlacement Placement { get; set; } = PopupPlacement.Top;

    /// <summary>
    /// 设置弹出层的内容。
    /// </summary>
    [ParameterApiDoc("弹出层的内容")]
    [Parameter] public string? Content { get; set; }
    /// <summary>
    /// 设置弹出层内容的模板。
    /// </summary>
    [ParameterApiDoc("弹出层内容的模板")]
    [Parameter] public RenderFragment? PopupContent { get; set; }

    /// <summary>
    /// 设置是否具备箭头指向。
    /// </summary>
    [ParameterApiDoc("是否具备箭头指向")]
    [Parameter] public bool Arrow { get; set; }

    /// <summary>
    /// 触发弹出的方式。
    /// </summary>
    [ParameterApiDoc("触发弹出的方式", Value = "Hover")]
    [Parameter] public PopupTrigger Trigger { get; set; } = PopupTrigger.Hover;

    /// <summary>
    /// 设置弹出层内部的 CSS 类名称。
    /// </summary>
    [ParameterApiDoc("弹出层内部的 CSS 类名称")]
    [Parameter]public string? PopupContentCssClass { get; set; }

    /// <summary>
    /// 获取一个布尔值，表示弹出框是否显示。
    /// </summary>
    public bool Visible { get; private set; }

    Popper? _popper;

    IJSModule _popupModule;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
            _popupModule = await JS.ImportTDesignModuleAsync("popup");
        }
    }

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
    protected override void BuildStyle(IStyleBuilder builder)
    {
        builder.Append("display:none");
    }

    /// <summary>
    /// 触发指定元素引用并显示弹出层。
    /// </summary>
    /// <param name="selector">被触发弹出层的元素引用。</param>
    public async Task Show(TDesignComponentBase selector)
    {
        var options = new PopperOptions
        {
            Placement = Placement
        };
        await _popupModule.Module.InvokeVoidAsync("popup.show", selector.Reference, Reference, options, DotNetObjectReference.Create(this));
    }

    /// <summary>
    /// 隐藏弹出层。
    /// </summary>
    public async Task Hide()
    {
        await _popupModule.Module.InvokeVoidAsync("popup.hide", Reference, DotNetObjectReference.Create(this));
    }

    [JSInvokable("onHidden")]
    public void InvokeOnHidden()
    {
        Visible = false;
    }

    [JSInvokable("onShown")]
    public void InvokeOnShown()
    {
        Visible = true;
        //_popper = new(_popupModule.Module, popper, new());
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