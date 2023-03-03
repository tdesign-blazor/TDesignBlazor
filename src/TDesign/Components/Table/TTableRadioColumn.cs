namespace TDesign;

/// <summary>
/// 表示呈现单选控件的列。
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class TTableRadioColumn<TItem> : TTableFieldColumnBase<TItem>
{
    protected override void AfterSetParameters(ParameterView parameters)
    {
        if ( string.IsNullOrEmpty(SelectionKey) )
        {
            throw new TDesignComponentException(this, $"必须设置{nameof(SelectionKey)}的值");
        }

        if(typeof(TItem).GetProperty(SelectionKey) is null )
        {
            throw new TDesignComponentException(this, $"指定的字段({SelectionKey})不存在");
        }
        base.AfterSetParameters(parameters);

    }

    /// <summary>
    /// 设置选择的字段主键。该名称必须要与 <see cref="TItem"/> 的属性名称一致。
    /// </summary>
    [Parameter][EditorRequired]public string? SelectionKey { get; set; }

    protected override void OnParametersSet()
    {

        CascadingTable.IsSingleSelection = true;
        base.OnParametersSet();

    }

    /// <inheritdoc/>
    protected override RenderFragment? GetColumnContent()
    {
        return HtmlHelper.CreateContent(builder =>
        {
            builder.Fluent().Element("label", "t-radio")
                            .Class("t-is-checked", IsChecked)
                            .Content(radio =>
                            {
                                radio.Fluent().Element("input", "t-radio__former")
                                                .Attribute("value", Value)
                                                .Attribute("type", "radio")
                                              .Close();
                                radio.Fluent().Span("t-radio__input").Close();
                                radio.Fluent().Span("t-radio__label").Close();
                            })
                            .Close();
        });
    }

    /// <summary>
    /// 是否被选中。
    /// </summary>
    bool IsChecked
    {
        get
        {
            if ( Value is null )
            {
                throw new TDesignComponentException(this, $"{nameof(Value)}是空值");
            }

            var item = CascadingTable.SelectedItem;
            if ( item is null )
            {
                return false;
            }

            var selectedValue = item.GetType().GetProperty(SelectionKey!)?.GetValue(item);

            return Value!.Equals(selectedValue);
        }
    }
}
