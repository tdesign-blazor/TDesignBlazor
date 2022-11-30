namespace TDesign.Test.Components;
public class PopupTest : TestBase<TPopup>
{
    [Fact(DisplayName = "Popup - 组件渲染")]
    public void Test_Popup_Render()
    {
        GetComponent().Should().HaveClass("t-popup");
    }
}
