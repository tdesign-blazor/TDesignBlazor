using Microsoft.AspNetCore.Components.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TDesign
{
    [HtmlTag("div")]
    [CssClass("t-anchor__item")]
    public class TAnchorItem : BlazorComponentBase
    {
        /// <summary>
        /// 锚点
        /// </summary>
        [Parameter] public string? Href { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Parameter] public string? Title { get; set; }
        /// <summary>
        /// 锚点文字
        /// </summary>
        [Parameter] public string? Target { get; set; }

        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            builder.CreateComponent<TLink>(sequence + 1, Title, new { Href, Title, Target });
        }
    }
}
