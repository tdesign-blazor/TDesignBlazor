namespace TDesignBlazor;
/// <summary>
/// 所在对象的相对位置。
/// </summary>
public enum Position
{
    /// <summary>
    /// 顶部。
    /// </summary>
    Top,
    /// <summary>
    /// 左部。
    /// </summary>
    Left,
    /// <summary>
    /// 右部。
    /// </summary>
    Right,
    /// <summary>
    /// 底部。
    /// </summary>
    Bottom,
    /// <summary>
    /// 居中
    /// </summary>
    Center
}
/// <summary>
/// 显示的相对摆放位置。
/// </summary>
public enum Placement
{
    /// <summary>
    /// 顶部左边。
    /// </summary>
    [CssClass("placement-top-left")] TopLeft,
    /// <summary>
    /// 顶部中间。
    /// </summary>
    [CssClass("placement-top-center")] TopCenter,
    /// <summary>
    /// 顶部右边。
    /// </summary>
    [CssClass("placement-top-right")] TopRight,
    /// <summary>
    /// 中部左边。
    /// </summary>
    [CssClass("placement-middle-left")] MiddleLeft,
    /// <summary>
    /// 中部中间。
    /// </summary>
    [CssClass("placement-middle-center")] MiddleCenter,
    /// <summary>
    /// 中部右边。
    /// </summary>
    [CssClass("placement-middle-right")] MiddleRight,
    /// <summary>
    /// 底部左边。
    /// </summary>
    [CssClass("placement-bottom-left")] BottomLeft,
    /// <summary>
    /// 底部中间。
    /// </summary>
    [CssClass("placement-bottom-center")] BottomCenter,
    /// <summary>
    /// 底部右边。
    /// </summary>
    [CssClass("placement-bottom-right")] BottomRight
}