namespace TDesign.Test.Components;
public class IconTest : TestBase
{
    [Fact(DisplayName = "TIcon - 渲染 i 元素和默认的样式")]
    public void Test_Render_Tag_And_Defailt_Class()
    {
        TestContext.RenderComponent<TIcon>().Should().HaveTag("i").And.HaveClass("t-icon");
    }

    [Theory(DisplayName = "TIcon - Name 参数")]
    [InlineData(new object[] { IconName.Home, "home" })]
    [InlineData(new object[] { IconName.Add, "add" })]
    [InlineData(new object[] { IconName.InfoCircle, "info-circle" })]
    [InlineData(new object[] { IconName.Browse, "browse" })]
    public void Test_Name_Parameter(object name, string css)
    {
        TestContext.RenderComponent<TIcon>(m => m.Add(p => p.Name, name)).Should().HaveClass($"t-icon-{css}");
    }

    [Fact(DisplayName = "TIcon - Size 参数")]
    public void Test_Size_Parameter()
    {
        TestContext.RenderComponent<TIcon>(m => m.Add(p => p.Size, "16px")).Should().HaveAttribute($"style", "font-size:16px");
    }
    [Fact(DisplayName = "TIcon - Color 参数")]
    public void Test_Color_Parameter()
    {
        TestContext.RenderComponent<TIcon>(m => m.Add(p => p.Color, "#ff0000")).Should().HaveAttribute($"style", "color:#ff0000");
    }
}
