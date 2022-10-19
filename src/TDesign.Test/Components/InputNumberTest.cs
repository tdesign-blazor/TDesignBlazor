using AngleSharp.Common;

using ComponentBuilder;

using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace TDesign.Test.Components;
public class InputNumberTest : TestBase
{
    IRenderedComponent<TInputNumber<TValue>> GetInputNumer<TValue>(Action<ComponentParameterCollectionBuilder<TInputNumber<TValue>>> parameterBuilder)
        => TestContext.RenderComponent(parameterBuilder);

    IRenderedComponent<TInputNumber<TValue>> GetInputNumer<TValue>(params ComponentParameter[] parameters)
        => TestContext.RenderComponent<TInputNumber<TValue>>(parameters);

    [Fact(DisplayName = "InputNumber - 渲染组件")]
    public void Test_Render_Int()
    {
        int value = 1;
        GetInputNumer<int>(x =>
        {
            x.Add(c => c.Value, value);
            x.Add(p => p.ValueChanged, (s) => value = s);
            x.Add(p => p.ValueExpression, () => value);
        })
        .Should()
        .HaveTag("div")
        .And.HaveClass("t-input-number")
        .And.HaveClass("t-size-m")
        .And.HaveClass("t-input-number--row");
    }
    [Fact(DisplayName = "InputNumber - Size")]
    public void Test_Render_Parameter_Size()
    {
        int value = 1;
        GetInputNumer<int>(x =>
        {
            x.Add(c => c.Value, value);
            x.Add(p => p.ValueChanged, (s) => value = s);
            x.Add(p => p.ValueExpression, () => value);
            x.Add(p => p.Size, Size.Small);
        }).Should().HaveClass(Size.Small.GetCssClass());

        GetInputNumer<int>(x =>
        {
            x.Add(c => c.Value, value);
            x.Add(p => p.ValueChanged, (s) => value = s);
            x.Add(p => p.ValueExpression, () => value);
            x.Add(p => p.Size, Size.Medium);
        }).Should().HaveClass(Size.Medium.GetCssClass());

        GetInputNumer<int>(x =>
        {
            x.Add(c => c.Value, value);
            x.Add(p => p.ValueChanged, (s) => value = s);
            x.Add(p => p.ValueExpression, () => value);
            x.Add(p => p.Size, Size.Large);
        }).Should().HaveClass(Size.Large.GetCssClass());

    }
}
