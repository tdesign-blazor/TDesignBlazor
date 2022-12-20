using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
[ParentComponent]
public partial class TSelect<TValue>
{
    [Parameter] public IEnumerable<SelectOption>? Options { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    TPopup? RefPopup { get; set; }


    string? SelectedLabel { get; set; }

    //protected override void AddContent(RenderTreeBuilder builder, int sequence)
    //{
    //    builder.Open<TPopup>()
    //        .Style("position:absolute")
    //        .Attributes("Trigger", PopupTrigger.Click)
    //        .Attributes("PopupContent", HtmlHelper.CreateContent(list =>
    //            {
    //                list.Div().Class("t-select__dropdown-inner t-select__dropdown-inner--size-m").Content(dropdown =>
    //                {
    //                    dropdown.Open("ul").Class("t-select__list")
    //                    .Content(option =>
    //                    {
    //                        if (Options is null)
    //                        {
    //                            option.AddContent(0, ChildContent);
    //                        }
    //                        else
    //                        {
    //                            foreach (var item in Options)
    //                            {
    //                                option.CreateComponent<TSelectOption<TValue>>(0, item.Label, new { item.Value });
    //                            }
    //                        }
    //                    })
    //                    .Close();
    //                })
    //                .Close();
    //            })
    //        )
    //        .Content(inner =>
    //        {
    //            inner.Div().Class("t-select__wrap")
    //                .Content(select =>
    //                {
    //                    select.Div().Class("t-select-input", "t-select")
    //                                .Content(input =>
    //                                {
    //                                    input.CreateComponent<TInputText<string>>(0,
    //                                            attributes: new
    //                                            {
    //                                                Value=SelectedValue,
    //                                                ValueExpression=()=> SelectedValue, 
    //                                                ValueChanged,
    //                                                Readonly = true,
    //                                                unselectable = "on",
    //                                                SuffixIcon = HtmlHelper.Class.Append("t-fake-arrow").Append("t-fake-arrow--active", RefPopup.Visible).Append("t-select__right-icon").ToString()
    //                                            });
    //                                })
    //                            .Close();
    //                })
    //            .Close();
    //        })
    //        .Capture<TPopup>(popup => RefPopup = popup)
    //    .Close();
    //}

    public async Task SelectValue(TValue? value, string? label)
    {
        Value = value;
        await ValueChanged?.InvokeAsync(value);
        SelectedLabel = label;
        //await this.Refresh();
        await RefPopup.Hide();
    }
}

public class SelectOption
{
    public string? Label { get; set; }
    public string? Value { get; set; }
}

[ChildComponent(typeof(TSelect<>))]
public class TSelectOption<TValue> : BlazorComponentBase, IHasChildContent
{
    [CascadingParameter] protected TSelect<TValue>? CascadingSelect { get; set; }
    [Parameter] public TValue? Value { get; set; }
    [Parameter] public string? Label { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnComponentParameterSet()
    {
        ChildContent ??= builder => builder.AddContent(0, Label);
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Div().Class("t-select-option")
            .EventCallback("onclick", HtmlHelper.Event.Create(this, () => CascadingSelect.SelectValue(Value, Label)))
            .Content(content => content.Span().Content(span =>
            span.AddContent(0, ChildContent)).Close())
        .Close();

    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-is-selected", CascadingSelect is not null && CascadingSelect.Value is not null && CascadingSelect.Value.Equals(Value));
    }
}