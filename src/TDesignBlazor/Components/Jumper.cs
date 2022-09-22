using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesignBlazor.Components
{
    public class Jumper : BlazorComponentBase, IHasChildContent
    {
        public RenderFragment? ChildContent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
