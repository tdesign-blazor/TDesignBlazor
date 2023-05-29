namespace TDesign;

/// <summary>
/// <see cref="IDialogReference"/> 默认实现。
/// </summary>
internal class DialogReference : IDialogReference
{
    readonly TaskCompletionSource<DialogResult> _result = new();

    /// <inheritdoc/>
    public Task<DialogResult> Result => _result.Task;

    /// <inheritdoc/>
    public Guid Id => Guid.NewGuid();

    /// <summary>
    /// 设置对话框结果。
    /// </summary>
    /// <param name="result">对话框的操作结果。</param>
    /// <returns></returns>
    public bool SetResult(DialogResult result) => _result.TrySetResult(result);
}
