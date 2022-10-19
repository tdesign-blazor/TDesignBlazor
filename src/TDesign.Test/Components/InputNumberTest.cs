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
    [Fact(DisplayName = "InputNumber - Size")]
    public void Test_Render_Parameter_Size()
    {
        int value = 1;
        GetInputNumer<int>(x =>
        {
            x.Add(c => c.Value, value);
            x.Add(p => p.ValueChanged, (s) => value = s);
            x.Add(p => p.ValueExpression, () => value);
            x.Add(p => p.Size,Size.Small);
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

    [Fact(DisplayName = "InputNumber - Placeholder")]
    public void Test_Render_Parameter_Placeholder()
    {
        //int value = 1;
        //GetInputNumer<int>(x =>
        //{
        //    x.Add(c => c.Value, value);
        //    x.Add(p => p.ValueChanged, (s) => value = s);
        //    x.Add(p => p.ValueExpression, () => value);
        //    //x.Add(p => p.Placeholder,"我是占位符");
        //}).Should().HaveClass(Size.Small.GetCssClass());

    }

    [Fact(DisplayName = "InputNumber - Align")]
    public void Test_Render_Parameter_Align()
    {
    }

    [Fact(DisplayName = "InputNumber - Step")]
    public void Test_Render_Parameter_Step()
    {
    }

    [Fact(DisplayName = "InputNumber - Max")]
    public void Test_Render_Parameter_Max()
    {
    }

    [Fact(DisplayName = "InputNumber - Theme")]
    public void Test_Render_Parameter_Theme()
    {
    }

    [Fact(DisplayName = "InputNumber - AutoWidth")]
    public void Test_Render_Parameter_AutoWidth()
    {
    }

    [Fact(DisplayName = "InputNumber - Status")]
    public void Test_Render_Parameter_Status()
    {
    }

    [Fact(DisplayName = "InputNumber - SuffixText")]
    public void Test_Render_Parameter_SuffixText()
    {
    }

    [Fact(DisplayName = "InputNumber - Label")]
    public void Test_Render_Parameter_Label()
    {
    }

    [Fact(DisplayName = "InputNumber - Tips")]
    public void Test_Render_Parameter_Tips()
    {
    }

    [Fact(DisplayName = "InputNumber - Disabled")]
    public void Test_Render_Parameter_Disabled()
    {
    }
}
