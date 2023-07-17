//using System.Diagnostics.CodeAnalysis;

//namespace TDesign;


///// <summary>
///// 表示作为 <see cref="TSelect{TValue}"/> 的选项组件。
///// </summary>
///// <typeparam name="TValue">值的类型。</typeparam>
//[ChildComponent(typeof(TSelect<>))]
//[CascadingTypeParameter("TValue")]
//[HtmlTag("li")]
//[CssClass("t-select-option")]
//public class TSelectOption<TValue> : TDesignComponentBase, IHasChildContent
//{
//    /// <summary>
//    /// 级联 <see cref="TSelect{TValue}"/> 组件。
//    /// </summary>
//    [CascadingParameter][NotNull] protected TSelect<TValue> CascadingSelect { get; set; }
//    /// <summary>
//    /// 选项的值。
//    /// </summary>
//    [Parameter] public TValue? Value { get; set; }

//    /// <summary>
//    /// 选中时显示的文本。
//    /// </summary>
//    [Parameter]public string? Label { get; set; }

//    /// <summary>
//    /// 禁用选项。
//    /// </summary>
//    [Parameter][CssClass("t-is-disabled")] public bool Disabled { get; set; }
//    /// <summary>
//    /// 自定义选项的内容。
//    /// </summary>
//    [Parameter] public RenderFragment? ChildContent { get; set; }

//    /// <summary>
//    /// 是否选中。
//    /// </summary>
//    bool Checked => CascadingSelect?.SelectedValue?.Equals(Value) ?? false;

//    protected override void OnInitialized()
//    {
//        base.OnInitialized();

//        if ( Value?.GetType() != typeof(TValue) )
//        {
//            throw new InvalidOperationException($"参数 {nameof(this.Value)} 的数据类型必须与 {typeof(TSelect<>).FullName} 的数据类型相同");
//        }
//    }

//    protected override void OnParametersSet()
//    {
//        base.OnParametersSet();
//        ChildContent ??=builder=> builder.Content(Label);
//    }

//    /// <inheritdoc/>
//    protected override void AddContent(RenderTreeBuilder builder, int sequence)
//    {
//        builder.CreateElement(sequence, "span", ChildContent);
//    }

//    /// <inheritdoc/>
//    protected override void BuildCssClass(ICssClassBuilder builder)
//    {
//        if ( builder.Contains("t-is-selected") )
//        {
//            builder.Remove("t-is-selected");
//        }
//        builder.Append("t-is-selected", Checked);

//        builder.Append(CascadingSelect?.Size.GetCssClass());
//    }

//    /// <inheritdoc/>
//    protected override void BuildAttributes(IDictionary<string, object> attributes)
//    {
//        if ( !Disabled && CascadingSelect is not null )
//        {
//            attributes["onclick"] = HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, () =>
//            {
//                CascadingSelect.ChangeValue(Value);
//                CascadingSelect.SelectedValue = Value;
//                CascadingSelect.SelectedLabel = Label;
//                StateHasChanged();
//            });
//        }
//    }
//}
