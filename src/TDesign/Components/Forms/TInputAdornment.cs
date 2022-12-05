using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDesign;
/// <summary>
/// 用于装饰输入类组件的装饰器
/// </summary>
[CssClass("t-input-adornment")]
public class TInputAdornment : BlazorComponentBase, IHasChildContent
{
    /// <inheritdoc/>
    [Parameter]public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 设置前面追加的文本。若设置了 <see cref="PrependContent"/> 参数，则该参数将被忽略。
    /// </summary>
    [Parameter]public string? Prepend { get; set; }
    /// <summary>
    /// 设置前面追加的任意 UI 内容。
    /// </summary>
    [Parameter]public RenderFragment? PrependContent { get; set; }
    /// <summary>
    /// 设置后面追加的文本。若设置了 <see cref="AppendContent"/> 参数，则该参数将被忽略。
    /// </summary>
    [Parameter]public string? Append { get; set; }
    /// <summary>
    /// 设置后面追加的任意 UI 内容。
    /// </summary>
    [Parameter]public RenderFragment? AppendContent { get; set; }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if ( !string.IsNullOrEmpty(Prepend) )
        {
            PrependContent ??= builder => builder.AddContent(0, Prepend);
        }
        if ( !string.IsNullOrEmpty(Append) )
        {
            AppendContent ??= builder => builder.AddContent(0, Append);
        }
    }

    /// <inheritdoc/>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-input-adornment--prepend", PrependContent is not null)
            .Append("t-input-adornment--append",AppendContent is not null)
            ;
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "span", PrependContent, new { @class = "t-input-adornment__prepend" }, PrependContent is not null);

        builder.AddContent(sequence + 1, ChildContent);

        builder.CreateElement(sequence+2, "span", AppendContent, new { @class = "t-input-adornment__append" },AppendContent is not null);
    }
}
