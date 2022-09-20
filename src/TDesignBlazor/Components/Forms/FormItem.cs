using System.Reflection;
using System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor;
/// <summary>
/// 表示作为表单的项。必须在 <see cref="Form"/> 组件中。
/// </summary>
[CssClass("t-form__item")]
[ChildComponent(typeof(Form))]
public class FormItem : BlazorComponentBase, IHasChildContent, IDisposable
{
    /// <summary>
    /// 级联 <see cref="Form"/> 组件的实例。
    /// </summary>
    [CascadingParameter][NotNull] public Form CascadingForm { get; set; }
    /// <summary>
    /// 级联 <see cref="EditContext"/> 实例。
    /// </summary>
    [CascadingParameter][NotNull] EditContext CascadingEditContext { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 设置表单项显示的标签文本。
    /// </summary>
    [Parameter] public string? Label { get; set; }
    /// <summary>
    /// 设置表单项文本的宽度，默认是 60px 。
    /// </summary>
    [Parameter] public string? LabelWidth { get; set; } = "60px";
    /// <summary>
    /// 设置文本自动加上冒号。
    /// </summary>
    [Parameter] public bool LabelColon { get; set; }
    /// <summary>
    /// 设置必填时自动加上红色的 “*” 符号。
    /// </summary>
    [Parameter] public bool Required { get; set; }
    /// <summary>
    /// 帮助的提示文字。
    /// </summary>
    [Parameter] public string? HelpText { get; set; }
    /// <summary>
    /// 设置一个委托，表示具备表单验证的字段。
    /// </summary>
    [Parameter] public Expression<Func<dynamic>>? For { get; set; }

    FieldIdentifier? Identifier { get; set; }


    string? StatusCssClass { get; set; }
    IEnumerable<string> ErrorMessages { get; set; } = Array.Empty<string>();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        CascadingEditContext.OnValidationStateChanged += CascadingEditContext_ValidationStateChanged;
        CascadingEditContext.SetFieldCssClassProvider(new FormFieldCssClassProvider());
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (For is not null)
        {
            Identifier = FieldIdentifier.Create(For);

            var member = GetMember(For.Body);

            if (member.TryGetCustomAttribute<RequiredAttribute>(out var requiredAttribute))
            {
                Required = !requiredAttribute!.AllowEmptyStrings;
            }

            if (member.TryGetCustomAttribute<DisplayAttribute>(out var displayAttribute) && string.IsNullOrEmpty(Label))
            {
                Label = displayAttribute?.Name;
            }
        }
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        if (Identifier.HasValue)
        {
            StatusCssClass = CascadingEditContext.FieldCssClass(Identifier.Value);
            ErrorMessages = CascadingEditContext.GetValidationMessages(Identifier.Value);
        }

        BuildLabel(builder, sequence);
        BuildControl(builder, sequence + 1);
    }

    void BuildLabel(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "label", Label, new { @for = "" });
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder()
                                .Append("t-form__label")
                                .Append($"t-form__label--{CascadingForm.Alignment.GetCssClass()}")
                                .Append("t-form__label--colon", LabelColon)
                                .Append("t-form__label--required", Required)
                                ,
            style = HtmlHelper.CreateStyleBuilder().Append($"width:{LabelWidth}", !string.IsNullOrEmpty(LabelWidth) && CascadingForm.Alignment != FormAlignment.Top)
        });
    }

    void BuildControl(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "div", ChildContent, new { @class = "t-form__controls-content" });

            content.CreateElement(1, "div", tip => tip.AddContent(0, HelpText), new { @class = "t-input__help" }, !string.IsNullOrEmpty(HelpText));

            int index = 0;
            foreach (var error in ErrorMessages)
            {
                content.CreateElement(index + 2, "div", extra => extra.AddContent(0, error), new { @class = "t-input__extra" });
                index++;
            }
        }, new
        {
            @class = HtmlHelper.CreateCssBuilder().Append("t-form__controls")
                                                .Append($"t-is-{StatusCssClass}", Identifier.HasValue)
                                                .Append($"t-form--success-border", StatusCssClass == Status.Success.GetCssClass()),
            style = HtmlHelper.CreateStyleBuilder().Append("margin-left:60px", CascadingForm.Alignment != FormAlignment.Top)
        });
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-form__item-with-help", !string.IsNullOrEmpty(HelpText))
            .Append("t-form__item-with-extra", ErrorMessages.Any())
            ;
    }

    /// <summary>
    /// Gets the member.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns></returns>
    private MemberInfo GetMember(Expression expression)
    {
        switch (expression.NodeType)
        {
            case ExpressionType.MemberAccess:
                return ((MemberExpression)expression).Member;
            case ExpressionType.Convert:
                return GetMember(((UnaryExpression)expression).Operand);
            default:
                break;
        }
        return default;
    }

    /// <summary>
    /// Cleanup of component
    /// </summary>
    public void Dispose()
    {
        if (CascadingEditContext != null)
        {
            CascadingEditContext.OnValidationStateChanged -= CascadingEditContext_ValidationStateChanged;
        }
    }

    private void CascadingEditContext_ValidationStateChanged(object? sender, ValidationStateChangedEventArgs? e)
        => StateHasChanged();
}
