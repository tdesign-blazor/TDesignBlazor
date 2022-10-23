using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 开关组件
/// </summary>
[CssClass("t-switch")]
public class TSwitch : BlazorInputComponentBase<bool>, IHasDisabled
{
    /// <summary>
    /// 是否禁用
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// 是否加载中
    /// </summary>
    [Parameter] public bool Loading { get; set; }

    /// <summary>
    /// 尺寸。
    /// </summary>
    [Parameter][CssClass] public Size Size { get; set; } = Size.Medium;

    /// <summary>
    ///  执行当 <see cref="TSwitch"/> 的值发生改变时的事件。
    /// </summary>
    [Parameter] public EventCallback<bool> OnChange { get; set; }

    /// <summary>
    /// 开关打开时，需要显示的自定义内容。
    /// </summary>
    [Parameter] public RenderFragment? TrueContent { get; set; }

    /// <summary>
    /// 开关关闭时，显示的自定义内容。
    /// </summary>
    [Parameter] public RenderFragment? FalseContent { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(++sequence, "span",
        span =>
        {
            span.CreateComponent<TIcon>(++sequence, attributes: new { Name = IconName.Loading }, condition: Loading);
        },
        attributes: new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-switch__handle").Append(LOADING_CLASS_NAME, Loading).Append(DISABLED_CLASS_NAME, Disabled)
        });
        var content = Value ? TrueContent : FalseContent;
        var cssBuilder = HtmlHelper.CreateCssBuilder().Append("t-switch__content").Append(Size.GetCssClass()).Append(DISABLED_CLASS_NAME, Disabled);
        if (content == null)
        {
            builder.CreateElement(++sequence, "div", attributes: new { @class = cssBuilder });
        }
        else
        {
            builder.CreateElement(++sequence, "div", childContent: content, attributes: new { @class = cssBuilder });
        }

    }

    private const string LOADING_CLASS_NAME = "t-is-loading";

    private const string DISABLED_CLASS_NAME = "t-is-disabled";

    private const string CHECKED_CLASS_NAME = "t-is-checked";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="attributes"></param>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["onclick"] = HtmlHelper.CreateCallback(this, () =>
        {
            CurrentValue = !Value;
            OnChange.InvokeAsync(CurrentValue);
        });
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        AppendClass(builder, CHECKED_CLASS_NAME, CurrentValue);
        AppendClass(builder, LOADING_CLASS_NAME, Loading);
        AppendClass(builder, DISABLED_CLASS_NAME, Disabled);
    }

    private static void AppendClass(ICssClassBuilder builder, string name, bool condition)
    {
        if (builder.Contains(name) && !condition)
        {
            builder.Remove(name);
        }
        else
        {
            builder.Append(name, condition);
        }
    }
}
