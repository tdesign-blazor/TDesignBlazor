namespace TDesign.Specifications;

/// <summary>
/// 定义具备头部文本的参数。
/// </summary>
public interface IHasHeaderText
{
    /// <summary>
    /// 头部文本。
    /// </summary>
    string? HeaderText { get; set; }
}

/// <summary>
/// 定义具备头部代码片段的参数。
/// </summary>
public interface IHasHeaderFragment
{
    /// <summary>
    /// 设置头部任意代码片段。
    /// </summary>
    RenderFragment? HeaderContent { get; set; }
}