using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
[ParentComponent]
public partial class TSelect<TValue> : TDesignInputComonentBase<TValue>, IHasChildContent
{
    [Parameter] public IEnumerable<SelectOption>? Options { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    TPopup? RefPopup { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Open<TPopup>()
            .Style("position:absolute")
            .Attributes("Trigger", PopupTrigger.Click)
            .Attributes("PopupContent", HtmlHelper.CreateContent(list =>
                {
                    list.Div().Content(option =>
                    {
                        if (Options is null)
                        {
                            option.AddContent(0, ChildContent);
                        }
                        else
                        {
                            foreach (var item in Options)
                            {
                                option.CreateComponent<TSelectOption<TValue>>(0, item.Label, new { item.Value });
                            }
                        }
                    })
                    .Close();
                })
            )
            .Content(inner =>
            {
                inner.Div().Class("t-select__wrap")
                    .Content(select =>
                    {
                        select.Div().Class("t-select-input", "t-select")
                                    .Content(input =>
                                    {
                                        input.CreateComponent<TInputText<TValue>>(0,
                                                attributes: new
                                                {
                                                    Value,
                                                    ValueExpression,
                                                    ValueChanged,
                                                    Readonly = true,
                                                    unselectable = "on",
                                                    SuffixIcon = HtmlHelper.Class.Append("t-fake-arrow").Append("t-fake-arrow--active", RefPopup.Visible).Append("t-select__right-icon").ToString()
                                                });
                                    })
                                .Close();
                    })
                .Close();
            })
            .Capture<TPopup>(popup => RefPopup = popup)
        .Close();
    }
}

public class SelectOption
{
    public string? Label { get; set; }
    public string? Value { get; set; }
}

[ChildComponent(typeof(TSelect<>))]
[CssClass("t-select-option")]
[CascadingTypeParameter("Value")]
public class TSelectOption<TValue> : BlazorComponentBase, IHasChildContent
{
    [CascadingParameter] public TSelect<TValue> CascadingSelect { get; set; }
    [Parameter] public TValue? Value { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Span().Content(content => base.AddContent(content, sequence)).Close();
    }
}