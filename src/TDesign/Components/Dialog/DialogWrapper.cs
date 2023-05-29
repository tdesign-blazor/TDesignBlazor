using Microsoft.JSInterop;
using TDesign.Specifications;

namespace TDesign;
[CssClass("t-dialog__ctx")]
internal class DialogWrapper : TDesignChildContentComponentBase,  IHasHeaderFragment, IHasFooterFragment
{
    public DialogWrapper() => CaptureReference = true;

    [Inject]IDialogService DialogService { get; set; }
    [CascadingParameter]DialogContainer DialogContainer { get; set; }
    [Parameter]public DialogParameters Parameters { get; set; }

    [Parameter]public Guid Id { get; set; }
    /// <summary>
    /// 设置非模态对话框。
    /// </summary>
    [Parameter][BooleanCssClass("t-dialog__ctx--modelss", "t-dialog__ctx--fixed")] public bool Modeless { get; set; }
    /// <summary>
    /// 设置屏幕居中显示。
    /// </summary>
    [Parameter] public bool Center { get; set; }
    /// <summary>
    /// 设置对话框顶部的代码片段。
    /// </summary>
    [Parameter] public RenderFragment? HeaderContent { get; set; }
    /// <summary>
    /// 设置对话框底部的内容。
    /// </summary>
    [Parameter] public RenderFragment? FooterContent { get; set; }

    /// <summary>
    /// 设置标题的图标。
    /// </summary>
    [Parameter] public object? Icon { get; set; }
    /// <summary>
    /// 设置图标的主题。
    /// </summary>
    [Parameter] public Theme? IconTheme { get; set; }
    /// <summary>
    /// 是否显示 x 关闭图标。
    /// </summary>
    [Parameter] public bool Closable { get; set; }
    /// <summary>
    /// 阻止点击遮罩层关闭对话框功能。
    /// </summary>
    [Parameter] public bool PreventMaskToClose { get; set; }

    IJSModule _dialogModel;

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dialogModel = await JS.ImportTDesignModuleAsync("dialog");
            await Open();
        }
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["id"] = $"dialog_{Id}";
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateCascadingComponent(this, 0, content =>
        {
            content.Div("t-dialog__mask", !Modeless)
                    .Callback("onclick",this,Close,!PreventMaskToClose)
                .Close();

            content.Div("t-dialog__wrap")
                    .Content(wrap =>
                    {
                        wrap.Div("t-dialog__position")
                            .Class("t-dialog--center", Center)
                            .Class("t-dialog--top", !Center)
                            .Content(position =>
                            {
                                content.CreateComponent<DialogContext>(0, BuildDialog, new { Parameters });
                            })
                            .Close();
                    })
                    .Close();
        });
    }
    void BuildDialog(RenderTreeBuilder builder)
    {
        builder.Div("t-dialog")
                .Class("t-dialog--default")
                .Content(dialog =>
                {
                    dialog.Div("t-dialog__header")
                        .Content(header =>
                        {
                            header.Div("t-dialog__header-content").Content(content =>
                            {
                                content.Component<TIcon>(Icon is not null)
                                    .Attribute(nameof(TIcon.AdditionalClass), IconTheme?.ToThemeMappingClassName("t-is-"),IconTheme is not null)
                                    .Attribute(nameof(TIcon.Name),Icon)
                                    .Close();

                                content.AddContent(0, HeaderContent);

                            }).Close();

                            header.Span("t-dialog__close", Closable)
                                .Callback("onclick", HtmlHelper.Instance.Callback().Create(this, Close))
                                .Content(close =>
                                {
                                    close.Component<TIcon>().Attribute("Name", IconName.Close).Close();
                                })
                                .Close();
                        })
                        .Close();

                    dialog.Div("t-dialog__body").Content(ChildContent).Close();

                    dialog.Div("t-dialog__footer", FooterContent is not null).Content(FooterContent).Close();
                })
                .Close();
    }

    /// <inheritdoc/>
    protected override void BuildStyle(IStyleBuilder builder) => builder.Append("display:none");

    /// <summary>
    /// 显示对话框。
    /// </summary>
    public async Task Open()
    {
        _dialogModel ??= await JS.ImportTDesignModuleAsync("dialog");
        await _dialogModel.Module.InvokeVoidAsync("dialog.open", $"dialog_{Id}");
    }

    /// <summary>
    /// 关闭对话框。
    /// </summary>
    public async Task Close(DialogResult result)
    {
        _dialogModel ??= await JS.ImportTDesignModuleAsync("dialog");
        await _dialogModel.Module.InvokeVoidAsync("dialog.close", $"dialog_{Id}");
        await DialogService.Close(Id, result);
        Reset();
    }

    public Task Close() => Close(DialogResult.Close());


    bool _parameterSet;
    internal void Set(TDialog dialog)
    {
        if (!_parameterSet)
        {
            HeaderContent = dialog.HeaderContent;
            FooterContent = dialog.FooterContent;
            ChildContent = dialog.ChildContent;
            Icon = dialog.Icon;
            IconTheme = dialog.IconTheme;
            Modeless = dialog.Modeless;
            Center = dialog.Center;
            Closable = dialog.Closable;
            PreventMaskToClose = dialog.PreventMaskToClose;
            _parameterSet = true;
            StateHasChanged();
        }
    }

    internal void Reset() => _parameterSet = false;
}
