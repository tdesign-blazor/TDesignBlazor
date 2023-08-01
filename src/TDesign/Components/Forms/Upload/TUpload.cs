using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text.Json;

namespace TDesign;

/// <summary>
/// 上传组件允许用户传输文件或提交自己的内容。
/// </summary>
[CssClass("t-upload")]
public partial class TUpload : TDesignComponentBase
{
    /// <summary>
    /// 上传文件在表单中的名称。
    /// </summary>
    [Parameter] public string Name { get; set; } = "file";
    /// <summary>
    /// 批量上传
    /// </summary>
    [Parameter] public bool Multiple { get; set; }
    /// <summary>
    /// 是否在选择文件后自动发起请求上传文件。
    /// </summary>
    [Parameter] public bool AutoUpload { get; set; } = true;
    /// <summary>
    /// 上传的风格样式。
    /// </summary>
    [Parameter] public UploadTheme Theme { get; set; } = UploadTheme.File;
    /// <summary>
    /// 上传的显示文本。
    /// </summary>
    [Parameter] public string Text { get; set; } = "选择文件";

    /// <summary>
    /// 当前最大上传文件数量。默认 100。
    /// </summary>
    [Parameter] public int Max { get; set; } = 100;
    /// <summary>
    /// 接受上传的文件类型。
    /// <para>
    /// 参考：https://developer.mozilla.org/zh-CN/docs/Web/HTML/Element/Input/file#accept
    /// </para>
    /// </summary>
    [Parameter] public string? Accept { get; set; }
    /// <summary>
    /// 设置服务端上传的 API 地址。要求 API 可以通过 POST 方式接收请求，并支持 FORM DATA 数据传输方式。
    /// </summary>
    [Parameter][EditorRequired] public string Action { get; set; }
    /// <summary>
    /// 单个上传的文件限制大小，单位 B。默认 512KB。
    /// </summary>
    [Parameter] public long Size { get; set; } = 512000;

    /// <summary>
    /// 按钮的主题。
    /// </summary>
    [Parameter] public Theme ButtonTheme { get; set; } = TDesign.Theme.Primary;
    /// <summary>
    /// 按钮图标，默认 IconName.Upload
    /// </summary>
    [Parameter] public object? ButtonIcon { get; set; } = IconName.Upload;

    /// <summary>
    /// 在文件选择之后，上传请求发起之前触发。
    /// </summary>
    [Parameter]public EventCallback<IReadOnlyList<UploadFileInfo>> OnSelected { get; set; }

    /// <summary>
    /// 当上传成功后触发的回调。
    /// </summary>
    [Parameter]public EventCallback<UploadFileInfo> OnSuccess { get; set; }

    /// <summary>
    /// 当上传失败后触发的回调。
    /// </summary>
    [Parameter]public EventCallback<UploadFileInfo> OnFailure { get; set; }

    /// <summary>
    /// 当移除文件后触发的回调。
    /// </summary>
    [Parameter]public EventCallback<UploadFileInfo> OnRemoved { get; set; }

    private List<UploadFileInfo> _fileList = new();

    [Inject] IJSRuntime JS { get; set; }

    ElementReference? RefInputFile;
    private IJSModule _uploadJSModule;

    protected override void OnParametersSet()
    {
        if (string.IsNullOrEmpty(Action))
        {
            throw new InvalidOperationException($"没有设置{nameof(Action)}参数");
        }

        base.OnParametersSet();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _uploadJSModule = await JS.ImportTDesignModuleAsync("upload");
        }
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Element("input")
            .Ref(e => RefInputFile = e)
            .Attribute("type","file")
            .Attribute("name",Name)
            .Attribute("multiple", "multiple", Multiple)
            .Attribute("hidden", "hidden")
            .Close();

