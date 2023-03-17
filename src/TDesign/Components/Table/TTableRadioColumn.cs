namespace TDesign;

/// <summary>
/// 表示呈现单选控件的列。
/// </summary>
/// <typeparam name="TItem">数据的类型。</typeparam>
[CssClass("t-table__cell-check")]
public class TTableRadioColumn<TItem,TField> : TTableFieldColumn<TItem,TField>
{
    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        Table.IsSingleSelection = true;
        base.OnParametersSet();
    }

    /// <inheritdoc/>
    protected internal override RenderFragment? GetColumnContent(int rowIndex,TItem item)
    {
        var value = CellTextFunc!(item);

        return builder => builder.Element("label", "t-radio")
                                         .Class("t-is-checked", IsChecked(value))
                                         .Attribute("tabindex", 0)
                                         .Content(radio =>
                                         {
                                             radio.Element("input", "t-radio__former")
                                                             .Attribute("value", value)
                                                             .Attribute("type", "radio")
                                                             .Callback<ChangeEventArgs>("onchange", this, e =>
                                                             {
                                                                 Table.SelectRow(rowIndex);
                                                             })
                                                         .Close();
                                             radio.Span("t-radio__input").Close();
                                             radio.Span("t-radio__label").Close();
                                         })
                                       .Close();
    }

    /// <summary>
    /// 是否被选中。
    /// </summary>
    protected virtual bool IsChecked(string? value)
    {
        var firstRow = Table.SelectedRows.FirstOrDefault();
        if ( firstRow is null || firstRow.Item is null )
        {
            return false;
        }

        var item = firstRow.Item;

        var selectedValue = CellTextFunc!(item);

        return value!.Equals(selectedValue);
    }
}
