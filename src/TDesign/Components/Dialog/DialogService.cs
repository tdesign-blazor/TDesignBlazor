namespace TDesign;

/// <summary>
/// 对话框服务的默认实现。
/// </summary>
internal class DialogService : IDialogService
{
    /// <inheritdoc/>
    public event Func<DialogConfiguration, Task> OnOpening;


    public async Task Open<TDialogTemplate>(DialogConfiguration? configuration = null) where TDialogTemplate : IDialogTemplate
    {
        var componentType = typeof(TDialogTemplate);

        configuration ??= new();

        configuration.ComponentType = componentType;
        if ( OnOpening is not null )
        {
            await OnOpening.Invoke(configuration);
        }
    }
}
