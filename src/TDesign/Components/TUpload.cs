using ComponentBuilder.FluentRenderTree;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text.Json;

namespace TDesign;

/// <summary>
/// 上传组件允许用户传输文件或提交自己的内容。
/// </summary>
[CssClass("t-upload")]
public class TUpload:TDesignComponentBase
{
    /// <summary>
    /// 批量上传
    /// </summary>
    [Parameter]public bool Multiple { get; set; }
    /// <summary>
    /// 是否在选择文件后自动发起请求上传文件。
    /// </summary>
    [Parameter]public bool AutoUpload { get; set; }
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
    [Parameter]public int Max { get; set;} = 100;
    /// <summary>
    /// 接受上传的文件类型。
    /// <para>
    /// 参考：https://developer.mozilla.org/zh-CN/docs/Web/HTML/Element/Input/file#accept
    /// </para>
    /// </summary>
    [Parameter]public string? Accept { get; set; }
    /// <summary>
    /// 设置服务端上传的 API 路径。
    /// </summary>
    [Parameter][EditorRequired]public string Action { get; set; }
    /// <summary>
    /// 单个上传的文件限制大小，单位 B。默认 512KB。
    /// </summary>
    [Parameter] public long Size { get; set; } = 512000;

    [Parameter] public Theme ButtonTheme { get; set; } = TDesign.Theme.Primary;
    [Parameter] public object? ButtonIcon { get; set; } = IconName.Upload;

    private List<UploadResult> _results = new();
    /// <summary>
    /// 获取上传结果。
    /// </summary>
    public IReadOnlyList<UploadResult> UploadResults => _results;

    [Inject]IJSRuntime JS { get; set; }
    [Inject]HttpClient Client { get; set; }

    InputFile? RefInputFile;
    private IJSModule _uploadJSModule;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
            _uploadJSModule = await JS.ImportTDesignModuleAsync("upload");
        }
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Component<InputFile>()
            .Ref(e => RefInputFile = e)
            .Attribute(m => m.OnChange, HtmlHelper.Instance.Callback().Create<InputFileChangeEventArgs>(this,async e=>
            {
                if ( AutoUpload )
                {
                   await Upload(e);
                }
            }))
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
        if(!new[] { UploadTheme.File, UploadTheme.FileInput }.Contains(Theme) )
        {
            return;
        }

        builder.Div("t-upload__single")
            .Class($"t-upload__{Theme.GetCssClass()}")
            .Content(content =>
            {
                if(Theme== UploadTheme.FileInput )
                {

                }

                BuildTrigger(content);
            })
            .Close();

    }

    void BuildTrigger(RenderTreeBuilder builder)
    {
        builder.Div("t-upload__trigger")
            .Content(button =>
            {
                button.Component<TButton>()
                        .Attribute(m=>m.Theme,ButtonTheme)
                        .Attribute(m=>m.Icon,ButtonIcon)
                        .Attribute(m => m.OnClick, HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, async e =>
                        {
                            await _uploadJSModule.Module.InvokeVoidAsync("upload.openDialog", RefInputFile?.Element);
                        }))
                        .Content(Text)
                        .Close();
            })
            .Close();
    }

    void BuildDisplayText(RenderTreeBuilder builder)
    {
        builder.Div("t-upload__single-display-text t-upload__display-text--margin")
            .Close();
    }

    private bool shouldRender;

    protected override bool ShouldRender() => shouldRender;
    
    /// <summary>
    /// 执行上传的操作。
    /// </summary>
    /// <param name="e"></param>
    public async Task Upload(InputFileChangeEventArgs e)
    {
        var browserFiles = new List<IBrowserFile>(Max);

        try
        {
            browserFiles.AddRange(e.GetMultipleFiles(Max));
        }
        catch(InvalidOperationException ex )
        {
            //超过设定的文件数量如何处理
            return;
        }

        shouldRender = false;
        var readyToUpload = false;

        using var content = new MultipartFormDataContent();

        foreach ( var file in browserFiles )
        {
            var result = new UploadResult
            {
                FileName = file.Name,
                FileSize = file.Size
            };
            try
            {
                var fileContent = new StreamContent(file.OpenReadStream(Size));

                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "\"files\"", file.Name);

                readyToUpload = true;
            }
            catch ( IOException ex )
            {
                result.Status = Status.Default;
                result.Tip = ex.Message;
            }
            finally
            {
                _results.Add(result);
            }
        }

        if ( readyToUpload )//可以上传到服务器
        {
            var response = await Client.PostAsync(Action, content);
            if ( response.IsSuccessStatusCode )
            {
                using var readStream = await response.Content.ReadAsStreamAsync();

                var serverUploadResults = await JsonSerializer.DeserializeAsync<IList<UploadResult>>(readStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if(serverUploadResults is not null )
                {
                    _results.AddRange(serverUploadResults);
                }
            }
        }

        shouldRender = true;
    }

    protected override void DisposeComponentResources()
    {
        Client.Dispose();
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
    [CssClass("single-file")]File,
    /// <summary>
    /// 输入框形式的文件上传风格。
    /// </summary>
    [CssClass("single-file-input")]FileInput,
    /// <summary>
    /// 文件批量上传。
    /// </summary>
    [CssClass("flow-file-flow")]FileFlow,
    /// <summary>
    /// 默认图片上传风格。
    /// </summary>
    Image,
    /// <summary>
    /// 图片批量上传风格。
    /// </summary>
    [CssClass("flow-image-flow")]ImageFlow,
    /// <summary>
    /// 完全自定义风格。
    /// </summary>
    Custom,
}

/// <summary>
/// 表示文件上传后的结果。
/// </summary>
public class UploadResult
{
    /// <summary>
    /// 获取或设置上传后的文件名。
    /// </summary>
    public string? FileName { get; set; }    
    /// <summary>
    /// 获取或设置上传后的文件大小。
    /// </summary>
    public long? FileSize { get; set; }
    /// <summary>
    /// 获取或设置上传后的时间。
    /// </summary>
    public DateTime? UploadedTime { get; set; }

    /// <summary>
    /// 获取或设置上传后的结果状态。
    /// </summary>
    public Status Status { get; set; } = Status.Default;
    /// <summary>
    /// 获取或设置上传后的提示
    /// </summary>
    public string? Tip { get; set; }

    /// <summary>
    /// 获取
    /// </summary>
    public Dictionary<string, string> Data { get; set; } = new();
}