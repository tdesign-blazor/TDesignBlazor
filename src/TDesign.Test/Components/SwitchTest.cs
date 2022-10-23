using ComponentBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign.Test.Components
{
    public class SwitchTest : TestBase<TSwitch>
    {
        [Fact(DisplayName = "Switch - 渲染并验证 css")]
        public void Test_Render()
        {
            var value = false;
            GetComponent(op =>
            {
                op.Bind(item => item.Value, value, (switchValue) => { }
                );
            }).Should().HaveClass("t-switch")
            .And
            .HaveChildMarkup(
                builder =>
                    {
                        builder.CreateElement(0, "span", attributes: new { @class = "t-switch__handle" });
                        builder.CreateElement(1, "div", attributes: new { @class = "t-switch__content t-size-m" });
                    }
             );
        }

        [Fact(DisplayName = "Switch - Value 参数")]
        public void Test_Value_Parameter()
        {
            var value = true;
            GetComponent(op =>
            {
                op.Bind(item => item.Value, value, (switchValue) => { }
                );
            }).Should()
            .HaveClass("t-switch")
            .And
            .HaveClass("t-is-checked");
        }

        [Fact(DisplayName = "Switch - Disabled 参数")]
        public void Test_Disabled_Parameter()
        {
            var value = false;
            GetComponent(op =>
            {
                op.Bind(item => item.Value, value, (switchValue) => { });
                op.Add(item => item.Disabled, true);
            }).Should()
            .HaveClass("t-is-disabled")
            .And
            .HaveChildMarkup(builder => builder.CreateElement(0, "span", attributes: new { @class = "t-switch__handle t-is-disabled" }));
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
    }
}
