namespace TDesignBlazor.Test.Components;
public class BreadcrumbTest : TestBase<BreadcrumbItem>
{
    [Fact(DisplayName = "Breadcrumb - 渲染元素和默认样式")]
    public void Test_Render_Breadcrumb()
    {
        TestContext.RenderComponent<Breadcrumb>().Should().HaveClass("t-breadcrumb");
    }

    [Fact(DisplayName = "BreadcrumbItem - 渲染子项和默认样式")]
    public void Test_Render_BreadcrumbItem()
    {
        GetComponent().Should().HaveClass("t-breadcrumb__item");
    }

    [Fact(DisplayName = "BreadcrumbItem - Link 参数")]
    public void Test_Link_Parameter_Render_Tag()
    {
        GetComponent(m => m.Add(p => p.Link, "home")).Should().HaveChildMarkup(@$"
<a class=""t-breadcrumb--text-overflow t-link"">
    <span class=""t-breadcrumb__inner"" style=""max-width:120px"">        
    </span>
</a>
");
    }

    [Fact(DisplayName = "BreadcrumbItem - 渲染内部元素和样式")]
    public void Test_Render_Inner_Text()
    {
        GetComponent(m => m.AddChildContent("home")).Should().HaveChildMarkup(@$"
<span class=""t-breadcrumb--text-overflow"">
    <span class=""t-breadcrumb__inner"" style=""max-width:120px"">
        home
    </span>
</span>
");
    }

    [Fact(DisplayName = "BreadcrumbItem - Seperator 参数")]
    public void Test_Seperator_Parameter()
    {
        GetComponent(m => m.Add(p => p.SeperatorContent, b => b.AddContent(0, "/")))
            .Should().HaveMarkup(@$"
<div class=""t-breadcrumb__item light"" href="""">
    <span class=""t-breadcrumb--text-overflow"">
        <span class=""t-breadcrumb__inner"" style=""max-width:120px""></span>
    </span>
    <span class=""t-breadcrumb__separator"">/</span>
</div>
");
    }
}
