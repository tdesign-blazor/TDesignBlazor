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
        /// <summary>
        /// 进度条状态
        /// </summary>
        [Parameter][CssClass("t-progress--status--")] public Status? Status { get; set; } = TDesignBlazor.Status.None;
        [Parameter][CssClass("t-progress--")] public ProgressThemeType? Theme { get; set; } = ProgressThemeType.Line;
    }
}
