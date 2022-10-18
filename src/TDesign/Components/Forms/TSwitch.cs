using ComponentBuilder;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign;

/// <summary>
/// 开关组件
/// </summary>
[CssClass("t-switch")]
[HtmlTag("div")]
public class TSwitch : BlazorInputComponentBase<bool>, IHasDisabled
{
    /// <summary>
    /// 是否禁用
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// 是否加载中
    /// </summary>
    [Parameter][CssClass(ICON_LOADING_NAME)] public bool Loading { get; set; }

    /// <summary>
    /// 尺寸。
    /// </summary>
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;

    /// <summary>
    ///  执行当 <see cref="TSwitch"/> 触发的事件。
    /// </summary>
    [Parameter][HtmlEvent("onchange")] public EventCallback<MouseEventArgs?> OnChange { get; set; }

    private const string ICON_LOADING_NAME = "t-is-loading";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {

        builder.CreateElement(++sequence, "span",
            span =>
            {
                span.CreateComponent<TIcon>(++sequence, attributes: new { Name = ICON_LOADING_NAME }, condition: Loading);
            },
            attributes: new { @class = $"t-switch__handle{(Loading ? $" {ICON_LOADING_NAME}" : string.Empty)}" });
        builder.CreateElement(++sequence, "div", attributes: new { @class = $"t-switch__content {Size.GetCssClass()}" });
        base.AddContent(builder, sequence);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this.Refresh();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attributes"></param>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["onclick"] = HtmlHelper.CreateCallback(this, () =>
        {
            Value = !Value;
        });
        base.BuildAttributes(attributes);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if (Value)
        {
            builder.Append("t-is-checked");
        }
        base.BuildCssClass(builder);
    }
}
