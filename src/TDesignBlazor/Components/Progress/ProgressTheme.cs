using ComponentBuilder;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesignBlazor
{
    [HtmlTag("div")]
    internal class ProgressTheme : BlazorComponentBase, IHasChildContent
    {
        //[Parameter] public Status? Status { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
        //[Parameter] public int? Percentage { get; set; } = 0;
        //protected override void AddContent(RenderTreeBuilder builder, int sequence)
        //{
        //    base.AddContent(builder, sequence);
        //    //builder.CreateComponent<ProgressBar>(sequence + 1, attributes: new { Percentage = Percentage });
        //    //builder.CreateComponent<ProgressInfo>(sequence + 2, attributes: new { Percentage = Percentage, Status= Status });
        //}
    }
}
