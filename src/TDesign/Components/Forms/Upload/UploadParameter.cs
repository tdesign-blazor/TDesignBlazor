using Microsoft.JSInterop;

namespace TDesign;

/// <summary>
/// 上传用的参数。
/// </summary>
internal class UploadParameter
{
    /// <summary>
    /// 文件索引编号。
    /// </summary>
    public int Index { get; init; }
    /// <summary>
    /// 后端上传地址。
    /// </summary>
    public string ActionUrl { get; init; }
    /// <summary>
    /// form/data 文件的名称。
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// 文件的Id。
    /// </summary>
    public Guid FileId { get;init; }
    /// <summary>
    /// 获取或设置 HTTP 提交方式。
    /// </summary>
    public string Method => "POST";
    /// <summary>
    /// 附带的 Header
    /// </summary>
    public Dictionary<string, string> Headers { get; set; } = new();
    /// <summary>
    /// 附带的 form/data 字段。
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();

}
