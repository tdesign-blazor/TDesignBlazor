using Microsoft.AspNetCore.Components.Rendering;

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
    [HtmlTag("img")]
    public class Image : BlazorComponentBase
    {
        /// <summary>
        /// 图片描述
        /// </summary>
        [Parameter]public string Alt { get; set; }
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
        [Parameter] public FitType Fit { get; set; }
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
        [Parameter] public OverlayTriggerType OverlayTrigger { get; set; }
        /// <summary>
        /// 占位元素
        /// </summary>
        [Parameter] public string Placeholder { get; set; }
        /// <summary>
        /// 定位
        /// </summary>
        [Parameter] public string Position { get; set; }
        /// <summary>
        /// 图片圆角类型
        /// </summary>
        [Parameter] public ShapeType Shape { get; set; }
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
            base.AddContent(builder, sequence);
        }
        protected override void BuildAttributes(IDictionary<string, object> attributes)
        {
            attributes.Add("src", Src);
            //base.BuildAttributes(attributes);
        }
        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            builder.Append("t-image t-image--fit-cover t-image--position-center");

        }
    }

    public enum FitType
    {
        Contain,
        Cover,
        Fill,
        None,
        ScaleDown
    }
    public enum OverlayTriggerType
    {
        Always,
        Hover
    }
    public enum ShapeType
    {
        Circle,
        Round,
        Square
    }
}
