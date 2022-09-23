using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign
{
    [CssClass("t-progress--")]
    public enum ProgressTheme
    {
        /// <summary>
        /// 横线
        /// </summary>
        [CssClass("thin")]
        Line,
        /// <summary>
        /// 行内
        /// </summary>
        [CssClass("plump")]
        Plump,
        /// <summary>
        /// 圆环
        /// </summary>
        [CssClass("circle")]
        Circle,
    }
}
