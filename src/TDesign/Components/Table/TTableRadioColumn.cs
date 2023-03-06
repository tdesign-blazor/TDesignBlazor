namespace TDesign;

/// <summary>
/// 表示呈现单选控件的列。
/// </summary>
/// <typeparam name="TItem">数据的类型。</typeparam>
[CssClass("t-table__cell-check")]
public class TTableRadioColumn<TItem> : TTableFieldColumnBase<TItem>
{
    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        CascadingGenericTable.IsSingleSelection = true;
        base.OnParametersSet();

    }

    /// <inheritdoc/>
    internal override RenderFragment? GetFieldValueContent(in int rowIndex, in int columnIndex, in TItem? rowData)
    {
        var value = GetValue(rowData);

       return  builder => builder.Fluent().Element("label", "t-radio")
                                        .Class("t-is-checked", IsChecked(value))
                                        .Attribute("tabindex", 0)
                                        .Content(radio =>
                                        {
                                            radio.Fluent().Element("input", "t-radio__former")
                                                            .Attribute("value", value)
                                                            .Attribute("type", "radio")
                                                            .Callback<ChangeEventArgs>("onchange", this, e =>
                                                            {
                                                                var getRowIndex = CascadingGenericTable.TableData.FindIndex(m =>
                                                                {
                                                                    if ( m.data is null )
                                                                    {
                                                                        return false;
                                                                    }
                                                                    return m.data.GetType().GetProperty(Field!).GetValue(m.data).Equals(value);
                                                                });
                                                                CascadingGenericTable.SelectRow(getRowIndex);
                                                            })
                                                        .Close();
                                            radio.Fluent().Span("t-radio__input").Close();
                                            radio.Fluent().Span("t-radio__label").Close();
                                        })
                                      .Close();
    }

    /// <summary>
    /// 是否被选中。
    /// </summary>
    protected virtual bool IsChecked(object? value)
    {
            var firstRow = CascadingGenericTable.SelectedRows.FirstOrDefault();
            if (firstRow is null || firstRow.Item is null)
            {
                return false;
            }

            var item = firstRow.Item;

            var selectedValue = item.GetType().GetProperty(Field)?.GetValue(item);

            return value!.Equals(selectedValue);
    }
}
