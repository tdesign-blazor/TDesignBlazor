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
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }
}
