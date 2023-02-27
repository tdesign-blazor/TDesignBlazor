using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.Forms;

namespace TDesign.Test.Components.Input;
public class InputRangeTest : TestBase<TInputRange<string>>
{
    [Fact(DisplayName = "InputRange - 渲染 css")]
    public void Test_Render()
    {
        var component = RenderComponent(m =>
         m.Bind(p => p.StartValue, "10", value => { })
         .Bind(p => p.EndValue, "50", value => { })
         );
        component.Should().HaveClass("t-range-input");

        component.Find(".t-range-input__inner");
        //component.FindComponent<InputText>().Find(".t-range-input__inner-left");
        //component.FindComponent<InputText>().Find(".t-range-input__inner-right");
    }

    [Fact(DisplayName = "InputRange - Size 参数")]
    public void Test_Size_Parameter()
    {
        var component = RenderComponent(m =>
         m.Bind(p => p.StartValue, "10", value => { })
         .Bind(p => p.EndValue, "50", value => { })
         .Add(p => p.Size, Size.Small)
         );
        component.Should().HaveClass("t-size-s");
    }

    [Fact(DisplayName ="InputRange - Seperator 参数")]
    public void Test_Seperator_Parameter()
    {
        var component = RenderComponent(m =>
         m.Bind(p => p.StartValue, "10", value => { })
         .Bind(p => p.EndValue, "50", value => { })
         .Add(p => p.Seperator, "/")
         );

        component.Find(".t-range-input__inner-separator").Html().Should().Be("/");
    }
}
