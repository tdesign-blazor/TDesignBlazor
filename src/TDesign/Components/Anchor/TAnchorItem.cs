using ComponentBuilder;

using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Security.Cryptography.X509Certificates;
using static System.Net.WebRequestMethods;

namespace TDesign
{
    [ChildComponent(typeof(TAnchor))]
    [HtmlTag("div")]
    [CssClass("t-anchor__item")]
    public class TAnchorItem : BlazorComponentBase, IHasChildContent, IHasActive, IHasDisabled
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
        [Parameter] public AnchorItemTarget? Target { get; set; }
        public RenderFragment? ChildContent { get; set; }
        internal int Index { get; private set; }
        [Parameter][CssClass("t-is-active")] public bool Active { get; set; }

        [Inject] public IJSRuntime? JS { get; set; }
        [Parameter][HtmlEvent("onclick")] public EventCallback<MouseEventArgs?> OnClick { get; set; }
        public bool Disabled { get; set; }

        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            builder.CreateComponent<TLink>(sequence + 1, Title,
                           attributes: new
                           {
                               Href,
                               Title,
                               Target = Target?.GetHtmlAttribute(),
                               onclick = HtmlHelper.CreateCallback<MouseEventArgs>(this, async x =>
                               {
                                   for (int i = 0; i < CascadingAnchor.ChildComponents.Count; i++)
                                   {
                                       if (CascadingAnchor.ChildComponents[i] is TAnchorItem item)
                                       {
                                           if (Index == i)
                                               item.Active = true;
                                           else
                                               item.Active = false;

                                           await item.Refresh();
                                       }
                                   }
                                   await this.Refresh();
                                   CascadingAnchor.SwitchIndex=Index;
                                   await CascadingAnchor.Refresh();
                                   await JS.InvokeVoidAsync("hash", Href);
                               })
                           });

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
