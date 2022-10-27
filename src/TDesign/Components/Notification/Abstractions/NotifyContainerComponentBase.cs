using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign.Notification;
public abstract class NotifyContainerComponentBase<TNotifyService, TConfiguration> : BlazorComponentBase, IDisposable
    where TNotifyService : INotifyService<TConfiguration>
    where TConfiguration : NotifyConfigurationBase
{

    [Inject] TNotifyService? NotifyService { get; set; }

    Dictionary<OneOf<Placement, (int offsetX, int offsetY)>, List<TConfiguration>> _messageListDic = new();



    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        NotifyService.OnShowing += NotifyService_OnShowing;
    }

    private async Task NotifyService_OnShowing(TConfiguration configuration)
    {
        if ( configuration is null )
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        await AddItem(configuration);
        await this.Refresh();
    }

    private async Task AddItem(TConfiguration configuration)
    {
        var key = configuration.Placement;
        if ( _messageListDic.ContainsKey(key) )
        {
            _messageListDic[key].Add(configuration);
        }
        else
        {
            _messageListDic.Add(key, new() { configuration });
        }

        await this.Refresh();

        await OnTimeoutRemove();

        Task OnTimeoutRemove()
        {
            Timer timer = new(async (state) =>
            {
                await RemoveItem((TConfiguration)state);
            }, configuration, configuration.Delay ??= Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }
    }

    private Task RemoveItem(TConfiguration configuration)
    {
        var key = configuration.Placement;
        if ( _messageListDic.ContainsKey(key) )
        {
            _messageListDic[key].Remove(configuration);

            if ( !_messageListDic[key].Any() )
            {
                _messageListDic.Remove(key);
            }
        }
        return this.Refresh();
    }

    public void Dispose()
    {
        if ( NotifyService is not null )
        {
            NotifyService.OnShowing -= NotifyService_OnShowing;
        }
    }
}
