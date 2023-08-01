using Microsoft.JSInterop;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TDesign;

/// <summary>
/// 要上传的文件信息。
/// </summary>
public class UploadFileInfo
{
    public UploadFileInfo(string name, long size, string contentType, string? url)
    {
        Name = name;
        Size = size;
        ContentType = contentType;
        Url = url;

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
    public string? Url { get; }

    /// <summary>
    /// 获取上传以后的文件完整路径。
    /// </summary>
    public string? UploadedFilePath { get;internal set; }
    /// <summary>
    /// 获取文件当前的上传状态。
    /// </summary>
    public UploadStatus Status { get;internal set; } = UploadStatus.NotStart;

    /// <summary>
    /// 获取文件上传的进度百分比。
    /// </summary>
    public int Percent { get; internal set; }
    /// <summary>
    /// 获取一个布尔值，表示上传是否成功。
    /// </summary>
    public bool IsSucceed { get; internal set; }

    internal string? ResponseText { get; set; }

    /// <summary>
    /// 内部使用。
    /// </summary>
    internal UploadParameter? Parameter { get; set; }

    /// <summary>
    /// 获取上传到服务器后的 HTTP 响应结果并反序列化成指定对象。
    /// </summary>
    /// <typeparam name="TValue">反序列化的值。</typeparam>
    /// <param name="jsonSerializerOptions">JSON 序列号配置。</param>
    /// <returns>反序列化后的值或 null。</returns>
    public TValue? GetDeserializeValueFromResponseText<TValue>(JsonSerializerOptions? jsonSerializerOptions=default)
    {
        jsonSerializerOptions ??= new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<TValue>(ResponseText, jsonSerializerOptions);
    }
}

/// <summary>
/// 上传状态。
/// </summary>
public enum UploadStatus
{
    /// <summary>
    /// 未开始。
    /// </summary>
    NotStart,
    /// <summary>
    /// 等待上传。
    /// </summary>
    Pending,
    /// <summary>
    /// 正在上传。
    /// </summary>
    InProgress,
    /// <summary>
    /// 上传结束。
    /// </summary>
    Finished
}