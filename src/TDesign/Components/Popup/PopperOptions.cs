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

public class PopperModifier
{
    public string Name { get; set; }
    public bool Enabled { get; set; }

}

public class PopperState
{
    [JsonConverter(typeof(EnumDescriptionConverter<PopperPlacement>))]
    [JsonPropertyName("placement")]
    public PopperPlacement Placement { get; set; }
}

public enum PopperPlacement
{
    [Description("auto")] Auto,
    [Description("auto-start")] AutoStart,
    [Description("auto-end")] AutoEnd,
    [Description("top")] Top,
    [Description("top-start")] TopStart,
    [Description("top-end")] TopEnd,
    [Description("bottom")] Bottom,
    [Description("bottom-start")] BottomStart,
    [Description("bottom-end")] BottomEnd,
    [Description("right")] Right,
    [Description("right-start")] RightStart,
    [Description("right-end")] RightEnd,
    [Description("left")] Left,
    [Description("left-start")] LeftStart,
    [Description("left-end")] LeftEnd
}

public enum PopperStrategy
{
    Absolute,
    Fixed
}