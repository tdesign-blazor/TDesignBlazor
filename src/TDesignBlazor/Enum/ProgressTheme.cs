using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesignBlazor
{
    [CssClass("t-progress--")]
    public enum ProgressTheme
    {
        [CssClass("line")]
        line,
        [CssClass("plump")]
        plump,
        [CssClass("circle")]
        circle,
    }
}
