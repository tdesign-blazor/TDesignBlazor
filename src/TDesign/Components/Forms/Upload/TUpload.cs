using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text.Json;

namespace TDesign;

/// <summary>
/// 上传组件允许用户传输文件或提交自己的内容。
/// </summary>
[CssClass("t-upload")]
public partial class TUpload : TDesignComponentBase,IHasDisabled
{
    /// <summary>
    /// 设置服务端上传的 API 地址。要求 API 支持 FORM DATA 数据传输方式。
    /// </summary>
    [ParameterApiDoc("服务端上传的 API 地址。要求 API 支持 FORM DATA 数据传输方式")]
    [Parameter][EditorRequired] public string Action { get; set; }
    /// <summary>
    /// 向服务端发送请求的 HTTP 方式，默认是 POST。
    /// </summary>
    [ParameterApiDoc("向服务端发送请求的 HTTP 方式",Value ="POST")]
    [Parameter] public string? Method { get; set; } = "POST";
    /// <summary>
    /// 上传文件在表单中的名称。该名称用于服务端进行文件对象的接收。
    /// </summary>
    [ParameterApiDoc("上传文件在表单中的名称。该名称用于服务端进行文件对象的接收", Value = "file")]
    [Parameter] public string Name { get; set; } = "file";
    /// <summary>
    /// 允许批量上传。
    /// </summary>
    [ParameterApiDoc("是否允许批量上传")]
    [Parameter] public bool Multiple { get; set; }
    /// <summary>
    /// 是否在选择文件后自动发起请求上传文件。否则手动调用 <see cref="Upload"/> 方法。
    /// </summary>
    [ParameterApiDoc("是否在选择文件后自动发起请求上传文件，否则需要手动调用 Upload 方法")]
    [Parameter] public bool AutoUpload { get; set; } = true;
    /// <summary>
    /// 上传的风格样式。
    /// </summary>
    [ParameterApiDoc("上传的风格样式")]
    [Parameter] public UploadTheme Theme { get; set; } = UploadTheme.File;
    /// <summary>
    /// 上传的显示文本。
    /// </summary>
    [ParameterApiDoc("上传的显示文本", Value ="选择文件")]
    [Parameter] public string Text { get; set; } = "选择文件";

    /// <summary>
    /// 当前最大上传文件数量。默认 100。
    /// </summary>
    [ParameterApiDoc("当前最大上传文件数量", Value = "100")]
    [Parameter] public int Max { get; set; } = 100;
    /// <summary>
    /// 接受上传的文件类型。例如 .png,.jpg,.xslx 等。
    /// <para>
    /// 参考：https://developer.mozilla.org/zh-CN/docs/Web/HTML/Element/Input/file#accept
    /// </para>
    /// </summary>
    [ParameterApiDoc("接受上传的文件类型。例如 .png,.jpg,.xslx 等")]
    [Parameter] public string? Accept { get; set; }
    /// <summary>
    /// 单个上传的文件限制大小，单位 B。默认 512KB。
    /// </summary>
    [ParameterApiDoc("单个上传的文件限制大小，单位 B", Value =512000)]
    [Parameter] public long Size { get; set; } = 512000;

