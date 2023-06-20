using ComponentBuilder.FluentRenderTree;

namespace TDesign.Test.Components.Basic;
public class ButtonTest : TestBase<TButton>
{
    [Fact(DisplayName = "按钮 - 渲染 button 元素标记")]
    public void Test_Render_Button_Tag()
    {
        RenderComponent().Should().HaveTag("button");
    }

    [Fact(DisplayName = "按钮 - Theme 参数 ")]
    public void Test_Theme_Parameter()
    {
        RenderComponent(m => m.Add(p => p.Theme, Theme.Primary)).Should().HaveClass("t-button--theme-primary");

        RenderComponent(m => m.Add(p => p.Theme, Theme.Success)).Should().HaveClass("t-button--theme-success");

        RenderComponent(m => m.Add(p => p.Theme, Theme.Danger)).Should().HaveClass("t-button--theme-danger");

        RenderComponent(m => m.Add(p => p.Theme, Theme.Warning)).Should().HaveClass("t-button--theme-warning");
    }

    [Fact(DisplayName = "按钮 - HtmlType 参数渲染 HTML 的 type 属性")]
    public void Test_HtmlType_Parameter()
    {
        RenderComponent(m => m.Add(p => p.HtmlType, ButtonHtmlType.Button)).Should().HaveAttribute("type", "button");

        RenderComponent(m => m.Add(p => p.HtmlType, ButtonHtmlType.Submit)).Should().HaveAttribute("type", "submit");

        RenderComponent(m => m.Add(p => p.HtmlType, ButtonHtmlType.Reset)).Should().HaveAttribute("type", "reset");
    }

    [Fact(DisplayName = "按钮 - Varient 参数")]
    public void Test_Type_Parameter()
    {
        RenderComponent(m => m.Add(p => p.Varient, ButtonVarient.Dashed)).Should().HaveClass("t-button--variant-dashed");

        RenderComponent(m => m.Add(p => p.Varient, ButtonVarient.Base)).Should().HaveClass("t-button--variant-base");

        RenderComponent(m => m.Add(p => p.Varient, ButtonVarient.Outline)).Should().HaveClass("t-button--variant-outline");

        RenderComponent(m => m.Add(p => p.Varient, ButtonVarient.Text)).Should().HaveClass("t-button--variant-text");
    }

    [Fact(DisplayName = "按钮 - Ghost 参数")]
    public void Test_Ghost_Parameter()
    {
        RenderComponent(m => m.Add(p => p.Ghost, true)).Should().HaveClass("t-button--ghost");
    }

    [Fact(DisplayName = "按钮 - Size 参数")]
    public void Test_Size_Parameter()
    {
        RenderComponent(m => m.Add(p => p.Size, Size.Small)).Should().HaveClass("t-size-s");
        RenderComponent(m => m.Add(p => p.Size, Size.Medium)).Should().HaveClass("t-size-m");
        RenderComponent(m => m.Add(p => p.Size, Size.Large)).Should().HaveClass("t-size-l");
    }

    [Fact(DisplayName = "按钮 - Block 参数")]
    public void Test_Block_Parameter()
    {
        RenderComponent(m => m.Add(p => p.Block, true)).Should().HaveClass("t-size-full-width");
    }

    [Fact(DisplayName = "按钮 - Shape 参数")]
    public void Test_Shape_Parameter()
    {
        RenderComponent(m => m.Add(p => p.Shape, ButtonShape.Rectangle)).Should().HaveClass("t-button--shape-rectangle");
        RenderComponent(m => m.Add(p => p.Shape, ButtonShape.Square)).Should().HaveClass("t-button--shape-square");
        RenderComponent(m => m.Add(p => p.Shape, ButtonShape.Circle)).Should().HaveClass("t-button--shape-circle");
        RenderComponent(m => m.Add(p => p.Shape, ButtonShape.Round)).Should().HaveClass("t-button--shape-round");
    }

    [Fact(DisplayName = "按钮 - Disabled 参数")]
    public void Test_Disabled_Parameter()
    {
        RenderComponent(m => m.Add(p => p.Disabled, true)).Should().HaveClass("t-is-disabled").And.HaveAttribute("disabled","disabled");
    }


    [Fact(DisplayName = "按钮 - Loading 参数")]
    public void Test_Loading_Parameter()
    {
        RenderComponent(m => m.Add(p => p.Loading, true)).Should().HaveClass("t-is-loading");
    }

    [Fact(DisplayName ="按钮 - 有图标和文字一起适配")]
    public void Test_Icon_With_Text()
    {
        var button= RenderComponent(m => m.Add(p => p.Icon, IconName.Add));
        button.FindComponent<TIcon>().MarkupMatches(b => b.Component<TIcon>().Attribute(m => m.Name, IconName.Add).Close());
    }

    [Theory(DisplayName ="按钮 - 不同的 HTML 标记渲染按钮")]
    [InlineData(new object[] { "div"})]
    [InlineData(new object[] { "a" })]
    [InlineData(new object[] { "span" })]
    public void Test_Button_With_TagName(string tag)
    {
        RenderComponent(m => m.Add(p => p.TagName, tag)).Should().HaveTag(tag);
    }
}