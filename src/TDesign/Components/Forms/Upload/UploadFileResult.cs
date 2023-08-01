namespace TDesign;

/// <summary>
/// 表示文件上传后的结果。
/// </summary>
public class UploadFileResult
{
    /// <summary>
    /// 获取或设置上传的文件名。
    /// </summary>
    public string? OriginalFileName { get; set; }
    /// <summary>
    /// 获取或设置上传以后的文件完整路径。
    /// </summary>
    public string? UploadedFilePath { get; set; }
    /// <summary>
    /// 获取或设置上传后的文件大小。
    /// </summary>
    public long? FileSize { get; set; }
    /// <summary>
    /// 获取或设置上传后的结果状态。
    /// </summary>
    public Status Status { get; set; } = Status.Default;
    /// <summary>
    /// 获取或设置上传后的提示
    /// </summary>
    public string? Message { get; set; }
}
