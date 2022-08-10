namespace TDesignBlazor;

/// <summary>
/// 水平对齐方式。
/// </summary>
public enum HorizontalAlignment
{
    Left,
    Center,
    Right
}

/// <summary>
/// 垂直对齐方式。
/// </summary>
public enum VerticalAlignment
{
    Top,
    Middle,
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