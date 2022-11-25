namespace TDesign.Test.Components;
public class InputRangeTest : TestBase<TInputRange<string>>
{
    [Fact(DisplayName = "InputRange - 渲染 css")]
    public void Test_Render()
    {
        var component = GetComponent(m =>
         m.Bind(p => p.StartValue, "10", value => { })
         .Bind(p => p.EndValue, "50", value => { })
         );
        component.Should().HaveClass("t-range-input");

        component.Find(".t-range-input__inner");
        component.Find(".t-range-input__inner-left");
        component.Find(".t-range-input__inner-right");
    }

    [Fact(DisplayName = "InputRange - 尺寸")]
    public void Test_Size()
    {
        var component = GetComponent(m =>
         m.Bind(p => p.StartValue, "10", value => { })
         .Bind(p => p.EndValue, "50", value => { })
         .Add(p => p.Size, Size.Small)
         );
        component.Should().HaveClass("t-size-s");
    }
}
