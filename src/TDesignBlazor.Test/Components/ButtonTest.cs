namespace TDesignBlazor.Test.Components;

public class ButtonTest : TestBase
{
    [Fact(DisplayName = "按钮 - 渲染 button 元素标记")]
    public void Test_Render_Button_Tag()
    {
        TestContext.RenderComponent<Button>().Should().HaveTag("button");
    }

    [Fact(DisplayName = "按钮 - Theme 参数 ")]
    public void Test_Theme_Parameter()
    {
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Theme, Theme.Primary)).Should().HaveClass("t-button--theme-primary");

        TestContext.RenderComponent<Button>(m => m.Add(p => p.Theme, Theme.Success)).Should().HaveClass("t-button--theme-success");

        TestContext.RenderComponent<Button>(m => m.Add(p => p.Theme, Theme.Danger)).Should().HaveClass("t-button--theme-danger");

        TestContext.RenderComponent<Button>(m => m.Add(p => p.Theme, Theme.Warning)).Should().HaveClass("t-button--theme-warning");
    }

    [Fact(DisplayName = "按钮 - HtmlType 参数渲染 HTML 的 type 属性")]
    public void Test_HtmlType_Parameter()
    {
        TestContext.RenderComponent<Button>(m => m.Add(p => p.HtmlType, ButtonHtmlType.Button)).Should().HaveAttribute("type", "button");

        TestContext.RenderComponent<Button>(m => m.Add(p => p.HtmlType, ButtonHtmlType.Submit)).Should().HaveAttribute("type", "submit");

        TestContext.RenderComponent<Button>(m => m.Add(p => p.HtmlType, ButtonHtmlType.Reset)).Should().HaveAttribute("type", "reset");
    }

    [Fact(DisplayName = "按钮 - Type 参数")]
    public void Test_Type_Parameter()
    {
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Type, ButtonType.Dashed)).Should().HaveClass("t-button--variant-dashed");

        TestContext.RenderComponent<Button>(m => m.Add(p => p.Type, ButtonType.Base)).Should().HaveClass("t-button--variant-base");

        TestContext.RenderComponent<Button>(m => m.Add(p => p.Type, ButtonType.Outline)).Should().HaveClass("t-button--variant-outline");

        TestContext.RenderComponent<Button>(m => m.Add(p => p.Type, ButtonType.Text)).Should().HaveClass("t-button--variant-text");
    }

    [Fact(DisplayName = "按钮 - Ghost 参数")]
    public void Test_Ghost_Parameter()
    {
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Ghost, true)).Should().HaveClass("t-button--ghost");
    }

    [Fact(DisplayName = "按钮 - Size 参数")]
    public void Test_Size_Parameter()
    {
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Size, Size.Small)).Should().HaveClass("t-size-s");
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Size, Size.Medium)).Should().HaveClass("t-size-m");
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Size, Size.Large)).Should().HaveClass("t-size-l");
    }

    [Fact(DisplayName = "按钮 - Block 参数")]
    public void Test_Block_Parameter()
    {
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Block, true)).Should().HaveClass("t-size-full-width");
    }

    [Fact(DisplayName = "按钮 - Shape 参数")]
    public void Test_Shape_Parameter()
    {
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Shape, ButtonShape.Rectangle)).Should().HaveClass("t-button--shape-rectangle");
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Shape, ButtonShape.Square)).Should().HaveClass("t-button--shape-square");
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Shape, ButtonShape.Circle)).Should().HaveClass("t-button--shape-circle");
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Shape, ButtonShape.Round)).Should().HaveClass("t-button--shape-round");
    }

    [Fact(DisplayName = "按钮 - Disabled 参数")]
    public void Test_Disabled_Parameter()
    {
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Disabled, true)).Should().HaveClass("t-is-disabled");
    }


    [Fact(DisplayName = "按钮 - Loading 参数")]
    public void Test_Loading_Parameter()
    {
        TestContext.RenderComponent<Button>(m => m.Add(p => p.Loading, true)).Should().HaveClass("t-is-loading");
    }
}