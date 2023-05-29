namespace TDesign.Specifications;

/// <summary>
/// 定义具备底部文本的参数。
/// </summary>
public interface IHasFooterText
{
    /// <summary>
    /// 底部文本。
    /// </summary>
    string? FooterText { get; set; }
}

/// <summary>
/// 定义具备底部代码片段的参数。
/// </summary>
public interface IHasFooterFragment
{
    /// <summary>
    /// 设置底部任意代码片段。
    /// </summary>
    RenderFragment? FooterContent { get; set; }
}