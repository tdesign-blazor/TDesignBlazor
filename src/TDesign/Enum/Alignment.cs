namespace TDesign;

/// <summary>
/// 水平对齐方式。
/// </summary>
public enum HorizontalAlignment
{
    /// <summary>
    /// 居左。
    /// </summary>
    Left,
    /// <summary>
    /// 居中。
    /// </summary>
    Center,
    /// <summary>
    /// 居右。
    /// </summary>
    Right
}

/// <summary>
/// 垂直对齐方式。
/// </summary>
public enum VerticalAlignment
{
    /// <summary>
    /// 居上。
    /// </summary>
    Top,
    /// <summary>
    /// 居中。
    /// </summary>
    Middle,
    /// <summary>
    /// 居下。
    /// </summary>
    Bottom
}

/// <summary>
/// Flex 的横版对齐方式
/// </summary>
public enum JustifyContent
{
    Start,
    Center,
    End,
    [CssClass("space-between")] Between,
    [CssClass("space-around")] Around,
}

public enum AlignItem
{
    Start,
    [CssClass("middle")] Center,
    End,
}