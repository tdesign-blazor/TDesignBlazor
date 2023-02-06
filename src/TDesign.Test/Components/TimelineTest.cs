using ComponentBuilder;

namespace TDesign.Test.Components;
public class TimelineTest : TestBase<TTimeline>
{
    [Fact(DisplayName = "Timeline - 渲染")]
    public void Test_Render()
    {
        var component = GetComponent(m => m.AddChildContent(builder =>
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

//        component.MarkupMatches(@"
//<ul class=""t-timeline t-timeline-left t-timeline-vertical t-timeline-label--same t-timeline-label"">
//    <li class=""t-timeline-item t-timeline-item-left "">
//        <div class=""t-timeline-item__wrapper"">
//            <div class=""t-timeline-item__dot  t-timeline-item__dot--primary"">
//                <div class=""t-timeline-item__dot-content""></div>
//            </div>
//            <div class=""t-timeline-item__tail t-timeline-item__tail--theme-default t-timeline-item__tail--status-primary"">
//            </div>
//        </div>
//        <div class=""t-timeline-item__content"">
//            事件一
//            <div class=""t-timeline-item__label t-timeline-item__label--same"">2022-01-01</div>
//        </div>
//    </li>
//</ul>
//");


    }

    [Fact(DisplayName = "Timeline - 基础时间轴 - 垂直")]
    public void Test_Basic_Timeline_Vertical()
    {
        GetComponent().Should().HaveClass("t-timeline-vertical");
    }

    [Fact(DisplayName = "Timeline - 基础时间轴 - 水平")]
    public void Test_Basic_Timeline_Horizontal()
    {
        GetComponent(m => m.Add(p => p.Horizontal, true)).Should().HaveClass("t-timeline-horizontal");
    }

    [Theory(DisplayName = "Timeline - 不同的颜色 Color")]
    [InlineData(new object[] { Color.Primary })]
    [InlineData(new object[] { Color.Success })]
    [InlineData(new object[] { Color.Warning })]
    [InlineData(new object[] { Color.Error })]
    public void Test_Timeline_With_Different_Color(Color color)
    {
        var item = GetComponent(m => m.AddChildContent(builder =>
                {
                    builder.CreateComponent<TTimelineItem>(0, attributes: new { Color = color });
                })).FindComponent<TTimelineItem>().Find(".t-timeline-item__dot");


        item.Should().HaveClass($"t-timeline-item__dot--{color.ToString().ToLower()}");
    }
}
