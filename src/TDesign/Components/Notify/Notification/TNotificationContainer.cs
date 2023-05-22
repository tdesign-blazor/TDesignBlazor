using Microsoft.AspNetCore.Components.Rendering;
using TDesign.Notification;

namespace TDesign;
/// <summary>
/// 承载 <see cref="TNotification"/> 动态组件的容器。
/// </summary>
public class TNotificationContainer : NotifyContainerComponentBase<INotificationService, NotificationConfiguration>
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

                    content.CreateComponent<TNotification>(sequence + 1, builder => builder.AddContent(0, configuration.Content), new
                    {
                        configuration.Title,
                        configuration.SubTitle,
                        configuration?.Theme,
                        configuration?.Icon,
                        style = HtmlHelper.Instance.Style().Append("margin-bottom:16px")
                    }); ;
                }
            },
                HtmlHelper.Instance.CreateHtmlAttributes(attributes =>
                {
                    var (classOrStyle, value) = MessageConfiguration.GetPlacement(item.Key);
                    attributes["class"] = HtmlHelper.Instance.Class().Append(value, classOrStyle);
                    attributes["style"] = HtmlHelper.Instance.Class().Append("z-index:6000").Append(value, !classOrStyle);
                })
            );
        }
    }
}
