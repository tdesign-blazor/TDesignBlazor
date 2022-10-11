using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesignBlazor.Components
{
    [HtmlTag("div")]
    [CssClass("t-input-number  t-size-m t-input-number--row")]
    public class InputNumber : BlazorComponentBase
    {

        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            builder.CreateComponent<Button>(sequence + 1);
            builder.CreateComponent<Input<int>>(sequence + 2);
            builder.CreateComponent<Button>(sequence + 3);
        }
    }
}
