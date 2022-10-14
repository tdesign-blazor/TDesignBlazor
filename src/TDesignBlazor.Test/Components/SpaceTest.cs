namespace TDesignBlazor.Test.Components;
public class SpaceTest : TestBase<TSpace>
{
    [Fact(DisplayName = "Space - 渲染和默认样式")]
    public void Test_Render()
    {
        GetComponent().Should().HaveClass("t-space").And.HaveClass("t-space-horizontal");
    }
    [Fact(DisplayName = "Space - Vertical 参数")]
    public void Test_Vertical_Parameter()
    {
        GetComponent(m => m.Add(p => p.Vertical, true)).Should().HaveClass("t-space-vertical");
    }

    [Fact(DisplayName = "Space - Gap 参数")]
    public void Test_Gap_Parameter()
    {
        GetComponent(m => m.Add(p => p.Gap, "12px")).Should().HaveAttribute("style", "gap:12px");
    }

    [Fact(DisplayName = "Space - BreakLine 参数")]
    public void Test_BreakLine_Parameter()
    {
        GetComponent(m => m.Add(p => p.BreakLine, true)).Should().HaveAttribute("style", "gap:16px;flex-wrap:wrap");
    }

    [Fact(DisplayName = "TSpaceItem - 渲染元素和默认样式")]
    public void Test_SpaceItem_Render()
    {
        TestContext.RenderComponent<TSpaceItem>().Should().HaveClass("t-space-item");
    }
}
