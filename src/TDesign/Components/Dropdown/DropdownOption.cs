namespace TDesign;

/// <summary>
/// 定义下拉菜单的选项
/// </summary>
public class DropdownOption
{
    /// <summary>
    /// 输出的文本内容。
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// 选项的值。
    /// </summary>
    public object? Value { get; set; }
    /// <summary>
    /// 是否具有分割线。
    /// </summary>
    public bool Divider { get; set; }
    /// <summary>
    /// 文本的颜色主题。
    /// </summary>
    public Color? Color { get; set; }
    /// <summary>
    /// 禁用状态。
    /// </summary>
    public bool Disabled { get; set; }
    /// <summary>
    /// 选中状态。
    /// </summary>
    public bool Selected { get; set; }
    /// <summary>
    /// 项的前缀图标。
    /// </summary>
    public object? PrefixIcon { get; set; }
    /// <summary>
    /// 子选项的集合。
    /// </summary>
    public IEnumerable<DropdownOption> Options { get; set; } = Enumerable.Empty<DropdownOption>();
}
