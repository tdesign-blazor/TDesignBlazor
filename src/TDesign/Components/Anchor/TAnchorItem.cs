using ComponentBuilder;

using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

using System;
using System.Security.Cryptography.X509Certificates;

using static System.Net.WebRequestMethods;

namespace TDesign
{
    /// <summary>
    /// 
    /// </summary>
    [ChildComponent(typeof(TAnchor))]
    [HtmlTag("div")]
    [CssClass("t-anchor__item")]
    public class TAnchorItem : BlazorComponentBase, IHasChildContent, IHasActive, IHasDisabled
    {
        private string? _href;
        /// <summary>
        /// 用于自动化获取父组件。
        /// </summary>
        [CascadingParameter] public TAnchor? CascadingAnchor { get; set; }
        /// <summary>
        /// 锚点
        /// </summary>
        [Parameter]
        public string? Href
        {
            get
            {
                return _href;
            }
            set
            {


                var anchors = navigationManager?.Uri.Split('#') ?? Array.Empty<string>();
                if (anchors.Length > 1)
                {
                    _href = navigationManager?.Uri?.Replace($"#{anchors[^1]}", value) ?? "";
                }
                else
                {
                    _href = navigationManager?.Uri + value;
                }

            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        [Parameter] public string? Title { get; set; }
        /// <summary>
        /// 锚点文字
        /// </summary>
        [Parameter] public AnchorItemTarget? Target { get; set; } = AnchorItemTarget.Self;
        /// <summary>
        /// 
        /// </summary>
        public RenderFragment? ChildContent { get; set; }
        internal int Index { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        [Parameter] public bool Active { get; set; }
        /// <summary>
        /// 
        /// </summary>

        [Inject] public new IJSRuntime? JS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Parameter][HtmlEvent("onclick")] public EventCallback<MouseEventArgs?> OnClick { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Disabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Inject] NavigationManager? navigationManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int OffsetTop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OffsetHeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
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
                                   for (int i = 0; i < CascadingAnchor?.ChildComponents.Count; i++)
                                   {
                                       if (CascadingAnchor.ChildComponents[i] is TAnchorItem item)
                                       {
                                           if (Index == i)
                                               item.Active = true;
                                           else
                                               item.Active = false;


                                           var itemId = item.Href?.Split("#")[1];
                                           item.OffsetTop = await JS!.InvokeAsync<int>("getOffsetTop", itemId);
                                           item.OffsetHeight = await JS!.InvokeAsync<int>("getOffsetHeight", itemId);

                                           await item.Refresh();
                                       }


                                   }
                                   await JS!.InvokeVoidAsync("hash", Href?.Split("#")[1]);

                                   await this.Refresh();
                                   CascadingAnchor.SwitchIndex = Index;
                                   await CascadingAnchor.Refresh();
                               }),
                               @class = "t-anchor__item-link"
                           });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            if (Active)
            {
                builder.Append("t-is-active");
            }
            else
            {
                builder.Remove("t-is-active");
            }
            base.BuildCssClass(builder);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Index = CascadingAnchor!.ChildComponents.Count - 1;

        }

        /// <summary>
        /// 
        /// </summary>
        public void SetActive(bool active)
        {
            Active = active;
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