        BuildFile(builder);
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-upload--theme-file-input", Theme == UploadTheme.FileInput);
    }

    void BuildFile(RenderTreeBuilder builder)
    {
        if (!new[] { UploadTheme.File, UploadTheme.FileInput }.Contains(Theme))
        {
            return;
        }

        builder.Div("t-upload__single")
            .Class($"t-upload__{Theme.GetCssClass()}")
            .Content(content =>
            {
                if (Theme == UploadTheme.FileInput)
                {

                }

                BuildTrigger(content);
                BuildDisplayText(content);
            })
            .Close();

    }

    void BuildTrigger(RenderTreeBuilder builder)
    {
        builder.Div("t-upload__trigger")
            .Content(button =>
            {
                button.Component<TButton>()
                        .Attribute(m => m.Theme, ButtonTheme)
                        .Attribute(m => m.Icon, ButtonIcon)
                        .Attribute(m => m.OnClick, HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, async e =>
                        {
                            await _uploadJSModule.Module.InvokeVoidAsync("upload.showDialog", RefInputFile, JSInvokeMethodFactory.Create<IReadOnlyList<UploadFileInfo>,Task>(SelectFiles));
                        }))
                        .Content(Text)
                        .Close();
            })
            .Close();
    }

    void BuildDisplayText(RenderTreeBuilder builder)
    {
        foreach (var item in _fileList)
        {
            builder.Div("t-upload__single-display-text t-upload__display-text--margin")
            .Content(content =>
            {
                if ( item.Status == UploadStatus.InProgress )//上传中
                {
                    BuildUploadingDisplayContent(content, item);
                }
                else
                {
                    BuildUploadedDisplayContent(content, item);
                }
            })
            .Close();
        }
    }


    /// <summary>
    /// 上传中的显示
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="fileInfo">文件信息。</param>
    void BuildUploadingDisplayContent(RenderTreeBuilder builder, UploadFileInfo fileInfo)
    {
        builder.Span("t-upload__single-name").Content(fileInfo.Name).Close();
        builder.Div("t-upload__single-progress")
            .Content(loader =>
            {
                loader.Component<TLoading>().Attribute(m => m.Center, true).Close();
                loader.Span("t-upload__single-percent").Content($"{fileInfo.Percent}%").Close();
            })
            .Close();
    }

    /// <summary>
    /// 上传后的显示
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="fileInfo">文件信息。</param>
    void BuildUploadedDisplayContent(RenderTreeBuilder builder, UploadFileInfo fileInfo)
    {
        builder.Component<TLink>()
                .Attribute(m => m.Size, TDesign.Size.Small)
                .Attribute(m => m.Hover, LinkHover.Color)
                .Attribute(m=>m.AdditionalClass, "t-upload__single-name")
                .Content(fileInfo.Name)
            .Close();

        //状态图标
        builder.Div("t-upload__flow-status").Content(status =>
        {
            builder.Component<TIcon>(fileInfo.IsSucceed)
                .Attribute(m => m.Name, IconName.CheckCircleFilled)
                .Attribute(m => m.AdditionalClass, ICON_SUCCESS)
                .Close();

            builder.Component<TIcon>(!fileInfo.IsSucceed)
                .Attribute(m => m.Name, IconName.InfoCircleFilled)
                .Attribute(m => m.AdditionalClass, ICON_FAILED)
                .Close();
        }).Close();

        builder.Component<TIcon>()
            .Attribute(m => m.Name, IconName.Close)
            .Attribute(m=>m.AdditionalClass, "t-upload__icon-delete")
            .Callback("onclick", this, () => RemoveFile(fileInfo.Id))
            .Close();
    }


    /// <inheritdoc/>
    protected override void DisposeComponentResources()
    {
        _fileList.Clear();
    }
}
/// <summary>
/// 上传组件显示的风格样式。
/// </summary>
public enum UploadTheme
{
    /// <summary>
    /// 默认的文件上传风格。
    /// </summary>
    [CssClass("single-file")] File,
    /// <summary>
    /// 输入框形式的文件上传风格。
    /// </summary>
    [CssClass("single-file-input")] FileInput,
    ///// <summary>
    ///// 文件批量上传。
    ///// </summary>
    //[CssClass("flow-file-flow")]FileFlow,
    ///// <summary>
    ///// 默认图片上传风格。
    ///// </summary>
    //Image,
    ///// <summary>
    ///// 图片批量上传风格。
    ///// </summary>
    //[CssClass("flow-image-flow")]ImageFlow,
    ///// <summary>
    ///// 完全自定义风格。
    ///// </summary>
    //Custom,
}