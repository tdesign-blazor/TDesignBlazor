using Microsoft.AspNetCore.Components.Rendering;

using TDesign.Notification;

namespace TDesign;
/// <summary>
/// 用于 <see cref="IMessageService"/> 动态显示 <see cref="TMessage"/> 组件的容器组件。
/// </summary>
public class TMessageContainer : NotifyContainerComponentBase<IMessageService, MessageConfiguration>
{
    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        foreach (var item in ConfigurationDics)
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
                        configuration?.Theme,
                    });
                }
            },
                HtmlHelper.CreateHtmlAttributes(attributes =>
                {
                    var (classOrStyle, value) = MessageConfiguration.GetPlacement(item.Key);
                    attributes["class"] = HtmlHelper.Class.Append("t-message__list").Append(value, classOrStyle);
                    if (!classOrStyle)
                    {
                        attributes["style"] = HtmlHelper.Class.Append(value);
                    }
                })
            );
        }
    }

}
