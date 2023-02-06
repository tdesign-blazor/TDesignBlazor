using AngleSharp.Dom;

namespace TDesign.Test.Components.Layout;
public class BreadcrumbTest : TestBase<TBreadcrumbItem>
{
    [Fact(DisplayName = "Breadcrumb - 渲染元素和默认样式")]
    public void Test_Render_Breadcrumb()
    {
        TestContext.RenderComponent<TBreadcrumb>().Should().HaveClass("t-breadcrumb");
    }

    [Fact(DisplayName = "BreadcrumbItem - 渲染子项和默认样式")]
    public void Test_Render_BreadcrumbItem()
    {
        GetComponent().Should().HaveClass("t-breadcrumb__item");
    }

    [Fact(DisplayName = "BreadcrumbItem - Link 参数")]
    public void Test_Link_Parameter_Render_Tag()
    {
        GetComponent(m => m.Add(p => p.Link, "home"))
            .Find(".t-breadcrumb__item>.t-breadcrumb--text-overflow").Should().HaveClass("t-link");
    }

    [Fact(DisplayName = "BreadcrumbItem - 渲染内部元素和样式")]
    public void Test_Render_Inner_Text()
    {
        GetComponent(m => m.AddChildContent("home"))
            .Find(".t-breadcrumb__item>.t-breadcrumb--text-overflow>.t-breadcrumb__inner")
            .Html().Should().Equals("home");
    }

    [Fact(DisplayName = "BreadcrumbItem - Seperator 参数")]
    public void Test_Seperator_Parameter()
    {
        var component = GetComponent(m => m.Add(p => p.SeperatorContent, b => b.AddContent(0, "/")));
        component.Find(".t-breadcrumb__item>.t-breadcrumb__separator").Html().Should().Equals("/");
    }
}
