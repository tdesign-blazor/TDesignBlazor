using ComponentBuilder;

using Microsoft.AspNetCore.Components.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TDesign
{
    [HtmlTag("div")]
    [ParentComponent]
    [CssClass("t-anchor")]
    public class TAnchor : BlazorComponentBase, IHasChildContent, IHasOnSwitch
    {
        public TAnchor()
        {
            SwitchIndex = 0;
        }
        [Parameter] public RenderFragment? ChildContent { get; set; }
        public EventCallback<int?> OnSwitch { get; set; }
        public int? SwitchIndex { get; set; }

        string GetActiveCss(int index) => index == SwitchIndex ? "t-is-active" : string.Empty;

        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            BuildLine(builder, sequence+1);
            builder.AddContent(sequence + 2, ChildContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="builder"></param>
        private void BuildLine(RenderTreeBuilder builder, int sequence)
        {
            builder.CreateElement(sequence+1, "div", line =>
            {

                line.CreateElement(sequence + 2, "div", wrapper =>
                {

                    wrapper.CreateElement(sequence + 3, "div", attributes: new
                    {

                        @class = "t-anchor__line-cursor",
                       

                    });

                }, new { @class = "t-anchor__line-cursor-wrapper", style = $"top: {SwitchIndex*24}px; height: 24px; opacity: 1;" });

            }, new { @class = "t-anchor__line" });
        }
    }
}
