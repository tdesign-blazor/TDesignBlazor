namespace TDesignBlazor;
internal class MessageService : IMessageService
{
    public event Action? OnClosed;
    public event Func<MessageConfiguration, Task> OnShowing;
    public void Dispose() => OnClosed?.Invoke();

    /// <inheritdoc/>
    public async Task Show(MessageConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (OnShowing is not null)
        {
            await OnShowing.Invoke(configuration);
        }

    }
}
