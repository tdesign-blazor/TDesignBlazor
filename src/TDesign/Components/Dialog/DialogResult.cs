namespace TDesign;

/// <summary>
/// 表示对话框的结果。
/// </summary>
public record struct DialogResult
{
    /// <summary>
    /// 获取一个布尔值，表示对话框是否已经关闭。
    /// </summary>
    public bool Closed { get; private set; }

    /// <summary>
    /// 获取自定义数据。
    /// </summary>
    public object? Data { get; private set; }
    /// <summary>
    /// 关闭对话框。
    /// </summary>
    /// <returns>对话框结果。</returns>
    public static DialogResult Close() => new() { Closed = true };
    /// <summary>
    /// 确认对话框。
    /// </summary>
    /// <typeparam name="T">返回数据的类型。</typeparam>
    /// <param name="result">要返回的结果。</param>
    /// <returns>对话框结果。</returns>
    public static DialogResult Ok<T>(T? result=default) => new() { Data = result, Closed = true };
}
