using ComponentBuilder;

namespace TDesign.Test.Components;
public class GridTest : TestBase
{
    [Fact(DisplayName = "Grid - 1行3列占比1")]
    public void Test_Grid_Render()
    {
        var component = TestContext.RenderComponent<TRow>(m => m.AddChildContent(b =>
        {
            b.CreateComponent<TColumn>(0, attributes: new { Span = ColumnSpan.Is1 });
            b.CreateComponent<TColumn>(1, attributes: new { Span = ColumnSpan.Is1 });
            b.CreateComponent<TColumn>(2, attributes: new { Span = ColumnSpan.Is1 });
        }));

//        Equal(3, component.Instance.ChildComponents.Count);

        var row = component.Find(".t-row");
        NotNull(row);

        row.GetAttribute("class").Should().Contain("t-row").And.ContainAny("t-row--start", "t-row--top");

        var columns = component.FindAll(".t-col");

        //columns.Should().HaveCount(3);
        columns[0].Should().HaveClass("t-col-1");
    }
}
