namespace TDesign.Test.Components.Basic;
public class LinkTest : TestBase<TLink>
{
    [Fact(DisplayName = "Link - 渲染 a 元素和默认 t-link 样式")]
    public void Test_Link_Has_A_Tag()
    {
        GetComponent().Should().HaveTag("a").And.HaveClass("t-link");
    }

    [Fact(DisplayName = "Link - Size 参数")]
    public void Test_Size_Parameter()
    {
        GetComponent(m => m.Add(p => p.Size, Size.Small)).Should().HaveClass("t-size-s");
        GetComponent(m => m.Add(p => p.Size, Size.Medium)).Should().HaveClass("t-size-m");
        GetComponent(m => m.Add(p => p.Size, Size.Large)).Should().HaveClass("t-size-l");
    }

    [Fact(DisplayName = "Link - Disabled 参数")]
    public void Test_Disabled_Parameter()
    {
        GetComponent(m => m.Add(p => p.Disabled, true)).Should().HaveClass("t-is-disabled");
    }

    [Fact(DisplayName = "Link - Theme 参数 ")]
    public void Test_Theme_Parameter()
    {
        GetComponent(m => m.Add(p => p.Theme, Theme.Primary)).Should().HaveClass("t-link--theme-primary");
        GetComponent(m => m.Add(p => p.Theme, Theme.Success)).Should().HaveClass("t-link--theme-success");
        GetComponent(m => m.Add(p => p.Theme, Theme.Danger)).Should().HaveClass("t-link--theme-danger");
        GetComponent(m => m.Add(p => p.Theme, Theme.Warning)).Should().HaveClass("t-link--theme-warning");
    }

    [Fact(DisplayName = "Link - Underline 参数")]
    public void Test_Underline_Parameter()
    {
        GetComponent(m => m.Add(p => p.Underline, true)).Should().HaveClass("t-is-underline");
    }
    [Fact(DisplayName = "Link - Hover 参数")]
    public void Test_Hover_Parameter()
    {
        GetComponent(m => m.Add(p => p.Hover, LinkHover.Underline)).Should().HaveClass("t-link--hover-underline");
        GetComponent(m => m.Add(p => p.Hover, LinkHover.Color)).Should().HaveClass("t-link--hover-color");
    }
    [Fact(DisplayName ="Link - Href 参数")]
    public void Test_Href_Parameter()
    {
        GetComponent(m => m.Add(p => p.Href, "tdesign.com")).Should().HaveAttribute("href", "tdesign.com");
    }
}
