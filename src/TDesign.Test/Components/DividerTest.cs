using ComponentBuilder;

namespace TDesign.Test.Components;
public class DividerTest : TestBase<TDivider>
{
    [Fact(DisplayName = "Divider - 默认样式的渲染")]
    public void Test_Render_With_Default_Class()
    {
        GetComponent().Should().HaveTag("div").And.HaveClass("t-divider");
    }

    [Fact(DisplayName = "Divider - Vertical 参数")]
    public void Test_Vertical_Parameter()
    {
        GetComponent(m => m.Add(p => p.Vertical, true)).Should().HaveClass("t-divider--vertical");
    }

    [Fact(DisplayName = "Divider - Dashed 参数")]
    public void Test_Dashed_Parameter()
    {
        GetComponent(m => m.Add(p => p.Dashed, true)).Should().HaveClass("t-divider--dashed");
    }

    [Theory(DisplayName = "Divider - Alighment 参数")]
    [InlineData(new object[] { HorizontalAlignment.Left })]
    [InlineData(new object[] { HorizontalAlignment.Center })]
    [InlineData(new object[] { HorizontalAlignment.Right })]
    public void Test_Alignment_Parameter(HorizontalAlignment alignment)
    {
        GetComponent(m => m.Add(p => p.Alignment, alignment)).Should().HaveClass($"t-divider--with-text-{alignment.GetCssClass()}");
    }

    [Fact(DisplayName = "Divider - ChildContent 赋值后会嵌套一层")]
    public void Test_ChildContent_Has_Inner_Span()
    {
        GetComponent(m => m.AddChildContent("test")).Should().HaveChildMarkup("<span class=\"t-divider__inner-text\">test</span>")
            .And.HaveClass("t-divider--with-text");
    }
}
