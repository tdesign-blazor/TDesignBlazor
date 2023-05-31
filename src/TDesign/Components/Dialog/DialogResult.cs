namespace TDesign;

/// <summary>
/// 表示对话框的结果。
/// </summary>
public record struct DialogResult
{
    /// <summary>
    /// 获取一个布尔值，表示对话框的结果是点击了取消操作。
    /// </summary>
    public bool Cancelled { get; private set; }

    /// <summary>
    /// 获取自定义数据。
    /// </summary>
    public object? Data { get; private set; }
    /// <summary>
    /// 设置对话框的结果是取消的，并设置 <see cref="Cancelled"/> 为 <c>true</c>。
    /// </summary>
    /// <returns>对话框结果。</returns>
    public static DialogResult Cancel() => new() { Cancelled = true };
    /// <summary>
    /// 设置对话框的结果是确认的，并可以返回一个结果。
    /// </summary>
    /// <typeparam name="T">返回数据的类型。</typeparam>
    /// <param name="result">要返回的结果。</param>
    /// <returns>对话框结果。</returns>
    public static DialogResult Confirm<T>(T? result = default) => new() { Data = result };
}
