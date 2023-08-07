namespace TDesign;
/// <summary>
/// 用于文字提示的气泡框。
/// </summary>
[CssClass("t-popup t-tooltip")]
public class TTooltip : TPopup
{
    /// <summary>
    /// 设置主题颜色。
    /// </summary>
    [ParameterApiDoc("主题颜色",Value ="Default")]
    [Parameter][CssClass("t-tooltip--")] public Theme Theme { get; set; } = Theme.Default;
}
