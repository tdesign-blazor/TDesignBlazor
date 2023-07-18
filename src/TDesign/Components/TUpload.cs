using ComponentBuilder.FluentRenderTree;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

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
    [Parameter] public string Text { get; set; } = "选择文件";

    /// <summary>
    /// 当前最大上传文件数量。默认 100。
    /// </summary>
    [Parameter]public int MaxCount { get; set;} = 100;
    /// <summary>
    /// 接受上传的文件类型。
    /// <para>
    /// 参考：https://developer.mozilla.org/zh-CN/docs/Web/HTML/Element/Input/file#accept
    /// </para>
    /// </summary>
    [Parameter]public string? Accept { get; set; }

    [Parameter][EditorRequired]public Func<InputFileChangeEventArgs,UploadedResult> UploadHandler { get; set; }

    [Parameter] public Theme ButtonTheme { get; set; } = TDesign.Theme.Primary;
    [Parameter] public object? ButtonIcon { get; set; } = IconName.Upload;

    [Inject]IJSRuntime JS { get; set; }

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
            .Attribute(m => m.OnChange, HtmlHelper.Instance.Callback().Create<InputFileChangeEventArgs>(this, InputFileChange))
            .Attribute("multiple", "multiple", Multiple)
            .Attribute("hidden", "hidden")
            .Close();

        BuildFile(builder);
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-upload--theme-file-input", Theme == UploadTheme.FileInput);
    }

    Task InputFileChange(InputFileChangeEventArgs e)
    {
        if ( e.FileCount > MaxCount )
        {
            Console.WriteLine("文件数量超出了最大上限");
            return Task.CompletedTask;
        }


        return Task.CompletedTask;
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
public class UploadedResult
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