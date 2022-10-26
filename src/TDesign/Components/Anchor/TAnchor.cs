using Microsoft.AspNetCore.Components.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign
{
    [HtmlTag("div")]
    [ParentComponent]
    [CssClass("t-anchor")]
    public class TAnchor : BlazorComponentBase, IHasChildContent, IHasOnSwitch
    {
        [Parameter]public RenderFragment? ChildContent { get; set; }
        public EventCallback<int?> OnSwitch { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int? SwitchIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {

            builder.CreateElement(sequence + 1, "div", line =>
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

            builder.AddContent(sequence + 2, ChildContent);

        }
    }
}
