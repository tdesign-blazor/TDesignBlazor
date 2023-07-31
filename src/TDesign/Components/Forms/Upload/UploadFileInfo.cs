using Microsoft.JSInterop;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TDesign;

/// <summary>
/// 要上传的文件信息。
/// </summary>
public class UploadFileInfo
{
    public UploadFileInfo(string name, long size, string contentType, string? objectUrl)
    {
        Name = name;
        Size = size;
        ContentType = contentType;
        ObjectUrl = objectUrl;

        Id = Guid.NewGuid();
    }
    /// <summary>
    /// 获取文件的编号。
    /// </summary>
    public Guid Id { get; }
    /// <summary>
    /// 获取只包含文件名，不包含任何路径信息的文件名称。
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// 获取字节数为单位的文件大小。
    /// </summary>
    public long Size { get; }
    /// <summary>
    /// 获取文件的 MIME 类型。
    /// </summary>
    public string? ContentType { get; }

    /// <summary>
    /// 获取文件对象的 URL 地址。
    /// </summary>
    public string? ObjectUrl { get; }

    /// <summary>
    /// 获取或设置上传以后的文件完整路径。
    /// </summary>
    public string? UploadedFilePath { get;internal set; }
    /// <summary>
    /// 获取或设置上传后的结果状态。
    /// </summary>
    public Status Status { get;internal set; } = Status.Default;

    public int Percent { get; internal set; }


    internal string ResponseText { get; set; }

    public TResult? GetResponse<TResult>(JsonSerializerOptions? jsonSerializerOptions=default)
    {
        jsonSerializerOptions ??= new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<TResult>(ResponseText, jsonSerializerOptions);
    }
}
