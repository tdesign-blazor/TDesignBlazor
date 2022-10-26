using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace TDesign
{
    [ChildComponent(typeof(TAnchor))]
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
        [Parameter] public AnchorItemTarget? Target { get; set; } = AnchorItemTarget.Self;
        [Parameter][HtmlEvent("onclick")] public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter][HtmlEvent("onchange")] public EventCallback<MouseEventArgs> OnChange { get; set; }

        [Inject] protected IJSRuntime Js { get; set; }

        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            builder.CreateComponent<TLink>(sequence + 1, Title,
                new
                {
                    Href,
                    Title,
                    Target = Target.GetHtmlAttribute(),
                    onclick = HtmlHelper.CreateCallback<MouseEventArgs>(this, async x =>
                    {
                       await Js.ScrollToHash("#content-2");
                       //await Js.InvokeVoidAsync("alert","1");
                    })
                });
        }
    }



    /// <summary>
    /// TAnchorItem 目标
    /// </summary>
    public enum AnchorItemTarget
    {
        [HtmlAttribute("_self")]
        Self,
        [HtmlAttribute("_blank")]
        Blank,
        [HtmlAttribute("_parent")]
        Parent,
        [HtmlAttribute("_top")]
        Top
    }
}
