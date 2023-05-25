using AngleSharp.Dom;
using TDesign;

namespace TDesign.Test.Components.Notification;
public class DialogTest:TestBase<TDialog>
{
    [Fact(DisplayName = "TDialog - 渲染布局")]
    public void Test_Render_Dialog()
    {
        var dialog = base.RenderComponent(b => b.AddChildContent("Dialog").Add(p => p.HeaderContent, "标题").Add(p => p.FooterContent, "底部"));

        dialog.Find("div").Should().HaveClass("t-dialog__ctx").And.HaveClass("t-dialog__ctx--fixed").And.HaveAttribute("style", "display:none");

        var inner= dialog.Find(".t-dialog__ctx").Children;
        inner.Should().HaveCount(2);

        inner[0].Should().HaveClass("t-dialog__mask");
        inner[1].Should().HaveClass("t-dialog__wrap");

        inner[1].Children[0].Should().HaveClass("t-dialog__position").And.HaveClass("t-dialog--top");

        var dialogInner = inner[1].Children[0].Children[0];

        dialogInner.Should().HaveClass("t-dialog");
        dialogInner.Children.Should().HaveCount(3);

        var header = dialogInner.Children[0];

        header.Should().HaveClass("t-dialog__header");
        header.Children[0].Should().HaveClass("t-dialog__header-content");

        var body = dialogInner.Children[1];
        body.Should().HaveClass("t-dialog__body");

        var footer = dialogInner.Children[2];
        footer.Should().HaveClass("t-dialog__footer");
    }
}
