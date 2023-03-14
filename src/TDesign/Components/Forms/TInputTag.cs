using ComponentBuilder.Fluent;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;
using System.Text;

namespace TDesign;

/// <summary>
/// 用于输入文本标签。
/// </summary>
[CssClass("t-input__inner")]
[HtmlTag("input")]
public class TInputTag : TDesignComponentBase,IHasInputValue<IEnumerable<string>>
{
    internal List<string> TagList { get; set; } = new();
    [CascadingParameter] public EditContext? CascadedEditContext { get; internal set; }
    [Parameter]public Expression<Func<IEnumerable<string>?>>? ValueExpression { get; set; }
    [Parameter] public IEnumerable<string>? Value { get; set; }
    [Parameter] public EventCallback<IEnumerable<string>?> ValueChanged { get; set; }

    private string? _inputText;

    private ElementReference? _inputRef;

    protected override void AfterSetParameters(ParameterView parameters)
    {
        this.InitializeInputValue();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.Div("t-input__wrap t-tag-input")
                .Class("t-is-empty", !TagList!.Any())
                .Class("t-tag-input--with-tag", TagList.Any())
                .Content(prefix =>
                {
                    prefix.Div("t-input t-input--prefix")
                        .Content(inner =>
                        {
                            inner.Div("t-input__prefix")
                                    .Content(tag =>
                                    {
                                        tag.ForEach<TTag, string>(TagList, loop =>
                                        {
                                            loop.attribute.ChildContent(loop.item);
                                        });
                                    })
                                .Close();
                            base.BuildRenderTree(inner);
                            inner.Span("t-input__input-pre").Close();

                        })
                        .Close();
                })
            .Close();

        //builder.Div("t-input t-input--prefix")
        //    .Content(inner =>
        //    {
        //        builder.CreateElement(0, "input", attributes: new
        //        {
        //            @class= "t-input__inner",
        //            onchange=HtmlHelper.Event.Create(this,Enter)
        //        });
        //        //inner
        //        ////              .Div("t-input__prefix")
        //        ////                .Content(tag =>
        //        ////                {
        //        ////                    tag.ForEach<TTag, string>(TagList, loop =>
        //        ////                    {
        //        ////                        loop.attribute.ChildContent(loop.item);
        //        ////                    });
        //        ////                })
        //        //            .Input(_inputText, @class: "t-input__inner")
        //        //                .Style("width:0px")
        //        //                //.Callback<ChangeEventArgs>("onchange", this, Enter)
        //        //                //.Callback<KeyboardEventArgs>("onkeypress", this, HandleKey)
        //        ////            .Span("t-input__input-pre")
        //        //        .Close();

        //    })
        //    .Close();
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["value"] = _inputText;
        attributes["onchange"] = HtmlHelper.Event.Create(this, Enter);
        attributes["onkeyup"] = HtmlHelper.Event.Create(this, HandleKey);
    }



    void Enter(ChangeEventArgs e)
    {
        _inputText = e.Value?.ToString();
    }

    void HandleKey(KeyboardEventArgs e)
    {
        switch ( e.Key )
        {
            case "Enter":
                var text = _inputText;
                if ( !string.IsNullOrWhiteSpace(text) )
                {
                    TagList.Add(text);
                    _inputText = default;
                    this.ChangeValue(TagList);
                    StateHasChanged();
                }
                break;
            case "Backspace":
                if ( TagList.Count > 0 )
                {
                    TagList.RemoveAt(TagList.Count - 1);
                    this.ChangeValue(TagList);
                    StateHasChanged();
                }
                break;
            default:
                break;
        }
    }
}
