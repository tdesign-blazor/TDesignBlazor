namespace TDesign;

/// <summary>
/// 对话框的引用。
/// </summary>
public interface IDialogReference
{
    /// <summary>
    /// 获取对话框结果的异步任务。
    /// </summary>
    Task<DialogResult> Result { get; }
    /// <summary>
    /// 对话框唯一标识。
    /// </summary>
    Guid Id { get; }
}
