using AngleSharp.Common;
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
        .HaveClass("t-input-number")
        .And.HaveClass("t-size-m")
        .And.HaveClass("t-input-number--row");
//        .MarkupMatches(@"
//<div class=""t-input-number t-size-m t-input-number--row"">
//    <button type=""button"" class=""t-button t-size-m t-button--variant-outline t-button--theme-default t-button--shape-square t-input-number__decrease"">
//        <i class=""t-icon t-icon-remove"" />
//    </button>
//    <div class=""t-input__wrap"">
//        <div class=""t-input t-size-m t-is-default t-align-center""><input autocomplete=""off"" placeholder=""请输入"" type=""text"" unselectable=""off"" class=""t-input__inner""></div>
//    </div>
//    <button type=""button"" class=""t-button t-size-m t-button--variant-outline t-button--theme-default t-button--shape-square t-input-number__increase"">
//        <i class=""t-icon t-icon-add"" />
//    </button>
//</div>
//");
    }
    //[Fact(DisplayName = "InputNumber - 测试")]
    //public void Test()
    //{
    //    GetInputNumer<int>().Should().HaveTag("div");
    //}
}
