using Microsoft.JSInterop;

namespace TDesign;
partial class TUpload
{
    void BuildFile(RenderTreeBuilder builder)
    {
        if ( !new[] { UploadTheme.File, UploadTheme.FileInput }.Contains(Theme) )
        {
            return;
        }

        builder.Div("t-upload__single")
            .Class($"t-upload__{Theme.GetCssClass()}")
            .Content(content =>
            {
                if ( Theme == UploadTheme.FileInput )
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
                        .Attribute(m => m.Disabled, Disabled)
                        .Attribute(m => m.OnClick, HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, async e =>
                        {
                            await _uploadJSModule.Module.InvokeVoidAsync("upload.showDialog", RefInputFile, JSInvokeMethodFactory.Create<IReadOnlyList<UploadFileInfo>, Task>(SelectFiles));
                        }))
                        .Content(Text)
                        .Close();
            })
            .Close();
    }

    /// <summary>
    /// 构建显示的文本。
    /// </summary>
    /// <param name="builder"></param>
    void BuildDisplayText(RenderTreeBuilder builder)
    {
        if ( !_fileList.Any() )
        {
            builder.Element("small", "t-upload__tips t-size-s", condition: Tip.IsNotNullOrEmpty()).Content(Tip).Close();
        }

        builder.Content(FileListContent?.Invoke(_fileList));
    }

    /// <summary>
    /// 默认的文件列表。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="files"></param>
    void DefaultFileListContent(RenderTreeBuilder builder, IReadOnlyList<UploadFileInfo> files)
    {
        foreach ( var item in files )
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
                .Attribute(m => m.AdditionalClass, "t-upload__single-name")
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

        builder.Component<TIcon>(!Disabled)
            .Attribute(m => m.Name, IconName.Close)
            .Attribute(m => m.AdditionalClass, "t-upload__icon-delete")
            .Callback("onclick", this, () => RemoveFile(fileInfo.Id))
            .Close();
    }
}
