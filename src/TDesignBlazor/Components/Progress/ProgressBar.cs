using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDesignBlazor.Components.Progress;

namespace TDesignBlazor
{
    [HtmlTag("div")]
    [CssClass("t-progress__bar")]
    internal class ProgressBar : BlazorComponentBase, IHasChildContent
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public int? Percentage { get; set; } = 0;
        //protected override void AddContent(RenderTreeBuilder builder, int sequence)
        //{
        //    base.AddContent(builder, sequence);
        //    //builder.CreateComponent<ProgressInner>(sequence+1,attributes:new { @style= $"width:{Percentage.ToSuffix("%")}" });
        //}
    }

}
