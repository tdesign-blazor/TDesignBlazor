using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesignBlazor.Components.Progress
{
    [HtmlTag("div")]
    [CssClass("t-progress__inner")]
    internal class ProgressInner : BlazorComponentBase, IHasChildContent
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }
}
