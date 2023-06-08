using ComponentBuilder.FluentRenderTree;

namespace TDesign.Test.Components.Notification;
public class AlertTest:TestBase<TAlert>
{
    [Fact(DisplayName ="TAlert - 普通的消息")]
    public void Test_Render_Alert()
    {
        RenderComponent(m=>m.AddChildContent("hello")).MarkupMatches(builder =>
        {
            builder.Div("t-alert t-alert--info")
            .Content(b => {
                b.Div("t-alert__icon").Content(i => i.Component<TIcon>().Attribute(m => m.Name, IconName.InfoCircle).Close())
                            .Close();

                b.Div("t-alert__content")
                    .Content(message =>
                    {
                        message.Div("t-alert__message")
                                .Content(desc=>desc.Div("t-alert__description").Content("hello").Close())
                                .Close();
                    })
                    .Close();
                            })
            .Close();
        });
    }

    [Fact(DisplayName = "TAlert - 有操作内容")]
    public void Test_Render_Alert_With_OperationContent()
    {
        RenderComponent(m => m.AddChildContent("hello").Add(m=>m.OperationContent,"操作内容")).MarkupMatches(builder =>
        {
            builder.Div("t-alert t-alert--info")
            .Content(b => {
                b.Div("t-alert__icon").Content(i => i.Component<TIcon>().Attribute(m => m.Name, IconName.InfoCircle).Close())
                            .Close();

                b.Div("t-alert__content")
                    .Content(content =>
                    {
                        content.Div("t-alert__message")
                                .Content(message =>
                                {
                                    message.Div("t-alert__description").Content("hello").Close();
                                    message.Div("t-alert__operation").Content("操作内容").Close();
                                })
                                .Close();
                    })
                    .Close();
            })
            .Close();
        });
    }

    [Fact(DisplayName = "TAlert - 有标题的警告")]
    public void Test_Render_Alert_With_TitleContent()
    {
        RenderComponent(m => m.AddChildContent("hello").Add(m => m.TitleText, "标题")).MarkupMatches(builder =>
        {
            builder.Div("t-alert t-alert--info")
            .Content(b => {
                b.Div("t-alert__icon").Content(i => i.Component<TIcon>().Attribute(m => m.Name, IconName.InfoCircle).Close())
                            .Close();

                b.Div("t-alert__content")
                    .Content(content =>
                    {
                        content.Div("t-alert__title").Content("标题").Close();

                        content.Div("t-alert__message")
                                .Content(message =>
                                {
                                    message.Div("t-alert__description").Content("hello").Close();
                                })
                                .Close();
                    })
                    .Close();
            })
            .Close();
        });
    }

    [Fact(DisplayName = "TAlert - 带关闭图标")]
    public void Test_Render_Alert_WithCloseIcon()
    {
        RenderComponent(m => m.AddChildContent("hello").Add(m=>m.Closable,true)).MarkupMatches(builder =>
        {
            builder.Div("t-alert t-alert--info")
            .Content(b => {
                b.Div("t-alert__icon").Content(i => i.Component<TIcon>().Attribute(m => m.Name, IconName.InfoCircle).Close())
                            .Close();

                b.Div("t-alert__content")
                    .Content(message =>
                    {
                        message.Div("t-alert__message")
                                .Content(desc => desc.Div("t-alert__description").Content("hello").Close())
                                .Close();
                    })
                    .Close();


                b.Div("t-alert__close").Content(icon => icon.Component<TIcon>().Attribute(m => m.Name, IconName.Close)).Close();
            })
            .Close();
        });
    }
}
