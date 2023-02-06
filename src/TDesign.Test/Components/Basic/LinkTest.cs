namespace TDesign.Test.Components.Basic;
public class LinkTest : TestBase
{
    [Fact(DisplayName = "Link - 渲染 a 元素和默认 t-link 样式")]
    public void Test_Link_Has_A_Tag()
    {
        TestContext.RenderComponent<TLink>().Should().HaveTag("a").And.HaveClass("t-link");
    }

    [Fact(DisplayName = "Link - Size 参数")]
    public void Test_Size_Parameter()
    {
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Size, Size.Small)).Should().HaveClass("t-size-s");
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Size, Size.Medium)).Should().HaveClass("t-size-m");
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Size, Size.Large)).Should().HaveClass("t-size-l");
    }

    [Fact(DisplayName = "Link - Disabled 参数")]
    public void Test_Disabled_Parameter()
    {
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Disabled, true)).Should().HaveClass("t-is-disabled");
    }

    [Fact(DisplayName = "Link - Theme 参数 ")]
    public void Test_Theme_Parameter()
    {
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Theme, Theme.Primary)).Should().HaveClass("t-link--theme-primary");
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Theme, Theme.Success)).Should().HaveClass("t-link--theme-success");
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Theme, Theme.Danger)).Should().HaveClass("t-link--theme-danger");
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Theme, Theme.Warning)).Should().HaveClass("t-link--theme-warning");
    }

    [Fact(DisplayName = "Link - Underline 参数")]
    public void Test_Underline_Parameter()
    {
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Underline, true)).Should().HaveClass("t-is-underline");
    }
    [Fact(DisplayName = "Link - Hover 参数")]
    public void Test_Hover_Parameter()
    {
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Hover, LinkHover.Underline)).Should().HaveClass("t-link--hover-underline");
        TestContext.RenderComponent<TLink>(m => m.Add(p => p.Hover, LinkHover.Color)).Should().HaveClass("t-link--hover-color");
    }
}
