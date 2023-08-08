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
public class TInputAdornment : TDesignAdditionParameterComponentBase, IHasChildContent
{
    /// <inheritdoc/>
    [ParameterApiDoc("装饰器任意内容")]
    [Parameter]public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 设置前面追加的文本。若设置了 <see cref="PrependContent"/> 参数，则该参数将被忽略。
    /// </summary>
    [ParameterApiDoc("前面追加的文本，若 PrependContent 则该参数将忽略")]
    [Parameter]public string? Prepend { get; set; }
    /// <summary>
    /// 设置前面追加的任意 UI 内容。
    /// </summary>
    [ParameterApiDoc("前面追加的任意 UI 内容")]
    [Parameter]public RenderFragment? PrependContent { get; set; }
    /// <summary>
    /// 设置后面追加的文本。若设置了 <see cref="AppendContent"/> 参数，则该参数将被忽略。
    /// </summary>
    [ParameterApiDoc("后面追加的文本，若 AppendContent 则该参数将忽略")]
    [Parameter]public string? Append { get; set; }
    /// <summary>
    /// 设置后面追加的任意 UI 内容。
    /// </summary>
    [ParameterApiDoc("后面追加的任意 UI 内容")]
    [Parameter]public RenderFragment? AppendContent { get; set; }

    /// <inheritdoc/>
    protected override void AfterSetParameters(ParameterView parameters)
    {
        base.AfterSetParameters(parameters);
        if (!string.IsNullOrWhiteSpace( Prepend ))
        {
            PrependContent ??= builder => builder.AddContent(0, Prepend);
        }
        if ( !string.IsNullOrWhiteSpace(Append) )
        {
            AppendContent ??= builder => builder.AddContent(0, Append);
        }
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Span("t-input-adornment__prepend",PrependContent is not null).Content(text => BuildText(text, PrependContent)).Close();
        builder.AddContent(sequence + 1, ChildContent);
        builder.Span("t-input-adornment__append", AppendContent is not null).Content(text => BuildText(text, AppendContent)).Close();

    }

    void BuildText(RenderTreeBuilder builder, RenderFragment? fragment)
        => builder.Span("t-input-adornment__text").Content(fragment).Close();
}
