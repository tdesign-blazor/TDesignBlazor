using Microsoft.JSInterop;
using TDesign.Specifications;

namespace TDesign;
/// <summary>
/// 对话框是一种临时窗口，通常在不想中断整体任务流程，但又需要为用户展示信息或获得用户响应时，在页面中打开一个对话框承载相应的信息及操作。
/// </summary>
[CssClass("t-dialog__ctx")]
public class TDialog : TDesignChildContentComponentBase,IHasHeaderText,IHasHeaderFragment,IHasFooterFragment
{
    public TDialog() => CaptureReference = true;

    /// <summary>
    /// 设置非模态对话框。
    /// </summary>
    [Parameter][BooleanCssClass("t-dialog__ctx--modelss", "t-dialog__ctx--fixed")] public bool Modeless { get; set; }
    /// <summary>
    /// 设置屏幕居中显示。
    /// </summary>
    [Parameter] public bool Center { get; set; }

    /// <summary>
    /// 设置当对话框显示时执行的回调方法。
    /// </summary>
    [Parameter]public EventCallback OnOpened { get; set; }
    /// <summary>
    /// 设置当对话框关闭时执行的回调方法。
    /// </summary>
    [Parameter]public EventCallback OnClosed { get; set; }
    /// <summary>
    /// 设置对话框顶部的代码片段。同时设置的 <see cref="HeaderText"/> 参数将被忽略。
    /// </summary>
    [Parameter] public RenderFragment? HeaderContent { get; set; }
    /// <summary>
    /// 设置对话框的标题文本。
    /// </summary>
    [Parameter] public string? HeaderText { get; set; }
    /// <summary>
    /// 设置对话框底部的内容。
    /// </summary>
    [Parameter] public RenderFragment? FooterContent { get; set; }

    [Parameter]public bool AutoOpen { get; set; }

    IJSModule _dialogModel;

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
            _dialogModel = await JS.ImportTDesignModuleAsync("dialog");
            if ( AutoOpen )
            {
                await Open();
            }
        }
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", attributes: new { @class = "t-dialog__mask" }, condition: !Modeless);

        builder.Div("t-dialog__wrap")
                .Content(wrap =>
                {
                    wrap.Div("t-dialog__position")
                        .Class("t-dialog--center", Center)
                        .Class("t-dialog--top", !Center)
                        .Content(position =>
                        {
                            position.Div("t-dialog")
                                    .Class("t-dialog--default")
                                    .Content(dialog =>
                                    {
                                        BuildHeader(dialog);

                                        dialog.Div("t-dialog__body").Content(ChildContent).Close();

                                        dialog.Div("t-dialog__footer", FooterContent is not null).Content(FooterContent).Close();
                                    })
                                    .Close();
                        })
                        .Close();
                })
                .Close();

        void BuildHeader(RenderTreeBuilder dialog)
        {
            dialog.Div("t-dialog__header")
                .Content(header =>
                {
                    header.Div("t-dialog__header-content").Content(HeaderContent).Close();
                    header.Span("t-dialog__close")
                        .Callback("onclick", HtmlHelper.Instance.Callback().Create(this, Close))
                        .Content(close =>
                        {
                            close.Component<TIcon>().Attribute("Name", IconName.Close).Close();
                        })
                        .Close();
                })
                .Close();
        }
    }
    /// <inheritdoc/>
    protected override void BuildStyle(IStyleBuilder builder) => builder.Append("display:none");

    /// <summary>
    /// 显示对话框。
    /// </summary>
    public async Task Open()
    {
        _dialogModel ??= await JS.ImportTDesignModuleAsync("dialog");
        await _dialogModel.Module.InvokeVoidAsync("dialog.open", Reference, DotNetObjectReference.Create(this));
    }

    /// <summary>
    /// 关闭对话框。
    /// </summary>
    public async Task Close()
    {
        _dialogModel ??= await JS.ImportTDesignModuleAsync("dialog");
        await _dialogModel.Module.InvokeVoidAsync("dialog.close", Reference, DotNetObjectReference.Create(this));
    }

    /// <summary>
    /// JS 执行的回调。
    /// </summary>
    [JSInvokable("OnOpened")]
    public Task JsInvokeDialogOpen() => OnOpened.InvokeAsync();

    /// <summary>
    /// JS 执行的回调。
    /// </summary>
    [JSInvokable("OnClosed")]
    public Task JsInvokeDialogClose() => OnClosed.InvokeAsync();
}