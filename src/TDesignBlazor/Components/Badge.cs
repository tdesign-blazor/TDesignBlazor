using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor.Components;
/// <summary>
/// 徽章组件，出现在图标或文字右上角的徽标标识。
/// </summary>
[CssClass("t-badge")]
public class Badge : TDesignComponentBase, IHasChildContent
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 徽章显示的文本。
    /// </summary>
    [Parameter] public string? Text { get; set; }

    /// <summary>
    /// 小尺寸。
    /// </summary>
    [Parameter] public bool Small { get; set; }
    /// <summary>
    /// 徽章形状，默认是 <see cref="BadgeShape.Circle"/> 。
    /// </summary>
    [Parameter] public BadgeShape Shape { get; set; } = BadgeShape.Circle;
    /// <summary>
    /// 徽章的颜色，支持十六进制的字符串。
    /// </summary>
    [Parameter] public string? Color { get; set; }
    /// <summary>
    /// 距离上面的偏移量，单位 px。
    /// </summary>
    [Parameter] public int? Top { get; set; }
    /// <summary>
    /// 距离右侧的偏移量，单位 px。
    /// </summary>
    [Parameter] public int? Right { get; set; }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", ChildContent, new { @class = "badge-black" });
        builder.CreateElement(sequence + 1, "div", Text, new
        {
            @class = HtmlHelper.CreateCssBuilder()
            .Append($"t-badge--{Shape.GetCssClass()}")
            .Append("t-size-s", Small),
            style = HtmlHelper.CreateStyleBuilder()
            .Append($"background-color:{Color}", !string.IsNullOrEmpty(Color))
            .Append($"top:{Top}px", Top.HasValue)
            .Append($"right:{Right}px", Right.HasValue)
        });
    }
}

/// <summary>
/// 徽标形状。
/// </summary>
public enum BadgeShape
{
    /// <summary>
    /// 圆形。
    /// </summary>
    Circle,
    /// <summary>
    /// 椭圆。
    /// </summary>
    Round,
    /// <summary>
    /// 点。
    /// </summary>
    Dot,
}