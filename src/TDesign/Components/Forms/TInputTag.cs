namespace TDesign;

/// <summary>
/// 用于输入文本标签。
/// </summary>
public class TInputTag : TDesignInputComonentBase<IEnumerable<string>>
{
    internal List<string> TagList { get; set; } = new();

    private string? _inputText;
    /// <summary>
    /// 设置标签的前缀文本。
    /// </summary>
    [Parameter] public string? Prefix { get; set; }
    /// <summary>
    /// 设置标签的前缀任意内容。
    /// </summary>
    [Parameter] public RenderFragment? PrefixContent { get; set; }
    
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        AdditionalClass += HtmlHelper.Class.Append("t-tag-input")
                .Append("t-is-empty", !TagList!.Any())
                .Append("t-tag-input--with-tag", TagList.Any())
                .ToString();

        if ( Prefix.IsNotNullOrEmpty() )
        {
            PrefixContent ??= HtmlHelper.CreateContent(Prefix);
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        BuildInputWrapper(builder, 0, inner =>
        {
            inner.Div("t-input__prefix")
                    .Content(tag =>
                    {
                        tag.Div("t-tag-input__prefix", PrefixContent is not null).Content(PrefixContent).Close();

                        tag.ForEach<TTag, string>(TagList, loop =>
                        {
                            loop.attribute.ChildContent(loop.item)
                                        .Attribute(nameof(TTag.Closable), true)
                                        .Callback<bool>(nameof(TTag.OnClosing), this, closed =>
                                        {
                                            if ( !closed )
                                            {
                                                return;
                                            }
                                            Remove(loop.index);
                                        })
                                        ;
                        });
                    })
                .Input(_inputText,@class: "t-input__inner").Style("width:0px")
                    .Callback<ChangeEventArgs>("onchange",this,EnterInputText)
                    .Callback<KeyboardEventArgs>("onkeyup",this,HandleKey)
                .Span("t-input__input-pre")
                .Close();
        }, "t-input--prefix");
    }

    Task EnterInputText(ChangeEventArgs e)
    {
        _inputText = e.Value?.ToString();
        return Task.CompletedTask;
    }

    protected override void BuildEventAttribute(IDictionary<string, object> attributes)
    {
    }

    void HandleKey(KeyboardEventArgs e)
    {
        switch ( e.Key )
        {
            case "Enter":
                if ( !string.IsNullOrWhiteSpace(_inputText) && !TagList.Contains(_inputText) )
                {
                    TagList.Add(_inputText);
                    Value = TagList;
                    ValueChanged.InvokeAsync(TagList);
                    StateHasChanged();
                    _inputText = default;
                }
                break;
            case "Backspace":
                if ( TagList.Count > 0 )
                {
                    Remove(TagList.Count - 1);
                }
                break;
            default:
                break;
        }
    }

    Task Remove(int index)
    {
        if ( index > -1 )
        {
            TagList.RemoveAt(index);
            Value = TagList;
            ValueChanged.InvokeAsync(TagList);
            StateHasChanged();
        }
        return Task.CompletedTask;
    }
}
