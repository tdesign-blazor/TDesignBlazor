using Microsoft.AspNetCore.Components.Rendering;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesignBlazor
{
    public class Jumper : BlazorComponentBase, IHasChildContent
    {
        public RenderFragment? ChildContent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        /// <summary>
        /// 按钮禁用配置
        /// </summary>
        [Parameter] public OneOf<bool, JumperDisabledConfig>? Disabled { get; set; }
        /// <summary>
        /// 按钮方向
        /// </summary>
        [Parameter]
        public JumperLayout? Layout { get; set; } = JumperLayout.Horizontal;
        /// <summary>
        /// 是否展示当前按钮。
        /// </summary>
        [Parameter] public bool? ShowCurrent { get; set; }
        /// <summary>
        /// 按钮尺寸。
        /// </summary>
        [Parameter][CssClass("t-size-")] public Size? Size { get; set; } = TDesignBlazor.Size.Small;
        /// <summary>
        /// 提示文案配置，值为 true 显示默认文案；值为 false 不显示提示文案；值类型为对象则单独配置文案内容
        /// </summary>
        [Parameter] public OneOf<bool, JumperTipsConfig> Tips { get; set; }
        /// <summary>
        /// 按钮形式。
        /// </summary>
        [Parameter] public JumperVariant? Variant { get; set; }


        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            JumperTipsConfig tipsConfig = GetTips();

            List<JumperConfig> jumperConfigs = new List<JumperConfig>()
            {
                new (){Icon=Layout==JumperLayout.Vertical?"t-icon-chevron-up": "t-icon-chevron-left",ButtonClass="t-jumper__prev",Title=tipsConfig.Prev},
                new (){Icon="t-icon-round" ,ButtonClass="t-jumper__current",Title=tipsConfig.Current},
                new (){Icon=Layout==JumperLayout.Vertical?"t-icon-chevron-down":"t-icon-chevron-right",ButtonClass="t-jumper__next", Title = tipsConfig.Next},
            };

            builder.CreateElement(sequence + 1, "div", a =>
            {
                var i = 0;
                foreach (var item in jumperConfigs)
                {

                    a.CreateElement(sequence + 2 + i, "button", b =>
                    {
                        b.CreateComponent<Icon>(sequence + 3 + i, attributes: new { Class = $"t-icon {item.Icon}" });
                    }, new { type = "button", @class = $"t-button {Size.GetCssClass()} t-button--variant-text t-button--theme-default t-button--shape-square {item.ButtonClass}", title = $"{item.Title}" });
                    i++;
                }

            }, new { @class = "t-jumper" });
        }

        private JumperTipsConfig GetTips()
        {

            return Tips.Match<JumperTipsConfig>(
                a =>
                {
                    if (a)
                    {
                        return new() { Prev = "上一个", Current = "下一个", Next = "当前" };
                    }
                    else
                    {
                        return new() { Prev = "", Current = "", Next = "" };
                    }
                },
                b => b);
        }
    }


}

/// <summary>
/// 按钮禁用配置
/// </summary>
public class JumperDisabledConfig
{
    /// <summary>
    /// '上一个'按钮是否禁用
    /// </summary>
    public bool? Prev { get; set; }
    /// <summary>
    /// '当前'按钮是否禁用
    /// </summary>
    public bool? Current { get; set; }
    /// <summary>
    /// '下一个'按钮是否禁用
    /// </summary>
    public bool? Next { get; set; }
}
/// <summary>
/// 按钮方向
/// </summary>
public enum JumperLayout
{
    Horizontal,
    Vertical
}
/// <summary>
/// 对象则单独配置文案内容
/// </summary>
public class JumperTipsConfig
{
    /// <summary> 
    /// '上一个'按钮 Tips
    /// </summary>
    public string? Prev { get; set; }
    /// <summary>
    /// '当前'按钮 Tips
    /// </summary>
    public string? Current { get; set; }
    /// <summary>
    /// '下一个'按钮 Tips
    /// </summary>
    public string? Next { get; set; }
}

/// <summary>
/// 按钮形式。
/// </summary>
public enum JumperVariant
{
    Text,
    Outline
}

/// <summary>
/// Jumper按钮内部初始化配置类
/// </summary>
public class JumperConfig
{
    public string Icon { get; set; }
    public string ButtonClass { get; set; }
    public string Title { get; set; }
}
