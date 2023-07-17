//using ComponentBuilder.FluentRenderTree;
//using Microsoft.AspNetCore.Components.Forms;
//using System.Linq.Expressions;

//namespace TDesign;

///// <summary>
///// 下拉菜单选择器，用于收纳大量选项的信息录入类组件。
///// </summary>
///// <typeparam name="TValue">值的类型。</typeparam>
//public partial class TSelect<TValue> : BlazorComponentBase,IHasInputValue<TValue>,IHasChildContent
//{
//    /// <inheritdoc/>
//    [CascadingParameter] public EditContext? CascadedEditContext { get; set; }
//    /// <inheritdoc/>
//    [Parameter] public TValue? Value { get; set; }
//    /// <inheritdoc/>
//    [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }
//    /// <inheritdoc/>
//    [Parameter] public Expression<Func<TValue?>>? ValueExpression { get; set; }
//    /// <summary>
//    /// 提示的文本。
//    /// </summary>
//    [Parameter] public string? Placeholder { get; set; }
//    /// <summary>
//    /// 尺寸。
//    /// </summary>
//    [Parameter] public Size Size { get; set; } = Size.Medium;
//    /// <summary>
//    /// 禁用状态。
//    /// </summary>
//    [Parameter]public bool Disabled { get; set; }
//    [Parameter]public RenderFragment? ChildContent { get; set; }

//    internal TValue? SelectedValue { get; set; }

//    internal string? SelectedLabel { get; set; }

//    TPopup? _refPopup;   

//    protected override void BuildRenderTree(RenderTreeBuilder builder)
//    {
//        builder.Component<TPopup>()
//                .Attribute(m => m.AdditionalClass, "t-select__dropdown")
//                .Attribute(m => m.Trigger, PopupTrigger.Click)
//                .Attribute(m => m.PopupContent, dropdown =>
//                {
//                    dropdown.Div("t-select__dropdown-inner")
//                            .Class($"t-select__dropdown-inner--size-{GetInnerSize(Size)}")
//                            .Content(list => list.Element("ul", "t-select__list").Content(option =>
//                            {
//                                option.CreateCascadingComponent(this, 0, ChildContent);
//                            }).Close())
//                            .Close();
//                })
//                .Ref(component=>_refPopup=component)
//                .Content(select =>
//                {
//                    select.Div("t-select__wrap")
//                        .Content(wrapper =>
//                        {
//                            wrapper.Div("t-select-input t-select").Content(BuildInput).Close();
//                        })
//                        .Close();
//                })
//                .Close();

//        static string GetInnerSize(Size size) => size switch
//        {
//            Size.Small => "s",
//            Size.Medium => "m",
//            Size.Large => "l",
//            _ => throw new NotImplementedException()
//        };
//    }


//    private void BuildInput(RenderTreeBuilder builder)
//    {
//        builder.Div("t-input__wrap")
//            .Content(wrap =>
//            {
//                wrap.Div("t-input")
//                    .Class("t-is-readonly")
//                    .Class("t-input--prefix").Class("t-input--suffix")
//                    .Content(input=>
//                    {
//                        input.Div("t-input__prefix").Close();

//                        input.Element("input", "t-input__inner")
//                                .Attribute("placeholder", Placeholder)
//                                .Attribute("type", "text")
//                                .Attribute("readonly", true)
//                                .Attribute("value", SelectedLabel)
//                              .Close();

//                            input.Span("t-input__suffix")
//                                .Class("t-input__suffix-icon")
//                                //.Content(icon => icon.Component<TIcon>().Attribute(m => m.Name, IconName.ChevronDown).Close())
//                             .Close();
//                    })
//                    .Close();
//            })
//            .Close();
//    }
//}

