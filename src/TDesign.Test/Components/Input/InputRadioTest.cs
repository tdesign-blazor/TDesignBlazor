using AngleSharp.Dom;

namespace TDesign.Test.Components.Input;
public class InputRadioTest:TestBase
{
    [Fact(DisplayName ="InputRadio - 单选框渲染")]
    public void Test_Render_Radio_In_Group()
    {
        int value = 0;
        var component = TestContext.RenderComponent<TInputRadioGroup<int>>(m => m.AddChildContent(builder =>
        {
            builder.CreateComponent<TInputRadio<int>>(0, "1", new
            {
                Value = 1,
            });
            builder.CreateComponent<TInputRadio<int>>(0, "2", new
            {
                Value = 2,
            });
            builder.CreateComponent<TInputRadio<int>>(0, "3", new
            {
                Value = 3,
            });
        }).Bind(p => p.Value, value, _value => value = _value));

        component.Find(".t-radio-group").Should().NotBeNull();

        //Equal(2, value);
        var radio = component.Find(".t-radio-group>.t-radio");
        radio.GetElementCount().Should().Be(3);

        var input = radio.FirstElementChild;
        input?.Should()?.HaveTag("input").And.HaveClass("t-radio__former");
        input?.NextElementSibling?.Should().HaveTag("span").And.HaveClass("t-radio__input");
        radio.LastElementChild?.Should().HaveTag("span").And.HaveClass("t-radio__label");
    }

    [Fact(DisplayName ="InputRadio - Button 样式")]
    public void Test_Radio_ButtonStyle()
    {
        int value = 0;
        var component = TestContext.RenderComponent<TInputRadioGroup<int>>(m => m.AddChildContent(builder =>
        {
            builder.CreateComponent<TInputRadio<int>>(0, "1", new
            {
                Value = 1,
            });
            builder.CreateComponent<TInputRadio<int>>(0, "2", new
            {
                Value = 2,
            });
            builder.CreateComponent<TInputRadio<int>>(0, "3", new
            {
                Value = 3,
            });
        }).Bind(p => p.Value, value, _value => value = _value)
        .Add(p=>p.ButtonStyle, RadioButtonStyle.Outline)
        );
        component.Should().HaveClass("t-radio-group__outline");
        var radio = component.Find(".t-radio-group>.t-radio-button");
        radio.GetElementCount().Should().Be(3);

        var input = radio.FirstElementChild;
        input?.Should()?.HaveTag("input").And.HaveClass("t-radio-button__former");
        input?.NextElementSibling?.Should().HaveTag("span").And.HaveClass("t-radio-button__input");
        radio.LastElementChild?.Should().HaveTag("span").And.HaveClass("t-radio-button__label");

    }
}
