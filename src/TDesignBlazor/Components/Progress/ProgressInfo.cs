using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign
{
    //[ChildComponent(typeof(ProgressTheme<>))]
    [HtmlTag("div")]
    [CssClass("t-progress__info")]
    internal class ProgressInfo : BlazorComponentBase, IHasChildContent
    {
        public RenderFragment? ChildContent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
