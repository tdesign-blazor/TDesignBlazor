namespace TDesign.Test.Components.Notification;
public class PopupTest : TestBase<TPopup>
{
    [Fact(DisplayName = "Popup - 组件渲染")]
    public void Test_Popup_Render()
    {
        GetComponent().Should().HaveClass("t-popup");
    }
}
