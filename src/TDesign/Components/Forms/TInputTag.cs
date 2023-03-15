using ComponentBuilder;
using ComponentBuilder.JSInterope;
using Microsoft.JSInterop;

namespace TDesign;

/// <summary>
/// 用于输入文本标签。
/// </summary>
public class TInputTag : TDesignInputComonentBase<IEnumerable<string>>
{
    List<string> _currentTagList = new();

    private string? _inputText;
    private ElementReference? _inputRef;
    /// <summary>
    /// 设置标签的前缀文本。
    /// </summary>
    [Parameter] public string? Prefix { get; set; }
    /// <summary>
    /// 设置标签的前缀任意内容。
    /// </summary>
    [Parameter] public RenderFragment? PrefixContent { get; set; }
    /// <summary>
    /// 最多容纳的标签数量。
    /// </summary>
    [Parameter] public int? Max { get; set; }
    /// <summary>
    /// 设置一个当按下回车键后的回调方法。
    /// </summary>
    [Parameter] public EventCallback<string?> OnEnterPressed { get; set; }

    /// <summary>
    /// 设置文本框的水印占位字符串。
    /// </summary>
    [Parameter] public string? Placeholder { get; set; }
    /// <summary>
    /// 设置标签的主题颜色。
    /// </summary>
    [Parameter] public Theme? Theme { get; set; }

    protected override void AfterSetParameters(ParameterView parameters)
    {
        base.AfterSetParameters(parameters);

    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if ( Prefix.IsNotNullOrEmpty() )
        {
            PrefixContent ??= HtmlHelper.CreateContent(Prefix);
        }
    }
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if ( firstRender )
        {
            _currentTagList.AddRange(Value);
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

                        tag.ForEach<TTag, string>(Value, loop =>
                        {
                            loop.attribute.ChildContent(loop.item)
                                        .Attribute(nameof(TTag.Closable), true)
                                        .Attribute(nameof(TTag.Size),Size)
                                        .Attribute(nameof(TTag.Theme),Theme)
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
                    }).Close();

            inner.CreateElement(0, "input", attributes: new
            {
                @class = "t-input__inner",
                placeholder = Placeholder
            }, captureReference: el => _inputRef = el);

            inner.Span("t-input__input-pre").Close();
        }, "t-input--prefix", HtmlHelper.Class.Append("t-tag-input")
                .Append("t-is-empty", !Value!.Any())
                .Append("t-tag-input--with-tag", Value.Any())
                .ToString());
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        //if ( firstRender )
        //{
            var tdesignJs = await JS.Value.ImportTDesignScriptAsync();
            await tdesignJs.InvokeVoidAsync("tagInput.pressKey", _inputRef, CallbackFactory.Create<int, string>((keyCode, inputText) =>
            {
                _inputText = inputText;
                HandleKey(keyCode);
            }));
            //await tdesignJs.InvokeVoidAsync("tagInput.remove", _inputRef, CallbackFactory.Create<int>(keyCode =>
            //{
            //    HandleKey(keyCode);
            //}));
        //}
    }

    protected override void BuildEventAttribute(IDictionary<string, object> attributes)
    {
    }

    void HandleKey(int keyCode)
    {
        switch ( keyCode )
        {
            case 13:
                OnEnterPressed.InvokeAsync(_inputText);

                if ( CanEnter() )
                {
                    _currentTagList.Add(_inputText);
                    Value = _currentTagList;
                    ValueChanged.InvokeAsync(_currentTagList);
                    StateHasChanged();
                    _inputText = default;
                }
                break;
            case 8: //Backspace
                if ( _currentTagList.Count > 0 )
                {
                    Remove(_currentTagList.Count - 1);
                }
                break;
            default:
                break;
        }
    }

    private bool CanEnter()
    {
        if ( string.IsNullOrEmpty(_inputText) )
        {
            return false;
        }
        if ( _currentTagList.Contains(_inputText!) )
        {
            return false;
        }
        if ( Max.HasValue && _currentTagList.Count >= Max.Value )
        {
            return false;
        }
        return true;
    }

    Task Remove(int index)
    {
        if ( index > -1 )
        {
            _currentTagList.RemoveAt(index);
            Value = _currentTagList;
            ValueChanged.InvokeAsync(_currentTagList);
            StateHasChanged();
        }
        return Task.CompletedTask;
    }
}
