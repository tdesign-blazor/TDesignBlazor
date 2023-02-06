using AngleSharp.Dom;
using ComponentBuilder;

namespace TDesign.Test.Components.Input
{
    public class SwitchTest : TestBase<TSwitch>
    {
        [Fact(DisplayName = "Switch - 渲染并验证 css")]
        public void Test_Render()
        {
            var value = false;
            var component = GetComponent(op =>
            {
                op.Bind(item => item.Value, value, (switchValue) => { });
            });

            component.Should().HaveClass("t-switch").And.HaveClass("t-size-m");

            component.Find(".t-switch__handle").Should().HaveTag("span");
            component.Find(".t-switch__content").Should().HaveTag("div");

            //.MarkupMatches("<div class=\"t-switch t-size-m\" placeholder=\"\"><span class=\"t-switch__handle\"></span><div class=\"t-switch__content t-size-m\"></div></div>");
        }

        [Fact(DisplayName = "Switch - Value 参数")]
        public void Test_Value_Parameter()
        {
            var value = true;
            GetComponent(op =>
            {
                op.Bind(item => item.Value, value, (switchValue) => { }
                );
            }).Should().HaveClass("t-switch").And.HaveClass("t-is-checked");
        }

        [Fact(DisplayName = "Switch - Disabled 参数")]
        public void Test_Disabled_Parameter()
        {
            var value = false;
            GetComponent(op =>
            {
                op.Bind(item => item.Value, value, (switchValue) => { });
                op.Add(item => item.Disabled, true);
            }).Should().HaveClass("t-is-disabled").And.HaveChildMarkup(builder => builder.CreateElement(0, "span", attributes: new { @class = "t-switch__handle t-is-disabled" }));
        }

        [Fact(DisplayName = "Switch - Loading 参数")]
        public void Test_Loading_Parameter()
        {
            var value = false;
            GetComponent(op =>
            {
                op.Bind(item => item.Value, value, (switchValue) => { });
                op.Add(item => item.Loading, true);
            }).Should()
            .HaveClass("t-is-loading")
            .And
            .HaveChildMarkup(
                builder =>
                    builder.CreateElement(0, "span", child => child.CreateElement(1, "i", attributes: new { @class = "t-icon t-icon-loading" }),
                attributes: new { @class = "t-switch__handle t-is-loading" })
             );
        }

        [Fact(DisplayName = "Switch - Size 参数")]
        public void Test_Size_Parameter()
        {
            var value = false;
            GetComponent(op =>
            {
                op.Bind(item => item.Value, value, (switchValue) => { });
                op.Add(item => item.Size, Size.Small);
            }).Should().HaveClass("t-size-s");

            GetComponent(op =>
            {
                op.Bind(item => item.Value, value, (switchValue) => { });
                op.Add(item => item.Size, Size.Medium);
            }).Should().HaveClass("t-size-m");

            GetComponent(op =>
            {
                op.Bind(item => item.Value, value, (switchValue) => { });
                op.Add(item => item.Size, Size.Large);
            }).Should().HaveClass("t-size-l");
        }

        //[Fact(DisplayName = "Switch - TrueContent 参数")]
        //public void Test_TrueContent_Parameter()
        //{
        //    var value = true;
        //    GetComponent(op =>
        //    {
        //        op.Bind(item => item.Value, value, (switchValue) => { });
        //        op.Add(item => item.TrueContent, builder => { builder.AddContent(0, "OPEN"); });
        //    }).MarkupMatches("<div class=\"t-switch t-size-m t-is-checked\" placeholder=\"\" blazor:onclick=\"1\"><span class=\"t-switch__handle\"></span><div class=\"t-switch__content t-size-m\">OPEN</div></div>");
        //}

        //[Fact(DisplayName = "Switch - FalseContent 参数")]
        //public void Test_FalseContent_Parameter()
        //{
        //    var value = false;
        //    GetComponent(op =>
        //    {
        //        op.Bind(item => item.Value, value, (switchValue) => { });
        //        op.Add(item => item.FalseContent, builder => { builder.AddContent(0, "CLOSE"); });
        //    }).MarkupMatches("<div class=\"t-switch t-size-m\" placeholder=\"\" blazor:onclick=\"1\"><span class=\"t-switch__handle\"></span><div class=\"t-switch__content t-size-m\">CLOSE</div></div>");
        //}
    }
}
