namespace TDesign;

/// <summary>
/// 提供具备动态对话框功能的服务。
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// 显示对话框。
    /// </summary>
    Task<IDialogReference> Open<TDialogTemplate>( DialogParameters? parameters = default) where TDialogTemplate : IComponent;

    /// <summary>
    /// 当对话框打开时触发的事件。
    /// </summary>
    event Action<Guid, DialogParameters?> OnOpening;

    Task Close(Guid id, DialogResult result);

    event Action<Guid , DialogResult> OnClosing;
}


class DialogService : IDialogService
{
    public event Action<Guid, DialogParameters?> OnOpening;
    public event Action<Guid, DialogResult> OnClosing;

    DialogReference _reference;
    public Task Close(Guid id, DialogResult result)
    {
        _reference.SetResult(result);
        OnClosing?.Invoke(id, result);
        return Task.CompletedTask;
    }

    public Task<IDialogReference> Open<TDialogTemplate>(DialogParameters? parameters = null) where TDialogTemplate : IComponent
    {
        parameters ??= new();
        parameters.SetDialogTemplate<TDialogTemplate>();
        return Open(parameters);
    }

    Task<IDialogReference> Open(DialogParameters parameters)
    {
        if (parameters is null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var reference = new DialogReference();
        _reference = reference;
        OnOpening?.Invoke(_reference.Id, parameters);
        return Task.FromResult((IDialogReference)reference);
    }
}
