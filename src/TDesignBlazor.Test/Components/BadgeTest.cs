using ComponentBuilder;

namespace TDesignBlazor.Test.Components;
public class BadgeTest : TestBase<Badge>
{
    [Fact(DisplayName = "Badge - 渲染 div 元素和默认样式")]
    public void Test_Render_Badge()
    {
        GetComponent().Should().HaveTag("div").And.HaveClass("t-badge");
    }

    [Fact(DisplayName = "Badge - ChildContent 参数")]
    public void Test_ChildContent_Parameter()
    {
        GetComponent(m => m.AddChildContent("test")).Should().HaveChildMarkup("<div class=\"badge-block\">test</div>");
    }

    [Theory(DisplayName = "Badge - Shape 参数")]
    [InlineData(new object[] { BadgeShape.Circle })]
    [InlineData(new object[] { BadgeShape.Round })]
    [InlineData(new object[] { BadgeShape.Dot })]
    public void Test_Shape_Parameter(BadgeShape shape)
    {
        GetComponent(m => m.Add(p => p.Shape, shape))
            .
            MarkupMatches($@"
<div class=""t-badge"">
    <div class=""badge-block""></div>
    <div class=""t-badge--{shape.GetCssClass()}"" style=""""></div>
</div>
");
    }

    [Fact(DisplayName = "Badge - Small 参数")]
    public void Test_Small_Parameter()
    {
        GetComponent(m => m.Add(p => p.Small, true))
    .MarkupMatches($@"
<div class=""t-badge"">
    <div class=""badge-block""></div>
    <div class=""t-badge--{ShapeType.Circle.GetCssClass()} t-size-s"" style=""""></div>
</div>
");
    }

    [Fact(DisplayName = "Badge - Text 参数")]
    public void Test_Text_Parameter()
    {
        GetComponent(m => m.Add(p => p.Text, "hello"))
    .MarkupMatches($@"
<div class=""t-badge"">
    <div class=""badge-block""></div>
    <div class=""t-badge--{ShapeType.Circle.GetCssClass()}"" style="""">hello</div>
</div>
");
    }
}
