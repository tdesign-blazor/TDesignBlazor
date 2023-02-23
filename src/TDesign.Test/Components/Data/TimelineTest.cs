using AngleSharp.Diffing.Extensions;
using ComponentBuilder;

namespace TDesign.Test.Components.Data;
public class TimelineTest : TestBase<TTimeline>
{
    [Fact(DisplayName = "Timeline - 渲染")]
    public void Test_Render()
    {
        var component = RenderComponent(m => m.AddChildContent(builder =>
        {
            builder.CreateComponent<TTimelineItem>(0, "事件一", new { Label = "2022-01-01" });
        }));

        component.Should().HaveTag("ul").And.HaveClass("t-timeline").And.HaveClass("t-timeline-left").And.HaveClass("t-timeline-vertical")
            .And.HaveClass("t-timeline-label--same").And.HaveClass("t-timeline-label");


        var item = component.FindComponent<TTimelineItem>();

        item.Should().NotBeNull().And.HaveTag("li").And.HaveClass("t-timeline-item").And.HaveClass("t-timeline-item-left");

        var wrapper = item.Find(".t-timeline-item__wrapper");
        wrapper.Should().NotBeNull().And.HaveTag("div");

        var dot = wrapper.Children[0];
        dot.Should().NotBeNull().And.HaveClass("t-timeline-item__dot").And.HaveClass("t-timeline-item__dot--primary");

        NotEmpty(dot.Children);

        dot.Children[0].Should().NotBeNull().And.HaveClass("t-timeline-item__dot-content");

        var tail = wrapper.LastElementChild;
        tail.Should().NotBeNull().And.HaveClass("t-timeline-item__tail").And.HaveClass("t-timeline-item__tail--theme-default")
            .And.HaveClass("t-timeline-item__tail--status-primary");


        var content = item.Find(".t-timeline-item__content");
        content.Should().NotBeNull();

        content.FirstElementChild.Should().NotBeNull().And.HaveClass("t-timeline-item__label").And.HaveClass("t-timeline-item__label--same")
            ;
    }

    [Fact(DisplayName ="Timeline - 为最后一个 TimelineItem 附加一个 AdditionalClass",Skip ="暂时不知道为什么无法加载样式")]
    public void Test_OnParameterSet_Then_Last_TimelineItem_Has_AdditionalClass()
    {
        var timeline = RenderComponent(m => m.AddChildContent(b =>
        {
            b.CreateComponent<TTimelineItem>(0);
            b.CreateComponent<TTimelineItem>(1);
        }));

        var cssClass= timeline.FindAll(".t-timeline-item").Last().GetAttribute("class");

        cssClass.Split(" ").Should().Contain("t-timeline-item--last");
    }

    [Fact(DisplayName = "Timeline - 基础时间轴 - 垂直")]
    public void Test_Basic_Timeline_Vertical()
    {
        RenderComponent().Should().HaveClass("t-timeline-vertical");
    }

    [Fact(DisplayName = "Timeline - 基础时间轴 - 水平")]
    public void Test_Basic_Timeline_Horizontal()
    {
        RenderComponent(m => m.Add(p => p.Horizontal, true)).Should().HaveClass("t-timeline-horizontal");
    }

    [Theory(DisplayName = "Timeline - 不同的颜色 Color")]
    [InlineData(new object[] { Color.Primary })]
    [InlineData(new object[] { Color.Success })]
    [InlineData(new object[] { Color.Warning })]
    [InlineData(new object[] { Color.Error })]
    public void Test_Timeline_With_Different_Color(Color color)
    {
        var item = RenderComponent(m => m.AddChildContent(builder =>
                {
                    builder.CreateComponent<TTimelineItem>(0, attributes: new { Color = color });
                })).FindComponent<TTimelineItem>().Find(".t-timeline-item__dot");


        item.Should().HaveClass($"t-timeline-item__dot--{color.ToString().ToLower()}");
    }

    [Theory(DisplayName ="Timeline - Them 参数")]
    [InlineData(new object[] { TimelineTheme.Default})]
    [InlineData(new object[] { TimelineTheme.Dot })]
    public void Test_Timeline_Theme_Parameter(TimelineTheme theme)
    {
        var item = RenderComponent(m => m.Add(p => p.Theme, theme).AddChildContent(builder =>
        {
            builder.CreateComponent<TTimelineItem>(0);
        })).FindComponent<TTimelineItem>().Find(".t-timeline-item__tail");

        item.Should().HaveClass($"t-timeline-item__tail--theme-{theme.GetCssClass()}").And.HaveClass("t-timeline-item__tail--status-primary");
    }

    [Fact(DisplayName ="Timeline - Alternative 参数")]
    public void Test_Timeline_Alternative_Parameter()
    {
        RenderComponent(m => m.Add(p => p.Alternate, false)).Should().HaveClass("t-timeline-label--same");
        RenderComponent(m => m.Add(p => p.Alternate, true)).Should().HaveClass("t-timeline-label--alternate");
    }

    [Fact(DisplayName = "Timeline - Horizontal 参数")]
    public void Test_Timeline_Horizontal_Parameter()
    {
        RenderComponent(m => m.Add(p => p.Horizontal, false)).Should().HaveClass("t-timeline-vertical");
        RenderComponent(m => m.Add(p => p.Horizontal, true)).Should().HaveClass("t-timeline-horizontal");
    }

    [Theory(DisplayName = "Timeline - LabelAlignment 参数")]
    [InlineData(new object[] { TimelineLabelAlignment.Top})]
    [InlineData(new object[] { TimelineLabelAlignment.Left })]
    [InlineData(new object[] { TimelineLabelAlignment.Right })]
    [InlineData(new object[] { TimelineLabelAlignment.Bottom })]
    [InlineData(new object[] { TimelineLabelAlignment.Alternate })]
    public void Test_Timeline_LabelAlignment_Parameter(TimelineLabelAlignment alignment)
    {
        RenderComponent(m => m.Add(p => p.LabelAlignment, alignment)).Should().HaveClass($"t-timeline-{alignment.GetCssClass()}");
    }

    [Fact(DisplayName ="Timeline - TimelineItem 的 Icon 参数")]
    public void Test_TimelineItem_Icon()
    {
        RenderComponent(m => m.AddChildContent(builder =>
        {
            builder.CreateComponent<TTimelineItem>(0, attributes: new { IconName = IconName.Add });
        }))
            .FindComponent<TTimelineItem>().FindComponent<TIcon>();
    }

    [Fact(DisplayName ="Timeline - TimelineItem 的 AdditionalClass 参数")]
    public void Test_TimelineItem_AdditionalClass_Parameter()
    {
        RenderComponent(m => m.AddChildContent(builder =>
        {
            builder.CreateComponent<TTimelineItem>(0, attributes: new { AdditionalClass = "custom-class" });
        }))
            .FindComponent<TTimelineItem>().Should().HaveClass("custom-class");
    }
}
