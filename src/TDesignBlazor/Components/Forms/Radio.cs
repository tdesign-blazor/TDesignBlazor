using Microsoft.AspNetCore.Components.Rendering;

namespace TDesignBlazor.Components;

/// <summary>
/// 单选框选项，必须应用于 <see cref="RadioGroup{TValue}"/> 组件中。
/// </summary>
/// <typeparam name="TValue">与 <see cref="RadioGroup{TValue}"/> 的值类型</typeparam>
[HtmlTag("input")]
[ChildComponent(typeof(RadioGroup<>))]
public class Radio<TValue> : BlazorComponentBase, IDisposable
{
    /// <summary>
    /// 级联的单选按钮组。
    /// </summary>
    [CascadingParameter] public RadioGroup<TValue> CascadingRadioGroup { get; set; }
    /// <summary>
    /// 获取或设置单选按钮的值。
    /// </summary>
    [Parameter] public TValue? Value { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 禁用状态。
    /// </summary>
    [Parameter][HtmlAttribute] public bool Disabled { get; set; }
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Value?.GetType() != typeof(TValue))
        {
            throw new InvalidOperationException($"参数 {nameof(this.Value)} 的数据类型必须与 {typeof(Radio<>).FullName} 的数据类型相同");
        }

        CascadingRadioGroup.RerenderRadioBoxes += StateHasChanged;

        this.Disabled = CascadingRadioGroup.Disabled;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateElement(0, "label", content =>
        {
            base.BuildRenderTree(content);

            content.OpenRegion(10000);
            content.CreateElement(0, "span", attributes: new
            {
                @class = HtmlHelper.CreateCssBuilder().Append(!CascadingRadioGroup.Button, "t-radio__input", "t-radio-button__input")
            });
            content.CreateElement(1, "span", ChildContent, new
            {
                @class = HtmlHelper.CreateCssBuilder().Append(!CascadingRadioGroup.Button, "t-radio__label", "t-radio-button__label")
            });
            content.CloseRegion();
        },
        new
        {
            @class = HtmlHelper.CreateCssBuilder()
                            .Append(!CascadingRadioGroup.Button, "t-radio", "t-radio-button")
                            .Append("t-is-disabled", Disabled)
                            .Append("t-is-checked", Checked.HasValue && Checked.Value),
            style = HtmlHelper.CreateStyleBuilder().Append("background-color: var(--td-bg-color-container-select);transition: all .2s cubic-bezier(.38,0,.24,1);", CascadingRadioGroup.Button && CascadingRadioGroup.ButtonStyle == RadioButtonStyle.Filled && Checked.HasValue && Checked.Value)
            .Append("background-color: var(--td-brand-color);transition: all .2s cubic-bezier(.38,0,.24,1);", CascadingRadioGroup.Button && CascadingRadioGroup.ButtonStyle == RadioButtonStyle.PrimaryFilled && Checked.HasValue && Checked.Value)
        });
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        base.BuildAttributes(attributes);
        attributes["type"] = "radio";
        attributes["name"] = CascadingRadioGroup.Name;
        attributes["value"] = BindConverter.FormatValue(Value)?.ToString();
        attributes["onchange"] = CascadingRadioGroup.ChangeEventCallback;

        attributes["checked"] = Checked.HasValue && Checked.Value;
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append(CascadingRadioGroup.Button, "t-radio-button__former", "t-radio__former");
    }

    public void Dispose()
    {
        if (CascadingRadioGroup != null)
        {
            CascadingRadioGroup.RerenderRadioBoxes -= StateHasChanged;
        }
    }

    bool? Checked => CascadingRadioGroup?.SelectedValue?.Equals(Value);
}
