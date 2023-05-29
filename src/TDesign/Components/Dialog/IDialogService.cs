namespace TDesign;

/// <summary>
/// 提供具备动态对话框功能的服务。
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// 显示指定对话框。
    /// </summary>
    /// <typeparam name="TDialogTemplate">对话框模板类型。</typeparam>
    Task<IDialogReference> Open<TDialogTemplate>(DialogParameters? parameters = default) where TDialogTemplate : IComponent;

    /// <summary>
    /// 当对话框打开时触发的事件。
    /// </summary>
    event Action<Guid, DialogParameters> OnOpening;

    /// <summary>
    /// 关闭指定 id 的对话框。
    /// </summary>
    /// <param name="id">要关闭的对话框 id。</param>
    /// <param name="result">对话框的执行结果。</param>
    /// <returns></returns>
    Task Close(Guid id, DialogResult result);

    /// <summary>
    /// 当对话框关闭时触发的事件。
    /// </summary>
    event Action<Guid , DialogResult> OnClosing;
}

