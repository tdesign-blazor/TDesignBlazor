using ComponentBuilder;
using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 用于承载用户信息录入的文本框。
/// </summary>
/// <typeparam name="TValue">值的类型。</typeparam>
[HtmlTag("input")]
[CssClass("t-input__inner")]
public class TInputText<TValue> : TDesignInputComonentBase<TValue>
{
    /// <summary>
    /// 输入框的类型。
    /// </summary>
    [Parameter][HtmlAttribute] public InputType Type { get; set; } = InputType.Text;
    /// <summary>
    /// 输入框前缀显示的文本。
    /// </summary>
    [Parameter] public string? PrefixText { get; set; }
    /// <summary>
    /// 输入框前缀显示的图标名称。
    /// </summary>
    [Parameter] public object? PrefixIcon { get; set; }
    /// <summary>
    /// 输入框后缀显示的文本。
    /// </summary>
    [Parameter] public string? SuffixText { get; set; }
    /// <summary>
    /// 输入框后缀显示的图标名称。
    /// </summary>
    [Parameter] public object? SuffixIcon { get; set; }

    bool HasPrefix => !string.IsNullOrEmpty(PrefixText) || PrefixIcon is not null;

    bool HasSuffix => !string.IsNullOrEmpty(SuffixText) || SuffixIcon is not null;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        BuildInputWrapper(builder, 0, content =>
        {
            BuildPrefixOrSuffix(content, 0, PrefixText, PrefixIcon, "prefix");
            base.BuildRenderTree(content);
            BuildPrefixOrSuffix(content, 2, SuffixText, SuffixIcon, "suffix");
        }, $"{(HasPrefix ? "t-input--prefix" : "")} {(HasSuffix ? "t-input--suffix" : "")}");
    }


    void BuildPrefixOrSuffix(RenderTreeBuilder builder, int sequence, string? text, object? icon, string cssClassName)
    {
        builder.CreateElement(sequence, "span", content =>
        {
            if (!string.IsNullOrEmpty(text))
            {
                content.AddContent(0, text);
            }
            if (icon is not null)
            {
                content.CreateComponent<TIcon>(0, attributes: new { Name = icon });
            }
        }, new
        {
            @class = HtmlHelper.Instance.Class()
                    .Append($"t-input__{cssClassName}", !string.IsNullOrEmpty(text) || icon is not null)
                    .Append($"t-input__{cssClassName}-icon", icon is not null)
        }, !string.IsNullOrEmpty(text) || icon is not null);
    }

    protected override string EventName => "onchange";

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        base.BuildAttributes(attributes);
        attributes["value"] = this.GetValueAsString();
    }
}

public enum InputType
{
    Text,
    Url,
    Number,
    Tel,
    Password,
    Search,
    Submit,
    Hidden
}