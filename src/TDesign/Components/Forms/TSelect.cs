using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
public class TSelect<TValue> : TDesignInputComonentBase<TValue>, IHasChildContent
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
                    list.Div().Class("t-select__dropdown-inner t-select__dropdown-inner--size-m").Content(dropdown =>
                    {
                        dropdown.Open("ul").Class("t-select__list")
                        .Content(option =>
                        {
                            option.CreateCascadingComponent(this, 0, select =>
                            {
                                if (Options is null)
                                {
                                    select.AddContent(0, ChildContent);
                                }
                                else
                                {
                                    foreach (var item in Options)
                                    {
                                        select.CreateComponent<TSelectOption<TValue>>(0, item.Label, new { item.Value });
                                    }
                                }
                            });
                        })
                        .Close();
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

    public async Task SelectValue(TValue? value)
    {
        Value = value;
        await ValueChanged?.InvokeAsync(value);
        await this.Refresh();
        await RefPopup.Hide();
    }
}

public class SelectOption
{
    public string? Label { get; set; }
    public string? Value { get; set; }
}


public class TSelectOption<TValue> : BlazorComponentBase, IHasChildContent
{
    [CascadingParameter] protected TSelect<TValue>? CascadingSelect { get; set; }
    [Parameter] public TValue? Value { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        ArgumentNullException.ThrowIfNull(CascadingSelect, nameof(CascadingSelect));

        await CascadingSelect.AddChildComponent(this);
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        return base.SetParametersAsync(parameters);
    }


    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.Div().Class("t-select-option")
            .EventCallback("onclick", HtmlHelper.Event.Create(this, () => CascadingSelect.SelectValue(Value)))
            .Content(content => AddContent(content, 0))
        .Close();
    }


    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Span()
            .Content(content => base.AddContent(content, sequence))
        .Close();
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("t-is-selected", CascadingSelect is not null && CascadingSelect.Value is not null && CascadingSelect.Value.Equals(Value));
    }
}