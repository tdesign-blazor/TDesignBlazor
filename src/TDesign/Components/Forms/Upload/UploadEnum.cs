namespace TDesign;
/// <summary>
/// 上传组件显示的风格样式。
/// </summary>
public enum UploadTheme
{
    /// <summary>
    /// 默认的文件上传风格。
    /// </summary>
    [CssClass("single-file")] File,
    /// <summary>
    /// 输入框形式的文件上传风格。
    /// </summary>
    [CssClass("single-file-input")] FileInput,
    ///// <summary>
    ///// 文件批量上传。
    ///// </summary>
    //[CssClass("flow-file-flow")]FileFlow,
    ///// <summary>
    ///// 默认图片上传风格。
    ///// </summary>
    //Image,
    ///// <summary>
    ///// 图片批量上传风格。
    ///// </summary>
    //[CssClass("flow-image-flow")]ImageFlow,
    ///// <summary>
    ///// 完全自定义风格。
    ///// </summary>
    //Custom,
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