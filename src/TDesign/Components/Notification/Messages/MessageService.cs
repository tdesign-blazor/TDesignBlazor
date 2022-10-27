namespace TDesign;

/// <summary>
/// The message service.
/// </summary>
internal class MessageService : IMessageService
{
    public event Action? OnClosed;
    public event Func<MessageConfiguration, Task>? OnShowing;
    /// <inheritdoc/>
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
