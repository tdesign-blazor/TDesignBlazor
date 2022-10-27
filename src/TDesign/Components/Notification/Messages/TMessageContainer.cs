using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 用于 <see cref="IMessageService"/> 动态显示 <see cref="Message"/> 组件的容器组件。
/// </summary>
public class MessageContainer : BlazorComponentBase, IDisposable
{
    [Inject] IMessageService? MessageService { get; set; }

    Dictionary<OneOf<Placement, (int offsetX, int offsetY)>, List<MessageConfiguration>> _messageListDic = new();



    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        MessageService.OnShowing += MessageService_OnShowing;
    }

    private async Task MessageService_OnShowing(MessageConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        await AddItem(configuration);
        await this.Refresh();
    }


    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        foreach (var item in _messageListDic)
        {
            builder.CreateElement(0, "div", content =>
            {
                foreach (var configuration in item.Value)
                {

                    content.CreateComponent<TMessage>(sequence + 1, builder => builder.AddContent(0, configuration.Content), new
                    {
                        configuration.Icon,
                        configuration.Loading,
                        configuration.Closable,
                        Theme = (Theme?)configuration?.Theme,
                    });
                }
            },
                HtmlHelper.CreateHtmlAttributes(attributes =>
                {
                    var (classOrStyle, value) = MessageConfiguration.GetPlacement(item.Key);
                    attributes["class"] = HtmlHelper.CreateCssBuilder().Append("t-message__list").Append(value, classOrStyle);
                    if (!classOrStyle)
                    {
                        attributes["style"] = HtmlHelper.CreateCssBuilder().Append(value);
                    }
                })
            );
        }
    }

    private async Task AddItem(MessageConfiguration configuration)
    {
        var key = configuration.Placement;
        if (_messageListDic.ContainsKey(key))
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
                await RemoveItem((MessageConfiguration)state);
            }, configuration, configuration.Delay ??= Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }
    }

    private Task RemoveItem(MessageConfiguration configuration)
    {
        var key = configuration.Placement;
        if (_messageListDic.ContainsKey(key))
        {
            _messageListDic[key].Remove(configuration);

            if (!_messageListDic[key].Any())
            {
                _messageListDic.Remove(key);
            }
        }
        return this.Refresh();
    }

    public void Dispose()
    {
        MessageService.OnShowing -= MessageService_OnShowing;
    }
}
