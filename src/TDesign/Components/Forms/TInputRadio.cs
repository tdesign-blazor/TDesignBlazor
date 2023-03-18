namespace TDesign;

/// <summary>
/// 单选框选项，必须应用于 <see cref="TInputRadioGroup{TValue}"/> 组件中。
/// </summary>
/// <typeparam name="TValue">与 <see cref="TInputRadioGroup{TValue}"/> 的值类型</typeparam>
[HtmlTag("input")]
[ChildComponent(typeof(TInputRadioGroup<>))]
[ChildComponent(typeof(TInputRadioContainer<>))]
public class TInputRadio<TValue> : TDesignComponentBase
{
    /// <summary>
    /// 级联的单选按钮组。这个对象时可选的
    /// </summary>
    [CascadingParameter] public TInputRadioGroup<TValue> CascadingRadioGroup { get; set; }

    /// <summary>
    /// 级联单选组件容器。
    /// </summary>
    [CascadingParameter] public TInputRadioContainer<TValue> CascadingRadioContainer { get; set; }

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

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if ( Value?.GetType() != typeof(TValue) )
        {
            throw new InvalidOperationException($"参数 {nameof(this.Value)} 的数据类型必须与 {typeof(TInputRadio<>).FullName} 的数据类型相同");
        }

        if ( CascadingRadioGroup is not null )
        {
            CascadingRadioContainer.NotifyRadioInputRendered += StateHasChanged;

            this.Disabled = CascadingRadioGroup.Disabled;
        }
    }

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateElement(0, "label", content =>
        {
            base.BuildRenderTree(content);

            content.OpenRegion(10000);
            content.CreateElement(0, "span", attributes: new
            {
                @class = HtmlHelper.Class.Append("t-radio__input", !IsButtonStyle, "t-radio-button__input")
            });
            content.CreateElement(1, "span", ChildContent, new
            {
                @class = HtmlHelper.Class.Append("t-radio__label", !IsButtonStyle, "t-radio-button__label")
            });
            content.CloseRegion();
        },
        new
        {
            @class = HtmlHelper.Class
                            .Append("t-radio", !IsButtonStyle, "t-radio-button")
                            .Append("t-is-disabled", Disabled)
                            .Append("t-is-checked", Checked),
            style = HtmlHelper.Style.Append("background-color: var(--td-bg-color-container-select);transition: all .2s cubic-bezier(.38,0,.24,1);", CascadingRadioGroup?.ButtonStyle == RadioButtonStyle.Filled && Checked)
            .Append("background-color: var(--td-brand-color);transition: all .2s cubic-bezier(.38,0,.24,1);", CascadingRadioGroup?.ButtonStyle == RadioButtonStyle.PrimaryFilled && Checked)
        });
    }

    /// <inheritdoc/>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        base.BuildAttributes(attributes);
        attributes["type"] = "radio";
        attributes["name"] = CascadingRadioContainer.ValueExpression.Name; 
        attributes["value"] = BindConverter.FormatValue(Value);
        attributes["onchange"] = CascadingRadioContainer.ChangeEventCallback;

        attributes["checked"] = Checked;
    }

    /// <inheritdoc/>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-radio-button__former", IsButtonStyle, "t-radio__former");
    }

    /// <inheritdoc/>
    protected override void DisposeComponentResources()
    {
        if ( CascadingRadioGroup != null )
        {
            CascadingRadioContainer.NotifyRadioInputRendered -= StateHasChanged;
        }
    }

    /// <summary>
    /// 是否为按钮风格的单选按钮，在 <see cref="CascadingRadioGroup"/> 有值时有效。
    /// </summary>
    bool IsButtonStyle =>  CascadingRadioGroup.ButtonStyle.HasValue;

    /// <summary>
    /// 是否选中。
    /// </summary>
    bool Checked => CascadingRadioGroup?.SelectedValue?.Equals(Value) ?? false;
}
