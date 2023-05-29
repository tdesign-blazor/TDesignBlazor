namespace TDesign.Specifications;

/// <summary>
/// 定义具备标题文本的参数。
/// </summary>
public interface IHasTitleText
{
    /// <summary>
    /// 标题文本。
    /// </summary>
    string? TitleText { get; set; }
}

/// <summary>
/// 定义具备标题代码片段的参数。
/// </summary>
public interface IHasTitleFragment
{
    /// <summary>
    /// 设置标题任意代码片段。
    /// </summary>
    RenderFragment? TitleContent { get; set; }
}