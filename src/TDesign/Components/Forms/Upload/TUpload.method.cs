using Microsoft.JSInterop;

namespace TDesign;
partial class TUpload
{
    /// <summary>
    /// 当已经选择了上传的文件。
    /// </summary>
    /// <param name="files">选择的文件列表。</param>
    /// <returns></returns>
    public async Task SelectFiles(IReadOnlyList<UploadFileInfo> files)
    {
        _fileList.Clear();
        await this.Refresh();

        int index = 0;
        foreach ( var file in files.Where(f=>f is not null) )
        {
            file.Status = UploadStatus.Pending;
            file.Parameter = new UploadParameter
            {
                Index = index,
                ActionUrl = Action,
                Name = Name,
                FileId = file.Id,
            };

            _fileList.Add(file);

            index++;
        }
        if ( OnSelected.HasDelegate )
        {
           await OnSelected.InvokeAsync(_fileList);
        }
        if ( AutoUpload )
        {
            await Upload();
        }
    }

    /// <summary>
    /// 执行上传操作。
    /// </summary>
    public async Task Upload()
    {
        foreach ( var file in _fileList )
        {
            await _uploadJSModule.Module.InvokeVoidAsync("upload.uploadFile", RefInputFile, file.Parameter, DotNetObjectReference.Create(this));
        }
    }

    /// <summary>
    /// 服务端上传成功后的 JS 回调。
    /// </summary>
    /// <param name="fileId">文件id。</param>
    /// <param name="responseText">服务端响应文本。</param>
    /// <returns></returns>
    [JSInvokable("onSuccess")]
    public async Task OnSuccessAsync(Guid fileId, string? responseText)
    {
        var fileInfo = GetFileInfo(fileId);

        fileInfo.Status = UploadStatus.Finished;
        fileInfo.Percent = 100;
        fileInfo.ResponseText = responseText;
        fileInfo.IsSucceed = true;

        if ( OnSuccess.HasDelegate )
        {
            await OnSuccess.InvokeAsync(fileInfo);
        }
        await this.Refresh();
    }

    /// <summary>
    /// 服务端上传失败后的 JS 回调。
    /// </summary>
    /// <param name="fileId">文件id。</param>
    /// <param name="responseText">服务端响应文本。</param>
    [JSInvokable("onFailure")]
    public async Task OnFailureAsync(Guid fileId, string? responseText)
    {
        var fileInfo = GetFileInfo(fileId);

        fileInfo.Status = UploadStatus.Finished;
        fileInfo.Percent = 100;
        fileInfo.ResponseText = responseText;
        fileInfo.IsSucceed = false;

        if ( OnFailure.HasDelegate )
        {
            await OnFailure.InvokeAsync(fileInfo);
        }
        await this.Refresh();
    }

    /// <summary>
    /// 文件上传中的 JS 回调。
    /// </summary>
    /// <param name="fileId">文件id。</param>
    /// <param name="percent">当前进度。</param>
    /// <returns></returns>
    [JSInvokable("onProgress")]
    public async Task OnProgressAsync(Guid fileId, int percent)
    {
        var fileInfo = GetFileInfo(fileId);
        fileInfo.Status = UploadStatus.InProgress;
        fileInfo.Percent = percent;
        await this.Refresh();
    }

    UploadFileInfo GetFileInfo(Guid id)
    {
        var fileInfo = _fileList.Find(m => m.Id == id);
        return fileInfo ?? throw new InvalidOperationException($"无法获取到上传的文件 id ({id})");
    }

    /// <summary>
    /// 移除指定文件。
    /// </summary>
    /// <param name="id">文件id。</param>
    public async Task RemoveFile(Guid id)
    {
        var fileInfo = GetFileInfo(id);
        _fileList.Remove(fileInfo);

        if ( OnRemoved.HasDelegate )
        {
           await OnRemoved.InvokeAsync(fileInfo);
        }
    }
}
