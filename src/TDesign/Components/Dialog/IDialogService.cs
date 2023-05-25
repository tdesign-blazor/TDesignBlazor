namespace TDesign;

/// <summary>
/// 提供具备动态对话框功能的服务。
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// 显示对话框。
    /// </summary>
    Task Open<TDialogTemplate>(DialogConfiguration? configuration = default) where TDialogTemplate : IDialogTemplate;

    /// <summary>
    /// 当对话框打开时触发的事件。
    /// </summary>
    event Func<DialogConfiguration, Task> OnOpening;
}

