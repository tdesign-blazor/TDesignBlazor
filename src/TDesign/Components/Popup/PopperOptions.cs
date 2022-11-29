using System.ComponentModel;
using System.Text.Json.Serialization;

using Microsoft.JSInterop;

namespace TDesign;
/// <summary>
/// 表示 popper 的配置项。
/// </summary>
public class PopperOptions
{
    /// <summary>
    /// 弹出层的位置。
    /// </summary>
    [JsonConverter(typeof(EnumDescriptionConverter<PopperPlacement>))]
    [JsonPropertyName("placement")]
    public PopperPlacement Placement { get; set; } = PopperPlacement.Auto;

    ///// <summary>
    ///// 获取或设置修饰符集合。
    ///// </summary>
    //[JsonPropertyName("modifiers")]
    //public PopperModifier[]? Modifiers { get; set; }

    /// <summary>
    /// 获取或设置要使用的定位策略。
    /// </summary>
    [JsonConverter(typeof(EnumDescriptionConverter<PopperStrategy>))]
    [JsonPropertyName("strategy")]
    public PopperStrategy Strategy { get; set; } = PopperStrategy.Absolute;

    [JsonIgnore]
    public Action<PopperState>? OnFirstUpdate { get; set; }

    [JSInvokable("CallOnFirstUpdate")]
    public void CallOnFirstUpdate(PopperState state) => OnFirstUpdate?.Invoke(state);
}

/// <summary>
/// Popup 修饰者。
/// </summary>
public class PopperModifier
{
    public string Name { get; set; }
    public bool Enabled { get; set; }

}

/// <summary>
/// Popup 状态。
/// </summary>
public class PopperState
{
    [JsonConverter(typeof(EnumDescriptionConverter<PopperPlacement>))]
    [JsonPropertyName("placement")]
    public PopperPlacement Placement { get; set; }
}

/// <summary>
/// Popper 的位置。
/// </summary>
public enum PopperPlacement
{
    /// <summary>
    /// 中间自动。
    /// </summary>
    [Description("auto")] Auto,
    /// <summary>
    /// 起始自动。
    /// </summary>
    [Description("auto-start")] AutoStart,
    /// <summary>
    /// 末尾自动。
    /// </summary>
    [Description("auto-end")] AutoEnd,
    /// <summary>
    /// 顶部中间。
    /// </summary>
    [Description("top")] Top,
    /// <summary>
    /// 顶部起始。
    /// </summary>
    [Description("top-start")] TopStart,
    /// <summary>
    /// 顶部末尾。
    /// </summary>
    [Description("top-end")] TopEnd,
    /// <summary>
    /// 底部中间。
    /// </summary>
    [Description("bottom")] Bottom,
    /// <summary>
    /// 底部起始。
    /// </summary>
    [Description("bottom-start")] BottomStart,
    /// <summary>
    /// 底部末尾。
    /// </summary>
    [Description("bottom-end")] BottomEnd,
    /// <summary>
    /// 右边中间。
    /// </summary>
    [Description("right")] Right,
    /// <summary>
    /// 右边起始。
    /// </summary>
    [Description("right-start")] RightStart,
    /// <summary>
    /// 右边末尾。
    /// </summary>
    [Description("right-end")] RightEnd,
    /// <summary>
    /// 左边中间。
    /// </summary>
    [Description("left")] Left,
    /// <summary>
    /// 左边起始。
    /// </summary>
    [Description("left-start")] LeftStart,
    /// <summary>
    /// 右边起始。
    /// </summary>
    [Description("left-end")] LeftEnd
}
/// <summary>
/// Popper 策略。
/// </summary>
public enum PopperStrategy
{
    /// <summary>
    /// 绝对定位。
    /// </summary>
    Absolute,
    /// <summary>
    /// 固定定位。
    /// </summary>
    Fixed
}