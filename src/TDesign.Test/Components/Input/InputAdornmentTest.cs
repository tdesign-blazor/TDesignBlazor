using ComponentBuilder;
using ComponentBuilder.FluentRenderTree;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TDesign.Test.Components.Input
{
    public class InputAdornmentTest : TestBase<TInputAdornment>
    {
        [Fact(DisplayName = "InputAdornment - 渲染并验证 css")]
        public void Test_Render()
        {
            RenderComponent().Should().HaveClass("t-input-adornment");
        }

        [Fact(DisplayName = "InputAdornment - Prepend 参数")]
        public void Test_Prepend_Parameter()
        {
            RenderComponent(m => m.Add(p => p.Prepend, "http"))
                .MarkupMatches(builder =>
                {
                    builder.Div("t-input-adornment").Content(prepend =>
                    {
                        prepend.Span("t-input-adornment__prepend").Content(text =>
                        {
                            text.Span("t-input-adornment__text").Content("http").Close();
                        }).Close();
                    })
                    .Close();
                });
        }

        [Fact(DisplayName = "InputAdornment - PrependContent 参数")]
        public void Test_PrependContent_Parameter()
        {
            RenderComponent(m => m.Add(p => p.PrependContent, builder => builder.AddContent(0, "html content")))
                .MarkupMatches(builder =>
                {
                    builder.Div("t-input-adornment").Content(prepend =>
                    {
                        prepend.Span("t-input-adornment__prepend").Content(text =>
                        {
                            text.Span("t-input-adornment__text").Content("html content").Close();
                        }).Close();
                    })
                    .Close();
                });
        }
        [Fact(DisplayName = "InputAdornment - Append 参数")]
        public void Test_Append_Parameter()
        {
            RenderComponent(m => m.Add(p => p.Append, "http"))
                .MarkupMatches(builder =>
                {
                    builder.Div("t-input-adornment").Content(prepend =>
                    {
                        prepend.Span("t-input-adornment__append").Content(text =>
                        {
                            text.Span("t-input-adornment__text").Content("http").Close();
                        }).Close();
                    })
                    .Close();
                });
        }

        [Fact(DisplayName = "InputAdornment - AppendContent 参数")]
        public void Test_AppendContent_Parameter()
        {
            RenderComponent(m => m.Add(p => p.AppendContent, builder => builder.AddContent(0, "html content")))
                .MarkupMatches(builder =>
                {
                    builder.Div("t-input-adornment").Content(prepend =>
                    {
                        prepend.Span("t-input-adornment__append").Content(text =>
                        {
                            text.Span("t-input-adornment__text").Content("html content").Close();
                        }).Close();
                    })
                    .Close();
                });
        }

    }
}
