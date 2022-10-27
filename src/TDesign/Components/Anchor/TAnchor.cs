using ComponentBuilder;

using Microsoft.AspNetCore.Components.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
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
            var s = this;
            BuildLine(builder, sequence);
            builder.AddContent(sequence + 2, ChildContent);

            for (int i = 0; i < ChildComponents.Count; i++)
            {
                var anchorItem = (TAnchorItem)ChildComponents[i];
                if (i == 0)
                {
                    anchorItem.Active = true;
                }

            }
            //builder.AddContent(sequence + 2, BuildItem);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="builder"></param>
        private void BuildLine(RenderTreeBuilder builder, int sequence)
        {
            builder.CreateElement(++sequence, "div", line =>
            {

                line.CreateElement(sequence + 2, "div", wrapper =>
                {

                    wrapper.CreateElement(sequence + 3, "div", attributes: new
                    {

                        @class = "t-anchor__line-cursor",
                        style = "top: 24px; height: 24px; opacity: 1;"

                    });

                }, new { @class = "t-anchor__line-cursor-wrapper" });

            }, new { @class = "t-anchor__line" });
        }

        private void BuildItem(RenderTreeBuilder builder, int sequence)
        {
            for (int i = 0; i < ChildComponents.Count; i++)
            {
                var tabItem = (TAnchorItem)ChildComponents[i];
                var index = i;
                builder.CreateComponent<TAnchorItem>(0, tabItem.ChildContent, attributes: new
                {
                    @class = GetActiveCss(index),
                });
            }
        }
    }
}
