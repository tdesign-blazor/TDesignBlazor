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
        [Parameter] public AnchorItemTarget? Target { get; set; } = AnchorItemTarget.Self;
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
                                       var child = CascadingAnchor.ChildComponents[i];
                                       if (child is TAnchorItem item)
                                       {
                                           item.Active = false;
                                           await item.Refresh();
                                           
                                       }
                                   }
                                   Active = true;
                                   await this.Refresh();
                               })
                           });

        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Index = CascadingAnchor.ChildComponents.Count - 1;

        }

        //public async Task Toggle()
        //{
        //    if (Disabled)
        //    {
        //        return;
        //    }

        //    if (CascadingCollaspe.Mutex)
        //    {
        //        for (int i = 0; i < CascadingCollaspe.ChildComponents.Count; i++)
        //        {
        //            var child = CascadingCollaspe.ChildComponents[i];
        //            if (child is TCollapsePanel panel)
        //            {
        //                panel.Active = false;
        //                await panel.Refresh();
        //            }
        //        }
        //    }
        //    Active = !Active;
        //    await this.Refresh();
        //}
        //onclick = HtmlHelper.CreateCallback(this, Toggle, CascadingCollaspe.TIconToggle)
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
