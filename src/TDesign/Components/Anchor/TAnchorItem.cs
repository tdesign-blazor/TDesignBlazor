using ComponentBuilder;

using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

using static System.Net.WebRequestMethods;

namespace TDesign
{
    [ChildComponent(typeof(TAnchor))]
    [HtmlTag("div")]
    [CssClass("t-anchor__item")]
    public class TAnchorItem : BlazorComponentBase, IHasChildContent
    {
        /// <summary>
        /// 用于自动化获取父组件。
        /// </summary>
        [CascadingParameter] public TAnchor CascadingAnchor { get; set; }
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
        [Parameter] public AnchorItemTarget? Target { get; set; } = AnchorItemTarget.Self;
        public RenderFragment? ChildContent { get; set; }
        internal int Index { get; private set; }
        [Parameter][CssClass("t-is-active")] public bool? Active { get; set; }

        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            if (CascadingAnchor.SwitchIndex.HasValue && CascadingAnchor.SwitchIndex.Value == Index)
            {
                builder.CreateComponent<TLink>(sequence + 1, Title,
                               attributes: new
                               {
                                   Href,
                                   Title,
                                   Target = Target?.GetHtmlAttribute(),
                               });
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Index = CascadingAnchor.ChildComponents.Count - 1;
        }
    }

    /// <summary>
    /// TAnchorItem 目标
    /// </summary>
    public enum AnchorItemTarget
    {
        /// <summary>
        /// 在当前页面加载
        /// </summary>
        [HtmlAttribute("_self")] Self,
        /// <summary>
        /// 在新窗口打开
        /// </summary>
        [HtmlAttribute("_blank")] Blank,
        /// <summary>
        /// <c></c>
        /// <see href="https://developer.mozilla.org/zh-CN/docs/Web/HTML/Element/a#attr-target"/>
        /// </summary>
        [HtmlAttribute("_parent")] Parent,
        /// <summary>
        /// 
        /// 
        /// </summary>
        [HtmlAttribute("_top")] Top
    }
}
