namespace TDesign;

/// <summary>
/// 动态对话框的配置。
/// </summary>
public class DialogConfiguration
{
    /// <summary>
    /// 获取或设置对话框的标题文本。
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// 获取或设置对话框是否居中显示。
    /// </summary>
    public bool Centered { get; set; }
    /// <summary>
    /// 获取或设置是否为非模态对话框（可以点击背景）。
    /// </summary>
    public bool Modeless { get; set; }

    #region 内部传值用

    internal Type ComponentType { get; set; }
    #endregion
}
