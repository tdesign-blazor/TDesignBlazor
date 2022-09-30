using Microsoft.AspNetCore.Components.Rendering;

using System;

namespace TDesignBlazor
{
    /// <summary>
    /// 图片
    /// </summary>
    [HtmlTag("div")]
    [CssClass("t-image__wrapper")]
    public class Image : BlazorComponentBase
    {
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
        [Parameter] public RenderFragment? Error { get; set; }

        /// <summary>
        /// 填充模式
        /// </summary>
        [Parameter] public FitType Fit { get; set; } = FitType.Fill;

        /// <summary>
        /// 是否展示为图集样式
        /// </summary>
        [Parameter] public bool Gallery { get; set; }

        /// <summary>
        /// 是否开启图片懒加载
        /// </summary>
        [Parameter] public bool Lazy { get; set; } = false;

        /// <summary>
        /// 加载中状态的图片内容
        /// </summary>
        [Parameter] public string? Loading { get; set; }

        /// <summary>
        /// 图片上方的浮层内容
        /// </summary>
        [Parameter] public string? OverlayContent { get; set; }

        /// <summary>
        /// 浮层 overlayContent 出现的时机
        /// </summary>
        [Parameter] public OverlayTriggerType? OverlayTrigger { get; set; }

        /// <summary>
        /// 占位元素
        /// </summary>
        [Parameter] public string? Placeholder { get; set; }

        /// <summary>
        /// 定位
        /// </summary>
        [Parameter] public Position Position { get; set; } = TDesignBlazor.Position.Center;

        /// <summary>
        /// 图片圆角类型
        /// </summary>
        [Parameter][CssClass("t-image__wrapper--shape-")] public ShapeType Shape { get; set; } = ShapeType.Square;

        /// <summary>
        /// 图片链接
        /// </summary>
        [Parameter] public string? Src { get; set; }

        /// <summary>
        /// 加载失败
        /// </summary>
        [Parameter] public Action? OnError { get; set; }

        /// <summary>
        /// 加载完成
        /// </summary>
        [Parameter] public Action? OnLoad { get; set; }

        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            builder.CreateElement(sequence + 1, "div", attributes: new { @class = "t-image__gallery-shadow" }, condition: Gallery);

            builder.CreateElement(sequence + 2, "img", attributes: new
            {
                src = Src,
                @class = HtmlHelper.CreateCssBuilder()
                                 .Append("t-image")
                                 .Append(Fit.GetCssClass())
                                 .Append("t-image--position-" + Position.GetCssClass()),
                style = "z-index:3;",
            });

            builder.CreateElement(sequence + 3, "div", x =>
            {
                x.CreateComponent<Tag>(sequence + 1, tag =>
                {

                    tag.CreateElement(sequence + 2, "span", OverlayContent);
                }, attributes: new
                {
                    style = "margin: 8px;border-radius: 3px;background: rgb(236, 242, 254);color: rgb(0, 82, 217);"
                });

            }, attributes: new
            {
                @class = HtmlHelper.CreateCssBuilder()
                                    .Append("t-image__overlay-content")
                                    .Append("t-image__overlay-content--hidden", OverlayTrigger != null)
            }, Gallery || OverlayTrigger != null);
        }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            builder.Append(OverlayTrigger?.GetCssClass());
        }
    }

    /// <summary>
    /// 填充模式
    /// </summary>
    [CssClass("t-image--fit-")]
    public enum FitType
    {
        /// <summary>
        /// 保持原有尺寸
        /// </summary>
        [CssClass("none")] None,

        /// <summary>
        /// 原比例缩放，全部可见
        /// </summary>
        [CssClass("contain")] Contain,

        /// <summary>
        /// 原比例缩放，部分可见
        /// </summary>
        [CssClass("cover")] Cover,

        /// <summary>
        /// 填充
        /// </summary>
        [CssClass("fill")] Fill,

        /// <summary>
        /// 保持原有尺寸比例,如果容器尺寸大于图片内容尺寸，保持图片的原有尺寸，不会放大失真；容器尺寸小于图片内容尺寸，用法跟contain一样。
        /// </summary>
        [CssClass("scale-down")] ScaleDown
    }

    /// <summary>
    /// 浮层出现时机类型
    /// </summary>
    [CssClass("t-image__wrapper--need-hover")]
    public enum OverlayTriggerType
    {
        /// <summary>
        /// 总是出现
        /// </summary>
        [CssClass("always")] Always,

        /// <summary>
        /// 浮动
        /// </summary>
        [CssClass("hover")] Hover
    }

    /// <summary>
    /// 圆角类型
    /// </summary>
    public enum ShapeType
    {
        /// <summary>
        /// 圆形
        /// </summary>
        [CssClass("circle")] Circle,

        /// <summary>
        /// 圆角
        /// </summary>
        [CssClass("round")] Round,

        /// <summary>
        /// 正方形
        /// </summary>
        [CssClass("square")] Square
    }
}
