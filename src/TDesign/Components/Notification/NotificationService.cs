using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign;
/// <summary>
/// The notification service.
/// </summary>

internal class NotificationService : INotificationService
{
    public event Action? OnClosed;
    public event Func<NotificationConfiguration, Task>? OnShowing;
    /// <inheritdoc/>
    public void Dispose() => OnClosed?.Invoke();

    /// <inheritdoc/>
    public async Task Show(NotificationConfiguration configuration)
    {
        if ( configuration is null )
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if ( OnShowing is not null )
        {
            await OnShowing.Invoke(configuration);
        }
    }
}
