using ComponentBuilder;

using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesignBlazor
{
    /// <summary>
    /// 图片
    /// </summary>
    [HtmlTag("div")]
    [CssClass("t-image__wrapper")]
    public class Image : BlazorComponentBase
    {
        private IJSRuntime _jS;

        public Image(IJSRuntime js=null)
        {
            _jS = js;
        }
        public Image()
        {

        }
        /// <summary>
        /// 图片描述
        /// </summary>
        [Parameter] public string Alt { get; set; }
        /// <summary>
        /// 禁用状态
        /// </summary>
        [Parameter] public bool Disabled { get; set; } = false;
        /// <summary>
        /// 错误内容
        /// </summary>
        [Parameter] public string Error { get; set; }
        /// <summary>
        /// 填充模式
        /// </summary>
        [Parameter] public FitType Fit { get; set; } = FitType.Fill;//[CssClass("t-image--fit-")]
        /// <summary>
        /// 是否展示为图集样式
        /// </summary>
        [Parameter] public bool Gallery { get; set; } = false;
        /// <summary>
        /// 是否开启图片懒加载
        /// </summary>
        [Parameter] public bool Lazy { get; set; } = false;
        /// <summary>
        /// 加载中状态的图片内容
        /// </summary>
        [Parameter] public string Loading { get; set; }
        /// <summary>
        /// 图片上方的浮层内容
        /// </summary>
        [Parameter] public string OverlayContent { get; set; }
        /// <summary>
        /// 浮层 overlayContent 出现的时机
        /// </summary>
        [Parameter] public OverlayTriggerType? OverlayTrigger { get; set; }
        /// <summary>
        /// 占位元素
        /// </summary>
        [Parameter] public string Placeholder { get; set; }
        /// <summary>
        /// 定位
        /// </summary>
        [Parameter] public string Position { get; set; } = "center"; //[CssClass("t-image--position-")]
        /// <summary>
        /// 图片圆角类型
        /// </summary>
        [Parameter][CssClass("t-image__wrapper--shape-")] public ShapeType Shape { get; set; } = ShapeType.Square;
        /// <summary>
        /// 图片链接
        /// </summary>
        [Parameter] public string Src { get; set; }
        /// <summary>
        /// 加载失败
        /// </summary>
        [Parameter] public Action OnError { get; set; }
        /// <summary>
        /// 加载完成
        /// </summary>
        [Parameter] public Action OnLoad { get; set; }

        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            var s1 = s;
            if (Gallery)
            {
                builder.CreateElement(sequence + 1, "div", attributes: new { @class = "t-image__gallery-shadow" });
            }
            builder.CreateElement(sequence + 2, "img", attributes: new
            {
                src = Src,
                @class = HtmlHelper.CreateCssBuilder()
                                 .Append("t-image")
                                 .Append(Fit.GetCssClass())
                                 .Append("t-image--position-" + Position),
                style = "z-index:3;",
                onmouseenter= s1
            });
            if (Gallery|| OverlayTrigger!=null)
            {
                builder.CreateElement(sequence + 3, "div", x =>
                {
                    x.CreateElement(sequence + 1, "span", span =>
                    {
                        span.CreateElement(sequence + 2, "span", OverlayContent);
                    },
                    attributes: new
                    {
                        @class = "t-tag t-tag--warning t-size-m t-tag--dark t-tag--mark",
                        style = "margin: 8px;border-radius: 3px;background: rgb(236, 242, 254);color: rgb(0, 82, 217);"
                    }, Gallery);

                    x.CreateElement(sequence + 1, "div", "预览", new { style = "background: rgba(0, 0, 0, 0.4); color: rgb(255, 255, 255); height: 100%; display: flex; align-items: center; justify-content: center;" }, OverlayTrigger != null);
                }, attributes: new
                {
                    @class =  HtmlHelper.CreateCssBuilder()
                                        .Append("t-image__overlay-content")
                                        .Append("t-image__overlay-content--hidden", OverlayTrigger != null)
                });
            }
        }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            builder.Append(OverlayTrigger?.GetCssClass());
        }
        void s()
        {
            _jS.InvokeVoidAsync("alert", "1");
            var s = "";
        }
    }
    [CssClass("t-image--fit-")]
    public enum FitType
    {
        [CssClass("contain")]
        Contain,
        [CssClass("cover")]
        Cover,
        [CssClass("fill")]
        Fill,
        [CssClass("none")]
        None,
        [CssClass("scale-down")]
        ScaleDown
    }
    [CssClass("t-image__wrapper--need-hover")]
    public enum OverlayTriggerType
    {
        [CssClass("always")]
        Always,
        [CssClass("thover")]
        Hover
    }
    public enum ShapeType
    {
        [CssClass("circle")]
        Circle,
        [CssClass("round")]
        Round,
        [CssClass("square")]
        Square
    }
}
