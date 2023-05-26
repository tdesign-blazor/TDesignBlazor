namespace TDesign;

/// <summary>
/// 提供具备动态对话框功能的服务。
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// 显示对话框。
    /// </summary>
    Task Open<TDialogTemplate>( DialogParameter? parameters = default) where TDialogTemplate : IComponent;

    /// <summary>
    /// 当对话框打开时触发的事件。
    /// </summary>
    event Func<DialogConfiguration, Task> OnOpening;
}

