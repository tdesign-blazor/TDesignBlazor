namespace TDesign;

/// <summary>
/// 对话框默认实现。
/// </summary>
class DialogService : IDialogService
{

    /// <inheritdoc/>
    public event Action<Guid, DialogParameters>? OnOpening;

    /// <inheritdoc/>
    public event Action<Guid, DialogResult>? OnClosing;

    private DialogReference _reference;

    /// <inheritdoc/>
    public Task Close(Guid id, DialogResult result)
    {
        _reference.SetResult(result);
        OnClosing?.Invoke(id, result);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<IDialogReference> Open<TDialogTemplate>(DialogParameters? parameters = default) where TDialogTemplate : IComponent
    {
        parameters ??= new();
        parameters.SetDialogTemplate<TDialogTemplate>();
        return Open(parameters);
    }

    Task<IDialogReference> Open(DialogParameters parameters)
    {
        if ( parameters is null )
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var reference = new DialogReference();
        _reference = reference;
        OnOpening?.Invoke(_reference.Id, parameters);
        return Task.FromResult((IDialogReference)reference);
    }
}
