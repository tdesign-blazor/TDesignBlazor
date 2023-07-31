using Microsoft.JSInterop;

namespace TDesign;
partial class TUpload
{
    /// <summary>
    /// 处理要上传的文件。
    /// </summary>
    /// <param name="files">要上传的文件列表。</param>
    /// <returns></returns>
    async Task HandleUploadingFiles(IReadOnlyList<UploadFileInfo> files)
    {
        int index = 0;
        foreach ( var file in files.Where(f=>f is not null) )
        {
            _fileList.Add(file);
            var parameterInfo = new UploadParameter
            {
                Index = index,
                ActionUrl = Action,
                Name = Name,
                FileId = file.Id,
            };

            await _uploadJSModule.Module.InvokeVoidAsync("upload.uploadFile", RefInputFile, parameterInfo, DotNetObjectReference.Create(this));

            index++;
        }
    }


    [JSInvokable("onSuccess")]
    public async Task OnSuccessAsync(Guid fileId, string responseText)
    {
        var fileInfo = GetFileInfo(fileId);

        fileInfo.Status = Status.Success;
        fileInfo.Percent = 100;
        fileInfo.ResponseText = responseText;
        await this.Refresh();
    }

    [JSInvokable("onError")]
    public async Task OnErrorAsync(Guid fileId, string responseText)
    {
        var fileInfo = GetFileInfo(fileId);

        fileInfo.Status = Status.Error;
        fileInfo.Percent = 100;
        fileInfo.ResponseText = responseText;
        await this.Refresh();
    }

    [JSInvokable("onProgress")]
    public async Task OnProgressAsync(Guid fileId, int percent)
    {
        var fileInfo = GetFileInfo(fileId);
        fileInfo.Percent = percent;
        await this.Refresh();
    }

    UploadFileInfo GetFileInfo(Guid id)
    {
        var fileInfo = _fileList.Find(m => m.Id == id);
        return fileInfo ?? throw new InvalidOperationException($"无法获取到上传的文件 id ({id})");
    }
}
