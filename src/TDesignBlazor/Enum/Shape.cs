namespace TDesignBlazor;

/// <summary>
/// 表示组件的边框形状。
/// </summary>
public class Shape : Enumeration
{
    /// <summary>
    /// 初始化 <see cref="Shape"/> 类的新实例。
    /// </summary>
    /// <param name="value"></param>
    internal protected Shape(string value) : base(value)
    {
    }
    /// <summary>
    /// 表示边角是弧形。
    /// </summary>
    public static readonly Shape Round = new("round");
    /// <summary>
    /// 表示圆形。
    /// </summary>
    public static readonly Shape Circle = new("circle");
}
