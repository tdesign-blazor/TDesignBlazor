using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign
{
    [ChildComponent(typeof(Progress<>))]
    [HtmlTag("div")]
    //[CascadingTypeParameter(nameof(TItem))]
    internal class ProgressTheme : BlazorComponentBase, IHasChildContent
    {
        public RenderFragment? ChildContent { get; set; }
    }
}