    /// <summary>
    /// 禁用上传组件。
    /// </summary>
    [ParameterApiDoc("禁用上传组件")]
    [Parameter]public bool Disabled { get; set; }
    /// <summary>
    /// 组件下方的提示文字。
    /// </summary>
    [ParameterApiDoc("下方的提示文字")]
    [Parameter] public string? Tip { get; set; }
    /// <summary>
    /// 按钮的主题。
    /// </summary>
    [ParameterApiDoc("按钮的主题", Value = "Primary")]
    [Parameter] public Theme ButtonTheme { get; set; } = TDesign.Theme.Primary;
    /// <summary>
    /// 按钮图标，默认 IconName.Upload
    /// </summary>
    [ParameterApiDoc("按钮图标", Value = "Upload")]
    [Parameter] public object? ButtonIcon { get; set; } = IconName.Upload;
    /// <summary>
    /// 上传文件列表的内容
    /// </summary>
    [ParameterApiDoc("上传文件列表的内容", Type = "RenderFragment<IReadOnlyList<UploadFileInfo>>?")]
    [Parameter]public RenderFragment<IReadOnlyList<UploadFileInfo>>? FileListContent { get; set; }
    /// <summary>
    /// 附带的 Header 字典
    /// </summary>
    [ParameterApiDoc("附带的 Header 字典", Type = "Dictionary<string, string>")]
    [Parameter]public Dictionary<string, string> Headers { get; set; } = new();
    /// <summary>
    /// 附带的 form/data 字段。
    /// </summary>
    [ParameterApiDoc("附带的 form/data 字段", Type = "Dictionary<string, object>")]
    [Parameter]public Dictionary<string, object> Data { get; set; } = new();
    /// <summary>
    /// 当上传前进行验证的处理委托。
    /// </summary>
    [ParameterApiDoc("当上传前进行验证的处理委托", Type = "Func<IReadOnlyList<UploadFileInfo>, Task<bool>>")]
    [Parameter] public Func<IReadOnlyList<UploadFileInfo>, Task<bool>> ValidationHandler { get; set; } = (_) => Task.FromResult(true);
    /// <summary>
    /// 在文件选择之后，上传请求发起之前触发。
    /// </summary>
    [ParameterApiDoc("在文件选择之后，上传请求发起之前触发", Type = "EventCallback<IReadOnlyList<UploadFileInfo>>")]
    [Parameter]public EventCallback<IReadOnlyList<UploadFileInfo>> OnSelected { get; set; }

    /// <summary>
    /// 当上传前触发的回调。
    /// </summary>
    [ParameterApiDoc("当上传前触发的回调", Type = "EventCallback<IReadOnlyList<UploadFileInfo>>")]
    [Parameter]public EventCallback<IReadOnlyList<UploadFileInfo>> OnBeforeUpload { get; set; }
    /// <summary>
    /// 当上传成功后触发的回调。
    /// </summary>
    [ParameterApiDoc("当上传成功后触发的回调", Type = "EventCallback<UploadFileInfo>")]
    [Parameter]public EventCallback<UploadFileInfo> OnSuccess { get; set; }

    /// <summary>
    /// 当上传失败后触发的回调。
    /// </summary>
    [ParameterApiDoc("当上传失败后触发的回调", Type = "EventCallback<UploadFileInfo>")]
    [Parameter]public EventCallback<UploadFileInfo> OnFailure { get; set; }

    /// <summary>
    /// 当移除文件后触发的回调。
    /// </summary>
    [ParameterApiDoc("当移除文件后触发的回调", Type = "EventCallback<UploadFileInfo>")]
    [Parameter]public EventCallback<UploadFileInfo> OnRemoved { get; set; }

    /// <summary>
    /// 当所有文件上传完成触发的回调。
    /// </summary>
    [ParameterApiDoc("当所有文件上传完成触发的回调", Type = "EventCallback<IReadOnlyList<UploadFileInfo>>")]
    [Parameter] public EventCallback<IReadOnlyList<UploadFileInfo>> OnFinished { get; set; }

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
        FileListContent ??= value => builder => DefaultFileListContent(builder, value);
        ValidationHandler ??= _ => Task.FromResult(true);
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
            .Attribute("type", "file")
            .Attribute("name", Name)
            .Attribute("multiple", "multiple", Multiple)
            .Attribute("hidden", "hidden")
            .Attribute("disabled", Disabled)
            .Close();

        BuildFile(builder);
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-upload--theme-file-input", Theme == UploadTheme.FileInput);
    }

    /// <inheritdoc/>
    protected override void DisposeComponentResources()
    {
        _fileList.Clear();
    }
}
